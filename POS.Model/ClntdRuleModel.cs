using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace POS.Model
{
    /// <summary>
    /// 会员日规则
    /// </summary>
    public class ClntdRuleModel : BaseModel
    {
        /// <summary>
        /// 货品分类
        /// </summary>
        public string goodtype { get; set; }
        /// <summary>
        /// 分类级别折扣设置
        /// </summary>
        public string uclssprics { get; set; }
        /// <summary>
        /// 分类类型 (大类、中类、小类)
        /// </summary>
        public string classtype { get; set; }
        /// <summary>
        /// n倍积分
        /// </summary>
        public decimal xtimes { get; set; }
    }
}
