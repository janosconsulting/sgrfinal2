using Dapper;
using Dapper.Contrib.Extensions;
using Mantenimiento.Datos.Entidades;
using Mantenimiento.Negocio.Contratos.Servicios;
using Mantenimiento.Negocio.Poco;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI.WebControls;

namespace Mantenimiento.Negocio.Servicios
{
    public class SubscripcionServicio : ISubscripcionServicio
    {
        public List<Frecuencia> ListarFrecuencia()
        {
            using (var connection= new SqlConnection(ConnectionConfig.ConnectionString))
            {
                return connection.GetAll<Frecuencia>().Where(t => t.idEstado != 2).ToList();
            }
        }
        public Subscripcion Obtener(int idSubscripcion)
        {
            using (var connection = new SqlConnection(ConnectionConfig.ConnectionString))
            {
                //ESTADO ELIMINADOS ES 4
                return connection.Query<Subscripcion>("SELECT * FROM dbo.Subscripcion WHERE idSubscripcion = @id and idEstado != 4", new {id = idSubscripcion}).FirstOrDefault();
            }
        }

        public List<Moneda> ListarMoneda()
        {
            using (var connection = new SqlConnection(ConnectionConfig.ConnectionString))
            {
                return connection.GetAll<Moneda>().Where(t => t.idEstado != 2).ToList();
            }
        }
        public List<Servicio> ListarServicio()
        {
            using (var connection = new SqlConnection(ConnectionConfig.ConnectionString))
            {
                return connection.GetAll<Servicio>().Where(t => t.idEstado != 2).ToList();
            }
        }
        public GestionarSubscripcion ListarSubscripcion(int idServicio, int idFrecuencia, int idEstado, int anio, int mes)
        {
            using (var connection = new SqlConnection(ConnectionConfig.ConnectionString))
            {
                var parameters = new DynamicParameters();
                // Parámetros opcionales
                parameters.Add("@idServicio", idServicio, DbType.Int32);
                parameters.Add("@idFrecuencia", idFrecuencia, DbType.Int32);
                parameters.Add("@idEstado", idEstado, DbType.Int32);
                parameters.Add("@anio", anio, DbType.Int32);
                parameters.Add("@mes", mes, DbType.Int32);

                using (var multi = connection.QueryMultiple("sp_ListarSubscripcion", parameters, commandType: CommandType.StoredProcedure))
                {
                    var listaSubscripcion = multi.Read<sp_ListarSubscripcion>().ToList();
                    var resumenDetallSubscripciones = multi.ReadFirstOrDefault<ResumenDetallSubscripcion>();
                    var resumenDetallSubscripcionesActivos = multi.ReadFirstOrDefault<ResumenDetallSubscripcion>();

                    return new GestionarSubscripcion
                    {
                        listaSuscripcion1 = listaSubscripcion,
                        resumenDetallSubscripcionCobrados = resumenDetallSubscripciones,
                        resumenDetallSubscripcionActivos = resumenDetallSubscripcionesActivos
                    };
                }
                //return connection.Query<sp_ListarSubscripcion>("sp_ListarSubscripcion", commandType: CommandType.StoredProcedure).ToList();
            }
        }
        public ResultadoTransaccion Insertar(Subscripcion obj)
        {
            ResultadoTransaccion objRes = new ResultadoTransaccion();

            bool result = false;

            using (SqlConnection connection = new SqlConnection(ConnectionConfig.ConnectionString))
            {
                connection.Open();

                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        Frecuencia f = connection.Get<Frecuencia>(obj.idFrecuencia, transaction);
                        if (f == null)
                        {
                            throw new Exception("Debe seleccionar la Frecuencia");
                        }
                        Subscripcion sub = new Subscripcion();
                        sub.idEstado = obj.idEstado;
                        sub.idMoneda = obj.idMoneda;
                        sub.idFrecuencia = obj.idFrecuencia;
                        sub.idServicio = obj.idServicio;
                        sub.idCliente = obj.idCliente;
                        sub.observacion = obj.observacion;
                        sub.fechaInicio = obj.fechaInicio;
                        sub.fechaVcto = obj.fechaVcto;
                        //ENCONTRAR LA FRECUENCIA PARA REALIZAR LA REDUCCION DE LAS FECHA
                        sub.fechaAviso1 = obj.fechaVcto.Value.AddDays( -f.aviso1);
                        sub.fechaAviso2 = obj.fechaVcto.Value.AddDays(-f.aviso2);
                        sub.fechaRegistro = DateTime.Now;
                        sub.importe = obj.importe;

                        connection.Insert(sub, transaction);
                        objRes.mensaje = "";
                        objRes.codigo = sub.idSubscripcion;

                        transaction.Commit();
                    }
                    catch (Exception)
                    {
                        transaction.Rollback();
                        throw;
                    }

                    return objRes;
                }
            }
        }
        public ResultadoTransaccion Actualizar(Subscripcion obj)
        {
            ResultadoTransaccion objRes = new ResultadoTransaccion();

            bool result = false;

            using (SqlConnection connection = new SqlConnection(ConnectionConfig.ConnectionString))
            {
                connection.Open();

                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        Frecuencia f = connection.Get<Frecuencia>(obj.idFrecuencia, transaction);
                        Subscripcion sub = connection.Get<Subscripcion>(obj.idSubscripcion, transaction);
                        if(sub != null)
                        {
                            sub.idEstado = obj.idEstado;
                            sub.idMoneda = obj.idMoneda;
                            sub.idFrecuencia = obj.idFrecuencia;
                            sub.idServicio = obj.idServicio;
                            sub.idCliente = obj.idCliente;
                            sub.observacion = obj.observacion;
                            sub.fechaInicio = obj.fechaInicio;
                            sub.fechaVcto = obj.fechaVcto;
                            sub.fechaAviso1 = obj.fechaVcto.Value.AddDays(-f.aviso1);
                            sub.fechaAviso2 = obj.fechaVcto.Value.AddDays(-f.aviso2);
                            sub.importe = obj.importe;

                            connection.Update(sub, transaction);

                        }
                        objRes.mensaje = "";
                        objRes.codigo = sub.idSubscripcion;

                        transaction.Commit();
                    }
                    catch (Exception)
                    {
                        transaction.Rollback();
                        throw;
                    }

                    return objRes;
                }
            }
        }
        public bool Eliminar(int id)
        {
            using (var connection = new SqlConnection(ConnectionConfig.ConnectionString))
            {
                Subscripcion sub = connection.Get<Subscripcion>(id);
                sub.idEstado = 4;
                return connection.Update(sub);
            }
        }
        public ResultadoTransaccion CobrarSuscripcion(Subscripcion obj)
        {
            ResultadoTransaccion objRes = new ResultadoTransaccion();

            bool result = false;

            using (SqlConnection connection = new SqlConnection(ConnectionConfig.ConnectionString))
            {
                connection.Open();

                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        Frecuencia f = connection.Get<Frecuencia>(obj.idFrecuencia, transaction);
                        Subscripcion sub = connection.Get<Subscripcion>(obj.idSubscripcion, transaction);
                        if (sub != null)
                        {
                            sub.idEstado = 3;
                            sub.fechaCobro = obj.fechaCobro;
                            sub.idCondicionPago = obj.idCondicionPago;

                            connection.Update(sub, transaction);

                        }
                        objRes.mensaje = "";
                        objRes.codigo = sub.idSubscripcion;

                        transaction.Commit();
                    }
                    catch (Exception)
                    {
                        transaction.Rollback();
                        throw;
                    }

                    return objRes;
                }
            }
        }
        public ResultadoTransaccion RenovarSuscripcion(Subscripcion obj)
        {
            ResultadoTransaccion objRes = new ResultadoTransaccion();

            bool result = false;

            using (SqlConnection connection = new SqlConnection(ConnectionConfig.ConnectionString))
            {
                connection.Open();

                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        Frecuencia f = connection.Get<Frecuencia>(obj.idFrecuencia, transaction);
                        Subscripcion sub = connection.Get<Subscripcion>(obj.idSubscripcion, transaction);
                        if (sub != null)
                        {
                            sub.idEstado = obj.idEstado;
                            sub.idMoneda = obj.idMoneda;
                            sub.idFrecuencia = obj.idFrecuencia;
                            sub.idServicio = obj.idServicio;
                            sub.idCliente = obj.idCliente;
                            sub.observacion = obj.observacion;
                            sub.fechaInicio = obj.fechaInicio;
                            sub.fechaVcto = obj.fechaVcto;
                            sub.fechaAviso1 = obj.fechaVcto.Value.AddDays(-f.aviso1);
                            sub.fechaAviso2 = obj.fechaVcto.Value.AddDays(-f.aviso2);
                            sub.importe = obj.importe;

                            connection.Insert(sub, transaction);

                        }
                        objRes.mensaje = "";
                        objRes.codigo = sub.idSubscripcion;

                        transaction.Commit();
                    }
                    catch (Exception)
                    {
                        transaction.Rollback();
                        throw;
                    }

                    return objRes;
                }
            }
        }
    }
}
