using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace POS.Model
{
    /// <summary>
    /// 收款单表头
    /// </summary>
    public class OfhhModel : BaseModel
    {
        /// <summary>
        /// 收款日期
        /// </summary>
        public DateTime xdate { get; set; }
        /// <summary>
        /// 顾客代码
        /// </summary>
        public string clntcode { get; set; }
        /// <summary>
        /// 顾客名称
        /// </summary>
        public string clntname { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string xnote { get; set; }
        /// <summary>
        /// 录入人
        /// </summary>
        public string xinname { get; set; }
        /// <summary>
        /// 录入时间
        /// </summary>
        public string xintime { get; set; }

        public List<OfbbModel> ofbbs { get; set; }

    }
}
