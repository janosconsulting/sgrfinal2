namespace Utilitario.Especificacion
{
    public abstract class ComposicionEspecificacion<TEntidad> : Especificacion<TEntidad> where TEntidad : class
    {
        public abstract IEspecificacion<TEntidad> EspecificacionIzquierda { get; }
        public abstract IEspecificacion<TEntidad> EspecificacionDerecha { get; }

    }
}
