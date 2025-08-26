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
    [Table("MAN.Color")]
    public class Color
    { 
       [Key]
       [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
       public int idColor { get; set; }
       public string nombre { get; set; }
       public string exadecimal { get; set; }

    }
}
