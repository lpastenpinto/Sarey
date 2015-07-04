using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace sarey_erp.Models
{
    public class licenciasTrabajadores
    {
        public DateTime fecha { get; set; }
        public string rut { get; set; }
        public string descripcion { get; set; }


        public static int cantidadDiasLicencia(string rut, DateTime fechaInicio, DateTime fechaFinal) { 
                
            SqlConnection cnx = conexion.crearConexion();
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = cnx;
            cmd.CommandText = "SELECT * FROM licencias_trabajadores WHERE fecha >= @fechaInicial AND fecha <=@fechaFinal AND rut=@rut";
            
            cmd.Parameters.Add("@fechaInicial", SqlDbType.DateTime).Value = fechaInicio;
            cmd.Parameters.Add("@fechaFinal", SqlDbType.DateTime).Value = fechaFinal;
            cmd.Parameters.Add("@rut", SqlDbType.VarChar).Value = rut;
            

            cmd.CommandType = CommandType.Text;
            SqlDataReader dr = cmd.ExecuteReader();
            int cantidadDias=0;
            
            while (dr.Read())
            {
                licenciasTrabajadores temp = new licenciasTrabajadores();
                temp.rut = (string)dr["rut"];
                temp.fecha = (DateTime)dr["fecha"];
                temp.descripcion = (string)dr["descripcion"];
                cantidadDias++;
                
            }
            cnx.Close();

            return cantidadDias;
        }


        public static List<licenciasTrabajadores> obtenerTodas(string rut)
        {
            List<licenciasTrabajadores> retorno = new List<licenciasTrabajadores>();

            SqlConnection cnx = conexion.crearConexion();
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = cnx;
            cmd.CommandText = "SELECT * FROM licencias_trabajadores WHERE rut='" + rut + "'";
            cmd.CommandType = CommandType.Text;

            SqlDataReader dr = cmd.ExecuteReader();

            while (dr.Read())
            {
                licenciasTrabajadores temp = new licenciasTrabajadores();
                temp.rut = (string)dr["rut"];
                temp.fecha = (DateTime)dr["fecha"];
                temp.descripcion = (string)dr["descripcion"];

                retorno.Add(temp);
            }
            cnx.Close();

            return retorno;
        }

        public void guardarDatos(licenciasTrabajadores licencia)
        {
            SqlConnection cnx = conexion.crearConexion();

            SqlCommand cmd = new SqlCommand();
            cmd.Connection = cnx;
            cmd.CommandText = "INSERT INTO licencias_trabajadores VALUES(@fecha,'"
                + licencia.rut + "','"
                + licencia.descripcion + "')";
            cmd.CommandType = CommandType.Text;
            cmd.Parameters.Add("@fecha", SqlDbType.DateTime).Value = licencia.fecha;
            
            cmd.ExecuteNonQuery();
            cnx.Close();
        }

        internal static bool existe(string fecha, string rut)
        {
            int año = int.Parse(fecha.Split('/')[2]);
            int mes = int.Parse(fecha.Split('/')[1]);
            int dia = int.Parse(fecha.Split('/')[0]);

            DateTime FECHA = new DateTime(año, mes, dia, 0, 0, 0);

            string rutFormateado = rut.Replace(".", "").Replace("-", "");

            bool retorno = false;

            SqlConnection cnx = conexion.crearConexion();
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = cnx;
            cmd.CommandText = "SELECT * FROM licencias_trabajadores WHERE rut='" + rutFormateado + "' AND fecha=@fecha" ;
            cmd.CommandType = CommandType.Text;
            cmd.Parameters.Add("@fecha", SqlDbType.DateTime).Value = FECHA;

            SqlDataReader dr = cmd.ExecuteReader();

            retorno = dr.HasRows;

            cnx.Close();

            return retorno;
        }

        internal static bool existe(licenciasTrabajadores licencia)
        {
            DateTime FECHA = licencia.fecha;

            string rutFormateado = licencia.rut.Replace(".", "").Replace("-", "");

            bool retorno = false;

            SqlConnection cnx = conexion.crearConexion();
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = cnx;
            cmd.CommandText = "SELECT * FROM licencias_trabajadores WHERE rut='" + rutFormateado + "' AND fecha=@fecha";
            cmd.CommandType = CommandType.Text;
            cmd.Parameters.Add("@fecha", SqlDbType.DateTime).Value = FECHA;

            SqlDataReader dr = cmd.ExecuteReader();

            retorno = dr.HasRows;

            cnx.Close();

            return retorno;
        }
    }
}