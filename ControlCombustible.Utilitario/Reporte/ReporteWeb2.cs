using System;
using System.Collections.Generic;
using System.Configuration;
using System.Web.SessionState;
using System.Web.UI;

namespace Utilitario.Reporte
{

    public class ReporteWeb2
    {
        private string nombre;
        private string ruta;
        private List<ParametroReporteWeb> listaParametros;
        private int contador;
        private ReporteConeccion coneccion;

        public ReporteConeccion Coneccion
        {
            get { return coneccion; }
            set { coneccion = value; }
        }
        public string Ruta
        {
            get { return ruta; }
            set { ruta = value; }
        }
        public string Nombre
        {
            get { return nombre; }
            set { nombre = value; }
        }

        public ReporteWeb2(string nombre, string ruta, ReporteConeccion coneccion)
        {
            contador = 0;
            listaParametros = new List<ParametroReporteWeb>();
            this.nombre = nombre;
            this.ruta = ruta;
            this.coneccion = coneccion;
        }
        public ReporteWeb2()
        {
            contador = 0;
            listaParametros = new List<ParametroReporteWeb>();
        }
        public ReporteWeb2(string ruta)
        {
            contador = 0;
            listaParametros = new List<ParametroReporteWeb>();
            this.ruta = ruta;
        }
        public ReporteWeb2(string nombre, string ruta)
        {
            contador = 0;
            listaParametros = new List<ParametroReporteWeb>();
            this.nombre = nombre;
            this.ruta = ruta;
            this.coneccion = new ReporteConeccion();
        }
        public void AgregarParametro(ParametroReporteWeb parametro)
        {
            ++contador;
            listaParametros.Add(parametro);
        }

        public void Abrir(Page pagina)
        {
            this.ruta = !string.IsNullOrEmpty(ConfigurationManager.AppSettings.Get("ruta_rpt_ruta"))
                && !string.IsNullOrWhiteSpace(ConfigurationManager.AppSettings.Get("ruta_rpt_ruta")) ? ConfigurationManager.AppSettings.Get("ruta_rpt_ruta") : this.ruta;

            if (string.IsNullOrWhiteSpace(this.ruta) && string.IsNullOrEmpty(this.ruta))
            {
                this.ruta = ConfigurationManager.AppSettings.Get("rutarpt");
                if (string.IsNullOrWhiteSpace(this.ruta) && string.IsNullOrEmpty(this.ruta))
                    throw new ArgumentNullException("ruta");
            }

            if (string.IsNullOrWhiteSpace(this.nombre) && string.IsNullOrEmpty(this.nombre))
                throw new ArgumentNullException("nombre");


            pagina.Session["parametros"] = listaParametros;
            pagina.Session["nomReporte"] = this.ruta + this.nombre + ".rpt";

            string formularioReporte = !string.IsNullOrEmpty(ConfigurationManager.AppSettings.Get("ruta_frm_rpt"))
                && !string.IsNullOrWhiteSpace(ConfigurationManager.AppSettings.Get("ruta_frm_rpt")) ?
                ConfigurationManager.AppSettings.Get("ruta_frm_rpt") : "~/Reportes/Contenedor/frmReporte.aspx";

            string mensaje = "<script language='JavaScript'>window.open('" + formularioReporte + "','','status=0, width=1000px, height=690px, top=10, left=10,scrollbars=1');</script>";
            ScriptManager.RegisterStartupScript(pagina, pagina.GetType(), "", mensaje, false);
        }

        public string GenerarRutaReporte(Page pagina)
        {
            if (string.IsNullOrWhiteSpace(this.ruta) && string.IsNullOrEmpty(this.ruta))
            {
                this.ruta = ConfigurationManager.AppSettings.Get("rutarpt");
                if (string.IsNullOrWhiteSpace(this.ruta) && string.IsNullOrEmpty(this.ruta))
                    throw new ArgumentNullException("ruta");
            }

            if (string.IsNullOrWhiteSpace(this.nombre) && string.IsNullOrEmpty(this.nombre))
                throw new ArgumentNullException("nombre");

            pagina.Session["parametros"] = listaParametros;
            pagina.Session["nomReporte"] = this.ruta + this.nombre + ".rpt";

            string formularioReporte = !string.IsNullOrEmpty(ConfigurationManager.AppSettings.Get("ruta_frm_rpt"))
                && !string.IsNullOrWhiteSpace(ConfigurationManager.AppSettings.Get("ruta_frm_rpt")) ?
                ConfigurationManager.AppSettings.Get("ruta_frm_rpt") : "~/Reportes/Contenedor/frmReporte.aspx";

            return formularioReporte;

        }


        public static string NomReporte(HttpSessionState Session)
        {
            return Session["nomReporte"].ToString();
        }
    }
}
