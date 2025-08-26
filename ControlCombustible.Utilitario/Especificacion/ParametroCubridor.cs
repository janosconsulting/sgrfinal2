using System.Collections.Generic;
using System.Linq.Expressions;


namespace Utilitario.Especificacion
{
    public sealed class ParametroCubridor : ExpressionVisitor
    {
        private readonly Dictionary<ParameterExpression, ParameterExpression> mapa;

        public ParametroCubridor(Dictionary<ParameterExpression, ParameterExpression> mapa)
        {
            this.mapa = mapa ?? new Dictionary<ParameterExpression, ParameterExpression>();
        }
        public static Expression ReemplazarParametro(Dictionary<ParameterExpression, ParameterExpression> mapa, Expression expresion)
        {
            return new ParametroCubridor(mapa).Visit(expresion);
        }
        protected override Expression VisitParameter(ParameterExpression parametro)
        {
            ParameterExpression replacement;
            if (mapa.TryGetValue(parametro, out replacement))
            {
                parametro = replacement;
            }

            return base.VisitParameter(parametro);
        }

    }
}
