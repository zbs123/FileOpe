using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.IO;
using System.Text;
using MySql.Data.MySqlClient;
using System.Configuration;
using System.Reflection;

namespace DAL
{
    /// <summary>
    /// DBHelper
    /// </summary>
    public class DBHelper
    {
        private static readonly string connectionString = ConfigurationManager.ConnectionStrings["connStr"].ConnectionString ;
        private static readonly string connectionString_wm = ConfigurationManager.ConnectionStrings["platform"].ConnectionString;
        private int connType = 0;//0：连接riyunping,1：连接徐工的库
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="_connType">0：连接riyunping,1：连接徐工的库</param>
        public DBHelper(int _connType = 0)
        {
            connType = _connType;
        }
        /// <summary>
        /// 连接字符串
        /// </summary>
        /// <returns></returns>
        public MySqlConnection CreateConnection()
        {
            MySqlConnection conn = connType == 0 ? new MySqlConnection(connectionString) : new MySqlConnection(connectionString_wm);
            return conn;
        }

        /// <summary>
        /// 连接字符串
        /// </summary>
        /// <returns></returns>
        public MySqlConnection CreateConnectionD()
        {
            MySqlConnection conn = connType == 0 ? new MySqlConnection(connectionString) : new MySqlConnection(connectionString_wm);
            conn.Open();
            return conn;
        }

        /// <summary>  
        /// 执行多条SQL语句，实现数据库事务。  
        /// </summary>mysql数据库  
        /// <param name="SQLStringList">多条SQL语句</param>  
        public void ExecuteSqlTran(List<string> SQLStringList)
        {
            using (MySqlConnection conn = CreateConnection())
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = conn;
                MySqlTransaction tx = conn.BeginTransaction();
                cmd.Transaction = tx;
                try
                {
                    for (int n = 0; n < SQLStringList.Count; n++)
                    {
                        string strsql = SQLStringList[n].ToString();
                        if (strsql.Trim().Length > 1)
                        {
                            cmd.CommandText = strsql;
                            cmd.ExecuteNonQuery();
                        }
                        //后来加上的  
                        if (n > 0 && (n % 500 == 0 || n == SQLStringList.Count - 1))
                        {
                            tx.Commit();
                            tx = conn.BeginTransaction();
                        }
                    }
                    //tx.Commit();//原来一次性提交  
                }
                catch (Exception E)
                {
                    tx.Rollback();
                    throw new Exception(E.Message);
                }
            }
        }

        #region 批量插入
        public void BatchInsert<T>(List<T> Lst) where T : new()
        {
            if (Lst == null || Lst.Count <= 0)
            {
                return;
            }
            using (MySqlConnection conn = CreateConnection())
            {
                MySqlCommand cmd = new MySqlCommand();
                conn.Open();
                cmd.Connection = conn;
                cmd.CommandText = GenBatchInserSql(Lst);
                if (cmd.CommandText == string.Empty)
                {
                    return;
                }
                cmd.ExecuteNonQuery();
            }
        }

        //生成批量插入的sql  
        private string GenBatchInserSql<T>(List<T> Lst) where T : new()
        {
            var names = string.Empty;
            var values = new StringBuilder();
            T t = new T();
            PropertyInfo[] propertys = t.GetType().GetProperties();// 获得此模型的公共属性
            foreach (PropertyInfo pi in propertys)
            {
                names += pi.Name + ",";
            }

            names = names.TrimEnd(',');
            var n = 0;
            foreach (var item in Lst)
            {
                if (n > 0)
                {
                    values.Append(",");
                }
                values.Append("(");
                PropertyInfo[] propertysItem = item.GetType().GetProperties();
                for (var j = 0; j < propertysItem.Length; j++)
                {
                    if (j > 0)
                    {
                        values.Append(",");
                    }
                    if (propertysItem[j].PropertyType == typeof(System.String))
                    {
                        values.AppendFormat("'{0}'", propertysItem[j].GetValue(item));
                    }
                    else if (propertysItem[j].PropertyType == typeof(System.DateTime))
                    {

                        values.Append("'" + ((DateTime)propertys[j].GetValue(item)).ToString("yyyy-MM-dd hh:mm:ss") + "'");
                    }
                    else
                    {
                        values.Append(propertys[j].GetValue(item));
                    }
                }
                values.Append(")");
                n++;
            }
            string res = string.Format("insert into {0} ({1}) values {2}", t.GetType().Name, names, values);
            return res;
        }

        #endregion

        #region MySqlParameter

        /// <summary>

        /// 执行无参SQL语句，并返回执行记录数

        /// </summary>

        public int GetScalar(string safeSql)
        {
            return GetScalar(safeSql, null);
        }

        /// <summary>

        /// 执行有参SQL语句，并返回执行记录数

        /// </summary>

        public int GetScalar(string sql, params MySqlParameter[] paras)

        {

            int i = 0;
            using (MySqlConnection conn = CreateConnection())
            {
                MySqlCommand cmd = new MySqlCommand(sql, conn);
                if (paras != null)
                {
                    cmd.Parameters.AddRange(paras);
                }
                conn.Open();
                i = Convert.ToInt32(cmd.ExecuteScalar());
            }
            return i;

        }

