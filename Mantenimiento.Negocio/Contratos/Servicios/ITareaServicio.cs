using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mantenimiento.Datos.Entidades;


namespace Mantenimiento.Negocio.Contratos.Servicios
{
    public interface ITareaServicio
    {
        List<sp_ListarTareas> ListarTareas();
        bool Insertar(Tarea oTarea);
        bool Actualizar(Tarea oTarea);
        bool Eliminar(int id);
        Tarea obtenerTarea(int id);
        // Método opcional para obtener la tarea por detalle requerimiento
        Tarea ObtenerPorRequerimiento(int idRequerimiento);
        Tarea ObtenerPorDetalleRequerimiento(int idDetalle);
        void GuardarCambios();


    }
}
