using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mantenimiento.Datos.Entidades
{
   public class sp_ListarTareas
    {
        public int idTarea { get; set; }
        public DateTime fechaRegistro { get; set; }
        public int prioridad { get; set; }
        public DateTime fechaInicio { get; set; }
        public DateTime fechaFin { get; set; }
        public string descripcion { get; set; }
        public string nombreCliente { get; set; }
        public string nombreResponsable { get; set; }
        public int porAvance { get; set; }
        public int idEstado { get; set; }
        public decimal tiempo { get; set; }
        public string nombreProyecto { get; set; }
        public string nombreTipoRequerimiento { get; set; }
        public string comentario { get; set; }
        public int idRequerimiento { get; set; }

        [Dapper.Contrib.Extensions.Write(false)]
        public string codigoRequerimiento { get; set; }
    }
}
