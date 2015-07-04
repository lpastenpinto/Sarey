using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Web.Mvc;
using System.Data.SqlClient;
using sarey_erp.Models;

namespace sarey_erp.Models
{
    public class solicitud_materiales
    {
        public string id_solicitud { get; set; }
        public DateTime fecha { get; set; }
        public string nombre_solicitante { get; set; }
        public string id_faena { set; get; }
        public string nombre_recepcion { set; get; }
        public DateTime fecha_recepcion { set; get; }
        public string nombre_revision_oficina { set; get; }
        public DateTime fecha_revision_oficina { set; get; }
        public string nombre_gerencia { set; get; }
        public DateTime fecha_gerencia { set; get; }
        public string numero_folio { get; set; }


        string presupuesto_faena;

        public static List<solicitud_materiales> obtenerTodasSolicitudesPendientes()
        {

            List<solicitud_materiales> Solicitudes = new List<solicitud_materiales>();
            SqlConnection cnx = conexion.crearConexion();

            SqlCommand cmd = new SqlCommand();
            cmd.Connection = cnx;
            cmd.CommandText = "SELECT * FROM solicitudes_materiales WHERE revisado is null ORDER BY fecha ASC";
            cmd.CommandType = CommandType.Text;
            SqlDataReader dr = cmd.ExecuteReader();

            while (dr.Read())
            {
                solicitud_materiales Solicitud = new solicitud_materiales();

                Solicitud.id_solicitud = (string)dr["id_solicitud"];
                Solicitud.nombre_solicitante = (string)dr["nombre_solicitante"];
                Solicitud.fecha = (DateTime)dr["fecha"];

                try
                {
                    Solicitud.nombre_recepcion = (string)dr["nombre_recepcion"];
                }
                catch (Exception)
                {
                    Solicitud.nombre_recepcion = "";
                }

                try
                {
                    Solicitud.nombre_revision_oficina = (string)dr["nombre_revision_oficina"];
                }
                catch (Exception)
                {
                    Solicitud.nombre_revision_oficina = "";
                }


                try
                {
                    Solicitud.nombre_gerencia = (string)dr["nombre_gerencia"];
                }
                catch (Exception)
                {
                    Solicitud.nombre_gerencia = "";
                }

                //Solicitud.fecha_revision_oficina=(DateTime)dr["fecha_revision_oficina"];

                //Solicitud.fecha_gerencia = (DateTime)dr["fecha_gerencia"];
                //Solicitud.fecha_recepcion = (DateTime)dr["fecha_recepcion"];

                Solicitudes.Add(Solicitud);
            }
            cnx.Close();


            return Solicitudes;

        }

        public static List<solicitud_materiales> obtenerTodasSolicitudes()
        {

            List<solicitud_materiales> Solicitudes = new List<solicitud_materiales>();
            SqlConnection cnx = conexion.crearConexion();

            SqlCommand cmd = new SqlCommand();
            cmd.Connection = cnx;
            cmd.CommandText = "SELECT * FROM solicitudes_materiales ORDER BY fecha ASC,revisado";
            cmd.CommandType = CommandType.Text;
            SqlDataReader dr = cmd.ExecuteReader();

            while (dr.Read())
            {
                solicitud_materiales Solicitud = new solicitud_materiales();

                Solicitud.id_solicitud = (string)dr["id_solicitud"];
                Solicitud.nombre_solicitante = (string)dr["nombre_solicitante"];
                Solicitud.fecha = (DateTime)dr["fecha"];

                try
                {
                    Solicitud.nombre_recepcion = (string)dr["nombre_recepcion"];
                }
                catch (Exception)
                {
                    Solicitud.nombre_recepcion = "";
                }

                try
                {
                    Solicitud.nombre_revision_oficina = (string)dr["nombre_revision_oficina"];
                }
                catch (Exception)
                {
                    Solicitud.nombre_revision_oficina = "";
                }


                try
                {
                    Solicitud.nombre_gerencia = (string)dr["nombre_gerencia"];
                }
                catch (Exception)
                {
                    Solicitud.nombre_gerencia = "";
                }

                //Solicitud.fecha_revision_oficina=(DateTime)dr["fecha_revision_oficina"];

                //Solicitud.fecha_gerencia = (DateTime)dr["fecha_gerencia"];
                //Solicitud.fecha_recepcion = (DateTime)dr["fecha_recepcion"];

                Solicitudes.Add(Solicitud);
            }
            cnx.Close();


            return Solicitudes;

        }

