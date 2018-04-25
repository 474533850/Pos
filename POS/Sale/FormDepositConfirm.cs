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
using POS.Model;
using POS.Common.utility;

namespace POS.Sale
{
    public partial class FormDepositConfirm : BaseForm
    {
        ClntModel clnt;
        public FormDepositConfirm(decimal money,ClntModel clnt)
        {
            InitializeComponent();
            this.clnt = clnt;
            lblMoney.Text = money.ToString();
        }

        private void btnConfirm_Click(object sender, EventArgs e)
        {
            string password = txtPassword.Text.Trim() == string.Empty ? string.Empty : MD5Helper.GetMd5Hash(txtPassword.Text.Trim());
            if (clnt.password == password || clnt.password == txtPassword.Text.Trim())
            {
                this.DialogResult = DialogResult.OK;
            }
            else
            {
                MessagePopup.ShowInformation("密码不正确！");
            }
        }
    }
}