using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace POS.Model
{
    /// <summary>
    /// 当前进销存帐
    /// </summary>
    public class Ku2Model : BaseModel
    {
        public string key { get; set; }
        /// <summary>
        /// 分部代码
        /// </summary>
        public string xls { get; set; }

        /// <summary>
        /// 分部名称
        /// </summary>
        public string xlsname { get; set; }

        /// <summary>
        /// 仓库代码
        /// </summary>
        public string cnkucode { get; set; }

        /// <summary>
        /// 仓库名称
        /// </summary>
        public string cnkuname { get; set; }

        /// <summary>
        /// 货品代码
        /// </summary>
        public string goodcode { get; set; }

        /// <summary>
        /// 货品名称
        /// </summary>
        public string goodname { get; set; }

        /// <summary>
        /// 货品单位
        /// </summary>
        public string goodunit { get; set; }

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
        /// 期初单价
        /// </summary>
        public decimal? xpricqc { get; set; }

        /// <summary>
        /// 期初数量
        /// </summary>
        public decimal? xquatqc { get; set; }

        /// <summary>
        /// 期初金额
        /// </summary>
        public decimal? xallpqc { get; set; }

        /// <summary>
        /// 进货单价
        /// </summary>
        public decimal? xpricin { get; set; }

        /// <summary>
        /// 进货数量
        /// </summary>
        public decimal? xquatin { get; set; }

        /// <summary>
        /// 进货金额
        /// </summary>
        public decimal? xallpin { get; set; }

        /// <summary>
        /// 销售单价
        /// </summary>
        public decimal? xpricot { get; set; }

        /// <summary>
        /// 销售数量
        /// </summary>
        public decimal? xquatot { get; set; }

        /// <summary>
        /// 销售金额
        /// </summary>
        public decimal? xallpot { get; set; }

        /// <summary>
        /// 销售成本
        /// </summary>
        public decimal? xchenot { get; set; }

        /// <summary>
        /// 销售毛润
        /// </summary>
        public decimal? xlirnot { get; set; }

        /// <summary>
        /// 库存单价
        /// </summary>
        public decimal? xpricku { get; set; }

        /// <summary>
        /// 库存数量
        /// </summary>
        public decimal? xquatku { get; set; }

        /// <summary>
        /// 库存金额
        /// </summary>
        public decimal? xallpku { get; set; }

        /// <summary>
        /// 最后更新时间
        /// </summary>
        public string xlastime { get; set; }

    }
}
