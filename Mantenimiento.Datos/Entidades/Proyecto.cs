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
    [Table("dbo.Proyecto")]
    public class Proyecto
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int idProyecto { get; set; }
        public string nombre { get; set; }
        public int idPersona { get; set; }
        public int estadoProyecto { get; set; }
        public DateTime fechaInicio { get; set; }
        public DateTime fechaFin { get; set; }
        public DateTime apellidoPaterno { get; set; }
        public int idEstado { get; set; }
        public DateTime fechaRegistro { get; set; }
    }
}
