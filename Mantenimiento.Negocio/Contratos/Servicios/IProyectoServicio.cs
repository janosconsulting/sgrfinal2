using Mantenimiento.Datos.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mantenimiento.Negocio.Contratos.Servicios
{
    public interface IProyectoServicio
    {
        List<sp_ListarProyectos> ListarProyectos();
        bool Insertar(Proyecto oProyecto);
        bool Actualizar(Proyecto oProyecto);
        bool Eliminar(int id);
        Proyecto obtenerProyecto(int id);
    }
}
