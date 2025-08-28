using Mantenimiento.Datos.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mantenimiento.Negocio.Poco
{
    public class GestionarPersonaPoco
    {
        public List<DocumentoIdentidad> ListaDocumentoIdentidad { get; set; }

        public Autor Autor { get; set; }

        public Usuario Usuario { get; set; }

        public Persona Persona { get; set; }

    }
    public class GestionarSubscripcion
    {
        public ResumenDetallSubscripcion resumenDetallSubscripcionCobrados { get; set; }
        public ResumenDetallSubscripcion resumenDetallSubscripcionActivos { get; set; }
        public List<sp_ListarSubscripcion> listaSuscripcion1 { get; set; }
    }
    public class ResumenDetallSubscripcion
    {
        public decimal totalImportePagado {  get; set; }
        public decimal totalGenerado { get; set; }
    }
}
