using Dapper;
using Dapper.Contrib.Extensions;
using Mantenimiento.Datos.Base;
using Mantenimiento.Datos.Contratos;
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
using System.Transactions;
using Utilitario.Especificacion;
namespace Mantenimiento.Negocio.Servicios
{

    public class CasosExitoServicio : ICasosExitoServicio
    {
        public CasosExitoServicio()
        {
        }

        public List<sp_ObtenerCasosExito> ListarCasosExito()
        {
            try
            {
                using (var connection = new SqlConnection(ConnectionConfig.ConnectionString))
                {
                    var result = connection.Query<sp_ObtenerCasosExito>(
                        "sp_ObtenerCasosExito",
                        commandType: CommandType.StoredProcedure
                    );

                    return result.AsList();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al listar casos de Éxito.", ex);
            }
        }
        public bool Insertar(CasosExito oCasosExito)
        {
            bool result = false;

            using (SqlConnection connection = new SqlConnection(ConnectionConfig.ConnectionString))
            {
                connection.Open();

                try
                {
                    connection.Insert(oCasosExito);
                    result = true;
                }
                catch (Exception)
                {

                    throw;
                }

                return result;
            }
        }
        public bool Actualizar(CasosExito oCasosExito)
        {
            try
            {
                using (var connection = new SqlConnection(ConnectionConfig.ConnectionString))
                {
                    connection.Open();
                    connection.Update(oCasosExito);
                    return true;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        public bool Eliminar(int id)
        {
            using (var connection = new SqlConnection(ConnectionConfig.ConnectionString))
            {
                CasosExito oCasosExito = connection.Get<CasosExito>(id);
                oCasosExito.estado = 2;
                return connection.Update(oCasosExito);
            }
        }
        public CasosExito obtenerCasosExito(int id)
        {
            using (var connection = new SqlConnection(ConnectionConfig.ConnectionString))
            {
                return connection.Get<CasosExito>(id);
            }
        }

        public sp_ObtenerCasosExito ObtenerCasoPorId(int idCaso)
        {
            using (var conexion = new SqlConnection(ConnectionConfig.ConnectionString))
            {
                var sql = @"
 SELECT 
     ce.idCaso,
     ce.Titulo,
     ce.Descripcion,
     ce.estado,
     ce.idPais,
     pa.nombre AS NombrePais,
     ce.cambio,
     ce.resultados,
     ce.nombreArchivo,
     ce.extension,
     ce.fechaRegistro,
     ce.mostrarEnWeb AS mostrar,
     p.nombres + ' ' + ISNULL(p.apellidoPaterno,'') + ' ' + ISNULL(p.apellidoMaterno,'') AS NombreCliente
 FROM CasoExito ce
 INNER JOIN Persona p ON ce.idCliente = p.idPersona
 INNER JOIN Pais pa ON ce.idPais = pa.idPais
 WHERE ce.idCaso = @idCaso";

                return conexion.QueryFirstOrDefault<sp_ObtenerCasosExito>(sql, new { idCaso });
            }
        }

        public List<sp_ListarClientes> ListarClientes()
        {
            try
            {
                using (var connection = new SqlConnection(ConnectionConfig.ConnectionString))
                {
                    var parameters = new DynamicParameters();
                    var result = connection.Query<sp_ListarClientes>("sp_ListarClientes", parameters, commandType: CommandType.StoredProcedure);

                    return result.AsList();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al listar los Clientes.", ex);
            }
        }
        public List<sp_ListarPaises> ListarPaises()
        {
            try
            {
                using (var connection = new SqlConnection(ConnectionConfig.ConnectionString))
                {
                    var parameters = new DynamicParameters();
                    var result = connection.Query<sp_ListarPaises>("sp_ListarPaises", parameters, commandType: CommandType.StoredProcedure);

                    return result.AsList();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al listar los Países.", ex);
            }
        }
    }
}