using System;
using System.Data.Objects;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;


namespace Utilitario.Extensiones
{
    public static class IQueryableExtensions
    {
        #region Metodos de extension

        public static IQueryable<TEntity> Include<TEntity>(this IQueryable<TEntity> Consultable, string ruta)
            where TEntity : class
        {
            if (String.IsNullOrEmpty(ruta))
                throw new ArgumentNullException("");

            ObjectQuery<TEntity> consulta = Consultable as ObjectQuery<TEntity>;

            return consulta.Include(ruta);

        }

        public static IQueryable<TEntity> Include<TEntity>(this IQueryable<TEntity> Consultable, Expression<Func<TEntity, object>> ruta)
            where TEntity : class
        {
            return Include<TEntity>(Consultable, AnalizarExpresionRuta(ruta));
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures")]
        public static IQueryable<TEntidad> Paginado<TEntidad, S>(this IQueryable<TEntidad> Consultable, Expression<Func<TEntidad, S>> OrdenarPor, int IndicePagina, int CantidadPagina, bool Ascendente)
            where TEntidad : class
        {
            ObjectQuery<TEntidad> consulta = Consultable as ObjectQuery<TEntidad>;

            if (consulta != null)
            {
                //Este metodo de paginado usa ESQL para resolver problemas con la  parametrizacion de las consultas
                //En L2E y Skip/Take metodos

                string orderPath = AnalizarExpresionRuta<TEntidad, S>(OrdenarPor);

                return consulta.Skip(string.Format(CultureInfo.InvariantCulture, "it.{0} {1}", orderPath, (Ascendente) ? "asc" : "desc"), "@skip", new ObjectParameter("skip", (IndicePagina) * CantidadPagina))
                            .Top("@limit", new ObjectParameter("limit", CantidadPagina));

            }
            else // for In-Memory object set
                return Consultable.OrderBy(OrdenarPor).Skip((IndicePagina * CantidadPagina)).Take(CantidadPagina);
        }

        #endregion

        #region Metodos privados

        private static string AnalizarExpresionRuta<TEntidad, S>(Expression<Func<TEntidad, S>> expresion)
            where TEntidad : class
        {
            if (expresion == (Expression<Func<TEntidad, S>>)null)
                throw new ArgumentNullException(""); // falta agregar argumento

            MemberExpression cuerpo = expresion.Body as MemberExpression;
            if (((cuerpo == null) || !cuerpo.Member.DeclaringType.IsAssignableFrom(typeof(TEntidad))) || (cuerpo.Expression.NodeType != ExpressionType.Parameter))
            {
                throw new ArgumentException(""); // falta agregar argumento
            }
            else
                return cuerpo.Member.Name;
        }
        #endregion
    }
}
