using DevExpress.XtraBars;
using DevExpress.XtraEditors;
using POS.BLL;
using POS.Common;
using POS.Common.Enum;
using POS.Common.utility;
using POS.Helper;
using POS.Model;
using POS.Sale;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Script.Serialization;
using System.Windows.Forms;

namespace POS.Client
{
    /// <summary>
    /// 会员充值
    /// </summary>
    public partial class FormClientRecharge : BaseForm
    {
        ClntModel clnt;
        OfhhBLL ofhhBLL = new OfhhBLL();
        ClientBLL clientBLL = new ClientBLL();
        PaytBLL paytBLL = new PaytBLL();
        JavaScriptSerializer js = new JavaScriptSerializer();
        static ApplicationLogger logger = new ApplicationLogger(typeof(FormClientRecharge).Name);

        //标识正在付款
        private bool isPay = false;
        public FormClientRecharge(ClntModel clnt)
        {
            InitializeComponent();
            decimal ojie2 = clientBLL.GetOjie2(clnt.clntcode);
            txtBalance.EditValue = ojie2;
            this.ActiveControl = txtAmount;
            this.clnt = clnt;

            List<PaytModel> payts = paytBLL.GetPayt().Where(r => r.payttype.Trim() == "刷卡").ToList();
            foreach (var item in payts)
            {
                BarButtonItem barButtonItem = new BarButtonItem();
                barButtonItem.Caption = item.paytname;
                barButtonItem.Tag = item.paytcode;
                barButtonItem.ItemClick += BarButtonItem_ItemClick;
                popupMenu1.AddItem(barButtonItem);
            }

            chkPrint.Enabled = GetPermission(Functions.Print);
        }

        private void BarButtonItem_ItemClick(object sender, ItemClickEventArgs e)
        {
            ckBtnUnionpayCard.Text = e.Item.Caption;
        }

        #region 充值方式

        private void ckBtnCash_CheckedChanged(object sender, EventArgs e)
        {
            if (ckBtnCash.Checked)
            {
                ckBtnMobilePayment.Checked = false;
                ckBtnUnionpayCard.Checked = false;
            }
            else if (ckBtnMobilePayment.Checked == false
                   && ckBtnUnionpayCard.Checked == false
                   )
            {
                ckBtnCash.Checked = true;
            }
            SetPaymentType(ckBtnCash);
        }

        private void ckBtnMobilePayment_CheckedChanged(object sender, EventArgs e)
        {
            if (ckBtnMobilePayment.Checked)
            {
                ckBtnCash.Checked = false;
                ckBtnUnionpayCard.Checked = false;
            }
            else if (ckBtnCash.Checked == false
                   && ckBtnUnionpayCard.Checked == false
                   )
            {
                ckBtnMobilePayment.Checked = true;
            }
            SetPaymentType(ckBtnMobilePayment);
        }

        private void ckBtnUnionpayCard_CheckedChanged(object sender, EventArgs e)
        {
            if (ckBtnUnionpayCard.Checked)
            {
                ckBtnMobilePayment.Checked = false;
                ckBtnCash.Checked = false;
            }
            else if (ckBtnMobilePayment.Checked == false
                   && ckBtnCash.Checked == false
                   )
            {
                ckBtnUnionpayCard.Checked = true;
            }
            SetPaymentType(ckBtnUnionpayCard);
        }

        private void SetPaymentType(CheckButton checkButton)
        {
            if (checkButton.Checked)
            {
                checkButton.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.UltraFlat;
            }
            else
            {
                checkButton.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Default;
            }
        }
        #endregion

        #region 充值

