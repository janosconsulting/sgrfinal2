using System;
using System.Linq.Expressions;


namespace Utilitario.Especificacion
{
    public class YEspecificacion<T> : ComposicionEspecificacion<T> where T : class
    {
        private IEspecificacion<T> LadoDerecho = null;
        private IEspecificacion<T> LadoIzquierdo = null;

        public YEspecificacion(IEspecificacion<T> LadoDerecho, IEspecificacion<T> LadoIzquierdo)
        {
            if (LadoDerecho == (IEspecificacion<T>)null)
                throw new ArgumentNullException("LadoDerecho");

            if (LadoIzquierdo == (IEspecificacion<T>)null)
                throw new ArgumentNullException("LadoIzquierdo");

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
            return Izquierda.Y(Derecha);
        }
    }
}
