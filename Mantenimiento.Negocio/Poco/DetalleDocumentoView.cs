namespace ReyDavid.Web.Models
{
    public class DetalleDocumentoView
    {
        public int idHabitacion { get; set; }
        public string nombre { get; set; }
        public int cantidad { get; set; }
        public decimal precio { get; set; }
        public decimal subTotal { get; set; }
        public string operacion { get; set; }
        public int cantDia { get; set; }
        public string key { get; set; }

    }
}