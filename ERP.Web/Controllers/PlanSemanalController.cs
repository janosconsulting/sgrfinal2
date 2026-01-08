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
using ERP.Web.Helpers;
using OfficeOpenXml.Style;
using OfficeOpenXml;
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
        public JsonResult ListarTarjetas(int idPlanSemana,string estado, int? idPersonaResponsable)
        {
            try
            {
                var cards = planSemanalServicio.ListarTarjetas(idPlanSemana,estado, idPersonaResponsable.GetValueOrDefault());
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
        public ActionResult GuardarTarjeta(PlanSemanaDetalle tarjeta, HttpPostedFileBase archivo)
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
                // Manejar el archivo si existe
                string rutaArchivo = null;
                if (archivo != null && archivo.ContentLength > 0)
                {
                    // Definir la ruta donde guardar el archivo, por ejemplo en ~/Uploads/
                    var uploadsPath = Server.MapPath("~/Uploads/");
                    if (!Directory.Exists(uploadsPath))
                    {
                        Directory.CreateDirectory(uploadsPath);
                    }
                    var fileName = Path.GetFileName(archivo.FileName);
                    rutaArchivo = Path.Combine(uploadsPath, fileName);
                    archivo.SaveAs(rutaArchivo);
                    // Guardar nombre y extensión en el objeto
                    tarjeta.NombreArchivo = archivo.FileName;
                    tarjeta.Extension = Path.GetExtension(archivo.FileName);
                    // Si se guarda la ruta en DB, setear oDT.rutaArchivo = rutaArchivo;
                }


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

       
        public ActionResult ExportarTareasSemanal(int idPlanSemana, string lunes, string estado, int? idPersonaResponsable = null)
        {
            try
            {
                if (Session["usuario"] == null)
                    return RedirectToAction("Index", "Login");

                var usuario = Session["usuario"].ToString();
                var fechaExportacion = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                var fechaLunes = DateTime.Parse(lunes);

                var cards = planSemanalServicio.ListarTarjetas(idPlanSemana,estado, idPersonaResponsable);
                var dayNames = new[] { "Lunes", "Martes", "Miércoles", "Jueves", "Viernes", "Sábado", "Domingo" };

                using (var package = new ExcelPackage())
                {
                    var worksheet = package.Workbook.Worksheets.Add("Plan Semanal");

                    // Título principal
                    worksheet.Cells[1, 1].Value = "Exportación de Tareas - Plan Semanal";
                    worksheet.Cells[1, 1].Style.Font.Bold = true;
                    worksheet.Cells[1, 1].Style.Font.Size = 14;
                    worksheet.Cells["A1:J1"].Merge = true;

                    // Información de la semana
                    var fechaFin = fechaLunes.AddDays(6);
                    worksheet.Cells[2, 1].Value = $"Semana: {fechaLunes:dd/MM/yyyy} - {fechaFin:dd/MM/yyyy}";
                    worksheet.Cells[2, 1].Style.Font.Bold = true;

                    worksheet.Cells[3, 1].Value = $"Fecha de Exportación: {fechaExportacion}";
                    worksheet.Cells[3, 1].Style.Font.Bold = true;

                    worksheet.Cells[4, 1].Value = $"Usuario: {usuario}";
                    worksheet.Cells[4, 1].Style.Font.Bold = true;

                    // Headers
                    worksheet.Cells[6, 1].Value = "Día";
                    worksheet.Cells[6, 2].Value = "Fecha";
                    worksheet.Cells[6, 3].Value = "Título";
                    worksheet.Cells[6, 4].Value = "Requerimiento";
                    worksheet.Cells[6, 5].Value = "SubRequerimiento";
                    worksheet.Cells[6, 6].Value = "Responsable";
                    worksheet.Cells[6, 7].Value = "Estado";
                    worksheet.Cells[6, 8].Value = "Prioridad";
                    worksheet.Cells[6, 9].Value = "Tipo";
                    worksheet.Cells[6, 10].Value = "Detalle";

                    // Estilo para headers
                    var headerRange = worksheet.Cells["A6:J6"];
                    headerRange.Style.Font.Bold = true;
                    headerRange.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    headerRange.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.FromArgb(173, 216, 230)); // LightBlue
                    headerRange.Style.Border.BorderAround(ExcelBorderStyle.Thin);

                    int row = 7;

                    // Agrupar tarjetas por día
                    var tarjetasOrdenadas = cards.OrderBy(c => c.dia).ThenBy(c => c.prioridad).ToList();

                    foreach (var card in tarjetasOrdenadas)
                    {
                        var dia = card.dia;
                        var fechaDia = fechaLunes.AddDays(dia);
                        var nombreDia = dia >= 0 && dia < 7 ? dayNames[dia] : "Desconocido";

                        string detalle = "";
                        string tipo = card.idObservacion.HasValue && card.idObservacion > 0 ? "Observación" : "Tarea";
                        if (tipo == "Tarea")
                        {
                            var tarjeta = planSemanalServicio.ObtenerTarjeta(card.idPlanDetalle);
                            detalle = tarjeta?.titulo ?? "";
                        }
                        else if (tipo == "Observación")
                        {
                            var observacion = planSemanalServicio.ObtenerObservacion(card.idObservacion.Value);
                            detalle = observacion?.comentario ?? "";
                        }

                        worksheet.Cells[row, 1].Value = nombreDia;
                        worksheet.Cells[row, 2].Value = fechaDia.ToString("dd/MM/yyyy");
                        worksheet.Cells[row, 3].Value = card.titulo ?? "";
                        worksheet.Cells[row, 4].Value = card.reqCodigo ?? "";
                        worksheet.Cells[row, 5].Value = card.subNombre ?? "";
                        worksheet.Cells[row, 6].Value = card.responsable ?? "";
                        worksheet.Cells[row, 7].Value = card.estado ?? "";
                        worksheet.Cells[row, 8].Value = $"P{card.prioridad}";
                        worksheet.Cells[row, 9].Value = tipo;
                        worksheet.Cells[row, 10].Value = detalle;

                        // Colorear según estado
                        var estadoLower = (card.estado ?? "").ToLower();
                        if (estadoLower == "hecho")
                        {
                            worksheet.Cells[row, 7].Style.Fill.PatternType = ExcelFillStyle.Solid;
                            worksheet.Cells[row, 7].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.FromArgb(144, 238, 144)); // LightGreen
                        }
                        else if (estadoLower == "proceso")
                        {
                            worksheet.Cells[row, 7].Style.Fill.PatternType = ExcelFillStyle.Solid;
                            worksheet.Cells[row, 7].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.FromArgb(255, 255, 224)); // LightYellow
                        }
                        else if (estadoLower == "bloqueado")
                        {
                            worksheet.Cells[row, 7].Style.Fill.PatternType = ExcelFillStyle.Solid;
                            worksheet.Cells[row, 7].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.FromArgb(255, 182, 193)); // LightPink
                        }

                        row++;
                    }

                    // Ajustar anchos de columna
                    worksheet.Column(1).Width = 12; // Día
                    worksheet.Column(2).Width = 12; // Fecha
                    worksheet.Column(3).Width = 40; // Título
                    worksheet.Column(3).Style.WrapText = true;
                    worksheet.Column(4).Width = 20; // Requerimiento
                    worksheet.Column(5).Width = 30; // SubRequerimiento
                    worksheet.Column(5).Style.WrapText = true;
                    worksheet.Column(6).Width = 20; // Responsable
                    worksheet.Column(7).Width = 12; // Estado
                    worksheet.Column(8).Width = 10; // Prioridad
                    worksheet.Column(9).Width = 12; // Tipo
                    worksheet.Column(10).Width = 50; // Detalle
                    worksheet.Column(10).Style.WrapText = true;

                    // Agregar bordes a la tabla de datos
                    if (row > 7)
                    {
                        var dataRange = worksheet.Cells[$"A7:J{row - 1}"];
                        dataRange.Style.Border.BorderAround(ExcelBorderStyle.Thin);
                        // Bordes internos: aplicar a cada celda
                        foreach (var cell in dataRange)
                        {
                            cell.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                            cell.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                            cell.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                            cell.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                        }
                    }

                    var stream = new MemoryStream();
                    package.SaveAs(stream);
                    stream.Position = 0;
                    var fileName = $"PlanSemanal_{fechaLunes:yyyyMMdd}.xlsx";
                    return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
                }
            }
            catch (Exception ex)
            {
                return new HttpStatusCodeResult(500, "Error al exportar: " + ex.Message);
            }
        }
        public JsonResult ListarTareasPorSubRequerimiento(int idRequerimientoDetalle)
        {
            try
            {
                // Obtener todas las tarjetas y filtrar por idRequerimientoDetalle
                var todasLasTarjetas = planSemanalServicio.ListarTarjetas(0, "0", 0);
                var tareas = todasLasTarjetas
                     .Where(t => t.idRequerimientoDetalle == idRequerimientoDetalle && (t.idObservacion == null || t.idObservacion == 0))
                     .ToList();
                return Json(new { ok = true, tareas }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { ok = false, error = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult ExportarDatos(int idAdicional, string q = "", string estado = "Todos")
        {
            if (Session["usuario"] == null)
                return RedirectToAction("Index", "Login");

            var usuario = Session["usuario"].ToString();
            var fechaExportacion = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

            var adicionales = planSemanalServicio.ListarAdicionales();
            var adicionalSeleccionado = adicionales
                .FirstOrDefault(a => a.idAdicional == idAdicional)?.nombre ?? "Desconocido";

            var data = new List<ExportPlanSemanalDto>();

            var requerimientos = planSemanalServicio.ListarRequerimientosPorAdicional(idAdicional);

            foreach (var req in requerimientos)
            {
                var detalles = planSemanalServicio.ListarSubRequerimientosPorRequerimiento(req.idRequerimiento);

                foreach (var det in detalles)
                {
                    var observaciones = planSemanalServicio.ListarObservaciones(det.idRequerimientoDetalle);

                    if (observaciones.Any())
                    {
                        foreach (var obs in observaciones)
                        {
                            data.Add(new ExportPlanSemanalDto
                            {
                                Requerimiento = req.nombre,
                                Detalle = det.nombre,
                                Observacion = obs.comentario,
                                EstadoObs = obs.estado,
                                Severidad = obs.severidad,
                                RegistradoPor = obs.registradoPor,
                                FechaRegistro = obs.fechaRegistro
                            });
                        }
                    }
                    else
                    {
                        data.Add(new ExportPlanSemanalDto
                        {
                            Requerimiento = req.nombre,
                            Detalle = det.nombre
                        });
                    }
                }
            }

            var fileBytes = ExcelExportUtil.ExportarPlanSemanal(
                data,
                adicionalSeleccionado,
                usuario,
                fechaExportacion);

            return File(
                fileBytes,
                "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                "ExportacionDatos.xlsx");
        }

    }
}
