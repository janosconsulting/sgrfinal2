using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
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
using System.Drawing;
using System.Drawing.Imaging;

namespace ReyDavid.Web.Controllers
{
    public class TrabajadorController : Controller
    {

        public IPersonaServicio personaServicio { get; set; }
        public IDocumentoIdentidadServicio documentoServicio { get; set; }
        public TrabajadorController()
        {
            this.personaServicio = IoCHelper.ResolverIoC<PersonaServicio>();
            this.documentoServicio = IoCHelper.ResolverIoC<DocumentoIdentidadServicio>();
        }
        public ActionResult Index()
        {

            if (Session["usuario"] == null)
            {
                return RedirectToAction("Index", "Login");
            }

            GestionarTrabajadorPoco oGestionar = new GestionarTrabajadorPoco();
            oGestionar.listarTrabajadores = this.personaServicio.ListarTrabajadores();

            return View(oGestionar);
        }
        public JsonResult ValidarNdocumento(string numeroDocumento)
        {
            try
            {
                int a = 3;

                bool oDato = this.personaServicio.ValidarNdocumento(numeroDocumento);
                return Json(oDato, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { error = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult Listar()
        {
            List<sp_ListarClientes> oLista = this.personaServicio.ListarClientes();
            return Json(oLista, JsonRequestBehavior.AllowGet);
        }
        public JsonResult ListarTrabajador()
        {
            List<sp_ListarTrabajadores> oLista = this.personaServicio.ListarTrabajadores();
            return Json(oLista, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Nuevo()
        {
            if (Session["usuario"] == null)
            {
                return RedirectToAction("Index", "Login");
            }

            GestionarTrabajadorPoco oGestionar = new GestionarTrabajadorPoco();
            oGestionar.ListarDocumentoIdentidad = documentoServicio.Listar();
            oGestionar.Persona = new Persona();
            return View(oGestionar);
        }
        public ActionResult Guardar(PersonaModel oRegistro)
        {
            Resultado objResultado = new Resultado();
            objResultado.idResultado = 0;
            oRegistro.Persona.idEstado = 1;

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
                if (oRegistro.Persona.idPersona == 0)
                {
                    resp = personaServicio.Insertar(oRegistro.Persona);
                }
                else
                {
                    resp = personaServicio.Actualizar(oRegistro.Persona);
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

            GestionarTrabajadorPoco oGestionar = new GestionarTrabajadorPoco();
            oGestionar.ListarDocumentoIdentidad = documentoServicio.Listar();
            oGestionar.Persona = this.personaServicio.obtenerPersona(id);
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

                this.personaServicio.Eliminar(id);

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