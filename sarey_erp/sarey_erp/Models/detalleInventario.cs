using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace sarey_erp.Models
{
    public class detalleInventario
    {
        public string idFaena { get; set; }
        public float cantidad { get; set; }
        public string nombreItem { get; set; }

        public static void agregarDetalle(detalleInventario detalle) 
        {
            SqlConnection cnx = conexion.crearConexion();
            if (detalleInventario.verificarSiExiste(detalle.idFaena, detalle.nombreItem))
            {
                SqlCommand cmd_ = new SqlCommand();
                cmd_.Connection = cnx;
                cmd_.CommandText = "UPDATE detalle_inventario SET cantidad=@cantidad"
                  + " WHERE id_faena='" + detalle.idFaena + "' AND nombre_item='" + detalle.nombreItem + "'";

                double cantidad_anterior = detalleInventario.obtenerCantidadItem(detalle.idFaena, detalle.nombreItem);

                cmd_.Parameters.Add("@cantidad", SqlDbType.Float).Value = detalle.cantidad + cantidad_anterior;
                cmd_.ExecuteNonQuery();
            }
            else
            {
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = cnx;
                cmd.CommandText = "insert into detalle_inventario(id_faena,cantidad,nombre_item) values('"
                   + detalle.idFaena + "','"
                   + detalle.cantidad + "','"
                   + detalle.nombreItem + "')";
                cmd.CommandType = CommandType.Text;
                cmd.ExecuteNonQuery();

            }
        }

        public static detalleInventario obtenerItem(string nombreItem, string faena) 
        {
            detalleInventario retorno = new detalleInventario();

            SqlConnection cnx = conexion.crearConexion();

            SqlCommand cmd = new SqlCommand();
            cmd.Connection = cnx;
            cmd.CommandText = "SELECT * FROM detalle_inventario WHERE nombre_item=@nombreItem AND id_faena=@faena";
            cmd.CommandType = CommandType.Text;

            cmd.Parameters.Add("@nombreItem", SqlDbType.VarChar).Value=nombreItem;
            cmd.Parameters.Add("@faena", SqlDbType.VarChar).Value = faena;

            SqlDataReader dr = cmd.ExecuteReader();

            while (dr.Read())
            {
                detalleInventario nueva = new detalleInventario();

                retorno.idFaena = (string)dr["id_faena"];
                retorno.cantidad = float.Parse(dr["cantidad"].ToString());
                retorno.nombreItem = (string)dr["nombre_item"];
            }

            cnx.Close();

            return retorno;
        }

        public static List<detalleInventario> obtenerTodos()
        {
            List<detalleInventario> retorno = new List<detalleInventario>();

            SqlConnection cnx = conexion.crearConexion();

            SqlCommand cmd = new SqlCommand();
            cmd.Connection = cnx;
            cmd.CommandText = "SELECT * from detalle_inventario ORDER BY id_faena ASC";
            cmd.CommandType = CommandType.Text;
            SqlDataReader dr = cmd.ExecuteReader();

            while (dr.Read())
            {
                detalleInventario nueva = new detalleInventario();

                nueva.idFaena = (string)dr["id_faena"];
                nueva.cantidad = float.Parse(dr["cantidad"].ToString());
                nueva.nombreItem = (string)dr["nombre_item"];

                retorno.Add(nueva);
            }

            cnx.Close();

            return retorno;
        }

        public static List<detalleInventario> obtenerTodosFaena(string faena)
        {
            List<detalleInventario> retorno = new List<detalleInventario>();

            SqlConnection cnx = conexion.crearConexion();

            SqlCommand cmd = new SqlCommand();
            cmd.Connection = cnx;
            cmd.CommandText = "SELECT * from detalle_inventario WHERE id_faena=@idFaena ORDER BY id_faena ASC";
            cmd.CommandType = CommandType.Text;
            cmd.Parameters.Add("@idFaena", SqlDbType.VarChar).Value = faena;

            SqlDataReader dr = cmd.ExecuteReader();

            while (dr.Read())
            {
                detalleInventario nueva = new detalleInventario();

                nueva.idFaena = (string)dr["id_faena"];
                nueva.cantidad = float.Parse(dr["cantidad"].ToString());
                nueva.nombreItem = (string)dr["nombre_item"];

                retorno.Add(nueva);
            }

            cnx.Close();

            return retorno;
        }

        public static List<detalleInventario> obtenerTodosFaenaItem(string faena, string item)
        {
            List<detalleInventario> retorno = new List<detalleInventario>();

            SqlConnection cnx = conexion.crearConexion();

            SqlCommand cmd = new SqlCommand();
            cmd.Connection = cnx;
            cmd.CommandText = "SELECT * from detalle_inventario WHERE id_faena=@idFaena AND nombre_item=@item ORDER BY id_faena ASC";
            cmd.CommandType = CommandType.Text;
            cmd.Parameters.Add("@idFaena", SqlDbType.VarChar).Value = faena;
            cmd.Parameters.Add("@item", SqlDbType.VarChar).Value = item;

            SqlDataReader dr = cmd.ExecuteReader();

            while (dr.Read())
            {
                detalleInventario nueva = new detalleInventario();

                nueva.idFaena = (string)dr["id_faena"];
                nueva.cantidad = float.Parse(dr["cantidad"].ToString());
                nueva.nombreItem = (string)dr["nombre_item"];

                retorno.Add(nueva);
            }

            cnx.Close();

            return retorno;
        }

        public static List<detalleInventario> obtenerTodosItem(string item)
        {
            List<detalleInventario> retorno = new List<detalleInventario>();

            SqlConnection cnx = conexion.crearConexion();

            SqlCommand cmd = new SqlCommand();
            cmd.Connection = cnx;
            cmd.CommandText = "SELECT * from detalle_inventario WHERE nombre_item=@item ORDER BY id_faena ASC";
            cmd.CommandType = CommandType.Text;
            cmd.Parameters.Add("@item", SqlDbType.VarChar).Value = item;

            SqlDataReader dr = cmd.ExecuteReader();

            while (dr.Read())
            {
                detalleInventario nueva = new detalleInventario();

                nueva.idFaena = (string)dr["id_faena"];
                nueva.cantidad = float.Parse(dr["cantidad"].ToString());
                nueva.nombreItem = (string)dr["nombre_item"];

                retorno.Add(nueva);
            }

            cnx.Close();

            return retorno;
        }

        public static List<string> obtenerFaenas()
        {
            List<string> retorno = new List<string>();

            SqlConnection cnx = conexion.crearConexion();

            SqlCommand cmd = new SqlCommand();
            cmd.Connection = cnx;
            cmd.CommandText = "SELECT DISTINCT id_faena from detalle_inventario ORDER BY id_faena ASC";
            cmd.CommandType = CommandType.Text;

            SqlDataReader dr = cmd.ExecuteReader();

            while (dr.Read())
            {
                retorno.Add((string)dr["id_faena"]);
            }

            cnx.Close();

            return retorno;
        }

        public static List<string> obtenerFaenasItem(string item)
        {
            List<string> retorno = new List<string>();

            SqlConnection cnx = conexion.crearConexion();

            SqlCommand cmd = new SqlCommand();
            cmd.Connection = cnx;
            cmd.CommandText = "SELECT DISTINCT id_faena from detalle_inventario WHERE nombre_item=@item ORDER BY id_faena ASC";
            cmd.CommandType = CommandType.Text;

            cmd.Parameters.Add("@item", SqlDbType.VarChar).Value = item;

            SqlDataReader dr = cmd.ExecuteReader();

            while (dr.Read())
            {
                retorno.Add((string)dr["id_faena"]);
            }

            cnx.Close();

            return retorno;
        }

        public static List<string> obtenerItems()
        {
            List<string> retorno = new List<string>();

            SqlConnection cnx = conexion.crearConexion();

            SqlCommand cmd = new SqlCommand();
            cmd.Connection = cnx;
            cmd.CommandText = "SELECT DISTINCT nombre_item from detalle_inventario ORDER BY nombre_item ASC";
            cmd.CommandType = CommandType.Text;

            SqlDataReader dr = cmd.ExecuteReader();

            while (dr.Read())
            {
                retorno.Add((string)dr["nombre_item"]);
            }

            cnx.Close();

            return retorno;
        }

        public static List<string> obtenerItemsFaena(string faena)
        {
            List<string> retorno = new List<string>();

            SqlConnection cnx = conexion.crearConexion();

            SqlCommand cmd = new SqlCommand();
            cmd.Connection = cnx;
            cmd.CommandText = "SELECT DISTINCT nombre_item from detalle_inventario WHERE id_faena=@faena ORDER BY nombre_item ASC";
            cmd.CommandType = CommandType.Text;
            cmd.Parameters.Add("@faena", SqlDbType.VarChar).Value = faena;

            SqlDataReader dr = cmd.ExecuteReader();

            while (dr.Read())
            {
                retorno.Add((string)dr["nombre_item"]);
            }

            cnx.Close();

            return retorno;
        }

        public static double obtenerCantidadItem(string faena, string item) {

            SqlConnection cnx = conexion.crearConexion();

            SqlCommand cmd = new SqlCommand();
            cmd.Connection = cnx;
            cmd.CommandText = "SELECT cantidad from detalle_inventario WHERE id_faena='" + faena + "' AND nombre_item='" + item + "'";
            cmd.CommandType = CommandType.Text;           

            SqlDataReader dr = cmd.ExecuteReader();
            double cantidad_retorno=0;
            while (dr.Read())
            {
                cantidad_retorno=(double)dr["cantidad"];
            }

            cnx.Close();

            return cantidad_retorno;

        
        }

        public static bool verificarSiExiste(string faena,string item)
        {
            bool retorno = false;

            SqlConnection cnx = conexion.crearConexion();
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = cnx;
            cmd.CommandText = "SELECT * FROM detalle_inventario WHERE id_faena='" + faena + "' AND nombre_item='" + item + "'";
            cmd.CommandType = CommandType.Text;

            SqlDataReader dr = cmd.ExecuteReader();

            retorno = dr.HasRows;

            cnx.Close();

            return retorno;
        }
    }
}