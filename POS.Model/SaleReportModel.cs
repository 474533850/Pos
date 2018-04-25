using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace POS.Model
{
    /// <summary>
    /// 销售报表
    /// </summary>
    public class SaleReportModel
    {
        public Guid ID { get; set; }
        /// <summary>
        /// 品名
        /// </summary>
        public string goodname { get; set; }
        /// <summary>
        /// 分类
        /// </summary>
        public string goodtype { get; set; }
        /// <summary>
        /// 数量
        /// </summary>
        public decimal Quantity { get; set; }
        /// <summary>
        /// 舍弃金额
        /// </summary>
        public decimal xrpay { get; set; }
        /// <summary>
        /// 单据状态
        /// </summary>
        public string xstate { get; set; }
        /// <summary>
        /// 小计
        /// </summary>
        public decimal Total { get; set; }
        /// <summary>
        /// 挂账
        /// </summary>
        public decimal debts { get; set; }

        public string xnote1 { get; set; }
    }
}
