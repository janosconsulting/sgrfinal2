namespace ERP.Web.Models
{
    public class Resultado
    {
        public int idResultado { get; set; }
        public string mensaje { get; set; }
        public int codigo { get; set; }
    }

    public enum enumTipoMensaje
    {
        exito = 1,
        error = -1,
        warning = 0
    }

    public class ReporteGrafico
    {
        public int id { get; set; }
        public string nombre { get; set; }
        public decimal importe { get; set; }
        public decimal? venta { get; set; }
        public decimal? utilidad { get; set; }
    }
}