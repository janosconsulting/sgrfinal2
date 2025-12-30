using Dapper;
using Dapper.Contrib.Extensions;
using Mantenimiento.Datos.Base;
using Mantenimiento.Datos.Entidades;
using Mantenimiento.Negocio.Contratos.Servicios;
using Mantenimiento.Negocio.Poco;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace Mantenimiento.Negocio.Servicios
{
    public class PlanSemanalServicio : IPlanSemanalServicio
    {
        public PlanSemanalServicio()
        {
        }

        public int ObtenerOCrearPlanSemana(DateTime lunes, string usuario)
        {
            try
            {
                using (var connection = new SqlConnection(ConnectionConfig.ConnectionString))
                {
                    var p = new DynamicParameters();
                    p.Add("@lunes", lunes.Date, DbType.Date);
                    p.Add("@usuario", usuario, DbType.String);

                    // SP devuelve IdPlanSemana como INT
                    var id = connection.QueryFirstOrDefault<int>(
                        "sp_PlanSemanal_ObtenerOCrearPlanSemana",
                        p,
                        commandType: CommandType.StoredProcedure
                    );

                    return id;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener/crear plan semanal.", ex);
            }
        }

        public List<sp_PlanSemanal_TreeRequerimiento> TreeRequerimientos(string q)
        {
            try
            {
                using (var connection = new SqlConnection(ConnectionConfig.ConnectionString))
                {
                    var p = new DynamicParameters();
                    p.Add("@q", q, DbType.String);

                    var result = connection.Query<sp_PlanSemanal_TreeRequerimiento>(
                        "sp_PlanSemanal_TreeRequerimientos",
                        p,
                        commandType: CommandType.StoredProcedure
                    );

                    return result.AsList();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al listar requerimientos (tree).", ex);
            }
        }

        public List<sp_PlanSemanal_TreeSubReq> TreeSubRequerimientos(string q)
        {
            try
            {
                using (var connection = new SqlConnection(ConnectionConfig.ConnectionString))
                {
                    var p = new DynamicParameters();
                    p.Add("@q", q, DbType.String);

                    var result = connection.Query<sp_PlanSemanal_TreeSubReq>(
                        "sp_PlanSemanal_TreeSubRequerimientos",
                        p,
                        commandType: CommandType.StoredProcedure
                    );

                    return result.AsList();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al listar subrequerimientos (tree).", ex);
            }
        }

        public List<sp_PlanSemanal_ListarTarjetas> ListarTarjetas(int idPlanSemana, int? idPersonaResponsable)
        {
            try
            {
                using (var connection = new SqlConnection(ConnectionConfig.ConnectionString))
                {
                    var p = new DynamicParameters();
                    p.Add("@idPlanSemana", idPlanSemana, DbType.Int32);

                    var result = connection.Query<sp_PlanSemanal_ListarTarjetas>(
                        "sp_PlanSemanal_ListarTarjetas",
                        p,
                        commandType: CommandType.StoredProcedure
                    );

                    return result.AsList();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al listar tarjetas del plan semanal.", ex);
            }
        }

        public PlanSemanaDetalle ObtenerTarjeta(int idPlanDetalle)
        {
            try
            {
                using (var connection = new SqlConnection(ConnectionConfig.ConnectionString))
                {
                    var p = new DynamicParameters();
                    p.Add("@idPlanDetalle", idPlanDetalle, DbType.Int32);

                    // Si tienes entidad PlanSemanaDetalle mapeada con Contrib:
                    // return connection.Get<PlanSemanaDetalle>(idPlanDetalle);

                    var result = connection.QueryFirstOrDefault<PlanSemanaDetalle>(
                        "sp_PlanSemanal_ObtenerTarjeta",
                        p,
                        commandType: CommandType.StoredProcedure
                    );

                    return result;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener tarjeta.", ex);
            }
        }

        public bool GuardarTarjeta(PlanSemanaDetalle tarjeta, string usuario)
        {
            try
            {
                using (var connection = new SqlConnection(ConnectionConfig.ConnectionString))
                {
                    connection.Open();

                    // Normaliza
                    if (string.IsNullOrWhiteSpace(tarjeta.estado)) tarjeta.estado = "pendiente";
                    if (tarjeta.prioridad <= 0) tarjeta.prioridad = 2;

                    // Si tienes [Table("PlanSemanaDetalle")] puedes usar Contrib
                    if (tarjeta.idPlanDetalle == 0)
                    {
                        tarjeta.fechaRegistro = DateTime.Now;
                        connection.Insert(tarjeta);
                        return true;
                    }
                    else
                    {
                        PlanSemanaDetalle oPn = connection.Get<PlanSemanaDetalle>(tarjeta.idPlanDetalle);
                        tarjeta.fechaRegistro = oPn.fechaRegistro;
                        tarjeta.fechaActualizacion = DateTime.Now;
                        connection.Update(tarjeta);
                        return true;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al guardar tarjeta.", ex);
            }
        }

        public bool MoverTarjeta(int idPlanDetalle, byte dia, string usuario)
        {
            try
            {
                using (var connection = new SqlConnection(ConnectionConfig.ConnectionString))
                {
                    var sql = @"
UPDATE dbo.PlanSemanaDetalle
SET Dia = @dia,
    FechaActualizacion = SYSUTCDATETIME()
WHERE IdPlanDetalle = @idPlanDetalle;";

                    var rows = connection.Execute(sql, new { idPlanDetalle, dia });
                    return rows > 0;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al mover tarjeta.", ex);
            }
        }

        public bool EliminarTarjeta(int idPlanDetalle)
        {
            try
            {
                using (var connection = new SqlConnection(ConnectionConfig.ConnectionString))
                {
                    // Si tienes entidad con Contrib:
                    // var t = connection.Get<PlanSemanaDetalle>(idPlanDetalle);
                    // return connection.Delete(t);

                    var sql = "DELETE dbo.PlanSemanaDetalle WHERE IdPlanDetalle = @idPlanDetalle;";
                    var rows = connection.Execute(sql, new { idPlanDetalle });
                    return rows > 0;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al eliminar tarjeta.", ex);
            }
        }

        public List<sp_Obs_ListarPorSubReq> ListarObservaciones(int idRequerimientoDetalle)
        {
            using (var cn = new SqlConnection(ConnectionConfig.ConnectionString))
            {
                return cn.Query<sp_Obs_ListarPorSubReq>(
                    "sp_Obs_ListarPorSubReq",
                    new { idRequerimientoDetalle },
                    commandType: CommandType.StoredProcedure
                ).AsList();
            }
        }

        public bool InsertarObservacion(RequerimientoDetalleObservacion oDT)
        {
            using (var cn = new SqlConnection(ConnectionConfig.ConnectionString))
            {
                var id = cn.Insert(oDT);
                return id > 0;
            }
        }
        public bool Actualizar(RequerimientoDetalleObservacion oDT)
        {
            bool resultado = false;
            using (var cn = new SqlConnection(ConnectionConfig.ConnectionString))
            {
                
                cn.Update(oDT);
                resultado = true;
                return resultado;
            }
        }

        public bool CerrarObservacion(int idObservacion, string usuario)
        {
            using (var cn = new SqlConnection(ConnectionConfig.ConnectionString))
            {
                var resul = false;
                RequerimientoDetalleObservacion rn = cn.Get<RequerimientoDetalleObservacion>(idObservacion);
                if(rn != null)
                {
                    rn.estado = "cerrada";
                    rn.cerradoPor = usuario;
                    rn.fechaCierre = DateTime.Now;
                    cn.Update(rn);
                    resul = true;
                }
                return resul;
            }
        }
        public RequerimientoDetalleObservacion ObtenerObservacion(int idObservacion)
        {
            using (var connection = new SqlConnection(ConnectionConfig.ConnectionString))
            {
                connection.Open();
                return connection.Get<RequerimientoDetalleObservacion>(idObservacion);
            }
        }

        public List<sp_Adicional_Listar> ListarAdicionales()
        {
            using (var cn = new SqlConnection(ConnectionConfig.ConnectionString))
            {
                var res = cn.Query<sp_Adicional_Listar>(
                    "sp_Adicional_Listar",
                    commandType: CommandType.StoredProcedure
                );
                return res.AsList();
            }
        }

        public List<sp_Requerimiento_ListarPorAdicional> ListarRequerimientosPorAdicional(int idAdicional)
        {
            using (var cn = new SqlConnection(ConnectionConfig.ConnectionString))
            {
                var p = new DynamicParameters();
                p.Add("@IdAdicional", idAdicional, DbType.Int32, ParameterDirection.Input);

                var res = cn.Query<sp_Requerimiento_ListarPorAdicional>(
                    "sp_Requerimiento_ListarPorAdicional", p,
                    commandType: CommandType.StoredProcedure
                );
                return res.AsList();
            }
        }

        public List<sp_SubRequerimiento_ListarPorRequerimiento> ListarSubRequerimientosPorRequerimiento(int idRequerimiento)
        {
            using (var cn = new SqlConnection(ConnectionConfig.ConnectionString))
            {
                var p = new DynamicParameters();
                p.Add("@IdRequerimiento", idRequerimiento, DbType.Int32, ParameterDirection.Input);

                var res = cn.Query<sp_SubRequerimiento_ListarPorRequerimiento>(
                    "sp_SubRequerimiento_ListarPorRequerimiento", p,
                    commandType: CommandType.StoredProcedure
                );
                return res.AsList();
            }
        }
    }
}
