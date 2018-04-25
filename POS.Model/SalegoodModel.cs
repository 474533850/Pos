using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace POS.Model
{
    /// <summary>
    /// 促销活动货品
    /// </summary>
    public class SalegoodModel : BaseModel
    {
        /// <summary>
        /// 类别 j:减价,m:免邮,p:赠品,z:打折
        /// </summary>
        public string xtype { get; set; }
        /// <summary>
        /// 对应 sale 表的XTABLEID
        /// </summary>
        public int xsaleid { get; set; }
        /// <summary>
        /// 类型(a:所有sku，s：单个sku)
        /// </summary>
        public string xgtype { get; set; }
        /// <summary>
        ///  a对应 good 表的XTABLEID s对应goodpric表的XTABLEID
        /// </summary>
        public int xgoodid { get; set; }

        public string goodcode { get; set; }
        /// <summary>
        /// 促销活动录入时间
        /// </summary>
        public string xintime { get; set; }

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
