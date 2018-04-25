using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace POS.Model
{
    /// <summary>
    /// 收款单表体
    /// </summary>
    public class OfbbModel : BaseModel
    {
        /// <summary>
        /// 摘要
        /// </summary>
        public string xnoteb { get; set; }
        /// <summary>
        /// 支付方式
        /// </summary>
        /// <returns></returns>
        public string xztype { get; set; }
        /// <summary>
        /// 支付状态
        /// </summary>
        public string xzstate { get; set; }
        /// <summary>
        /// 支付单号
        /// </summary>
        public string paybillno { get; set; }
        /// <summary>
        /// 收款金额
        /// </summary>
        public decimal xfee { get; set; }
        /// <summary>
        /// 赠送金额
        /// </summary>
        public decimal xsubsidy { get; set; }
        /// <summary>
        /// 交易流水号
        /// </summary>
        public string transno { get; set; }

    }
}
