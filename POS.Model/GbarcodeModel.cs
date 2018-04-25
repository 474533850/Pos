using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace POS.Model
{
    /// <summary>
    /// 一品多码
    /// </summary>
    public class GbarcodeModel : BaseModel
    {
        /// <summary>
        /// 条形码
        /// </summary>
        public string xbarcode { get; set; }
        /// <summary>
        /// 单位
        /// </summary>
        public string goodunit { get; set; }
        /// <summary>
        /// 类型(通用, SKU)
        /// </summary>
        public string xtype { get; set; }
        /// <summary>
        /// 价格
        /// </summary>
        public decimal xpric { get; set; }

    }
}
