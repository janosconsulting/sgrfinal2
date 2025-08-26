using System;
using System.Linq.Expressions;


namespace Utilitario.Especificacion
{
    public abstract class Especificacion<TEntidad> : IEspecificacion<TEntidad> where TEntidad : class
    {

        public abstract Expression<Func<TEntidad, bool>> SatisfechoPor();


        //Sobrecarga de Operadores

        //Operador AND
        public static Especificacion<TEntidad> operator &(Especificacion<TEntidad> EspecificacionIzquierda, Especificacion<TEntidad> EspecificacionDerecha)
        {
            return new YEspecificacion<TEntidad>(EspecificacionIzquierda, EspecificacionDerecha);
        }
        //Operador OR
        public static Especificacion<TEntidad> operator |(Especificacion<TEntidad> EspecificacionIzquierda, Especificacion<TEntidad> EspecificacionDerecha)
        {
            return new OEspecificacion<TEntidad>(EspecificacionIzquierda, EspecificacionDerecha);
        }
        //Operador NOT
        public static Especificacion<TEntidad> operator !(Especificacion<TEntidad> Especificacion_)
        {
            return new NoEspecificacion<TEntidad>(Especificacion_);
        }
        //Operador FALSE
        public static bool operator false(Especificacion<TEntidad> Especificacion_)
        {
            return false;
        }
        //Operador TRUE
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "especificacion")]
        public static bool operator true(Especificacion<TEntidad> Especificacion_)
        {
            return true;
        }
    }
}
