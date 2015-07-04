using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace sarey_erp.Models
{
    public class apv
    {
        public string descripcion { get; set; }
        public double valor { get; set; }

        public static List<apv> obtenerTodos()
        {
            List<apv> retorno = new List<apv>();

            SqlConnection cnx = conexion.crearConexion();

            SqlCommand cmd = new SqlCommand();
            cmd.Connection = cnx;
            cmd.CommandText = "SELECT * from ahorro_previsional_voluntario";
            cmd.CommandType = CommandType.Text;
            SqlDataReader dr = cmd.ExecuteReader();

            while (dr.Read())
            {
                apv rentaMinima = new apv();
                rentaMinima.descripcion = (string)dr["descripcion"];
                rentaMinima.valor = double.Parse(dr["valor"].ToString());

                retorno.Add(rentaMinima);
            }

            cnx.Close();

            return retorno;
        }

        public static void guardar(apv nuevo)
        {
            SqlConnection cnx = conexion.crearConexion();

            SqlCommand cmd = new SqlCommand();
            cmd.Connection = cnx;
            cmd.CommandText = "INSERT INTO ahorro_previsional_voluntario VALUES(@descripcion,@valor)";

            cmd.Parameters.Add("@descripcion", SqlDbType.VarChar).Value = nuevo.descripcion;
            cmd.Parameters.Add("@valor", SqlDbType.Float).Value = nuevo.valor;
            cmd.CommandType = CommandType.Text;
            cmd.ExecuteNonQuery();

            cnx.Close();
        }

        public static void borrarDatos()
        {
            SqlConnection cnx = conexion.crearConexion();

            SqlCommand cmd = new SqlCommand();
            cmd.Connection = cnx;
            cmd.CommandText = "DELETE FROM ahorro_previsional_voluntario";

            cmd.CommandType = CommandType.Text;
            cmd.ExecuteNonQuery();

            cnx.Close();
        }

        public static apv obtenerTopeMensual() 
        {
            return obtenerSegunDescripcion("Tope Mensual (50 UF):");
        }

        public static apv obtenerTopeAnual()
        {
            return obtenerSegunDescripcion("Tope Anual (600 UF):");
        }

        private static apv obtenerSegunDescripcion(string descripcion)
        {
            apv datoAhorroPrevision = new apv();
            SqlConnection cnx = conexion.crearConexion();

            SqlCommand cmd = new SqlCommand();
            cmd.Connection = cnx;
            cmd.CommandText = "SELECT * from ahorro_previsional_voluntario WHERE descripcion=@descripcion";
            cmd.CommandType = CommandType.Text;
            cmd.Parameters.Add("@descripcion", SqlDbType.VarChar).Value = descripcion;

            SqlDataReader dr = cmd.ExecuteReader();

            while (dr.Read())
            {
                datoAhorroPrevision.descripcion = (string)dr["descripcion"];
                datoAhorroPrevision.valor = double.Parse(dr["valor"].ToString());
            }

            cnx.Close();
            return datoAhorroPrevision;
        }
    }
}