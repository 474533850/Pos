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
    public partial class FormPopup : BaseForm
    {
        public string Key { get; set; }

        int type;
        public FormPopup(int type)
        {
            InitializeComponent();
            this.type = type;
            if (type == 1)
            {
                this.Text = "商品关键字";
                txtKey.Properties.NullValuePrompt = "请输入商品关键字";
            }
            else if (type == 2)
            {
                this.Text = "会员关键字";
                txtKey.Properties.NullValuePrompt = "请输入会员/手机号";
            }
            else if (type == 3)
            {
                this.Text = "无码收银";
                txtKey.Properties.NullValuePrompt = "请输入商品价格";
            }
        }

        private void btnConfirm_Click(object sender, EventArgs e)
        {
            Key = txtKey.Text.Trim();
            if (type == 3)
            {
                decimal price;
                if (decimal.TryParse(txtKey.Text.Trim(), out price))
                {
                    this.DialogResult = DialogResult.OK;
                }
                else
                {
                    MessagePopup.ShowInformation("请输入正确的价格");
                }
            }
            else
            {
                this.DialogResult = DialogResult.OK;
            }
        }
    }
}