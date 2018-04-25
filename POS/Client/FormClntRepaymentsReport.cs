using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Linq;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using POS.BLL.Report;
using POS.Model;
using POS.Helper;
using System.IO;
using System.Configuration;
using POS.Common.utility;
using POS.Common.Enum;

namespace POS.Sale
{
    public partial class FormClntRepaymentsReport : BaseForm
    {
        SaleDetailReportBLL saleDetailReportBLL = new SaleDetailReportBLL();

        //表格布局文件路径
        string filePath = Path.Combine(Application.StartupPath, "ClntRepaymentsReportGridlayout.xml");
        string filePath_default = Path.Combine(Application.StartupPath, "ClntRepaymentsReportGridlayout_Default.xml");

        Uri baseUrl = new Uri(ConfigurationManager.AppSettings["serviceUrl"].ToString());
        public FormClntRepaymentsReport()
        {
            InitializeComponent();
            Init();
        }

        private void Init()
        {
            if (!File.Exists(filePath_default))
            {
                gvDetail.SaveLayoutToXml(filePath_default);
            }
            if (!File.Exists(filePath))
            {
                gvDetail.SaveLayoutToXml(filePath);
            }
            else
            {
                gvDetail.RestoreLayoutFromXml(filePath);
            }

            dteStart.DateTime = DateTime.Now;
            dteEnd.DateTime = DateTime.Now;
            btnQuery_Click(null, null);

        }

        #region 查询
        private void btnQuery_Click(object sender, EventArgs e)
        {
            if (dteStart.DateTime > dteEnd.DateTime)
            {
                MessagePopup.ShowInformation("开始时间不能大于结束时间！");
                return;
            }
            if (CheckConnect())
            {
                Query();
            }
            // bdsReport.DataSource = saleDetailReportBLL.Get(txtBillNO.Text.Trim(), txtProduct.Text.Trim(), txtClnt.Text.Trim(), dteStart.DateTime, dteEnd.DateTime, false);
        }

        private void Query()
        {
            DevExpress.Utils.WaitDialogForm dlg = null;
            List<PosjhhModel> posjhhs = new List<PosjhhModel>();
            Uri url = new Uri(baseUrl,
               string.Format("pos/pull?sid={0}&mod=posjhh&xls={1}&clntname={2}&sdate={3}&edate={4} "
               , RuntimeObject.CurrentUser.bookID, RuntimeObject.CurrentUser.xls, txtClnt.Text.Trim(), dteStart.DateTime.ToString("yyyy-MM-dd"), dteEnd.DateTime.AddDays(1).ToString("yyyy-MM-dd")));
            string queryString = url.Query;
            if (!string.IsNullOrEmpty(queryString))
            {
                url = new Uri(url.ToString() + "&sign=" + GlobalHelper.GetSign(queryString, GetSecretKey()));
            }
            try
            {
                dlg = new DevExpress.Utils.WaitDialogForm("正在查询数据，请稍后……", new Size(250, 100));
                dlg.Show();
                HttpClient.GetAasync<PosjhhModel>(url.ToString(), null, syncResult =>
                {
                    posjhhs = syncResult.datas;

                    List<PosjbbModel> posjbbs = new List<PosjbbModel>();

                    List<List<PosjbbModel>> posjbbList = posjhhs.Select(r => r.posjbbs).ToList();
                    foreach (var item in posjbbList)
                    {
                        posjbbs.AddRange(item);
                    }

                    if (!string.IsNullOrEmpty(txtPosBillNO.Text.Trim()))
                    {
                        List<int> subids = posjbbs.Where(r => r.billnob.Contains(txtPosBillNO.Text.Trim())).Select(r => r.xsubid).ToList();
                        posjbbs = (from p in posjbbs
                                  join b in subids on p.xsubid equals b
                                  select p).ToList();
                    }

                    var query = (from p in posjhhs
                                 join b in posjbbs on p.xtableid equals b.xsubid
                                 select new ClntRepaymentsReportModel
                                 {
                                     billno = p.billno,
                                     xintime = p.xintime,
                                     clntcode = p.clntcode,
                                     clntname = p.clntname,
                                     xpay = p.xpay,
                                     xls = p.xls,
                                     xlsname = p.xlsname,
                                     xnote = p.xnote,
                                     xpayp = p.xpay,
                                     billnob = b.billnob,
                                     xallp = b.xallp,
                                     xlast = b.xlast,
                                     xnowpay = b.xnowpay,
                                     xnowzhe = b.xnowzhe,
                                     xjie = b.xjie,
                                     xnoteb = b.xnoteb
                                 }).ToList();


                    if (!string.IsNullOrEmpty(txtBillNO.Text.Trim()))
                    {
                        query = query.Where(r => r.billno.Contains(txtBillNO.Text.Trim())).ToList();
                    }

                    if (InvokeRequired)
                    {
                        this.Invoke((MethodInvoker)delegate { SetData(query); });
                    }
                    else
                    {
                        SetData(query);
                    }


                });
            }
            catch (Exception ex)
            {
                MessagePopup.ShowError("获取挂账结算明细账记录失败！");
            }
            finally
            {
                dlg.Close();
            }
        }
        #endregion

