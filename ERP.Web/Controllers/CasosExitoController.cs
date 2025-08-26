using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Mantenimiento.Datos.Entidades;
using Mantenimiento.Negocio.Contratos.Servicios;
using Mantenimiento.Negocio.Servicios; // para crear instancia si no usas IoC
using ReyDavid.Web.Models;
using System.Web.Mvc;
using ERP.Web.Models;
using Mantenimiento.ERP.Helper;
using Mantenimiento.Negocio.Poco;
using System.Data.SqlClient;
using System.IO;


namespace ReyDavid.Web.Controllers
{
    public class CasosExitoController : Controller
    {
        public ICasosExitoServicio casosExitoServicio { get; set; }
        public IPersonaServicio personaServicio { get; set; }
        public IPaisServicio paisServicio { get; set; }

        public CasosExitoController()
        {
            this.casosExitoServicio = IoCHelper.ResolverIoC<CasosExitoServicio>();
            this.personaServicio = IoCHelper.ResolverIoC<PersonaServicio>();
            this.paisServicio = IoCHelper.ResolverIoC<PaisServicio>();

        }

        // Acción que retorna la vista principal con la lista en el modelo
        public ActionResult Index()
        {

            if (Session["usuario"] == null)
            {
                return RedirectToAction("Index", "Login");
            }

            GestionarCasosExitoPoco oGestionar = new GestionarCasosExitoPoco();
            oGestionar.listarCasosExito = this.casosExitoServicio.ListarCasosExito();

            return View(oGestionar);
        }

        // Acción que devuelve JSON con la lista (para uso AJAX o APIs)
        public JsonResult Listar()
        {
            try
            {
                List<sp_ObtenerCasosExito> lista = this.casosExitoServicio.ListarCasosExito();
                return Json(lista, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { error = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult Nuevo()
        {
            if (Session["usuario"] == null)
            {
                return RedirectToAction("Index", "Login");
            }

            GestionarCasosExitoPoco oGestionar = new GestionarCasosExitoPoco();
            oGestionar.CasosExito = new CasosExito();
            oGestionar.listarCasosExito = this.casosExitoServicio.ListarCasosExito();
            oGestionar.listarClientes = this.personaServicio.ListarClientes();
            oGestionar.listarPaises = this.paisServicio.ListarPaises();
            return View(oGestionar);
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Guardar(CasosExitoModel oRegistro, HttpPostedFileBase archivoImagen)
        {
            Resultado objResultado = new Resultado();
            objResultado.idResultado = 0;
            oRegistro.CasosExito.fechaRegistro = DateTime.Now;
            objResultado.mensaje = "";

            Resultado res = new Resultado();

            try
            {
                if (Session["usuario"] == null)
                {
                    return RedirectToAction("Index", "Login");
                }

                // 1. Procesar imagen si se envía
                if (archivoImagen != null && archivoImagen.ContentLength > 0)
                {
                    string extensionArchivo = Path.GetExtension(archivoImagen.FileName).ToLower();
                    if (extensionArchivo == ".jpg" || extensionArchivo == ".jpeg" || extensionArchivo == ".png")
                    {
                        string rutaBase = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Uploads", "Files","CasosExito");
                        if (!Directory.Exists(rutaBase))
                            Directory.CreateDirectory(rutaBase);

                        string timestamp = DateTime.Now.ToString("yyyyMMdd-HHmmss-fff");
                        string nombreArchivo = Path.GetFileName(archivoImagen.FileName);
                        string pathCompleto = Path.Combine(rutaBase, nombreArchivo);

                        // Guardar el archivo en disco
                        archivoImagen.SaveAs(pathCompleto);

                        // Asignar a la entidad Persona
                        oRegistro.CasosExito.nombreArchivo = nombreArchivo;
                        oRegistro.CasosExito.extension = extensionArchivo;
                    }
                }

                bool resp = false;
                ResultadoTransaccion oRes = new ResultadoTransaccion();

                if (oRegistro.CasosExito.idCaso == 0)
                {
                    resp = casosExitoServicio.Insertar(oRegistro.CasosExito);
                }
                else
                {
                    resp = casosExitoServicio.Actualizar(oRegistro.CasosExito);
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
                    res.mensaje = oRes.mensaje;
                }
            }
            catch (SqlException sqlEx)
            {
                Console.WriteLine("Error SQL: " + sqlEx.Message);
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

            GestionarCasosExitoPoco oGestionar = new GestionarCasosExitoPoco();
            oGestionar.CasosExito = this.casosExitoServicio.obtenerCasosExito(id);
            oGestionar.listarCasosExito = this.casosExitoServicio.ListarCasosExito();
            oGestionar.listarClientes = this.personaServicio.ListarClientes();
            oGestionar.listarPaises = this.paisServicio.ListarPaises();
            oGestionar.IsEditing = true;
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

                this.casosExitoServicio.Eliminar(id);

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