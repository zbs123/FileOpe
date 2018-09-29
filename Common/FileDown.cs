using DAL;
using ICSharpCode.SharpZipLib.Zip;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Commonn
{
    public class FileDown
    {
        public static Tuple<bool, Stream> Zip(string strZipTopDirectoryPath, int intZipLevel, string strPassword, string[] filesOrDirectoriesPaths)
        {
            try
            {
                List<string> AllFilesPath = new List<string>();
                if (filesOrDirectoriesPaths.Length > 0) // get all files path
                {
                    for (int i = 0; i < filesOrDirectoriesPaths.Length; i++)
                    {
                        if (File.Exists(filesOrDirectoriesPaths[i]))
                        {
                            AllFilesPath.Add(filesOrDirectoriesPaths[i]);
                        }
                        else if (Directory.Exists(filesOrDirectoriesPaths[i]))
                        {
                            GetDirectoryFiles(filesOrDirectoriesPaths[i], AllFilesPath);
                        }
                    }
                }

                if (AllFilesPath.Count > 0)
                {
                    MemoryStream ms = new MemoryStream();
                    ZipOutputStream zipOutputStream = new ZipOutputStream(ms);
                    zipOutputStream.SetLevel(intZipLevel);
                    zipOutputStream.Password = strPassword;

                    for (int i = 0; i < AllFilesPath.Count; i++)
                    {
                        string strFile = AllFilesPath[i];
                        try
                        {
                            if (Directory.Exists(strFile)) //folder
                            {
                                string strFileName = strFile.Replace(strZipTopDirectoryPath, "");
                                if (strFileName.StartsWith(""))
                                {
                                    strFileName = strFileName.Substring(0);
                                }
                                ZipEntry entry = new ZipEntry(strFileName + "/");
                                entry.DateTime = DateTime.Now;
                                zipOutputStream.PutNextEntry(entry);
                            }
                            else //file
                            {
                                FileStream fs = File.OpenRead(strFile);

                                byte[] buffer = new byte[fs.Length];
                                fs.Read(buffer, 0, buffer.Length);

                                string strFileName = strFile.Replace(strZipTopDirectoryPath, "");
                                if (strFileName.StartsWith(""))
                                {
                                    strFileName = strFileName.Substring(0);
                                }
                                ZipEntry entry = new ZipEntry(strFileName);
                                entry.DateTime = DateTime.Now;
                                zipOutputStream.PutNextEntry(entry);
                                zipOutputStream.Write(buffer, 0, buffer.Length);

                                fs.Close();
                                fs.Dispose();
                            }
                        }
                        catch
                        {
                            continue;
                        }
                    }

                    zipOutputStream.Finish();
                    
                    return new Tuple<bool, Stream>(true, ms);
                }
                else
                {
                    return new Tuple<bool, Stream>(false, null);
                }
            }
            catch
            {
                return new Tuple<bool, Stream>(false, null);
            }
        }
        public static bool Zip1(string strZipTopDirectoryPath, int intZipLevel, string strPassword, string[] filesOrDirectoriesPaths,string zipfile)
        {
            try
            {
                List<string> AllFilesPath = new List<string>();
                if (filesOrDirectoriesPaths.Length > 0) // get all files path
                {
                    for (int i = 0; i < filesOrDirectoriesPaths.Length; i++)
                    {
                        if (File.Exists(filesOrDirectoriesPaths[i]))
                        {
                            AllFilesPath.Add(filesOrDirectoriesPaths[i]);
                        }
                        else if (Directory.Exists(filesOrDirectoriesPaths[i]))
                        {
                            GetDirectoryFiles(filesOrDirectoriesPaths[i], AllFilesPath);
                        }
                    }
                }

                if (AllFilesPath.Count > 0)
                {
                    FileStream myfs = new FileStream(zipfile, FileMode.Create);
                    ZipOutputStream zipOutputStream = new ZipOutputStream(myfs);
                    zipOutputStream.SetLevel(intZipLevel);
                    zipOutputStream.Password = strPassword;

                    for (int i = 0; i < AllFilesPath.Count; i++)
                    {
                        string strFile = AllFilesPath[i];
                        try
                        {
                            if (Directory.Exists(strFile)) //folder
                            {
                                strFile = strFile.Replace("//", "/");
                                string strFileName = strFile.Replace(strZipTopDirectoryPath, "");
                                if (strFileName.StartsWith(""))
                                {
                                    strFileName = strFileName.Substring(0);
                                }
                                ZipEntry entry = new ZipEntry(strFileName + "/");
                                entry.DateTime = DateTime.Now;
                                zipOutputStream.PutNextEntry(entry);
                            }
                            else //file
                            {
                                FileStream fs = File.OpenRead(strFile);

                                byte[] buffer = new byte[fs.Length];
                                fs.Read(buffer, 0, buffer.Length);
                                strFile = strFile.Replace("//", "/");
                                string strFileName = strFile.Replace(strZipTopDirectoryPath, "");
                                if (strFileName.StartsWith(""))
                                {
                                    strFileName = strFileName.Substring(0);
                                }
                                ZipEntry entry = new ZipEntry(strFileName);
                                entry.DateTime = DateTime.Now;
                                zipOutputStream.PutNextEntry(entry);
                                zipOutputStream.Write(buffer, 0, buffer.Length);

                                fs.Close();
                                fs.Dispose();
                            }
                        }
                        catch
                        {
                            continue;
                        }
                       
                    }

                    zipOutputStream.Finish();
                    myfs.Flush();
                    myfs.Close();
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch
            {
                return false;
            }
        }

        private static void GetDirectoryFiles(string strParentDirectoryPath, List<string> AllFilesPath)
        {
            string[] files = Directory.GetFiles(strParentDirectoryPath);
            for (int i = 0; i < files.Length; i++)
            {
                AllFilesPath.Add(files[i]);
            }
            string[] directorys = Directory.GetDirectories(strParentDirectoryPath);
            for (int i = 0; i < directorys.Length; i++)
            {
                GetDirectoryFiles(directorys[i], AllFilesPath);
            }
            if (files.Length == 0 && directorys.Length == 0) //empty folder
            {
                AllFilesPath.Add(strParentDirectoryPath);
            }
        }
    }
}
