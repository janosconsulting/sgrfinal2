using Mantenimiento.Datos.Entidades;
using Mantenimiento.Datos.Modelo;
using Mantenimiento.Datos.Recursos;
using System;
using System.Collections.Generic;
using System.Data.Objects;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using Utilitario.Especificacion;
using Utilitario.Extensiones;
using Utilitario.Log;


namespace Mantenimiento.Datos.Base
{
    public class Repositorio<TEntidad> : IRepositorio<TEntidad> where TEntidad : class, IObjectWithChangeTracker
    {
        //Declaracion de variables privadas para la administracion de ocurrencias y centralizar el trabajo(Patrón UNIDAD DE TRABAJO)
        private IOcurrencia _AdministradorOcurrencia = null;
        private IContexto _ActualContexto;

        //Elementos públicos
        public IUnidadTrabajo Contexto
        {
            get
            {
                return _ActualContexto as IUnidadTrabajo;
            }
        }

        //Solo trabajo con las interfaces que serán reeemplazados por el DI del UNITY
        public Repositorio(IContexto Contexto, IOcurrencia administradorocurrencia)
        {
            //Verificar condiciones
            if (Contexto == (IContexto)null)
                throw new ArgumentNullException("Transaccion", Mensaje.excepcion_ContenedorNoPuedeSerNull);

            if (administradorocurrencia == (IOcurrencia)null)
                throw new ArgumentNullException("AdministradorOcurrencia", Mensaje.excepcion_AdministradorDeSeguimientoNoPuedeSerNull);

            //Ingresa valores
            _ActualContexto = Contexto;
            _AdministradorOcurrencia = administradorocurrencia;

            _AdministradorOcurrencia.InformacionOcurrencia(string.Format(CultureInfo.InvariantCulture, Mensaje.traza_ConstruirRepositorio, typeof(TEntidad).Name));
        }

        public void Nuevo(TEntidad Objeto)
        {
            //Verificamos Objeto
            if (Objeto == (TEntidad)null)
                throw new ArgumentNullException("objeto", Mensaje.excepcion_ArgumentoElementoInvalido);

            if (Objeto.ChangeTracker != null && Objeto.ChangeTracker.State == ObjectState.Added)
            {
                _ActualContexto.RegistrarCambios<TEntidad>(Objeto);
                _AdministradorOcurrencia.InformacionOcurrencia(string.Format(CultureInfo.InvariantCulture, Mensaje.traza_AgregarElementoRepositorio, typeof(TEntidad).Name));
            }
            else
                throw new InvalidOperationException(Mensaje.excepcion_seguidordeCambio_esNulooEstadoNoEsCorrecto);
        }

        public void Modificar(TEntidad Objeto)
        {
            //Verificamos Objeto
            if (Objeto == (TEntidad)null)
                throw new ArgumentNullException("objeto", Mensaje.excepcion_ArgumentoElementoInvalido);

            //establecer modificacion de estado si 
            if (Objeto.ChangeTracker != null && ((Objeto.ChangeTracker.State & ObjectState.Deleted) != ObjectState.Deleted))
            {
                Objeto.MarkAsModified();
            }
            //Aplicar cambios 
            _ActualContexto.RegistrarCambios(Objeto);
            _AdministradorOcurrencia.InformacionOcurrencia(string.Format(CultureInfo.InvariantCulture, Mensaje.traza_AplicarCambiosElementoRepositorio, typeof(TEntidad).Name));
        }

        public void Eliminar(TEntidad Objeto)
        {
            //Verificar Objeto
            if (Objeto == (TEntidad)null)
                throw new ArgumentNullException("objeto", Mensaje.excepcion_ArgumentoElementoInvalido);


            IObjectSet<TEntidad> ObjetoConjunto = _ActualContexto.CrearConjunto<TEntidad>();

            //Establecer la opcion de combinacion fundamental del ObjetoConjunto
            ObjectQuery<TEntidad> Consulta = ObjetoConjunto as ObjectQuery<TEntidad>;

            if (Consulta != null) // Verifica si ese objeto no esta en memoria para pruebas
                Consulta.MergeOption = MergeOption.NoTracking;

            IObjectSet<TEntidad> objectSet = ObjetoConjunto;//CrearConjunto();

            //Eliminar el objeto y agregar este
            // Esto es solo valido si el TEntidad(Objeto) es un tipo en el modelo
            objectSet.Attach(Objeto);

            //Eliminar Objeto para esto
            objectSet.DeleteObject(Objeto);

            _AdministradorOcurrencia.InformacionOcurrencia(string.Format(CultureInfo.InvariantCulture, Mensaje.traza_EliminarElementoRepositorio, typeof(TEntidad).Name));
        }

