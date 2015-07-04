using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace sarey_erp.Models
{
    public class formatearString
    {

        public string valoresPesos(string valor) {

            string retorno = "";

            char[] caracteres = valor.ToCharArray();

            for (int i = caracteres.Length - 1; i >= 0; i--) 
            {
                if (i == caracteres.Length - 3)
                {
                    retorno = "." + caracteres[i] + retorno;
                }
                else if (i == caracteres.Length - 6)
                {
                    retorno = "." + caracteres[i] + retorno;
                }
                else if (i == caracteres.Length - 9)
                {
                    retorno = "." + caracteres[i] + retorno;
                }
                else if (i == caracteres.Length - 12)
                {
                    retorno = "." + caracteres[i] + retorno;
                }
                else if (i == caracteres.Length - 15)
                {
                    retorno = "." + caracteres[i] + retorno;
                }
                else if (i == caracteres.Length - 18)
                {
                    retorno = "." + caracteres[i] + retorno;
                }
                else if (i == caracteres.Length - 21)
                {
                    retorno = "." + caracteres[i] + retorno;
                }
                else {
                    retorno = caracteres[i] + retorno;
                }
            }

            if (retorno.StartsWith(".")) retorno = retorno.TrimStart('.');

            return retorno;
        }

        public string valoresPesos(int valor)
        {
            return valoresPesos(valor.ToString());
        }

        public string valoresPesos(double valor)
        {
            return valoresPesos(valor.ToString());
        }

        public string valoresPesos(float valor)
        {
            return valoresPesos(valor.ToString());
        }
    }
}