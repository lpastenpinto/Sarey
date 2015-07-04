using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using sarey_erp.Models;


namespace sarey_erp.Controllers
{
    public class finiquitosController : Controller
    {
        //
        // GET: /finiquitos/

        public ActionResult Index(string rut)
        {
            ViewBag.rut = rut;
            return View();
        }

        public ActionResult verTodos() {
            List<finiquitos> Finiquitos = finiquitos.obtenerFiniquitos();            
            return View(Finiquitos);
        
        }

        public ActionResult verDetalle(string rut) {

            finiquitos Finiquito = finiquitos.obtenerFiniquitoTrabajador(rut);
            ViewData["finiquito"] = Finiquito;
            return View();
        }
        public ActionResult finiquitarTrabajador(FormCollection form) {
            string rut= (string)form["rut"];
            string motivo =(string)form["motivo"];
            string detalle_motivo = (string)form["detalle_motivo"];
            string fecha = (string)form["fecha_finiquito"];
            string[] fecha_1 = fecha.Split('/');
            int dia = Convert.ToInt32(fecha_1[0]);
            int mes = Convert.ToInt32(fecha_1[1]);
            int ano = Convert.ToInt32(fecha_1[2]);

            DateTime fechaFiniquito = new DateTime(ano, mes, dia);
            finiquitos Finiquito = new finiquitos();
            Finiquito.rut = rut;
            Finiquito.fecha_finiquito = fechaFiniquito;
            Finiquito.motivo = motivo;
            Finiquito.detalle_motivo = motivo;
            Finiquito.generarFiniquito();
            Finiquito.guardarFiniquito();

            List<trabajador> trabajadores = new List<trabajador>();
            trabajadores = trabajador.obtenerTodos();


            return View("../Trabajador/todos",trabajadores);  
        }

    }
}
