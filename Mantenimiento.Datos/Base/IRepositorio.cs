

using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Utilitario.Especificacion;

namespace Mantenimiento.Datos.Base
{
    public interface IRepositorio<TEntidad> where TEntidad : class
    {
        /// <summary>
        /// Conseguir la ITransaccion de este repositorio 
        /// </summary>
        IUnidadTrabajo Contexto { get; }

        /// <summary>
        /// Agregar nuevo elemento al repositorio
        /// </summary>
        /// <param name="Objeto">Elemento para agregar al repositorio</param>
        void Nuevo(TEntidad Objeto);

        /// <summary>
        /// Establece modificado en el repositorio.
        /// Cuando se llama al metodo Commit() en Unidad de Trabajo
        /// estos cambios son guardados en los repositorios
        /// <remarks>
        /// Internamente este metodo invoca a los metodos Repositorio.Attach() y Context.SetChanges()
        /// </remarks>
        /// <param name="item">Objeto con cambios</param>
        void Modificar(TEntidad Objeto);

        /// <summary>
        /// Eliminar elemento al repositorio
        /// </summary>
        /// <param name="Objeto">Elemento a eliminar del repositorio</param>
        void Eliminar(TEntidad Objeto);

        /// <summary>
        ///Registra la entidad en este repositorio, sobre la Unidad de Trabajo
        ///En Entity framework puede ser hecho con Attach y con Update in NH
        /// </summary>
        /// <param name="Objeto">Elemento a adjuntar</param>
        void RegistrarElemento(TEntidad Objeto);

        /// <summary>
        /// Traer todos los elementos del tipo {TEntidad} en el repositorio
        /// </summary>
        /// <returns>Lista de elementos seleccionados</returns>
        List<TEntidad> TraerTodos();

        /// <summary>
        /// Traer todos los elementos del tipo {TEntidad} en el repositorio
        /// </summary>
        /// <param name="ordenarPorExpresion">expresión de orden</param>
        /// <param name="ascendente">indica si el orden es ascendente o descendente</param>
        /// <returns>Lista de elementos seleccionados</returns>
        List<TEntidad> TraerTodos<S>(Expression<Func<TEntidad, S>> ordenarPorExpresion, bool ascendente);

        /// <summary>
        /// Traer todos los elementos del tipo {TEntidad} en el repositorio por la especificación dada
        /// <paramref name="Especificacion"/>
        /// </summary>
        /// <param name="especificacion">Especificacion que produce el resultado</param>
        /// <returns></returns>
        List<TEntidad> TraerPorEspecificacion(IEspecificacion<TEntidad> especificacion);

        /// <summary>
        /// Traer todos los elementos del tipo {TEntidad} en el repositorio por la especificación dada
        /// <paramref name="Especificacion"/>
        /// </summary>
        /// <param name="especificacion">Especificacion que produce el resultado</param>
        /// <param name="ordenarPorExpresion">expresión de orden</param>
        /// <param name="ascendente">indica si el orden es ascendente o descendente</param>
        /// <returns></returns>
        List<TEntidad> TraerPorEspecificacion<S>(IEspecificacion<TEntidad> especificacion, Expression<Func<TEntidad, S>> ordenarPorExpresion, bool ascendente);

        /// <summary>
        /// Busca el primer elemento del tipo {TEntidad} en el repositorio por la especificación dada
        /// <paramref name="Especificacion"/>
        /// </summary>
        /// <param name="especificacion">Especificacion que produce el resultado</param>
        /// <returns></returns>
        TEntidad Buscar(IEspecificacion<TEntidad> especificacion);

        /// <summary>
        /// Traer todos los elementos del tipo {TEntidad} en el repositorio
        /// </summary>
        /// <param name="filtro">Filtrar elementos por la expresión</param>
        /// <returns>Lista de elementos</returns>
        List<TEntidad> TraerElementoFiltrado(Expression<Func<TEntidad, bool>> filtro);

        /// <summary>
        /// Traer todos los elementos del tipo {TEntidad} en el repositorio
        /// </summary>
        /// <param name="filtro">Filtrar elementos por la expresión</param>
        /// <param name="IndicePagina">Indice de página</param>
        /// <param name="CantidadPagina">Número de elementos en cada página</param>
        /// <param name="ordenarPorExpresion">Ordenar por la expresión dada para esta consulta</param>
        /// <param name="ascendente">Especificar si el resultado va ser en orden ascendente</param>
        /// <returns>Lista de elementos seleccionados</returns>
        List<TEntidad> TraerElementoFiltrado<S>(Expression<Func<TEntidad, bool>> filtro, int IndicePagina, int CantidadPagina, Expression<Func<TEntidad, S>> ordenarPorExpresion, bool ascendente);

        /// <summary>
        /// Traer todos los elementos del tipo {TEntidad} en el repositorio
        /// </summary>
        /// /// <param name="filtro">Filtrar elementos por la expresión</param>
        /// <param name="ordenarPorExpresion">Ordenar por la expresión dada para esta consulta</param>
        /// <param name="ascendente">Especificar si el resultado va ser en orden ascendente</param>
        /// <returns>Lista de elementos seleccionados</returns>
        List<TEntidad> TraerElementoFiltrado<S>(Expression<Func<TEntidad, bool>> filtro, Expression<Func<TEntidad, S>> ordenarPorExpresion, bool ascendente);

        /// <summary>
        /// Traer todos los elementos del tipo {TEntidad} en el repositorio
        /// </summary>
        /// <param name="IndicePagina">Indice de página</param>
        /// <param name="CantidadPagina">Número de elementos en cada página</param>
        /// <param name="ordenarPorExpresion">Ordenar por la expresión dada para esta consulta</param>
        /// <param name="ascendente">Especificar si el resultado va ser en orden ascendente</param>
        /// <returns>Lista de elementos seleccionados</returns>
        List<TEntidad> TraerElementosPaginados<S>(int IndicePagina, int CantidadPagina, Expression<Func<TEntidad, S>> ordenarPorExpresion, IEspecificacion<TEntidad> especificacion, bool ascendente);

        /// <summary>
        /// Traer todos los elementos del tipo {TEntidad} en el repositorio
        /// </summary>
        /// <param name="IndicePagina">Indice de página</param>
        /// <param name="CantidadPagina">Número de elementos en cada página</param>
        /// <param name="ordenarPorExpresion">Ordenar por la expresión dada para esta consulta</param>
        /// <param name="ascendente">Especificar si el resultado va ser en orden ascendente</param>
        /// <returns>Lista de elementos seleccionados</returns>
        List<TEntidad> TraerElementosPaginados<S>(int IndicePagina, int CantidadPagina, Expression<Func<TEntidad, S>> ordenarPorExpresion, bool ascendente);

    }
}
