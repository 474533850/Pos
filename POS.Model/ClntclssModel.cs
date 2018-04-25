using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS.Model
{
    /// <summary>
    /// 顾客等级
    /// </summary>
    public class ClntclssModel : BaseModel
    {
        /// <summary>
        /// 顾客等级
        /// </summary>
        public string clntclss { get; set; }
        /// <summary>
        /// 折扣百分比
        /// </summary>
        public decimal xzhe { get; set; }

        /// <summary>
        /// 默认等级
        /// </summary>
        public bool xdefault { get; set; }
    }
}
