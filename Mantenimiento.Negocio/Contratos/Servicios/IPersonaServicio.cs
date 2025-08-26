using Mantenimiento.Datos.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mantenimiento.Negocio.Contratos.Servicios
{
    public interface IPersonaServicio
    {
        bool ValidarNdocumento(string numeroDocumento);
        List<Persona> ListarPersona();
        List<sp_ListarClientes> ListarClientes();
        List<sp_ListarTrabajadores> ListarTrabajadores();
        bool Insertar(Persona oPersona);
        bool Actualizar(Persona oPersona);
        bool Eliminar(int id);
        Persona obtenerPersona(int id);
    }
}