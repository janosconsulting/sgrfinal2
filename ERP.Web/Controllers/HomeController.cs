using Mantenimiento.Datos.Entidades;
using Mantenimiento.ERP.Helper;
using Mantenimiento.Negocio.Contratos.Servicios;
using Mantenimiento.Negocio.Servicios;
using ReyDavid.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.IO;



namespace ReyDavid.Web.Controllers
{
    public class HomeController : Controller
    {
       
        public HomeController()
        {
        }

        public ActionResult Index()
        {
            
            return View();
        }


        public ActionResult AboutAs()
        {
            return View();
        }

        public ActionResult Gallery()
        {
            return View();
        }
        public ActionResult Nosotros()
        {
            return View();
        }
        public ActionResult Organigrama()
        {
            return View();
        }
        public ActionResult AsesoriayConsultoria()
        {
            return View();
        }
        public ActionResult DesarrollodeSistemas()
        {
            return View();
        }
        public ActionResult kedroPro()
        {
            return View();
        }
        private string ObtenerRubroDesdeNombreArchivo(string nombreArchivo)
        {
            if (string.IsNullOrEmpty(nombreArchivo))
                return "GENERAL";

            string nombreSinExtension = Path.GetFileNameWithoutExtension(nombreArchivo); // industria_logoDavid
            string[] partes = nombreSinExtension.Split('_');

            if (partes.Length > 0)
                return partes[0].Replace("-", " ").ToUpper(); // INDUSTRIA

            return "GENERAL";
        }

        public ActionResult Clientes()
        {
            var rubros = new Dictionary<string, List<string>>();
            string rutaBase = Server.MapPath("~/Uploads/Images");

            if (Directory.Exists(rutaBase))
            {
                var archivos = Directory.GetFiles(rutaBase)
                    .Where(f => f.EndsWith(".jpg", StringComparison.OrdinalIgnoreCase) ||
                                f.EndsWith(".png", StringComparison.OrdinalIgnoreCase) ||
                                f.EndsWith(".jpeg", StringComparison.OrdinalIgnoreCase));

                foreach (var archivo in archivos)
                {
                    string nombreArchivo = Path.GetFileName(archivo); // ejemplo: industria_logoDavid.jpg
                    string rubro = ObtenerRubroDesdeNombreArchivo(nombreArchivo); // INDUSTRIA

                    if (!rubros.ContainsKey(rubro))
                        rubros[rubro] = new List<string>();

                    rubros[rubro].Add(nombreArchivo);
                }
            }

            return View(rubros); // Envía el Dictionary<string, List<string>> a la vista
        }

        public ActionResult DiseñoWeb()
        {
            return View();
        }
        public ActionResult AsesoresComerciales()
        {
            return View();
        }
        public ActionResult FormadePago()
        {
            return View();
        }
        public ActionResult CasosdeExito()
        {
            var servicio = new CasosExitoServicio(); // Crear instancia del servicio
            var lista = servicio.ListarCasosExito(); // Obtener la lista
            var listaFiltrada = lista.Where(c => c.mostrarEnWeb == true).ToList();
            return View(listaFiltrada);

        }
        public ActionResult ProyectoDetalle(int id)
        {
            var servicio = new CasosExitoServicio();
            var vistaCaso = servicio.ObtenerCasoPorId(id); // ✅ este trae todo, incluyendo NombrePais

            if (vistaCaso == null)
            {
                return HttpNotFound();
            }

            return View(vistaCaso); // ya es del tipo correcto

        }
        public ActionResult Contacto()
        {
            return View();
        }
        public ActionResult Renders()
        {
            return View();
        }
        
    }
}
