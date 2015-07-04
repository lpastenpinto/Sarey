using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using sarey_erp.Models;
using Microsoft.Reporting.WebForms;

namespace sarey_erp.Controllers
{
    public class ReportController : Controller
    {
        //
        // GET: /Report/

        public ActionResult Index()
        {
            return View();
        }

        public FileContentResult VistaReporte_HojaRuta(string idOrdenCompra, string numeroFactura)
        {
            // Nota los datos creados en el dataset deben ser con el mismo nombre que tengan los Datos del Modelo

            // Nota los datos creados en el dataset deben ser con el mismo nombre que tengan los Datos del Modelo
            LocalReport reporte_local = new LocalReport();
            // pasa la ruta donde se encuentra el reporte
            reporte_local.ReportPath = Server.MapPath("~/Report/hojaRuta.rdlc");
            // creamos un recurso de datos del tipo report
            ReportDataSource conjunto_datos = new ReportDataSource();
            // le asginamos al conjuto de datos el nombre del datasource del reporte
            conjunto_datos.Name = "DataSet1";
            List<hojaRutaFacturaInforme> hojaDeRutaInforme = new List<hojaRutaFacturaInforme>();
            if (Session["rol"] != null)
            {
                hojaDeRutaInforme.Add(new hojaRutaFacturaInforme(hojaRutaFactura.obtenerHojaRuta(idOrdenCompra, numeroFactura)));
            }
            // se le asigna el datasource el conjunto de datos desde el modelo
            conjunto_datos.Value = hojaDeRutaInforme;
            // se agrega el conjunto de datos del tipo report al reporte local
            reporte_local.DataSources.Add(conjunto_datos);
            // datos para renderizar como se mostrara el reporte
            string reportType = "PDF";
            string mimeType;
            string encoding;
            string fileNameExtension;
            string deviceInfo = "<DeviceInfo>" +
                 "  <OutputFormat>jpeg</OutputFormat>" +
                 "  <PageWidth>10in</PageWidth>" +
                 "  <PageHeight>12in</PageHeight>" +
                 "  <MarginTop>0.5in</MarginTop>" +
                 "  <MarginLeft>1in</MarginLeft>" +
                 "  <MarginRight>1in</MarginRight>" +
                 "  <MarginBottom>0.5in</MarginBottom>" +
                 "</DeviceInfo>";
            Warning[] warnings;
            string[] streams;
            byte[] renderedBytes;
            //Se renderiza el reporte            
            renderedBytes = reporte_local.Render(reportType, deviceInfo, out mimeType, out encoding, out fileNameExtension, out streams, out warnings);
            // el reporte es mostrado como una imagen
            return File(renderedBytes, mimeType);
        }

        public FileContentResult VistaReporte_Solicitud(string idSolicitudMateriales)
        {
            // Nota los datos creados en el dataset deben ser con el mismo nombre que tengan los Datos del Modelo

            // Nota los datos creados en el dataset deben ser con el mismo nombre que tengan los Datos del Modelo
            LocalReport reporte_local = new LocalReport();
            // pasa la ruta donde se encuentra el reporte
            reporte_local.ReportPath = Server.MapPath("~/Report/solicitudMateriales.rdlc");
            // creamos un recurso de datos del tipo report
            ReportDataSource conjunto_datos = new ReportDataSource();
            ReportDataSource conjunto_datos2 = new ReportDataSource();
            // le asginamos al conjuto de datos el nombre del datasource del reporte
            conjunto_datos.Name = "DataSet1";
            conjunto_datos2.Name = "DataSet2";
            List<solicitudMaterialesInforme> solicitudesMateriales = new List<solicitudMaterialesInforme>();
            List<detallePartidas> detalle = new List<detallePartidas>();
            if (Session["rol"] != null)
            {
                solicitud_materiales solicitud = new solicitud_materiales();
                solicitud.id_solicitud = idSolicitudMateriales;
                solicitud = solicitud.obtenerSolicitud();
                solicitudesMateriales = solicitudMaterialesInforme.convertirSolicitudEnSolicitudInforme(solicitud);

                detalle = detallePartidas.obtenerDetallePartidasSolicitud(solicitud);
            }
            // se le asigna el datasource el conjunto de datos desde el modelo
            conjunto_datos.Value = solicitudesMateriales;
            conjunto_datos2.Value = detalle;
            // se agrega el conjunto de datos del tipo report al reporte local
            reporte_local.DataSources.Add(conjunto_datos);
            reporte_local.DataSources.Add(conjunto_datos2);
            // datos para renderizar como se mostrara el reporte
            string reportType = "PDF";
            string mimeType;
            string encoding;
            string fileNameExtension;
            string deviceInfo = "<DeviceInfo>" +
                 "  <OutputFormat>jpeg</OutputFormat>" +
                 "  <PageWidth>10in</PageWidth>" +
                 "  <PageHeight>12in</PageHeight>" +
                 "  <MarginTop>0.5in</MarginTop>" +
                 "  <MarginLeft>1in</MarginLeft>" +
                 "  <MarginRight>1in</MarginRight>" +
                 "  <MarginBottom>0.5in</MarginBottom>" +
                 "</DeviceInfo>";
            Warning[] warnings;
            string[] streams;
            byte[] renderedBytes;
            //Se renderiza el reporte            
            renderedBytes = reporte_local.Render(reportType, deviceInfo, out mimeType, out encoding, out fileNameExtension, out streams, out warnings);
            // el reporte es mostrado como una imagen
            return File(renderedBytes, mimeType);
        }

