using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Data;

namespace sarey_erp.Models
{
    public class detalle_ordencompra
    {
        public string id_numero_orden { get; set; }
        public string id_item { get; set; }
        public int precio_unitario { get; set; }
        public int cantidad_item { get; set; }
        public string unidad { get; set; }
        public string id_faena { get; set; }
        public string id_partida { get; set; }
        public int numero_item_partida { get; set; }
        public string nombre_item_partida { get; set; }
        public string id_solicitud { get; set; }

        public static List<detalle_ordencompra> obtenerTodas(string id_compra)
        {
            List<detalle_ordencompra> retorno = new List<detalle_ordencompra>();

            SqlConnection cnx = conexion.crearConexion();

            SqlCommand cmd = new SqlCommand();
            cmd.Connection = cnx;
            cmd.CommandText = "SELECT * from detalle_orden_compra WHERE id_orden_compra='" + id_compra + "'";
            cmd.CommandType = CommandType.Text;
            SqlDataReader dr = cmd.ExecuteReader();

            while (dr.Read())
            {
                detalle_ordencompra detallecompra = new detalle_ordencompra();
                detallecompra.id_numero_orden = (string)dr["id_orden_compra"];
                detallecompra.id_item = (string)dr["id_item"];
                detallecompra.precio_unitario = (int)dr["precio_unitario"];
                detallecompra.cantidad_item = (int)dr["cantidad"];
                detallecompra.unidad = (string)dr["unidad"];
                detallecompra.id_faena = (string)dr["id_faena"];
                detallecompra.id_partida = (string)dr["id_partida"];
                detallecompra.numero_item_partida = (int)dr["numero_item_partida"];
                detallecompra.nombre_item_partida = (string)dr["nombre_item_partida"];
                detallecompra.id_solicitud = (string)dr["id_solicitud"];
            //    detallecompra.monto_total = (int)dr["monto_total"];
                retorno.Add(detallecompra);
            }
            cnx.Close();

            return retorno;
        }
    }
}