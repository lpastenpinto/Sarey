using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using sarey_erp.Models;
using System.Globalization;

namespace sarey_erp.Controllers
{
    public class LiquidacionesController : Controller
    {
        //
        // GET: /Liquidaciones/

        public ActionResult Index()
        {
            return View();
        }

        
        public ActionResult verLiquidaciones(FormCollection form)
        {

            int mes = Convert.ToInt32((string)form["mes"]);
            int anio = Convert.ToInt32((string)form["anio"]);
            ViewBag.mes = mes;
            ViewBag.anio = anio;
            List<liquidacion> Liquidaciones = liquidacion.obtenerTodasLiquidacionesPorFecha(mes, anio);
            return View("todasLiquidacionesMes", Liquidaciones);
        }


        public ActionResult verLiquidacionTrabajador(string rutTrabajador, int mes, int anio) {
           
            liquidacion LiquidacionTrabajador = liquidacion.obtenerLiquidacionPorFecha(rutTrabajador,mes,anio);
           
            trabajador Trabajador = new trabajador();
            Trabajador.rut = rutTrabajador;
            Trabajador = Trabajador.obtenerTrabajador();

            ViewData["liquidacion"] = LiquidacionTrabajador;
            ViewData["trabajador"] = Trabajador;
         
            return View("../Trabajador/verLiquidacion");            
        }


        public ActionResult todasLiquidacionesMes(FormCollection form)
        {
            List<trabajador> Trabajadores = new List<trabajador>();
            List<liquidacion> Liquidaciones = new List<liquidacion>();
            Trabajadores = trabajador.obtenerTodos();
            int mes = Convert.ToInt32((string)form["mes"]);
            int anio = Convert.ToInt32((string)form["anio"]);

            ViewBag.mes = mes;
            ViewBag.anio = anio;
            foreach (trabajador Trab in Trabajadores)
            {
                liquidacion LiquidacionTrabajador = new liquidacion(anio, mes);
                LiquidacionTrabajador.rut = Trab.rut;
                LiquidacionTrabajador.generarLiquidacion();
                LiquidacionTrabajador.borrarLiquidacion();
                LiquidacionTrabajador.guardarLiquidacion();
                Liquidaciones.Add(LiquidacionTrabajador);
            }

            return View(Liquidaciones);
        }

    }
}
