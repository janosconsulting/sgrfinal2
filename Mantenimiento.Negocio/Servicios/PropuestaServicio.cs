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
using System.Web.Mvc;
using Utilitario.Especificacion;
namespace Mantenimiento.Negocio.Servicios
{

    public class PropuestaServicio : IPropuestaServicio
    {
        public PropuestaServicio()
        {
        }

        public List<sp_ListarPropuesta> Listar(int? cliente, int? estado)
        {
            try
            {
                using (var connection = new SqlConnection(ConnectionConfig.ConnectionString))
                {
                    connection.Open();

                    var result = connection.Query<sp_ListarPropuesta>(
                        "sp_Propuesta_Listar",
                        new
                        {
                            cliente = (cliente.HasValue && cliente.Value > 0) ? cliente : null,
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

        public PropuestaPdfPoco ObtenerParaPdf(int idPropuesta)
        {
            try
            {
                using (var connection = new SqlConnection(ConnectionConfig.ConnectionString))
                {
                    connection.Open();

                    const string sql = @"
        SELECT
            p.idPropuesta,
            p.codigo,
            p.titulo,
            p.responsable,
            p.resumenEjecutivo,
            p.caracteristicas,
            p.incluye,
            p.noIncluye,
            p.plazoDias,
            p.plazoDetalle,
            p.montoReferencia,
            p.incluyeIgv,
            p.observacionPrecio,
            p.formaPago,
            p.formaPagoDetalle,
            p.validezDias,
            p.condiciones,
            p.textoAceptacion,
            p.fechaRegistro,

            c.nombres AS cliente,
            '' AS estado,
            m.nombre AS moneda
        FROM dbo.Propuesta p
        LEFT JOIN dbo.Persona c ON c.idPersona = p.idCliente
       
        LEFT JOIN dbo.Moneda  m ON m.idMoneda  = p.idMoneda
        WHERE p.idPropuesta = @idPropuesta;
    ";

                    return connection.QueryFirstOrDefault<PropuestaPdfPoco>(sql, new { idPropuesta });
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
        public PropuestaEntidad Obtener(int id)
        {
            try
            {
                using (var connection = new SqlConnection(ConnectionConfig.ConnectionString))
                {
                    connection.Open();

                    // Opción A: Contrib
                    return connection.Get<PropuestaEntidad>(id);

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

        private string GenerarCodigoPropuesta(SqlConnection cn, SqlTransaction tx)
        {
            const string sql = @"
        SELECT 
            ISNULL(
                MAX(TRY_CAST(SUBSTRING(codigo, 5, 10) AS INT)),
            0) + 1
        FROM dbo.Propuesta WITH (UPDLOCK, HOLDLOCK)
        WHERE codigo LIKE 'PDS-%';
    ";

            int correlativo = cn.ExecuteScalar<int>(sql, transaction: tx);

            return $"PDS-{correlativo.ToString().PadLeft(4, '0')}";
        }

        // ==========================
        // INSERTAR (Contrib)
        // ==========================
        public bool Insertar(PropuestaEntidad oPropuesta)
        {
            using (var connection = new SqlConnection(ConnectionConfig.ConnectionString))
            {
                connection.Open();

                using (var tx = connection.BeginTransaction())
                {
                    try
                    {
                        // 1️⃣ Generar código desde la MISMA tabla Propuesta
                        oPropuesta.codigo = GenerarCodigoPropuesta(connection, tx);

                        // 2️⃣ Valores por defecto
                        oPropuesta.fechaRegistro = DateTime.Now;
                        oPropuesta.idEstado = 1; // BORRADOR

                        // 3️⃣ Insert con Dapper.Contrib
                        connection.Insert(oPropuesta, tx);

                        tx.Commit();
                        return true;
                    }
                    catch
                    {
                        tx.Rollback();
                        throw;
                    }
                }
            }
        }


        // ==========================
        // ACTUALIZAR (Contrib)
        // ==========================
        public bool Actualizar(PropuestaEntidad oDocumentoOrigen)
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

                var doc = connection.Get<PropuestaEntidad>(idDocumentoOrigen);
                if (doc == null) return false;

                return connection.Update(doc);
            }
        }

        // ==========================
        // CÓDIGO AUTOMÁTICO (SP)
        // ==========================
      
     
    }
}