using Mantenimiento.Datos.Contratos;
using Mantenimiento.Datos.Modelo;
using Mantenimiento.Datos.Repositorios;
using Mantenimiento.Negocio.Contratos.Servicios;
using Mantenimiento.Negocio.Servicios;
using System;
using System.Collections.Generic;
using System.Configuration;
//using Mantenimiento.Negocio.Contratos.Servicios;
//using Mantenimiento.Negocio.Servicios;

//using SIA_Presupuesto.Datos.Repositorio;
using Unity;
using Unity.Injection;
using Unity.Lifetime;
using Utilitario.Log;


//using Mantenimiento.Negocio.Contratos.Repositorios;
//using Mantenimiento.Datos.Repositorios;

namespace Mantenimiento.IoC.Contenedor
{

    public class cContenedor : IContenedor
    {
        #region Miembros

        IDictionary<string, IUnityContainer> _DiccionarioContenedores;

        #endregion

        #region Constructor
        public cContenedor()
        {
            _DiccionarioContenedores = new Dictionary<string, IUnityContainer>();

            IUnityContainer ContenedorRaiz = new UnityContainer();

            //Crear contenedor raiz
            //IUnityContainer ContenedorRaiz = ContenedorRaiz;// new UnityContainer();
            _DiccionarioContenedores.Add("ContextoRaiz", ContenedorRaiz);

            //Crear contenedor para contexto real, hijo de contenedor raiz
            //IUnityContainer realContenedor = ContenedorRaiz.CreateChildContainer();
            //_DiccionarioContenedores.Add("RealContenedor", realContenedor);

            ////Crear contenedor para testeo, hijo de contenedor raiz
            //IUnityContainer FalsoContenedor = ContenedorRaiz.CreateChildContainer();
            //_DiccionarioContenedores.Add("FalsoContenedor", FalsoContenedor);

            ConfiguracionContenedorRaiz(ContenedorRaiz);
            ConfiguracionContedorReal(ContenedorRaiz);
            ConfiguracionFalsoContenedor(ContenedorRaiz);
        }

        public cContenedor(IUnityContainer ContenedorRaiz)
        {
            _DiccionarioContenedores = new Dictionary<string, IUnityContainer>();

            //Crear contenedor raiz
            //IUnityContainer ContenedorRaiz = ContenedorRaiz;// new UnityContainer();
            _DiccionarioContenedores.Add("ContextoRaiz", ContenedorRaiz);

            //Crear contenedor para contexto real, hijo de contenedor raiz
            //IUnityContainer realContenedor = ContenedorRaiz.CreateChildContainer();
            //_DiccionarioContenedores.Add("RealContenedor", realContenedor);

            ////Crear contenedor para testeo, hijo de contenedor raiz
            //IUnityContainer FalsoContenedor = ContenedorRaiz.CreateChildContainer();
            //_DiccionarioContenedores.Add("FalsoContenedor", FalsoContenedor);

            ConfiguracionContenedorRaiz(ContenedorRaiz);
            ConfiguracionContedorReal(ContenedorRaiz);
            ConfiguracionFalsoContenedor(ContenedorRaiz);
        }

        private void ConfiguracionContedorReal(IUnityContainer contenedor)
        {
            contenedor.RegisterType<IContexto, EDBCORAHEntities>(new AdministracionTiempoVidaPorEjecucionContexto(), new InjectionConstructor());
            //contenedor.RegisterType<IFabricaBaseDatos, FabricaBaseDatos>(new AdministracionTiempoVidaPorEjecucionContexto(), new InjectionConstructor());
        }

        private void ConfiguracionFalsoContenedor(IUnityContainer contenedor)
        {

        }

        #endregion

