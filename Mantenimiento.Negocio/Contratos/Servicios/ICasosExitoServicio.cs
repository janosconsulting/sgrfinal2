using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mantenimiento.Datos.Entidades;


namespace Mantenimiento.Negocio.Contratos.Servicios
{
    public interface ICasosExitoServicio
    {
        sp_ObtenerCasosExito ObtenerCasoPorId(int idCaso);

        List<sp_ObtenerCasosExito> ListarCasosExito();
        bool Insertar(CasosExito oCasosExito);
        bool Actualizar(CasosExito oCasosExito);
        bool Eliminar(int id);
        CasosExito obtenerCasosExito(int id);
    }
}
