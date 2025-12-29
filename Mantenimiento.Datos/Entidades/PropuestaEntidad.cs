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
    [Table("dbo.Propuesta")]
    public class PropuestaEntidad
    {
        [Key]
        public int idPropuesta { get; set; }

        public string codigo { get; set; }
        public int? idCliente { get; set; }
        public string titulo { get; set; }
        public string descripcion { get; set; }

        public int? idEstado { get; set; }
        public int? idMoneda { get; set; }

        public decimal? montoReferencia { get; set; }
        public string observacionPrecio { get; set; }

        public DateTime? fechaRegistro { get; set; }
        public string usuarioRegistro { get; set; }

        public DateTime? fechaModificacion { get; set; }
        public string usuarioModificacion { get; set; }

        public DateTime? fechaAprobacion { get; set; }

        public string responsable { get; set; }
        public string resumenEjecutivo { get; set; }
        public string incluye { get; set; }
        public string noIncluye { get; set; }

        public int? plazoDias { get; set; }
        public string plazoDetalle { get; set; }

        public int? incluyeIgv { get; set; }
        public string formaPago { get; set; }
        public string formaPagoDetalle { get; set; }
        public int? validezDias { get; set; }
        public string condiciones { get; set; }

        public string textoAceptacion { get; set; }
        public string caracteristicas { get; set; }

        public string detalleHitos { get; set; }
        public int? esPorHitos { get; set; } // 0/1
        public string criterioAceptacionHitos { get; set; }
    }
}
