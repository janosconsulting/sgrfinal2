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
    [Table("dbo.Tarea")]
    public class Tarea
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int idTarea { get; set; }
        public DateTime? fechaRegistro { get; set; }
        public int prioridad { get; set; }
        public DateTime? fechaInicio { get; set; }
        public DateTime? fechaFin { get; set; }
        public string descripcion { get; set; }
        public int idCliente { get; set; }
        public int idResponsable { get; set; }
        public int porAvance { get; set; }
        public int idEstado { get; set; }
        public decimal tiempo { get; set; }
        public int idProyecto { get; set; }
        public int idTipoRequerimiento { get; set; }
        public int idRequerimiento { get; set; }
        public string comentario { get; set; }
        public int? idDetalleRequerimiento { get; set; }

        [Write(false)]
        public string codigoRequerimiento { get; set; }

    }
}
