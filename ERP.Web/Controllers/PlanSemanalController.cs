using Mantenimiento.Negocio.Contratos.Servicios;
using Mantenimiento.ERP.Helper;
using Mantenimiento.Negocio.Poco;
using ERP.Web.Models;
using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Mantenimiento.Negocio.Servicios;
using Mantenimiento.Datos.Entidades;
using System.Web;
using System.IO;
using Newtonsoft.Json;

namespace ReyDavid.Web.Controllers
{
    public class PlanSemanalController : Controller
    {
        public IPlanSemanalServicio planSemanalServicio { get; set; }

        public PlanSemanalController()
        {
            this.planSemanalServicio = IoCHelper.ResolverIoC<PlanSemanalServicio>();
        }

        public ActionResult Index(string fecha = null)
        {
            if (Session["usuario"] == null)
                return RedirectToAction("Index", "Login");

            var baseDate = DateTime.Today;
            if (!string.IsNullOrWhiteSpace(fecha))
                DateTime.TryParse(fecha, out baseDate);

            var lunes = ObtenerLunes(baseDate);
            var usuario = Session["usuario"].ToString();

            var idPlan = planSemanalServicio.ObtenerOCrearPlanSemana(lunes, usuario);

            var poco = new GestionarPlanSemanalPoco
            {
                idPlanSemana = idPlan,
                lunes = lunes,
                lunesTexto = lunes.ToString("yyyy-MM-dd")
            };
            ViewBag.usuario = usuario;

            var model = new PlanSemanalModel { Plan = poco };
            return View(model);
        }

