using Mantenimiento.Datos.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ReyDavid.Web.Models
{
    public class PersonaModel
    {
        public Persona Persona { get; set; }
        public bool IsEditing { get; set; }
    }   
}