        private void ConfiguracionContenedorRaiz(IUnityContainer contenedor)
        {
            
            contenedor.RegisterType<IUsuarioRepositorio, UsuarioRepositorio>(new TransientLifetimeManager());
            
            contenedor.RegisterType<IAutorRepositorio, AutorRepositorio>(new TransientLifetimeManager());
            contenedor.RegisterType<IArticuloRepositorio, ArticuloRepositorio>(new TransientLifetimeManager());
            contenedor.RegisterType<IArticuloAutorRepositorio, ArticuloAutorRepositorio>(new TransientLifetimeManager());
            contenedor.RegisterType<IArticuloConclusionRepositorio, ArticuloConclusionRepositorio>(new TransientLifetimeManager());
            contenedor.RegisterType<IArticuloReferenciaRepositorio, ArticuloReferenciaRepositorio>(new TransientLifetimeManager());
            contenedor.RegisterType<IDocumentoIdentidadRepositorio, DocumentoIdentidadRepositorio>(new TransientLifetimeManager());
            contenedor.RegisterType<IArticuloAdjuntoRepositorio, ArticuloAdjuntoRepositorio>(new TransientLifetimeManager());           
            contenedor.RegisterType<IUsuarioServicio, UsuarioServicio>(new TransientLifetimeManager());

            contenedor.RegisterType<IRequerimientoServicio, RequerimientoServicio>(new TransientLifetimeManager());
            contenedor.RegisterType<IPersonaServicio, PersonaServicio>(new TransientLifetimeManager());
            contenedor.RegisterType<IProyectoServicio, ProyectoServicio>(new TransientLifetimeManager());
            contenedor.RegisterType<ITipoRequerimientoServicio, TipoRequerimientoServicio>(new TransientLifetimeManager());
            contenedor.RegisterType<IDetalleRequerimiento, DetalleRequerimientoServicio>(new TransientLifetimeManager());
            contenedor.RegisterType<IRequerimientosporTrabajadorServicio, RequerimientosporTrabajadorServicio>(new TransientLifetimeManager());

            //Registro de la capa transversal
            contenedor.RegisterType<IOcurrencia, cOcurrencia>(new TransientLifetimeManager());
        }

        private void ConfiguracionContedorServidor(IUnityContainer contenedor)
        {
            //contenedor.RegisterType<IContextoBase, ContextoBase>(new TransientLifetimeManager());
            //contenedor.RegisterType<IContexto, SIACORAHEntities>(new AdministracionTiempoVidaPorEjecucionContexto(), new InjectionConstructor());
            //contenedor.RegisterType<IContexto, SIACORAHEntities>(new TransientLifetimeManager());
            //contenedor.RegisterType<IFabricaBaseDatos, FabricaBaseDatos>(new AdministracionTiempoVidaPorEjecucionContexto(), new InjectionConstructor());
        }

        private void ConfiguracionContenedorCliente(IUnityContainer contenedor)
        {
            //contenedor.RegisterType<IContexto, SIACORAHLOCALEntities>(new AdministracionTiempoVidaPorEjecucionContexto(), new InjectionConstructor());
            //contenedor.RegisterType<IFabricaBaseDatos, FabricaBaseDatos>(new AdministracionTiempoVidaPorEjecucionContexto(), new InjectionConstructor());
        }

        #region Miembros IFabricaServicios

        public TServicio Resolver<TServicio>()
        {
            //Nosotros usamos el contenedor predeterminado especificado en el AppSettings
            string NombreContenedor = ConfigurationManager.AppSettings["PredeterminadoContenedorIoC"];

            if (String.IsNullOrEmpty(NombreContenedor) || String.IsNullOrWhiteSpace(NombreContenedor))
            {
                throw new ArgumentNullException();
            }

            if (!_DiccionarioContenedores.ContainsKey(NombreContenedor))
                throw new InvalidOperationException();

            IUnityContainer contenedor = _DiccionarioContenedores[NombreContenedor];

            return contenedor.Resolve<TServicio>();
        }

        public object Resolver(Type tipo)
        {
            //Nosotros usamos el contenedor predeterminado especificado en el AppSettings
            string NombreContenedor = ConfigurationManager.AppSettings["PredeterminadoContenedorIoC"];

            if (String.IsNullOrEmpty(NombreContenedor) || String.IsNullOrWhiteSpace(NombreContenedor))
            {
                throw new ArgumentNullException();
            }

            if (!_DiccionarioContenedores.ContainsKey(NombreContenedor))
                throw new InvalidOperationException();

            IUnityContainer contenedor = _DiccionarioContenedores[NombreContenedor];

            return contenedor.Resolve(tipo, null);
        }

        public void RegistrarTipo(Type tipo)
        {
            IUnityContainer contenedor = this._DiccionarioContenedores["ContextoRaiz"];

            if (contenedor != null)
                contenedor.RegisterType(tipo, new TransientLifetimeManager());
        }

        public IUnityContainer EntregarContenedor()
        {
            IUnityContainer contenedor = this._DiccionarioContenedores["ContextoRaiz"];
            return contenedor;
        }

        #endregion

    }
}
