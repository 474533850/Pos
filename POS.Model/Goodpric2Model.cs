using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace POS.Model
{
    /// <summary>
    /// 等级价格
    /// </summary>
    public class Goodpric2Model : BaseModel
    {
        /// <summary>
        /// 顾客等级
        /// </summary>
        public string clntclss { get; set; }
        /// <summary>
        /// 价格
        /// </summary>
        public decimal xpric { get; set; }


    }
}
