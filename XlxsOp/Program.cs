using DAL;
using DB;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using XlsxOperate.Model;

namespace XlsxOperate
{
    class Program
    {

        static void Main(string[] args)
        {
            IBase _cb = new IBase();
            MySQLDBHelper db = new MySQLDBHelper();
            //db.ConnStr = "sl";
            List<SLCourse> list= XlsxOpe.ReadSLCourseXlsx(@"C:\Users\admin\Desktop\2017级第2期课表4.16.xlsx", 0, 6, 0,(cname,type)=> {
                string str = string.Empty;
                if (cname.Length < 3)
                {
                    cname = "0" + cname;
                }
                if (type == "1")//学生
                {
                    DataTable dt = db.ExecuteDataTable("select u.ri_realname,u.ri_studentid,u.ri_realname from map_classes m left join ri_user u on m.ri_classid=u.ri_classid where m.ri_classname='" + cname + "' and m.ri_gradeid='G2017' and m.ri_delflag='0' and u.ri_delflag='0'");
                    if (dt.Rows.Count > 0)
                    {
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            str += dt.Rows[i][0].ToString()+"/" + dt.Rows[i][1].ToString()+ ",";
                        }
                       str = str.Substring(0, str.Length - 1);
                   }
                    
                }
                if (type == "2")
                {
                    DataTable dt = db.ExecuteDataTable("select u.ri_realname,u.ri_tel from map_classes m left join ri_user u on m.ri_teacher=u.ri_userid where m.ri_classname='" + cname + "' and m.ri_gradeid='G2017' and m.ri_delflag='0' and u.ri_delflag='0'");
                    if (dt.Rows.Count > 0)
                    {
                        str = dt.Rows[0][0].ToString() + "/" + dt.Rows[0][1].ToString();
                    }
                }
                return str;
            });
            XlsxOpe.WriteXlsxFile(list);
            /*XlsxOpe.ReadXlsx1(@"D:\工作夹\单科试卷得分明细表110 - 副本.xls", 1, 2, 0, (rname,ac) =>
            {
                //db.ConnStr = "connStr";
                if (rname == "罗壁嘉华")
                {
                    rname = "罗璧嘉华";
                }
                if(rname== "蔡沁坤")
                {
                    rname = "花生米";
                }
                if (rname == "陈冠廷")
                {
                    rname = "陈冠西";
                }
                if (rname == "赖楚汉")
                {
                    rname = "路星河";
                }
                if (rname == "文阅政")
                {
                    rname = "文跃帧";
                }
                if (rname == "车林嵘")
                {
                    rname = "车";
                }
                if (rname == "高航1"|| rname == "高航2")
                {
                    rname = "高航";
                }
                DataTable dt= db.ExecuteDataTable("select ri_examcode,c.RI_ClassName from ri_user u LEFT JOIN map_classes c ON u.RI_ClassId=c.RI_ClassId where ri_realname like '%" + rname+ "%' and u.ri_type='0' and u.ri_gradeid='G2017' and u.ri_delflag='0'");
                if (dt.Rows.Count > 0)
                {
                    ac(dt.Rows[0]["RI_ClassName"].ToString());
                    if (rname == "高航1")
                    {
                        return "510117300311";
                    }
                    if (rname == "高航2")
                    {
                        return "510117300312";
                    }
                    return dt.Rows[0]["ri_examcode"].ToString();
                }
                return "";
            });*/
            //IBase _cb = new IBase();
            //MySQLDBHelper db = new MySQLDBHelper();

            ////课表数据
            //List<Model.Schedule> slist = XlsOperate.ReadXlsxFile(@"D:\asp.net\workspace\2017级第2期课表3.5 - 副本.xlsx");
            ////教师上课对应班级数据
            //List<Model.TeacherCourse> tlist = XlsOperate.ReadXlsxFile1(@"D:\asp.net\workspace\教室上课对应班级表 - 副本 - 副本.xlsx");
            ////合并数据
            //for (int i = 0; i < slist.Count; i++)
            //{
            //    string tname = "";
            //    if (slist[i].CourseName.IndexOf('/') != -1)
            //    {
            //        string[] temp = slist[i].CourseName.Split('/');

            //        var tc0 = tlist.Find(t => t.ClassName.Equals(slist[i].ClassName) && t.CourseName.Equals(temp[0]));
            //        var tc1 = tlist.Find(t => t.ClassName.Equals(slist[i].ClassName) && t.CourseName.Equals(temp[1]));
            //        tname = (tc0 == null ? "" : tc0.TeacherName) + "/" + (tc1 == null ? "" : tc1.TeacherName);
            //        slist[i].TeacherName = tname;
            //        if (slist[i].CourseName.IndexOf("数学")!=-1)
            //        {
            //            slist[i].CourseName = slist[i].CourseName.Replace("数学", slist[i].SubjectType + "科" + slist[i].CourseName); 
            //        }
            //        continue;
            //    }

