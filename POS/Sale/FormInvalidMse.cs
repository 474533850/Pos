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
    public partial class FormInvalidMse : BaseForm
    {
        public string Remark { get; set; }
        public bool IsInvalidAndNew { get; set; }
        public FormInvalidMse()
        {
            InitializeComponent();
        }

        private void btnInvalidAndNew_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            Remark = metRemark.Text.Trim();
            IsInvalidAndNew = true;
        }

        private void btnInvalid_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            Remark = metRemark.Text.Trim();
        }
    }
}