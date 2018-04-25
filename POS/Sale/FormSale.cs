using DevExpress.XtraEditors;
using DevExpress.XtraGrid.Views.Grid.ViewInfo;
using POS.Client;
using POS.Pending;
using POS.Shifts;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace POS.Sale
{
    public partial class FormSale : BaseForm
    {
        public FormSale()
        {
            InitializeComponent();
        }

        private void FormSale_Load(object sender, EventArgs e)
        {

        }

        #region 交接班
        private void blbtnShift_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            FormShift frm = new FormShift();
            frm.ShowDialog();
        }
        #endregion

        #region 新增会员
        private void blbtnClientAdd_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            FormClientAdd frm = new FormClientAdd();
            if (frm.ShowDialog() == DialogResult.OK)
            {

            }
        }
        #endregion

        #region 销售单据
        private void blbtnSaleQuery_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            FormSaleQuery frm = new FormSaleQuery();
            frm.ShowDialog();
        }
        #endregion

        #region 快捷键
        private void FormSale_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F9)
            {
                //查看会员
                FormClientQuery frm = new FormClientQuery();
                frm.ShowDialog();
            }
            else if (e.Control && e.KeyCode == Keys.G)
            {
                //挂单
                if (MessageBox.Show("确定要挂单？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {

                }
            }
            else if (e.Control && e.KeyCode == Keys.Q)
            {
                //取单
                FormPendingOrder frm = new FormPendingOrder();
                if (frm.ShowDialog() == DialogResult.OK)
                { }
            }
        }

        #endregion

        #region 搜索产品
        private void btnEtProduct_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                FormProductQuery frm = new FormProductQuery();
                if (frm.ShowDialog() == DialogResult.OK) { }
            }
        }

        #endregion

        #region 收款
        private void btnReceipt_Click(object sender, EventArgs e)
        {
            FormReceipt frm = new FormReceipt();
            frm.ShowDialog();
        }
        #endregion

        #region 显示右键
        private void gv_MouseUp(object sender, MouseEventArgs e)
        {
            GridHitInfo gridInfo = gv.CalcHitInfo(e.Location);

            if (gridInfo.InRow && e.Button == MouseButtons.Right)
            {
                contextMenuStrip1.Show(e.Location);
            }
        }
        #endregion

        #region 双击单元格查看明细
        private void gv_RowClick(object sender, DevExpress.XtraGrid.Views.Grid.RowClickEventArgs e)
        {
            if (e.Clicks ==2 && e.Button == MouseButtons.Left)
            {
                FormSaleDetail frm = new FormSaleDetail();
                if (frm.ShowDialog() == DialogResult.OK)
                {

                }
            }
        }
        #endregion
    }
}
