using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Data;

namespace sarey_erp.Models
{
    public class orden_compra
    {
        public string numero_orden { get; set; }
        public DateTime fecha { get; set; }
        public string observacion { get; set; }
        public string sub_total { get; set; }
        public string iva { get; set; }
        public string total { get; set; }
        public string id_proveedor { get; set; }
        public string id_remplazada { get; set; }
        
        public static void agregarOrden(orden_compra nuevo)
        {
            SqlConnection cnx = conexion.crearConexion();

            SqlCommand cmd = new SqlCommand();
            cmd.Connection = cnx;
            cmd.CommandText = "INSERT INTO orden_de_compra VALUES(@numero_orden_compra,@fecha,"
                + "@observaciones,@subtotal,@iva,@total_pagar,@id_proveedor)";

            cmd.Parameters.Add("@numero_orden_compra", SqlDbType.VarChar).Value = nuevo.numero_orden;
            cmd.Parameters.Add("@fecha", SqlDbType.DateTime).Value = nuevo.fecha;
            cmd.Parameters.Add("@observaciones", SqlDbType.VarChar).Value = nuevo.observacion;
            cmd.Parameters.Add("@subtotal", SqlDbType.VarChar).Value = nuevo.sub_total;
            cmd.Parameters.Add("@iva", SqlDbType.VarChar).Value = nuevo.iva;
            cmd.Parameters.Add("@total_pagar", SqlDbType.VarChar).Value = nuevo.total;
            cmd.Parameters.Add("@id_proveedor", SqlDbType.VarChar).Value = nuevo.id_proveedor;

            cmd.CommandType = CommandType.Text;
            cmd.ExecuteNonQuery();
            cnx.Close();
        }
        public static void agregardetalle(string id, string[] item, string[] cantidad, string[] precio, string[] unidad, string[] id_faena, string[] id_partida, string[] numero_item_partida, string[] nombre_item_partida, string[] id_solicitud)
        {

            SqlConnection cnx = conexion.crearConexion();
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = cnx;


            for (int i = 0; i < cantidad.Length; i++)
            {
                cmd.CommandText = "insert into detalle_orden_compra values('"
                   + id + "','"
                   + item[i] + "','"
                   + cantidad[i] + "','"
                   + precio[i] + "','"
                   + unidad[i] + "','"
                   + id_faena[i] + "','"
                   + id_partida[i] + "','"
                   + numero_item_partida[i] + "','"
                   + nombre_item_partida[i] + "','"
                   + id_solicitud[i] + "')";

                cmd.CommandType = CommandType.Text;
                cmd.ExecuteNonQuery();
            }
            cnx.Close();
        }
        public static List<orden_compra> obtenerTodas()
        {
            List<orden_compra> retorno = new List<orden_compra>();

            SqlConnection cnx = conexion.crearConexion();

            SqlCommand cmd = new SqlCommand();
            cmd.Connection = cnx;
            cmd.CommandText = "SELECT * from orden_de_compra ORDER BY fecha DESC";
            cmd.CommandType = CommandType.Text;
            SqlDataReader dr = cmd.ExecuteReader();

            while (dr.Read())
            {
                orden_compra O_compra  = new orden_compra();
                O_compra.numero_orden = (string)dr["numero_orden_compra"];
                O_compra.fecha = (DateTime)dr["fecha"];
                O_compra.observacion = (string)dr["observaciones"];
                O_compra.sub_total = (string)dr["subtotal"];
                O_compra.iva = (string)dr["iva"];
                O_compra.total = (string)dr["total_pagar"];
                O_compra.id_proveedor = (string)dr["id_proveedor"];

                retorno.Add(O_compra);
            }
            cnx.Close();

            return retorno;
        }

