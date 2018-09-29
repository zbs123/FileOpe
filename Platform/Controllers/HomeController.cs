using Aspose.Cells;
using Aspose.Slides.Export;
using Aspose.Words;
using BLL;
using DAL;
using Models;
using Newtonsoft.Json;
using Platform.Common;
using Platform.Commonn;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using System.Web.UI.WebControls;
using Utilities;
namespace Platform.Controllers
{
    [Authentication]
    public class HomeController : BaseController
    {


        private ShareBll sharebll;
        public HomeController()
        {
            this.sharebll = new ShareBll();
        }

        public ActionResult AddShare(string usersid, string filepath)
        {
            string schoolid = StorageHelper.Cookie.GetCookieValue("User", "sguid");//通过cookie获取
            JavaScriptSerializer jss = new JavaScriptSerializer();
            List<string> uslist = jss.Deserialize<List<string>>(usersid);
            List<string> fplist = jss.Deserialize<List<string>>(filepath);
            string us = "";

            foreach (var item in uslist)
            {
                us += item + ",";
            }

            Jw_share js = new Jw_share();
            js.Jw_ShareId = Guid.NewGuid().ToString();
            js.Jw_schoolid = schoolid;
            js.Jw_UserId = StorageHelper.Cookie.GetCookieValue("User", "uguid");
            js.Jw_UsersId = us.Substring(0, us.Length - 1);
            js.Jw_CreateTime = DateTime.Now;
            js.Jw_DelFlag = 0;
            js.Jw_ShareType = 1;
            List<Jw_share_file> jsfList = new List<Jw_share_file>();
            foreach (var item in fplist)
            {
                Jw_share_file jsf = new Jw_share_file();
                jsf.Jw_shareid = js.Jw_ShareId;
                jsf.Jw_filepath = item;
                jsf.Jw_filestatus = 0;
                jsfList.Add(jsf);
            }
            Utilities.ConvertJson c = new Utilities.ConvertJson();
            if (sharebll.AddShare(js, jsfList) > 0)
            {
                string a = c.ToJson("200", "", "{}");
                return Content(a);
            }
            return Content(c.ToJson("0", "", "{}"));
        }
        public ActionResult GetFile(string path, string type, string search, string start, string end, string ftype, string token, string field, string order)
        {
            HttpCookie cookie = StorageHelper.Cookie.GetCookie("User");
            if (String.IsNullOrEmpty(path))
            {
                path = "/File/" + StorageHelper.Cookie.GetCookieValue("User", "uguid");//通过cookie获取
                //path = "/File/1234567";//通过cookie获取
            }
            FileOpe f = new FileOpe();
            f.Token = token;
            if (string.IsNullOrEmpty(f.Token))
            {
                f.Token = Utilities.Encryption.DESEnCode(StorageHelper.Cookie.GetCookieValue("User", "uguid") + "|" + DateTime.Now.ToFileTimeUtc(), Utilities.Encryption.DESKey);
            }
            List<FileOp> list = null;
            if (!string.IsNullOrEmpty(type))
            {
                list = f.Category(type);
            }
            else if (!string.IsNullOrEmpty(search) || !string.IsNullOrEmpty(start) || !string.IsNullOrEmpty(end) || !string.IsNullOrEmpty(ftype))
            {
                list = f.Search(search, start, end, ftype);
            }
            else
            {
                list = f.GetFileName(path);
            }
            if (!string.IsNullOrEmpty(field))
            {
                string myorder = order == "" ? "asc" : order;
                if (myorder == "asc")
                {
                    if (field == "Size")
                    {
                        list = list.OrderBy((fo) => fo.Ysize).ToList();
                    }
                    if (field == "Name")
                    {
                        list = list.OrderBy((fo) => fo.Name).ToList();
                    }
                    if (field == "Date")
                    {
                        list = list.OrderBy((fo) => fo.Date).ToList();
                    }

                }
                else
                {
                    if (field == "Size")
                    {
                        list = list.OrderByDescending(fo => fo.Ysize).ToList();
                    }
                    if (field == "Name")
                    {
                        list = list.OrderByDescending(fo => fo.Name).ToList();
                    }
                    if (field == "Date")
                    {
                        list = list.OrderByDescending(fo => fo.Date).ToList();
                    }

                }
            }
            string data = JsonConvert.SerializeObject(list);
            Utilities.ConvertJson c = new Utilities.ConvertJson();
            string a = c.ToJson("0", "", data, list.Count.ToString());

            return Content(a);

        }


