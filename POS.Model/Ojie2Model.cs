using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace POS.Model
{
    public class Ojie2Model : BaseModel
    {
        /// <summary>
        /// 顾客代码
        /// </summary>
        public string clntcode { get; set; }
        /// <summary>
        /// 顾客名称
        /// </summary>
        public string clntname { get; set; }
        /// <summary>
        /// 上期结余金额
        /// </summary>
        public decimal xlast{ get; set; }
        /// <summary>
        /// 收款金额
        /// </summary>
        public decimal xpay { get; set; }
        /// <summary>
        /// 划扣金额
        /// </summary>
        public decimal xhk { get; set; }
        /// <summary>
        /// 销售金额
        /// </summary>
        public decimal xdo { get; set; }
        /// <summary>
        /// 折让金额
        /// </summary>
        public decimal xzhe { get; set; }
        /// <summary>
        /// 期末结余金额
        /// </summary>
        public decimal xjie { get; set; }
        /// <summary>
        /// 上期消费
        /// </summary>
        public decimal xxflast { get; set; }
        /// <summary>
        /// 本期消费
        /// </summary>
        public decimal xxfnow { get; set; }
        /// <summary>
        /// 期末消费
        /// </summary>
        public decimal xxfjie { get; set; }

    }
}
