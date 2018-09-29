using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class Jw_recycle
    {
        private int jw_id;
        private string jw_recycleid;
        private string jw_userid;
        private string jw_sourcePath;
        private string jw_virtualName;
        private string jw_realName;
        private DateTime jw_delTime;
        private int jw_delflag;
        private string jw_type;
        private string jw_size;

        public int Jw_id { get => jw_id; set => jw_id = value; }
        public string Jw_recycleid { get => jw_recycleid; set => jw_recycleid = value; }
        public string Jw_userid { get => jw_userid; set => jw_userid = value; }
        public string Jw_sourcePath { get => jw_sourcePath; set => jw_sourcePath = value; }
       
        public DateTime Jw_delTime { get => jw_delTime; set => jw_delTime = value; }
        public int Jw_delflag { get => jw_delflag; set => jw_delflag = value; }
        public string Jw_virtualName { get => jw_virtualName; set => jw_virtualName = value; }
        public string Jw_realName { get => jw_realName; set => jw_realName = value; }
        public string Jw_type { get => jw_type; set => jw_type = value; }
        public string Jw_size { get => jw_size; set => jw_size = value; }
    }
}
