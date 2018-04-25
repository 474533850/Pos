namespace POS.Client
{
    partial class FormClientUpdate
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
            this.btnSave = new DevExpress.XtraEditors.SimpleButton();
            this.btnCancel = new DevExpress.XtraEditors.SimpleButton();
            this.metxnotes = new DevExpress.XtraEditors.MemoEdit();
            this.labelControl9 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl8 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl7 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl6 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl5 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl4 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl3 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl15 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl14 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl2 = new DevExpress.XtraEditors.LabelControl();
            this.txtxadd = new DevExpress.XtraEditors.TextEdit();
            this.textEdit9 = new DevExpress.XtraEditors.TextEdit();
            this.textEdit11 = new DevExpress.XtraEditors.TextEdit();
            this.txtxzhe = new DevExpress.XtraEditors.TextEdit();
            this.txtxpho = new DevExpress.XtraEditors.TextEdit();
            this.txtclntname = new DevExpress.XtraEditors.TextEdit();
            this.txtclntcode = new DevExpress.XtraEditors.TextEdit();
            this.lueclntclss = new DevExpress.XtraEditors.LookUpEdit();
            this.dtexbro = new DevExpress.XtraEditors.DateEdit();
            this.txtxintime = new DevExpress.XtraEditors.TextEdit();
            ((System.ComponentModel.ISupportInitialize)(this.metxnotes.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtxadd.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.textEdit9.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.textEdit11.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtxzhe.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtxpho.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtclntname.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtclntcode.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lueclntclss.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtexbro.Properties.CalendarTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtexbro.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtxintime.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // btnSave
            // 
            this.btnSave.Appearance.Font = new System.Drawing.Font("Tahoma", 12F);
            this.btnSave.Appearance.Options.UseFont = true;
            this.btnSave.Location = new System.Drawing.Point(576, 384);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(128, 38);
            this.btnSave.TabIndex = 13;
            this.btnSave.Text = "保存";
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Appearance.Font = new System.Drawing.Font("Tahoma", 12F);
            this.btnCancel.Appearance.Options.UseFont = true;
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(43, 384);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(128, 38);
            this.btnCancel.TabIndex = 12;
            this.btnCancel.Text = "取消";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // metxnotes
            // 
            this.metxnotes.Location = new System.Drawing.Point(129, 258);
            this.metxnotes.Name = "metxnotes";
            this.metxnotes.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 12F);
            this.metxnotes.Properties.Appearance.Options.UseFont = true;
            this.metxnotes.Properties.NullValuePrompt = "请填写会员备注";
            this.metxnotes.Properties.NullValuePromptShowForEmptyValue = true;
            this.metxnotes.Properties.ShowNullValuePromptWhenFocused = true;
            this.metxnotes.Size = new System.Drawing.Size(575, 50);
            this.metxnotes.TabIndex = 11;
            // 
            // labelControl9
            // 
            this.labelControl9.Appearance.Font = new System.Drawing.Font("Tahoma", 12F);
            this.labelControl9.Location = new System.Drawing.Point(75, 255);
            this.labelControl9.Name = "labelControl9";
            this.labelControl9.Size = new System.Drawing.Size(48, 19);
            this.labelControl9.TabIndex = 2;
            this.labelControl9.Text = "备注：";
            // 
            // labelControl8
            // 
            this.labelControl8.Appearance.Font = new System.Drawing.Font("Tahoma", 12F);
            this.labelControl8.Location = new System.Drawing.Point(43, 221);
            this.labelControl8.Name = "labelControl8";
            this.labelControl8.Size = new System.Drawing.Size(80, 19);
            this.labelControl8.TabIndex = 2;
            this.labelControl8.Text = "联系地址：";
            // 
            // labelControl7
            // 
            this.labelControl7.Appearance.Font = new System.Drawing.Font("Tahoma", 12F);
            this.labelControl7.Location = new System.Drawing.Point(43, 188);
            this.labelControl7.Name = "labelControl7";
            this.labelControl7.Size = new System.Drawing.Size(80, 19);
            this.labelControl7.TabIndex = 2;
            this.labelControl7.Text = "开卡日期：";
            // 
            // labelControl6
            // 
            this.labelControl6.Appearance.Font = new System.Drawing.Font("Tahoma", 12F);
            this.labelControl6.Location = new System.Drawing.Point(43, 157);
            this.labelControl6.Name = "labelControl6";
            this.labelControl6.Size = new System.Drawing.Size(80, 19);
            this.labelControl6.TabIndex = 2;
            this.labelControl6.Text = "会员生日：";
            // 
            // labelControl5
            // 
            this.labelControl5.Appearance.Font = new System.Drawing.Font("Tahoma", 12F);
            this.labelControl5.Location = new System.Drawing.Point(378, 193);
            this.labelControl5.Name = "labelControl5";
            this.labelControl5.Size = new System.Drawing.Size(80, 19);
            this.labelControl5.TabIndex = 2;
            this.labelControl5.Text = "会员折扣：";
            this.labelControl5.Visible = false;
            // 
            // labelControl4
            // 
            this.labelControl4.Appearance.Font = new System.Drawing.Font("Tahoma", 12F);
            this.labelControl4.Location = new System.Drawing.Point(43, 124);
            this.labelControl4.Name = "labelControl4";
            this.labelControl4.Size = new System.Drawing.Size(80, 19);
            this.labelControl4.TabIndex = 2;
            this.labelControl4.Text = "会员等级：";
            // 
            // labelControl3
            // 
            this.labelControl3.Appearance.Font = new System.Drawing.Font("Tahoma", 12F);
            this.labelControl3.Location = new System.Drawing.Point(75, 91);
            this.labelControl3.Name = "labelControl3";
            this.labelControl3.Size = new System.Drawing.Size(48, 19);
            this.labelControl3.TabIndex = 2;
            this.labelControl3.Text = "电话：";
            // 
            // labelControl1
            // 
            this.labelControl1.Appearance.Font = new System.Drawing.Font("Tahoma", 12F);
            this.labelControl1.Location = new System.Drawing.Point(75, 58);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(48, 19);
            this.labelControl1.TabIndex = 2;
            this.labelControl1.Text = "姓名：";
            // 
            // labelControl15
            // 
            this.labelControl15.Appearance.Font = new System.Drawing.Font("Tahoma", 12F);
            this.labelControl15.Location = new System.Drawing.Point(378, 157);
            this.labelControl15.Name = "labelControl15";
            this.labelControl15.Size = new System.Drawing.Size(80, 19);
            this.labelControl15.TabIndex = 2;
            this.labelControl15.Text = "能否赊账：";
            this.labelControl15.Visible = false;
            // 
            // labelControl14
            // 
            this.labelControl14.Appearance.Font = new System.Drawing.Font("Tahoma", 12F);
            this.labelControl14.Location = new System.Drawing.Point(394, 124);
            this.labelControl14.Name = "labelControl14";
            this.labelControl14.Size = new System.Drawing.Size(64, 19);
            this.labelControl14.TabIndex = 2;
            this.labelControl14.Text = "购物卡：";
            this.labelControl14.Visible = false;
            // 
            // labelControl2
            // 
            this.labelControl2.Appearance.Font = new System.Drawing.Font("Tahoma", 12F);
            this.labelControl2.Location = new System.Drawing.Point(75, 25);
            this.labelControl2.Name = "labelControl2";
            this.labelControl2.Size = new System.Drawing.Size(48, 19);
            this.labelControl2.TabIndex = 2;
            this.labelControl2.Text = "卡号：";
            // 
            // txtxadd
            // 
            this.txtxadd.Location = new System.Drawing.Point(129, 223);
            this.txtxadd.Name = "txtxadd";
            this.txtxadd.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 12F);
            this.txtxadd.Properties.Appearance.Options.UseFont = true;
            this.txtxadd.Properties.NullValuePrompt = "请填写联系地址";
            this.txtxadd.Properties.NullValuePromptShowForEmptyValue = true;
            this.txtxadd.Properties.ShowNullValuePromptWhenFocused = true;
            this.txtxadd.Size = new System.Drawing.Size(575, 26);
            this.txtxadd.TabIndex = 10;
            // 
            // textEdit9
            // 
            this.textEdit9.Location = new System.Drawing.Point(464, 122);
            this.textEdit9.Name = "textEdit9";
            this.textEdit9.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 12F);
            this.textEdit9.Properties.Appearance.Options.UseFont = true;
            this.textEdit9.Properties.ReadOnly = true;
            this.textEdit9.Size = new System.Drawing.Size(240, 26);
            this.textEdit9.TabIndex = 4;
            this.textEdit9.Visible = false;
            // 
            // textEdit11
            // 
            this.textEdit11.Location = new System.Drawing.Point(464, 156);
            this.textEdit11.Name = "textEdit11";
            this.textEdit11.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 12F);
            this.textEdit11.Properties.Appearance.Options.UseFont = true;
            this.textEdit11.Properties.ReadOnly = true;
            this.textEdit11.Size = new System.Drawing.Size(240, 26);
            this.textEdit11.TabIndex = 6;
            this.textEdit11.Visible = false;
            // 
            // txtxzhe
            // 
            this.txtxzhe.Location = new System.Drawing.Point(464, 192);
            this.txtxzhe.Name = "txtxzhe";
            this.txtxzhe.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 12F);
            this.txtxzhe.Properties.Appearance.Options.UseFont = true;
            this.txtxzhe.Properties.Mask.EditMask = "P";
            this.txtxzhe.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric;
            this.txtxzhe.Properties.ReadOnly = true;
            this.txtxzhe.Size = new System.Drawing.Size(240, 26);
            this.txtxzhe.TabIndex = 5;
            this.txtxzhe.Visible = false;
            // 
            // txtxpho
            // 
            this.txtxpho.Location = new System.Drawing.Point(129, 90);
            this.txtxpho.Name = "txtxpho";
            this.txtxpho.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 12F);
            this.txtxpho.Properties.Appearance.Options.UseFont = true;
            this.txtxpho.Size = new System.Drawing.Size(240, 26);
            this.txtxpho.TabIndex = 2;
            // 
            // txtclntname
            // 
            this.txtclntname.Location = new System.Drawing.Point(129, 57);
            this.txtclntname.Name = "txtclntname";
            this.txtclntname.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 12F);
            this.txtclntname.Properties.Appearance.Options.UseFont = true;
            this.txtclntname.Size = new System.Drawing.Size(240, 26);
            this.txtclntname.TabIndex = 1;
            // 
            // txtclntcode
            // 
            this.txtclntcode.Location = new System.Drawing.Point(129, 23);
            this.txtclntcode.Name = "txtclntcode";
            this.txtclntcode.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 12F);
            this.txtclntcode.Properties.Appearance.Options.UseFont = true;
            this.txtclntcode.Properties.ReadOnly = true;
            this.txtclntcode.Size = new System.Drawing.Size(240, 26);
            this.txtclntcode.TabIndex = 0;
            // 
            // lueclntclss
            // 
            this.lueclntclss.Location = new System.Drawing.Point(129, 123);
            this.lueclntclss.Name = "lueclntclss";
            this.lueclntclss.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 12F);
            this.lueclntclss.Properties.Appearance.Options.UseFont = true;
            this.lueclntclss.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.lueclntclss.Properties.Columns.AddRange(new DevExpress.XtraEditors.Controls.LookUpColumnInfo[] {
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("clntclss", "名称")});
            this.lueclntclss.Properties.DisplayMember = "clntclss";
            this.lueclntclss.Properties.NullText = "";
            this.lueclntclss.Properties.ReadOnly = true;
            this.lueclntclss.Properties.ValueMember = "clntclss";
            this.lueclntclss.Size = new System.Drawing.Size(240, 26);
            this.lueclntclss.TabIndex = 3;
            // 
            // dtexbro
            // 
            this.dtexbro.EditValue = null;
            this.dtexbro.Location = new System.Drawing.Point(129, 156);
            this.dtexbro.Name = "dtexbro";
            this.dtexbro.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 12F);
            this.dtexbro.Properties.Appearance.Options.UseFont = true;
            this.dtexbro.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.dtexbro.Properties.CalendarTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.dtexbro.Properties.DisplayFormat.FormatString = "m";
            this.dtexbro.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            this.dtexbro.Properties.EditFormat.FormatString = "m";
            this.dtexbro.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            this.dtexbro.Properties.Mask.EditMask = "";
            this.dtexbro.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.None;
            this.dtexbro.Size = new System.Drawing.Size(240, 26);
            this.dtexbro.TabIndex = 7;
            // 
            // txtxintime
            // 
            this.txtxintime.Location = new System.Drawing.Point(129, 189);
            this.txtxintime.Name = "txtxintime";
            this.txtxintime.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 12F);
            this.txtxintime.Properties.Appearance.Options.UseFont = true;
            this.txtxintime.Properties.DisplayFormat.FormatString = "d";
            this.txtxintime.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            this.txtxintime.Properties.EditFormat.FormatString = "d";
            this.txtxintime.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            this.txtxintime.Properties.ReadOnly = true;
            this.txtxintime.Size = new System.Drawing.Size(240, 26);
            this.txtxintime.TabIndex = 14;
            // 
            // FormClientUpdate
            // 
            this.AcceptButton = this.btnSave;
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(746, 442);
            this.Controls.Add(this.txtxintime);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.metxnotes);
            this.Controls.Add(this.labelControl9);
            this.Controls.Add(this.labelControl8);
            this.Controls.Add(this.labelControl7);
            this.Controls.Add(this.labelControl6);
            this.Controls.Add(this.labelControl5);
            this.Controls.Add(this.labelControl4);
            this.Controls.Add(this.labelControl3);
            this.Controls.Add(this.labelControl1);
            this.Controls.Add(this.labelControl15);
            this.Controls.Add(this.labelControl14);
            this.Controls.Add(this.labelControl2);
            this.Controls.Add(this.txtxadd);
            this.Controls.Add(this.textEdit9);
            this.Controls.Add(this.textEdit11);
            this.Controls.Add(this.txtxzhe);
            this.Controls.Add(this.txtxpho);
            this.Controls.Add(this.txtclntname);
            this.Controls.Add(this.txtclntcode);
            this.Controls.Add(this.lueclntclss);
            this.Controls.Add(this.dtexbro);
            this.Name = "FormClientUpdate";
            this.Text = "编辑会员";
            this.Load += new System.EventHandler(this.FormClientUpdate_Load);
            ((System.ComponentModel.ISupportInitialize)(this.metxnotes.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtxadd.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.textEdit9.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.textEdit11.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtxzhe.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtxpho.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtclntname.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtclntcode.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lueclntclss.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtexbro.Properties.CalendarTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtexbro.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtxintime.Properties)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraEditors.LabelControl labelControl2;
        private DevExpress.XtraEditors.TextEdit txtclntcode;
        private DevExpress.XtraEditors.TextEdit txtclntname;
        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraEditors.TextEdit txtxpho;
        private DevExpress.XtraEditors.LabelControl labelControl3;
        private DevExpress.XtraEditors.LabelControl labelControl4;
        private DevExpress.XtraEditors.TextEdit txtxzhe;
        private DevExpress.XtraEditors.LabelControl labelControl5;
        private DevExpress.XtraEditors.LabelControl labelControl6;
        private DevExpress.XtraEditors.LabelControl labelControl7;
        private DevExpress.XtraEditors.TextEdit txtxadd;
        private DevExpress.XtraEditors.LabelControl labelControl8;
        private DevExpress.XtraEditors.LabelControl labelControl9;
        private DevExpress.XtraEditors.MemoEdit metxnotes;
        private DevExpress.XtraEditors.LabelControl labelControl14;
        private DevExpress.XtraEditors.LabelControl labelControl15;
        private DevExpress.XtraEditors.TextEdit textEdit11;
        private DevExpress.XtraEditors.SimpleButton btnCancel;
        private DevExpress.XtraEditors.SimpleButton btnSave;
        private DevExpress.XtraEditors.LookUpEdit lueclntclss;
        private DevExpress.XtraEditors.DateEdit dtexbro;
        private DevExpress.XtraEditors.TextEdit textEdit9;
        private DevExpress.XtraEditors.TextEdit txtxintime;
    }
}