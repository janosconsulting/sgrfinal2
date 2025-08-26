using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mantenimiento.Datos.Entidades;

namespace Mantenimiento.Negocio.Poco
{
    public class GestionarCasosExitoPoco
    {
        public CasosExito CasosExito { get; set; }
        public List<sp_ObtenerCasosExito> listarCasosExito { get; set; }
        public List<sp_ListarClientes> listarClientes { get; set; }
        public List<sp_ListarPaises> listarPaises { get; set; }
        public bool IsEditing { get; set; }
    }
}
