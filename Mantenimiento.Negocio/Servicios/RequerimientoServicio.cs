using Dapper;
using Dapper.Contrib.Extensions;
using Mantenimiento.Datos.Base;
using Mantenimiento.Datos.Contratos;
using Mantenimiento.Datos.Entidades;
using Mantenimiento.Negocio.Contratos.Servicios;
using Mantenimiento.Negocio.Poco;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using Utilitario.Especificacion;

namespace Mantenimiento.Negocio.Servicios
{
    public class RequerimientoServicio : IRequerimientoServicio
    {
        public RequerimientoServicio()
        {

        }
        public List<sp_ListarRequerimientosxFiltro> ListaRequerimientos(int? cliente, int? proyecto, int? estado)
        {
            try
            {
                using (var connection = new SqlConnection(ConnectionConfig.ConnectionString))
                {
                    var parameters = new DynamicParameters();
                    parameters.Add("@cliente", cliente, DbType.Int32, ParameterDirection.Input);
                    parameters.Add("@proyecto", proyecto, DbType.Int32, ParameterDirection.Input);
                    parameters.Add("@estado", estado, DbType.Int32, ParameterDirection.Input);

                    var result = connection.Query<sp_ListarRequerimientosxFiltro>("sp_ListarRequerimientosxFiltro", parameters, commandType: CommandType.StoredProcedure);
                    return result.AsList();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al filtrar la lista de los requerimientos.", ex);
            }
        }
        public string GenerarCodigoAutomatico()
        {
            try
            {
                using (var connection = new SqlConnection(ConnectionConfig.ConnectionString))
                {
                    connection.Open();

                    // Obtener el último código de la base de datos
                    string ultimoCodigo = connection.QueryFirstOrDefault<string>("SELECT TOP 1 codigo FROM Requerimiento ORDER BY idRequerimiento DESC");

                    // Si no hay códigos en la base de datos, empezar desde REQ-0001
                    if (string.IsNullOrEmpty(ultimoCodigo))
                    {
                        return "REQ-0001";
                    }

                    // Extraer el número de secuencia del último código
                    int numeroSecuencia = int.Parse(ultimoCodigo.Substring(4));

                    // Incrementar el número de secuencia
                    numeroSecuencia++;

                    // Verificar si el nuevo número secuencial excede el límite permitido
                    if (numeroSecuencia > 9999)
                    {
                        throw new InvalidOperationException("Se ha alcanzado el límite máximo de códigos.");
                    }

                    // Formatear el nuevo código
                    string nuevoCodigo = $"REQ-{numeroSecuencia:D4}";

                    return nuevoCodigo;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al generar el código automático.", ex);
            }
        }

        public string GenerarCodigoAutomaticoTx(SqlConnection con, SqlTransaction tx)
        {
            try
            {
                if (con == null) throw new ArgumentNullException(nameof(con));
                if (tx == null) throw new ArgumentNullException(nameof(tx));
                if (tx.Connection != con) throw new InvalidOperationException("La transacción no pertenece a esta conexión.");

                // Obtener el último código de la base de datos
                string ultimoCodigo = con.QueryFirstOrDefault<string>("SELECT TOP 1 codigo FROM Requerimiento ORDER BY idRequerimiento DESC", transaction: tx);

                // Si no hay códigos en la base de datos, empezar desde REQ-0001
                if (string.IsNullOrEmpty(ultimoCodigo))
                {
                    return "REQ-0001";
                }

                // Extraer el número de secuencia del último código
                int numeroSecuencia = int.Parse(ultimoCodigo.Substring(4));

                // Incrementar el número de secuencia
                numeroSecuencia++;

                // Verificar si el nuevo número secuencial excede el límite permitido
                if (numeroSecuencia > 9999)
                {
                    throw new InvalidOperationException("Se ha alcanzado el límite máximo de códigos.");
                }

                // Formatear el nuevo código
                string nuevoCodigo = $"REQ-{numeroSecuencia:D4}";

                return nuevoCodigo;
            }
            catch (Exception ex)
            {
                throw new Exception("Error al generar el código automático.", ex);
            }
        }

        public string GetSubFolderByExtension(string extension)
        {
            switch (extension.ToLower())
            {
                case ".jpg":
                case ".jpeg":
                case ".png":
                    return "Images";
                case ".pdf":
                    return "PDFs";
                case ".doc":
                case ".docx":
                    return "WordDocuments";
                case ".xls":
                case ".xlsx":
                    return "ExcelSheets";
                default:
                    return "Others";
            }
        }
        public bool Insertar(GestionarRequerimientoPoco objeto)
        {
            bool result = false;

            using (SqlConnection connection = new SqlConnection(ConnectionConfig.ConnectionString))
            {
                connection.Open();
                int idRequerimiento = 0;

                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        #region REGISTRAR REQUERIMIENTO
                        Requerimiento requerimiento = new Requerimiento();
                        {
                            requerimiento.idPersona = objeto.Requerimiento.idPersona;
                            requerimiento.idProyecto = objeto.Requerimiento.idProyecto;
                            requerimiento.idTipoRequerimiento = objeto.Requerimiento.idTipoRequerimiento;
                            requerimiento.codigo = objeto.Requerimiento.codigo;
                            requerimiento.solicitante = objeto.Requerimiento.solicitante;
                            requerimiento.resumen = objeto.Requerimiento.resumen;
                            requerimiento.prioridad = objeto.Requerimiento.prioridad;
                            requerimiento.fechaInicio = objeto.Requerimiento.fechaInicio;
                            requerimiento.fechaFin = objeto.Requerimiento.fechaFin;
                            requerimiento.avanceDesarrollo = objeto.Requerimiento.avanceDesarrollo;
                            requerimiento.aprobacion = objeto.Requerimiento.aprobacion;
                            requerimiento.fechaRegistro = DateTime.Now;
                            requerimiento.estadoReq = objeto.Requerimiento.estadoReq;
                            requerimiento.idEstado = 1;
                        };

                        connection.Insert(requerimiento, transaction);

                        idRequerimiento = requerimiento.idRequerimiento;
                        #endregion

                        #region ASOCIAR REQUERIMIENTO A DETALLE REQUERIMIENTO
                        foreach (var item in objeto.ListaDetalleRequerimiento)
                        {
                            // Verificar que todos los campos necesarios estén llenos
                            if (
                                item.estadoDesarrollo != 0
                                && item.estadoCliente != 0
                                && !string.IsNullOrEmpty(item.operacion))
                            {
                                switch (item.operacion)
                                {
                                    case "N":
                                        DetalleRequerimiento detalleRequerimiento = new DetalleRequerimiento();
                                        {
                                            detalleRequerimiento.idRequerimiento = idRequerimiento;
                                            detalleRequerimiento.idPersona = item.idPersona;
                                            detalleRequerimiento.descripcion = item.descripcion;
                                            detalleRequerimiento.estadoDesarrollo = item.estadoDesarrollo;
                                            detalleRequerimiento.comentarioCliente = item.comentarioCliente;
                                            detalleRequerimiento.estadoCliente = item.estadoCliente;
                                            detalleRequerimiento.fechaRegistro = DateTime.Now;
                                            detalleRequerimiento.idEstado = 1;
                                            detalleRequerimiento.fechaInicio = item.fechaInicio;
                                            detalleRequerimiento.fechaFin = item.fechaFin;
                                            detalleRequerimiento.nombreArchivo = item.nombreArchivo;
                                            detalleRequerimiento.extension = item.extension;
                                            detalleRequerimiento.criterioAceptacion = item.criterioAceptacion;
                                        };

                                        connection.Insert(detalleRequerimiento, transaction);

                                        break;
                                    default:
                                        break;
                                }
                            }                                
                        }
                        #endregion

                        transaction.Commit();
                        result = true;
                    }
                    catch (Exception)
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }

            return result;
        }
        public bool Actualizar(GestionarRequerimientoPoco objeto)
        {
            bool result = false;

            using (SqlConnection connection = new SqlConnection(ConnectionConfig.ConnectionString))
            {
                connection.Open();
                int idRequerimiento = 0;

                using (var transaction = connection.BeginTransaction())
                    try
                    {
                        #region ACTUALIZAR REQUERIMIENTO
                       
                        connection.Update(objeto.Requerimiento, transaction);

                        idRequerimiento = objeto.Requerimiento.idRequerimiento;

                        #endregion

                        #region ASOCIAR REQUERIMIENTO A DETALLE REQUERIMIENTO
                        foreach (var item in objeto.ListaDetalleRequerimiento)
                        {
                            // Verificar que todos los campos necesarios estén llenos
                            if (
                                item.estadoDesarrollo != 0
                                && item.estadoCliente != 0
                                && !string.IsNullOrEmpty(item.operacion))
                            {
                                switch (item.operacion)
                                {
                                    case "N":
                                        DetalleRequerimiento detalleRequerimiento = new DetalleRequerimiento();
                                        {
                                            detalleRequerimiento.idRequerimiento = idRequerimiento;
                                            detalleRequerimiento.idPersona = item.idPersona;
                                            detalleRequerimiento.descripcion = item.descripcion;
                                            detalleRequerimiento.estadoDesarrollo = item.estadoDesarrollo;
                                            detalleRequerimiento.comentarioCliente = item.comentarioCliente;
                                            detalleRequerimiento.estadoCliente = item.estadoCliente;
                                            detalleRequerimiento.fechaRegistro = DateTime.Now;
                                            detalleRequerimiento.idEstado = 1;
                                            detalleRequerimiento.fechaInicio = item.fechaInicio;
                                            detalleRequerimiento.fechaFin = item.fechaFin;
                                            detalleRequerimiento.nombreArchivo = item.nombreArchivo;
                                            detalleRequerimiento.extension = item.extension;
                                            detalleRequerimiento.criterioAceptacion = item.criterioAceptacion;
                                        };

                                        connection.Insert(detalleRequerimiento, transaction);

                                        break;
                                    case "M":
                                        DetalleRequerimiento mDetalleRequerimiento = new DetalleRequerimiento();
                                        {
                                            mDetalleRequerimiento.idDetalleRequerimiento = item.idDetalleRequerimiento;
                                            mDetalleRequerimiento.idRequerimiento = idRequerimiento;
                                            mDetalleRequerimiento.idPersona = item.idPersona;
                                            mDetalleRequerimiento.descripcion = item.descripcion;
                                            mDetalleRequerimiento.comentarioCliente = item.comentarioCliente;
                                            mDetalleRequerimiento.estadoDesarrollo = item.estadoDesarrollo;
                                            mDetalleRequerimiento.estadoCliente = item.estadoCliente;
                                            mDetalleRequerimiento.fechaRegistro = DateTime.Now;
                                            mDetalleRequerimiento.idEstado = 1;
                                            mDetalleRequerimiento.fechaInicio = item.fechaInicio;
                                            mDetalleRequerimiento.fechaFin = item.fechaFin;
                                            mDetalleRequerimiento.nombreArchivo = item.nombreArchivo;
                                            mDetalleRequerimiento.extension = item.extension;
                                            mDetalleRequerimiento.criterioAceptacion = item.criterioAceptacion;
                                        };

                                        connection.Update(mDetalleRequerimiento, transaction);

                                        break;
                                    case "E":
                                        DetalleRequerimiento eDetalleRequerimiento = connection.Get<DetalleRequerimiento>(item.idDetalleRequerimiento, transaction);
                                        if (eDetalleRequerimiento != null)
                                        {
                                            // Establecer el estado del objeto como 2
                                            eDetalleRequerimiento.idEstado = 2;

                                            // Actualizar el objeto en la base de datos
                                            connection.Update(eDetalleRequerimiento, transaction);
                                        }

                                        break;
                                    default:
                                        break;
                                }
                            }
                        }
                        #endregion

                        transaction.Commit();
                        result = true;
                    }
                    catch (Exception ex)
                    {                        
                        transaction.Rollback();
                        throw new Exception("Error al actualizar los datos del Requerimiento.", ex);
                    }
            }

            return result;
        }
        public bool Eliminar(int id)
        {
            using (var connection = new SqlConnection(ConnectionConfig.ConnectionString))
            {
                connection.Open();

                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        // Obtener el requerimiento
                        Requerimiento oRequerimiento = connection.Get<Requerimiento>(id, transaction);

                        if (oRequerimiento != null)
                        {
                            // Actualizar el estado del requerimiento
                            oRequerimiento.idEstado = 2;
                            connection.Update(oRequerimiento, transaction);

                            // Obtener los detalles del requerimiento y asociar la transacción
                            var oDetalleRequerimiento = connection.GetAll<DetalleRequerimiento>(transaction)
                                .Where(dr => dr.idRequerimiento == id)
                                .ToList();

                            // Actualizar el estado de cada fila de Detalle Requerimiento                      
                            foreach (var detalleRequerimiento in oDetalleRequerimiento)
                            {
                                detalleRequerimiento.idEstado = 2;
                                // Actualizar todas las filas con el nuevo estado en una sola operación                        
                                connection.Update(oDetalleRequerimiento, transaction);
                            }

                        }
                        
                        transaction.Commit();
                        return true;
                    }
                    catch (Exception ex)
                    {                        
                        transaction.Rollback();
                        throw new Exception("Error al eliminar los datos del Requerimiento.", ex);
                    }
                }
            }
        }
        public Requerimiento ObtenerRequerimiento(int id)
        {
            using (var connection = new SqlConnection(ConnectionConfig.ConnectionString))
            {
                return connection.Get<Requerimiento>(id);
            }
        }

