using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mantenimiento.Datos.Entidades;
using Mantenimiento.Negocio.Poco;


namespace Mantenimiento.Negocio.Contratos.Servicios
{
   public interface IRequerimientosporTrabajadorServicio
    {
        List<sp_listarRequerimientosporTrabajador> listarRequerimientosporTrabajador(int? idTrabajador = null);
        bool Actualizar(GestionarRequerimientoPoco objeto);
        DetalleRequerimiento ObtenerDetallePorId(int idDetalle);
        List<sp_ListarTrabajadores> ListarTrabajadores();

    }
}
