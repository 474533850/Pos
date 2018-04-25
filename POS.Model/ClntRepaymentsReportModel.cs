using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace POS.Model
{
    /// <summary>
    /// 
    /// </summary>
   public class ClntRepaymentsReportModel
    {
        /// <summary>
        /// 单号
        /// </summary>
        public string billno { get; set; }
        /// <summary>
        /// 日期
        /// </summary>
        public string xintime { get; set; }
        /// <summary>
        /// 会员代码
        /// </summary>
        public string clntcode { get; set; }

        /// <summary>
        /// 会员名称
        /// </summary>
        public string clntname { get; set; }

        /// <summary>
        /// 收款金额
        /// </summary>
        public decimal xpay { get; set; }

        /// <summary>
        /// 分部代码
        /// </summary>
        public string xls { get; set; }

        /// <summary>
        /// 分部名称
        /// </summary>
        public string xlsname { get; set; }

        /// <summary>
        /// 摘要
        /// </summary>
        public string xnote { get; set; }

        /// <summary>
        /// 单号
        /// </summary>
        public string billnob { get; set; }

        /// <summary>
        /// 货款金额
        /// </summary>
        public decimal xallp { get; set; }

        /// <summary>
        /// 以前未结
        /// </summary>
        public decimal xlast { get; set; }

        /// <summary>
        /// 本次结算
        /// </summary>
        public decimal xnowpay { get; set; }

        /// <summary>
        /// 结算折让
        /// </summary>
        public decimal xnowzhe { get; set; }

        /// <summary>
        /// 欠结金额
        /// </summary>
        public decimal xjie { get; set; }

        /// <summary>
        /// 摘要
        /// </summary>
        public string xnoteb { get; set; }

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
        public decimal xpayp { get; set; }
        /// <summary>
        /// 说明一
        /// </summary>
        public string xnote1 { get; set; }
        /// <summary>
        /// 说明二
        /// </summary>
        public string xnote2 { get; set; }

    }
}