            //    var tc = tlist.Find(t => t.ClassName.Equals(slist[i].ClassName) && t.CourseName.Equals(slist[i].CourseName));
            //    tname = tc == null ? "" : tc.TeacherName;
            //    slist[i].TeacherName = tname;
            //    if (slist[i].CourseName == "数学")
            //    {
            //        slist[i].CourseName = slist[i].SubjectType + "科" + slist[i].CourseName;
            //    }
            //}

            ////XlsOperate.WriteXlsxFile(slist);
            //DateTime dt = Convert.ToDateTime("2018-3-5");
            //DateTime dt1 = Convert.ToDateTime("2018-7-31");
            //int flag = 1;
            //int type = 1;
            //while (dt < dt1)
            //{

            //    List<Model.Map_Coursegroup_Beta> mcblist = GetOneData(GetData(slist, type), dt);
            //    for (int i = 0; i < mcblist.Count; i++)
            //    {
            //        db.ConnStr = "connStr";
            //        string sql = _cb.InsertModel<Model.Map_Coursegroup_Beta>(mcblist[i]);
            //        db.ExecuteNonQuery(sql);
            //    }
            //    dt = dt.AddDays(1);

            //    if (flag % 7 == 0)
            //    {
            //        type = -type;
            //        flag = 1;
            //    }
            //    else
            //    {
            //        flag++;
            //    }

            //}


        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="list">源数据</param>
        /// <param name="zhou">单1双2周</param>
        static List<Model.Schedule> GetData(List<Model.Schedule> list, int zhou)
        {
            List<Model.Schedule> l = Clone<List<Model.Schedule>>(list);
            for (int i = 0; i < l.Count; i++)
            {
                if (l[i].CourseName.IndexOf('/') != -1)
                {
                    string[] temp = l[i].CourseName.Split('/');
                    string[] temp_t = l[i].TeacherName.Split('/');
                    if (zhou == 1)
                    {
                        l[i].TeacherName = temp_t[0];
                        l[i].CourseName = temp[0];

                    }
                    if (zhou == -1)
                    {
                        l[i].TeacherName = temp_t[1];
                        l[i].CourseName = temp[1];

                    }
                }
            }
            return l;
        }
        static List<Model.Map_Coursegroup_Beta> GetOneData(List<Model.Schedule> list, DateTime dtime)
        {
            List<Model.Map_Coursegroup_Beta> mcblist = new List<Model.Map_Coursegroup_Beta>();
            var selectList = list.FindAll(l => l.Week == (dtime.DayOfWeek.ToString("d") == "0" ? "7" : dtime.DayOfWeek.ToString("d")));
            for (int i = 0; i < selectList.Count; i++)
            {
                if (XlsOperate.GetCourseId(selectList[i].CourseName) == "")
                {
                    continue;
                }
                Model.Map_Coursegroup_Beta mcb = new Model.Map_Coursegroup_Beta();
                mcb.ri_schoolid = "30E0E3D1341049D39AD513E854836AC1";
                mcb.ri_gradeid = "AE89E4170D774DD28765463D94B35E2D";
                mcb.ri_classid = XlsOperate.GetClassId(selectList[i].ClassName);
                mcb.ri_teacherid = selectList[i].TeacherName;
                mcb.ri_subject = 0;
                mcb.ri_courseid = XlsOperate.GetCourseId(selectList[i].CourseName) == "" ? "" : XlsOperate.GetCourseId(selectList[i].CourseName);
                mcb.ri_week = Convert.ToInt16(selectList[i].Week);
                mcb.ri_number = Convert.ToInt16(selectList[i].Section);
                mcb.ri_scheduletime = dtime;
                mcb.ri_createTime = dtime;
                mcb.ri_updatetime = DateTime.Now;
                mcb.ri_synflag = 0;
                mcb.ri_delflag = 0;
                mcblist.Add(mcb);
            }
            return mcblist;
        }
        public static T Clone<T>(T RealObject)

        {
            using (Stream objectStream = new MemoryStream())
            {
                //利用 System.Runtime.Serialization序列化与反序列化完成引用对象的复制 
                IFormatter formatter = new BinaryFormatter();
                formatter.Serialize(objectStream, RealObject);
                objectStream.Seek(0, SeekOrigin.Begin);
                return (T)formatter.Deserialize(objectStream);
            }
        }
    }
}
