using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace POS.Model
{
    /// <summary>
    /// 当班记录
    /// </summary>
    public class PosbanModel : BaseModel
    {
        /// <summary>
        /// POS类别
        /// </summary>
        public string xposset { get; set; }
        /// <summary>
        /// 当班单号
        /// </summary>
        public string posnono { get; set; }
        /// <summary>
        /// 门店
        /// </summary>
        public string posposi { get; set; }
        /// <summary>
        /// 班次
        /// </summary>
        public string posbcode { get; set; }
        /// <summary>
        /// 收银员代码
        /// </summary>
        public string xopcode { get; set; }
        /// <summary>
        /// 收银员
        /// </summary>
        public string xopname { get; set; }
        /// <summary>
        /// 当班开始时间
        /// </summary>
        public string xtime1 { get; set; }
        /// <summary>
        /// 当班结束时间
        /// </summary>
        public string xtime2 { get; set; }
        /// <summary>
        /// 上次结余
        /// </summary>
        public decimal xjielst { get; set; }
        /// <summary>
        /// 当班收入
        /// </summary>
        public decimal xjiepos { get; set; }
        /// <summary>
        /// 当前备用金
        /// </summary>
        public decimal xjienow { get; set; }
        /// <summary>
        /// 结留现金
        /// </summary>
        public decimal xjiehav { get; set; }
        /// <summary>
        /// 结转现金
        /// </summary>
        public decimal xjieget { get; set; }
        /// <summary>
        /// 已结
        /// </summary>
        public bool xjieok { get; set; } 

    }
}
