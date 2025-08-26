using Mantenimiento.Datos.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mantenimiento.Negocio.Poco;

namespace Mantenimiento.Negocio.Contratos.Servicios
{
    public interface IDetalleRequerimiento
    {       
        List<ListaDetalleRequerimiento> ObtenerDetalleReq(int id);
        DetalleRequerimiento Obtener(int id);
    }
}