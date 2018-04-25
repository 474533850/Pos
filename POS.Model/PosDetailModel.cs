using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace POS.Model
{
    /// <summary>
    /// 小票明细
    /// </summary>
    public class PosDetailModel
    {
        /// <summary>
        ///货品分类
        /// </summary>
        public string goodtype { get; set; }
        public string goodcode { get; set; }
        public string goodname { get; set; }
        public decimal xquat { get; set; }
        public decimal xpric { get; set; }
        public decimal xallp { get; set; }
    }
}
