using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;

namespace sarey_erp.Models
{
    public class feriados
    {
        public DateTime dia { get; set; }
        public string festividad { get; set; }
        public string tipo_feriado { get; set; }
        public string irrenunciable { get; set; }

        public static void Guardar(feriados nuevo)
        {
            SqlConnection cnx = conexion.crearConexion();
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = cnx;

            cmd.CommandText = "insert into dias_feriados (dia,festividad,tipo_feriado,irrenunciable) values(@dia,@festividad,@tipo_feriado,@irrenunciable)";
            cmd.Parameters.Add("@dia", SqlDbType.DateTime).Value = nuevo.dia;
            cmd.Parameters.Add("@festividad", SqlDbType.VarChar).Value = nuevo.festividad;
            cmd.Parameters.Add("@tipo_feriado", SqlDbType.VarChar).Value = nuevo.tipo_feriado;
            cmd.Parameters.Add("@irrenunciable", SqlDbType.VarChar).Value = nuevo.irrenunciable;
   
            cmd.CommandType = CommandType.Text;
            cmd.ExecuteNonQuery();
            cnx.Close();
        }
        public static List<feriados> Obtenerdias()
        {
            List<feriados> retorno = new List<feriados>();

            SqlConnection cnx = conexion.crearConexion();
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = cnx;
            cmd.CommandText = "SELECT * FROM dias_feriados ORDER BY dia ASC";
            cmd.CommandType = CommandType.Text;

            SqlDataReader dr = cmd.ExecuteReader();

            while (dr.Read())
            {
                feriados temp = new feriados();
                temp.dia = (DateTime)dr["dia"];
                temp.festividad = (string)dr["festividad"];
                temp.tipo_feriado = (string)dr["tipo_feriado"];
                temp.irrenunciable = (string)dr["irrenunciable"];
                retorno.Add(temp);
            }
            cnx.Close();
            return retorno;   
        }
        public static int ObtenerdiasRango(DateTime fechainicio, DateTime fechafin)
        {
            SqlConnection cnx = conexion.crearConexion();
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = cnx;

            /*int año = int.Parse(inicio.Split('/')[2]);
            int mes = int.Parse(inicio.Split('/')[1]);
            int dia = int.Parse(inicio.Split('/')[0]);
            
            DateTime fechainicio = new DateTime(año, mes, dia, 0, 0, 0);

            año = int.Parse(fin.Split('/')[2]);
            mes = int.Parse(fin.Split('/')[1]);
            dia = int.Parse(fin.Split('/')[0]);

            DateTime fechafin = new DateTime(año, mes, dia, 0, 0, 0);
            */
            cmd.CommandText = "SELECT COUNT(*) FROM dias_feriados WHERE dia>=@fechainicio AND dia<=@fechafin";
            cmd.CommandType = CommandType.Text;
            cmd.Parameters.Add("@fechainicio", SqlDbType.DateTime).Value = fechainicio;
            cmd.Parameters.Add("@fechafin", SqlDbType.DateTime).Value = fechafin;

            int count = (int)cmd.ExecuteScalar();

            cnx.Close();
            return count;
        }
    }
}