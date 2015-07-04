using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace sarey_erp.Models
{
    public class uf
    {
        public double valor { get; set; }
        public DateTime fecha { get; set; }

        public static DateTime obtenerUltimaFecha() 
        {
            DateTime retorno = new DateTime(2000, 1, 1);

            SqlConnection cnx = conexion.crearConexion();

            SqlCommand cmd = new SqlCommand();
            cmd.Connection = cnx;
            cmd.CommandText = "SELECT TOP 1 fecha FROM uf ORDER BY fecha DESC";
            cmd.CommandType = CommandType.Text;
            SqlDataReader dr = cmd.ExecuteReader();

            while (dr.Read())
            {
                retorno = (DateTime)dr["fecha"];
            }

            cnx.Close();

            return retorno;
        }

        public static uf obtenerUltimaUF()
        {
            uf retorno = new uf();

            SqlConnection cnx = conexion.crearConexion();

            SqlCommand cmd = new SqlCommand();
            cmd.Connection = cnx;
            cmd.CommandText = "SELECT TOP 1 * FROM uf ORDER BY fecha DESC";
            cmd.CommandType = CommandType.Text;
            SqlDataReader dr = cmd.ExecuteReader();

            while (dr.Read())
            {
                retorno.fecha = (DateTime)dr["fecha"];
                retorno.valor = double.Parse(dr["valor"].ToString());
            }

            cnx.Close();

            return retorno;
        }

        public static uf obtenerUF(int MES, int año)
        {
            uf retorno = new uf();

            DateTime finDeMes = new DateTime(año, MES, 1);
            int mes = finDeMes.Month;
            DateTime temp = finDeMes;
            while (temp.Month == mes)
            {
                finDeMes = temp;
                temp = temp.AddDays(1);
            }

            SqlConnection cnx = conexion.crearConexion();

            SqlCommand cmd = new SqlCommand();
            cmd.Connection = cnx;
            cmd.CommandText = "SELECT * FROM uf WHERE fecha=@fecha";
            cmd.CommandType = CommandType.Text;

            cmd.Parameters.Add("@fecha", SqlDbType.DateTime).Value= finDeMes;

            SqlDataReader dr = cmd.ExecuteReader();

            while (dr.Read())
            {
                retorno.fecha = (DateTime)dr["fecha"];
                retorno.valor = double.Parse(dr["valor"].ToString());
            }

            cnx.Close();

            return retorno;
        }
        
        internal static void guardarNueva(double nuevaUF, DateTime finDeMes)
        {
            SqlConnection cnx = conexion.crearConexion();

            SqlCommand cmd = new SqlCommand();
            cmd.Connection = cnx;
            cmd.CommandText = "INSERT INTO uf VALUES(@valor,@fecha)";
            cmd.CommandType = CommandType.Text;

            cmd.Parameters.Add("@valor", SqlDbType.Float).Value=nuevaUF;
            cmd.Parameters.Add("@fecha", SqlDbType.DateTime).Value = finDeMes;

            cmd.ExecuteNonQuery();

            cnx.Close();
        }
    }
}