using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;

namespace sarey_erp.Models
{
    public class faena
    {
        public string nombre { set; get; }
        public double partida_presupuesto { get; set; }
        public string centro_costo { get; set; }
        
        public double valor_neto{set;get;}
        public double gastos_generales { set; get; }
        public double utilidades { set; get; }
        public double subtotal { set; get; }
        public double iva { set; get; }


        public bool verificarCentroCosto (){          

            bool retorno = false;

            SqlConnection cnx = conexion.crearConexion();
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = cnx;
            cmd.CommandText = "SELECT * FROM faenas WHERE centro_costo='" + centro_costo + "'";
            cmd.CommandType = CommandType.Text;

            SqlDataReader dr = cmd.ExecuteReader();

            retorno = dr.HasRows;

            cnx.Close();

            return retorno;

        }
        public bool verificarFaena()
        {

            bool retorno = false;

            SqlConnection cnx = conexion.crearConexion();
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = cnx;
            cmd.CommandText = "SELECT * FROM faenas WHERE nombre='" + nombre + "'";
            cmd.CommandType = CommandType.Text;

            SqlDataReader dr = cmd.ExecuteReader();

            retorno = dr.HasRows;

            cnx.Close();

            return retorno;

        }

        public static faena obtenerFaena(string nombreFaena) {

            SqlConnection cnx = conexion.crearConexion();
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = cnx;
            cmd.CommandText = "SELECT * from faenas WHERE nombre='"+nombreFaena+"'";
            cmd.CommandType = CommandType.Text;
            SqlDataReader dr = cmd.ExecuteReader();

            faena Faena = new faena();
            while (dr.Read())
            {                
                Faena.nombre = (string)dr["nombre"];
                Faena.partida_presupuesto = (double)dr["partida_presupuesto"];
                Faena.centro_costo = (string)dr["centro_costo"];
                Faena.valor_neto=(double)dr["valor_neto"];
                Faena.gastos_generales=(double)dr["gastos_generales"]; 
                Faena.utilidades=(double)dr["utilidades"];
                Faena.subtotal=(double)dr["subtotal"];
                Faena.iva=(double)dr["iva"];
                             
            }
            cnx.Close();
            return Faena;

        }
        public static List<faena> obtenerTodas()
        {

            List<faena> retorno = new List<faena>();

            SqlConnection cnx = conexion.crearConexion();

            SqlCommand cmd = new SqlCommand();
            cmd.Connection = cnx;
            cmd.CommandText = "SELECT * from faenas ORDER BY nombre ASC";
            cmd.CommandType = CommandType.Text;
            SqlDataReader dr = cmd.ExecuteReader();

            while (dr.Read())
            {
                faena Faenas = new faena();

                Faenas.nombre = (string)dr["nombre"];
                Faenas.partida_presupuesto = (double)dr["partida_presupuesto"];
                Faenas.centro_costo = (string)dr["centro_costo"];

                retorno.Add(Faenas);
            }
            cnx.Close();

            return retorno;

        }

        public static string obtenerNombreFaena(string cc)
        {
            string retorno = "";

            SqlConnection cnx = conexion.crearConexion();

            SqlCommand cmd = new SqlCommand();
            cmd.Connection = cnx;
            cmd.CommandText = "SELECT * from faenas WHERE centro_costo='" + cc + "'";
            cmd.CommandType = CommandType.Text;
            SqlDataReader dr = cmd.ExecuteReader();

            while (dr.Read())
            {
                retorno = (string)dr["nombre"];
            }
            cnx.Close();

            return retorno;
        }

        public string obtenerPresupuestoPartida() {
            
            SqlConnection cnx = conexion.crearConexion();
            string presupuesto="";
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = cnx;
            cmd.CommandText = "SELECT * from faenas WHERE  ( nombre = '" + nombre + "')";
            cmd.CommandType = CommandType.Text;
            SqlDataReader dr = cmd.ExecuteReader();

            while (dr.Read())
            {                
                //presupuesto = (string)dr["partida_presupuesto"];
                presupuesto = Convert.ToString((double)dr["partida_presupuesto"]);
            }
            cnx.Close();

            return presupuesto;
        }
        
