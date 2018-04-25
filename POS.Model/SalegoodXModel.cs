using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace POS.Model
{
    /// <summary>
    /// 促销赠品
    /// </summary>
    public class SalegoodXModel : BaseModel
    {
        /// <summary>
        /// 类别
        /// </summary>
        public string xtype { get; set; }
        /// <summary>
        /// 对应 sale 表的XTABLEID
        /// </summary>
        public int xsaleid { get; set; }
        /// <summary>
        /// 类型(a:所有SKU, s:单个SKU)
        /// </summary>
        public string xgtype { get; set; }
        /// <summary>
        ///  xgtype为 a 时对应 good 表的XTABLEID,为 s 时对应goodpric表的XTABLEID
        /// </summary>
        public int xgoodid { get; set; }
        public int xno { get; set; }

        public string goodcode { get; set; }

        public string goodname { get; set; }

        public string goodunit { get; set; }

        public string goodkind { get; set; }
        public string goodkind1 { get; set; }
        public string goodkind2 { get; set; }
        public string goodkind3 { get; set; }
        public string goodkind4 { get; set; }
        public string goodkind5 { get; set; }
        public string goodkind6 { get; set; }
        public string goodkind7 { get; set; }

        public string goodkind8 { get; set; }
        public string goodkind9 { get; set; }
        public string goodkind10 { get; set; }
    }
}
