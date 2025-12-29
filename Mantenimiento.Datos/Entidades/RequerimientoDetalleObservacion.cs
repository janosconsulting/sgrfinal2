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
    [Table("RequerimientoDetalleObservacion")]
    public class RequerimientoDetalleObservacion
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int idObservacion { get; set; }

        public int idRequerimientoDetalle { get; set; }

        public string comentario { get; set; }

        public string severidad { get; set; }          // baja | media | alta

        public string estado { get; set; }             // abierta | cerrada

        public string registradoPor { get; set; }

        public DateTime fechaRegistro { get; set; }

        public DateTime? fechaCierre { get; set; }

        public string cerradoPor { get; set; }

        public string nombreArchivo { get; set; }

        public string extension { get; set; }
        public int? ObservadorPor {  get; set; }
    }
}
