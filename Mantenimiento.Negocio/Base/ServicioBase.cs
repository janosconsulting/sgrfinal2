using ControlCombustible.Negocio.Contratos.Base;
using ControlCombustible.Negocio.Poco;
using ControlCombustible.Negocio.Recursos;
using DevExpress.ExpressApp.MiddleTier;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControlCombustible.Negocio.Base
{
    public class ServicioBase : IService
    {
        protected UsuarioOperacion usuInfo;
        protected Resultado resultado;
        public UsuarioOperacion UsuarioOperacion
        {
            set
            {
                usuInfo = value;
                CargarUsuarioRepositorio();
            }
        }

        protected virtual void CargarUsuarioRepositorio()
        {

        }

        protected void CargarInformacionUsuario<T>(IRepositorio<T> repositorio) where T : class
        {
            if (usuInfo == null) return;
            repositorio.UsuarioOperacion = usuInfo;
        }
        protected Resultado MensajeSatisfactorio(TipoMensaje tipoMensaje, object objetoPrincipal)
        {
            resultado = new Resultado();
            resultado.esCorrecto = true;
            switch (tipoMensaje)
            {
                case TipoMensaje.Creacion:
                    resultado.mensajeMostrar = Mensajes.MensajeCreacionSatisfactoria;
                    break;
                case TipoMensaje.Modificacion:
                    resultado.mensajeMostrar = Mensajes.MensajeModificacionSatisfactoria;
                    break;
                case TipoMensaje.Eliminacion:
                    resultado.mensajeMostrar = Mensajes.MensajeElimininacionSatisfactoria;
                    break;
                case TipoMensaje.Anulacion:
                    resultado.mensajeMostrar = Mensajes.MensajeAnulacionSatisfactoria;
                    break;
            }

            resultado.objetoPrincipal = objetoPrincipal;
            return resultado;
        }

        protected Resultado MensajeError<T>(IRepositorio<T> repositorio, TipoMensaje tipoMensaje, object objetoPrincipal, Exception ex) where T : class
        {
            TipoAccion tipoaccion = TipoAccion.Otros;
            switch (tipoMensaje)
            {
                case TipoMensaje.Creacion:
                    tipoaccion = TipoAccion.Ingreso;
                    break;
                case TipoMensaje.Modificacion:
                    tipoaccion = TipoAccion.Modificacion;
                    break;
                case TipoMensaje.Eliminacion:
                    tipoaccion = TipoAccion.Modificacion;
                    break;
                case TipoMensaje.Anulacion:
                    tipoaccion = TipoAccion.Anulacion;
                    break;
            }
           // repositorio.RegistrarErroresAlLog(string.Format(Mensajes.MensajeError, ex.Message, ex.StackTrace), tipoaccion);
            return MensajeError(tipoMensaje, objetoPrincipal, ex);
        }
        protected Resultado MensajeError(TipoMensaje tipoMensaje, object objetoPrincipal, Exception ex)
        {
            resultado = new Resultado();
            resultado.esCorrecto = false;
            switch (tipoMensaje)
            {
                case TipoMensaje.Creacion:
                    resultado.mensajeMostrar = Mensajes.ErrorDeCreacion;
                    break;
                case TipoMensaje.Modificacion:
                    resultado.mensajeMostrar = Mensajes.ErrorDeModificacion;
                    break;
                case TipoMensaje.Eliminacion:
                    resultado.mensajeMostrar = Mensajes.ErrorDeEliminacion;
                    break;
                case TipoMensaje.Anulacion:
                    resultado.mensajeMostrar = Mensajes.ErrorDeAnulacion;
                    break;
            }
            resultado.mensajeInterno = string.Format(Mensajes.MensajeError, ex.Message, ex.StackTrace);
            resultado.objetoPrincipal = objetoPrincipal;
            if (ex.InnerException != null)
            {
                resultado.mensajeInterno2 = ex.InnerException.Message;
                resultado.mensajeInterno3 = ex.InnerException.StackTrace;
            }

            //if(ex.)
            //ex.Data.
            //RegistrarError("Mensaje:" + ex.Message + ", \nSeguimiento: " + ex.StackTrace, TipoAccion.Modificacion);

            return resultado;
        }
    }
}
