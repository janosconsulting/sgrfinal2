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
    public class PaisServicio : IPaisServicio
    {
        public List<sp_ListarPaises> ListarPaises()
        {
            try
            {
                using (var connection = new SqlConnection(ConnectionConfig.ConnectionString))
                {
                    var result = connection.Query<sp_ListarPaises>(
                        "sp_ListarPaises",
                        commandType: CommandType.StoredProcedure);

                    return result.AsList();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al listar los países.", ex);
            }
        }
    }
}

