using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using sarey_erp.Models;
using System.Globalization;

namespace sarey_erp.Controllers
{
    public class TrabajadorController : Controller
    {
        //
        // GET: /Trabajador/
        public static int mensaje=0;

        public ActionResult Index()
        {
            return RedirectToAction("todos");
        }

        public ActionResult verLiquidacion(string rutTrabajador) {

            DateTime fecha_actual = DateTime.Today;
            liquidacion LiquidacionTrabajador = new liquidacion(fecha_actual.Year,fecha_actual.Month);
           
            LiquidacionTrabajador.rut = rutTrabajador;

            LiquidacionTrabajador = LiquidacionTrabajador.generarLiquidacion();
            LiquidacionTrabajador.borrarLiquidacion();
            LiquidacionTrabajador.guardarLiquidacion();
            trabajador Trabajador = new trabajador();
            Trabajador.rut = rutTrabajador;
            Trabajador = Trabajador.obtenerTrabajador();

            ViewData["liquidacion"] = LiquidacionTrabajador;
            ViewData["trabajador"] = Trabajador;
            return View();
        }

        public ActionResult nuevo() {

            if (Session["rol"] != null && Session["rol"].Equals("admin") || Session["rol"].Equals("contabilidad"))
            {
                ViewData["listaIsapres"] = isapre.obtenerTodasIsapres();
                ViewData["listaAFP"] = afp.obtenerTodasAFP();
                ViewData["obras"] = faena.obtenerTodas();
                return View("nuevo");
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        public ActionResult todos() {

            if (Session["rol"] != null && (Session["rol"].Equals("admin") || Session["rol"].Equals("contabilidad")))
            {
                List<trabajador> trabajadores = new List<trabajador>();
                trabajadores = trabajador.obtenerTodos();                
                if (mensaje == 1)
                {
                    ViewBag.verificador = "si";
                }
                if (mensaje == 2)
                {
                    ViewBag.verificador = "no";
                }
                // ViewBag.verificador = mensaje;

                return View(trabajadores);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        public ActionResult verTrabajador(string rut) {
            if (Session["rol"] != null && (Session["rol"].Equals("admin") || Session["rol"].Equals("contabilidad")))
            {
                trabajador Trabajador = new trabajador();
                Trabajador.rut = rut;
                ViewData["listaIsapres"] = isapre.obtenerTodasIsapres();
                ViewData["listaAFP"] = afp.obtenerTodasAFP();
                ViewData["obras"] = faena.obtenerTodas();
                ViewData["trabajador"] = Trabajador.obtenerTrabajador();
                return View();
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        public ActionResult agregarNuevo(HttpPostedFileBase file, FormCollection form)
        {
            if (Session["rol"] != null && (Session["rol"].Equals("admin") || Session["rol"].Equals("contabilidad"))) 
            {
                mensaje = 0;
                string rut = (string)form["rut"];
                string nombres = (string)form["nombres"];
                string apellidos = (string)form["apellidos"];

                string fecha1 = (string)form["fechaIngresoEmpresa"];
                string[] fecha_1 = fecha1.Split('/');
                int dia1 = Convert.ToInt32(fecha_1[0]);
                int mes1 = Convert.ToInt32(fecha_1[1]);
                int ano1 = Convert.ToInt32(fecha_1[2]);

                DateTime fechaIngresoEmpresa = new DateTime(ano1, mes1, dia1);

                string fecha2 = (string)form["fechaIngresoSistema"];
                string[] fecha_2 = fecha2.Split('/');
                int dia2 = Convert.ToInt32(fecha_2[0]);
                int mes2 = Convert.ToInt32(fecha_2[1]);
                int ano2 = Convert.ToInt32(fecha_2[2]);

                DateTime fechaIngresoSistema = new DateTime(ano2, mes2, dia2);


                string direccion = (string)form["direccion"];
                string telefono = (string)form["telefono"];
                string obra = (string)form["obra"];
                string cargo = (string)form["cargo"];

                string tipo_regimen = (string)form["tipo_regimen"];
                string tipo_contrato = (string)form["tipo_contrato"];
                
                string porcentajeAFP;
                string porcentajeAPV;
                string porcentajeSalud;
                string porcentajeAFC;
                string sueldo_acordado;
                try
                {
                    sueldo_acordado = (string)form["sueldo_acordado"];
                }
                catch (Exception)
                {
                    sueldo_acordado = "";
                }
                try
                {
                    porcentajeAFC = (string)form["porcentajeAFC"];
                }
                catch (Exception)
                {
                    porcentajeAFC = "";
                }
                try
                {
                    porcentajeAFP = (string)form["porcentajeAFP"];
                }
                catch (Exception)
                {
                    porcentajeAFP = "";
                }

                try
                {
                    porcentajeAPV = (string)form["porcentajeAPV"];
                }
                catch (Exception ex)
                {
                    porcentajeAPV = "";
                }

                try
                {

                    porcentajeSalud = (string)form["porcentajeSalud"];
                }
                catch (Exception ex)
                {

                    porcentajeSalud = "";
                }

                string afp = (string)form["afp"];
                string salud = (string)form["salud"];


                string extImage = Convert.ToString(Request.Files["file"].ContentType);
                string[] infoImage = extImage.Split('/');

                try
                {
                    string fileExtension = System.IO.Path.GetExtension(Request.Files["file"].FileName);
                    string fileLocation = Server.MapPath("~/Images/copiasCI/") + rut + "." + infoImage[1];//Request.Files["file"].FileName;
                    Request.Files["file"].SaveAs(fileLocation);
                    trabajador Trabajador = new trabajador();

                    Trabajador.obra = obra;
                    Trabajador.rut = rut;
                    Trabajador.nombres = nombres;
                    Trabajador.apellidos = apellidos;
                    Trabajador.salud = salud;
                    Trabajador.afp = afp;
                    Trabajador.cargo = cargo;
                    Trabajador.direccion = direccion;
                    Trabajador.telefono = telefono;
                    Trabajador.fechaIngresoEmpresa = fechaIngresoEmpresa;
                    Trabajador.fechaIngresoSistema = fechaIngresoSistema;

                    if (!porcentajeAFP.Equals(""))
                    {

                        Trabajador.porcentajeAFP = Convert.ToDouble(porcentajeAFP);

                    }
                   
                    if (!porcentajeAFC.Equals(""))
                    {

                        Trabajador.porcentajeAFC = Convert.ToDouble(porcentajeAFC);

                    }

                    if (!porcentajeAPV.Equals(""))
                    {
                        Trabajador.porcentajeAPV = Convert.ToDouble(porcentajeAPV);
                    }

                    if (!porcentajeSalud.Equals(""))
                    {
                        Trabajador.porcentajeSalud = Convert.ToDouble(porcentajeSalud);
                    }
                    Trabajador.tipo_contrato=tipo_contrato;
                    Trabajador.tipo_regimen=tipo_regimen;
                    Trabajador.guardarTrabajador();
                    mensaje = 1;
                }
                catch (Exception ex)
                {
                    mensaje = 2;
                    trabajador.eliminarTrabajador(rut);
                    Console.Write(ex);


                }


                return RedirectToAction("todos", "Trabajador");
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }           
        }

        public string verificarExisteTrabajador(string rut)
        {
            //if (Session["rol"] != null
              //  && (Session["rol"].ToString().Equals("admin") || Session["rol"].ToString().Equals("gerencias")))
                            
                if (trabajador.existeTrabajador(rut))
                    return "true";
                else
                    return "false";          
        }

        public ActionResult guardarDatosPersonales(FormCollection form)
        {
            if (Session["rol"] != null && (Session["rol"].Equals("admin") || Session["rol"].Equals("contabilidad")))
            {
                trabajador Trabajador = new trabajador();
                Trabajador.rut = (string)form["rutTrabajador"];

                Trabajador.direccion=(string)form["direccion"];
                Trabajador.telefono=(string)form["telefono"];
                Trabajador.cargo=(string)form["cargo"];
                Trabajador.tipo_contrato = (string)form["tipo_contrato"];
                
                Trabajador.obra=(string)form["obra"];
                string tieneapv=(string)form["apv"];

                if (tieneapv.Equals("No"))
                {

                    Trabajador.porcentajeAPV = 0;
                }
                else {
                    try
                    {
                        Trabajador.porcentajeAPV = Convert.ToDouble((string)form["porcentajeAPV"]);
                    }
                    catch (Exception)
                    {
                        Trabajador.porcentajeAPV = 0;
                    } 
                }

               

                try
                {
                    Trabajador.porcentajeAFC = Convert.ToDouble((string)form["porcentajeAFC"]);
                }
                catch (Exception)
                {
                    Trabajador.porcentajeAFC = 0;
                }

                try
                {
                    Trabajador.porcentajeSalud = Convert.ToDouble((string)form["porcentajeSalud"]);
                }
                catch (Exception)
                {
                    Trabajador.porcentajeSalud = 0;
                }
                try
                {
                    Trabajador.porcentajeAFP = Convert.ToDouble((string)form["porcentajeAFP"]);
                }
                catch (Exception)
                {
                    Trabajador.porcentajeAFP = 0;
                }

                Trabajador.afp = (string)form["afp"];
                Trabajador.salud = (string)form["salud"];


                Trabajador.guardarDatosPersonales();

                ViewData["listaIsapres"] = isapre.obtenerTodasIsapres();
                ViewData["listaAFP"] = afp.obtenerTodasAFP();
                ViewData["obras"] = faena.obtenerTodas();
                ViewData["trabajador"] = Trabajador.obtenerTrabajador();
                return View("verTrabajador");
            }
            else {
                return RedirectToAction("Index", "Home");
            }
        
        }

        public ActionResult datosPersonales(string rutTrabajador) {
            if (Session["rol"] != null && (Session["rol"].Equals("admin") || Session["rol"].Equals("contabilidad")))
            {
                trabajador Trabajador = new trabajador();
                Trabajador.rut = rutTrabajador;
                ViewData["listaIsapres"] = isapre.obtenerTodasIsapres();
                ViewData["listaAFP"] = afp.obtenerTodasAFP();
                ViewData["obras"] = faena.obtenerTodas();
                ViewData["trabajador"] = Trabajador.obtenerTrabajador();                
                return View();
            }
            else { 
                return RedirectToAction("Index", "Home"); 
            }
        }  

        public ActionResult datosPago(string rutTrabajador)
        {
            if (Session["rol"]!=null && (Session["rol"].Equals("admin") || Session["rol"].Equals("contabilidad")))
            {
                if (datosPagoTrabajador.existenDatos(rutTrabajador))
                {
                    datosPagoTrabajador datosPago = datosPagoTrabajador.obtenerDatosPago(rutTrabajador);

                    trabajador trab = new trabajador();
                    trab.rut = rutTrabajador;
                    trab = trab.obtenerTrabajador();

                    ViewBag.NombreTrabajador = trab.nombres + trab.apellidos;

                    return View(datosPago);
                }
                else
                {
                    datosPagoTrabajador datosPago = new datosPagoTrabajador();
                    datosPago.rut = rutTrabajador;

                    trabajador trab = new trabajador();
                    trab.rut = rutTrabajador;
                    trab = trab.obtenerTrabajador();

                    ViewBag.NombreTrabajador = trab.nombres + trab.apellidos;

                    return View(datosPago);
                }
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        public ActionResult licencias(string rut)
        {
            if (Session["rol"] != null && (Session["rol"].Equals("admin") || Session["rol"].Equals("contabilidad")))
            {
                trabajador trab = new trabajador();
                trab.rut = rut;
                trab= trab.obtenerTrabajador();
                
                ViewBag.Rut = rut;                
                ViewBag.NombreTrabajador = trab.nombres + trab.apellidos;

                List<licenciasTrabajadores> licencias = licenciasTrabajadores.obtenerTodas(rut);

                return View(licencias);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        public ActionResult nuevaLicencia(string rut)
        {
            if (Session["rol"] != null && (Session["rol"].Equals("admin") || Session["rol"].Equals("contabilidad")))
            {
                trabajador trab = new trabajador();
                trab.rut = rut;
                trab = trab.obtenerTrabajador();

                ViewBag.Rut = rut;
                ViewBag.NombreTrabajador = trab.nombres + trab.apellidos;

                return View();
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        public ActionResult nuevasLicencias(string rut)
        {
            if (Session["rol"] != null && (Session["rol"].Equals("admin") || Session["rol"].Equals("contabilidad")))
            {
                trabajador trab = new trabajador();
                trab.rut = rut;
                trab = trab.obtenerTrabajador();

                ViewBag.Rut = rut;
                ViewBag.NombreTrabajador = trab.nombres + trab.apellidos;

                return View();
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        public string verificarLicencia(string fecha, string rut)
        {
            //if (Session["rol"] != null
            //  && (Session["rol"].ToString().Equals("admin") || Session["rol"].ToString().Equals("gerencias")))
            
            if (licenciasTrabajadores.existe(fecha, rut))
                return "true";
            else
                return "false";
        }

        public ActionResult guardarDatosPago(FormCollection post)
        {
            if (Session["rol"]!=null && (Session["rol"].Equals("admin") || Session["rol"].Equals("contabilidad")))
            {
                datosPagoTrabajador datosPago = new datosPagoTrabajador();

                datosPago.rut = (string)post["rut"].Replace("-", "").Replace(".", "");
                datosPago.sueldoBase = int.Parse(post["sueldoBase"].ToString());
                datosPago.gratificacion = int.Parse(post["gratificacion"].ToString());
                datosPago.bonoProduccion = int.Parse(post["bonoProduccion"].ToString());
                datosPago.bonoResponsabilidad = int.Parse(post["bonoResponsabilidad"].ToString());
                datosPago.asignacionFamiliar = int.Parse(post["asignacionFamiliar"].ToString());
                datosPago.bonoColacion = int.Parse(post["bonoColacion"].ToString());
                datosPago.bonoMovilizacion = int.Parse(post["bonoMovilizacion"].ToString());
                datosPago.viatico = int.Parse(post["viatico"].ToString());
                datosPago.desgasteHerramientas = int.Parse(post["desgasteHerramientas"].ToString());
                datosPago.cantidadHorasSemanales = float.Parse(post["horasSemanales"].ToString());

                datosPagoTrabajador.guardarDatos(datosPago);

                return RedirectToAction("verTrabajador", "Trabajador", new { rut = datosPago.rut });
            }
            else
            {
                return RedirectToAction("Index", "Trabajador");
            }
        }

        public ActionResult guardarLicencia(FormCollection post)
        {
            if (Session["rol"] != null && (Session["rol"].Equals("admin") || Session["rol"].Equals("contabilidad")))
            {
                licenciasTrabajadores nueva = new licenciasTrabajadores();

                nueva.rut = (string)post["rut"].Replace("-", "").Replace(".", "");

                DateTime fechaLicencia;

                string fecha = post["fecha"].ToString();

                int año = int.Parse(fecha.Split('/')[2]);
                int mes = int.Parse(fecha.Split('/')[1]);
                int dia = int.Parse(fecha.Split('/')[0]);

                fechaLicencia = new DateTime(año, mes, dia, 0, 0, 0);

                nueva.fecha = fechaLicencia;
                nueva.descripcion = (string)post["descripcion"];
                
                nueva.guardarDatos(nueva);

                return RedirectToAction("licencias", "Trabajador", new { rut = nueva.rut });
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        public ActionResult guardarLicencias(FormCollection post)
        {
            if (Session["rol"] != null && (Session["rol"].Equals("admin") || Session["rol"].Equals("contabilidad")))
            {
                licenciasTrabajadores nueva = new licenciasTrabajadores();

                nueva.rut = (string)post["rut"].Replace("-", "").Replace(".", "");

                DateTime fechaInicio;
                DateTime fechaTermino;

                string fechaInicioString = post["fechaInicio"].ToString();
                int año = int.Parse(fechaInicioString.Split('/')[2]);
                int mes = int.Parse(fechaInicioString.Split('/')[1]);
                int dia = int.Parse(fechaInicioString.Split('/')[0]);
                fechaInicio = new DateTime(año, mes, dia, 0, 0, 0);


                string fechaTerminoString = post["fechaTermino"].ToString();
                int añoT = int.Parse(fechaTerminoString.Split('/')[2]);
                int mesT = int.Parse(fechaTerminoString.Split('/')[1]);
                int diaT = int.Parse(fechaTerminoString.Split('/')[0]);
                fechaTermino = new DateTime(añoT, mesT, diaT, 0, 0, 0);

                DateTime fecha=fechaInicio;

                while (fecha.CompareTo(fechaTermino) <= 0) 
                {
                    nueva.fecha = fecha;
                    nueva.descripcion = (string)post["descripcion"];

                    if (!licenciasTrabajadores.existe(nueva)) 
                    {
                        nueva.guardarDatos(nueva);
                    }
                    fecha = fecha.AddDays(1);
                }

                return RedirectToAction("licencias", "Trabajador", new { rut = nueva.rut });
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }
        public ActionResult guardarInfoComplementaria(FormCollection post)
        {
            if (Session["rol"] != null && Session["rol"].Equals("admin"))
            {
                infoadicional nuevo = new infoadicional();
                nuevo.rut = (string)post["rut"];
                nuevo.estado_civil = (string)post["estadoCivil"];
                nuevo.talla_ropa = (string)post["talla"];
                nuevo.grupo_sangre = (string)post["grupoSangre"];
                nuevo.alergico = (string)post["alergico"];
                nuevo.personal_destacado = (string)post["destacado"];
                nuevo.numero_calzado = (string)post["nCalzado"];
                nuevo.antecedentes_conducir = (string)post["anteConducir"];
              /*  nuevo.antecedentes_salud = (string)post["anteLicencias"];
                nuevo.examenes = (string)post["examenes"];
                nuevo.nivel_estudio = (string)post["nivelEstudio"];*/
             /*   nuevo.detalle_capacitacion = (string)post["comment"];*/

                infoadicional.guardarDatosTrabajador(nuevo);

                string rut_trabajador = (string)post["rut"];
                string[] rutcarga = Request.Form.GetValues("rutCarga");
                string[] nombre_carga = Request.Form.GetValues("nombreCompleto");
                string[] sexocarga = Request.Form.GetValues("sexo");
                string[] edadcarga = Request.Form.GetValues("edad");
                string[] carga_registrada = Request.Form.GetValues("cargaRegistrada");

                if (cargafamiliar.tieneCargas(nuevo.rut))
                {
                    cargafamiliar.borrarCargas(nuevo.rut);
                }
                cargafamiliar.guardarDatosFamilia(rut_trabajador, rutcarga, nombre_carga, sexocarga, edadcarga, carga_registrada);

                return RedirectToAction("Index", "Home");
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }
        public ActionResult infoAdicional(string rutTrabajador)
        {
            if (Session["rol"] != null && Session["rol"].Equals("admin"))
            {
                if (infoadicional.existenDatos(rutTrabajador))
                {
                    ViewBag.rut = rutTrabajador;
                    infoadicional datos = infoadicional.obtenerComplementariosTrabajador(rutTrabajador);
                    if (cargafamiliar.tieneCargas(rutTrabajador))
                    {
                        datos.carga = cargafamiliar.obtenerCargas(rutTrabajador);
                        return View(datos);
                    }
                    else
                    {
                        return View(datos);
                    }
                }
                else
                {
                    infoadicional datos = new infoadicional();
                    datos.rut = rutTrabajador;
                  //  ViewBag.rut = rutTrabajador;
                    return View(datos);
                }
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }
    }
}
