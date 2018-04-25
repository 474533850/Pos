using DevExpress.XtraEditors;
using DevExpress.XtraGrid.Views.Grid.ViewInfo;
using POS.BLL;
using POS.Common;
using POS.Helper;
using POS.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace POS.Sale
{
    public partial class FormProductQuery : BaseForm
    {
        GoodBLL goodBLL = new GoodBLL();

        private GoodModel currentGood;
        public GoodModel CurrentGood { get { return currentGood; } }

        //表格布局文件路径
        string filePath = Path.Combine(Application.StartupPath, "ProductGridlayout.xml");
        string filePath_default = Path.Combine(Application.StartupPath, "ProductGridlayout_Default.xml");
        //是否允许负库存开单
        bool NGKU_SALE = false;

        PossettingBLL possettingBLL = new PossettingBLL();

        List<Ku2Model> ku2s = new List<Ku2Model>();

        public FormProductQuery(string key)
        {
            InitializeComponent();
            this.ActiveControl = gd;
            this.gv.GotFocus += Gv_GotFocus;
            this.gv.LostFocus += Gv_LostFocus;

            if (!File.Exists(filePath_default))
            {
                gv.SaveLayoutToXml(filePath_default);
            }
            if (!File.Exists(filePath))
            {
                gv.SaveLayoutToXml(filePath);
            }
            else
            {
                gv.RestoreLayoutFromXml(filePath);
            }
            InitKu2();
            bteSearch.EditValue = key;

            PossettingModel entity = possettingBLL.GetPossettingByKey(AppConst.NGKU_SALE);
            if (entity != null)
            {
                int result = 0;
                if (int.TryParse(entity.xpvalue, out result))
                {
                    NGKU_SALE = result > 0;
                }
            }

        }

        #region 初始化库存
        /// <summary>
        /// 初始化库存
        /// </summary>
        private void InitKu2()
        {
            ku2s = goodBLL.GetKu2(RuntimeObject.CurrentUser.xls, RuntimeObject.CurrentUser.cnkucode);
            rluexquatku.DataSource = ku2s;
        }
        #endregion

        private void Gv_LostFocus(object sender, EventArgs e)
        {
            this.AcceptButton = null;
        }

        private void Gv_GotFocus(object sender, EventArgs e)
        {
            this.AcceptButton = btnJoin;
        }

        #region 清除搜索条件
        private void bteSearch_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            if (e.Button.Index == 0)
            {
                bteSearch.EditValue = null;
            }
        }
        #endregion

        #region 搜索货品

        private void btnQuery_Click(object sender, EventArgs e)
        {
            Search();
        }

        private void bteSearch_EditValueChanged(object sender, EventArgs e)
        {
            Search();
        }

        private void Search()
        {
            if (!string.IsNullOrEmpty(bteSearch.Text.Trim()))
            {
                List<GoodModel> goods = goodBLL.GetGoodByKey(bteSearch.Text.Trim(), RuntimeObject.CurrentUser.xls);
                if (chkShowZero.Checked)
                {
                    bds.DataSource = goods;
                }
                else
                {
                    List<string> keys = ku2s.Where(r => r.xquatku.HasValue && r.xquatku.Value != 0).Select(r => r.key).ToList();
                    bds.DataSource = (from p in goods join k in keys on p.key equals k select p).Distinct().ToList();
                }
            }
            else
            {
                bds.DataSource = null;
            }
        }
        #endregion

        #region 加入
        private void btnJoin_Click(object sender, EventArgs e)
        {
            Join();
        }

        private void gv_RowClick(object sender, DevExpress.XtraGrid.Views.Grid.RowClickEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                if (e.Clicks == 2)
                {
                    if (gv.FocusedColumn != colRefresh)
                    {
                        Join();
                    }
                }
            }
            else if (e.Button == MouseButtons.Right)
            {
                GridHitInfo gridInfo = gv.CalcHitInfo(e.Location);
                Point panelScreenPoint = gd.PointToScreen(e.Location);
                if (e.Button == MouseButtons.Right)
                {
                    if (gridInfo.InRow || gridInfo.HitTest == GridHitTest.EmptyRow)
                    {
                        contextMenuStrip1.Show(panelScreenPoint);
                    }
                }
            }
        }

        private void Join()
        {
            GoodModel good = bds.Current as GoodModel;
            if (good != null)
            {
                List<Ku2Model> ku2 = rluexquatku.DataSource as List<Ku2Model>;
                decimal xquatku = ku2.Where(r => r.key == good.key).Where(r => r.xquatku.HasValue).Sum(r => r.xquatku.Value);

                if (!NGKU_SALE)
                {
                    good.unitrate = 1;
                    if (good.unitname != null && good.unitname != good.goodunit)
                    {
                        //多单位
                        if (good.xmulunit != null)
                        {
                            List<string> mulunits = good.xmulunit.Split(',').ToList();
                            string mulunit = mulunits.Where(r => r.ToLower().Contains(good.unitname.ToLower())).FirstOrDefault();
                            if (!string.IsNullOrEmpty(mulunit))
                            {
                                string[] array = mulunit.Split('=');
                                good.unitrate = decimal.Parse(array[1]);
                            }
                        }
                    }

                    if (xquatku < good.unitrate)
                    {
                        MessagePopup.ShowInformation("不允许负库存开单！");
                        return;
                    }
                }
                currentGood = good;
                this.DialogResult = DialogResult.OK;
            }
        }
        #endregion

        #region 列设置
        private void tsmColumnsSetting_Click(object sender, EventArgs e)
        {
            gv.ShowCustomization();
            gv.CustomizationForm.Text = "自定义列";
        }
        #endregion

        #region 还原默认设置
        private void tsmRestore_Click(object sender, EventArgs e)
        {
            gv.RestoreLayoutFromXml(filePath_default);
        }
        #endregion

        #region 保存样式
        private void FormProductQuery_FormClosing(object sender, FormClosingEventArgs e)
        {
            gv.SaveLayoutToXml(filePath);
        }
        #endregion

        #region 自定义显示
        private void gv_CustomColumnDisplayText(object sender, DevExpress.XtraGrid.Views.Base.CustomColumnDisplayTextEventArgs e)
        {
            if (e.Column == colxquatku)
            {
                GoodModel good = gv.GetRow(e.ListSourceRowIndex) as GoodModel;
                if (good != null)
                {
                    if (good.unitname != good.goodunit)
                    {
                        e.DisplayText = string.Empty;
                    }
                }
            }
        }
        #endregion

        #region 自定义刷新图标
        private void gv_CustomUnboundColumnData(object sender, DevExpress.XtraGrid.Views.Base.CustomColumnDataEventArgs e)
        {
            if (e.Column == colRefresh)
            {
                e.Value = DevExpress.Images.ImageResourceCache.Default.GetImage("images/actions/refresh_16x16.png");
            }
        }
        #endregion

        #region 同步数据
        private void rpicRefresh_Click(object sender, EventArgs e)
        {
            if (gv.FocusedColumn == colRefresh)
            {
                GoodModel good = bds.Current as GoodModel;
                if (good != null)
                {
                    List<Ku2Model> ku2s = rluexquatku.DataSource as List<Ku2Model>;
                    Ku2Model ku = ku2s.Where(r => r.key == good.key).FirstOrDefault();
                    if (ku != null)
                    {
                        bool result = SyncData(new List<string> { "ku2" }, ku.SID.Value);
                        if (result)
                        {
                            InitKu2();
                        }
                    }
                    else
                    {
                        bool result = SyncData(new List<string> { "ku2" }, 0, string.Empty, true, null, good.goodcode);
                        if (result)
                        {
                            InitKu2();
                        }
                    }
                }
            }
        }

        private void gv_RowCellClick(object sender, DevExpress.XtraGrid.Views.Grid.RowCellClickEventArgs e)
        {
            rpicRefresh_Click(null, null);
        }
        #endregion

        #region 是否显示为零的库存
        private void chkShowZero_CheckedChanged(object sender, EventArgs e)
        {
            Search();
        }
        #endregion

        #region 方向键切换焦点
        private void FormProductQuery_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Down)
            {
                if (this.ActiveControl.Parent != null && this.ActiveControl.Parent.GetType() == typeof(DevExpress.XtraEditors.ButtonEdit))
                {
                    gv.Focus();
                }
            }
            else if (e.KeyCode == Keys.Up)
            {
                if (gv.FocusedRowHandle == 0)
                {
                    bteSearch.Focus();
                }
            }
        }
        #endregion
    }
}
