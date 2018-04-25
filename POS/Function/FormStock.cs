using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Linq;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using POS.BLL;
using POS.DAL;
using POS.Model;
using System.IO;
using DevExpress.XtraGrid.Views.Grid.ViewInfo;
using POS.Helper;
using POS.Common;

namespace POS.Function
{
    public partial class FormStock : BaseForm
    {
        GoodBLL goodBLL = new GoodBLL();

        //表格布局文件路径
        string filePath_Stock = Path.Combine(Application.StartupPath, "StockGridlayout.xml");
        string filePath_Stock_default = Path.Combine(Application.StartupPath, "StockGridlayout_Default.xml");

        string filePath_StockDetail = Path.Combine(Application.StartupPath, "StockDetailGridlayout.xml");
        string filePath_StockDetail_default = Path.Combine(Application.StartupPath, "StockDetailGridlayout_Default.xml");

        public FormStock()
        {
            InitializeComponent();
            rluexquatku.DataSource = goodBLL.GetKu2();
            btnSearch_Click(null, null);
        }

        private void FormStock_Load(object sender, EventArgs e)
        {
            if (!File.Exists(filePath_Stock_default))
            {
                gv.SaveLayoutToXml(filePath_Stock_default);
            }
            if (!File.Exists(filePath_Stock))
            {
                gv.SaveLayoutToXml(filePath_Stock);
            }
            else
            {
                gv.RestoreLayoutFromXml(filePath_Stock);
            }

            if (!File.Exists(filePath_StockDetail_default))
            {
                gvDetail.SaveLayoutToXml(filePath_StockDetail_default);
            }
            if (!File.Exists(filePath_StockDetail))
            {
                gvDetail.SaveLayoutToXml(filePath_StockDetail);
            }
            else
            {
                gvDetail.RestoreLayoutFromXml(filePath_StockDetail);
            }
        }

        #region 查询

        private void btnSearch_Click(object sender, EventArgs e)
        {
            DevExpress.Utils.WaitDialogForm dlg = new DevExpress.Utils.WaitDialogForm("正在查询数据，请稍后……", new Size(250, 100));
            dlg.Show();

            try
            {
                List<GoodModel> data = goodBLL.GetGoodByKey(txtProduct.Text.Trim(), RuntimeObject.CurrentUser.xls).ToList();
                bdsLeft.DataSource = data.Where(r => r.unitname == r.goodunit);
            }
            catch (Exception)
            {
            }
            finally
            {
                dlg.Close();
            }
        }
        #endregion

