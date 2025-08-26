using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ReyDavid.Web.DTO
{
    public class Cart
    {
        public int id {  get; set; }
        public int idProducto {  get; set; }
        public string name {  get; set; }
        public decimal price { get; set; }
        public string item { get; set; }
        public int cantidad { get; set; }
        public string nombreArchivo { get; set; }
        public string description { get; set; }
        public Cart() { }
        
    }
}