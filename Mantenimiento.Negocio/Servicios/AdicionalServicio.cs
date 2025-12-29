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

    public class AdicionalServicio : IAdicionalServicio
    {
        public AdicionalServicio()
        {
        }

        public List<Adicional> Listar()
        {
            try
            {
                using (var connection = new SqlConnection(ConnectionConfig.ConnectionString))
                {
                    var result = connection.GetAll<Adicional>().ToList();

                    return result;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al listar casos de Éxito.", ex);
            }
        }

        public List<sp_ListarDocumentoOrigen> Listar(int? cliente, int? proyecto, int? tipo, int? estado)
        {
            try
            {
                using (var connection = new SqlConnection(ConnectionConfig.ConnectionString))
                {
                    connection.Open();

                    var result = connection.Query<sp_ListarDocumentoOrigen>(
                        "sp_ListarDocumentoOrigen",
                        new
                        {
                            cliente = (cliente.HasValue && cliente.Value > 0) ? cliente : null,
                            proyecto = (proyecto.HasValue && proyecto.Value > 0) ? proyecto : null,
                            tipo = (tipo.HasValue && tipo.Value > 0) ? tipo : null,
                            estado = (estado.HasValue && estado.Value > 0) ? estado : null
                        },
                        commandType: CommandType.StoredProcedure
                    );

                    return result.AsList();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al listar documentos de origen.", ex);
            }
        }

        // ==========================
        // OBTENER
        // ==========================
        public Adicional Obtener(int idDocumentoOrigen)
        {
            try
            {
                using (var connection = new SqlConnection(ConnectionConfig.ConnectionString))
                {
                    connection.Open();

                    // Opción A: Contrib
                    return connection.Get<Adicional>(idDocumentoOrigen);

                    // Opción B: SP (si lo prefieres)
                    // return connection.QueryFirstOrDefault<DocumentoOrigen>(
                    //     "Doc.sp_ObtenerDocumentoOrigen",
                    //     new { idDocumentoOrigen },
                    //     commandType: CommandType.StoredProcedure
                    // );
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener documento de origen.", ex);
            }
        }

        // ==========================
        // INSERTAR (Contrib)
        // ==========================
        public bool Insertar(Adicional oDocumentoOrigen)
        {
            using (var connection = new SqlConnection(ConnectionConfig.ConnectionString))
            {
                connection.Open();

                connection.Insert(oDocumentoOrigen);
                return true;
            }
        }

        // ==========================
        // ACTUALIZAR (Contrib)
        // ==========================
        public bool Actualizar(Adicional oDocumentoOrigen)
        {
            using (var connection = new SqlConnection(ConnectionConfig.ConnectionString))
            {
                connection.Open();

              
                return connection.Update(oDocumentoOrigen);
            }
        }

        // ==========================
        // ELIMINAR (Lógico)
        // ==========================
        public bool Eliminar(int idDocumentoOrigen)
        {
            using (var connection = new SqlConnection(ConnectionConfig.ConnectionString))
            {
                connection.Open();

                var doc = connection.Get<Adicional>(idDocumentoOrigen);
                if (doc == null) return false;

                return connection.Update(doc);
            }
        }

        // ==========================
        // CÓDIGO AUTOMÁTICO (SP)
        // ==========================
        public string GenerarCodigo()
        {
            using (var connection = new SqlConnection(ConnectionConfig.ConnectionString))
            {
                connection.Open();
                return GenerarCodigo(connection, null);
            }
        }

        private string GenerarCodigo(SqlConnection con, SqlTransaction tx)
        {
            // SP: Doc.sp_GenerarCodigoDocumentoOrigen (@codigo OUTPUT)
            var p = new DynamicParameters();
            p.Add("@codigo", dbType: DbType.String, direction: ParameterDirection.Output, size: 20);

            con.Execute(
                "Doc.sp_GenerarCodigoDocumentoOrigen",
                p,
                transaction: tx,
                commandType: CommandType.StoredProcedure
            );

            return p.Get<string>("@codigo");
        }

        // ==========================
        // CHECKLIST (SP con 2 resultsets)
        // ==========================
        public ReporteChecklistDocumentoOrigenVm ObtenerChecklistVm(int idDocumentoOrigen)
        {
            try
            {
                using (var connection = new SqlConnection(ConnectionConfig.ConnectionString))
                {
                    connection.Open();

                    using (var multi = connection.QueryMultiple(
                        "Doc.sp_ChecklistDocumentoOrigen",
                        new { idDocumentoOrigen },
                        commandType: CommandType.StoredProcedure))
                    {
                        var header = multi.ReadFirstOrDefault<ChecklistHeaderRow>();
                        if (header == null) return null;

                        var items = multi.Read<ChecklistItemRow>().ToList();

                        return new ReporteChecklistDocumentoOrigenVm
                        {
                            EmpresaNombre = "REY DAVID",            // cámbialo por tu empresa real si lo tienes
                            CodigoReporte = "RPT-" + header.codigo,

                            IdDocumentoOrigen = header.idDocumentoOrigen,
                            DocCodigo = header.codigo,
                            DocTipo = MapTipo(header.tipoDoc),
                            DocFecha = header.fechaDocumento,
                            DocTitulo = header.titulo,
                            DocDescripcion = header.descripcion,
                            EstadoGeneral = MapEstadoDoc(header.estado),

                            ClienteNombre = header.clienteNombre,
                            ProyectoNombre = header.proyectoNombre,

                            Items = items.Select(x => new ReporteChecklistDocumentoOrigenItemVm
                            {
                                IdRequerimiento = x.idRequerimiento,
                                Codigo = x.codigo,
                                Titulo = x.titulo,
                                Detalle = x.detalle,
                                Solicitante = x.solicitante,
                                Asignado = x.asignado,
                                Prioridad = x.prioridad,
                                EstadoReq = x.estadoReq,
                                FechaInicio = x.fechaInicio,
                                FechaFin = x.fechaFin
                            }).ToList()
                        };
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al generar checklist del documento de origen.", ex);
            }
        }

        // ==========================
        // VERSIONES CON TRANSACCIÓN
        // ==========================
        public bool InsertarTx(SqlConnection con, SqlTransaction tx, Adicional oDocumentoOrigen)
        {
           
            con.Insert(oDocumentoOrigen, transaction: tx);
            return true;
        }

        public bool ActualizarTx(SqlConnection con, SqlTransaction tx, Adicional oDocumentoOrigen)
        {
            //oDocumentoOrigen.fechaActualiza = DateTime.Now;
            return con.Update(oDocumentoOrigen, transaction: tx);
        }

        public bool EliminarTx(SqlConnection con, SqlTransaction tx, int idDocumentoOrigen)
        {
            var doc = con.Get<Adicional>(idDocumentoOrigen, transaction: tx);
            if (doc == null) return false;

            
            return con.Update(doc, transaction: tx);
        }

        public Adicional ObtenerTx(SqlConnection con, SqlTransaction tx, int idDocumentoOrigen)
        {
            return con.Get<Adicional>(idDocumentoOrigen, transaction: tx);
        }

        // ==========================
        // HELPERS (MAP)
        // ==========================
        private string MapTipo(int tipo)
        {
            switch (tipo)
            {
                case 1: return "ACTA";
                case 2: return "COTIZACIÓN";
                case 3: return "DOCUMENTO";
                case 4: return "CORREO";
                case 5: return "WHATSAPP";
                default: return "DOCUMENTO";
            }
        }

        private string MapEstadoDoc(int estado)
        {
            switch (estado)
            {
                case 1: return "REGISTRADO";
                case 2: return "EN REVISIÓN";
                case 3: return "CERRADO";
                default: return "REGISTRADO";
            }
        }

        // ==========================
        // DTOs internos para el SP Checklist
        // (deben matchear columnas del SP Doc.sp_ChecklistDocumentoOrigen)
        // ==========================
        private class ChecklistHeaderRow
        {
            public int idDocumentoOrigen { get; set; }
            public string codigo { get; set; }
            public int tipoDoc { get; set; }
            public DateTime fechaDocumento { get; set; }
            public string titulo { get; set; }
            public string descripcion { get; set; }
            public int estado { get; set; }
            public string clienteNombre { get; set; }
            public string proyectoNombre { get; set; }
        }

        private class ChecklistItemRow
        {
            public int idRequerimiento { get; set; }
            public string codigo { get; set; }
            public string titulo { get; set; }
            public string detalle { get; set; }
            public string solicitante { get; set; }
            public int prioridad { get; set; }
            public int estadoReq { get; set; }
            public DateTime? fechaInicio { get; set; }
            public DateTime? fechaFin { get; set; }
            public string asignado { get; set; }
        }
    }
}