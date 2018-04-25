using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace POS.Model
{
    public class ConfigModel
    {
        /// <summary>
        /// 账套
        /// </summary>
        public string sid { get; set; }
        /// <summary>
        /// 分部
        /// </summary>
        public string xls { get; set; }

        /// <summary>
        /// 分部名称
        /// </summary>
        public string xlsname { get; set; }
    }
}
