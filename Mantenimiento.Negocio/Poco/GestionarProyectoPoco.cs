using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Handlers;
using Mantenimiento.Datos.Entidades;
using System.Web;


namespace Mantenimiento.Negocio.Poco
{
    public class GestionarProyectoPoco
    {
        public List<sp_ListarProyectos> listarProyectos { get; set; }
        public Proyecto proyecto { get; set; }
        public bool PsEditing { get; set; }
    }
}
