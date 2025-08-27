using Mantenimiento.Datos.Entidades;
using Mantenimiento.ERP.Helper;
using Mantenimiento.Negocio.Contratos.Servicios;
using Mantenimiento.Negocio.Poco;
using Mantenimiento.Negocio.Servicios;
using System;
using System.Collections.Generic;
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
            return View();
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
            return View(oGestionar);
        }
        public JsonResult Listar()
        {
            List<sp_ListarSubscripcion> oLista = this.servicio.ListarSubscripcion();
            return Json(oLista, JsonRequestBehavior.AllowGet);
        }
    }
}