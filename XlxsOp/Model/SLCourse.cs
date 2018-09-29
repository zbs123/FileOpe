using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XlsxOperate.Model
{
   public class SLCourse
    {
        /// <summary>
        /// 教师姓名
        /// </summary>
        public string CourseId { get; set; }
        /// <summary>
        /// 科目
        /// </summary>
        public string CourseName { get; set; }
        /// <summary>
        /// 上课班级
        /// </summary>
        public string TeacherName { get; set; }
        public string ClassName { get; set; }
        public string CourseTime { get; set; }
        public string Students { get; set; }

    }
}
