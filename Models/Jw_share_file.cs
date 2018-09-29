using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
   public class Jw_share_file
    {
        private int jw_id;
        private string jw_shareid;
        private string jw_filepath;
        private int jw_filestatus;

        public int Jw_id { get => jw_id; set => jw_id = value; }
        public string Jw_shareid { get => jw_shareid; set => jw_shareid = value; }
        public string Jw_filepath { get => jw_filepath; set => jw_filepath = value; }
        public int Jw_filestatus { get => jw_filestatus; set => jw_filestatus = value; }
    }
}
