using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace POS.Common.Enum
{
    /// <summary>
    /// 促销应用方式
    /// </summary>
    public enum SaleKind
    {
        /// <summary>
        /// 单品
        /// </summary>
        Product=1,
        /// <summary>
        /// 整单
        /// </summary>
        Order=2,
        /// <summary>
        /// 第二件半价
        /// </summary>
        Half=3
    }
}
