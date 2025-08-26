using Mantenimiento.Datos.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mantenimiento.Negocio.Contratos.Servicios
{
    public interface ICategoriaClienteServicio
    {
        List<sp_ListarCategoriaCliente> ListarCategoriaClientes();
        bool Insertar(CategoriaCliente oCategoriaCliente);
        bool Actualizar(CategoriaCliente oCategoriaCliente);
        bool Eliminar(int id);
        CategoriaCliente  obtenerCategoriaCliente(int id);
    }
}
