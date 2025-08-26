using Mantenimiento.Datos.Base;
using Mantenimiento.Datos.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mantenimiento.Datos.Contratos
{
    public interface IArticuloRepositorio : IRepositorio<Articulo>
    {
        List<sp_ListarPublicaciones_Result> ListarPublicaciones();

        List<sp_ListarArticulosxAutor_Result> ListarArticulosxAutor(int idAutor);

        List<sp_BuscarPorPalabraClave_Result> BuscarPorPalabraClave(string palabraClave);
    }
}