        public ActionResult pageOne(string path, string type, string search, string token)
        {

            HttpCookie cookie = StorageHelper.Cookie.GetCookie("User");
            if (cookie != null)
            {
                string schoolid = StorageHelper.Cookie.GetCookieValue("User", "sguid");//通过cookie获取
                string uid = StorageHelper.Cookie.GetCookieValue("User", "uguid");
                List<Role_User> list1 = sharebll.GetUser(schoolid, uid);
                ViewData["list"] = list1;
            }
            else Response.Redirect(ConfigHelper.GetConfigString("out"));
            if (String.IsNullOrEmpty(path))
            {
                path = "/File/" + StorageHelper.Cookie.GetCookieValue("User", "uguid");//通过cookie获取
                //path = "/File/123456";//通过cookie获取
            }
            FileOpe f = new FileOpe();
            f.Token = token;
            List<FileOp> list = f.GetFileName(path);
            ViewBag.filecount = list.Count;
            ViewBag.filepath = path;
            string[] paths = path.Replace("//", "/").Split('/');
            List<FileInA> l = new List<FileInA>();
            for (int i = 3; i < paths.Length; i++)
            {
                FileInA fia = new FileInA();
                string tp = "";
                for (int j = 0; j <= i; j++)
                {
                    tp += paths[j] + "/";
                }
                tp = tp.Substring(0, tp.Length - 1);
                fia.Address = tp;
                fia.Name = paths[i];
                l.Add(fia);
            }
            ViewBag.remindPath = l;
            ViewBag.type = type;
            ViewBag.token = token;
            //DoAspose doAspose = new DoAspose();
            //bool b = doAspose.Excel2Pdf(@"C:\Users\admin\Desktop\ri_usersl0913.xlsx", @"C:\Users\admin\Desktop\aa.pdf", Response);
            return View();

        }
        public ActionResult NewFile(string path)
        {
            FileOpe f = new FileOpe();
            Utilities.ConvertJson c = new Utilities.ConvertJson();
            if (f.NewFile(path))
            {
                return Content(c.ToJson("0", "", "{}"));
            }
            return Content(c.ToJson("250", "", "{}"));
        }

        public ActionResult Category(string type)
        {
            ViewBag.type = type;
            return View("index");
        }
        public ActionResult picture()
        {
            HttpCookie cookie = StorageHelper.Cookie.GetCookie("User");
            if (cookie != null)
            {
                string schoolid = StorageHelper.Cookie.GetCookieValue("User", "sguid");//通过cookie获取
                string uid = StorageHelper.Cookie.GetCookieValue("User", "uguid");
                List<Role_User> list1 = sharebll.GetUser(schoolid, uid);
                ViewData["list"] = list1;
            }
            else Response.Redirect(ConfigHelper.GetConfigString("out"));
            FileOpe f = new FileOpe();
            List<FileOp> list = f.Category("1");
            var l = list.GroupBy(s => s.Date.Split(' ')[0]);
            var result = l.Select(x => new DateFiles
            {
                Date = x.Key,
                List = x.ToList()
            }).OrderByDescending(x => x.Date);
            ViewData["imagelist"] = result.ToList();
            return View();
        }

