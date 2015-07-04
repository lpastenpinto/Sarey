using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace sarey_erp.Models
{
    public class rentasTopesImponibles
    {
        public string descripcion { get; set; }
        public double valor { get; set; }

        public static List<rentasTopesImponibles> obtenerTodos()
        {
            List<rentasTopesImponibles> retorno = new List<rentasTopesImponibles>();

            SqlConnection cnx = conexion.crearConexion();

            SqlCommand cmd = new SqlCommand();
            cmd.Connection = cnx;
            cmd.CommandText = "SELECT * from rentas_topes_imponibles";
            cmd.CommandType = CommandType.Text;
            SqlDataReader dr = cmd.ExecuteReader();

            while (dr.Read())
            {
                rentasTopesImponibles rentaMinima = new rentasTopesImponibles();
                rentaMinima.descripcion = (string)dr["descripcion"];
                rentaMinima.valor = double.Parse(dr["valor"].ToString());

                retorno.Add(rentaMinima);
            }

            cnx.Close();

            return retorno;
        }

        public static void guardar(rentasTopesImponibles nuevo)
        {
            SqlConnection cnx = conexion.crearConexion();

            SqlCommand cmd = new SqlCommand();
            cmd.Connection = cnx;
            cmd.CommandText = "INSERT INTO rentas_topes_imponibles VALUES(@descripcion,@valor)";

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
            cmd.CommandText = "DELETE FROM rentas_topes_imponibles";

            cmd.CommandType = CommandType.Text;
            cmd.ExecuteNonQuery();

            cnx.Close();
        }

        public static rentasTopesImponibles obtenerAfiliadosAFP() {
            return obtenerSegunDescripcion("Para afiliados a una AFP (73,2 UF):");
        }

        public static rentasTopesImponibles obtenerAfiliadosIPS()
        {
            return obtenerSegunDescripcion("Para afiliados al IPS (ex INP) (60 UF):");
        }

        public static rentasTopesImponibles obtenerSeguroCesantia()
        {
            return obtenerSegunDescripcion("Para Seguro de Cesantía (109,8 UF):");
        }

        private static rentasTopesImponibles obtenerSegunDescripcion(string descripcion)
        {
            rentasTopesImponibles datosRentaTope = new rentasTopesImponibles();
            SqlConnection cnx = conexion.crearConexion();

            SqlCommand cmd = new SqlCommand();
            cmd.Connection = cnx;
            cmd.CommandText = "SELECT * from rentas_topes_imponibles WHERE descripcion=@descripcion";
            cmd.CommandType = CommandType.Text;
            cmd.Parameters.Add("@descripcion", SqlDbType.VarChar).Value = descripcion;

            SqlDataReader dr = cmd.ExecuteReader();

            while (dr.Read())
            {
                datosRentaTope.descripcion = (string)dr["descripcion"];
                datosRentaTope.valor = double.Parse(dr["valor"].ToString());
            }

            cnx.Close();
            return datosRentaTope;
        }
    }
}