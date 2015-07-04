using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.OleDb;
using System.Data;
using System.Data.SqlClient;
using sarey_erp.Models;
using System.Globalization;

namespace sarey_erp.Models
{
    public class parserExcel
    {

        public void leerPresupuesto(DataSet ds, string id_faena, string centroCosto, List<string> listaPartidas, SqlConnection cnx, string nombreHoja)
        {
            int i = 0;
           
                int numero_partida = 0;
                string nombre_partida = "";
                int verificador_espacios = 0;
                string partida = "";
                
                string nombre_partida_global = "";
                while (i < ds.Tables[0].Rows.Count - 1)
                {
                    if (i == 94) { 
                    
                    }
                    string campo_0 = Convert.ToString(ds.Tables[0].Rows[i].ItemArray[0]);
                    string campo_1 = Convert.ToString(ds.Tables[0].Rows[i].ItemArray[1]);
                    string campo_2 = Convert.ToString(ds.Tables[0].Rows[i].ItemArray[2]);

                    if (campo_0.Equals("") && campo_1.Equals(""))
                    {
                        verificador_espacios++;

                        if (verificador_espacios > 1)
                        {
                            throw new System.InvalidOperationException("Hay mas de un salto de linea en la linea " + (i + 2) + " de la hoja " + nombreHoja);
                        }
                        nombre_partida = campo_1;
                    }


                    //GUARDAR FAENA                           
                    if (campo_1.Equals("TOTAL"))
                    {
                        verificador_espacios = 0;
                        faena NuevaFaena = new faena();
                        NuevaFaena.nombre = id_faena;
                        NuevaFaena.centro_costo = centroCosto;

                        NuevaFaena.partida_presupuesto = Math.Round(Convert.ToDouble(ds.Tables[0].Rows[i - 5].ItemArray[5]), MidpointRounding.AwayFromZero);
                        NuevaFaena.valor_neto = Math.Round(Convert.ToDouble(ds.Tables[0].Rows[i - 5].ItemArray[5]), MidpointRounding.AwayFromZero);
                        NuevaFaena.gastos_generales = Math.Round(Convert.ToDouble(ds.Tables[0].Rows[i - 4].ItemArray[5]), MidpointRounding.AwayFromZero);
                        NuevaFaena.subtotal = Math.Round(Convert.ToDouble(ds.Tables[0].Rows[i - 2].ItemArray[5]), MidpointRounding.AwayFromZero);
                        NuevaFaena.utilidades = Math.Round(Convert.ToDouble(ds.Tables[0].Rows[i - 3].ItemArray[5]), MidpointRounding.AwayFromZero);
                        NuevaFaena.iva = Math.Round(Convert.ToDouble(ds.Tables[0].Rows[i - 1].ItemArray[5]), MidpointRounding.AwayFromZero);
                        NuevaFaena.guardarFaena();
                        i = ds.Tables[0].Rows.Count;
                        break;
                    }

                    //GUARDAR PARTIDA GLOBAL
                    if (!campo_1.Equals("") && campo_2.Equals(""))
                    {
                        verificador_espacios = 0;
                        numero_partida++;
                        int numero = numero_partida;
                        if (campo_0.Equals(""))
                        {
                            partida = EnteroARomano(numero);
                        }
                        else
                        {
                            partida = formatearCadena(campo_0);
                        }

                        nombre_partida_global = campo_1;
                        nombre_partida = campo_1;
                        partida PartidaGlobal = new partida();
                        PartidaGlobal.guardarPartidaGlobal(partida, nombre_partida_global, id_faena);


                    }


                    //GUARDAR PARTIDA 
                    if (!campo_1.Equals("") && !campo_2.Equals(""))
                    {
                        verificador_espacios = 0;
                        partida NuevaPartida = new partida();
                        NuevaPartida.id_faena = id_faena;
                        NuevaPartida.id_partida = partida + "." + formatearCadena(Convert.ToString(ds.Tables[0].Rows[i].ItemArray[0]));
                        NuevaPartida.descripcion = formatearCadena(Convert.ToString(ds.Tables[0].Rows[i].ItemArray[1]));// +"/" + nombre_partida; 
                        NuevaPartida.unidad = formatearCadena(Convert.ToString(ds.Tables[0].Rows[i].ItemArray[2])); //5
                        NuevaPartida.cantidad = Convert.ToDouble(ds.Tables[0].Rows[i].ItemArray[3]);    //6
                        NuevaPartida.precio_unitario = Convert.ToDouble(ds.Tables[0].Rows[i].ItemArray[4]);//7
                        NuevaPartida.total = Convert.ToDouble(ds.Tables[0].Rows[i].ItemArray[5]);//8
                        NuevaPartida.id_partida_global = partida;
                        listaPartidas.Add(NuevaPartida.id_partida);
                        NuevaPartida.guardarPartida(cnx);

                    }

                    i++;
                }
            
            
        }


