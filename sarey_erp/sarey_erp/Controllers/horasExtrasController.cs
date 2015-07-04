using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using sarey_erp.Models;
namespace sarey_erp.Controllers
{
    public class horasExtrasController : Controller
    {
        //
        // GET: /horasExtras/

        public ActionResult Index(string rut)
        {
            ViewBag.rut = rut;
            return View();
        }

        public ActionResult agregarHoraExtra(FormCollection form) {

            string fecha = (string)form["fecha"];
            string[] fecha_1 = fecha.Split('/');
            int dia = Convert.ToInt32(fecha_1[0]);
            int mes = Convert.ToInt32(fecha_1[1]);
            int ano = Convert.ToInt32(fecha_1[2]);

            DateTime fechaHE = new DateTime(ano, mes, dia);
            string rut = (string)form["rut"];
            double cantidadHoras = Convert.ToDouble((string)form["cantidadHoras"]);

            horasExtras.GuardarHorasExtras(rut, fechaHE, cantidadHoras);
            return View("Index");
        }

    }
}
