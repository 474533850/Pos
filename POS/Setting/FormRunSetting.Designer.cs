namespace POS.Setting
{
    partial class FormRunSetting
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
            this.btnConfirm = new DevExpress.XtraEditors.SimpleButton();
            this.labelControl3 = new DevExpress.XtraEditors.LabelControl();
            this.账套 = new DevExpress.XtraEditors.LabelControl();
            this.txtSID = new DevExpress.XtraEditors.TextEdit();
            this.lueDepartment = new DevExpress.XtraEditors.LookUpEdit();
            this.btnPre = new DevExpress.XtraEditors.SimpleButton();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.tab = new DevExpress.XtraTab.XtraTabControl();
            this.xtraTabPage1 = new DevExpress.XtraTab.XtraTabPage();
            this.btnNext = new DevExpress.XtraEditors.SimpleButton();
            this.btnCancel = new DevExpress.XtraEditors.SimpleButton();
            this.xtraTabPage2 = new DevExpress.XtraTab.XtraTabPage();
            this.labelControl2 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl23 = new DevExpress.XtraEditors.LabelControl();
            this.memoKey = new DevExpress.XtraEditors.MemoEdit();
            ((System.ComponentModel.ISupportInitialize)(this.txtSID.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lueDepartment.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tab)).BeginInit();
            this.tab.SuspendLayout();
            this.xtraTabPage1.SuspendLayout();
            this.xtraTabPage2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.memoKey.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // btnConfirm
            // 
            this.btnConfirm.Appearance.Font = new System.Drawing.Font("Tahoma", 12F);
            this.btnConfirm.Appearance.Options.UseFont = true;
            this.btnConfirm.Location = new System.Drawing.Point(234, 169);
            this.btnConfirm.Margin = new System.Windows.Forms.Padding(2);
            this.btnConfirm.Name = "btnConfirm";
            this.btnConfirm.Size = new System.Drawing.Size(91, 34);
            this.btnConfirm.TabIndex = 11;
            this.btnConfirm.Text = "确定";
            this.btnConfirm.Click += new System.EventHandler(this.btnConfirm_Click);
            // 
            // labelControl3
            // 
            this.labelControl3.Appearance.Font = new System.Drawing.Font("Tahoma", 12F);
            this.labelControl3.Location = new System.Drawing.Point(73, 87);
            this.labelControl3.Margin = new System.Windows.Forms.Padding(2);
            this.labelControl3.Name = "labelControl3";
            this.labelControl3.Size = new System.Drawing.Size(48, 19);
            this.labelControl3.TabIndex = 8;
            this.labelControl3.Text = "分部：";
            // 
            // 账套
            // 
            this.账套.Appearance.Font = new System.Drawing.Font("Tahoma", 12F);
            this.账套.Location = new System.Drawing.Point(70, 66);
            this.账套.Margin = new System.Windows.Forms.Padding(2);
            this.账套.Name = "账套";
            this.账套.Size = new System.Drawing.Size(48, 19);
            this.账套.TabIndex = 9;
            this.账套.Text = "账套：";
            // 
            // txtSID
            // 
            this.txtSID.EditValue = "1754";
            this.txtSID.Location = new System.Drawing.Point(123, 63);
            this.txtSID.Margin = new System.Windows.Forms.Padding(2);
            this.txtSID.Name = "txtSID";
            this.txtSID.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 12F);
            this.txtSID.Properties.Appearance.Options.UseFont = true;
            this.txtSID.Size = new System.Drawing.Size(199, 26);
            this.txtSID.TabIndex = 3;
            // 
            // lueDepartment
            // 
            this.lueDepartment.EditValue = "";
            this.lueDepartment.Location = new System.Drawing.Point(126, 84);
            this.lueDepartment.Margin = new System.Windows.Forms.Padding(2);
            this.lueDepartment.Name = "lueDepartment";
            this.lueDepartment.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 12F);
            this.lueDepartment.Properties.Appearance.Options.UseFont = true;
            this.lueDepartment.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.lueDepartment.Properties.Columns.AddRange(new DevExpress.XtraEditors.Controls.LookUpColumnInfo[] {
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("xlsname", "名称")});
            this.lueDepartment.Properties.DisplayMember = "xlsname";
            this.lueDepartment.Properties.NullText = "";
            this.lueDepartment.Properties.ValueMember = "xls";
            this.lueDepartment.Size = new System.Drawing.Size(199, 26);
            this.lueDepartment.TabIndex = 4;
            // 
            // btnPre
            // 
            this.btnPre.Appearance.Font = new System.Drawing.Font("Tahoma", 12F);
            this.btnPre.Appearance.Options.UseFont = true;
            this.btnPre.Location = new System.Drawing.Point(73, 169);
            this.btnPre.Margin = new System.Windows.Forms.Padding(2);
            this.btnPre.Name = "btnPre";
            this.btnPre.Size = new System.Drawing.Size(91, 34);
            this.btnPre.TabIndex = 11;
            this.btnPre.Text = "上一步";
            this.btnPre.Click += new System.EventHandler(this.btnPre_Click);
            // 
            // labelControl1
            // 
            this.labelControl1.Appearance.Font = new System.Drawing.Font("Tahoma", 12F);
            this.labelControl1.Appearance.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.labelControl1.Location = new System.Drawing.Point(75, 27);
            this.labelControl1.Margin = new System.Windows.Forms.Padding(2);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(256, 19);
            this.labelControl1.TabIndex = 12;
            this.labelControl1.Text = "请认真核对信息，设置后不能修改！";
            // 
            // tab
            // 
            this.tab.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tab.Location = new System.Drawing.Point(0, 0);
            this.tab.Name = "tab";
            this.tab.SelectedTabPage = this.xtraTabPage1;
            this.tab.Size = new System.Drawing.Size(402, 270);
            this.tab.TabIndex = 13;
            this.tab.TabPages.AddRange(new DevExpress.XtraTab.XtraTabPage[] {
            this.xtraTabPage1,
            this.xtraTabPage2});
            this.tab.SelectedPageChanged += new DevExpress.XtraTab.TabPageChangedEventHandler(this.tab_SelectedPageChanged);
            // 
            // xtraTabPage1
            // 
            this.xtraTabPage1.Controls.Add(this.memoKey);
            this.xtraTabPage1.Controls.Add(this.labelControl23);
            this.xtraTabPage1.Controls.Add(this.btnNext);
            this.xtraTabPage1.Controls.Add(this.btnCancel);
            this.xtraTabPage1.Controls.Add(this.labelControl1);
            this.xtraTabPage1.Controls.Add(this.txtSID);
            this.xtraTabPage1.Controls.Add(this.账套);
            this.xtraTabPage1.Name = "xtraTabPage1";
            this.xtraTabPage1.Size = new System.Drawing.Size(396, 241);
            this.xtraTabPage1.Text = "xtraTabPage1";
            // 
            // btnNext
            // 
            this.btnNext.Appearance.Font = new System.Drawing.Font("Tahoma", 12F);
            this.btnNext.Appearance.Options.UseFont = true;
            this.btnNext.Location = new System.Drawing.Point(236, 188);
            this.btnNext.Margin = new System.Windows.Forms.Padding(2);
            this.btnNext.Name = "btnNext";
            this.btnNext.Size = new System.Drawing.Size(91, 34);
            this.btnNext.TabIndex = 13;
            this.btnNext.Text = "下一步";
            this.btnNext.Click += new System.EventHandler(this.btnNext_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Appearance.Font = new System.Drawing.Font("Tahoma", 12F);
            this.btnCancel.Appearance.Options.UseFont = true;
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(75, 188);
            this.btnCancel.Margin = new System.Windows.Forms.Padding(2);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(91, 34);
            this.btnCancel.TabIndex = 14;
            this.btnCancel.Text = "取消";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // xtraTabPage2
            // 
            this.xtraTabPage2.Controls.Add(this.labelControl2);
            this.xtraTabPage2.Controls.Add(this.labelControl3);
            this.xtraTabPage2.Controls.Add(this.btnConfirm);
            this.xtraTabPage2.Controls.Add(this.btnPre);
            this.xtraTabPage2.Controls.Add(this.lueDepartment);
            this.xtraTabPage2.Name = "xtraTabPage2";
            this.xtraTabPage2.Size = new System.Drawing.Size(396, 241);
            this.xtraTabPage2.Text = "xtraTabPage2";
            // 
            // labelControl2
            // 
            this.labelControl2.Appearance.Font = new System.Drawing.Font("Tahoma", 12F);
            this.labelControl2.Appearance.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.labelControl2.Location = new System.Drawing.Point(73, 25);
            this.labelControl2.Margin = new System.Windows.Forms.Padding(2);
            this.labelControl2.Name = "labelControl2";
            this.labelControl2.Size = new System.Drawing.Size(256, 19);
            this.labelControl2.TabIndex = 13;
            this.labelControl2.Text = "请认真核对信息，设置后不能修改！";
            // 
            // labelControl23
            // 
            this.labelControl23.Appearance.Font = new System.Drawing.Font("Tahoma", 12F);
            this.labelControl23.Location = new System.Drawing.Point(70, 100);
            this.labelControl23.Margin = new System.Windows.Forms.Padding(2);
            this.labelControl23.Name = "labelControl23";
            this.labelControl23.Size = new System.Drawing.Size(48, 19);
            this.labelControl23.TabIndex = 16;
            this.labelControl23.Text = "密钥：";
            // 
            // memoKey
            // 
            this.memoKey.Location = new System.Drawing.Point(123, 98);
            this.memoKey.Name = "memoKey";
            this.memoKey.Size = new System.Drawing.Size(199, 63);
            this.memoKey.TabIndex = 17;
            // 
            // FormRunSetting
            // 
            this.AcceptButton = this.btnConfirm;
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(402, 270);
            this.Controls.Add(this.tab);
            this.Name = "FormRunSetting";
            this.Text = "系统设置";
            ((System.ComponentModel.ISupportInitialize)(this.txtSID.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lueDepartment.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tab)).EndInit();
            this.tab.ResumeLayout(false);
            this.xtraTabPage1.ResumeLayout(false);
            this.xtraTabPage1.PerformLayout();
            this.xtraTabPage2.ResumeLayout(false);
            this.xtraTabPage2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.memoKey.Properties)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.SimpleButton btnConfirm;
        private DevExpress.XtraEditors.LabelControl labelControl3;
        private DevExpress.XtraEditors.LabelControl 账套;
        private DevExpress.XtraEditors.TextEdit txtSID;
        private DevExpress.XtraEditors.LookUpEdit lueDepartment;
        private DevExpress.XtraEditors.SimpleButton btnPre;
        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraTab.XtraTabControl tab;
        private DevExpress.XtraTab.XtraTabPage xtraTabPage1;
        private DevExpress.XtraTab.XtraTabPage xtraTabPage2;
        private DevExpress.XtraEditors.LabelControl labelControl2;
        private DevExpress.XtraEditors.SimpleButton btnNext;
        private DevExpress.XtraEditors.SimpleButton btnCancel;
        private DevExpress.XtraEditors.MemoEdit memoKey;
        private DevExpress.XtraEditors.LabelControl labelControl23;
    }
}