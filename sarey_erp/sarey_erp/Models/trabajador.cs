using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;

namespace sarey_erp.Models
{
    public class trabajador
    {
        public string rut { set; get; }
        public string nombres { set; get; }
        public string apellidos { get; set; }
        public string obra { get; set; }
        public DateTime fechaIngresoSistema { set; get; }
        public DateTime fechaIngresoEmpresa { set; get; }
        public string direccion { set; get; }
        public string telefono { set; get; }
        public string cargo { set; get; }
        public string salud { set; get; }
        public string afp { set; get; }
        public double porcentajeAPV { set; get; }
        public double porcentajeAFP { set; get; }
        public double porcentajeAFC { set; get; }
        public double porcentajeSalud { set; get; }
        public string tipo_regimen { set; get; }
        public string tipo_contrato { set; get; }     
       
        public void guardarDatosPersonales()
        {
            try
            {
                SqlConnection cnx = conexion.crearConexion();
                SqlCommand cmd_ = new SqlCommand();
                cmd_.Connection = cnx;
                cmd_.CommandText = "UPDATE trabajador SET direccion=@direccion, telefono=@telefono,cargo=@cargo,obra=@obra,afp=@afp,salud=@salud,porcentajeAPV=@porcentajeAPV,porcentajeAFC=@porcentajeAFC,porcentajeSalud=@porcentajeSalud,porcentajeAFP=@porcentajeAFP,tipo_contrato=@tipo_contrato WHERE rut='" + rut + "'";

                cmd_.Parameters.Add("@direccion", SqlDbType.VarChar).Value = direccion;
                cmd_.Parameters.Add("@telefono", SqlDbType.VarChar).Value = telefono;
                cmd_.Parameters.Add("@cargo", SqlDbType.VarChar).Value = cargo;
                cmd_.Parameters.Add("@obra", SqlDbType.VarChar).Value = obra;
                cmd_.Parameters.Add("@afp", SqlDbType.VarChar).Value = afp;
                cmd_.Parameters.Add("@salud", SqlDbType.VarChar).Value = salud;
                cmd_.Parameters.Add("@porcentajeAPV", SqlDbType.Float).Value = porcentajeAPV;
                cmd_.Parameters.Add("@porcentajeAFC", SqlDbType.Float).Value = porcentajeAFC;
                cmd_.Parameters.Add("@porcentajeSalud", SqlDbType.Float).Value = porcentajeSalud;
                
                cmd_.Parameters.Add("@porcentajeAFP", SqlDbType.Float).Value = porcentajeAFP;
                cmd_.Parameters.Add("@tipo_contrato", SqlDbType.VarChar).Value = tipo_contrato;

                cmd_.CommandType = CommandType.Text;
                cmd_.ExecuteNonQuery();
            }
            catch (Exception ex) {
                Console.Write(ex);
            }
        }
        
        public trabajador obtenerTrabajador() {
            trabajador Trabajador = new trabajador();
            SqlConnection cnx = conexion.crearConexion();
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = cnx;
            cmd.CommandText = "SELECT * from trabajador WHERE rut='" + rut + "'";
            cmd.CommandType = CommandType.Text;
            SqlDataReader dr = cmd.ExecuteReader();

            while (dr.Read())
            {
                
                Trabajador.rut = (string)dr["rut"];
                Trabajador.nombres = (string)dr["nombres"];
                Trabajador.apellidos = (string)dr["apellidos"];
                Trabajador.obra = (string)dr["obra"];
                Trabajador.cargo = (string)dr["cargo"];
                Trabajador.direccion = (string)dr["direccion"];
                Trabajador.telefono = (string)dr["telefono"];
                Trabajador.salud = (string)dr["salud"];
                Trabajador.afp = (string)dr["afp"];
                Trabajador.fechaIngresoEmpresa = (DateTime)dr["fechaIngresoEmpresa"];
                Trabajador.fechaIngresoSistema = (DateTime)dr["fechaIngresoSistema"];
                Trabajador.tipo_contrato = (string)dr["tipo_contrato"];
                Trabajador.tipo_regimen = (string)dr["tipo_regimen"];
                
                try
                {
                    Trabajador.porcentajeAFP = (double)dr["porcentajeAFP"];
                }
                catch (Exception) {
                    Trabajador.porcentajeAFP = 0;
                }

                try
                {
                    Trabajador.porcentajeAPV = (double)dr["porcentajeAPV"];
                }
                catch (Exception) {
                    Trabajador.porcentajeAPV = 0;
                }

                try
                {
                    Trabajador.porcentajeSalud = (double)dr["porcentajeSalud"];
                }
                catch (Exception) {
                    Trabajador.porcentajeSalud = 0;
                }
                try
                {
                    Trabajador.porcentajeAFC = (double)dr["porcentajeAFC"];
                }
                catch (Exception)
                {
                    Trabajador.porcentajeAFC = 0;
                }

            }
            cnx.Close();
            return Trabajador;
        }

