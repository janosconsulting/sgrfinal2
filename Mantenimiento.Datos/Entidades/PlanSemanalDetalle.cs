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
    [Table("PlanSemanaDetalle")]
    public class PlanSemanaDetalle
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int idPlanDetalle { get; set; }

        public int idPlanSemana { get; set; }

        public byte dia { get; set; }                 // tinyint -> byte

        public string titulo { get; set; }            // nvarchar(200)

        public string responsable { get; set; }       // nvarchar(120) null

        public string estado { get; set; }            // varchar(15)

        public byte prioridad { get; set; }           // tinyint -> byte

        public decimal? horas { get; set; }           // decimal(10,2) null

        public int idRequerimientoDetalle { get; set; }

        public int? idObservacion { get; set; }       // nullable

        public DateTime fechaRegistro { get; set; }   // datetime

        public DateTime? fechaActualizacion { get; set; }

        public int? idPersonaResponsable { get; set; }
    }
}
