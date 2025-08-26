using System;
using System.IO;
using System.Web.Hosting;

namespace ERP.Web.Helpers
{
    public static class PlantillaEmail
    {
        public static string ConfirmarPagoPedido(string Cliente, string IdPago, string IdPedido, string MontoTotal)
        {
            string HtmlEmail = "";
            string pathFile = HostingEnvironment.MapPath("/Template/Email/pedidopagoconfirmado2.html");

            try
            {
                HtmlEmail = File.ReadAllText(pathFile);
                HtmlEmail = HtmlEmail.Replace("@Cliente", Cliente);
                HtmlEmail = HtmlEmail.Replace("@IdPago", IdPago);
                HtmlEmail = HtmlEmail.Replace("@IdPedido", IdPedido);
                HtmlEmail = HtmlEmail.Replace("@MontoTotal", MontoTotal);

                return HtmlEmail;
            }
#pragma warning disable CS0168 // La variable 'ex' se ha declarado pero nunca se usa
            catch (Exception ex)
#pragma warning restore CS0168 // La variable 'ex' se ha declarado pero nunca se usa
            {
                return string.Empty;
            }
        }

        public static string ConfirmarBienvenida(string empresa, int esCompartido, string ruc, string usuario, string contraseña, string url)
        {
            string HtmlEmail = "";
            string pathFile = HostingEnvironment.MapPath("/Template/Email/bienvenida.html");

            try
            {
                HtmlEmail = File.ReadAllText(pathFile);
                HtmlEmail = HtmlEmail.Replace("@Empresa", empresa);
                HtmlEmail = HtmlEmail.Replace("@EsCompartido", esCompartido.ToString());
                HtmlEmail = HtmlEmail.Replace("@Ruc", ruc);
                HtmlEmail = HtmlEmail.Replace("@Usuario", usuario);
                HtmlEmail = HtmlEmail.Replace("@Contrasenia", contraseña);
                HtmlEmail = HtmlEmail.Replace("@Url", url);

                return HtmlEmail;
            }
#pragma warning disable CS0168 // La variable 'ex' se ha declarado pero nunca se usa
            catch (Exception ex)
#pragma warning restore CS0168 // La variable 'ex' se ha declarado pero nunca se usa
            {
                return string.Empty;
            }
        }

        public static string GenerarCupon(string nombre, string correo, string cupon)
        {
            string HtmlEmail = "";
            string pathFile = HostingEnvironment.MapPath("/Template/Email/cupongenerado.html");

            try
            {
                HtmlEmail = File.ReadAllText(pathFile);
                HtmlEmail = HtmlEmail.Replace("@nombre", nombre);
                HtmlEmail = HtmlEmail.Replace("@correo", correo);
                HtmlEmail = HtmlEmail.Replace("@cupon", cupon);

                return HtmlEmail;
            }
#pragma warning disable CS0168 // La variable 'ex' se ha declarado pero nunca se usa
            catch (Exception ex)
#pragma warning restore CS0168 // La variable 'ex' se ha declarado pero nunca se usa
            {
                return string.Empty;
            }
        }


        public static string RegistroCliente(string nombreC, string usuario, string password)
        //(string Cliente, string IdPago, string IdPedido, string MontoTotal)
        {
            string HtmlEmail = "";
            string pathFile = HostingEnvironment.MapPath("/Template/Email/registrocliente.html");

            try
            {
                HtmlEmail = File.ReadAllText(pathFile);
                HtmlEmail = HtmlEmail.Replace("@nombrecompleto", nombreC);
                HtmlEmail = HtmlEmail.Replace("@usuario", usuario);
                HtmlEmail = HtmlEmail.Replace("@password", password);
                //HtmlEmail = HtmlEmail.Replace("@MontoTotal", MontoTotal);

                return HtmlEmail;
            }
#pragma warning disable CS0168 // La variable 'ex' se ha declarado pero nunca se usa
            catch (Exception ex)
#pragma warning restore CS0168 // La variable 'ex' se ha declarado pero nunca se usa
            {
                return string.Empty;
            }
        }
    }
}