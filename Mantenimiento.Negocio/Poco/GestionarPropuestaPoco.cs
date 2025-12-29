using Mantenimiento.Datos.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mantenimiento.Negocio.Poco
{
    public class GestionarPropuestaPoco
    {
        public PropuestaEntidad Propuesta { get; set; }

        public List<sp_ListarClientes> ListaClientes { get; set; }
        public List<sp_ListarProyectos> ListaEstados { get; set; }
        public List<Moneda> ListaMonedas { get; set; }
        public bool IsEditing { get; set; }
    }

    public class sp_ListarPropuesta
    {
        public int idPropuesta { get; set; }
        public string codigo { get; set; }
        public string titulo { get; set; }
        public string descripcion { get; set; }

        public decimal? montoReferencia { get; set; }
        public string observacionPrecio { get; set; }

        public int? idEstado { get; set; }
        public string estado { get; set; }

        public int? idCliente { get; set; }
        public string cliente { get; set; }

        public int? idMoneda { get; set; }
        public string moneda { get; set; }

        public DateTime? fechaRegistro { get; set; }
        public DateTime? fechaModificacion { get; set; }
        public DateTime? fechaAprobacion { get; set; }
    }

    public class PropuestaPdfPoco
    {
        public int idPropuesta { get; set; }
        public string codigo { get; set; }
        public string titulo { get; set; }

        public string cliente { get; set; }
        public string estado { get; set; }

        public string moneda { get; set; }
        public decimal? montoReferencia { get; set; }
        public int? incluyeIgv { get; set; }
        public string observacionPrecio { get; set; }

        public string responsable { get; set; }

        public string resumenEjecutivo { get; set; }
        public string caracteristicas { get; set; }
        public string incluye { get; set; }
        public string noIncluye { get; set; }

        public int? plazoDias { get; set; }
        public string plazoDetalle { get; set; }

        public string formaPago { get; set; }
        public string formaPagoDetalle { get; set; }
        public int? validezDias { get; set; }
        public string condiciones { get; set; }

        public string textoAceptacion { get; set; }

        public DateTime? fechaRegistro { get; set; }

        public string detalleHitos { get; set; }
        public int? esPorHitos { get; set; } // 0/1
        public string criterioAceptacionHitos { get; set; }
    }

}