        public static string numeroNuevaSolicitud()
        {

            List<faena> retorno = new List<faena>();
            SqlConnection cnx = conexion.crearConexion();

            SqlCommand cmd = new SqlCommand();
            cmd.Connection = cnx;
            cmd.CommandText = "SELECT COUNT(*) from solicitudes_materiales";
            cmd.CommandType = CommandType.Text;
            var countStart = (Int32)cmd.ExecuteScalar() + 1;
            cnx.Close();

            return countStart.ToString();

        }

        public void agregarNuevaSolicitud()
        {
            SqlConnection cnx = conexion.crearConexion();
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = cnx;

            cmd.CommandText = "insert into solicitudes_materiales(id_solicitud,fecha,nombre_solicitante,id_faena,numero_folio) values('"
               + id_solicitud + "',@fecha,'"
               + nombre_solicitante + "','"
                + id_faena + "','"
               + numero_folio + "')";

            cmd.Parameters.Add("@fecha", SqlDbType.DateTime).Value = fecha;

            cmd.CommandType = CommandType.Text;
            cmd.ExecuteNonQuery();
            cnx.Close();
        }


        public void insertarItemSolicitud(string[] cantidad, string[] unidad, string[] nombre_numero_item, string[] dimensiones, string[] partida, string[] nuevoItem)
        {

            SqlConnection cnx = conexion.crearConexion();
            

            for (int i = 0; i < cantidad.Length; i++)
            {
                SqlCommand cmdnew = new SqlCommand();
                cmdnew.Connection = cnx;

                string[] words = nombre_numero_item[i].Split('/');
                string numero_item_partida = words[0];
                string nombre_item_partida = words[1];



                cmdnew.CommandText = "insert into detalle_solicitud_materiales(id_solicitud,nombre_item,cantidad,id_partida,id_faena,unidad,numero_item_partida,nombre_item_partida,dimensiones) values(@id_solicitud,@nombre_item,@cantidad,@id_partida,@id_faena,@unidad,@numero_item_partida,@nombre_item_partida,@dimensiones)";
                
               
                cmdnew.Parameters.Add("@id_solicitud", SqlDbType.VarChar).Value =id_solicitud;
                cmdnew.Parameters.Add("@nombre_item", SqlDbType.VarChar).Value =nuevoItem[i];
                cmdnew.Parameters.Add("@cantidad", SqlDbType.Float).Value =cantidad[i];
                cmdnew.Parameters.Add("@id_partida", SqlDbType.VarChar).Value =partida[i];
                cmdnew.Parameters.Add("@id_faena", SqlDbType.VarChar).Value =id_faena;
                cmdnew.Parameters.Add("@unidad", SqlDbType.VarChar).Value =unidad[i];
                cmdnew.Parameters.Add("@numero_item_partida", SqlDbType.VarChar).Value =numero_item_partida;
                cmdnew.Parameters.Add("@nombre_item_partida", SqlDbType.VarChar).Value =nombre_item_partida;
                cmdnew.Parameters.Add("@dimensiones", SqlDbType.VarChar).Value =dimensiones[i];
                                   
                cmdnew.CommandType = CommandType.Text;
                cmdnew.ExecuteNonQuery();


            }
            cnx.Close();
        }

