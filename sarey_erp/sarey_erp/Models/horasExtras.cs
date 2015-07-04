using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;

namespace sarey_erp.Models
{
    public class horasExtras
    {
        public string rut {set; get;}
        public DateTime fecha { set; get; }
        public double cantidadHoras { set;get; }

        public static void GuardarHorasExtras(string rut, DateTime fecha, double cantidadHoras) {

            SqlConnection cnx = conexion.crearConexion();

            SqlCommand cmd = new SqlCommand();
            cmd.Connection = cnx;
            cmd.CommandText = "INSERT INTO horas_extras VALUES(@rut,@fecha,@cantidadHoras)";
            cmd.CommandType = CommandType.Text;
            cmd.Parameters.Add("@rut", SqlDbType.VarChar).Value = rut;
            cmd.Parameters.Add("@fecha", SqlDbType.DateTime).Value = fecha;
            cmd.Parameters.Add("@cantidadHoras", SqlDbType.Float).Value = cantidadHoras; 

            cmd.ExecuteNonQuery();
            cnx.Close();
        
        }

        public static double obtenerHorasExtrasMes(string rut, int mes, int anio) {
            double horasExtras=0;

            DateTime fechaInicio = new DateTime(anio,mes,1);
            DateTime fechaFinal = new DateTime(anio, mes, 30);


            SqlConnection cnx = conexion.crearConexion();
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = cnx;
            cmd.CommandText = "SELECT * FROM horas_extras WHERE fecha >= @fechaInicial AND fecha <=@fechaFinal AND rut=@rut";

            cmd.Parameters.Add("@fechaInicial", SqlDbType.DateTime).Value = fechaInicio;
            cmd.Parameters.Add("@fechaFinal", SqlDbType.DateTime).Value = fechaFinal;
            cmd.Parameters.Add("@rut", SqlDbType.VarChar).Value = rut;


            cmd.CommandType = CommandType.Text;
            SqlDataReader dr = cmd.ExecuteReader();
           
            
            while (dr.Read())
            {
                horasExtras=horasExtras+(double)dr["cantidadHoras"];                

            }
            cnx.Close();
            return horasExtras;
        }


        public static List<horasExtras> obtenerListHorasExtrasMes(string rut, int mes, int anio)
        {

            List<horasExtras> ListaHorasExtras = new List<horasExtras>();

            DateTime fechaInicio = new DateTime(anio, mes, 1);
            DateTime fechaFinal = new DateTime(anio, mes, 30);


            SqlConnection cnx = conexion.crearConexion();
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = cnx;
            cmd.CommandText = "SELECT * FROM horas_extras WHERE fecha >= @fechaInicial AND fecha <=@fechaFinal AND rut=@rut";

            cmd.Parameters.Add("@fechaInicial", SqlDbType.DateTime).Value = fechaInicio;
            cmd.Parameters.Add("@fechaFinal", SqlDbType.DateTime).Value = fechaFinal;
            cmd.Parameters.Add("@rut", SqlDbType.VarChar).Value = rut;


            cmd.CommandType = CommandType.Text;
            SqlDataReader dr = cmd.ExecuteReader();


            while (dr.Read())
            {
                horasExtras HoraExtra = new horasExtras();
                HoraExtra.fecha=(DateTime)dr["fecha"];
                HoraExtra.cantidadHoras=(double)dr["cantidadHoras"];
                ListaHorasExtras.Add(HoraExtra);
            }
            cnx.Close();
            return ListaHorasExtras;
        }

    }
}