        private IObjectSet<TEntidad> CrearConjunto()
        {
            if (_ActualContexto != (IUnidadTrabajo)null)
            {
                IObjectSet<TEntidad> ObjetoConjunto = _ActualContexto.CrearConjunto<TEntidad>();

                //Establecer la opcion de combinacion fundamenteal del ObjetoConjunto

                ObjectQuery<TEntidad> Consulta = ObjetoConjunto as ObjectQuery<TEntidad>;

                if (Consulta != null) // Verifica si ese objeto no esta en memoria para pruebas
                    Consulta.MergeOption = MergeOption.NoTracking;

                return ObjetoConjunto;
            }
            else
                throw new InvalidOperationException(Mensaje.excepcion_ContenedorNoPuedeSerNull);
        }

        public void RegistrarElemento(TEntidad Objeto)
        {
            if (Objeto == (TEntidad)null)
                throw new ArgumentNullException("objeto");

            (CrearConjunto()).Attach(Objeto);

            _AdministradorOcurrencia.InformacionOcurrencia(string.Format(CultureInfo.InvariantCulture, Mensaje.traza_ActualizarElementoaRepositorio, typeof(TEntidad).Name));
        }

        public List<TEntidad> TraerTodos()
        {
            _AdministradorOcurrencia.InformacionOcurrencia(string.Format(CultureInfo.InvariantCulture, Mensaje.traza_TraerTodosRepositorio, typeof(TEntidad).Name));

            //Crear IObjectSet y realizar consulta
            return (CrearConjunto()).ToList<TEntidad>();
        }

        public List<TEntidad> TraerTodos<S>(Expression<Func<TEntidad, S>> ordenarPorExpresion, bool ascendente)
        {
            _AdministradorOcurrencia.InformacionOcurrencia(string.Format(CultureInfo.InvariantCulture, Mensaje.traza_TraerTodosRepositorio, typeof(TEntidad).Name));

            //Crear IObjectSet y realizar consulta
            return (ascendente) ?
                (CrearConjunto())
                .OrderBy(ordenarPorExpresion)
                .ToList<TEntidad>()
                :
                (CrearConjunto())
                .OrderByDescending(ordenarPorExpresion)
                .ToList<TEntidad>();
        }

        public List<TEntidad> TraerPorEspecificacion(IEspecificacion<TEntidad> especificacion)
        {
            if (especificacion == (IEspecificacion<TEntidad>)null)
                throw new ArgumentNullException("Especificacion");

            _AdministradorOcurrencia.InformacionOcurrencia(string.Format(CultureInfo.InvariantCulture, Mensaje.traza_TraerPorEspecificacion, typeof(TEntidad).Name));

            return (CrearConjunto().Where(especificacion.SatisfechoPor()).ToList<TEntidad>());
        }

        public List<TEntidad> TraerPorEspecificacion<S>(IEspecificacion<TEntidad> especificacion, Expression<Func<TEntidad, S>> ordenarPorExpresion, bool ascendente)
        {
            if (especificacion == (IEspecificacion<TEntidad>)null)
                throw new ArgumentNullException("Especificacion");

            _AdministradorOcurrencia.InformacionOcurrencia(string.Format(CultureInfo.InvariantCulture, Mensaje.traza_TraerPorEspecificacion, typeof(TEntidad).Name));

            return (ascendente) ?
                    (CrearConjunto()
                    .Where(especificacion.SatisfechoPor())
                    .OrderBy(ordenarPorExpresion)
                    .ToList<TEntidad>())

                    :

                    (CrearConjunto()
                    .Where(especificacion.SatisfechoPor())
                    .OrderByDescending(ordenarPorExpresion)
                    .ToList<TEntidad>());
        }

