using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class FileOp
    {
        private string name;
        private string size;
        private string date;
        private string type;
        private string fileUrl;
        private string fileThumbnailUrl;
        private string shareid;
        private string userid;
        private string username;
        private long ysize;
        public string Name { get => name; set => name = value; }
        public string Size { get => size; set => size = value; }
        public string Date { get => date; set => date = value; }
        public string Type { get => type; set => type = value; }
        public string FileUrl { get => fileUrl; set => fileUrl = value; }
        public string Shareid { get => shareid; set => shareid = value; }
        public string Userid { get => userid; set => userid = value; }
        public string Username { get => username; set => username = value; }
        public string FileThumbnailUrl { get => fileThumbnailUrl; set => fileThumbnailUrl = value; }
        public long Ysize { get => ysize; set => ysize = value; }
    }
}
