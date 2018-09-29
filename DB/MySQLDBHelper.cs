﻿using System;
using System.Data;
using System.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using MySql.Data;
using MySql.Data.MySqlClient;
using System.Text.RegularExpressions;
namespace DB
{


    public class MySQLDBHelper
    {
        string connStr = "connStr"; /*数据库默认链接字符*/
        public string ConnStr { get => connStr; set => connStr = value; }
        public MySqlConnection conn()
        {

            string url = ConfigurationManager.ConnectionStrings[ConnStr].ConnectionString;
            //string url = "Data Source=192.168.10.254;Port=3307;Initial Catalog = RIYunPingtest; Allow User Variables=True; Persist Security Info=True;User=root;Password=wm360";
            return new MySqlConnection(url);
        }



        /// <summary>  
        /// 执行SQL语句，返回影响的记录数  
        /// </summary>  
        /// <param name="SQLString">SQL语句</param>  
        /// <returns>影响的记录数</returns>  
        public int ExecuteNonQuery(string SQLString)
        {
            using (MySqlConnection connection = conn())
            {
                using (MySqlCommand cmd = new MySqlCommand(SQLString, connection))
                {
                    try
                    {
                        connection.Open();
                        int rows = cmd.ExecuteNonQuery();
                        return rows;
                    }
                    catch (MySqlException e)
                    {
                        connection.Close();
                        throw e;
                    }
                }
            }
        }

        /// <summary>  
        /// 执行SQL语句，返回影响的记录数  
        /// </summary>  
        /// <param name="SQLString">SQL语句</param>  
        /// <returns>影响的记录数</returns>  
        public int ExecuteNonQuery(string SQLString, params MySqlParameter[] cmdParms)
        {
            using (MySqlConnection connection = conn())
            {
                using (MySqlCommand cmd = new MySqlCommand())
                {
                    try
                    {
                        PrepareCommand(cmd, connection, null, SQLString, cmdParms);
                        int rows = cmd.ExecuteNonQuery();
                        cmd.Parameters.Clear();
                        return rows;
                    }
                    catch (MySqlException e)
                    {
                        throw e;
                    }
                }
            }
        }

        /// <summary>  
        /// 执行一条计算查询结果语句，返回查询结果（object）。  
        /// </summary>  
        /// <param name="SQLString">计算查询结果语句</param>  
        /// <returns>查询结果（object）</returns>  
        public object ExecuteScalar(string SQLString, params MySqlParameter[] cmdParms)
        {
            using (MySqlConnection connection = conn())
            {
                using (MySqlCommand cmd = new MySqlCommand())
                {
                    try
                    {
                        PrepareCommand(cmd, connection, null, SQLString, cmdParms);
                        object obj = cmd.ExecuteScalar();
                        cmd.Parameters.Clear();
                        if ((Object.Equals(obj, null)) || (Object.Equals(obj, System.DBNull.Value)))
                        {
                            return null;
                        }
                        else
                        {
                            return obj;
                        }
                    }
                    catch (MySqlException e)
                    {
                        throw e;
                    }
                }
            }
        }

        /// <summary>  
        /// 执行查询语句，返回MySqlDataReader (注意：调用该方法后，一定要对MySqlDataReader进行Close )  
        /// </summary>  
        /// <param name="strSQL">查询语句</param>  
        /// <returns>MySqlDataReader</returns>  
        public MySqlDataReader ExecuteReader(string strSQL)
        {
            MySqlConnection connection = conn();
            MySqlCommand cmd = new MySqlCommand(strSQL, connection);
            MySqlDataReader myReader = null;
            try
            {
                connection.Open();
                myReader = cmd.ExecuteReader();
                return myReader;
            }
            catch (MySqlException e)
            {
                throw e;
            }
            finally
            {
                myReader.Close();
            }
        }
        /// <summary>  
        /// 执行查询语句，返回MySqlDataReader ( 注意：调用该方法后，一定要对MySqlDataReader进行Close )  
        /// </summary>  
        /// <param name="strSQL">查询语句</param>  
        /// <returns>MySqlDataReader</returns>  
        public MySqlDataReader ExecuteReader(string SQLString, params MySqlParameter[] cmdParms)
        {
            MySqlConnection connection = conn();
            MySqlCommand cmd = new MySqlCommand();
            MySqlDataReader myReader = null;
            try
            {
                PrepareCommand(cmd, connection, null, SQLString, cmdParms);
                myReader = cmd.ExecuteReader();
                cmd.Parameters.Clear();
                return myReader;
            }
            catch (MySqlException e)
            {
                throw e;
            }
            finally
            {
                myReader.Close();
                cmd.Dispose();
                connection.Close();
            }
        }

