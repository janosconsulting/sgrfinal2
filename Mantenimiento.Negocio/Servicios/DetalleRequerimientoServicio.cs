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
    public class DetalleRequerimientoServicio : IDetalleRequerimiento
    {
        public DetalleRequerimientoServicio()
        {

        }
        public List<ListaDetalleRequerimiento> ObtenerDetalleReq(int id)
        {
            try
            {
                int idRequerimiento = 0;

                using (var connection = new SqlConnection(ConnectionConfig.ConnectionString))
                {
                    connection.Open();

                    var Requerimiento = connection.Get<Requerimiento>(id);

                    idRequerimiento = Requerimiento.idRequerimiento;

                    // Obtener los detalles de requerimiento para el idRequerimiento especificado
                    var detallesRequerimiento = connection.GetAll<DetalleRequerimiento>()
                        .Where(dr => dr.idRequerimiento == idRequerimiento && dr.idEstado == 1)
                        .ToList();

                    // Mapear a ListaDetalleRequerimiento
                    var listaDetalleRequerimiento = detallesRequerimiento.Select(dr => new ListaDetalleRequerimiento
                    {
                        idDetalleRequerimiento = dr.idDetalleRequerimiento,
                        idPersona = dr.idPersona,
                        descripcion = dr.descripcion,
                        comentarios = dr.comentarios,
                        estadoDesarrollo = dr.estadoDesarrollo,
                        estadoCliente = dr.estadoCliente,
                        fechaInicio = dr.fechaInicio,
                        fechaFin = dr.fechaFin,
                        nombreArchivo = dr.nombreArchivo,
                        extension = dr.extension,
                        criterioAceptacion = dr.criterioAceptacion,
                        comentarioCliente = dr.comentarioCliente
                    }).ToList();

                    return listaDetalleRequerimiento;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener el detalle requerimiento.", ex);
            }
        }
        public DetalleRequerimiento Obtener(int id)
        {
            using (var connection = new SqlConnection(ConnectionConfig.ConnectionString))
            {
                return connection.Get<DetalleRequerimiento>(id);
            }
        }
    }
}
