using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace XlsxOperate.Model
{
    [Serializable]
   public class Schedule:ICloneable
    {
        /// <summary>
        /// 班级
        /// </summary>
        public string ClassName { get; set; }
        /// <summary>
        /// 老师
        /// </summary>
        public string TeacherName { get; set; }
        /// <summary>
        /// 周数
        /// </summary>
        public string Week { get; set; }
        /// <summary>
        /// 节数
        /// </summary>
        public string Section { get; set; }
        /// <summary>
        /// 课程
        /// </summary>
        public string CourseName { get; set; }
        /// <summary>
        /// 科目
        /// </summary>
        public string SubjectType { get; set; }

        public object Clone()
        {
            BinaryFormatter formatter = new BinaryFormatter(null, new System.Runtime.Serialization.StreamingContext(System.Runtime.Serialization.StreamingContextStates.Clone));
            MemoryStream stream = new MemoryStream();
            formatter.Serialize(stream, this);
            stream.Position = 0;
            object clonedObj = formatter.Deserialize(stream);
            stream.Close();
            return clonedObj;
        }
    }
}
