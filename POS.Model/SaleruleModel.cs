using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace POS.Model
{
    /// <summary>
    /// 活动规则
    /// </summary>
    public class SaleruleModel : BaseModel
    {
        /// <summary>
        /// 满
        /// </summary>
        public decimal xhave { get; set; }
        /// <summary>
        /// 打
        /// </summary>
        public decimal xdo { get; set; }
        /// <summary>
        /// 积分
        /// </summary>
        public decimal xfen { get; set; }

        public List<SalegoodXModel> SalegoodXs { get; set; }

    }
}
