using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mantenimiento.Datos.Entidades
{
    public class sp_ListarSubscripcionReporte
    {
        public int mes {  get; set; }
        public string nombreMes {  get; set; }  
        public decimal totalImportePagado { get; set; }
    }
}
