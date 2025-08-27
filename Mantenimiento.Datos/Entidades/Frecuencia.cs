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
    [Table("dbo.Frecuencia")]
    public class Frecuencia
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int idFrecuencia { get; set; }
        public string nombre { get; set; }
        public int numDias { get; set; }
        public int aviso1 { get; set; }
        public int aviso2 { get; set; }
        public int idEstado { get; set; }
    }
}
