using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Linq;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using POS.Sale;

namespace POS.Function
{
    public partial class FormFunction : BaseForm
    {
        public FormFunction()
        {
            InitializeComponent();
        }

        #region 库存管理
        private void btnStock_Click(object sender, EventArgs e)
        {
            CreateWaitDialog();
            FormStock frm = new FormStock();
            frm.ShowDialog();
            CloseWaitDialog();
        }
        #endregion

        #region 零售明细表
        private void btnSaleDetailReport_Click(object sender, EventArgs e)
        {
            CreateWaitDialog();
            FormSaleDetailReport frm = new FormSaleDetailReport();
            frm.ShowDialog();
            CloseWaitDialog();
        }
        #endregion

        #region 挂账结算明细账
        private void btnClntRepayments_Click(object sender, EventArgs e)
        {
            CreateWaitDialog();
            FormClntRepaymentsReport frm = new FormClntRepaymentsReport();
            frm.ShowDialog();
            CloseWaitDialog();
        }
        #endregion
    }
}