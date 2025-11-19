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
    public class PersonaServicio : IPersonaServicio
    {
        public PersonaServicio()
        {

        }
        public bool ValidarNdocumento(string numeroDocumento)
        {
            using (var connection = new SqlConnection(ConnectionConfig.ConnectionString))
            {
                var parameters = new DynamicParameters();
                parameters.Add("@NumeroDocumento", numeroDocumento);

                int resultado = connection.QueryFirstOrDefault<int>("sp_ValidarNdocumento", parameters, commandType: CommandType.StoredProcedure);
                // Si el resultado es 1, existe; si es 0, no existe
                return resultado == 1;
            }
        }
        public List<Persona> ListarPersona()
        {
            using (var connection = new SqlConnection(ConnectionConfig.ConnectionString))
            {
                return connection.GetAll<Persona>().Where(m=>m.idEstado==1).AsList();
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
        public List<sp_ListarCategoriaCliente> ListarCategoriaClientes()
        {
            try
            {
                using (var connection = new SqlConnection(ConnectionConfig.ConnectionString))
                {
                    var parameters = new DynamicParameters();
                    var result = connection.Query<sp_ListarCategoriaCliente>("sp_ListarCategoriaCliente", parameters, commandType: CommandType.StoredProcedure);

                    return result.AsList();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al listar Categoria  Clientes.", ex);
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
        public string GetSubFolderByExtension(string extension)
        {
            switch (extension.ToLower())
            {
                case ".jpg":
                case ".jpeg":
                case ".png":
                    return "Images";
                case ".pdf":
                    return "PDFs";
                case ".doc":
                case ".docx":
                    return "WordDocuments";
                case ".xls":
                case ".xlsx":
                    return "ExcelSheets";
                default:
                    return "Others";
            }
        }
        public bool Insertar(Persona oPersona)
        {
            bool result = false;

            using (SqlConnection connection = new SqlConnection(ConnectionConfig.ConnectionString))
            {
                connection.Open();

                try
                {
                    connection.Insert(oPersona);
                    result = true;
                }
                catch (Exception)
                {

                    throw;
                }

                return result;
            }
        }
        public bool Actualizar(Persona oPersona)
        {
            try
            {
                using (var connection = new SqlConnection(ConnectionConfig.ConnectionString))
                {
                    connection.Open();
                    connection.Update(oPersona);
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
                Persona oPersona = connection.Get<Persona>(id);
                oPersona.idEstado = 2;
                return connection.Update(oPersona);
            }
        }
        public List<Persona> ListarPersonaResponsable()
        {
            using (var connection = new SqlConnection(ConnectionConfig.ConnectionString))
            {
                var query = "SELECT * FROM persona WHERE  idTipoPersona = 2 and idEstado != 2";
                return connection.Query<Persona>(query).ToList();
            }
        }
        public Persona obtenerPersona(int id)
        {
            using (var connection = new SqlConnection(ConnectionConfig.ConnectionString))
            {
                return connection.Get<Persona>(id);
            }
        }
    }
}