        public ActionResult myShare(string category)
        {
            if (!string.IsNullOrEmpty(category))
            {
                ViewBag.category = category;//接收页面
            }
            return View();
        }
        //获取分享文件
        public ActionResult GetMyShare(string path, string type, string category, string shareid, string username, string date)
        {
            HttpCookie cookie = StorageHelper.Cookie.GetCookie("User");
            string userid = StorageHelper.Cookie.GetCookieValue("User", "uguid");//通过cookie获取;//从cookie中取
            Utilities.ConvertJson c = new Utilities.ConvertJson();
            if (string.IsNullOrEmpty(path))
            {
                List<FileOp> list = new List<FileOp>();
                if (string.IsNullOrEmpty(category))
                {
                    list = sharebll.MyShare(userid);
                }
                else
                {
                    list = sharebll.MyReceive(userid);
                }

                string data = JsonConvert.SerializeObject(list);

                string a = c.ToJson("0", "", data, list.Count.ToString());
                return Content(a);
            }
            else
            {
                FileOpe fo = new FileOpe();

                if (type == "3")
                {
                    //JavaScriptSerializer jss = new JavaScriptSerializer();
                    //List<string> paths = jss.Deserialize<List<string>>(path);
                    List<string> paths = sharebll.GetFilepathByShareid(shareid);
                    List<FileOp> list = fo.GetFilesByPath(paths, username, date);

                    string data = JsonConvert.SerializeObject(list);

                    string a = c.ToJson("0", "", data, list.Count.ToString());
                    return Content(a);
                }
                else if (type == "1")
                {
                    List<FileOp> list = fo.GetFileByPath(path, username, date);

                    string data = JsonConvert.SerializeObject(list);

                    string a = c.ToJson("0", "", data, list.Count.ToString());
                    return Content(a);
                }
                else
                {
                    return new EmptyResult();
                }

            }

        }
        public ActionResult myReceive()
        {
            return View();
        }
        //取消分享
        public ActionResult DelShare(string shareid)
        {
            Utilities.ConvertJson c = new Utilities.ConvertJson();
            JavaScriptSerializer jss = new JavaScriptSerializer();
            if (string.IsNullOrEmpty(shareid))
            {
                return Content(c.ToJson("0", "", "{}"));
            }
            try
            {
                List<string> list = jss.Deserialize<List<string>>(shareid);
                for (int i = 0; i < list.Count; i++)
                {
                    sharebll.UpdateShare(new Jw_share { Jw_ShareId = list[i], Jw_DelFlag = 1 });
                }
                return Content(c.ToJson("200", "", "{}"));
            }
            catch (Exception)
            {
                return Content(c.ToJson("0", "", "{}"));
            }


        }
        [HttpPost]
        public ActionResult Upload(string path)
        {
            HttpCookie cookie = StorageHelper.Cookie.GetCookie("User");
            if (String.IsNullOrEmpty(path))
            {
                path = "/File/" + StorageHelper.Cookie.GetCookieValue("User", "uguid");//通过cookie获取
            }
            Utilities.ConvertJson c = new Utilities.ConvertJson();
            if (Request.Files.Count == 0)
            {
                //Request.Files.Count 文件数为0上传不成功
                //return View("index");
                return Content(c.ToJson("0", "上传失败", "{}"));
            }
            var file = Request.Files[0];
            if (file.ContentLength == 0)
            {
                //文件大小大（以字节为单位）为0时，做一些操作
                //return View("index");
                return Content(c.ToJson("0", "上传失败", "{}"));
            }
            else
            {
                //文件大小不为0
                file = Request.Files[0];
                //保存成自己的文件全路径,newfile就是你上传后保存的文件,
                //服务器上的UpLoadFile文件夹必须有读写权限
                string target = AppDomain.CurrentDomain.BaseDirectory + path + "/";//取得目标文件夹的路径
                string filename = file.FileName;//取得文件名字
                string pa = target + filename;//获取存储的目标地址
                file.SaveAs(pa);

                //Response.Redirect("/home/index");

            }
            return Content(c.ToJson("200", "上传成功", "{}"));
            //return View("index");
        }

