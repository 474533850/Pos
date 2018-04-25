using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace POS.Model
{
    /// <summary>
    /// 零售明细表
    /// </summary>
    public class SaleDetailReportModel
    {
        /// <summary>
        /// 单号
        /// </summary>
        public string billno { get; set; }
        /// <summary>
        /// 当班单号
        /// </summary>
        public string posnono { get; set; }
        /// <summary>
        /// 单据类别
        /// </summary>
        public string xtype { get; set; }
        /// <summary>
        /// 状态
        /// </summary>
        public string xstate { get; set; }
        /// <summary>
        /// 日期
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
        /// 顾客手机号码
        /// </summary>
        public string xpho { get; set; }
        /// <summary>
        /// 货款金额
        /// </summary>
        public decimal xheallp { get; set; }
        /// <summary>
        /// 付款金额
        /// </summary>
        public decimal xpay { get; set; }
        /// <summary>
        /// 折让金额
        /// </summary>
        public decimal xhezhe { get; set; }
        /// <summary>
        /// 结算金额
        /// </summary>
        public decimal xhejie { get; set; }
        /// <summary>
        /// 结算折让
        /// </summary>
        public decimal xnowzhe { get; set; }
        /// <summary>
        /// 欠付金额
        /// </summary>
        public decimal xhenojie { get; set; }
        /// <summary>
        /// 摘要
        /// </summary>
        public string xnote { get; set; }
        /// <summary>
        /// 职员代码
        /// </summary>
        public string workcode { get; set; }
        /// <summary>
        /// 职员名称
        /// </summary>
        public string workname { get; set; }
        /// <summary>
        /// 分部代码
        /// </summary>
        public string xls { get; set; }
        /// <summary>
        /// 分部名称
        /// </summary>
        public string xlsname { get; set; }
        /// <summary>
        /// 录入人
        /// </summary>
        public string xinname { get; set; }
        /// <summary>
        /// 录入时间
        /// </summary>
        public string xintime { get; set; }
        /// <summary>
        /// 已审
        /// </summary>
        public bool xhe { get; set; }
        /// <summary>
        /// 审核人
        /// </summary>
        public string xheman { get; set; }
        /// <summary>
        /// 审核时间
        /// </summary>
        public string xhetime { get; set; }
        /// <summary>
        /// 结算期间
        /// </summary>
        public string xterm { get; set; }
        /// <summary>
        /// 支付方式
        /// </summary>
        public string paytype { get; set; }
        /// <summary>
        /// 支付交易号
        /// </summary>
        public string transno { get; set; }
        /// <summary>
        /// 上级单号
        /// </summary>
        public string pbillno { get; set; }
        /// <summary>
        /// 舍弃金额
        /// </summary>
        public decimal xrpay { get; set; }
        /// <summary>
        /// 表体集合
        /// </summary>
        public List<PosbbModel> Posbbs { get; set; }

        /// <summary>
        /// 支付积分数
        /// </summary>
        public decimal? xpoints { get; set; }
        /// <summary>
        /// 赠送积分
        /// </summary>
        public int? xsendjf { get; set; }

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
        public decimal unitquat { get; set; }
        /// <summary>
        /// 当前单价
        /// </summary>
        public decimal unitpric { get; set; }
        /// <summary>
        /// 促销类型
        /// </summary>
        public string xsalestype { get; set; }
        /// <summary>
        /// 促销ID
        /// </summary>
        public int? xsalesid { get; set; }

        public string xchg { get; set; }

        /// <summary>
        /// 消耗积分数
        /// </summary>
        public decimal? xpointsb { get; set; }
        /// <summary>
        /// n倍积分
        /// </summary>
        public decimal? xtimes { get; set; }

    }
}
