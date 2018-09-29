using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
   public class User
    {
        private string uid;
        private string name;

        public string Uid { get => uid; set => uid = value; }
        public string Name { get => name; set => name = value; }
    }
}
