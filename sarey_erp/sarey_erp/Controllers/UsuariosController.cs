using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using sarey_erp.Models;

namespace sarey_Erp.Controllers
{
    public class UsuariosController : Controller
    {
        //
        // GET: /Usuarios/

        public static int flag = 0;

        public ActionResult Index()
        {
            if (Session["rol"] != null
                && (Session["rol"].ToString().Equals("admin") || Session["rol"].ToString().Equals("gerencias")))
            {
                /*if (flag == 1)
                {
                    ViewBag.mensaje = "se usa";
                }//*/
                if (flag == 2)
                {
                    ViewBag.mensaje = "agregado";
                }
                if (flag == 3)
                {
                    ViewBag.mensaje = "eliminado";
                }
                if (flag == 4)
                {
                    ViewBag.mensaje = "modificado";
                }

                flag = 0;

                List<usuarios> todos = usuarios.obtenerTodos();
                return View(todos);
            }
            else
            {
                return RedirectToAction("login", "Home");
            }
        }

        public ActionResult Nuevo()
        {
            if (Session["rol"] != null && 
                (Session["rol"].ToString().Equals("admin") || Session["rol"].ToString().Equals("gerencias")))
            {
                ViewBag.roles = usuarios.obtenerRoles();
                return View();
            }
            else
            {
                return RedirectToAction("login", "Home");
            }
        }

        public ActionResult Editar(string nombreUsuario)
        {
            if (Session["rol"] != null && 
                (Session["rol"].ToString().Equals("admin") || Session["rol"].ToString().Equals("gerencias")))
            {
                usuarios editar = usuarios.obtenerUsuario(nombreUsuario);
                ViewBag.roles = usuarios.obtenerRoles();
                return View(editar);
            }
            else
            {
                return RedirectToAction("login", "Home");
            }
        }

        [HttpPost]
        public ActionResult agregarUsuario(FormCollection post)
        {
            if (Session["rol"] != null && 
                (Session["rol"].ToString().Equals("admin") || Session["rol"].ToString().Equals("gerencias")))
            {
                usuarios nuevo = new usuarios();
                nuevo.nombre = post["nombreUsuario"];
                nuevo.nombreCompleto = post["nombreCompleto"];
                nuevo.correoElectronico = post["correo"];
                string telefono = (string)post["celular"];
                if (telefono.Equals("")) 
                {
                    nuevo.telefono = 0;
                }
                else {
                    nuevo.telefono = int.Parse(telefono);
                }
                
                nuevo.rol = post["rol"];
                nuevo.cargo = (string)post["cargo"];
                nuevo.rut = post["rut"];

                usuarios.agregarBD(nuevo, encriptacion.GetMD5(post["password"].ToString()));

                /*registros nuevoRegistro = new registros();
                nuevoRegistro.fecha = DateTime.Now;
                nuevoRegistro.tipo = "Creación de usuario";
                nuevoRegistro.usuario = Session["nombre"].ToString();
                nuevoRegistro.descripcion = "El usuario " + nuevoRegistro.usuario 
                    + " ha agregado al usuario " + nuevo.nombreUsuario + " al sistema";
                registros.agregarRegistro(nuevoRegistro);

                registros nuevoRegistro2 = new registros();
                nuevoRegistro2.fecha = DateTime.Now;
                nuevoRegistro2.tipo = "Creación de usuario";
                nuevoRegistro2.usuario = nuevo.nombreUsuario;
                nuevoRegistro2.descripcion = "El usuario " + nuevo.nombreUsuario 
                    + " ha sido agregado al sistema por el usuario " + Session["nombre"].ToString();
                registros.agregarRegistro(nuevoRegistro2);
                //*/

                flag = 2;//Agregado con éxito

                return RedirectToAction("Index", "Usuarios");
            }
            else
            {
                return RedirectToAction("login", "Home");
            }
        }