        public void revisarSolicitud(string nombre_revisa, string[] cantidad, string[] item, string[] unidad, string[] partida, string[] dimensiones, string[] item_partida, string[] nueva_cantidad, string[] nuevo_item, string[] nueva_partida, string[] nueva_unidad, string[] nueva_item_partida, string[] nueva_dimensiones)
        {

            SqlConnection cnx = conexion.crearConexion();
            DateTime fecha_actual = DateTime.Today;


            SqlCommand cmd_ = new SqlCommand();
            cmd_.Connection = cnx;
            cmd_.CommandText = "UPDATE solicitudes_materiales SET nombre_recepcion=@nombre_recepcion, "
            + "fecha_recepcion=@fecha_recepcion,nombre_revision_oficina=@nombre_revision_oficina,fecha_revision_oficina=@fecha_revision_oficina,revisado=@revisado WHERE id_solicitud='" + id_solicitud + "'";

            cmd_.Parameters.Add("@nombre_recepcion", SqlDbType.VarChar).Value = nombre_revisa;
            cmd_.Parameters.Add("@fecha_recepcion", SqlDbType.DateTime).Value = fecha_actual;
            cmd_.Parameters.Add("@nombre_revision_oficina", SqlDbType.VarChar).Value = nombre_revisa;
            cmd_.Parameters.Add("@fecha_revision_oficina", SqlDbType.DateTime).Value = fecha_actual;
            cmd_.Parameters.Add("@revisado", SqlDbType.VarChar).Value = "true";

            cmd_.CommandType = CommandType.Text;
            cmd_.ExecuteNonQuery();

            for (int i = 0; i < cantidad.Length; i++)
            {
                try
                {
                    string[] words = item_partida[i].Split('/');
                    int numero_item_partida = Convert.ToInt32(words[0]);
                    string nombre_item_partida = words[1];

                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = cnx;
                    cmd.CommandText = "UPDATE detalle_solicitud_materiales SET cantidad=@cantidad, "
                    + "id_partida=@id_partida,unidad=@unidad,dimensiones=@dimensiones,numero_item_partida=@numero_item_partida,nombre_item_partida=@nombre_item_partida WHERE id_solicitud='" + id_solicitud + "' AND nombre_item='" + item[i] + "'";

                    cmd.Parameters.Add("@cantidad", SqlDbType.Float).Value = Convert.ToDouble(cantidad[i]);
                    cmd.Parameters.Add("@id_partida", SqlDbType.VarChar).Value = partida[i];
                    cmd.Parameters.Add("@unidad", SqlDbType.VarChar).Value = unidad[i];
                    cmd.Parameters.Add("@dimensiones", SqlDbType.VarChar).Value = dimensiones[i];
                    cmd.Parameters.Add("@numero_item_partida", SqlDbType.Int).Value = numero_item_partida;
                    cmd.Parameters.Add("@nombre_item_partida", SqlDbType.VarChar).Value = nombre_item_partida;

                    cmd.CommandType = CommandType.Text;
                    cmd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    Console.Write(ex);
                }
            }

            if (((nueva_cantidad != null) && (nueva_cantidad.Length > 0)))
            {
                for (int i = 0; i < nueva_cantidad.Length; i++)
                {
                    try
                    {
                        string[] words = nueva_item_partida[i].Split('/');
                        int numero_item_partida = Convert.ToInt32(words[0]);
                        string nombre_item_partida = words[1];
                        SqlCommand cmd = new SqlCommand();
                        cmd.Connection = cnx;
                        cmd.CommandText = "insert into detalle_solicitud_materiales(id_solicitud,nombre_item,cantidad,id_partida,id_faena,unidad,numero_item_partida,nombre_item_partida,dimensiones) values('"
                           + id_solicitud + "','"
                           + nuevo_item[i] + "','"
                           + Convert.ToDouble(nueva_cantidad[i]) + "','"
                           + nueva_partida[i] + "','"
                           + id_faena + "','"
                           + nueva_unidad[i] + "','"
                           + numero_item_partida + "','"
                           + nombre_item_partida + "','"
                           + nueva_dimensiones[i] + "')";

                        cmd.CommandType = CommandType.Text;
                        cmd.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        Console.Write(ex);
                    }

                }
            }
            cnx.Close();
        }

