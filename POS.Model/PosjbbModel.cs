using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace POS.Model
{
    public class PosjbbModel
    {
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

        public int xsubid { get; set; }
    }
}
