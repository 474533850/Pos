namespace POS.Function
{
    partial class FormFunction
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormFunction));
            this.btnStock = new DevExpress.XtraEditors.SimpleButton();
            this.btnSaleDetailReport = new DevExpress.XtraEditors.SimpleButton();
            this.btnClntRepayments = new DevExpress.XtraEditors.SimpleButton();
            this.SuspendLayout();
            // 
            // btnStock
            // 
            this.btnStock.Appearance.Font = new System.Drawing.Font("Tahoma", 12F);
            this.btnStock.Appearance.Options.UseFont = true;
            this.btnStock.ButtonStyle = DevExpress.XtraEditors.Controls.BorderStyles.UltraFlat;
            this.btnStock.Image = ((System.Drawing.Image)(resources.GetObject("btnStock.Image")));
            this.btnStock.ImageLocation = DevExpress.XtraEditors.ImageLocation.TopCenter;
            this.btnStock.Location = new System.Drawing.Point(50, 38);
            this.btnStock.Name = "btnStock";
            this.btnStock.Size = new System.Drawing.Size(117, 64);
            this.btnStock.TabIndex = 1;
            this.btnStock.Text = "库存明细";
            this.btnStock.Click += new System.EventHandler(this.btnStock_Click);
            // 
            // btnSaleDetailReport
            // 
            this.btnSaleDetailReport.Appearance.Font = new System.Drawing.Font("Tahoma", 12F);
            this.btnSaleDetailReport.Appearance.Options.UseFont = true;
            this.btnSaleDetailReport.ButtonStyle = DevExpress.XtraEditors.Controls.BorderStyles.UltraFlat;
            this.btnSaleDetailReport.Image = ((System.Drawing.Image)(resources.GetObject("btnSaleDetailReport.Image")));
            this.btnSaleDetailReport.ImageLocation = DevExpress.XtraEditors.ImageLocation.TopCenter;
            this.btnSaleDetailReport.Location = new System.Drawing.Point(207, 38);
            this.btnSaleDetailReport.Name = "btnSaleDetailReport";
            this.btnSaleDetailReport.Size = new System.Drawing.Size(117, 64);
            this.btnSaleDetailReport.TabIndex = 2;
            this.btnSaleDetailReport.Text = "零售明细表";
            this.btnSaleDetailReport.Click += new System.EventHandler(this.btnSaleDetailReport_Click);
            // 
            // btnClntRepayments
            // 
            this.btnClntRepayments.Appearance.Font = new System.Drawing.Font("Tahoma", 12F);
            this.btnClntRepayments.Appearance.Options.UseFont = true;
            this.btnClntRepayments.ButtonStyle = DevExpress.XtraEditors.Controls.BorderStyles.UltraFlat;
            this.btnClntRepayments.Image = ((System.Drawing.Image)(resources.GetObject("btnClntRepayments.Image")));
            this.btnClntRepayments.ImageLocation = DevExpress.XtraEditors.ImageLocation.TopCenter;
            this.btnClntRepayments.Location = new System.Drawing.Point(364, 38);
            this.btnClntRepayments.Name = "btnClntRepayments";
            this.btnClntRepayments.Size = new System.Drawing.Size(125, 64);
            this.btnClntRepayments.TabIndex = 3;
            this.btnClntRepayments.Text = "挂账结算明细账";
            this.btnClntRepayments.Click += new System.EventHandler(this.btnClntRepayments_Click);
            // 
            // FormFunction
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(830, 501);
            this.Controls.Add(this.btnClntRepayments);
            this.Controls.Add(this.btnSaleDetailReport);
            this.Controls.Add(this.btnStock);
            this.Name = "FormFunction";
            this.Text = "所有功能";
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.SimpleButton btnStock;
        private DevExpress.XtraEditors.SimpleButton btnSaleDetailReport;
        private DevExpress.XtraEditors.SimpleButton btnClntRepayments;
    }
}