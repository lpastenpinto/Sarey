using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using sarey_erp.Models;

namespace sarey_erp.Controllers
{
    public class FeriadoController : Controller
    {
        //
        // GET: /Feriado/

        public ActionResult Index()
        {
            return View();
        }
        public ActionResult nuevo()
        {
            if (Session["rol"] != null && (Session["rol"].Equals("admin") || Session["rol"].Equals("contabilidad")))
            {
                return View("nuevo");
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }
        public ActionResult todos()
        {
            if (Session["rol"] != null && (Session["rol"].Equals("admin") || Session["rol"].Equals("contabilidad")))
            {
                List<feriados> todos = feriados.Obtenerdias();
                return View(todos);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }
        public ActionResult guardarDia(FormCollection post)
        {
            if (Session["rol"] != null && (Session["rol"].Equals("admin") || Session["rol"].Equals("contabilidad")))
            {
                feriados nuevo = new feriados();
                DateTime fechaFeriado;

                string fecha = post["datetimepicker1"].ToString();

                int año = int.Parse(fecha.Split('/')[2]);
                int mes = int.Parse(fecha.Split('/')[1]);
                int dia = int.Parse(fecha.Split('/')[0]);

                fechaFeriado = new DateTime(año, mes, dia, 0, 0, 0);
                nuevo.dia = fechaFeriado;
                nuevo.festividad = (string)post["festivo"];
                nuevo.tipo_feriado = (string)post["tipo_feriado"];
                nuevo.irrenunciable = (string)post["irrenunciable"];
                feriados.Guardar(nuevo);

                return RedirectToAction("todos", "Feriado");
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

    }
}
