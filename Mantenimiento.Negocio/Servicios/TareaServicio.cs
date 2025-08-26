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
    public class TareaServicio : ITareaServicio
    {
        public TareaServicio()
        {

        }
        public List<sp_ListarTareas> ListarTareas()
        {
            try
            {
                using (var connection = new SqlConnection(ConnectionConfig.ConnectionString))
                {
                    var parameters = new DynamicParameters();
                    var result = connection.Query<sp_ListarTareas>("sp_ListarTareas", parameters, commandType: CommandType.StoredProcedure
                    );
                    return result.AsList();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al listar las Tareas.", ex);
            }
        }


        public bool Insertar(Tarea oTarea)
        {
            bool result = false;

            using (SqlConnection connection = new SqlConnection(ConnectionConfig.ConnectionString))
            {
                connection.Open();

                try
                {
                    connection.Insert(oTarea);
                    result = true;
                }
                catch (Exception)
                {

                    throw;
                }

                return result;
            }
        }
        public bool Actualizar(Tarea oTarea)
        {
            try
            {
                using (var connection = new SqlConnection(ConnectionConfig.ConnectionString))
                {
                    connection.Open();
                    connection.Update(oTarea);
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
                Tarea oTarea = connection.Get<Tarea>(id);
                oTarea.idEstado = 2;
                return connection.Update(oTarea);
            }
        }
        public Tarea obtenerTarea(int id)
        {
            using (var connection = new SqlConnection(ConnectionConfig.ConnectionString))
            {
                return connection.Get<Tarea>(id);
            }
        }
        public Tarea ObtenerPorRequerimiento(int idRequerimiento)
        {
            using (var connection = new SqlConnection(ConnectionConfig.ConnectionString))
            {
                string sql = "SELECT TOP 1 * FROM Tarea WHERE idRequerimiento = @idRequerimiento";
                return connection.QueryFirstOrDefault<Tarea>(sql, new { idRequerimiento });
            }
        }
        public Tarea ObtenerPorDetalleRequerimiento(int idDetalle)
        {
            using (var connection = new SqlConnection(ConnectionConfig.ConnectionString))
            {
                return connection.QueryFirstOrDefault<Tarea>(
                    "SELECT * FROM Tarea WHERE idDetalleRequerimiento = @idDetalle",
                    new { idDetalle });
            }
        }
        public void GuardarCambios()
        {
            // Este método no se usa con Dapper.Contrib ya que Update guarda directamente.
            // Pero se implementa para cumplir la interfaz.
        }

    }
}
