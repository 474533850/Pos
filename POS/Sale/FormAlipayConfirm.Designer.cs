namespace POS.Sale
{
    partial class FormAlipayConfirm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.lblAmount = new System.Windows.Forms.Label();
            this.lblTradeNO = new System.Windows.Forms.Label();
            this.Label2 = new System.Windows.Forms.Label();
            this.lblAmt = new System.Windows.Forms.Label();
            this.lblMsg = new System.Windows.Forms.Label();
            this.btnConfirm = new DevExpress.XtraEditors.SimpleButton();
            this.SuspendLayout();
            // 
            // lblAmount
            // 
            this.lblAmount.AutoSize = true;
            this.lblAmount.Font = new System.Drawing.Font("宋体", 25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblAmount.Location = new System.Drawing.Point(265, 111);
            this.lblAmount.Name = "lblAmount";
            this.lblAmount.Size = new System.Drawing.Size(32, 34);
            this.lblAmount.TabIndex = 22;
            this.lblAmount.Text = "2";
            this.lblAmount.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblTradeNO
            // 
            this.lblTradeNO.AutoSize = true;
            this.lblTradeNO.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblTradeNO.Location = new System.Drawing.Point(249, 186);
            this.lblTradeNO.Name = "lblTradeNO";
            this.lblTradeNO.Size = new System.Drawing.Size(48, 16);
            this.lblTradeNO.TabIndex = 21;
            this.lblTradeNO.Text = "11111";
            this.lblTradeNO.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // Label2
            // 
            this.Label2.AutoSize = true;
            this.Label2.Font = new System.Drawing.Font("宋体", 25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Label2.Location = new System.Drawing.Point(99, 171);
            this.Label2.Name = "Label2";
            this.Label2.Size = new System.Drawing.Size(151, 34);
            this.Label2.TabIndex = 20;
            this.Label2.Tag = "6";
            this.Label2.Text = "交易号：";
            this.Label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblAmt
            // 
            this.lblAmt.AutoSize = true;
            this.lblAmt.Font = new System.Drawing.Font("宋体", 25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblAmt.Location = new System.Drawing.Point(116, 111);
            this.lblAmt.Name = "lblAmt";
            this.lblAmt.Size = new System.Drawing.Size(134, 34);
            this.lblAmt.TabIndex = 19;
            this.lblAmt.Tag = 4;
            this.lblAmt.Text = "金额 ：";
            this.lblAmt.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblMsg
            // 
            this.lblMsg.AutoSize = true;
            this.lblMsg.Font = new System.Drawing.Font("宋体", 25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblMsg.Location = new System.Drawing.Point(174, 51);
            this.lblMsg.Name = "lblMsg";
            this.lblMsg.Size = new System.Drawing.Size(219, 34);
            this.lblMsg.TabIndex = 23;
            this.lblMsg.Tag = "6";
            this.lblMsg.Text = "微信支付成功";
            this.lblMsg.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // btnConfirm
            // 
            this.btnConfirm.Appearance.Font = new System.Drawing.Font("Tahoma", 18F);
            this.btnConfirm.Appearance.Options.UseFont = true;
            this.btnConfirm.Location = new System.Drawing.Point(219, 235);
            this.btnConfirm.Name = "btnConfirm";
            this.btnConfirm.Size = new System.Drawing.Size(131, 47);
            this.btnConfirm.TabIndex = 24;
            this.btnConfirm.Text = "确认";
            this.btnConfirm.Click += new System.EventHandler(this.btnConfirm_Click);
            // 
            // FormAlipayConfirm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(577, 320);
            this.Controls.Add(this.btnConfirm);
            this.Controls.Add(this.lblMsg);
            this.Controls.Add(this.lblAmount);
            this.Controls.Add(this.lblTradeNO);
            this.Controls.Add(this.Label2);
            this.Controls.Add(this.lblAmt);
            this.Name = "FormAlipayConfirm";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "移动支付成功";
            this.TopMost = true;
            this.Load += new System.EventHandler(this.FormAlipayConfirm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblAmount;
        private System.Windows.Forms.Label lblTradeNO;
        private System.Windows.Forms.Label Label2;
        private System.Windows.Forms.Label lblAmt;
        private System.Windows.Forms.Label lblMsg;
        private DevExpress.XtraEditors.SimpleButton btnConfirm;
    }
}