        [HttpPost]
        public ActionResult editarUsuario(FormCollection post)
        {
            if (Session["rol"] != null 
                && (Session["rol"].ToString().Equals("admin") || Session["rol"].ToString().Equals("gerencias")))
            {
                usuarios editado = new usuarios();
                editado.nombre = post["nombreUsuario"];
                editado.nombreCompleto = post["nombreCompleto"];
                editado.correoElectronico = post["correo"];
                string telefono = (string)post["celular"];
                if (telefono.Equals(""))
                {
                    editado.telefono = 0;
                }
                else
                {
                    editado.telefono = int.Parse(telefono);
                }
                editado.rol = post["rol"];
                editado.cargo = (string)post["cargo"];
                editado.rut = post["rut"];

                usuarios.editarBD(editado, encriptacion.GetMD5(post["password"].ToString()), post["nombreAnterior"]);
                
                /*
                registros nuevoRegistro = new registros();
                nuevoRegistro.fecha = DateTime.Now;
                nuevoRegistro.tipo = "Edición de usuario";
                nuevoRegistro.usuario = Session["nombre"].ToString();
                nuevoRegistro.descripcion = "El usuario " + nuevoRegistro.usuario
                    + " ha editado los datos del usuario " + editado.nombreUsuario;
                registros.agregarRegistro(nuevoRegistro);

                registros nuevoRegistro2 = new registros();
                nuevoRegistro2.fecha = DateTime.Now;
                nuevoRegistro2.tipo = "Edición de usuario";
                nuevoRegistro2.usuario = editado.nombreUsuario;
                nuevoRegistro2.descripcion = "Los datos del usuario " + editado.nombreUsuario
                    + " han sido editados por el usuario " + Session["nombre"].ToString();
                registros.agregarRegistro(nuevoRegistro2);
                //*/
                
                flag = 4;//Editado con éxito

                return RedirectToAction("Index", "Usuarios");
            }
            else
            {
                return RedirectToAction("login", "Home");
            }
        }

        public ActionResult eliminar(string nombreUsuario)
        {
            if (Session["rol"] != null 
                && (Session["rol"].ToString().Equals("admin") || Session["rol"].ToString().Equals("gerencias")))
            {
                usuarios.eliminarBD(nombreUsuario);

                /*
                registros nuevo = new registros();
                nuevo.fecha = DateTime.Now;
                nuevo.usuario = Session["nombre"].ToString();
                nuevo.tipo="Eliminación de usuario";
                nuevo.descripcion = "El usuario " + nuevo.usuario + " ha eliminado al usuario " + nombreUsuario + " del sistema";
                registros.agregarRegistro(nuevo);

                registros nuevo2 = new registros();
                nuevo2.fecha = DateTime.Now;
                nuevo2.usuario = nombreUsuario;
                nuevo2.tipo = "Eliminación de usuario";
                nuevo2.descripcion = "El usuario " + nombreUsuario 
                    + " ha sido eliminado del sistema por el usuario " + nuevo.usuario;
                registros.agregarRegistro(nuevo2);
                //*/


                flag = 3;//Editado con éxito

                return RedirectToAction("Index", "Usuarios");
            }
            else
            {
                return RedirectToAction("login", "Home");
            }
        }

        public string existeUsuario()
        {
            if (Session["rol"] != null
                && (Session["rol"].ToString().Equals("admin") || Session["rol"].ToString().Equals("gerencias")))
            {
                string nombreusuario = Request["nombreUsuario"];

                if (usuarios.existeUsuario(nombreusuario)) return "true";
                else return "false";
            }
            else             
            {
                return "null";
            }
        }

        public string verificarRut()
        {
            if (Session["rol"] != null)
            {
                string rut = Request["rut"];

                if (esRutValido(rut)) return "true";
                else return "false";
            }
            else
            {
                return "null";
            }
        }

        bool esRutValido(string rut)
        {
            if (rut.Length < 8) return false;

            char[] arreglo = rut.ToCharArray();

            int
                suma = 0,
                contador = 0;

            for (int i = 0; i < arreglo.Length - 1; i++)
            {
                if (!int.TryParse(arreglo[i].ToString(), out suma))
                {
                    return false;
                }
            }

            suma = 0;

            if (rut.Length == 9)
            {
                suma = int.Parse(arreglo[contador].ToString()) * 3;
                contador++;
            }

            suma += int.Parse(arreglo[contador].ToString()) * 2;
            contador++;

            for (int i = 7; i > 1; i--)
            {
                suma += int.Parse(arreglo[contador].ToString()) * i;
                contador++;
            }

            int resultado = 11 - (suma % 11);

            char digitoVerificador;
            if (resultado == 11)
            {
                digitoVerificador = '0';
            }
            else if (resultado == 10)
            {
                digitoVerificador = 'k';
            }
            else
            {
                digitoVerificador = resultado.ToString().ToCharArray()[0];
            }

            if (digitoVerificador == arreglo[contador].ToString().Replace('K','k').ToCharArray()[0]) return true;

            return false;
        }
    }
}