        private bool ExisteCodigoEnProyecto(SqlConnection connection, SqlTransaction tx, int idProyecto, string codigo)
        {
            const string sql = @"SELECT TOP 1 1 FROM Requerimiento WITH (NOLOCK) WHERE idProyecto=@idProyecto AND codigo=@codigo AND idEstado=1";
            var r = connection.QueryFirstOrDefault<int?>(sql, new { idProyecto, codigo }, tx);
            return r.HasValue;
        }

        private void InsertarDetalleMasivo(SqlConnection connection, SqlTransaction tx, int idRequerimiento, int idPersonaAsignado, List<string> items, string tipo)
        {
            if (items == null || items.Count == 0) return;

            int orden = 1;

            foreach (var raw in items)
            {
                var txt = (raw ?? "").Trim();
                if (string.IsNullOrWhiteSpace(txt)) continue;

                var detalle = new DetalleRequerimiento
                {
                    idRequerimiento = idRequerimiento,
                    idPersona = idPersonaAsignado,     // o 0 si no aplica
                    descripcion = $"{txt}",   // ✅ separa visualmente

                    // Ajusta a tus catálogos
                    estadoDesarrollo = 1,              // pendiente
                    estadoCliente = 1,                 // pendiente
                    comentarioCliente = "",

                    fechaRegistro = DateTime.Now,
                    idEstado = 1,

                    fechaInicio = DateTime.Now,
                    fechaFin = DateTime.Now.AddDays(7),

                    nombreArchivo = null,
                    extension = null,

                    // Si el tipo es CRITERIO, también lo guardo en criterioAceptacion
                    criterioAceptacion = (tipo == "CRITERIO") ? txt : null
                };

                connection.Insert(detalle, tx);
                orden++;
            }
        }


