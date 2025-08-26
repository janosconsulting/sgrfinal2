using System.Configuration;

namespace Utilitario.Reporte
{
    public class ReporteConeccion
    {
        //private string CadenaConeccion;

        public string servidor { get; set; }
        public string basedatos { get; set; }
        public string usuario { get; set; }
        public string clave { get; set; }


        public ReporteConeccion()
        {
            this.servidor = ConfigurationManager.AppSettings.Get("servidor");
            if (this.servidor == null)
                this.servidor = "";
            this.basedatos = ConfigurationManager.AppSettings.Get("basedatos");
            if (this.basedatos == null)
                this.basedatos = "";
            this.usuario = ConfigurationManager.AppSettings.Get("usuario");
            if (this.usuario == null)
                this.usuario = "";
            this.clave = ConfigurationManager.AppSettings.Get("clave");
            if (this.clave == null)
                this.clave = "";
        }
    }
}
