using Mantenimiento.Datos.Contratos;
using Mantenimiento.Datos.Entidades;
using Mantenimiento.Negocio.Contratos.Servicios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilitario.Especificacion;

namespace Mantenimiento.Negocio.Servicios
{
    public class UsuarioServicio : IUsuarioServicio
    {
        IUsuarioRepositorio repositorio;
        public UsuarioServicio(IUsuarioRepositorio repositorio)
        {
            this.repositorio = repositorio;
        }
        public Usuario Obtener(int idAutor)
        {
            return repositorio.Buscar(new EspecificacionDirecta<Usuario>(s => s.idAlumno == idAutor));
        }
        public Usuario ValidarLogin(string username, string pass)
        {
            Usuario u = repositorio.Buscar(
                new EspecificacionDirecta<Usuario>(
                    s => s.username == username &&
                    s.contrasenia == pass &&
                    s.idEstado == 1));

            return u;
        }
    }
}
