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
    public partial class FormReturnJF : BaseForm
    {
        int jf;
        public int JF { get { return jf; } }
        public FormReturnJF(int jf)
        {
            InitializeComponent();
            this.jf = jf;
            txtQuantity.EditValue = jf;
        }

        private void btnConfirm_Click(object sender, EventArgs e)
        {
            int outJF = 0;
            if (int.TryParse(txtQuantity.Text.Trim(), out outJF))
            {
                if (outJF < 0)
                {
                    MessagePopup.ShowInformation("已超出规定的最小值！");
                }
                else if (outJF > jf)
                {
                    MessagePopup.ShowInformation("已超出规定的最大值！");
                }
                else
                {
                    jf = outJF;
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