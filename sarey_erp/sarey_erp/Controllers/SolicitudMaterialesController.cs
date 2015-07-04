using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using sarey_erp.Models;
using System.Data;
using System.Data.SqlClient;


namespace sarey_erp.Controllers
{
    public class SolicitudMaterialesController : Controller
    {
        //
        // GET: /SolicitudMateriales/

        public ActionResult Index()
        {
            return View();
        }
        public ActionResult revisar(string id_solicitud) {
            if (Session["nombre"] != null)
            {
                solicitud_materiales Solicitud = new solicitud_materiales();
                solicitud_materiales TempSolicitud = new solicitud_materiales();
                faena laFaena = new faena();

                Solicitud.id_solicitud = id_solicitud;
                TempSolicitud = Solicitud.obtenerSolicitud();
                ViewData["unidad"] = items.obtenerTodasUnidades();
                ViewData["solicitud"] = TempSolicitud;//Solicitud.obtenerSolicitud();
                ViewData["itemSolicitud"] = Solicitud.ObtenerItemsDeSolicitudMateriales();
                //ViewData["items"] = items.obtenerTodos();               

                ViewBag.rolSession=Session["rol"].ToString();
                ViewBag.IDFAENA = TempSolicitud.id_faena;
                ViewBag.fecha_solicitud = TempSolicitud.fecha.ToString("d");
                ViewBag.solicitante = TempSolicitud.nombre_solicitante;
                ViewBag.nombre_revision_oficina = TempSolicitud.nombre_revision_oficina;
                ViewBag.fecha_revision_oficina = TempSolicitud.fecha_revision_oficina.ToString("d");
                laFaena.nombre = TempSolicitud.id_faena;

                ViewData["partidasFaena"] = new faena().obtenerPartidas(TempSolicitud.id_faena);
                ViewBag.Presupuesto = Solicitud.obtenerPresupuestoPartidaFaenaSolicitud();                                
                return View();
            }
            else {
                return RedirectToAction("Index", "Home");
            }
        }
        public ActionResult todos() {
            if (Session["nombre"] != null)
            {
              


                solicitud_materiales Solicitudes = new solicitud_materiales();
                //ViewData["solicitudes"]=todasSolicitudes.obtenerTodasSolicitudes();
                List<solicitud_materiales> listaSolicitudes = new List<solicitud_materiales>();
                listaSolicitudes = solicitud_materiales.obtenerTodasSolicitudes();
                return View(listaSolicitudes);
            }
            else {
                return RedirectToAction("Index", "Home");
            }
        }
        public ActionResult pendientes()
        {
            if (Session["nombre"] != null)
            {
                solicitud_materiales Solicitudes = new solicitud_materiales();
                //ViewData["solicitudes"]=todasSolicitudes.obtenerTodasSolicitudes();
                List<solicitud_materiales> listaSolicitudes = new List<solicitud_materiales>();
                listaSolicitudes = solicitud_materiales.obtenerTodasSolicitudesPendientes();
                ViewBag.pendiente = "Pendientes";
                return View("todos",listaSolicitudes);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }


        public ActionResult nueva()
        {
            if (Session["nombre"] != null && (Session["rol"].ToString().Equals("adquisiciones") || Session["rol"].ToString().Equals("admin") || Session["rol"].ToString().Equals("supervisorObra")))
            {
                DateTime fecha_actual = DateTime.Today;
                // ViewData["items"] = items.obtenerTodos();
                ViewData["unidad"] = items.obtenerTodasUnidades();
                ViewData["faenas"] = faena.obtenerTodas();
                ViewBag.NombreUser = Session["nombre"];
                ViewBag.NumeroSolicitud = solicitud_materiales.numeroNuevaSolicitud();
                ViewBag.Fecha = fecha_actual.ToString("d");
                ViewBag.rolSession = Session["rol"].ToString();
                return View();
            }
            else {
                return RedirectToAction("Index", "Home");
            }
        }

        
        public ActionResult enviado()
        {
            return View("enviado");
        }

      


        public ActionResult Guardar(FormCollection form)
        {
            if (Session["nombre"] != null && (Session["rol"].ToString().Equals("adquisiciones") || Session["rol"].ToString().Equals("admin") || Session["rol"].ToString().Equals("supervisorObra")))
            {
                DateTime fecha_actual = DateTime.Today;
                //string id_faenaForm = (string)form["id_faenaForm"];
                //string numero_solicitudForm = (string)form["numero_solicitudForm"];
                //string nombre_solicitante="usuario";

                solicitud_materiales NuevaSolicitudMateriales = new solicitud_materiales();
                NuevaSolicitudMateriales.id_faena = (string)form["id_faenaForm"];
                NuevaSolicitudMateriales.id_solicitud = (string)form["numero_solicitudForm"];
                NuevaSolicitudMateriales.nombre_solicitante = Convert.ToString(Session["nombre"]);
                NuevaSolicitudMateriales.fecha = fecha_actual;
                NuevaSolicitudMateriales.numero_folio=(string)form["numero_folio"];

                NuevaSolicitudMateriales.agregarNuevaSolicitud();

                string[] nuevoItem = Request.Form.GetValues("nuevo_item");
                string[] cantidad = Request.Form.GetValues("cantidad");
                string[] unidad = Request.Form.GetValues("unidad");
                string[] nombre_numero_item = Request.Form.GetValues("item");
                string[] dimensiones = Request.Form.GetValues("dimensiones");
                string[] partida = Request.Form.GetValues("partida");

                NuevaSolicitudMateriales.insertarItemSolicitud(cantidad, unidad, nombre_numero_item, dimensiones, partida,nuevoItem);

                //string[] amounts = Request.Form.GetValues("cantidad");
                //string v = (string)form["v"];
                //string a = amounts[0];

                return RedirectToAction("todos", "SolicitudMateriales");
            }
            else {
                return RedirectToAction("Index", "Home");
            }
           // return View("enviado");
        }


        public ActionResult revisarSolicitud(FormCollection form)
        {
            if (Session["nombre"] != null && (Session["rol"].ToString().Equals("oficinaTecnica") || Session["rol"].ToString().Equals("admin")))
            {
                solicitud_materiales SolicitudMaterialesActualizada = new solicitud_materiales();

                SolicitudMaterialesActualizada.id_solicitud = (string)form["numero_solicitudForm"];
                SolicitudMaterialesActualizada.id_faena = (string)form["idFaena"];
                string[] cantidad = Request.Form.GetValues("cantidad");
                string[] item = Request.Form.GetValues("itemSolicitud");
                string[] partida = Request.Form.GetValues("partida");
                string[] unidad = Request.Form.GetValues("unidad");
                string[] item_partida = Request.Form.GetValues("item");
                string[] dimensiones = Request.Form.GetValues("dimensiones");

                string[] nueva_cantidad = Request.Form.GetValues("nueva_cantidad");
                string[] nuevo_item = Request.Form.GetValues("nuevo_itemSolicitud");
                string[] nueva_partida = Request.Form.GetValues("nueva_partida");
                string[] nueva_unidad = Request.Form.GetValues("nueva_unidad");
                string[] nueva_item_partida = Request.Form.GetValues("nuevo_item");
                string[] nueva_dimensiones = Request.Form.GetValues("nueva_dimensiones");

                SolicitudMaterialesActualizada.revisarSolicitud(Convert.ToString(Session["nombre"]), cantidad, item, unidad, partida,dimensiones, item_partida, nueva_cantidad, nuevo_item, nueva_partida, nueva_unidad, nueva_item_partida, nueva_dimensiones);

                //string[] amounts = Request.Form.GetValues("cantidad");
                //string v = (string)form["v"];
                //string a = amounts[0];

                return RedirectToAction("todos", "SolicitudMateriales");
            }
            else {
                return RedirectToAction("Index", "Home");            
            }
            //return View("todos");
        }



        public JsonResult obtenerPartidasFaena(string idfaena)
        {   

            faena Faena = new faena();
            Faena.nombre=idfaena;
            var partidas = new faena().obtenerPartidas(idfaena);
            return Json(partidas , JsonRequestBehavior.AllowGet);
        }



        public JsonResult obtenerItemsPartida(string id_faena, string id_partida) {

            List<items> itemsDePartida = items.obtenerItemsdePartida(id_faena, id_partida);
            /*itemsDePartida = juntarListasItems(itemsDePartida, items.obtenerItemsdePartida(id_faena, id_partida + ".1"));
            itemsDePartida = juntarListasItems(itemsDePartida, items.obtenerItemsdePartida(id_faena, id_partida + ".2"));
            itemsDePartida = juntarListasItems(itemsDePartida, items.obtenerItemsdePartida(id_faena, id_partida + ".3"));
            itemsDePartida = juntarListasItems(itemsDePartida, items.obtenerItemsdePartida(id_faena, id_partida + ".4"));
            itemsDePartida = juntarListasItems(itemsDePartida, items.obtenerItemsdePartida(id_faena, id_partida + ".5"));
            itemsDePartida = juntarListasItems(itemsDePartida, items.obtenerItemsdePartida(id_faena, id_partida + ".6"));
            itemsDePartida = juntarListasItems(itemsDePartida, items.obtenerItemsdePartida(id_faena, id_partida + ".7"));
            itemsDePartida = juntarListasItems(itemsDePartida, items.obtenerItemsdePartida(id_faena, id_partida + ".8"));
            itemsDePartida = juntarListasItems(itemsDePartida, items.obtenerItemsdePartida(id_faena, id_partida + ".9"));//*/
            var Itemspartida = itemsDePartida;
            return Json(Itemspartida, JsonRequestBehavior.AllowGet);
        }

        List<items> juntarListasItems(List<items> lista1, List<items> lista2) 
        {
            for (int i = 0; i < lista2.Count; i++) 
            {
                lista1.Add(lista2[i]);
            }
            return lista1;
        }



        public string existeNumeroFolio(string numeroFolio)
        {

            bool retorno = false;

            SqlConnection cnx = conexion.crearConexion();
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = cnx;
            cmd.CommandText = "SELECT * FROM solicitudes_materiales WHERE numero_folio='" + numeroFolio + "'";
            cmd.CommandType = CommandType.Text;

            SqlDataReader dr = cmd.ExecuteReader();

            retorno = dr.HasRows;

            cnx.Close();

            return Convert.ToString(retorno);

        }


        public string obtenerPresupuestoPartidaFaena(string idfaena)
        {

            faena Faena = new faena();
            Faena.nombre = idfaena;
            return Faena.obtenerPresupuestoPartida();
        }

        public string obtenerPresupuestoPartida(string idfaena,string idpartida)
        {
            string presupuesto = partida.obtenerPresupuesto(idfaena,idpartida);
            return presupuesto;
        }

        public string obtenerPresupuestoItem(string iditem,string idfaena, string idpartida)
        {
            string presupuestoItem = items.obtenerPresupuesto(iditem,idfaena, idpartida);
            return presupuestoItem;
        }
        
        public bool eliminarItemDeSolicitud(string item, string solicitud, string id_partida, string numero_item_partida)
        {

            solicitud_materiales Solicitud = new solicitud_materiales();
            bool retorno=Solicitud.eliminarItemdeSolicitud(item, solicitud,id_partida,numero_item_partida);
            return retorno; 
        }

        public JsonResult obtenerItem(string idItem,string id_partida,string id_faena)
        {

            items Item = new items();
            Item.nombre = idItem;
            Item=Item.obtenerItem(id_partida,id_faena);
            var partidas = Item;
            return Json(partidas, JsonRequestBehavior.AllowGet);
            
        }

    }
}
