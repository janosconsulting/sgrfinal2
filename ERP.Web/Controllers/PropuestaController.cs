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
    public class PropuestaController : Controller
    {
        public IPropuestaServicio documentoOrigenServicio { get; set; }
        public IPersonaServicio personaServicio { get; set; }
        public IProyectoServicio proyectoServicio { get; set; } // ajusta al servicio real que usas
        public ISubscripcionServicio subscripcionServicio { get; set; }
        public PropuestaController()
        {
            this.documentoOrigenServicio = IoCHelper.ResolverIoC<PropuestaServicio>();
            this.personaServicio = IoCHelper.ResolverIoC<PersonaServicio>();
            this.proyectoServicio = IoCHelper.ResolverIoC<ProyectoServicio>(); 
            this.subscripcionServicio = IoCHelper.ResolverIoC<SubscripcionServicio>();
        }

        // ==========================
        // VISTAS
        // ==========================

        public ActionResult Index()
        {
            if (Session["usuario"] == null)
                return RedirectToAction("Index", "Login");

            var oGestionar = new GestionarPropuestaPoco();
            oGestionar.Propuesta = new PropuestaEntidad(); // entidad real
            oGestionar.ListaClientes = this.personaServicio.ListarClientes();
          
            return View(oGestionar);
        }

        public ActionResult Nuevo()
        {
            if (Session["usuario"] == null)
                return RedirectToAction("Index", "Login");

            var oGestionar = new GestionarPropuestaPoco();
            oGestionar.Propuesta = new PropuestaEntidad(); // entidad real
            oGestionar.ListaClientes = this.personaServicio.ListarClientes();
            oGestionar.ListaMonedas = this.subscripcionServicio.ListarMoneda();
            return View(oGestionar);
        }

        [HttpGet]
        public ActionResult Imprimir(int id)
        {

            var model = this.documentoOrigenServicio.ObtenerParaPdf(id);
           
            return View("Imprimir", model);
        }

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

                List<sp_ListarPropuesta> lista =
                    this.documentoOrigenServicio.Listar(cliente, estado);

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
        public ActionResult Guardar(GestionarPropuestaPoco oRegistro)
        {
            var res = new Resultado();

            try
            {
                if (Session["usuario"] == null)
                    return RedirectToAction("Index", "Login");

                if (oRegistro == null || oRegistro.Propuesta == null)
                {
                    res.idResultado = (int)enumTipoMensaje.error;
                    res.mensaje = "Modelo inválido.";
                    return Json(res, JsonRequestBehavior.AllowGet);
                }

                // set fechas/usuario si manejas auditoría
                if (oRegistro.Propuesta.idPropuesta == 0)
                {
                    //oRegistro.DocumentoOrigen.fechaRegistro = DateTime.Now;

                    // si quieres generar código aquí:
                    // oRegistro.DocumentoOrigen.codigo = documentoOrigenServicio.GenerarCodigo();

                    bool ok = this.documentoOrigenServicio.Insertar(oRegistro.Propuesta);

                    res.idResultado = ok ? (int)enumTipoMensaje.exito : (int)enumTipoMensaje.error;
                    res.mensaje = ok ? "Éxito al guardar el Documento de Origen" : "No se pudo guardar.";
                }
                else
                {
                    //oRegistro.DocumentoOrigen.fechaActualiza = DateTime.Now;

                    bool ok = this.documentoOrigenServicio.Actualizar(oRegistro.Propuesta);

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

    }

}
