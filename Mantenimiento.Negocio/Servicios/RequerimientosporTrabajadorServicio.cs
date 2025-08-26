using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using Dapper.Contrib.Extensions;
using Mantenimiento.Datos.Base;
using Mantenimiento.Datos.Contratos;
using Mantenimiento.Datos.Entidades;
using Mantenimiento.Negocio.Contratos.Servicios;
using Mantenimiento.Negocio.Poco;
using System.Data;
using System.Data.SqlClient;
using System.Transactions;
using Utilitario.Especificacion;




namespace Mantenimiento.Negocio.Servicios
{
    public class RequerimientosporTrabajadorServicio : IRequerimientosporTrabajadorServicio
    {
        public RequerimientosporTrabajadorServicio()
        {
        }

        // Método con parámetro opcional para filtrar por trabajador
        public List<sp_listarRequerimientosporTrabajador> listarRequerimientosporTrabajador(int? idTrabajador = null)
        {
            try
            {
                using (var connection = new SqlConnection(ConnectionConfig.ConnectionString))
                {
                    var parameters = new DynamicParameters();
                    parameters.Add("@idTrabajador", idTrabajador, DbType.Int32);

                    var result = connection.Query<sp_listarRequerimientosporTrabajador>(
                        "listarRequerimientoporTrabajador",
                        parameters,
                        commandType: CommandType.StoredProcedure
                    );

                    return result.AsList();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al listar los requerimientos por trabajador.", ex);
            }
        }
        public bool Actualizar(GestionarRequerimientoPoco objeto)
        {
            bool result = false;

            using (SqlConnection connection = new SqlConnection(ConnectionConfig.ConnectionString))
            {
                connection.Open();

                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        var detalle = objeto.DetalleRequerimiento; // asumiendo que ahora tienes una propiedad única

                        if (detalle == null)
                            throw new Exception("No se proporcionó detalle para actualizar.");

                        // Obtener el detalle actual desde la base de datos
                        var original = connection.Get<DetalleRequerimiento>(detalle.idDetalleRequerimiento, transaction);
                        if (original == null)
                            throw new Exception("No se encontró el detalle a actualizar.");

                        // Actualizar campos
                        original.idPersona = detalle.idPersona;
                        original.descripcion = detalle.descripcion;
                        original.comentarioCliente = detalle.comentarioCliente;
                        original.estadoDesarrollo = detalle.estadoDesarrollo;
                        original.estadoCliente = detalle.estadoCliente;
                        original.fechaInicio = detalle.fechaInicio;
                        original.fechaFin = detalle.fechaFin;
                        original.nombreArchivo = detalle.nombreArchivo;
                        original.extension = detalle.extension;
                        original.modulo = detalle.modulo;
                        original.criterioAceptacion = detalle.criterioAceptacion;
                        original.comentario = detalle.comentario;

                        connection.Update(original, transaction);

                        transaction.Commit();
                        result = true;
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        throw new Exception("Error al actualizar el detalle del requerimiento.", ex);
                    }
                }
            }

            return result;
        }
        public DetalleRequerimiento ObtenerDetallePorId(int idDetalle)
        {
            using (SqlConnection connection = new SqlConnection(ConnectionConfig.ConnectionString))
            {
                connection.Open();

                var query = @"
            SELECT 
                idDetalleRequerimiento,
                idRequerimiento,
                idPersona,
                ISNULL(descripcion, '') AS descripcion,
                ISNULL(comentarioCliente, '') AS comentarioCliente,
                ISNULL(estadoDesarrollo, 0) AS estadoDesarrollo,
                ISNULL(estadoCliente, 0) AS estadoCliente,
                ISNULL(fechaRegistro, '1900-01-01') AS fechaRegistro,
                ISNULL(idEstado, 0) AS idEstado,
                ISNULL(fechaInicio, '1900-01-01') AS fechaInicio,
                ISNULL(fechaFin, '1900-01-01') AS fechaFin,
                ISNULL(nombreArchivo, '') AS nombreArchivo,
                ISNULL(extension, '') AS extension,
                ISNULL(comentario, '') AS comentario,
                ISNULL(modulo, '') AS modulo,
                ISNULL(criterioAceptacion, '') AS criterioAceptacion
            FROM DetalleRequerimiento 
            WHERE idDetalleRequerimiento = @Id";

                var detalle = connection.QueryFirstOrDefault<DetalleRequerimiento>(query, new { Id = idDetalle });

                return detalle;
            }
        }
        public List<sp_ListarTrabajadores> ListarTrabajadores()
        {
            try
            {
                using (var connection = new SqlConnection(ConnectionConfig.ConnectionString))
                {
                    var parameters = new DynamicParameters();
                    var result = connection.Query<sp_ListarTrabajadores>("sp_ListarTrabajadores", parameters, commandType: CommandType.StoredProcedure);

                    return result.AsList();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al listar los Trabajadores.", ex);
            }
        }      
    }
}
