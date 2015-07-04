using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.OleDb;
using System.Data;
using System.Data.SqlClient;
using sarey_erp.Models;
using System.Globalization;

namespace sarey_erp.Controllers
{
    public class SubirDatosExcelController : Controller
    {
        //
        // GET: /SubirDatosExcel/

        public ActionResult Index()
        {
            if (Session["nombre"] != null && Session["rol"].ToString().Equals("admin"))
            {
                return View();
            }
            return RedirectToAction("Todos", "Faenas");
        }
      

        public ActionResult importarExcel(HttpPostedFileBase file, FormCollection form)
        {
            if (Session["nombre"] != null && Session["rol"].ToString().Equals("admin"))
            {
                SqlConnection cnx = conexion.crearConexion();
                string id_faena = (string)form["nombreFaena"];
                string centroCosto = (string)form["centroCosto"]; 
                string Error = "";
                string fileLocation = "";
                string nombreHoja = "";
                 
                try
                {
                    if (Request.Files["file"].ContentLength > 0)
                    {
                                                                                                                                                                                                  
                        List<string> listaPartidas = new List<string>();                        
                        parserExcel LectorExcel = new parserExcel();

                        string fileExtension = System.IO.Path.GetExtension(Request.Files["file"].FileName);
                        fileLocation = Server.MapPath("~/Content/ArchivosExcel/") + Request.Files["file"].FileName;
                        Request.Files["file"].SaveAs(fileLocation);

                        string excelConnectionString = string.Empty;

                        excelConnectionString = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" +
                        fileLocation + ";Extended Properties=\"Excel 12.0;HDR=Yes;IMEX=1\"";

                        if (fileExtension == ".xls")
                        {
                            excelConnectionString = @"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" +
                            fileLocation + ";Extended Properties=\"Excel 8.0;HDR=Yes;IMEX=1\"";

                        }
                        else if (fileExtension == ".xlsx")
                        {
                            excelConnectionString = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" +
                            fileLocation + ";Extended Properties=\"Excel 12.0;HDR=Yes;IMEX=1\"";
                        }

                        OleDbConnection excelConnection = new OleDbConnection(excelConnectionString);
                        
                      
                        nombreHoja = "Presupuesto$";
                        string strSQL = "SELECT * FROM [" + nombreHoja + "]";

                        OleDbDataAdapter da = new OleDbDataAdapter(strSQL, excelConnection);
                        DataSet ds = new DataSet();

                        da.Fill(ds);                        
                        LectorExcel.leerPresupuesto(ds,id_faena,centroCosto,listaPartidas,cnx,nombreHoja);

                        listaPartidas.RemoveAt(0);
                        nombreHoja = "Precio Unitario$";
                        strSQL = "SELECT * FROM [" + nombreHoja + "]";
                        da = new OleDbDataAdapter(strSQL, excelConnection);
                        ds = new DataSet();
                        da.Fill(ds);

                        LectorExcel.leerPrecioUnitario(ds,id_faena,listaPartidas,cnx,nombreHoja);                        
                                                                   
                        cnx.Close();
                                          
                    }
                    ViewBag.mensaje = "agregado";
                    return RedirectToAction("Todos", "Faenas", new { subidoCorrectamente = "true" });
                }
                catch (Exception ex)
                {
                   
                    partida.eliminarPartidasFaena(id_faena);
                    partida.eliminarPartidasGlobal(id_faena);
                    faena.eliminarFaena(id_faena);
                    items.eliminarItemsdeFaena(id_faena);

                    System.IO.File.Delete(fileLocation);                    
                    ViewBag.Verifica = false;                   
                    ViewBag.Error = Error + ex;
                    cnx.Close();
                }

                return View("Index");
            }
            return RedirectToAction("Todos", "Faenas");
        }






        public ActionResult descargarEjemplo() {
            
            var imgPath = Server.MapPath("~/Content/ArchivosExcel/PRESUPUESTOEJEMPLO.xlsx");
            Response.AddHeader("Content-Disposition", "attachment; filename=\"PRESUPUESTOEJEMPLO.xlsx\"");             
            Response.WriteFile(imgPath);
            Response.End();
            return null;
        }


        public string existeFaena(string nombreFaena)
        {
            if (Session["rol"] != null
                && (Session["rol"].ToString().Equals("admin") || Session["rol"].ToString().Equals("gerencias")))
            {
                 faena NuevaFaena = new faena();
                 NuevaFaena.nombre = nombreFaena;
                 if (NuevaFaena.verificarFaena()) 
                     return "true";
                 else 
                     return "false";
                 
            }
            else
            {
                return "null";
            }
        }


        public string existeCentroCosto(string centroCosto)
        {
            if (Session["rol"] != null
                && (Session["rol"].ToString().Equals("admin") || Session["rol"].ToString().Equals("gerencias")))
            {
                faena NuevaFaena = new faena();
                NuevaFaena.centro_costo = centroCosto;
                if (NuevaFaena.verificarCentroCosto())                
                    return "true";                
                else                
                    return "false";         
            }
            else
            {
                return "null";
            }
        }
    }
}
