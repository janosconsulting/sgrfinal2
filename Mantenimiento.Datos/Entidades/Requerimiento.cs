using Dapper.Contrib.Extensions;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TableAttribute = Dapper.Contrib.Extensions.TableAttribute;

namespace Mantenimiento.Datos.Entidades
{
    [Table("dbo.Requerimiento")]
    public class Requerimiento
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int idRequerimiento { get; set; }
        public int idPersona { get; set; }
        public int? idAdicional { get; set; }
        public int? idProyecto { get; set; }
        public int idTipoRequerimiento { get; set; }
        public string codigo { get; set; }
        public string solicitante { get; set; }
        public string resumen { get; set; }
        public int prioridad { get; set; }
        public DateTime fechaInicio { get; set; }
        public DateTime fechaFin { get; set; }
        public int avanceDesarrollo { get; set; }
        public int aprobacion { get; set; }
        public DateTime fechaRegistro { get; set; }
        public decimal tiempo { get; set; }
        public int estadoReq { get; set; }
        public int idEstado { get; set; }
    }
}