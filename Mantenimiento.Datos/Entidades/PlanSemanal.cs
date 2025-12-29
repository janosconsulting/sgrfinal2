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
    [Table("PlanSemana")]
    public class PlanSemana
    {
        [Key]
        public int idPlanSemana { get; set; }

        public DateTime fechaInicio { get; set; }     // date (lunes)

        public DateTime fechaFin { get; set; }        // date (domingo)

        public string registradoPor { get; set; }    // nvarchar(120) null

        public DateTime fechaRegistro { get; set; }  // datetime
    }
}
