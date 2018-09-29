using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Platform.Controllers
{
    public class DefaultController : Controller
    {
        // GET: Default
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Export(string pathArr)
        {

            var strZipTopDirectoryPath = Server.MapPath("~/");
            const int intZipLevel = 6;
            const string strPassword = "";
            var filesOrDirectoriesPaths = new string[] { Server.MapPath("~/File/123456/2222") };
            string zipfilaname = DateTime.Now.ToString("yyyyMMddhhmmss") + ".zip";
            string zipfile = Server.MapPath("/File/123456/"+zipfilaname);
            bool result =global::Commonn.FileDown.Zip1(strZipTopDirectoryPath, intZipLevel, strPassword, filesOrDirectoriesPaths,zipfile);
            //var result = Common.FileDown.Zip(strZipTopDirectoryPath, intZipLevel, strPassword, filesOrDirectoriesPaths);
            //var buffer = new byte[result.Item2.Length];
            //result.Item2.Position = 0;
            //result.Item2.Read(buffer, 0, buffer.Length);
            //result.Item2.Close();

            //Response.ContentType = "application/octet-stream";
            //Response.AppendHeader("content-disposition", "attachment;filename="+ HttpUtility.UrlEncode("打包文档.zip", System.Text.Encoding.UTF8));
            //Response.BinaryWrite(buffer);
            //Response.Flush();
            //Response.End();
            string data = JsonConvert.SerializeObject("/File/123456/" + zipfilaname);
            Utilities.ConvertJson c = new Utilities.ConvertJson();
            string a = c.ToJson("0", "", data);
            return Content(a);
        }
    }
}