        public solicitud_materiales obtenerSolicitud()
        {

            solicitud_materiales Solicitud = new solicitud_materiales();

            DateTime fecha_vacia = new DateTime();

            SqlConnection cnx = conexion.crearConexion();

            SqlCommand cmd = new SqlCommand();
            cmd.Connection = cnx;
            cmd.CommandText = "SELECT * from solicitudes_materiales,faenas  WHERE  ( id_solicitud = '" + id_solicitud + "')";
            cmd.CommandType = CommandType.Text;
            SqlDataReader dr = cmd.ExecuteReader();

            while (dr.Read())
            {


                Solicitud.id_solicitud = (string)dr["id_solicitud"];
                Solicitud.nombre_solicitante = (string)dr["nombre_solicitante"];
                Solicitud.fecha = (DateTime)dr["fecha"];
                Solicitud.id_faena = (string)dr["id_faena"];
                Solicitud.presupuesto_faena = Convert.ToString((double)dr["partida_presupuesto"]);

                try
                {
                    Solicitud.nombre_recepcion = (string)dr["nombre_recepcion"];
                }
                catch (Exception)
                {
                    Solicitud.nombre_recepcion = "";
                }

                try
                {
                    Solicitud.nombre_revision_oficina = (string)dr["nombre_revision_oficina"];
                }
                catch (Exception)
                {
                    Solicitud.nombre_revision_oficina = "";
                }

                try
                {
                    Solicitud.nombre_gerencia = (string)dr["nombre_gerencia"];
                }
                catch (Exception)
                {
                    Solicitud.nombre_gerencia = "";
                }



                try
                {
                    Solicitud.fecha_revision_oficina = (DateTime)dr["fecha_revision_oficina"];
                }
                catch (Exception)
                {
                    Solicitud.fecha_revision_oficina = fecha_vacia;
                }

                try
                {
                    Solicitud.fecha_gerencia = (DateTime)dr["fecha_gerencia"];
                }
                catch (Exception)
                {
                    Solicitud.fecha_gerencia = fecha_vacia;
                }


                try
                {
                    Solicitud.fecha_recepcion = (DateTime)dr["fecha_recepcion"];
                }
                catch (Exception)
                {
                    Solicitud.fecha_recepcion = fecha_vacia;
                }
                try
                {
                    Solicitud.numero_folio = (string)dr["numero_folio"];
                }
                catch (Exception)
                {
                    Solicitud.numero_folio = "";
                }
                //Solicitud.fecha_revision_oficina=(DateTime)dr["fecha_revision_oficina"];

                //Solicitud.fecha_gerencia = (DateTime)dr["fecha_gerencia"];
                //Solicitud.fecha_recepcion = (DateTime)dr["fecha_recepcion"];                
            }
            cnx.Close();
            return Solicitud;
        }

        public List<itemSolicitudMateriales> ObtenerItemsDeSolicitudMateriales()
        {

            List<itemSolicitudMateriales> itemsTotales = new List<itemSolicitudMateriales>();

            SqlConnection cnx = conexion.crearConexion();

            SqlCommand cmd = new SqlCommand();
            cmd.Connection = cnx;
            cmd.CommandText = "SELECT * from detalle_solicitud_materiales WHERE  ( id_solicitud = '" + id_solicitud + "')";
            cmd.CommandType = CommandType.Text;
            SqlDataReader dr = cmd.ExecuteReader();

            while (dr.Read())
            {
                itemSolicitudMateriales item = new itemSolicitudMateriales();
                item.nombre_item = (string)dr["nombre_item"];

                item.cantidad = Convert.ToString((double)dr["cantidad"]);
                item.id_partida = (string)dr["id_partida"];
                item.unidad = (string)dr["unidad"];
                item.nombre_item_partida = (string)dr["nombre_item_partida"];
                item.numero_item_partida = (string)dr["numero_item_partida"];
                item.id_partida = (string)dr["id_partida"];
                item.id_faena = (string)dr["id_faena"];

                string id_partida = (string)dr["id_partida"];
                string id_faena = (string)dr["id_faena"];

                SqlConnection cnx_ = conexion.crearConexion();
                SqlCommand cmd_ = new SqlCommand();
                cmd_.Connection = cnx_;
                //cmd_.CommandText = "SELECT total from partidas WHERE  (partidas.id_partida = '" + id_partida + "' AND partidas.id_faena = '" + id_faena + "')";
               
                cmd_.CommandText = "SELECT total,presupuesto_compra from items,partidas WHERE  ( items.id_partida = '" + id_partida + "' AND items.id_faena = '" + id_faena + "' AND items.nombre = '" + item.nombre_item_partida + "' AND partidas.id_partida = '" + id_partida + "' AND partidas.id_faena = '" + id_faena + "')";
               
                SqlDataReader dr_ = cmd_.ExecuteReader();
                while (dr_.Read())
                {
                    item.presupuesto_item = Convert.ToString((int)dr_["presupuesto_compra"]);
                    item.presupuesto_partida = Convert.ToString((double)dr_["total"]);
                }
                cnx_.Close();

                try
                {
                    item.dimensiones = (string)dr["dimensiones"];
                }
                catch (Exception)
                {
                    item.dimensiones = "";
                }
                itemsTotales.Add(item);
            }
            cnx.Close();
            return itemsTotales;

        }

        public string obtenerPresupuestoPartidaFaenaSolicitud()
        {
            return presupuesto_faena;
        }

