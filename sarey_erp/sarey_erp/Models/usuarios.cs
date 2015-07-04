using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace sarey_erp.Models
{
    public class usuarios
    {
        public string nombre { get; set; }
        public string nombreCompleto { get; set; }
        public string correoElectronico { get; set; }
        public int telefono { get; set; }
        public string rol { get; set; }
        public string rut { get; set; }
        public string cargo { get; set; }

        public static usuarios obtenerUsuario(string nombre) 
        {
            usuarios retorno = new usuarios();

            retorno.nombre = nombre;
            SqlConnection cnx = conexion.crearConexion();

            SqlCommand cmd = new SqlCommand();
            cmd.Connection = cnx;
            cmd.CommandText = "SELECT * from usuarios where nombre='" + nombre + "'";
            cmd.CommandType = CommandType.Text;
            SqlDataReader dr = cmd.ExecuteReader();

            while (dr.Read())
            {
                retorno.nombreCompleto = (string)dr["nombre_completo"];
                retorno.correoElectronico = (string)dr["correo"];
                retorno.telefono = (int)dr["telefono"];
                retorno.rol = (string)dr["rol"];
                retorno.rut = (string)dr["rut"];
                retorno.cargo = (string)dr["cargo"];
            }

            cnx.Close();

            return retorno;
        }

        public static bool revisarUsuarioPassword(string usuario, string password)
        {

            SqlConnection con = conexion.crearConexion();

            SqlCommand cmd = new SqlCommand();
            cmd.Connection = con;
            cmd.CommandText = "SELECT password from usuarios where nombre='" + usuario + "'";
            cmd.CommandType = CommandType.Text;
            SqlDataReader dr = cmd.ExecuteReader();

            string pass = "";

            while (dr.Read())
            {
                pass = (string)dr["password"];
            }

            con.Close();
            if (pass == password) return true;
            return false;
        }

        public static string obtenerRol(string usuario)
        {
            SqlConnection cnx = conexion.crearConexion();

            SqlCommand cmd = new SqlCommand();
            cmd.Connection = cnx;
            cmd.CommandText = "SELECT rol from usuarios where nombre='" + usuario + "'";
            cmd.CommandType = CommandType.Text;
            SqlDataReader dr = cmd.ExecuteReader();

            string rol = "";

            while (dr.Read())
            {
                rol = (string)dr["rol"];
            }

            cnx.Close();
            return rol;
        }

        public static void cambiarContraseña(string nombre, string password) 
        {
            SqlConnection cnx = conexion.crearConexion();

            SqlCommand cmd = new SqlCommand();
            cmd.Connection = cnx;
            cmd.CommandText = "UPDATE usuarios SET password='" + password 
                + "' WHERE nombre='" + nombre + "'";
            cmd.CommandType = CommandType.Text;
            cmd.ExecuteNonQuery();
            cnx.Close();
        }

        public static List<usuarios> obtenerTodos()
        {
            List<usuarios> retorno = new List<usuarios>();
            SqlConnection cnx = conexion.crearConexion();

            SqlCommand cmd = new SqlCommand();
            cmd.Connection = cnx;
            cmd.CommandText = "SELECT nombre from usuarios";
            cmd.CommandType = CommandType.Text;
            SqlDataReader dr = cmd.ExecuteReader();

            while (dr.Read())
            {
                usuarios temp = usuarios.obtenerUsuario((string)dr["nombre"]);
                retorno.Add(temp);
            }

            cnx.Close();

            return retorno;
        }

        public static void agregarBD(usuarios user, string password) 
        {
            SqlConnection cnx = conexion.crearConexion();

            SqlCommand cmd = new SqlCommand();
            cmd.Connection = cnx;
            cmd.CommandText = "INSERT INTO usuarios VALUES(@nombre,@password,@nombre_completo,"
                + "@correo,@telefono,@rol,@rut,@cargo)";

            cmd.Parameters.Add("@nombre",SqlDbType.VarChar).Value=user.nombre;
            cmd.Parameters.Add("@password", SqlDbType.VarChar).Value = password;
            cmd.Parameters.Add("@nombre_completo", SqlDbType.VarChar).Value = user.nombreCompleto;
            cmd.Parameters.Add("@correo", SqlDbType.VarChar).Value = user.correoElectronico;
            cmd.Parameters.Add("@telefono", SqlDbType.VarChar).Value = user.telefono;
            cmd.Parameters.Add("@rol", SqlDbType.VarChar).Value = user.rol;
            cmd.Parameters.Add("@rut", SqlDbType.VarChar).Value = user.rut;
            cmd.Parameters.Add("@cargo", SqlDbType.VarChar).Value = user.cargo;            

            cmd.CommandType = CommandType.Text;
            cmd.ExecuteNonQuery();
            cnx.Close();
        }
        public static void editarBD(usuarios user, string password, string nombreAnterior) 
        {
            SqlConnection cnx = conexion.crearConexion();

            if (user.nombre.Equals("admin")) return;//El usuario admin no se puede modificar

            SqlCommand cmd = new SqlCommand();
            cmd.Connection = cnx;
            cmd.CommandText = "UPDATE usuarios SET nombre=@nombre, nombre_completo=@nombre_completo,"
                + "correo=@correo,telefono=@telefono,rol=@rol,password=@password, rut=@rut, cargo=@cargo"
                + " WHERE nombre=@nombreAnterior";

            cmd.Parameters.Add("@nombre",SqlDbType.VarChar).Value=user.nombre;
            cmd.Parameters.Add("@nombre_completo", SqlDbType.VarChar).Value = user.nombreCompleto;
            cmd.Parameters.Add("@correo", SqlDbType.VarChar).Value = user.correoElectronico;
            cmd.Parameters.Add("@telefono", SqlDbType.VarChar).Value = user.telefono;
            cmd.Parameters.Add("@rol", SqlDbType.VarChar).Value = user.rol;
            cmd.Parameters.Add("@password", SqlDbType.VarChar).Value = password;
            cmd.Parameters.Add("@rut", SqlDbType.VarChar).Value = user.rut;
            cmd.Parameters.Add("@cargo", SqlDbType.VarChar).Value = user.cargo;
            cmd.Parameters.Add("@nombreAnterior", SqlDbType.VarChar).Value = nombreAnterior;

            cmd.CommandType = CommandType.Text;
            cmd.ExecuteNonQuery();
            cnx.Close();
        }
        
        public static void eliminarBD(string nombre) 
        {
            SqlConnection cnx = conexion.crearConexion();

            SqlCommand cmd = new SqlCommand();
            cmd.Connection = cnx;
            cmd.CommandText = "DELETE FROM usuarios WHERE nombre='" + nombre + "'";
            cmd.CommandType = CommandType.Text;
            cmd.ExecuteNonQuery();
            cnx.Close();
        }

        public static bool existeUsuario(string nombre)
        {
            bool retorno = false;

            SqlConnection con = conexion.crearConexion();

            SqlCommand cmd = new SqlCommand();
            cmd.Connection = con;
            cmd.CommandText = "SELECT nombre from usuarios where nombre='" + nombre + "'";
            cmd.CommandType = CommandType.Text;
            SqlDataReader dr = cmd.ExecuteReader();

            retorno = dr.HasRows;

            con.Close();

            return retorno;
        }

        public static List<string> obtenerRoles()
        {
            List<string> retorno = new List<string>();

            SqlConnection con = conexion.crearConexion();

            SqlCommand cmd = new SqlCommand();
            cmd.Connection = con;
            cmd.CommandText = "SELECT nombre from tipos_usuario";
            cmd.CommandType = CommandType.Text;
            SqlDataReader dr = cmd.ExecuteReader();

            while (dr.Read()) 
            {
                retorno.Add((string)dr["nombre"]);
            }

            con.Close();

            return retorno;
        }
    }
}