using ControlCombustible.Negocio.Poco;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControlCombustible.Negocio.Contratos.Base
{
    public interface IServicio
    {
        UsuarioOperacion UsuarioOperacion { set; }

        //void CargarInformacionUsuario<T>(IRepositorio<T> repositorio) where T : class
    }
}
