using ERP.Web.Models;
using Mantenimiento.Datos.Entidades;
using Mantenimiento.ERP.Helper;
using Mantenimiento.Negocio.Contratos.Servicios;
using Mantenimiento.Negocio.Poco;
using Mantenimiento.Negocio.Servicios;
using ReyDavid.Web.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ReyDavid.Web.Controllers
{
    public class RequerimientoController : Controller
    {
        public IRequerimientoServicio requerimientoServicio { get; set; }
        public IDetalleRequerimiento detalleRequerimientoServicio { get; set; }
        public IPersonaServicio personaServicio { get; set; }
        public IProyectoServicio proyectoServicio { get; set; }
        public ITipoRequerimientoServicio tipoRequerimientoServicio { get; set; }
        public RequerimientoController()
        {
            this.requerimientoServicio = IoCHelper.ResolverIoC<IRequerimientoServicio>();
            this.personaServicio = IoCHelper.ResolverIoC<IPersonaServicio>();
            this.proyectoServicio = IoCHelper.ResolverIoC<IProyectoServicio>();
            this.tipoRequerimientoServicio = IoCHelper.ResolverIoC<ITipoRequerimientoServicio>();
            this.detalleRequerimientoServicio = IoCHelper.ResolverIoC<IDetalleRequerimiento>();
        }
        // GET: Requerimiento
        public ActionResult Index()
        {
            if (Session["usuario"] == null)
            {
                return RedirectToAction("Index", "Login");
            }

            GestionarRequerimientoPoco oGestionar = new GestionarRequerimientoPoco();
            oGestionar.ListarClientes = personaServicio.ListarClientes();
            oGestionar.ListarProyectos = proyectoServicio.ListarProyectos();

            return View(oGestionar);
        }
        public JsonResult Listar(int? cliente, int? proyecto, int? estado)
        {
            List<sp_ListarRequerimientosxFiltro> oLista = this.requerimientoServicio.ListaRequerimientos(cliente, proyecto, estado);
            return Json(oLista, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Nuevo()
        {
            if (Session["usuario"] == null)
            {
                return RedirectToAction("Index", "Login");
            }

            GestionarRequerimientoPoco oGestionar = new GestionarRequerimientoPoco();
            oGestionar.Codigo = requerimientoServicio.GenerarCodigoAutomatico();
            oGestionar.ListarClientes = personaServicio.ListarClientes();
            oGestionar.ListarTrabajadores = personaServicio.ListarTrabajadores();
            oGestionar.ListarProyectos = proyectoServicio.ListarProyectos();
            oGestionar.ListarTipoRequerimientos = tipoRequerimientoServicio.ListarTipoReq();
            oGestionar.Requerimiento = new Requerimiento();
            oGestionar.DetalleRequerimiento = new DetalleRequerimiento();

            return View(oGestionar);
        }

        [ValidateInput(false)]
        public ActionResult Guardar(GestionarRequerimientoPoco oRegistro)
        {
            Resultado objResultado = new Resultado();
            objResultado.idResultado = 0;
            oRegistro.Requerimiento.idEstado = 1;
            oRegistro.Requerimiento.fechaRegistro = DateTime.Now;

            objResultado.mensaje = "";

            Resultado res = new Resultado();
            try
            {
                if (Session["usuario"] == null)
                {
                    return RedirectToAction("Index", "Login");
                }

                string rutaBase = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Uploads\\Files\\");
                string rutaSubida;
                // Asegurarse de que el directorio base existe
                if (!Directory.Exists(rutaBase))
                {
                    Directory.CreateDirectory(rutaBase);
                }

                bool resp = false;
                ResultadoTransaccion oRes = new ResultadoTransaccion();

                if (oRegistro.Requerimiento.idRequerimiento == 0)
                {
                    foreach (var detalle in oRegistro.ListaDetalleRequerimiento)
                    {
                        if (detalle.archivo != null)
                        {
                            string nombreArchivo = Path.GetFileNameWithoutExtension(detalle.archivo.FileName);
                            string extensionArchivo = Path.GetExtension(detalle.archivo.FileName).ToLower();
                            string timestamp = DateTime.Now.ToString("yyyyMMdd-HHmmss-fff");
                            string archivoConTimestamp = $"{timestamp}-{nombreArchivo.Trim()}{extensionArchivo}";
                            
                            // Determinar la ruta de almacenamiento según la extensión del archivo
                            switch (extensionArchivo)
                            {
                                case ".jpg":
                                case ".jpeg":
                                case ".png":
                                    rutaSubida = Path.Combine(rutaBase, "Images");
                                    break;
                                case ".pdf":
                                    rutaSubida = Path.Combine(rutaBase, "PDFs");
                                    break;
                                case ".doc":
                                case ".docx":
                                    rutaSubida = Path.Combine(rutaBase, "WordDocuments");
                                    break;
                                case ".xls":
                                case ".xlsx":
                                    rutaSubida = Path.Combine(rutaBase, "ExcelSheets");
                                    break;
                                default:
                                    rutaSubida = Path.Combine(rutaBase, "Others");
                                    break;
                            }

                            string path = Path.Combine(rutaSubida, archivoConTimestamp);

                            // Verificar si el archivo existe, y si es así, agregar un GUID al nombre del archivo
                            while (System.IO.File.Exists(path)) // Aquí se usa System.IO.File en lugar de solo File
                            {
                                string uniqueSuffix = Guid.NewGuid().ToString();
                                archivoConTimestamp = $"{timestamp}-{nombreArchivo.Trim()}-{uniqueSuffix}{extensionArchivo}";
                                path = Path.Combine(rutaSubida, archivoConTimestamp);
                            }

                            // Guardar el archivo en la ruta especificada
                            try
                            {
                                detalle.archivo.SaveAs(path);
                                // Actualizar los detalles del archivo en el objeto `detalle`
                                detalle.nombreArchivo = archivoConTimestamp;
                                detalle.extension = extensionArchivo;
                            }
                            catch (Exception ex)
                            {
                                // Registrar el error en consola o logs
                                Console.WriteLine($"Error al guardar el archivo {detalle.archivo.FileName}: {ex.Message}");
                                objResultado.mensaje = $"Error al guardar el archivo {detalle.archivo.FileName}: {ex.Message}";
                                return Json(objResultado, JsonRequestBehavior.AllowGet);
                            }
                        }
                    }

                    resp = requerimientoServicio.Insertar(oRegistro);
                }
                else
                {
                    foreach (var detalle in oRegistro.ListaDetalleRequerimiento)
                    {
                        DetalleRequerimiento objDetalleRequerimiento = detalleRequerimientoServicio.Obtener(detalle.idDetalleRequerimiento);

                        if (detalle.archivo != null)
                        {
                            string nombreArchivo = Path.GetFileNameWithoutExtension(detalle.archivo.FileName);
                            string extensionArchivo = Path.GetExtension(detalle.archivo.FileName);

                            // Determinar la ruta de almacenamiento según la extensión del archivo
                            switch (extensionArchivo)
                            {
                                case ".jpg":
                                case ".jpeg":
                                case ".png":
                                    rutaSubida = Path.Combine(rutaBase, "Images");
                                    break;
                                case ".pdf":
                                    rutaSubida = Path.Combine(rutaBase, "PDFs");
                                    break;
                                case ".doc":
                                case ".docx":
                                    rutaSubida = Path.Combine(rutaBase, "WordDocuments");
                                    break;
                                case ".xls":
                                case ".xlsx":
                                    rutaSubida = Path.Combine(rutaBase, "ExcelSheets");
                                    break;
                                default:
                                    rutaSubida = Path.Combine(rutaBase, "Others");
                                    break;
                            }

                            if (objDetalleRequerimiento != null)
                            {
                                string nombreArchivoObjeto = objDetalleRequerimiento.nombreArchivo;
                                string nombreArchivoExistente = Path.GetFileNameWithoutExtension(nombreArchivoObjeto);
                                string extensionArchivoExistente = objDetalleRequerimiento.extension;


                                // Verificar si el nombre del archivo es el mismo y si el archivo ya existe
                                if (nombreArchivoExistente == nombreArchivo && extensionArchivoExistente == extensionArchivo
                                    && System.IO.File.Exists(Path.Combine(rutaSubida, nombreArchivoObjeto)))
                                {
                                    // Archivo en el objeto
                                    detalle.nombreArchivo = nombreArchivoObjeto;
                                    detalle.extension = extensionArchivoExistente;

                                    continue;  // Ir al siguiente detalle
                                }
                            }

                            // Si el archivo no existe o tiene un nombre diferente, guardar el nuevo archivo en el detalle
                            // Generar nombre de archivo único con timestamp y posibles GUIDs
                            string timestamp = DateTime.Now.ToString("yyyyMMdd-HHmmss-fff");
                            string nuevoNombreArchivo = $"{timestamp}-{nombreArchivo.Trim()}{extensionArchivo}";
                            string path = Path.Combine(rutaSubida, nuevoNombreArchivo);

                            // Verificar si el archivo existe y agregar un sufijo único si es necesario
                            while (System.IO.File.Exists(path))
                            {
                                string uniqueSuffix = Guid.NewGuid().ToString();
                                nuevoNombreArchivo = $"{timestamp}-{nombreArchivo.Trim()}-{uniqueSuffix}{extensionArchivo}";
                                path = Path.Combine(rutaSubida, nuevoNombreArchivo);
                            }

                            // Cargar la archivo desde el stream
                            detalle.archivo.SaveAs(path);

                            // Actualizar los detalles del archivo en el objeto `detalle`
                            detalle.nombreArchivo = nuevoNombreArchivo;
                            detalle.extension = extensionArchivo;
                        }
                    }

                    resp = requerimientoServicio.Actualizar(oRegistro);
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

            GestionarRequerimientoPoco oGestionar = new GestionarRequerimientoPoco();
            oGestionar.ListarClientes = personaServicio.ListarClientes();
            oGestionar.ListarTrabajadores = personaServicio.ListarTrabajadores();
            oGestionar.ListarProyectos = proyectoServicio.ListarProyectos();
            oGestionar.ListarTipoRequerimientos = tipoRequerimientoServicio.ListarTipoReq();
            oGestionar.Requerimiento = requerimientoServicio.ObtenerRequerimiento(id);
            oGestionar.DetalleRequerimiento = new DetalleRequerimiento();
            oGestionar.ListaDetalleRequerimiento = detalleRequerimientoServicio.ObtenerDetalleReq(id);

            // Crear una lista para almacenar los folders únicos
            List<ListaFoldersMapping> folders = new List<ListaFoldersMapping>();

            // Iterar sobre cada detalle de requerimiento
            foreach (var detalle in oGestionar.ListaDetalleRequerimiento)
            {
                // Verificar que tanto la extensión como el nombre del archivo no sean nulos o vacíos
                if (!string.IsNullOrEmpty(detalle.extension) && !string.IsNullOrEmpty(detalle.nombreArchivo))
                {
                    // Obtener la extensión del archivo
                    string extension = detalle.extension.ToLower();

                    // Obtener el folder correspondiente a la extensión
                    string folder = requerimientoServicio.GetSubFolderByExtension(extension);

                    // Verificar si ya existe un objeto ListaFoldersMapping con la misma extensión
                    var existingMapping = folders.FirstOrDefault(m => m.extension == extension);

                    if (existingMapping == null)
                    {
                        // Si no existe, crear un nuevo objeto ListaFoldersMapping y agregarlo a la lista
                        folders.Add(new ListaFoldersMapping { extension = extension, folder = folder });
                    }
                }
            }

            // Actualizar la lista de folders en el modelo
            oGestionar.ListaFoldersMapping = folders;

            return View(oGestionar);
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

                this.requerimientoServicio.Eliminar(id);

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