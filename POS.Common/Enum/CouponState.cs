using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace POS.Common.Enum
{
    /// <summary>
    /// 优惠券状态
    /// </summary>
    public enum CouponState
    {
        /// <summary>
        /// 已使用
        /// </summary>
        [Description("已使用")]
        Used,
        /// <summary>
        /// 未使用
        /// </summary>
        [Description("未使用")]
        Unused
    }
}
