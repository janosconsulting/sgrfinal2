using Mantenimiento.Datos.Entidades;
using Mantenimiento.Negocio.Poco;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mantenimiento.Negocio.Contratos.Servicios
{
    public interface IArticuloServicio
    {
        Articulo Obtener(int idArticulo);

        List<sp_ListarArticulosxAutor_Result> ListarxAutor(int idAutor);

        List<sp_ListarPublicaciones_Result> ListarPublicaciones();

        List<sp_BuscarPorPalabraClave_Result> BuscarPorPalabraClave(string palabraClave);

        List<ArticuloAutor> ListarAutor(int idArticulo);

        List<ArticuloConclusion> ListarConclusion(int idArticulo);

        List<ArticuloReferencia> ListarReferencia(int idArticulo);

        List<ArticuloAdjunto> ListarAdjunto(int idArticulo);

        ResultadoTransaccion Insertar(GestionarArticuloPoco objeto);

        bool Actualizar(GestionarArticuloPoco objeto);

        bool Eliminar(int idArticulo);

        bool EliminarAdjunto(int idAdjunto);

        bool Publicar(int idArticulo);

        bool NoPublicar(int idArticulo);
    }
}
