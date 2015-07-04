using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using sarey_erp.Models;

namespace sarey_erp.Controllers
{
    public class diasNoTrabajadosController : Controller
    {
        public ActionResult Index(string rut)
        {
            if (Session["rol"] != null && (Session["rol"].Equals("admin") || Session["rol"].Equals("contabilidad")))
            {
                List<diasNoTrabajados> dias = diasNoTrabajados.obtenerTodas(rut);

                trabajador trab = new trabajador();
                trab.rut = rut;
                trab = trab.obtenerTrabajador();

                ViewBag.Rut = rut;
                ViewBag.NombreTrabajador = trab.nombres + trab.apellidos;

                return View(dias);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        public ActionResult nuevoDiaNoTrabajado(string rut) 
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

        public ActionResult guardarDiaNoTrabajado(FormCollection post)
        {
            if (Session["rol"] != null && (Session["rol"].Equals("admin") || Session["rol"].Equals("contabilidad")))
            {
                diasNoTrabajados nueva = new diasNoTrabajados();

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

                return RedirectToAction("Index", "diasNoTrabajados", new { rut = nueva.rut });
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        public string verificarDiaNoTrabajado(string fecha, string rut)
        {
            //if (Session["rol"] != null
            //  && (Session["rol"].ToString().Equals("admin") || Session["rol"].ToString().Equals("gerencias")))

            if (diasNoTrabajados.existe(fecha, rut))
                return "true";
            else
                return "false";
        }
    }
}
