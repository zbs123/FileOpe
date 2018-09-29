using NPOI.HSSF.UserModel;
using NPOI.SS.Converter;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using NPOI.XWPF.UserModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace Platform.Common
{
    public class NPOIOpe
    {
        public static void PoiWordToHtml(string path)
        {
            FileStream file = new FileStream(path, FileMode.Open);
            XWPFDocument wordDoc = new XWPFDocument(file);
            
            
        }
        public static string PoiExcelToHtml(string path,string topath)
        {
            MemoryStream memStream = new MemoryStream();
            IWorkbook workbook = null;  //新建IWorkbook对象  
            FileStream fileStream = new FileStream(path, FileMode.Open, FileAccess.Read);
            if (path.ToLower().IndexOf(".xlsx") > 0) // 2007版本  
            {
                workbook = new XSSFWorkbook(fileStream);  //xlsx数据读入workbook  
            }
            else if (path.ToLower().IndexOf(".xls") > 0) // 2003版本  
            {
                workbook = new HSSFWorkbook(fileStream);  //xls数据读入workbook  
            }
            ExcelToHtmlConverter excelToHtmlConverter = new ExcelToHtmlConverter();
            //set output parameter
            excelToHtmlConverter.OutputColumnHeaders = false;
            excelToHtmlConverter.OutputHiddenColumns = false;
            excelToHtmlConverter.OutputHiddenRows = false;
            excelToHtmlConverter.OutputLeadingSpacesAsNonBreaking = false;
            excelToHtmlConverter.OutputRowNumbers = false;
            excelToHtmlConverter.UseDivsToSpan = false;


            //process the excel file
            excelToHtmlConverter.ProcessWorkbook(workbook);
            //output the html file
            //excelToHtmlConverter.Document.Save(memStream);
            //return memStream;
            excelToHtmlConverter.Document.Save(topath);
            return Path.ChangeExtension(path, "html");
        }
    }
}