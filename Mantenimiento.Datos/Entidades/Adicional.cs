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
    [Table("dbo.Adicional")]
    public class Adicional
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int idAdicional { get; set; }
        public string nombre { get; set; }
        public int idCliente { get; set; }
        public DateTime fechaRecepcion { get; set; }
    
        public int idEstado { get; set; }
        public string notas { get; set; }
        public decimal precio { get; set; }

        public string codigo { get; set; }
        public int tipoDoc { get; set; }
        public int estado { get; set; }
        public int idProyecto { get; set; }
        public string descripcion { get; set; }
    }
}
