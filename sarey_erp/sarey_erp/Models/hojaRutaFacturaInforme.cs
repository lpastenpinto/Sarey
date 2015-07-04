using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace sarey_erp.Models
{
    public class hojaRutaFacturaInforme
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

        public DateTime fechaOrdenCompra { get; set; }
        public string valorOrdenCompra { get; set; }
        public string observacionesAdquisiciones { get; set; }
        public string nombreCompletoUsuarioAdquisiciones { get; set; }

        public int numero { get; set; }
        public DateTime fecha { get; set; }
        public string observacionesFinanzas { get; set; }
        public string nombreCompletoUsuarioFinanzas { get; set; }

        public string registroNubox { get; set; }
        public string cuenta { get; set; }
        public string observacionesContabilidad { get; set; }
        public string nombreCompletoUsuarioContabilidad { get; set; }

        public string nombre { get; set; }
        public string rut { get; set; }
        public DateTime fechaRetira { get; set; }
        public string observacionesRetira { get; set; }
        public string nombreCompletoUsuarioRetira { get; set; }


        public hojaRutaFacturaInforme(hojaRutaFactura HojaFactura)
        {
            numeroOrdenCompra = HojaFactura.numeroOrdenCompra;
            idProveedor = HojaFactura.idProveedor;
            nombreProvedor = HojaFactura.nombreProvedor;
            rutProveedor = HojaFactura.rutProveedor;
            numeroFactura = HojaFactura.numeroFactura;
            fechaRecepcionFactura = HojaFactura.fechaRecepcionFactura;
            nombreQuienEntrega = HojaFactura.nombreQuienEntrega;
            formaPago = HojaFactura.formaPago;
            idCentroCosto = HojaFactura.idCentroCosto + " - " + faena.obtenerNombreFaena(HojaFactura.idCentroCosto);

            fechaOrdenCompra = HojaFactura.adquisiciones.fechaOrdenCompra;
            valorOrdenCompra = "$" + new formatearString().valoresPesos(HojaFactura.adquisiciones.valorOrdenCompra);
            observacionesAdquisiciones = HojaFactura.adquisiciones.observaciones;
            nombreCompletoUsuarioAdquisiciones = HojaFactura.adquisiciones.usuario.nombreCompleto;

            numero = HojaFactura.finanzas.numero;
            fecha = HojaFactura.finanzas.fecha;
            observacionesFinanzas = HojaFactura.finanzas.observaciones;
            nombreCompletoUsuarioFinanzas = HojaFactura.finanzas.usuario.nombreCompleto;

            registroNubox = HojaFactura.contabilidad.registroNubox;
            cuenta = HojaFactura.contabilidad.cuenta;
            observacionesContabilidad = HojaFactura.contabilidad.observaciones;
            nombreCompletoUsuarioContabilidad = HojaFactura.contabilidad.usuario.nombreCompleto;

            nombre = HojaFactura.retiraPago.nombre;
            rut = HojaFactura.retiraPago.rut;
            fechaRetira = HojaFactura.retiraPago.fecha;
            observacionesRetira = HojaFactura.retiraPago.observaciones;
            nombreCompletoUsuarioRetira = HojaFactura.retiraPago.usuario.nombreCompleto;
        }
    }
}