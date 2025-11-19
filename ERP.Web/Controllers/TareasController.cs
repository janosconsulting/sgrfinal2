using System;
using System.Collections.Generic;
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
    public class TareasController : Controller
    {
        public ITareaServicio tareaServicio { get; set; }
        public IPersonaServicio personaServicio { get; set; }
        public IProyectoServicio proyectoServicio { get; set; }
        public ITipoRequerimientoServicio tipoRequerimientoServicio { get; set; }
        public IRequerimientoServicio requerimientoServicio { get; set; }
        public TareasController()
        {
            this.tareaServicio = IoCHelper.ResolverIoC<TareaServicio>();
            this.personaServicio = IoCHelper.ResolverIoC<IPersonaServicio>();
            this.proyectoServicio = IoCHelper.ResolverIoC<IProyectoServicio>();
            this.tipoRequerimientoServicio = IoCHelper.ResolverIoC<ITipoRequerimientoServicio>();
            this.requerimientoServicio = IoCHelper.ResolverIoC<IRequerimientoServicio>();
        }
        public ActionResult Index()
        {

            if (Session["usuario"] == null)
            {
                return RedirectToAction("Index", "Login");
            }

            GestionarTareaPoco oGestionar = new GestionarTareaPoco();
            oGestionar.listarTareas = this.tareaServicio.ListarTareas();

            return View(oGestionar);
        }

        public ActionResult Seguimiento()
        {
            if (Session["usuario"] == null)
            {
                return RedirectToAction("Index", "Login");
            }
            GestionarTareaPoco oGestionar = new GestionarTareaPoco();
            oGestionar.ListarProyectos = proyectoServicio.ListarProyectos();
            oGestionar.ListarPersonaResponsable = personaServicio.ListarPersonaResponsable();
            return View(oGestionar);
        }
        public JsonResult ListarSeguimientoTareas(int idProyecto,int idEstado, int idResponsable)
        {
            List<sp_ListarSeguimientoTareas> oLista = this.tareaServicio.ListarSeguimientoTareas(idProyecto, idEstado, idResponsable);
            return Json(oLista, JsonRequestBehavior.AllowGet);
        }
        public JsonResult Listar()
        {
            List<sp_ListarTareas> oLista = this.tareaServicio.ListarTareas();
            return Json(oLista, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Nuevo()
        {
            if (Session["usuario"] == null)
            {
                return RedirectToAction("Index", "Login");
            }

            GestionarTareaPoco oGestionar = new GestionarTareaPoco();
            oGestionar.ListarClientes = personaServicio.ListarClientes();
            oGestionar.ListarTrabajadores = personaServicio.ListarTrabajadores();
            oGestionar.ListarProyectos = proyectoServicio.ListarProyectos();
            oGestionar.ListarTipoRequerimientos = tipoRequerimientoServicio.ListarTipoReq();
            oGestionar.tarea = new Tarea();
            return View(oGestionar);
        }

        [ValidateInput(false)]
        public ActionResult Guardar(TareaModel oRegistro)
        {
            Resultado objResultado = new Resultado();
            objResultado.idResultado = 0;
            //oRegistro.tarea.idEstado = 1;
            oRegistro.tarea.fechaRegistro = DateTime.Now;

            objResultado.mensaje = "";

            Resultado res = new Resultado();
            try
            {
                if (Session["usuario"] == null)
                {
                    return RedirectToAction("Index", "Login");
                }

                bool resp = false;
                ResultadoTransaccion oRes = new ResultadoTransaccion();
                if (oRegistro.tarea.idTarea == 0)
                {
                    resp = tareaServicio.Insertar(oRegistro.tarea);
                }
                else
                {
                    resp = tareaServicio.Actualizar(oRegistro.tarea);
                }

                if (resp)
                {
                    oRes.codigo = 1;
                }

                if (oRes.codigo > 0)
                {
                    res.idResultado = (int)enumTipoMensaje.exito;
                    res.mensaje = "Éxito al guardar el Registro";
                    res.codigo = oRes.codigo;
                }
                else
                {
                    res.idResultado = (int)enumTipoMensaje.error;
                    res.mensaje = oRes.mensaje.ToString();
                }
            }
            catch (SqlException sqlEx)
            {
                // Imprime detalles de la excepción SQL en la consola
                Console.WriteLine("Error SQL: " + sqlEx.Message);
                Console.WriteLine("Número de error: " + sqlEx.Number);
                Console.WriteLine("Procedimiento almacenado: " + sqlEx.Procedure);
                Console.WriteLine("Línea de error: " + sqlEx.LineNumber);

                throw;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error general: " + ex.Message);
            }

            return Json(res, JsonRequestBehavior.AllowGet);

        }
        public ActionResult Editar(int id)
        {
            if (Session["usuario"] == null)
            {
                return RedirectToAction("Index", "Login");
            }

            GestionarTareaPoco oGestionar = new GestionarTareaPoco();
            oGestionar.ListarClientes = personaServicio.ListarClientes();
            oGestionar.ListarTrabajadores = personaServicio.ListarTrabajadores();
            oGestionar.ListarProyectos = proyectoServicio.ListarProyectos();
            oGestionar.ListarTipoRequerimientos = tipoRequerimientoServicio.ListarTipoReq();
            oGestionar.tarea = this.tareaServicio.obtenerTarea(id);
            var requerimiento = requerimientoServicio.ObtenerRequerimiento(oGestionar.tarea.idRequerimiento);
            oGestionar.tarea.codigoRequerimiento = requerimiento?.codigo;
            oGestionar.TrEditing = true;
            return View("Nuevo", oGestionar);
        }
        public ActionResult Eliminar(int id)
        {
            Resultado objResultado = new Resultado();
            objResultado.idResultado = 0;

            objResultado.mensaje = "";
            Resultado res = new Resultado();
            try
            {
                if (Session["usuario"] == null)
                {
                    return RedirectToAction("Index", "Login");
                }

                this.tareaServicio.Eliminar(id);

                res.idResultado = (int)enumTipoMensaje.exito;
                res.mensaje = "Éxito al eliminar el Registro";
            }
            catch (Exception ex)
            {

                res.idResultado = (int)enumTipoMensaje.error;
                if (ex.InnerException != null)
                {
                    res.mensaje = ex.InnerException.Message;
                }
                else
                {
                    res.mensaje = ex.Message;
                }
            }

            return Json(res, JsonRequestBehavior.AllowGet);
        }
    }
}