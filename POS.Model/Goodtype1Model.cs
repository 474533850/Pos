using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace POS.Model
{
    /// <summary>
    /// 货品大类
    /// </summary>
    public class Goodtype1Model : BaseModel
    {
        /// <summary>
        /// 货品大类
        /// </summary>
        public string goodtype1 { get; set; }
        public string goodtype3 { get; set; }
        /// <summary>
        /// 级别折扣设置
        /// </summary>

        public string uclssprics { get; set; }

    }
}
