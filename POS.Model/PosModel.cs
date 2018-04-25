using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace POS.Model
{
   public class PosModel
    {
        /// <summary>
        /// 分部名称
        /// </summary>
        public string xlsname { get; set; }
        /// <summary>
        /// 单号
        /// </summary>
        public string billno { get; set; }
        /// <summary>
        ///共多少项
        /// </summary>
        public string totalCount { get; set; }
        /// <summary>
        /// 应收
        /// </summary>
        public decimal xpay { get; set; }
        /// <summary>
        /// 优惠
        /// </summary>
        public decimal xhezhe { get; set; }
        /// <summary>
        /// 现金
        /// </summary>
        public decimal cash { get; set; }
        /// <summary>
        /// 预存款
        /// </summary>
        public decimal deposit { get; set; }
        /// <summary>
        /// 积分抵扣现金
        /// </summary>
        public decimal jfcash { get; set; }
        /// <summary>
        /// 刷卡、支付宝、微信
        /// </summary>
        public string paytype { get; set; }
        /// <summary>
        /// 支付宝
        /// </summary>
        public decimal alipay { get; set; }
        /// <summary>
        /// 微信
        /// </summary>
        public decimal wechat { get; set; }
        /// <summary>
        /// 银联卡
        /// </summary>
        public decimal unionpaycard { get; set; }
        /// <summary>
        /// 优惠券
        /// </summary>
        public decimal coupon { get; set; }
        /// <summary>
        /// 支票
        /// </summary>
        public decimal check { get; set; }
        
        /// <summary>
        /// 找零
        /// </summary>
        public decimal balance { get; set; }
        /// <summary>
        /// 录入时间
        /// </summary>
        public string xintime { get; set; }
        /// <summary>
        /// 会员号
        /// </summary>
        public string clntcode { get; set; }
        /// <summary>
        /// 地址
        /// </summary>
        public string address { get; set; }
        /// <summary>
        /// 电话号码
        /// </summary>
        public string telephone { get; set; }

        /// <summary>
        /// 分部代码
        /// </summary>
        public string xls { get; set; }
        /// <summary>
        /// 操作员
        /// </summary>
        public string username { get; set; }
        /// <summary>
        /// 合计单据总额
        /// </summary>
        public decimal TotalOrderMoney { get; set; }
        /// <summary>
        /// 合计支付明细总额
        /// </summary>
        public decimal TotalPaytMoney { get; set; }
        /// <summary>
        /// 积分抵扣金额
        /// </summary>
        public decimal deductible { get; set; }
        /// <summary>
        /// 舍弃金额
        /// </summary>
        public decimal xrpay { get; set; }

        /// <summary>
        /// 上次累计积分
        /// </summary>
        public string totalXsendjf { get; set; }
        /// <summary>
        /// 本次积分
        /// </summary>
        public int? xsendjf { get; set; }
        /// <summary>
        /// 挂账
        /// </summary>
        public decimal debts { get; set; }
        /// <summary>
        /// 回款
        /// </summary>
        public decimal repayments { get; set; }
        public List<PosDetailModel> Details { get; set; }
    }
}
