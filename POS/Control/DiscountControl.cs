using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Linq;
using System.Windows.Forms;
using DevExpress.XtraEditors;

namespace POS.Control
{
    public partial class DiscountControl : DevExpress.XtraEditors.XtraUserControl
    {
        public delegate void DiscountEventHandler(object sender, EventArgs e);

        public event DiscountEventHandler IntegerClick;
        public event DiscountEventHandler ConfirmClick;
        public DiscountControl()
        {
            InitializeComponent();
        }

        private decimal currentDiscount;
        /// <summary>
        /// 当前折扣
        /// </summary>
        public decimal CurrentDiscount { get { return currentDiscount; } }

        #region 抹零
        private void btnInteger_Click(object sender, EventArgs e)
        {
            IntegerClick?.Invoke(sender, e);
        }
        #endregion

        #region 确认
        private void btnConfirm_Click(object sender, EventArgs e)
        {
            if (btnEditDiscount.EditValue !=null && btnEditDiscount.EditValue.ToString() !=string.Empty)
            {
                decimal discount = 1;
                if (decimal.TryParse(btnEditDiscount.EditValue.ToString(),out discount))
                {
                    currentDiscount = discount/100;
                }
                
                ConfirmClick?.Invoke(sender, e);
            }
         
           
        }
        #endregion

        private void btnKey_Click(object sender, EventArgs e)
        {
            SimpleButton btn = sender as SimpleButton;

            if (btn.Tag != null && btn.Tag.ToString() != string.Empty)
            {
                btnEditDiscount.EditValue = btn.Tag;

                btnConfirm_Click(sender,e);
            }
        }
    }
}