        public static orden_compra obtenerOrdenCompra( string numeroOrdenCompra)
        {
            orden_compra retorno = new orden_compra();

            SqlConnection cnx = conexion.crearConexion();

            SqlCommand cmd = new SqlCommand();
            cmd.Connection = cnx;
            cmd.CommandText = "SELECT * from orden_de_compra WHERE numero_orden_compra='" + numeroOrdenCompra +"'";
            cmd.CommandType = CommandType.Text;
            SqlDataReader dr = cmd.ExecuteReader();

            while (dr.Read())
            {
                retorno.numero_orden = (string)dr["numero_orden_compra"];
                retorno.fecha = (DateTime)dr["fecha"];
                retorno.observacion = (string)dr["observaciones"];
                retorno.sub_total = (string)dr["subtotal"];
                retorno.iva = (string)dr["iva"];
                retorno.total = (string)dr["total_pagar"];
                retorno.id_proveedor = (string)dr["id_proveedor"];
            }
            cnx.Close();

            return retorno;
        }
        public static string numeroNuevaOrden()
        {
            SqlConnection cnx = conexion.crearConexion();

            SqlCommand cmd = new SqlCommand();
            cmd.Connection = cnx;
            cmd.CommandText = "SELECT numero_orden_compra from orden_de_compra ";
            cmd.CommandType = CommandType.Text;
            SqlDataReader dr = cmd.ExecuteReader();

            int retorno = 1999;

            while (dr.Read())
            {
                if (int.Parse(dr["numero_orden_compra"].ToString()) > retorno)
                    retorno = int.Parse(dr["numero_orden_compra"].ToString());
            }

            cnx.Close();

            return (retorno+1).ToString();

        }
        public static void agregarOrdenNueva(orden_compra nuevo)
        {
            SqlConnection cnx = conexion.crearConexion();

            SqlCommand cmd = new SqlCommand();
            cmd.Connection = cnx;
            cmd.CommandText = "INSERT INTO orden_de_compra VALUES(@numero_orden_compra,@fecha,"
                + "@observaciones,@subtotal,@iva,@total_pagar,@id_proveedor)";

            cmd.Parameters.Add("@numero_orden_compra", SqlDbType.VarChar).Value = nuevo.numero_orden;
            cmd.Parameters.Add("@fecha", SqlDbType.DateTime).Value = nuevo.fecha;
            cmd.Parameters.Add("@observaciones", SqlDbType.VarChar).Value = nuevo.observacion;
            cmd.Parameters.Add("@subtotal", SqlDbType.VarChar).Value = nuevo.sub_total;
            cmd.Parameters.Add("@iva", SqlDbType.VarChar).Value = nuevo.iva;
            cmd.Parameters.Add("@total_pagar", SqlDbType.VarChar).Value = nuevo.total;
            cmd.Parameters.Add("@id_proveedor", SqlDbType.VarChar).Value = nuevo.id_proveedor;

            cmd.CommandType = CommandType.Text;
            cmd.ExecuteNonQuery();
            cnx.Close();

            SqlConnection cnx2 = conexion.crearConexion();

            SqlCommand cmd2 = new SqlCommand();
            cmd2.Connection = cnx2;
            cmd2.CommandText = "INSERT INTO orden_de_compra_nueva VALUES(@id_oc_vieja,@id_oc_nueva)";

            cmd2.Parameters.Add("@id_oc_vieja", SqlDbType.VarChar).Value = nuevo.id_remplazada;
            cmd2.Parameters.Add("@id_oc_nueva", SqlDbType.VarChar).Value = nuevo.numero_orden;
           
            cmd2.CommandType = CommandType.Text;
            cmd2.ExecuteNonQuery();
            cnx2.Close();
        }

        public static List<orden_compra> obtenerTodasReemplazadas()
        {
            List<orden_compra> retorno = new List<orden_compra>();

            SqlConnection cnx = conexion.crearConexion();

            SqlCommand cmd = new SqlCommand();
            cmd.Connection = cnx;
            cmd.CommandText = "SELECT * from orden_de_compra,orden_de_compra_nueva WHERE numero_orden_compra = id_oc_vieja ORDER BY fecha DESC";
            cmd.CommandType = CommandType.Text;
            SqlDataReader dr = cmd.ExecuteReader();

            while (dr.Read())
            {
                orden_compra O_compra = new orden_compra();
                O_compra.numero_orden = (string)dr["numero_orden_compra"];
                O_compra.fecha = (DateTime)dr["fecha"];
                O_compra.observacion = (string)dr["observaciones"];
                O_compra.sub_total = (string)dr["subtotal"];
                O_compra.iva = (string)dr["iva"];
                O_compra.total = (string)dr["total_pagar"];
                O_compra.id_proveedor = (string)dr["id_proveedor"];

                retorno.Add(O_compra);
            }
            cnx.Close();

            return retorno;
        }

        public bool esReeemplazada()
        {
            bool retorno = false;

            SqlConnection cnx = conexion.crearConexion();

            SqlCommand cmd = new SqlCommand();
            cmd.Connection = cnx;
            cmd.CommandText = "SELECT * from orden_de_compra_nueva WHERE '" + numero_orden + "' = id_oc_vieja";
            cmd.CommandType = CommandType.Text;
            SqlDataReader dr = cmd.ExecuteReader();

            retorno = dr.HasRows;
            cnx.Close();

            return retorno;
        }

        public string obtenerNumeroOrdenQueReemplaza()
        {
            string numeroOrden = "";

            SqlConnection cnx = conexion.crearConexion();

            SqlCommand cmd = new SqlCommand();
            cmd.Connection = cnx;
            cmd.CommandText = "SELECT * from orden_de_compra_nueva WHERE '" + numero_orden + "' = id_oc_vieja";
            cmd.CommandType = CommandType.Text;
            SqlDataReader dr = cmd.ExecuteReader();

            while (dr.Read())
            {
                numeroOrden = (string)dr["id_oc_nueva"];
            }
            cnx.Close();

            return numeroOrden;
        }

        public bool reemplaza()
        {
            bool retorno = false;

            SqlConnection cnx = conexion.crearConexion();

            SqlCommand cmd = new SqlCommand();
            cmd.Connection = cnx;
            cmd.CommandText = "SELECT * from orden_de_compra_nueva WHERE '" + numero_orden + "' = id_oc_nueva";
            cmd.CommandType = CommandType.Text;
            SqlDataReader dr = cmd.ExecuteReader();

            retorno = dr.HasRows;
            cnx.Close();

            return retorno;
        }

        public string obtenerNumeroOrdenReemplazada()
        {
            string numeroOrden = "";

            SqlConnection cnx = conexion.crearConexion();

            SqlCommand cmd = new SqlCommand();
            cmd.Connection = cnx;
            cmd.CommandText = "SELECT * from orden_de_compra_nueva WHERE '" + numero_orden + "' = id_oc_nueva";
            cmd.CommandType = CommandType.Text;
            SqlDataReader dr = cmd.ExecuteReader();

            while (dr.Read())
            {
                numeroOrden = (string)dr["id_oc_vieja"];
            }
            cnx.Close();

            return numeroOrden;
        }
    }    
}