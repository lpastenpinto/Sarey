using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace sarey_erp.Models
{
    public class datosResumenFaena
    {
        public string nombre { get; set; }
        public string tipo { get; set; }
        public int total { get; set; }

        public static List<datosResumenFaena> obtenerDatosGlobales(string nombreFaena)
        {
            List<datosResumenFaena> retorno = new List<datosResumenFaena>();

            SqlConnection cnx = conexion.crearConexion();

            SqlCommand cmd = new SqlCommand();
            cmd.Connection = cnx;
            cmd.CommandText = "SELECT * from items WHERE id_faena = '" + nombreFaena + "' ORDER BY nombre ASC";
            cmd.CommandType = CommandType.Text;
            SqlDataReader dr = cmd.ExecuteReader();

            while (dr.Read())
            {
                bool encontrado = false;
                int posicionEncontrado = 0;

                for (int i = 0; i < retorno.Count; i++) 
                {
                    if (retorno[i].tipo.Equals((string)dr["tipo"])) 
                    {
                        encontrado = true;
                        posicionEncontrado = i;
                    }
                }
                if (encontrado)
                {
                    retorno[posicionEncontrado].total += int.Parse(dr["presupuesto_compra"].ToString());
                }
                else
                {
                    datosResumenFaena Item = new datosResumenFaena();
                    Item.tipo = (string)dr["tipo"];
                    if (Item.tipo.Equals("MAT"))
                    {
                        Item.nombre = "MATERIALES";
                    }
                    else if (Item.tipo.Equals("MAQ"))
                    {
                        Item.nombre = "MAQUINARIA";
                    }
                    else if (Item.tipo.Equals("MO"))
                    {
                        Item.nombre = "MANO DE OBRA";
                    }
                    else if (Item.tipo.Equals("LS"))
                    {
                        Item.nombre = "LEYES SOCIALES";
                    }
                    else if (Item.tipo.Equals("SUBC"))
                    {
                        Item.nombre = "SUBCONTRATO";
                    }
                    Item.total = int.Parse(dr["presupuesto_compra"].ToString());
                    retorno.Add(Item);
                }
            }
            cnx.Close();

            return retorno;
        }
    }
}