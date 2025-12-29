using Mantenimiento.Datos.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mantenimiento.Negocio.Poco
{
    public class PlanSemanalModel
    {
        public GestionarPlanSemanalPoco Plan { get; set; } = new GestionarPlanSemanalPoco();

        // Form tarjeta
        public PlanSemanaDetalle Tarjeta { get; set; } = new PlanSemanaDetalle();
    }

    
    public class ObservacionModel
    {
        public RequerimientoDetalleObservacion Observacion { get; set; } = new RequerimientoDetalleObservacion();
    }

    public class RequerimientoDetalleObservacion
    {
        public int idObservacion { get; set; }
        public int idRequerimientoDetalle { get; set; }
        public int numero { get; set; }
        public string severidad { get; set; }
        public string estado { get; set; }
        public string descripcion { get; set; }
    }

    public class GestionarObservacionPoco
    {
        public int idRequerimientoDetalle { get; set; }
        public List<sp_Obs_ListarPorDetalle> observaciones { get; set; } = new List<sp_Obs_ListarPorDetalle>();
    }

    public class sp_Obs_ListarPorDetalle
    {
        public int idObservacion { get; set; }
        public int idRequerimientoDetalle { get; set; }
        public int numero { get; set; }
        public string severidad { get; set; }
        public string estado { get; set; }
        public string descripcion { get; set; }
        public string fechaRegistro { get; set; } // o DateTime si prefieres
        public string registradoPor { get; set; }
    }

    public class sp_Obs_Log
    {
        public int idLog { get; set; }
        public int idObservacion { get; set; }
        public string accion { get; set; }
        public string mensaje { get; set; }
        public string registradoPor { get; set; }
        public string fechaRegistro { get; set; }
    }

    public class GestionarPlanSemanalPoco
    {
        public int idPlanSemana { get; set; }
        public DateTime lunes { get; set; }
        public string lunesTexto { get; set; }

        // Para render inicial (si quieres server-side)
        public List<sp_PlanSemanal_ListarTarjetas> tarjetas { get; set; } = new List<sp_PlanSemanal_ListarTarjetas>();

        // Árbol de req/subreq (si quieres server-side)
        public List<sp_PlanSemanal_TreeRequerimiento> requerimientos { get; set; } = new List<sp_PlanSemanal_TreeRequerimiento>();
        public List<sp_PlanSemanal_TreeSubReq> subrequerimientos { get; set; } = new List<sp_PlanSemanal_TreeSubReq>();
    }

    // DTOs (pueden ser SP results)
    public class sp_PlanSemanal_ListarTarjetas
    {
        public int idPlanDetalle { get; set; }
        public int idPlanSemana { get; set; }
        public byte dia { get; set; }
        public string titulo { get; set; }
        public string responsable { get; set; }
        public string estado { get; set; }
        public byte prioridad { get; set; }
        public decimal? horas { get; set; }
        public int idRequerimientoDetalle { get; set; }
        public int? idPersonaResponsable { get; set; }
        public int? idObservacion { get; set; }
    }

    public class sp_PlanSemanal_TreeRequerimiento
    {
        public int idRequerimiento { get; set; }
        public string codigo { get; set; }
        public string nombre { get; set; }
    }

    public class sp_PlanSemanal_TreeSubReq
    {
        public int idRequerimientoDetalle { get; set; }
        public int idRequerimiento { get; set; }
        public string nombre { get; set; }
        public string estado { get; set; }
        public int? prioridad { get; set; }
    }
}
