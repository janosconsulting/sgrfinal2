using Mantenimiento.Datos.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mantenimiento.Negocio.Poco
{
    public class GestionarDocumentoOrigenPoco
    {
        // Entidad principal para Nuevo/Editar
        public Adicional DocumentoOrigen { get; set; }

        // Listas para combos (filtros y formulario)
        public List<sp_ListarClientes> ListarClientes { get; set; } = new List<sp_ListarClientes>();
        public List<sp_ListarProyectos> ListarProyectos { get; set; } = new List<sp_ListarProyectos>();

        // (Opcional) lista para mostrar en la vista sin DataTable
        public List<sp_ListarDocumentoOrigen> ListarDocumentoOrigen { get; set; } = new List<sp_ListarDocumentoOrigen>();

        // Para la vista Nuevo en modo edición (como haces en CasosExito)
        public bool IsEditing { get; set; } = false;
    }

    public class sp_ListarDocumentoOrigen
    {
        public int idDocumentoOrigen { get; set; }
        public string codigo { get; set; }
        public int tipoDoc { get; set; }

        public string nombreCli { get; set; }
        public string nombreProyec { get; set; }

        public DateTime fechaDocumento { get; set; }
        public string titulo { get; set; }
        public string descripcion { get; set; }

        public int estado { get; set; }
        public int totalRequerimientos { get; set; }
        public bool tieneChecklist { get; set; }
    }

    public class ReporteChecklistDocumentoOrigenVm
    {
        public string EmpresaNombre { get; set; }
        public string CodigoReporte { get; set; }

        public int IdDocumentoOrigen { get; set; }

        public string DocCodigo { get; set; }
        public string DocTipo { get; set; }
        public DateTime DocFecha { get; set; }
        public string DocTitulo { get; set; }
        public string DocDescripcion { get; set; }
        public string EstadoGeneral { get; set; }

        public string ClienteNombre { get; set; }
        public string ProyectoNombre { get; set; }

        public List<ReporteChecklistDocumentoOrigenItemVm> Items { get; set; } = new List<ReporteChecklistDocumentoOrigenItemVm>();
    }

    public class ReporteChecklistDocumentoOrigenItemVm
    {
        public int IdRequerimiento { get; set; }
        public string Codigo { get; set; }
        public string Titulo { get; set; }
        public string Detalle { get; set; }

        public string Solicitante { get; set; }
        public string Asignado { get; set; }

        public int Prioridad { get; set; }
        public int EstadoReq { get; set; }

        public DateTime? FechaInicio { get; set; }
        public DateTime? FechaFin { get; set; }
    }
}
