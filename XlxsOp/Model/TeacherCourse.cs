using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XlsxOperate.Model
{
   public class TeacherCourse
    {
        /// <summary>
        /// 教师姓名
        /// </summary>
        public string TeacherName { get; set; }
        /// <summary>
        /// 科目
        /// </summary>
        public string CourseName { get; set; }
        /// <summary>
        /// 上课班级
        /// </summary>
        public string ClassName { get; set; }
    }
}
