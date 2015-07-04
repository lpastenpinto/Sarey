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
    public class detalleInventarioController : Controller
    {
        //
        // GET: /detalleInventario/

        public ActionResult Index()
        {
            List<detalleInventario> lista = detalleInventario.obtenerTodos();

            ViewBag.Faenas = detalleInventario.obtenerFaenas();
            ViewBag.Items = detalleInventario.obtenerItems();

            ViewBag.Faena = "";
            ViewBag.Item = "";

            return View(lista);
        }

        public ActionResult IndexFiltro(string faena, string item)
        {
            List<detalleInventario> lista  = new List<detalleInventario>();
            if (item == null)
            {
                lista = detalleInventario.obtenerTodosFaena(faena);
                ViewBag.Faena = faena;
                ViewBag.Item = "";

                ViewBag.Faenas = detalleInventario.obtenerFaenas();
                ViewBag.Items = detalleInventario.obtenerItemsFaena(faena);
            }
            else if (faena == null)
            {
                lista = detalleInventario.obtenerTodosItem(item);
                ViewBag.Faena = "";
                ViewBag.Item = item;

                ViewBag.Faenas = detalleInventario.obtenerFaenasItem(item);
                ViewBag.Items = detalleInventario.obtenerItems();
            }
            else if (item == null && faena == null)
            {
                RedirectToAction("Index");
            }
            else
            {
                lista = detalleInventario.obtenerTodosFaenaItem(faena, item);
                ViewBag.Faena = faena;
                ViewBag.Item = item;

                ViewBag.Faenas = detalleInventario.obtenerFaenasItem(item);
                ViewBag.Items = detalleInventario.obtenerItemsFaena(faena);
            }

            return View("Index",lista);
        }

        public ActionResult realizarTraspaso(FormCollection form)
        {
            string item = (string)form["item"];            
            string antiguaFaena = (string)form["anteriorFaena"];
            double cantidadExistenteItem = Convert.ToDouble((string)form["cantidadExistenteItem"]);
            string nuevaFaena = (string)form["faena"];
            double cantidadTraspasar = Convert.ToDouble((string)form["cantidadTraspasar"]);

            double diferencia = 0;

            SqlConnection cnx = conexion.crearConexion();

            if (detalleInventario.verificarSiExiste(nuevaFaena, item))
            {
                SqlCommand cmd_ = new SqlCommand();
                cmd_.Connection = cnx;
                cmd_.CommandText = "UPDATE detalle_inventario SET cantidad=@cantidad"
                  + " WHERE id_faena='" + nuevaFaena + "' AND nombre_item='" + item + "'";

                double cantidad_anterior = detalleInventario.obtenerCantidadItem(nuevaFaena, item);

                cmd_.Parameters.Add("@cantidad", SqlDbType.Float).Value = cantidadTraspasar + cantidad_anterior;
                cmd_.ExecuteNonQuery();
            }
            else {
               
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = cnx;
                cmd.CommandText = "insert into detalle_inventario(id_faena,cantidad,nombre_item) values('"
                   + nuevaFaena + "','"
                   + cantidadTraspasar + "','"
                   + item + "')";
                cmd.CommandType = CommandType.Text;
                cmd.ExecuteNonQuery();
               
            }
            diferencia = (cantidadExistenteItem - cantidadTraspasar);
            if (diferencia == 0)
            {
                SqlCommand cmd_ = new SqlCommand();
                cmd_.Connection = cnx;                          
                cmd_.CommandText = "DELETE FROM detalle_inventario WHERE (id_faena='" + antiguaFaena + "' AND nombre_item='" + item + "')";
                cmd_.CommandType = CommandType.Text;
                cmd_.ExecuteNonQuery();

            }
            else
            {

                SqlCommand cmd_ = new SqlCommand();
                cmd_.Connection = cnx;
                cmd_.CommandText = "UPDATE detalle_inventario SET cantidad=@cantidad "
                  + " WHERE id_faena='" + antiguaFaena + "' AND nombre_item='" + item + "'";

                cmd_.Parameters.Add("@cantidad", SqlDbType.Float).Value = diferencia;
                cmd_.ExecuteNonQuery();

            }
                        
            cnx.Close();

            return RedirectToAction("Index");
        }

        public ActionResult Traspaso(string nombreItem, string idFaena)
        {
            //ViewData["items"] = detalleInventario.obtenerTodos(); 
            ViewBag.item = nombreItem;
            ViewBag.faena = idFaena;
            detalleInventario detalleInv = detalleInventario.obtenerItem(nombreItem, idFaena);
            ViewBag.cantidadItem = detalleInv.cantidad;

            ViewData["faenas"] = faena.obtenerTodas();
            return View();
        }             
    }
}
