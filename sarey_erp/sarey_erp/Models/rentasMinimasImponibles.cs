using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace sarey_erp.Models
{
    public class rentasMinimasImponibles
    {
        public string descripcion { get; set; }
        public double valor { get; set; }

        public static List<rentasMinimasImponibles> obtenerTodos()
        {
            List<rentasMinimasImponibles> retorno = new List<rentasMinimasImponibles>();

            SqlConnection cnx = conexion.crearConexion();

            SqlCommand cmd = new SqlCommand();
            cmd.Connection = cnx;
            cmd.CommandText = "SELECT * from rentas_minimas_imponibles";
            cmd.CommandType = CommandType.Text;
            SqlDataReader dr = cmd.ExecuteReader();

            while (dr.Read())
            {
                rentasMinimasImponibles rentaMinima = new rentasMinimasImponibles();
                rentaMinima.descripcion = (string)dr["descripcion"];
                rentaMinima.valor = double.Parse(dr["valor"].ToString());

                retorno.Add(rentaMinima);
            }

            cnx.Close();

            return retorno;
        }

        public static void guardar(rentasMinimasImponibles nuevo)
        {
            SqlConnection cnx = conexion.crearConexion();

            SqlCommand cmd = new SqlCommand();
            cmd.Connection = cnx;
            cmd.CommandText = "INSERT INTO rentas_minimas_imponibles VALUES(@descripcion,@valor)";

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
            cmd.CommandText = "DELETE FROM rentas_minimas_imponibles";

            cmd.CommandType = CommandType.Text;
            cmd.ExecuteNonQuery();

            cnx.Close();
        }

        public static rentasMinimasImponibles obtenerMenores18Mayores65() {
            return obtenerSegunDescripcion("Menores de 18 y Mayores de 65:");
        }

        public static rentasMinimasImponibles obtenerTrabajadoresDependientesIndependientes()
        {
            return obtenerSegunDescripcion("Trab. Dependientes e Independientes:");
        }

        public static rentasMinimasImponibles obtenerTrabajadoresCasaParticular()
        {
            return obtenerSegunDescripcion("Trabajadores de Casa Particular:");
        }

        private static rentasMinimasImponibles obtenerSegunDescripcion(string descripcion)
        {
            rentasMinimasImponibles datoSeguroCesantia = new rentasMinimasImponibles();
            SqlConnection cnx = conexion.crearConexion();

            SqlCommand cmd = new SqlCommand();
            cmd.Connection = cnx;
            cmd.CommandText = "SELECT * from rentas_minimas_imponibles WHERE descripcion=@descripcion";
            cmd.CommandType = CommandType.Text;
            cmd.Parameters.Add("@descripcion", SqlDbType.VarChar).Value = descripcion;

            SqlDataReader dr = cmd.ExecuteReader();

            while (dr.Read())
            {
                datoSeguroCesantia.descripcion = (string)dr["descripcion"];
                datoSeguroCesantia.valor = double.Parse(dr["valor"].ToString());
            }

            cnx.Close();
            return datoSeguroCesantia;
        }
    }
}