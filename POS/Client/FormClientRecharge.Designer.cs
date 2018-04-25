namespace POS.Client
{
    partial class FormClientRecharge
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
            this.components = new System.ComponentModel.Container();
            this.labelControl2 = new DevExpress.XtraEditors.LabelControl();
            this.txtAmount = new DevExpress.XtraEditors.TextEdit();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.txtGiftAmount = new DevExpress.XtraEditors.TextEdit();
            this.labelControl3 = new DevExpress.XtraEditors.LabelControl();
            this.txtBalance = new DevExpress.XtraEditors.TextEdit();
            this.ckBtnUnionpayCard = new DevExpress.XtraEditors.CheckButton();
            this.ckBtnMobilePayment = new DevExpress.XtraEditors.CheckButton();
            this.ckBtnCash = new DevExpress.XtraEditors.CheckButton();
            this.groupControl1 = new DevExpress.XtraEditors.GroupControl();
            this.chkPrint = new DevExpress.XtraEditors.CheckEdit();
            this.btnConfirm = new DevExpress.XtraEditors.SimpleButton();
            this.btnCancel = new DevExpress.XtraEditors.SimpleButton();
            this.popupMenu1 = new DevExpress.XtraBars.PopupMenu(this.components);
            this.barManager1 = new DevExpress.XtraBars.BarManager(this.components);
            this.barDockControlTop = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlBottom = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlLeft = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlRight = new DevExpress.XtraBars.BarDockControl();
            this.barButtonItem1 = new DevExpress.XtraBars.BarButtonItem();
            this.barButtonItem2 = new DevExpress.XtraBars.BarButtonItem();
            this.metxnote = new DevExpress.XtraEditors.MemoEdit();
            this.labelControl4 = new DevExpress.XtraEditors.LabelControl();
            ((System.ComponentModel.ISupportInitialize)(this.txtAmount.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtGiftAmount.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtBalance.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).BeginInit();
            this.groupControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chkPrint.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.popupMenu1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.barManager1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.metxnote.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // labelControl2
            // 
            this.labelControl2.Appearance.Font = new System.Drawing.Font("Tahoma", 12F);
            this.labelControl2.Location = new System.Drawing.Point(58, 28);
            this.labelControl2.Margin = new System.Windows.Forms.Padding(2);
            this.labelControl2.Name = "labelControl2";
            this.labelControl2.Size = new System.Drawing.Size(80, 19);
            this.labelControl2.TabIndex = 2;
            this.labelControl2.Text = "充值金额：";
            // 
            // txtAmount
            // 
            this.txtAmount.EditValue = "0";
            this.txtAmount.Location = new System.Drawing.Point(141, 25);
            this.txtAmount.Name = "txtAmount";
            this.txtAmount.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 12F);
            this.txtAmount.Properties.Appearance.Options.UseFont = true;
            this.txtAmount.Properties.Appearance.Options.UseTextOptions = true;
            this.txtAmount.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.txtAmount.Properties.Mask.EditMask = "(0|[1-9][0-9]{0,9})(\\.[0-9]{1,2})?";
            this.txtAmount.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.RegEx;
            this.txtAmount.Properties.Mask.ShowPlaceHolders = false;
            this.txtAmount.Properties.MaxLength = 7;
            this.txtAmount.Size = new System.Drawing.Size(250, 26);
            this.txtAmount.TabIndex = 0;
            // 
            // labelControl1
            // 
            this.labelControl1.Appearance.Font = new System.Drawing.Font("Tahoma", 12F);
            this.labelControl1.Location = new System.Drawing.Point(58, 64);
            this.labelControl1.Margin = new System.Windows.Forms.Padding(2);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(80, 19);
            this.labelControl1.TabIndex = 2;
            this.labelControl1.Text = "赠送金额：";
            // 
            // txtGiftAmount
            // 
            this.txtGiftAmount.EditValue = "0";
            this.txtGiftAmount.Location = new System.Drawing.Point(141, 61);
            this.txtGiftAmount.Name = "txtGiftAmount";
            this.txtGiftAmount.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 12F);
            this.txtGiftAmount.Properties.Appearance.Options.UseFont = true;
            this.txtGiftAmount.Properties.Appearance.Options.UseTextOptions = true;
            this.txtGiftAmount.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.txtGiftAmount.Properties.Mask.EditMask = "(0|[1-9][0-9]{0,9})(\\.[0-9]{1,2})?";
            this.txtGiftAmount.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.RegEx;
            this.txtGiftAmount.Properties.MaxLength = 7;
            this.txtGiftAmount.Size = new System.Drawing.Size(250, 26);
            this.txtGiftAmount.TabIndex = 1;
            // 
            // labelControl3
            // 
            this.labelControl3.Appearance.Font = new System.Drawing.Font("Tahoma", 12F);
            this.labelControl3.Location = new System.Drawing.Point(58, 100);
            this.labelControl3.Margin = new System.Windows.Forms.Padding(2);
            this.labelControl3.Name = "labelControl3";
            this.labelControl3.Size = new System.Drawing.Size(80, 19);
            this.labelControl3.TabIndex = 2;
            this.labelControl3.Text = "当前余额：";
            // 
            // txtBalance
            // 
            this.txtBalance.EditValue = "0";
            this.txtBalance.Location = new System.Drawing.Point(141, 97);
            this.txtBalance.Name = "txtBalance";
            this.txtBalance.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 12F);
            this.txtBalance.Properties.Appearance.Options.UseFont = true;
            this.txtBalance.Properties.Appearance.Options.UseTextOptions = true;
            this.txtBalance.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.txtBalance.Properties.Mask.EditMask = "(0|[1-9][0-9]{0,9})(\\.[0-9]{1,2})?";
            this.txtBalance.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.RegEx;
            this.txtBalance.Properties.MaxLength = 7;
            this.txtBalance.Properties.ReadOnly = true;
            this.txtBalance.Size = new System.Drawing.Size(250, 26);
            this.txtBalance.TabIndex = 3;
            this.txtBalance.TabStop = false;
            // 
            // ckBtnUnionpayCard
            // 
            this.ckBtnUnionpayCard.AllowFocus = false;
            this.ckBtnUnionpayCard.Appearance.Font = new System.Drawing.Font("Tahoma", 12F);
            this.ckBtnUnionpayCard.Appearance.Options.UseFont = true;
            this.ckBtnUnionpayCard.Location = new System.Drawing.Point(224, 32);
            this.ckBtnUnionpayCard.Name = "ckBtnUnionpayCard";
            this.ckBtnUnionpayCard.Size = new System.Drawing.Size(100, 35);
            this.ckBtnUnionpayCard.TabIndex = 4;
            this.ckBtnUnionpayCard.Text = "银联卡";
            this.ckBtnUnionpayCard.CheckedChanged += new System.EventHandler(this.ckBtnUnionpayCard_CheckedChanged);
            this.ckBtnUnionpayCard.Click += new System.EventHandler(this.ckBtnUnionpayCard_Click);
            // 
            // ckBtnMobilePayment
            // 
            this.ckBtnMobilePayment.AllowFocus = false;
            this.ckBtnMobilePayment.Appearance.Font = new System.Drawing.Font("Tahoma", 12F);
            this.ckBtnMobilePayment.Appearance.Options.UseFont = true;
            this.ckBtnMobilePayment.Location = new System.Drawing.Point(118, 32);
            this.ckBtnMobilePayment.Name = "ckBtnMobilePayment";
            this.ckBtnMobilePayment.Size = new System.Drawing.Size(100, 35);
            this.ckBtnMobilePayment.TabIndex = 3;
            this.ckBtnMobilePayment.Text = "移动支付";
            this.ckBtnMobilePayment.CheckedChanged += new System.EventHandler(this.ckBtnMobilePayment_CheckedChanged);
            // 
            // ckBtnCash
            // 
            this.ckBtnCash.AllowFocus = false;
            this.ckBtnCash.Appearance.Font = new System.Drawing.Font("Tahoma", 12F);
            this.ckBtnCash.Appearance.Options.UseFont = true;
            this.ckBtnCash.ButtonStyle = DevExpress.XtraEditors.Controls.BorderStyles.UltraFlat;
            this.ckBtnCash.Checked = true;
            this.ckBtnCash.Location = new System.Drawing.Point(12, 32);
            this.ckBtnCash.Name = "ckBtnCash";
            this.ckBtnCash.Size = new System.Drawing.Size(100, 35);
            this.ckBtnCash.TabIndex = 2;
            this.ckBtnCash.Text = "现金";
            this.ckBtnCash.CheckedChanged += new System.EventHandler(this.ckBtnCash_CheckedChanged);
            // 
            // groupControl1
            // 
            this.groupControl1.Appearance.Font = new System.Drawing.Font("Tahoma", 12F);
            this.groupControl1.Appearance.Options.UseFont = true;
            this.groupControl1.AppearanceCaption.Font = new System.Drawing.Font("Tahoma", 12F);
            this.groupControl1.AppearanceCaption.Options.UseFont = true;
            this.groupControl1.Controls.Add(this.ckBtnCash);
            this.groupControl1.Controls.Add(this.ckBtnUnionpayCard);
            this.groupControl1.Controls.Add(this.ckBtnMobilePayment);
            this.groupControl1.Location = new System.Drawing.Point(58, 192);
            this.groupControl1.Name = "groupControl1";
            this.groupControl1.Size = new System.Drawing.Size(337, 76);
            this.groupControl1.TabIndex = 18;
            this.groupControl1.Text = "充值方式";
            // 
            // chkPrint
            // 
            this.chkPrint.Location = new System.Drawing.Point(58, 273);
            this.chkPrint.Name = "chkPrint";
            this.chkPrint.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 12F);
            this.chkPrint.Properties.Appearance.Options.UseFont = true;
            this.chkPrint.Properties.AutoWidth = true;
            this.chkPrint.Properties.Caption = "充值成功后打印小票";
            this.chkPrint.Size = new System.Drawing.Size(166, 23);
            this.chkPrint.TabIndex = 5;
            // 
            // btnConfirm
            // 
            this.btnConfirm.Appearance.Font = new System.Drawing.Font("Tahoma", 12F);
            this.btnConfirm.Appearance.Options.UseFont = true;
            this.btnConfirm.Location = new System.Drawing.Point(255, 304);
            this.btnConfirm.Name = "btnConfirm";
            this.btnConfirm.Size = new System.Drawing.Size(128, 38);
            this.btnConfirm.TabIndex = 7;
            this.btnConfirm.Text = "确认充值";
            this.btnConfirm.Click += new System.EventHandler(this.btnConfirm_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Appearance.Font = new System.Drawing.Font("Tahoma", 12F);
            this.btnCancel.Appearance.Options.UseFont = true;
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(58, 304);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(128, 38);
            this.btnCancel.TabIndex = 6;
            this.btnCancel.Text = "取消";
            // 
            // popupMenu1
            // 
            this.popupMenu1.Manager = this.barManager1;
            this.popupMenu1.Name = "popupMenu1";
            // 
            // barManager1
            // 
            this.barManager1.DockControls.Add(this.barDockControlTop);
            this.barManager1.DockControls.Add(this.barDockControlBottom);
            this.barManager1.DockControls.Add(this.barDockControlLeft);
            this.barManager1.DockControls.Add(this.barDockControlRight);
            this.barManager1.Form = this;
            this.barManager1.Items.AddRange(new DevExpress.XtraBars.BarItem[] {
            this.barButtonItem1,
            this.barButtonItem2});
            this.barManager1.MaxItemId = 2;
            // 
            // barDockControlTop
            // 
            this.barDockControlTop.CausesValidation = false;
            this.barDockControlTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.barDockControlTop.Location = new System.Drawing.Point(0, 0);
            this.barDockControlTop.Size = new System.Drawing.Size(446, 0);
            // 
            // barDockControlBottom
            // 
            this.barDockControlBottom.CausesValidation = false;
            this.barDockControlBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.barDockControlBottom.Location = new System.Drawing.Point(0, 364);
            this.barDockControlBottom.Size = new System.Drawing.Size(446, 0);
            // 
            // barDockControlLeft
            // 
            this.barDockControlLeft.CausesValidation = false;
            this.barDockControlLeft.Dock = System.Windows.Forms.DockStyle.Left;
            this.barDockControlLeft.Location = new System.Drawing.Point(0, 0);
            this.barDockControlLeft.Size = new System.Drawing.Size(0, 364);
            // 
            // barDockControlRight
            // 
            this.barDockControlRight.CausesValidation = false;
            this.barDockControlRight.Dock = System.Windows.Forms.DockStyle.Right;
            this.barDockControlRight.Location = new System.Drawing.Point(446, 0);
            this.barDockControlRight.Size = new System.Drawing.Size(0, 364);
            // 
            // barButtonItem1
            // 
            this.barButtonItem1.Caption = "农行";
            this.barButtonItem1.Id = 0;
            this.barButtonItem1.Name = "barButtonItem1";
            // 
            // barButtonItem2
            // 
            this.barButtonItem2.Caption = "工行";
            this.barButtonItem2.Id = 1;
            this.barButtonItem2.Name = "barButtonItem2";
            // 
            // metxnote
            // 
            this.metxnote.Location = new System.Drawing.Point(141, 129);
            this.metxnote.MenuManager = this.barManager1;
            this.metxnote.Name = "metxnote";
            this.metxnote.Size = new System.Drawing.Size(250, 57);
            this.metxnote.TabIndex = 23;
            // 
            // labelControl4
            // 
            this.labelControl4.Appearance.Font = new System.Drawing.Font("Tahoma", 12F);
            this.labelControl4.Location = new System.Drawing.Point(90, 130);
            this.labelControl4.Margin = new System.Windows.Forms.Padding(2);
            this.labelControl4.Name = "labelControl4";
            this.labelControl4.Size = new System.Drawing.Size(48, 19);
            this.labelControl4.TabIndex = 24;
            this.labelControl4.Text = "备注：";
            // 
            // FormClientRecharge
            // 
            this.AcceptButton = this.btnConfirm;
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(446, 364);
            this.Controls.Add(this.labelControl4);
            this.Controls.Add(this.metxnote);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnConfirm);
            this.Controls.Add(this.chkPrint);
            this.Controls.Add(this.groupControl1);
            this.Controls.Add(this.txtBalance);
            this.Controls.Add(this.txtGiftAmount);
            this.Controls.Add(this.labelControl3);
            this.Controls.Add(this.labelControl1);
            this.Controls.Add(this.txtAmount);
            this.Controls.Add(this.labelControl2);
            this.Controls.Add(this.barDockControlLeft);
            this.Controls.Add(this.barDockControlRight);
            this.Controls.Add(this.barDockControlBottom);
            this.Controls.Add(this.barDockControlTop);
            this.Name = "FormClientRecharge";
            this.Text = "充值";
            ((System.ComponentModel.ISupportInitialize)(this.txtAmount.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtGiftAmount.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtBalance.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).EndInit();
            this.groupControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.chkPrint.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.popupMenu1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.barManager1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.metxnote.Properties)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraEditors.LabelControl labelControl2;
        private DevExpress.XtraEditors.TextEdit txtAmount;
        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraEditors.TextEdit txtGiftAmount;
        private DevExpress.XtraEditors.LabelControl labelControl3;
        private DevExpress.XtraEditors.TextEdit txtBalance;
        private DevExpress.XtraEditors.CheckButton ckBtnUnionpayCard;
        private DevExpress.XtraEditors.CheckButton ckBtnMobilePayment;
        private DevExpress.XtraEditors.CheckButton ckBtnCash;
        private DevExpress.XtraEditors.GroupControl groupControl1;
        private DevExpress.XtraEditors.CheckEdit chkPrint;
        private DevExpress.XtraEditors.SimpleButton btnConfirm;
        private DevExpress.XtraEditors.SimpleButton btnCancel;
        private DevExpress.XtraBars.PopupMenu popupMenu1;
        private DevExpress.XtraBars.BarButtonItem barButtonItem1;
        private DevExpress.XtraBars.BarButtonItem barButtonItem2;
        private DevExpress.XtraBars.BarManager barManager1;
        private DevExpress.XtraBars.BarDockControl barDockControlTop;
        private DevExpress.XtraBars.BarDockControl barDockControlBottom;
        private DevExpress.XtraBars.BarDockControl barDockControlLeft;
        private DevExpress.XtraBars.BarDockControl barDockControlRight;
        private DevExpress.XtraEditors.LabelControl labelControl4;
        private DevExpress.XtraEditors.MemoEdit metxnote;
    }
}