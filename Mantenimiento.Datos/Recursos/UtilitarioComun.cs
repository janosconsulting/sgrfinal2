using System;
using System.Text;

namespace Mantenimiento.Datos.Recursos
{
    public class UtilitarioComun
    {
        public static string CompletarCeros(int numero, int cantidadceros)
        {
            if (cantidadceros < 0)
                throw new Exception("Cantidad de ceros debe ser mayor a cero");
            if (numero.ToString().Length < cantidadceros)
            {
                StringBuilder concatenaceros = new StringBuilder();
                cantidadceros -= numero.ToString().Length;
                for (int i = 0; i < cantidadceros; i++)
                {
                    concatenaceros.Append("0");
                }
                concatenaceros.Append(numero);

                return concatenaceros.ToString();
            }
            else
                return numero.ToString();
        }

    }
}
