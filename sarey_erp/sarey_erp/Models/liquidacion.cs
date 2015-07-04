using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace sarey_erp.Models
{
   

    public class liquidacion
    {
        public string rut { get; set; }
        public string anio { set; get; }
        public string mes { set; get; }
        public int sueldo_base { set; get; }
        public string obra { set; get; }
        public int bono_produccion { set; get; }
        public int bono_responsabilidad { set; get; }
        public int gratificacion { set; get; }
        public int sueldo_imponible{set;get;}
        public int sueldo_liquido { set; get; }
        public int sueldo_bruto { set; get; }
        public int asignacion_familiar { set; get; }
        public int bono_colacion { set; get; }
        public int bono_movilizacion { set; get; }
        public int viatico { set; get; }
        public int desgaste_herramientas{set;get;}
        public int cantidad_horas_semanales{set;get;}
        public double cantidad_horas_extras{set;get;}
        public int cantidad_dias_notrabajados{set;get;}                            
        public int cantidad_dias_licencia{set;get;}    
        public int total_haberes{set;get;}    
        public int salud{set;get;}
        public int apv{set;get;}
        public int afp{set;get; }    
        public int afc{set;get;}
        public int afc_cargo_empleador { set; get; }
        public int afp_sis_cargo_empleador { set; get; }
        public int suple { set; get; }
        public int prestamo { set; get; }
        public int adelanto { set; get; }
        public int dias_licencia { set; get; }
        public int descuento_licencia { set; get; }
        public int descuento_noTrabajo { set; get; }


        double porcentaje_descuento_afp;
        double porcentaje_descuento_salud=7;
        double porcentaje_descuento_afc;
        double porcentaje_descuento_apv;
        double valor_hora_extra;


        double porcentaje_descuento_afp_sis = 0;
        int numero_cargas = 0;
        double porcentaje_descuento_afc_empleador = 0;
        double porcentaje_descuento_afc_trabajador = 0;
        double descuento_isapre_uf = 0;

        int anio_actual;
        int mes_actual;
        
        DateTime fechaInicio;
        DateTime fechaFinal;

        public liquidacion(int anio, int mes){

            //fecha_actual = DateTime.Today;
            anio_actual = anio;
            mes_actual = mes;            
            fechaInicio = new DateTime(anio_actual, mes_actual, 01);
            fechaFinal = new DateTime(anio_actual, mes_actual, 30);
            
        }
        public liquidacion() { 
        
        }
        
        public liquidacion generarLiquidacion(){                                    

            datosPagoTrabajador datosPago = datosPagoTrabajador.obtenerDatosPago(rut);

            trabajador Trabajador = new trabajador();
            Trabajador.rut = rut;
            Trabajador = Trabajador.obtenerTrabajador();
           
            porcentaje_descuento_apv = Trabajador.porcentajeAPV;            

            sueldo_base = datosPago.sueldoBase;
            obra=Trabajador.obra;
            bono_produccion=datosPago.bonoProduccion;
            bono_responsabilidad=datosPago.bonoResponsabilidad;
            gratificacion=datosPago.gratificacion;           

            bono_colacion=datosPago.bonoColacion;
            bono_movilizacion=datosPago.bonoMovilizacion;
            viatico=datosPago.viatico;
            desgaste_herramientas=datosPago.desgasteHerramientas;

            //CALCULO VALOR HORA
            double valor_dia = datosPago.sueldoBase / 30;
            double valor_hora = valor_dia * 7;
            valor_hora = valor_hora / datosPago.cantidadHorasSemanales;
            valor_hora_extra = valor_hora * 1.5;

           // cantidad_horas_extras = horasExtras.obtenerHorasExtrasMes(rut,mes_actual,anio_actual);
            sueldo_imponible = datosPago.sueldoBase + datosPago.gratificacion + datosPago.bonoProduccion + datosPago.bonoResponsabilidad;//+ Convert.ToInt32(cantidad_horas_extras*valor_hora_extra);
            
            sueldo_bruto = sueldo_imponible + asignacion_familiar + bono_colacion + bono_movilizacion + viatico + desgaste_herramientas;

            //ASIGNACION FAMILIAR   
            List<cargafamiliar> cargas = Models.cargafamiliar.obtenerCargas(rut);
            numero_cargas=cargas.Count;

            asignacion_familiar = numero_cargas*Convert.ToInt32(asignacionFamiliar.obtenerAsignacionSegunMontoImponible(sueldo_imponible));
                       
            //AFP
            Models.afp afp_trabajador = Models.afp.obtenerAFP(Trabajador.afp);
            porcentaje_descuento_afp_sis=afp_trabajador.sis;
            porcentaje_descuento_afp=afp_trabajador.tasa;
            


            //SALUD
            if (!Trabajador.salud.Equals("Fonasa"))
            {
                
                //OBTENER PLAN ISAPRE
                double UFIsapre = Trabajador.porcentajeSalud;
                
                //VALOR UF
                double valorUF = uf.obtenerUF(mes_actual,anio_actual).valor;



                if (UFIsapre != 0)
                {
                    porcentaje_descuento_salud = 0;
                    
                    descuento_isapre_uf = UFIsapre*valorUF;
                }
                
            }
            


            //AFC
            if (Trabajador.tipo_regimen.Equals("Antiguo"))
            {
                porcentaje_descuento_afc = Trabajador.porcentajeAFC;
            }
            else {
                //SEGURO CESANTIA
                if (Trabajador.tipo_contrato.Equals("Fijo"))
                {//OBTENER PORCENTAJE AFC 
                    seguroCesantia SeguroCesantia= seguroCesantia.obtenerPlazoFijo();
                    porcentaje_descuento_afc_trabajador = SeguroCesantia.trabajador;
                    porcentaje_descuento_afc_empleador = SeguroCesantia.empleador;
                    porcentaje_descuento_afc = porcentaje_descuento_afc_trabajador;

                }else{
                    seguroCesantia SeguroCesantia = seguroCesantia.obtenerPlazoIndefinido();
                    porcentaje_descuento_afc_trabajador= SeguroCesantia.trabajador;
                    porcentaje_descuento_afc_empleador = SeguroCesantia.empleador;
                    porcentaje_descuento_afc = porcentaje_descuento_afc_trabajador;
                }
            
            }




            //CALCULOS                                                
            if (porcentaje_descuento_apv == 0)
            {
                apv=0;
            }
            else
            {
                apv= Convert.ToInt32(sueldo_imponible * porcentaje_descuento_apv)/100;
            }



            if (porcentaje_descuento_afp == 0)
            {
                afp = 0;
            }
            else {
                afp = Convert.ToInt32(sueldo_imponible * porcentaje_descuento_afp)/100;
                afp_sis_cargo_empleador = Convert.ToInt32(sueldo_imponible * porcentaje_descuento_afp_sis) / 100;
            
            }



            if (porcentaje_descuento_salud == 0) {

                salud = Convert.ToInt32(descuento_isapre_uf);                
                //salud = 0;
            }else{
                salud = Convert.ToInt32(sueldo_imponible * porcentaje_descuento_salud)/100;
            }


            
            if (porcentaje_descuento_afc == 0)
            {
                afc = 0;
                afc_cargo_empleador = Convert.ToInt32(sueldo_imponible * porcentaje_descuento_afc_empleador) / 100;
            }
            else {
                afc = Convert.ToInt32(sueldo_imponible * porcentaje_descuento_afc)/100;
                afc_cargo_empleador = Convert.ToInt32(sueldo_imponible * porcentaje_descuento_afc_empleador) / 100;
            }



                             
            double descuentos=afc + afp + apv + salud;
            double total_despues_descuentos = sueldo_imponible - descuentos;
            sueldo_liquido=Convert.ToInt32(total_despues_descuentos+ asignacion_familiar + datosPago.bonoColacion + datosPago.bonoMovilizacion +datosPago.viatico +datosPago.desgasteHerramientas);                      
            
            DateTime fechaInicio = new DateTime(anio_actual,mes_actual,1);
            DateTime fechaFinal = new DateTime(anio_actual, mes_actual, 30);
            cantidad_dias_notrabajados = diasNoTrabajados.cantidadDiasNoTrabajados(rut, fechaInicio, fechaFinal);
            descuento_noTrabajo = Convert.ToInt32(cantidad_dias_notrabajados * valor_dia);
            
            dias_licencia = licenciasTrabajadores.cantidadDiasLicencia(rut, fechaInicio, fechaFinal);           
            descuento_licencia = Convert.ToInt32(dias_licencia * valor_dia);
            sueldo_liquido = sueldo_liquido - descuento_licencia - descuento_noTrabajo;
             
            return this;
            
        }



        public void guardarLiquidacion() {
            try
            {
                SqlConnection cnx = conexion.crearConexion();

                SqlCommand cmd = new SqlCommand();
                cmd.Connection = cnx;

                cmd.CommandText = "INSERT INTO liquidaciones VALUES(@rut,@anio,@mes,@sueldo_base,@obra,@bono_produccion,@bono_responsabilidad,"
                    + "@gratificacion,@sueldo_imponible,@sueldo_liquido,@sueldo_bruto,@asignacion_familiar,@bono_colacion," +
                    "@bono_movilizacion,@viatico,@desgaste_herramientas,@cantidad_horas_semanales,@cantidad_horas_extras,@cantidad_dias_notrabajados," +
                    "@cantidad_dias_licencia,@total_haberes,@salud,@apv,@afp,@afc,@afc_cargo_empleador,@afp_sis_cargo_empleador," +
                    "@suple,@prestamo,@adelanto,@dias_licencia,@descuento_licencia,@descuento_noTrabajo)";

                cmd.Parameters.Add("@rut", SqlDbType.VarChar).Value = rut;
                cmd.Parameters.Add("@anio", SqlDbType.VarChar).Value = Convert.ToString(anio_actual);
                cmd.Parameters.Add("@mes", SqlDbType.VarChar).Value = Convert.ToString(mes_actual);
                cmd.Parameters.Add("@sueldo_base", SqlDbType.Int).Value = sueldo_base;
                cmd.Parameters.Add("@obra", SqlDbType.VarChar).Value = obra;
                cmd.Parameters.Add("@bono_produccion", SqlDbType.Int).Value = bono_produccion;
                cmd.Parameters.Add("@bono_responsabilidad", SqlDbType.Int).Value = bono_responsabilidad;
                cmd.Parameters.Add("@gratificacion", SqlDbType.Int).Value = gratificacion;
                cmd.Parameters.Add("@sueldo_imponible", SqlDbType.Int).Value = sueldo_imponible;
                cmd.Parameters.Add("@sueldo_liquido", SqlDbType.Int).Value = sueldo_liquido;
                cmd.Parameters.Add("@sueldo_bruto", SqlDbType.Int).Value = sueldo_bruto;
                cmd.Parameters.Add("@asignacion_familiar", SqlDbType.Int).Value = asignacion_familiar;
                cmd.Parameters.Add("@bono_colacion", SqlDbType.Int).Value = bono_colacion;
                cmd.Parameters.Add("@bono_movilizacion", SqlDbType.Int).Value = bono_movilizacion;
                cmd.Parameters.Add("@viatico", SqlDbType.Int).Value = viatico;
                cmd.Parameters.Add("@desgaste_herramientas", SqlDbType.Int).Value = desgaste_herramientas;
                cmd.Parameters.Add("@cantidad_horas_semanales", SqlDbType.Int).Value = cantidad_horas_semanales;
                cmd.Parameters.Add("@cantidad_horas_extras", SqlDbType.Int).Value = cantidad_horas_extras;
                cmd.Parameters.Add("@cantidad_dias_notrabajados", SqlDbType.Int).Value = cantidad_dias_notrabajados;
                cmd.Parameters.Add("@cantidad_dias_licencia", SqlDbType.Int).Value = cantidad_dias_licencia;
                cmd.Parameters.Add("@total_haberes", SqlDbType.Int).Value = total_haberes;
                cmd.Parameters.Add("@salud", SqlDbType.Int).Value = salud;
                cmd.Parameters.Add("@apv", SqlDbType.Int).Value = apv;
                cmd.Parameters.Add("@afp", SqlDbType.Int).Value = afp;
                cmd.Parameters.Add("@afc", SqlDbType.Int).Value = afc;
                cmd.Parameters.Add("@afc_cargo_empleador", SqlDbType.Int).Value = afc_cargo_empleador;
                cmd.Parameters.Add("@afp_sis_cargo_empleador", SqlDbType.Int).Value = afp_sis_cargo_empleador;
                cmd.Parameters.Add("@suple", SqlDbType.Int).Value = suple;
                cmd.Parameters.Add("@prestamo", SqlDbType.Int).Value = prestamo;
                cmd.Parameters.Add("@adelanto", SqlDbType.Int).Value = adelanto;
                cmd.Parameters.Add("@dias_licencia", SqlDbType.Int).Value = dias_licencia;
                cmd.Parameters.Add("@descuento_licencia", SqlDbType.Int).Value = descuento_licencia;
                cmd.Parameters.Add("@descuento_noTrabajo", SqlDbType.Int).Value = descuento_noTrabajo;

                cmd.CommandType = CommandType.Text;
                cmd.ExecuteNonQuery();


                cnx.Close();
            }
            catch (Exception ex) {

                Console.Write(ex);
            }
        
        }


        public void borrarLiquidacion() {


            SqlConnection cnx = conexion.crearConexion();

            SqlCommand cmd = new SqlCommand();
            cmd.Connection = cnx;
            cmd.CommandText = "DELETE FROM liquidaciones WHERE anio=@anio AND mes=@mes AND rut=@rut";
             
            cmd.Parameters.Add("@anio", SqlDbType.VarChar).Value = Convert.ToString(anio_actual);
            cmd.Parameters.Add("@mes", SqlDbType.VarChar).Value = Convert.ToString(mes_actual);
            cmd.Parameters.Add("@rut", SqlDbType.VarChar).Value = rut;
            

            cmd.CommandType = CommandType.Text;
            cmd.ExecuteNonQuery();

            cnx.Close();
            
        
        }


        public static List<liquidacion> obtenerTodasLiquidacionesPorFecha(int mes, int anio) {
            List<liquidacion> liquidaciones = new List<liquidacion>();
            try
            {
                

                SqlConnection cnx = conexion.crearConexion();

                SqlCommand cmd = new SqlCommand();
                cmd.Connection = cnx;
                cmd.CommandText = "SELECT * from liquidaciones WHERE anio=@anio AND mes=@mes";
                cmd.Parameters.Add("@anio", SqlDbType.VarChar).Value = Convert.ToString(anio);
                cmd.Parameters.Add("@mes", SqlDbType.VarChar).Value = Convert.ToString(mes);
                cmd.CommandType = CommandType.Text;
                SqlDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {

                    liquidacion Liquidacion = new liquidacion();

                    Liquidacion.rut = (string)dr["rut"];
                    Liquidacion.anio = (string)dr["anio"];
                    Liquidacion.mes = (string)dr["mes"];
                    Liquidacion.obra = (string)dr["obra"];
                    Liquidacion.sueldo_base = (int)dr["sueldo_base"];
                    Liquidacion.bono_produccion = (int)dr["bono_produccion"];
                    Liquidacion.bono_responsabilidad = (int)dr["bono_responsabilidad"];
                    Liquidacion.gratificacion = (int)dr["gratificacion"];
                    Liquidacion.sueldo_imponible = (int)dr["sueldo_imponible"];
                    Liquidacion.sueldo_liquido = (int)dr["sueldo_liquido"];
                    Liquidacion.sueldo_bruto = (int)dr["sueldo_bruto"];
                    Liquidacion.asignacion_familiar = (int)dr["asignacion_familiar"];
                    Liquidacion.bono_colacion = (int)dr["bono_colacion"];
                    Liquidacion.bono_colacion = (int)dr["bono_colacion"];
                    Liquidacion.viatico = (int)dr["viatico"];
                    Liquidacion.desgaste_herramientas = (int)dr["desgaste_herramientas"];
                    Liquidacion.cantidad_horas_semanales = (int)dr["cantidad_horas_semanales"];
                    Liquidacion.cantidad_horas_extras = (int)dr["cantidad_horas_extras"];
                    Liquidacion.cantidad_dias_notrabajados = (int)dr["cantidad_dias_notrabajados"];
                    Liquidacion.cantidad_dias_licencia = (int)dr["cantidad_dias_licencia"];
                    Liquidacion.total_haberes = (int)dr["total_haberes"];
                    Liquidacion.salud = (int)dr["salud"];
                    Liquidacion.apv = (int)dr["apv"];
                    Liquidacion.afc = (int)dr["afc"];
                    Liquidacion.afp = (int)dr["afp"];
                    Liquidacion.afc_cargo_empleador = (int)dr["afc_cargo_empleador"];
                    Liquidacion.afp_sis_cargo_empleador = (int)dr["afp_sis_cargo_empleador"];
                    Liquidacion.suple = (int)dr["suple"];
                    Liquidacion.prestamo = (int)dr["prestamo"];
                    Liquidacion.adelanto = (int)dr["adelanto"];
                    Liquidacion.dias_licencia = (int)dr["dias_licencia"];
                    Liquidacion.descuento_licencia = (int)dr["descuentos_licencia"];
                    Liquidacion.descuento_noTrabajo = (int)dr["descuento_noTrabajo"];
                    liquidaciones.Add(Liquidacion);

                }

                cnx.Close();
            }
            catch (Exception ex)
            {
                Console.Write(ex);
            }

            return liquidaciones;
            
        }




        public static liquidacion obtenerLiquidacionPorFecha(string rut,int mes, int anio)
        {
            liquidacion Liquidacion = new liquidacion();
            try
            {

                SqlConnection cnx = conexion.crearConexion();

                SqlCommand cmd = new SqlCommand();
                cmd.Connection = cnx;
                cmd.CommandText = "SELECT * from liquidaciones WHERE anio=@anio AND mes=@mes AND rut=@rut";
                cmd.Parameters.Add("@anio", SqlDbType.VarChar).Value = Convert.ToString(anio);
                cmd.Parameters.Add("@mes", SqlDbType.VarChar).Value = Convert.ToString(mes);
                cmd.Parameters.Add("@rut", SqlDbType.VarChar).Value = rut;
                cmd.CommandType = CommandType.Text;
                SqlDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {

                    

                    Liquidacion.rut = (string)dr["rut"];
                    Liquidacion.anio = (string)dr["anio"];
                    Liquidacion.mes = (string)dr["mes"];
                    Liquidacion.obra = (string)dr["obra"];
                    Liquidacion.sueldo_base = (int)dr["sueldo_base"];
                    Liquidacion.bono_produccion = (int)dr["bono_produccion"];
                    Liquidacion.bono_responsabilidad = (int)dr["bono_responsabilidad"];
                    Liquidacion.gratificacion = (int)dr["gratificacion"];
                    Liquidacion.sueldo_imponible = (int)dr["sueldo_imponible"];
                    Liquidacion.sueldo_liquido = (int)dr["sueldo_liquido"];
                    Liquidacion.sueldo_bruto = (int)dr["sueldo_bruto"];
                    Liquidacion.asignacion_familiar = (int)dr["asignacion_familiar"];
                    Liquidacion.bono_colacion = (int)dr["bono_colacion"];
                    Liquidacion.bono_colacion = (int)dr["bono_colacion"];
                    Liquidacion.viatico = (int)dr["viatico"];
                    Liquidacion.desgaste_herramientas = (int)dr["desgaste_herramientas"];
                    Liquidacion.cantidad_horas_semanales = (int)dr["cantidad_horas_semanales"];
                    Liquidacion.cantidad_horas_extras = (int)dr["cantidad_horas_extras"];
                    Liquidacion.cantidad_dias_notrabajados = (int)dr["cantidad_dias_notrabajados"];
                    Liquidacion.cantidad_dias_licencia = (int)dr["cantidad_dias_licencia"];
                    Liquidacion.total_haberes = (int)dr["total_haberes"];
                    Liquidacion.salud = (int)dr["salud"];
                    Liquidacion.apv = (int)dr["apv"];
                    Liquidacion.afc = (int)dr["afc"];
                    Liquidacion.afp = (int)dr["afp"];
                    Liquidacion.afc_cargo_empleador = (int)dr["afc_cargo_empleador"];
                    Liquidacion.afp_sis_cargo_empleador = (int)dr["afp_sis_cargo_empleador"];
                    Liquidacion.suple = (int)dr["suple"];
                    Liquidacion.prestamo = (int)dr["prestamo"];
                    Liquidacion.adelanto = (int)dr["adelanto"];
                    Liquidacion.dias_licencia = (int)dr["dias_licencia"];
                    Liquidacion.descuento_licencia = (int)dr["descuentos_licencia"];
                    Liquidacion.descuento_noTrabajo = (int)dr["descuento_noTrabajo"];
                   

                }

                cnx.Close();
            }
            catch (Exception ex)
            {
                Console.Write(ex);
            }

            return Liquidacion;

        }

        public static List<liquidacion> obtenerTodasLiquidacionesTrabajador(string rut)
        {
            List<liquidacion> Liquidaciones = new List<liquidacion>();
            try
            {

                SqlConnection cnx = conexion.crearConexion();

                SqlCommand cmd = new SqlCommand();
                cmd.Connection = cnx;
                cmd.CommandText = "SELECT * from liquidaciones WHERE rut=@rut";               
                cmd.Parameters.Add("@rut", SqlDbType.VarChar).Value = rut;
                cmd.CommandType = CommandType.Text;
                SqlDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {


                    liquidacion Liquidacion = new liquidacion();
                    Liquidacion.rut = (string)dr["rut"];
                    Liquidacion.anio = (string)dr["anio"];
                    Liquidacion.mes = (string)dr["mes"];
                    Liquidacion.obra = (string)dr["obra"];
                    Liquidacion.sueldo_base = (int)dr["sueldo_base"];
                    Liquidacion.bono_produccion = (int)dr["bono_produccion"];
                    Liquidacion.bono_responsabilidad = (int)dr["bono_responsabilidad"];
                    Liquidacion.gratificacion = (int)dr["gratificacion"];
                    Liquidacion.sueldo_imponible = (int)dr["sueldo_imponible"];
                    Liquidacion.sueldo_liquido = (int)dr["sueldo_liquido"];
                    Liquidacion.sueldo_bruto = (int)dr["sueldo_bruto"];
                    Liquidacion.asignacion_familiar = (int)dr["asignacion_familiar"];
                    Liquidacion.bono_colacion = (int)dr["bono_colacion"];
                    Liquidacion.bono_colacion = (int)dr["bono_colacion"];
                    Liquidacion.viatico = (int)dr["viatico"];
                    Liquidacion.desgaste_herramientas = (int)dr["desgaste_herramientas"];
                    Liquidacion.cantidad_horas_semanales = (int)dr["cantidad_horas_semanales"];
                    Liquidacion.cantidad_horas_extras = (int)dr["cantidad_horas_extras"];
                    Liquidacion.cantidad_dias_notrabajados = (int)dr["cantidad_dias_notrabajados"];
                    Liquidacion.cantidad_dias_licencia = (int)dr["cantidad_dias_licencia"];
                    Liquidacion.total_haberes = (int)dr["total_haberes"];
                    Liquidacion.salud = (int)dr["salud"];
                    Liquidacion.apv = (int)dr["apv"];
                    Liquidacion.afc = (int)dr["afc"];
                    Liquidacion.afp = (int)dr["afp"];
                    Liquidacion.afc_cargo_empleador = (int)dr["afc_cargo_empleador"];
                    Liquidacion.afp_sis_cargo_empleador = (int)dr["afp_sis_cargo_empleador"];
                    Liquidacion.suple = (int)dr["suple"];
                    Liquidacion.prestamo = (int)dr["prestamo"];
                    Liquidacion.adelanto = (int)dr["adelanto"];
                    Liquidacion.dias_licencia = (int)dr["dias_licencia"];
                    Liquidacion.descuento_licencia = (int)dr["descuentos_licencia"];
                    Liquidacion.descuento_noTrabajo = (int)dr["descuento_noTrabajo"];
                    Liquidaciones.Add(Liquidacion);

                }

                cnx.Close();
            }
            catch (Exception ex)
            {
                Console.Write(ex);
            }

            return Liquidaciones;

        }


    }
}