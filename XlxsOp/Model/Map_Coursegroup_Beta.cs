using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XlsxOperate.Model
{
   public class Map_Coursegroup_Beta
    {
        public int ri_id { get; set; }
        public string ri_schoolid { get; set; }
        public string ri_gradeid { get; set; }
        public string ri_classid { get; set; }
        public string ri_teacherid { get; set; }
        public int ri_subject { get; set; }
        public string ri_courseid { get; set; }
        public string ri_studentid { get; set; }
        public int ri_week { get; set; }
        public int ri_number { get; set; }
        public string ri_classroom { get; set; }
        public DateTime ri_scheduletime { get; set; }
        public DateTime ri_createTime { get; set; }
        public DateTime ri_updatetime { get; set; }
        public int ri_synflag { get; set; }
        public int ri_delflag { get; set; }
    }
}
