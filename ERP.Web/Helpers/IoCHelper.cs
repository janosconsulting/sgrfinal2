

using Mantenimiento.IoC.Contenedor;
using System;

namespace Mantenimiento.ERP.Helper
{
    public class IoCHelper
    {
        public static T ResolverIoC<T>() where T : class
        {
            IContenedor contenedor = Fabrica.Instancia.ContenedorActual;
            Type tipo = typeof(T);
            return contenedor.Resolver(tipo) as T;
        }
    }
}