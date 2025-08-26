using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mantenimiento.Negocio.Poco
{
    public static class ConnectionConfig
    {
        public static string ConnectionString { get; } = ConfigurationManager.AppSettings["conexion"];
        //public static string ConnectionString { get; } = "Data Source=154.53.38.152;Initial Catalog=bdKedro;Persist Security Info=True;User ID=userKedro;Password=8Od0_c3h4;Connection Timeout=60";
    }
}
