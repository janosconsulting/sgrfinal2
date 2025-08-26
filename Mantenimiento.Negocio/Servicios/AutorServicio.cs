using Mantenimiento.Datos.Base;
using Mantenimiento.Datos.Contratos;
using Mantenimiento.Datos.Entidades;
using Mantenimiento.Negocio.Contratos.Servicios;
using Mantenimiento.Negocio.Poco;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using Utilitario.Especificacion;

namespace Mantenimiento.Negocio.Servicios
{
    public class AutorServicio : IAutorServicio
    {

        IAutorRepositorio repositorio;
        IUsuarioRepositorio repositorioUsuario;

        public AutorServicio(
            IAutorRepositorio repositorio,
            IUsuarioRepositorio repositorioUsuario
            )
        {
            this.repositorio = repositorio;
            this.repositorioUsuario = repositorioUsuario;
        }

        public List<sp_ListarPersonas_Result> Listar()
        {
            return repositorio.Listar();
        }

        public List<sp_ListarPerfiles_Result> ListarPerfil()
        {
            return repositorio.ListarPerfil();
        }
        public ResultadoTransaccion Insertar(GestionarPersonaPoco objeto)
        {
            ResultadoTransaccion objResTransaccion = new ResultadoTransaccion();
            TransactionOptions Tranconfiguracion = new TransactionOptions()
            {
                Timeout = TransactionManager.MaximumTimeout,
                IsolationLevel = IsolationLevel.Serializable
            };

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.RequiresNew, Tranconfiguracion))
            {
                IUnidadTrabajo unidadTrabajo = repositorio.Contexto;
                IUnidadTrabajo unidadTrabajoUsuario = repositorioUsuario.Contexto as IUnidadTrabajo;
                try
                {
                    Autor objAutor = this.repositorio.Buscar(new EspecificacionDirecta<Autor>(s => s.nroIdentidad == objeto.Autor.nroIdentidad && s.idTipoIdentidad == objeto.Autor.idTipoIdentidad));

                    if (objAutor != null)
                    {
                        throw new Exception("La persona ya se encuentra registrada.");
                    }
                    objeto.Autor.fechaRegistro = DateTime.Now;

                    repositorio.Nuevo(objeto.Autor);
                    unidadTrabajo.Commit();

                    if (objeto.Usuario != null)
                    {
                        Usuario objUsuario = new Usuario();
                        objUsuario.idAlumno = objeto.Autor.idAutor;
                        objUsuario.username = objeto.Usuario.username;
                        objUsuario.idEstado = 1;
                        objUsuario.contrasenia = objeto.Usuario.contrasenia;
                        objUsuario.esAdmin = 0;
                        repositorioUsuario.Nuevo(objUsuario);
                        unidadTrabajoUsuario.Commit();
                    }

                    scope.Complete();
                    objResTransaccion.codigo = objeto.Autor.idAutor;
                }
                catch (Exception ex)
                {
                    objResTransaccion.codigo = -1;
                    objResTransaccion.mensaje = ex.Message.ToString();
                }

                return objResTransaccion;
            }

        }

        public Autor Obtener(int idAutor)
        {
            return repositorio.Buscar(new EspecificacionDirecta<Autor>(a => a.idAutor == idAutor));
        }

        public bool Actualizar(GestionarPersonaPoco objeto)
        {
            ResultadoTransaccion objResTransaccion = new ResultadoTransaccion();
            TransactionOptions Tranconfiguracion = new TransactionOptions()
            {
                Timeout = TransactionManager.MaximumTimeout,
                IsolationLevel = IsolationLevel.Serializable
            };

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.RequiresNew, Tranconfiguracion))
            {
                bool resultado = false;

                IUnidadTrabajo unidadTrabajo = repositorio.Contexto as IUnidadTrabajo;
                IUnidadTrabajo unidadTrabajoUsuario = repositorioUsuario.Contexto as IUnidadTrabajo;
                try
                {
                    Autor objAutor = this.repositorio.Buscar(new EspecificacionDirecta<Autor>(s => s.idAutor == objeto.Autor.idAutor));

                    objAutor.esAlumno = objeto.Autor.esAlumno;
                    objAutor.nombres = objeto.Autor.nombres;
                    objAutor.apellidoPaterno = objeto.Autor.apellidoPaterno;
                    objAutor.apellidoMaterno = objeto.Autor.apellidoMaterno;
                    //objAutor.idTipoIdentidad = objeto.Autor.idTipoIdentidad;
                    //objAutor.nroIdentidad = objeto.Autor.nroIdentidad;
                    objAutor.fechaRegistro = DateTime.Now;

                    repositorio.Modificar(objAutor);
                    unidadTrabajo.Commit();

                    if (objeto.Usuario != null)
                    {
                        Usuario objUsuario = repositorioUsuario.Buscar(new EspecificacionDirecta<Usuario>(s => s.idAlumno == objeto.Autor.idAutor));
                        objUsuario.username = objeto.Usuario.username;
                        if (objeto.Usuario.contrasenia != null)
                        {
                            objUsuario.contrasenia = objeto.Usuario.contrasenia;
                        }
                        repositorioUsuario.Modificar(objUsuario);
                        unidadTrabajoUsuario.Commit();
                    }

                    scope.Complete();
                    resultado = true;
                }
                catch (Exception ex)
                {
                    objResTransaccion.codigo = -1;
                    objResTransaccion.mensaje = ex.Message.ToString();
                }

                return resultado;
            }

        }

        public bool Eliminar(int idAutor)
        {
            TransactionOptions Tranconfiguracion = new TransactionOptions()
            {
                Timeout = TransactionManager.MaximumTimeout,
                IsolationLevel = IsolationLevel.Serializable
            };

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.RequiresNew, Tranconfiguracion))
            {
                bool resultado = false;

                IUnidadTrabajo unidadTrabajo = repositorio.Contexto as IUnidadTrabajo;
                IUnidadTrabajo unidadTrabajoUsuario = repositorioUsuario.Contexto as IUnidadTrabajo;
                try
                {
                    Autor objAutor = repositorio.Buscar(new EspecificacionDirecta<Autor>(s => s.idAutor == idAutor));
                    objAutor.idEstado = 2;
                    repositorio.Modificar(objAutor);
                    unidadTrabajo.Commit();

                    Usuario objUsuario = repositorioUsuario.Buscar(new EspecificacionDirecta<Usuario>(s => s.idAlumno == idAutor));
                    objUsuario.idEstado = 2;
                    repositorioUsuario.Modificar(objUsuario);
                    unidadTrabajoUsuario.Commit();

                    scope.Complete();
                    resultado = true;
                }
                catch (Exception)
                {
                    throw;
                }

                return resultado;
            }
        }
    }
}
