using POS.Helper;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace POS.WxPayAPI
{
    public partial class FormWxPay : Form
    {
        public FormWxPay()
        {
            InitializeComponent();
        }

        private void btnConfirm_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtAuth_code.Text))
            {
                MessagePopup.ShowInformation("请输入授权码！");
                return;
            }
            if (string.IsNullOrEmpty(txtFee.Text))
            {
                MessagePopup.ShowInformation("请输入商品总金额！");
                return;
            }
            //调用刷卡支付,如果内部出现异常则在页面上显示异常原因
            try
            {
                WxPayData result = MicroPay.Run("jt001", txtFee.Text, txtAuth_code.Text);
               // MessagePopup.ShowInformation(result);
            }
            catch (WxPayException ex)
            {
                MessagePopup.ShowInformation(ex.ToString());
            }
            catch (Exception ex)
            {
                MessagePopup.ShowInformation(ex.ToString());
            }
        }
    }
}
