using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mantenimiento.Datos.Entidades
{
    public class sp_ListarRequerimientosxFiltro
    {
        public int idRequerimiento { get; set; }
        public string codigo { get; set; }
        public string nombreCli { get; set; }
        public string nombreProyec { get; set; }
        public DateTime fechaInicio { get; set; }
        public DateTime fechaFin { get; set; }
        public string solicitante { get; set; }
        public string nombreReq { get; set; }
        public string resumen { get; set; }
        public int prioridad { get; set; }
        public int estadoReq { get; set; }
        public int totalItems { get; set; }
        public int aprobados { get; set; }
    }
}