using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mantenimiento.Datos.Entidades
{
    public class sp_listarRequerimientosporTrabajador
    {
        public int id { get; set; }
        public string NombreProyecto { get; set; }
        public string CodigoRequerimiento { get; set; }
        public string DescripcionRequerimiento { get; set; }
        public string NombreTrabajador { get; set; }
        public int Estado { get; set; }
        public string ComentarioCliente { get; set; }
        public int? EstadoDesarrollo { get; set; }
        public int? EstadoCliente { get; set; }
        public DateTime? FechaInicio { get; set; }
        public DateTime? FechaFin { get; set; }
        public string NombreArchivo { get; set; }
        public string Extension { get; set; }
        public int? idPersona { get; set; }
        public string Modulo { get; set; }
        public string CriterioAceptacion { get; set; }
        public int idRequerimiento { get; set; }

    }
}
