using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace sarey_erp.Models
{
    public class asignacionFamiliar
    {
        public string tramo { get; set; }
        public float monto { get; set; }
        public float menorQue { get; set; }
        public float mayorQue { get; set; }

        public static void guardar(asignacionFamiliar nueva)
        {
            SqlConnection cnx = conexion.crearConexion();

            SqlCommand cmd = new SqlCommand();
            cmd.Connection = cnx;
            cmd.CommandText = "INSERT INTO asignacion_familiar VALUES(@tramo,@monto,@menorQue,@mayorQue)";

            cmd.Parameters.Add("@tramo", SqlDbType.VarChar).Value = nueva.tramo;
            cmd.Parameters.Add("@monto", SqlDbType.Float).Value = nueva.monto;
            cmd.Parameters.Add("@menorQue", SqlDbType.Float).Value = nueva.menorQue;
            cmd.Parameters.Add("@mayorQue", SqlDbType.Float).Value = nueva.mayorQue;

            cmd.CommandType = CommandType.Text;
            cmd.ExecuteNonQuery();

            cnx.Close();
        }

        public static void borrarDatos()
        {
            SqlConnection cnx = conexion.crearConexion();

            SqlCommand cmd = new SqlCommand();
            cmd.Connection = cnx;
            cmd.CommandText = "DELETE FROM asignacion_familiar";

            cmd.CommandType = CommandType.Text;
            cmd.ExecuteNonQuery();

            cnx.Close();
        }

        public static List<asignacionFamiliar> obtenerTodas() 
        {
            List<asignacionFamiliar> asignaciones = new List<asignacionFamiliar>();

            SqlConnection cnx = conexion.crearConexion();

            SqlCommand cmd = new SqlCommand();
            cmd.Connection = cnx;
            cmd.CommandText = "SELECT * from asignacion_familiar";
            cmd.CommandType = CommandType.Text;
            SqlDataReader dr = cmd.ExecuteReader();

            while (dr.Read())
            {
                asignacionFamiliar asignacionFamiliar = new asignacionFamiliar();
                asignacionFamiliar.tramo = (string)dr["tramo"];
                asignacionFamiliar.monto = float.Parse(dr["monto"].ToString());
                asignacionFamiliar.menorQue = float.Parse(dr["menorQue"].ToString());
                asignacionFamiliar.mayorQue = float.Parse(dr["mayorQue"].ToString());

                asignaciones.Add(asignacionFamiliar);
            }

            cnx.Close();

            return asignaciones;
        }

        public static double obtenerAsignacionSegunMontoImponible(double monto)        
        {
            List<asignacionFamiliar> asignaciones = asignacionFamiliar.obtenerTodas();

            for (int i = 0; i < asignaciones.Count; i++) 
            {
                if(asignaciones[i].menorQue>0){
                    if (monto < asignaciones[i].menorQue && monto > asignaciones[i].mayorQue) return asignaciones[i].monto;
                }
                else
                {
                    if (monto > asignaciones[i].mayorQue) return asignaciones[i].monto;
                }                
            }
            return 0;
        }
    }
}