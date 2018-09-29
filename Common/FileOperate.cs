using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Commonn
{
    public class FileOperate
    {
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
        public static bool MoveFile(string orignFile, string newFile)
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
                throw;
            }

        }
    }
}
