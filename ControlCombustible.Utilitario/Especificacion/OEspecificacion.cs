using System;
using System.Linq.Expressions;

namespace Utilitario.Especificacion
{
    public class OEspecificacion<T> : ComposicionEspecificacion<T> where T : class
    {
        private IEspecificacion<T> LadoDerecho = null;
        private IEspecificacion<T> LadoIzquierdo = null;


        public OEspecificacion(IEspecificacion<T> LadoDerecho, IEspecificacion<T> LadoIzquierdo)
        {
            if (LadoDerecho == (IEspecificacion<T>)null)
                throw new ArgumentNullException("Lado Derecho");

            if (LadoIzquierdo == (IEspecificacion<T>)null)
                throw new ArgumentNullException("Lado Izquierdo");

            this.LadoDerecho = LadoDerecho;
            this.LadoIzquierdo = LadoIzquierdo;
        }

        public override IEspecificacion<T> EspecificacionDerecha
        {
            get { return LadoDerecho; }
        }

        public override IEspecificacion<T> EspecificacionIzquierda
        {
            get { return LadoIzquierdo; }
        }
        public override Expression<Func<T, bool>> SatisfechoPor()
        {
            Expression<Func<T, bool>> Derecha = LadoDerecho.SatisfechoPor();
            Expression<Func<T, bool>> Izquierda = LadoIzquierdo.SatisfechoPor();
            return Izquierda.O(Derecha);
        }
    }
}