        /// <summary>
        /// 执行SQL语句，返回DataTable
        /// </summary>
        /// <param name="strSqlText"></param>
        /// <returns></returns>
        public DataTable ExecuteQuery(string strSqlText)
        {
            return ExecuteQuery(strSqlText, null);
        }

        public DataTable ExecuteQuery(string strSqlText, MySqlParameter[] pas)
        {
            return ExecuteQuery(strSqlText, pas, CommandType.Text);
        }
        /// <summary>
        /// 执行查询，返回DataTable对象
        /// </summary>
        /// <param name="strSQL">sql语句</param>
        /// <param name="pas">参数数组</param>
        /// <param name="cmdtype">Command类型</param>
        /// <returns>DataTable对象</returns>
        public DataTable ExecuteQuery(string strSQL, MySqlParameter[] pas, CommandType cmdtype)
        {
            DataTable dt = new DataTable(); ;
            using (MySqlConnection conn = CreateConnection())
            {
                MySqlDataAdapter da = new MySqlDataAdapter(strSQL, conn);
                da.SelectCommand.CommandType = cmdtype;
                if (pas != null)
                {
                    da.SelectCommand.Parameters.AddRange(pas);
                }
                da.Fill(dt);
            }
            return dt;
        }


        public int ExcuteProc(string ProcName)
        {
            return ExcuteSQL(ProcName, null, CommandType.StoredProcedure);
        }

        public int ExcuteProc(string ProcName, MySqlParameter[] pars)
        {
            return ExcuteSQL(ProcName, pars, CommandType.StoredProcedure);
        }

        public int ExcuteSQL(string strSQL)
        {
            return ExcuteSQL(strSQL, null);
        }

        public int ExcuteSQL(string strSQL, MySqlParameter[] paras)
        {
            return ExcuteSQL(strSQL, paras, CommandType.Text);
        }

        /// 执行非查询存储过程和SQL语句
        /// 增、删、改
        /// </summary>
        /// <param name="strSQL">要执行的SQL语句</param>
        /// <param name="paras">参数列表，没有参数填入null</param>
        /// <param name="cmdType">Command类型</param>
        /// <returns>返回影响行数</returns>
        public int ExcuteSQL(string strSQL, MySqlParameter[] paras, CommandType cmdType)
        {
            int i = 0;
            using (MySqlConnection conn = CreateConnection())
            {
                MySqlCommand cmd = new MySqlCommand(strSQL, conn);
                cmd.CommandType = cmdType;
                if (paras != null)
                {
                    cmd.Parameters.AddRange(paras);
                }
                conn.Open();
                i = cmd.ExecuteNonQuery();
                conn.Close();
            }
            return i;

        }
        #endregion


        /// <summary>
        /// 执行非查询语句
        /// </summary>
        /// <param name="strSqlText"></param>
        /// <returns></returns>
        public int ExecuteNonQuery(string strSqlText)
        {
            int nCount = 0;
            try
            {
                MySqlConnection conn = CreateConnection();
                conn.Open();
                MySqlCommand cmd = new MySqlCommand(strSqlText, conn);
                nCount = cmd.ExecuteNonQuery();
                conn.Close();
            }
            catch (Exception ex)
            {
                Log(ex.ToString());
            }
            return nCount;
        }

        /// <summary>
        /// 将错误信息写入日志文件
        /// </summary>
        /// <param name="strText"></param>
        public void Log(string strText)
        {
            //string logUrl = AppDomain.CurrentDomain.BaseDirectory + "Log.txt";
            //using (StreamWriter sw = new StreamWriter(logUrl, true, Encoding.Default))
            //{
            //    sw.WriteLine(strText);
            //    sw.Close();
            //}

            WriteLog(strText, 0);
        }

        /// <summary>
        /// 将错误信息写入日志文件 多行
        /// </summary>
        /// <param name="strTextLst"></param>
        public void Log(List<string> strTextLst)
        {
            WriteLog(strTextLst, 1);
        }

        private void WriteLog(dynamic strText, int type)
        {
            string filePath = AppDomain.CurrentDomain.BaseDirectory + "Log";
            if (!Directory.Exists(filePath))
            {
                Directory.CreateDirectory(filePath);
            }
            string logPath = AppDomain.CurrentDomain.BaseDirectory + "Log\\" + DateTime.Now.ToString("yyyy-MM-dd") + ".log";
            try
            {
                using (StreamWriter sw = File.AppendText(logPath))
                {
                    sw.WriteLine("--------------------   时间：" + DateTime.Now.ToString("yyy-MM-dd HH:mm:ss") + "   --------------------");
                    if (type == 0)
                    {
                        sw.WriteLine(strText);
                    }
                    else
                    {
                        foreach (var item in strText)
                        {
                            sw.WriteLine(item);
                        }
                    }
                    sw.WriteLine();
                    sw.Flush();
                    sw.Close();
                    sw.Dispose();
                }
            }
            catch (IOException e)
            {
                using (StreamWriter sw = File.AppendText(logPath))
                {
                    sw.WriteLine("异常：" + e.Message);
                    sw.WriteLine("时间：" + DateTime.Now.ToString("yyy-MM-dd HH:mm:ss"));
                    sw.WriteLine("**************************************************");
                    sw.WriteLine();
                    sw.Flush();
                    sw.Close();
                    sw.Dispose();
                }
            }
        }

    }
}