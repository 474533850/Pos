using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace POS.Common.Enum
{
    /// <summary>
    /// 促销类型 类别 j:减价, m:免邮, p:赠品, z:打折, a:加送
    /// </summary>
    public enum SaleType
    {

        /// <summary>
        /// 减价
        /// </summary>
       [Description("j")]
        j,
        /// <summary>
        /// 免邮
        /// </summary>
        [Description("m")]
        m,
        /// <summary>
        /// 赠品
        /// </summary>
        [Description("p")]
        p,
        /// <summary>
        /// 打折
        /// </summary>
        [Description("z")]
        z,
        /// <summary>
        /// 加送
        /// </summary>
        [Description("a")]
        a,
        /// <summary>
        /// 特价
        /// </summary>
        [Description("t")]
        t
    }
}
