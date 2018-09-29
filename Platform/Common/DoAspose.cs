
using Aspose.Cells;
using Aspose.Slides.Export;
using Aspose.Words;
using Aspose.Words.Rendering;
using ICSharpCode.SharpZipLib.Zip;
using Platform.Common;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;

namespace Platform.Commonn
{
    public class DoAspose
    {
        //测试
        /// <summary>
        /// 转换ppt文件在线预览
        /// </summary>
        /// <param name="fileName">文件路径</param>
        /// <param name="savepath">保存路径</param>
        /// <returns></returns>
        //public bool ppt2Pdf(string fileName, string savepath, HttpResponse Response)
        //{
        //    try
        //    {
        //        Aspose.Slides.Presentation ppt = new Aspose.Slides.Presentation(fileName);
        //        //将ppt保存到指定路径下 savepath：保存路径
        //        ppt.Save(savepath, Aspose.Slides.Export.SaveFormat.Pdf);
        //        //将ppt保存至数据流中 再通过Response显示在页面中
        //        MemoryStream memStream = new MemoryStream();
        //        ppt.Save(memStream, Aspose.Slides.Export.SaveFormat.Pdf);
        //        byte[] bt = memStream.ToArray();
        //        Response.ContentType = "application/pdf";
        //        Response.OutputStream.Write(bt, 0, bt.Length);
        //        return true;
        //    }
        //    catch (Exception)
        //    {
        //        return false;
        //    }
        //}
        /// <summary>
        /// 将Word转换为Png 
        /// </summary>
        /// <param name="filepath">文件地址</param>
        /// <param name="pageIndex">要转换的页</param>
        /// <returns></returns>
        public void Word2Png(string filepath, int pageIndex, HttpResponse Response)
        {
            MemoryStream memStream = new MemoryStream();
            Aspose.Words.Document doc = new Aspose.Words.Document(filepath);
            PageInfo pageInfo = doc.GetPageInfo(pageIndex);
            Document d = new Document();
            float scale = 100 / 100.0f;
            const int Resolution = 96;
            Size imgSize = pageInfo.GetSizeInPixels(scale, Resolution);
            using (Bitmap img = new Bitmap(imgSize.Width, imgSize.Height))
            {
                img.SetResolution(Resolution, Resolution);
                using (Graphics gfx = Graphics.FromImage(img))
                {
                    gfx.Clear(Color.White);
                    doc.RenderToScale(pageIndex, gfx, 0, 0, scale);
                    img.Save(memStream, ImageFormat.Png);
                }
            }

            // Send the bitmap data to the output stream.
            Response.ContentType = "image/png";
            byte[] imageData = memStream.ToArray();
            Response.OutputStream.Write(imageData, 0, imageData.Length);
        }
        /// <summary>
        /// 将Word转换为Pdf 
        /// </summary>
        /// <param name="strFilePath">文件地址</param>
        /// <param name="savepath">保存地址</param>
        /// <returns></returns>
        public bool Excel2Pdf(string fileName, string savepath, HttpResponseBase Response)
        {
            try
            {
                Aspose.Cells.Workbook excel = new Aspose.Cells.Workbook(fileName);
                //将ppt保存到指定路径下 savepath：保存路径
                excel.Save(savepath, Aspose.Cells.SaveFormat.Pdf);
                //将ppt保存至数据流中 再通过Response显示在页面中
                //MemoryStream memStream = new MemoryStream();
                //excel.Save(memStream, Aspose.Cells.SaveFormat.Pdf);
                //byte[] bt = memStream.ToArray();
                //Response.ContentType = "application/pdf";
                //Response.OutputStream.Write(bt, 0, bt.Length);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        /// <summary>
        /// 将Word转换为Pdf 
        /// </summary>
        /// <param name="strFilePath">文件地址</param>
        /// <param name="savepath">保存地址</param>
        /// <returns></returns>
        public bool Word2Pdf(string strFilePath, string savepath, HttpResponse Response)
        {

            try
            {

                Aspose.Words.Document doc = new Aspose.Words.Document(strFilePath);
                //将Word保存到指定路径下 savepath：保存路径
                doc.Save(savepath, Aspose.Words.SaveFormat.Pdf);
                //将Word保存至数据流中 再通过Response显示在页面中
                MemoryStream memStream = new MemoryStream();
                doc.Save(memStream, Aspose.Words.SaveFormat.Pdf);
                byte[] bt = memStream.ToArray();
                Response.ContentType = "application/pdf";
                Response.OutputStream.Write(bt, 0, bt.Length);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        public string GetAsposeOfficeFiles(string filePath, int pageIndex = 0)
        {
            Stream lstream = new MemoryStream(Convert.FromBase64String(LicenseHelper.Key));
            string sessionID = Guid.NewGuid().ToString("N");
            var pageView = false;
            var fileInfo = new FileInfo(filePath);
            var hostName = HttpUtility.UrlPathEncode(filePath.Replace("\\", "//"));
            var webPath = Path.Combine("/", string.Format("Office/{0}.html", sessionID));
            var generateFilePath = AppDomain.CurrentDomain.BaseDirectory + webPath;
            try
            {
                #region 动态第一次生成文件
                if (!System.IO.File.Exists(generateFilePath))
                {
                    if (fileInfo.Extension == ".doc" || fileInfo.Extension == ".docx")
                    {
                        new Aspose.Words.License().SetLicense(lstream);
                        Document doc = new Document(filePath);
                        doc.Save(generateFilePath, Aspose.Words.SaveFormat.Html);
                    }
                    else if (fileInfo.Extension == ".txt")
                    {
                        new Aspose.Words.License().SetLicense(lstream);
                        Document doc = new Document(filePath,new Aspose.Words.LoadOptions { Encoding = Encoding.Default });
                        doc.Save(generateFilePath, Aspose.Words.SaveFormat.Html);
                    }
                    else if (fileInfo.Extension == ".xls" || fileInfo.Extension == ".xlsx")
                    {
                        new Aspose.Cells.License().SetLicense(lstream);
                        Workbook workbook = new Workbook(filePath);
                        workbook.Save(generateFilePath, Aspose.Cells.SaveFormat.Html);
                    }
                    else if (fileInfo.Extension == ".ppt" || fileInfo.Extension == ".pptx")
                    {
                        using (Aspose.Slides.Presentation pres = new Aspose.Slides.Presentation(filePath))
                        {
                            new Aspose.Slides.License().SetLicense(lstream);
                            var a = pres.Slides.Count;
                            HtmlOptions htmlOpt = new HtmlOptions();
                            htmlOpt.HtmlFormatter = HtmlFormatter.CreateDocumentFormatter("", false);
                            pres.Save(generateFilePath, Aspose.Slides.Export.SaveFormat.Html, htmlOpt);
                        }
                    }
                    else if (fileInfo.Extension == ".pdf")
                    {
                        new Aspose.Pdf.License().SetLicense(lstream);
                        if (pageIndex == 0 && fileInfo.Length / 1024 / 1024 < 2)
                        {
                            var pdfDocument = new Aspose.Pdf.Document(filePath);
                            pdfDocument.Save(generateFilePath, Aspose.Pdf.SaveFormat.Html);
                        }
                        else if (pageIndex == 0)
                        {
                            pageIndex++;
                            pageView = true;
                            var pdfDocument = new Aspose.Pdf.Document(filePath);
                            Aspose.Pdf.Document newDocument = new Aspose.Pdf.Document();
                            newDocument.Pages.Add(pdfDocument.Pages[pageIndex]);
                            newDocument.Save(generateFilePath, Aspose.Pdf.SaveFormat.Html);

                        }
                        else if (pageIndex == -1)
                        {
                            pageView = true;
                            var pdfDocument = new Aspose.Pdf.Document(filePath);
                            Aspose.Pdf.Document newDocument = new Aspose.Pdf.Document();
                            newDocument.Pages.Add(pdfDocument.Pages[pdfDocument.Pages.Count]);
                            newDocument.Save(generateFilePath, Aspose.Pdf.SaveFormat.Html);
                            pageIndex = pdfDocument.Pages.Count;
                        }
                        else
                        {
                            pageView = true;
                            var pdfDocument = new Aspose.Pdf.Document(filePath);
                            if (pageIndex > 0 && pageIndex < pdfDocument.Pages.Count)
                            {
                                Aspose.Pdf.Document newDocument = new Aspose.Pdf.Document();
                                newDocument.Pages.Add(pdfDocument.Pages[pageIndex]);
                                newDocument.Save(generateFilePath, Aspose.Pdf.SaveFormat.Html);
                            }
                            else
                            {
                                Aspose.Pdf.Document newDocument = new Aspose.Pdf.Document();
                                newDocument.Pages.Add(pdfDocument.Pages[pdfDocument.Pages.Count]);
                                newDocument.Save(generateFilePath, Aspose.Pdf.SaveFormat.Html);
                                pageIndex = pdfDocument.Pages.Count;
                            }
                        }
                    }
                    //else if (fileInfo.Extension == ".rar"|| fileInfo.Extension == ".zip")
                    //{
                    //    try
                    //    {
                    //        //读取压缩文件(zip文件)，准备解压缩  
                    //        ZipInputStream zipStream = new ZipInputStream(File.OpenRead(filePath));
                    //        ZipEntry theEntry;
                    //        List<string> ddddd = new List<string>();
                    //        List<Item> myitems = new List<Item>();
                    //        while ((theEntry = zipStream.GetNextEntry()) != null)
                    //        {
                    //            ddddd.Add(theEntry.Name);
                    //            string dir = Path.GetDirectoryName(theEntry.Name);
                    //            string fileName = Path.GetFileName(theEntry.Name);
                    //            string[] ssl = theEntry.Name.Split(new[] { "/" }, StringSplitOptions.None);
                    //            for (int i = 0; i < ssl.Length; i++)
                    //            {
                    //                Item item = new Item { JieDian = i, JName = ssl[i] };
                    //                if (i == 0)
                    //                {
                    //                    var flag = true;
                    //                     for (int j = 0; j < myitems.Count; j++)
                    //                    {
                    //                        if (myitems[j].JName == item.JName)
                    //                        {
                    //                            flag = false;
                    //                            break;
                    //                        }
                    //                    }
                    //                    if (flag)
                    //                    {
                    //                        myitems.Add(item);
                    //                    }
                    //                }
                    //                else
                    //                {
                    //                    for (int j = 0; j < myitems[0].Items.Count; j++)
                    //                    {

                    //                    }
                    //                }
                    //            }
                    //            //以下为解压缩zip文件的基本步骤  
                    //            //基本思路就是遍历压缩文件里的所有文件，创建一个相同的文件。  
                    //            //if (fileName != String.Empty)
                    //            //{
                    //            //    FileStream streamWriter = File.Create(path + "\\" + fileName);

                    //            //    int size = 2048;
                    //            //    byte[] data = new byte[2048];
                    //            //    while (true)
                    //            //    {
                    //            //        size = zipStream.Read(data, 0, data.Length);
                    //            //        if (size > 0)
                    //            //        {
                    //            //            streamWriter.Write(data, 0, size);
                    //            //        }
                    //            //        else
                    //            //        {
                    //            //            break;
                    //            //        }
                    //            //    }

                    //            //    streamWriter.Close();
                    //            //}
                    //        }
                    //        int cou = ddddd.Count;
                    //        if (theEntry != null)
                    //        {
                    //            theEntry = null;
                    //        }
                    //        if (zipStream != null)
                    //        {
                    //            zipStream.Close();
                    //        }
                    //    }
                    //    catch (Exception ex)
                    //    {
                    //        throw ex;
                    //    }
                    //    finally
                    //    {
                    //        GC.Collect();
                    //        GC.Collect(1);
                    //    }

                    //}
                    else
                    {
                        webPath = "";
                    }
                    #endregion
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                lstream.Close();
            }
            return webPath;
        }
        private void IsHasItem(List<Item> items,Item item)
        {
            for (int i = 0; i < item.JieDian; i++)
            {
                IsHasItem(items[i].Items, item);
            }
        }
        
    }
    public class Item
    {
        public Item()
        {
            Items = new List<Item>();
        }
        public int JieDian { get; set; }
        public string JName { get; set; }
        public List<Item> Items { get; set; }
    }
}