using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS.Model
{
    /// <summary>
    /// 积分结余
    /// </summary>
    public class Jjie2Model : BaseModel
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
        /// 上期结余
        /// </summary>
        public decimal xlast { get; set; }
        /// <summary>
        /// 本期消费
        /// </summary>
        public decimal xdo { get; set; }
        /// <summary>
        /// 本期积分
        /// </summary>
        public decimal xpay { get; set; }
        /// <summary>
        /// 期末结余
        /// </summary>
        public decimal xjie { get; set; }

    }
}
