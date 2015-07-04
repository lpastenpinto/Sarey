using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace sarey_erp.Models
{
    public class controlDatosFinancieros
    {

        public static DateTime obtenerUltimaFechaActualizacion()
        {
            DateTime retorno = DateTime.Now.AddYears(-10);

            SqlConnection cnx = conexion.crearConexion();

            SqlCommand cmd = new SqlCommand();
            cmd.Connection = cnx;
            cmd.CommandText = "SELECT TOP 1 ultima_fecha from control_datos_financieros ORDER BY ultima_fecha DESC";
            cmd.CommandType = CommandType.Text;
            SqlDataReader dr = cmd.ExecuteReader();

            while (dr.Read())
            {
                retorno = (DateTime)dr["ultima_fecha"];
            }

            cnx.Close();

            return retorno;

        }

        public static void registrarNuevaActualizacion()
        {
            SqlConnection cnx = conexion.crearConexion();

            SqlCommand cmd = new SqlCommand();
            cmd.Connection = cnx;
            cmd.CommandText = "INSERT INTO control_datos_financieros VALUES(@fecha)";
            cmd.Parameters.Add("@fecha", SqlDbType.DateTime).Value=DateTime.Now;
            cmd.CommandType = CommandType.Text;
            cmd.ExecuteNonQuery();

            cnx.Close();
        }
    }
}