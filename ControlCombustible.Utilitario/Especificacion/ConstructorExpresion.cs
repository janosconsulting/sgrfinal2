using System;
using System.Linq;
using System.Linq.Expressions;

namespace Utilitario.Especificacion
{
    public static class ConstructorExpresion
    {
        public static Expression<T> Composicion<T>(this Expression<T> primero, Expression<T> segundo, Func<Expression, Expression, Expression> combinacion)
        {
            var mapa = primero.Parameters.Select((f, i) => new { f, s = segundo.Parameters[i] }).ToDictionary(p => p.s, p => p.f);
            var SegundoCuerpo = ParametroCubridor.ReemplazarParametro(mapa, segundo.Body);
            return Expression.Lambda<T>(combinacion(primero.Body, SegundoCuerpo), primero.Parameters);
        }
        public static Expression<Func<T, bool>> Y<T>(this Expression<Func<T, bool>> primero, Expression<Func<T, bool>> segundo)
        {
            return primero.Composicion(segundo, Expression.And);
        }
        public static Expression<Func<T, bool>> O<T>(this Expression<Func<T, bool>> primero, Expression<Func<T, bool>> segundo)
        {
            return primero.Composicion(segundo, Expression.Or);
        }
    }
}
