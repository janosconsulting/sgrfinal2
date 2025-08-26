using Mantenimiento.Datos.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Mantenimiento.Negocio.Poco
{
    public class GestionarArticuloPoco
    {
        public Articulo Articulo { get; set; }

        public HttpPostedFileBase PortadaFile { get; set; }

        public List<ArticuloAutor> ListaAutor { get; set; }

        public List<ArticuloConclusion> ListaConclusion { get; set; }

        public List<ArticuloReferencia> ListaReferencia { get; set; }

        public List<HttpPostedFileBase> ListaAdjuntoFile { get; set; }

        public List<ArticuloAdjunto> ListaAdjunto { get; set; }

        public List<DetalleRepeatablePoco> ListaAutorRepeatablePoco { get; set; }

        public List<DetalleRepeatablePoco> ListaReferenciaRepeatablePoco { get; set; }

        public List<DetalleRepeatablePoco> ListaConclusionRepeatablePoco { get; set; }

        public List<DetalleRepeatablePoco> ListaAdjuntoRepeatablePoco { get; set; }
    }
}
