using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
   public class DateFiles
    {
        private string date;
        private List<FileOp> list;

        public string Date { get => date; set => date = value; }
        public List<FileOp> List { get => list; set => list = value; }
    }
}
