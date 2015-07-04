using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using sarey_erp.Models;

namespace sarey_erp.Controllers
{
    public class HomeController : Controller
    {
        static int flag = 0;

        public void opcionPantallaAncha() {
            if (Session["pantallaAncha"] == null || Session["pantallaAncha"].Equals("false"))
                Session["pantallaAncha"] = "true";
            else Session["pantallaAncha"] = "false";
        }

        string formatearNumero(string valor) 
        {
            string retorno = "";

            char[] caracteres = valor.ToCharArray();

            for (int i = 0; i < caracteres.Length; i++) 
            {
                if (caracteres[i] == '0' || caracteres[i] == '1' || caracteres[i] == '2' || caracteres[i] == '3' || caracteres[i] == '4'
                    || caracteres[i] == '5' || caracteres[i] == '6' || caracteres[i] == '7' || caracteres[i] == '8' || caracteres[i] == '9') 
                {
                    retorno += caracteres[i];
                }
            }
            if (retorno.Equals("")) retorno = "0";
            return retorno;
        }

        string formatearDecimal(string valor)
        {
            string retorno = "";

            char[] caracteres = valor.ToCharArray();

            for (int i = 0; i < caracteres.Length; i++)
            {
                if (caracteres[i] == '0' || caracteres[i] == '1' || caracteres[i] == '2' || caracteres[i] == '3' 
                    || caracteres[i] == '4' || caracteres[i] == '5' || caracteres[i] == '6' || caracteres[i] == '7'
                    || caracteres[i] == '8' || caracteres[i] == '9' || caracteres[i] == ',' || caracteres[i] == '.')
                {
                    retorno += caracteres[i];
                }
            }
            if (retorno.Equals("")) retorno = "0";
            retorno = retorno.TrimEnd('.');
            retorno = retorno.TrimEnd('.');
            retorno = retorno.TrimEnd(',');
            retorno = retorno.TrimEnd(',');
            return retorno;
        }

