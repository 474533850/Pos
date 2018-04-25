namespace POS.Sale
{
    partial class FormInvalidMse
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
            this.btnInvalidAndNew = new DevExpress.XtraEditors.SimpleButton();
            this.btnCancel = new DevExpress.XtraEditors.SimpleButton();
            this.metRemark = new DevExpress.XtraEditors.MemoEdit();
            this.btnInvalid = new DevExpress.XtraEditors.SimpleButton();
            ((System.ComponentModel.ISupportInitialize)(this.metRemark.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // labelControl1
            // 
            this.labelControl1.Appearance.Font = new System.Drawing.Font("Tahoma", 12F);
            this.labelControl1.Location = new System.Drawing.Point(12, 12);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(192, 19);
            this.labelControl1.TabIndex = 6;
            this.labelControl1.Text = "确认要反结账当前单据吗？";
            // 
            // btnInvalidAndNew
            // 
            this.btnInvalidAndNew.Appearance.Font = new System.Drawing.Font("Tahoma", 12F);
            this.btnInvalidAndNew.Appearance.Options.UseFont = true;
            this.btnInvalidAndNew.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnInvalidAndNew.Location = new System.Drawing.Point(114, 223);
            this.btnInvalidAndNew.Name = "btnInvalidAndNew";
            this.btnInvalidAndNew.Size = new System.Drawing.Size(118, 43);
            this.btnInvalidAndNew.TabIndex = 4;
            this.btnInvalidAndNew.Text = "作废并复用";
            this.btnInvalidAndNew.Click += new System.EventHandler(this.btnInvalidAndNew_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Appearance.Font = new System.Drawing.Font("Tahoma", 12F);
            this.btnCancel.Appearance.Options.UseFont = true;
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(12, 223);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(96, 43);
            this.btnCancel.TabIndex = 5;
            this.btnCancel.Text = "取消";
            // 
            // metRemark
            // 
            this.metRemark.Location = new System.Drawing.Point(12, 37);
            this.metRemark.Name = "metRemark";
            this.metRemark.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 12F);
            this.metRemark.Properties.Appearance.Options.UseFont = true;
            this.metRemark.Properties.NullValuePrompt = "在此输入反结账备注信息！";
            this.metRemark.Properties.NullValuePromptShowForEmptyValue = true;
            this.metRemark.Properties.ShowNullValuePromptWhenFocused = true;
            this.metRemark.Size = new System.Drawing.Size(357, 171);
            this.metRemark.TabIndex = 3;
            // 
            // btnInvalid
            // 
            this.btnInvalid.Appearance.Font = new System.Drawing.Font("Tahoma", 12F);
            this.btnInvalid.Appearance.Options.UseFont = true;
            this.btnInvalid.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnInvalid.Location = new System.Drawing.Point(238, 223);
            this.btnInvalid.Name = "btnInvalid";
            this.btnInvalid.Size = new System.Drawing.Size(131, 43);
            this.btnInvalid.TabIndex = 4;
            this.btnInvalid.Text = "直接反结账";
            this.btnInvalid.Click += new System.EventHandler(this.btnInvalid_Click);
            // 
            // FormInvalidMse
            // 
            this.AcceptButton = this.btnInvalid;
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(379, 277);
            this.Controls.Add(this.labelControl1);
            this.Controls.Add(this.btnInvalid);
            this.Controls.Add(this.btnInvalidAndNew);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.metRemark);
            this.Name = "FormInvalidMse";
            this.Text = "你正在选择反结账单据";
            ((System.ComponentModel.ISupportInitialize)(this.metRemark.Properties)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraEditors.SimpleButton btnInvalidAndNew;
        private DevExpress.XtraEditors.SimpleButton btnCancel;
        private DevExpress.XtraEditors.MemoEdit metRemark;
        private DevExpress.XtraEditors.SimpleButton btnInvalid;
    }
}