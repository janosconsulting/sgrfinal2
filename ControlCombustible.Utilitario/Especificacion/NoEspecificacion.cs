using System;
using System.Linq;
using System.Linq.Expressions;


namespace Utilitario.Especificacion
{
    public sealed class NoEspecificacion<TEntidad> : Especificacion<TEntidad> where TEntidad : class
    {
        Expression<Func<TEntidad, bool>> CriterioOriginal;

        public NoEspecificacion(IEspecificacion<TEntidad> EspecificacionOriginal)
        {

            if (EspecificacionOriginal == (IEspecificacion<TEntidad>)null)
                throw new ArgumentNullException("EspecificacionOriginal");

            CriterioOriginal = EspecificacionOriginal.SatisfechoPor();
        }
        public NoEspecificacion(Expression<Func<TEntidad, bool>> EspecificacionOriginal)
        {
            if (EspecificacionOriginal == (Expression<Func<TEntidad, bool>>)null)
                throw new ArgumentNullException("EspecificacionOriginal");

            CriterioOriginal = EspecificacionOriginal;
        }
        public override Expression<Func<TEntidad, bool>> SatisfechoPor()
        {

            return Expression.Lambda<Func<TEntidad, bool>>(Expression.Not(CriterioOriginal.Body), CriterioOriginal.Parameters.Single());
        }
    }
}
