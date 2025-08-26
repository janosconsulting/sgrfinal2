
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using ERP.Web.Models;
using System.Web.Helpers;
using Mantenimiento.Datos.Entidades;
using Mantenimiento.ERP.Helper;
using Mantenimiento.Negocio.Contratos.Servicios;
using Mantenimiento.Negocio.Poco;
using Mantenimiento.Negocio.Servicios;
using ReyDavid.Web.Models;
using System.Data.SqlClient;
using System.IO;
using System.Web.Mvc;
using System.Drawing;
using System.Drawing.Imaging;


namespace ReyDavid.Web.Controllers
{
    public class RequerimientosporTrabajadorController : Controller
    {
        // Inyección de dependencias para el servicio de requerimientos por trabajador
        public IRequerimientosporTrabajadorServicio requerimientosporTrabajadorServicio { get; set; }
        public IPersonaServicio personaServicio { get; set; }
        public IDetalleRequerimiento detalleRequerimientoServicio { get; set; }

        public RequerimientosporTrabajadorController()
        {
            this.requerimientosporTrabajadorServicio = IoCHelper.ResolverIoC<IRequerimientosporTrabajadorServicio>();
            this.personaServicio = IoCHelper.ResolverIoC<IPersonaServicio>();
            this.detalleRequerimientoServicio = IoCHelper.ResolverIoC<IDetalleRequerimiento>();
        }

        // Vista principal
        public ActionResult Index(int? id)
        {
            if (Session["usuario"] == null)
            {
                return RedirectToAction("Index", "Login");
            }

            GestionarRequerimientoPoco oGestionar = new GestionarRequerimientoPoco();
            oGestionar.listarRequerimientosporTrabajador = this.requerimientosporTrabajadorServicio.listarRequerimientosporTrabajador();
            oGestionar.ListarTrabajadores = personaServicio.ListarTrabajadores();          

            return View(oGestionar);

        }

        // Método para listar todos (o por trabajador si se envía ID)
        public JsonResult Listar(int? idTrabajador = null)
        {
            List<sp_listarRequerimientosporTrabajador> lista = requerimientosporTrabajadorServicio.listarRequerimientosporTrabajador(idTrabajador);
            return Json(lista, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Editar(int? idDetalleRequerimiento)
        {
            if (!idDetalleRequerimiento.HasValue)
            {
                TempData["Error"] = "No se especificó el detalle a editar.";
                return RedirectToAction("Index");
            }
            try
            {
                var detalle = requerimientosporTrabajadorServicio.ObtenerDetallePorId(idDetalleRequerimiento.Value);
                if (detalle == null)
                    throw new Exception("No se encontró el detalle con el id proporcionado.");

                var listaTrabajadores = requerimientosporTrabajadorServicio.ListarTrabajadores();
                if (listaTrabajadores == null)
                    listaTrabajadores = new List<sp_ListarTrabajadores>();

                var modelo = new GestionarRequerimientoPoco
                {
                    DetalleRequerimiento = detalle,
                    ListarTrabajadores = listaTrabajadores
                };

                return View(modelo);
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Error al cargar el detalle: " + ex.Message;
                return RedirectToAction("Index");
            }
        }
        [HttpPost]
        [ValidateInput(false)]
        public JsonResult ActualizarDetalle(
         int IdDetalleRequerimiento,
         int? IdPersona,
         string Descripcion,
         string ComentarioCliente,
         int EstadoDesarrollo,
         int EstadoCliente,
         string FechaInicio,
         string FechaFin,
         string Modulo,
         string CriterioAceptacion
)
        {
            try
            {
                DateTime? fechaInicio = ParseFecha(FechaInicio);
                DateTime? fechaFin = ParseFecha(FechaFin);

                var detalle = new DetalleRequerimiento
                {
                    idDetalleRequerimiento = IdDetalleRequerimiento,
                    idPersona = IdPersona,
                    descripcion = Descripcion,
                    comentarioCliente = ComentarioCliente,
                    estadoDesarrollo = EstadoDesarrollo == -1 ? (int?)null : EstadoDesarrollo,
                    estadoCliente = EstadoCliente == -1 ? (int?)null : EstadoCliente,
                    fechaInicio = fechaInicio,
                    fechaFin = fechaFin,
                    modulo = Modulo,
                    criterioAceptacion = CriterioAceptacion
                    // Si tienes otros campos, agrégalos también
                };

                var poco = new GestionarRequerimientoPoco
                {
                    DetalleRequerimiento = detalle
                };

                bool actualizado = requerimientosporTrabajadorServicio.Actualizar(poco);

                if (actualizado)
                    return Json(new { success = true });

                return Json(new { success = false, message = "No se pudo actualizar." });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        // Método para parsear fecha dd/MM/yyyy
        private DateTime? ParseFecha(string fecha)
        {
            if (DateTime.TryParseExact(fecha, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime fechaParseada))
                return fechaParseada;
            return null;
        }

    }
}