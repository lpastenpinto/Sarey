using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;

namespace sarey_erp.Models
{
    public class isapre
    {
        public int id_isapre{get;set;}
        public string nombre_isapre { set; get; }

        public static List<isapre> obtenerTodasIsapres() {
            List<isapre> isapres = new List<isapre>();
            SqlConnection cnx = conexion.crearConexion();

            SqlCommand cmd = new SqlCommand();
            cmd.Connection = cnx;
            cmd.CommandText = "SELECT * from isapres";
            cmd.CommandType = CommandType.Text;
            SqlDataReader dr = cmd.ExecuteReader();

            while (dr.Read())
            {
                isapre isapre = new isapre();
                isapre.id_isapre = (int)dr["id_isapre"];
                isapre.nombre_isapre = (string)dr["nombre_isapre"];
                isapres.Add(isapre);
                
            }
            return isapres;
            
        }

    }
}