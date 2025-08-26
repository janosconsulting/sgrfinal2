using System;
using System.Runtime.Remoting.Messaging;
using System.ServiceModel;
using Unity.Lifetime;

namespace Mantenimiento.IoC.Contenedor
{
    /// <summary>
    /// Clase que permite la administracion del tiempor de vida de los diferentes contexto por ejecucion
    /// Permite la personalización para el trabajo con diferentes entornos sobre la capa de presentación 
    /// WCF, ASP .NET, etc
    /// En caso de no realizar la personalizacion, ni la extensión  es suficiente instanciar la clase LifetimeManager
    /// </summary>
    sealed class AdministracionTiempoVidaPorEjecucionContexto : LifetimeManager
    {
        /// <summary>
        /// Extension personalizada para OperationContext alcance
        /// </summary>
        class ExtensionContenedor : IExtension<OperationContext>
        {
            #region Members

            public object Valor { get; set; }

            #endregion

            #region Mienbros IExtension<OperationContext> 

            //UNIR
            public void Attach(OperationContext propietario)
            {

            }

            //SEPARAR
            public void Detach(OperationContext propietario)
            {

            }

            #endregion
        }

        #region Mienbros

        Guid _Llave;

        #endregion


        #region Constructor

        /// <summary>
        /// Constructor predeterminado
        /// </summary>
        public AdministracionTiempoVidaPorEjecucionContexto() : this(Guid.NewGuid()) { }

        /// <summary>
        ///  Constructor
        /// </summary>
        /// <param name="llave">Una llave para este resolucion del Administrador </param>
        AdministracionTiempoVidaPorEjecucionContexto(Guid llave)
        {
            if (llave == Guid.Empty)
                throw new ArgumentException("");

            _Llave = llave;
        }

        #endregion

        #region ILifetimeManager Members

        /// <summary>
        /// <see cref="M:Microsoft.Practices.Unity.LifetimeManager.GetValue"/>
        /// </summary>
        /// <returns><see cref="M:Microsoft.Practices.Unity.LifetimeManager.GetValue"/></returns>

        //Permite conseguir el valor de la Interfaz ILifetimeManager
        public override object GetValue(ILifetimeContainer container = null)
        {
            object resultado = null;

            //Conseguir objeto sobre el contexto de una ejecución ( WCF sin HttpContext,HttpContext o CallContext)

            if (OperationContext.Current != null)
            {
                //WCF sin entorno HttpContext
                ExtensionContenedor extencionContenedor = OperationContext.Current.Extensions.Find<ExtensionContenedor>();
                if (extencionContenedor != null)
                {
                    resultado = extencionContenedor.Valor;
                }
            }
            //else if (HttpContext.Current != null)
            //{
            //    //HttpContext disponible ( ASP.NET ..)
            //    if (HttpContext.Current.Items[_Llave.ToString()] != null)
            //        resultado = HttpContext.Current.Items[_Llave.ToString()];
            //}
            else
            {
                //no sobre WCF o ASP.NET Environment, UnitTesting, WinForms, WPF etc.
                resultado = CallContext.GetData(_Llave.ToString());
            }

            return resultado;
        }
        /// <summary>
        /// <see cref="M:Microsoft.Practices.Unity.LifetimeManager.RemoveValue"/>
        /// </summary>
        public override void RemoveValue(ILifetimeContainer container = null)
        {
            if (OperationContext.Current != null)
            {
                //WCF sin entorno HttpContext 
                ExtensionContenedor containerExtension = OperationContext.Current.Extensions.Find<ExtensionContenedor>();
                if (containerExtension != null)
                    OperationContext.Current.Extensions.Remove(containerExtension);
            }
            //else if (HttpContext.Current != null)
            //{
            //    //HttpContext disponible ( ASP.NET ..)
            //    if (HttpContext.Current.Items[_Llave.ToString()] != null)
            //        HttpContext.Current.Items[_Llave.ToString()] = null;
            //}
            else
            {
                //No sobre WCF o entornos ASP.NET, UnitTesting, WinForms, WPF etc.
                CallContext.FreeNamedDataSlot(_Llave.ToString());
            }
        }
        /// <summary>
        /// <see cref="M:Microsoft.Practices.Unity.LifetimeManager.SetValue"/>
        /// </summary>
        /// <param name="newValue"><see cref="M:Microsoft.Practices.Unity.LifetimeManager.SetValue"/></param>
        public override void SetValue(object NuevoValor, ILifetimeContainer container = null)
        {

            if (OperationContext.Current != null)
            {
                //WCF sin Entorno HttpContext 
                ExtensionContenedor extensionContenedor = OperationContext.Current.Extensions.Find<ExtensionContenedor>();
                if (extensionContenedor == null)
                {
                    extensionContenedor = new ExtensionContenedor()
                    {
                        Valor = NuevoValor
                    };

                    OperationContext.Current.Extensions.Add(extensionContenedor);
                }
            }
            //else if (HttpContext.Current != null)
            //{
            //    //HttpContext disponible ( ASP.NET ..)
            //    if (HttpContext.Current.Items[_Llave.ToString()] == null)
            //        HttpContext.Current.Items[_Llave.ToString()] = NuevoValor;
            //}
            else
            {
                //No sobre WCF o entornos ASP.NET, UnitTesting, WinForms, WPF etc.
                CallContext.SetData(_Llave.ToString(), NuevoValor);
            }
        }


        protected override LifetimeManager OnCreateLifetimeManager()
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
