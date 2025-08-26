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
    public class AutorRepositorio : Repositorio<Autor>, IAutorRepositorio
    {
        public AutorRepositorio(IContexto contexto, IOcurrencia ocurrencia)
            :base(contexto, ocurrencia)
        { 
        }

        public List<sp_ListarPersonas_Result> Listar()
        {
            using (EDBCORAHEntities contextoActivo = new EDBCORAHEntities())
            {
                var lista = contextoActivo.sp_ListarPersonas().ToList();
                return lista;
            }
        }

        public List<sp_ListarPerfiles_Result> ListarPerfil()
        {
            using (EDBCORAHEntities contextoActivo = new EDBCORAHEntities())
            {
                var lista = contextoActivo.sp_ListarPerfiles().ToList();
                return lista;
            }
        }
    }
}
