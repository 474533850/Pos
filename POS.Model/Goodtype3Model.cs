using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace POS.Model
{
    /// <summary>
    /// 货品小类
    /// </summary>
    public class Goodtype3Model : BaseModel
    {
        /// <summary>
        /// 货品大类
        /// </summary>
        public string goodtype1 { get; set; }
        /// <summary>
        /// 货品中类
        /// </summary>
        public string goodtype2 { get; set; }
        /// <summary>
        /// 货品小类
        /// </summary>
        public string goodtype3 { get; set; }
        /// <summary>
        /// 级别折扣设置
        /// </summary>

        public string uclssprics { get; set; }

    }
}
