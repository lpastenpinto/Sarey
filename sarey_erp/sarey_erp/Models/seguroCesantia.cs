using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace sarey_erp.Models
{
    public class seguroCesantia
    {
        public string descripcion { get; set; }
        public double empleador { get; set; }
        public double trabajador { get; set; }

        public static List<seguroCesantia> obtenerTodos()
        {
            List<seguroCesantia> afps = new List<seguroCesantia>();

            SqlConnection cnx = conexion.crearConexion();

            SqlCommand cmd = new SqlCommand();
            cmd.Connection = cnx;
            cmd.CommandText = "SELECT * from seguro_cesantia";
            cmd.CommandType = CommandType.Text;
            SqlDataReader dr = cmd.ExecuteReader();

            while (dr.Read())
            {

                seguroCesantia seguroCesantia = new seguroCesantia();
                seguroCesantia.descripcion = (string)dr["descripcion"];
                seguroCesantia.empleador = double.Parse(dr["empleador"].ToString());
                seguroCesantia.trabajador = double.Parse(dr["trabajador"].ToString());

                afps.Add(seguroCesantia);
            }

            cnx.Close();

            return afps;

        }

        public static void guardar(seguroCesantia nuevo)
        {
            SqlConnection cnx = conexion.crearConexion();

            SqlCommand cmd = new SqlCommand();
            cmd.Connection = cnx;
            cmd.CommandText = "INSERT INTO seguro_cesantia VALUES(@descripcion,@empleador,@trabajador)";

            cmd.Parameters.Add("@descripcion", SqlDbType.VarChar).Value = nuevo.descripcion;
            cmd.Parameters.Add("@empleador", SqlDbType.Float).Value = nuevo.empleador;
            cmd.Parameters.Add("@trabajador", SqlDbType.Float).Value = nuevo.trabajador;
            cmd.CommandType = CommandType.Text;
            cmd.ExecuteNonQuery();

            cnx.Close();
        }

        public static void borrarDatos()
        {
            SqlConnection cnx = conexion.crearConexion();

            SqlCommand cmd = new SqlCommand();
            cmd.Connection = cnx;
            cmd.CommandText = "DELETE FROM seguro_cesantia";

            cmd.CommandType = CommandType.Text;
            cmd.ExecuteNonQuery();

            cnx.Close();
        }

        public static seguroCesantia obtenerPlazoFijo() 
        {
            return obtenerSegunDescripcion("Contrato Plazo Fijo");
        }

        public static seguroCesantia obtenerPlazoIndefinido()
        {
            return obtenerSegunDescripcion("Contrato Plazo Indefinido");
        }

        public static seguroCesantia obtenerPlazoIndefinidoOnceAñosoMas()
        {
            return obtenerSegunDescripcion("Contrato Plazo Indefinido 11 años o más");
        }

        private static seguroCesantia obtenerSegunDescripcion(string descripcion)
        {
            seguroCesantia datoSeguroCesantia = new seguroCesantia();
            SqlConnection cnx = conexion.crearConexion();

            SqlCommand cmd = new SqlCommand();
            cmd.Connection = cnx;
            cmd.CommandText = "SELECT * from seguro_cesantia WHERE descripcion=@descripcion";
            cmd.CommandType = CommandType.Text;
            cmd.Parameters.Add("@descripcion", SqlDbType.VarChar).Value = descripcion;

            SqlDataReader dr = cmd.ExecuteReader();

            while (dr.Read())
            {
                datoSeguroCesantia.descripcion = (string)dr["descripcion"];
                datoSeguroCesantia.empleador = double.Parse(dr["empleador"].ToString());
                datoSeguroCesantia.trabajador = double.Parse(dr["trabajador"].ToString());
            }

            cnx.Close();
            return datoSeguroCesantia;
        }

    }
}