        public List<partida> obtenerPartidas(string nombre)
        {

            List<partida> retorno = new List<partida>();
            SqlConnection cnx = conexion.crearConexion();

            SqlCommand cmd = new SqlCommand();
            cmd.Connection = cnx;
            cmd.CommandText = "SELECT * from partidas WHERE( id_faena = '" + nombre + "') ORDER BY id_tabla_partidas";
            cmd.CommandType = CommandType.Text;
            SqlDataReader dr = cmd.ExecuteReader();

            while (dr.Read())
            {
                partida Partida = new partida();
                Partida.descripcion = (string)dr["descripcion"];
                Partida.id_partida = (string)dr["id_partida"];
               

                retorno.Add(Partida);
            }
            cnx.Close();

            return retorno;

        }

        public List<partida> obtenerPartidas()
        {

            List<partida> retorno = new List<partida>();
            SqlConnection cnx = conexion.crearConexion();

            SqlCommand cmd = new SqlCommand();
            cmd.Connection = cnx;
            cmd.CommandText = "SELECT * from partidas ORDER BY id_partida ASC";
            cmd.CommandType = CommandType.Text;
            SqlDataReader dr = cmd.ExecuteReader();

            while (dr.Read())
            {
                partida Partida = new partida();
                Partida.descripcion = (string)dr["descripcion"];
                Partida.id_partida = (string)dr["id_partida"];


                retorno.Add(Partida);
            }
            cnx.Close();

            return retorno;

        }

        public void guardarFaena() {

            SqlConnection cnx = conexion.crearConexion();
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = cnx;
            try
            {

                cmd.CommandText = "insert into faenas(nombre,partida_presupuesto,centro_costo,valor_neto,gastos_generales,utilidades,subtotal,iva) values(@nombre,@partida_presupuesto,@centro_costo,@valor_neto,@gastos_generales,@utilidades,@subtotal,@iva)";
                cmd.Parameters.Add("@nombre", SqlDbType.VarChar).Value = nombre;
                cmd.Parameters.Add("@partida_presupuesto", SqlDbType.Float).Value = partida_presupuesto;
                cmd.Parameters.Add("@centro_costo", SqlDbType.VarChar).Value = centro_costo;
                cmd.Parameters.Add("@valor_neto", SqlDbType.Float).Value = valor_neto;
                cmd.Parameters.Add("@gastos_generales", SqlDbType.Float).Value = gastos_generales;
                cmd.Parameters.Add("@utilidades", SqlDbType.Float).Value = utilidades;
                cmd.Parameters.Add("@subtotal", SqlDbType.Float).Value = subtotal;
                cmd.Parameters.Add("@iva", SqlDbType.Float).Value = iva;

                cmd.CommandType = CommandType.Text;
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Console.Write(ex);                 
            }
            cnx.Close();


        }

        public static void eliminarFaena(string nombre) {
            try
            {
                SqlConnection cnx = conexion.crearConexion();

                SqlCommand cmd = new SqlCommand();
                cmd.Connection = cnx;
                cmd.CommandText = "DELETE FROM faenas WHERE nombre='" + nombre + "'";
                cmd.CommandType = CommandType.Text;
                cmd.ExecuteNonQuery();
                cnx.Close();
            }
            catch (Exception ex) { 
            
            
            }
            
        }

        public List<detalle_ordencompra> obtenerDetalleGastos()
        {
            List<detalle_ordencompra> retorno = new List<detalle_ordencompra>();

            List<partidaGlobal> listaPartidasGlobales = partidaGlobal.obtenerPartidasGlobales(this.nombre);

            for (int i = 0; i < listaPartidasGlobales.Count; i++)
            {
                List<detalle_ordencompra> temp = listaPartidasGlobales[i].obtenerDetalleGastos();
                foreach (detalle_ordencompra gasto in temp)                 
                {
                    retorno.Add(gasto);
                }
            }

            return retorno;
        }
        public double obtenerGastosTotales()
        {
            double retorno = 0;

            List<partidaGlobal> listaPartidasGlobales = partidaGlobal.obtenerPartidasGlobales(this.nombre);

            for (int i = 0; i < listaPartidasGlobales.Count; i++)
            {
                retorno += listaPartidasGlobales[i].obtenerGastosTotales();
            }

            return retorno;
        }

