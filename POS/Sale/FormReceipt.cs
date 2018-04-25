using DevExpress.Skins;
using DevExpress.XtraBars;
using DevExpress.XtraBars.Alerter;
using DevExpress.XtraBars.Docking;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.ViewInfo;
using POS.BLL;
using POS.Client;
using POS.Common;
using POS.Common.Enum;
using POS.Common.utility;
using POS.Helper;
using POS.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Web.Script.Serialization;
using System.Windows.Forms;

namespace POS.Sale
{
    public partial class FormReceipt : BaseForm
    {
        //订单总额
        private decimal totalAmount;
        //是否手动录入金额
        private bool isInputAmount = false;

        PoshhModel poshh;
        ClntModel client;
        private AlipayResult alipayResult;
        //零头处理金额
        private decimal eraseAmount = decimal.Zero;
        //零头处理舍弃的金额
        decimal difference;
        //积分兑换预存款比例
        decimal rate = 0;
        //最大积分百分比
        decimal maxJFExch = 0;
        //是否快速开单
        bool fastBilling = false;

        POSBLL posBLL = new POSBLL();
        PossettingBLL possettingBLL = new PossettingBLL();
        TickoffBLL tickoffBLL = new TickoffBLL();
        ClientBLL clientBLL = new ClientBLL();
        PaytBLL paytBLL = new PaytBLL();
        JavaScriptSerializer js = new JavaScriptSerializer();
        Dictionary<string, string> stateDic = EnumHelper.GetEnumDictionary(typeof(PosState));
        Form preForm;

        /// <summary>
        /// 换货原单据,不为空为换货
        /// </summary>
        PoshhModel prePoshh = null;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="poshh"></param>
        /// <param name="client"></param>
        /// <param name="fastBilling">是否快速开单</param>
        /// <param name="prePoshh">不为空为换货</param>
        public FormReceipt(PoshhModel poshh, ClntModel client, bool fastBilling = false, PoshhModel prePoshh = null, Form preForm = null)
        {
            InitializeComponent();
            this.poshh = poshh;
            this.prePoshh = prePoshh;
            this.client = client;
            this.fastBilling = fastBilling;
            this.preForm = preForm;
            UCDiscount.ConfirmClick += UCDiscount_ConfirmClick;
            UCDiscount.IntegerClick += UCDiscount_IntegerClick;
            txtCash.Focus();
            //if (prePoshh == null)
            //{
            totalAmount = poshh.xpay;
            eraseAmount = CalcEraseAmount(poshh.xpay, out difference);
            UCDiscount.Enabled = true;
            //}
            //else
            //{
            //    totalAmount = Math.Abs(CalcMoneyHelper.Subtract(prePoshh.xpay, poshh.xpay));
            //    eraseAmount = CalcEraseAmount(totalAmount, out difference);
            //    UCDiscount.Enabled = false;
            //}

            txtTotal.Text = eraseAmount.ToString();
            txtCash.Text = eraseAmount.ToString();
            txtTotal.ReadOnly = !fastBilling;
            txtDeposit.ReadOnly = true;
            txtMobile.ReadOnly = true;
            txtUnionpayCard.ReadOnly = true;
            txtCheck.ReadOnly = true;
            if (fastBilling)
            {
                this.ActiveControl = txtTotal;
            }
            else
            {
                this.ActiveControl = txtCash;
            }
            txtTotal.MouseUp += Txt_MouseUp;
            txtTotal.MouseUp += Txt_MouseUp;
            txtDeposit.MouseUp += Txt_MouseUp;
            txtCash.MouseUp += Txt_MouseUp;
            txtMobile.MouseUp += Txt_MouseUp;
            txtUnionpayCard.MouseUp += Txt_MouseUp;
            txtCheck.MouseUp += Txt_MouseUp;

            //DateTime currentTime = DateTime.Now;
            //if (SyncHelperBLL.CheckConnect(out currentTime))
            //{
            //    IntegralDeduction(client, null);
            //}

            List<PaytModel> payts = paytBLL.GetPayt().Where(r => r.payttype.Trim() == "刷卡").ToList();
            foreach (var item in payts)
            {
                BarButtonItem barButtonItem = new BarButtonItem();
                barButtonItem.Caption = item.paytname;
                barButtonItem.Tag = item.paytcode;
                barButtonItem.ItemClick += BarButtonItem_ItemClick;
                popupMenu1.AddItem(barButtonItem);
            }
        }

        private void BarButtonItem_ItemClick(object sender, ItemClickEventArgs e)
        {
            ckBtnUnionpayCard.Text = e.Item.Caption;
        }

        #region 积分抵扣金额
        private void IntegralDeduction(ClntModel client, decimal? xpoints)
        {
            if (client != null)
            {
                //if (SyncHelperBLL.CheckConnect())
                //{
                PossettingModel possetting = possettingBLL.GetPossettingByKey(AppConst.INTEGRAL_RULES);
                if (possetting != null)
                {
                    if (!string.IsNullOrEmpty(possetting.xpvalue))
                    {
                        Dictionary<object, object> uclsspricsDic = js.Deserialize<Dictionary<object, object>>(possetting.xpvalue);
                        var query = uclsspricsDic.Where(r => r.Key.ToString() == "type").FirstOrDefault();
                        decimal value = 0;
                        if (decimal.TryParse(query.Value.ToString(), out value))
                        {
                            if (value != 0)
                            {
                                // decimal jjie2 = clientBLL.GetJjie2(client.clntcode);
                                decimal jjie2 = 0;
                                DevExpress.Utils.WaitDialogForm dlg = new DevExpress.Utils.WaitDialogForm("正在读取会员积分，请稍后……", new Size(250, 100));
                                dlg.Show();
                                try
                                {
                                    UserModel user = RuntimeObject.CurrentUser;
                                    jjie2 = SyncHelperBLL.Getjifen(user.bookID, client.clntcode,user.xls);
                                }
                                catch (Exception)
                                {
                                    MessagePopup.ShowInformation("网络异常,没有获取到会员积分,可以继续开单！");
                                }
                                finally
                                {
                                    dlg.Close();
                                }
                                query = uclsspricsDic.Where(r => r.Key.ToString() == "rate").FirstOrDefault();
                                maxJFExch = 100;
                                //最大积分百分比
                                var maxJFExch_Query = uclsspricsDic.Where(r => r.Key.ToString() == "maxJFExch").FirstOrDefault();
                                if (maxJFExch_Query.Value != null)
                                {
                                    if (decimal.TryParse(maxJFExch_Query.Value.ToString(), out maxJFExch)) { }
                                }

                                if (decimal.TryParse(query.Value.ToString(), out rate))
                                {
                                    if (rate != 0)
                                    {
                                        if (jjie2 > 0)
                                        {
                                            //int maxJF = (int)CalcMoneyHelper.Multiply(jjie2, (maxJFExch / 100));
                                            //int increment = (int)CalcMoneyHelper.Divide(1, rate);
                                            //int multiple = (int)CalcMoneyHelper.Divide(maxJF, increment);

                                            //speXpoints.Properties.Increment = increment;
                                            //speXpoints.Properties.MinValue = 0;
                                            //speXpoints.Properties.MaxValue = (int)increment * multiple;
                                            //speXpoints.Value = xpoints.HasValue ? xpoints.Value : speXpoints.Properties.MaxValue;
                                            //整单抵扣的比例金额
                                            int maxMoney = (int)CalcMoneyHelper.Multiply(txtTotal.Text, (maxJFExch / 100));
                                            int maxJF = (int)CalcMoneyHelper.Divide(maxMoney, rate);
                                            if (jjie2 < maxJF)
                                            {
                                                maxJF = (int)jjie2;
                                            }

                                            //int maxJF = (int)CalcMoneyHelper.Multiply(jjie2, (maxJFExch / 100));
                                            //一块钱需要的积分
                                            int increment = (int)CalcMoneyHelper.Divide(1, rate);
                                            //最大抵扣金额
                                            int multiple = (int)CalcMoneyHelper.Divide(maxJF, increment);

                                            speXpoints.Properties.Increment = increment;
                                            speXpoints.Properties.MinValue = 0;
                                            speXpoints.Properties.MaxValue = (int)increment * multiple;
                                            speXpoints.Value = xpoints.HasValue ? xpoints.Value : speXpoints.Properties.MaxValue;

                                            speXpoints_EditValueChanged(null, null);

                                            chkxpoints.Visible = true;
                                            speXpoints.Visible = true;
                                        }
                                        else
                                        {
                                            chkxpoints.Visible = false;
                                            speXpoints.Visible = false;
                                        }
                                    }
                                    else
                                    {
                                        chkxpoints.Visible = false;
                                        speXpoints.Visible = false;
                                    }
                                }
                                else
                                {
                                    chkxpoints.Visible = false;
                                    speXpoints.Visible = false;
                                }
                            }
                            else
                            {
                                chkxpoints.Visible = false;
                                speXpoints.Visible = false;
                            }
                        }
                        else
                        {
                            chkxpoints.Visible = false;
                            speXpoints.Visible = false;
                        }

                    }
                    else
                    {
                        chkxpoints.Visible = false;
                        speXpoints.Visible = false;
                    }
                }
                else
                {
                    chkxpoints.Visible = false;
                    speXpoints.Visible = false;
                }
                //}
                //else
                //{
                //    chkxpoints.Visible = false;
                //}
            }
            else
            {
                chkxpoints.Visible = false;
                speXpoints.Visible = false;
            }
        }
        #endregion