        private void btnConfirm_Click(object sender, EventArgs e)
        {
            if (!GetPermission(Functions.Recharge))
            {
                return;
            }
            Dictionary<string, string> payTypeDic = EnumHelper.GetEnumDictionary(typeof(PayType));
            AlipayResult alipayResult = null;

            decimal fee = 0;
            if (!decimal.TryParse(txtAmount.Text.Trim(), out fee))
            {
                MessagePopup.ShowInformation("请输入正确的金额！");
            }
            else if (fee <= 0)
            {
                MessagePopup.ShowInformation("充值金额必须大于零！");
            }
            else
            {
                if (ckBtnUnionpayCard.Checked)
                {
                    if (ckBtnUnionpayCard.Text == "银联卡")
                    {
                        MessagePopup.ShowInformation("请选择银联卡账户！");
                        return;
                    }
                }
                if (CheckConnect())
                {
                    try
                    {
                        if (MessagePopup.ShowQuestion("确定给该会员充值？") == DialogResult.No)
                        {
                            return;
                        }
                        if (isPay)
                        {
                            MessagePopup.ShowInformation("正在付款,请勿重复提交！");
                            return;
                        }
                        isPay = true;

                        if (ckBtnMobilePayment.Checked)
                        {
                            FormAlipay frm = new FormAlipay(fee);
                            if (frm.ShowDialog() == DialogResult.OK)
                            {
                                alipayResult = frm.AlipayResult;
                            }
                            else
                            {
                                MessagePopup.ShowError("移动支付失败,请重试！");
                                return;
                            }
                        }

                        OfhhModel ofhh = new OfhhModel();
                        ofhh.ID = Guid.NewGuid();
                        ofhh.clntcode = clnt.clntcode;
                        ofhh.clntname = clnt.clntname;
                        ofhh.xinname = RuntimeObject.CurrentUser.username;
                        ofhh.xintime=DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");
                        ofhh.xnote = RuntimeObject.CurrentUser.posnono;
                        ofhh.xversion = double.Parse(GetTimeStamp());
                        ofhh.xdate = DateTime.Now;
                        ofhh.xnote = metxnote.Text.Trim();

                        OfbbModel ofbb = new OfbbModel();
                        ofbb.ID = Guid.NewGuid();
                        if (ckBtnCash.Checked)
                        {
                            ofbb.xztype = payTypeDic[Enum.GetName(typeof(PayType), PayType.Cash)];
                        }
                        else if (ckBtnMobilePayment.Checked)
                        {
                            ofbb.transno = alipayResult.out_trade_no;
                            ofbb.xztype = alipayResult.way;
                        }
                        else if (ckBtnUnionpayCard.Checked)
                        {
                            // ofbb.xztype = payTypeDic[Enum.GetName(typeof(PayType), PayType.UnionpayCard)];
                            ofbb.xztype = ckBtnUnionpayCard.Text.Trim();
                        }
                        //状态为已支付、未支付
                        ofbb.xzstate = "已支付";
                        ofbb.xfee = fee;
                        ofbb.xsubsidy = 0;
                        ofbb.xversion = double.Parse(GetTimeStamp());
                        List<OfbbModel> ofbbList = new List<OfbbModel>();
                        ofbbList.Add(ofbb);
                        ofhh.ofbbs = ofbbList;

                        OfhhModel sofhh = AddOfhh(ofhh);
                        if (sofhh != null)
                        {
                            ofhh.SID = sofhh.SID;
                            ofhh.xtableid = sofhh.xtableid;


                            OfbbModel sofbb = sofhh.ofbbs.FirstOrDefault();
                            if (sofbb != null)
                            {
                                foreach (OfbbModel p in ofhh.ofbbs)
                                {
                                    p.SID = sofbb.SID;
                                    p.xtableid = sofbb.xtableid;
                                    p.xsubid = ofhh.xtableid;
                                }
                            }
                            bool result = ofhhBLL.AddOfhh(ofhh);
                            SyncData(new List<string> { "ojie2" }, 0, AppConst.Sync_Cashier_Msg, false);

                            Print(ofbb);
                            this.DialogResult = DialogResult.OK;
                        }
                        else if (ckBtnMobilePayment.Checked && alipayResult != null)
                        {
                            bool result = ofhhBLL.AddOfhh(ofhh);
                            SyncData(new List<string> { "ofhh", "ojie2" }, 0, AppConst.Sync_Cashier_Msg, false);

                            Print(ofbb);
                            this.DialogResult = DialogResult.OK;
                        }
                        else
                        {
                            MessagePopup.ShowError("充值失败！");
                        }
                    }
                    catch (Exception)
                    {
                        MessagePopup.ShowError("充值失败！");
                        SyncData(new List<string> { "ojie2" }, 0, AppConst.Sync_Cashier_Msg, false);
                    }
                    finally
                    {
                        isPay = false;
                    }
                }
            }

        }

