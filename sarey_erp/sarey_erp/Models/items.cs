using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;

namespace sarey_erp.Models
{
    public class items
    {
        public string nombre { get; set; }
       public string descripcion{get;set;}
        public string unidad{get;set;}
        public string dimensiones{get;set;}
        public string cantidad_inventario { get; set; }
        public double cantidad { get; set; }
        public double precio_unitario { get; set; }
        public double costo_unitario { get; set; }
        public double cantidad_comprar { get; set; }
        public double presupuesto_compra { get; set; }
        public string tipo { get; set; }
        public int numero { get; set; }
        public string id_faena { get; set; }
        public string id_partida { get; set; }

        public static string obtenerPresupuesto(string iditem, string idfaena, string idpartida) {

            string presupuesto = "";

            SqlConnection cnx = conexion.crearConexion();
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = cnx;
            cmd.CommandText = "SELECT * from items WHERE  (  nombre = '" + iditem + "' AND id_faena = '" + idfaena + "' AND id_partida = '" + idpartida + "')";
            cmd.CommandType = CommandType.Text;
            SqlDataReader dr = cmd.ExecuteReader();

            while (dr.Read())
            {
                presupuesto = Convert.ToString((int)dr["presupuesto_compra"]);
            }
            cnx.Close();

            return presupuesto;
        
        }

        public void guardarNuevoItem(string id_partida, string id_faena, SqlConnection cnx)
        {   
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = cnx;
            try
            {
                cmd.CommandText = "insert into items(nombre,unidad,cantidad,precio_unitario,costo_unitario,cantidad_comprar,presupuesto_compra,id_partida,id_faena,tipo,numero) values(@nombre,@unidad,@cantidad,@precio_unitario,@costo_unitario,@cantidad_comprar,@presupuesto_compra,@id_partida,@id_faena,@tipo,@numero)";               
                cmd.Parameters.Add("@nombre", SqlDbType.VarChar).Value = nombre;
                cmd.Parameters.Add("@unidad", SqlDbType.VarChar).Value = unidad;                               
                cmd.Parameters.Add("@cantidad", SqlDbType.Float).Value = cantidad;
                cmd.Parameters.Add("@precio_unitario", SqlDbType.Float).Value = precio_unitario;
                cmd.Parameters.Add("@costo_unitario", SqlDbType.Float).Value = costo_unitario;
                cmd.Parameters.Add("@cantidad_comprar", SqlDbType.Float).Value = cantidad_comprar;
                cmd.Parameters.Add("@presupuesto_compra", SqlDbType.Float).Value = presupuesto_compra;
                cmd.Parameters.Add("@id_partida", SqlDbType.VarChar).Value = id_partida;
                cmd.Parameters.Add("@id_faena", SqlDbType.VarChar).Value = id_faena;
                cmd.Parameters.Add("@tipo", SqlDbType.VarChar).Value = tipo;
                cmd.Parameters.Add("@numero", SqlDbType.Float).Value = numero;
                cmd.CommandType = CommandType.Text;
                cmd.ExecuteNonQuery();
                
            }
           catch (Exception ex) {
               throw new System.InvalidOperationException("Elemento Duplicado:"+nombre+"    partida:"+id_partida+"  numero:"+numero);
            }
            
            
        }

        public static List<items> obtenerItemsdePartida(string id_faena, string id_partida) {

            List<items> retorno = new List<items>();

            SqlConnection cnx = conexion.crearConexion();

            SqlCommand cmd = new SqlCommand();
            cmd.Connection = cnx;
            cmd.CommandText = "SELECT * from items WHERE ( id_partida = '" + id_partida + "' AND id_faena = '" + id_faena + "') ORDER BY nombre ASC";
            cmd.CommandType = CommandType.Text;
            SqlDataReader dr = cmd.ExecuteReader();

            while (dr.Read())
            {
                items Item = new items();

                Item.nombre = (string)dr["nombre"];
                Item.numero = (int)dr["numero"];
                //  Item.descripcion = (string)dr["descripcion"];
                /*try
                {
                    Item.dimensiones = (string)dr["dimensiones"];
                }
                catch (Exception)
                {
                    Item.dimensiones = "";
                }

                Item.unidad = (string)dr["unidad"];
                Item.cantidad_inventario = Convert.ToString((double)dr["cantidad_inventario"]);
                */
                retorno.Add(Item);
            }
            cnx.Close();

            return retorno;
        }