        private void Txt_MouseUp(object sender, MouseEventArgs e)
        {
            TextEdit editor = sender as TextEdit;
            if (editor != null)
                editor.SelectAll();
        }

        #region 抹零
        private void UCDiscount_IntegerClick(object sender, EventArgs e)
        {
            decimal total;
            if (decimal.TryParse(txtTotal.Text, out total))
            {
                txtTotal.EditValue = (int)total;
                txtTotal.Focus();
            }

        }
        #endregion

        #region 确认（折扣）
        private void UCDiscount_ConfirmClick(object sender, EventArgs e)
        {
            decimal currentDiscount = UCDiscount.CurrentDiscount;
            decimal totalMoney = CalcMoneyHelper.Multiply(totalAmount, currentDiscount);
            eraseAmount = CalcEraseAmount(totalMoney, out difference);
            //计算金额
            txtTotal.EditValue = eraseAmount;

        }

        #endregion

        #region 键盘操作
        private void btnBack_Click(object sender, EventArgs e)
        {
            SendKeys.Send("{BACKSPACE}");
        }
        private void btnKey_Click(object sender, EventArgs e)
        {
            SimpleButton btn = sender as SimpleButton;
            SendKeys.Send(btn.Tag.ToString());
        }
        #endregion

        #region  确认收款
        private void btnConfirm_Click(object sender, EventArgs e)
        {
            this.Validate(true);

            if (Checked())
            {
                if (btnConfirm.Enabled)
                {
                    btnConfirm.Enabled = false;
                    if (ckBtnDeposit.Checked || chkxpoints.Checked)
                    {
                        if (decimal.Parse(txtDeposit.Text) != 0 || chkxpoints.Tag != null)
                        {
                            if (!CheckConnect())
                            {
                                return;
                            }
                            //SyncHelperBLL.SyncData(RuntimeObject.CurrentUser.bookID, RuntimeObject.CurrentUser.xls, RuntimeObject.CurrentUser.usercode, new List<string> { "ojie2", "jjie2" });
                            //Thread.Sleep(1000);
                            //decimal ojie2 = clientBLL.GetOjie2(client.clntcode);
                            if (ckBtnDeposit.Checked)
                            {
                                DevExpress.Utils.WaitDialogForm dlg = new DevExpress.Utils.WaitDialogForm("正在读取会员余额，请稍后……", new Size(250, 100));
                                dlg.Show();
                                try
                                {
                                    UserModel user = RuntimeObject.CurrentUser;
                                    client.balance = SyncHelperBLL.Getyck(user.bookID, client.clntcode,user.xls);
                                }
                                catch (Exception)
                                {
                                    btnConfirm.Enabled = true;
                                    //MessagePopup.ShowError("获取会员的预存款失败,请检查网络！");
                                    MessagePopup.ShowInformation("网络异常,没有获取到会员预存款,可以继续开单！");
                                }
                                finally
                                {
                                    dlg.Close();
                                }
                            }
                            if (chkxpoints.Checked)
                            {
                                IntegralDeduction(client, speXpoints.Value);
                            }
                        }
                    }
                    if (client != null)
                    {
                        if (ckBtnDeposit.Checked)
                        {
                            if (client.balance < decimal.Parse(txtDeposit.Text.Trim()))
                            {
                                btnConfirm.Enabled = true;
                                MessagePopup.ShowInformation("当前会员预存款不足！");
                                return;
                            }
                        }
                    }

                    if (chkxpoints.Checked)
                    {
                        string[] array = chkxpoints.Tag.ToString().Split(',');
                        decimal jjie2 = clientBLL.GetJjie2(client.clntcode);
                        if (decimal.Parse(array[0]) > jjie2)
                        {
                            btnConfirm.Enabled = true;
                            MessagePopup.ShowInformation("抵扣积分不能大于该用户的积分结余,请退出该界面重新进入！");
                            return;
                        }
                    }

                    if (ckBtnMobilePayment.Checked)
                    {
                        decimal mobile = 0;
                        if (decimal.TryParse(txtMobile.Text, out mobile))
                        {
                            if (mobile > 0)
                            {
                                FormAlipay frm = new FormAlipay(mobile);
                                if (frm.ShowDialog() == DialogResult.OK)
                                {
                                    alipayResult = frm.AlipayResult;
                                }
                                else
                                {
                                    MessagePopup.ShowError("移动支付失败,请重试！");
                                    btnConfirm.Enabled = true;
                                    return;
                                }
                            }
                        }
                    }
                    poshh.xnote = metRemark.Text.Trim();
                    decimal pay = decimal.Parse(txtTotal.Text);
                    //poshh.xpay = pay;
                    poshh.xpay = CalcPay();
                    if (fastBilling)
                    {
                        poshh.xheallp = decimal.Parse(txtTotal.Text);
                        poshh.xhezhe = 0;
                    }
                    //else
                    //{
                    //    //if (prePoshh != null)
                    //    //{
                    //    //    //换货整单付款金额
                    //    //    decimal total = CalcMoneyHelper.Add(txtTotal.Text, prePoshh.xpay);
                    //    //    poshh.xhezhe = CalcMoneyHelper.Subtract(poshh.xheallp, total);
                    //    //}
                    //    //else
                    //    //{
                    //        //poshh.xhezhe = CalcMoneyHelper.Subtract(poshh.xheallp, pay);
                    //    //}
                    //}
                    poshh.xhenojie = 0;
                    //poshh.xrpay = difference;
                    //是否赠送积分
                    if (chkIsSendjf.Checked)
                    {
                        poshh.xsendjf = SsendJF(poshh);
                    }
                    //重算价税
                    foreach (var item in poshh.Posbbs)
                    {
                        item.xtax = CalcMoneyHelper.Multiply(item.xallp, item.xtaxr);
                        item.xallpt = CalcMoneyHelper.Add(item.xallp, item.xtax);
                        item.xprict = CalcMoneyHelper.Divide(item.xallpt, item.unitquat);
                    }
                    if (!fastBilling)
                    {
                        decimal banlce = CalcMoneyHelper.Subtract(totalAmount, pay);
                        poshh.xrpay = banlce;
                        poshh.xhezhe = CalcMoneyHelper.Subtract(poshh.xheallp, (pay + banlce));
                        //舍弃金额
                        // banlce = CalcMoneyHelper.Subtract(banlce, difference);
                        //if (banlce > 0)
                        //{
                        //    //分摊整单折扣
                        //    decimal avg = CalcMoneyHelper.Divide(banlce, poshh.Posbbs.Sum(r => r.xquat));
                        //    foreach (var item in poshh.Posbbs)
                        //    {
                        //        decimal money = CalcMoneyHelper.Multiply(item.xquat, avg);
                        //        item.xallp = CalcMoneyHelper.Subtract(item.xallp, money);
                        //        item.xpric = CalcMoneyHelper.Divide(item.xallp, item.xquat);
                        //        item.xzhe = CalcMoneyHelper.CalcZhe(item.xpric, item.xpricold);
                        //    }
                        //}
                    }
                    Dictionary<string, string> payTypeDic = EnumHelper.GetEnumDictionary(typeof(PayType));
                    List<BillpaytModel> billpaytList = new List<Model.BillpaytModel>();
                    if (ckBtnCash.Checked)
                    {
                        if (decimal.Parse(txtCash.Text) != 0)
                        {
                            BillpaytModel entity = new BillpaytModel();
                            entity.paytcode = string.Empty;
                            entity.paytname = payTypeDic[Enum.GetName(typeof(PayType), PayType.Cash)];
                            entity.xreceipt = decimal.Parse(txtCash.Text);
                            if (decimal.Parse(lblBalance.Text) > 0)
                            {
                                entity.xpay = CalcMoneyHelper.Subtract(txtCash.Text, lblBalance.Text);
                            }
                            else
                            {
                                entity.xpay = decimal.Parse(txtCash.Text);
                            }
                            entity.billflag = "pos";
                            billpaytList.Add(entity);
                        }
                    }
                    if (ckBtnDeposit.Checked)
                    {
                        if (decimal.Parse(txtDeposit.Text) != 0)
                        {
                            BillpaytModel entity = new BillpaytModel();
                            entity.paytcode = string.Empty;
                            entity.paytname = payTypeDic[Enum.GetName(typeof(PayType), PayType.Deposit)];
                            entity.xpay = decimal.Parse(txtDeposit.Text);
                            entity.billflag = "pos";
                            billpaytList.Add(entity);
                        }
                    }
                    if (ckBtnMobilePayment.Checked)
                    {
                        if (alipayResult != null)
                        {
                            poshh.transno = alipayResult.out_trade_no;
                            if (decimal.Parse(txtMobile.Text) != 0)
                            {
                                BillpaytModel entity = new BillpaytModel();
                                entity.paytcode = string.Empty;
                                entity.paytname = alipayResult.way;
                                entity.xpay = decimal.Parse(txtMobile.Text);
                                entity.billflag = "pos";
                                billpaytList.Add(entity);
                            }
                        }
                    }
                    if (ckBtnUnionpayCard.Checked)
                    {
                        if (decimal.Parse(txtUnionpayCard.Text) != 0)
                        {
                            if (ckBtnUnionpayCard.Text == "银联卡")
                            {
                                btnConfirm.Enabled = true;
                                MessagePopup.ShowInformation("请选择银联卡账户！");
                                return;
                            }
                            else
                            {
                                BillpaytModel entity = new BillpaytModel();
                                entity.paytcode = string.Empty;
                                entity.paytname = ckBtnUnionpayCard.Text.Trim();
                                entity.xnote1 = payTypeDic[Enum.GetName(typeof(PayType), PayType.UnionpayCard)];
                                entity.xpay = decimal.Parse(txtUnionpayCard.Text);
                                entity.billflag = "pos";
                                billpaytList.Add(entity);
                            }
                        }
                        // poshh.paytype = payTypeDic[Enum.GetName(typeof(PayType), PayType.UnionpayCard)];
                    }

                    if (txtCouponCode.Tag != null && txtCouponCode.Tag.ToString() != string.Empty)
                    {
                        BillpaytModel entity = new BillpaytModel();
                        entity.paytcode = string.Empty;
                        entity.paytname = payTypeDic[Enum.GetName(typeof(PayType), PayType.Coupon)];
                        entity.xpay = decimal.Parse(lblCouponMoney.Text) <= decimal.Parse(txtTotal.Text.Trim())
                            ? decimal.Parse(lblCouponMoney.Text) : decimal.Parse(txtTotal.Text.Trim());
                        entity.xnote1 = (txtCouponCode.Tag as TickoffmxModel).xcode;
                        entity.billflag = "pos";
                        billpaytList.Add(entity);
                    }

                    if (chkxpoints.Checked)
                    {
                        if (chkxpoints.Tag != null && chkxpoints.Tag.ToString() != string.Empty)
                        {
                            string[] array = chkxpoints.Tag.ToString().Split(',');
                            BillpaytModel entity = new BillpaytModel();
                            entity.paytcode = string.Empty;
                            entity.paytname = payTypeDic[Enum.GetName(typeof(PayType), PayType.Deposit)];
                            entity.xpay = decimal.Parse(array[1]);
                            entity.xnote1 = "积分兑换";
                            entity.xnote2 = array[0];
                            entity.billflag = "pos";
                            billpaytList.Add(entity);

                            poshh.xpoints = decimal.Parse(array[0]);
                            poshh.deductiblecash = decimal.Parse(array[1]);
                        }
                    }

                    if (ckBtnCheck.Checked)
                    {
                        if (decimal.Parse(txtCheck.Text) != 0)
                        {
                            BillpaytModel entity = new BillpaytModel();
                            entity.paytcode = string.Empty;
                            entity.paytname = payTypeDic[Enum.GetName(typeof(PayType), PayType.Check)];
                            entity.xpay = decimal.Parse(txtCheck.Text);
                            entity.billflag = "pos";
                            billpaytList.Add(entity);
                        }
                    }

                    poshh.payts = billpaytList;
                    poshh.isClntDay = poshh.Posbbs.Where(r => r.xtimes.HasValue).Count() > 0;
                    //if (prePoshh != null)
                    //{
                    //    if (prePoshh.payts != null)
                    //    {
                    //        poshh.payts.AddRange(prePoshh.payts);
                    //    }
                    //}
                    decimal balance = decimal.Parse(lblBalance.Text);
                    if (balance < 0)
                    {
                        poshh.xhenojie = Math.Abs(balance);
                    }
                    try
                    {
                        bool result = false;

                        if (fastBilling == false)
                        {
                            if (prePoshh != null)
                            {
                                //换货
                                string billno = string.Empty;
                                //付款
                                //decimal total = CalcMoneyHelper.Add(txtTotal.Text, prePoshh.xpay);
                                //poshh.xpay = total;
                                CloseForm(poshh);
                                result = posBLL.ChangeGoods(poshh, prePoshh.ID, out billno);
                                poshh.billno = billno;
                            }
                            else
                            {
                                if (poshh.ID == Guid.Empty)
                                {
                                    string billno = string.Empty;
                                    CloseForm(poshh);
                                    //付款
                                    result = posBLL.AddPOS(poshh, out billno);
                                    poshh.billno = billno;
                                }
                                else
                                {
                                    CloseForm(poshh);
                                    //取单付款
                                    result = posBLL.PendingReceipt(poshh);
                                }
                            }
                        }
                        else
                        {
                            //快速开单
                            string billno = string.Empty;
                            result = posBLL.FastBilling(poshh, out billno);
                            poshh.billno = billno;
                        }
                        poshh.xintime = DateTime.Now.ToString();
                        //实时同步单据抵扣预存款和积分、库存
                        SyncPOS(preForm);
                        try
                        {
                            PossettingModel entity = possettingBLL.GetPossettingByKey(AppConst.IsPrintBill);
                            if (entity != null)
                            {
                                if (bool.Parse(entity.xpvalue))
                                {
                                    poshh.xpho = client == null ? string.Empty : client.xpho;
                                    if (GetPermission(Functions.Print, false))
                                    {
                                        PrintHelper.Print(poshh);
                                    }
                                }
                            }
                            else
                            {
                                //PrintHelper.Print(poshh);
                            }
                        }
                        catch (Exception ex)
                        {
                            MessagePopup.ShowError(string.Format("打印小票失败！{0}", ex.Message));
                        }
                        //finally
                        //{
                        //    Clear();
                        //    this.DialogResult = DialogResult.OK;
                        //}
                    }
                    catch (Exception ex)
                    {
                        MessagePopup.ShowError(string.Format("未知错误,请重新开单！{0}", ex.Message));
                    }
                    finally
                    {
                        Clear();
                        this.DialogResult = DialogResult.OK;
                        this.Close();
                    }
                }
            }
        }
        #endregion