        private void Print(OfbbModel ofbb)
        {
            if (chkPrint.Checked)
            {
                System.Threading.Thread.Sleep(1000);
                ClientRechargeModel clientRecharge = new ClientRechargeModel();
                clientRecharge.xls = RuntimeObject.CurrentUser.xls;
                clientRecharge.username = RuntimeObject.CurrentUser.username;
                clientRecharge.xintime = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");
                clientRecharge.clntcode = clnt.clntcode;
                clientRecharge.clntname = clnt.clntname;
                clientRecharge.xfee = string.Format("{0}({1})", txtAmount.Text, ofbb.xztype);
                UserModel user = RuntimeObject.CurrentUser;
                decimal ojie2 = SyncHelperBLL.Getyck(user.bookID, clnt.clntcode,user.xls);
                clientRecharge.balance = ojie2;
                clientRecharge.integral = SyncHelperBLL.Getjifen(user.bookID, clnt.clntcode,user.xls);
                PrintClientRechargeHelper.Print(clientRecharge);
            }
        }
        #endregion

        #region 同步数据
        private void SyncData(string sid, string xls, string usercode)
        {
            //DevExpress.Utils.WaitDialogForm dlg = new DevExpress.Utils.WaitDialogForm("正在同步单据数据，请稍后……", new Size(250, 100));
            //dlg.Show();
            //try
            //{
            //    SyncHelperBLL.SyncData(sid, xls, usercode, new List<string> { "ofhh", "ojie2" });
            //}
            //catch (Exception ex)
            //{
            //    dlg.Close();
            //    string msg = AppConst.Sync_Cashier_Msg;
            //    MessagePopup.ShowInformation(msg);
            //}
            //finally
            //{
            //    dlg.Close();
            //}
        }
        #endregion

        private void ckBtnUnionpayCard_Click(object sender, EventArgs e)
        {
            Point panelScreenPoint = groupControl1.PointToScreen(ckBtnUnionpayCard.Location);
            Point point = new Point(panelScreenPoint.X, panelScreenPoint.Y + ckBtnUnionpayCard.Location.Y);
            popupMenu1.ShowPopup(point);
        }

        private OfhhModel AddOfhh(OfhhModel ofhh)
        {
            List<OfhhModel> ofhhs = new List<OfhhModel>();
            ofhhs.Add(ofhh);
            var query = from p in ofhhs
                        select new
                        {
                            ID = p.ID,
                            SID = p.SID,
                            xdate = p.xdate.ToString("yyyy-MM-dd"),
                            clntcode = p.clntcode,
                            clntname = p.clntname,
                            xnote = p.xnote,
                            xinname = p.xinname,
                            xintime = p.xintime,
                            xversion = p.xversion,
                            ofbbs = from d in p.ofbbs
                                    select new
                                    {
                                        ID = d.ID,
                                        SID = d.SID,
                                        xnoteb = d.xnoteb,
                                        xztype = d.xztype,
                                        xzstate = d.xzstate,
                                        xfee = d.xfee,
                                        xversion = p.xversion,
                                    }
                        };

            string json = js.Serialize(query);
            UserModel user = RuntimeObject.CurrentUser;
            //string postData = "sid=" + RuntimeObject.CurrentUser.bookID;
            //postData += "&mod=" + "ofhh";
            //postData += "&xls=" + RuntimeObject.CurrentUser.xls;
            //postData += "&usercode=" + RuntimeObject.CurrentUser.usercode;
            //postData += "&data=" + HttpUtility.UrlEncode(json, Encoding.UTF8);
            string postData = GlobalHelper.GetPostData(user.bookID, "ofhh", user.xls, user.usercode, json, GetSecretKey());
            byte[] byteArray = Encoding.UTF8.GetBytes(postData);

            DevExpress.Utils.WaitDialogForm dlg = new DevExpress.Utils.WaitDialogForm("正在处理数据，请稍后……", new Size(250, 100));
            dlg.Show();
            try
            {
                SyncResultModel<OfhhModel> syncResult = HttpClient.PostAasync<OfhhModel>(baseUrl_Push.ToString(), null, postData);
                List<OfhhModel> datas = syncResult.datas;
                if (datas != null && datas.Count > 0)
                {
                    return datas.FirstOrDefault();
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
                return null;
            }
            finally
            {
                dlg.Close();
            }
        }
    }
}
