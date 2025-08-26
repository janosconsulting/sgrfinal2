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
    public class ArticuloConclusionRepositorio : Repositorio<ArticuloConclusion>, IArticuloConclusionRepositorio
    {
        public ArticuloConclusionRepositorio(IContexto contexto, IOcurrencia ocurrencia)
            :base(contexto, ocurrencia)
        {

        }
    }
}
