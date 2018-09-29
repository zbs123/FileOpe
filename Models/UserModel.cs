using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    /// <summary>
    /// 登录用户基础信息
    /// </summary>
    public class UserModel
    {
        /// <summary>
        /// 用户Guid
        /// </summary>
        public string RI_UserId { get; set; }
        /// <summary>
        /// 用户名
        /// </summary>
        public string RI_UserName { get; set; }

        /// <summary>
        /// 学号
        /// </summary>
        public string ri_schoolId { get; set; }
        /// <summary>
        /// 姓名
        /// </summary>
        public string RI_RealName { get; set; }

    }
    /// <summary>
    /// 学生信息Models
    /// </summary>
    public class StudentModel
    {
        /// <summary>
        /// 编号
        /// </summary>
        public string RI_ID { get; set; }
        /// <summary>
        /// 用户Guid
        /// </summary>
        public string RI_UserId { get; set; }
        /// <summary>
        /// 用户名
        /// </summary>
        public string RI_UserName { get; set; }
        /// <summary>
        /// 考生号
        /// </summary>
        public string RI_ExamCode { get; set; }
        /// <summary>
        /// 班级Guid
        /// </summary>
        public string RI_ClassId { get; set; }
        /// <summary>
        /// 学号
        /// </summary>
        public string RI_StudentId { get; set; }
        /// <summary>
        /// 姓名
        /// </summary>
        public string RI_RealName { get; set; }

    }
    public class TeacherModel
    {
        /// <summary>
        /// 编号
        /// </summary>
        public string RI_ID { get; set; }
        /// <summary>
        /// 用户Guid
        /// </summary>
        public string RI_UserId { get; set; }
        /// <summary>
        /// 用户名
        /// </summary>
        public string RI_UserName { get; set; }
 
        /// <summary>
        /// 学号
        /// </summary>
        public string RI_StudentId { get; set; }
        /// <summary>
        /// 姓名
        /// </summary>
        public string RI_RealName { get; set; }
        /// <summary>
        /// 手机号
        /// </summary>
        public string RI_Tel { get; set; }
    }
}
