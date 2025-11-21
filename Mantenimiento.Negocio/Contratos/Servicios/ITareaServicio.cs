using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mantenimiento.Datos.Entidades;
using Mantenimiento.Negocio.Poco;


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
        DetalleRequerimiento ObtenerDetalleRequerimiento(int idDetalleRequerimiento);
        List<sp_ListarSeguimientoTareas> ListarSeguimientoTareas(int idProyecto, int idEstadoDesarrollo, int idResponsable);
        void GuardarCambios();
        bool EditarDetalleRequerimiento(GestionarSeguimientoPoco obj);


    }
}
