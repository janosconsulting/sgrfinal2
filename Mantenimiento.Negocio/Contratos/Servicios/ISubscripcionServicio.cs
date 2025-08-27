using Mantenimiento.Datos.Entidades;
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
        List<sp_ListarSubscripcion> ListarSubscripcion();
    }
}
