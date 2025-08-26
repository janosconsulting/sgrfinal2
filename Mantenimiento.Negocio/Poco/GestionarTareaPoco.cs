using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mantenimiento.Datos.Entidades;
using System.Web;

namespace Mantenimiento.Negocio.Poco
{
   public class GestionarTareaPoco
    {
        public List<sp_ListarTareas> listarTareas { get; set; }
        public List<sp_ListarClientes> ListarClientes { get; set; }
        public List<sp_ListarTrabajadores> ListarTrabajadores { get; set; }
        public List<sp_ListarTrabajadores> listarTrabajadores { get; set; }
        public List<TipoRequerimiento> ListarTipoRequerimientos { get; set; }
        public List<sp_ListarProyectos> ListarProyectos { get; set; }
        public Persona Persona { get; set; }      
        public Tarea tarea { get; set; }
        public bool TrEditing { get; set; }
    }
}
