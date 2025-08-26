using System;

using Unity;

namespace Mantenimiento.IoC.Contenedor
{
    /// <summary>
    /// Contrato base de localizador y registro de depedencias (Basado de IoC y ID)
    /// </summary>
    public interface IContenedor
    {
        /// <summary>
        /// Resolver TServicio dependencia
        /// </summary>
        /// <typeparam name="TServicio">Tipo de dependencia</typeparam>
        /// <returns>instacia de TServicio</returns>
        TServicio Resolver<TServicio>();

        /// <summary>
        /// Resuelve tipo de construcción y retorna el objeto como una instacia TServicio
        /// </summary>
        /// <returns>Instancia de este tipo</returns>
        object Resolver(Type tipo);

        /// <summary>
        /// Registra el tipo dentro del servicio localizador
        /// </summary>
        /// <param name="type">Tipo a registrar</param>
        void RegistrarTipo(Type tipo);

        IUnityContainer EntregarContenedor();
    }
}
