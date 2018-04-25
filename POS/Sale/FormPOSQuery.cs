using DevExpress.XtraGrid.Views.Grid.ViewInfo;
using DevExpress.XtraPrinting;
using DevExpress.XtraPrinting.NativeBricks;
using DevExpress.XtraReports.UI;
using POS.BLL;
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
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web.Script.Serialization;
using System.Windows.Forms;
using DevExpress.XtraBars.Alerter;
using POS.WxPayAPI;
using Com.Alipay.Business;
using POS.Alipay;
using Com.Alipay.Model;

namespace POS.Sale
{
    public partial class FormPOSQuery : BaseForm
    {
        POSBLL posBLL = new POSBLL();
        ClientBLL clientBLL = new ClientBLL();
        SaleBLL saleBLL = new SaleBLL();
        PosBanBLL posBanBLL = new PosBanBLL();
        int pageSize = 10;
        int currentPate = 1;
        long totalPage = 0;
        //组合支付
        bool isComPayment = false;
        Dictionary<string, string> stateDic = EnumHelper.GetEnumDictionary(typeof(PosState));
        Dictionary<string, string> payTypeDic = EnumHelper.GetEnumDictionary(typeof(PayType));

        PossettingBLL possettingBLL = new PossettingBLL();

        JavaScriptSerializer js = new JavaScriptSerializer();

        //当前单据
        private PoshhModel currentPoshh;

        public PoshhModel CurrentPoshh { get { return currentPoshh; } }

        public OperationState CurrentOperationState { get; set; }

        //表格布局文件路径
        string filePath_Order = Path.Combine(Application.StartupPath, "PosGridlayout.xml");
        string filePath_Order_default = Path.Combine(Application.StartupPath, "PosGridlayout_Default.xml");

        string filePath_Detail = Path.Combine(Application.StartupPath, "PosDetailGridlayout.xml");
        string filePath_Detail_default = Path.Combine(Application.StartupPath, "PosDetailGridlayout_Default.xml");

        public FormPOSQuery()
        {
            InitializeComponent();
            CurrentOperationState = OperationState.None;
            panelBottom_SizeChanged(null, null);
            chkisAll.Checked = true;
            dteStart.DateTime = DateTime.Now;
            dteEnd.DateTime = DateTime.Now;

            Clear();
            rlueSale.DataSource = saleBLL.GetAllSales();

            Dictionary<int, string> uploadstatus = new Dictionary<int, string>
            {
                { -1, "全部" },
                { 0, "未上传" },
                { 1, "上传中" },
                { 2, "已上传" }
            };

            lueuploadstatus.Properties.DataSource = uploadstatus;
            lueuploadstatus.EditValue = -1;

            if (!File.Exists(filePath_Order_default))
            {
                gvPOS.SaveLayoutToXml(filePath_Order_default);
            }
            if (!File.Exists(filePath_Order))
            {
                gvPOS.SaveLayoutToXml(filePath_Order);
            }
            else
            {
                gvPOS.RestoreLayoutFromXml(filePath_Order);
            }

            if (!File.Exists(filePath_Detail_default))
            {
                gvDetail.SaveLayoutToXml(filePath_Detail_default);
            }
            if (!File.Exists(filePath_Detail))
            {
                gvDetail.SaveLayoutToXml(filePath_Detail);
            }
            else
            {
                gvDetail.RestoreLayoutFromXml(filePath_Detail);
            }

            pageSize = Properties.Settings.Default.PageSize;
            if (pageSize <= 0)
            {
                pageSize = 10;
            }
            cboPageSize.EditValue = pageSize;

            // Query();
            btnPage.Text = string.Format("{0}/{1}", currentPate, totalPage);
        }

        #region 分页按钮居中
        private void panelBottom_SizeChanged(object sender, EventArgs e)
        {
            this.panelPage.Left = (panelBottom.Width - panelPage.Width) / 2;
        }
        #endregion

        #region 搜索
        private void btnQuery_Click(object sender, EventArgs e)
        {
            Query();
        }

        private void Query()
        {
            int uploadstatus = int.Parse(lueuploadstatus.EditValue.ToString());
            bdsPos.DataSource = posBLL.GetPOS(currentPate, pageSize, txtKey.Text.Trim(), txtProduct.Text.Trim(), txtClnt.Text.Trim(), dteStart.DateTime, dteEnd.DateTime, chkisAll.Checked, uploadstatus, out totalPage);
            btnPage.Text = string.Format("{0}/{1}", currentPate, totalPage);
        }
        #endregion

        #region 全选日期
        private void chkisAll_CheckedChanged(object sender, EventArgs e)
        {
            dteStart.Enabled = !chkisAll.Checked;
            dteEnd.Enabled = !chkisAll.Checked;
        }
        #endregion

        #region 上一页
        private void btnPrevious_Click(object sender, EventArgs e)
        {
            if (currentPate > 1)
            {
                currentPate--;
                Query();
            }
        }
        #endregion

        #region 下一页
        private void btnNext_Click(object sender, EventArgs e)
        {
            if (currentPate < totalPage)
            {
                currentPate++;
                Query();
            }
        }
        #endregion