        private void SetData(List<ClntRepaymentsReportModel> data)
        {
            if (data != null && data.Count() > 0)
            {
                bdsReport.DataSource = data;
                gvDetail.ViewCaption = " ";
            }
            else
            {
                bdsReport.DataSource = null;
                gvDetail.ViewCaption = "没有挂账结算记录";
            }
        }

        #region 合并单元格
        private void gvDetail_CellMerge(object sender, DevExpress.XtraGrid.Views.Grid.CellMergeEventArgs e)
        {
            if (e.Column == colbillno
                || e.Column == colxlsname
                || e.Column == colxintime
                || e.Column == colxpay
                || e.Column == colclntname
                || e.Column == colxnote
                )
            {
                ClntRepaymentsReportModel date1 = gvDetail.GetRow(e.RowHandle1) as ClntRepaymentsReportModel;
                ClntRepaymentsReportModel date2 = gvDetail.GetRow(e.RowHandle2) as ClntRepaymentsReportModel;
                if (date1 != null && date2 != null)
                {
                    if (date1.billno == date2.billno)
                    {
                        e.Merge = true;
                        e.Handled = true;
                    }
                    else
                    {
                        e.Merge = false;
                        e.Handled = true;
                    }
                }
            }
        }
        #endregion

        #region 导出Excel
        private void btnExport_Click(object sender, EventArgs e)
        {
            if (!GetPermission(Functions.Export))
            {
                return;
            }
            ExportToXls(gvDetail, "挂账结算明细账");
        }
        #endregion

        #region 退出
        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        #endregion

        private void gvDetail_RowStyle(object sender, DevExpress.XtraGrid.Views.Grid.RowStyleEventArgs e)
        {
            try
            {
                if (e.RowHandle >= 0)
                {
                    List<SaleDetailReportModel> dt = bdsReport.DataSource as List<SaleDetailReportModel>;
                    if (dt != null)
                    {
                        List<string> formCodeList = dt.Select(r => r.billno).Distinct().ToList();
                        SaleDetailReportModel current = gvDetail.GetRow(e.RowHandle) as SaleDetailReportModel;

                        int index = formCodeList.IndexOf(current.billno);

                        if (index % 2 == 0)
                        {
                            e.Appearance.BackColor = System.Drawing.Color.AliceBlue;
                        }
                    }
                }
            }
            catch (Exception)
            {
            }
        }

        #region 行号
        private void gvDetail_CustomDrawRowIndicator(object sender, DevExpress.XtraGrid.Views.Grid.RowIndicatorCustomDrawEventArgs e)
        {
            if (gvDetail.IsDataRow(e.RowHandle))
            {
                e.Info.DisplayText = (e.RowHandle + 1).ToString();
            }
        }
        #endregion

        #region 回车查询
        private void FormSaleDetailReport_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btnQuery_Click(null, null);
            }
        }
        #endregion

        #region 列设置
        private void tsmColumnsSetting_Click(object sender, EventArgs e)
        {
            gvDetail.ShowCustomization();
            gvDetail.CustomizationForm.Text = "自定义列";
        }
        #endregion

        #region 还原默认设置
        private void tsmRestore_Click(object sender, EventArgs e)
        {
            gvDetail.RestoreLayoutFromXml(filePath_default);
        }
        #endregion

        private void FormSaleDetailReport_FormClosing(object sender, FormClosingEventArgs e)
        {
            gvDetail.SaveLayoutToXml(filePath);
        }
    }
}