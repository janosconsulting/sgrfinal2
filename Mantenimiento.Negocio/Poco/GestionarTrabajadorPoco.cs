using System;
using Mantenimiento.Datos.Entidades;
using System.Collections.Generic;
using System.Web;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mantenimiento.Negocio.Poco
{
  public  class GestionarTrabajadorPoco
    {
        public List<DocumentoIdentidad> ListarDocumentoIdentidad { get; set; }
        public Persona Persona { get; set; }
        public List<sp_ListarTrabajadores> listarTrabajadores { get; set; }
        public bool IsEditing { get; set; }
    }
}
