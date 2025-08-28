using ERP.Web.Models;
using Mantenimiento.Datos.Entidades;
using Mantenimiento.ERP.Helper;
using Mantenimiento.Negocio.Contratos.Servicios;
using Mantenimiento.Negocio.Poco;
using Mantenimiento.Negocio.Servicios;
using ReyDavid.Web.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ERP.Web.Controllers
{
    public class SubscripcionController : Controller
    {
        public ISubscripcionServicio servicio { get; set; }
        public IPersonaServicio servicioCliente { get; set; }
        public SubscripcionController()
        {
            this.servicio = IoCHelper.ResolverIoC<SubscripcionServicio>();
            this.servicioCliente = IoCHelper.ResolverIoC<PersonaServicio>();
        }

        // GET: Subscripcion
        public ActionResult Index()
        {
            GestionarClientePoco oGestionar = new GestionarClientePoco();
            oGestionar.ListarFrecuencia = this.servicio.ListarFrecuencia();
            oGestionar.ListarServicio = this.servicio.ListarServicio();
            return View(oGestionar);
        }
        public ActionResult Nuevo()
        {
            if (Session["usuario"] == null)
            {
                return RedirectToAction("Index", "Login");
            }

            GestionarClientePoco oGestionar = new GestionarClientePoco();
            oGestionar.ListarMoneda = this.servicio.ListarMoneda();
            oGestionar.ListarFrecuencia = this.servicio.ListarFrecuencia();
            oGestionar.ListarServicio = this.servicio.ListarServicio();
            oGestionar.listarClientes = this.servicioCliente.ListarClientes();
            ViewBag.esEditar = 0;
            ViewBag.esRenovacion = 1;//no

            return View(oGestionar);
        }
        public ActionResult Editar(int id)
        {
            if (Session["usuario"] == null)
            {
                return RedirectToAction("Index", "Login");
            }

            GestionarClientePoco oGestionar = new GestionarClientePoco();
            oGestionar.ListarMoneda = this.servicio.ListarMoneda();
            oGestionar.ListarFrecuencia = this.servicio.ListarFrecuencia();
            oGestionar.ListarServicio = this.servicio.ListarServicio();
            oGestionar.listarClientes = this.servicioCliente.ListarClientes();
            ViewBag.esEditar = id;
            ViewBag.esRenovacion = 1;//no

            oGestionar.ObtenerSubscripcion = this.servicio.Obtener(id);
            return View("Nuevo",oGestionar);
        }
        public ActionResult Renovacion(int id)
        {
            if (Session["usuario"] == null)
            {
                return RedirectToAction("Index", "Login");
            }

            GestionarClientePoco oGestionar = new GestionarClientePoco();
            oGestionar.ListarMoneda = this.servicio.ListarMoneda();
            oGestionar.ListarFrecuencia = this.servicio.ListarFrecuencia();
            oGestionar.ListarServicio = this.servicio.ListarServicio();
            oGestionar.listarClientes = this.servicioCliente.ListarClientes();
            ViewBag.esEditar = id;
            ViewBag.esRenovacion = 2;//si
            oGestionar.ObtenerSubscripcion = this.servicio.Obtener(id);
            return View("Nuevo", oGestionar);
        }
        public JsonResult Listar(int idServicio, int idFrecuencia, int idEstado, int anio, int mes)
        {
            GestionarSubscripcion oLista = this.servicio.ListarSubscripcion( idServicio, idFrecuencia, idEstado, anio, mes);
            return Json(new
            {
                data = oLista.listaSuscripcion1,         // Esto es lo que usará DataTables
                infoExtra = oLista.resumenDetallSubscripcionCobrados,
                infoActios= oLista.resumenDetallSubscripcionActivos
            }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Guardar(Subscripcion oRegistro)
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

                ResultadoTransaccion oRes = new ResultadoTransaccion();
                if (oRegistro.idSubscripcion == 0)
                {
                    oRes = this.servicio.Insertar(oRegistro);
                }
                else
                {
                    if(oRegistro.estadoFormulario == 3)
                    {
                        oRes = this.servicio.CobrarSuscripcion(oRegistro);

                    }else if(oRegistro.estadoFormulario == 4)
                    {
                        oRes = this.servicio.RenovarSuscripcion(oRegistro);
                    }
                    else
                    {
                        oRes = this.servicio.Actualizar(oRegistro);

                    }
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
            
            catch (Exception ex)
            {
                Console.WriteLine("Error general: " + ex.Message);
            }

            return Json(res, JsonRequestBehavior.AllowGet);

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

                this.servicio.Eliminar(id);

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