using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using sarey_erp.Models;
using System.Data.SqlClient;
using System.Data;

namespace sarey_erp.Controllers
{
    public class HojaRutaController : Controller
    {
        //
        // GET: /HojaRuta/

        public static int flag = 0;

        #region Vistas Hoja Ruta

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

                List<hojaRutaFactura> hojasDeRuta = new List<hojaRutaFactura>();

                if (Session["rol"].Equals("adquisiciones"))
                {
                     List<hojaRutaFactura> todas= hojaRutaFactura.obtenerTodas();
                     foreach (hojaRutaFactura hojaRuta in todas) 
                     {
                         if (hojaRuta.adquisiciones == null || hojaRuta.contabilidad==null)
                         {
                             hojasDeRuta.Add(hojaRuta);
                         }
                     }
                }
                else if (Session["rol"].Equals("contabilidad"))
                {
                    List<hojaRutaFactura> todas = hojaRutaFactura.obtenerTodas();
                    foreach (hojaRutaFactura hojaRuta in todas)
                    {
                        if ((hojaRuta.contabilidad == null || hojaRuta.finanzas == null) && hojaRuta.adquisiciones!=null)
                        {
                            hojasDeRuta.Add(hojaRuta);
                        }
                    }
                }

                else if (Session["rol"].Equals("contabilidad"))
                {
                    List<hojaRutaFactura> todas = hojaRutaFactura.obtenerTodas();
                    foreach (hojaRutaFactura hojaRuta in todas)
                    {
                        if ((hojaRuta.finanzas == null || hojaRuta.retiraPago == null) && hojaRuta.contabilidad != null)
                        {
                            hojasDeRuta.Add(hojaRuta);
                        }
                    }
                }

                else if (Session["rol"].Equals("secretario(a)"))
                {
                    List<hojaRutaFactura> todas = hojaRutaFactura.obtenerTodas();
                    foreach (hojaRutaFactura hojaRuta in todas)
                    {
                        if (hojaRuta.finanzas != null)
                        {
                            hojasDeRuta.Add(hojaRuta);
                        }
                    }
                }
                else {
                    hojasDeRuta = hojaRutaFactura.obtenerTodas();
                }
                

