using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace POS.Model
{
    /// <summary>
    /// 营业款缴交凭证
    /// </summary>
    public class PaymentVoucherModel
    {
        /// <summary>
        /// 借方科目
        /// </summary>
        public string xlsname { get; set; }
        /// <summary>
        /// 日期
        /// </summary>
        public string xdate { get; set; }
        /// <summary>
        /// 审核人
        /// </summary>
        public string xheman { get; set; }

        /// <summary>
        /// 制表
        /// </summary>
        public string xusername { get; set; }
        /// <summary>
        /// 金额合计
        /// </summary>
        public string xtotalmoney { get; set; }
        /// <summary>
        /// 大写金额
        /// </summary>
        public string xcapital { get; set; }

        public List<PaymentVoucherDetailModel> Details { get; set; }
    }
}
