using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using sarey_erp.Models;

namespace sarey_erp.Controllers
{
    public class ProveedorController : Controller
    {
        //
        // GET: /Proveedor/

        public ActionResult Index()
        {
            if (Session["nombre"] != null)
            {
                return RedirectToAction("nuevo");
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        public ActionResult nuevo()
        {
            if (Session["nombre"] != null && (Session["rol"].ToString().Equals("adquisiciones") || Session["rol"].ToString().Equals("admin")))
            {
                return View("nuevo");
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }
        public ActionResult Guardar(FormCollection post)
        {
                proveedores nuevo = new proveedores();
                nuevo.nombre_proveedor = (string)post["nombre"];
                nuevo.nombre_contacto = (string)post["nombreContacto"];
                nuevo.correo_contacto = (string)post["correoContacto"];
                nuevo.telefono = (string)post["telefonoContacto"];
                nuevo.direccion = (string)post["dirProveedor"];
                nuevo.razonsocial = (string)post["rSocial"];
                nuevo.rut = (string)post["rutProv"];

                proveedores.agregarProveedor(nuevo);
                return RedirectToAction("todos");
         
        }
        public ActionResult todos()
        {
            if (Session["nombre"] != null && (Session["rol"].ToString().Equals("adquisiciones") || Session["rol"].ToString().Equals("admin")))
            {
                List<proveedores> proveedor = proveedores.obtenerTodos();
                return View(proveedor);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }
        public string verificarProveedor(string valor_proveedor)
        {
            if (Session["nombre"] != null && (Session["rol"].ToString().Equals("adquisiciones") || Session["rol"].ToString().Equals("admin")))
            {

                if (proveedores.verificarSiExiste(valor_proveedor)) return "true";
                else return "false";
            }
            else
            {
                return "null";
            }
        }
        public ActionResult editar(string id)
        {
            if (Session["nombre"] != null && (Session["rol"].ToString().Equals("adquisiciones") || Session["rol"].ToString().Equals("admin")))
            {
                proveedores tdato = new proveedores().getProveedor(id);
                if (tdato.nombre_proveedor != null)
                {
                    return View(tdato);
                }
                return RedirectToAction("Index", "Home");
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }
        public ActionResult GuardarEditar(FormCollection form)
        {
            if (Session["nombre"] != null && (Session["rol"].ToString().Equals("adquisiciones") || Session["rol"].ToString().Equals("admin")))
            {
                proveedores proveedor = new proveedores();
                string id_old = form["nombreAnterior"];//old
                proveedores.borrarproveedor(id_old);
                //Actualizar producto..            
                proveedor.nombre_proveedor = (string)form["nombre"];
                proveedor.nombre_contacto = (string)form["nombreContacto"];
                proveedor.correo_contacto = (string)form["correoContacto"];
                proveedor.telefono = (string)form["telefonoContacto"];
                proveedor.direccion = (string)form["dirProveedor"];
                proveedor.razonsocial = (string)form["rSocial"];
                proveedor.rut = (string)form["rutProv"];
                proveedores.agregarProveedor(proveedor);

                return RedirectToAction("todos");
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }
        public ActionResult eliminar(string nombreProveedor)
        {
            if (Session["rol"] != null
                && (Session["rol"].ToString().Equals("admin") || Session["rol"].ToString().Equals("adquisiciones")))
            {
                proveedores.borrarproveedor(nombreProveedor);
                return RedirectToAction("todos", "Proveedor");
            }
            else
            {
                return RedirectToAction("login", "Home");
            }
        }
    }
}
