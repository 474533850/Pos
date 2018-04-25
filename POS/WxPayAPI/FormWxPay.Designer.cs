namespace POS.WxPayAPI
{
    partial class FormWxPay
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
            this.labelControl2 = new DevExpress.XtraEditors.LabelControl();
            this.txtBody = new DevExpress.XtraEditors.TextEdit();
            this.txtFee = new DevExpress.XtraEditors.TextEdit();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.txtAuth_code = new DevExpress.XtraEditors.TextEdit();
            this.labelControl3 = new DevExpress.XtraEditors.LabelControl();
            this.btnCancel = new DevExpress.XtraEditors.SimpleButton();
            this.btnConfirm = new DevExpress.XtraEditors.SimpleButton();
            ((System.ComponentModel.ISupportInitialize)(this.txtBody.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtFee.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtAuth_code.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // labelControl2
            // 
            this.labelControl2.Appearance.Font = new System.Drawing.Font("Tahoma", 12F);
            this.labelControl2.Location = new System.Drawing.Point(125, 68);
            this.labelControl2.Margin = new System.Windows.Forms.Padding(2);
            this.labelControl2.Name = "labelControl2";
            this.labelControl2.Size = new System.Drawing.Size(80, 19);
            this.labelControl2.TabIndex = 2;
            this.labelControl2.Text = "商品描述：";
            // 
            // txtBody
            // 
            this.txtBody.EditValue = "";
            this.txtBody.Location = new System.Drawing.Point(209, 65);
            this.txtBody.Margin = new System.Windows.Forms.Padding(2);
            this.txtBody.Name = "txtBody";
            this.txtBody.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 12F);
            this.txtBody.Properties.Appearance.Options.UseFont = true;
            this.txtBody.Size = new System.Drawing.Size(199, 26);
            this.txtBody.TabIndex = 3;
            // 
            // txtFee
            // 
            this.txtFee.EditValue = "";
            this.txtFee.Location = new System.Drawing.Point(209, 114);
            this.txtFee.Margin = new System.Windows.Forms.Padding(2);
            this.txtFee.Name = "txtFee";
            this.txtFee.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 12F);
            this.txtFee.Properties.Appearance.Options.UseFont = true;
            this.txtFee.Size = new System.Drawing.Size(199, 26);
            this.txtFee.TabIndex = 3;
            // 
            // labelControl1
            // 
            this.labelControl1.Appearance.Font = new System.Drawing.Font("Tahoma", 12F);
            this.labelControl1.Location = new System.Drawing.Point(125, 117);
            this.labelControl1.Margin = new System.Windows.Forms.Padding(2);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(80, 19);
            this.labelControl1.TabIndex = 2;
            this.labelControl1.Text = "支付金额：";
            // 
            // txtAuth_code
            // 
            this.txtAuth_code.EditValue = "";
            this.txtAuth_code.Location = new System.Drawing.Point(209, 174);
            this.txtAuth_code.Margin = new System.Windows.Forms.Padding(2);
            this.txtAuth_code.Name = "txtAuth_code";
            this.txtAuth_code.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 12F);
            this.txtAuth_code.Properties.Appearance.Options.UseFont = true;
            this.txtAuth_code.Size = new System.Drawing.Size(199, 26);
            this.txtAuth_code.TabIndex = 3;
            // 
            // labelControl3
            // 
            this.labelControl3.Appearance.Font = new System.Drawing.Font("Tahoma", 12F);
            this.labelControl3.Location = new System.Drawing.Point(125, 177);
            this.labelControl3.Margin = new System.Windows.Forms.Padding(2);
            this.labelControl3.Name = "labelControl3";
            this.labelControl3.Size = new System.Drawing.Size(64, 19);
            this.labelControl3.TabIndex = 2;
            this.labelControl3.Text = "授权码：";
            // 
            // btnCancel
            // 
            this.btnCancel.Appearance.Font = new System.Drawing.Font("Tahoma", 12F);
            this.btnCancel.Appearance.Options.UseFont = true;
            this.btnCancel.Location = new System.Drawing.Point(156, 280);
            this.btnCancel.Margin = new System.Windows.Forms.Padding(2);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(91, 34);
            this.btnCancel.TabIndex = 23;
            this.btnCancel.Text = "取消";
            // 
            // btnConfirm
            // 
            this.btnConfirm.Appearance.Font = new System.Drawing.Font("Tahoma", 12F);
            this.btnConfirm.Appearance.Options.UseFont = true;
            this.btnConfirm.Location = new System.Drawing.Point(317, 280);
            this.btnConfirm.Margin = new System.Windows.Forms.Padding(2);
            this.btnConfirm.Name = "btnConfirm";
            this.btnConfirm.Size = new System.Drawing.Size(91, 34);
            this.btnConfirm.TabIndex = 22;
            this.btnConfirm.Text = "确定";
            this.btnConfirm.Click += new System.EventHandler(this.btnConfirm_Click);
            // 
            // FormWxPay
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(584, 403);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnConfirm);
            this.Controls.Add(this.labelControl3);
            this.Controls.Add(this.txtAuth_code);
            this.Controls.Add(this.labelControl1);
            this.Controls.Add(this.txtFee);
            this.Controls.Add(this.labelControl2);
            this.Controls.Add(this.txtBody);
            this.Name = "FormWxPay";
            this.Text = "FormWxPay";
            ((System.ComponentModel.ISupportInitialize)(this.txtBody.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtFee.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtAuth_code.Properties)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraEditors.LabelControl labelControl2;
        private DevExpress.XtraEditors.TextEdit txtBody;
        private DevExpress.XtraEditors.TextEdit txtFee;
        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraEditors.TextEdit txtAuth_code;
        private DevExpress.XtraEditors.LabelControl labelControl3;
        private DevExpress.XtraEditors.SimpleButton btnCancel;
        private DevExpress.XtraEditors.SimpleButton btnConfirm;
    }
}