using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Linq;
using System.Windows.Forms;
using DevExpress.XtraEditors;

namespace POS.Sale
{
    public partial class FormSaleDetail : BaseForm
    {
        public FormSaleDetail()
        {
            InitializeComponent();
            
        }

        private void FormSaleDetail_Load(object sender, EventArgs e)
        {
           this.ActiveControl = txtQuantity;
        }

        #region 键盘操作
        private void btnBack_Click(object sender, EventArgs e)
        {
            SendKeys.Send("{BACKSPACE}");
        }

        private void btnConfirm_Click(object sender, EventArgs e)
        {
            this.Validate(true);
        }

        private void btnKey_Click(object sender, EventArgs e)
        {
            SimpleButton btn = sender as SimpleButton;
            SendKeys.Send(btn.Tag.ToString());
        }
        #endregion

     
    }
}