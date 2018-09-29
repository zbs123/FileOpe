using DB;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dal
{
    public class ShareDal
    {
        DB.MySQLDBHelper db = new MySQLDBHelper();
        
        public DataTable Get()
        {
           return db.ExecuteDataTable("select * from jw_share");
        }
        public int Add()
        {
            return db.ExecuteNonQuery("insert into");
        }
    }
}
