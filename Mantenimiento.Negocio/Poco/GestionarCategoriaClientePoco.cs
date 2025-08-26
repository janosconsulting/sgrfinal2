using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mantenimiento.Datos.Entidades;

namespace Mantenimiento.Negocio.Poco
{
    public class GestionarCategoriaClientePoco
    {
        public CategoriaCliente CategoriaCliente { get; set; }
        public List<sp_ListarClientes> listarClientes { get; set; }
        public List<sp_ListarCategoriaCliente> listarCategoriaClientes { get; set; }
        public bool IsEditing { get; set; }
    }
}
