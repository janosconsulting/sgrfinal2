using System;
using System.Linq.Expressions;

namespace Utilitario.Especificacion
{
    public interface IEspecificacion<TEntidad> where TEntidad : class
    {
        Expression<Func<TEntidad, bool>> SatisfechoPor();
    }
}
