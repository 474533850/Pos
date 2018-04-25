using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace POS.Model
{
    /// <summary>
    /// POS单据表头
    /// </summary>
    [Serializable]
    public class PoshhModel : BaseModel
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
        public string workname    { get; set; }
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
        /// 支付明细
        /// </summary>
        public List<BillpaytModel> payts { get; set; }

        /// <summary>
        /// 支付积分数
        /// </summary>
        public decimal? xpoints { get; set; }
        /// <summary>
        /// 赠送积分
        /// </summary>
        public int? xsendjf { get; set; }
        /// <summary>
        /// 积分抵扣现金
        /// </summary>
        public decimal deductiblecash { get; set; }
        /// <summary>
        /// 单据上传状态
        /// </summary>
        public string uploadstatus { get; set; }

        public bool isHistory { get; set; }
        /// <summary>
        /// 是否为会员日
        /// </summary>
        public bool isClntDay { get; set; }
    }
}
