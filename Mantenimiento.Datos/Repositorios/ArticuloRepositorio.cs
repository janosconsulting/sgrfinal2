using Mantenimiento.Datos.Base;
using Mantenimiento.Datos.Contratos;
using Mantenimiento.Datos.Entidades;
using Mantenimiento.Datos.Modelo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilitario.Log;

namespace Mantenimiento.Datos.Repositorios
{
    public class ArticuloRepositorio : Repositorio<Articulo>, IArticuloRepositorio
    {
        public ArticuloRepositorio(IContexto contexto, IOcurrencia ocurrencia)
            :base(contexto, ocurrencia)
        {

        }

        public List<sp_ListarArticulosxAutor_Result> ListarArticulosxAutor(int idAutor)
        {
            using (EDBCORAHEntities contextoActivo = new EDBCORAHEntities())
            {
                var lista = contextoActivo.sp_ListarArticulosxAutor(idAutor).ToList();
                return lista;
            }
        }

        public List<sp_ListarPublicaciones_Result> ListarPublicaciones()
        {
            using (EDBCORAHEntities contextoActivo = new EDBCORAHEntities())
            {
                var lista = contextoActivo.sp_ListarPublicaciones().ToList();
                return lista;
            }
        }

        public List<sp_BuscarPorPalabraClave_Result> BuscarPorPalabraClave(string palabraClave)
        {
            using (EDBCORAHEntities contextoActivo = new EDBCORAHEntities())
            {
                var lista = contextoActivo.sp_BuscarPorPalabraClave(palabraClave).ToList();
                return lista;
            }
        }
    }    
}
