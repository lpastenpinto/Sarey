using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace sarey_erp.Models
{
    public class datosPagoTrabajador
    {
        public string rut { get; set; }
        public int sueldoBase { get; set; }
        public int gratificacion { get; set; }
        public int bonoProduccion { get; set; }
        public int bonoResponsabilidad { get; set; }
        public int asignacionFamiliar { get; set; }
        public int bonoColacion { get; set; }
        public int bonoMovilizacion { get; set; }
        public int viatico { get; set; }
        public int desgasteHerramientas { get; set; }
        public double cantidadHorasSemanales { get; set; }

        public static bool existenDatos(string rut) 
        {
            bool retorno = false;

            SqlConnection cnx = conexion.crearConexion();

            SqlCommand cmd = new SqlCommand();
            cmd.Connection = cnx;
            cmd.CommandText = "SELECT * from datos_pago_trabajador WHERE rut='" + rut + "'";
            cmd.CommandType = CommandType.Text;
            SqlDataReader dr = cmd.ExecuteReader();

            retorno = dr.HasRows;

            cnx.Close();
            
            return retorno;
        }

        public static datosPagoTrabajador obtenerDatosPago(string rut)
        {
            datosPagoTrabajador retorno = new datosPagoTrabajador();

            SqlConnection cnx = conexion.crearConexion();

            SqlCommand cmd = new SqlCommand();
            cmd.Connection = cnx;
            cmd.CommandText = "SELECT * from datos_pago_trabajador WHERE rut='" + rut + "'";
            cmd.CommandType = CommandType.Text;
            SqlDataReader dr = cmd.ExecuteReader();

            while (dr.Read()) 
            {
                retorno.rut = (string)dr["rut"];
                retorno.sueldoBase = (int)dr["sueldo_base"];
                retorno.gratificacion = (int)dr["gratificacion"];
                retorno.bonoProduccion = (int)dr["bono_produccion"];
                retorno.bonoResponsabilidad = (int)dr["bono_responsabilidad"];
                retorno.asignacionFamiliar = (int)dr["asignacion_familiar"];
                retorno.bonoColacion = (int)dr["bono_colacion"];
                retorno.bonoMovilizacion = (int)dr["bono_movilizacion"];
                retorno.viatico = (int)dr["viatico"];
                retorno.desgasteHerramientas = (int)dr["desgaste_herramientas"];
                retorno.cantidadHorasSemanales = double.Parse(dr["cantidad_horas_semanales"].ToString());
            }

            cnx.Close();

            return retorno;
        }

        public static void guardarDatos(datosPagoTrabajador trabajador)
        {
            if (existenDatos(trabajador.rut))
            {
                SqlConnection cnx = conexion.crearConexion();

                SqlCommand cmd = new SqlCommand();
                cmd.Connection = cnx;
                cmd.CommandText = "UPDATE datos_pago_trabajador "
                    + " SET sueldo_base='" + trabajador.sueldoBase
                    + "', gratificacion='" + trabajador.gratificacion
                    + "', bono_produccion='" + trabajador.bonoProduccion
                    + "', bono_responsabilidad='" + trabajador.bonoResponsabilidad
                    + "', asignacion_familiar='" + trabajador.asignacionFamiliar
                    + "', bono_colacion='" + trabajador.bonoColacion
                    + "', bono_movilizacion='" + trabajador.bonoMovilizacion
                    + "', viatico='" + trabajador.viatico
                    + "', desgaste_herramientas='" + trabajador.desgasteHerramientas
                    + "', cantidad_horas_semanales='" + trabajador.cantidadHorasSemanales
                    + "' WHERE rut='" + trabajador.rut + "'";
                cmd.CommandType = CommandType.Text;
                cmd.ExecuteNonQuery();

                cnx.Close();
            }
            else 
            {
                SqlConnection cnx = conexion.crearConexion();

                SqlCommand cmd = new SqlCommand();
                cmd.Connection = cnx;
                cmd.CommandText = "INSERT INTO datos_pago_trabajador VALUES('" 
                    + trabajador.rut + "','"
                    + trabajador.sueldoBase + "','"
                    + trabajador.gratificacion + "','"
                    + trabajador.bonoProduccion + "','"
                    + trabajador.bonoResponsabilidad + "','"
                    + trabajador.asignacionFamiliar + "','"
                    + trabajador.bonoColacion + "','"
                    + trabajador.bonoMovilizacion + "','"
                    + trabajador.viatico + "','"
                    + trabajador.desgasteHerramientas + "','"
                    + trabajador.cantidadHorasSemanales + "')";
                cmd.CommandType = CommandType.Text;
                
                cmd.ExecuteNonQuery();
                cnx.Close();
            }
        }

    }
}