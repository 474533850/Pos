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
    public partial class FormAlipayConfirm : BaseForm
    {
        System.Timers.Timer timer = null;
        public FormAlipayConfirm(string way, string momey,string tradeNO)
        {
            InitializeComponent();
            lblMsg.Text = string.Format("{0}支付成功", way);
            lblAmount.Text = momey;
            lblTradeNO.Text = tradeNO;
        }

        private void FormAlipayConfirm_Load(object sender, EventArgs e)
        {

            timer = new System.Timers.Timer();
            timer.Elapsed += Timer_Elapsed;
            timer.Interval = 5000;
            timer.Start();
        }

        private void Timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            if (this.InvokeRequired)
            {
                this.Invoke((MethodInvoker)delegate
                {
                    this.Close();
                });
            }
            else
            {
                this.Close();
            }
        }

        private void btnConfirm_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
        }
    }
}