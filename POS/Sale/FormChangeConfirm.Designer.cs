namespace POS.Sale
{
    partial class FormChangeConfirm
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
            DevExpress.XtraGrid.GridLevelNode gridLevelNode2 = new DevExpress.XtraGrid.GridLevelNode();
            this.panelControl1 = new DevExpress.XtraEditors.PanelControl();
            this.gridControl1 = new DevExpress.XtraGrid.GridControl();
            this.gv = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.colxbarcode = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn15 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn16 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colunitname = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn18 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colxzhe = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colunitquat = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colxpric = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colxallp = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colxsalesid = new DevExpress.XtraGrid.Columns.GridColumn();
            this.rlueSale = new DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit();
            this.repositoryItemTextEdit2 = new DevExpress.XtraEditors.Repository.RepositoryItemTextEdit();
            this.btnConfirm = new DevExpress.XtraEditors.SimpleButton();
            this.btnCancel = new DevExpress.XtraEditors.SimpleButton();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).BeginInit();
            this.panelControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridControl1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gv)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rlueSale)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemTextEdit2)).BeginInit();
            this.SuspendLayout();
            // 
            // panelControl1
            // 
            this.panelControl1.Controls.Add(this.btnConfirm);
            this.panelControl1.Controls.Add(this.btnCancel);
            this.panelControl1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelControl1.Location = new System.Drawing.Point(0, 388);
            this.panelControl1.Name = "panelControl1";
            this.panelControl1.Size = new System.Drawing.Size(985, 73);
            this.panelControl1.TabIndex = 0;
            // 
            // gridControl1
            // 
            this.gridControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridControl1.EmbeddedNavigator.Margin = new System.Windows.Forms.Padding(2);
            gridLevelNode2.RelationName = "Level1";
            this.gridControl1.LevelTree.Nodes.AddRange(new DevExpress.XtraGrid.GridLevelNode[] {
            gridLevelNode2});
            this.gridControl1.Location = new System.Drawing.Point(0, 0);
            this.gridControl1.MainView = this.gv;
            this.gridControl1.Margin = new System.Windows.Forms.Padding(2);
            this.gridControl1.Name = "gridControl1";
            this.gridControl1.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.repositoryItemTextEdit2,
            this.rlueSale});
            this.gridControl1.Size = new System.Drawing.Size(985, 388);
            this.gridControl1.TabIndex = 27;
            this.gridControl1.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gv});
            // 
            // gv
            // 
            this.gv.ColumnPanelRowHeight = 35;
            this.gv.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.colxbarcode,
            this.gridColumn15,
            this.gridColumn16,
            this.colunitname,
            this.gridColumn18,
            this.colxzhe,
            this.colunitquat,
            this.colxpric,
            this.colxallp,
            this.colxsalesid});
            this.gv.GridControl = this.gridControl1;
            this.gv.Name = "gv";
            this.gv.OptionsCustomization.AllowFilter = false;
            this.gv.OptionsCustomization.AllowQuickHideColumns = false;
            this.gv.OptionsCustomization.AllowSort = false;
            this.gv.OptionsMenu.EnableColumnMenu = false;
            this.gv.OptionsMenu.EnableFooterMenu = false;
            this.gv.OptionsMenu.EnableGroupPanelMenu = false;
            this.gv.OptionsMenu.ShowAutoFilterRowItem = false;
            this.gv.OptionsView.ColumnHeaderAutoHeight = DevExpress.Utils.DefaultBoolean.True;
            this.gv.OptionsView.RowAutoHeight = true;
            this.gv.OptionsView.ShowGroupPanel = false;
            this.gv.RowHeight = 40;
            // 
            // colxbarcode
            // 
            this.colxbarcode.AppearanceCell.Font = new System.Drawing.Font("Tahoma", 12F);
            this.colxbarcode.AppearanceCell.Options.UseFont = true;
            this.colxbarcode.AppearanceCell.Options.UseTextOptions = true;
            this.colxbarcode.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.colxbarcode.AppearanceHeader.Font = new System.Drawing.Font("Tahoma", 12F);
            this.colxbarcode.AppearanceHeader.Options.UseFont = true;
            this.colxbarcode.AppearanceHeader.Options.UseTextOptions = true;
            this.colxbarcode.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.colxbarcode.Caption = "商品条码";
            this.colxbarcode.FieldName = "xbarcode";
            this.colxbarcode.Name = "colxbarcode";
            this.colxbarcode.OptionsColumn.AllowEdit = false;
            this.colxbarcode.Visible = true;
            this.colxbarcode.VisibleIndex = 0;
            this.colxbarcode.Width = 162;
            // 
            // gridColumn15
            // 
            this.gridColumn15.AppearanceCell.Font = new System.Drawing.Font("Tahoma", 12F);
            this.gridColumn15.AppearanceCell.Options.UseFont = true;
            this.gridColumn15.AppearanceCell.Options.UseTextOptions = true;
            this.gridColumn15.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumn15.AppearanceHeader.Font = new System.Drawing.Font("Tahoma", 12F);
            this.gridColumn15.AppearanceHeader.Options.UseFont = true;
            this.gridColumn15.AppearanceHeader.Options.UseTextOptions = true;
            this.gridColumn15.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumn15.Caption = "商品名称";
            this.gridColumn15.FieldName = "goodname";
            this.gridColumn15.Name = "gridColumn15";
            this.gridColumn15.OptionsColumn.AllowEdit = false;
            this.gridColumn15.Visible = true;
            this.gridColumn15.VisibleIndex = 1;
            this.gridColumn15.Width = 201;
            // 
            // gridColumn16
            // 
            this.gridColumn16.AppearanceCell.Font = new System.Drawing.Font("Tahoma", 12F);
            this.gridColumn16.AppearanceCell.Options.UseFont = true;
            this.gridColumn16.AppearanceCell.Options.UseTextOptions = true;
            this.gridColumn16.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumn16.AppearanceHeader.Font = new System.Drawing.Font("Tahoma", 12F);
            this.gridColumn16.AppearanceHeader.Options.UseFont = true;
            this.gridColumn16.AppearanceHeader.Options.UseTextOptions = true;
            this.gridColumn16.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumn16.Caption = "规格";
            this.gridColumn16.FieldName = "goodtm";
            this.gridColumn16.Name = "gridColumn16";
            this.gridColumn16.OptionsColumn.AllowEdit = false;
            this.gridColumn16.Visible = true;
            this.gridColumn16.VisibleIndex = 2;
            this.gridColumn16.Width = 135;
            // 
            // colunitname
            // 
            this.colunitname.AppearanceCell.Font = new System.Drawing.Font("Tahoma", 12F);
            this.colunitname.AppearanceCell.Options.UseFont = true;
            this.colunitname.AppearanceCell.Options.UseTextOptions = true;
            this.colunitname.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.colunitname.AppearanceHeader.Font = new System.Drawing.Font("Tahoma", 12F);
            this.colunitname.AppearanceHeader.Options.UseFont = true;
            this.colunitname.AppearanceHeader.Options.UseTextOptions = true;
            this.colunitname.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.colunitname.Caption = "单位";
            this.colunitname.FieldName = "unitname";
            this.colunitname.Name = "colunitname";
            this.colunitname.OptionsColumn.AllowEdit = false;
            this.colunitname.Visible = true;
            this.colunitname.VisibleIndex = 3;
            // 
            // gridColumn18
            // 
            this.gridColumn18.AppearanceCell.Font = new System.Drawing.Font("Tahoma", 12F);
            this.gridColumn18.AppearanceCell.Options.UseFont = true;
            this.gridColumn18.AppearanceCell.Options.UseTextOptions = true;
            this.gridColumn18.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumn18.AppearanceHeader.Font = new System.Drawing.Font("Tahoma", 12F);
            this.gridColumn18.AppearanceHeader.Options.UseFont = true;
            this.gridColumn18.AppearanceHeader.Options.UseTextOptions = true;
            this.gridColumn18.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumn18.Caption = "原价";
            this.gridColumn18.FieldName = "xpricold";
            this.gridColumn18.Name = "gridColumn18";
            this.gridColumn18.OptionsColumn.AllowEdit = false;
            this.gridColumn18.Visible = true;
            this.gridColumn18.VisibleIndex = 4;
            this.gridColumn18.Width = 99;
            // 
            // colxzhe
            // 
            this.colxzhe.AppearanceCell.Font = new System.Drawing.Font("Tahoma", 12F);
            this.colxzhe.AppearanceCell.Options.UseFont = true;
            this.colxzhe.AppearanceCell.Options.UseTextOptions = true;
            this.colxzhe.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.colxzhe.AppearanceHeader.Font = new System.Drawing.Font("Tahoma", 12F);
            this.colxzhe.AppearanceHeader.Options.UseFont = true;
            this.colxzhe.AppearanceHeader.Options.UseTextOptions = true;
            this.colxzhe.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.colxzhe.Caption = "折扣";
            this.colxzhe.FieldName = "xzhe";
            this.colxzhe.Name = "colxzhe";
            this.colxzhe.OptionsColumn.AllowEdit = false;
            this.colxzhe.Visible = true;
            this.colxzhe.VisibleIndex = 5;
            this.colxzhe.Width = 99;
            // 
            // colunitquat
            // 
            this.colunitquat.AppearanceCell.Font = new System.Drawing.Font("Tahoma", 12F);
            this.colunitquat.AppearanceCell.Options.UseFont = true;
            this.colunitquat.AppearanceCell.Options.UseTextOptions = true;
            this.colunitquat.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.colunitquat.AppearanceHeader.Font = new System.Drawing.Font("Tahoma", 12F);
            this.colunitquat.AppearanceHeader.Options.UseFont = true;
            this.colunitquat.AppearanceHeader.Options.UseTextOptions = true;
            this.colunitquat.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.colunitquat.Caption = "数量";
            this.colunitquat.FieldName = "unitquat";
            this.colunitquat.Name = "colunitquat";
            this.colunitquat.Summary.AddRange(new DevExpress.XtraGrid.GridSummaryItem[] {
            new DevExpress.XtraGrid.GridColumnSummaryItem(DevExpress.Data.SummaryItemType.Sum, "unitquat", "{0:0.##}")});
            this.colunitquat.Visible = true;
            this.colunitquat.VisibleIndex = 6;
            // 
            // colxpric
            // 
            this.colxpric.AppearanceCell.Font = new System.Drawing.Font("Tahoma", 12F);
            this.colxpric.AppearanceCell.Options.UseFont = true;
            this.colxpric.AppearanceCell.Options.UseTextOptions = true;
            this.colxpric.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.colxpric.AppearanceHeader.Font = new System.Drawing.Font("Tahoma", 12F);
            this.colxpric.AppearanceHeader.Options.UseFont = true;
            this.colxpric.AppearanceHeader.Options.UseTextOptions = true;
            this.colxpric.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.colxpric.Caption = "现价";
            this.colxpric.DisplayFormat.FormatString = "0.###";
            this.colxpric.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.colxpric.FieldName = "xpric";
            this.colxpric.Name = "colxpric";
            this.colxpric.Visible = true;
            this.colxpric.VisibleIndex = 7;
            this.colxpric.Width = 92;
            // 
            // colxallp
            // 
            this.colxallp.AppearanceCell.Font = new System.Drawing.Font("Tahoma", 12F);
            this.colxallp.AppearanceCell.Options.UseFont = true;
            this.colxallp.AppearanceCell.Options.UseTextOptions = true;
            this.colxallp.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.colxallp.AppearanceHeader.Font = new System.Drawing.Font("Tahoma", 12F);
            this.colxallp.AppearanceHeader.Options.UseFont = true;
            this.colxallp.AppearanceHeader.Options.UseTextOptions = true;
            this.colxallp.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.colxallp.Caption = "小计";
            this.colxallp.FieldName = "xallp";
            this.colxallp.Name = "colxallp";
            this.colxallp.OptionsColumn.AllowEdit = false;
            this.colxallp.Summary.AddRange(new DevExpress.XtraGrid.GridSummaryItem[] {
            new DevExpress.XtraGrid.GridColumnSummaryItem(DevExpress.Data.SummaryItemType.Sum, "xallp", "{0:0.##}")});
            this.colxallp.Visible = true;
            this.colxallp.VisibleIndex = 8;
            this.colxallp.Width = 99;
            // 
            // colxsalesid
            // 
            this.colxsalesid.AppearanceCell.Font = new System.Drawing.Font("Tahoma", 12F);
            this.colxsalesid.AppearanceCell.Options.UseFont = true;
            this.colxsalesid.AppearanceCell.Options.UseTextOptions = true;
            this.colxsalesid.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.colxsalesid.AppearanceHeader.Font = new System.Drawing.Font("Tahoma", 12F);
            this.colxsalesid.AppearanceHeader.Options.UseFont = true;
            this.colxsalesid.AppearanceHeader.Options.UseTextOptions = true;
            this.colxsalesid.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.colxsalesid.Caption = "活动名称";
            this.colxsalesid.ColumnEdit = this.rlueSale;
            this.colxsalesid.FieldName = "xsalesid";
            this.colxsalesid.Name = "colxsalesid";
            this.colxsalesid.Visible = true;
            this.colxsalesid.VisibleIndex = 9;
            this.colxsalesid.Width = 93;
            // 
            // rlueSale
            // 
            this.rlueSale.AutoHeight = false;
            this.rlueSale.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.rlueSale.Columns.AddRange(new DevExpress.XtraEditors.Controls.LookUpColumnInfo[] {
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("xname", "名称")});
            this.rlueSale.DisplayMember = "xname";
            this.rlueSale.Name = "rlueSale";
            this.rlueSale.NullText = "";
            this.rlueSale.ValueMember = "xtableid";
            // 
            // repositoryItemTextEdit2
            // 
            this.repositoryItemTextEdit2.AutoHeight = false;
            this.repositoryItemTextEdit2.DisplayFormat.FormatString = "0.##";
            this.repositoryItemTextEdit2.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.repositoryItemTextEdit2.EditFormat.FormatString = "0.##";
            this.repositoryItemTextEdit2.EditFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.repositoryItemTextEdit2.Mask.EditMask = "([1-9][0-9]{0,9})";
            this.repositoryItemTextEdit2.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.RegEx;
            this.repositoryItemTextEdit2.Name = "repositoryItemTextEdit2";
            // 
            // btnConfirm
            // 
            this.btnConfirm.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnConfirm.Appearance.Font = new System.Drawing.Font("Tahoma", 12F);
            this.btnConfirm.Appearance.Options.UseFont = true;
            this.btnConfirm.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnConfirm.Location = new System.Drawing.Point(814, 19);
            this.btnConfirm.Name = "btnConfirm";
            this.btnConfirm.Size = new System.Drawing.Size(131, 43);
            this.btnConfirm.TabIndex = 2;
            this.btnConfirm.Text = "确认";
            this.btnConfirm.Click += new System.EventHandler(this.btnConfirm_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.Appearance.Font = new System.Drawing.Font("Tahoma", 12F);
            this.btnCancel.Appearance.Options.UseFont = true;
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(645, 19);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(105, 43);
            this.btnCancel.TabIndex = 3;
            this.btnCancel.Text = "取消";
            // 
            // FormChangeConfirm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(985, 461);
            this.Controls.Add(this.gridControl1);
            this.Controls.Add(this.panelControl1);
            this.Name = "FormChangeConfirm";
            this.Text = "确认换货商品信息";
            this.Load += new System.EventHandler(this.FormChangeConfirm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).EndInit();
            this.panelControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridControl1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gv)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rlueSale)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemTextEdit2)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.PanelControl panelControl1;
        private DevExpress.XtraGrid.GridControl gridControl1;
        private DevExpress.XtraGrid.Views.Grid.GridView gv;
        private DevExpress.XtraGrid.Columns.GridColumn colxbarcode;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn15;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn16;
        private DevExpress.XtraGrid.Columns.GridColumn colunitname;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn18;
        private DevExpress.XtraGrid.Columns.GridColumn colxzhe;
        private DevExpress.XtraGrid.Columns.GridColumn colunitquat;
        private DevExpress.XtraGrid.Columns.GridColumn colxpric;
        private DevExpress.XtraGrid.Columns.GridColumn colxallp;
        private DevExpress.XtraGrid.Columns.GridColumn colxsalesid;
        private DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit rlueSale;
        private DevExpress.XtraEditors.Repository.RepositoryItemTextEdit repositoryItemTextEdit2;
        private DevExpress.XtraEditors.SimpleButton btnConfirm;
        private DevExpress.XtraEditors.SimpleButton btnCancel;
    }
}