namespace POS.Pending
{
    partial class FormPendingMes
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
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.btnModify = new DevExpress.XtraEditors.SimpleButton();
            this.btnReceipt = new DevExpress.XtraEditors.SimpleButton();
            this.SuspendLayout();
            // 
            // labelControl1
            // 
            this.labelControl1.Appearance.Font = new System.Drawing.Font("Tahoma", 10F);
            this.labelControl1.Location = new System.Drawing.Point(12, 24);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(168, 17);
            this.labelControl1.TabIndex = 0;
            this.labelControl1.Text = "本单据已确认可直接付款？";
            // 
            // btnModify
            // 
            this.btnModify.Appearance.Font = new System.Drawing.Font("Tahoma", 10F);
            this.btnModify.Appearance.Options.UseFont = true;
            this.btnModify.Location = new System.Drawing.Point(12, 70);
            this.btnModify.Name = "btnModify";
            this.btnModify.Size = new System.Drawing.Size(125, 40);
            this.btnModify.TabIndex = 1;
            this.btnModify.Text = "再编辑";
            this.btnModify.Click += new System.EventHandler(this.btnModify_Click);
            // 
            // btnReceipt
            // 
            this.btnReceipt.Appearance.Font = new System.Drawing.Font("Tahoma", 10F);
            this.btnReceipt.Appearance.Options.UseFont = true;
            this.btnReceipt.Location = new System.Drawing.Point(206, 70);
            this.btnReceipt.Name = "btnReceipt";
            this.btnReceipt.Size = new System.Drawing.Size(164, 40);
            this.btnReceipt.TabIndex = 1;
            this.btnReceipt.Text = "马上收款";
            this.btnReceipt.Click += new System.EventHandler(this.btnReceipt_Click);
            // 
            // FormPendingMss
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(382, 119);
            this.ControlBox = false;
            this.Controls.Add(this.btnReceipt);
            this.Controls.Add(this.btnModify);
            this.Controls.Add(this.labelControl1);
            this.Name = "FormPendingMss";
            this.Text = "提示";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraEditors.SimpleButton btnModify;
        private DevExpress.XtraEditors.SimpleButton btnReceipt;
    }
}