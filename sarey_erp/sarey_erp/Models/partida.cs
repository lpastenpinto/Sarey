using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;


namespace sarey_erp.Models
{
    public class partida
    {
        public string id_faena { get; set; }
        public string descripcion { get; set; }
        public string id_partida { get; set; }
        public string unidad { get; set; }
        public double cantidad { get; set; }
        public double precio_unitario { get; set; }
        public double total { get; set; }
        public string id_partida_global { set; get; }

        public static string obtenerPresupuesto(string idfaena,string idpartida) {
            string presupuesto = "";

            SqlConnection cnx = conexion.crearConexion();          
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = cnx;
            cmd.CommandText = "SELECT * from partidas WHERE  ( id_faena = '" + idfaena+"' AND id_partida = '" + idpartida+"')";
            cmd.CommandType = CommandType.Text;
            SqlDataReader dr = cmd.ExecuteReader();

            while (dr.Read())
            {                
                presupuesto = Convert.ToString((double)dr["total"]);
            }
            cnx.Close();

            return presupuesto;
           
        }
        public void guardarPartida(SqlConnection cnx){
            //SqlConnection cnx = conexion.crearConexion();
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = cnx;
            try
            {

                cmd.CommandText = "insert into partidas(id_faena,descripcion,id_partida,unidad,precio_unitario,cantidad,total,id_partida_global) values(@id_faena,@descripcion,@id_partida,@unidad,@precio_unitario,@cantidad,@total,@id_partida_global)";

                /*cmd.CommandText = "insert into partidas(id_faena,descripcion,id_partida,unidad,precio_unitario,cantidad,total) values('"
                   + id_faena + "','"
                   + descripcion + "','"
                   + id_partida + "','"
                   + unidad + "','"
                   + precio_unitario + "','"
                   + cantidad + "','"
                   + total + "')";

                cmd_.CommandText = "UPDATE solicitudes_materiales SET nombre_recepcion=@nombre_recepcion, "
            + "fecha_recepcion=@fecha_recepcion WHERE id_solicitud='" + id_solicitud + "'";

                cmd_.Parameters.Add("@nombre_recepcion", SqlDbType.VarChar).Value = nombre_revisa;
                cmd_.Parameters.Add("@fecha_recepcion", SqlDbType.DateTime).Value = fecha_actual; */
                cmd.Parameters.Add("@id_faena", SqlDbType.VarChar).Value = id_faena;
                cmd.Parameters.Add("@descripcion", SqlDbType.VarChar).Value = descripcion;
                cmd.Parameters.Add("@id_partida", SqlDbType.VarChar).Value = id_partida;
                cmd.Parameters.Add("@unidad", SqlDbType.VarChar).Value = unidad;
                cmd.Parameters.Add("@precio_unitario", SqlDbType.Float).Value = precio_unitario;
                cmd.Parameters.Add("@cantidad", SqlDbType.Float).Value = cantidad;
                cmd.Parameters.Add("@total", SqlDbType.Float).Value =  Math.Round(total, MidpointRounding.ToEven);
                cmd.Parameters.Add("@id_partida_global", SqlDbType.VarChar).Value = id_partida_global;
                cmd.CommandType = CommandType.Text;
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Console.Write(ex);
              //  return false;
            }
            //cnx.Close();
            //return true;
        }

        public static List<partida> obtenerPartidas(string faena, string partidaGlobal)
        {
            List<partida> retorno = new List<partida>();

            SqlConnection cnx = conexion.crearConexion();
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = cnx;
            cmd.CommandText = "SELECT * from partidas WHERE  ( id_faena = '" + faena + "' AND id_partida_global = '" + partidaGlobal + "')";
            cmd.CommandType = CommandType.Text;
            SqlDataReader dr = cmd.ExecuteReader();

            while (dr.Read())
            {
                partida temp = new partida();

                temp.id_faena = (string)dr["id_faena"];
                temp.descripcion = (string)dr["descripcion"];
                temp.id_partida = (string)dr["id_partida"];
                temp.unidad = (string)dr["unidad"];
                temp.cantidad = double.Parse(dr["cantidad"].ToString());
                temp.precio_unitario = double.Parse(dr["precio_unitario"].ToString());
                temp.total = double.Parse(dr["total"].ToString());
                temp.id_partida_global = (string)dr["id_partida_global"];

                retorno.Add(temp);
            }
            cnx.Close();

            return retorno;
        }

