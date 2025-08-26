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
   
    public class CategoriaClienteServicio : ICategoriaClienteServicio
    {
        public CategoriaClienteServicio()
        {
        }

        public List<sp_ListarCategoriaCliente> ListarCategoriaClientes()
        {
            try
            {
                using (var connection = new SqlConnection(ConnectionConfig.ConnectionString))
                {
                    var result = connection.Query<sp_ListarCategoriaCliente>(
                        "sp_ListarCategoriaCliente",
                        commandType: CommandType.StoredProcedure
                    );

                    return result.AsList();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al listar las categorías de cliente.", ex);
            }
        }
        public bool Insertar(CategoriaCliente oCategoriaCliente)
        {
            bool result = false;

            using (SqlConnection connection = new SqlConnection(ConnectionConfig.ConnectionString))
            {
                connection.Open();

                try
                {
                    connection.Insert(oCategoriaCliente);
                    result = true;
                }
                catch (Exception)
                {

                    throw;
                }

                return result;
            }
        }
        public bool Actualizar(CategoriaCliente oCategoriaCliente)
        {
            try
            {
                using (var connection = new SqlConnection(ConnectionConfig.ConnectionString))
                {
                    connection.Open();
                    connection.Update(oCategoriaCliente);
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
                CategoriaCliente oCategoriaCliente = connection.Get<CategoriaCliente> (id);
                oCategoriaCliente.estado = 2;
                return connection.Update(oCategoriaCliente);
            }
        }
        public CategoriaCliente  obtenerCategoriaCliente(int id)
        {
            using (var connection = new SqlConnection(ConnectionConfig.ConnectionString))
            {
                return connection.Get<CategoriaCliente>(id);
            }
        }
    }
}

