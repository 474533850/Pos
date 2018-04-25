using Com.Alipay;
using Com.Alipay.Business;
using Com.Alipay.Domain;
using POS.Common.utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace POS.Alipay
{
    public class AlipayRefund
    {
        static ApplicationLogger logger = new ApplicationLogger(typeof(AlipayRefund).Name);
        static IAlipayTradeService serviceClient = F2FBiz.CreateClientInstance(AlipayConfig.serverUrl, AlipayConfig.appId, AlipayConfig.merchant_private_key, AlipayConfig.version,
                            AlipayConfig.sign_type, AlipayConfig.alipay_public_key, AlipayConfig.charset);
        public static AlipayF2FRefundResult Run(string out_trade_no, string total_fee, string refund_fee)
        {
            AlipayTradeRefundContentBuilder builder = BuildContent(out_trade_no, total_fee, refund_fee);
            AlipayF2FRefundResult refundResult = serviceClient.tradeRefund(builder);
            return refundResult;
        }

        /// <summary>
        /// 构造退款请求数据
        /// </summary>
        /// <returns>请求数据集</returns>
        private static AlipayTradeRefundContentBuilder BuildContent(string out_trade_no, string total_fee, string refund_fee)
        {
            AlipayTradeRefundContentBuilder builder = new AlipayTradeRefundContentBuilder();

            //支付宝交易号与商户网站订单号不能同时为空
            builder.out_trade_no = out_trade_no;

            //退款请求单号保持唯一性。
            //builder.out_request_no = WIDout_request_no.Text.Trim();

            //退款金额
            builder.refund_amount = refund_fee;

            builder.refund_reason = "refund reason";

            return builder;

        }
    }
}
