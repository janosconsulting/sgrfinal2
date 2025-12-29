using Mantenimiento.Datos.Entidades;
using System.Collections.Generic;
using System.Web;

namespace Mantenimiento.Negocio.Poco
{
    public class GestionarRequerimientoPoco
    {
        public Persona Persona { get; set; }
        public List<sp_listarRequerimientosporTrabajador> listarRequerimientosporTrabajador { get; set; }
        public List<sp_ListarClientes> ListarClientes { get; set; }
        public List<sp_ListarTrabajadores> ListarTrabajadores { get; set; }
        public List<sp_ListarProyectos> ListarProyectos { get; set; }
        public List<sp_ListarRequerimientosxFiltro> ListarRequerimientos { get; set; }
        public List<TipoRequerimiento> ListarTipoRequerimientos { get; set; }
        public Requerimiento Requerimiento { get; set; }
        public DetalleRequerimiento DetalleRequerimiento { get; set; }
        public List<ListaDetalleRequerimiento> ListaDetalleRequerimiento { get; set; }
        public string Codigo { get; set; }
        public List<Adicional> ListarAdicionales { get; set; }
        public List<ListaFoldersMapping> ListaFoldersMapping { get; set; }
    }
}