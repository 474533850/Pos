using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Linq;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using System.Net;
using System.IO;
using POS.Common.utility;
using System.Web;
using System.Web.Script.Serialization;
using POS.Model;
using POS.Helper;
using System.Configuration;
using POS.BLL;
using POS.Common;
using POS.WxPayAPI;
using Com.Alipay.Business;
using POS.Alipay;
using Com.Alipay.Model;

namespace POS.Sale
{
    public partial class FormAlipay : BaseForm
    {
        static ApplicationLogger logger = new ApplicationLogger(typeof(FormAlipay).Name);
        public AlipayResult AlipayResult { get; set; }
        PossettingBLL possettingBLL = new PossettingBLL();
        public FormAlipay(decimal amount)
        {
            InitializeComponent();
            lblAmount.Text = amount.ToString();
            lblTradeNO.Text = string.Empty;
            btnConfirm.Enabled = true;
        }

        private void txtBarCode_KeyDown(object sender, KeyEventArgs e)
        {
            if (!string.IsNullOrEmpty(txtBarCode.Text.Trim()))
            {
                if (e.KeyCode == Keys.Return)
                {
                    PPayTo();
                }
            }
        }
        public void PPayTo()
        {
            btnConfirm.Enabled = false;
            object obj = new object();
            lock (obj)
            {
                string deviceid = string.Empty;
                Dictionary<object, object> machineIDs = GetPayCodeMachineIDs();
                if (machineIDs != null)
                {
                    var query = machineIDs.Where(r => r.Key.ToString() == RuntimeObject.CurrentUser.xls).FirstOrDefault();
                    if (query.Value == null || string.IsNullOrEmpty(query.Value.ToString()))
                    {
                        MessagePopup.ShowInformation("请先设置贝壳设备号！");
                        return;
                    }
                    else
                    {
                        deviceid = query.Value.ToString();
                    }
                }
                else
                {
                    MessagePopup.ShowInformation("请先设置贝壳设备号！");
                    return;
                }

                DevExpress.Utils.WaitDialogForm dlg = new DevExpress.Utils.WaitDialogForm("正在支付，请稍后……", new Size(250, 100));
                dlg.Show();

                int paymentChannel = GetPaymentChannel();
                if (paymentChannel == 0)
                {
                    try
                    {
                        Uri url = AppConst.payUrl;
                        //Uri url = new Uri("http://www.51bfmall.com/pay/controller/machines.php");

                        string code = txtBarCode.Text.Trim();
                        string amount = lblAmount.Text;
                        string service = "pay";
                        string key = "asjdIIdakscKasd56522asd";
                        string paramStr = string.Format("amount={0}&code={1}&deviceid={2}&service={3}&key={4}", amount, code, deviceid, service, key);
                        string sign = MD5Helper.GetMd5Hash(paramStr.Trim().ToUpper());
                        Alipay alipay = new Alipay();
                        alipay.deviceid = deviceid;
                        alipay.code = code;
                        alipay.amount = amount;
                        alipay.service = service;
                        alipay.sign = sign;

                        JavaScriptSerializer serializer = new JavaScriptSerializer();
                        string json = serializer.Serialize(alipay);
                        //转换输入参数的编码类型，获取bytep[]数组 
                        byte[] byteArray = Encoding.UTF8.GetBytes(json);
                        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                        request.Method = "POST";
                        request.ContentType = "application/x-www-form-urlencoded";
                        request.ContentLength = byteArray.Length;
                        Stream newStream = request.GetRequestStream();
                        newStream.Write(byteArray, 0, byteArray.Length);
                        newStream.Close();
                        //4． 读取服务器的返回信息
                        HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                        StreamReader php = new StreamReader(response.GetResponseStream(), Encoding.UTF8);
                        string result = php.ReadToEnd();
                        php.Close();
                        response.Close();
                        AlipayResult alipayResult = serializer.Deserialize<AlipayResult>(result);
                        if (alipayResult != null)
                        {
                            AlipayResult = alipayResult;
                            lblTradeNO.Text = alipayResult.out_trade_no;
                            if (alipayResult.Trade_status == "PAY_FAIL")
                            {
                                Pay_Fail();
                            }
                            else if (alipayResult.Trade_status == "TRADE_SUCCESS" || alipayResult.Trade_status == "SUCCESS")
                            {
                                Pay_Success(amount, alipayResult);
                            }
                            else if (alipayResult.Trade_status == "WAIT_BUYER_PAY" || alipayResult.Trade_status == "USERPAYING")
                            {
                                MessagePopup.ShowInformation("正在支付,请勿重复提交！");
                            }
                            else
                            {
                                MessagePopup.ShowInformation("支付超时！");
                            }
                        }
                        else
                        {
                            Pay_Fail();
                        }
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                    finally
                    {
                        btnConfirm.Enabled = true;
                        dlg.Close();
                    }
                }
                else if (paymentChannel == 1)
                {
                    //原生通道
                    string start = txtBarCode.Text.Trim().Substring(0, 1);
                    try
                    {
                        AlipayResult alipayResult = new AlipayResult();
                        if (start == "1")
                        {
                            //微信
                            alipayResult.way_id = "2";
                            alipayResult.way = "微信";
                            int amount = (int)(decimal.Parse(lblAmount.Text) * 100);
                            WxPayData result = MicroPay.Run(deviceid, amount.ToString(), txtBarCode.Text);
                            //支付成功
                            if (result.GetValue("trade_state").ToString() == "SUCCESS")
                            {
                                //成功
                                alipayResult.out_trade_no = result.GetValue("out_trade_no").ToString();
                                AlipayResult = alipayResult;
                                Pay_Success(lblAmount.Text, alipayResult);
                            }
                            else if (result.GetValue("err_code").ToString() != "USERPAYING" &&
                                     result.GetValue("err_code").ToString() != "SYSTEMERROR")
                            {
                                Pay_Fail();
                            }
                            //用户支付中，需要继续查询
                            else if (result.GetValue("trade_state") != null && result.GetValue("trade_state").ToString() == "USERPAYING")
                            {
                                MessagePopup.ShowInformation("正在支付,请勿重复提交！");
                            }
                            else if (result.GetValue("err_code") != null && result.GetValue("err_code").ToString() == "ORDERNOTEXIST")
                            {
                                Pay_Fail();
                            }
                            else
                            {
                                MessagePopup.ShowInformation("支付超时！");
                            }
                        }
                        else if (start == "2")
                        {
                            //支付宝
                            alipayResult.way_id = "1";
                            alipayResult.way = "支付宝";

                            AlipayF2FPayResult payResult = BarcodePay.Run(deviceid, lblAmount.Text.Trim(), txtBarCode.Text.Trim());
                            switch (payResult.Status)
                            {
                                case ResultEnum.SUCCESS:
                                    alipayResult.out_trade_no = payResult.response.OutTradeNo;
                                    AlipayResult = alipayResult;
                                    Pay_Success(lblAmount.Text, alipayResult);
                                    break;
                                case ResultEnum.FAILED:
                                    logger.Debug(payResult.response.Body);
                                    Pay_Fail();
                                    break;
                                case ResultEnum.UNKNOWN:
                                    MessagePopup.ShowInformation("网络异常，请检查网络配置后，更换外部订单号重试！");
                                    break;
                            }
                        }
                        else
                        {
                            MessagePopup.ShowInformation("非法条码！");
                        }
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                    finally
                    {
                        btnConfirm.Enabled = true;
                        dlg.Close();
                    }
                }
            }
        }

        private void Pay_Success(string amount, AlipayResult alipayResult)
        {
            lblInfo.Text = "支付成功！";
            FormAlipayConfirm frm = new FormAlipayConfirm(alipayResult.way, amount, alipayResult.out_trade_no);
            frm.ShowDialog();
            btnConfirm.Enabled = true;
            this.DialogResult = DialogResult.OK;
        }

        private void Pay_Fail()
        {
            MessagePopup.ShowError("支付失败，请换一种方式重试！");
            txtBarCode.Text = string.Empty;
        }
        private void btnConfirm_Click(object sender, EventArgs e)
        {
            PPayTo();
        }
    }

    public class Alipay
    {
        public string deviceid { get; set; }

        public string code { get; set; }

        public string amount { get; set; }

        public string service { get; set; }
        public string sign { get; set; }

    }
}