        public static items obtenerItem(string id_faena, string id_partida, string nombreMaterial)
        {
            items retorno = new items();

            SqlConnection cnx = conexion.crearConexion();

            SqlCommand cmd = new SqlCommand();
            cmd.Connection = cnx;
            cmd.CommandText = "SELECT * from items WHERE ( id_partida = '" + id_partida + "' AND id_faena = '" + id_faena
                + "' AND nombre='"+nombreMaterial+"') ORDER BY nombre ASC";
            cmd.CommandType = CommandType.Text;
            SqlDataReader dr = cmd.ExecuteReader();

            while (dr.Read())
            {
                retorno.nombre = (string)dr["nombre"];
                retorno.numero = (int)dr["numero"];
                retorno.id_faena = id_faena;
                retorno.id_partida = id_partida;
                //  Item.descripcion = (string)dr["descripcion"];
                /*try
                {
                    Item.dimensiones = (string)dr["dimensiones"];
                }
                catch (Exception)
                {
                    Item.dimensiones = "";
                }

                Item.unidad = (string)dr["unidad"];
                Item.cantidad_inventario = Convert.ToString((double)dr["cantidad_inventario"]);
                */
            }
            cnx.Close();

            return retorno;
        }    

        public items obtenerItem(string id_partida,string id_faena) {

            items Item = new items();                      

            SqlConnection cnx = conexion.crearConexion();

            SqlCommand cmd = new SqlCommand();
            cmd.Connection = cnx;
            cmd.CommandText = "SELECT * from items WHERE ( nombre = '" + nombre + "' AND id_partida = '" + id_partida + "' AND id_faena = '" + id_faena + "')";
            cmd.CommandType = CommandType.Text;
            SqlDataReader dr = cmd.ExecuteReader();

            while (dr.Read())
            {               

                /*Item.nombre = (string)dr["nombre"];
                //Item.descripcion = (string)dr["descripcion"];
                try
                {
                    Item.dimensiones = (string)dr["dimensiones"];
                }
                catch (Exception)
                {
                    Item.dimensiones = "";
                }

                Item.unidad = (string)dr["unidad"];
                //Item.cantidad_inventario = Convert.ToString((double)dr["cantidad_inventario"]);//*/

                Item.nombre = (string)dr["nombre"];

                if (typeof(DBNull).Equals(dr["descripcion"].GetType()))
                {
                    Item.descripcion = "";
                }
                else
                {
                    Item.descripcion = (string)dr["descripcion"];
                }

                Item.unidad = (string)dr["unidad"];

                if (typeof(DBNull).Equals(dr["dimensiones"].GetType()))
                {
                    Item.dimensiones = "";
                }
                else
                {
                    Item.dimensiones = (string)dr["dimensiones"];
                }

                Item.cantidad = double.Parse(dr["cantidad"].ToString());
                Item.precio_unitario = int.Parse(dr["precio_unitario"].ToString());
                Item.costo_unitario = int.Parse(dr["costo_unitario"].ToString());
                Item.cantidad_comprar = double.Parse(dr["cantidad_comprar"].ToString());
                Item.presupuesto_compra = int.Parse(dr["presupuesto_compra"].ToString());
                Item.id_partida = (string)dr["id_partida"];
                Item.id_faena = (string)dr["id_faena"];
                Item.tipo = (string)dr["tipo"];
                Item.numero = int.Parse(dr["numero"].ToString());

                
            }
            cnx.Close();

            return Item;

        }

        public static List<items> obtenerTodos()
        {

            List<items> retorno = new List<items>();

            SqlConnection cnx = conexion.crearConexion();

            SqlCommand cmd = new SqlCommand();
            cmd.Connection = cnx;
            cmd.CommandText = "SELECT * from items ORDER BY nombre ASC";
            cmd.CommandType = CommandType.Text;
            SqlDataReader dr = cmd.ExecuteReader();

            while (dr.Read())
            {
                items Item = new items();

                Item.nombre = (string)dr["nombre"];
              //  Item.descripcion = (string)dr["descripcion"];
                try
                {
                    Item.dimensiones = (string)dr["dimensiones"];
                }
                catch (Exception) {
                    Item.dimensiones = "";
                }
              
                Item.unidad= (string)dr["unidad"];
                Item.cantidad_inventario = Convert.ToString((double)dr["cantidad_inventario"]);

                retorno.Add(Item);
            }
            cnx.Close();

            return retorno;
        
        }

