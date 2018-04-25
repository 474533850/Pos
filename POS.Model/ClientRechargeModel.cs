using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace POS.Model
{
    /// <summary>
    /// 会员充值
    /// </summary>
    public class ClientRechargeModel
    {
        /// <summary>
        /// 分部名称
        /// </summary>
        public string xlsname { get; set; }
        /// <summary>
        /// 录入时间
        /// </summary>
        public string xintime { get; set; }
        /// <summary>
        /// 会员号
        /// </summary>
        public string clntcode { get; set; }
        /// <summary>
        /// 会员名称
        /// </summary>
        public string clntname { get; set; }
        /// <summary>
        /// 分部代码
        /// </summary>
        public string xls { get; set; }
        /// <summary>
        /// 操作员
        /// </summary>
        public string username { get; set; }
        /// <summary>
        /// 充值金额
        /// </summary>
        public string xfee { get; set; }
        /// <summary>
        /// 通用余额
        /// </summary>
        public decimal balance { get; set; }
        /// <summary>
        /// 持有积分
        /// </summary>
        public decimal integral { get; set; }
    }
}