        //断点续传
        public ActionResult UploaderProcess(string path)
        {
            if (!String.IsNullOrEmpty(path))
            {
                Session["myp"] = path;
            }
            try
            {
                HttpCookie cookie = StorageHelper.Cookie.GetCookie("User");
                //string n = sharebll.GetParentFileUrl(path);
                Platform.Commonn.test.UploaderResult result = new Commonn.test.WebUploader().Process(Request, "", filepath =>
                {
                    //插入
                    //FileInfo fi = new FileInfo(filepath);
                    //sharebll.AddFile(new Jw_file
                    //{
                    //    jw_ParentId = "",
                    //    jw_FileGuid = Guid.NewGuid().ToString("N"),
                    //    jw_Userid = cookie["uguid"].ToString(),
                    //    jw_FileName = fi.Name,
                    //    jw_FileSize = FileOpe.GetFileSize(fi.Length),
                    //    jw_FileType = fi.Extension,
                    //    jw_FileUrl = fi.FullName.Replace("\\","\\\\"),
                    //    jw_CreateDate = DateTime.Now,
                    //    jw_UpdateDate = DateTime.Now,
                    //    jw_DelFlag = 0
                    //});
                    return true;
                }, path == null ? Session["myp"].ToString() : path, filename =>
                      {
                          return "_" + filename;//保存的文件名
                      }, md5 =>
                      {
                          return false;//指示文件是否存在
                      });

                return Json(result);
            }
            catch (Exception)
            {
                return null;
            }

        }
        public ActionResult DeleteUp()
        {
            FileOpe.DeleteFolder(AppDomain.CurrentDomain.BaseDirectory + "\\upload");
            Utilities.ConvertJson c = new Utilities.ConvertJson();
            string a = c.ToJson("200", "", "{}");
            return Content(a);
        }
        //下载
        public ActionResult Export(string pathArr, string type)
        {
            string uguid = StorageHelper.Cookie.GetCookieValue("User", "uguid");
            Utilities.ConvertJson c = new Utilities.ConvertJson();
            JavaScriptSerializer js = new JavaScriptSerializer();
            var list = js.Deserialize<List<string>>(pathArr);
            if (type == "2")
            {
                return Content(c.ToJson("200", "", JsonConvert.SerializeObject("/fileos" + list[0])));
            }
            List<string> rlist = new List<string>();
            foreach (var item in list)
            {
                rlist.Add(AppDomain.CurrentDomain.BaseDirectory + item);
            }

            var strZipTopDirectoryPath = AppDomain.CurrentDomain.BaseDirectory + "/File/" + uguid + "/";//通过cookie获取
            const int intZipLevel = 6;
            const string strPassword = "";
            var filesOrDirectoriesPaths = rlist.ToArray();
            string filepath = AppDomain.CurrentDomain.BaseDirectory + "/Download/" + uguid;
            if (!Directory.Exists(filepath))
            {
                Directory.CreateDirectory(filepath);
            }
            string zipfilaname = "/Download/" + uguid + "/" + DateTime.Now.ToString("yyyyMMddhhmmss") + ".zip";
            string zipfile = AppDomain.CurrentDomain.BaseDirectory + zipfilaname;
            bool result = global::Commonn.FileDown.Zip1(strZipTopDirectoryPath, intZipLevel, strPassword, filesOrDirectoriesPaths, zipfile);
            if (result)
            {
                string data = JsonConvert.SerializeObject("/fileos" + zipfilaname);
                string a = c.ToJson("200", "", data);
                return Content(a);
            }

            return Content(c.ToJson("500", "", "{}"));
        }
        //回收站
        public ActionResult Recycle(string path)
        {
            //HttpCookie cookie = StorageHelper.Cookie.GetCookie("UserInfo");
            Utilities.ConvertJson c = new Utilities.ConvertJson();
            JavaScriptSerializer jss = new JavaScriptSerializer();
            List<string> list = jss.Deserialize<List<string>>(path);
            try
            {
                foreach (var item in list)
                {
                    Jw_recycle jr = new Jw_recycle();
                    jr.Jw_recycleid = Guid.NewGuid().ToString("N");
                    jr.Jw_userid = StorageHelper.Cookie.GetCookieValue("User", "uguid");//通过cookie获取
                    jr.Jw_sourcePath = item;
                    string p = AppDomain.CurrentDomain.BaseDirectory + item;
                    if (System.IO.File.Exists(p))
                    {
                        FileInfo fi = new FileInfo(p);
                        jr.Jw_realName = fi.Name;
                        jr.Jw_type = fi.Extension.Substring(1);
                        jr.Jw_size = global::Commonn.FileOperate.GetFileSize(fi.Length);
                        jr.Jw_virtualName = DateTime.Now.ToString("yyyyMMddhhmmssfffff") + fi.Extension;
                    }
                    if (Directory.Exists(p))
                    {
                        DirectoryInfo di = new DirectoryInfo(p);
                        jr.Jw_realName = di.Name;
                        jr.Jw_type = "1";
                        jr.Jw_size = "--";
                        jr.Jw_virtualName = DateTime.Now.ToString("yyyyMMddhhmmssfffff");
                    }

                    jr.Jw_delTime = DateTime.Now;
                    jr.Jw_delflag = 0;
                    Jw_share_file jsf = new Jw_share_file();
                    if (sharebll.AddMycycle(jr, new Jw_share_file { Jw_filepath = item, Jw_filestatus = 1 }) > 0)
                    {
                        FileOpe fo = new FileOpe();
                        string recycleFilePath = AppDomain.CurrentDomain.BaseDirectory + "/Recycle/" + StorageHelper.Cookie.GetCookieValue("User", "uguid");
                        if (!Directory.Exists(recycleFilePath))
                        {
                            Directory.CreateDirectory(recycleFilePath);
                        }
                        fo.MoveFile(AppDomain.CurrentDomain.BaseDirectory + item, AppDomain.CurrentDomain.BaseDirectory + "/Recycle/" + StorageHelper.Cookie.GetCookieValue("User", "uguid") + "/" + jr.Jw_virtualName);

                    }
                }
                return Content(c.ToJson("200", "", "{}"));

            }
            catch (Exception ex)
            {
                new DBHelper().Log(ex.ToString());
                return Content(c.ToJson("0", "", "{}"));
            }


        }
        public ActionResult recovery()
        {
            return View();
        }
        public ActionResult GetRecycle()
        {
            //HttpCookie cookie = StorageHelper.Cookie.GetCookie("UserInfo");
            string userid = StorageHelper.Cookie.GetCookieValue("User", "uguid");//通过cookie获取
            List<FileOp> list = sharebll.GetMycycle(userid);
            string data = JsonConvert.SerializeObject(list);
            Utilities.ConvertJson c = new Utilities.ConvertJson();
            string a = c.ToJson("200", "", data, list.Count.ToString());
            return Content(a);
        }
        //还原
        public ActionResult Reduction(string id)
        {
            //HttpCookie cookie = StorageHelper.Cookie.GetCookie("UserInfo");
            Utilities.ConvertJson c = new Utilities.ConvertJson();

            JavaScriptSerializer jss = new JavaScriptSerializer();
            List<string> list = jss.Deserialize<List<string>>(id);
            try
            {
                foreach (var item in list)
                {
                    DataTable dt = sharebll.GetRecycleByid(item);
                    //通过cookie获取
                    FileOpe fo = new FileOpe();
                    bool isSuc = fo.MoveFile(AppDomain.CurrentDomain.BaseDirectory + "/Recycle/" + StorageHelper.Cookie.GetCookieValue("User", "uguid") + "/" + dt.Rows[0]["jw_virtualname"].ToString(), AppDomain.CurrentDomain.BaseDirectory + dt.Rows[0]["jw_sourcepath"].ToString());
                    sharebll.DelRecycleByid(item);
                    sharebll.UpdateShareFileStatus(new Jw_share_file { Jw_filepath = dt.Rows[0]["jw_sourcepath"].ToString(), Jw_filestatus = 0 });
                }
                return Content(c.ToJson("200", "", "{}"));

            }
            catch (Exception)
            {
                return Content(c.ToJson("0", "", "{}"));
            }


        }
        //清空
        public ActionResult empty()
        {
            //HttpCookie cookie = StorageHelper.Cookie.GetCookie("UserInfo");
            Utilities.ConvertJson c = new Utilities.ConvertJson();
            if (sharebll.DelRecycleAll(StorageHelper.Cookie.GetCookieValue("User", "uguid")) > 0)
            {
                FileOpe fo = new FileOpe();
                fo.DelFile(AppDomain.CurrentDomain.BaseDirectory + "/recycle/" + StorageHelper.Cookie.GetCookieValue("User", "uguid"));
                return Content(c.ToJson("200", "", "{}"));
            }
            return Content(c.ToJson("0", "", "{}"));
        }
        //重命名
        public ActionResult Rename(string orignFile, string newFile)
        {
            Utilities.ConvertJson c = new Utilities.ConvertJson();
            FileOpe fo = new FileOpe();
            string nf = orignFile.Substring(0, orignFile.LastIndexOf('/')) + "/" + newFile;
            string orignFileP = AppDomain.CurrentDomain.BaseDirectory + orignFile;
            string newFileP = AppDomain.CurrentDomain.BaseDirectory + nf;
            if (fo.MoveFile(orignFileP, newFileP))
            {
                sharebll.UpdateShareFilePath(new Jw_share_file { Jw_filepath = orignFile }, nf);
                return Content(c.ToJson("200", "", "{}"));
            }
            return Content(c.ToJson("0", "当文件已存在时，无法创建该文件", "{}"));
        }
        //删除文件
        public ActionResult DelFile(string path)
        {
            Utilities.ConvertJson c = new Utilities.ConvertJson();
            if (string.IsNullOrEmpty(path))
            {
                return Content(c.ToJson("00000", "请求错误", "{}"));
            }
            FileOpe fo = new FileOpe();
            if (fo.DelFile(AppDomain.CurrentDomain.BaseDirectory + "Download" + path.Split(new string[] { "Download" }, StringSplitOptions.None)[1]))
            {
                return Content(c.ToJson("200", "", "{}"));
            }
            return Content(c.ToJson("00000", "请求错误", "{}"));
        }

