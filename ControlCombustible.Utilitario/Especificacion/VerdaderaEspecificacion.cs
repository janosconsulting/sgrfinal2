using System;
using System.Linq.Expressions;

namespace Utilitario.Especificacion
{
    public sealed class VerdaderaEspecificacion<TEntidad> : Especificacion<TEntidad> where TEntidad : class
    {
        public override Expression<Func<TEntidad, bool>> SatisfechoPor()
        {
            bool result = true;

            Expression<Func<TEntidad, bool>> ExpresionVerdadera = t => result;
            return ExpresionVerdadera;
        }
    }
}
