using POS.BLL.Report;
using POS.Common.Enum;
using POS.Helper;
using POS.Model;
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
    public partial class FormSaleReport : BaseForm
    {
        SaleReportBLL saleReportBLL = new SaleReportBLL();
        public FormSaleReport()
        {
            InitializeComponent();
            Init();
        }

        void Init()
        {
            dteStart.Enabled = chkTime.Checked;
            dteEnd.Enabled = chkTime.Checked;
            chkTime.Text = "按班次统计";

            dteStart.DateTime = DateTime.Now;
            dteEnd.DateTime = DateTime.Now;

            btnQuery_Click(null,null);

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
                saveFileDialog.FileName = GetReportExportFileName("销售商品报表");
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

        private void btnQuery_Click(object sender, EventArgs e)
        {
            if (dteStart.DateTime > dteEnd.DateTime)
            {
                MessagePopup.ShowInformation("开始时间不能大于结束时间！");
                return;
            }
            string posnono = RuntimeObject.CurrentUser.posnono;
            if (chkTime.Checked)
            {
                posnono = string.Empty;
            }

            List<SaleReportModel> datas = saleReportBLL.GetSaleReport(posnono, dteStart.DateTime, dteEnd.DateTime);
            bdsReport.DataSource = datas;

            gv.UpdateSummary();
            lblTotalQuantity.Text = datas.Sum(r => r.Quantity).ToString();
            lblTotal.Text = string.Format("￥{0}", datas.Sum(r => r.Total));
        }

        private void chkTime_CheckedChanged(object sender, EventArgs e)
        {
            dteStart.Enabled = chkTime.Checked;
            dteEnd.Enabled = chkTime.Checked;
            chkTime.Text = chkTime.Checked? "按时间统计" : "按班次统计";
        }
    }
}