        public ActionResult Test()
        {
            DoAspose doAspose = new DoAspose();
            Utilities.ConvertJson c = new Utilities.ConvertJson();
            bool b = doAspose.Excel2Pdf(@"D:\asp.net\workspace\Platform_File\Platform\File\3F86CE57694F4498923AB7566C88A51D\2017级第2期课表_new.xlsx", @"C:\Users\admin\Desktop\", Response);
            if (b)
            {
                return Content(c.ToJson("200", "", "{}"));
            }
            return Content(c.ToJson("00000", "请求错误", "{}"));
        }
        public ActionResult GetAsposeOfficeFiles(string filePath, int pageIndex = 0)
        {
            Utilities.ConvertJson c = new Utilities.ConvertJson();
            try
            {
               string orcFilePath = AppDomain.CurrentDomain.BaseDirectory+filePath;
                FileInfo fi = new FileInfo(orcFilePath);
                var viewUrl = string.Empty;
                DoAspose doAspose = new DoAspose();
                viewUrl = doAspose.GetAsposeOfficeFiles(orcFilePath);
                if (string.IsNullOrEmpty(viewUrl))
                {
                    if(fi.Extension==".rar"||fi.Extension==".zip"||fi.Extension==".dps" || fi.Extension == ".et")
                    {
                        return Content(c.ToJson("40001", "不支持预览", "{}"));
                    }
                    return Content(c.ToJson("200", filePath, "{}"));
                }
                return Content(c.ToJson("200", viewUrl, "{}"));
            }
            catch (Exception ex)
            {
                new DBHelper().Log(ex.ToString());
                //throw;
            }
            return Content(c.ToJson("40000", "请求错误", "{}"));
            
        }

    }
}