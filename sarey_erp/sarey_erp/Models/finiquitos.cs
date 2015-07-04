using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;

namespace sarey_erp.Models
{
    public class finiquitos
    {
        public string rut { set; get; }
        public string nombre_completo {set;get;}
        public DateTime fecha_ingreso_empresa { set; get; }
        public DateTime fecha_finiquito { set; get; }
        public string motivo { set; get; }
        public string detalle_motivo { set; get; }
        public int vacaciones_progresivas_totales { set; get; }
        public int vacaciones_consumidas { set; get; }
        public int vacaciones_anticipadas { set; get; }
        public int vacaciones_pendientes { set; get; }
        public int vacaciones_totales { set; get; }
        public int total_dias_vacaciones { set; get; }
        public int valor_dia { set; get; }
        public int total_finiquito { set; get; }


        private int total_dias_pagar;
        private double factor_mes = 1.25;
        private double factor_dia = 0.04167;

        public void generarFiniquito() {
            
            trabajador Trabajador = new trabajador();            
            Trabajador.rut = rut;
            Trabajador=Trabajador.obtenerTrabajador();
            nombre_completo = Trabajador.nombres + " " + Trabajador.apellidos;
            fecha_ingreso_empresa = Trabajador.fechaIngresoEmpresa;

            int dias_feriados = feriados.ObtenerdiasRango(fecha_ingreso_empresa,fecha_finiquito);
           
            int sueldo = 0;
            datosPagoTrabajador datosPago = datosPagoTrabajador.obtenerDatosPago(rut);
            sueldo = datosPago.sueldoBase;
            valor_dia = sueldo / 30;

            int meses_diferencia = fecha_finiquito.Month - fecha_ingreso_empresa.Month;
            int mes_finiquito = fecha_finiquito.Month;
            int anio_finiquito = fecha_finiquito.Year;
            int dias_diferencia = DateTime.DaysInMonth(anio_finiquito, mes_finiquito) - fecha_finiquito.Day;

            double feriado_proporcional = (meses_diferencia*factor_mes + dias_diferencia*factor_dia) + dias_feriados;

            total_finiquito = Convert.ToInt32(feriado_proporcional * valor_dia);
            
            
        }

        public void  guardarFiniquito() {
            SqlConnection cnx = conexion.crearConexion();

            SqlCommand cmd = new SqlCommand();
            cmd.Connection = cnx;
            cmd.CommandText = "INSERT INTO finiquitos VALUES(@rut,@fecha_finiquito,@nombre_completo,@fecha_ingreso_empresa," +
                "@motivo,@detalle_motivo,@vacaciones_progresivas_totales,@vacaciones_consumidas,@vacaciones_anticipadas,@vacaciones_pendientes,"+
                "@vacaciones_totales,@total_dias_vacaciones,@valor_dia,@total_finiquito)";
            cmd.CommandType = CommandType.Text;
            cmd.Parameters.Add("@rut", SqlDbType.VarChar).Value = rut;
            cmd.Parameters.Add("@nombre_completo", SqlDbType.VarChar).Value = nombre_completo;
            cmd.Parameters.Add("@fecha_ingreso_empresa", SqlDbType.DateTime).Value = fecha_ingreso_empresa;
            cmd.Parameters.Add("@fecha_finiquito", SqlDbType.DateTime).Value = fecha_finiquito;
            cmd.Parameters.Add("@motivo", SqlDbType.VarChar).Value = motivo;
            cmd.Parameters.Add("@detalle_motivo", SqlDbType.VarChar).Value = detalle_motivo;
            cmd.Parameters.Add("@vacaciones_progresivas_totales", SqlDbType.Int).Value = vacaciones_progresivas_totales;
            cmd.Parameters.Add("@vacaciones_consumidas", SqlDbType.Int).Value = vacaciones_consumidas;
            cmd.Parameters.Add("@vacaciones_anticipadas", SqlDbType.Int).Value = vacaciones_anticipadas;
            cmd.Parameters.Add("@vacaciones_pendientes", SqlDbType.Int).Value = vacaciones_pendientes;
            cmd.Parameters.Add("@vacaciones_totales", SqlDbType.Int).Value = vacaciones_totales;
            cmd.Parameters.Add("@total_dias_vacaciones", SqlDbType.Int).Value = total_dias_vacaciones;
            cmd.Parameters.Add("@valor_dia", SqlDbType.Int).Value = valor_dia;
            cmd.Parameters.Add("@total_finiquito", SqlDbType.Int).Value = total_finiquito;
           
            cmd.ExecuteNonQuery();
            cnx.Close();


            SqlConnection cnx_2 = conexion.crearConexion();

            SqlCommand cmd_2 = new SqlCommand();
            cmd_2.Connection = cnx_2;
            cmd_2.CommandText = "UPDATE trabajador "
                + " SET obra='" + null               
                + "' WHERE rut='" + rut + "'";
            cmd_2.CommandType = CommandType.Text;
            cmd_2.ExecuteNonQuery();

            cnx_2.Close();


        
        }

