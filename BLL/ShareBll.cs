using Dal;
using DAL;
using Models;
using System;
using System.Collections.Generic;
using System.Data;
using Newtonsoft.Json;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Net.Http;
using System.Web;
using Commonn;

namespace BLL
{
    public class ShareBll
    {
        private Selete sel;
        private Insert ins;
        private Update up;
        private Delete del;
        private ShareDal sharedal;
        public ShareBll()
        {
            sel = new Selete();
            ins = new Insert();
            up = new Update();
            del = new Delete();
            sharedal = new ShareDal();
        }
        //获取部门人
        public List<Role_User> GetUser(string schoolid,string uid)
        {
           
            return sel.GetDeptUsers(schoolid,uid);
        }
        //部门人搜索
        public List<Role_User> GetSearchUser(string schoolid, string search)
        {
            //List<Role_User> list = new List<Role_User>();
            ////DataTable dt = sel.GetRole(schoolid);
            //DataTable dt = sel.GetDeptUsers(schoolid);
            //if (dt.Rows.Count > 0)
            //{
            //    for (int i = 0; i < dt.Rows.Count; i++)
            //    {
            //        Role_User ru = new Role_User();
            //        ru.Rolename = dt.Rows[i]["RI_DepartmentName"].ToString();
            //        if (dt.Rows[i]["RI_TeacherList"].ToString().Length > 0)
            //        {
            //            List<User> JObject = JsonConvert.DeserializeObject<List<User>>(dt.Rows[i]["RI_TeacherList"].ToString());
            //            ru.Users = JObject.Where(x => x.Name.Contains(search)).ToList();
            //        }
            //        list.Add(ru);
            //    }
            //}
            return sel.GetDeptUsers(schoolid);
        }
        public List<FileOp> MyShare(string userid)
        {
            List<FileOp> list = new List<FileOp>();
            DataTable dt = sel.MyShare(userid);
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    FileOp fo = new FileOp();
                    string filepath = dt.Rows[i]["jw_filepath"].ToString();
                    fo.Date = dt.Rows[i]["jw_createtime"].ToString();
                    fo.Shareid = dt.Rows[i]["jw_shareid"].ToString();
                    string userids = dt.Rows[i]["jw_UsersId"].ToString();
                    string usersid = string.Empty;
                    if (userids.IndexOf(",") > 0)
                    {
                        string[] strarr = userids.Split(',');
                        foreach (var item in strarr)
                        {
                            usersid += "'" + item + "',";
                        }
                        usersid = usersid.Substring(0, usersid.Length - 1);
                    }
                    else
                    {
                        usersid = "'" + userids + "'";
                    }
                    DataTable udt = sel.GetUsersByUserids(usersid);
                    string uname = string.Empty;
                    if (udt.Rows.Count > 0)
                    {
                        for (int k = 0; k < udt.Rows.Count; k++)
                        {
                            uname += udt.Rows[k][0].ToString() + ",";
                        }
                        uname = uname.Substring(0, uname.Length - 1);

                    }
                    fo.Username = uname;
                    //List<string> li = JsonConvert.DeserializeObject<List<string>>(filepath);
                    List<string> li = filepath.Split(',').ToList();
                    if (li.Count == 1)
                    {
                        fo.FileUrl = li[0];

                        string path = AppDomain.CurrentDomain.BaseDirectory + li[0];
                        if (File.Exists(path))
                        {
                            FileInfo fi = new FileInfo(path);
                            fo.Name = fi.Name;
                            fo.Size = Commonn.FileOperate.GetFileSize(fi.Length);
                            fo.Type = path.Substring(path.LastIndexOf('.') + 1);
                        }
                        else if (Directory.Exists(path))
                        {
                            DirectoryInfo di = new DirectoryInfo(path);
                            fo.Name = di.Name;
                            fo.Type = "1";
                            fo.Size = "--";
                        }
                    }
                    else if (li.Count > 1)
                    {
                        fo.Type = "3";
                        fo.FileUrl = JsonConvert.SerializeObject(li.ToArray());
                        fo.Name = li[0].Substring(li[0].LastIndexOf('/') + 1) + "等多个文件";
                        fo.Size = "--";
                    }
                    if (string.IsNullOrEmpty(fo.Name))
                    {
                        continue;
                    }
                    list.Add(fo);
                }
            }
            return list;
        }

        public List<FileOp> MyReceive(string userid)
        {
            List<FileOp> list = new List<FileOp>();
            DataTable dt = sel.ToMyShare(userid);
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    FileOp fo = new FileOp();
                    string filepath = dt.Rows[i]["jw_filepath"].ToString();
                    fo.Date = dt.Rows[i]["jw_createtime"].ToString();
                    fo.Shareid = dt.Rows[i]["jw_shareid"].ToString();
                    fo.Userid = dt.Rows[i]["jw_userid"].ToString();
                    DataTable dtt = sel.GetUserByUserid(fo.Userid);
                    if(dtt.Rows.Count>0)
                    {
                        fo.Username = dtt.Rows[0]["ri_realname"].ToString();
                    }
                    else
                    {
                        fo.Username = "";
                    }
                    
                    //List<string> li = JsonConvert.DeserializeObject<List<string>>(filepath);
                    List<string> li = filepath.Split(',').ToList();
                    if (li.Count == 1)
                    {
                        fo.FileUrl = li[0];

                        string path = AppDomain.CurrentDomain.BaseDirectory + li[0];
                        if (File.Exists(path))
                        {
                            FileInfo fi = new FileInfo(path);
                            fo.Name = fi.Name;
                            fo.Size = FileOperate.GetFileSize(fi.Length);
                            fo.Type = path.Substring(path.LastIndexOf('.') + 1);
                        }
                        else if (Directory.Exists(path))
                        {
                            DirectoryInfo di = new DirectoryInfo(path);
                            fo.Name = di.Name;
                            fo.Type = "1";
                            fo.Size = "--";
                        }
                    }
                    else if (li.Count > 1)
                    {
                        fo.Type = "3";
                        fo.FileUrl = JsonConvert.SerializeObject(li.ToArray());
                        fo.Name = li[0].Substring(li[0].LastIndexOf('/') + 1) + "等多个文件";
                        fo.Size = "--";
                    }
                    if (string.IsNullOrEmpty(fo.Name))
                    {
                        continue;
                    }
                    list.Add(fo);
                }
            }
            return list;
        }
        public bool UpdateShare(Jw_share js)
        {
            return up.UpdateShareDel(js);
        }
        public int AddShare(Jw_share js, List<Jw_share_file> list)
        {
            try
            {
                ins.AddShare(js);
                foreach (var item in list)
                {
                    ins.AddShareFile(item);
                }
                return 1;
            }
            catch (Exception)
            {
                return -1;
                throw;
            }


        }
        public int AddMycycle(Jw_recycle jr, Jw_share_file jsf)
        {
            UpdateShareFileStatus(jsf);
            return ins.AddRecycle(jr);
        }
        public bool UpdateShareFileStatus(Jw_share_file jsf)
        {
            return up.UpdateShareFileStatus(jsf);
        }
        public bool UpdateShareFilePath(Jw_share_file jsf, string str)
        {
            return up.UpdateShareFilePath(jsf, str);
        }
        public List<FileOp> GetMycycle(string userid)
        {
            List<FileOp> list = new List<FileOp>();
            DataTable dt = sel.GetRecycle(userid);
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    if (!string.IsNullOrEmpty(dt.Rows[i]["jw_realname"].ToString()))
                    {
                        FileOp fo = new FileOp();
                        fo.Name = dt.Rows[i]["jw_realname"].ToString();
                        fo.Type = dt.Rows[i]["jw_type"].ToString();
                        fo.Size = dt.Rows[i]["jw_size"].ToString();
                        fo.Shareid = dt.Rows[i]["jw_recycleid"].ToString();
                        fo.Date = dt.Rows[i]["jw_deltime"].ToString();
                        list.Add(fo);
                    }
                   
                }
            }
            return list;
        }
        //还原
        public DataTable GetRecycleByid(string recycleid)
        {
            return sel.GetRecycleByid(recycleid); ;
        }
        public int DelRecycleByid(string recycleid)
        {
            return del.DelRecycleByid(recycleid);
        }
        public int DelRecycleAll(string userid)
        {
            return del.DelRecycleAll(userid);
        }
        //shareid===》filepath
        public List<string> GetFilepathByShareid(string shareid)
        {
            List<string> list = new List<string>();
            DataTable dt = sel.FileByShareid(shareid);
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    list.Add(dt.Rows[i]["jw_filepath"].ToString());
                }
            }
            return list;
        }
        //2018-6-4
        public int AddFile(Jw_file jf)
        {
            return ins.AddFile(jf);
        }
        public string GetParentFileUrl(string id)
        {
            DataTable dt = sel.GetParentFileUrl(id);
            if (dt.Rows.Count > 0)
            {
                return dt.Rows[0][0].ToString();
            }
            return "";
        }
        public UserModel GetUserById(string id)
        {
            DataTable dt = sel.GetUserById(id);
            if (dt.Rows.Count > 0)
            {
                return new UserModel
                {
                    RI_UserId = dt.Rows[0]["RI_UserId"].ToString(),
                    ri_schoolId = dt.Rows[0]["ri_schoolId"].ToString(),
                    RI_RealName = dt.Rows[0]["RI_RealName"].ToString(),
                    RI_UserName = dt.Rows[0]["RI_UserName"].ToString()
                };
            }
            return null;
        }
    }
}