        #region 验证是否通过
        private bool Checked()
        {
            if (string.IsNullOrEmpty(txtTotal.Text.Trim()))
            {
                MessagePopup.ShowInformation("请填写总金额！");
                return false;
            }
            if (decimal.Parse(txtTotal.Text.Trim()) <= 0)
            {
                if (prePoshh == null)
                {
                    MessagePopup.ShowInformation("总金额必须大于零！");
                    return false;
                }
            }
            if (ckBtnCash.Checked)
            {
                if (string.IsNullOrEmpty(txtCash.Text.Trim()))
                {
                    MessagePopup.ShowInformation("请填写收款现金金额！");
                    return false;
                }
            }
            if (ckBtnDeposit.Checked)
            {
                if (string.IsNullOrEmpty(txtDeposit.Text.Trim()))
                {
                    MessagePopup.ShowInformation("请填写预存款！");
                    return false;
                }
                if (client == null)
                {
                    MessagePopup.ShowInformation("选择了预存款支付，请选择会员！");
                    return false;
                }
                if (decimal.Parse(txtDeposit.Text)>decimal.Parse(txtTotal.Text.Trim()))
                {
                    MessagePopup.ShowInformation("抵扣预存款的金额不能大于整单的金额！");
                    return false;
                }
            }
            if (ckBtnMobilePayment.Checked || ckBtnUnionpayCard.Checked)
            {
                if (string.IsNullOrEmpty(txtMobile.Text.Trim()))
                {
                    MessagePopup.ShowInformation("请填写移动支付金额！");
                    return false;
                }
            }
            if (ckBtnMobilePayment.Checked)
            {
                Dictionary<object, object> machineIDs = GetPayCodeMachineIDs();
                if (machineIDs != null)
                {
                    var query = machineIDs.Where(r => r.Key.ToString() == RuntimeObject.CurrentUser.xls).FirstOrDefault();
                    if (query.Value == null || string.IsNullOrEmpty(query.Value.ToString()))
                    {
                        MessagePopup.ShowInformation("请先设置贝壳设备号！");
                        return false;
                    }
                }
                else
                {
                    MessagePopup.ShowInformation("请先设置贝壳设备号！");
                    return false;
                }
            }
            decimal balance = 0;
            if (decimal.TryParse(lblBalance.Text, out balance))
            {
                if (balance < 0)
                {
                    if (client == null)
                    {
                        MessagePopup.ShowInformation("找零金额不能小于零，如需挂账请选择会员！");
                        return false;
                    }
                    else
                    {
                        decimal deposit = 0;
                        if (decimal.TryParse(txtDeposit.Text, out deposit))
                            if (deposit < client.balance)
                            {
                                MessagePopup.ShowInformation("该会员有预存款，请使用预存款抵扣！");
                                return false;
                            }
                    }
                }
            }
            else
            {
                MessagePopup.ShowInformation("请填写正确的金额！");
                return false;
            }
            return true;
        }
        #endregion

