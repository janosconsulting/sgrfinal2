using Dapper.Contrib.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Mantenimiento.Datos.Entidades
{
    public class sp_ObtenerCasosExito
    {
        public int idCaso { get; set; }
        public string Titulo { get; set; }
        public string Descripcion { get; set; }
        public string NombreCliente { get; set; }
        public int estado { get; set; }
        public string NombrePais { get; set; }
        public string nombreArchivo { get; set; }
        public string extension { get; set; }
        public string cambio { get; set; }
        public string resultados { get; set; }
        public bool mostrarEnWeb { get; set; }



        [Write(false)] // No se guarda en la base de datos
        public string rubro { get; set; }

    }
}
