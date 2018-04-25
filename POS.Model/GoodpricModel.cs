using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace POS.Model
{
    /// <summary>
    /// 货品价格
    /// </summary>
    public class GoodpricModel:BaseModel
    {
        /// <summary>
        /// 价格
        /// </summary>
        public decimal xpric { get; set; }
        /// <summary>
        /// 规格1
        /// </summary>
        /// <returns></returns>
      public string  goodkind1 { get; set; }
        /// <summary>
        /// 规格2
        /// </summary>
        /// <returns></returns>
        public string goodkind2 { get; set; }
        /// <summary>
        /// 规格3
        /// </summary>
        public string goodkind3 { get; set; }
        /// <summary>
        /// 规格4
        /// </summary>
        public string goodkind4 { get; set; }
        /// <summary>
        /// 规格5
        /// </summary>
        public string goodkind5 { get; set; }
        /// <summary>
        /// 规格6
        /// </summary>
        public string goodkind6 { get; set; }
        /// <summary>
        /// 规格7
        /// </summary>
        public string goodkind7 { get; set; }
        /// <summary>
        /// 规格8
        /// </summary>
        public string goodkind8 { get; set; }
        /// <summary>
        /// 规格9
        /// </summary>
        public string goodkind9 { get; set; }
        /// <summary>
        /// 规格10
        /// </summary>
        public string goodkind10 { get; set; }
        /// <summary>
        /// 规格例如：规格1,规格2
        /// </summary>
        public string goodkind { get; set; }

        /// <summary>
        /// 赠送积分
        /// </summary>
        public decimal xsendjf { get; set; }
        /// <summary>
        /// 换购积分
        /// </summary>
        public decimal xchagjf { get; set; }
    }
}
