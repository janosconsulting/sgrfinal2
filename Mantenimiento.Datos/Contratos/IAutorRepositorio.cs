using Mantenimiento.Datos.Base;
using Mantenimiento.Datos.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mantenimiento.Datos.Contratos
{
    public interface IAutorRepositorio : IRepositorio<Autor>
    {
        List<sp_ListarPersonas_Result> Listar();

        List<sp_ListarPerfiles_Result> ListarPerfil();
    }
}
