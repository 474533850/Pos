using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Linq;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using POS.Model;
using POS.Helper;
using POS.Common.utility;

namespace POS.Sale
{
    public partial class FormPOSDetail : BaseForm
    {
        private PosbbModel currentPosbb;
        public PosbbModel CurrentPosbb { get { return currentPosbb; } }

        private bool isChange_zhe = false;
        private bool isChange_price = false;
        public FormPOSDetail(PosbbModel currentPosbb)
        {
            InitializeComponent();
            this.currentPosbb = currentPosbb;
            Init();
        }

        private void Init()
        {
            isChange_zhe = true;
            isChange_price = true;
            lblxpricold.Text = currentPosbb.xpricold.ToString();
            bteZhe.EditValue = currentPosbb.xzhe;
            txtxpric.EditValue = currentPosbb.xpric;
            txtQuantity.EditValue = currentPosbb.unitquat;
            lblxallp.Text = string.Format("￥{0}", currentPosbb.xallp.ToString());
        }

        private void FormSaleDetail_Load(object sender, EventArgs e)
        {
            this.ActiveControl = txtQuantity;
        }

        #region 键盘操作
        private void btnBack_Click(object sender, EventArgs e)
        {
            SendKeys.Send("{BACKSPACE}");
        }

        private void btnConfirm_Click(object sender, EventArgs e)
        {
            this.Validate(true);
            if (string.IsNullOrEmpty(txtxpric.Text.Trim()))
            {
                MessagePopup.ShowInformation("现价不能为空？");
                return;
            }
            if (string.IsNullOrEmpty(txtQuantity.Text.Trim()))
            {
                MessagePopup.ShowInformation("数量不能为空？");
                return;
            }
            if (decimal.Parse(txtxpric.Text.Trim())<0)
            {
                MessagePopup.ShowInformation("现价不能小于零？");
                return;
            }
            if (decimal.Parse(txtQuantity.Text.Trim()) <= 0)
            {
                MessagePopup.ShowInformation("数量必须大于零？");
                return;
            }
            currentPosbb.xzhe = CalcMoneyHelper.CalcZhe(txtxpric.Text.Trim(), CurrentPosbb.xpricold);
            currentPosbb.xpric = decimal.Parse(txtxpric.Text.Trim());
            currentPosbb.xquat = decimal.Parse(txtQuantity.Text.Trim());
            currentPosbb.xallp = decimal.Parse(lblxallp.Tag.ToString());
            this.DialogResult = DialogResult.OK;
        }

        private void btnKey_Click(object sender, EventArgs e)
        {
            SimpleButton btn = sender as SimpleButton;
            SendKeys.Send(btn.Tag.ToString());
        }

        #endregion

        private void bteZhe_EditValueChanged(object sender, EventArgs e)
        {
            if (tsZhe.EditValue != null && !string.IsNullOrEmpty(tsZhe.EditValue.ToString()))
            {
                if (isChange_price)
                {
                    isChange_price = false;
                    return;
                }
                decimal zhe;
                if (decimal.TryParse(bteZhe.Text.Trim(), out zhe))
                {
                    if (tsZhe.IsOn)
                    {
                        isChange_zhe = true;
                        decimal d = CalcMoneyHelper.Multiply(CurrentPosbb.xpricold, (zhe / 100));
                        txtxpric.Text = d.ToString();
                    }
                    else
                    {
                        txtxpric.EditValue = CalcMoneyHelper.Subtract(CurrentPosbb.xpricold, zhe);
                    }
                }
                CalcMoney();
            }
        }

        private void txtxpric_EditValueChanged(object sender, EventArgs e)
        {
            if (txtxpric.EditValue != null && !string.IsNullOrEmpty(txtxpric.EditValue.ToString()))
            {
                if (isChange_zhe)
                {
                    isChange_zhe = false;
                    return;
                }
                decimal price;
                if (decimal.TryParse(txtxpric.Text.Trim(), out price))
                {
                    isChange_price = true;
                    if (tsZhe.IsOn)
                    {
                        bteZhe.EditValue = CalcMoneyHelper.CalcZhe(price, CurrentPosbb.xpricold);
                    }
                    else
                    {
                        bteZhe.EditValue = CalcMoneyHelper.Subtract(CurrentPosbb.xpricold, price);
                    }
                }
                CalcMoney();
            }
        }

        private void txtQuantity_EditValueChanged(object sender, EventArgs e)
        {
            if (txtQuantity.EditValue != null && !string.IsNullOrEmpty(txtQuantity.EditValue.ToString()))
            {
                currentPosbb.unitquat = decimal.Parse(txtQuantity.EditValue.ToString());
                currentPosbb.xquat = CalcMoneyHelper.Multiply(currentPosbb.unitquat, txtQuantity.EditValue);
                CalcMoney();
            }
        }

        private void tsZhe_Toggled(object sender, EventArgs e)
        {
            bteZhe.Properties.Buttons[0].Visible = tsZhe.IsOn;
            float price;
            if (float.TryParse(txtxpric.Text, out price))
            {
                isChange_zhe = true;
                if (tsZhe.IsOn)
                {
                    bteZhe.EditValue = CalcMoneyHelper.CalcZhe(txtxpric.Text, currentPosbb.xpricold);
                }
                else
                {
                    bteZhe.EditValue = CalcMoneyHelper.Subtract(currentPosbb.xpricold, price);
                }
            }
        }

        private void CalcMoney()
        {
            decimal zhe;
            if (decimal.TryParse(bteZhe.Text.Trim(), out zhe))
            {
                decimal totalMoney = 0;
                if (tsZhe.IsOn)
                {
                    totalMoney = CalcMoneyHelper.Multiply(txtxpric.EditValue, txtQuantity.EditValue);
                }
                else
                {
                    totalMoney = CalcMoneyHelper.Multiply(txtxpric.EditValue, txtQuantity.EditValue);
                }
                lblxallp.Tag = totalMoney;
                lblxallp.Text = totalMoney.ToString("C");
            }
        }

        private void FormPOSDetail_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btnConfirm_Click(null,null);
            }
        }
    }
}