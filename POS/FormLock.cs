using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Linq;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using POS.Common.utility;
using POS.Helper;

namespace POS
{
    public partial class FormLock : BaseForm
    {
        public FormLock()
        {
            InitializeComponent();
            this.FormBorderStyle = FormBorderStyle.None;
        }

        private void FormLock_SizeChanged(object sender, EventArgs e)
        {
            panelControl1.Location = new Point((this.Width-panelControl1.Width)/2, (this.Height - panelControl1.Height) / 2);
        }

        private void btnConfirm_Click(object sender, EventArgs e)
        {
            string password = txtPassword.Text.Trim() == string.Empty ? string.Empty : MD5Helper.GetMd5Hash(txtPassword.Text.Trim());
            if (RuntimeObject.CurrentUser.password == password)
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