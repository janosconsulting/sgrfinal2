using Mantenimiento.Negocio.Contratos.Servicios;
using Mantenimiento.ERP.Helper;
using Mantenimiento.Negocio.Poco;
using ERP.Web.Models;
using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Mantenimiento.Negocio.Servicios;

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

            var model = new PlanSemanalModel { Plan = poco };
            return View(model);
        }

        // JSON: Tree (Req + SubReq)
        public JsonResult Tree(string q = "")
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
        public JsonResult ListarTarjetas(int idPlanSemana)
        {
            try
            {
                var cards = planSemanalServicio.ListarTarjetas(idPlanSemana);
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
                    idObservacion = idObservacion
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
    }
}