        public TEntidad Buscar(IEspecificacion<TEntidad> especificacion)
        {
            if (especificacion == (IEspecificacion<TEntidad>)null)
                throw new ArgumentNullException("Especificacion");

            _AdministradorOcurrencia.InformacionOcurrencia(string.Format(CultureInfo.InvariantCulture, Mensaje.traza_BuscarPorEspecificacion, typeof(TEntidad).Name));

            return (CrearConjunto().Where(especificacion.SatisfechoPor()).FirstOrDefault<TEntidad>());
        }

        public List<TEntidad> TraerElementoFiltrado(Expression<Func<TEntidad, bool>> filtro)
        {
            //Verificando argumentos de la consulta
            if (filtro == (Expression<Func<TEntidad, bool>>)null)
                throw new ArgumentNullException("filtro", Mensaje.excepcion_FilTroNoPuedeSerNull);

            _AdministradorOcurrencia.InformacionOcurrencia(string.Format(CultureInfo.InvariantCulture, Mensaje.traza_TraerFiltradoElementoRepositorio, typeof(TEntidad).Name, filtro.ToString()));

            //Crear IObjectSet y realizar consulta
            return CrearConjunto().Where(filtro).ToList();
        }

        public List<TEntidad> TraerElementoFiltrado<S>(Expression<Func<TEntidad, bool>> filtro, int IndicePagina, int CantidadPagina, Expression<Func<TEntidad, S>> ordenarPorExpresion, bool ascendente)
        {
            //Verificando argumentos de la consulta
            if (filtro == (Expression<Func<TEntidad, bool>>)null)
                throw new ArgumentNullException("filtro", Mensaje.excepcion_FilTroNoPuedeSerNull);

            if (IndicePagina < 0)
                throw new ArgumentException(Mensaje.excepcion_IndicePaginaInvalida, "IndicePagina");

            if (CantidadPagina <= 0)
                throw new ArgumentException(Mensaje.excepcion_CantidadPaginaInvalida, "CantidadPagina");

            if (ordenarPorExpresion == (Expression<Func<TEntidad, S>>)null)
                throw new ArgumentNullException("OrdenarPorExpresion", Mensaje.excepcion_OrdenarPorExpresionNoPuedeSerNull);

            _AdministradorOcurrencia.InformacionOcurrencia(string.Format(CultureInfo.InvariantCulture,
                                        Mensaje.traza_TraerElementoFiltradoPaginadoRepositorio,
                                        typeof(TEntidad).Name, filtro.ToString(), IndicePagina, CantidadPagina, ordenarPorExpresion.ToString()));

            //Crear IObjectSet para este tipo y realizar consulta
            IObjectSet<TEntidad> objectSet = CrearConjunto();

            return (ascendente)
                                ?
                                    objectSet
                                     .Where(filtro)
                                     .OrderBy(ordenarPorExpresion)

                                     .Take(CantidadPagina)
                                     .ToList()
                                :
                                    objectSet
                                     .Where(filtro)
                                     .OrderByDescending(ordenarPorExpresion)

                                     .Take(CantidadPagina)
                                     .ToList();
        }

        public List<TEntidad> TraerElementoFiltrado<S>(Expression<Func<TEntidad, bool>> filtro, Expression<Func<TEntidad, S>> ordenarPorExpresion, bool ascendente)
        {
            //Verificando argumentos de la consulta
            if (filtro == (Expression<Func<TEntidad, bool>>)null)
                throw new ArgumentNullException("filtro", Mensaje.excepcion_FilTroNoPuedeSerNull);

            if (ordenarPorExpresion == (Expression<Func<TEntidad, S>>)null)
                throw new ArgumentNullException("OrdenarPorExpresion", Mensaje.excepcion_OrdenarPorExpresionNoPuedeSerNull);

            _AdministradorOcurrencia.InformacionOcurrencia(
                           string.Format(CultureInfo.InvariantCulture,
                                        Mensaje.traza_TraerFiltradoElementoRepositorio,
                                        typeof(TEntidad).Name, filtro.ToString()));

            //Crear IObjectSet para este tipo y realizar consulta
            IObjectSet<TEntidad> objectSet = CrearConjunto();

            return (ascendente)
                                ?
                                    objectSet.Where(filtro).OrderBy(ordenarPorExpresion).ToList()
                                :
                                    objectSet.Where(filtro).OrderByDescending(ordenarPorExpresion).ToList();
        }

