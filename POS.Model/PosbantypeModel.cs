using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace POS.Model
{
    /// <summary>
    /// 班次
    /// </summary>
    public class PosbantypeModel : BaseModel
    {
        /// <summary>
        /// 班次
        /// </summary>
        public string posbcode { get; set; }
        /// <summary>
        /// 起始时间
        /// </summary>
        /// <returns></returns>
        public string xtime1 { get; set; }
        /// <summary>
        /// 终止时间
        /// </summary>
        public string xtime2 { get; set; }
        /// <summary>
        /// 说明
        /// </summary>
        public string xnote { get; set; }
    }
}
