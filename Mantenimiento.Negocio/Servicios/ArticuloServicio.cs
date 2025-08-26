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
    public class ArticuloServicio : IArticuloServicio
    {
        IArticuloRepositorio repositorio;
        IArticuloAutorRepositorio repositorioAutor;
        IArticuloReferenciaRepositorio repositorioReferencia;
        IArticuloConclusionRepositorio repositorioConclusion;
        IArticuloAdjuntoRepositorio repositorioAdjunto;
        public ArticuloServicio(
            IArticuloRepositorio repositorio,
            IArticuloAutorRepositorio repositorioAutor,
            IArticuloReferenciaRepositorio repositorioReferencia,
            IArticuloConclusionRepositorio repositorioConclusion,
            IArticuloAdjuntoRepositorio repositorioAdjunto)
        {
            this.repositorio = repositorio;
            this.repositorioAutor = repositorioAutor;
            this.repositorioReferencia = repositorioReferencia;
            this.repositorioConclusion = repositorioConclusion;
            this.repositorioAdjunto = repositorioAdjunto;
        }

        public Articulo Obtener(int idArticulo)
        {
            return repositorio.Buscar(new EspecificacionDirecta<Articulo>(s => s.idArticulo == idArticulo));
        }

        public List<sp_BuscarPorPalabraClave_Result> BuscarPorPalabraClave(string palabraClave)
        {
            return repositorio.BuscarPorPalabraClave(palabraClave);
        }

        public ResultadoTransaccion Insertar(GestionarArticuloPoco objeto)
        {
            ResultadoTransaccion objResTransaccion = new ResultadoTransaccion();
            TransactionOptions Tranconfiguracion = new TransactionOptions()
            {
                Timeout = TransactionManager.MaximumTimeout,
                IsolationLevel = IsolationLevel.Serializable
            };

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.RequiresNew, Tranconfiguracion))
            {
                IUnidadTrabajo unidadTrabajo = repositorio.Contexto as IUnidadTrabajo;
                IUnidadTrabajo unidadTrabajoAutor = repositorioAutor.Contexto as IUnidadTrabajo;
                IUnidadTrabajo unidadTrabajoReferencia = repositorioReferencia.Contexto as IUnidadTrabajo;
                IUnidadTrabajo unidadTrabajoConclusion = repositorioConclusion.Contexto as IUnidadTrabajo;
                IUnidadTrabajo unidadTrabajoAdjunto = repositorioAdjunto.Contexto as IUnidadTrabajo;
                try
                {
                    Articulo objArticulo = this.repositorio.Buscar(new EspecificacionDirecta<Articulo>(s => s.titulo == objeto.Articulo.titulo && s.idAutor == objeto.Articulo.idAutor));

                    if (objArticulo != null)
                    {
                        throw new Exception("El articulo ya se encuentra registrado.");
                    }
                    objeto.Articulo.idEstado = 1;
                    objeto.Articulo.fechaRegistro = DateTime.Now;
                    repositorio.Nuevo(objeto.Articulo);
                    unidadTrabajo.Commit();

                    if (objeto.ListaAutorRepeatablePoco != null)
                    {
                        foreach (var item in objeto.ListaAutorRepeatablePoco)
                        {
                            if (item.Operacion == "N")
                            {
                                ArticuloAutor objArticuloAutor = new ArticuloAutor();
                                objArticuloAutor.idArticulo = objeto.Articulo.idArticulo;
                                objArticuloAutor.nombres = item.Nombres;
                                objArticuloAutor.correo = item.Correo;
                                objArticuloAutor.idEstado = 1;
                                repositorioAutor.Nuevo(objArticuloAutor);
                                unidadTrabajoAutor.Commit();
                            }
                        }
                    }

                    if (objeto.ListaConclusionRepeatablePoco != null)
                    {
                        foreach (var item in objeto.ListaConclusionRepeatablePoco)
                        {
                            if (item.Operacion == "N")
                            {
                                ArticuloConclusion objArticuloConclusion = new ArticuloConclusion();
                                objArticuloConclusion.idArticulo = objeto.Articulo.idArticulo;
                                objArticuloConclusion.descripcion = item.Descripcion;
                                objArticuloConclusion.idEstado = 1;
                                repositorioConclusion.Nuevo(objArticuloConclusion);
                                unidadTrabajoConclusion.Commit();
                            }
                        }
                    }

                    if (objeto.ListaReferenciaRepeatablePoco != null)
                    {
                        foreach (var item in objeto.ListaReferenciaRepeatablePoco)
                        {
                            if (item.Operacion == "N")
                            {
                                ArticuloReferencia objArticuloReferencia = new ArticuloReferencia();
                                objArticuloReferencia.idArticulo = objeto.Articulo.idArticulo;
                                objArticuloReferencia.referencia = item.Referencia;
                                objArticuloReferencia.idEstado = 1;
                                repositorioReferencia.Nuevo(objArticuloReferencia);
                                unidadTrabajoReferencia.Commit();
                            }
                        }
                    }

                    if (objeto.ListaAdjunto != null)
                    {
                        foreach (var item in objeto.ListaAdjunto)
                        {
                            item.idArticulo = objeto.Articulo.idArticulo;
                            repositorioAdjunto.Nuevo(item);
                            unidadTrabajoAdjunto.Commit();

                        }
                    }

                    scope.Complete();
                    objResTransaccion.codigo = objeto.Articulo.idArticulo;

                }
                catch (Exception ex)
                {
                    objResTransaccion.codigo = -1;
                    objResTransaccion.mensaje = ex.Message.ToString();
                }

                return objResTransaccion;
            }

        }

        public bool Actualizar(GestionarArticuloPoco objeto)
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
                IUnidadTrabajo unidadTrabajoAutor = repositorioAutor.Contexto as IUnidadTrabajo;
                IUnidadTrabajo unidadTrabajoReferencia = repositorioReferencia.Contexto as IUnidadTrabajo;
                IUnidadTrabajo unidadTrabajoConclusion = repositorioConclusion.Contexto as IUnidadTrabajo;
                IUnidadTrabajo unidadTrabajoAdjunto = repositorioAdjunto.Contexto as IUnidadTrabajo;
                try
                {
                    objeto.Articulo.fechaRegistro = DateTime.Now;
                    repositorio.Modificar(objeto.Articulo);
                    unidadTrabajo.Commit();

                    if (objeto.ListaAutorRepeatablePoco != null)
                    {
                        foreach (var item in objeto.ListaAutorRepeatablePoco)
                        {
                            switch (item.Operacion)
                            {
                                case "N":
                                    ArticuloAutor objArticuloAutor = new ArticuloAutor();
                                    objArticuloAutor.idArticulo = objeto.Articulo.idArticulo;
                                    objArticuloAutor.nombres = item.Nombres;
                                    objArticuloAutor.correo = item.Correo;
                                    objArticuloAutor.idEstado = 1;
                                    repositorioAutor.Nuevo(objArticuloAutor);
                                    unidadTrabajoAutor.Commit();
                                    break;
                                case "M":
                                    ArticuloAutor objMod = repositorioAutor.Buscar(new EspecificacionDirecta<ArticuloAutor>(a => a.idArticuloAutor == item.Id));
                                    objMod.nombres = item.Nombres;
                                    objMod.correo = item.Correo;
                                    repositorioAutor.Modificar(objMod);
                                    unidadTrabajoAutor.Commit();
                                    break;
                                case "E":
                                    ArticuloAutor objElim = repositorioAutor.Buscar(new EspecificacionDirecta<ArticuloAutor>(a => a.idArticuloAutor == item.Id));
                                    objElim.idEstado = 2;
                                    repositorioAutor.Modificar(objElim);
                                    unidadTrabajoAutor.Commit();
                                    break;
                            }

                        }
                    }

                    if (objeto.ListaConclusionRepeatablePoco != null)
                    {
                        foreach (var item in objeto.ListaConclusionRepeatablePoco)
                        {
                            switch (item.Operacion)
                            {
                                case "N":
                                    ArticuloConclusion objArticuloConclusion = new ArticuloConclusion();
                                    objArticuloConclusion.idArticulo = objeto.Articulo.idArticulo;
                                    objArticuloConclusion.descripcion = item.Descripcion;
                                    objArticuloConclusion.idEstado = 1;
                                    repositorioConclusion.Nuevo(objArticuloConclusion);
                                    unidadTrabajoConclusion.Commit();
                                    break;
                                case "M":
                                    ArticuloConclusion objMod = repositorioConclusion.Buscar(new EspecificacionDirecta<ArticuloConclusion>(c => c.idArticuloConclusion == item.Id));
                                    objMod.descripcion = item.Descripcion;
                                    repositorioConclusion.Modificar(objMod);
                                    unidadTrabajoConclusion.Commit();
                                    break;
                                case "E":
                                    ArticuloConclusion objDel = repositorioConclusion.Buscar(new EspecificacionDirecta<ArticuloConclusion>(c => c.idArticuloConclusion == item.Id));
                                    objDel.idEstado = 2;
                                    repositorioConclusion.Modificar(objDel);
                                    unidadTrabajoConclusion.Commit();
                                    break;
                            }

                        }
                    }

                    if (objeto.ListaReferenciaRepeatablePoco != null)
                    {
                        foreach (var item in objeto.ListaReferenciaRepeatablePoco)
                        {
                            switch (item.Operacion)
                            {
                                case "N":
                                    ArticuloReferencia objArticuloReferencia = new ArticuloReferencia();
                                    objArticuloReferencia.idArticulo = objeto.Articulo.idArticulo;
                                    objArticuloReferencia.referencia = item.Referencia;
                                    objArticuloReferencia.idEstado = 1;
                                    repositorioReferencia.Nuevo(objArticuloReferencia);
                                    unidadTrabajoReferencia.Commit();
                                    break;
                                case "M":
                                    ArticuloReferencia objModRef = repositorioReferencia.Buscar(new EspecificacionDirecta<ArticuloReferencia>(r => r.idArticuloReferencia == item.Id));
                                    objModRef.referencia = item.Referencia;
                                    repositorioReferencia.Modificar(objModRef);
                                    unidadTrabajoReferencia.Commit();
                                    break;
                                case "E":
                                    ArticuloReferencia objElimRef = repositorioReferencia.Buscar(new EspecificacionDirecta<ArticuloReferencia>(r => r.idArticuloReferencia == item.Id));
                                    objElimRef.idEstado = 2;
                                    repositorioReferencia.Modificar(objElimRef);
                                    unidadTrabajoReferencia.Commit();
                                    break;
                            }
                        }
                    }
                   
                    if (objeto.ListaAdjunto != null)
                    {
                        foreach (var item in objeto.ListaAdjunto)
                        {
                            item.idArticulo = objeto.Articulo.idArticulo;
                            repositorioAdjunto.Nuevo(item);
                            unidadTrabajoAdjunto.Commit();

                        }
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




        public bool Eliminar(int idArticulo)
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

                try
                {
                    Articulo objArticulo = repositorio.Buscar(new EspecificacionDirecta<Articulo>(s => s.idArticulo == idArticulo));
                    objArticulo.idEstado = 2;
                    repositorio.Modificar(objArticulo);
                    unidadTrabajo.Commit();

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

        public bool EliminarAdjunto(int idArticulo)
        {
            TransactionOptions Tranconfiguracion = new TransactionOptions()
            {
                Timeout = TransactionManager.MaximumTimeout,
                IsolationLevel = IsolationLevel.Serializable
            };

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.RequiresNew, Tranconfiguracion))
            {
                bool resultado = false;

                IUnidadTrabajo unidadTrabajoAdjunto = repositorioAdjunto.Contexto as IUnidadTrabajo;

                try
                {
                    ArticuloAdjunto objArticuloAd = repositorioAdjunto.Buscar(new EspecificacionDirecta<ArticuloAdjunto>(s => s.idArticulo == idArticulo));
                   
                    repositorioAdjunto.Eliminar(objArticuloAd);
                    unidadTrabajoAdjunto.Commit();

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

        public bool Publicar(int idArticulo)
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

                try
                {
                    Articulo objArticulo = repositorio.Buscar(new EspecificacionDirecta<Articulo>(s => s.idArticulo == idArticulo));
                    objArticulo.idEstado = 3;
                    repositorio.Modificar(objArticulo);
                    unidadTrabajo.Commit();

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


        public bool NoPublicar(int idArticulo)
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

                try
                {
                    Articulo objArticulo = repositorio.Buscar(new EspecificacionDirecta<Articulo>(s => s.idArticulo == idArticulo));
                    objArticulo.idEstado = 1;
                    repositorio.Modificar(objArticulo);
                    unidadTrabajo.Commit();

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

        public List<sp_ListarArticulosxAutor_Result> ListarxAutor(int idAutor)
        {
            return repositorio.ListarArticulosxAutor(idAutor);
        }

        public List<sp_ListarPublicaciones_Result> ListarPublicaciones()
        {
            return repositorio.ListarPublicaciones();
        }

        public List<ArticuloAutor> ListarAutor(int idArticulo)
        {
            return repositorioAutor.TraerElementoFiltrado(s => s.idArticulo == idArticulo && s.idEstado == 1);
        }

        public List<ArticuloConclusion> ListarConclusion(int idArticulo)
        {
            return repositorioConclusion.TraerElementoFiltrado(s => s.idArticulo == idArticulo && s.idEstado == 1);
        }

        public List<ArticuloReferencia> ListarReferencia(int idArticulo)
        {
            return repositorioReferencia.TraerElementoFiltrado(s => s.idArticulo == idArticulo && s.idEstado == 1);
        }

        public List<ArticuloAdjunto> ListarAdjunto(int idArticulo)
        {
            return repositorioAdjunto.TraerElementoFiltrado(e => e.idArticulo == idArticulo);
        }
    }
}
