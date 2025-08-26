using Mantenimiento.ERP.Helper;
using Mantenimiento.Negocio.Contratos.Servicios;
using Mantenimiento.Negocio.Poco;
using Mantenimiento.Negocio.Servicios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ReyDavid.Web.Controllers
{
    public class DashboardController : Controller
    {
        // GET: Administracion
        public IAutorServicio autorServicio { get; set; }
        public IUsuarioServicio usuarioServicio { get; set; }
        public IDocumentoIdentidadServicio documentoServicio { get; set; }
        public DashboardController()
        {
            this.autorServicio = IoCHelper.ResolverIoC<AutorServicio>();
            this.usuarioServicio = IoCHelper.ResolverIoC<UsuarioServicio>();
            this.documentoServicio = IoCHelper.ResolverIoC<DocumentoIdentidadServicio>();
        }

        public ActionResult Index()
        {
            if (Session["usuario"] == null)
            {
                return RedirectToAction("Index", "Login");
            }
            return View();
        }

        public ActionResult MisDatos()
        {
            if (Session["usuario"] == null)
            {
                return RedirectToAction("Index", "Login");
            }
            GestionarPersonaPoco oGestionar = new GestionarPersonaPoco();
            oGestionar.ListaDocumentoIdentidad = documentoServicio.Listar();
            oGestionar.Autor = autorServicio.Obtener(Convert.ToInt32(Session["idPersona"]));
            oGestionar.Usuario = usuarioServicio.Obtener(Convert.ToInt32(Session["idPersona"]));

            return View(oGestionar);
        }       

    }
}