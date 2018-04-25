using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace POS.Model
{
    /// <summary>
    /// 支付明细
    /// </summary>
    [Serializable]
    public class BillpaytModel : BaseModel
    {
        public Guid XID { get; set; }
        /// <summary>
        /// 帐户代码
        /// </summary>
        public string paytcode { get; set; }
        /// <summary>
        /// 帐户名称
        /// </summary>
        public string paytname { get; set; }
        /// <summary>
        /// 金额
        /// </summary>
        public decimal xpay { get; set; }
        /// <summary>
        /// 实收现金
        /// </summary>
        public decimal? xreceipt { get; set; }
        /// <summary>
        /// 说明一
        /// </summary>
        public string xnote1 { get; set; }
        /// <summary>
        /// 说明二
        /// </summary>
        public string xnote2 { get; set; }
        /// <summary>
        /// 单标志
        /// </summary>
        public string billflag { get; set; }

    }
}
