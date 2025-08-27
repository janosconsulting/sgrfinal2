using Mantenimiento.Datos.Entidades;
using System.Collections.Generic;
using System.Web;


namespace Mantenimiento.Negocio.Poco
{
    public class GestionarClientePoco
    {       
        public List<DocumentoIdentidad> ListarDocumentoIdentidad { get; set; }
        public Persona Persona { get; set; }
        public List<sp_ListarClientes> listarClientes { get; set; }
        public bool IsEditing { get; set; }
        public List<Moneda> ListarMoneda { get; set; }
        public List<Frecuencia> ListarFrecuencia { get; set; }
        public List<Servicio> ListarServicio { get; set; }
        public List<sp_ListarCategoriaCliente> listarCategoriaClientes { get; set; }
    }
}
