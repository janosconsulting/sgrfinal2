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
    public class ProyectoServicio : IProyectoServicio
    {
        public ProyectoServicio()
        {

        }
        public List<sp_ListarProyectos> ListarProyectos()
        {
            try
            {
                using (var connection = new SqlConnection(ConnectionConfig.ConnectionString))
                {
                    var parameters = new DynamicParameters();
                    var result = connection.Query<sp_ListarProyectos>("sp_ListarProyectos", parameters, commandType: CommandType.StoredProcedure
                    );
                    return result.AsList();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al listar los Proyectos.", ex);
            }
        }
        public bool Insertar(Proyecto oProyecto)
        {
            bool result = false;

            using (SqlConnection connection = new SqlConnection(ConnectionConfig.ConnectionString))
            {
                connection.Open();

                try
                {
                    connection.Insert(oProyecto);
                    result = true;
                }
                catch (Exception)
                {

                    throw;
                }

                return result;
            }
        }
        public bool Actualizar(Proyecto oProyecto)
        {
            try
            {
                using (var connection = new SqlConnection(ConnectionConfig.ConnectionString))
                {
                    connection.Open();
                    connection.Update(oProyecto);
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
                Proyecto oProyecto = connection.Get<Proyecto>(id);
                oProyecto.idEstado = 2;
                return connection.Update(oProyecto);
            }
        }
        public Proyecto obtenerProyecto(int id)
        {
            using (var connection = new SqlConnection(ConnectionConfig.ConnectionString))
            {
                return connection.Get<Proyecto>(id);
            }
        }
    }
}
