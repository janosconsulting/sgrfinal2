using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace ControlCombustible.Negocio.Poco
{
    // Usuario DTO
    [DataContract(IsReference = true)]
    public class UsuarioOperacion
    {
        [DataMember]
        public int IDSistema { get; set; }
        [DataMember]
        public int IDTrabajador { get; set; }
        [DataMember]
        public int IDUsuario { get; set; }
        [DataMember]
        public string NomUsuario { get; set; }
        [DataMember]
        public string NomUsuarioPC { get; set; }
        [DataMember]
        public string IPUsuario { get; set; }
        [DataMember]
        public string NombrePC { get; set; }
        [DataMember]
        public Array Permisos { get; set; }
        [DataMember]
        public string NombrePersona { get; set; }
        [DataMember]
        public string DniPersona { get; set; }
        [DataMember]
        public bool estado { get; set; }
    }
}
