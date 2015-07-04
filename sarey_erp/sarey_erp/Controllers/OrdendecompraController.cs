using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using sarey_erp.Models;

namespace sarey_erp.Controllers
{
    public class OrdendecompraController : Controller
    {
        //
        // GET: /Ordendecompra/

        public ActionResult Index()
        {
            return View();
        }
        public ActionResult nuevaOrden(string id)
        {
            if (Session["nombre"] != null && (Session["rol"].ToString().Equals("adquisiciones") || Session["rol"].ToString().Equals("admin")))
            {
                ViewBag.NumOld = id;
                ViewBag.NumeroOrden = orden_compra.numeroNuevaOrden();
                ViewData["proveedores"] = proveedores.obtenerTodos();
                ViewData["faenas"] = faena.obtenerTodas();
          //      ViewData["solicitudes"] = solicitud_materiales.obtenerTodasSolicitudesRevisadasNoProcesadas();
                return View("nuevaOrden");
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }

        }
        public ActionResult nueva()
        {
            if (Session["nombre"] != null && (Session["rol"].ToString().Equals("adquisiciones") || Session["rol"].ToString().Equals("admin")))
            {
                ViewBag.NumeroOrden = orden_compra.numeroNuevaOrden();
                ViewData["proveedores"] = proveedores.obtenerTodos();
                ViewData["faenas"] = faena.obtenerTodas();
              ViewData["solicitudes"] = solicitud_materiales.obtenerTodasSolicitudesRevisadasNoProcesadas();
                return View("nueva");
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }
        public string rescatar_datos_item(string valor_solicitud)
        {
            string retorno = "";
            string[] items = solicitud_materiales.obtener_items_pendientes(valor_solicitud);
            for (int i = 0; i < items.Length; i++)
            {
                retorno += items[i] + ";";
            }
            retorno = retorno.TrimEnd(';');
            return retorno;
        }
        public string rescatar_datos_solicitudes(string valor_seleccion)
        {
            string retorno = "";
            string[] solicitudes = solicitud_materiales.obtenerTodasSolicitudesFaenas(valor_seleccion);
            for (int i = 0; i < solicitudes.Length; i++)
            {
                retorno += solicitudes[i] + ";";
            }
            retorno = retorno.TrimEnd(';');
            return retorno;


        }
        public string rescatar_partida_asignada(string valor_partida, string valor_faena, string valor_material)
        {
            string retorno2 = "";
            string[] items = solicitud_materiales.obtener_asignada(valor_partida,valor_faena,valor_material);
            for (int i = 0; i < items.Length; i++)
            {
                retorno2 += items[i] + ";";
            }
            retorno2 = retorno2.TrimEnd(';');
            return retorno2;
        }
        public string rescatar_valor_asignado(string valor_partida, string valor_faena, string valor_material){
            string retorno3 = solicitud_materiales.obtener_valor_material(valor_partida,valor_faena,valor_material);
            return (retorno3);
        }

        public ActionResult Guardar(FormCollection post)
        {
            if (Session["nombre"] != null && (Session["rol"].ToString().Equals("adquisiciones") || Session["rol"].ToString().Equals("admin")))
            {
                DateTime fecha = DateTime.Now;
                orden_compra nuevo = new orden_compra();
                string id = (string)post["numOrden"];
                nuevo.numero_orden = (string)post["numOrden"];
                nuevo.fecha = fecha;
                nuevo.observacion = (string)post["comment"];
                nuevo.sub_total = (string)post["subtotal"];
                if ((string)post["con_iva"] == "true")
                {
                    nuevo.iva = "no aplica";
                    nuevo.total = (string)post["subtotal"];
                }
                else
                {
                    nuevo.iva = (string)post["iva"];
                    nuevo.total = (string)post["total"];
                }
                nuevo.id_proveedor = (string)post["proveedor"];
                orden_compra.agregarOrden(nuevo);
                string[] item = Request.Form.GetValues("item");
                string[] elementos = new string[item.Length];
                string valor = "";
                //  var elemento;
                for (int i = 0; i < item.Length; i++)
                {
                    var elemento = item[i].Split('|');
                    valor = elemento[1].ToString();
                    elementos[i] = valor;
                }
                string[] cantidad = Request.Form.GetValues("cantidad");
                string[] precio = Request.Form.GetValues("precio");
                for (int i = 0; i < precio.Length; i++)
                {
                    precio[i] = precio[i].Replace(".", "");
                }
                string[] unidad = Request.Form.GetValues("unidad");
                string[] id_partida = Request.Form.GetValues("partida_asociada");
                string[] id_faena = Request.Form.GetValues("id_faena");
                string[] numero_item_partida = Request.Form.GetValues("numero_item");
                string[] nombre_item_partida = Request.Form.GetValues("nombre_item");
                string[] id_partida_asig = Request.Form.GetValues("item_partida");
                string[] idSolicitud = Request.Form.GetValues("solicitud");

                orden_compra.agregardetalle(id, elementos, precio, cantidad, unidad, id_faena, id_partida, numero_item_partida, nombre_item_partida, idSolicitud);

                nota nueva = new nota();
                nueva.donde = (string)post["donde"];
                nueva.dir_entrega = (string)post["entrega"];
                nueva.id_numero_orden = (string)post["numOrden"];
                nueva.transporte = (string)post["transporte"];
                nueva.cuenta = (string)post["pago"];
                if((string)post["plazoentrega"] == "otros"){
                    nueva.plazo_entrega = (string)post["plazoentrega2"];
                }
                else {
                nueva.plazo_entrega = (string)post["plazoentrega"];
                }
                nota.agregarNota(nueva);

                usointerno_oc usointerno = new usointerno_oc();
                usointerno.id_orden = (string)post["numOrden"];
                usointerno.faena = (string)post["nombreobra"];
                usointerno.comuna = (string)post["comuna"];
                usointerno.observacion = (string)post["observacion"];
                usointerno.item = (string)post["item_uso"];
                usointerno.presupuesto = (string)post["presupuesto"];
                usointerno.oc = (string)post["oc"];
                usointerno.saldo = (string)post["saldo"];
                usointerno.revisadoPor = Session["nombre"].ToString();
                usointerno_oc.agregar_usoInterno(usointerno);
                return RedirectToAction("todas", "Ordendecompra");
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }
        public ActionResult todas()
        {
            if (Session["nombre"] != null)
            {
                List<orden_compra> ocompra = orden_compra.obtenerTodas();
                return View(ocompra);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }
        public ActionResult detalle(string id)
        {
            if (Session["nombre"] != null)
            {
                List<detalle_ordencompra> detail = detalle_ordencompra.obtenerTodas(id);
                return View(detail);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }
        public ActionResult GuardarNueva(FormCollection post)
        {
            if (Session["nombre"] != null && (Session["rol"].ToString().Equals("adquisiciones") || Session["rol"].ToString().Equals("admin")))
            {
                DateTime fecha = DateTime.Now;
                orden_compra nuevo = new orden_compra();
                string id = (string)post["numOrden"];
                nuevo.numero_orden = (string)post["numOrden"];
                nuevo.fecha = fecha;
                nuevo.observacion = (string)post["comment"];
                nuevo.sub_total = (string)post["subtotal"];
                if ((string)post["con_iva"] == "true")
                {
                    nuevo.iva = "no aplica";
                    nuevo.total = (string)post["subtotal"];
                }
                else
                {
                    nuevo.iva = (string)post["iva"];
                    nuevo.total = (string)post["total"];
                }
                nuevo.id_proveedor = (string)post["proveedor"];
                nuevo.id_remplazada = (string)post["numOrdenOLD"];
                orden_compra.agregarOrdenNueva(nuevo);
                string[] item = Request.Form.GetValues("item");
                string[] elementos = new string[item.Length];
                string valor = "";
                //  var elemento;
                for (int i = 0; i < item.Length; i++)
                {
                    var elemento = item[i].Split('|');
                    valor = elemento[1].ToString();
                    elementos[i] = valor;
                }
                string[] cantidad = Request.Form.GetValues("cantidad");
                string[] precio = Request.Form.GetValues("precio");

                for (int i = 0; i < precio.Length; i++) 
                {
                    precio[i] = precio[i].Replace(".","");
                }

                string[] unidad = Request.Form.GetValues("unidad");
                string[] id_partida = Request.Form.GetValues("partida_asociada");
                string[] id_faena = Request.Form.GetValues("id_faena");
                string[] numero_item_partida = Request.Form.GetValues("numero_item");
                string[] nombre_item_partida = Request.Form.GetValues("nombre_item");
                string[] id_partida_asig = Request.Form.GetValues("item_partida");
                string[] idSolicitud = Request.Form.GetValues("solicitud");

                orden_compra.agregardetalle(id, elementos, precio, cantidad, unidad, id_faena, id_partida, numero_item_partida, nombre_item_partida, idSolicitud);

                nota nueva = new nota();
                nueva.donde = (string)post["donde"];
                nueva.dir_entrega = (string)post["entrega"];
                nueva.id_numero_orden = (string)post["numOrden"];
                nueva.transporte = (string)post["transporte"];
                nueva.cuenta = (string)post["pago"];
                if ((string)post["plazoentrega"] == "otros")
                {
                    nueva.plazo_entrega = (string)post["plazoentrega2"];
                }
                else
                {
                    nueva.plazo_entrega = (string)post["plazoentrega"];
                }
                nota.agregarNota(nueva);

                usointerno_oc usointerno = new usointerno_oc();
                usointerno.id_orden = (string)post["numOrden"];
                usointerno.faena = (string)post["nombreobra"];
                usointerno.comuna = (string)post["comuna"];
                usointerno.observacion = (string)post["observacion"];
                usointerno.item = (string)post["item_uso"];
                usointerno.presupuesto = (string)post["presupuesto"];
                usointerno.oc = (string)post["oc"];
                usointerno.saldo = (string)post["saldo"];
                usointerno.revisadoPor = Session["nombre"].ToString();
                usointerno_oc.agregar_usoInterno(usointerno);
                return RedirectToAction("todas", "Ordendecompra");
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }
        public ActionResult Reemplazadas()
        {
            if (Session["nombre"] != null)
            {
                List<orden_compra> ocompra = orden_compra.obtenerTodasReemplazadas();
                return View(ocompra);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }
    }
}
