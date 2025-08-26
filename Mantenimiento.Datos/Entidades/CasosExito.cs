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
    [Table("dbo.CasoExito")]
    public class CasosExito
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int idCaso { get; set; }
        public string Titulo { get; set; }
        public string Descripcion { get; set; }
        public int idCliente { get; set; }
        public int estado { get; set; }
        public int idPais { get; set; }
        public string nombreArchivo { get; set; }
        public string extension { get; set; }
        public string cambio { get; set; }
        public string resultados { get; set; }
        public DateTime? fechaRegistro { get; set; }
        public bool mostrarEnWeb { get; set; }



    }
}
