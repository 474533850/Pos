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
using POS.BLL;
using POS.Common.Enum;

namespace POS.Sale
{
    public partial class FormSaleDetailReport : BaseForm
    {
        SaleDetailReportBLL saleDetailReportBLL = new SaleDetailReportBLL();
        SaleBLL saleBLL = new SaleBLL();

        //表格布局文件路径
        string filePath = Path.Combine(Application.StartupPath, "SaleDetailReportGridlayout.xml");
        string filePath_default = Path.Combine(Application.StartupPath, "SaleDetailReportGridlayout_Default.xml");
        public FormSaleDetailReport()
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
            rlueSale.DataSource = saleBLL.GetAllSales();
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
            List<SaleDetailReportModel> data = saleDetailReportBLL.Get(txtBillNO.Text.Trim(), txtProduct.Text.Trim(), txtClnt.Text.Trim(), dteStart.DateTime, dteEnd.DateTime, false);
            bdsReport.DataSource = data;

            lblTotalQuantity.Text = data.Sum(r => r.xstate == "退货" ? 0 - r.xquat : r.xquat).ToString();
            var query = (from p in data select new { billno = p.billno, xstate = p.xstate, xpay = p.xpay , xpoints = p.xpoints, xsendjf =p.xsendjf }).ToList().Distinct();
            lblTotal.Text = string.Format("￥{0}", query.Sum(r => r.xstate == "退货" ? 0 - r.xpay : r.xpay));

            lblxpoints.Text = string.Format("{0}", data.Where(r=>r.xpointsb.HasValue).Sum(r=>r.xpointsb.Value)+query.Where(r=>r.xpoints.HasValue).Sum(r=>r.xpoints.Value));
            lblxsendjf.Text = string.Format("{0}", query.Sum(r => r.xsendjf));
        }
        #endregion

        #region 合并单元格
        private void gvDetail_CellMerge(object sender, DevExpress.XtraGrid.Views.Grid.CellMergeEventArgs e)
        {
            if (e.Column == colbillno
                || e.Column == colxlsname
                || e.Column == colxintime
                || e.Column == colxinname
                || e.Column == colclntname
                || e.Column == colxpay
                || e.Column == colxnote
                || e.Column == colxstate
                )
            {
                SaleDetailReportModel date1 = gvDetail.GetRow(e.RowHandle1) as SaleDetailReportModel;
                SaleDetailReportModel date2 = gvDetail.GetRow(e.RowHandle2) as SaleDetailReportModel;
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
            ExportToXls(gvDetail, "零售明细表");
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

        private void gvDetail_CustomColumnDisplayText(object sender, DevExpress.XtraGrid.Views.Base.CustomColumnDisplayTextEventArgs e)
        {
            //if (e.Column == colxallp || e.Column == colunitquat)
            //{
            //    SaleDetailReportModel current = gvDetail.GetRow(e.ListSourceRowIndex) as SaleDetailReportModel;
            //    if (current.xchg == "换进")
            //    {
            //        e.DisplayText = (0 - decimal.Parse(e.Value.ToString())).ToString();
            //    }
            //}
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
    }
}