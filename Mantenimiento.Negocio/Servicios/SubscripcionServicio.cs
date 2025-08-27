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
        public List<sp_ListarSubscripcion> ListarSubscripcion()
        {
            using (var connection = new SqlConnection(ConnectionConfig.ConnectionString))
            {
                return connection.Query<sp_ListarSubscripcion>("sp_ListarSubscripcion", commandType: CommandType.StoredProcedure).ToList();
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
                        Subscripcion sub = new Subscripcion();
                        sub.idEstado = obj.idEstado;
                        sub.idMoneda = obj.idMoneda;
                        sub.idFrecuencia = obj.idFrecuencia;
                        sub.idServicio = obj.idServicio;
                        sub.idCliente = obj.idCliente;
                        sub.observacion = obj.observacion;
                        sub.fechaInicio = obj.fechaInicio;
                        sub.fechaVcto = obj.fechaVcto;
                        sub.fechaAviso1 = obj.fechaAviso1;
                        sub.fechaAviso2 = obj.fechaAviso2;
                        sub.fechaRegistro = obj.fechaRegistro;
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
    }
}
