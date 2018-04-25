using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace POS.Common.Enum
{
    /// <summary>
    /// 货品类型
    /// </summary>
   public enum GoodsType
    {
        /// <summary>
        /// 通用
        /// </summary>
        [Description("通用")]
        General,
        /// <summary>
        /// SKU
        /// </summary>
        [Description("SKU")]
        SKU,
    }
}
