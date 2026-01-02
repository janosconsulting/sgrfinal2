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
using System.Linq;
using ClosedXML.Excel;
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
            var usuario = Session["usuario"].ToString();
            ViewBag.usuario = usuario;
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
        public ActionResult DescargarArchivo(int idObservacion)
        {
            try
            {
                if (Session["usuario"] == null)
                    return RedirectToAction("Index", "Login");

                var observacion = planSemanalServicio.ObtenerObservacion(idObservacion);
                if (observacion == null || string.IsNullOrEmpty(observacion.nombreArchivo))
                {
                    return HttpNotFound("Archivo no encontrado.");
                }

                var uploadsPath = Server.MapPath("~/Uploads/");
                var filePath = Path.Combine(uploadsPath, observacion.nombreArchivo);

                if (!System.IO.File.Exists(filePath))
                {
                    return HttpNotFound("Archivo no encontrado en el servidor.");
                }

                var fileBytes = System.IO.File.ReadAllBytes(filePath);
                var contentType = MimeMapping.GetMimeMapping(observacion.nombreArchivo);

                return File(fileBytes, contentType, observacion.nombreArchivo);
            }
            catch (Exception ex)
            {
                return new HttpStatusCodeResult(500, "Error al descargar el archivo: " + ex.Message);
            }
        }
        [HttpPost]
        public ActionResult EliminarObservacion(int idObservacion)
        {
            try
            {
                if (Session["usuario"] == null)
                    return RedirectToAction("Index", "Login");
                ResultadoTransaccion rs = planSemanalServicio.EliminarObservacion(idObservacion);
               
                return Json(new { ok = rs.codigo == -1 ? false: true, mensaje = rs.mensaje});
            }
            catch (Exception ex)
            {
                return Json(new { ok = false, mensaje = ex.Message });
            }
        }
        public ActionResult ExportarDatos(int idAdicional, string q = "", string estado = "Todos", int idRequerimiento = 0, int idSubReq = 0)
        {
            try
            {
                if (Session["usuario"] == null)
                    return RedirectToAction("Index", "Login");

                var usuario = Session["usuario"].ToString();
                var fechaExportacion = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

                // Obtener el nombre del adicional seleccionado
                var adicionales = planSemanalServicio.ListarAdicionales();
                var adicionalSeleccionado = adicionales.FirstOrDefault(a => a.idAdicional == idAdicional)?.nombre ?? "Desconocido";

                using (var workbook = new XLWorkbook())
                {
                    var worksheet = workbook.Worksheets.Add("Datos");

                    // Título principal
                    worksheet.Cell(1, 1).Value = "Exportación de Datos - Plan Semanal";
                    worksheet.Cell(1, 1).Style.Font.Bold = true;
                    worksheet.Cell(1, 1).Style.Font.FontSize = 14;
                    worksheet.Range("A1:G1").Merge();

                    // Información adicional
                    worksheet.Cell(2, 1).Value = $"Adicional Seleccionado: {adicionalSeleccionado}";
                    worksheet.Cell(2, 1).Style.Font.Bold = true;

                    worksheet.Cell(3, 1).Value = $"Fecha de Exportación: {fechaExportacion}";
                    worksheet.Cell(3, 1).Style.Font.Bold = true;

                    worksheet.Cell(4, 1).Value = $"Usuario que Exporta: {usuario}";
                    worksheet.Cell(4, 1).Style.Font.Bold = true;

                    // Headers
                    worksheet.Cell(6, 1).Value = "Requerimiento";
                    worksheet.Cell(6, 2).Value = "Detalle";
                    worksheet.Cell(6, 3).Value = "Observación";
                    worksheet.Cell(6, 4).Value = "Estado Obs";
                    worksheet.Cell(6, 5).Value = "Severidad";
                    worksheet.Cell(6, 6).Value = "Registrado Por";
                    worksheet.Cell(6, 7).Value = "Fecha Registro";

                    // Estilo para headers
                    var headerRange = worksheet.Range("A6:G6");
                    headerRange.Style.Font.Bold = true;
                    headerRange.Style.Fill.BackgroundColor = XLColor.LightGray;
                    headerRange.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;

                    int row = 7;
                    var requerimientos = planSemanalServicio.ListarRequerimientosPorAdicional(idAdicional);
                    if (!string.IsNullOrEmpty(q))
                    {
                        requerimientos = requerimientos.Where(x => (x.nombre ?? "").IndexOf(q, StringComparison.OrdinalIgnoreCase) >= 0 || (x.codigo ?? "").IndexOf(q, StringComparison.OrdinalIgnoreCase) >= 0).ToList();
                    }
                    if (estado != "Todos")
                    {
                        requerimientos = requerimientos.Where(x => (x.estado ?? "") == estado).ToList();
                    }

                    foreach (var req in requerimientos)
                    {
                        if (idRequerimiento > 0 && req.idRequerimiento != idRequerimiento) continue;

                        var detalles = planSemanalServicio.ListarSubRequerimientosPorRequerimiento(req.idRequerimiento);
                        if (!string.IsNullOrEmpty(q))
                        {
                            detalles = detalles.Where(x => (x.nombre ?? "").IndexOf(q, StringComparison.OrdinalIgnoreCase) >= 0 || (x.codigo ?? "").IndexOf(q, StringComparison.OrdinalIgnoreCase) >= 0).ToList();
                        }
                        if (estado != "Todos")
                        {
                            detalles = detalles.Where(x => (x.estado ?? "") == estado).ToList();
                        }

                        foreach (var det in detalles)
                        {
                            if (idSubReq > 0 && det.idRequerimientoDetalle != idSubReq) continue;

                            var observaciones = planSemanalServicio.ListarObservaciones(det.idRequerimientoDetalle);
                            if (!string.IsNullOrEmpty(q))
                            {
                                observaciones = observaciones.Where(x => (x.comentario ?? "").IndexOf(q, StringComparison.OrdinalIgnoreCase) >= 0).ToList();
                            }
                            if (estado != "Todos")
                            {
                                observaciones = observaciones.Where(x => (x.estado ?? "") == estado).ToList();
                            }

                            if (observaciones.Any())
                            {
                                foreach (var obs in observaciones)
                                {
                                    worksheet.Cell(row, 1).Value = req.nombre;
                                    worksheet.Cell(row, 2).Value = det.nombre;
                                    worksheet.Cell(row, 3).Value = obs.comentario;
                                    worksheet.Cell(row, 4).Value = obs.estado;
                                    worksheet.Cell(row, 5).Value = obs.severidad;
                                    worksheet.Cell(row, 6).Value = obs.registradoPor;
                                    worksheet.Cell(row, 7).Value = obs.fechaRegistro.ToString("yyyy-MM-dd");
                                    row++;
                                }
                            }
                            else
                            {
                                worksheet.Cell(row, 1).Value = req.nombre;
                                worksheet.Cell(row, 2).Value = det.nombre;
                                worksheet.Cell(row, 3).Value = "";
                                worksheet.Cell(row, 4).Value = "";
                                worksheet.Cell(row, 5).Value = "";
                                worksheet.Cell(row, 6).Value = "";
                                worksheet.Cell(row, 7).Value = "";
                                row++;
                            }
                        }
                    }


                    // Ajustar anchos de columna para mejor visualización
                    worksheet.Column(1).Width = 25; // Requerimiento
                    worksheet.Column(2).Width = 55; // Detalle
                    worksheet.Column(2).Style.Alignment.WrapText = true;
                    worksheet.Column(3).Width = 50; // Observación
                    worksheet.Column(3).Style.Alignment.WrapText = true;
                    worksheet.Column(4).Width = 15; // Estado Obs
                    worksheet.Column(5).Width = 15; // Severidad
                    worksheet.Column(6).Width = 20; // Registrado Por
                    worksheet.Column(7).Width = 15; // Fecha Registro

                    // Agregar bordes a la tabla de datos si hay datos
                    if (row > 7)
                    {
                        var dataRange = worksheet.Range($"A7:G{row - 1}");
                        dataRange.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                        dataRange.Style.Border.InsideBorder = XLBorderStyleValues.Thin;
                    }

                    using (var stream = new MemoryStream())
                    {
                        workbook.SaveAs(stream);
                        stream.Position = 0;
                        return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "ExportacionDatos.xlsx");
                    }
                }
            }
            catch (Exception ex)
            {
                return new HttpStatusCodeResult(500, ex.Message);
            }
        }
    }
}
