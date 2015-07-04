using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Data;

namespace sarey_erp.Models
{
    public class usointerno_oc
    {
        public string id_orden { get; set; }
        public string faena { get; set; }
        public string comuna { get; set; }
        public string observacion { get; set; }
        public string item { get; set; }
        public string presupuesto { get; set; }
        public string oc { get; set; }
        public string saldo { get; set; }
        public string revisadoPor { get; set; }


        public static void agregar_usoInterno(usointerno_oc nuevo)
        {
            SqlConnection cnx = conexion.crearConexion();

            SqlCommand cmd = new SqlCommand();
            cmd.Connection = cnx;
            cmd.CommandText = "INSERT INTO uso_interno_oc VALUES(@id_orden_compra,@faena,"
                + "@comuna,@observacion,@item_cant,@presupuesto,@valor_oc,@saldo,@revisado)";

            cmd.Parameters.Add("@id_orden_compra", SqlDbType.VarChar).Value = nuevo.id_orden;
            cmd.Parameters.Add("@faena", SqlDbType.VarChar).Value = nuevo.faena;
            cmd.Parameters.Add("@comuna", SqlDbType.VarChar).Value = nuevo.comuna;
            cmd.Parameters.Add("@observacion", SqlDbType.VarChar).Value = nuevo.observacion;
            cmd.Parameters.Add("@item_cant", SqlDbType.VarChar).Value = nuevo.item;
            cmd.Parameters.Add("@presupuesto", SqlDbType.VarChar).Value = nuevo.presupuesto;
            cmd.Parameters.Add("@valor_oc", SqlDbType.VarChar).Value = nuevo.oc;
            cmd.Parameters.Add("@saldo", SqlDbType.VarChar).Value = nuevo.saldo;
            cmd.Parameters.Add("@revisado", SqlDbType.VarChar).Value = nuevo.revisadoPor;          


            cmd.CommandType = CommandType.Text;
            cmd.ExecuteNonQuery();
            cnx.Close();
        }
        public static usointerno_oc obtenerUsointerno(string id_compra)
        {
            usointerno_oc detalleinterno = new usointerno_oc();

            SqlConnection cnx = conexion.crearConexion();

            SqlCommand cmd = new SqlCommand();
            cmd.Connection = cnx;
            cmd.CommandText = "SELECT * from uso_interno_oc WHERE id_orden_compra='" + id_compra + "'";
            cmd.CommandType = CommandType.Text;
            SqlDataReader dr = cmd.ExecuteReader();

            while (dr.Read())
            {
                detalleinterno.id_orden = (string)dr["id_orden_compra"];
                detalleinterno.faena = (string)dr["faena"];
                detalleinterno.comuna = (string)dr["comuna"];
                detalleinterno.observacion = (string)dr["observacion"];
                detalleinterno.item = (string)dr["item_cant"];
                detalleinterno.presupuesto = (string)dr["presupuesto"];
                detalleinterno.oc = (string)dr["valor_oc"];
                detalleinterno.saldo = (string)dr["saldo"];
                detalleinterno.revisadoPor = (string)dr["revisado"];
            }
            cnx.Close();

            return detalleinterno; ;
        }

    }
}