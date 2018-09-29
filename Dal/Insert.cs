using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using DB;
using Models;
namespace DAL
{
    
    public  class Insert
    {
        IBase _cb = new IBase();
        MySQLDBHelper db = new MySQLDBHelper();
        public int AddShare(Jw_share js)
        {
            string sql = _cb.InsertModel<Jw_share>(js);
            return db.ExecuteNonQuery(sql);
        }
        public int AddShareFile(Jw_share_file jsf)
        {
            string sql = _cb.InsertModel<Jw_share_file>(jsf);
            return db.ExecuteNonQuery(sql);
        }
        public int AddRecycle(Jw_recycle jr)
        {
            string sql = _cb.InsertModel<Jw_recycle>(jr);
            return db.ExecuteNonQuery(sql);
        }
        public int AddFile(Jw_file jf)
        {
            string sql = _cb.InsertModel<Jw_file>(jf);
            return db.ExecuteNonQuery(sql);
        }
    }
   
    public class IBase {
        public string  InsertModel<T>(T t) {
            return  GetSqlInsert<T>(t);
        }
        private string GetSqlInsert<T>(T t)
        {
            Type type = t.GetType();
            PropertyInfo[] properties = type.GetProperties();
            string str = "INSERT INTO " + type.Name + " ( ";
            foreach (var proper in properties)
            {
                str += proper.Name + ",";
            }
            str = str.Substring(0, str.LastIndexOf(","));
            str += " ) VALUES( ";
            foreach (var proper in properties)
            {
                object val = proper.GetValue(t, null);
                if (val is int || val is float || val is decimal || val is double)
                {
                    str += proper.GetValue(t, null) + ",";
                }
                else
                {
                    if (val == null)
                    {
                        str += "null,";
                    }
                    else
                    {
                        str += "'" + ReplaceQuot(proper.GetValue(t, null).ToString())+ "'" + ",";
                    }
                }
            }
            str = str.Substring(0, str.LastIndexOf(","));
            str += " );";
            return str;
        }
        private string ReplaceQuot(string txt)
        {
            return txt.Replace("'", "");
        }
    }
}
