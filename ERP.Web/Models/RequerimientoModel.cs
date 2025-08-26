using Mantenimiento.Datos.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ReyDavid.Web.Models
{
    public class RequerimientoModel
    {
        public Requerimiento Requerimiento { get; set; }
        public HttpPostedFileBase foto { get; set; }
    }
}