        #region 付款方式
        private void ckBtnCash_CheckedChanged(object sender, EventArgs e)
        {
            SetPaymentType(ckBtnCash);
            if (!ckBtnCash.Checked)
            {
                txtCash.EditValue = 0;
            }
            else
            {
                if (!isInputAmount)
                {
                    decimal total = CalcMoneyHelper.Add(CalcMoneyHelper.Add(txtMobile.EditValue, txtDeposit.EditValue), txtCheck.EditValue);
                    total = CalcMoneyHelper.Add(total, txtUnionpayCard.Text);
                    decimal balance = CalcMoneyHelper.Subtract(txtTotal.EditValue, total);
                    balance = CalcMoneyHelper.Subtract(balance, lblCouponMoney.Text);
                    balance = CalcMoneyHelper.Subtract(balance, GetDeductible());
                    if (balance > decimal.Parse(lblCouponMoney.Text))
                    {
                        txtCash.EditValue = CalcMoneyHelper.Subtract(balance, lblCouponMoney.Text);
                    }
                    else
                    {
                        txtCash.EditValue = 0;
                    }
                }
                this.ActiveControl = txtCash;
                txtCash.SelectAll();
            }

            CalcMobileMoney();
            CalcUnionpayCardMoney();
            CalcDepositMoney();
            CalcCheckMoney();
            CalcBalance();

            txtCash.ReadOnly = !ckBtnCash.Checked;
        }
        private void ckBtnDeposit_CheckedChanged(object sender, EventArgs e)
        {
            SetPaymentType(ckBtnDeposit);
            if (!ckBtnDeposit.Checked)
            {
                txtDeposit.EditValue = 0;
            }
            else
            {
                if (client == null)
                {
                    FormClientQuery frm = new FormClientQuery(null, string.Empty);
                    if (frm.ShowDialog() == DialogResult.OK)
                    {
                        client = frm.currentClient;
                        if (client.balance < 0)
                        {
                            MessagePopup.ShowInformation("该会员预存款小于零,不能使用预存款支付！");
                            ckBtnDeposit.Checked = false;
                            return;
                        }
                        poshh.clntcode = client.clntcode;
                        poshh.clntname = client.clntname;
                        IntegralDeduction(client, null);
                        decimal total = 0;
                        if (decimal.TryParse(txtTotal.Text.Trim(), out total))
                        {
                            decimal balance = CalcMoneyHelper.Add(client.balance, GetDeductible());
                            if (balance > total)
                            {
                                balance = total;
                                balance = CalcMoneyHelper.Subtract(balance, GetDeductible());
                                txtDeposit.EditValue = balance;
                            }
                            else
                            {
                                txtDeposit.EditValue = client.balance;
                            }
                        }

                    }
                }
                else
                {
                    if (client.balance < 0)
                    {
                        MessagePopup.ShowInformation("该会员预存款小于零,不能使用预存款支付！");
                        txtDeposit.EditValue = 0;
                        ckBtnDeposit.Checked = false;
                        return;
                    }
                    decimal total = 0;
                    if (decimal.TryParse(txtTotal.Text.Trim(), out total))
                    {
                        decimal balance = CalcMoneyHelper.Add(client.balance, GetDeductible());
                        if (balance > total)
                        {
                            balance = total;
                            balance = CalcMoneyHelper.Subtract(balance, GetDeductible());
                            txtDeposit.EditValue = balance;
                        }
                        else
                        {
                            txtDeposit.EditValue = client.balance;
                        }
                    }
                }
                this.ActiveControl = txtDeposit;
                txtDeposit.SelectAll();
            }
            CalcMobileMoney();
            CalcUnionpayCardMoney();
            CalcCash();
            CalcCheckMoney();
            CalcBalance();
            txtDeposit.ReadOnly = !ckBtnDeposit.Checked;
        }
        private void ckBtnMobilePayment_CheckedChanged(object sender, EventArgs e)
        {
            //if (ckBtnMobilePayment.Checked)
            //{
            //    ckBtnUnionpayCard.Checked = false;
            //}

            SetPaymentType(ckBtnMobilePayment);
            if (!ckBtnMobilePayment.Checked)
            {
                txtMobile.EditValue = 0;
            }
            else
            {
                if (!isInputAmount)
                {
                    txtCash.EditValue = 0;
                }
                decimal total = CalcMoneyHelper.Add(CalcMoneyHelper.Add(txtCash.EditValue, txtDeposit.EditValue), txtCheck.EditValue);
                total = CalcMoneyHelper.Add(total, txtUnionpayCard.Text);
                decimal balance = CalcMoneyHelper.Subtract(txtTotal.EditValue, total);
                balance = CalcMoneyHelper.Subtract(balance, lblCouponMoney.Text);
                balance = CalcMoneyHelper.Subtract(balance, GetDeductible());
                if (balance > decimal.Parse(lblCouponMoney.Text))
                {
                    txtMobile.EditValue = CalcMoneyHelper.Subtract(balance, lblCouponMoney.Text);
                }
                else
                {
                    txtMobile.EditValue = 0;
                }

                this.ActiveControl = txtMobile;
                txtMobile.SelectAll();
            }

            CalcDepositMoney();
            CalcUnionpayCardMoney();
            CalcCash();
            CalcCheckMoney();
            CalcBalance();

            txtMobile.ReadOnly = !ckBtnMobilePayment.Checked;
            //btnPaymentType.Text = ckBtnMobilePayment.Text;
        }
        private void ckBtnUnionpayCard_CheckedChanged(object sender, EventArgs e)
        {
            //if (ckBtnUnionpayCard.Checked)
            //{
            //    ckBtnMobilePayment.Checked = false;
            //}
            //else
            //{
            //  if (ckBtnCash.Checked == true
            //|| ckBtnDeposit.Checked == true
            //|| ckBtnMobilePayment.Checked == true)
            //  {
            //      ckBtnUnionpayCard.Text = "银联卡";
            //  }
            // }
            SetPaymentType(ckBtnUnionpayCard);
            if (!ckBtnUnionpayCard.Checked)
            {
                txtUnionpayCard.EditValue = 0;
            }
            else
            {
                if (!isInputAmount)
                {
                    txtCash.EditValue = 0;
                }
                decimal total = CalcMoneyHelper.Add(CalcMoneyHelper.Add(txtCash.EditValue, txtDeposit.EditValue), txtCheck.EditValue);
                total = CalcMoneyHelper.Add(total, txtMobile.Text);
                decimal balance = CalcMoneyHelper.Subtract(txtTotal.EditValue, total);
                balance = CalcMoneyHelper.Subtract(balance, lblCouponMoney.Text);
                balance = CalcMoneyHelper.Subtract(balance, GetDeductible());
                if (balance > decimal.Parse(lblCouponMoney.Text))
                {
                    txtUnionpayCard.EditValue = CalcMoneyHelper.Subtract(balance, lblCouponMoney.Text);
                }
                else
                {
                    txtUnionpayCard.EditValue = 0;
                }

                this.ActiveControl = txtUnionpayCard;
                txtUnionpayCard.SelectAll();
            }
            CalcDepositMoney();
            CalcMobileMoney();
            CalcCash();
            CalcCheckMoney();
            CalcBalance();
            txtUnionpayCard.ReadOnly = !ckBtnUnionpayCard.Checked;
            // btnPaymentType.Text = ckBtnUnionpayCard.Text;
        }
        private void ckBtnCheck_CheckedChanged(object sender, EventArgs e)
        {
            SetPaymentType(ckBtnCheck);

            if (!ckBtnCheck.Checked)
            {
                metRemark.Properties.NullValuePrompt = "单据备注";
                txtCheck.EditValue = 0;
            }
            else
            {
                metRemark.Properties.NullValuePrompt = "出票人账号";
                xtraTabControl1.SelectedTabPageIndex = 2;
                if (!isInputAmount)
                {
                    txtCash.EditValue = 0;
                }
                decimal total = CalcMoneyHelper.Add(CalcMoneyHelper.Add(txtCash.EditValue, txtDeposit.EditValue), txtMobile.EditValue);
                total = CalcMoneyHelper.Add(total, txtUnionpayCard.Text);
                decimal balance = CalcMoneyHelper.Subtract(txtTotal.EditValue, total);
                balance = CalcMoneyHelper.Subtract(balance, lblCouponMoney.Text);
                balance = CalcMoneyHelper.Subtract(balance, GetDeductible());
                if (balance > decimal.Parse(lblCouponMoney.Text))
                {
                    txtCheck.EditValue = CalcMoneyHelper.Subtract(balance, lblCouponMoney.Text);
                }
                else
                {
                    txtCheck.EditValue = 0;
                }
                this.ActiveControl = txtCheck;
                txtCheck.SelectAll();
            }
            CalcDepositMoney();
            CalcCash();
            CalcMobileMoney();
            CalcUnionpayCardMoney();
            CalcBalance();
            txtCheck.ReadOnly = !ckBtnCheck.Checked;

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
            if (ckBtnCash.Checked == false
               && ckBtnDeposit.Checked == false
               && ckBtnMobilePayment.Checked == false
               && ckBtnUnionpayCard.Checked == false
               && ckBtnCheck.Checked == false)
            {
                checkButton.Checked = true;
                if (checkButton.Text == "银联卡")
                {
                    ShowPaytMenu();
                }
                else
                {
                    MessagePopup.ShowInformation("必须选择一种支付方式！");
                }
            }
        }
        #endregion

