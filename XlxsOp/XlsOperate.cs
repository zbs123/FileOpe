using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XlsxOperate
{

    public class XlsOperate
    {
        //读xlsx文件
        //
        public static List<Model.Schedule> ReadXlsxFile(string path)
        {
            List<Model.Schedule> list = new List<Model.Schedule>();
            if (string.IsNullOrEmpty(path))
            {
                return list;
            }

            IWorkbook workbook = null;  //新建IWorkbook对象  
            string fileName = "E:\\Excel2003.xls";
            FileStream fileStream = new FileStream(path, FileMode.Open, FileAccess.Read);
            if (path.IndexOf(".xlsx") > 0) // 2007版本  
            {
                workbook = new XSSFWorkbook(fileStream);  //xlsx数据读入workbook  
            }
            else if (path.IndexOf(".xls") > 0) // 2003版本  
            {
                workbook = new HSSFWorkbook(fileStream);  //xls数据读入workbook  
            }
            ISheet sheet = workbook.GetSheetAt(0);  //获取第一个工作表  
            IRow row;// = sheet.GetRow(0);            //新建当前工作表行数据  
            //课表  白
            for (int i = 6; i < sheet.LastRowNum - 1; i++)  //对工作表每一行  真实数据从第6行开始
            {
                row = sheet.GetRow(i);   //row读入第i行数据  
                if (row != null)
                {
                    //不知道为什么从16行开始，列数从53变成了52
                    if (i < 16)
                    {
                        string classAndSubject = row.Cells[1].ToString();
                        string className = classAndSubject.Substring(0, classAndSubject.Length - 2);
                        string subject = classAndSubject.Substring(classAndSubject.Length - 1);
                        int section_t = 2;
                        for (int j = 2; j < row.LastCellNum - 2; j++)  //对工作表每一列  
                        {
                            string cellValue = row.Cells[j].ToString(); //获取i行j列数据  
                            if (cellValue.Length > 4 || cellValue.Length <= 0)
                            {
                                if ((j - 1) % 10 == 0)
                                {
                                    section_t = 2;
                                    continue;
                                }
                                else
                                {
                                    section_t = 3;
                                    continue;
                                }

                            }
                            int w = (j - 2) / 10 + 1;
                            Model.Schedule schedule = new Model.Schedule();
                            schedule.ClassName = className;
                            schedule.SubjectType = subject;
                            schedule.Week = w + "";

                            //schedule.Section = j % 12 - section_t + "";
                            schedule.Section = (j - section_t) % 10 + 1 + "";
                            schedule.CourseName = GetSub(cellValue) == "" ? cellValue : GetSub(cellValue);

                            if ((j - 1) % 10 == 0)
                            {
                                section_t = 2;
                            }
                            list.Add(schedule);
                        }
                    }
                    else
                    {
                        string classAndSubject = row.Cells[0].ToString();
                        string className = classAndSubject.Substring(0, classAndSubject.Length - 2);
                        string subject = classAndSubject.Substring(classAndSubject.Length - 1);
                        int section_t = 2;
                        for (int j = 1; j < row.LastCellNum - 3; j++)  //对工作表每一列  
                        {
                            string cellValue = row.Cells[j].ToString(); //获取i行j列数据  
                            if (cellValue.Length > 4 || cellValue.Length <= 0)
                            {
                                if (j % 10 == 0)
                                {
                                    section_t = 2;
                                    continue;
                                }
                                else
                                {
                                    section_t = 3;
                                    continue;
                                }
                            }
                            j++;

                            int w = (j - 2) / 10 + 1;
                            Model.Schedule schedule = new Model.Schedule();
                            schedule.ClassName = className;
                            schedule.SubjectType = subject;
                            schedule.Week = w + "";

                            schedule.Section = (j - section_t) % 10 + 1 + "";
                            schedule.CourseName = GetSub(cellValue) == "" ? cellValue : GetSub(cellValue); 
                            // Console.WriteLine(cellValue);
                            if ((j - 1) % 10 == 0)
                            {
                                section_t = 2;
                            }
                            list.Add(schedule);
                            j--;
                        }
                    }

                }
            }
            //Console.ReadLine();
            //课表  黑
            ISheet sheet1 = workbook.GetSheetAt(1);
            for (int i = 5; i < sheet1.LastRowNum; i++)
            {
                row = sheet1.GetRow(i);   //row读入第i行数据  
                if (row != null)
                {
                    //不知道为什么从16行开始，列数从53变成了52

                    string classAndSubject = row.Cells[1].ToString();
                    string className = classAndSubject.Substring(0, classAndSubject.Length - 2);
                    string subject = classAndSubject.Substring(classAndSubject.Length - 1);
                    int section_t = 1;
                    for (int j = 2; j < row.LastCellNum - 2; j++)  //对工作表每一列  
                    {
                        string cellValue = row.Cells[j].ToString(); //获取i行j列数据  
                        Model.Schedule schedule = new Model.Schedule();

                        if (j <= 4)
                        {
                            schedule.Week = 7 + "";
                        }
                        else
                        {
                            schedule.Week = (j + 1) / 3 - 1 + "";
                        }
                        schedule.ClassName = className;
                        schedule.SubjectType = subject;


                        if (section_t == 1)
                        {
                            schedule.Section = 10 + "";
                        }
                        if (section_t == 2)
                        {
                            schedule.Section = 11 + "";
                        }
                        if (section_t == 3)
                        {
                            schedule.Section = 12 + "";
                        }
                        schedule.CourseName = GetSub(cellValue) == "" ? cellValue : GetSub(cellValue);


                        list.Add(schedule);
                        if (section_t % 3 == 0)
                        {
                            section_t = 1;
                        }
                        else
                        {
                            section_t++;

                        }
                    }

                }
            }
            fileStream.Close();
            workbook.Close();
            return list;
        }
        public static List<Model.TeacherCourse> ReadXlsxFile1(string path)
        {
            List<Model.TeacherCourse> list = new List<Model.TeacherCourse>();
            if (string.IsNullOrEmpty(path))
            {
                return list;
            }

            IWorkbook workbook = null;  //新建IWorkbook对象  
            string fileName = "E:\\Excel2003.xls";
            FileStream fileStream = new FileStream(path, FileMode.Open, FileAccess.Read);
            if (path.IndexOf(".xlsx") > 0) // 2007版本  
            {
                workbook = new XSSFWorkbook(fileStream);  //xlsx数据读入workbook  
            }
            else if (path.IndexOf(".xls") > 0) // 2003版本  
            {
                workbook = new HSSFWorkbook(fileStream);  //xls数据读入workbook  
            }
            ISheet sheet = workbook.GetSheetAt(0);  //获取第一个工作表  
            IRow row;// = sheet.GetRow(0);            //新建当前工作表行数据  
            //课表  黑
            for (int i = 1; i < sheet.LastRowNum + 1; i++)
            {
                row = sheet.GetRow(i);   //row读入第i行数据  
                if (row != null)
                {
                    string tempStr = row.Cells[2].ToString();
                    if (tempStr.IndexOf("、") != -1)
                    {
                        string[] arr = tempStr.Split('、');
                        for (int j = 0; j < arr.Length; j++)
                        {
                            Model.TeacherCourse tc = new Model.TeacherCourse();
                            tc.TeacherName = row.Cells[0].ToString(); //获取i行j列数据  
                            tc.CourseName = row.Cells[1].ToString();
                            tc.ClassName = arr[j];
                            list.Add(tc);
                        }

                    }
                    else
                    {
                        Model.TeacherCourse tc = new Model.TeacherCourse();
                        tc.TeacherName = row.Cells[0].ToString(); //获取i行j列数据  
                        tc.CourseName = row.Cells[1].ToString();
                        tc.ClassName = row.Cells[2].ToString();
                        list.Add(tc);
                    }
                }
            }
            fileStream.Close();
            workbook.Close();
            return list;
        }
        private static string GetSub(string sub)
        {
            switch (sub)
            {
                case "语":
                    return "语文";
                case "数":
                    return "数学";
                case "英":
                    return "英语";
                case "物":
                    return "物理";
                case "化":
                    return "化学";
                case "生":
                    return "生物";
                case "政":
                    return "政治";
                case "历":
                    return "历史";
                case "地":
                    return "地理";
                case "自":
                    return "自习";
                case "外自":
                    return "外语/自习";
                case "自外":
                    return "自习/外语";
                case "计":
                    return "计算机";
                case "数自":
                    return "数学/自习";
                case "化自":
                    return "化学/自习";
                case "语自":
                    return "语文/自习";
                case "物自":
                    return "物理/自习";
                case "生自":
                    return "生物/自习";
                case "历自":
                    return "历史/自习";
                case "地自":
                    return "地理/自习";
                case "政自":
                    return "政治/自习";
                default:
                    break;
            }
            return "";
        }
        //写xlsx文件
        public static void WriteXlsxFile(List<Model.Schedule> slist)
        {
            //HSSF可以读取xls格式的Excel文件
            //IWorkbook workbook = new HSSFWorkbook();
            //XSSF可以读取xlsx格式的Excel文件
            IWorkbook workbook = new XSSFWorkbook();

            var grouplist = slist.GroupBy(s => new { s.ClassName });
            foreach (var item in grouplist)
            {
                List<Model.Schedule> list = item.ToList();
                ISheet sheet = workbook.CreateSheet(item.Key.ClassName + "班");
                //创建行
                IRow row = sheet.CreateRow(0); //i表示了创建行的索引，从0开始
                                               //创建单元格
                row.CreateCell(0).SetCellValue("学校id");
                row.CreateCell(1).SetCellValue("年级id");
                row.CreateCell(2).SetCellValue("班级id");
                row.CreateCell(3).SetCellValue("班级");
                row.CreateCell(4).SetCellValue("老师id");
                row.CreateCell(5).SetCellValue("周");
                row.CreateCell(6).SetCellValue("节");
                row.CreateCell(7).SetCellValue("课程");
                row.CreateCell(8).SetCellValue("课程id");
                row.CreateCell(9).SetCellValue("科目");
                for (int i = 0; i < list.Count; i++)
                {
                    row = sheet.CreateRow(i+1); //i表示了创建行的索引，从0开始
                                                   //创建单元格
                    row.CreateCell(0).SetCellValue("30E0E3D1341049D39AD513E854836AC1 ");
                    row.CreateCell(1).SetCellValue("AE89E4170D774DD28765463D94B35E2D");
                    row.CreateCell(2).SetCellValue(GetClassId(item.Key.ClassName));
                    row.CreateCell(3).SetCellValue(list[i].ClassName);
                    row.CreateCell(4).SetCellValue(list[i].TeacherName);
                    row.CreateCell(5).SetCellValue(list[i].Week);
                    row.CreateCell(6).SetCellValue(list[i].Section);
                   
                    if (list[i].CourseName.IndexOf('/') != -1)
                    {
                        string[] temp = list[i].CourseName.Split('/');
                        //row.CreateCell(7).SetCellValue(list[i].CourseName);

                        //row.CreateCell(8).SetCellValue((GetCourseId(temp[0]) == "" ? "" : GetCourseId(temp[0]))+"/"+(GetCourseId(temp[1]) == "" ? "" : GetCourseId(temp[1])));
                        row.CreateCell(7).SetCellValue(temp[0]);

                        row.CreateCell(8).SetCellValue(GetCourseId(temp[0]) == "" ? "" : GetCourseId(temp[0]));

                    }
                    else
                    {
                        row.CreateCell(7).SetCellValue(list[i].CourseName);

                        row.CreateCell(8).SetCellValue(GetCourseId(list[i].CourseName) == "" ? "" : GetCourseId(list[i].CourseName));

                    }
                    row.CreateCell(9).SetCellValue(list[i].SubjectType);
                    //for (int j = 0; j < 6; j++)
                    //{
                    //    ICell cell = row.CreateCell(j);  //同时这个函数还有第二个重载，可以指定单元格存放数据的类型
                    //    cell.SetCellValue(i.ToString() + j.ToString());
                    //}
                }

            }
            //Excel文件至少要有一个工作表sheet

            //表格制作完成后，保存
            //创建一个文件流对象
            using (FileStream fs = File.Open(@"D:\asp.net\workspace\test_beta_2_dan.xlsx", FileMode.OpenOrCreate))
            {
                workbook.Write(fs);
                //最后记得关闭对象
                workbook.Close();
            }

        }
        public static string GetClassId(string ClassName)
        {
            switch (ClassName)
            {
                case "1":
                    return "5833a7a6c4ad4ab0a6fc9fae30eee77e";
                case "2":
                    return "611908994b734f91b67c1b933ab81239";
                case "3":
                    return "74e663c8dc8c47c6a93b12d6feb50fd7";
                case "4":
                    return "76581023a8bf44b4a660dcdc4e1329a9";
                case "5":
                    return "00f893a0344c4da59204c2d0bd49d5b0";
                case "6":
                    return "37ee880dfbaf44e1a7655a33a75b4739";
                case "7":
                    return "6bdbee27b33a44f5b77d6cc509dcb4a2";
                case "8":
                    return "ba9bf0c15ab94b9a8de5653ecb885f86";
                case "9":
                    return "5ded3cea195d42d38e2b4ceeb7afe9f6";
                case "10":
                    return "c7791308a2c645d09ee44e17081b56f6";
                case "11":
                    return "8c2b017139714a4e853d297f0d5b23b2";
                case "12":
                    return "28c5f32250c343f295c4c08e4c51308d";
                case "13":
                    return "9951a37a49ab430f964e7bb1c57816e3";
                case "14":
                    return "4a70c86ef4d7478cbf35727c24b8be56";
                case "15":
                    return "7db1fd1785394781bc991b5c067cc696";
                case "16":
                    return "c9cae7178aba4cdfbd736c5b0e714ed5";
                case "17":
                    return "60e2a52c0a7c466895da84d979e4e39d";
                case "18":
                    return "6a9d6b7f8fc54f4fbc5bf1d8d6c8d6c3";
                case "19":
                    return "46141da42bfa4640862caf3a1498d5a9";
                case "20":
                    return "2256966127f54d81bf259760f7ea5bcb";
                case "21":

                    return "42715f454d344916b3e72b2f1fc94470";
                case "22":
                    return "c1aa1346ec484734935a2576ef978325";
                default:
                    break;
            }
            return "";
        }
        public static string GetCourseId(string courseName)
        {
            switch (courseName)
            {
                case "语文":
                    return "01";
                case "数学":
                    return "10";
                case "理科数学":
                    return "11";
                case "文科数学":
                    return "12";
                case "英语":
                    return "21";
                case "物理":
                    return "31";
                case "化学":
                    return "32";
                case "生物":
                    return "33";
                case "政治":
                    return "43";
                case "历史":
                    return "41";
                case "地理":
                    return "42";
               
                default:
                    break;
            }
            return "";
        }
    }
}

