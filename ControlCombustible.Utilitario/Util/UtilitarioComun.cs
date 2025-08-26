using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Utilitario.Util
{
    public class UtilitarioComun2
    {


        public static string Random()
        {
            string randomNum = string.Empty;
            Random autoRand = new Random();

            for (int x = 0; x < 4; x++)
            {
                randomNum += System.Convert.ToInt32(autoRand.Next(0, 9)).ToString();
            }

            int i_letra = System.Convert.ToInt32(autoRand.Next(65, 90));

            string letra = ((char)i_letra).ToString();
            randomNum += letra;

            return randomNum;
        }

        public static string encryptaClave(string clave)
        {
            string claveAscii = "";
            int cont = 0;
            int[] codAsciiOWA = { 65, 79, 87 }; // para el calculo ubico el codigo ascci de las letras OWA en este orden A O W

            for (int i = 1; i <= clave.Length; i++)
            {
                if (i % 3 == 0)
                    cont += 3;

                foreach (char letra in clave.Substring(i - 1, 1))
                {
                    if (i == 1)
                        claveAscii += ((int)letra ^ codAsciiOWA.ElementAt(i - cont));
                    else
                        claveAscii += "-" + ((int)letra ^ codAsciiOWA.ElementAt(i - cont));
                }
            }
            return claveAscii;
        }

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

        //public static string CompletarCeros(string numero, int cantidadceros)
        //{
        //    if (cantidadceros < 0)
        //        throw new Exception("Cantidad de ceros debe ser mayor a cero");
        //    if (numero.Length < cantidadceros)
        //    {
        //        StringBuilder concatenaceros = new StringBuilder();
        //        cantidadceros -= numero.Length;
        //        for (int i = 0; i < cantidadceros; i++)
        //        {
        //            concatenaceros.Append("0");
        //        }
        //        concatenaceros.Append(numero);

        //        return concatenaceros.ToString();
        //    }
        //    else
        //        return numero.ToString();
        //}

        public static List<Mes> ListarMeses()
        {
            List<Mes> ListaMeses = new List<Mes>();
            ListaMeses.Add(new Mes { indice = 1, nombre = "ENERO" });
            ListaMeses.Add(new Mes { indice = 2, nombre = "FEBRERO" });
            ListaMeses.Add(new Mes { indice = 3, nombre = "MARZO" });
            ListaMeses.Add(new Mes { indice = 4, nombre = "ABRIL" });
            ListaMeses.Add(new Mes { indice = 5, nombre = "MAYO" });
            ListaMeses.Add(new Mes { indice = 6, nombre = "JUNIO" });
            ListaMeses.Add(new Mes { indice = 7, nombre = "JULIO" });
            ListaMeses.Add(new Mes { indice = 8, nombre = "AGOSTO" });
            ListaMeses.Add(new Mes { indice = 9, nombre = "SEPTIEMBRE" });
            ListaMeses.Add(new Mes { indice = 10, nombre = "OCTUBRE" });
            ListaMeses.Add(new Mes { indice = 11, nombre = "NOVIEMBRE" });
            ListaMeses.Add(new Mes { indice = 12, nombre = "DICIEMBRE" });
            return ListaMeses;
        }

        public static List<Anio> ListarAnios(int vanio_fin, int vanio_inicio)
        {
            List<Anio> lst = new List<Anio>();
            int y = vanio_fin;
            while (y >= vanio_inicio)
            {
                lst.Add(new Anio { indice = y, nombre = y.ToString() });
                y--;
            }
            return lst;
        }

        public static List<Dia> ListaDias()
        {
            List<Dia> list = new List<Dia>();
            Dia dia = new Dia();
            list.Add(new Dia { codDia = 1, nomDia = "Domingo", posicion = 7 });
            list.Add(new Dia { codDia = 2, nomDia = "Lunes", posicion = 1 });
            list.Add(new Dia { codDia = 3, nomDia = "Martes", posicion = 2 });
            list.Add(new Dia { codDia = 4, nomDia = "Miercoles", posicion = 3 });
            list.Add(new Dia { codDia = 5, nomDia = "Jueves", posicion = 4 });
            list.Add(new Dia { codDia = 6, nomDia = "Viernes", posicion = 5 });
            list.Add(new Dia { codDia = 7, nomDia = "Sabado", posicion = 6 });

            return list;
        }

        public static string NombDia(int codDia)
        {

            string dia = "";

            if (codDia == 2)
                dia = "Lun";
            else
            {
                if (codDia == 3)
                    dia = "Mar";
                else
                {
                    if (codDia == 4)
                        dia = "Mie";
                    else
                    {
                        if (codDia == 5)
                            dia = "Jue";
                        else
                        {
                            if (codDia == 6)
                                dia = "Vie";
                            else
                            {
                                if (codDia == 7)
                                    dia = "Sab";
                                else
                                    dia = "Dom";
                            }
                        }
                    }
                }
            }


            return dia;
        }

        public static bool ValidarFecha(DateTime Fecha)
        {
            return ValidarFecha(Fecha.ToString());
        }

        public static bool ValidarFecha(string Fecha)
        {
            bool bolCorrecta;
            string[] strFecha = Fecha.Split('/');
            if (strFecha.Length < 3)
            {
                bolCorrecta = false;
            }
            else if (!(EsFecha(strFecha[2] + "-" + strFecha[1] + "-" + strFecha[0])))
            {
                bolCorrecta = false;
            }
            else
            {
                bolCorrecta = true;
            }
            return bolCorrecta;
        }

        public static bool ValidarNumeroEntero(string ingreso)
        {
            bool des = true;
            foreach (char caracter in ingreso)
            {
                if (!Char.IsNumber(caracter))
                {
                    des = false;
                    break;
                }
            }
            return des;
        }

        public static bool ValidarNumeroFlotante(string ingreso)
        {
            Regex expresion = new Regex(@"/^([0-9])*[.]?[0-9]*$/");
            return expresion.IsMatch(ingreso);
        }

        public static int DiasPorMes(int mes, int anio)
        {
            return DateTime.DaysInMonth(anio, mes);
        }

        public static int DiasPorMesAnterior(int mesActual, int anioActual)
        {
            anioActual = mesActual == 1 ? anioActual - 1 : anioActual;
            mesActual = mesActual == 1 ? 12 : mesActual - 1;
            return DateTime.DaysInMonth(anioActual, mesActual);
        }

        public static int AnioAnterior(int mesActual, int anioActual)
        {
            return mesActual == 1 ? anioActual - 1 : anioActual;
        }

        public static string NombreMes(int mes)
        {
            string nombre = string.Empty;
            switch (mes)
            {
                case 1: nombre = "ENERO"; break;
                case 2: nombre = "FEBRERO"; break;
                case 3: nombre = "MARZO"; break;
                case 4: nombre = "ABRIL"; break;
                case 5: nombre = "MAYO"; break;
                case 6: nombre = "JUNIO"; break;
                case 7: nombre = "JULIO"; break;
                case 8: nombre = "AGOSTO"; break;
                case 9: nombre = "SEPTIEMBRE"; break;
                case 10: nombre = "OCTUBRE"; break;
                case 11: nombre = "NOVIEMBRE"; break;
                case 12: nombre = "DICIEMBRE"; break;
            }
            return nombre;
        }

        public static int MesAnterior(int mesActual)
        {
            return mesActual == 1 ? 12 : mesActual - 1;
        }

        private static bool EsFecha(string expresion)
        {
            if (expresion == null) { return false; }

            try
            {
                DateTime dateTime = DateTime.Parse(expresion);
            }
            catch (FormatException)
            {
                return false;
            }
            return true;
        }

        public static string devuelveFecha_SinHora(DateTime fecha)
        {
            return (CompletarCeros(fecha.Day, 2) + "/" + CompletarCeros(fecha.Month, 2) + "/" + fecha.Year);
        }

        public static byte[] archivoA_Byte(string ruta)
        {
            string sBase64 = "";
            FileStream fs = new FileStream(ruta, FileMode.Open);
            BinaryReader br = new BinaryReader(fs);
            byte[] bytes = new byte[(int)fs.Length];
            try
            {
                br.Read(bytes, 0, bytes.Length);
                sBase64 = Convert.ToBase64String(bytes);

                return bytes;
            }
            catch
            {
            }

            return bytes;
        }

        //public static Image byteA_Imagen(byte[] byteArrayIn)
        //{
        //    if(byteArrayIn==null) return null;
        //    MemoryStream ms = new MemoryStream(byteArrayIn);
        //    Image returnImage = Image.FromStream(ms);
        //    return returnImage;
        //}

        //public static byte[] imagenA_Byte(System.Drawing.Image imageIn)
        //{
        //    MemoryStream ms = new MemoryStream();
        //    imageIn.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);           
        //    return ms.ToArray();          
        //}

        //public static Image archivoA_Imagen(string ruta)
        //{
        //    return byteA_Imagen(archivoA_Byte(ruta));
        //}

        //public static Image RedimensionarImagen(Image Imagen, int Ancho, int Alto, int resolucion)
        //{
        //    //Bitmap sera donde trabajaremos los cambios
        //    using (Bitmap imagenBitmap = new Bitmap(Ancho, Alto, PixelFormat.Format32bppRgb))
        //    {
        //        imagenBitmap.SetResolution(resolucion, resolucion);
        //        //Hacemos los cambios a ImagenBitmap usando a ImagenGraphics y la Imagen Original(Imagen)
        //        //ImagenBitmap se comporta como un objeto de referenciado
        //        using (Graphics imagenGraphics = Graphics.FromImage(imagenBitmap))
        //        {
        //            imagenGraphics.SmoothingMode = SmoothingMode.AntiAlias;
        //            imagenGraphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
        //            imagenGraphics.PixelOffsetMode = PixelOffsetMode.HighQuality;
        //            imagenGraphics.DrawImage(Imagen, new Rectangle(0, 0, Ancho, Alto), new Rectangle(0, 0, Imagen.Width, Imagen.Height), GraphicsUnit.Pixel);
        //            //todos los cambios hechos en imagenBitmap lo llevaremos un Image(Imagen) con nuevos datos a travez de un MemoryStream
        //            MemoryStream imagenMemoryStream = new MemoryStream();
        //            imagenBitmap.Save(imagenMemoryStream, ImageFormat.Jpeg);
        //            Imagen = Image.FromStream(imagenMemoryStream);
        //        }
        //    }
        //    return Imagen;
        //}

        public static string obtieneExtensionArchivo(string archivo)
        {
            string cadenaCortada = "";
            string tipoCorte = ".";
            int contador = 0;

            for (int i = (archivo.Length - 1); i > 0; i--)
            {

                string dato = archivo.Substring(i, 1);
                string dato2 = archivo.Substring(1, 1);

                if (tipoCorte.Equals(archivo.Substring(i, 1)))
                {
                    cadenaCortada = archivo.Substring(i + 1, contador);
                    i = 0;
                }

                contador++;
            }

            return cadenaCortada;
        }

        public static bool validaArchivo(string[] tiposArchivos, string extension_AValidar)
        {
            bool valido = false;

            for (int i = 0; i < tiposArchivos.Count(); i++)
            {
                if (tiposArchivos.ElementAt(i).Equals(extension_AValidar))
                    valido = true;
            }

            return valido;
        }

        public static bool validarFormato_Documento(int numDigitosInicio, int numDigitosTermino, string numDocumento, bool soloNumeros)
        {
            bool DocValido = false;

            if (numDigitosTermino > 0)
            {
                if (soloNumeros)
                {
                    try
                    {
                        Convert.ToDecimal(numDocumento);
                        if (numDocumento.Length >= numDigitosInicio && numDocumento.Length <= numDigitosTermino)
                            DocValido = true;
                    }
#pragma warning disable CS0168 // La variable 'ex' se ha declarado pero nunca se usa
                    catch (FormatException ex)
#pragma warning restore CS0168 // La variable 'ex' se ha declarado pero nunca se usa
                    {
                    }
#pragma warning disable CS0168 // La variable 'e' se ha declarado pero nunca se usa
                    catch (OverflowException e)
#pragma warning restore CS0168 // La variable 'e' se ha declarado pero nunca se usa
                    {
                    }
                }
                else
                {
                    if (numDocumento.Length >= numDigitosInicio && numDocumento.Length <= numDigitosTermino)
                        DocValido = true;
                }
            }

            if (numDigitosTermino == 0)
            {
                if (numDocumento.Length > 0)
                    DocValido = false;
                else
                    DocValido = true;
            }

            return DocValido;
        }

        public static string devuelveMensajeFormato_Documento(int numDigitosInicio, int numDigitosTermino, bool soloNumeros)
        {
            string textoSoloNumeros = "";
            string textoMensaje = "El campo requiere un número de ";

            if (numDigitosTermino == 0)
                textoMensaje = "La configuración de este campo, no requiere ingreso de dato ";
            else
            {
                if (soloNumeros)
                    textoSoloNumeros = " de solo Números";

                if (numDigitosInicio == numDigitosTermino)
                    textoMensaje += numDigitosInicio + " Digito(s)" + textoSoloNumeros;
                else
                    textoMensaje += "de " + numDigitosInicio + " a " + numDigitosTermino + " Digito(s)" + textoSoloNumeros;
            }

            return textoMensaje;
        }

        public static bool validarMayoria_Edad(DateTime fechaNac)
        {
            bool Valido = false;

            TimeSpan valor = DateTime.Now - fechaNac;

            DateTime d1 = fechaNac;
            DateTime d2 = DateTime.Now;

            TimeSpan ts = d2 - d1;
            DateTime d = DateTime.MinValue + ts;

            int dias = d.Day - 1;
            int meses = d.Month - 1;
            int anios = d.Year - 1;

            if (anios >= 18)
                Valido = true;


            return Valido;
        }

        public static bool validar_Fecha(string txtFecha)
        {
            bool valido = true;

            try
            {
                Convert.ToDateTime(txtFecha);
            }
            catch (System.FormatException)
            {
                valido = false;
            }

            return valido;
        }

        public static string TraerNumeroAlfabeticamente(int i)
        {
            return TraerNumeroAlfabeticamente(i, string.Empty);
        }

        private static string TraerNumeroAlfabeticamente(int i, string s)
        {
            var rem = i % 26;
            s = (char)((int)'A' + rem) + s;
            i = i / 26 - 1;
            return i < 0 ? s : TraerNumeroAlfabeticamente(i, s);
        }

        public static int restaDosFechas(DateTime fechaUno, DateTime fechaDos)
        {
            TimeSpan difer = fechaUno - fechaDos;

            return difer.Days;
        }

        public static string agregarEspaciosEnBlanco(int cantEspacios)
        {
            string cadena = "";

            for (int i = 0; i < cantEspacios; i++)
            {
                cadena = cadena + " ";
            }

            return cadena;
        }

        public static string agregarEspaciosEnBlanco(int maxTamanio, string palabara)
        {
            string cadena = "";

            if (palabara.Length > maxTamanio)
                cadena = palabara.Substring(1, maxTamanio);
            else
            {

                for (int i = 0; i < (maxTamanio - palabara.Length); i++)
                {
                    cadena = cadena + " ";
                }

                cadena = palabara + cadena;
            }
            return cadena;
        }

        public static string devuelveSoloNumerosCadena(string cadena)
        {
            string devolver = "";

            for (int i = 0; i < cadena.Length; i++)
            {
                if (cadena.Substring(i, 1).Equals("1") || cadena.Substring(i, 1).Equals("6") ||
                    cadena.Substring(i, 1).Equals("2") || cadena.Substring(i, 1).Equals("7") ||
                    cadena.Substring(i, 1).Equals("3") || cadena.Substring(i, 1).Equals("8") ||
                    cadena.Substring(i, 1).Equals("4") || cadena.Substring(i, 1).Equals("9") ||
                    cadena.Substring(i, 1).Equals("5") || cadena.Substring(i, 1).Equals("0")
                    )
                    devolver = devolver + cadena.Substring(i, 1);
            }

            return devolver;
        }
    }



    public class Anio
    {
        public int indice { get; set; }
        public string nombre { get; set; }
    }

    public class Mes
    {
        public int indice { get; set; }
        public string nombre { get; set; }
    }

    public class Dia
    {
        public int codDia { get; set; }
        public string nomDia { get; set; }
        public int posicion { get; set; }
    }
}
