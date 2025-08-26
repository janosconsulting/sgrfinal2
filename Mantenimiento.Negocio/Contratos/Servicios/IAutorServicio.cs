using Mantenimiento.Datos.Entidades;
using Mantenimiento.Negocio.Poco;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mantenimiento.Negocio.Contratos.Servicios
{
    public interface IAutorServicio
    {
        Autor Obtener(int idAutor);

        List<sp_ListarPersonas_Result> Listar();

        List<sp_ListarPerfiles_Result> ListarPerfil();

        ResultadoTransaccion Insertar(GestionarPersonaPoco objeto);

        bool Actualizar(GestionarPersonaPoco objeto);

        bool Eliminar(int idAutor);

    }
}
