using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;

namespace sarey_erp.Models
{
    public class cargafamiliar
    {
        public string rut_trabajador { set; get; }
        public string rut { set; get; }
        public string nombre_carga { set; get; }
        public string sexo { get; set; }
        public string edad { get; set; }
        public string carga_registrada { set; get; }

        public static void guardarDatosFamilia(string rut_trabajador, string[] rutcarga, string[] nombre_carga, string[] sexocarga, string[] edadcarga, string[] carga_registrada)
        {
            SqlConnection cnx = conexion.crearConexion();
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = cnx;
            for (int i = 0; i < nombre_carga.Length; i++)
            {
                cmd.CommandText = "insert into cargas_trabajador values('"
                       + rut_trabajador + "','"
                       + rutcarga[i] + "','"
                       + nombre_carga[i] + "','"
                       + sexocarga[i] + "','"
                       + edadcarga[i] + "','"
                       + carga_registrada[i] + "')";

                    cmd.CommandType = CommandType.Text;
                    cmd.ExecuteNonQuery();
            }
            cnx.Close();
        }
        public static List<cargafamiliar> obtenerCargas(string rut)
        {
           
            List<cargafamiliar> cargas = new List<cargafamiliar>();
            SqlConnection cnx = conexion.crearConexion();
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = cnx;
            cmd.CommandText = "SELECT * from cargas_trabajador WHERE rut_trabajador='" + rut + "'";
            cmd.CommandType = CommandType.Text;
            SqlDataReader dr = cmd.ExecuteReader();

            while (dr.Read())
            {
                    cargafamiliar carga = new cargafamiliar();
                    carga.rut = (string)dr["rut_carga"];
                    carga.nombre_carga = (string)dr["nombre"];
                    carga.sexo = (string)dr["sexo"];
                    carga.edad = (string)dr["edad"];
                    carga.carga_registrada = (string)dr["carga_registrada"];
                    cargas.Add(carga);
            }
            cnx.Close();            
            return cargas;
        }
        public static void borrarCargas (string rut_trabajador)
        {
            SqlConnection cnx = conexion.crearConexion();
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = cnx;
            cmd.CommandText = "DELETE FROM cargas_trabajador WHERE rut_trabajador='" + rut_trabajador + "'";
            cmd.CommandType = CommandType.Text;
            cmd.ExecuteNonQuery();
            cnx.Close();
        }
        public static bool tieneCargas(string rut_trabajador)
        {
      
            bool retorno = false;

            SqlConnection cnx = conexion.crearConexion();

            SqlCommand cmd = new SqlCommand();
            cmd.Connection = cnx;
            cmd.CommandText = "SELECT * from cargas_trabajador WHERE rut_trabajador='" + rut_trabajador + "'";
            cmd.CommandType = CommandType.Text;
            SqlDataReader dr = cmd.ExecuteReader();
           
            retorno = dr.HasRows;
            
            cnx.Close();

            return retorno;
        }
    }
}