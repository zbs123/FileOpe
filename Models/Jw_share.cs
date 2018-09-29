using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
   public class Jw_share
    {
        private int jw_id;
        private string jw_ShareId;
        private string jw_ShareName;
        private string jw_schoolid;
        private string jw_UserId;
        private string jw_UsersId;
     
        private DateTime jw_CreateTime;
        private int jw_DelFlag;
        private int jw_ShareType;

        public int Jw_id { get => jw_id; set => jw_id = value; }
        public string Jw_ShareId { get => jw_ShareId; set => jw_ShareId = value; }
        public string Jw_ShareName { get => jw_ShareName; set => jw_ShareName = value; }
        public string Jw_schoolid { get => jw_schoolid; set => jw_schoolid = value; }
        public string Jw_UserId { get => jw_UserId; set => jw_UserId = value; }
        public string Jw_UsersId { get => jw_UsersId; set => jw_UsersId = value; }
       
        public DateTime Jw_CreateTime { get => jw_CreateTime; set => jw_CreateTime = value; }
        public int Jw_DelFlag { get => jw_DelFlag; set => jw_DelFlag = value; }
        public int Jw_ShareType { get => jw_ShareType; set => jw_ShareType = value; }
    }
}
