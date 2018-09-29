using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
   public class Jw_file
    {
        public int jw_id { get; set; }
        public string jw_ParentId { get; set; }
        public string jw_FileGuid { get; set; }
        public string  jw_Userid { get; set; }
        public string jw_FileName { get; set; }
        public string jw_FileSize { get; set; }
        public string jw_FileType { get; set; }
        public string jw_FileUrl { get; set; }
        public DateTime jw_CreateDate { get; set; }
        public DateTime jw_UpdateDate { get; set; }
        public int jw_DelFlag { get; set; }

    }
}
