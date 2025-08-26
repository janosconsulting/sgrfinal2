using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mantenimiento.Datos.Entidades
{
    public class sp_ListarProyectos
    {
        public int idProyecto { get; set; }
        public string nombre { get; set; }        
        public string nombreCli { get; set; }
        public int estadoProyecto { get; set; }
        public DateTime fechaInicio { get; set; }
        public DateTime fechaFin { get; set; }
    }
}
