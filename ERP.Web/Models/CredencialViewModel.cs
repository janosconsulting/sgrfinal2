using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ERP.Web.Models
{
    public class CredencialViewModel
    {
        public string Usuario { get; set; }
        public string Password { get; set; }

        public int esCarritoCompra { get; set; }
        public int idEmpresa { get; set; }

        #region Login Checkout Compra Carrito
        public string tipoRedirect { get; set; }
        #endregion
    }
}