        #region 选择一张单据
        private void gvPOS_FocusedRowObjectChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowObjectChangedEventArgs e)
        {
            //组合支付
            isComPayment = false;
            PoshhModel current = bdsPos.Current as PoshhModel;
            if (current != null)
            {
                currentPoshh = posBLL.GetPOSByID(current.ID);
                lblquat.Text = currentPoshh.Posbbs.Sum(r => Math.Abs(r.unitquat)).ToString();
                lblTotalMoney.Text = currentPoshh.xpay.ToString();
                lblClientName.Text = currentPoshh.clntname;
                lblxrpay.Text = currentPoshh.xrpay.ToString();
                lblRemark.Text = currentPoshh.xnote;
                lblpbillno.Text = current.pbillno;
                //挂账
                //decimal debts = CalcMoneyHelper.Subtract(currentPoshh.xheallp, CalcMoneyHelper.Add(currentPoshh.xpay, currentPoshh.xhezhe));
                lblDebts.Text = currentPoshh.xhenojie.ToString();

                if (!string.IsNullOrEmpty(currentPoshh.clntcode))
                {
                    ClntModel client = clientBLL.GetClientByCode(currentPoshh.clntcode);

                    if (client != null)
                    {
                        lblxpho.Text = client.xpho;
                        lblAddress.Text = client.xadd;
                        currentPoshh.xpho = client.xpho;
                    }
                    else
                    {
                        lblxpho.Text = string.Empty;
                        lblAddress.Text = string.Empty;
                    }
                }
                else
                {
                    lblxpho.Text = string.Empty;
                    lblAddress.Text = string.Empty;
                }

                bdsDetail.DataSource = currentPoshh.Posbbs;
                var query = currentPoshh.Posbbs.Where(r => r.xpoints.HasValue).ToList();
                //消费积分
                if (currentPoshh.xpoints.HasValue)
                {
                    lblxpoints.Text = currentPoshh.xpoints.ToString();
                }
                else if (query.Count > 0)
                {
                    decimal? totalXpoints = query.Sum(r => r.xpoints);
                    if (totalXpoints.HasValue)
                    {
                        lblxpoints.Text = totalXpoints.Value.ToString();
                    }
                }
                else
                {
                    lblxpoints.Text = string.Empty;
                }
                lblxsendjf.Text = currentPoshh.xsendjf.HasValue ? currentPoshh.xsendjf.Value.ToString() : string.Empty;

                IEnumerable<BillpaytModel> pay = currentPoshh.payts.Where(r => r.paytname == payTypeDic[Enum.GetName(typeof(PayType), PayType.Coupon)]);
                if (pay != null && pay.Count() > 0)
                {
                    isComPayment = true;
                    lblCoupon.Text = pay.Sum(r => r.xpay).ToString();
                }
                else
                {
                    lblCoupon.Text = string.Empty;
                }
                pay = currentPoshh.payts.Where(r => r.paytname == payTypeDic[Enum.GetName(typeof(PayType), PayType.Cash)]);
                if (pay != null && pay.Count() > 0)
                {
                    lblcash.Text = pay.Sum(r => r.xpay).ToString();
                }
                else
                {
                    lblcash.Text = string.Empty;
                }
                pay = currentPoshh.payts.Where(r => r.paytname == payTypeDic[Enum.GetName(typeof(PayType), PayType.Deposit)] && r.xnote1 != "积分兑换");
                if (pay != null && pay.Count() > 0)
                {
                    isComPayment = true;
                    lbldeposit.Text = pay.Sum(r => r.xpay).ToString();
                }
                else
                {
                    lbldeposit.Text = string.Empty;
                }

                pay = currentPoshh.payts.Where(r => r.paytname == payTypeDic[Enum.GetName(typeof(PayType), PayType.Deposit)] && r.xnote1 == "积分兑换");
                if (pay != null && pay.Count() > 0)
                {
                    isComPayment = true;
                    lbldeductible.Text = pay.Sum(r => r.xpay).ToString();
                }
                else
                {
                    lbldeductible.Text = string.Empty;
                }
                pay = currentPoshh.payts.Where(r => r.paytname == payTypeDic[Enum.GetName(typeof(PayType), PayType.Check)]);
                if (pay != null && pay.Count() > 0)
                {
                    isComPayment = true;
                    lblCheck.Text = pay.Sum(r => r.xpay).ToString();
                }
                else
                {
                    lblCheck.Text = string.Empty;
                }
                lblalipay.Tag = null;
                pay = currentPoshh.payts.Where(r => r.paytname == payTypeDic[Enum.GetName(typeof(PayType), PayType.Alipay)]);
                if (pay != null && pay.Count() > 0)
                {
                    isComPayment = true;
                    lblalipay.Tag = string.Empty;
                    lblalipay.Text = pay.Sum(r => r.xpay).ToString();
                    lblpaytype.Text = payTypeDic[Enum.GetName(typeof(PayType), PayType.Alipay)] + ":";
                }

                pay = currentPoshh.payts.Where(r => r.paytname == payTypeDic[Enum.GetName(typeof(PayType), PayType.WeChat)]);
                if (pay != null && pay.Count() > 0)
                {
                    isComPayment = true;
                    lblalipay.Tag = string.Empty;
                    lblalipay.Text = pay.Sum(r => r.xpay).ToString();
                    lblpaytype.Text = payTypeDic[Enum.GetName(typeof(PayType), PayType.WeChat)] + ":";
                }

                pay = currentPoshh.payts.Where(r => r.xnote1 == payTypeDic[Enum.GetName(typeof(PayType), PayType.UnionpayCard)]);
                if (pay != null && pay.Count() > 0)
                {
                    isComPayment = true;
                    lblalipay.Tag = string.Empty;
                    lblalipay.Text = pay.Sum(r => r.xpay).ToString();
                    lblpaytype.Text = payTypeDic[Enum.GetName(typeof(PayType), PayType.UnionpayCard)] + ":";
                }

                //判断是否存在移动支付或者银联卡支付
                if (lblalipay.Tag == null)
                {
                    lblalipay.Text = string.Empty;
                }
                //判断是否为换货单
                PoshhModel pPoshh = posBLL.GetPOSByPNO(current.billno);
                if (current.xstate == stateDic[Enum.GetName(typeof(PosState), PosState.Invalid)])
                {
                    btnInvalid.Enabled = false;
                    btnReturned.Visible = false;
                    btnChange.Enabled = false;
                    btnAdd.Enabled = false;
                }
                else if (current.xstate == stateDic[Enum.GetName(typeof(PosState), PosState.Change)]
                     || (pPoshh != null
                     && pPoshh.xstate == stateDic[Enum.GetName(typeof(PosState), PosState.Change)]))
                {
                    btnChange.Enabled = false;
                    btnAdd.Enabled = false;
                    btnReturned.Enabled = false;
                    btnInvalid.Enabled = true;
                }
                else
                {
                    btnChange.Enabled = true;
                    //btnAdd.Enabled = true;
                    //btnReturned.Enabled = true;
                    btnInvalid.Enabled = true;
                    //退货的单据不能重新退货
                    if (current.xstate == stateDic[Enum.GetName(typeof(PosState), PosState.Returned)])
                    {
                        btnReturned.Visible = false;
                        btnChange.Enabled = false;
                        btnInvalid.Enabled = false;
                    }
                    else
                    {
                        btnReturned.Visible = true;
                    }

                    if (current.xstate == stateDic[Enum.GetName(typeof(PosState), PosState.Additional)])
                    {
                        btnAdd.Enabled = true;
                        btnChange.Enabled = false;
                        btnInvalid.Enabled = false;
                    }
                    else
                    {
                        btnAdd.Enabled = false;
                    }

                    btnReturned.Enabled = !IsReturned();
                    if (IsReturned())
                    {
                        btnChange.Enabled = false;
                    }
                }
            }
            else
            {
                Clear();
                btnInvalid.Enabled = true;
                btnReturned.Visible = true;
                btnChange.Enabled = true;
                btnAdd.Enabled = true;
            }
        }
        #endregion