        public bool eliminarItemdeSolicitud(string item, string id_solicitud, string id_partida, string numero_item_partida)
        {
            double numero_item = Convert.ToInt32(numero_item_partida);
            SqlConnection cnx = conexion.crearConexion();

            SqlCommand cmd = new SqlCommand();
            cmd.Connection = cnx;
            cmd.CommandText = "DELETE FROM detalle_solicitud_materiales  WHERE  ( id_solicitud = '" + id_solicitud + "' AND nombre_item='" + item + "' AND id_partida='" + id_partida + "' AND numero_item_partida=" + numero_item + ")";
            cmd.CommandType = CommandType.Text;
            cmd.ExecuteNonQuery();
            cnx.Close();

            return true;
        }

        public static string[] obtener_items(string id)
        {
            string texto_item = "";
            SqlConnection cnx = conexion.crearConexion();

            SqlCommand cmd = new SqlCommand();
            cmd.Connection = cnx;
            cmd.CommandText = "select * from detalle_solicitud_materiales where id_solicitud='" + id + "'";
            cmd.CommandType = CommandType.Text;
            SqlDataReader dr = cmd.ExecuteReader();

            while (dr.Read())
            {
                texto_item += (string)dr["id_partida"] + "|" + (string)dr["nombre_item"] + "|" + (string)dr["unidad"] + "|" + (double)dr["cantidad"] + "|" + (string)dr["id_faena"] + "|" + (string)dr["nombre_item_partida"] + "|" + (int)dr["numero_item_partida"] + ";";
            }
            texto_item = texto_item.TrimEnd(';');
            string[] items = texto_item.Split(';');
            cnx.Close();
            return items;
        }

        public static string[] obtener_items_pendientes(string id)
        {

            solicitud_materiales sol = new solicitud_materiales();
            sol.id_solicitud = id;
            sol=sol.obtenerSolicitud();

            List<itemSolicitudMateriales> listaItems = sol.ObtenerItemsDeSolicitudMateriales();

            string texto_item = "";
            

            for(int i=0;i<listaItems.Count;i++)
            {
                if (!listaItems[i].fuePedido(id)) 
                {
                    texto_item += listaItems[i].id_partida + "|" + listaItems[i].nombre_item + "|" + listaItems[i].unidad + "|" + listaItems[i].cantidad + "|" + listaItems[i].id_faena + "|" + listaItems[i].nombre_item_partida + "|" + listaItems[i].numero_item_partida + ";";
                }                
            }
            texto_item = texto_item.TrimEnd(';');
            string[] items = texto_item.Split(';');
            if(items.Length==1 && items[0].Equals(""))items = new string[0];
            return items;
        }

        public static string[] obtener_asignada(string partida, string faena, string material)
        {
            string asignada = "";
            SqlConnection cnx = conexion.crearConexion();

            SqlCommand cmd = new SqlCommand();
            cmd.Connection = cnx;
            cmd.CommandText = "select * from items where id_partida='" + partida + "' and id_faena='" + faena + "'";
            cmd.CommandType = CommandType.Text;
            SqlDataReader dr = cmd.ExecuteReader();

            while (dr.Read())
            {
                asignada += (string)dr["nombre"] + ";";
            }

            cnx.Close();
            SqlConnection cnx2 = conexion.crearConexion();

            SqlCommand cmd2 = new SqlCommand();
            cmd2.Connection = cnx2;
            cmd2.CommandText = "select * from items where id_partida='" + partida + "' and id_faena='" + faena + "'and nombre='" + material + "'";
            cmd2.CommandType = CommandType.Text;
            SqlDataReader dr2 = cmd2.ExecuteReader();

            while (dr2.Read())
            {
                asignada += (string)dr2["nombre"] + ";";
            }

            cnx2.Close();

            asignada = asignada.TrimEnd(';');
            string[] items = asignada.Split(';');

            return items;
        }

        public static string obtener_valor_material(string partida, string faena, string material)
        {
            items elItem = new items();
            elItem.nombre = material;
            elItem = elItem.obtenerItem(partida, faena);

            string asignada = "";

            asignada = elItem.obtenerSaldo().ToString();

            return asignada;

        }
        public static List<solicitud_materiales> obtenerTodasSolicitudesRevisadas()
        {

            List<solicitud_materiales> Solicitudes = new List<solicitud_materiales>();
            SqlConnection cnx = conexion.crearConexion();

            SqlCommand cmd = new SqlCommand();
            cmd.Connection = cnx;
            cmd.CommandText = "SELECT * FROM solicitudes_materiales WHERE nombre_revision_oficina IS NOT NULL ORDER BY fecha ASC";
            cmd.CommandType = CommandType.Text;
            SqlDataReader dr = cmd.ExecuteReader();

            while (dr.Read())
            {
                solicitud_materiales Solicitud = new solicitud_materiales();

                Solicitud.id_solicitud = (string)dr["id_solicitud"];

                Solicitudes.Add(Solicitud);
            }
            cnx.Close();


            return Solicitudes;

        }

