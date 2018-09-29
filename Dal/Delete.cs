using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DB;
using Models;
namespace DAL
{
   public  class Delete
    {
        DB.MySQLDBHelper db = new MySQLDBHelper();
        //删除回收站的文件
        public int DelRecycleByid(string recycleid)
        {
            return db.ExecuteNonQuery("delete from jw_recycle where jw_recycleid='"+recycleid+"'");
        }
        public int DelRecycleAll(string userid)
        {
            return db.ExecuteNonQuery("delete from jw_recycle where jw_userid='"+userid+"'");
        }
    }
}
