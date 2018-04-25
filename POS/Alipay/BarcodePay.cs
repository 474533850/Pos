using Com.Alipay;
using Com.Alipay.Business;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Com.Alipay.Domain;

namespace POS.Alipay
{
    public class BarcodePay
    {
        //static IAlipayTradeService serviceClient = F2FBiz.CreateClientInstance(AlipayConfig.serverUrl, AlipayConfig.appId, AlipayConfig.merchant_private_key, AlipayConfig.version,
        //                                                                  AlipayConfig.sign_type, AlipayConfig.alipay_public_key, AlipayConfig.charset);

        static IAlipayTradeService serviceClient = F2FBiz.CreateClientInstance(AlipayConfig.serverUrl, AlipayConfig.appId, AlipayConfig.merchant_private_key, AlipayConfig.version,
                                                                         AlipayConfig.sign_type, AlipayConfig.alipay_public_key, AlipayConfig.charset);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="terminal_id">机具号</param>
        /// <param name="total_fee"></param>
        /// <param name="auth_code"></param>
        /// <returns></returns>
        public static AlipayF2FPayResult Run(string terminal_id, string total_fee, string auth_code)
        {
            AlipayTradePayContentBuilder builder = BuildPayContent(terminal_id, total_fee, auth_code);
            string out_trade_no = builder.out_trade_no;

            AlipayF2FPayResult payResult = serviceClient.tradePay(builder);

            //switch (payResult.Status)
            //{
            //    case ResultEnum.SUCCESS:
            //        DoSuccessProcess(payResult);
            //        break;
            //    case ResultEnum.FAILED:
            //        DoFailedProcess(payResult);
            //        break;
            //    case ResultEnum.UNKNOWN:
            //        result = "网络异常，请检查网络配置后，更换外部订单号重试";
            //        break;
            //}
            //Response.Redirect("result.aspx?Text=" + result);
            return payResult;
        }

        /// <summary>
        /// 构造支付请求数据
        /// </summary>
        /// <returns>请求数据集</returns>
        private static AlipayTradePayContentBuilder BuildPayContent(string terminal_id, string total_fee, string auth_code,string seller_id="")
        {
            //线上联调时，请输入真实的外部订单号。
            string out_trade_no = System.DateTime.Now.ToString("yyyyMMddHHmmss") + "0000" + (new Random()).Next(1, 10000).ToString();
            //扫码枪扫描到的用户手机钱包中的付款条码
            AlipayTradePayContentBuilder builder = new AlipayTradePayContentBuilder();

            //收款账号
            builder.seller_id = AlipayConfig.pid;
            //订单编号
            builder.out_trade_no = out_trade_no;
            //支付场景，无需修改
            builder.scene = "bar_code";
            //支付授权码,付款码
            builder.auth_code = auth_code.Trim();
            //订单总金额
            builder.total_amount = total_fee.Trim();
            //参与优惠计算的金额
            //builder.discountable_amount = "";
            //不参与优惠计算的金额
            //builder.undiscountable_amount = "";
            //订单名称
            builder.subject = "条码支付";
            //自定义超时时间
            builder.timeout_express = "2m";
            //订单描述
            builder.body = "";
            //门店编号，很重要的参数，可以用作之后的营销
            //builder.store_id = "test store id";
            //操作员编号，很重要的参数，可以用作之后的营销
            //builder.operator_id = "test";
            //商户机具终端编号
            builder.terminal_id = terminal_id;


            //传入商品信息详情
            //List<GoodsInfo> gList = new List<GoodsInfo>();

            //GoodsInfo goods = new GoodsInfo();
            //goods.goods_id = "304";
            //goods.goods_name = "goods#name";
            //goods.price = "0.01";
            //goods.quantity = "1";
            //gList.Add(goods);
            //builder.goods_detail = gList;

            //系统商接入可以填此参数用作返佣
            //ExtendParams exParam = new ExtendParams();
            //exParam.sysServiceProviderId = "20880000000000";
            //builder.extendParams = exParam;

            return builder;

        }
    }
}
