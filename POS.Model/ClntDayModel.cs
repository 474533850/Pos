using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace POS.Model
{
    /// <summary>
    /// 会员日 
    /// </summary>
    public class ClntDayModel : BaseModel
    {
        /// <summary>
        /// 会员日期
        /// </summary>
        public int xday { get; set; }
        /// <summary>
        /// 所在月份
        /// </summary>
        public List<int> xmonth { get; set; }
        /// <summary>
        /// 是否启用
        /// </summary>
        public bool xstart { get; set; }

        public List<ClntdRuleModel> clntprices { get; set; }
    }
}
