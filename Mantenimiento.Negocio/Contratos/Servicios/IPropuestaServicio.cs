using Mantenimiento.Datos.Entidades;
using Mantenimiento.Negocio.Poco;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mantenimiento.Negocio.Contratos.Servicios
{
    public interface IPropuestaServicio
    {
        List<sp_ListarPropuesta> Listar(int? cliente, int? estado);
        PropuestaEntidad Obtener(int id);
        PropuestaPdfPoco ObtenerParaPdf(int idPropuesta);

        bool Insertar(PropuestaEntidad doc);
        bool Actualizar(PropuestaEntidad doc);

        bool Eliminar(int id); // recomendado lógico

    }
}
