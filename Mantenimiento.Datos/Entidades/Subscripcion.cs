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
    [Table("dbo.Subscripcion")]
    public class Subscripcion
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int idSubscripcion { get; set; }
        public DateTime? fechaRegistro { get; set; }
        public int? idCliente { get; set; }
        public int idFrecuencia { get; set; }
        public int idServicio { get; set; }
        public DateTime? fechaInicio { get; set; }
        public DateTime? fechaVcto { get; set; }
        public int importe { get; set; }
        public DateTime? fechaAviso1 { get; set; }
        public DateTime? fechaAviso2 { get; set; }
        public int idMoneda { get; set; }
        public string observacion { get; set; }
        public int idEstado { get; set; }
        public DateTime? fechaCobro { get; set; }
        public string referencia { get; set; }
        public string idCondicionPago { get; set; }
        [Write(false)]
        public int estadoFormulario { get; set; }

    }
}