        public static partida obtenerPartida(string faena, string idPartida)
        {
            partida temp = new partida();

            SqlConnection cnx = conexion.crearConexion();
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = cnx;
            cmd.CommandText = "SELECT * from partidas WHERE  ( id_faena = '" + faena  + "' AND id_partida='"+ idPartida +"')";
            cmd.CommandType = CommandType.Text;
            SqlDataReader dr = cmd.ExecuteReader();

            while (dr.Read())
            {
                temp.id_faena = (string)dr["id_faena"];
                temp.descripcion = (string)dr["descripcion"];
                temp.id_partida = (string)dr["id_partida"];
                temp.unidad = (string)dr["unidad"];
                temp.cantidad = double.Parse(dr["cantidad"].ToString());
                temp.precio_unitario = double.Parse(dr["precio_unitario"].ToString());
                temp.total = double.Parse(dr["total"].ToString());
                temp.id_partida_global = (string)dr["id_partida_global"];
            }
            cnx.Close();

            return temp;
        }

        public void guardarPartidaGlobal(string id,string nombre,string faena)
        {
            SqlConnection cnx = conexion.crearConexion();
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = cnx;
            try
            {

                cmd.CommandText = "insert into partidas_global(id,nombre,faena) values(@id,@nombre,@faena)";

                cmd.Parameters.Add("@id", SqlDbType.VarChar).Value = id;
                cmd.Parameters.Add("@nombre", SqlDbType.VarChar).Value = nombre;
                cmd.Parameters.Add("@faena", SqlDbType.VarChar).Value = faena;
                //cmd.Parameters.Add("@descripcion", SqlDbType.VarChar).Value = descripcion;                               
                cmd.CommandType = CommandType.Text;
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Console.Write(ex);
                
            }
            cnx.Close();
           
        }
    
        public static void eliminarPartidasFaena(string idFaena){

            SqlConnection cnx = conexion.crearConexion();
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = cnx;
            cmd.CommandText = "DELETE FROM partidas  WHERE  ( id_faena = '" + idFaena + "')";               
            cmd.CommandType = CommandType.Text;
            cmd.ExecuteNonQuery();
            cnx.Close();
        }

        public static void eliminarPartidasGlobal(string faena)
        {

            SqlConnection cnx = conexion.crearConexion();
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = cnx;
            cmd.CommandText = "DELETE FROM partidas_global  WHERE  ( faena = '" + faena + "')";
            cmd.CommandType = CommandType.Text;
            cmd.ExecuteNonQuery();
            cnx.Close();
        }

        public List<detalle_ordencompra> obtenerDetalleGastos()
        {
            List<detalle_ordencompra> retorno = new List<detalle_ordencompra>();

            SqlConnection cnx = conexion.crearConexion();
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = cnx;
            cmd.CommandText = "SELECT * from detalle_orden_compra WHERE  ( id_faena = '" + this.id_faena + "' AND id_partida = '" + this.id_partida + "')";
            cmd.CommandType = CommandType.Text;
            SqlDataReader dr = cmd.ExecuteReader();

            while (dr.Read())
            {
                detalle_ordencompra temp = new detalle_ordencompra();

                temp.id_numero_orden = (string)dr["id_orden_compra"];
                temp.id_item = (string)dr["id_item"];
                temp.precio_unitario = int.Parse(dr["precio_unitario"].ToString());
                temp.cantidad_item = int.Parse(dr["cantidad"].ToString());

                retorno.Add(temp);
            }
            cnx.Close();
            return retorno;
        }

        public double obtenerGastosTotales()
        {
            double retorno = 0;

            SqlConnection cnx = conexion.crearConexion();
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = cnx;
            cmd.CommandText = "SELECT * from detalle_orden_compra WHERE  ( id_faena = '" + this.id_faena + "' AND id_partida = '" + this.id_partida + "')";
            cmd.CommandType = CommandType.Text;
            SqlDataReader dr = cmd.ExecuteReader();

            while (dr.Read())
            {
                retorno += (double.Parse(dr["cantidad"].ToString()) * double.Parse(dr["precio_unitario"].ToString()));
            }
            cnx.Close();
            return retorno;
        }

        public double obtenerSaldo()
        {
            double retorno = 0;

            retorno = this.total - this.obtenerGastosTotales();

            return retorno;
        }
    }    
}
