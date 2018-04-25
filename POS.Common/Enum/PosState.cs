using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace POS.Common.Enum
{
    /// <summary>
    /// Pos单状态
    /// </summary>
    public enum PosState
    {
        /// <summary>
        /// 挂单
        /// </summary>
        [Description("挂单")]
        Pending,
        /// <summary>
        /// 反结
        /// </summary>
        [Description("反结")]
        Invalid,
        /// <summary>
        /// 退货
        /// </summary>
        [Description("退货")]
        Returned,
        /// <summary>
        /// 成交
        /// </summary>
        [Description("成交")]
        Deal,
        /// <summary>
        /// 追加
        /// </summary>
        [Description("追加")]
        Additional,
        /// <summary>
        /// 换货
        /// </summary>
        [Description("换货")]
        Change,
    }
}
