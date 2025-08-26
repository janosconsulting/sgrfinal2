using Mantenimiento.Datos.Entidades;
using Mantenimiento.Negocio.Contratos.Servicios;
using Mantenimiento.Negocio.Servicios; // para crear instancia si no usas IoC
using ReyDavid.Web.Models;
using System;
using System.Collections.Generic;
using System.Web.Mvc;
using ERP.Web.Models;
using Mantenimiento.ERP.Helper;
using Mantenimiento.Negocio.Poco;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;

namespace ReyDavid.Web.Controllers
{
    public class CategoriaClienteController : Controller
    {
        public ICategoriaClienteServicio categoriaClienteServicio { get; set; }

        public CategoriaClienteController()
        {
            // Si tienes IoC (inyección de dependencias) usa algo similar a:
            // this.categoriaClienteServicio = IoCHelper.ResolverIoC<CategoriaClienteServicio>();

            // Si no, instancia manual:
            this.categoriaClienteServicio = IoCHelper.ResolverIoC<CategoriaClienteServicio>();
        }

        // Acción que retorna la vista principal con la lista en el modelo
        public ActionResult Index()
        {

            if (Session["usuario"] == null)
            {
                return RedirectToAction("Index", "Login");
            }

            GestionarCategoriaClientePoco oGestionar = new GestionarCategoriaClientePoco();
            oGestionar.listarCategoriaClientes = this.categoriaClienteServicio.ListarCategoriaClientes();

            return View(oGestionar);
        }

        // Acción que devuelve JSON con la lista (para uso AJAX o APIs)
        public JsonResult Listar()
        {
            try
            {
                List<sp_ListarCategoriaCliente> lista = this.categoriaClienteServicio.ListarCategoriaClientes();
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

            GestionarCategoriaClientePoco oGestionar = new GestionarCategoriaClientePoco();
            oGestionar.CategoriaCliente = new CategoriaCliente();
            oGestionar.listarCategoriaClientes = this.categoriaClienteServicio.ListarCategoriaClientes();
            return View(oGestionar);
        }
        public ActionResult Guardar(CategoriaClienteModel oRegistro)
        {
            Resultado objResultado = new Resultado();
            objResultado.idResultado = 0;
            //oRegistro.CategoriaCliente.estado = 1;

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
                if (oRegistro.CategoriaCliente.idCategoria == 0)
                {
                    resp = categoriaClienteServicio.Insertar(oRegistro.CategoriaCliente);
                }
                else
                {
                    resp = categoriaClienteServicio.Actualizar(oRegistro.CategoriaCliente);
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

            GestionarCategoriaClientePoco oGestionar = new GestionarCategoriaClientePoco();
            oGestionar.CategoriaCliente = this.categoriaClienteServicio.obtenerCategoriaCliente(id);
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

                this.categoriaClienteServicio.Eliminar(id);

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
    