        #region 清空明细
        private void Clear()
        {
            lblquat.Text = string.Empty; ;
            lblTotalMoney.Text = string.Empty;
            lblClientName.Text = string.Empty;
            lblxpho.Text = string.Empty;
            lblAddress.Text = string.Empty;
            lblxrpay.Text = string.Empty;
            lblCoupon.Text = string.Empty;
            lblalipay.Text = string.Empty;
            lblcash.Text = string.Empty;
            lbldeposit.Text = string.Empty;
            lblxpoints.Text = string.Empty;
            lblxsendjf.Text = string.Empty;
            lblCheck.Text = string.Empty;
            bdsDetail.DataSource = null;
            currentPoshh = null;
            lblpbillno.Text = string.Empty;
        }
        #endregion

        #region 退货
        private void btnReturned_Click(object sender, EventArgs e)
        {
            if (!GetPermission(Functions.Returns))
            {
                return;
            }
            PoshhModel current = bdsPos.Current as PoshhModel;
            if (current != null)
            {
                if (currentPoshh.isHistory)
                {
                    MessagePopup.ShowInformation("不能操作历史单据！");
                    return;
                }
                if (!IsReturned())
                {
                    List<PosbbModel> data = bdsDetail.DataSource as List<PosbbModel>;
                    var query = data.Where(r => r.xtquat.HasValue && r.xtquat != 0).ToList();
                    int jf = 0 - ReturnJF(query);
                    if (query.Count == 0)
                    {
                        MessagePopup.ShowInformation("退货数量不能为零！");
                    }
                    else
                    {
                        query = data.Where(r => r.xsalesid.HasValue).ToList();
                        if (query.Count > 0 || currentPoshh.isClntDay)
                        {
                            jf = currentPoshh.xsendjf.HasValue ? 0 - currentPoshh.xsendjf.Value : 0;
                            query = data.Where(r => r.xtquat.HasValue == false || r.unitquat != r.xtquat).ToList();
                            if (query.Count > 0)
                            {
                                MessagePopup.ShowInformation("促销商品的单据只能整单退货！");
                                return;
                            }
                        }

                        //组合支付只能反结或者整单退货
                        if (isComPayment)
                        {
                            jf = currentPoshh.xsendjf.HasValue ? 0 - currentPoshh.xsendjf.Value : 0;
                            query = data.Where(r => r.xtquat.HasValue == false || r.unitquat != r.xtquat).ToList();
                            if (query.Count > 0)
                            {
                                MessagePopup.ShowInformation("非现金支付的单据只能整单退货！");
                                return;
                            }
                        }

                        Dictionary<string, string> payTypeDic = EnumHelper.GetEnumDictionary(typeof(PayType));
                        List<BillpaytModel> billpaytList = new List<Model.BillpaytModel>();

                        FormReturnedMes frm = new FormReturnedMes();
                        if (frm.ShowDialog() == DialogResult.OK)
                        {
                            //确认返还积分
                            if (currentPoshh.clntcode != null && !string.IsNullOrEmpty(currentPoshh.clntcode))
                            {
                                FormReturnJF frmReturnJF = new FormReturnJF(0 - jf);
                                if (frmReturnJF.ShowDialog() != DialogResult.OK)
                                {
                                    return;
                                }
                                jf = 0 - frmReturnJF.JF;
                            }
                            decimal totalPay = 0;
                            PoshhModel poshh = SetPoshh(jf, out totalPay);
                            poshh.xnote = frm.Remark;

                            if (isComPayment)
                            {
                                foreach (var item in currentPoshh.payts)
                                {
                                    BillpaytModel entity = new BillpaytModel();
                                    entity.paytcode = string.Empty;
                                    entity.paytname = item.paytname;
                                    entity.xpay = item.xpay;
                                    entity.billflag = "pos";
                                    entity.xnote1 = item.xnote1;
                                    entity.xreceipt = item.xreceipt;
                                    billpaytList.Add(entity);
                                }
                                var billpayts = currentPoshh.payts.Where(r => r.paytname == payTypeDic[Enum.GetName(typeof(PayType), PayType.Deposit)]).ToList();
                                if (billpayts.Count > 0)
                                {
                                    if (!CheckConnect())
                                    {
                                        return;
                                    }
                                }
                            }
                            else
                            {
                                BillpaytModel entity = new BillpaytModel();
                                entity.paytcode = string.Empty;
                                entity.paytname = payTypeDic[Enum.GetName(typeof(PayType), PayType.Cash)];
                                entity.xpay = totalPay;
                                entity.billflag = "pos";
                                billpaytList.Add(entity);
                            }

                            poshh.payts = billpaytList;

                            DevExpress.Utils.WaitDialogForm dlg = new DevExpress.Utils.WaitDialogForm("正在处理，请稍后……", new Size(250, 100));
                            dlg.Show();

                            if (!string.IsNullOrEmpty(currentPoshh.transno))
                            {
                                if (!RefundMobile(currentPoshh))
                                {
                                    dlg.Close();
                                    return;
                                }
                            }
                            bool result = posBLL.Returned(poshh, current.ID);
                            dlg.Close();
                            if (result)
                            {
                                SyncData();
                                if (MessagePopup.ShowQuestion("退货完成,是否返回收银？") == DialogResult.Yes)
                                {
                                    this.DialogResult = DialogResult.Cancel;
                                }
                                else
                                {
                                    currentPoshh = null;
                                    System.Threading.Thread.Sleep(1000);
                                    Query();
                                }
                            }
                        }
                    }
                }
            }
            else
            {
                MessagePopup.ShowInformation("请选择一张单据！");
            }

        }
        #endregion

