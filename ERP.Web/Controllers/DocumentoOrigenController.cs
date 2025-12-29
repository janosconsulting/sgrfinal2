using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Web.Mvc;

using Mantenimiento.Datos.Entidades;
using Mantenimiento.Negocio.Contratos.Servicios;
using Mantenimiento.Negocio.Servicios; // por si no usas IoC (igual lo usas)
using Mantenimiento.Negocio.Poco;

using ERP.Web.Models;                  // Resultado, enumTipoMensaje, ResultadoTransaccion (según tu proyecto)
using Mantenimiento.ERP.Helper;        // IoCHelper

namespace ERP.Web.Controllers
{
    public class DocumentoOrigenController : Controller
    {
        public IAdicionalServicio documentoOrigenServicio { get; set; }
        public IPersonaServicio personaServicio { get; set; }
        public IProyectoServicio proyectoServicio { get; set; } // ajusta al servicio real que usas

        public DocumentoOrigenController()
        {
            this.documentoOrigenServicio = IoCHelper.ResolverIoC<AdicionalServicio>();
            this.personaServicio = IoCHelper.ResolverIoC<PersonaServicio>();
            this.proyectoServicio = IoCHelper.ResolverIoC<ProyectoServicio>(); // ajusta
        }

        // ==========================
        // VISTAS
        // ==========================

        public ActionResult Index()
        {
            if (Session["usuario"] == null)
                return RedirectToAction("Index", "Login");

            var oGestionar = new GestionarDocumentoOrigenPoco();
            oGestionar.DocumentoOrigen = new Adicional(); // entidad real
            oGestionar.ListarClientes = this.personaServicio.ListarClientes();
            oGestionar.ListarProyectos = this.proyectoServicio.ListarProyectos(); // ajusta
            oGestionar.IsEditing = false;

            return View(oGestionar);
        }

        public ActionResult Nuevo()
        {
            if (Session["usuario"] == null)
                return RedirectToAction("Index", "Login");

            var oGestionar = new GestionarDocumentoOrigenPoco();
            oGestionar.DocumentoOrigen = new Adicional(); // entidad real
            oGestionar.DocumentoOrigen.fechaRecepcion = DateTime.Now;
            oGestionar.ListarClientes = this.personaServicio.ListarClientes();
            oGestionar.ListarProyectos = this.proyectoServicio.ListarProyectos(); // ajusta
            oGestionar.IsEditing = false;
            return View(oGestionar);
        }

        //public ActionResult Editar(int id)
        //{
        //    if (Session["usuario"] == null)
        //        return RedirectToAction("Index", "Login");

        //    var oGestionar = new GestionarDocumentoOrigenPoco();
        //    oGestionar.DocumentoOrigen = this.documentoOrigenServicio.Obtener(id);
        //    oGestionar.ListarClientes = this.personaServicio.ListarClientes();
        //    oGestionar.ListarProyectos = this.proyectoServicio.ListarProyectos(); // ajusta
        //    oGestionar.IsEditing = true;

        //    return View("Nuevo", oGestionar);
        //}

        // ==========================
        // DATATABLE (JSON)
        // ==========================

