using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mantenimiento.Datos.Entidades
{
    public class sp_ListarSubscripcion
    {
        public int idSubscripcion {  get; set; }
        public int idEstado { get; set; }
        public string cliente { get; set; }
        public string servicio { get; set; }
        public string frecuencia { get; set; }
        public string moneda { get; set; }
        public int importe { get; set; }
        public DateTime fechaInicio { get; set; }
    }
}
