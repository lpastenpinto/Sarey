using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace sarey_erp.Models
{
    public class ordenCompraReporte
    {
        public string numeroOrdenCompra { get; set; }
        public DateTime fecha { get; set; }

        //datosEmpresa
        public string rutEmpresa { get; set; }
        public string giroEmpresa { get; set; }
        public string direccionEmpresa { get; set; }
        public string telefonoEmpresa { get; set; }
        public string correoEmpresa { get; set; }

        //datos empresa receptora
        public string nombreEmpresaPara { get; set; }
        public string rutEmpresaPara { get; set; }
        public string giroEmpresaPara { get; set; }
        public string direccionEmpresaPara { get; set; }
        public string telefonoEmpresaPara { get; set; }

        //detalle
        public string cantidad { get; set; }
        public string unidad { get; set; }
        public string detalle { get; set; }
        public string precio { get; set; }
        public string montoTotal { get; set; }

        //totales
        public string subtotal { get; set; }
        public string iva { get; set; }
        public string totalPagar { get; set; }

        //sirvase a entregar en
        public string sirvaseEntregar { get; set; }
        public string direccionEntregar { get; set; }
        public string transporteCuentaEntregar { get; set; }
        public string formaPagoEntregar { get; set; }
        public string plazoEntregaEntregar { get; set; }

        //uso interno
        public string nombreObraUsoInterno { get; set; }
        public string comunaUsoInterno { get; set; }
        public string observacionesUsoInterno { get; set; }
        public string emRevisadoPorUsoInterno { get; set; }

        public string item { get; set; }
        public string presupuesto { get; set; }
        public string OC { get; set; }
        public string saldo { get; set; }

        public string observacion { get; set; }

        public string reemplazada { get; set; }
        public string numeroReemplaza { get; set; }
        public string reemplaza { get; set; }
        public string numeroReemplazada { get; set; }        

        public static List<ordenCompraReporte> convertirEnDatosOrdenCompraReporte(orden_compra ordenCompra)
        {
            List<ordenCompraReporte> retorno = new List<ordenCompraReporte>();

            datosEmpresa DatosEmpresa = datosEmpresa.obtenerDatos();

            List<detalle_ordencompra> detalle = detalle_ordencompra.obtenerTodas(ordenCompra.numero_orden);
            nota notaOrden = nota.obtenerNota(ordenCompra.numero_orden);
            usointerno_oc usoInterno = usointerno_oc.obtenerUsointerno(ordenCompra.numero_orden);

            proveedores proveedor = new proveedores().getProveedor(ordenCompra.id_proveedor);
            
                //Para cada item del detalle se crea un elemento
            for (int i = 0; i < detalle.Count; i++)
            {
                ordenCompraReporte temp = new ordenCompraReporte();

                temp.numeroOrdenCompra = ordenCompra.numero_orden;
                temp.fecha = ordenCompra.fecha;

                temp.rutEmpresa = DatosEmpresa.rut;
                temp.giroEmpresa = DatosEmpresa.giro;
                temp.direccionEmpresa = DatosEmpresa.direccion;
                temp.telefonoEmpresa = DatosEmpresa.telefono;
                temp.correoEmpresa = DatosEmpresa.correo;

                temp.nombreEmpresaPara = ordenCompra.id_proveedor;
                temp.rutEmpresaPara = proveedor.rut;
                temp.giroEmpresaPara = proveedor.razonsocial;
                temp.direccionEmpresaPara = proveedor.direccion;
                temp.telefonoEmpresaPara = proveedor.telefono;

                temp.cantidad = detalle[i].cantidad_item.ToString();
                temp.unidad = detalle[i].unidad;
                temp.detalle = detalle[i].id_item;
                temp.precio = "$" + new formatearString().valoresPesos(detalle[i].precio_unitario.ToString());
                temp.montoTotal = "$" + new formatearString().valoresPesos((detalle[i].precio_unitario * detalle[i].cantidad_item).ToString());

                temp.subtotal = "$" + new formatearString().valoresPesos(ordenCompra.sub_total);
                temp.iva = "$" + new formatearString().valoresPesos(ordenCompra.iva);
                temp.totalPagar = "$" + new formatearString().valoresPesos(ordenCompra.total);

                //Nota
                temp.sirvaseEntregar = notaOrden.donde;
                temp.direccionEntregar = notaOrden.dir_entrega;
                temp.transporteCuentaEntregar = notaOrden.transporte;
                temp.formaPagoEntregar = notaOrden.cuenta;
                temp.plazoEntregaEntregar = notaOrden.plazo_entrega;

                //Uso Interno
                temp.nombreObraUsoInterno = usoInterno.faena;
                temp.comunaUsoInterno = usoInterno.comuna;
                temp.observacionesUsoInterno = usoInterno.observacion;
                temp.emRevisadoPorUsoInterno = usuarios.obtenerUsuario(usoInterno.revisadoPor).nombreCompleto;

                temp.item = usoInterno.item;
                temp.presupuesto = usoInterno.presupuesto;
                temp.OC = usoInterno.oc;
                temp.saldo = usoInterno.saldo;

                temp.observacion = ordenCompra.observacion;
                if (ordenCompra.esReeemplazada())
                {
                    temp.reemplazada = "true";
                    temp.numeroReemplaza = ordenCompra.obtenerNumeroOrdenQueReemplaza();
                }
                else 
                {
                    temp.reemplazada = "false";
                }

                if (ordenCompra.reemplaza())
                {
                    temp.reemplaza = "true";
                    temp.numeroReemplazada = ordenCompra.obtenerNumeroOrdenReemplazada();
                }
                else
                {
                    temp.reemplaza = "false";
                }

                retorno = agregarParaListaSinRepetir(retorno, temp);
            }

            return retorno;
        }

        static List<ordenCompraReporte> agregarParaListaSinRepetir(List<ordenCompraReporte> Lista, ordenCompraReporte temp) 
        {
            List<ordenCompraReporte> retorno = new List<ordenCompraReporte>();
            bool encontrado = false;

            for (int i = 0; i < Lista.Count; i++) 
            {
                if (Lista[i].unidad.Equals(temp.unidad) && Lista[i].precio.Equals(temp.precio) && Lista[i].detalle.Equals(temp.detalle)) 
                {
                    encontrado = true;

                    Lista[i].cantidad = (int.Parse(Lista[i].cantidad) + int.Parse(temp.cantidad)).ToString();
                    Lista[i].montoTotal = (int.Parse(Lista[i].cantidad) * int.Parse(Lista[i].precio)).ToString();
                }
                retorno.Add(Lista[i]);
            }
            if (!encontrado) {
                retorno.Add(temp);
            }

            return retorno;
        }
    }
}