        public ActionResult Index()
        {
            if (Session["rol"] != null)
            {
                //Si ha pasado más de un día desde que se actualizaron los datos financieros, se vuelven a actualizar
                if ((DateTime.Now - controlDatosFinancieros.obtenerUltimaFechaActualizacion()).TotalDays > 1) 
                {
                    string ultimosDatosAFP = sarey_erp.Models.datosFinancieros.obtenerDatosAFP();
                    if (!ultimosDatosAFP.Equals("")) {

                        afp.borrarAFPs();

                        string[] datosAfpAgrupados = ultimosDatosAFP.Split(';');

                        foreach (string datosAFP in datosAfpAgrupados)
                        {
                            string[] datosAfpSeparados = datosAFP.Split('|');

                            afp nuevaAFP = new afp();

                            nuevaAFP.nombre_afp = datosAfpSeparados[0];
                            nuevaAFP.tasa = double.Parse(datosAfpSeparados[1].Replace("%", "").Replace(".", ","));
                            nuevaAFP.sis = double.Parse(datosAfpSeparados[2].Replace("%", "").Replace(".", ","));
                            nuevaAFP.tasa_independientes = double.Parse(datosAfpSeparados[3].Replace("%", "").Replace(".", ","));

                            afp.guardar(nuevaAFP);
                        }
                    }


                    string ultimosDatosASignacionFamiliar = sarey_erp.Models.datosFinancieros.obtenerDatosAsignacionFamiliar();
                    if (!ultimosDatosASignacionFamiliar.Equals(""))
                    {

                        asignacionFamiliar.borrarDatos();

                        string[] datosAsignacionAgrupados = ultimosDatosASignacionFamiliar.Split('!');

                        foreach (string datosAsignacion in datosAsignacionAgrupados)
                        {
                            string[] datosAsignacionSeparados = datosAsignacion.Split('|');

                            asignacionFamiliar asignacion = new asignacionFamiliar();

                            asignacion.tramo= datosAsignacionSeparados[0];
                            asignacion.monto = float.Parse(formatearNumero(datosAsignacionSeparados[1]));
                            
                            asignacion.menorQue = 
                                float.Parse(formatearNumero(separarDatosAsignacion(datosAsignacionSeparados[2])[1]));                            
                            asignacion.mayorQue = 
                                float.Parse(formatearNumero(separarDatosAsignacion(datosAsignacionSeparados[2])[0]));

                            asignacionFamiliar.guardar(asignacion);
                        }
                    }

                    string ultimosDatosSeguroCesantia = sarey_erp.Models.datosFinancieros.obtenerDatosSeguroCesantia();
                    if (!ultimosDatosSeguroCesantia.Equals(""))
                    {
                        seguroCesantia.borrarDatos();

                        string[] datosSeguroAgrupados = ultimosDatosSeguroCesantia.Split('!');

                        foreach (string datosSeguro in datosSeguroAgrupados)
                        {
                            string[] datosSeguroSeparados = datosSeguro.Split('|');

                            seguroCesantia datosSeguroCesantia = new seguroCesantia();

                            datosSeguroCesantia.descripcion = datosSeguroSeparados[0];
                            datosSeguroCesantia.empleador = double.Parse(formatearDecimal(datosSeguroSeparados[1]));

                            datosSeguroCesantia.trabajador = double.Parse(formatearDecimal(datosSeguroSeparados[2]));

                            seguroCesantia.guardar(datosSeguroCesantia);
                        }
                    }
                    string ultimosDatosRentasMinimas = sarey_erp.Models.datosFinancieros.obtenerDatosRentasMinimas();
                    if (!ultimosDatosRentasMinimas.Equals(""))
                    {
                        rentasMinimasImponibles.borrarDatos();

                        string[] datosRentasMinimasAgrupados = ultimosDatosRentasMinimas.Split('!');

                        foreach (string datosRentas in datosRentasMinimasAgrupados)
                        {
                            string[] datosRentasSeparados = datosRentas.Split('|');

                            rentasMinimasImponibles datosRentasMinimas = new rentasMinimasImponibles();

                            datosRentasMinimas.descripcion = datosRentasSeparados[0];
                            datosRentasMinimas.valor = double.Parse(formatearDecimal(datosRentasSeparados[1]));

                            rentasMinimasImponibles.guardar(datosRentasMinimas);
                        }
                    }
                    string ultimosDatosRentasTopes = sarey_erp.Models.datosFinancieros.obtenerDatosRentasTopes();
                    if (!ultimosDatosRentasTopes.Equals(""))
                    {
                        rentasTopesImponibles.borrarDatos();

                        string[] datosRentasTopesAgrupados = ultimosDatosRentasTopes.Split('!');

                        foreach (string datosRentasTopes in datosRentasTopesAgrupados)
                        {
                            string[] datosRentasTopesSeparados = datosRentasTopes.Split('|');

                            rentasTopesImponibles datosRentasTopesImponibles = new rentasTopesImponibles();

                            datosRentasTopesImponibles.descripcion = datosRentasTopesSeparados[0];
                            datosRentasTopesImponibles.valor = double.Parse(formatearDecimal(datosRentasTopesSeparados[1]));

                            rentasTopesImponibles.guardar(datosRentasTopesImponibles);
                        }
                    }

                    string ultimosDatosAPV = sarey_erp.Models.datosFinancieros.obtenerDatosAPV();
                    if (!ultimosDatosRentasTopes.Equals(""))
                    {
                        apv.borrarDatos();

                        string[] datosAPVAgrupados = ultimosDatosAPV.Split('!');

                        foreach (string datosAPV in datosAPVAgrupados)
                        {
                            string[] datosAPVSeparados = datosAPV.Split('|');

                            apv datosAhorroPV = new apv();

                            datosAhorroPV.descripcion = datosAPVSeparados[0];
                            datosAhorroPV.valor = double.Parse(formatearDecimal(datosAPVSeparados[1]));

                            apv.guardar(datosAhorroPV);
                        }
                    }

                    controlDatosFinancieros.registrarNuevaActualizacion();
                }
                ViewBag.datosAFP = afp.obtenerTodasAFP();
                ViewBag.datosAsignacion = asignacionFamiliar.obtenerTodas();
                ViewBag.seguroCesantia = seguroCesantia.obtenerTodos();
                ViewBag.rentasMinimas = rentasMinimasImponibles.obtenerTodos();
                ViewBag.rentasTopes = rentasTopesImponibles.obtenerTodos();
                ViewBag.apv = apv.obtenerTodos();
                ViewBag.ultimaActualizacionUF = uf.obtenerUltimaFecha();
                ViewBag.ultimaUF = uf.obtenerUltimaUF().valor;

                return View();
            }
            else
            {
                return RedirectToAction("login", "Home");
            }

        }

        public void guardarUltimaUF(string valor) 
        {
            double nuevaUF = double.Parse(valor);

            DateTime finDeMes = DateTime.Now;
            int mes = finDeMes.Month;
            DateTime temp =finDeMes;
            while (temp.Month == mes) {
                finDeMes = temp;
                temp = temp.AddDays(1);
            }

            uf.guardarNueva(nuevaUF, finDeMes);
        }

