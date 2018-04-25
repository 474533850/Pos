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

namespace POS.Client
{
    public partial class FormClientExchangeQuery : BaseForm
    {
        int quantity;
        public int Quantity { get { return quantity; } }
        public FormClientExchangeQuery(int quantity)
        {
            InitializeComponent();
            this.quantity = quantity;
            txtQuantity.EditValue = quantity;
        }

        private void btnConfirm_Click(object sender, EventArgs e)
        {
            int outquantity = 0;
            if (int.TryParse(txtQuantity.Text.Trim(), out outquantity))
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