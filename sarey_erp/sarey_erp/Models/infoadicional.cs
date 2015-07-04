using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;

namespace sarey_erp.Models
{
    public class infoadicional
    {
        public string rut { set; get; }
        public string estado_civil { set; get; }
        public string numero_calzado { get; set; }
        public string talla_ropa { get; set; }
        public string grupo_sangre { set; get; }
        public string alergico { set; get; }
        public string personal_destacado { set; get; }
        public string antecedentes_conducir { set; get; }
      
        public List<cargafamiliar> carga { set; get; }

        public static void guardarDatosTrabajador(infoadicional nuevo)
        {

            if (existenDatos(nuevo.rut))
            {
                SqlConnection cnx = conexion.crearConexion();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = cnx;
                try
                {
                    cmd.CommandText = "UPDATE info_trabajador_adicional SET estado_civil=@estado_civil,talla_ropa=@talla_ropa,grupo_sanguineo=@grupo_sanguineo,alergico=@alergico,personal_destacado=@personal_destacado,calzado=@calzado,ante_conducir=@ante_conducir WHERE rut_trabajador='" + nuevo.rut + "'";
                    cmd.Parameters.Add("@rut_trabajador", SqlDbType.VarChar).Value = nuevo.rut;
                    cmd.Parameters.Add("@estado_civil", SqlDbType.VarChar).Value = nuevo.estado_civil;
                    cmd.Parameters.Add("@talla_ropa", SqlDbType.VarChar).Value = nuevo.talla_ropa;
                    cmd.Parameters.Add("@grupo_sanguineo", SqlDbType.VarChar).Value = nuevo.grupo_sangre;
                    cmd.Parameters.Add("@alergico", SqlDbType.VarChar).Value = nuevo.alergico;
                    cmd.Parameters.Add("@personal_destacado", SqlDbType.VarChar).Value = nuevo.personal_destacado;
                    cmd.Parameters.Add("@calzado", SqlDbType.VarChar).Value = nuevo.numero_calzado;

                    cmd.Parameters.Add("@ante_conducir", SqlDbType.VarChar).Value = nuevo.antecedentes_conducir;

                   
                    cmd.CommandType = CommandType.Text;
                    cmd.ExecuteNonQuery();


                }
                catch (Exception ex)
                {
                    Console.Write(ex);
                }
                cnx.Close();
            }
            else
            {
                SqlConnection cnx = conexion.crearConexion();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = cnx;
          
                    cmd.CommandText = "insert into info_trabajador_adicional (rut_trabajador,estado_civil,talla_ropa,grupo_sanguineo,alergico,personal_destacado,calzado,ante_conducir) values(@rut_trabajador,@estado_civil,@talla_ropa,@grupo_sanguineo,@alergico,@personal_destacado,@calzado,@ante_conducir)";
                    cmd.Parameters.Add("@rut_trabajador", SqlDbType.VarChar).Value = nuevo.rut;
                    cmd.Parameters.Add("@estado_civil", SqlDbType.VarChar).Value = nuevo.estado_civil;
                    cmd.Parameters.Add("@talla_ropa", SqlDbType.VarChar).Value = nuevo.talla_ropa;
                    cmd.Parameters.Add("@grupo_sanguineo", SqlDbType.VarChar).Value = nuevo.grupo_sangre;
                    cmd.Parameters.Add("@alergico", SqlDbType.VarChar).Value = nuevo.alergico;
                    cmd.Parameters.Add("@personal_destacado", SqlDbType.VarChar).Value = nuevo.personal_destacado;
                    cmd.Parameters.Add("@calzado", SqlDbType.VarChar).Value = nuevo.numero_calzado;

                    cmd.Parameters.Add("@ante_conducir", SqlDbType.VarChar).Value = nuevo.antecedentes_conducir;

                 
                    cmd.CommandType = CommandType.Text;
                    cmd.ExecuteNonQuery();



                cnx.Close();
            }
        }
        public static infoadicional obtenerComplementariosTrabajador(string rut)
        {
            infoadicional Trabajador = new infoadicional();
            SqlConnection cnx = conexion.crearConexion();
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = cnx;
            cmd.CommandText = "SELECT * from info_trabajador_adicional WHERE rut_trabajador='" + rut + "'";
            cmd.CommandType = CommandType.Text;
            SqlDataReader dr = cmd.ExecuteReader();

            while (dr.Read())
            {
                
                Trabajador.rut = (string)dr["rut_trabajador"];
                Trabajador.estado_civil = (string)dr["estado_civil"];
                Trabajador.talla_ropa = (string)dr["talla_ropa"];
                Trabajador.grupo_sangre = (string)dr["grupo_sanguineo"];
                Trabajador.alergico = (string)dr["alergico"];
                Trabajador.personal_destacado = (string)dr["personal_destacado"];
                Trabajador.numero_calzado = (string)dr["calzado"];
                Trabajador.antecedentes_conducir = (string)dr["ante_conducir"];
              
            }
            cnx.Close();
            return Trabajador;
        }
        public static bool existenDatos(string rut)
        {
            bool retorno = false;

            SqlConnection cnx = conexion.crearConexion();

            SqlCommand cmd = new SqlCommand();
            cmd.Connection = cnx;
            cmd.CommandText = "SELECT * from info_trabajador_adicional WHERE rut_trabajador='" + rut + "'";
            cmd.CommandType = CommandType.Text;
            SqlDataReader dr = cmd.ExecuteReader();

            retorno = dr.HasRows;

            cnx.Close();

            return retorno;
        }
        
    }
    
}