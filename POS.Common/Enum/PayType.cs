using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace POS.Common.Enum
{
    /// <summary>
    /// 支付方式
    /// </summary>
    public enum PayType
    {
        /// <summary>
        /// 库存现金
        /// </summary>
        [Description("库存现金")]
        Cash,
        /// <summary>
        /// 微信
        /// </summary>
        [Description("微信")]
        WeChat,
        /// <summary>
        /// 支付宝
        /// </summary>
        [Description("支付宝")]
        Alipay,
        /// <summary>
        /// 银联卡
        /// </summary>
        [Description("银联卡")]
        UnionpayCard,
        /// <summary>
        /// 预存款
        /// </summary>
        [Description("预存款")]
        Deposit,
        /// <summary>
        /// 优惠券
        /// </summary>
        [Description("优惠券")]
        Coupon,
        /// <summary>
        /// 支票
        /// </summary>
        [Description("支票")]
        Check,
    }
}
