using System;
using System.Collections.Generic;
using System.IO;
using System.Web;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilities;
using Models;
using System.Drawing;

namespace Platform.Commonn
{
    public class FileOpe
    {
        HttpCookie cookie = StorageHelper.Cookie.GetCookie("User");
        private List<FileOp> FileList;
        private List<FileOp> SearchFileList;
        public string Token { get; set; }
        public FileOpe()
        {
            FileList = new List<FileOp>();
            SearchFileList = new List<FileOp>();
        }
        
        //获取文件目录及文件
        public List<FileOp> GetFileName(string path)
        {
            List<FileOp> list = new List<FileOp>();
            //DirectoryInfo di = new DirectoryInfo(HttpContext.Current.Server.MapPath(path));

            DirectoryInfo di = new DirectoryInfo(AppDomain.CurrentDomain.BaseDirectory + path);
            if (Directory.Exists(AppDomain.CurrentDomain.BaseDirectory + path) == false || di.GetFiles().Length + di.GetDirectories().Length == 0)
            {
                //没有上传文件
                return list;
            }
            else
            {
                //FileInfo[] fis = di.GetFiles();
                //DirectoryInfo[] dis = di.GetDirectories();
                foreach (var fi in di.GetDirectories())
                {
                    FileOp fo = new FileOp();
                    fo.Name = "<a href='pageone?path=" + path + "/" + fi.Name + "&token="+Token+"'>" + fi.Name + "</a>";
                    fo.FileUrl = path + "/" + fi.Name;
                    fo.Size = "--";
                    fo.Date = fi.CreationTime.ToString("yyyy-MM-dd HH:mm");
                    fo.Type = "1";
                    list.Add(fo);
                }
                foreach (var fi in di.GetFiles())
                {
                    FileOp fo = new FileOp();
                    //fo.Name = "<a href='/fileos/" + path.Substring(1) + "/" + fi.Name + "'>" + fi.Name + "</a>";
                    fo.Name = "<a javascript:void(0)>" + fi.Name + "</a>";
                    fo.FileUrl = "/" + path.Substring(1) + "/" + fi.Name;
                    fo.Ysize = fi.Length;
                    fo.Size = GetFileSize(fi.Length);
                    fo.Date = fi.CreationTime.ToString("yyyy-MM-dd HH:mm"); ;
                    fo.Type = fi.Extension.Substring(1);
                    list.Add(fo);
                }
                return list;

            }

        }
        /// <summary>
        /// 计算文件大小函数(保留两位小数),Size为字节大小
        /// </summary>
        /// <param name="size">初始文件大小</param>
        /// <returns></returns>
        public static string GetFileSize(long size)
        {
            var num = 1024.00; //byte

            if (size < num)
                return size + "B";
            if (size < Math.Pow(num, 2))
                return (size / num).ToString("f2") + "K"; //kb
            if (size < Math.Pow(num, 3))
                return (size / Math.Pow(num, 2)).ToString("f2") + "M"; //M
            if (size < Math.Pow(num, 4))
                return (size / Math.Pow(num, 3)).ToString("f2") + "G"; //G

            return (size / Math.Pow(num, 4)).ToString("f2") + "T"; //T
        }
        //分类
        public List<FileOp> Category(string type)
        {
            string path = AppDomain.CurrentDomain.BaseDirectory + "/File/" + StorageHelper.Cookie.GetCookieValue("User", "uguid");//通过cookie获取
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            getDirectory(path, type);
            return FileList;
        }
        //搜索
        public List<FileOp> Search(string search, string start, string end, string type)
        {
            string path = AppDomain.CurrentDomain.BaseDirectory + "/File/" + StorageHelper.Cookie.GetCookieValue("User", "uguid");//通过cookie获取
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            getDirectorySearch(path, search, start, end, type);
            return SearchFileList;
        }
        /*
         * 获得指定路径下所有文件名
         * StreamWriter sw  文件写入流
         * string path      文件路径
         * int indent       输出时的缩进量
         */
        public static List<FileOp> getFileName(string path, string type)
        {
            //string[] patharr= path.Split(new string[] { "FileOS" }, StringSplitOptions.None);
            string[] patharr = path.Split(new string[] { ConfigHelper.GetConfigString("path") }, StringSplitOptions.None);
            if (!Directory.Exists(path))
            {
                return null;
            }
            DirectoryInfo root = new DirectoryInfo(path);
            List<FileOp> list = new List<FileOp>();
            string ext = "";
            switch (type)
            {
                case "1":
                    ext = ".jpg,.png,.bmp";
                    break;
                case "2":
                    ext = ".doc,.docx.,.xls,.xlsx,.ppt,.pptx,.pdf.,.txt";
                    break;
                default:
                    ext = "";
                    break;
            }
            foreach (FileInfo f in root.GetFiles())
            {
                if (ext.IndexOf(f.Extension.ToLower()) != -1)
                {
                    if (type == "1")
                    {
                        FileOp fo = new FileOp();
                        fo.Name = "/" + ConfigHelper.GetConfigString("path") + "/" + path.Replace(AppDomain.CurrentDomain.BaseDirectory, "") + "/" + f.Name;
                        fo.FileUrl = patharr[1].Replace('\\', '/') + "/" + f.Name;
                        string thumbnailUrl = "d://thum//";
                        fo.FileThumbnailUrl = "data:image/jpeg;base64," + MakeThumbnail(f.FullName, thumbnailUrl + f.Name, 140, 140, "HW");
                        //fo.FileThumbnailUrl = thumbnailUrl;
                        //fo.FileUrl=  patharr[1].Replace('\\', '/') + "/" + f.Name;
                        fo.Size = GetFileSize(f.Length);
                        fo.Ysize = f.Length;
                        fo.Date = f.CreationTime.ToString("yyyy-MM-dd HH:mm"); ;
                        fo.Type = f.Extension.Substring(1);
                        list.Add(fo);
                    }
                    else
                    {
                        LogHelp.WriteLog(path);
                        FileOp fo = new FileOp();
                        //fo.Name = "<a href='/"+ ConfigHelper.GetConfigString("path") + patharr[1].Replace('\\', '/') + "/" + f.Name + "'>" + f.Name + "</a>";
                        fo.Name = "<a javascript:void(0)>" + f.Name + "</a>";
                        fo.FileUrl = patharr[1].Replace('\\', '/') + "/" + f.Name;
                        fo.Size = GetFileSize(f.Length);
                        fo.Ysize = f.Length;
                        fo.Date = f.CreationTime.ToString("yyyy-MM-dd HH:mm"); ;
                        fo.Type = f.Extension.Substring(1);
                        list.Add(fo);
                    }

                }
                if (ext == "")
                {
                    if (".jpg,.png,.bmp,.doc,.docx.,.xls,.xlsx,.ppt,.pptx,.pdf.,.txt".IndexOf(f.Extension.ToLower()) == -1)
                    {
                        FileOp fo = new FileOp();
                        //fo.Name = "<a href='/"+ ConfigHelper.GetConfigString("path") + patharr[1].Replace('\\', '/') + "/" + f.Name + "'>" + f.Name + "</a>";
                        fo.Name = "<a javascript:void(0)>" + f.Name + "</a>";
                        fo.FileUrl = patharr[1].Replace('\\', '/') + "/" + f.Name;
                        fo.Size = GetFileSize(f.Length);
                        fo.Ysize = f.Length;
                        fo.Date = f.CreationTime.ToString("yyyy-MM-dd HH:mm"); ;
                        fo.Type = f.Extension.Substring(1);
                        list.Add(fo);
                    }
                }
            }
            return list;
        }
        public static List<FileOp> getSearchFileName(string path, string search, string start, string end, string type)
        {

            string[] patharr = path.Split(new string[] { ConfigHelper.GetConfigString("path") }, StringSplitOptions.None);
            DirectoryInfo root = new DirectoryInfo(path);
            List<FileOp> list = new List<FileOp>();

            foreach (FileInfo f in root.GetFiles())
            {
                if (string.IsNullOrEmpty(start))
                {
                    if (string.IsNullOrEmpty(search)&& f.Extension.ToLower().HasStr(type.ToLower()) != -1)
                    {
                        FileOp fo = new FileOp();
                        //fo.Name = "<a href='/"+ ConfigHelper.GetConfigString("path") + patharr[1].Replace('\\', '/') + "/"+f.Name+"'>" + f.Name + "</a>";
                        fo.Name = "<a javascript:void(0)>" + f.Name + "</a>";
                        fo.FileUrl = patharr[1].Replace('\\', '/') + "/" + f.Name;
                        fo.Size = GetFileSize(f.Length);
                        fo.Ysize = f.Length;
                        fo.Date = f.CreationTime.ToString("yyyy-MM-dd HH:mm"); ;
                        fo.Type = f.Extension.Substring(1);
                        list.Add(fo);
                    }else if (string.IsNullOrEmpty(type)&& f.Name.ToLower().HasStr(search.ToLower()) != -1)
                    {
                        FileOp fo = new FileOp();
                        //fo.Name = "<a href='/"+ ConfigHelper.GetConfigString("path") + patharr[1].Replace('\\', '/') + "/"+f.Name+"'>" + f.Name + "</a>";
                        fo.Name = "<a javascript:void(0)>" + f.Name + "</a>";
                        fo.FileUrl = patharr[1].Replace('\\', '/') + "/" + f.Name;
                        fo.Size = GetFileSize(f.Length);
                        fo.Ysize = f.Length;
                        fo.Date = f.CreationTime.ToString("yyyy-MM-dd HH:mm"); ;
                        fo.Type = f.Extension.Substring(1);
                        list.Add(fo);
                    }else if
                     (f.Name.ToLower().HasStr(search.ToLower()) != -1 && f.Extension.ToLower().HasStr(type.ToLower()) != -1)
                    {
                        FileOp fo = new FileOp();
                        //fo.Name = "<a href='/"+ ConfigHelper.GetConfigString("path") + patharr[1].Replace('\\', '/') + "/"+f.Name+"'>" + f.Name + "</a>";
                        fo.Name = "<a javascript:void(0)>" + f.Name + "</a>";
                        fo.FileUrl = patharr[1].Replace('\\', '/') + "/" + f.Name;
                        fo.Size = GetFileSize(f.Length);
                        fo.Ysize = f.Length;
                        fo.Date = f.CreationTime.ToString("yyyy-MM-dd HH:mm"); ;
                        fo.Type = f.Extension.Substring(1);
                        list.Add(fo);
                    }
                }
                else
                {
                    DateTime startD = Convert.ToDateTime(start);
                    DateTime endD = Convert.ToDateTime(end);
                    if((string.IsNullOrEmpty(search)&& f.Extension.ToLower().HasStr(type.ToLower()) != -1) || (f.CreationTime >= startD && f.CreationTime <= endD))
                    {
                        FileOp fo = new FileOp();
                        //fo.Name = "<a href='/"+ ConfigHelper.GetConfigString("path") + patharr[1].Replace('\\', '/') + "/"+f.Name+"'>" + f.Name + "</a>";
                        fo.Name = "<a javascript:void(0)>" + f.Name + "</a>";
                        fo.FileUrl = patharr[1].Replace('\\', '/') + "/" + f.Name;
                        fo.Size = GetFileSize(f.Length);
                        fo.Ysize = f.Length;
                        fo.Date = f.CreationTime.ToString("yyyy-MM-dd HH:mm"); ;
                        fo.Type = f.Extension.Substring(1);
                        list.Add(fo);
                    }else if ((string.IsNullOrEmpty(type) && f.Name.ToLower().HasStr(search.ToLower()) != -1) || (f.CreationTime >= startD && f.CreationTime <= endD))
                    {
                        FileOp fo = new FileOp();
                        //fo.Name = "<a href='/"+ ConfigHelper.GetConfigString("path") + patharr[1].Replace('\\', '/') + "/"+f.Name+"'>" + f.Name + "</a>";
                        fo.Name = "<a javascript:void(0)>" + f.Name + "</a>";
                        fo.FileUrl = patharr[1].Replace('\\', '/') + "/" + f.Name;
                        fo.Size = GetFileSize(f.Length);
                        fo.Ysize = f.Length;
                        fo.Date = f.CreationTime.ToString("yyyy-MM-dd HH:mm"); ;
                        fo.Type = f.Extension.Substring(1);
                        list.Add(fo);

                    }
                    else
                    if ((f.Name.ToLower().HasStr(search.ToLower()) != -1 && f.Extension.ToLower().HasStr(type.ToLower()) != -1) || (f.CreationTime >= startD && f.CreationTime <= endD))
                    {
                        FileOp fo = new FileOp();
                        //fo.Name = "<a href='/"+ ConfigHelper.GetConfigString("path") + patharr[1].Replace('\\', '/') + "/"+f.Name+"'>" + f.Name + "</a>";
                        fo.Name = "<a javascript:void(0)>" + f.Name + "</a>";
                        fo.FileUrl = patharr[1].Replace('\\', '/') + "/" + f.Name;
                        fo.Size = GetFileSize(f.Length);
                        fo.Ysize = f.Length;
                        fo.Date = f.CreationTime.ToString("yyyy-MM-dd HH:mm"); ;
                        fo.Type = f.Extension.Substring(1);
                        list.Add(fo);
                    }
                }


            }
            return list;
        }
        //获得指定路径下所有子目录名
        public void getDirectory(string path, string type)
        {
            FileList.AddRange(getFileName(path, type));
            DirectoryInfo root = new DirectoryInfo(path);
            foreach (DirectoryInfo d in root.GetDirectories())
            {

                getDirectory(d.FullName, type);
            }
        }
        public void getDirectorySearch(string path, string search, string start, string end, string type)
        {
            
            string[] patharr = path.Split(new string[] { ConfigHelper.GetConfigString("path") }, StringSplitOptions.None);
            SearchFileList.AddRange(getSearchFileName(path, search, start, end, type));
            DirectoryInfo root = new DirectoryInfo(path);
            foreach (DirectoryInfo d in root.GetDirectories())
            {
                if (string.IsNullOrEmpty(start))
                {
                    if (d.Name.ToLower().HasStr(search.ToLower()) != -1 || d.Extension.ToLower().HasStr(type.ToLower()) != -1)
                    {
                        FileOp fo = new FileOp();
                        fo.Name = "<a href='pageone?path=" + patharr[1].Replace('\\', '/') + "/" + d.Name + "&token=" + Token + "'>" + d.Name + "</a>";
                        fo.FileUrl = patharr[1].Replace('\\', '/') + "/" + d.Name;
                        fo.Size = "--";
                        fo.Date = d.CreationTime.ToString("yyyy-MM-dd HH:mm");
                        fo.Type = "1";
                        SearchFileList.Add(fo);
                    }
                }
                else
                {
                    DateTime startD = Convert.ToDateTime(start);
                    DateTime endD = Convert.ToDateTime(end);
                    if (d.Name.ToLower().HasStr(search.ToLower()) != -1 || d.Extension.ToLower().HasStr(type.ToLower()) != -1 || (d.CreationTime >= startD && d.CreationTime <= endD))
                    {
                        FileOp fo = new FileOp();
                        fo.Name = "<a href='pageone?path=" + patharr[1].Replace('\\', '/') + "/" + d.Name + "&token=" + Token + "'>" + d.Name + "</a>";
                        fo.FileUrl = patharr[1].Replace('\\', '/') + "/" + d.Name;
                        fo.Size = "--";
                        fo.Date = d.CreationTime.ToString("yyyy-MM-dd HH:mm");
                        fo.Type = "1";
                        SearchFileList.Add(fo);
                    }
                }
               
                getDirectorySearch(d.FullName, search, start, end, type);
            }
        }
        //新建文件夹
        public bool NewFile(string path)
        {
            string tPath = AppDomain.CurrentDomain.BaseDirectory + path;
            string tempP = tPath;
            int count = 1;
            while (Directory.Exists(tempP))
            {
                tempP = tPath;
                tempP += "(" + count + ")";
                count++;
            }
            try
            {
                Directory.CreateDirectory(tempP);
                return true;

            }
            catch (Exception)
            {
                return false;
            }
        }
        //根据路径获取file
        public List<FileOp> GetFilesByPath(List<string> paths, string username,string date)
        {
            List<FileOp> list = new List<FileOp>();
            foreach (var path in paths)
            {
                FileOp fo = new FileOp();
                string p = AppDomain.CurrentDomain.BaseDirectory + path;
                if (File.Exists(p))
                {
                    FileInfo fi = new FileInfo(p);
                    if (fi.CreationTime > Convert.ToDateTime(date))
                    {
                        continue;
                    }
                    fo.Name = fi.Name;
                    fo.Size = GetFileSize(fi.Length);
                    fo.Ysize = fi.Length;
                    fo.Type = fi.Extension.Substring(1);
                    fo.Date = fi.CreationTime.ToString();
                    fo.FileUrl = path;
                    fo.Username = username;
                    list.Add(fo);
                }
                if (Directory.Exists(p))
                {
                    DirectoryInfo di = new DirectoryInfo(p);
                    if (di.CreationTime > Convert.ToDateTime(date))
                    {
                        continue;
                    }
                    fo.Name = di.Name;
                    fo.Size = "--";
                    fo.Type = "1";
                    fo.Date = di.CreationTime.ToString();
                    fo.FileUrl = path;
                    fo.Username = username;
                    list.Add(fo);
                }
            }
            return list;
        }
        //获取文件目录及文件
        public List<FileOp> GetFileByPath(string path, string username,string date)
        {
            List<FileOp> list = new List<FileOp>();
            DirectoryInfo di = new DirectoryInfo(AppDomain.CurrentDomain.BaseDirectory + path);
            if (Directory.Exists(AppDomain.CurrentDomain.BaseDirectory + path) == false || di.GetFiles().Length + di.GetDirectories().Length == 0)
            {

                return list;
            }
            else
            {
                //FileInfo[] fis = di.GetFiles();
                //DirectoryInfo[] dis = di.GetDirectories();
                foreach (var fi in di.GetDirectories())
                {
                    if (fi.CreationTime > Convert.ToDateTime(date))
                    {
                        continue;
                    }
                    FileOp fo = new FileOp();
                    fo.Name = fi.Name;
                    fo.FileUrl = path + "/" + fi.Name;
                    fo.Size = "--";
                    fo.Date = fi.CreationTime.ToString("yyyy-MM-dd HH:mm");
                    fo.Type = "1";
                    fo.Username = username;
                    list.Add(fo);
                }
                foreach (var fi in di.GetFiles())
                {
                    if (fi.CreationTime > Convert.ToDateTime(date))
                    {
                        continue;
                    }
                    FileOp fo = new FileOp();
                    //fo.Name = "<a href='/" + path.Substring(1) + "/" + fi.Name + "'>" + fi.Name + "</a>";
                    fo.Name = "<a javascript:void(0)>" + fi.Name + "</a>";
                    fo.FileUrl = "/" + path.Substring(1) + "/" + fi.Name;
                    fo.Size = GetFileSize(fi.Length);
                    fo.Ysize = fi.Length;
                    fo.Date = fi.CreationTime.ToString("yyyy-MM-dd HH:mm"); ;
                    fo.Type = fi.Extension.Substring(1);
                    fo.Username = username;
                    list.Add(fo);
                }
                return list;

            }

        }
        public bool MoveFile(string orignFile, string newFile)
        {
            if (string.IsNullOrEmpty(orignFile) || string.IsNullOrEmpty(newFile))
            {
                return false;
            }
            try
            {
                if (File.Exists(orignFile))
                {
                    FileInfo fi = new FileInfo(orignFile);
                    fi.MoveTo(newFile);
                    return true;
                }
                else
                if (Directory.Exists(orignFile))
                {
                    DirectoryInfo di = new DirectoryInfo(orignFile);
                    di.MoveTo(newFile);
                    return true;
                }
                else
                {
                    return false;

                }

            }
            catch (Exception)
            {
                return false;
            }

        }
        public bool DelFile(string path)
        {
            try
            {
                if (File.Exists(path))
                {
                    File.Delete(path);
                    return true;

                }
                if (Directory.Exists(path))
                {
                    Directory.Delete(path, true);
                    return true;

                }

            }
            catch (Exception ex)
            {
                LogHelp.WriteLog(ex.ToString());
                return false;
            }

            return true;

        }
        /**//// <summary> 
            /// 生成缩略图 
            /// </summary> 
            /// <param name="originalImagePath">源图路径（物理路径）</param> 
            /// <param name="thumbnailPath">缩略图路径（物理路径）</param> 
            /// <param name="width">缩略图宽度</param> 
            /// <param name="height">缩略图高度</param> 
            /// <param name="mode">生成缩略图的方式</param>     
        public static string MakeThumbnail(string originalImagePath, string thumbnailPath, int width, int height, string mode)
        {
            Image originalImage = Image.FromFile(originalImagePath);

            int towidth = width;
            int toheight = height;

            int x = 0;
            int y = 0;
            int ow = originalImage.Width;
            int oh = originalImage.Height;

            switch (mode)
            {
                case "HW"://指定高宽缩放（可能变形）                 
                    break;
                case "W"://指定宽，高按比例                     
                    toheight = originalImage.Height * width / originalImage.Width;
                    break;
                case "H"://指定高，宽按比例 
                    towidth = originalImage.Width * height / originalImage.Height;
                    break;
                case "Cut"://指定高宽裁减（不变形）                 
                    if ((double)originalImage.Width / (double)originalImage.Height > (double)towidth / (double)toheight)
                    {
                        oh = originalImage.Height;
                        ow = originalImage.Height * towidth / toheight;
                        y = 0;
                        x = (originalImage.Width - ow) / 2;
                    }
                    else
                    {
                        ow = originalImage.Width;
                        oh = originalImage.Width * height / towidth;
                        x = 0;
                        y = (originalImage.Height - oh) / 2;
                    }
                    break;
                default:
                    break;
            }

            //新建一个bmp图片 
            Image bitmap = new System.Drawing.Bitmap(towidth, toheight);

            //新建一个画板 
            Graphics g = System.Drawing.Graphics.FromImage(bitmap);

            //设置高质量插值法 
            g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.High;

            //设置高质量,低速度呈现平滑程度 
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;

            //清空画布并以透明背景色填充 
            g.Clear(Color.Transparent);

            //在指定位置并且按指定大小绘制原图片的指定部分 
            g.DrawImage(originalImage, new Rectangle(towidth > ow ? 70 - ow / 2 : 0, toheight > oh ? 70 - oh / 2 : 0, towidth < ow ? towidth : ow, toheight < oh ? toheight : oh),
                new Rectangle(x, y, ow, oh),
                GraphicsUnit.Pixel);

            try
            {
                //以jpg格式保存缩略图 
                //bitmap.Save(thumbnailPath, System.Drawing.Imaging.ImageFormat.Jpeg);
                MemoryStream ms = new MemoryStream();
                bitmap.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                byte[] arr = new byte[ms.Length];
                ms.Position = 0;
                ms.Read(arr, 0, (int)ms.Length);
                ms.Close();
                return Convert.ToBase64String(arr);
            }
            catch (System.Exception e)
            {
                throw e;
            }
            finally
            {
                originalImage.Dispose();
                bitmap.Dispose();
                g.Dispose();
            }
        }
        /// 清空指定的文件夹，但不删除文件夹
        /// </summary>
        /// <param name="dir"></param>
        public static void DeleteFolder(string dir)
        {
            foreach (string d in Directory.GetFileSystemEntries(dir))
            {
                if (File.Exists(d))
                {
                    FileInfo fi = new FileInfo(d);
                    if (fi.Attributes.ToString().IndexOf("ReadOnly") != -1)
                        fi.Attributes = FileAttributes.Normal;
                    File.Delete(d);//直接删除其中的文件  
                }
                else
                {
                    DirectoryInfo d1 = new DirectoryInfo(d);
                    if (d1.GetFiles().Length != 0)
                    {
                        DeleteFolder(d1.FullName);////递归删除子文件夹
                    }
                    Directory.Delete(d);
                }
            }
        }
    }
    public static class Ext
    {
        public static int HasStr(this string s, string str)
        {
            if (string.IsNullOrEmpty(str))
            {
                return -1;
            }
            return s.IndexOf(str);
        }
        
    }
}
