using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;

namespace sarey_erp.Models
{
    public class conexion
    {
        public static SqlConnection crearConexion()
        {
            //string conexion sql server            
            var fileName = Path.GetFileName("conexion.dat");
            var path = Path.Combine(HttpContext.Current.Server.MapPath("~/Document/"), fileName);

            StreamReader streamReader = new StreamReader(path);
            string text = streamReader.ReadToEnd();
            streamReader.Close();

            string sqldb = text;
            SqlConnection conn = new SqlConnection(sqldb);

            conn.Open();
            return conn;
        }
    }
}