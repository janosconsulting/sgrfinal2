using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mantenimiento.Negocio.Poco
{
    public class WizardGenerarReqDesdeDoRequest
    {
        // Contexto mínimo para crear el REQ en tu tabla actual
        public int idPersona { get; set; }            // cliente
        public int idProyecto { get; set; }           // proyecto
        public int idTipoRequerimiento { get; set; }  // tipo (ej. Desarrollo / Mejora)
        public int prioridad { get; set; }            // 1..n según tu tabla
        public string solicitante { get; set; }       // opcional
        public DateTime? fechaInicio { get; set; }
        public DateTime? fechaFin { get; set; }
        public int? idAdicional { get; set; }

        // Payload wizard
        public List<WizardReqItemDto> requerimientos { get; set; }
    }

    public class WizardReqItemDto
    {
        public string codigo { get; set; }                 // REQ-01
        public string titulo { get; set; }                 // Gestión de Bienes
        public string descripcion { get; set; }            // Macro

        public List<string> detalleItems { get; set; }
        public List<string> criteriosAceptacion { get; set; }
        public List<string> checklistQA { get; set; }
        public List<string> subtareasSugeridas { get; set; }
    }

    public class WizardGenerarReqDesdeDoResponse
    {
        public int cantidad { get; set; }
        public List<int> idsRequerimiento { get; set; }
    }

}
