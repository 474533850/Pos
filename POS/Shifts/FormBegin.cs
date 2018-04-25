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

namespace POS.Shifts
{
    public partial class FormBegin :BaseForm
    {
        decimal money=0;
        /// <summary>
        /// 当前备用金
        /// </summary>
        public decimal Money { get { return money; } }
        public FormBegin()
        {
            InitializeComponent();
        }

        private void btnConfirm_Click(object sender, EventArgs e)
        {
            if (decimal.TryParse(txtMoney.Text.Trim(), out money))
            {
                this.DialogResult = DialogResult.OK;
            }
            else
            {
                MessagePopup.ShowInformation("输入的值不正确！");
            }
        }
    }
}