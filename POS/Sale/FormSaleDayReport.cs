using POS.BLL.Report;
using POS.Common.Enum;
using POS.Common.utility;
using POS.Helper;
using POS.Model;
using POS.Shifts;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace POS.Sale
{
    public partial class FormSaleDayReport : BaseForm
    {
        SaleDayReportBLL saleDayReportBLL = new SaleDayReportBLL();

        Dictionary<string, string> payTypeDic = EnumHelper.GetEnumDictionary(typeof(PayType));

        PosModel entity = null;
        List<PosjhhModel> posjhhs = new List<PosjhhModel>();
        //日结支付明细
        List<BillpaytModel> billpayts = null;
        public FormSaleDayReport()
        {
            InitializeComponent();
            Init();
        }

        void Init()
        {
            dteStart.DateTime = DateTime.Now;
            dteEnd.DateTime = DateTime.Now;
            Query();
        }

        #region 导出
        private void btnExport_Click(object sender, EventArgs e)
        {
            if (bdsReport.List.Count > 0)
            {
                if (!GetPermission(Functions.Export))
                {
                    return;
                }
                System.Windows.Forms.SaveFileDialog saveFileDialog = new SaveFileDialog();
                saveFileDialog.Filter = "导出Excel (*.xls)|*.xls";
                saveFileDialog.FilterIndex = 0;
                saveFileDialog.RestoreDirectory = true;
                saveFileDialog.FileName = GetReportExportFileName("销售日结报表");
                saveFileDialog.Title = "导出文件";

                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    DevExpress.XtraPrinting.XlsExportOptionsEx opEx = new DevExpress.XtraPrinting.XlsExportOptionsEx();
                    opEx.ExportType = DevExpress.Export.ExportType.WYSIWYG;
                    gv.ExportToXls(saveFileDialog.FileName, opEx);
                }
            }
        }
        #endregion

        #region 统计
        private void btnQuery_Click(object sender, EventArgs e)
        {
            Query();
        }

        private void Query()
        {
            if (dteStart.DateTime > dteEnd.DateTime)
            {
                MessagePopup.ShowInformation("开始时间不能大于结束时间！");
                return;
            }
            //挂账
            decimal debts = 0;
            List<SaleReportModel> datas = saleDayReportBLL.GetSaleDayReport(dteStart.DateTime, dteEnd.DateTime, out debts);
            bdsReport.DataSource = datas;
            gv.UpdateSummary();
            //舍弃金额
            decimal xrpay = datas.Sum(r => r.xrpay);

            entity = new PosModel();
            entity.xls = RuntimeObject.CurrentUser.xls;
            entity.username = RuntimeObject.CurrentUser.username;
            entity.xintime = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");
            entity.TotalOrderMoney = datas.Sum(r => r.Total);
            billpayts = saleDayReportBLL.GetSaleDayBillpayt(dteStart.DateTime, dteEnd.DateTime);
            entity.cash = billpayts.Where(r => r.paytname == payTypeDic[Enum.GetName(typeof(PayType), PayType.Cash)]).Sum(r => r.xpay);
            entity.alipay = billpayts.Where(r => r.paytname == payTypeDic[Enum.GetName(typeof(PayType), PayType.Alipay)]).Sum(r => r.xpay);
            entity.wechat = billpayts.Where(r => r.paytname == payTypeDic[Enum.GetName(typeof(PayType), PayType.WeChat)]).Sum(r => r.xpay);
            entity.deposit = billpayts.Where(r => r.paytname == payTypeDic[Enum.GetName(typeof(PayType), PayType.Deposit)] && r.xnote1 != "积分兑换").Sum(r => r.xpay);
            entity.jfcash= billpayts.Where(r => r.paytname == payTypeDic[Enum.GetName(typeof(PayType), PayType.Deposit)] && r.xnote1 == "积分兑换").Sum(r => r.xpay);
            entity.coupon = billpayts.Where(r => r.paytname == payTypeDic[Enum.GetName(typeof(PayType), PayType.Coupon)]).Sum(r => r.xpay);
            entity.unionpaycard = billpayts.Where(r => r.xnote1 == payTypeDic[Enum.GetName(typeof(PayType), PayType.UnionpayCard)]).Sum(r => r.xpay);
            entity.check = billpayts.Where(r => r.paytname == payTypeDic[Enum.GetName(typeof(PayType), PayType.Check)]).Sum(r => r.xpay);
            entity.xrpay = xrpay;
            entity.TotalPaytMoney = (entity.cash + entity.alipay + entity.wechat + entity.deposit + xrpay + entity.coupon + entity.unionpaycard + debts + entity.check+ entity.jfcash);

            lblOrderTotal.Text = string.Format("{0}", entity.TotalOrderMoney);

            lblDebts.Text = debts.ToString();
            entity.debts = debts;
            lblcash.Text = entity.cash.ToString();
            lblalipay.Text = entity.alipay.ToString();
            lblWeChat.Text = entity.wechat.ToString();
            lbldeposit.Text = entity.deposit.ToString();
            lblCoupon.Text = entity.coupon.ToString();
            lblxrpay.Text = xrpay.ToString();
            lblUnionpayCard.Text = entity.unionpaycard.ToString();
            lblCheck.Text = entity.check.ToString();
            lblJFCash.Text = entity.jfcash.ToString();

            var details = (from p in datas
                           select new PosDetailModel
                           {
                               goodtype = p.goodtype,
                               xallp = p.Total
                           }).ToList();
            entity.Details = details;

            //回款
            posjhhs = GetClntRepayments(dteStart.DateTime.ToString("yyyy-MM-dd"), dteEnd.DateTime.AddDays(1).ToString("yyyy-MM-dd"));
            decimal xpay = posjhhs.Sum(r => r.xpay);
            entity.repayments = xpay;
            lblRepayments.Text = xpay.ToString();

            //lblTotalMoney.Text = (entity.TotalPaytMoney + xpay).ToString();
            lblTotalMoney.Text = (entity.TotalPaytMoney).ToString();
        }
        #endregion

        private void btnConfirm_Click(object sender, EventArgs e)
        {
            FormSaleDayMes frm = new FormSaleDayMes();
            DialogResult dialogResult = frm.ShowDialog();
            if (dialogResult == DialogResult.OK)
            {
                this.DialogResult = DialogResult.OK;
            }
            else
            {
                this.DialogResult = DialogResult.No;
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            if (!GetPermission(Functions.Print))
            {
                return;
            }
            PrintSaleDayReportHelper.Print(entity);
        }

        #region 打印营业款缴交凭证
        private void btnPrintVoucher_Click(object sender, EventArgs e)
        {
            if (!GetPermission(Functions.Print))
            {
                return;
            }
            PaymentVoucherModel voucher = new PaymentVoucherModel();
            voucher.xlsname = RuntimeObject.CurrentUser.xlsname;
            voucher.xdate = dteStart.DateTime.ToString("yyyy年MM月dd日");
            voucher.xheman = RuntimeObject.CurrentUser.username;
            voucher.xusername = RuntimeObject.CurrentUser.username;
            voucher.xtotalmoney = string.Format("￥{0}", lblTotalMoney.Text);
            voucher.xcapital = ConvertToChinese(Decimal.Parse(lblTotalMoney.Text));
            List<PaymentVoucherDetailModel> details = new List<PaymentVoucherDetailModel>();
            if (billpayts != null)
            {
                var query = (from p in billpayts
                             group p by new { p.paytname } into g
                             select new
                             {
                                 paytname = g.Key.paytname,
                                 count = g.Count(),
                                 xpay = g.Sum(r => r.xpay)
                             }).ToList();
                foreach (var item in query)
                {
                    PaymentVoucherDetailModel detail = new PaymentVoucherDetailModel();
                    if (query.IndexOf(item) == 0)
                    {
                        detail.xnote = "营业款";
                    }
                    detail.xsubject = item.paytname;
                    detail.xdsubject = item.count.ToString();
                    detail.xpay = item.xpay;
                    details.Add(detail);
                }
            }
            if (posjhhs != null && posjhhs.Count > 0)
            {
                PaymentVoucherDetailModel detail = new PaymentVoucherDetailModel();
                detail.xsubject = "回款";
                detail.xdsubject = posjhhs.Select(r=>r.billno).Distinct().Count().ToString();
                detail.xpay = posjhhs.Sum(r => r.xpay);
                List<BillpaytModel> billpayts = new List<BillpaytModel>();
                foreach (var item in posjhhs)
                {
                    billpayts.AddRange(item.payts);
                }

                StringBuilder str = new StringBuilder();
                foreach (var item in billpayts)
                {
                    str.AppendLine(item.paytname);
                    str.AppendLine(":");
                    str.AppendLine(item.xpay.ToString());
                }
                detail.remark = str.ToString();
                details.Add(detail);
            }
            voucher.Details = details;
            PrintPaymentVoucherHelper.Print(voucher);
        }
        #endregion
    }
}
