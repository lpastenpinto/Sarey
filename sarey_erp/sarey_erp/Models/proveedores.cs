using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Data;

namespace sarey_erp.Models
{
    public class proveedores
    {
        public string nombre_proveedor { get; set; }
        public string nombre_contacto { get; set; }
        public string correo_contacto { get; set; }
        public string telefono { get; set; }
        public string direccion { get; set; }
        public string razonsocial { get; set; }
        public string rut { get; set; }

        public static void agregarProveedor(proveedores nuevo)
        {
            SqlConnection cnx = conexion.crearConexion();

            SqlCommand cmd = new SqlCommand();
            cmd.Connection = cnx;
            cmd.CommandText = "INSERT INTO proveedores VALUES(@nombre_proveedor,@nombre_contacto,"
                + "@correo_contacto,@telefono,@direccion,@razon_social,@rut)";

            cmd.Parameters.Add("@nombre_proveedor", SqlDbType.VarChar).Value = nuevo.nombre_proveedor;
            cmd.Parameters.Add("@nombre_contacto", SqlDbType.VarChar).Value = nuevo.nombre_contacto;
            cmd.Parameters.Add("@correo_contacto", SqlDbType.VarChar).Value = nuevo.correo_contacto;
            cmd.Parameters.Add("@telefono", SqlDbType.VarChar).Value = nuevo.telefono;
            cmd.Parameters.Add("@direccion", SqlDbType.VarChar).Value = nuevo.direccion;
            cmd.Parameters.Add("@razon_social", SqlDbType.VarChar).Value = nuevo.razonsocial;
            cmd.Parameters.Add("@rut", SqlDbType.VarChar).Value = nuevo.rut;

            cmd.CommandType = CommandType.Text;
            cmd.ExecuteNonQuery();
            cnx.Close();
        }
        public static List<proveedores> obtenerTodos()
        {
            List<proveedores> retorno = new List<proveedores>();

            SqlConnection cnx = conexion.crearConexion();

            SqlCommand cmd = new SqlCommand();
            cmd.Connection = cnx;
            cmd.CommandText = "SELECT * from proveedores ORDER BY nombre_proveedor ASC";
            cmd.CommandType = CommandType.Text;
            SqlDataReader dr = cmd.ExecuteReader();

            while (dr.Read())
            {
                proveedores proveedor = new proveedores();
                proveedor.nombre_proveedor = (string)dr["nombre_proveedor"];
                proveedor.nombre_contacto = (string)dr["nombre_contacto"];
                proveedor.correo_contacto = (string)dr["correo_contacto"];
                proveedor.telefono = (string)dr["telefono"];
                proveedor.direccion = (string)dr["direccion"];
                proveedor.razonsocial = (string)dr["razon_social"];
                proveedor.rut = (string)dr["rut"];
                
                retorno.Add(proveedor);
            }
            cnx.Close();

            return retorno;
        }
        public static bool verificarSiExiste(string valor_proveedor)
        {
            bool retorno = false;

            SqlConnection cnx = conexion.crearConexion();
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = cnx;
            cmd.CommandText = "SELECT * FROM proveedores WHERE nombre_proveedor='" + valor_proveedor + "'";
            cmd.CommandType = CommandType.Text;

            SqlDataReader dr = cmd.ExecuteReader();

            retorno = dr.HasRows;

            cnx.Close();

            return retorno;
        }
        public proveedores getProveedor(string id)
        {
            SqlConnection cnx = conexion.crearConexion();
            proveedores tdato = new proveedores();
            try
            {
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = cnx;
                string sqlcmd = string.Empty;
                sqlcmd = "select * ";
                sqlcmd += "from proveedores ";
                sqlcmd += "where ( nombre_proveedor = '" + id + "')";

                cmd.CommandText = sqlcmd;
                cmd.CommandType = CommandType.Text;
                SqlDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    tdato.nombre_proveedor = (string)dr["nombre_proveedor"];
                    tdato.nombre_contacto = (string)dr["nombre_contacto"];
                    tdato.correo_contacto = (string)dr["correo_contacto"];
                    tdato.telefono = (string)dr["telefono"];
                    tdato.direccion = (string)dr["direccion"];
                    tdato.razonsocial = (string)dr["razon_social"];
                    tdato.rut = (string)dr["rut"];
                }
                dr.Close();
            }
            catch (Exception)
            {
            }
            cnx.Close();
            return tdato;
        }
        public static void borrarproveedor(string id)
        {
            SqlConnection cnx = conexion.crearConexion();
            try
            {
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = cnx;
                string sqlcmd = string.Empty;
                sqlcmd = "Delete From ";
                sqlcmd += "proveedores ";
                sqlcmd += "where (nombre_proveedor= '" + id + "')";

                cmd.CommandText = sqlcmd;
                cmd.CommandType = CommandType.Text;
                cmd.ExecuteNonQuery();
                cnx.Close();

            }
            catch (Exception e)
            {
                Console.Write(e);
                cnx.Close();
            }

        }
    }

}