        public string[] separarDatosAsignacion(string datosAsignacion) 
        {
            string[] retorno = new string[2];
            retorno[0] = "";
            retorno[1] = "";

            if (datosAsignacion.Contains("&lt;"))
            {
                char[] caracteres = datosAsignacion.ToCharArray();
                int posicion=0;

                for (int i = 0; i < caracteres.Length; i++) 
                {
                    if (datosAsignacion[i] == '&' &&
                        (i + 1) < caracteres.Length && datosAsignacion[i + 1] == 'l' &&
                        (i + 2) < caracteres.Length && datosAsignacion[i + 2] == 't' &&
                        (i + 3) < caracteres.Length && datosAsignacion[i + 3] == ';')
                        posicion = i;
                }

                retorno[0] = datosAsignacion.Substring(0,posicion);
                retorno[1] = datosAsignacion.Substring(posicion, datosAsignacion.Length - posicion);
            }
            else 
            {
                retorno[0] = datosAsignacion;
            }

            return retorno;
        }

        public ActionResult login()
        {
            if (Session["rol"] != null)
            {
                return RedirectToAction("Index", "Home");
            }
            else
            {
                if (flag == 2) ViewBag.mensaje = "Contraseña cambiada";
                if (flag == 4) ViewBag.mensaje = "login erroneo";
                flag = 0;
                return View();
            }
        }

        public ActionResult cambiarContraseña()
        {
            if (Session["rol"] != null)
            {
                if (flag == 1) ViewBag.error = "contraseñaIncorrecta";
                flag = 0;
                usuarios usuarioActual = usuarios.obtenerUsuario(Session["nombre"].ToString());
                return View(usuarioActual);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        [HttpPost]
        public ActionResult verificarLogin(FormCollection post)
        {
            if (post["nombre"] == null || post["nombre"].ToString().Equals(""))
            {
                Session.RemoveAll();
                return RedirectToAction("login", "Home");
            }
            else if (usuarios.revisarUsuarioPassword(post["nombre"], encriptacion.GetMD5(post["password"])))
            {
                Session["nombre"] = post["nombre"];
                Session["rol"] = usuarios.obtenerRol(post["nombre"]);
                Session["password"] = encriptacion.GetMD5(post["password"]);
                //La sesión dura 1.5 horas por defecto
                Session.Timeout = 90;

                /*
                registros nuevo = new registros();
                nuevo.fecha = DateTime.Now;
                nuevo.tipo = "Inicio de Sesión";
                nuevo.descripcion = "El usuario " + Session["nombre"] + " inició sesión en el sistema";
                nuevo.usuario = Session["nombre"].ToString();

                registros.agregarRegistro(nuevo);

                //*/

                return RedirectToAction("Index", "Home");
            }
            flag = 4; //usuario y/o contraseña erróneos
            return RedirectToAction("Index");
        }

        public string verificarLoginAJAX()
        {
            if (usuarios.revisarUsuarioPassword(Session["nombre"].ToString(), Session["password"].ToString()))
            {
                //La sesión se aplaza 1.5 horas por defecto
                Session.Timeout = 90;
                
                return "true";
            }

            Session.RemoveAll();
            return "false";
        }

        public ActionResult cerrarSesion()
        {
            Session.RemoveAll();

            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public ActionResult metodoCambioPassword(FormCollection post)
        {
            if (usuarios.revisarUsuarioPassword(post["nombreUsuario"], post["passwordActual"]))
            {
                if (post["passwordNuevo"].ToString().Equals(post["passwordNuevo2"].ToString()))
                {
                    usuarios.cambiarContraseña(post["nombreUsuario"].ToString(), post["passwordNuevo"].ToString());
                    flag = 2;//contraseña cambiada correctamente

                    /*
                    registros nuevo = new registros();
                    nuevo.fecha = DateTime.Now;
                    nuevo.tipo = "Cambio de contraseña";
                    nuevo.usuario = Session["nombre"].ToString();
                    nuevo.descripcion = "El usuario " + nuevo.usuario + " ha modificado su contraseña";

                    registros.agregarRegistro(nuevo);
                    //*/

                    Session.RemoveAll();

                }
                return RedirectToAction("Index", "Home");
            }
            else
            {
                flag = 1;//Contraseña incorrecta
                return RedirectToAction("cambiarContraseña");
            }
        }
    }
}
