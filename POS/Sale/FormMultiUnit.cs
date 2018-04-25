using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Linq;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using POS.Helper;

namespace POS.Sale
{
    public partial class FormMultiUnit : BaseForm
    {
        decimal quantity;
        public decimal Quantity { get { return quantity; } }
        public FormMultiUnit()
        {
            InitializeComponent();
            txtQuantity.EditValue = 1;
        }

        private void btnConfirm_Click(object sender, EventArgs e)
        {
            decimal outquantity = 0;
            if (decimal.TryParse(txtQuantity.Text.Trim(), out outquantity))
            {
                if (outquantity <= 0)
                {
                    MessagePopup.ShowInformation("已超出规定的最小值！");
                }
                else
                {
                    quantity = outquantity;
                    this.DialogResult = DialogResult.OK;
                }

            }
            else
            {
                MessagePopup.ShowInformation("输入的值不正确！");
            }
        }
    }
}