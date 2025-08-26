namespace Mantenimiento.IoC.Contenedor
{
    public sealed class Fabrica
    {
        #region Singleton

        static readonly Fabrica instancia = new Fabrica();

        /// <summary>
        /// Conseguir Instancia a través del patrón singleton
        /// </summary>
        public static Fabrica Instancia
        {
            get
            {
                return instancia;
            }
        }

        #endregion

        #region Miembros

        IContenedor _ContenedorActual;

        /// <summary>
        /// obtiene actual configuracion de IContenedor
        /// <remarks>
        /// En este momento solo existe  IContenedor 
        /// </remarks>
        /// </summary>
        public IContenedor ContenedorActual
        {
            get
            {
                return _ContenedorActual;
            }
        }

        #endregion

        #region Constructor

        /// <summary>
        /// Solo para uso de patrón singleton, remove before field init IL anotation
        /// </summary>
        static Fabrica()
        {

        }
        Fabrica()
        {
            _ContenedorActual = new cContenedor();
        }

        #endregion
    }
}
