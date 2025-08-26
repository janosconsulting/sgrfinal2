using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mantenimiento.Negocio.Poco
{
    public class DetalleRepeatablePoco
    {
        public int Id { get; set; }
        public string Nombres { get; set; }
        public string Correo { get; set; }
        public string Referencia { get; set; }
        public string Descripcion { get; set; }
        public string Operacion { get; set; }

        public string Extension { get; set; }

        public string Ruta { get; set; }
    }
}
