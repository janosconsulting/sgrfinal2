namespace Utilitario.Reporte
{
    public enum TipoParametroReporteWeb : short
    {
        Entero = 1,
        Cadena = 2,
        Caracter = 3,
        Decimal = 4,
        Fecha = 5,
        Boleano = 6
    }
    public class ParametroReporteWeb
    {
        public string Nombre { get; set; }
        public object Valor { get; set; }
        public TipoParametroReporteWeb Tipo { get; set; }
    }
}
