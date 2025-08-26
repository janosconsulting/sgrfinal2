using System;
using System.Linq.Expressions;


namespace Utilitario.Especificacion
{
    public sealed class EspecificacionDirecta<TEntidad> : Especificacion<TEntidad> where TEntidad : class
    {

        Expression<Func<TEntidad, bool>> CriterioIgualado;

        public EspecificacionDirecta(Expression<Func<TEntidad, bool>> CriterioIgualado)
        {
            if (CriterioIgualado == (Expression<Func<TEntidad, bool>>)null)
                throw new ArgumentNullException("CriterioIgualado");

            this.CriterioIgualado = CriterioIgualado;
        }
        public override Expression<Func<TEntidad, bool>> SatisfechoPor()
        {
            return CriterioIgualado;
        }
    }
}
