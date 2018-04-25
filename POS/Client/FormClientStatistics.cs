using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Linq;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using POS.Model;
using POS.BLL;
using POS.Helper;
using POS.Common.Enum;

namespace POS.Client
{
    public partial class FormClientStatistics : BaseForm
    {
        ClientBLL clientBLL = new ClientBLL();
        /// <summary>
        /// 当前选择的会员
        /// </summary>
        public ClntModel currentClient { get; set; }
        public FormClientStatistics()
        {
            InitializeComponent();
        }
        private void Search()
        {
            DevExpress.Utils.WaitDialogForm dlg = new DevExpress.Utils.WaitDialogForm("正在查询会员信息，请稍后……", new Size(250, 100));
            dlg.Show();
            try
            {
                List<ClntModel> clinets = clientBLL.GetClientStatistics(dteStart.DateTime, dteEnd.DateTime, RuntimeObject.CurrentUser.xls);
                bdsData.DataSource = clinets;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                dlg.Close();
            }

            lblTotalQuantity.Text = string.Format("{0}", bdsData.List.Count);
        }
        private void btnQuery_Click(object sender, EventArgs e)
        {
            Search();
        }

        private void btnExport_Click(object sender, EventArgs e)
        {
            if (!GetPermission(Functions.Export))
            {
                return;
            }
            if (bdsData.List.Count > 0)
            {
                System.Windows.Forms.SaveFileDialog saveFileDialog = new SaveFileDialog();
                saveFileDialog.Filter = "导出Excel (*.xls)|*.xls";
                saveFileDialog.FilterIndex = 0;
                saveFileDialog.RestoreDirectory = true;
                saveFileDialog.FileName = GetReportExportFileName("会员统计");
                saveFileDialog.Title = "导出文件";

                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    DevExpress.XtraPrinting.XlsExportOptionsEx opEx = new DevExpress.XtraPrinting.XlsExportOptionsEx();
                    opEx.ExportType = DevExpress.Export.ExportType.WYSIWYG;
                    gv.ExportToXls(saveFileDialog.FileName, opEx);
                }
            }
        }

        private void FormClientStatistics_Load(object sender, EventArgs e)
        {
            dteStart.DateTime = DateTime.Now.AddDays(1 - DateTime.Now.Day);
            dteEnd.DateTime = DateTime.Now.AddDays(1 - DateTime.Now.Day).AddMonths(1).AddDays(-1);
        }

        private void gv_CustomDrawRowIndicator(object sender, DevExpress.XtraGrid.Views.Grid.RowIndicatorCustomDrawEventArgs e)
        {
            if (e.Info.IsRowIndicator && e.RowHandle >= 0)
            {
                e.Info.DisplayText = (e.RowHandle + 1).ToString();
            }
        }
    }
}