        public double obtenerPresupuesto()
        {
            double retorno = 0;

            List<partidaGlobal> listaPartidasGlobales = partidaGlobal.obtenerPartidasGlobales(this.nombre);

            for (int i = 0; i < listaPartidasGlobales.Count; i++)
            {
                retorno += listaPartidasGlobales[i].obtenerPresupuesto();
            }

            return retorno;
        }

        public double obtenerSaldo()
        {
            double retorno = 0;

            retorno = this.obtenerPresupuesto() - this.obtenerGastosTotales();

            return retorno;
        }
    }

    public class partidaGlobal
    {
        public string nombre { get; set; }
        public string ID { get; set; }
        public string faena { get; set; }
        public List<partida> partidas { get; set; }

        public static List<partidaGlobal> obtenerPartidasGlobales(string nombreFaena)
        {
            List<partidaGlobal> retorno = new List<partidaGlobal>();

            SqlConnection cnx = conexion.crearConexion();

            SqlCommand cmd = new SqlCommand();
            cmd.Connection = cnx;
            cmd.CommandText = "SELECT * from partidas_global WHERE( faena = '" + nombreFaena + "')";
            cmd.CommandType = CommandType.Text;
            SqlDataReader dr = cmd.ExecuteReader();

            while (dr.Read())
            {
                partidaGlobal partidaGlobal = new partidaGlobal();
                partidaGlobal.nombre = (string)dr["nombre"];
                partidaGlobal.ID = (string)dr["id"];
                partidaGlobal.faena = nombreFaena;

                //Agregar el detalle

                retorno.Add(partidaGlobal);
            }
            cnx.Close();

            return retorno;
        }

        public static partidaGlobal obtenerPartidaGlobal(string nombreFaena, string id)
        {
            partidaGlobal retorno = new partidaGlobal();

            SqlConnection cnx = conexion.crearConexion();

            SqlCommand cmd = new SqlCommand();
            cmd.Connection = cnx;
            cmd.CommandText = "SELECT * from partidas_global WHERE( faena = '" + nombreFaena + "' AND id='" + id + "')";
            cmd.CommandType = CommandType.Text;
            SqlDataReader dr = cmd.ExecuteReader();

            while (dr.Read())
            {
                retorno.nombre = (string)dr["nombre"];
                retorno.ID = id;
                retorno.faena = nombreFaena;
            }
            cnx.Close();

            return retorno;
        }

        public List<detalle_ordencompra> obtenerDetalleGastos()
        {
            List<detalle_ordencompra> retorno = new List<detalle_ordencompra>();

            List<partida> listaPartidas = partida.obtenerPartidas(this.faena, this.ID);

            for (int i = 0; i < listaPartidas.Count; i++)
            {
                List<detalle_ordencompra> detalleOrden =  listaPartidas[i].obtenerDetalleGastos();
                foreach(detalle_ordencompra detalle in detalleOrden)
                {
                    retorno.Add(detalle);
                }
            }

            return retorno;
        }

        public double obtenerGastosTotales()
        {
            double retorno = 0;

            List<partida> listaPartidas = partida.obtenerPartidas(this.faena, this.ID);

            for (int i = 0; i < listaPartidas.Count; i++) 
            {
                retorno += listaPartidas[i].obtenerGastosTotales();
            }

            return retorno;
        }
        public double obtenerPresupuesto() 
        {
            double retorno = 0;

            List<partida> listaPartidas = partida.obtenerPartidas(this.faena, this.ID);

            for (int i = 0; i < listaPartidas.Count; i++)
            {
                retorno += listaPartidas[i].total;
            }

            return retorno;
        }
        public double obtenerSaldo()
        {
            double retorno = 0;

            retorno = this.obtenerPresupuesto() - this.obtenerGastosTotales();

            return retorno;
        }
    }
}