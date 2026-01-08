using System;
using System.Collections.Generic;
using Mantenimiento.Datos.Entidades;
using Mantenimiento.Negocio.Poco;

namespace Mantenimiento.Negocio.Contratos.Servicios
{
    public interface IPlanSemanalServicio
    {
        int ObtenerOCrearPlanSemana(DateTime lunes, string usuario);

        List<sp_PlanSemanal_TreeRequerimiento> TreeRequerimientos(string q);
        List<sp_PlanSemanal_TreeSubReq> TreeSubRequerimientos(string q);

        List<sp_PlanSemanal_ListarTarjetas> ListarTarjetas(int idPlanSemana,string estado, int? idPersonaResponsable);
        PlanSemanaDetalle ObtenerTarjeta(int idPlanDetalle);

        bool GuardarTarjeta(PlanSemanaDetalle tarjeta, string usuario);
        bool MoverTarjeta(int idPlanDetalle, byte dia, string usuario);
        bool EliminarTarjeta(int idPlanDetalle);

        List<sp_Obs_ListarPorSubReq> ListarObservaciones(int idRequerimientoDetalle);
        bool InsertarObservacion(RequerimientoDetalleObservacion oDT);
        bool CerrarObservacion(int idObservacion, string usuario);
        RequerimientoDetalleObservacion ObtenerObservacion(int idObservacion);
        bool Actualizar(RequerimientoDetalleObservacion oDT);
        List<sp_Adicional_Listar> ListarAdicionales();
        List<sp_Requerimiento_ListarPorAdicional> ListarRequerimientosPorAdicional(int idAdicional);
        List<sp_SubRequerimiento_ListarPorRequerimiento> ListarSubRequerimientosPorRequerimiento(int idRequerimiento);
        ResultadoTransaccion EliminarObservacion(int idObservacion);

    }
}