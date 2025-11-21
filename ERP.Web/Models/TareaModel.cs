using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Mantenimiento.Datos.Entidades;

namespace ReyDavid.Web.Models
{
    public class TareaModel
    {
        public Tarea tarea { get; set; }
        public DetalleRequerimiento requerimientoDetalle { get; set; }
        public string Descripcion { get; set; }
        public int EstadoDesarrollo { get; set; }
        public int AsignadoA { get; set; }
        public DateTime? FechaInicio { get; set; }
        public DateTime? FechaFin { get; set; }
    }
}