        /// <summary>  
        /// 执行查询语句，返回DataTable  
        /// </summary>  
        /// <param name="SQLString">查询语句</param>  
        /// <returns>DataTable</returns>  
        public DataTable ExecuteDataTable(string SQLString)
        {
            using (MySqlConnection connection = conn())
            {
                DataSet ds = new DataSet();
                try
                {
                    connection.Open();
                    MySqlDataAdapter command = new MySqlDataAdapter(SQLString, connection);
                    command.Fill(ds, "ds");
                }
                catch (MySqlException ex)
                {
                    throw new Exception(ex.Message);
                }
                return ds.Tables[0];
            }
        }

        /// <summary>  
        /// 执行查询语句，返回DataSet  
        /// </summary>  
        /// <param name="SQLString">查询语句</param>  
        /// <returns>DataTable</returns>  
        public DataTable ExecuteDataTable(string SQLString, params MySqlParameter[] cmdParms)
        {
            using (MySqlConnection connection = conn())
            {
                MySqlCommand cmd = new MySqlCommand();
                PrepareCommand(cmd, connection, null, SQLString, cmdParms);
                using (MySqlDataAdapter da = new MySqlDataAdapter(cmd))
                {
                    DataSet ds = new DataSet();
                    try
                    {
                        da.Fill(ds, "ds");
                        cmd.Parameters.Clear();
                    }
                    catch (MySqlException ex)
                    {
                        throw new Exception(ex.Message);
                    }
                    return ds.Tables[0];
                }
            }
        }
        /// <summary>
        /// 获取起始页码和结束页码  
        /// </summary>
        /// <param name="cmdText"></param>
        /// <param name="startResord"></param>
        /// <param name="maxRecord"></param>
        /// <returns></returns>
        public DataTable ExecuteDataTable(string cmdText, int startResord, int maxRecord)
        {
            using (MySqlConnection connection = conn())
            {
                DataSet ds = new DataSet();
                try
                {
                    connection.Open();
                    MySqlDataAdapter command = new MySqlDataAdapter(cmdText, connection);
                    command.Fill(ds, startResord, maxRecord, "ds");
                }
                catch (MySqlException ex)
                {
                    throw new Exception(ex.Message);
                }
                return ds.Tables[0];
            }
        }

        /// <summary>  
        /// 获取分页数据 在不用存储过程情况下  
        /// </summary>  
        /// <param name="recordCount">总记录条数</param>  
        /// <param name="selectList">选择的列逗号隔开,支持top num</param>  
        /// <param name="tableName">表名字</param>  
        /// <param name="whereStr">条件字符 必须前加 and</param>  
        /// <param name="orderExpression">排序 例如 ID</param>  
        /// <param name="pageIdex">当前索引页</param>  
        /// <param name="pageSize">每页记录数</param>  
        /// <returns></returns>  
        public DataTable getPager(out int recordCount, string selectList, string tableName, string whereStr, string orderExpression, int pageIdex, int pageSize)
        {
            int rows = 0;
            DataTable dt = new DataTable();
            MatchCollection matchs = Regex.Matches(selectList, @"top\s+\d{1,}", RegexOptions.IgnoreCase);//含有top  
            string sqlStr = sqlStr = string.Format("select {0} from {1} where 1=1 {2}", selectList, tableName, whereStr);
            if (!string.IsNullOrEmpty(orderExpression)) { sqlStr += string.Format(" Order by {0}", orderExpression); }
            if (matchs.Count > 0) //含有top的时候  
            {
                DataTable dtTemp = ExecuteDataTable(sqlStr);
                rows = dtTemp.Rows.Count;
            }
            else //不含有top的时候  
            {
                string sqlCount = string.Format("select count(*) from {0} where 1=1 {1} ", tableName, whereStr);
                //获取行数  
                object obj = ExecuteScalar(sqlCount);
                if (obj != null)
                {
                    rows = Convert.ToInt32(obj);
                }
            }
            dt = ExecuteDataTable(sqlStr, (pageIdex - 1) * pageSize, pageSize);
            recordCount = rows;
            return dt;
        }

        #region 创建command  
        private void PrepareCommand(MySqlCommand cmd, MySqlConnection conn, MySqlTransaction trans, string cmdText, MySqlParameter[] cmdParms)
        {
            if (conn.State != ConnectionState.Open)
                conn.Open();
            cmd.Connection = conn;
            cmd.CommandText = cmdText;
            if (trans != null)
                cmd.Transaction = trans;
            cmd.CommandType = CommandType.Text;//cmdType;  
            if (cmdParms != null)
            {
                foreach (MySqlParameter parameter in cmdParms)
                {
                    if ((parameter.Direction == ParameterDirection.InputOutput || parameter.Direction == ParameterDirection.Input) &&
                    (parameter.Value == null))
                    {
                        parameter.Value = DBNull.Value;
                    }
                    cmd.Parameters.Add(parameter);
                }
            }
        }
        #endregion

    }


}
