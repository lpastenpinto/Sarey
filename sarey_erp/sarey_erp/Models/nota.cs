using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Data;

namespace sarey_erp.Models
{
    public class nota
    {
        public string donde { get; set; }
        public string dir_entrega { get; set; }
        public string id_numero_orden { get; set; }
        public string transporte { get; set; }
        public string cuenta { get; set; }
        public string plazo_entrega { get; set; }

        public static void agregarNota(nota nuevo)
        {
            SqlConnection cnx = conexion.crearConexion();

            SqlCommand cmd = new SqlCommand();
            cmd.Connection = cnx;
            cmd.CommandText = "INSERT INTO notas VALUES(@direccion_entrega,@id_orden_compra,"
                + "@transporte_cuenta,@forma_pago,@plazo_entrega,@lugar_entrega)";

            cmd.Parameters.Add("@direccion_entrega", SqlDbType.VarChar).Value = nuevo.dir_entrega;
            cmd.Parameters.Add("@id_orden_compra", SqlDbType.VarChar).Value = nuevo.id_numero_orden;
            cmd.Parameters.Add("@transporte_cuenta", SqlDbType.VarChar).Value = nuevo.transporte;
            cmd.Parameters.Add("@forma_pago", SqlDbType.VarChar).Value = nuevo.cuenta;
            cmd.Parameters.Add("@plazo_entrega", SqlDbType.VarChar).Value = nuevo.plazo_entrega;
            cmd.Parameters.Add("@lugar_entrega", SqlDbType.VarChar).Value = nuevo.donde;
         

            cmd.CommandType = CommandType.Text;
            cmd.ExecuteNonQuery();
            cnx.Close();
        }
        public static nota obtenerNota(string id_compra)
        {
            nota detallenota = new nota();

            SqlConnection cnx = conexion.crearConexion();

            SqlCommand cmd = new SqlCommand();
            cmd.Connection = cnx;
            cmd.CommandText = "SELECT * from notas WHERE id_orden_compra='" + id_compra + "'";
            cmd.CommandType = CommandType.Text;
            SqlDataReader dr = cmd.ExecuteReader();

            while (dr.Read())
            {
                detallenota.id_numero_orden = (string)dr["id_orden_compra"];
                detallenota.donde = (string)dr["lugar_entrega"];
                detallenota.dir_entrega = (string)dr["direccion_entrega"];
                detallenota.transporte = (string)dr["transporte_cuenta"];
                detallenota.cuenta = (string)dr["forma_pago"];
                detallenota.plazo_entrega = (string)dr["plazo_entrega"];
            }
            cnx.Close();

            return detallenota;
        }
    }
}