        [HttpGet]
        public JsonResult Listar(int? cliente, int? proyecto, int? tipo, int? estado)
        {
            try
            {
                // tu DataTable manda "" -> en JS tú conviertes a null, pero igual prevenimos aquí:
                cliente = (cliente.HasValue && cliente.Value > 0) ? cliente : null;
                proyecto = (proyecto.HasValue && proyecto.Value > 0) ? proyecto : null;
                tipo = (tipo.HasValue && tipo.Value > 0) ? tipo : null;
                estado = (estado.HasValue && estado.Value > 0) ? estado : null;

                List<sp_ListarDocumentoOrigen> lista =
                    this.documentoOrigenServicio.Listar(cliente, proyecto, tipo, estado);

                return Json(lista, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { error = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        // ==========================
        // GUARDAR(INSERT / UPDATE)
        // ==========================

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Guardar(GestionarDocumentoOrigenPoco oRegistro)
        {
            var res = new Resultado();

            try
            {
                if (Session["usuario"] == null)
                    return RedirectToAction("Index", "Login");

                if (oRegistro == null || oRegistro.DocumentoOrigen == null)
                {
                    res.idResultado = (int)enumTipoMensaje.error;
                    res.mensaje = "Modelo inválido.";
                    return Json(res, JsonRequestBehavior.AllowGet);
                }

                // set fechas/usuario si manejas auditoría
                if (oRegistro.DocumentoOrigen.idAdicional == 0)
                {
                    //oRegistro.DocumentoOrigen.fechaRegistro = DateTime.Now;

                    // si quieres generar código aquí:
                    // oRegistro.DocumentoOrigen.codigo = documentoOrigenServicio.GenerarCodigo();
                    
                    oRegistro.DocumentoOrigen.fechaRecepcion = DateTime.Now;

                    bool ok = this.documentoOrigenServicio.Insertar(oRegistro.DocumentoOrigen);

                    res.idResultado = ok ? (int)enumTipoMensaje.exito : (int)enumTipoMensaje.error;
                    res.mensaje = ok ? "Éxito al guardar el Documento de Origen" : "No se pudo guardar.";
                }
                else
                {
                    //oRegistro.DocumentoOrigen.fechaActualiza = DateTime.Now;

                    bool ok = this.documentoOrigenServicio.Actualizar(oRegistro.DocumentoOrigen);

                    res.idResultado = ok ? (int)enumTipoMensaje.exito : (int)enumTipoMensaje.error;
                    res.mensaje = ok ? "Éxito al actualizar el Documento de Origen" : "No se pudo actualizar.";
                }
            }
            catch (SqlException sqlEx)
            {
                res.idResultado = (int)enumTipoMensaje.error;
                res.mensaje = "Error SQL: " + sqlEx.Message;
            }
            catch (Exception ex)
            {
                res.idResultado = (int)enumTipoMensaje.error;
                res.mensaje = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
            }

            return Json(res, JsonRequestBehavior.AllowGet);
        }

        // ==========================
        // ELIMINAR
        // ==========================

        public ActionResult Eliminar(int id)
        {
            var res = new Resultado();

            try
            {
                if (Session["usuario"] == null)
                    return RedirectToAction("Index", "Login");

                bool ok = this.documentoOrigenServicio.Eliminar(id);

                res.idResultado = ok ? (int)enumTipoMensaje.exito : (int)enumTipoMensaje.error;
                res.mensaje = ok ? "Éxito al eliminar el Documento de Origen" : "No se pudo eliminar.";
            }
            catch (Exception ex)
            {
                res.idResultado = (int)enumTipoMensaje.error;
                res.mensaje = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
            }

            return Json(res, JsonRequestBehavior.AllowGet);
        }

        // ==========================
        // CHECKLIST IMPRIMIBLE
        // ==========================

        public ActionResult Checklist(int id)
        {
            if (Session["usuario"] == null)
                return RedirectToAction("Index", "Login");

            // Este VM es para la vista imprimible (Layout null)
            var vm = this.documentoOrigenServicio.ObtenerChecklistVm(id);

            if (vm == null)
                return HttpNotFound();

            return View(vm); // Views/DocumentoOrigen/Checklist.cshtml
        }
    }

    // ==========================
    // NOTAS IMPORTANTES
    // ==========================
    // 1) DocumentoOrigen = tu entidad real (Mantenimiento.Datos.Entidades).
    //    Debe tener idDocumentoOrigen, codigo, tipoDoc, fechaDocumento, titulo, descripcion, estado,
    //    idCliente, idProyecto, fechaRegistro/fechaActualiza (si usas).
    //
    // 2) sp_ListarDocumentoOrigen = DTO que retorna tu SP/Query para el datatable
    //    con campos:
    //    idDocumentoOrigen, codigo, tipoDoc, nombreCli, nombreProyec, fechaDocumento, titulo, descripcion, estado,
    //    totalRequerimientos, tieneChecklist
    //
    // 3) en DocumentoOrigenServicio debes implementar:
    //    - Listar(int? cliente, int? proyecto, int? tipo, int? estado)
    //    - Obtener(int id)
    //    - Insertar(DocumentoOrigen doc)
    //    - Actualizar(DocumentoOrigen doc)
    //    - Eliminar(int id)
    //    - ObtenerChecklistVm(int idDocumentoOrigen)
}
