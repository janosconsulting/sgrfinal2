using Mantenimiento.Datos.Entidades;
using Mantenimiento.Negocio.Poco;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mantenimiento.Negocio.Contratos.Servicios
{
    public interface IRequerimientoServicio
    {
        WizardGenerarReqDesdeDoResponse GenerarDesdeDocumentoOrigenWizard(WizardGenerarReqDesdeDoRequest request);
        List<sp_ListarRequerimientosxFiltro> ListaRequerimientos(int? cliente, int? proyecto, int? estado);
        string GenerarCodigoAutomatico();
        string GetSubFolderByExtension(string extension);
        bool Actualizar(GestionarRequerimientoPoco oRequerimiento);
        bool Eliminar(int id);
        Requerimiento ObtenerRequerimiento(int id);
        bool Insertar(GestionarRequerimientoPoco requerimiento);
    }
}