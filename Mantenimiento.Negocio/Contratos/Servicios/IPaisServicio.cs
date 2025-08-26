using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mantenimiento.Datos.Entidades;

namespace Mantenimiento.Negocio.Contratos.Servicios
{
    public interface IPaisServicio
    {
        List<sp_ListarPaises> ListarPaises();
    }
}
