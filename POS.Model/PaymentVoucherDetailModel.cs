using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace POS.Model
{
    /// <summary>
    /// 营业款缴交凭证明细
    /// </summary>
    public class PaymentVoucherDetailModel
    {
        /// <summary>
        /// 摘要
        /// </summary>
        public string xnote { get; set; }
        /// <summary>
        /// 科目
        /// </summary>
        public string xsubject { get; set; }
        /// <summary>
        /// 明细科目
        /// </summary>
        public string xdsubject { get; set; }
        /// <summary>
        /// 金额
        /// </summary>
        public decimal xpay { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string remark { get; set; }
    }
}
