using System;
using System.Web;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Data.SqlClient;
using ArchivoExcel = Microsoft.Office.Interop.Excel;

namespace sarey_erp.Models
{
    public class Excel
    {        

        public Excel() {
           
            Excel.generarExcel(trabajador.obtenerTodosEXCEL(),"archivo.xls");
            Excel.descargarExcel("archivo.xls");
            
        
        }
        public static void generarExcel(DataSet ds, String name)
        {
           
            string fileLocation = HttpContext.Current.Server.MapPath("~/Content/ArchivosExcel/") + name;   
            object misValue = System.Reflection.Missing.Value;
            ArchivoExcel.Application excelApp;
            ArchivoExcel.Workbook myworkbook;
            ArchivoExcel.Worksheet myWorkSheet;             

            excelApp = new ArchivoExcel.Application();
             
           
            myworkbook = excelApp.Workbooks.Add(ArchivoExcel.XlWBATemplate.xlWBATWorksheet);
 
            int worksheetIndex = 1;
           
           foreach (DataTable dt in ds.Tables)
                {
                    //una worksheet para cada tabla nueva.
                    myworkbook.Worksheets.Add(misValue,
excelApp.ActiveWorkbook.Worksheets[excelApp.ActiveWorkbook.Worksheets.Count], worksheetIndex, misValue);
                     
                    //selecciono la última worksheet creada para escribir ahí los datos
                    myWorkSheet = (ArchivoExcel.Worksheet)myworkbook.Sheets[worksheetIndex];
 
                    int rowIndex = 1;
                    foreach (DataRow row in dt.Rows)
                    {
                        int columnIndex = 1;
                         
                        foreach (object dc in row.ItemArray)
                        {
                            myWorkSheet.Cells[rowIndex, columnIndex] = dc.ToString();
                            columnIndex++;
                        }
                        rowIndex++;
                    }
                    worksheetIndex++;
            }

            System.IO.File.Delete(fileLocation);    
            myworkbook.SaveAs(fileLocation);
            myworkbook.Close(true, misValue, misValue);
            excelApp.Quit();
             
        }


        public static void descargarExcel(string nombreArchivo) {
            var imgPath = HttpContext.Current.Server.MapPath("~/Content/ArchivosExcel/" + nombreArchivo + "");

            HttpContext.Current.Response.Clear();           
            HttpContext.Current.Response.AddHeader("Content-Disposition", "attachment; filename=\"" + nombreArchivo + "\"");
            HttpContext.Current.Response.ContentType = "application/octet-stream";
            HttpContext.Current.Response.WriteFile(imgPath);
            HttpContext.Current.Response.Flush();
            HttpContext.Current.Response.End();
        
        }
    }
}