        #region 反结帐
        private void btnInvalid_Click(object sender, EventArgs e)
        {
            if (!GetPermission(Functions.Invalid))
            {
                return;
            }
            PoshhModel current = bdsPos.Current as PoshhModel;
            if (current != null)
            {
                if (currentPoshh.isHistory)
                {
                    MessagePopup.ShowInformation("不能操作历史单据！");
                    return;
                }
                if (currentPoshh.Posbbs != null && currentPoshh.Posbbs.Count > 0 && currentPoshh.Posbbs.FirstOrDefault().xsalestype == "积分兑换")
                {
                    MessagePopup.ShowInformation("积分兑换的商品不允许反结！");
                    return;
                }

                PosbanModel posban = posBanBLL.GetPosbanByNO(currentPoshh.posnono);
                if (posban.xjieok)
                {
                    MessagePopup.ShowInformation("已交接班的单据不允许反结,请使用退货流程！");
                }
                else if (IsReturned())
                {
                    MessagePopup.ShowInformation("已退款的单据不能整单退货！");
                }
                else
                {
                    if (isComPayment)
                    {
                        var billpayts = currentPoshh.payts.Where(r => r.paytname == payTypeDic[Enum.GetName(typeof(PayType), PayType.Deposit)]).ToList();
                        if (billpayts.Count > 0)
                        {
                            //用了预存款检查是否能访问服务端
                            if (!CheckConnect())
                            {
                                return;
                            }
                        }
                    }
                    FormInvalidMse frm = new FormInvalidMse();
                    if (frm.ShowDialog() == DialogResult.OK)
                    {
                        DevExpress.Utils.WaitDialogForm dlg = new DevExpress.Utils.WaitDialogForm("正在处理，请稍后……", new Size(250, 100));
                        dlg.Show();

                        if (!string.IsNullOrEmpty(currentPoshh.transno))
                        {
                            if (!RefundMobile(currentPoshh))
                            {
                                dlg.Close();
                                return;
                            }
                        }

                        bool result = false;
                        try
                        {
                            result = posBLL.Invalid(current.ID, frm.Remark);
                        }
                        catch (Exception ex)
                        {
                            throw ex;
                        }
                        finally
                        {
                            dlg.Close();
                        }

                        if (result)
                        {
                            SyncData();
                            MessagePopup.ShowInformation("反结账成功！");
                            if (frm.IsInvalidAndNew)
                            {
                                currentPoshh.ID = Guid.Empty;
                                this.DialogResult = DialogResult.OK;
                            }
                            else
                            {
                                System.Threading.Thread.Sleep(1000);
                                Query();
                            }
                        }
                        else
                        {
                            MessagePopup.ShowError("反结账失败！");
                        }
                    }
                }
            }
            else
            {
                MessagePopup.ShowInformation("请选择一张单据！");
            }
        }
        #endregion

