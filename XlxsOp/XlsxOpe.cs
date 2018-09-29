using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using NPOI;
using NPOI.SS.UserModel;
using System.IO;
using NPOI.XSSF.UserModel;
using NPOI.HSSF.UserModel;
using System.Collections;
using XlsxOperate.Model;

namespace XlsxOperate
{
    public class XlsxOpe
    {
        /// <summary>
        /// 将一行作为一个对象操作
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="path">xlsx文件路径</param>
        /// <param name="sheetnum">表从0开始</param>
        /// <param name="rownum">行</param>
        /// <param name="cellnum">列</param>
        /// <param name="Fun">操作</param>
        /// <returns></returns>
        public static List<T> ReadXlsx<T>(string path, int sheetnum, int rownum, int cellnum, Func<IRow, T> Fun)
        {
            List<T> list = new List<T>();
            if (string.IsNullOrEmpty(path))
            {
                return list;
            }

            IWorkbook workbook = null;  //新建IWorkbook对象  
            FileStream fileStream = new FileStream(path, FileMode.Open, FileAccess.Read);
            if (path.ToLower().IndexOf(".xlsx") > 0) // 2007版本  
            {
                workbook = new XSSFWorkbook(fileStream);  //xlsx数据读入workbook  
            }
            else if (path.ToLower().IndexOf(".xls") > 0) // 2003版本  
            {
                workbook = new HSSFWorkbook(fileStream);  //xls数据读入workbook  
            }
            ISheet sheet = workbook.GetSheetAt(sheetnum);  //获取第一个工作表  
            IRow row;// = sheet.GetRow(0);            //新建当前工作表行数据  
            //课表  白
            for (int i = rownum; i < sheet.LastRowNum; i++)  //对工作表每一行  真实数据从第6行开始
            {
                var o = Fun(sheet.GetRow(i));
                if (o == null)
                {
                    continue;
                }
                list.Add(o);
            }
            return list;
        }
        /// <summary>
        /// 将一行作为一个对象操作
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="path">xlsx文件路径</param>
        /// <param name="sheetnum">表从0开始</param>
        /// <param name="rownum">行</param>
        /// <param name="cellnum">列</param>
        /// <param name="Fun">操作</param>
        /// <returns></returns>
        public static List<T> ReadXlsx<T>(string path, int sheetnum, int rownum, int cellnum, Func<IRow, string, Func<ICell, string>, T> Fun, Func<IRow, string> Fun1)
        {
            List<T> list = new List<T>();
            if (string.IsNullOrEmpty(path))
            {
                return list;
            }

            IWorkbook workbook = null;  //新建IWorkbook对象  
            FileStream fileStream = new FileStream(path, FileMode.Open, FileAccess.Read);
            if (path.ToLower().IndexOf(".xlsx") > 0) // 2007版本  
            {
                workbook = new XSSFWorkbook(fileStream);  //xlsx数据读入workbook  
            }
            else if (path.ToLower().IndexOf(".xls") > 0) // 2003版本  
            {
                workbook = new HSSFWorkbook(fileStream);  //xls数据读入workbook  
            }
            ISheet sheet = workbook.GetSheetAt(sheetnum);  //获取第一个工作表  
            IRow row;// = sheet.GetRow(0);            //新建当前工作表行数据  

            //课表  白
            for (int i = rownum; i < sheet.LastRowNum + 1; i++)  //对工作表每一行  真实数据从第6行开始
            {
                var o = Fun(sheet.GetRow(i), Fun1(sheet.GetRow(1)), (c) =>
                {
                    switch (c.CellType)
                    {
                        case CellType.Numeric:
                            return c.NumericCellValue.ToString();
                        case CellType.Blank:
                            return null;
                        case CellType.Formula:
                            return null;
                        //if (Path.GetExtension(strFileName).ToLower().Trim() == ".xlsx")
                        //{
                        //    XSSFFormulaEvaluator eva = new XSSFFormulaEvaluator(hssfworkbook);
                        //    if (eva.Evaluate(row.GetCell(j)).CellType == CellType.NUMERIC)
                        //    {
                        //        if (HSSFDateUtil.IsCellDateFormatted(row.GetCell(j)))//日期类型
                        //        {
                        //            itemArray[j] = row.GetCell(j).DateCellValue.ToString("yyyy-MM-dd");
                        //        }
                        //        else//其他数字类型
                        //        {
                        //            itemArray[j] = row.GetCell(j).NumericCellValue;
                        //        }
                        //    }
                        //    else
                        //    {
                        //        itemArray[j] = eva.Evaluate(row.GetCell(j)).StringValue;
                        //    }
                        //}
                        //else
                        //{
                        //    HSSFFormulaEvaluator eva = new HSSFFormulaEvaluator(hssfworkbook);
                        //    if (eva.Evaluate(row.GetCell(j)).CellType == CellType.NUMERIC)
                        //    {
                        //        if (HSSFDateUtil.IsCellDateFormatted(row.GetCell(j)))//日期类型
                        //        {
                        //            itemArray[j] = row.GetCell(j).DateCellValue.ToString("yyyy-MM-dd");
                        //        }
                        //        else//其他数字类型
                        //        {
                        //            itemArray[j] = row.GetCell(j).NumericCellValue;
                        //        }
                        //    }
                        //    else
                        //    {
                        //        itemArray[j] = eva.Evaluate(row.GetCell(j)).StringValue;
                        //    }
                        //}
                        //break;
                        default:
                            return System.Text.RegularExpressions.Regex.Replace(c.StringCellValue, @"\s", "").Length == 0 ? null : c.StringCellValue;

                    }
                });
                if (o == null)
                {
                    continue;
                }
                list.Add(o);
            }
            return list;
        }
        public static void ReadXlsx1(string path, int sheetnum, int rownum, int cellnum, Func<string, Action<string>, string> Fun)
        {
            FileStream fs = null;
            IWorkbook workbook = null;  //新建IWorkbook对象  
            FileStream fileStream = new FileStream(path, FileMode.Open, FileAccess.Read);
            if (path.ToLower().IndexOf(".xlsx") > 0) // 2007版本  
            {
                workbook = new XSSFWorkbook(fileStream);  //xlsx数据读入workbook  
            }
            else if (path.ToLower().IndexOf(".xls") > 0) // 2003版本  
            {
                workbook = new HSSFWorkbook(fileStream);  //xls数据读入workbook  
            }
            ISheet sheet;  //获取第一个工作表  
            IRow row;// = sheet.GetRow(0);            //新建当前工作表行数据  
            for (int k = 0; k < 12; k++)
            {
                sheet = workbook.GetSheetAt(k + 1);  //获取第一个工作表  
                for (int i = rownum; i < sheet.LastRowNum + 1; i++)
                {
                    row = sheet.GetRow(i);
                    for (int j = 0; j < row.LastCellNum; j++)
                    {
                        ICell cell = row.GetCell(j, MissingCellPolicy.CREATE_NULL_AS_BLANK);
                    }
                }
                //课表  白
                for (int i = rownum; i < sheet.LastRowNum + 1; i++)  //对工作表每一行  真实数据从第6行开始
                {
                    row = sheet.GetRow(i);
                    row.Cells[0].SetCellValue("高一");
                    var o = Fun(row.Cells[4].ToString(), (cname) =>
                    {
                        row.Cells[1].SetCellValue(cname);
                    });
                    if (!string.IsNullOrEmpty(o))
                    {
                        row.Cells[2].SetCellValue(o);
                        row.Cells[3].SetCellValue(o);
                    }


                }
            }


            using (fs = File.OpenWrite(@"D:/myxls1.xls"))
            {
                workbook.Write(fs);//向打开的这个xls文件中写入数据  

            }

        }
        /// <summary>
        /// 将一行作为一个对象操作
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="path">xlsx文件路径</param>
        /// <param name="sheetnum">表从0开始</param>
        /// <param name="rownum">行</param>
        /// <param name="cellnum">列</param>
        /// <param name="Fun">操作</param>
        /// <returns></returns>
        public static List<SLCourse> ReadSLCourseXlsx(string path, int sheetnum, int rownum, int cellnum, Func<string, string, string> func)
        {
            List<SLCourse> list = new List<SLCourse>();
            IWorkbook workbook = null;  //新建IWorkbook对象  
            FileStream fileStream = new FileStream(path, FileMode.Open, FileAccess.Read);
            if (path.ToLower().IndexOf(".xlsx") > 0) // 2007版本  
            {
                workbook = new XSSFWorkbook(fileStream);  //xlsx数据读入workbook  
            }
            else if (path.ToLower().IndexOf(".xls") > 0) // 2003版本  
            {
                workbook = new HSSFWorkbook(fileStream);  //xls数据读入workbook  
            }
            ISheet sheet;  //获取第一个工作表  
            IRow row;// = sheet.GetRow(0);            //新建当前工作表行数据  

            sheet = workbook.GetSheetAt(sheetnum);  //获取第一个工作表  
            for (int i = rownum; i < sheet.LastRowNum; i++)
            {
                int week = 1;
                int section = 0;
                Dictionary<string, string> cTime = new Dictionary<string, string>();
                row = sheet.GetRow(i);
                string classname = row.GetCell(56, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString() + "班";
                for (int j = 2; j < 56; j++)
                {
                    section++;
                    ICell cell = row.GetCell(j, MissingCellPolicy.CREATE_NULL_AS_BLANK);
                    string courseName = cell.ToString().Trim();
                    //if (section>=9|| courseName.Length == 0)//空格周+1
                    if (section>9)//空格周+1
                    {
                        week++;
                        section = 0;
                    }
                    else if (courseName.Length > 3|| courseName.Length == 0)//长度大于3的是大课间
                    {
                        if (section == 9) continue;
                        section--;
                    }
                    else
                    {
                        if (cTime.ContainsKey(courseName))
                        {
                            if(cTime[courseName].IndexOf("周" + week) != -1)
                            {
                                cTime[courseName] = cTime[courseName].Substring(0, cTime[courseName].Length - 2);
                                cTime[courseName] += "," +section + "节;";
                            }
                            else
                            {
                                cTime[courseName] += "周" + week + "第" + section + "节;";
                            }
                            
                        }
                        else
                        {
                            cTime.Add(courseName, "周" + week + "第" + section + "节;");
                        }

                    }
                }
                //遍历dic
                foreach (var item in cTime)
                {
                    SLCourse slCourse = new SLCourse();
                    slCourse.ClassName = classname;
                    slCourse.CourseName = item.Key;
                    slCourse.CourseTime = item.Value.Substring(0,item.Value.Length-1);
                    slCourse.Students = func(classname, "1");
                    slCourse.TeacherName = func(classname, "2");
                    list.Add(slCourse);
                }
            }
            return list;

            //using (fs = File.OpenWrite(@"D:/myxls1.xls"))
            //{
            //    workbook.Write(fs);//向打开的这个xls文件中写入数据  

            //}

        }
        public static void WriteXlsxFile(List<SLCourse> slist)
        {
            //HSSF可以读取xls格式的Excel文件
            //IWorkbook workbook = new HSSFWorkbook();
            //XSSF可以读取xlsx格式的Excel文件
            IWorkbook workbook = new XSSFWorkbook();
            ISheet sheet = workbook.CreateSheet("sheet1");
            //创建行
            IRow row = sheet.CreateRow(0); //i表示了创建行的索引，从0开始
                                           //创建单元格
            row.CreateCell(0).SetCellValue("课程编号");
            row.CreateCell(1).SetCellValue("课程名称");
            row.CreateCell(2).SetCellValue("任课教师");
            row.CreateCell(3).SetCellValue("班级名称");
            row.CreateCell(4).SetCellValue("上课时间");
            row.CreateCell(5).SetCellValue("学生");

            for (int i = 0; i < slist.Count; i++)
            {
                row = sheet.CreateRow(i + 1); //i表示了创建行的索引，从0开始
                string c = "";
                if (i < 9)
                {
                    c = "00" + (i+1);
                }
                else if(i<99)
                {
                    c = "0" + (i+1);
                }
                else
                {
                    c = "" + (i + 1);
                }
                
                row.CreateCell(0).SetCellValue("CH"+c);
                row.CreateCell(1).SetCellValue(slist[i].CourseName);
                row.CreateCell(2).SetCellValue(slist[i].TeacherName);
                row.CreateCell(3).SetCellValue(slist[i].ClassName);
                row.CreateCell(4).SetCellValue(slist[i].CourseTime);
                row.CreateCell(5).SetCellValue(slist[i].Students);
                
                //for (int j = 0; j < 6; j++)
                //{
                //    ICell cell = row.CreateCell(j);  //同时这个函数还有第二个重载，可以指定单元格存放数据的类型
                //    cell.SetCellValue(i.ToString() + j.ToString());
                //}
            }

            //Excel文件至少要有一个工作表sheet

            //表格制作完成后，保存
            //创建一个文件流对象
            using (FileStream fs = File.Open(@"C:\Users\admin\Desktop\2017级第2期课表_new.xlsx", FileMode.OpenOrCreate))
            {
                workbook.Write(fs);
                //最后记得关闭对象
                workbook.Close();
            }

        }
    }
}