        #region 计算现金金额（是否由总金额生成）
        /// <summary>
        /// 计算现金金额
        /// </summary>
        private void CalcCash()
        {
            if (isInputAmount)
            {
                decimal money;
                if (decimal.TryParse(txtCash.Text, out money) || txtCash.Text.Trim() == string.Empty)
                {
                    if (money == decimal.Zero)
                    {
                        Calc();
                    }
                }
            }
            else
            {
                Calc();
            }
        }

        private void Calc()
        {
            if (ckBtnCash.Checked)
            {
                decimal totalPay = CalcMoneyHelper.Add(CalcMoneyHelper.Add(txtDeposit.EditValue, txtMobile.EditValue), txtCheck.EditValue);
                totalPay = CalcMoneyHelper.Add(totalPay, txtUnionpayCard.Text);
                decimal balance = CalcMoneyHelper.Subtract(txtTotal.EditValue, lblCouponMoney.Text);
                balance = CalcMoneyHelper.Subtract(balance, GetDeductible());
                balance = CalcMoneyHelper.Subtract(balance, totalPay);
                if (balance > 0)
                {
                    txtCash.EditValue = balance;
                }
                else
                {
                    txtCash.EditValue = 0;
                }
            }
        }
        #endregion

        #region 计算余额
        private void CalcBalance()
        {
            decimal total = 0;
            if (txtTotal.Text == string.Empty || decimal.TryParse(txtTotal.Text, out total))
            {
                total = CalcMoneyHelper.Subtract(total, lblCouponMoney.Text);
                total = CalcMoneyHelper.Subtract(total, GetDeductible());
                decimal cash = 0;
                if (txtCash.Text.Trim() == string.Empty || decimal.TryParse(txtCash.Text, out cash))
                {
                    decimal deposit = 0;
                    if (txtDeposit.Text.Trim() == string.Empty || decimal.TryParse(txtDeposit.Text, out deposit))
                    {
                        decimal mobile = 0;
                        if (txtMobile.Text.Trim() == string.Empty || decimal.TryParse(txtMobile.Text, out mobile))
                        {
                            decimal unionpayCard = 0;
                            if (txtUnionpayCard.Text.Trim() == string.Empty || decimal.TryParse(txtUnionpayCard.Text, out unionpayCard))
                            {
                                //支票
                                decimal check = 0;
                                if (txtCheck.Text.Trim() == string.Empty || decimal.TryParse(txtCheck.Text, out check))
                                {
                                    decimal momey = CalcMoneyHelper.Add(CalcMoneyHelper.Add(CalcMoneyHelper.Add(cash, deposit), mobile), check);
                                    momey = CalcMoneyHelper.Add(momey, unionpayCard);
                                    decimal balance = CalcMoneyHelper.Subtract(momey, total);
                                    if (total > 0)
                                    {
                                        lblBalance.Text = balance.ToString();
                                    }
                                    else
                                    {
                                        lblBalance.Text = CalcMoneyHelper.Add(balance, total).ToString();
                                    }
                                }
                            }
                        }
                    }
                }
            }
            else
            {
                MessagePopup.ShowInformation("请填写正确的金额！");
            }
        }
        #endregion

        #region 计算移动支付金额
        private void CalcMobileMoney()
        {
            if (ckBtnMobilePayment.Checked)
            {
                decimal total = 0;
                if (txtTotal.Text.Trim() == string.Empty || decimal.TryParse(txtTotal.Text, out total))
                {
                    total = CalcMoneyHelper.Subtract(total, lblCouponMoney.Text);
                    total = CalcMoneyHelper.Subtract(total, GetDeductible());
                    decimal cash = 0;
                    if (txtCash.Text.Trim() == string.Empty || decimal.TryParse(txtCash.Text, out cash))
                    {
                        decimal deposit = 0;
                        if (txtDeposit.Text.Trim() == string.Empty || decimal.TryParse(txtDeposit.Text, out deposit))
                        {
                            decimal unionpayCard = 0;
                            if (txtUnionpayCard.Text.Trim() == string.Empty || decimal.TryParse(txtUnionpayCard.Text, out unionpayCard))
                            {
                                //支票
                                decimal check = 0;
                                if (txtCheck.Text.Trim() == string.Empty || decimal.TryParse(txtCheck.Text, out check))
                                {
                                    decimal momey = CalcMoneyHelper.Add(CalcMoneyHelper.Add(cash, deposit), check);
                                    momey = CalcMoneyHelper.Add(momey, unionpayCard);
                                    decimal balance = CalcMoneyHelper.Subtract(total, momey);
                                    if (balance > 0)
                                    {
                                        txtMobile.EditValue = balance;
                                    }
                                    else
                                    {
                                        txtMobile.EditValue = 0;
                                    }
                                }
                                else
                                {
                                    MessagePopup.ShowInformation("请填写正确的金额！");
                                    return;
                                }
                            }
                        }
                        else
                        {
                            MessagePopup.ShowInformation("请填写正确的金额！");
                            return;
                        }
                    }
                    else
                    {
                        MessagePopup.ShowInformation("请填写正确的金额！");
                        return;
                    }
                }
                else
                {
                    MessagePopup.ShowInformation("请填写正确的金额！");
                    return;
                }
            }
        }
        #endregion

