using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DB;
using System.Data;
using Dapper;
using Models;

namespace DAL
{
    public class Selete
    {
        DB.MySQLDBHelper db = new MySQLDBHelper();

        //获取部门下的人

        public List<Role_User> GetDeptUsers(string schoolid, string myuid = "")
        {
            using (var dbHelp = new DBHelper(1).CreateConnection())
            {
                List<Role_User> list = new List<Role_User>();
                string sql = "SELECT mrg.RI_guid roleid,mrg.RI_GroupName rolename,mrgu.RI_UserId uid,mrgu.RI_RealName name from map_rolegroup mrg left join map_rolegroupuser mrgu on mrg.ri_guid=mrgu.ri_rolegroupid WHERE mrg.RI_SchoolId=@schoolid AND mrg.RI_DelFlag=0;";
                var model = dbHelp.Query<Role_User, User, Role_User>(sql, (ru, u) =>
                {
                    for (int i = 0; i < list.Count; i++)
                    {
                        if (list[i].RoleId == ru.RoleId)
                        {
                            if (u.Uid != myuid)
                            {
                                list[i].Users.Add(u);
                            }
                            return ru;
                        }
                    }
                    if (null != u)
                    {
                        if (u.Uid != myuid)
                        {
                            ru.Users.Add(u);
                        }
                    }
                    
                    list.Add(ru);
                    return ru;
                }, new { schoolid = schoolid }, null, true, "uid", null, null).ToList();
                return list;
            }
        }
        //我的分享
        public DataTable MyShare(string userid)
        {
            //db.ConnStr = "platform";
            // return db.ExecuteDataTable("select jw_filepath,jw_createtime,jw_shareid from jw_share where jw_userid='"+userid+"' and jw_delflag=0;");
            return db.ExecuteDataTable("SELECT group_concat(jsf.jw_filepath) as jw_filepath,js.jw_CreateTime,js.jw_ShareId,js.jw_UsersId FROM jw_share_file jsf LEFT JOIN jw_share js ON js.jw_ShareId=jsf.jw_shareid WHERE jsf.jw_filestatus=0 and js.jw_UserId='" + userid + "' and js.jw_DelFlag=0 GROUP BY js.jw_ShareId order by js.jw_createtime desc;");
        }
        //根据分享id获取分享文件
        public DataTable FileByShareid(string shareid)
        {
            //db.ConnStr = "platform";
            // return db.ExecuteDataTable("select jw_filepath,jw_createtime,jw_shareid from jw_share where jw_userid='"+userid+"' and jw_delflag=0;");
            return db.ExecuteDataTable("select jw_filepath from jw_share_file where jw_shareid='" + shareid + "' and jw_filestatus=0;");
        }
        //分享给我的
        public DataTable ToMyShare(string userid)
        {
            //db.ConnStr = "platform";
            //return db.ExecuteDataTable("select jw_userid,jw_filepath,jw_createtime,jw_shareid from jw_share where FIND_IN_SET('"+userid+"',jw_UsersId) and jw_delflag=0;");
            return db.ExecuteDataTable("SELECT group_concat(jsf.jw_filepath) as jw_filepath,js.jw_CreateTime,js.jw_ShareId,js.jw_userid FROM jw_share_file jsf LEFT JOIN jw_share js ON js.jw_ShareId=jsf.jw_shareid WHERE FIND_IN_SET('" + userid + "',js.jw_UsersId) and jsf.jw_filestatus=0  and js.jw_DelFlag=0 GROUP BY js.jw_ShareId order by js.jw_createtime desc;");
        }
        //根据userid获取人的信息
        public DataTable GetUserByUserid(string userid)
        {
            db.ConnStr = "platform";
            return db.ExecuteDataTable("SELECT ri_realname from ri_user WHERE RI_UserId='" + userid + "'");
        }
        public DataTable GetUsersByUserids(string userid)
        {
            db.ConnStr = "platform";
            return db.ExecuteDataTable("SELECT ri_realname from ri_user WHERE RI_UserId in(" + userid + ")");
        }
        //获取回收站的文件
        public DataTable GetRecycle(string userid)
        {
            //db.ConnStr = "platform";
            return db.ExecuteDataTable("select jw_recycleid,jw_sourcepath,jw_virtualname,jw_realname,jw_deltime,jw_type,jw_size from jw_recycle where jw_userid='" + userid + "' and jw_delflag=0");
        }
        //获取回收站某文件的信息
        public DataTable GetRecycleByid(string recycleid)
        {
            //db.ConnStr = "platform";
            return db.ExecuteDataTable("select jw_sourcepath,jw_virtualname from jw_recycle where jw_recycleid='" + recycleid + "' and jw_delflag=0;");
        }
        public DataTable GetParentFileUrl(string id)
        {
            //db.ConnStr = "platform";
            return db.ExecuteDataTable("select jw_FileUrl from jw_file where jw_fileguid='" + id + "' and jw_delflag=0;");
        }
        public DataTable GetUserById(string id)
        {
            db.ConnStr = "platform";
            string sql = "SELECT  RI_UserId,RI_UserName,RI_RealName,ri_schoolId from ri_user  where RI_DelFlag=0 and ri_userId='" + id + "'";
            return db.ExecuteDataTable(sql);
        }
        #region 获取条件下数据的总数
        private int Totle(string table, string StrWhere, string field)
        {
            string sql = string.Format("SELECT COUNT({0}) AS num  FROM {1} WHERE {2} ;", field, table, StrWhere);
            DataTable dr = db.ExecuteDataTable(sql);
            return int.Parse(dr.Rows[0]["num"].ToString());
        }
        #endregion

        #region 拼接SQL语句

        /// <summary>
        /// 拼接SQL语句
        /// </summary>
        /// <param name="Table">表名</param>
        /// <param name="strWhere">条件</param>
        /// <param name="field">字段名称</param>
        /// <returns></returns>
        private string JoinSQL(string Table, string strWhere, string field)
        {

            StringBuilder sb = new StringBuilder();
            sb.Append("SELECT ");
            if (field == string.Empty) sb.Append(" * ");
            else sb.Append(field);
            sb.Append(" FROM " + Table);
            sb.Append(" WHERE " + strWhere);

            return sb.ToString();
        }

        /// <summary>
        /// 带分页拼接SQL语句
        /// </summary>
        /// <param name="Table">表名</param>
        /// <param name="strWhere">条件</param>
        /// <param name="field">字段名称</param>
        /// <param name="page">字段名称</param>
        /// <returns></returns>
        private string JoinSQL(string Table, string strWhere, string field, int page)
        {
            //int num = int.Parse(Utilities.ConfigHelper.GetConfigString("page"));
            int num = 5;
            StringBuilder sb = new StringBuilder();
            sb.Append("SELECT ");
            if (field == string.Empty) sb.Append(" * ");
            else sb.Append(field);
            sb.Append(" FROM " + Table);
            sb.Append(" WHERE " + strWhere);
            sb.Append(" LIMIT " + ((page - 1) * num).ToString() + ", " + num.ToString() + " ");
            return sb.ToString();
        }

        #endregion


    }
}
