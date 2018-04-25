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
    public partial class FormReturnedMes : BaseForm
    {
        public string Remark { get; set; }
        public FormReturnedMes()
        {
            InitializeComponent();
        }

        private void btnConfirm_Click(object sender, EventArgs e)
        {
            Remark = metRemark.Text.Trim();
            this.DialogResult = DialogResult.OK;
        }
    }
}