        public void leerPrecioUnitario(DataSet ds, string id_faena, List<string> listaPartidas, SqlConnection cnx, string nombreHoja)
        {



            int i = 0;
          
                int codigo = 0;
                int subcodigo = 0;
                string codigotemporal = "";
                string codigoActual = "";
                string codigo_partida = "";
                string subcodigo_partida = "";
                int verificador_espacios = 0;
                int numeroItem = 0;
                string Error = "";
                string nombre_partida = "";
                while (i < ds.Tables[0].Rows.Count - 1)
                {

                    string campo_0 = Convert.ToString(ds.Tables[0].Rows[i].ItemArray[0]);
                    string campo_1 = Convert.ToString(ds.Tables[0].Rows[i].ItemArray[1]);
                    string campo_2 = Convert.ToString(ds.Tables[0].Rows[i].ItemArray[2]);
                    string campo_3 = Convert.ToString(ds.Tables[0].Rows[i].ItemArray[3]);


                    if (campo_1.Equals("ITEM"))
                    {
                        char[] CharDelete = { '-', '.', ',', '$', ' ' };
                        string descripcion_item = formatearCadena(Convert.ToString(ds.Tables[0].Rows[i].ItemArray[2]).TrimEnd(CharDelete));


                        string codigo_partida_actual = formatearCadena(Convert.ToString(ds.Tables[0].Rows[i + 1].ItemArray[2]).TrimEnd(CharDelete));
                        string codigo_item_actual = formatearCadena(Convert.ToString(ds.Tables[0].Rows[i + 2].ItemArray[2]).TrimEnd(CharDelete));
                        string partidaBuscar = codigo_partida_actual + "." + codigo_item_actual;// +"." + descripcion_item;
                        if (!listaPartidas.Contains(partidaBuscar))
                        {
                            Error = "No se encuentra la partida:" + partidaBuscar + " en la hoja de  Presupuesto, solo esta en hoja Precio Unitario en la linea " + (i + 2);
                            throw new System.InvalidOperationException("No se encuentra la partida:" + partidaBuscar + " en la hoja de Presupuesto, solo esta en hoja Precio Unitario, en la linea " + (i + 2));

                        }
                        else
                        {
                            listaPartidas.Remove(partidaBuscar);
                            Console.Write("");

                        }

                    }


                    if (campo_0.Equals("") && (!campo_1.Equals("") && !campo_3.Equals("")))
                    {

                        throw new System.InvalidOperationException("Falta asignar tipo de Item en la hoja de Precio Unitario, en la linea " + (i + 2));
                    }

                    if (campo_0.Equals("") && campo_1.Equals(""))
                    {
                        verificador_espacios++;
                        campo_2 = formatearCadena(Convert.ToString(ds.Tables[0].Rows[i].ItemArray[2]));
                        if (campo_2.Equals("RESUMEN GLOBAL") || campo_2.Equals("Resumen Global"))
                        {

                            i = ds.Tables[0].Rows.Count;
                            break;
                        }

                        else if (verificador_espacios > 1 && campo_3.Equals(""))
                        {
                            Error = "Hay mas de un salto de linea en la linea" + (i + 2) + " de la hoja " + nombreHoja;
                            throw new System.InvalidOperationException("Hay mas de un salto de linea en la linea" + (i + 2) + " de la hoja " + nombreHoja);

                        }

                        nombre_partida = campo_1;
                        i = i + 1;
                    }


                    if (!Convert.ToString(ds.Tables[0].Rows[i].ItemArray[2]).Equals("RESUMEN GLOBAL") && !Convert.ToString(ds.Tables[0].Rows[i].ItemArray[2]).Equals("MATERIALES"))
                    {


                        if (campo_1.Equals("CODIGO"))
                        {
                            numeroItem = 0;
                            verificador_espacios = 0;
                            codigoActual = formatearCadena(Convert.ToString(ds.Tables[0].Rows[i].ItemArray[2]));

                            char[] CharDelete = { '-', '.', ',', '$' };
                            codigo_partida = formatearCadena(Convert.ToString(ds.Tables[0].Rows[i].ItemArray[2]).TrimEnd(CharDelete));

                            if (!codigoActual.Equals(codigotemporal))
                            {
                                codigotemporal = codigoActual;
                                codigo++;
                                subcodigo = 0;
                            }
                        }
                        else if (campo_1.Equals("SUBCODIGO"))
                        {
                            char[] CharDelete = { ' ', '-', '.', ',', '$' };

                            subcodigo_partida = formatearCadena(Convert.ToString(ds.Tables[0].Rows[i].ItemArray[2]).TrimEnd(CharDelete));
                            verificador_espacios = 0;
                            subcodigo++;
                            i = i + 3;
                        }

                        //GUARDAR ITEM
                        else if (!campo_2.Equals("") && !campo_3.Equals(""))
                        {
                            verificador_espacios = 0;
                            items NuevoItem = new items();
                            NuevoItem.nombre = formatearCadena(Convert.ToString(ds.Tables[0].Rows[i].ItemArray[2]));
                            NuevoItem.unidad = formatearCadena(Convert.ToString(ds.Tables[0].Rows[i].ItemArray[3]));
                            string cant = formatearCadena(Convert.ToString(ds.Tables[0].Rows[i].ItemArray[4]));
                            NuevoItem.cantidad = Math.Round(Convert.ToDouble(ds.Tables[0].Rows[i].ItemArray[4]), 3);

                            NuevoItem.precio_unitario = Convert.ToDouble(Convert.ToString(ds.Tables[0].Rows[i].ItemArray[5]));
                            NuevoItem.costo_unitario = Math.Round(Convert.ToDouble(ds.Tables[0].Rows[i].ItemArray[6]), MidpointRounding.ToEven); //Toeven
                            NuevoItem.cantidad_comprar = Math.Round(Convert.ToDouble(ds.Tables[0].Rows[i].ItemArray[7]), 2);
                            NuevoItem.presupuesto_compra = Math.Round(Convert.ToDouble(ds.Tables[0].Rows[i].ItemArray[8]), MidpointRounding.ToEven);
                            NuevoItem.tipo = formatearCadena(Convert.ToString(ds.Tables[0].Rows[i].ItemArray[0]));
                            try
                            {
                                NuevoItem.numero = Convert.ToInt32(ds.Tables[0].Rows[i].ItemArray[1]);
                            }
                            catch (Exception ex)
                            {
                                numeroItem++;
                                NuevoItem.numero = numeroItem;
                            }

                            char[] CharDelete = { ' ', '-', '.', ',', '$' };
                            subcodigo_partida = subcodigo_partida.TrimEnd(CharDelete);
                            string id_partida = codigo_partida + "." + subcodigo_partida;
                            NuevoItem.guardarNuevoItem(id_partida, id_faena, cnx);
                        }
                        else
                        {
                            verificador_espacios++;

                        }

                    }
                    else
                    {
                        if (listaPartidas.Count > 0)
                        {
                            for (int x = 0; x < listaPartidas.Count; x++)
                            {

                                Error = Error + "\nLa partida " + listaPartidas[x] + " esta solo en la hoja de Presupuesto. No existe en la hoja Precio Unitario";
                            }

                            throw new System.InvalidOperationException("Hay mas de un salto de linea en la linea" + (i + 2) + " de la hoja " + nombreHoja);

                        }
                        i = ds.Tables[0].Rows.Count;
                    }
                    i++;
                }

           
        }













        string EnteroARomano(int i)
        {

            int[] valor ={ 1000, 900, 500, 400, 100, 90,
                  50, 40, 10, 9, 5, 4, 1 };
            string[] simbolo ={ "M", "CM", "D", "CD", "C",
                  "XC", "L", "XL", "X", 
                  "IX", "V", "IV", "I"};
            string r = "";
            int p = 0;
            if (i >= 1 && i < 4000)
            {
                int x = i;
                while (x > 0)
                {
                    while (x >= valor[p])
                    {
                        r += simbolo[p];
                        x = x - valor[p];
                    }

                    p++;
                } //while
            }//if
            return r;
        }

        string formatearCadena(string cadena)
        {

            char[] charsToTrim = { '-', '.', ' ' };
            string retorno = cadena.TrimEnd(charsToTrim);
            retorno = retorno.TrimEnd(charsToTrim);

            while (retorno.Contains(";"))
            {
                retorno = retorno.Replace(";", "");
            }
            while (retorno.Contains("/"))
            {
                retorno = retorno.Replace("/", "}");
            }
            while (retorno.Contains("\""))
            {
                retorno = retorno.Replace("\"", "");
            }

            return retorno;
        }
    }
}