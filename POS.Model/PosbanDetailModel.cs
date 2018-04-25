using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace POS.Model
{
    /// <summary>
    /// 交班详情
    /// </summary>
    public class PosbanDetailModel
    {
        /// <summary>
        ///现金
        /// </summary>
        public decimal TotalCash { get; set; }
        /// <summary>
        /// 现金支付单数
        /// </summary>
        public int CashCount { get; set; }

        /// <summary>
        ///预存款
        /// </summary>
        public decimal TotalDeposit { get; set; }
        /// <summary>
        /// 预存款支付单数
        /// </summary>
        public int DepositCount { get; set; }

        /// <summary>
        ///微信
        /// </summary>
        public decimal TotalWeChat { get; set; }
        /// <summary>
        /// 微信支付单数
        /// </summary>
        public int WeChatCount { get; set; }

        /// <summary>
        ///支付宝
        /// </summary>
        public decimal TotalAlipay { get; set; }
        /// <summary>
        /// 支付宝支付单数
        /// </summary>
        public int AlipayCount { get; set; }

        /// <summary>
        ///银联卡
        /// </summary>
        public decimal TotalUnionpayCard { get; set; }
        /// <summary>
        /// 银联卡支付单数
        /// </summary>
        public int UnionpayCardCount { get; set; }

        /// <summary>
        ///优惠券数量
        /// </summary>
        public int TotalCouponQuantity { get; set; }
        /// <summary>
        /// 优惠券支付单数
        /// </summary>
        public int CouponCount { get; set; }

        /// <summary>
        ///优惠券金额
        /// </summary>
        public decimal TotalCoupon { get; set; }

        /// <summary>
        ///销售总额
        /// </summary>
        public decimal TotalAmount { get; set; }
        /// <summary>
        /// 退款总额
        /// </summary>
        public decimal TotalRefund { get; set; }
        /// <summary>
        /// 反结总额
        /// </summary>
        public decimal TotalInvalid { get; set; }
        /// <summary>
        /// 总单据数
        /// </summary>
        public decimal TotalOrderCount { get; set; }

        /// <summary>
        /// 正常单数
        /// </summary>
        public decimal PosCount { get; set; }
        /// <summary>
        /// 反结单数
        /// </summary>
        public decimal InvalidCount { get; set; }
        /// <summary>
        /// 退货单数
        /// </summary>
        public decimal ReturnCount { get; set; }
        /// <summary>
        /// 挂单单数
        /// </summary>
        public decimal PendingCount { get; set; }
        /// <summary>
        /// 促销统计
        /// </summary>
        public decimal TotalSale { get; set; }
        /// <summary>
        /// 手动折扣金额
        /// </summary>
        public decimal Money_Manual { get; set; }
        /// <summary>
        /// 手动折扣数量
        /// </summary>
        public decimal Quantity_Manual { get; set; }
        /// <summary>
        /// 手动折扣单数
        /// </summary>
        public decimal Count_Manual { get; set; }
        /// <summary>
        /// 客户折扣金额
        /// </summary>
        public decimal Money_Clnt { get; set; }
        /// <summary>
        /// 客户折扣数量
        /// </summary>
        public decimal Quantity_Clnt { get; set; }
        /// <summary>
        /// 客户折扣单数
        /// </summary>
        public decimal Count_Clnt { get; set; }
        /// <summary>
        /// 促销折扣金额
        /// </summary>
        public decimal Money_Sale { get; set; }
        /// <summary>
        /// 促销折扣数量
        /// </summary>
        public decimal Quantity_Sale { get; set; }
        /// <summary>
        /// 促销折扣单数
        /// </summary>
        public decimal Count_Sale { get; set; }
        /// <summary>
        /// 积分兑换金额
        /// </summary>
        public decimal Money_Exchange { get; set; }
        /// <summary>
        /// 积分兑换数量
        /// </summary>
        public decimal Quantity_Exchange { get; set; }
        /// <summary>
        /// 积分兑换单数
        /// </summary>
        public decimal Count_Exchange { get; set; }

        /// <summary>
        /// 积分抵扣金额
        /// </summary>
        public decimal Money_Deductible { get; set; }
        /// <summary>
        /// 积分抵扣金额单数
        /// </summary>
        public decimal Count_Deductible { get; set; }

        /// <summary>
        /// 会员充值—总计
        /// </summary>
        public decimal Total_Recharge { get; set; }
        /// <summary>
        /// 会员充值—现金
        /// </summary>
        public decimal Cash_Recharge { get; set; }
        /// <summary>
        /// 会员充值—微信
        /// </summary>
        public decimal WeChat_Recharge { get; set; }
        /// <summary>
        /// 会员充值—支付宝
        /// </summary>
        public decimal Alipay_Recharge { get; set; }
        /// <summary>
        /// 会员充值—银联卡
        /// </summary>
        public decimal UnionpayCard_Recharge { get; set; }
        /// <summary>
        /// 挂账
        /// </summary>
        public decimal Debts { get; set; }
        /// <summary>
        /// 挂账支付单数
        /// </summary>
        public int DebtsCount { get; set; }
        /// <summary>
        /// 支票
        /// </summary>
        public decimal Check { get; set; }
        /// <summary>
        /// 支票支付单数
        /// </summary>
        public int CheckCount { get; set; }

        public string Address { get; set; }
        public string UserName { get; set; }
        public string DateTime { get; set; }
        /// <summary>
        /// 回款金额
        /// </summary>
        public decimal Repayments { get; set; }
    }
}
