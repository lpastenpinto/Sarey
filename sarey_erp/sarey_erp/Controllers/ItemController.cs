using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using sarey_erp.Models;

namespace sarey_erp.Controllers
{
    public class ItemController : Controller
    {
        //
        // GET: /Item/

        public ActionResult Index()
        {
            return View();
        }
        public ActionResult nuevoItem(){
            return View();
        }
        public ActionResult crearItem(FormCollection form) {

            items nuevoItem = new items();
            nuevoItem.nombre = (string)form["nombre"];
            nuevoItem.descripcion=(string)form["descripcion"];
            nuevoItem.unidad=(string)form["unidad"];
            nuevoItem.dimensiones=(string)form["dimensiones"];
            
            //bool verificar=nuevoItem.guardarNuevoItem();
            //ViewBag.verifica_creacion = verificar;
            
            return View("nuevoItem");   
        }
    }
}
