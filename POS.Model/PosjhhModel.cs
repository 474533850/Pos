using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace POS.Model
{
    /// <summary>
    /// pos 结算单
    /// </summary>
    public class PosjhhModel
    {
        /// <summary>
        /// 单号
        /// </summary>
        public string billno { get; set; }
        /// <summary>
        /// 日期
        /// </summary>
        public DateTime xdate { get; set; }
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

        public string xnote { get; set; }

        public int xtableid { get; set; }

        public List<PosjbbModel> posjbbs { get; set; }

        public List<BillpaytModel> payts { get; set; }
    }
}