        #region 打印
        private void btnPrint_Click(object sender, EventArgs e)
        {
            if (!GetPermission(Functions.RPrint))
            {
                return;
            }
            if (currentPoshh != null)
            {
                currentPoshh.xsendjf = 0;
                PrintHelper.Print(currentPoshh);
            }
            else
            {
                MessagePopup.ShowInformation("请选择一张单据！");
            }
        }
        #endregion

        #region 弹出退货窗
        private void gvDetail_RowClick(object sender, DevExpress.XtraGrid.Views.Grid.RowClickEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                PoshhModel poshh = bdsPos.Current as PoshhModel;
                if (poshh != null)
                {
                    if (poshh.xstate == stateDic[Enum.GetName(typeof(PosState), PosState.Deal)]
                        && string.IsNullOrEmpty(poshh.pbillno))
                    {
                        PoshhModel pPoshh = posBLL.GetPOSByPNO(poshh.billno);
                        if (IsReturned())
                        {
                            MessagePopup.ShowInformation("退货单据无法再退货！");
                            return;
                        }
                        if (pPoshh != null)
                        {
                            return;
                        }
                        List<PosbbModel> datas = bdsDetail.DataSource as List<PosbbModel>;

                        PosbbModel current = bdsDetail.Current as PosbbModel;
                        if (current.xsalestype == "积分兑换")
                        {
                            MessagePopup.ShowInformation("积分兑换的商品不允许退货！");
                            return;
                        }
                        IEnumerable<BillpaytModel> pay = currentPoshh.payts.Where(r => r.paytname == payTypeDic[Enum.GetName(typeof(PayType), PayType.Coupon)]);
                        if (pay != null && pay.Count() > 0)
                        {
                            MessagePopup.ShowInformation("优惠券抵扣的单据不能进行退货！");
                            return;
                        }
                        if (currentPoshh.xhenojie > 0)
                        {
                            MessagePopup.ShowInformation("挂账的单据不能进行退货！");
                            return;
                        }
                        if (current != null)
                        {
                            FormReturned frm = new FormReturned(current.unitquat);
                            if (frm.ShowDialog() == DialogResult.OK)
                            {
                                // current.unitquat = frm.Quantity;
                                current.xtquat = frm.Quantity;
                                current.xquat = frm.Quantity * current.unitrate.Value;
                                bdsDetail.ResetCurrentItem();
                            }
                        }
                    }
                }
            }
        }
        #endregion

        #region 判断是否存在退货数量（判断数据库）
        /// <summary>
        /// 判断是否存在退货数量
        /// </summary>
        /// <returns></returns>
        private bool IsReturned()
        {
            PoshhModel current = bdsPos.Current as PoshhModel;
            if (current != null)
            {
                PoshhModel entity = posBLL.GetPOSByID(current.ID);
                var query = entity.Posbbs.Where(r => r.xtquat.HasValue && r.xtquat != 0).Count();
                return query > 0;
            }
            else
            {
                return false;
            }
        }
        #endregion

        #region 赋值退货单
        /// <summary>
        /// 赋值退货单
        /// </summary>
        /// <param name="posState"></param>
        /// <returns></returns>
        private PoshhModel SetPoshh(int returnJF, out decimal totalPay)
        {
            Dictionary<string, string> stateDic = EnumHelper.GetEnumDictionary(typeof(PosState));

            List<PosbbModel> details = (bdsDetail.DataSource as List<PosbbModel>).Where(r => r.xtquat.HasValue && r.xtquat != 0).ToList();

            foreach (var item in details)
            {
                item.xallp = CalcMoneyHelper.Multiply(item.xpric, item.xtquat);
                item.xtax = CalcMoneyHelper.Multiply(item.xallp, item.xtaxr);
                item.xallpt = CalcMoneyHelper.Add(item.xallp, item.xtax);
                item.xprict = CalcMoneyHelper.Divide(item.xallpt, item.xtquat);
                item.unitquat = item.xtquat.Value;
            }

            decimal? totalMoney = details.Select(r => (r.xpricold * r.xtquat)).Sum();
            totalPay = details.Select(r => (r.xpric * r.xtquat.Value)).Sum();

            PoshhModel entity = new PoshhModel();
            //整单退货不退舍弃金额
            if (currentPoshh.xrpay != 0)
            {
                List<PosbbModel> data = bdsDetail.DataSource as List<PosbbModel>;
                var query = data.Where(r => r.xtquat.HasValue == false || r.unitquat != r.xtquat).ToList();
                if (query.Count == 0)
                {
                    entity.xrpay = currentPoshh.xrpay;
                    totalPay = totalPay - currentPoshh.xrpay;
                }
            }


            entity.posnono = RuntimeObject.CurrentUser.posnono;
            entity.xstate = stateDic[Enum.GetName(typeof(PosState), PosState.Returned)];
            entity.clntcode = currentPoshh.clntcode;
            entity.clntname = currentPoshh.clntname;
            entity.pbillno = currentPoshh.billno;
            entity.xnote = currentPoshh.xnote;
            entity.xls = RuntimeObject.CurrentUser.xls;
            entity.xlsname = RuntimeObject.CurrentUser.xlsname;
            entity.xinname = RuntimeObject.CurrentUser.username;
            //entity.xheallp = totalMoney.Value;
            entity.xpay = totalPay;
            entity.xheallp = entity.xpay;
            entity.xhezhe = 0;
            entity.xhenojie = 0;
            if (!string.IsNullOrEmpty(currentPoshh.clntcode))
            {
                entity.xsendjf = returnJF;
            }
            entity.Posbbs = details;
            return entity;
        }
        #endregion

        #region 导出
        private void btnExport_Click(object sender, EventArgs e)
        {
            if (!GetPermission(Functions.Export))
            {
                return;
            }
            if (bdsDetail.List.Count > 0)
            {
                System.Windows.Forms.SaveFileDialog saveFileDialog = new SaveFileDialog();
                saveFileDialog.Filter = "导出Excel (*.xls)|*.xls";
                saveFileDialog.FilterIndex = 0;
                saveFileDialog.RestoreDirectory = true;
                saveFileDialog.FileName = GetReportExportFileName("POS单");
                saveFileDialog.Title = "导出文件";

                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    DevExpress.XtraPrinting.XlsExportOptionsEx opEx = new DevExpress.XtraPrinting.XlsExportOptionsEx();
                    opEx.ExportType = DevExpress.Export.ExportType.WYSIWYG;
                    gvDetail.ExportToXls(saveFileDialog.FileName, opEx);
                }
            }
        }
        #endregion

        #region 退款移动支付
        private bool RefundMobile(PoshhModel poshh)
        {

            string deviceid = string.Empty;

            Dictionary<object, object> machineIDs = GetPayCodeMachineIDs();
            if (machineIDs != null)
            {
                var query = machineIDs.Where(r => r.Key.ToString() == RuntimeObject.CurrentUser.xls).FirstOrDefault();
                if (query.Value == null || string.IsNullOrEmpty(query.Value.ToString()))
                {
                    MessagePopup.ShowInformation("请先设置贝壳设备号！");
                    return false;
                }
                else
                {
                    deviceid = query.Value.ToString();
                }
            }
            else
            {
                MessagePopup.ShowInformation("请先设置贝壳设备号！");
                return false;
            }
            int paymentChannel = GetPaymentChannel();
            if (paymentChannel == 0)
            {
                try
                {
                    Uri url = AppConst.payUrl;
                    Refund refund = new Refund();
                    refund.deviceid = deviceid;
                    BillpaytModel pay = poshh.payts.Where(r => r.paytname == payTypeDic[Enum.GetName(typeof(PayType), PayType.Alipay)]).FirstOrDefault();
                    if (pay != null)
                    {
                        refund.total_amount = pay.xpay.ToString();
                        refund.refund_amount = pay.xpay.ToString();
                    }

                    pay = poshh.payts.Where(r => r.paytname == payTypeDic[Enum.GetName(typeof(PayType), PayType.WeChat)]).FirstOrDefault();
                    if (pay != null)
                    {
                        refund.total_amount = pay.xpay.ToString();
                        refund.refund_amount = pay.xpay.ToString();
                    }
                    refund.order = poshh.transno;

                    refund.service = "refund";

                    JavaScriptSerializer serializer = new JavaScriptSerializer();
                    string json = serializer.Serialize(refund);
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
                        if (alipayResult.result != "10000")
                        {
                            MessagePopup.ShowError("移动支付退款失败,请重试！");
                            return false;
                        }
                        else
                        {
                            return true;
                        }
                    }
                    else
                    {
                        MessagePopup.ShowError("移动支付退款失败,请重试！");
                    }
                    return false;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            else if (paymentChannel == 1)
            {
                //bool isWxPay = false;
                //原生通道
                BillpaytModel pay = poshh.payts.Where(r => r.paytname == payTypeDic[Enum.GetName(typeof(PayType), PayType.Alipay)]).FirstOrDefault();
                if (pay != null)
                {
                    AlipayF2FRefundResult refundResult = AlipayRefund.Run(poshh.transno, pay.xpay.ToString(), pay.xpay.ToString());
                    //请在这里加上商户的业务逻辑程序代码
                    switch (refundResult.Status)
                    {
                        case ResultEnum.SUCCESS:
                            return true;
                        case ResultEnum.FAILED:
                            MessagePopup.ShowError("移动支付退款失败,请重试！");
                            return false;
                        case ResultEnum.UNKNOWN:
                            MessagePopup.ShowError("移动支付退款失败,请重试！");
                            return false;
                    }
                    //return true;
                }

                pay = poshh.payts.Where(r => r.paytname == payTypeDic[Enum.GetName(typeof(PayType), PayType.WeChat)]).FirstOrDefault();
                if (pay != null)
                {
                    int amount = (int)(pay.xpay * 100);
                    try
                    {
                        WxPayData result = POS.WxPayAPI.Refund.Run(string.Empty, poshh.transno, amount.ToString(), amount.ToString());
                        if (result.GetValue("result_code") != null && result.GetValue("result_code").ToString() == "SUCCESS")
                        {
                            return true;
                        }
                        else
                        {
                            MessagePopup.ShowError("移动支付退款失败,请重试！");
                            return false;
                        }
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                }
                else
                    return false;

            }
            else
            {
                return false;
            }
        }
        #endregion

        #region 同步数据
        private void SyncData()
        {
            SyncPOS(this);
        }
        #endregion

        #region 显示右键
        private void gvPOS_MouseUp(object sender, MouseEventArgs e)
        {
            GridHitInfo gridInfo = gvPOS.CalcHitInfo(e.Location);

            if (e.Button == MouseButtons.Right)
            {
                if (gridInfo.InRow || gridInfo.HitTest == GridHitTest.EmptyRow)
                {
                    PoshhModel pos = bdsPos.Current as PoshhModel;
                    if (pos != null)
                    {
                        ;
                        if (pos.xstate != stateDic[Enum.GetName(typeof(PosState), PosState.Additional)])
                        {
                            if (pos.uploadstatus == "未上传"
                                || pos.uploadstatus == "上传中")
                            {
                                tsmuploadstatus.Enabled = true;
                            }
                            else
                            {
                                tsmuploadstatus.Enabled = false;
                            }
                        }
                        else
                        {
                            tsmuploadstatus.Enabled = false;
                        }

                    }
                    else
                    {
                        tsmuploadstatus.Enabled = false;
                    }
                    contextMenuStrip1.Tag = "order";
                    contextMenuStrip1.Show(e.X, e.Y + panelControl3.Height);
                }
            }
        }

        private void gvDetail_MouseUp(object sender, MouseEventArgs e)
        {
            GridHitInfo gridInfo = gvDetail.CalcHitInfo(e.Location);

            if (e.Button == MouseButtons.Right)
            {
                if (gridInfo.InRow || gridInfo.HitTest == GridHitTest.EmptyRow)
                {
                    contextMenuStrip1.Tag = "detail";
                    contextMenuStrip1.Show(e.X + splitContainerControl1.Panel1.Width, e.Y);
                }
            }
        }
        #endregion

        #region 列设置
        private void tsmColumnsSetting_Click(object sender, EventArgs e)
        {
            if (contextMenuStrip1.Tag != null)
            {
                if (contextMenuStrip1.Tag.ToString() == "order")
                {
                    gvDetail.HideCustomization();
                    gvPOS.ShowCustomization();
                    gvPOS.CustomizationForm.Text = "自定义列";
                }
                else if (contextMenuStrip1.Tag.ToString() == "detail")
                {
                    gvPOS.HideCustomization();
                    gvDetail.ShowCustomization();
                    gvDetail.CustomizationForm.Text = "自定义列";
                }
            }
        }
        #endregion

        #region 还原默认值
        private void tsmRestore_Click(object sender, EventArgs e)
        {
            if (contextMenuStrip1.Tag != null)
            {
                if (contextMenuStrip1.Tag.ToString() == "order")
                {
                    gvPOS.RestoreLayoutFromXml(filePath_Order_default);
                }
                else if (contextMenuStrip1.Tag.ToString() == "detail")
                {
                    gvDetail.RestoreLayoutFromXml(filePath_Detail_default);
                }
            }
        }
        #endregion

        #region 关闭
        private void FormPOSQuery_FormClosing(object sender, FormClosingEventArgs e)
        {
            gvPOS.SaveLayoutToXml(filePath_Order);
            gvDetail.SaveLayoutToXml(filePath_Detail);
        }
        #endregion

        #region 退出
        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        #endregion

        #region 自定义行号
        private void gvPOS_CustomDrawRowIndicator(object sender, DevExpress.XtraGrid.Views.Grid.RowIndicatorCustomDrawEventArgs e)
        {
            if (e.Info.IsRowIndicator)
            {
                e.Info.DisplayText = (e.RowHandle + 1).ToString();
            }
        }
        #endregion

        #region 追加商品
        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (!GetPermission(Functions.Cashier))
            {
                return;
            }
            PoshhModel current = bdsPos.Current as PoshhModel;
            if (current != null)
            {
                if (currentPoshh.isHistory)
                {
                    MessagePopup.ShowInformation("不能操作历史单据！");
                    return;
                }
                CurrentOperationState = OperationState.FastAppend;
                this.DialogResult = DialogResult.OK;
            }
            else
            {
                MessagePopup.ShowInformation("请选择一张单据！");
            }
        }
        #endregion

        #region 表格样式
        private void gvPOS_RowCellStyle(object sender, DevExpress.XtraGrid.Views.Grid.RowCellStyleEventArgs e)
        {
            if (e.Column == colState)
            {
                if (e.CellValue != null &&
                    e.CellValue.ToString() == stateDic[Enum.GetName(typeof(PosState), PosState.Additional)])
                {
                    e.Appearance.ForeColor = Color.Red;
                }
            }
            else if (e.Column == coluploadstatus)
            {
                if (e.CellValue != null &&
                   (e.CellValue.ToString() == "未上传"
                   || e.CellValue.ToString() == "上传中"
                   ))
                {
                    e.Appearance.ForeColor = Color.Red;
                }
            }
        }
        #endregion

        #region 换货
        private void btnChange_Click(object sender, EventArgs e)
        {
            if (!GetPermission(Functions.Exchange))
            {
                return;
            }
            PoshhModel current = bdsPos.Current as PoshhModel;
            if (current != null)
            {
                if (currentPoshh.isHistory)
                {
                    MessagePopup.ShowInformation("不能操作历史单据！");
                    return;
                }

                if (currentPoshh.Posbbs != null && currentPoshh.Posbbs.Count > 0 && currentPoshh.Posbbs.FirstOrDefault().xsalestype == "积分兑换")
                {
                    MessagePopup.ShowInformation("积分兑换的商品不能进行换货！");
                    return;
                }

                List<PosbbModel> data = bdsDetail.DataSource as List<PosbbModel>;
                var query = data.Where(r => r.xsalesid.HasValue).ToList();
                if (query.Count > 0 || currentPoshh.isClntDay)
                {
                    MessagePopup.ShowInformation("促销商品的单据不能进行换货！");
                    return;
                }
                IEnumerable<BillpaytModel> pay = currentPoshh.payts.Where(r => r.paytname == payTypeDic[Enum.GetName(typeof(PayType), PayType.Coupon)]);
                if (pay != null && pay.Count() > 0)
                {
                    MessagePopup.ShowInformation("优惠券抵扣的单据不能进行换货！");
                    return;
                }
                if (currentPoshh.xhenojie > 0)
                {
                    MessagePopup.ShowInformation("挂账的单据不能进行换货！");
                    return;
                }
                CurrentOperationState = OperationState.Change;
                this.DialogResult = DialogResult.OK;
            }
            else
            {
                MessagePopup.ShowInformation("请选择一张单据！");
            }
        }
        #endregion

        #region 退货返还积分
        /// <summary>
        ///积分规则type : 1,//类型 0：不使用积分 1：按订单商品总额计算积分 2：为商品单独设置积分 3：按促销规则计算积分
        ///orderTick : 100,//订单金额每多少元赠送一个积分
        ///rate : 0.01,//1个积分可以兑换多少元预存款。
        ///minJfExch:100//最低兑换积分数
        /// </summary>
        /// <returns></returns>
        private int ReturnJF(List<PosbbModel> posbbs)
        {
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
                                    decimal total = posbbs.Sum(r => r.xtquat.Value * r.xpric);
                                    jf = (int)CalcMoneyHelper.Divide(total, value);
                                }
                            }
                        }
                        else if (value == 2)
                        {
                            //为商品单独设置积分
                            jf = (int)posbbs.Where(r => r.xsendjf.HasValue).Sum(r => r.xsendjf * r.unitquat * r.unitrate);

                        }
                    }
                }
            }
            return jf;
        }
        #endregion

        #region 切换页码
        private void cboPageSize_TextChanged(object sender, EventArgs e)
        {
            int size = 0;
            if (int.TryParse(cboPageSize.Text.Trim(), out size))
            {
                if (size > 0)
                {
                    pageSize = size;
                    Properties.Settings.Default.PageSize = pageSize;
                    Query();
                }
            }
        }
        #endregion

        #region 切换单据上传状态
        private void lueuploadstatus_EditValueChanged(object sender, EventArgs e)
        {
            Query();
        }
        #endregion

        #region 上传单据
        private void tsmuploadstatus_Click(object sender, EventArgs e)
        {
            PoshhModel current = bdsPos.Current as PoshhModel;
            if (current != null)
            {
                bool result = SyncData(new List<string> { "poshh" }, 0, string.Empty, false, current.ID);
                if (result)
                {
                    Query();
                }
            }
            else
            {
                MessagePopup.ShowInformation("请选择一张单据！");
            }
        }

        private void tsmupload_Click(object sender, EventArgs e)
        {
            bool result = SyncData(new List<string> { "poshh" });
            if (result)
            {
                Query();
            }
        }
        #endregion

        #region 自定义显示
        private void gvDetail_CustomColumnDisplayText(object sender, DevExpress.XtraGrid.Views.Base.CustomColumnDisplayTextEventArgs e)
        {
            int dataSourceIndex = e.ListSourceRowIndex;
            object xchg = gvDetail.GetListSourceRowCellValue(dataSourceIndex, colxchg);
            if (xchg != null && xchg.ToString() == "换进")
            {
                if (e.Column == colunitquat || e.Column == colxallp)
                {
                    decimal value = 0;
                    if (decimal.TryParse(e.Value.ToString(), out value))
                    {
                        e.DisplayText = (0 - value).ToString();
                    }
                }
            }
            if (e.Column == colxsalesid)
            {
                object times = gvDetail.GetListSourceRowCellValue(e.ListSourceRowIndex, "xtimes");
                if (times != null)
                {
                    decimal value = 0;
                    if (decimal.TryParse(times.ToString(), out value))
                    {
                        if (value != 0)
                            e.DisplayText = "会员日";
                    }
                }
            }
        }
        #endregion

        #region 回车查询
        private void FormPOSQuery_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                Query();
            }
        }
        #endregion
    }
    public class Refund
    {
        public string deviceid { get; set; }

        public string service { get; set; }
        /// <summary>
        /// 商户订单号
        /// </summary>
        public string order { get; set; }
        /// <summary>
        /// 订单总金额
        /// </summary>
        public string total_amount { get; set; }
        /// <summary>
        /// 退款金额
        /// </summary>
        public string refund_amount { get; set; }
    }
}