        public static List<trabajador> obtenerTodos() {

            List<trabajador> listaTrabajadores = new List<trabajador>();
            SqlConnection cnx = conexion.crearConexion();
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = cnx;
            cmd.CommandText = "SELECT * from trabajador ORDER BY apellidos ASC";
            cmd.CommandType = CommandType.Text;
            SqlDataReader dr = cmd.ExecuteReader();

            while (dr.Read())
            {
                trabajador Trabajador = new trabajador();

                Trabajador.rut = (string)dr["rut"];
                Trabajador.nombres = (string)dr["nombres"];
                Trabajador.apellidos = (string)dr["apellidos"];
                Trabajador.obra = (string)dr["obra"];
                Trabajador.cargo = (string)dr["cargo"];
                Trabajador.direccion = (string)dr["direccion"];
                Trabajador.telefono = (string)dr["telefono"];
                Trabajador.salud = (string)dr["salud"];
                Trabajador.afp = (string)dr["afp"];
                Trabajador.fechaIngresoEmpresa = (DateTime)dr["fechaIngresoSistema"];
                Trabajador.fechaIngresoSistema = (DateTime)dr["fechaIngresoEmpresa"];
                Trabajador.tipo_contrato = (string)dr["tipo_contrato"];
                Trabajador.tipo_regimen = (string)dr["tipo_regimen"];

                try
                {
                    Trabajador.porcentajeAFP = (double)dr["porcentajeAFP"];
                }
                catch (Exception)
                {
                    Trabajador.porcentajeAFP = 0;
                }

                try
                {
                    Trabajador.porcentajeAPV = (double)dr["porcentajeAPV"];
                }
                catch (Exception)
                {
                    Trabajador.porcentajeAPV = 0;
                }

                try
                {
                    Trabajador.porcentajeSalud = (double)dr["porcentajeSalud"];
                }
                catch (Exception)
                {
                    Trabajador.porcentajeSalud = 0;
                }
                try
                {
                    Trabajador.porcentajeAFC = (double)dr["porcentajeAFC"];
                }
                catch (Exception)
                {
                    Trabajador.porcentajeAFC = 0;
                }

                if (!Trabajador.obra.Equals(""))
                {
                    listaTrabajadores.Add(Trabajador);
                }
            }
            cnx.Close();

            return listaTrabajadores;
        }

        public static bool existeTrabajador(string rut)
        {

            bool retorno = false;

            SqlConnection cnx = conexion.crearConexion();
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = cnx;
            cmd.CommandText = "SELECT * FROM trabajador WHERE rut='" + rut + "'";
            cmd.CommandType = CommandType.Text;

            SqlDataReader dr = cmd.ExecuteReader();

            retorno = dr.HasRows;

            cnx.Close();

            return retorno;

        } 

        public void guardarTrabajador() { 
            
            SqlConnection cnx = conexion.crearConexion();
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = cnx;
            try
            {
                cmd.CommandText = "insert into trabajador(rut,nombres,apellidos,direccion,telefono,salud,afp,porcentajeAPV,porcentajeAFP,porcentajeAFC,porcentajeSalud,cargo,fechaIngresoSistema,fechaIngresoEmpresa,obra,tipo_contrato,tipo_regimen) values(@rut,@nombres,@apellidos,@direccion,@telefono,@salud,@afp,@porcentajeAPV,@porcentajeAFP,@porcentajeAFC,@porcentajeSalud,@cargo,@fechaIngresoSistema,@fechaIngresoEmpresa,@obra,@tipo_contrato,@tipo_regimen)";
                cmd.Parameters.Add("@rut", SqlDbType.VarChar).Value = rut;
                cmd.Parameters.Add("@nombres", SqlDbType.VarChar).Value = nombres;
                cmd.Parameters.Add("@apellidos", SqlDbType.VarChar).Value = apellidos;
                cmd.Parameters.Add("@direccion", SqlDbType.VarChar).Value = direccion;
                cmd.Parameters.Add("@telefono", SqlDbType.VarChar).Value = telefono;
                cmd.Parameters.Add("@salud", SqlDbType.VarChar).Value = salud;
                cmd.Parameters.Add("@afp", SqlDbType.VarChar).Value = afp;
                
                cmd.Parameters.Add("@porcentajeAPV", SqlDbType.Float).Value = porcentajeAPV;              
              
                cmd.Parameters.Add("@porcentajeAFP", SqlDbType.Float).Value = porcentajeAFP;
                                
                cmd.Parameters.Add("@porcentajeSalud", SqlDbType.Float).Value = porcentajeSalud;
                cmd.Parameters.Add("@porcentajeAFC", SqlDbType.Float).Value = porcentajeAFC;

                                                               

                cmd.Parameters.Add("@cargo", SqlDbType.VarChar).Value = cargo;
                cmd.Parameters.Add("@fechaIngresoSistema", SqlDbType.DateTime).Value = fechaIngresoSistema;
                cmd.Parameters.Add("@fechaIngresoEmpresa", SqlDbType.DateTime).Value = fechaIngresoEmpresa;
                cmd.Parameters.Add("@obra", SqlDbType.VarChar).Value = obra;

                cmd.Parameters.Add("@tipo_regimen", SqlDbType.VarChar).Value = tipo_regimen;
                cmd.Parameters.Add("@tipo_contrato", SqlDbType.VarChar).Value = tipo_contrato;
                

                cmd.CommandType = CommandType.Text;
                cmd.ExecuteNonQuery();


            }
            catch (Exception ex)
            {
                Console.Write(ex);                 
            }
            cnx.Close();

        }

        public static void eliminarTrabajador(string rut)
        {

            SqlConnection cnx = conexion.crearConexion();
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = cnx;
            cmd.CommandText = "DELETE FROM trabajador  WHERE  ( rut = '" + rut + "')";
            cmd.CommandType = CommandType.Text;
            cmd.ExecuteNonQuery();
            cnx.Close();
        }






        public static DataSet obtenerTodosEXCEL()
        {

            List<trabajador> listaTrabajadores = new List<trabajador>();
            SqlConnection cnx = conexion.crearConexion();
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = cnx;
            cmd.CommandText = "SELECT * from trabajador ORDER BY apellidos ASC";
            cmd.CommandType = CommandType.Text;
            SqlDataReader dr = cmd.ExecuteReader();
            DataTable dt = new DataTable();
            DataSet ds = new DataSet();
            dt.Load(dr);
            ds.Tables.Add(dt);
           
            cnx.Close();

            return ds;
        }


    }
}