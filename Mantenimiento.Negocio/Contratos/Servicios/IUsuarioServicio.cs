using Mantenimiento.Datos.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mantenimiento.Negocio.Contratos.Servicios
{
    public interface IUsuarioServicio
    {
        Usuario Obtener(int idAutor);
        Usuario ValidarLogin(string nombre, string pass);
    }
}
