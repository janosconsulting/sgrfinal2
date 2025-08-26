namespace Mantenimiento.Negocio.Poco
{
    public class ResultadoTransaccion
    {
        public int idResultado { get; set; }

        public int codigo { get; set; }

        public string mensaje { get; set; }

        public byte[] data { get; set; }
    }
}