        public static List<finiquitos> obtenerFiniquitos()
        {
            List<finiquitos> ListaFiniquitos = new List<finiquitos>();            
            SqlConnection cnx = conexion.crearConexion();
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = cnx;
            cmd.CommandText = "SELECT * FROM finiquitos";

            cmd.CommandType = CommandType.Text;
            SqlDataReader dr = cmd.ExecuteReader();


            while (dr.Read())
            {
                finiquitos Finiquito = new finiquitos();
                Finiquito.motivo = (string)dr["motivo"];
                Finiquito.nombre_completo=(string)dr["nombre_completo"];
                Finiquito.rut = (string)dr["rut"];
                Finiquito.fecha_finiquito = (DateTime)dr["fecha_finiquito"];
                Finiquito.total_dias_vacaciones = (int)dr["total_dias_vacaciones"];
                Finiquito.total_finiquito = (int)dr["total_finiquito"];
                Finiquito.vacaciones_anticipadas = (int)dr["vacaciones_anticipadas"];
                Finiquito.vacaciones_consumidas = (int)dr["vacaciones_consumidas"];
                Finiquito.vacaciones_pendientes = (int)dr["vacaciones_pendientes"];
                Finiquito.vacaciones_progresivas_totales = (int)dr["vacaciones_progresivas_totales"];
                Finiquito.vacaciones_totales = (int)dr["vacaciones_totales"];
                Finiquito.valor_dia = (int)dr["valor_dia"];
                Finiquito.detalle_motivo = (string)dr["detalle_motivo"];
                
                ListaFiniquitos.Add(Finiquito);
            }
            cnx.Close();
            return ListaFiniquitos;
        }


        public static finiquitos obtenerFiniquitoTrabajador(string rut)
        {            
            SqlConnection cnx = conexion.crearConexion();
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = cnx;
            cmd.CommandText = "SELECT * FROM finiquitos WHERE rut=@rut";
            cmd.Parameters.Add("@rut", SqlDbType.VarChar).Value = rut;

            cmd.CommandType = CommandType.Text;
            SqlDataReader dr = cmd.ExecuteReader();

            finiquitos Finiquito = new finiquitos();

            while (dr.Read())
            {
                
                Finiquito.motivo = (string)dr["motivo"];
                Finiquito.nombre_completo = (string)dr["nombre_completo"];
                Finiquito.rut = (string)dr["rut"];
                Finiquito.fecha_finiquito = (DateTime)dr["fecha_finiquito"];
                Finiquito.total_dias_vacaciones = (int)dr["total_dias_vacaciones"];
                Finiquito.total_finiquito = (int)dr["total_finiquito"];
                Finiquito.vacaciones_anticipadas = (int)dr["vacaciones_anticipadas"];
                Finiquito.vacaciones_consumidas = (int)dr["vacaciones_consumidas"];
                Finiquito.vacaciones_pendientes = (int)dr["vacaciones_pendientes"];
                Finiquito.vacaciones_progresivas_totales = (int)dr["vacaciones_progresivas_totales"];
                Finiquito.vacaciones_totales = (int)dr["vacaciones_totales"];
                Finiquito.valor_dia = (int)dr["valor_dia"];
                Finiquito.detalle_motivo = (string)dr["detalle_motivo"];
               
            }
            cnx.Close();
            return Finiquito;
        }


    }
}