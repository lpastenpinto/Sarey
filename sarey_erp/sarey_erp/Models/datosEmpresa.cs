using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace sarey_erp.Models
{
    public class datosEmpresa
    {

        public string rut { get; set; }
        public string giro { get; set; }
        public string direccion { get; set; }
        public string telefono { get; set; }
        public string correo { get; set; }


        public static datosEmpresa obtenerDatos() 
        {
            datosEmpresa datos = new datosEmpresa();

            SqlConnection cnx = conexion.crearConexion();

            SqlCommand cmd = new SqlCommand();
            cmd.Connection = cnx;
            cmd.CommandText = "SELECT TOP 1 * from datos_empresa";
            cmd.CommandType = CommandType.Text;
            SqlDataReader dr = cmd.ExecuteReader();

            while (dr.Read())
            {
                datos.rut = (string)dr["rut"];
                datos.giro = (string)dr["giro"];
                datos.direccion = (string)dr["direccion"];
                datos.telefono = (string)dr["telefono"];
                datos.correo = (string)dr["correo"];
            }

            cnx.Close();

            return datos;
        }

    }
}