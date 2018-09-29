using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class PowerModel
    {
        public int code { get; set; }
        public List<PModel> menu { get; set; }
        public string insert { get; set; }
        public string update { get; set; }
        public string select { get; set; }
        public string delete { get; set; }
        public string view { get; set; }
        public string admin { get; set; }
        public string limit { get; set; }

    }

    public class PModel
    {
        public string  modelid { get; set; }
        public string icon { get; set; }
        public string name { get; set; }
        public string url { get; set; }
        public string pars { get; set; }
        public List<CModel> child { get; set; }

    }
    public class CModel
    {
        public string modelid { get; set; }
        public string icon { get; set; }
        public string name { get; set; }
        public string url { get; set; }
        public string pars { get; set; }
    }
}