        public static List<items> obtenerTodosFaenaPartida(string faena, string partida)
        {

            List<items> retorno = new List<items>();

            SqlConnection cnx = conexion.crearConexion();

            SqlCommand cmd = new SqlCommand();
            cmd.Connection = cnx;
            cmd.CommandText = "SELECT * from items WHERE id_partida='" + partida + "' AND id_faena='" + faena + "'";
            cmd.CommandType = CommandType.Text;
            SqlDataReader dr = cmd.ExecuteReader();

            while (dr.Read())
            {
                items Item = new items();

                Item.nombre = (string)dr["nombre"];

                if (typeof(DBNull).Equals(dr["descripcion"].GetType()))
                {
                    Item.descripcion = "";
                }
                else
                {
                    Item.descripcion = (string)dr["descripcion"];
                }

                Item.unidad = (string)dr["unidad"];

                if (typeof(DBNull).Equals(dr["dimensiones"].GetType()))
                {
                    Item.dimensiones = "";
                }
                else
                {
                    Item.dimensiones = (string)dr["dimensiones"];
                }

                Item.cantidad = double.Parse(dr["cantidad"].ToString());
                Item.precio_unitario = int.Parse(dr["precio_unitario"].ToString());
                Item.costo_unitario = int.Parse(dr["costo_unitario"].ToString());
                Item.cantidad_comprar = double.Parse(dr["cantidad_comprar"].ToString());
                Item.presupuesto_compra = int.Parse(dr["presupuesto_compra"].ToString());
                Item.id_partida = (string)dr["id_partida"];
                Item.id_faena = (string)dr["id_faena"];
                Item.tipo = (string)dr["tipo"];
                Item.numero = int.Parse(dr["numero"].ToString());

                retorno.Add(Item);
            }
            cnx.Close();

            return retorno;

        }

        public static void eliminarItemsdeFaena(string id_faena) {
            try
            {
                SqlConnection cnx = conexion.crearConexion();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = cnx;
                cmd.CommandText = "DELETE FROM items  WHERE  ( id_faena = '" + id_faena + "')";
                cmd.CommandType = CommandType.Text;
                cmd.ExecuteNonQuery();
                cnx.Close();
            }
            catch (Exception ex) {
                Console.Write(ex);
            }
        
        
        }

        public static List<string> obtenerTodasUnidades() {
            List<string> retorno = new List<string>();

            SqlConnection cnx = conexion.crearConexion();

            SqlCommand cmd = new SqlCommand();
            cmd.Connection = cnx;
            cmd.CommandText = "SELECT DISTINCT unidad from items";
            cmd.CommandType = CommandType.Text;
            SqlDataReader dr = cmd.ExecuteReader();

            while (dr.Read())
            {                                
                retorno.Add((string)dr["unidad"]);
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
            cmd.CommandText = "SELECT * from detalle_orden_compra WHERE  ( id_faena = '" + this.id_faena + "' AND id_partida = '" + this.id_partida + "' AND numero_item_partida = '" + this.numero + "')";
            cmd.CommandType = CommandType.Text;
            SqlDataReader dr = cmd.ExecuteReader();

            while (dr.Read())
            {
                retorno += (double.Parse(dr["cantidad"].ToString()) * double.Parse(dr["precio_unitario"].ToString()));
            }
            cnx.Close();
            return retorno;
        }

        public List<detalle_ordencompra> obtenerDetalleGastos()
        {
            List<detalle_ordencompra> retorno = new List<detalle_ordencompra>();

            SqlConnection cnx = conexion.crearConexion();
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = cnx;
            cmd.CommandText = "SELECT * from detalle_orden_compra WHERE  ( id_faena = '" + this.id_faena + "' AND id_partida = '" + this.id_partida + "' AND numero_item_partida = '" + this.numero + "')";
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

        public double obtenerSaldo()
        {
            double retorno = 0;

            retorno = this.presupuesto_compra - this.obtenerGastosTotales();

            return retorno;
        }
    }

    
}