        public FileContentResult VistaReporte_OrdenCompraUsoInterno(string idOrdenCompra)
        {
            // Nota los datos creados en el dataset deben ser con el mismo nombre que tengan los Datos del Modelo

            // Nota los datos creados en el dataset deben ser con el mismo nombre que tengan los Datos del Modelo
            LocalReport reporte_local = new LocalReport();
            // pasa la ruta donde se encuentra el reporte
            reporte_local.ReportPath = Server.MapPath("~/Report/ordenCompraUsoInterno.rdlc");
            // creamos un recurso de datos del tipo report
            ReportDataSource conjunto_datos = new ReportDataSource();
            // le asginamos al conjuto de datos el nombre del datasource del reporte
            conjunto_datos.Name = "DataSet1";
            List<ordenCompraReporte> ordenesCompra = new List<ordenCompraReporte>();
            if (Session["rol"] != null)
            {
                orden_compra ordenCompra = orden_compra.obtenerOrdenCompra(idOrdenCompra);
                ordenesCompra = ordenCompraReporte.convertirEnDatosOrdenCompraReporte(ordenCompra);
            }
            // se le asigna el datasource el conjunto de datos desde el modelo
            conjunto_datos.Value = ordenesCompra;
            // se agrega el conjunto de datos del tipo report al reporte local
            reporte_local.DataSources.Add(conjunto_datos);
            // datos para renderizar como se mostrara el reporte
            string reportType = "PDF";
            string mimeType;
            string encoding;
            string fileNameExtension;
            string deviceInfo = "<DeviceInfo>" +
                 "  <OutputFormat>jpeg</OutputFormat>" +
                 "  <PageWidth>10in</PageWidth>" +
                 "  <PageHeight>13in</PageHeight>" +
                 "  <MarginTop>0.5in</MarginTop>" +
                 "  <MarginLeft>1in</MarginLeft>" +
                 "  <MarginRight>1in</MarginRight>" +
                 "  <MarginBottom>0.5in</MarginBottom>" +
                 "</DeviceInfo>";
            Warning[] warnings;
            string[] streams;
            byte[] renderedBytes;
            //Se renderiza el reporte            
            renderedBytes = reporte_local.Render(reportType, deviceInfo, out mimeType, out encoding, out fileNameExtension, out streams, out warnings);
            // el reporte es mostrado como una imagen
            return File(renderedBytes, mimeType);
        }

        public FileContentResult VistaReporte_OrdenCompraProveedor(string idOrdenCompra)
        {
            // Nota los datos creados en el dataset deben ser con el mismo nombre que tengan los Datos del Modelo

            // Nota los datos creados en el dataset deben ser con el mismo nombre que tengan los Datos del Modelo
            LocalReport reporte_local = new LocalReport();
            // pasa la ruta donde se encuentra el reporte
            reporte_local.ReportPath = Server.MapPath("~/Report/ordenCompraProveedor.rdlc");
            // creamos un recurso de datos del tipo report
            ReportDataSource conjunto_datos = new ReportDataSource();
            // le asginamos al conjuto de datos el nombre del datasource del reporte
            conjunto_datos.Name = "DataSet1";
            List<ordenCompraReporte> ordenesCompra = new List<ordenCompraReporte>();
            if (Session["rol"] != null)
            {
                orden_compra ordenCompra = orden_compra.obtenerOrdenCompra(idOrdenCompra);
                ordenesCompra = ordenCompraReporte.convertirEnDatosOrdenCompraReporte(ordenCompra);
            }
            // se le asigna el datasource el conjunto de datos desde el modelo
            conjunto_datos.Value = ordenesCompra;
            // se agrega el conjunto de datos del tipo report al reporte local
            reporte_local.DataSources.Add(conjunto_datos);
            // datos para renderizar como se mostrara el reporte
            string reportType = "PDF";
            string mimeType;
            string encoding;
            string fileNameExtension;
            string deviceInfo = "<DeviceInfo>" +
                 "  <OutputFormat>jpeg</OutputFormat>" +
                 "  <PageWidth>10in</PageWidth>" +
                 "  <PageHeight>13in</PageHeight>" +
                 "  <MarginTop>0.5in</MarginTop>" +
                 "  <MarginLeft>1in</MarginLeft>" +
                 "  <MarginRight>1in</MarginRight>" +
                 "  <MarginBottom>0.5in</MarginBottom>" +
                 "</DeviceInfo>";
            Warning[] warnings;
            string[] streams;
            byte[] renderedBytes;
            //Se renderiza el reporte            
            renderedBytes = reporte_local.Render(reportType, deviceInfo, out mimeType, out encoding, out fileNameExtension, out streams, out warnings);
            // el reporte es mostrado como una imagen
            return File(renderedBytes, mimeType);
        }

    }
}
