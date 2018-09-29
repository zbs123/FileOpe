using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DB;
using Models;
namespace DAL
{
    
    public class Update
    {
        UBase _cb = new UBase();

        /// <summary>
        /// 数据库链接字符
        /// </summary>
        string conn = "connStr";
        string plat = "platform";

        /// <summary>
        /// 修改日程
        /// </summary>
        /// <param name="sc"></param>
        /// <returns></returns>
        //public bool Schedule(Models.xw_Schedule sc) {
        //    Dictionary<string, string> dic = new Dictionary<string, string>();
        //    dic.Add("xw_Type", sc.Xw_Type);
        //    dic.Add("xw_Title", sc.Xw_Title);
        //    dic.Add("xw_Content", sc.Xw_Content);
        //    dic.Add("xw_UpdateTime", sc.Xw_UpdateTime.ToString());
        //    dic.Add("xw_DelFlag", sc.Xw_DelFlag);
        //    string strWhere = string.Format("xw_ScheduleId='{0}'", sc.Xw_ScheduleId);
        //    return _cb.UpdateModel<xw_Schedule>(dic, strWhere, plat) > 0 ? true : false;
        //}
        public bool UpdateShareDel(Models.Jw_share sc)
        {
            
            Dictionary<string, string> dic = new Dictionary<string, string>();
            dic.Add("Jw_DelFlag", sc.Jw_DelFlag.ToString());
            string strWhere = string.Format("Jw_ShareId='{0}'", sc.Jw_ShareId);
            return _cb.UpdateModel<Jw_share>(dic, strWhere, conn) > 0 ? true : false;
        }
        public bool UpdateShareFileStatus(Models.Jw_share_file jsf)
        {

            Dictionary<string, string> dic = new Dictionary<string, string>();
            dic.Add("Jw_filestatus", jsf.Jw_filestatus.ToString());
            string strWhere = string.Format("Jw_filepath='{0}'", jsf.Jw_filepath);
            return _cb.UpdateModel<Jw_share_file>(dic, strWhere, conn) > 0 ? true : false;
        }
        public bool UpdateShareFilePath(Models.Jw_share_file jsf,string str)
        {

            Dictionary<string, string> dic = new Dictionary<string, string>();
            dic.Add("Jw_filepath", str);
            string strWhere = string.Format("Jw_filepath='{0}'", jsf.Jw_filepath);
            return _cb.UpdateModel<Jw_share_file>(dic, strWhere, conn) > 0 ? true : false;
        }
    }
    public class UBase {

        MySQLDBHelper db = new MySQLDBHelper();
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dicSet"></param>
        /// <param name="strWhere"></param>
        /// <param name="connStr">数据库链接字符，可跨库查询</param>
        /// <returns></returns>
        public int UpdateModel<T>(Dictionary<string, string> dicSet, string strWhere,string connStr) where T : new()
        {
            T t = new T();
            Type type = t.GetType();
            string strSql = "Update " + type.Name + " Set ";
            int flat = 0;
            foreach (var item in dicSet)
            {
                if (flat == 0)
                {
                    strSql += item.Key + "='" + item.Value + "'";
                    flat++;
                }
                else
                {
                    strSql += " , " + item.Key + "='" + item.Value + "'";
                }
            }
            if (strWhere == string.Empty) return 0;
            else
            {
                strSql = strSql + " where  " + strWhere;

                db.ConnStr = connStr;

                return db.ExecuteNonQuery(strSql);
            }
        }
    }
}
