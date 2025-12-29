using Mantenimiento.Datos.Entidades;
using Mantenimiento.Negocio.Poco;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mantenimiento.Negocio.Contratos.Servicios
{
    public interface IDocumentoOrigenServicio
    {
        List<sp_ListarDocumentoOrigen> Listar(int? cliente, int? proyecto, int? tipo, int? estado);
        Adicional Obtener(int idDocumentoOrigen);

        bool Insertar(Adicional doc);
        bool Actualizar(Adicional doc);

        bool Eliminar(int idDocumentoOrigen); // recomendado lógico
        ReporteChecklistDocumentoOrigenVm ObtenerChecklistVm(int idDocumentoOrigen);

        string GenerarCodigo(); // opcional
    }
}
