using Mantenimiento.Datos.Contratos;
using Mantenimiento.Datos.Entidades;
using Mantenimiento.Negocio.Contratos.Servicios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mantenimiento.Negocio.Servicios
{
    public class DocumentoIdentidadServicio : IDocumentoIdentidadServicio
    {
        public IDocumentoIdentidadRepositorio repositorio;

        public DocumentoIdentidadServicio(IDocumentoIdentidadRepositorio repositorio)
        {
            this.repositorio = repositorio;
        }
        public List<DocumentoIdentidad> Listar()
        {
            return repositorio.TraerElementoFiltrado(e => e.idEstado == 1);
        }
    }
}
