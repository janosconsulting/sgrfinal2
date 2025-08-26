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
    [Table("dbo.Persona")]
    public class Persona
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int idPersona { get; set; }
        public int idDocumentoIdentidad { get; set; }
        public int idTipoPersona { get; set; }
        public string nombres { get; set; }
        public string apellidoPaterno { get; set; }
        public string nroIdentidad { get; set; }
        public string apellidoMaterno { get; set; }
        public string correoelectronico { get; set; }
        public string direccion { get; set; }
        public string paginaweb { get; set; }
        public string telefonos { get; set; }
        public string celular { get; set; }
        public string nombre2 { get; set; }
        public string detalleContacto { get; set; }
        public string sexo { get; set; }
        public int? esActivo { get; set; }
        public DateTime? fechaRegistro { get; set; }
        public int? esRegistroWeb { get; set; }
        public DateTime? fechaNacimiento { get; set; }
        public string ocupacion { get; set; }
        public string estadoCivil { get; set; }
        public string codigo { get; set; }
        public string ciudad { get; set; }
        public int idEstado { get; set; }
       public int idCategoria { get; set; }
        public string nombreArchivo { get; set; }
        public string extension { get; set; }
        [Dapper.Contrib.Extensions.Computed]
        public string rubro { get; set; }

    }
}
