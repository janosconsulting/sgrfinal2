using ERP.Web.Models;
using Mantenimiento.Datos.Entidades;
using Mantenimiento.ERP.Helper;
using Mantenimiento.Negocio.Contratos.Servicios;
using Mantenimiento.Negocio.Servicios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ReyDavid.Web.Controllers
{
    public class LoginController : Controller
    {
        // GET: Login
        public IUsuarioServicio usuarioServicio { get; set; }

        public LoginController()
        {
            this.usuarioServicio = IoCHelper.ResolverIoC<UsuarioServicio>();
        }

        public ActionResult Index()
        {
            if (Convert.ToInt32(Session["idUsuario"]) == 0)
            {
                return View();
            }
            else
            {
                return RedirectToAction("Index", "Cliente");
            }
        }

        public ActionResult Ingresar(CredencialViewModel credencial)
        {
            CredencialViewModel model = new CredencialViewModel();
           
            if (credencial.Usuario == null)
            {
                ViewBag.Mensaje = "Debe ingresar el nombre de Usuario";
                return View("Index");
            }

            if (credencial.Password == null)
            {
                ViewBag.Mensaje = "Debe ingresar la contraseña";
                return View("Index");
            }
            
            Usuario objUsuario = this.usuarioServicio.ValidarLogin(credencial.Usuario, credencial.Password);
           
            if (objUsuario != null)
            {
                Session["idUsuario"] = objUsuario.idUsuario;
                Session["usuario"] = objUsuario.username;
                Session["idPersona"] = objUsuario.idAlumno;                
                Session["esAdmin"] = objUsuario.esAdmin;                
                return RedirectToAction("Index", "Cliente");
                
            }
            else
            {
                ViewBag.Mensaje = "Credenciales ingresadas incorrectas";
                return View("Index");
            }
        }

        public JsonResult ValidarLogin(CredencialViewModel credencial)
        {
            Usuario objUsuario = this.usuarioServicio.ValidarLogin(credencial.Usuario, credencial.Password);
            
            if(objUsuario != null)
            {
                return Json(objUsuario, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new Usuario(), JsonRequestBehavior.AllowGet) ;
            }
          
        }

        public ActionResult Salir()
        {
            Session["usuario"] = null;
            Session["idPersona"] = null;
            Session["idUsuario"] = null;
            Session["esAdmin"] = null;
            return RedirectToAction("Index", "Home");
        }
    }
}