        public static List<solicitud_materiales> obtenerTodasSolicitudesRevisadasNoProcesadas()
        {

            List<solicitud_materiales> Solicitudes = new List<solicitud_materiales>();
            SqlConnection cnx = conexion.crearConexion();

            SqlCommand cmd = new SqlCommand();
            cmd.Connection = cnx;
            cmd.CommandText = "SELECT * FROM solicitudes_materiales WHERE nombre_revision_oficina IS NOT NULL ORDER BY fecha ASC";
            cmd.CommandType = CommandType.Text;
            SqlDataReader dr = cmd.ExecuteReader();

            while (dr.Read())
            {
                solicitud_materiales Solicitud = new solicitud_materiales();

                Solicitud.id_solicitud = (string)dr["id_solicitud"];
                Solicitud = Solicitud.obtenerSolicitud();
                List<itemSolicitudMateriales> detalleSol = Solicitud.ObtenerItemsDeSolicitudMateriales();
                bool agregar = false;

                for (int i = 0; i < detalleSol.Count; i++) 
                {
                    if (!detalleSol[i].fuePedido(Solicitud.id_solicitud))
                        agregar = true;
                }

                if (agregar) 
                {
                    Solicitudes.Add(Solicitud);
                }
            }
            cnx.Close();


            return Solicitudes;

        }
        public static string [] obtenerTodasSolicitudesFaenas(string faena)
        {
            string solicitudes = "";
            SqlConnection cnx = conexion.crearConexion();

            SqlCommand cmd = new SqlCommand();
            cmd.Connection = cnx;
            cmd.CommandText = "select * from solicitudes_materiales where id_faena='" + faena + "' and nombre_revision_oficina IS NOT NULL ORDER BY fecha ASC";
            cmd.CommandType = CommandType.Text;
            SqlDataReader dr = cmd.ExecuteReader();

            while (dr.Read())
            {
                solicitud_materiales Solicitud = new solicitud_materiales();

                Solicitud.id_solicitud = (string)dr["id_solicitud"];
                Solicitud = Solicitud.obtenerSolicitud();
                List<itemSolicitudMateriales> detalleSol = Solicitud.ObtenerItemsDeSolicitudMateriales();
                bool agregar = false;

                for (int i = 0; i < detalleSol.Count; i++) 
                {
                    if (!detalleSol[i].fuePedido(Solicitud.id_solicitud))
                        agregar = true;
                }

                if (agregar) 
                {
                 //   Solicitudes.Add(Solicitud);
                    solicitudes += Solicitud.id_solicitud + ";";
                }
                
                // solicitudes += (string)dr["id_solicitud"] + ";";
            }
            cnx.Close();
            solicitudes = solicitudes.TrimEnd(';');
            string[] items = solicitudes.Split(';');
            return items;
        }
    }

    public class itemSolicitudMateriales
    {
        public string nombre_item { set; get; }
        public string cantidad { set; get; }
        public string id_partida { set; get; }
        public string id_faena { set; get; }
        public string unidad { set; get; }
        public string dimensiones { set; get; }
        public string numero_item_partida { get; set; }
        public string nombre_item_partida { get; set; }
        public string presupuesto_partida { set; get; }
        public string presupuesto_item { set; get; }

        public bool fuePedido(string idSolicitud)
        {
            bool retorno = false;

            SqlConnection cnx = conexion.crearConexion();

            SqlCommand cmd = new SqlCommand();
            cmd.Connection = cnx;
            cmd.CommandText = "SELECT id_partida from detalle_orden_compra WHERE id_solicitud='" + idSolicitud
                + "' AND id_item='" + nombre_item + "' AND id_faena='" + id_faena + "'";
            cmd.CommandType = CommandType.Text;
            SqlDataReader dr = cmd.ExecuteReader();

            while (dr.Read())
            {
                if (dr[0].ToString().Split('.')[0].Equals(id_partida.Split('.')[0]))
                {
                    retorno = true;
                }
            }
            cnx.Close();

            return retorno;
        }
    }
}