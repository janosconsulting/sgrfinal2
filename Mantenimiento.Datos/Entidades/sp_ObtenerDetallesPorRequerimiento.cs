using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Mantenimiento.Datos.Entidades
{
    public class sp_ObtenerDetallesPorRequerimiento
    {
        public int idDetalleRequerimiento { get; set; }
        public int idRequerimiento { get; set; }
        public int idPersona { get; set; }
        public string nombrePersona { get; set; }
        public string descripcion { get; set; }
        public string comentarioCliente { get; set; }
        public int estadoDesarrollo { get; set; }
        public int estadoCliente { get; set; }
        public DateTime fechaRegistro { get; set; }
        public int idEstado { get; set; }
        public DateTime? fechaInicio { get; set; }
        public DateTime? fechaFin { get; set; }
        public string nombreArchivo { get; set; }
        public string extension { get; set; }
        public string comentario { get; set; }
        public HttpPostedFileBase archivo { get; set; }

    }
}