        private void txtProduct_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btnSearch_Click(null, null);
            }

        }

        private void gv_FocusedRowObjectChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowObjectChangedEventArgs e)
        {
            GoodModel good = bdsLeft.Current as GoodModel;
            if (good != null)
            {
                bdsRight.DataSource = goodBLL.GetAllKu2(good);
            }
            else
            {
                bdsRight.DataSource = null;
            }
        }

        #region 显示右键
        private void gv_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                GridHitInfo gridInfo = gv.CalcHitInfo(e.Location);

                if (e.Button == MouseButtons.Right)
                {
                    if (gridInfo.InRow || gridInfo.HitTest == GridHitTest.EmptyRow)
                    {
                        contextMenuStrip1.Tag = "order";
                        contextMenuStrip1.Show(e.X, e.Y + panelControl1.Height);
                    }
                }
            }
        }

        private void gvDetail_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                GridHitInfo gridInfo = gvDetail.CalcHitInfo(e.Location);

                if (e.Button == MouseButtons.Right)
                {
                    if (gridInfo.InRow || gridInfo.HitTest == GridHitTest.EmptyRow)
                    {
                        contextMenuStrip1.Tag = "detail";
                        contextMenuStrip1.Show(e.X + splitContainerControl1.Panel1.Width, e.Y + panelControl1.Height);
                    }
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
                    gv.ShowCustomization();
                    gv.CustomizationForm.Text = "自定义列";
                }
                else if (contextMenuStrip1.Tag.ToString() == "detail")
                {
                    gv.HideCustomization();
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
                    gv.RestoreLayoutFromXml(filePath_Stock_default);
                }
                else if (contextMenuStrip1.Tag.ToString() == "detail")
                {
                    gvDetail.RestoreLayoutFromXml(filePath_StockDetail_default);
                }
            }
        }
        #endregion

        private void gv_CustomDrawRowIndicator(object sender, DevExpress.XtraGrid.Views.Grid.RowIndicatorCustomDrawEventArgs e)
        {
            if (e.Info.IsRowIndicator && e.RowHandle >= 0)
            {
                e.Info.DisplayText = (e.RowHandle + 1).ToString();
            }
        }

        private void gvDetail_CustomDrawRowIndicator(object sender, DevExpress.XtraGrid.Views.Grid.RowIndicatorCustomDrawEventArgs e)
        {
            if (e.Info.IsRowIndicator && e.RowHandle >= 0)
            {
                e.Info.DisplayText = (e.RowHandle + 1).ToString();
            }
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            ParentFrom = this;
            SyncData(new List<string> { "poshh", "ku2" },0, AppConst.Sync_Cashier_Msg,false);
            rluexquatku.DataSource = goodBLL.GetKu2();
            //if (CheckConnect())
            //{
            //    DevExpress.Utils.WaitDialogForm dlg = new DevExpress.Utils.WaitDialogForm("正在刷新数据，请稍后……", new Size(250, 100));
            //    dlg.Show();
            //    try
            //    {
            //        SyncHelperBLL.SyncData(RuntimeObject.CurrentUser.bookID, RuntimeObject.CurrentUser.xls, RuntimeObject.CurrentUser.usercode, new List<string> { "poshh", "ku2" });
            //        rluexquatku.DataSource = goodBLL.GetKu2();
            //    }
            //    catch (Exception ex)
            //    {
            //        dlg.Close();
            //        string msg = AppConst.Sync_Cashier_Msg;
            //        MessagePopup.ShowInformation(msg);
            //    }
            //    finally
            //    {
            //        dlg.Close();
            //    }
            //}
        }

        private void gv_CustomColumnSort(object sender, DevExpress.XtraGrid.Views.Base.CustomColumnSortEventArgs e)
        {
            if (e.Column == colxquatku)
            {
                string value1 = gv.GetRowCellDisplayText(e.ListSourceRowIndex1, colxquatku);
                string value2 = gv.GetRowCellDisplayText(e.ListSourceRowIndex2, colxquatku);
                int result = Comparer<int>.Default.Compare(ConvertToDecimal(value1, e.SortOrder), ConvertToDecimal(value2, e.SortOrder));
                e.Result = result;
                e.Handled = true;
            }
        }

        private int ConvertToDecimal(string input, DevExpress.Data.ColumnSortOrder sortOrder)
        {
            int result = 0;
            if (string.IsNullOrWhiteSpace(input))
                // result = sortOrder == DevExpress.Data.ColumnSortOrder.Ascending ? 9999 :-9999;
                result = 0;
            else
                int.TryParse(input, out result);
            return result;
        }

        #region 同步数据
        private void rpicRefresh_Click(object sender, EventArgs e)
        {
            if (gvDetail.FocusedColumn == colRefresh)
            {
                Ku2Model ku = bdsRight.Current as Ku2Model;
                if (ku != null)
                {
                    bool result = SyncData(new List<string> { "ku2" }, ku.SID.Value);
                    if (result)
                    {
                        rluexquatku.DataSource = goodBLL.GetKu2(RuntimeObject.CurrentUser.xls, RuntimeObject.CurrentUser.cnkucode);
                        gv_FocusedRowObjectChanged(null,null);
                    }
                }
            }
        }

        private void gvDetail_RowCellClick(object sender, DevExpress.XtraGrid.Views.Grid.RowCellClickEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                if (e.Column == colRefresh)
                {
                    rpicRefresh_Click(null, null);
                }
            }
        }
        #endregion

        private void gvDetail_CustomUnboundColumnData(object sender, DevExpress.XtraGrid.Views.Base.CustomColumnDataEventArgs e)
        {
            if (e.Column == colRefresh)
            {
                e.Value = DevExpress.Images.ImageResourceCache.Default.GetImage("images/actions/refresh_16x16.png");
            }
        }
    }
}