        #region 计算银联卡金额
        private void CalcUnionpayCardMoney()
        {
            if (ckBtnUnionpayCard.Checked)
            {
                decimal total = 0;
                if (txtTotal.Text.Trim() == string.Empty || decimal.TryParse(txtTotal.Text, out total))
                {
                    total = CalcMoneyHelper.Subtract(total, lblCouponMoney.Text);
                    total = CalcMoneyHelper.Subtract(total, GetDeductible());
                    decimal cash = 0;
                    if (txtCash.Text.Trim() == string.Empty || decimal.TryParse(txtCash.Text, out cash))
                    {
                        decimal deposit = 0;
                        if (txtDeposit.Text.Trim() == string.Empty || decimal.TryParse(txtDeposit.Text, out deposit))
                        {
                            //移动支付
                            decimal mobile = 0;
                            if (txtMobile.Text.Trim() == string.Empty || decimal.TryParse(txtMobile.Text, out mobile))
                            {
                                //支票
                                decimal check = 0;
                                if (txtCheck.Text.Trim() == string.Empty || decimal.TryParse(txtCheck.Text, out check))
                                {
                                    decimal momey = CalcMoneyHelper.Add(CalcMoneyHelper.Add(cash, deposit), check);
                                    momey = CalcMoneyHelper.Add(momey, mobile);
                                    decimal balance = CalcMoneyHelper.Subtract(total, momey);
                                    if (balance > 0)
                                    {
                                        txtUnionpayCard.EditValue = balance;
                                    }
                                    else
                                    {
                                        txtUnionpayCard.EditValue = 0;
                                    }
                                }
                                else
                                {
                                    MessagePopup.ShowInformation("请填写正确的金额！");
                                    return;
                                }
                            }
                            else
                            {
                                MessagePopup.ShowInformation("请填写正确的金额！");
                                return;
                            }
                        }
                        else
                        {
                            MessagePopup.ShowInformation("请填写正确的金额！");
                            return;
                        }
                    }
                    else
                    {
                        MessagePopup.ShowInformation("请填写正确的金额！");
                        return;
                    }
                }
                else
                {
                    MessagePopup.ShowInformation("请填写正确的金额！");
                    return;
                }
            }
        }
        #endregion

        #region 计算预付款
        private void CalcDepositMoney()
        {
            if (ckBtnDeposit.Checked && client != null)
            {
                decimal total = 0;
                if (txtTotal.Text.Trim() == string.Empty || decimal.TryParse(txtTotal.Text, out total))
                {
                    total = CalcMoneyHelper.Subtract(total, lblCouponMoney.Text);
                    total = CalcMoneyHelper.Subtract(total, GetDeductible());
                    decimal cash = 0;
                    if (txtCash.Text.Trim() == string.Empty || decimal.TryParse(txtCash.Text, out cash))
                    {
                        decimal mobile = 0;
                        if (txtMobile.Text.Trim() == string.Empty || decimal.TryParse(txtMobile.Text, out mobile))
                        {
                            decimal unionpayCard = 0;
                            if (txtUnionpayCard.Text.Trim() == string.Empty || decimal.TryParse(txtUnionpayCard.Text, out unionpayCard))
                            {
                                //支票
                                decimal check = 0;
                                if (txtCheck.Text.Trim() == string.Empty || decimal.TryParse(txtCheck.Text, out check))
                                {
                                    decimal momey = CalcMoneyHelper.Add(CalcMoneyHelper.Add(cash, mobile), check);
                                    momey = CalcMoneyHelper.Add(momey, unionpayCard);
                                    decimal balance = CalcMoneyHelper.Subtract(total, momey);
                                    if (balance > 0)
                                    {
                                        if (balance > client.balance)
                                        {
                                            txtDeposit.EditValue = client.balance;
                                        }
                                        else
                                        {
                                            txtDeposit.EditValue = balance;
                                        }

                                    }
                                    else
                                    {
                                        txtDeposit.EditValue = 0;
                                    }
                                }
                            }
                            else
                            {
                                MessagePopup.ShowInformation("请填写正确的金额！");
                                return;
                            }
                        }
                        else
                        {
                            MessagePopup.ShowInformation("请填写正确的金额！");
                            return;
                        }
                    }
                    else
                    {
                        MessagePopup.ShowInformation("请填写正确的金额！");
                        return;
                    }
                }
                else
                {
                    MessagePopup.ShowInformation("请填写正确的金额！");
                    return;
                }
            }
        }
        #endregion

        #region 计算支票支付
        private void CalcCheckMoney()
        {
            if (ckBtnCheck.Checked)
            {
                decimal total = 0;
                if (txtTotal.Text.Trim() == string.Empty || decimal.TryParse(txtTotal.Text, out total))
                {
                    total = CalcMoneyHelper.Subtract(total, lblCouponMoney.Text);
                    total = CalcMoneyHelper.Subtract(total, GetDeductible());
                    decimal cash = 0;
                    if (txtCash.Text.Trim() == string.Empty || decimal.TryParse(txtCash.Text, out cash))
                    {
                        decimal mobile = 0;
                        if (txtMobile.Text.Trim() == string.Empty || decimal.TryParse(txtMobile.Text, out mobile))
                        {
                            decimal unionpayCard = 0;
                            if (txtUnionpayCard.Text.Trim() == string.Empty || decimal.TryParse(txtUnionpayCard.Text, out unionpayCard))
                            {
                                //预存款
                                decimal deposit = 0;
                                if (txtDeposit.Text.Trim() == string.Empty || decimal.TryParse(txtDeposit.Text, out deposit))
                                {
                                    decimal momey = CalcMoneyHelper.Add(CalcMoneyHelper.Add(cash, mobile), deposit);
                                    momey = CalcMoneyHelper.Add(momey, unionpayCard);
                                    decimal balance = CalcMoneyHelper.Subtract(total, momey);
                                    if (balance > 0)
                                    {
                                        txtCheck.EditValue = balance;
                                    }
                                    else
                                    {
                                        txtCheck.EditValue = 0;
                                    }
                                }
                            }
                            else
                            {
                                MessagePopup.ShowInformation("请填写正确的金额！");
                                return;
                            }
                        }
                        else
                        {
                            MessagePopup.ShowInformation("请填写正确的金额！");
                            return;
                        }
                    }
                    else
                    {
                        MessagePopup.ShowInformation("请填写正确的金额！");
                        return;
                    }
                }
                else
                {
                    MessagePopup.ShowInformation("请填写正确的金额！");
                    return;
                }
            }
        }
        #endregion

        #region 计算支付金额
        private decimal CalcPay()
        {
            decimal total = 0;
            total = CalcMoneyHelper.Add(total, lblCouponMoney.Text);
            total = CalcMoneyHelper.Add(total, GetDeductible());
            total = CalcMoneyHelper.Add(total, txtCash.Text.Trim());
            total = CalcMoneyHelper.Add(total, txtDeposit.Text.Trim());
            total = CalcMoneyHelper.Add(total, txtMobile.Text.Trim());
            total = CalcMoneyHelper.Add(total, txtUnionpayCard.Text.Trim());
            total = CalcMoneyHelper.Add(total, txtCheck.Text.Trim());
            total= CalcMoneyHelper.Subtract(total, lblBalance.Text.Trim());
            return total;
        }
        #endregion

        #region 单据总额发生改变
        private void txtTotal_EditValueChanged(object sender, EventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(txtTotal.Text.Trim()))
                {
                    if (client != null)
                    {
                        if (CheckConnect(false))
                        {
                            IntegralDeduction(client, null);
                        }
                    }
                }
                CalcCash();
                CalcMobileMoney();
                CalcUnionpayCardMoney();
                CalcDepositMoney();
                CalcBalance();

