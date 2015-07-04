using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace sarey_erp.Models
{
    public class hojaRutaFactura
    {
        public string numeroOrdenCompra { get; set; }
        public string idProveedor { get; set; }
        public string nombreProvedor { get; set; }
        public string rutProveedor { get; set; }
        public string numeroFactura { get; set; }
        public DateTime fechaRecepcionFactura { get; set; }
        public string nombreQuienEntrega { get; set; }
        public string formaPago { get; set; }
        public string idCentroCosto { get; set; }
        public List<detalleHojaRutaFactura> detalle { get; set; }

        public adquisicionesHojaRuta adquisiciones { get; set; }
        public finanzasHojaRuta finanzas { get; set; }
        public contabilidadHojaRuta contabilidad { get; set; }
        public retiraPagoHojaRuta retiraPago { get; set; }

        public static hojaRutaFactura obtenerHojaRuta(string idOrdenCompra, string numeroFactura)
        {
            hojaRutaFactura retorno = new hojaRutaFactura();

            retorno.numeroOrdenCompra = idOrdenCompra;
            retorno.numeroFactura = numeroFactura;

            retorno.obtenerDatosGenerales();
            retorno.detalle = detalleHojaRutaFactura.obtenerDetalle(idOrdenCompra, numeroFactura);

            if (adquisicionesHojaRuta.existenDatos(retorno.numeroOrdenCompra, retorno.numeroFactura)) {                
                retorno.adquisiciones = adquisicionesHojaRuta.obtenerDatos(retorno.numeroOrdenCompra, retorno.numeroFactura);
            }

            if (finanzasHojaRuta.existenDatos(retorno.numeroOrdenCompra, retorno.numeroFactura))
            {
                retorno.finanzas = finanzasHojaRuta.obtenerDatos(retorno.numeroOrdenCompra, retorno.numeroFactura);
            }

            if (contabilidadHojaRuta.existenDatos(retorno.numeroOrdenCompra, retorno.numeroFactura))
            {
                retorno.contabilidad = contabilidadHojaRuta.obtenerDatos(retorno.numeroOrdenCompra, retorno.numeroFactura);
            }

            if (retiraPagoHojaRuta.existenDatos(retorno.numeroOrdenCompra, retorno.numeroFactura))
            {
                retorno.retiraPago = retiraPagoHojaRuta.obtenerDatos(retorno.numeroOrdenCompra, retorno.numeroFactura);
            }

            return retorno;
        }        

        public static bool existeHojaRuta(string idOrdenCompra, string numeroFactura)
        {
            bool retorno=false;

            SqlConnection cnx = conexion.crearConexion();

            SqlCommand cmd = new SqlCommand();
            cmd.Connection = cnx;
            cmd.CommandText = "SELECT * from hoja_ruta "
                +" WHERE id_orden_compra='"+ idOrdenCompra
                + "' AND numero_factura='" + numeroFactura + "'";
            cmd.CommandType = CommandType.Text;
            SqlDataReader dr = cmd.ExecuteReader();

            retorno = dr.HasRows;

            cnx.Close();

            return retorno;
        }

        public static List<hojaRutaFactura> obtenerTodas() {

            List<hojaRutaFactura> retorno = new List<hojaRutaFactura>();

            SqlConnection cnx = conexion.crearConexion();

            SqlCommand cmd = new SqlCommand();
            cmd.Connection = cnx;
            cmd.CommandText = "SELECT id_orden_compra, numero_factura, id_proveedor from hoja_ruta ORDER BY fecha_recepcion_factura DESC";
            cmd.CommandType = CommandType.Text;
            SqlDataReader dr = cmd.ExecuteReader();

            while (dr.Read())
            {

                hojaRutaFactura nueva = new hojaRutaFactura();

                nueva.numeroOrdenCompra = (string)dr["id_orden_compra"];
                nueva.numeroFactura = (string)dr["numero_factura"];
                nueva.idProveedor = (string)dr["id_proveedor"];

                retorno.Add(nueva);
            }

            cnx.Close();

            for (int i = 0; i < retorno.Count; i++) 
            {
                retorno[i].obtenerDatosGenerales();
                if (adquisicionesHojaRuta.existenDatos(retorno[i].numeroOrdenCompra, retorno[i].numeroFactura))
                {
                    retorno[i].adquisiciones = adquisicionesHojaRuta.obtenerDatos(retorno[i].numeroOrdenCompra, retorno[i].numeroFactura);
                }

                if (finanzasHojaRuta.existenDatos(retorno[i].numeroOrdenCompra, retorno[i].numeroFactura))
                {
                    retorno[i].finanzas = finanzasHojaRuta.obtenerDatos(retorno[i].numeroOrdenCompra, retorno[i].numeroFactura);
                }

                if (contabilidadHojaRuta.existenDatos(retorno[i].numeroOrdenCompra, retorno[i].numeroFactura))
                {
                    retorno[i].contabilidad = contabilidadHojaRuta.obtenerDatos(retorno[i].numeroOrdenCompra, retorno[i].numeroFactura);
                }

                if (retiraPagoHojaRuta.existenDatos(retorno[i].numeroOrdenCompra, retorno[i].numeroFactura))
                {
                    retorno[i].retiraPago = retiraPagoHojaRuta.obtenerDatos(retorno[i].numeroOrdenCompra, retorno[i].numeroFactura);
                }
            }

            return retorno;
        }

        public void obtenerDatosGenerales()
        {
            SqlConnection cnx = conexion.crearConexion();

            SqlCommand cmd = new SqlCommand();
            cmd.Connection = cnx;
            cmd.CommandText = "SELECT * from hoja_ruta "
                +"WHERE id_orden_compra='" + this.numeroOrdenCompra 
                + "' AND numero_factura='"+this.numeroFactura+"'";
            cmd.CommandType = CommandType.Text;
            SqlDataReader dr = cmd.ExecuteReader();

            while (dr.Read())
            {
                numeroFactura = (string)dr["numero_factura"];
                idProveedor = (string)dr["id_proveedor"];
                nombreProvedor = idProveedor;
                rutProveedor = obtenerRutProveedor(idProveedor);
                fechaRecepcionFactura = (DateTime)dr["fecha_recepcion_factura"];
                nombreQuienEntrega = (string)dr["nombre_quien_entrega_factura"];
                formaPago = (string)dr["forma_pago"];
                idCentroCosto = (string)dr["id_centro_costos"];
            }

            cnx.Close();
        }

        public static void agregarNueva(hojaRutaFactura nueva) 
        {
            SqlConnection cnx = conexion.crearConexion();

            SqlCommand cmd = new SqlCommand();
            cmd.Connection = cnx;
            cmd.CommandText = "INSERT INTO hoja_ruta VALUES(@idProveedor,@numeroFactura,@fechaRecepcionFactura,"
                + "@nombreQuienEntrega,@formaPago,@idCentroCostos,@idOrdenCompra)";

            cmd.Parameters.Add("@idProveedor", SqlDbType.VarChar).Value = nueva.idProveedor;
            cmd.Parameters.Add("@numeroFactura", SqlDbType.VarChar).Value = nueva.numeroFactura;
            cmd.Parameters.Add("@fechaRecepcionFactura", SqlDbType.DateTime).Value = nueva.fechaRecepcionFactura;
            cmd.Parameters.Add("@nombreQuienEntrega", SqlDbType.VarChar).Value = nueva.nombreQuienEntrega;
            cmd.Parameters.Add("@formaPago", SqlDbType.VarChar).Value = nueva.formaPago;
            cmd.Parameters.Add("@idCentroCostos", SqlDbType.VarChar).Value = nueva.idCentroCosto;
            cmd.Parameters.Add("@idOrdenCompra", SqlDbType.VarChar).Value = nueva.numeroOrdenCompra;

            cmd.CommandType = CommandType.Text;
            cmd.ExecuteNonQuery();
            cnx.Close();

            //Guardar Detalle:
            for (int i = 0; i < nueva.detalle.Count; i++) 
            {
                cnx = conexion.crearConexion();
                cmd = new SqlCommand();
                cmd.Connection = cnx;
                cmd.CommandText = "INSERT INTO detalle_hoja_ruta VALUES(@idOrdenCompra,@nombreItem,@idPartida,"
                    + "@idFaena,@numeroItemPartida,@nombreItemPartida,@numeroFactura,@cantidad)";

                cmd.Parameters.Add("@idOrdenCompra", SqlDbType.VarChar).Value = nueva.numeroOrdenCompra;
                cmd.Parameters.Add("@nombreItem", SqlDbType.VarChar).Value = nueva.detalle[i].nombreItem;
                cmd.Parameters.Add("@idPartida", SqlDbType.VarChar).Value = nueva.detalle[i].idPartida;
                cmd.Parameters.Add("@idFaena", SqlDbType.VarChar).Value = nueva.detalle[i].idFaena;
                cmd.Parameters.Add("@numeroItemPartida", SqlDbType.Int).Value = nueva.detalle[i].numeroItemPartida;
                cmd.Parameters.Add("@nombreItemPartida", SqlDbType.VarChar).Value = nueva.detalle[i].nombreItemPartida;
                cmd.Parameters.Add("@numeroFactura", SqlDbType.VarChar).Value = nueva.numeroFactura;
                cmd.Parameters.Add("@cantidad", SqlDbType.Int).Value = nueva.detalle[i].cantidad;

                cmd.CommandType = CommandType.Text;
                cmd.ExecuteNonQuery();
                cnx.Close();
            }
        }

        public static void editar(hojaRutaFactura editada, string numeroFacturaAnterior)
        {
            SqlConnection cnx = conexion.crearConexion();

            SqlCommand cmd = new SqlCommand();
            cmd.Connection = cnx;
            cmd.CommandText = "UPDATE hoja_ruta  SET id_proveedor=@idProveedor, numero_factura=@numeroFactura, "
                +"fecha_recepcion_factura=@fechaRecepcionFactura, nombre_quien_entrega_factura=@nombreQuienEntrega, "
                +"forma_pago=@formaPago, id_centro_costos=@idCentroCostos "
                +" WHERE id_orden_compra=@idOrdenCompra AND numero_factura='" + numeroFacturaAnterior + "'";

            cmd.Parameters.Add("@idProveedor", SqlDbType.VarChar).Value = editada.idProveedor;
            cmd.Parameters.Add("@numeroFactura", SqlDbType.VarChar).Value = editada.numeroFactura;
            cmd.Parameters.Add("@fechaRecepcionFactura", SqlDbType.VarChar).Value = editada.fechaRecepcionFactura;
            cmd.Parameters.Add("@nombreQuienEntrega", SqlDbType.VarChar).Value = editada.nombreQuienEntrega;
            cmd.Parameters.Add("@formaPago", SqlDbType.VarChar).Value = editada.formaPago;
            cmd.Parameters.Add("@idCentroCostos", SqlDbType.VarChar).Value = editada.idCentroCosto;
            cmd.Parameters.Add("@idOrdenCompra", SqlDbType.VarChar).Value = editada.numeroOrdenCompra;

            cmd.CommandType = CommandType.Text;
            cmd.ExecuteNonQuery();
            cnx.Close();

            //Borrar Detalle:
            cnx = conexion.crearConexion();

            cmd = new SqlCommand();
            cmd.Connection = cnx;
            cmd.CommandText = "DELETE FROM detalle_hoja_ruta WHERE numero_factura=@numeroFactura AND id_orden_compra=@idOrdenCompra";

            cmd.Parameters.Add("@numeroFactura", SqlDbType.VarChar).Value = editada.numeroFactura;
            cmd.Parameters.Add("@idOrdenCompra", SqlDbType.VarChar).Value = editada.numeroOrdenCompra;

            cmd.CommandType = CommandType.Text;
            cmd.ExecuteNonQuery();
            cnx.Close();

            //Guardar Detalle:
            for (int i = 0; i < editada.detalle.Count; i++)
            {
                cnx = conexion.crearConexion();
                cmd = new SqlCommand();
                cmd.Connection = cnx;
                cmd.CommandText = "INSERT INTO detalle_hoja_ruta VALUES(@idOrdenCompra,@nombreItem,@idPartida,"
                    + "@idFaena,@numeroItemPartida,@nombreItemPartida,@numeroFactura,@cantidad)";

                cmd.Parameters.Add("@idOrdenCompra", SqlDbType.VarChar).Value = editada.numeroOrdenCompra;
                cmd.Parameters.Add("@nombreItem", SqlDbType.VarChar).Value = editada.detalle[i].nombreItem;
                cmd.Parameters.Add("@idPartida", SqlDbType.VarChar).Value = editada.detalle[i].idPartida;
                cmd.Parameters.Add("@idFaena", SqlDbType.VarChar).Value = editada.detalle[i].idFaena;
                cmd.Parameters.Add("@numeroItemPartida", SqlDbType.Int).Value = editada.detalle[i].numeroItemPartida;
                cmd.Parameters.Add("@nombreItemPartida", SqlDbType.VarChar).Value = editada.detalle[i].nombreItemPartida;
                cmd.Parameters.Add("@numeroFactura", SqlDbType.VarChar).Value = editada.numeroFactura;
                cmd.Parameters.Add("@cantidad", SqlDbType.Int).Value = editada.detalle[i].cantidad;

                cmd.CommandType = CommandType.Text;
                cmd.ExecuteNonQuery();
                cnx.Close();
            }
        }

        public static string obtenerProveedorNuevaHojaRuta(string idOrdenCompra)
        {
            string retorno = "";

            SqlConnection cnx = conexion.crearConexion();

            SqlCommand cmd = new SqlCommand();
            cmd.Connection = cnx;
            cmd.CommandText = "SELECT id_proveedor from orden_de_compra "
                +"WHERE numero_orden_compra='" + idOrdenCompra + "'";
            cmd.CommandType = CommandType.Text;
            SqlDataReader dr = cmd.ExecuteReader();

            while (dr.Read())
            {
                retorno = (string)dr["id_proveedor"];
            }

            cnx.Close();

            return retorno;
        }

        public static List<string> obtenerCentroCostoNuevaHojaRuta(string idOrdenCompra)
        {
            List<string> retorno = new List<string>();

            SqlConnection cnx = conexion.crearConexion();

            SqlCommand cmd = new SqlCommand();
            cmd.Connection = cnx;
            cmd.CommandText = "SELECT centro_costo "
                + "FROM detalle_orden_compra, faenas "
                +"WHERE id_faena=nombre AND id_orden_compra='"+idOrdenCompra+"'";
            cmd.CommandType = CommandType.Text;
            SqlDataReader dr = cmd.ExecuteReader();

            while (dr.Read())
            {
                if (!retorno.Contains((string)dr["centro_costo"]))
                    retorno.Add((string)dr["centro_costo"]);
            }

            cnx.Close();

            return retorno;
        }

        public string obtenerRutProveedor(string idProveedor) 
        {
            string retorno = "";

            SqlConnection cnx = conexion.crearConexion();

            SqlCommand cmd = new SqlCommand();
            cmd.Connection = cnx;
            cmd.CommandText = "SELECT rut from proveedores where nombre_proveedor='" + idProveedor + "'";
            cmd.CommandType = CommandType.Text;
            SqlDataReader dr = cmd.ExecuteReader();

            while (dr.Read())
            {
                retorno = (string)dr["rut"];
            }

            cnx.Close();
            
            return retorno;
        }

        public static DateTime obtenerFechaOrdenCompra(string idOrdenCompra){

            DateTime retorno = new DateTime();

            SqlConnection cnx = conexion.crearConexion();

            SqlCommand cmd = new SqlCommand();
            cmd.Connection = cnx;
            cmd.CommandText = "SELECT fecha from orden_de_compra where numero_orden_compra='" + idOrdenCompra + "'";
            cmd.CommandType = CommandType.Text;
            SqlDataReader dr = cmd.ExecuteReader();

            while (dr.Read())
            {
                retorno = (DateTime)dr["fecha"];
            }

            cnx.Close();

            return retorno;

        }

        public static string obtenerValorOrdenCompra(string idOrdenCompra){

            string retorno = "";

            SqlConnection cnx = conexion.crearConexion();

            SqlCommand cmd = new SqlCommand();
            cmd.Connection = cnx;
            cmd.CommandText = "SELECT total_pagar from orden_de_compra where numero_orden_compra='" + idOrdenCompra + "'";
            cmd.CommandType = CommandType.Text;
            SqlDataReader dr = cmd.ExecuteReader();

            while (dr.Read())
            {
                retorno = (string)dr["total_pagar"];
            }

            cnx.Close();

            return retorno;

        }

        public class adquisicionesHojaRuta {

            public DateTime fechaOrdenCompra { get; set; }
            public float valorOrdenCompra { get; set; }
            public string observaciones { get; set; }
            public usuarios usuario { get; set; }
            
            public static bool existenDatos(string idOrdenCompra, string numeroFactura){

                bool retorno = false;

                SqlConnection cnx = conexion.crearConexion();

                SqlCommand cmd = new SqlCommand();
                cmd.Connection = cnx;
                cmd.CommandText = "SELECT * from adquisiciones_hoja_ruta "
                    +"WHERE id_orden_compra='" + idOrdenCompra + "' AND numero_factura='"+numeroFactura+"'";
                cmd.CommandType = CommandType.Text;
                SqlDataReader dr = cmd.ExecuteReader();

                retorno = dr.HasRows;

                cnx.Close();

                return retorno;
            }

            public static adquisicionesHojaRuta obtenerDatos(string idOrdenCompra, string numeroFactura) {

                adquisicionesHojaRuta retorno = new adquisicionesHojaRuta();

                SqlConnection cnx = conexion.crearConexion();

                SqlCommand cmd = new SqlCommand();
                cmd.Connection = cnx;
                cmd.CommandText = "SELECT * from adquisiciones_hoja_ruta where id_orden_compra='" + idOrdenCompra + "' AND numero_factura='"+numeroFactura+"'";
                cmd.CommandType = CommandType.Text;
                SqlDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    retorno.obtenerDatosFactura((string)dr["id_orden_compra"]);
                    retorno.observaciones = (string)dr["observaciones"];
                    retorno.usuario = usuarios.obtenerUsuario((string)dr["id_usuario"]);
                }

                cnx.Close();
                
                return retorno;
            }

            private void obtenerDatosFactura(string idOrdenCompra)
            {
                SqlConnection cnx = conexion.crearConexion();

                SqlCommand cmd = new SqlCommand();
                cmd.Connection = cnx;
                cmd.CommandText = "SELECT * from orden_de_compra where numero_orden_compra='" + idOrdenCompra + "'";
                cmd.CommandType = CommandType.Text;
                SqlDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    this.fechaOrdenCompra = (DateTime)dr["fecha"];
                    this.valorOrdenCompra = float.Parse((string)dr["total_pagar"]);
                }

                cnx.Close();
            }

            public static void guardarDatos(hojaRutaFactura hojaRuta)
            {
                if (existenDatos(hojaRuta.numeroOrdenCompra, hojaRuta.numeroFactura))
                {
                    SqlConnection cnx = conexion.crearConexion();

                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = cnx;
                    cmd.CommandText = "UPDATE adquisiciones_hoja_ruta SET "
                        +"id_usuario=@idUsuario, observaciones=@observaciones WHERE id_orden_compra=@idOrdenCompra AND numero_factura=@numeroFactura";

                    cmd.Parameters.Add("@idOrdenCompra", SqlDbType.VarChar).Value = hojaRuta.numeroOrdenCompra;
                    cmd.Parameters.Add("@idUsuario", SqlDbType.VarChar).Value = hojaRuta.adquisiciones.usuario.nombre;
                    cmd.Parameters.Add("@observaciones", SqlDbType.VarChar).Value = hojaRuta.adquisiciones.observaciones;
                    cmd.Parameters.Add("@numeroFactura", SqlDbType.VarChar).Value = hojaRuta.numeroFactura;

                    cmd.CommandType = CommandType.Text;
                    cmd.ExecuteNonQuery();
                    cnx.Close();
                }
                else 
                {
                    SqlConnection cnx = conexion.crearConexion();

                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = cnx;
                    cmd.CommandText = "INSERT INTO adquisiciones_hoja_ruta VALUES(@idOrdenCompra,@idUsuario,@observaciones,@numeroFactura)";

                    cmd.Parameters.Add("@idOrdenCompra", SqlDbType.VarChar).Value = hojaRuta.numeroOrdenCompra;
                    cmd.Parameters.Add("@idUsuario", SqlDbType.VarChar).Value = hojaRuta.adquisiciones.usuario.nombre;
                    cmd.Parameters.Add("@observaciones", SqlDbType.VarChar).Value = hojaRuta.adquisiciones.observaciones;
                    cmd.Parameters.Add("@numeroFactura", SqlDbType.VarChar).Value = hojaRuta.numeroFactura;

                    cmd.CommandType = CommandType.Text;
                    cmd.ExecuteNonQuery();
                    cnx.Close();
                }
            }
        }

        public class finanzasHojaRuta
        {

            public int numero { get; set; }
            public DateTime fecha { get; set; }
            public string observaciones { get; set; }
            public usuarios usuario { get; set; }

            public static bool existenDatos(string idOrdenCompra, string numeroFactura)
            {
                bool retorno = false;

                SqlConnection cnx = conexion.crearConexion();

                SqlCommand cmd = new SqlCommand();
                cmd.Connection = cnx;
                cmd.CommandText = "SELECT * from finanzas_hoja_ruta where id_orden_compra='" + idOrdenCompra + "' AND numero_factura='"+numeroFactura+"'";
                cmd.CommandType = CommandType.Text;
                SqlDataReader dr = cmd.ExecuteReader();

                retorno = dr.HasRows;

                cnx.Close();

                return retorno;
            }

            public static finanzasHojaRuta obtenerDatos(string idOrdenCompra, string numeroFactura)
            {
                finanzasHojaRuta retorno = new finanzasHojaRuta();

                SqlConnection cnx = conexion.crearConexion();

                SqlCommand cmd = new SqlCommand();
                cmd.Connection = cnx;
                cmd.CommandText = "SELECT * from finanzas_hoja_ruta where id_orden_compra='" + idOrdenCompra + "' AND numero_factura='"+numeroFactura+"'";
                cmd.CommandType = CommandType.Text;
                SqlDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    retorno.numero = int.Parse((string)dr["numero"]);
                    retorno.fecha = (DateTime)dr["fecha"];
                    retorno.observaciones = (string)dr["observaciones"];
                    retorno.usuario = usuarios.obtenerUsuario((string)dr["id_usuario"]);
                }

                cnx.Close();

                return retorno;
            }

            public static void guardarDatos(hojaRutaFactura hojaRuta)
            {
                if (existenDatos(hojaRuta.numeroOrdenCompra, hojaRuta.numeroFactura))
                {
                    SqlConnection cnx = conexion.crearConexion();

                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = cnx;
                    cmd.CommandText = "UPDATE finanzas_hoja_ruta SET "
                        + "id_usuario=@idUsuario, observaciones=@observaciones, fecha=@fecha, numero=@numero" 
                        +" WHERE id_orden_compra=@idOrdenCompra AND numero_factura=@numeroFactura";

                    cmd.Parameters.Add("@idOrdenCompra", SqlDbType.VarChar).Value = hojaRuta.numeroOrdenCompra;
                    cmd.Parameters.Add("@idUsuario", SqlDbType.VarChar).Value = hojaRuta.finanzas.usuario.nombre;
                    cmd.Parameters.Add("@observaciones", SqlDbType.VarChar).Value = hojaRuta.finanzas.observaciones;
                    cmd.Parameters.Add("@fecha", SqlDbType.DateTime).Value = hojaRuta.finanzas.fecha;
                    cmd.Parameters.Add("@numero", SqlDbType.VarChar).Value = hojaRuta.finanzas.numero.ToString();
                    cmd.Parameters.Add("@numeroFactura", SqlDbType.VarChar).Value = hojaRuta.numeroFactura;
                    

                    cmd.CommandType = CommandType.Text;
                    cmd.ExecuteNonQuery();
                    cnx.Close();
                }
                else
                {
                    SqlConnection cnx = conexion.crearConexion();

                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = cnx;
                    cmd.CommandText = "INSERT INTO finanzas_hoja_ruta "
                        + " VALUES(@idOrdenCompra,@numero,@fecha,@observaciones,@idUsuario,@numeroFactura)";


                    cmd.Parameters.Add("@idOrdenCompra", SqlDbType.VarChar).Value = hojaRuta.numeroOrdenCompra;
                    cmd.Parameters.Add("@idUsuario", SqlDbType.VarChar).Value = hojaRuta.finanzas.usuario.nombre;
                    cmd.Parameters.Add("@observaciones", SqlDbType.VarChar).Value = hojaRuta.finanzas.observaciones;
                    cmd.Parameters.Add("@fecha", SqlDbType.DateTime).Value = hojaRuta.finanzas.fecha;
                    cmd.Parameters.Add("@numero", SqlDbType.VarChar).Value = hojaRuta.finanzas.numero.ToString();
                    cmd.Parameters.Add("@numeroFactura", SqlDbType.VarChar).Value = hojaRuta.numeroFactura;

                    cmd.CommandType = CommandType.Text;
                    cmd.ExecuteNonQuery();
                    cnx.Close();
                }
            }
        }

        public class contabilidadHojaRuta
        {

            public string registroNubox { get; set; }
            public string cuenta { get; set; }
            public string observaciones { get; set; }
            public usuarios usuario { get; set; }

            public static bool existenDatos(string idOrdenCompra, string numeroFactura)
            {
                bool retorno = false;

                SqlConnection cnx = conexion.crearConexion();

                SqlCommand cmd = new SqlCommand();
                cmd.Connection = cnx;
                cmd.CommandText = "SELECT * from contabilidad_hoja_ruta where id_orden_compra='" + idOrdenCompra + "'  AND numero_factura='"+numeroFactura+"'";
                cmd.CommandType = CommandType.Text;
                SqlDataReader dr = cmd.ExecuteReader();

                retorno = dr.HasRows;

                cnx.Close();

                return retorno;
            }

            public static contabilidadHojaRuta obtenerDatos(string idOrdenCompra, string numeroFactura)
            {
                contabilidadHojaRuta retorno = new contabilidadHojaRuta();

                SqlConnection cnx = conexion.crearConexion();

                SqlCommand cmd = new SqlCommand();
                cmd.Connection = cnx;
                cmd.CommandText = "SELECT * from contabilidad_hoja_ruta where id_orden_compra='" + idOrdenCompra + "' AND numero_factura='"+numeroFactura+"'";
                cmd.CommandType = CommandType.Text;
                SqlDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    retorno.registroNubox = (string)dr["registro_nubox"];
                    retorno.cuenta = (string)dr["cuenta_contabilidad"];
                    retorno.observaciones = (string)dr["observaciones"];
                    retorno.usuario = usuarios.obtenerUsuario((string)dr["id_usuario"]);
                }

                cnx.Close();

                return retorno;
            }

            public static void guardarDatos(hojaRutaFactura hojaRuta)
            {
                if (existenDatos(hojaRuta.numeroOrdenCompra, hojaRuta.numeroFactura))
                {
                    SqlConnection cnx = conexion.crearConexion();

                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = cnx;
                    cmd.CommandText = "UPDATE contabilidad_hoja_ruta SET "
                        + "id_usuario=@idUsuario, observaciones=@observaciones, registro_nubox=@registroNubox, "
                        +" cuenta_contabilidad=@cuentaContabilidad WHERE id_orden_compra=@idOrdenCompra AND numero_factura=@numeroFactura";

                    cmd.Parameters.Add("@idOrdenCompra", SqlDbType.VarChar).Value = hojaRuta.numeroOrdenCompra;
                    cmd.Parameters.Add("@idUsuario", SqlDbType.VarChar).Value = hojaRuta.contabilidad.usuario.nombre;
                    cmd.Parameters.Add("@observaciones", SqlDbType.VarChar).Value = hojaRuta.contabilidad.observaciones;
                    cmd.Parameters.Add("@registroNubox", SqlDbType.VarChar).Value = hojaRuta.contabilidad.registroNubox;
                    cmd.Parameters.Add("@cuentaContabilidad", SqlDbType.VarChar).Value = hojaRuta.contabilidad.cuenta;
                    cmd.Parameters.Add("@numeroFactura", SqlDbType.VarChar).Value = hojaRuta.numeroFactura;

                    cmd.CommandType = CommandType.Text;
                    cmd.ExecuteNonQuery();
                    cnx.Close();
                }
                else
                {
                    SqlConnection cnx = conexion.crearConexion();

                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = cnx;
                    cmd.CommandText = "INSERT INTO contabilidad_hoja_ruta "
                        + " VALUES(@idOrdenCompra,@registroNubox,@idUsuario,@cuentaContabilidad,@observaciones,@numeroFactura)";


                    cmd.Parameters.Add("@idOrdenCompra", SqlDbType.VarChar).Value = hojaRuta.numeroOrdenCompra;
                    cmd.Parameters.Add("@idUsuario", SqlDbType.VarChar).Value = hojaRuta.contabilidad.usuario.nombre;
                    cmd.Parameters.Add("@observaciones", SqlDbType.VarChar).Value = hojaRuta.contabilidad.observaciones;
                    cmd.Parameters.Add("@registroNubox", SqlDbType.VarChar).Value = hojaRuta.contabilidad.registroNubox;
                    cmd.Parameters.Add("@cuentaContabilidad", SqlDbType.VarChar).Value = hojaRuta.contabilidad.cuenta;
                    cmd.Parameters.Add("@numeroFactura", SqlDbType.VarChar).Value = hojaRuta.numeroFactura;

                    cmd.CommandType = CommandType.Text;
                    cmd.ExecuteNonQuery();
                    cnx.Close();
                }
            }
        }

        public class retiraPagoHojaRuta
        {
            public string nombre { get; set; }
            public string rut { get; set; }
            public DateTime fecha { get; set; }
            public string observaciones { get; set; }
            public usuarios usuario { get; set; }
            public string urlFoto { get; set; }

            public static bool existenDatos(string idOrdenCompra, string numeroFactura)
            {
                bool retorno = false;

                SqlConnection cnx = conexion.crearConexion();

                SqlCommand cmd = new SqlCommand();
                cmd.Connection = cnx;
                cmd.CommandText = "SELECT * from retira_pago_hoja_ruta where id_orden_compra='" + idOrdenCompra + "' AND numero_factura='"+numeroFactura+"'";
                cmd.CommandType = CommandType.Text;
                SqlDataReader dr = cmd.ExecuteReader();

                retorno = dr.HasRows;

                cnx.Close();

                return retorno;
            }

            public static retiraPagoHojaRuta obtenerDatos(string idOrdenCompra, string numeroFactura)
            {
                retiraPagoHojaRuta retorno = new retiraPagoHojaRuta();
                
                SqlConnection cnx = conexion.crearConexion();

                SqlCommand cmd = new SqlCommand();
                cmd.Connection = cnx;
                cmd.CommandText = "SELECT * from retira_pago_hoja_ruta where id_orden_compra='" + idOrdenCompra + "' AND numero_factura='"+numeroFactura+"'";
                cmd.CommandType = CommandType.Text;
                SqlDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    retorno.nombre = (string)dr["nombre"];
                    retorno.rut = (string)dr["rut"];
                    retorno.fecha = (DateTime)dr["fecha"];
                    retorno.observaciones = (string)dr["observaciones"];
                    retorno.usuario = usuarios.obtenerUsuario((string)dr["id_usuario"]);
                    retorno.urlFoto = (string)dr["url_foto"];
                }

                cnx.Close();

                return retorno;
            }

            public static void guardarDatos(hojaRutaFactura hojaRuta)
            {
                if (existenDatos(hojaRuta.numeroOrdenCompra, hojaRuta.numeroFactura))
                {
                    SqlConnection cnx = conexion.crearConexion();

                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = cnx;
                    cmd.CommandText = "UPDATE retira_pago_hoja_ruta SET "
                        + "id_usuario=@idUsuario, observaciones=@observaciones, nombre=@nombre, "
                        + " rut=@rut, fecha=@fecha WHERE id_orden_compra=@idOrdenCompra AND numero_factura=@numeroFactura";

                    cmd.Parameters.Add("@idOrdenCompra", SqlDbType.VarChar).Value = hojaRuta.numeroOrdenCompra;
                    cmd.Parameters.Add("@nombre", SqlDbType.VarChar).Value = hojaRuta.retiraPago.nombre;
                    cmd.Parameters.Add("@rut", SqlDbType.VarChar).Value = hojaRuta.retiraPago.rut;
                    cmd.Parameters.Add("@fecha", SqlDbType.DateTime).Value = hojaRuta.retiraPago.fecha;
                    cmd.Parameters.Add("@observaciones", SqlDbType.VarChar).Value = hojaRuta.retiraPago.observaciones;
                    cmd.Parameters.Add("@idUsuario", SqlDbType.VarChar).Value = hojaRuta.retiraPago.usuario.nombre;
                    cmd.Parameters.Add("@numeroFactura", SqlDbType.VarChar).Value = hojaRuta.numeroFactura;

                    cmd.CommandType = CommandType.Text;
                    cmd.ExecuteNonQuery();
                    cnx.Close();
                }
                else
                {
                    SqlConnection cnx = conexion.crearConexion();

                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = cnx;
                    cmd.CommandText = "INSERT INTO retira_pago_hoja_ruta "
                        + " VALUES(@idOrdenCompra,@nombre,@fecha,@observaciones,'',@rut,@idUsuario, @numeroFactura)";


                    cmd.Parameters.Add("@idOrdenCompra", SqlDbType.VarChar).Value = hojaRuta.numeroOrdenCompra;
                    cmd.Parameters.Add("@nombre", SqlDbType.VarChar).Value = hojaRuta.retiraPago.nombre;
                    cmd.Parameters.Add("@rut", SqlDbType.VarChar).Value = hojaRuta.retiraPago.rut;
                    cmd.Parameters.Add("@fecha", SqlDbType.DateTime).Value = hojaRuta.retiraPago.fecha;
                    cmd.Parameters.Add("@observaciones", SqlDbType.VarChar).Value = hojaRuta.retiraPago.observaciones;
                    cmd.Parameters.Add("@idUsuario", SqlDbType.VarChar).Value = hojaRuta.retiraPago.usuario.nombre;
                    cmd.Parameters.Add("@numeroFactura", SqlDbType.VarChar).Value = hojaRuta.numeroFactura;

                    cmd.CommandType = CommandType.Text;
                    cmd.ExecuteNonQuery();
                    cnx.Close();
                }
            }
        }

        internal static bool existeFactura(string factura, string proveedor)
        {
            bool retorno = false;

            SqlConnection cnx = conexion.crearConexion();

            SqlCommand cmd = new SqlCommand();
            cmd.Connection = cnx;
            cmd.CommandText = "SELECT * from hoja_ruta where numero_factura='" + factura + "' AND id_proveedor='" + proveedor + "'";
            cmd.CommandType = CommandType.Text;
            SqlDataReader dr = cmd.ExecuteReader();

            retorno = dr.HasRows;

            cnx.Close();

            return retorno;
        }
    }

    public class detalleHojaRutaFactura
    {
        public string nombreItem { get; set; }
        public string idPartida { get; set; }
        public string idFaena { get; set; }
        public string numeroItemPartida { get; set; }
        public string nombreItemPartida { get; set; }
        public float cantidad { get; set; }

        public static List<detalleHojaRutaFactura> obtenerDetalle(string idOrdenCompra, string numeroFactura) 
        {
            List<detalleHojaRutaFactura> retorno = new List<detalleHojaRutaFactura>();
            SqlConnection cnx = conexion.crearConexion();

            SqlCommand cmd = new SqlCommand();
            cmd.Connection = cnx;
            cmd.CommandText = "SELECT * "
                + "FROM detalle_hoja_ruta "
                + "WHERE id_orden_compra='" + idOrdenCompra + "' AND numero_factura='" + numeroFactura + "'";
            cmd.CommandType = CommandType.Text;
            SqlDataReader dr = cmd.ExecuteReader();

            while (dr.Read())
            {
                detalleHojaRutaFactura nueva = new detalleHojaRutaFactura();

                nueva.nombreItem = (string)dr["nombre_item"];
                nueva.idPartida = (string)dr["id_partida"];
                nueva.idFaena = (string)dr["id_faena"];
                nueva.numeroItemPartida = dr["numero_item_partida"].ToString();
                nueva.nombreItemPartida = (string)dr["nombre_item_partida"];
                nueva.cantidad = float.Parse(dr["cantidad"].ToString());

                retorno.Add(nueva);
            }

            cnx.Close();

            return retorno;
        }
    }

}