        public ActionResult Observaciones()
        {
            if (Session["usuario"] == null)
                return RedirectToAction("Index", "Login");

           
            return View();
        }

    
        public JsonResult Tree(int? idAdicional = null, string q = "")
        {
            try
            {
                var reqs = planSemanalServicio.TreeRequerimientos(q);
                var subs = planSemanalServicio.TreeSubRequerimientos(q);
                return Json(new { ok = true, reqs, subs }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { ok = false, error = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        // JSON: Cards
        public JsonResult ListarTarjetas(int idPlanSemana, int? idPersonaResponsable = null)
        {
            try
            {
                var cards = planSemanalServicio.ListarTarjetas(idPlanSemana, idPersonaResponsable);
                return Json(new { ok = true, cards }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { ok = false, error = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        // Partial: Modal tarjeta
        public ActionResult FormTarjeta(int idPlanSemana, int idPlanDetalle = 0, int idRequerimientoDetalle = 0, int? idObservacion = null, byte dia = 0)
        {
            if (Session["usuario"] == null)
                return RedirectToAction("Index", "Login");

            PlanSemanaDetalle tarjeta;

            if (idPlanDetalle > 0)
            {
                tarjeta = planSemanalServicio.ObtenerTarjeta(idPlanDetalle);
            }
            else
            {
                tarjeta = new PlanSemanaDetalle
                {
                    idPlanSemana = idPlanSemana,
                    dia = dia,
                    estado = "pendiente",
                    prioridad = 2,
                    idRequerimientoDetalle = idRequerimientoDetalle,
                    idObservacion = idObservacion,
                    //idPersonaResponsable= tarjeta.idPersonaResponsable
                };
            }

            return PartialView("_FormTarjeta", tarjeta);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult GuardarTarjeta(PlanSemanaDetalle tarjeta)
        {
            Resultado res = new Resultado();
            try
            {
                if (Session["usuario"] == null)
                    return RedirectToAction("Index", "Login");

                if (string.IsNullOrWhiteSpace(tarjeta.titulo))
                    return Json(new { idResultado = (int)enumTipoMensaje.error, mensaje = "Falta título." }, JsonRequestBehavior.AllowGet);

                if (tarjeta.idRequerimientoDetalle <= 0)
                    return Json(new { idResultado = (int)enumTipoMensaje.error, mensaje = "Selecciona un SubRequerimiento." }, JsonRequestBehavior.AllowGet);

                var usuario = Session["usuario"].ToString();

                var ok = planSemanalServicio.GuardarTarjeta(tarjeta, usuario);
                if (ok)
                {
                    res.idResultado = (int)enumTipoMensaje.exito;
                    res.mensaje = "Éxito al guardar la tarjeta";
                }
                else
                {
                    res.idResultado = (int)enumTipoMensaje.error;
                    res.mensaje = "No se pudo guardar la tarjeta";
                }
            }
            catch (Exception ex)
            {
                res.idResultado = (int)enumTipoMensaje.error;
                res.mensaje = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
            }
            return Json(res, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult MoverTarjeta(int idPlanDetalle, byte dia)
        {
            Resultado res = new Resultado();
            try
            {
                if (Session["usuario"] == null)
                    return RedirectToAction("Index", "Login");

                var usuario = Session["usuario"].ToString();
                var ok = planSemanalServicio.MoverTarjeta(idPlanDetalle, dia, usuario);

                res.idResultado = ok ? (int)enumTipoMensaje.exito : (int)enumTipoMensaje.error;
                res.mensaje = ok ? "Tarjeta movida" : "No se pudo mover";
            }
            catch (Exception ex)
            {
                res.idResultado = (int)enumTipoMensaje.error;
                res.mensaje = ex.Message;
            }
            return Json(res, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult EliminarTarjeta(int idPlanDetalle)
        {
            Resultado res = new Resultado();
            try
            {
                if (Session["usuario"] == null)
                    return RedirectToAction("Index", "Login");

                var ok = planSemanalServicio.EliminarTarjeta(idPlanDetalle);
                res.idResultado = ok ? (int)enumTipoMensaje.exito : (int)enumTipoMensaje.error;
                res.mensaje = ok ? "Éxito al eliminar" : "No se pudo eliminar";
            }
            catch (Exception ex)
            {
                res.idResultado = (int)enumTipoMensaje.error;
                res.mensaje = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
            }
            return Json(res, JsonRequestBehavior.AllowGet);
        }

        private static DateTime ObtenerLunes(DateTime d)
        {
            // Lunes como inicio
            int day = (int)d.DayOfWeek; // Sunday=0
            int diff = day == 0 ? -6 : (1 - day);
            return d.AddDays(diff).Date;
        }

        public JsonResult ListarAdicionales()
        {
            try
            {
                var lista = planSemanalServicio.ListarAdicionales();
                return Json(new { ok = true, lista }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { ok = false, error = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult ListarRequerimientosPorAdicional(int idAdicional)
        {
            try
            {
                var lista = planSemanalServicio.ListarRequerimientosPorAdicional(idAdicional);
                return Json(new { ok = true, lista }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { ok = false, error = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult ListarSubRequerimientosPorRequerimiento(int idRequerimiento)
        {
            try
            {
                var lista = planSemanalServicio.ListarSubRequerimientosPorRequerimiento(idRequerimiento);
                return Json(new { ok = true, lista }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { ok = false, error = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult ListarObservaciones(int idRequerimientoDetalle)
        {
            try
            {
                var lista = planSemanalServicio.ListarObservaciones(idRequerimientoDetalle);
                return Json(new { ok = true, lista }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { ok = false, error = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult ObtenerObservacion(int idObservacion)
        {
            try
            {
                var observacion = planSemanalServicio.ObtenerObservacion(idObservacion);
                return Json(new { ok = true, observacion }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { ok = false, error = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public ActionResult GuardarObservacion(string data, HttpPostedFileBase file)
        {
            try
            {
                if (Session["usuario"] == null)
                    return RedirectToAction("Index", "Login");

                var usuario = Session["usuario"].ToString();

                // Deserializar el objeto data
                var obsData = JsonConvert.DeserializeObject<dynamic>(data);
                RequerimientoDetalleObservacion oDT = new Mantenimiento.Datos.Entidades.RequerimientoDetalleObservacion();
                oDT.idObservacion = obsData.idObservacion;
                oDT.idRequerimientoDetalle = obsData.idRequerimientoDetalle;
                oDT.comentario = obsData.comentario;
                oDT.severidad = obsData.severidad;
                oDT.estado = obsData.estado;
                oDT.ObservadorPor = obsData.observadopor;
                oDT.registradoPor = obsData.registradoPor;
                oDT.fechaRegistro = string.IsNullOrEmpty((string)obsData.fechaRegistro) ? DateTime.Now : DateTime.Parse((string)obsData.fechaRegistro);
                oDT.cerradoPor = obsData.cerradoPor;
                //oDT.fechaCierre = string.IsNullOrEmpty((string)obsData.fechaCierre) ? (DateTime?)null : DateTime.Parse((string)obsData.fechaCierre);

                if (string.IsNullOrWhiteSpace(oDT.comentario))
                    return Json(new { ok = false, mensaje = "Falta comentario." });

                // Manejar el archivo si existe
                string rutaArchivo = null;
                if (file != null && file.ContentLength > 0)
                {
                    // Definir la ruta donde guardar el archivo, por ejemplo en ~/Uploads/
                    var uploadsPath = Server.MapPath("~/Uploads/");
                    if (!Directory.Exists(uploadsPath))
                    {
                        Directory.CreateDirectory(uploadsPath);
                    }
                    var fileName = Path.GetFileName(file.FileName);
                    rutaArchivo = Path.Combine(uploadsPath, fileName);
                    file.SaveAs(rutaArchivo);
                    // Guardar nombre y extensión en el objeto
                    oDT.nombreArchivo = file.FileName;
                    oDT.extension = Path.GetExtension(file.FileName);
                    // Si se guarda la ruta en DB, setear oDT.rutaArchivo = rutaArchivo;
                }
                else
                {
                    oDT.nombreArchivo = obsData.nombreArchivo;
                    oDT.extension = obsData.extension;
                }
                if(oDT.idObservacion == 0)
                {
                    var ok = planSemanalServicio.InsertarObservacion(oDT);
                    return Json(new { ok = ok, mensaje = ok ? "Observación registrada." : "No se pudo registrar." });
                }
                else
                {
                    var ok = planSemanalServicio.Actualizar(oDT);
                    return Json(new { ok = ok, mensaje = ok ? "Observación actualizada." : "No se pudo registrar." });
                }
            }
            catch (Exception ex)
            {
                return Json(new { ok = false, mensaje = ex.Message });
            }
        }

        [HttpPost]
        public ActionResult CerrarObservacion(int idObservacion)
        {
            try
            {
                if (Session["usuario"] == null)
                    return RedirectToAction("Index", "Login");

                var usuario = Session["usuario"].ToString();
                var ok = planSemanalServicio.CerrarObservacion(idObservacion, usuario);

                return Json(new { ok = ok, mensaje = ok ? "Observación cerrada." : "No se pudo cerrar." });
            }
            catch (Exception ex)
            {
                return Json(new { ok = false, mensaje = ex.Message });
            }
        }
    }
}