                SetCustomerDisplay(txtTotal.Text);
            }
            catch (Exception)
            {
            }

        }
        #endregion

        #region 金额发生改变
        private void txtTotal_KeyUp(object sender, KeyEventArgs e)
        {
            if (!string.IsNullOrEmpty(txtTotal.Text.Trim()))
            {
                if (txtTotal.Text.Last().ToString() != ".")
                {
                    decimal difference = 0;
                    txtTotal.Text = txtTotal.Text.Trim() == string.Empty ? "0" : CalcEraseAmount(decimal.Parse(txtTotal.Text), out difference).ToString();
                }
                txtTotal.SelectionStart = txtTotal.Text.Length;
            }
            else
            {
                txtTotal.EditValue = 0;
                txtTotal.SelectAll();
            }
        }

        private void txtDeposit_KeyUp(object sender, KeyEventArgs e)
        {
            if (string.IsNullOrEmpty(txtDeposit.Text.Trim()))
            {
                txtDeposit.EditValue = 0;
                txtDeposit.SelectAll();
            }
            CalcMobileMoney();
            CalcUnionpayCardMoney();
            CalcCash();
            CalcCheckMoney();
            CalcBalance();
        }

        private void txtMobile_KeyUp(object sender, KeyEventArgs e)
        {
            if (string.IsNullOrEmpty(txtMobile.Text.Trim()))
            {
                txtMobile.EditValue = 0;
                txtMobile.SelectAll();
            }
            CalcDepositMoney();
            CalcCash();
            CalcUnionpayCardMoney();
            CalcCheckMoney();
            CalcBalance();
        }
        private void txtUnionpayCard_KeyUp(object sender, KeyEventArgs e)
        {
            if (string.IsNullOrEmpty(txtUnionpayCard.Text.Trim()))
            {
                txtUnionpayCard.EditValue = 0;
                txtUnionpayCard.SelectAll();
            }
            CalcDepositMoney();
            CalcCash();
            CalcMobileMoney();
            CalcCheckMoney();
            CalcBalance();
        }

        private void txtCheck_KeyUp(object sender, KeyEventArgs e)
        {
            if (string.IsNullOrEmpty(txtCheck.Text.Trim()))
            {
                txtCheck.EditValue = 0;
                txtCheck.SelectAll();
            }
            CalcDepositMoney();
            CalcCash();
            CalcMobileMoney();
            CalcUnionpayCardMoney();
            CalcBalance();
        }
        #endregion

        #region 判断是否手动录入金额
        private void txtCash_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Space)
            {
                decimal money;
                if (decimal.TryParse(txtCash.Text.Trim(), out money))
                {
                    isInputAmount = true;
                }

                if (string.IsNullOrEmpty(txtCash.Text.Trim()))
                {
                    txtCash.EditValue = 0;
                    txtCash.SelectAll();
                }

                CalcMobileMoney();
                CalcUnionpayCardMoney();
                CalcDepositMoney();
                CalcCheckMoney();
                CalcBalance();
            }
        }
        #endregion

        private void txtCash_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            txtCash.EditValue = 0;
            CalcMobileMoney();
            CalcUnionpayCardMoney();
            CalcDepositMoney();
            CalcCheckMoney();
            CalcBalance();
        }

        #region 快捷方式
        private void FormReceipt_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Space)
            {
                if (btnConfirm.Enabled == true)
                {
                    btnConfirm_Click(null, null);
                }
            }
        }
        #endregion

        #region 移动支付
        private void ckBtnMobilePayment_Click(object sender, EventArgs e)
        {
            //txtCash.Text = txtTotal.Text;
            //decimal amount;
            //if (decimal.TryParse(txtCash.Text.Trim(), out amount))
            //{
            //    ckBtnMobilePayment.Checked = true;
            //    FormAlipay frm = new FormAlipay(amount);
            //    if (frm.ShowDialog() == DialogResult.OK)
            //    {
            //        alipayResult = frm.AlipayResult;
            //    }
            //}
        }
        #endregion

        #region 银联卡支付
        private void ckBtnUnionpayCard_Click(object sender, EventArgs e)
        {
            //txtCash.Text = txtTotal.Text;
            //decimal amount;
            //if (decimal.TryParse(txtCash.Text.Trim(), out amount))
            //{
            //    ckBtnUnionpayCard.Checked = true;
            //}
            if (ckBtnUnionpayCard.Checked)
            {
                popupMenu1.HidePopup();
            }
            else
            {
                ShowPaytMenu();
            }

        }
        private void ShowPaytMenu()
        {
            Point panelScreenPoint = groupControl1.PointToScreen(ckBtnUnionpayCard.Location);
            Point point = new Point(panelScreenPoint.X, panelScreenPoint.Y + ckBtnUnionpayCard.Height);
            popupMenu1.ShowPopup(point);
        }
        #endregion

        #region 清空
        private void Clear()
        {
            txtTotal.EditValue = null;
            txtCash.EditValue = null;
            txtDeposit.EditValue = null;
            txtMobile.EditValue = null;
            txtUnionpayCard.EditValue = null;
            btnConfirm.Enabled = true;
            txtCouponCode.Tag = null;
            chkxpoints.Tag = null;
            alipayResult = null;
            ckBtnUnionpayCard.Text = "银联卡";
            poshh = null;
        }
        #endregion

        #region 优惠券
        private void btnCancel_Coupon_Click(object sender, EventArgs e)
        {
            txtCouponCode.EditValue = null;
            lblCouponMoney.Text = "0";
            txtCouponCode.Tag = string.Empty;

            CalcMobileMoney();
            CalcDepositMoney();
            CalcCash();
            CalcBalance();
        }

        private void btnConfirm_Coupon_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtCouponCode.Text.Trim()))
            {
                TickoffmxModel tickoffmx = null;
                DevExpress.Utils.WaitDialogForm dlg = null;
                try
                {
                    dlg = new DevExpress.Utils.WaitDialogForm("正在查询优惠券，请稍后……", new Size(250, 100));
                    dlg.Show();
                    tickoffmx = tickoffBLL.GetTickoffmx(txtCouponCode.Text.Trim());

                    if (tickoffmx == null)
                    {
                        dlg.Close();
                        MessagePopup.ShowInformation("优惠券不存在！");
                    }
                    else
                    {
                        DateTime currentTime = DateTime.Now;
                        UserModel user = RuntimeObject.CurrentUser;
                        bool result = SyncHelperBLL.CheckConnect(out currentTime, user.bookID, user.username, user.password);
                        if (result)
                        {
                            lblMsg.Visible = false;
                            try
                            {
                                SyncData(new List<string> { "tickoffmx" }, 0, string.Empty, false);
                                Thread.Sleep(500);
                            }
                            catch (Exception ex)
                            {
                                dlg.Close();
                            }
                            if (!tickoffmx.xnotime)
                            {
                                //判断优惠券是否过期
                                DateTime startTime;
                                if (DateTime.TryParse(tickoffmx.xtime1, out startTime))
                                {
                                    if (startTime.Date > currentTime.Date)
                                    {
                                        dlg.Close();
                                        MessagePopup.ShowInformation("该优惠券没有在有效时间内使用！");
                                        return;
                                    }
                                }
                                else
                                {
                                    dlg.Close();
                                    MessagePopup.ShowInformation("该优惠券没有设置开始时间！");
                                }
                                DateTime endTime;
                                if (DateTime.TryParse(tickoffmx.xtime2, out endTime))
                                {
                                    if (endTime.Date < currentTime.Date)
                                    {
                                        dlg.Close();
                                        MessagePopup.ShowInformation("该优惠券已过期！");
                                        return;
                                    }
                                }
                                else
                                {
                                    dlg.Close();
                                    MessagePopup.ShowInformation("该优惠券没有设置结束时间！");
                                }
                            }
                        }
                        else
                        {
                            lblMsg.Visible = true;
                        }

                        Dictionary<string, string> couponStateDic = EnumHelper.GetEnumDictionary(typeof(CouponState));
                        if (tickoffmx.xstate == couponStateDic[Enum.GetName(typeof(CouponState), CouponState.Used)])
                        {
                            dlg.Close();
                            MessagePopup.ShowInformation("优惠券已使用！");
                        }
                        else
                        {
                            txtCouponCode.Tag = tickoffmx;
                            lblCouponMoney.Text = tickoffmx.xallp.ToString();


                            CalcMobileMoney();
                            CalcUnionpayCardMoney();
                            CalcDepositMoney();
                            CalcCash();
                            CalcBalance();
                        }
                    }
                }
                catch (Exception ex)
                {
                }
                finally
                {
                    dlg.Close();
                }
            }

        }

        private void txtCouponCode_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btnConfirm_Coupon_Click(null, null);
            }
        }

        #endregion

        #region  计算积分抵扣金额
        private void chkxpoints_CheckedChanged(object sender, EventArgs e)
        {
            if (chkxpoints.Checked)
            {
                IntegralDeduction(client, speXpoints.Value);
            }
            else
            {
                chkxpoints.Tag = null;
            }
            //CalcMobileMoney();
            //CalcUnionpayCardMoney();
            //CalcDepositMoney();
            //CalcCheckMoney();
            //CalcCash();
            //CalcBalance();
            ckBtnDeposit_CheckedChanged(null, null);
        }
        /// <summary>
        /// 获取积分抵扣多少钱
        /// </summary>
        /// <returns></returns>
        private decimal GetDeductible()
        {
            if (chkxpoints.Checked && chkxpoints.Tag != null && !string.IsNullOrEmpty(chkxpoints.Tag.ToString()))
            {
                string[] array = chkxpoints.Tag.ToString().Split(',');

                decimal deductible = 0;
                if (decimal.TryParse(array[1], out deductible))
                {
                    return deductible;
                }
                else
                {
                    return 0;
                }
            }
            else
            {
                return 0;
            }
        }
        #endregion

        #region 计算抹掉金额
        /// <summary>
        /// 计算抹掉金额
        /// </summary>
        /// <returns></returns>
        private decimal CalcEraseAmount(decimal amount, out decimal difference)
        {
            decimal money = 0;
            difference = 0;
            int decimalPlaces = AppConst.DecimalPlaces;
            PossettingModel possetting = possettingBLL.GetPossettingByKey(AppConst.SCALE_MODE);
            if (possetting != null && !string.IsNullOrEmpty(possetting.xpvalue))
            {
                int result = 0;
                if (int.TryParse(possetting.xpvalue, out result))
                {
                    if (result == AppConst.ScaleMode[0])
                    {
                        //不抹零
                        decimalPlaces = AppConst.DecimalPlaces;
                        money = CalcEraseAmountHelper.Calc(amount, decimalPlaces, true);
                    }
                    else if (result == AppConst.ScaleMode[1])
                    {
                        //四舍五入到角
                        decimalPlaces = 1;
                        money = CalcEraseAmountHelper.Calc(amount, decimalPlaces, true);
                    }
                    else if (result == AppConst.ScaleMode[2])
                    {
                        //四舍五入到元
                        decimalPlaces = 0;
                        money = CalcEraseAmountHelper.Calc(amount, decimalPlaces, true);
                    }
                    else if (result == AppConst.ScaleMode[3])
                    {
                        //抹掉零头分
                        decimalPlaces = 1;
                        money = CalcEraseAmountHelper.Calc(amount, decimalPlaces, false);
                    }
                    else if (result == AppConst.ScaleMode[4])
                    {
                        //抹掉零头角
                        decimalPlaces = 0;
                        money = CalcEraseAmountHelper.Calc(amount, decimalPlaces, false);
                    }
                    difference = amount - money;
                }
            }
            else
            {
                money = amount;
            }
            return money;
        }
        #endregion

        #region 客显
        private void SetCustomerDisplay(string dispiay)
        {
            CustomerDisplayHelper disliay;
            try
            {
                disliay = new CustomerDisplayHelper();
                disliay.DispiayType = CustomerDispiayType.Total;
                disliay.DisplayData(dispiay);

            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.ToString());
            }
        }
        #endregion

        #region 积分抵扣
        private void speXpoints_EditValueChanged(object sender, EventArgs e)
        {
            decimal xpoints = 0;
            if (decimal.TryParse(speXpoints.Value.ToString(), out xpoints))
            {
                if (rate != 0)
                {
                    //抵扣多少钱
                    decimal deductible = CalcMoneyHelper.Multiply(xpoints, rate);
                    decimal total = 0;
                    if (decimal.TryParse(txtTotal.Text.Trim(), out total))
                    {
                        if (total < deductible)
                        {
                            deductible = total;
                            xpoints = CalcMoneyHelper.Divide(deductible, rate);
                        }
                    }
                    chkxpoints.Tag = xpoints + "," + deductible;
                    chkxpoints.Text = string.Format("积分抵扣{0}元", deductible);
                    //poshh.deductiblecash = deductible;
                    if (chkxpoints.Checked)
                    {
                        CalcMobileMoney();
                        CalcDepositMoney();
                        CalcCash();
                        CalcBalance();
                    }
                }
            }
        }
        #endregion

        #region 赠送积分
        /// <summary>
        ///积分规则type : 1,//类型 0：不使用积分 1：按订单商品总额计算积分 2：为商品单独设置积分 3：按促销规则计算积分
        ///orderTick : 100,//订单金额每多少元赠送一个积分
        ///rate : 0.01,//1个积分可以兑换多少元预存款。
        ///minJfExch:100//最低兑换积分数
        /// </summary>
        /// <returns></returns>
        private int SsendJF(PoshhModel poshh)
        {
            //if (decimal.Parse(lblBalance.Text) < 0 || client == null)
            //{
            //    return 0;
            //}
            if (client == null)
            {
                return 0;
            }
            int jf = 0;
            PossettingModel possetting = possettingBLL.GetPossettingByKey(AppConst.INTEGRAL_RULES);
            if (possetting != null)
            {
                if (!string.IsNullOrEmpty(possetting.xpvalue))
                {
                    Dictionary<object, object> uclsspricsDic = js.Deserialize<Dictionary<object, object>>(possetting.xpvalue);
                    var query = uclsspricsDic.Where(r => r.Key.ToString() == "type").FirstOrDefault();
                    decimal value = 0;
                    if (decimal.TryParse(query.Value.ToString(), out value))
                    {
                        if (value == 1)
                        {
                            //按订单商品总额计算积分
                            query = uclsspricsDic.Where(r => r.Key.ToString() == "orderTick").FirstOrDefault();
                            if (decimal.TryParse(query.Value.ToString(), out value))
                            {
                                if (value != 0)
                                {
                                    decimal total = CalcMoneyHelper.Subtract(txtTotal.Text, GetDeductible());
                                    //if (prePoshh != null)
                                    //{
                                    //    total = CalcMoneyHelper.Add(total, prePoshh.xpay);
                                    //}
                                    List<PosbbModel> posbbs = poshh.Posbbs.Where(r => r.xtimes.HasValue).ToList();
                                    decimal xallp = posbbs.Sum(r => r.xallp);
                                    decimal times_xallp = posbbs.Sum(r => ((int)r.xallp) * r.xtimes.Value);
                                    jf = (int)CalcMoneyHelper.Divide((total - xallp + times_xallp), value);
                                }
                            }
                        }
                        else if (value == 2)
                        {
                            //为商品单独设置积分
                            jf = (int)poshh.Posbbs.Where(r => r.xsendjf.HasValue && !r.xtimes.HasValue).Sum(r => r.xsendjf * r.unitquat * r.unitrate)
                                + (int)poshh.Posbbs.Where(r => r.xsendjf.HasValue && r.xtimes.HasValue).Sum(r => r.xsendjf * r.unitquat * r.unitrate * r.xtimes);
                        }
                    }
                }
            }
            return jf;
        }
        #endregion

        #region 关闭当前窗口(特殊处理，防止重复开单，本地测试不出来)
        private void CloseForm(PoshhModel poshh)
        {
            if (poshh != null)
            {
                if (poshh.xstate == stateDic[Enum.GetName(typeof(PosState), PosState.Deal)])
                {
                    if (poshh.Posbbs == null || poshh.Posbbs.Count == 0)
                    {
                        this.DialogResult = DialogResult.OK;
                        return;
                    }
                }
            }
            else
            {
                this.DialogResult = DialogResult.OK;
                return;
            }
        }
        #endregion
    }
}
