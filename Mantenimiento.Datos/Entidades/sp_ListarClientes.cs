using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mantenimiento.Datos.Entidades
{
    public class sp_ListarClientes
    {
        public int idPersona { get; set; }
        public string nombreIdentidad { get; set; }
        public string nroIdentidad { get; set; }
        public string nombreCli { get; set; }
        public string nombreCategoria { get; set; }
        public string celular { get; set; }
        public string telefonos { get; set; }
        public string direccion { get; set; }
        
    }
}