        public List<TEntidad> TraerElementosPaginados<S>(int IndicePagina, int CantidadPagina, Expression<Func<TEntidad, S>> ordenarPorExpresion, IEspecificacion<TEntidad> especificacion, bool ascendente)
        {
            //Verificando argumentos de la consulta
            if (IndicePagina < 0)
                throw new ArgumentException(Mensaje.excepcion_IndicePaginaInvalida, "IndicePagina");

            if (CantidadPagina <= 0)
                throw new ArgumentException(Mensaje.excepcion_CantidadPaginaInvalida, "CantidadPagina");

            if (ordenarPorExpresion == (Expression<Func<TEntidad, S>>)null)
                throw new ArgumentNullException("ordenarPorExpresion", Mensaje.excepcion_OrdenarPorExpresionNoPuedeSerNull);

            if (especificacion == (IEspecificacion<TEntidad>)null)
                throw new ArgumentNullException("especificacion", Mensaje.excepcion_EspecificacionEsNull);

            _AdministradorOcurrencia.InformacionOcurrencia(
                           string.Format(CultureInfo.InvariantCulture,
                                        Mensaje.traza_TraerPaginadoElementoRepositorio,
                                        typeof(TEntidad).Name, IndicePagina, CantidadPagina, ordenarPorExpresion.ToString()));

            //Crear IObjectSet para este tipo y realizar consulta

            IObjectSet<TEntidad> objectSet = CrearConjunto();

            //Este metodo no puede utlizar paginado de  IQueryable extension metodo porque queries no puede ser
            //fusionada con el metodo contructor del Objeto. Mirar documentacion del EF

            return (ascendente)
                                ?
                                    objectSet
                                     .Where(especificacion.SatisfechoPor())
                                     .OrderBy(ordenarPorExpresion)
                                     .Skip(IndicePagina * CantidadPagina)
                                     .Take(CantidadPagina)
                                     .ToList()
                                :
                                    objectSet
                                     .Where(especificacion.SatisfechoPor())
                                     .OrderByDescending(ordenarPorExpresion)
                                     .Skip(IndicePagina * CantidadPagina)
                                     .Take(CantidadPagina)
                                     .ToList();
        }

        public List<TEntidad> TraerElementosPaginados<S>(int IndicePagina, int CantidadPagina, Expression<Func<TEntidad, S>> ordenarPorExpresion, bool ascendente)
        {
            //Verificando argumentos de la consulta
            if (IndicePagina < 0)
                throw new ArgumentException(Mensaje.excepcion_IndicePaginaInvalida, "IndicePagina");

            if (CantidadPagina <= 0)
                throw new ArgumentException(Mensaje.excepcion_CantidadPaginaInvalida, "CantidadPagina");

            if (ordenarPorExpresion == (Expression<Func<TEntidad, S>>)null)
                throw new ArgumentNullException("ordenarPorExpresion", Mensaje.excepcion_OrdenarPorExpresionNoPuedeSerNull);

            _AdministradorOcurrencia.InformacionOcurrencia(
                           string.Format(CultureInfo.InvariantCulture,
                                        Mensaje.traza_TraerPaginadoElementoRepositorio,
                                        typeof(TEntidad).Name, IndicePagina, CantidadPagina, ordenarPorExpresion.ToString()));

            ////Crear asociaciones de IObjectSet y realizar consulta

            IObjectSet<TEntidad> objectSet = CrearConjunto();

            return objectSet.Paginado<TEntidad, S>(ordenarPorExpresion, IndicePagina, CantidadPagina, ascendente).ToList();
            //throw new NotImplementedException();
        }

    }
}