        public WizardGenerarReqDesdeDoResponse GenerarDesdeDocumentoOrigenWizard(WizardGenerarReqDesdeDoRequest request)
        {
            if (request == null) throw new Exception("Request inválido.");
            if (request.idPersona <= 0) throw new Exception("Debe indicar idPersona (cliente).");
            if (request.idProyecto <= 0) throw new Exception("Debe indicar idProyecto.");
            if (request.idTipoRequerimiento <= 0) throw new Exception("Debe indicar idTipoRequerimiento.");
            if (request.requerimientos == null || request.requerimientos.Count == 0)
                throw new Exception("No hay requerimientos para generar.");

            var resp = new WizardGenerarReqDesdeDoResponse();
            resp.idsRequerimiento = new List<int>();

            using (SqlConnection connection = new SqlConnection(ConnectionConfig.ConnectionString))
            {
                connection.Open();

                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        foreach (var item in request.requerimientos)
                        {
                            var codigo = GenerarCodigoAutomaticoTx(connection, transaction);
                            var titulo = (item.titulo ?? "").Trim();
                            var descripcionMacro = (item.descripcion ?? "").Trim();

                            if (string.IsNullOrWhiteSpace(codigo))
                                throw new Exception("Existe un requerimiento sin código.");
                            if (string.IsNullOrWhiteSpace(titulo))
                                throw new Exception($"El requerimiento {codigo} no tiene título.");

                            // Evitar duplicado de código dentro del proyecto (ajusta si quieres global)
                           
                            // ----------------------------
                            // Insert REQUERIMIENTO (tabla actual)
                            // ----------------------------
                            Requerimiento requerimiento = new Requerimiento
                            {
                                idPersona = request.idPersona,
                                idProyecto = request.idProyecto,
                                idTipoRequerimiento = request.idTipoRequerimiento,
                                codigo = codigo,
                                solicitante = string.IsNullOrWhiteSpace(request.solicitante) ? "Cliente" : request.solicitante,
                                idAdicional = request.idAdicional,
                                // ✅ Mapeo Wizard → tu tabla
                                resumen = titulo,             // título del módulo
                                prioridad = request.prioridad,

                                fechaInicio = request.fechaInicio ?? DateTime.Now,
                                fechaFin = request.fechaFin ?? DateTime.Now.AddDays(7),

                                avanceDesarrollo = 0,
                                aprobacion = 0,
                                fechaRegistro = DateTime.Now,

                                // Ajusta según tus estados reales
                                estadoReq = 1, // 1=Activo/Pendiente (ajusta)
                                idEstado = 1
                            };

                            connection.Insert(requerimiento, transaction);
                            int idRequerimiento = requerimiento.idRequerimiento;

                            resp.idsRequerimiento.Add(idRequerimiento);
                            resp.cantidad++;

                            // ----------------------------
                            // Insert DETALLES (unificando tipos)
                            // ----------------------------
                            // NOTA: Como tu DetalleRequerimiento no tiene campo "tipo",
                            // lo guardaremos como prefijo en descripcion: [DETALLE_DO], [CRITERIO], [QA], [SUBTAREA]
                            // (Si luego agregas campo tipo, lo cambias aquí y listo)
                            
                            
                            InsertarDetalleMasivo(connection, transaction, idRequerimiento, request.idPersona, item.detalleItems, "DETALLE_DO");
                            //InsertarDetalleMasivo(connection, transaction, idRequerimiento, request.idPersona, item.criteriosAceptacion, "CRITERIO");
                            //InsertarDetalleMasivo(connection, transaction, idRequerimiento, request.idPersona, item.checklistQA, "QA");
                            //InsertarDetalleMasivo(connection, transaction, idRequerimiento, request.idPersona, item.subtareasSugeridas, "SUBTAREA");

                            // Si quieres guardar la descripción macro como un detalle también:
                            //if (!string.IsNullOrWhiteSpace(descripcionMacro))
                            //{
                            //    InsertarDetalleMasivo(connection, transaction, idRequerimiento, request.idPersona,
                            //        new List<string> { descripcionMacro }, "DESCRIPCION");
                            //}
                        }

                        transaction.Commit();
                        return resp;
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        throw new Exception("Error al generar requerimientos desde DO (wizard).", ex);
                    }
                }
            }
        }

    }
}