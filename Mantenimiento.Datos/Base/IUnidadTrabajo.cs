using Mantenimiento.Datos.Entidades;
using System.Collections.Generic;

namespace Mantenimiento.Datos.Base
{
    public interface IUnidadTrabajo
    {
        IEnumerable<TEntidad> EjecutarConsulta<TEntidad>(string sqlConsulta, params object[] parametros);
        int EjecutarComando(string sqlComando, params object[] parametros);
        void Commit();
        void Commit_RefrecarCambios();
        void Rollback_Cambios();
        void RegistrarCambios<TEntidad>(TEntidad item) where TEntidad : class, IObjectWithChangeTracker;
    }
}
