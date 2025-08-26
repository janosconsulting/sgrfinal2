using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.SqlClient;
using System.IO;
using ReyDavid.Web.Models;
using Mantenimiento.Negocio.Contratos.Servicios;
using Mantenimiento.ERP.Helper;
using ERP.Web.Models;
using Mantenimiento.Datos.Entidades;
using Mantenimiento.Negocio.Poco;
using Mantenimiento.Negocio.Servicios;

namespace ReyDavid.Web.Controllers
{
    public class ProyectoController : Controller
    {
        public IProyectoServicio proyectoServicio { get; set; }

        public ProyectoController()
        {
            this.proyectoServicio = IoCHelper.ResolverIoC<ProyectoServicio>();
        }


        // GET: Proyecto
        public ActionResult Index()
        {
            if (Session["usuario"] == null)
            {
                return RedirectToAction("Index", "Login");
            }

            GestionarProyectoPoco oGestionar = new GestionarProyectoPoco();
            oGestionar.listarProyectos = this.proyectoServicio.ListarProyectos();

            return View(oGestionar);
         
        }
        public JsonResult Listar()
        {
            List<sp_ListarProyectos> oLista = this.proyectoServicio.ListarProyectos();
            return Json(oLista, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Nuevo()
        {
            if (Session["usuario"] == null)
            {
                return RedirectToAction("Index", "Login");
            }

            GestionarProyectoPoco oGestionar = new GestionarProyectoPoco();
            oGestionar.proyecto = new Proyecto();
            return View(oGestionar);
        }
        public ActionResult Guardar(ProyectoModel oRegistro)
        {
            Resultado objResultado = new Resultado();
            objResultado.idResultado = 0;
            oRegistro.proyecto.idEstado = 1;
            oRegistro.proyecto.fechaRegistro = DateTime.Now;

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
                if (oRegistro.proyecto.idProyecto == 0)
                {
                    resp = proyectoServicio.Insertar(oRegistro.proyecto);
                }
                else
                {
                    resp = proyectoServicio.Actualizar(oRegistro.proyecto);
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

            GestionarProyectoPoco oGestionar = new GestionarProyectoPoco();          
            oGestionar.proyecto = this.proyectoServicio.obtenerProyecto(id);
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

                this.proyectoServicio.Eliminar(id);

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