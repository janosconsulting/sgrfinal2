using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mantenimiento.Datos.Entidades
{
    public class sp_ListarSeguimientoTareas
    {
        public string proyecto {  get; set; }
        public string requerimiento {  get; set; }
        public string cliente {  get; set; }
        public string actividad {  get; set; }
        public string fechaInicio {  get; set; }
        public string fechaFin {  get; set; }
        public string estado {  get; set; }
        public string responsable {  get; set; }
        public int estadoDesarrollo { get; set; }
        public int idDetalleRequerimiento { get; set; }
        public string tipoRequerimiento { get; set; }
        public int? idResponsable { get; set; }
        public int idProyecto { get; set; }
        public int idRequerimiento { get; set; }
    }
}
