using ERP.Web.Models;
using Mantenimiento.Datos.Entidades;
using Mantenimiento.ERP.Helper;
using Mantenimiento.Negocio.Contratos.Servicios;
using Mantenimiento.Negocio.Poco;
using Mantenimiento.Negocio.Servicios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http.Results;
using System.Web.Mvc;

namespace ReyDavid.Web.Controllers
{
    public class AutorController : Controller
    {
        public IAutorServicio autorServicio { get; set; }
        public IUsuarioServicio usuarioServicio { get; set; }

        public IDocumentoIdentidadServicio documentoServicio { get; set; }
        public AutorController()
        {
            this.autorServicio = IoCHelper.ResolverIoC<AutorServicio>();
            this.usuarioServicio = IoCHelper.ResolverIoC<UsuarioServicio>();
            this.documentoServicio = IoCHelper.ResolverIoC<DocumentoIdentidadServicio>();
        }

        // GET: Autor
        public ActionResult Index()
        {            
            return View();
        }

        public JsonResult Listar()
        {
            List<sp_ListarPersonas_Result> oListaAutor = autorServicio.Listar();
            return Json(oListaAutor, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Perfiles()
        {
            List<sp_ListarPerfiles_Result> oListaPerfil = autorServicio.ListarPerfil();
            return View(oListaPerfil);
        }
        public ActionResult Editar(int id)
        {
            GestionarPersonaPoco oGestionar = new GestionarPersonaPoco();

            oGestionar.Autor = autorServicio.Obtener(id);
            oGestionar.Usuario = usuarioServicio.Obtener(id);
            return Json(oGestionar, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Perfil()
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

        public ActionResult Registrar(GestionarPersonaPoco oRegistro)
        {            
            Resultado res = new Resultado();
            try
            {
                
                ResultadoTransaccion oRes = new ResultadoTransaccion();
                if (oRegistro.Autor.idAutor == 0)
                {
                    oRes = autorServicio.Insertar(oRegistro);
                }
                else
                {
                    bool resp = autorServicio.Actualizar(oRegistro);
                    if (resp)
                    {
                        oRes.codigo = 1;

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

        public ActionResult Eliminar(int id)
        {
            Resultado objResultado = new Resultado();
            objResultado.idResultado = 0;

            objResultado.mensaje = "";
            Resultado res = new Resultado();
            try
            {
                
                this.autorServicio.Eliminar(id);

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