                return View(hojasDeRuta);
            }
            else
            {
                return RedirectToAction("login", "Home");
            }
        }

        public ActionResult atrasadas()
        {
            if (Session["rol"] != null)
            {
                List<hojaRutaFactura> hojasDeRuta = new List<hojaRutaFactura>();

                if (Session["rol"].Equals("adquisiciones"))
                {
                     List<hojaRutaFactura> todas= hojaRutaFactura.obtenerTodas();
                     foreach (hojaRutaFactura hojaRuta in todas) 
                     {
                         if (hojaRuta.adquisiciones == null || hojaRuta.contabilidad==null)
                         {
                             hojasDeRuta.Add(hojaRuta);
                         }
                     }
                }
                else if (Session["rol"].Equals("contabilidad"))
                {
                    List<hojaRutaFactura> todas = hojaRutaFactura.obtenerTodas();
                    foreach (hojaRutaFactura hojaRuta in todas)
                    {
                        if ((hojaRuta.contabilidad == null || hojaRuta.finanzas == null) && hojaRuta.adquisiciones!=null)
                        {
                            hojasDeRuta.Add(hojaRuta);
                        }
                    }
                }

                else if (Session["rol"].Equals("contabilidad"))
                {
                    List<hojaRutaFactura> todas = hojaRutaFactura.obtenerTodas();
                    foreach (hojaRutaFactura hojaRuta in todas)
                    {
                        if ((hojaRuta.finanzas == null || hojaRuta.retiraPago == null) && hojaRuta.contabilidad != null)
                        {
                            hojasDeRuta.Add(hojaRuta);
                        }
                    }
                }

                else if (Session["rol"].Equals("secretario(a)"))
                {
                    List<hojaRutaFactura> todas = hojaRutaFactura.obtenerTodas();
                    foreach (hojaRutaFactura hojaRuta in todas)
                    {
                        if (hojaRuta.finanzas != null)
                        {
                            hojasDeRuta.Add(hojaRuta);
                        }
                    }
                }
                else {
                    hojasDeRuta = hojaRutaFactura.obtenerTodas();
                }

                List<hojaRutaFactura> hojasDeRutaAtrasadas = new List<hojaRutaFactura>();

                for (int i = 0; i < hojasDeRuta.Count; i++) 
                {
                    if ((DateTime.Now - hojasDeRuta[i].fechaRecepcionFactura).TotalDays > 3) 
                    {
                        hojasDeRutaAtrasadas.Add(hojasDeRuta[i]);
                    }
                }

                    return View("Index", hojasDeRutaAtrasadas);
            }
            else
            {
                return RedirectToAction("login", "Home");
            }
        }

        public ActionResult general(string idOrdenCompra, string numeroFactura)
        {
            if (Session["rol"] != null)
            {
                if (hojaRutaFactura.existeHojaRuta(idOrdenCompra, numeroFactura))
                {
                    hojaRutaFactura hojaRuta = hojaRutaFactura.obtenerHojaRuta(idOrdenCompra, numeroFactura);

                    return View(hojaRuta);
                }
                else return RedirectToAction("Index");
            }
            else
            {
                return RedirectToAction("login", "Home");
            }
        }

        public ActionResult nueva(string idOrdenCompra, string numeroFactura) 
        {
            if (Session["rol"] != null
                && (Session["rol"].ToString().Equals("admin") || Session["rol"].ToString().Equals("adquisiciones")))
            {
                if (!hojaRutaFactura.existeHojaRuta(idOrdenCompra, numeroFactura))
                {
                    hojaRutaFactura nueva = hojaRutaFactura.obtenerHojaRuta(idOrdenCompra, numeroFactura);

                    ViewBag.productos = obtenerProductosOrdenCompra(idOrdenCompra);
                    ViewBag.proveedor = hojaRutaFactura.obtenerProveedorNuevaHojaRuta(idOrdenCompra);
                    ViewBag.centroCosto = hojaRutaFactura.obtenerCentroCostoNuevaHojaRuta(idOrdenCompra);

                    return View(nueva);
                }
                else return RedirectToAction("editar", "HojaRuta", new { idOrdenCompra=idOrdenCompra });
            }
            else
            {
                return RedirectToAction("login", "Home");
            }
        }

        public ActionResult editar(string idOrdenCompra, string numeroFactura)
        {
            if (Session["rol"] != null
                && (Session["rol"].ToString().Equals("admin") || Session["rol"].ToString().Equals("adquisiciones")))
            {
                if (hojaRutaFactura.existeHojaRuta(idOrdenCompra, numeroFactura))
                {
                    hojaRutaFactura editada = hojaRutaFactura.obtenerHojaRuta(idOrdenCompra, numeroFactura);

                    ViewBag.proveedor = hojaRutaFactura.obtenerProveedorNuevaHojaRuta(idOrdenCompra);
                    ViewBag.centroCosto = hojaRutaFactura.obtenerCentroCostoNuevaHojaRuta(idOrdenCompra);
                    ViewBag.rutProveedor = obtenerDatosProveedor(editada.idProveedor);

                    List<string> nuevosProductos = new List<string>();
                    List<string> antiguosProductos = obtenerProductosOrdenCompra(idOrdenCompra);

                    for (int i = 0; i < editada.detalle.Count; i++)
                    {

                        double cantidad = editada.detalle[i].cantidad;
                        nuevosProductos.Add(editada.detalle[i].nombreItem + ";" + antiguosProductos[i].Split(';')[1] + ";" + antiguosProductos[i].Split(';')[2] + ";" + antiguosProductos[i].Split(';')[3] + ";" 
                        + cantidad);
                    }

                    ViewBag.productos = nuevosProductos;

                    return View(editada);
                }
                else
                {
                    return RedirectToAction("login", "Home");
                }                
            }
            else
            {
                return RedirectToAction("login", "Home");
            }
        }

        #endregion

        #region metodos HojaRuta

        [HttpPost]
        public ActionResult agregarHojaRuta(FormCollection post) 
        {
            if (Session["rol"] != null 
                && (Session["rol"].ToString().Equals("admin") || Session["rol"].ToString().Equals("adquisiciones")))
            {
                hojaRutaFactura nueva = hojaRutaFactura.obtenerHojaRuta(post["numeroOrdenCompra"].ToString(), post["numeroFactura"].ToString());

                nueva.idProveedor = post["proveedor"].ToString();
                nueva.numeroFactura = post["numeroFactura"].ToString();

                string fecha = post["fechaEntrega"].ToString();

                int año = int.Parse(fecha.Split('/')[2]);
                int mes = int.Parse(fecha.Split('/')[1]);
                int dia = int.Parse(fecha.Split('/')[0]);

                DateTime fechaRecepcionFactura = new DateTime(año, mes, dia, 0, 0, 0);

                nueva.fechaRecepcionFactura = fechaRecepcionFactura;
                nueva.nombreQuienEntrega = post["nombreQuienEntrega"].ToString();
                nueva.formaPago = post["formaPago"].ToString();
                nueva.idCentroCosto = post["centroCostos"].ToString();

                nueva.detalle = new List<detalleHojaRutaFactura>();
                string[] cantidadEntregada = Request.Form.GetValues("cantidadEntregada");
                string[] registrar = Request.Form.GetValues("guardarInventario");

                List<detalle_ordencompra> detalleOC = detalle_ordencompra.obtenerTodas(nueva.numeroOrdenCompra);
                for (int i = 0; i < detalleOC.Count; i++) 
                {
                    detalleHojaRutaFactura temp = new detalleHojaRutaFactura();

                    temp.nombreItem = detalleOC[i].id_item;
                    temp.idPartida = detalleOC[i].id_partida;
                    temp.idFaena = detalleOC[i].id_faena;
                    temp.numeroItemPartida = detalleOC[i].numero_item_partida.ToString();
                    temp.nombreItemPartida = detalleOC[i].nombre_item_partida;
                    temp.cantidad = int.Parse(cantidadEntregada[i]);

                    nueva.detalle.Add(temp);
                    
                    if (registrar[i].Equals("true")) 
                    {
                        detalleInventario detalle = new detalleInventario();
                        detalle.idFaena = faena.obtenerNombreFaena(nueva.idCentroCosto);
                        detalle.cantidad = temp.cantidad;
                        detalle.nombreItem = temp.nombreItem;
                        detalleInventario.agregarDetalle(detalle);
                    }
                }

                hojaRutaFactura.agregarNueva(nueva);

                flag = 2; //Agregado correctamente

                return RedirectToAction("Index");
            }
            else
            {
                return RedirectToAction("login", "Home");
            }
        }

        [HttpPost]
        public ActionResult editarHojaRuta(FormCollection post)
        {
            if (Session["rol"] != null
                && (Session["rol"].ToString().Equals("admin") || Session["rol"].ToString().Equals("adquisiciones")))
            {
                hojaRutaFactura editada = hojaRutaFactura.obtenerHojaRuta(post["numeroOrdenCompra"].ToString(), post["numeroFactura"].ToString());

                editada.idProveedor = post["proveedor"].ToString();
                editada.numeroFactura = post["numeroFactura"].ToString();

                string fecha = post["fechaEntrega"].ToString();

                int año = int.Parse(fecha.Split('/')[2]);
                int mes = int.Parse(fecha.Split('/')[1]);
                int dia = int.Parse(fecha.Split('/')[0]);

                DateTime fechaRecepcionFactura = new DateTime(año, mes, dia, 0, 0, 0);

                editada.fechaRecepcionFactura = fechaRecepcionFactura;
                editada.nombreQuienEntrega = post["nombreQuienEntrega"].ToString();
                editada.formaPago = post["formaPago"].ToString();
                editada.idCentroCosto = post["centroCostos"].ToString();

                editada.detalle = new List<detalleHojaRutaFactura>();
                string[] cantidadEntregada = Request.Form.GetValues("cantidadEntregada");

                List<detalle_ordencompra> detalleOC = detalle_ordencompra.obtenerTodas(editada.numeroOrdenCompra);


                for (int i = 0; i < detalleOC.Count; i++)
                {
                    detalleHojaRutaFactura temp = new detalleHojaRutaFactura();

                    temp.nombreItem = detalleOC[i].id_item;
                    temp.idPartida = detalleOC[i].id_partida;
                    temp.idFaena = detalleOC[i].id_faena;
                    temp.numeroItemPartida = detalleOC[i].numero_item_partida.ToString();
                    temp.nombreItemPartida = detalleOC[i].nombre_item_partida;
                    temp.cantidad = int.Parse(cantidadEntregada[i]);

                    editada.detalle.Add(temp);
                }
                string numeroFacturaAnterior = post["numeroFacturaAnterior"].ToString();
                hojaRutaFactura.editar(editada, numeroFacturaAnterior);

                flag = 4;//Editado con éxito

                return RedirectToAction("Index");
            }
            else
            {
                return RedirectToAction("login", "Home");
            }
        }

        #endregion

        #region Métodos Auxiliares

        public string existeFacturaProveedor(string factura, string proveedor) 
        {
            string retorno = "";

            if (Session["rol"] != null)
            {
                if(hojaRutaFactura.existeFactura(factura, proveedor))
                {
                    return "true";
                }
                else return "false";
            }

            return retorno;
        }

        public string obtenerDatosProveedor (string nombre)
        {
            string retorno = "";

            if (Session["rol"] != null)
            {
                SqlConnection cnx = conexion.crearConexion();

                SqlCommand cmd = new SqlCommand();
                cmd.Connection = cnx;
                cmd.CommandText = "SELECT rut from proveedores where nombre_proveedor='" + nombre + "'";
                cmd.CommandType = CommandType.Text;
                SqlDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    retorno = (string)dr["rut"];
                }

                cnx.Close();
            }

            return retorno;
        }

        public List<string> obtenerListaProveedores()
        {
            List<string> retorno = new List<string>();

            if (Session["rol"] != null)
            {
                SqlConnection cnx = conexion.crearConexion();

                SqlCommand cmd = new SqlCommand();
                cmd.Connection = cnx;
                cmd.CommandText = "SELECT nombre_proveedor from proveedores";
                cmd.CommandType = CommandType.Text;
                SqlDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    retorno.Add((string)dr["nombre_proveedor"]);
                }

                cnx.Close();
            }

            return retorno;
        }

        public List<string> obtenerListaCentrosCosto()
        {
            List<string> retorno = new List<string>();

            if (Session["rol"] != null)
            {
                SqlConnection cnx = conexion.crearConexion();

                SqlCommand cmd = new SqlCommand();
                cmd.Connection = cnx;
                cmd.CommandText = "SELECT id_centro_costos from centros_costos";
                cmd.CommandType = CommandType.Text;
                SqlDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    retorno.Add((string)dr["id_centro_costos"]);
                }

                cnx.Close();
            }

            return retorno;
        }

        public List<string> obtenerProductosOrdenCompra(string idOrdenCompra) 
        {
            List<string> retorno = new List<string>();

            if (Session["rol"] != null)
            {
                SqlConnection cnx = conexion.crearConexion();

                SqlCommand cmd = new SqlCommand();
                cmd.Connection = cnx;
                cmd.CommandText = "SELECT * "
                    + "FROM detalle_orden_compra "
                    + "WHERE id_orden_compra='" + idOrdenCompra + "'";
                cmd.CommandType = CommandType.Text;
                SqlDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    double cantidad = (double.Parse(((int)dr["cantidad"]).ToString()) - obtenerCantidadOrdenCompra(idOrdenCompra, (string)dr["id_item"], (string)dr["id_partida"], (string)dr["id_faena"], ((int)dr["numero_item_partida"]).ToString(), (string)dr["nombre_item_partida"]));
                    retorno.Add((string)dr["id_item"] + ";" + ((int)dr["precio_unitario"]).ToString() + ";" + (string)dr["unidad"] + ";" 
                        + cantidad);
                }

                cnx.Close();
            }

            return retorno;
        }

        public double obtenerCantidadOrdenCompra(string idOrdenCompra, string nombreItem, string idPartida, string idFaena, string numeroItemPartida, string nombreItemPartida)
        {
            double retorno = 0;

            if (Session["rol"] != null)
            {
                SqlConnection cnx = conexion.crearConexion();

                SqlCommand cmd = new SqlCommand();
                cmd.Connection = cnx;
                cmd.CommandText = "SELECT cantidad "
                    + "FROM detalle_hoja_ruta "
                    + "WHERE id_orden_compra='" + idOrdenCompra + "' AND nombre_item='" + nombreItem
                    + "' AND id_partida='" + idPartida + "' AND id_faena='" + idFaena
                    + "' AND numero_item_partida='" + numeroItemPartida + "' AND nombre_item_partida='" + nombreItemPartida + "'";
                cmd.CommandType = CommandType.Text;
                SqlDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    retorno+=double.Parse(((int)dr["cantidad"]).ToString());
                }

                cnx.Close();
            }

            return retorno;
        }

        #endregion

        #region Adquisiciones

        public ActionResult agregarAdquisiciones(string idOrdenCompra, string numeroFactura) 
        {
            if (Session["rol"] != null
                && (Session["rol"].ToString().Equals("admin") || Session["rol"].ToString().Equals("adquisiciones")))
            {
                if (hojaRutaFactura.existeHojaRuta(idOrdenCompra, numeroFactura))
                {
                    if (!hojaRutaFactura.adquisicionesHojaRuta.existenDatos(idOrdenCompra, numeroFactura))
                    {
                        hojaRutaFactura hojaRuta = hojaRutaFactura.obtenerHojaRuta(idOrdenCompra, numeroFactura);

                        ViewBag.fechaOrdenCompra = hojaRutaFactura.obtenerFechaOrdenCompra(idOrdenCompra);
                        ViewBag.valorOrdenCompra = hojaRutaFactura.obtenerValorOrdenCompra(idOrdenCompra);

                        return View(hojaRuta);
                    }
                    else 
                    {
                        return RedirectToAction("editarAdquisiciones", "HojaRuta", new { idOrdenCompra = idOrdenCompra });
                    }
                }
                else
                {
                    return RedirectToAction("Index");
                }
            }
            else
            {
                return RedirectToAction("login", "Home");
            }
        }

        public ActionResult editarAdquisiciones(string idOrdenCompra, string numeroFactura)
        {
            if (Session["rol"] != null && Session["rol"].ToString().Equals("admin"))
            {
                if (hojaRutaFactura.existeHojaRuta(idOrdenCompra, numeroFactura))
                {
                    if (hojaRutaFactura.adquisicionesHojaRuta.existenDatos(idOrdenCompra, numeroFactura))
                    {
                        hojaRutaFactura hojaRuta = hojaRutaFactura.obtenerHojaRuta(idOrdenCompra, numeroFactura);

                        return View(hojaRuta);
                    }
                    else 
                    {
                        return RedirectToAction("Index"); 
                    }                    
                }
                else
                {
                    return RedirectToAction("Index");
                }
            }
            else
            {
                return RedirectToAction("login", "Home");
            }
        }

        [HttpPost]
        public ActionResult agregarAdquisicion(FormCollection post) 
        {
            if (Session["rol"] != null
                && (Session["rol"].ToString().Equals("admin") || Session["rol"].ToString().Equals("adquisiciones")))
            {
                hojaRutaFactura nueva = hojaRutaFactura.obtenerHojaRuta(post["numeroOrdenCompra"].ToString(), post["numeroFactura"].ToString());

                nueva.adquisiciones = new hojaRutaFactura.adquisicionesHojaRuta();

                nueva.adquisiciones.observaciones = post["observaciones"];
                nueva.adquisiciones.usuario = usuarios.obtenerUsuario(Session["nombre"].ToString());

                hojaRutaFactura.adquisicionesHojaRuta.guardarDatos(nueva);

                //flag = 4; //Modificado correctamente

                return RedirectToAction("general", "HojaRuta", new { idOrdenCompra = nueva.numeroOrdenCompra, numeroFactura = nueva.numeroFactura });
            }
            else
            {
                return RedirectToAction("login", "Home");
            }
        }
        #endregion

        #region Finanzas

        public ActionResult agregarFinanzas(string idOrdenCompra, string numeroFactura)
        {

            if (Session["rol"] != null
                && (Session["rol"].ToString().Equals("admin") || Session["rol"].ToString().Equals("finanzas")))
            {
                if (hojaRutaFactura.existeHojaRuta(idOrdenCompra, numeroFactura))
                {
                    if (!hojaRutaFactura.finanzasHojaRuta.existenDatos(idOrdenCompra, numeroFactura))
                    {
                        hojaRutaFactura hojaRuta = hojaRutaFactura.obtenerHojaRuta(idOrdenCompra, numeroFactura);

                        return View(hojaRuta);
                    }
                    else 
                    {
                        return RedirectToAction("editarFinanzas", "HojaRuta", new { idOrdenCompra = idOrdenCompra });
                    }
                }
                else
                {
                    return RedirectToAction("Index");
                }
            }
            else
            {
                return RedirectToAction("login", "Home");
            }
        }

        public ActionResult editarFinanzas(string idOrdenCompra, string numeroFactura)
        {

            if (Session["rol"] != null
                && (Session["rol"].ToString().Equals("admin") || Session["rol"].ToString().Equals("finanzas")))
            {
                if (hojaRutaFactura.existeHojaRuta(idOrdenCompra, numeroFactura))
                {
                    if (hojaRutaFactura.finanzasHojaRuta.existenDatos(idOrdenCompra, numeroFactura))
                    {
                        hojaRutaFactura hojaRuta = hojaRutaFactura.obtenerHojaRuta(idOrdenCompra, numeroFactura);

                        return View(hojaRuta);
                    }
                    else
                    {
                        return RedirectToAction("Index");
                    }
                }
                else
                {
                    return RedirectToAction("Index");
                }
            }
            else
            {
                return RedirectToAction("login", "Home");
            }
        }

        [HttpPost]
        public ActionResult metodoAgregarFinanzas(FormCollection post) 
        {
            if (Session["rol"] != null
                && (Session["rol"].ToString().Equals("admin") || Session["rol"].ToString().Equals("finanzas")))
            {
                hojaRutaFactura nueva = hojaRutaFactura.obtenerHojaRuta(post["numeroOrdenCompra"].ToString(), post["numeroFactura"].ToString());

                nueva.finanzas = new hojaRutaFactura.finanzasHojaRuta();

                nueva.finanzas.numero = int.Parse(post["numero"].ToString());

                string fechaString = post["fecha"].ToString();

                int año = int.Parse(fechaString.Split('/')[2]);
                int mes = int.Parse(fechaString.Split('/')[1]);

                int dia = int.Parse(fechaString.Split('/')[0]);

                DateTime fecha = new DateTime(año, mes, dia);

                nueva.finanzas.fecha = fecha;

                nueva.finanzas.observaciones = post["observaciones"];
                nueva.finanzas.usuario = usuarios.obtenerUsuario(Session["nombre"].ToString());

                hojaRutaFactura.finanzasHojaRuta.guardarDatos(nueva);

                //flag = 4; //Modificado correctamente

                return RedirectToAction("general", "HojaRuta", new { idOrdenCompra = nueva.numeroOrdenCompra, numeroFactura = nueva.numeroFactura });
            }
            else
            {
                return RedirectToAction("login", "Home");
            }
        }        

        #endregion

        #region Contabilidad

        public ActionResult agregarContabilidad(string idOrdenCompra, string numeroFactura)
        {
            if (Session["rol"] != null
                && (Session["rol"].ToString().Equals("admin") || Session["rol"].ToString().Equals("contabilidad")))
            {
                if (hojaRutaFactura.existeHojaRuta(idOrdenCompra, numeroFactura))
                {
                    if (!hojaRutaFactura.contabilidadHojaRuta.existenDatos(idOrdenCompra, numeroFactura))
                    {
                        hojaRutaFactura hojaRuta = hojaRutaFactura.obtenerHojaRuta(idOrdenCompra, numeroFactura);

                        return View(hojaRuta);
                    }
                    else 
                    {
                        return RedirectToAction("editarContabilidad", "HojaRuta", new { idOrdenCompra = idOrdenCompra });
                    }
                }
                else
                {
                    return RedirectToAction("Index");
                }
            }
            else
            {
                return RedirectToAction("login", "Home");
            }
        }

        public ActionResult editarContabilidad(string idOrdenCompra, string numeroFactura)
        {
            if (Session["rol"] != null
                && (Session["rol"].ToString().Equals("admin") || Session["rol"].ToString().Equals("contabilidad")))
            {
                if (hojaRutaFactura.existeHojaRuta(idOrdenCompra, numeroFactura))
                {
                    if (hojaRutaFactura.contabilidadHojaRuta.existenDatos(idOrdenCompra, numeroFactura))
                    {
                        hojaRutaFactura hojaRuta = hojaRutaFactura.obtenerHojaRuta(idOrdenCompra, numeroFactura);

                        return View(hojaRuta);
                    }
                    else 
                    {
                        return RedirectToAction("Index");
                    }
                }
                else
                {
                    return RedirectToAction("Index");
                }
            }
            else
            {
                return RedirectToAction("login", "Home");
            }
        }

        [HttpPost]
        public ActionResult metodoAgregarContabilidad(FormCollection post)
        {
            if (Session["rol"] != null
                && (Session["rol"].ToString().Equals("admin") || Session["rol"].ToString().Equals("contabilidad")))
            {
                hojaRutaFactura nueva = hojaRutaFactura.obtenerHojaRuta(post["numeroOrdenCompra"].ToString(), post["numeroFactura"].ToString());

                nueva.contabilidad = new hojaRutaFactura.contabilidadHojaRuta();

                nueva.contabilidad.registroNubox = post["registroNubox"];

                nueva.contabilidad.cuenta = post["cuentaContabilidad"];

                nueva.contabilidad.observaciones = post["observaciones"];
                nueva.contabilidad.usuario = usuarios.obtenerUsuario(Session["nombre"].ToString());

                hojaRutaFactura.contabilidadHojaRuta.guardarDatos(nueva);

                //flag = 4; //Modificado correctamente

                return RedirectToAction("general", "HojaRuta", new { idOrdenCompra = nueva.numeroOrdenCompra, numeroFactura = nueva.numeroFactura });
            }
            else
            {
                return RedirectToAction("login", "Home");
            }
        }

        #endregion

        #region RetiraPago

        public ActionResult agregarRetiraPago(string idOrdenCompra, string numeroFactura)
        {
            if (Session["rol"] != null
                && (Session["rol"].ToString().Equals("admin") || Session["rol"].ToString().Equals("secretario(a)")))
            {
                if (hojaRutaFactura.existeHojaRuta(idOrdenCompra, numeroFactura))
                {
                    if (!hojaRutaFactura.retiraPagoHojaRuta.existenDatos(idOrdenCompra, numeroFactura))
                    {
                        hojaRutaFactura hojaRuta = hojaRutaFactura.obtenerHojaRuta(idOrdenCompra, numeroFactura);

                        return View(hojaRuta);
                    }
                    else 
                    {
                        return RedirectToAction("", "HojaRuta", new { idOrdenCompra = idOrdenCompra });
                    }
                }
                else
                {
                    return RedirectToAction("Index");
                }
            }
            else
            {
                return RedirectToAction("login", "Home");
            }
        }

        public ActionResult editarRetiraPago(string idOrdenCompra, string numeroFactura)
        {
            if (Session["rol"] != null
                && (Session["rol"].ToString().Equals("admin") || Session["rol"].ToString().Equals("secretario(a)")))
            {
                if (hojaRutaFactura.existeHojaRuta(idOrdenCompra, numeroFactura))
                {
                    hojaRutaFactura hojaRuta = hojaRutaFactura.obtenerHojaRuta(idOrdenCompra, numeroFactura);

                    return View(hojaRuta);
                }
                else
                {
                    return RedirectToAction("Index");
                }
            }
            else
            {
                return RedirectToAction("login", "Home");
            }
        }

        [HttpPost]
        public ActionResult metodoAgregarRetiraPago(FormCollection post)
        {
            if (Session["rol"] != null
                && (Session["rol"].ToString().Equals("admin") || Session["rol"].ToString().Equals("secretario(a)")))
            {
                hojaRutaFactura nueva = hojaRutaFactura.obtenerHojaRuta(post["numeroOrdenCompra"].ToString(), post["numeroFactura"].ToString());

                nueva.retiraPago = new hojaRutaFactura.retiraPagoHojaRuta();

                nueva.retiraPago.nombre = post["nombre"];

                nueva.retiraPago.rut = post["rut"];

                string fechaString = post["fecha"].ToString();

                int año = int.Parse(fechaString.Split('/')[2]);
                int mes = int.Parse(fechaString.Split('/')[1]);

                int dia = int.Parse(fechaString.Split('/')[0]);

                DateTime fecha = new DateTime(año, mes, dia);

                nueva.retiraPago.fecha = fecha;

                nueva.retiraPago.observaciones = post["observaciones"];
                nueva.retiraPago.usuario = usuarios.obtenerUsuario(Session["nombre"].ToString());

                hojaRutaFactura.retiraPagoHojaRuta.guardarDatos(nueva);

                //flag = 4; //Modificado correctamente

                return RedirectToAction("general", "HojaRuta", new { idOrdenCompra = nueva.numeroOrdenCompra, numeroFactura = nueva.numeroFactura });
            }
            else
            {
                return RedirectToAction("login", "Home");
            }
        }

        #endregion
    }
}
