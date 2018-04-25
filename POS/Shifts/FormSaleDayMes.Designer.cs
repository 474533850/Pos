namespace POS.Shifts
{
    partial class FormSaleDayMes
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
            this.btnBack = new DevExpress.XtraEditors.SimpleButton();
            this.btnShift = new DevExpress.XtraEditors.SimpleButton();
            this.SuspendLayout();
            // 
            // labelControl1
            // 
            this.labelControl1.Appearance.Font = new System.Drawing.Font("Tahoma", 10F);
            this.labelControl1.Location = new System.Drawing.Point(12, 24);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(186, 17);
            this.labelControl1.TabIndex = 0;
            this.labelControl1.Text = "你已完成日结,是否退出系统？";
            // 
            // btnBack
            // 
            this.btnBack.Appearance.Font = new System.Drawing.Font("Tahoma", 10F);
            this.btnBack.Appearance.Options.UseFont = true;
            this.btnBack.Location = new System.Drawing.Point(12, 70);
            this.btnBack.Name = "btnBack";
            this.btnBack.Size = new System.Drawing.Size(125, 40);
            this.btnBack.TabIndex = 1;
            this.btnBack.Text = "返回收银";
            this.btnBack.Click += new System.EventHandler(this.btnBack_Click);
            // 
            // btnShift
            // 
            this.btnShift.Appearance.Font = new System.Drawing.Font("Tahoma", 10F);
            this.btnShift.Appearance.Options.UseFont = true;
            this.btnShift.Location = new System.Drawing.Point(206, 70);
            this.btnShift.Name = "btnShift";
            this.btnShift.Size = new System.Drawing.Size(164, 40);
            this.btnShift.TabIndex = 1;
            this.btnShift.Text = "交接班";
            this.btnShift.Click += new System.EventHandler(this.btnShift_Click);
            // 
            // FormSaleDayMes
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(382, 119);
            this.ControlBox = false;
            this.Controls.Add(this.btnShift);
            this.Controls.Add(this.btnBack);
            this.Controls.Add(this.labelControl1);
            this.Name = "FormSaleDayMes";
            this.Text = "提示";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraEditors.SimpleButton btnBack;
        private DevExpress.XtraEditors.SimpleButton btnShift;
    }
}