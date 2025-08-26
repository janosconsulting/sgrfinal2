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
    public class TipoRequerimientoServicio : ITipoRequerimientoServicio
    {
        public TipoRequerimientoServicio()
        {

        }
        public List<TipoRequerimiento> ListarTipoReq()
        {
            using (var connection = new SqlConnection(ConnectionConfig.ConnectionString))
            {
                return connection.GetAll<TipoRequerimiento>().Where(m => m.idEstado == 1).AsList();
            }
        }
    }
}
