using Mantenimiento.Datos.Entidades;
using Mantenimiento.Negocio.Poco;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mantenimiento.Negocio.Contratos.Servicios
{
    public interface ISubscripcionServicio
    {
        List<Moneda> ListarMoneda();
        List<Frecuencia> ListarFrecuencia();
        List<Servicio> ListarServicio();
        GestionarSubscripcion ListarSubscripcion(int idServicio, int idFrecuencia, int idEstado, int anio, int mes);
        Subscripcion Obtener(int idSubscripcion);
        ResultadoTransaccion Insertar(Subscripcion obj);
        ResultadoTransaccion Actualizar(Subscripcion obj);
        bool Eliminar(int id);
        ResultadoTransaccion CobrarSuscripcion(Subscripcion obj);
        ResultadoTransaccion RenovarSuscripcion(Subscripcion obj);
        List<sp_ListarSubscripcionReporte> ListarSubscripcionReporte(int anio);
    }
}
