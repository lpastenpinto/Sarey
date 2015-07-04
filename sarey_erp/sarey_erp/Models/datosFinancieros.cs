using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace sarey_erp.Models
{
    public class datosFinancieros
    {

        public static string obtenerDatosAFP()
        {
            string retorno = "";

            var url = "http://www.previred.com/web/previred/indicadores-previsionales";
            string textFromFile = (new System.Net.WebClient()).DownloadString(url);

            HtmlAgilityPack.HtmlDocument htmlDoc = new HtmlAgilityPack.HtmlDocument();

            htmlDoc.LoadHtml(textFromFile);

            if (htmlDoc.ParseErrors != null && htmlDoc.ParseErrors.Count() > 0)
            {
                // Handle any parse errors as required

            }
            else
            {

                if (htmlDoc.DocumentNode != null)
                {
                    HtmlNode body = htmlDoc.DocumentNode.SelectSingleNode("//body");

                    if (body != null)
                    {
                        HtmlNodeCollection td = body.SelectNodes("//td");

                        int contadorLeer = 0;

                        for (int i = 0; i < td.Count; i++)
                        {
                            if (td[i].InnerText == "Capital" || td[i].InnerText == "Cuprum" || td[i].InnerText == "Habitat"
                                 || td[i].InnerText == "PlanVital" || td[i].InnerText == "Modelo" || td[i].InnerText == "ProVida")
                            {
                                retorno += ";" + td[i].InnerText;
                                contadorLeer = 3;
                            }
                            else if (contadorLeer > 0)
                            {                                                                   
                                retorno += "|" + td[i].InnerText;
                                contadorLeer--;
                            }
                        }

                        return retorno.TrimStart(';');
                    }
                }
            }
            return retorno;
        }

        public static string obtenerDatosAsignacionFamiliar()
        {
            string retorno = "";

            var url = "http://www.previred.com/web/previred/indicadores-previsionales";
            string textFromFile = (new System.Net.WebClient()).DownloadString(url);

            HtmlAgilityPack.HtmlDocument htmlDoc = new HtmlAgilityPack.HtmlDocument();

            htmlDoc.LoadHtml(textFromFile);

            if (htmlDoc.ParseErrors != null && htmlDoc.ParseErrors.Count() > 0)
            {
                // Handle any parse errors as required

            }
            else
            {

                if (htmlDoc.DocumentNode != null)
                {
                    HtmlNode body = htmlDoc.DocumentNode.SelectSingleNode("//body");

                    if (body != null)
                    {
                        HtmlNodeCollection td = body.SelectNodes("//td");

                        int contadorLeer = 0;

                        for (int i = 0; i < td.Count; i++)
                        {
                            if (td[i].InnerText == "A" || td[i].InnerText == "B" 
                                || td[i].InnerText == "C" || td[i].InnerText == "D")
                            {
                                retorno += "!" + td[i].InnerText;
                                contadorLeer = 2;
                            }
                            else if (contadorLeer > 0)
                            {
                                retorno += "|" + td[i].InnerText;
                                contadorLeer--;
                            }
                        }

                        return retorno.TrimStart('!');
                    }
                }
            }
            return retorno;
        }

        public static string obtenerDatosSeguroCesantia()
        {
            string retorno = "";

            var url = "http://www.previred.com/web/previred/indicadores-previsionales";
            string textFromFile = (new System.Net.WebClient()).DownloadString(url);

            HtmlAgilityPack.HtmlDocument htmlDoc = new HtmlAgilityPack.HtmlDocument();

            htmlDoc.LoadHtml(textFromFile);

            if (htmlDoc.ParseErrors != null && htmlDoc.ParseErrors.Count() > 0)
            {
                // Handle any parse errors as required

            }
            else
            {

                if (htmlDoc.DocumentNode != null)
                {
                    HtmlNode body = htmlDoc.DocumentNode.SelectSingleNode("//body");

                    if (body != null)
                    {
                        HtmlNodeCollection td = body.SelectNodes("//td");

                        int contadorLeer = 0;

                        for (int i = 0; i < td.Count; i++)
                        {
                            if (td[i].InnerText == "Contrato Plazo Indefinido" || td[i].InnerText == "Contrato Plazo Fijo"
                                || td[i].InnerText == "Contrato Plazo Indefinido 11 aÃ±os o mÃ¡s (**)")
                            {
                                if (td[i].InnerText == "Contrato Plazo Indefinido 11 aÃ±os o mÃ¡s (**)")
                                    retorno += "!Contrato Plazo Indefinido 11 años o más";
                                else
                                    retorno += "!" + td[i].InnerText;
                                contadorLeer = 2;
                            }
                            else if (contadorLeer > 0)
                            {
                                retorno += "|" + td[i].InnerText;
                                contadorLeer--;
                            }
                        }

                        return retorno.TrimStart('!');
                    }
                }
            }
            return retorno;
        }

        public static string obtenerDatosRentasMinimas()
        {
            string retorno = "";

            var url = "http://www.previred.com/web/previred/indicadores-previsionales";
            string textFromFile = (new System.Net.WebClient()).DownloadString(url);

            HtmlAgilityPack.HtmlDocument htmlDoc = new HtmlAgilityPack.HtmlDocument();

            htmlDoc.LoadHtml(textFromFile);

            if (htmlDoc.ParseErrors != null && htmlDoc.ParseErrors.Count() > 0)
            {
                // Handle any parse errors as required

            }
            else
            {

                if (htmlDoc.DocumentNode != null)
                {
                    HtmlNode body = htmlDoc.DocumentNode.SelectSingleNode("//body");

                    if (body != null)
                    {
                        HtmlNodeCollection td = body.SelectNodes("//td");

                        int contadorLeer = 0;

                        for (int i = 0; i < td.Count; i++)
                        {
                            if (td[i].InnerText == "Trab. Dependientes e Independientes:"
                                || td[i].InnerText == "Menores de 18 y Mayores de 65:"
                                || td[i].InnerText == "Trabajadores de Casa Particular:")
                            {   
                                retorno += "!" + td[i].InnerText;
                                contadorLeer = 1;
                            }
                            else if (contadorLeer > 0)
                            {
                                retorno += "|" + td[i].InnerText;
                                contadorLeer--;
                            }
                        }

                        return retorno.TrimStart('!');
                    }
                }
            }
            return retorno;
        }

        public static string obtenerDatosRentasTopes()
        {
            string retorno = "";

            var url = "http://www.previred.com/web/previred/indicadores-previsionales";
            string textFromFile = (new System.Net.WebClient()).DownloadString(url);

            HtmlAgilityPack.HtmlDocument htmlDoc = new HtmlAgilityPack.HtmlDocument();

            htmlDoc.LoadHtml(textFromFile);

            if (htmlDoc.ParseErrors != null && htmlDoc.ParseErrors.Count() > 0)
            {
                // Handle any parse errors as required

            }
            else
            {

                if (htmlDoc.DocumentNode != null)
                {
                    HtmlNode body = htmlDoc.DocumentNode.SelectSingleNode("//body");

                    if (body != null)
                    {
                        HtmlNodeCollection td = body.SelectNodes("//td");

                        int contadorLeer = 0;

                        for (int i = 0; i < td.Count; i++)
                        {
                            if (td[i].InnerText == "Para afiliados a una AFP (73,2 UF):"
                                || td[i].InnerText == "Para afiliados al IPS (ex INP) (60 UF):"
                                || td[i].InnerText == "Para Seguro de CesantÃ­a (109,8 UF):")
                            {
                                if (td[i].InnerText == "Para Seguro de CesantÃ­a (109,8 UF):") 
                                {
                                    retorno += "!Para Seguro de Cesantía (109,8 UF):";
                                }
                                else
                                    retorno += "!" + td[i].InnerText;

                                contadorLeer = 1;
                            }
                            else if (contadorLeer > 0)
                            {
                                retorno += "|" + td[i].InnerText;
                                contadorLeer--;
                            }
                        }

                        return retorno.TrimStart('!');
                    }
                }
            }
            return retorno;
        }

        public static string obtenerDatosAPV()
        {
            string retorno = "";

            var url = "http://www.previred.com/web/previred/indicadores-previsionales";
            string textFromFile = (new System.Net.WebClient()).DownloadString(url);

            HtmlAgilityPack.HtmlDocument htmlDoc = new HtmlAgilityPack.HtmlDocument();

            htmlDoc.LoadHtml(textFromFile);

            if (htmlDoc.ParseErrors != null && htmlDoc.ParseErrors.Count() > 0)
            {
                // Handle any parse errors as required

            }
            else
            {

                if (htmlDoc.DocumentNode != null)
                {
                    HtmlNode body = htmlDoc.DocumentNode.SelectSingleNode("//body");

                    if (body != null)
                    {
                        HtmlNodeCollection td = body.SelectNodes("//td");

                        int contadorLeer = 0;

                        for (int i = 0; i < td.Count; i++)
                        {
                            if (td[i].InnerText == "Tope Mensual (50 UF):"
                                || td[i].InnerText == "Tope Anual (600 UF):")
                            {
                                retorno += "!" + td[i].InnerText;
                                contadorLeer = 1;
                            }
                            else if (contadorLeer > 0)
                            {
                                retorno += "|" + td[i].InnerText;
                                contadorLeer--;
                            }
                        }

                        return retorno.TrimStart('!');
                    }
                }
            }
            return retorno;
        }
    }
}