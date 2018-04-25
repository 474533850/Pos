using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace POS.Model
{
    /// <summary>
    /// 分部仓库
    /// </summary>
    public class CnkuModel:BaseModel
    {
        /// <summary>
        /// 仓库类别
        /// </summary>
        public string cnkutype { get; set; }
        /// <summary>
        /// 仓库代码
        /// </summary>
        public string cnkucode { get; set; }
        /// <summary>
        /// 仓库名称
        /// </summary>
        public string cnkuname { get; set; }
        /// <summary>
        /// 分部代码
        /// </summary>
        public string xls { get; set; }
        /// <summary>
        /// 分部名称
        /// </summary>
        public string xlsname { get; set; }

    }
}
