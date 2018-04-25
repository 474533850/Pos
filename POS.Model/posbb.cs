using POS.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace POS.Model
{
    /// <summary>
    /// POS单据表体
    /// </summary>
    public class Posbb:BaseModel
    {
        /// <summary>
        /// 外键ID
        /// </summary>
        public Guid XID { get; set; }
        /// <summary>
        /// 货品代码
        /// </summary>
        public string goodcode { get; set; }
        /// <summary>
        /// 货品名称
        /// </summary>
        public string goodname { get; set; }
        /// <summary>
        /// 货品规格
        /// </summary>
        public string goodtm { get; set; }
        /// <summary>
        /// 货品单位
        /// </summary>
        public string goodunit { get; set; }
        /// <summary>
        /// 货品条码
        /// </summary>
        public string xbarcode { get; set; }
        /// <summary>
        /// 规格1
        /// </summary>
        public string goodkind1 { get; set; }
        /// <summary>
        /// 规格2
        /// </summary>
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
        /// 组装包
        /// </summary>
        public string goodgive { get; set; }
        /// <summary>
        ///  仓库代码
        /// </summary>
        public string cnkucode { get; set; }
        /// <summary>
        /// 仓库名称
        /// </summary>
        public string cnkuname { get; set; }
        /// <summary>
        /// 数量
        /// </summary>
        public decimal xquat { get; set; }
        /// <summary>
        /// 单价
        /// </summary>
        public decimal xpricold { get; set; }
        /// <summary>
        /// 折扣
        /// </summary>
        public decimal xzhe { get; set; }
        /// <summary>
        /// 折后单价
        /// </summary>
        public decimal xpric { get; set; }
        /// <summary>
        /// 金额
        /// </summary>
        public decimal xallp { get; set; }
        /// <summary>
        /// 税率
        /// </summary>
        public decimal xtaxr { get; set; }
        /// <summary>
        /// 税额
        /// </summary>
        public decimal xtax { get; set; }
        /// <summary>
        /// 含税单价
        /// </summary>
        public decimal xprict { get; set; }
        /// <summary>
        /// 价税合计
        /// </summary>
        public decimal xallpt { get; set; }
        /// <summary>
        /// 退货数量
        /// </summary>
        public decimal? xtquat { get; set; }
        /// <summary>
        /// 当前单位
        /// </summary>
        public string unitname { get; set; }
        /// <summary>
        /// 对换率
        /// </summary>
        public decimal? unitrate { get; set; }
        /// <summary>
        /// 当前数量
        /// </summary>
        public decimal? unitquat { get; set; }

    }
}
