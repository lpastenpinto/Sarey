using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using sarey_erp.Models;

namespace sarey_erp.Controllers
{
    public class FaenasController : Controller
    {
        //
        // GET: /Faenas/
        public static int flag = 0;

        public ActionResult resumenGlobal(string nombreFaena) 
        {
            List<datosResumenFaena> listaDatosResumen = datosResumenFaena.obtenerDatosGlobales(nombreFaena);
            return View(listaDatosResumen);            
        }

        public ActionResult detalleGastos(List<detalle_ordencompra> gastos)
        {
            return View(gastos);
        }

        public ActionResult resumenTotal(string nombreFaena) {
            faena listaTotalesFaena = faena.obtenerFaena(nombreFaena);
            return View(listaTotalesFaena);  
        }
        public ActionResult detalleGastosFaena(string nombreFaena)
        {
            if (Session["rol"] != null)
            {
                faena actual = new faena();
                actual.nombre = nombreFaena;

                ViewBag.Modo = "Faenas";

                List<detalle_ordencompra> gastos = actual.obtenerDetalleGastos();

                return View("detalleGastos", gastos);                
            }
            else
            {
                return RedirectToAction("login", "Home");
            }
        }

        public ActionResult detalleGastosPartida(string nombreFaena, string numeroPartida)
        {
            if (Session["rol"] != null)
            {
                partidaGlobal actual = partidaGlobal.obtenerPartidaGlobal(nombreFaena, numeroPartida);                

                ViewBag.Modo = "Partidas";
                ViewBag.Faena = nombreFaena;
                List<detalle_ordencompra> gastos = actual.obtenerDetalleGastos();

                return View("detalleGastos", gastos);
            }
            else
            {
                return RedirectToAction("login", "Home");
            }
        }

        public ActionResult detalleGastosItem(string nombreFaena, string idPartida)
        {
            if (Session["rol"] != null)
            {
                partida actual = partida.obtenerPartida(nombreFaena, idPartida);

                ViewBag.Modo = "Items";
                ViewBag.Faena = nombreFaena;
                ViewBag.PartidaGlobal = idPartida.Split('.')[0];
                List<detalle_ordencompra> gastos = actual.obtenerDetalleGastos();

                return View("detalleGastos", gastos);
            }
            else
            {
                return RedirectToAction("login", "Home");
            }
        }

        public ActionResult detalleGastosMaterial(string nombreFaena, string idPartida, string material)
        {
            if (Session["rol"] != null)
            {
                items actual = items.obtenerItem(nombreFaena, idPartida, material);

                ViewBag.Modo = "Material";
                ViewBag.Faena = nombreFaena;
                ViewBag.Partida= idPartida;
                List<detalle_ordencompra> gastos = actual.obtenerDetalleGastos();

                return View("detalleGastos", gastos);
            }
            else
            {
                return RedirectToAction("login", "Home");
            }
        }

        public ActionResult Index()
        {
            if (Session["rol"] != null)
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

                List<faena> todas = faena.obtenerTodas();

                return View(todas);
            }
            else
            {
                return RedirectToAction("login", "Home");
            }
        }

        public ActionResult Todos(string subidoCorrectamente)
        {
            if (subidoCorrectamente!=null && subidoCorrectamente.Equals("true")) 
            {
                ViewBag.mensaje = "agregado";
            }
            return RedirectToAction("Index");
        }

        public ActionResult nueva()
        {
            if (Session["rol"] != null)
            {
                return RedirectToAction("Index", "SubirDatosExcel");
            }
            else
            {
                return RedirectToAction("login", "Home");
            }
        }

        public ActionResult partidasGlobal(string Faena)
        {
            if (Session["rol"] != null)
            {
                List<partidaGlobal> lista = partidaGlobal.obtenerPartidasGlobales(Faena);
                ViewBag.Faena = Faena;
                return View(lista);
            }
            else
            {
                return RedirectToAction("login", "Home");
            }
        }

        public ActionResult itemsPartida(string Faena, string PartidaGlobal) 
        {
            if (Session["rol"] != null)
            {
                List<partida> lista = partida.obtenerPartidas(Faena, PartidaGlobal);
                ViewBag.Faena = Faena;
                ViewBag.Partida = PartidaGlobal;
                return View(lista);
            }
            else
            {
                return RedirectToAction("login", "Home");
            }
        }

        public ActionResult materialesItem(string Faena, string Partida)
        {
            if (Session["rol"] != null)
            {
                List<items> lista = items.obtenerTodosFaenaPartida(Faena, Partida);
                ViewBag.Faena = Faena;
                ViewBag.Partida = Partida;
                return View(lista);
            }
            else
            {
                return RedirectToAction("login", "Home");
            }
        }
    }
}
