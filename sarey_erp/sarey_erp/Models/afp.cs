using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;

namespace sarey_erp.Models
{
    public class afp
    {
        public int id_afp{get;set;}
        public string nombre_afp { get; set; }
        public double tasa { get; set; }
        public double sis { get; set; }
        public double tasa_independientes { get; set; }

        public static List<afp> obtenerTodasAFP(){
            List<afp> afps = new List<afp>();

            SqlConnection cnx = conexion.crearConexion();

            SqlCommand cmd = new SqlCommand();
            cmd.Connection = cnx;
            cmd.CommandText = "SELECT * from afp";
            cmd.CommandType = CommandType.Text;
            SqlDataReader dr = cmd.ExecuteReader();

            while (dr.Read())
            {

                afp AFP = new afp();
                AFP.id_afp = (int)dr["id_afp"];
                AFP.nombre_afp = (string)dr["nombre_afp"];
                AFP.tasa = double.Parse(dr["tasa"].ToString());
                AFP.sis = double.Parse(dr["sis"].ToString());
                AFP.tasa_independientes = double.Parse(dr["tasa_independientes"].ToString());

                afps.Add(AFP);
               
            }

            cnx.Close();

            return afps;
            
        }
        public static afp obtenerAFP(string nombre_afp)
        {
           

            SqlConnection cnx = conexion.crearConexion();

            SqlCommand cmd = new SqlCommand();
            cmd.Connection = cnx;
            cmd.CommandText = "SELECT * from afp WHERE nombre_afp='"+nombre_afp+"'";
            cmd.CommandType = CommandType.Text;
            SqlDataReader dr = cmd.ExecuteReader();
            afp AFP = new afp();
            while (dr.Read())
            {

                
                AFP.id_afp = (int)dr["id_afp"];
                AFP.nombre_afp = (string)dr["nombre_afp"];
                AFP.tasa = double.Parse(dr["tasa"].ToString());
                AFP.sis = double.Parse(dr["sis"].ToString());
                AFP.tasa_independientes = double.Parse(dr["tasa_independientes"].ToString());

               

            }

            cnx.Close();

            return AFP;

        }

        public static int obtenerMayorID()
        {
            int retorno = 0;

            SqlConnection cnx = conexion.crearConexion();

            SqlCommand cmd = new SqlCommand();
            cmd.Connection = cnx;
            cmd.CommandText = "SELECT TOP 1 id_afp from afp ORDER BY id_afp DESC";
            cmd.CommandType = CommandType.Text;
            SqlDataReader dr = cmd.ExecuteReader();

            while (dr.Read())
            {
                retorno = (int)dr["id_afp"];
            }

            cnx.Close();

            return retorno;

        }

        public static void guardar(afp nueva)
        {
            SqlConnection cnx = conexion.crearConexion();

            SqlCommand cmd = new SqlCommand();
            cmd.Connection = cnx;
            cmd.CommandText = "INSERT INTO afp VALUES(@id,@nombre,@tasa,@sis,@tasaInd)";

            cmd.Parameters.Add("@id", SqlDbType.Int).Value = obtenerMayorID() + 1;
            cmd.Parameters.Add("@nombre", SqlDbType.VarChar).Value = nueva.nombre_afp;
            cmd.Parameters.Add("@tasa", SqlDbType.Float).Value = nueva.tasa;
            cmd.Parameters.Add("@sis", SqlDbType.Float).Value = nueva.sis;
            cmd.Parameters.Add("@tasaInd", SqlDbType.Float).Value = nueva.tasa_independientes;
            cmd.CommandType = CommandType.Text;
            cmd.ExecuteNonQuery();

            cnx.Close();
        }

        public static void borrarAFPs()
        {
            SqlConnection cnx = conexion.crearConexion();

            SqlCommand cmd = new SqlCommand();
            cmd.Connection = cnx;
            cmd.CommandText = "DELETE FROM afp";

            cmd.CommandType = CommandType.Text;
            cmd.ExecuteNonQuery();

            cnx.Close();
        }
    }
}