using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace POS.Model
{
    public class SyncResultModel<T> where T : class
    {
        public List<T> datas { get; set; }
        public int code { get; set; }
        public string msg { get; set; }
        public int total { get; set; }
        public List<T> rows { get; set; }

        public string data { get; set; }

        public int totalPage { get; set; }

    }
}
