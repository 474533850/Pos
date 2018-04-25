using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace POS.Model
{
    /// <summary>
    /// 移动支付返回的结果
    /// </summary>
    public class AlipayResult
    {
        /// <summary>
        /// 收款方式标识 1为支付宝,2为微信
        /// </summary>
        public string way_id { get; set; }
        /// <summary>
        /// 收款方式名称
        /// </summary>
        public string way { get; set; }
        /// <summary>
        /// 商户订单号
        /// </summary>
        public string out_trade_no { get; set; }

        /// <summary>
        /// 订单金额
        /// </summary>
        public string total_amount { get; set; }

        /// <summary>
        /// 订单状态
        /// </summary>
        public string Trade_status { get; set; }
        /// <summary>
        /// 交易时间
        /// </summary>
        public string Send_pay_time { get; set; }
        /// <summary>
        /// 实收金额
        /// </summary>
        public string Receipt_amount { get; set; }
        /// <summary>
        /// 商户折扣
        /// </summary>
        public string discount { get; set; }
        /// <summary>
        /// 官方交易号
        /// </summary>
        public string trade_no { get; set; }
        /// <summary>
        /// 结果状态
        /// </summary>
        public string result { get; set; }
        /// <summary>
        /// 结果描述
        /// </summary>
        public string message { get; set; }
    }
}
