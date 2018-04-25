namespace POS.Sale
{
    partial class FormProductQuery
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormProductQuery));
            DevExpress.Utils.SerializableAppearanceObject serializableAppearanceObject1 = new DevExpress.Utils.SerializableAppearanceObject();
            DevExpress.Utils.SerializableAppearanceObject serializableAppearanceObject2 = new DevExpress.Utils.SerializableAppearanceObject();
            this.panelControl1 = new DevExpress.XtraEditors.PanelControl();
            this.btnQuery = new DevExpress.XtraEditors.SimpleButton();
            this.bteSearch = new DevExpress.XtraEditors.ButtonEdit();
            this.panelControl2 = new DevExpress.XtraEditors.PanelControl();
            this.chkShowZero = new DevExpress.XtraEditors.CheckEdit();
            this.btnJoin = new DevExpress.XtraEditors.SimpleButton();
            this.btnCancel = new DevExpress.XtraEditors.SimpleButton();
            this.gd = new DevExpress.XtraGrid.GridControl();
            this.bds = new System.Windows.Forms.BindingSource(this.components);
            this.gv = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.gridColumn1 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn2 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn3 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn5 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colxquatku = new DevExpress.XtraGrid.Columns.GridColumn();
            this.rluexquatku = new DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit();
            this.gridColumn4 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colRefresh = new DevExpress.XtraGrid.Columns.GridColumn();
            this.rpicRefresh = new DevExpress.XtraEditors.Repository.RepositoryItemPictureEdit();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.tsmColumnsSetting = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmRestore = new System.Windows.Forms.ToolStripMenuItem();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).BeginInit();
            this.panelControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.bteSearch.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl2)).BeginInit();
            this.panelControl2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chkShowZero.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gd)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bds)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gv)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rluexquatku)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rpicRefresh)).BeginInit();
            this.contextMenuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelControl1
            // 
            this.panelControl1.Controls.Add(this.btnQuery);
            this.panelControl1.Controls.Add(this.bteSearch);
            this.panelControl1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelControl1.Location = new System.Drawing.Point(0, 0);
            this.panelControl1.Name = "panelControl1";
            this.panelControl1.Size = new System.Drawing.Size(649, 60);
            this.panelControl1.TabIndex = 0;
            // 
            // btnQuery
            // 
            this.btnQuery.Appearance.Font = new System.Drawing.Font("Tahoma", 12F);
            this.btnQuery.Appearance.Options.UseFont = true;
            this.btnQuery.Location = new System.Drawing.Point(499, 12);
            this.btnQuery.Name = "btnQuery";
            this.btnQuery.Size = new System.Drawing.Size(137, 38);
            this.btnQuery.TabIndex = 2;
            this.btnQuery.Text = "搜索";
            this.btnQuery.Click += new System.EventHandler(this.btnQuery_Click);
            // 
            // bteSearch
            // 
            this.bteSearch.Location = new System.Drawing.Point(12, 12);
            this.bteSearch.Name = "bteSearch";
            this.bteSearch.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 12F);
            this.bteSearch.Properties.Appearance.Options.UseFont = true;
            this.bteSearch.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Glyph, "", -1, true, true, false, DevExpress.XtraEditors.ImageLocation.MiddleCenter, ((System.Drawing.Image)(resources.GetObject("bteSearch.Properties.Buttons"))), new DevExpress.Utils.KeyShortcut(System.Windows.Forms.Keys.None), serializableAppearanceObject1, "", null, null, true),
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Glyph, "", -1, true, true, true, DevExpress.XtraEditors.ImageLocation.MiddleCenter, ((System.Drawing.Image)(resources.GetObject("bteSearch.Properties.Buttons1"))), new DevExpress.Utils.KeyShortcut(System.Windows.Forms.Keys.None), serializableAppearanceObject2, "", null, null, true)});
            this.bteSearch.Properties.NullValuePrompt = "条码/拼音码/品名";
            this.bteSearch.Properties.NullValuePromptShowForEmptyValue = true;
            this.bteSearch.Properties.ShowNullValuePromptWhenFocused = true;
            this.bteSearch.Size = new System.Drawing.Size(478, 38);
            this.bteSearch.TabIndex = 1;
            this.bteSearch.ButtonClick += new DevExpress.XtraEditors.Controls.ButtonPressedEventHandler(this.bteSearch_ButtonClick);
            this.bteSearch.EditValueChanged += new System.EventHandler(this.bteSearch_EditValueChanged);
            // 
            // panelControl2
            // 
            this.panelControl2.Controls.Add(this.chkShowZero);
            this.panelControl2.Controls.Add(this.btnJoin);
            this.panelControl2.Controls.Add(this.btnCancel);
            this.panelControl2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelControl2.Location = new System.Drawing.Point(0, 459);
            this.panelControl2.Name = "panelControl2";
            this.panelControl2.Size = new System.Drawing.Size(649, 60);
            this.panelControl2.TabIndex = 0;
            // 
            // chkShowZero
            // 
            this.chkShowZero.Location = new System.Drawing.Point(12, 19);
            this.chkShowZero.Name = "chkShowZero";
            this.chkShowZero.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 12F);
            this.chkShowZero.Properties.Appearance.Options.UseFont = true;
            this.chkShowZero.Properties.Caption = "显示为零的库存";
            this.chkShowZero.Size = new System.Drawing.Size(148, 23);
            this.chkShowZero.TabIndex = 10;
            this.chkShowZero.CheckedChanged += new System.EventHandler(this.chkShowZero_CheckedChanged);
            // 
            // btnJoin
            // 
            this.btnJoin.Appearance.Font = new System.Drawing.Font("Tahoma", 12F);
            this.btnJoin.Appearance.Options.UseFont = true;
            this.btnJoin.Location = new System.Drawing.Point(496, 13);
            this.btnJoin.Name = "btnJoin";
            this.btnJoin.Size = new System.Drawing.Size(141, 35);
            this.btnJoin.TabIndex = 1;
            this.btnJoin.Text = "加入";
            this.btnJoin.Click += new System.EventHandler(this.btnJoin_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Appearance.Font = new System.Drawing.Font("Tahoma", 12F);
            this.btnCancel.Appearance.Options.UseFont = true;
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(403, 13);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 35);
            this.btnCancel.TabIndex = 2;
            this.btnCancel.Text = "取消";
            // 
            // gd
            // 
            this.gd.DataSource = this.bds;
            this.gd.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gd.EmbeddedNavigator.Margin = new System.Windows.Forms.Padding(2);
            this.gd.Location = new System.Drawing.Point(0, 60);
            this.gd.MainView = this.gv;
            this.gd.Margin = new System.Windows.Forms.Padding(2);
            this.gd.Name = "gd";
            this.gd.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.rluexquatku,
            this.rpicRefresh});
            this.gd.Size = new System.Drawing.Size(649, 399);
            this.gd.TabIndex = 7;
            this.gd.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gv});
            // 
            // gv
            // 
            this.gv.ColumnPanelRowHeight = 35;
            this.gv.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.gridColumn1,
            this.gridColumn2,
            this.gridColumn3,
            this.gridColumn5,
            this.colxquatku,
            this.gridColumn4,
            this.colRefresh});
            this.gv.GridControl = this.gd;
            this.gv.Name = "gv";
            this.gv.OptionsBehavior.Editable = false;
            this.gv.OptionsCustomization.AllowFilter = false;
            this.gv.OptionsCustomization.AllowSort = false;
            this.gv.OptionsMenu.EnableColumnMenu = false;
            this.gv.OptionsMenu.EnableFooterMenu = false;
            this.gv.OptionsMenu.EnableGroupPanelMenu = false;
            this.gv.OptionsMenu.ShowAutoFilterRowItem = false;
            this.gv.OptionsView.EnableAppearanceEvenRow = true;
            this.gv.OptionsView.RowAutoHeight = true;
            this.gv.OptionsView.ShowGroupPanel = false;
            this.gv.RowHeight = 40;
            this.gv.RowClick += new DevExpress.XtraGrid.Views.Grid.RowClickEventHandler(this.gv_RowClick);
            this.gv.RowCellClick += new DevExpress.XtraGrid.Views.Grid.RowCellClickEventHandler(this.gv_RowCellClick);
            this.gv.CustomUnboundColumnData += new DevExpress.XtraGrid.Views.Base.CustomColumnDataEventHandler(this.gv_CustomUnboundColumnData);
            this.gv.CustomColumnDisplayText += new DevExpress.XtraGrid.Views.Base.CustomColumnDisplayTextEventHandler(this.gv_CustomColumnDisplayText);
            // 
            // gridColumn1
            // 
            this.gridColumn1.AppearanceCell.Options.UseTextOptions = true;
            this.gridColumn1.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumn1.AppearanceHeader.Options.UseTextOptions = true;
            this.gridColumn1.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumn1.Caption = "商品名称";
            this.gridColumn1.FieldName = "goodname";
            this.gridColumn1.Name = "gridColumn1";
            this.gridColumn1.OptionsColumn.AllowEdit = false;
            this.gridColumn1.SortMode = DevExpress.XtraGrid.ColumnSortMode.DisplayText;
            this.gridColumn1.Visible = true;
            this.gridColumn1.VisibleIndex = 1;
            this.gridColumn1.Width = 139;
            // 
            // gridColumn2
            // 
            this.gridColumn2.AppearanceCell.Options.UseTextOptions = true;
            this.gridColumn2.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumn2.AppearanceHeader.Options.UseTextOptions = true;
            this.gridColumn2.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumn2.Caption = "商品条码";
            this.gridColumn2.FieldName = "xbarcode";
            this.gridColumn2.Name = "gridColumn2";
            this.gridColumn2.OptionsColumn.AllowEdit = false;
            this.gridColumn2.SortMode = DevExpress.XtraGrid.ColumnSortMode.DisplayText;
            this.gridColumn2.Visible = true;
            this.gridColumn2.VisibleIndex = 0;
            this.gridColumn2.Width = 116;
            // 
            // gridColumn3
            // 
            this.gridColumn3.AppearanceCell.Options.UseTextOptions = true;
            this.gridColumn3.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumn3.AppearanceHeader.Options.UseTextOptions = true;
            this.gridColumn3.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumn3.Caption = "价格";
            this.gridColumn3.FieldName = "xprico";
            this.gridColumn3.Name = "gridColumn3";
            this.gridColumn3.OptionsColumn.AllowEdit = false;
            this.gridColumn3.SortMode = DevExpress.XtraGrid.ColumnSortMode.DisplayText;
            this.gridColumn3.Visible = true;
            this.gridColumn3.VisibleIndex = 2;
            this.gridColumn3.Width = 104;
            // 
            // gridColumn5
            // 
            this.gridColumn5.AppearanceCell.Options.UseTextOptions = true;
            this.gridColumn5.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumn5.AppearanceHeader.Options.UseTextOptions = true;
            this.gridColumn5.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumn5.Caption = "单位";
            this.gridColumn5.FieldName = "unitname";
            this.gridColumn5.Name = "gridColumn5";
            this.gridColumn5.OptionsColumn.AllowEdit = false;
            this.gridColumn5.SortMode = DevExpress.XtraGrid.ColumnSortMode.DisplayText;
            this.gridColumn5.Visible = true;
            this.gridColumn5.VisibleIndex = 3;
            // 
            // colxquatku
            // 
            this.colxquatku.AppearanceCell.Options.UseTextOptions = true;
            this.colxquatku.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.colxquatku.AppearanceHeader.Options.UseTextOptions = true;
            this.colxquatku.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.colxquatku.Caption = "当前库存";
            this.colxquatku.ColumnEdit = this.rluexquatku;
            this.colxquatku.FieldName = "key";
            this.colxquatku.Name = "colxquatku";
            this.colxquatku.OptionsColumn.AllowEdit = false;
            this.colxquatku.OptionsColumn.AllowSort = DevExpress.Utils.DefaultBoolean.False;
            this.colxquatku.SortMode = DevExpress.XtraGrid.ColumnSortMode.DisplayText;
            this.colxquatku.Visible = true;
            this.colxquatku.VisibleIndex = 4;
            // 
            // rluexquatku
            // 
            this.rluexquatku.AutoHeight = false;
            this.rluexquatku.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.rluexquatku.Columns.AddRange(new DevExpress.XtraEditors.Controls.LookUpColumnInfo[] {
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("xquatku", "数量")});
            this.rluexquatku.DisplayMember = "xquatku";
            this.rluexquatku.Name = "rluexquatku";
            this.rluexquatku.NullText = "";
            this.rluexquatku.ValueMember = "key";
            // 
            // gridColumn4
            // 
            this.gridColumn4.AppearanceCell.Options.UseTextOptions = true;
            this.gridColumn4.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumn4.AppearanceHeader.Options.UseTextOptions = true;
            this.gridColumn4.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumn4.Caption = "规格";
            this.gridColumn4.FieldName = "goodkind";
            this.gridColumn4.Name = "gridColumn4";
            this.gridColumn4.OptionsColumn.AllowEdit = false;
            this.gridColumn4.SortMode = DevExpress.XtraGrid.ColumnSortMode.DisplayText;
            this.gridColumn4.Visible = true;
            this.gridColumn4.VisibleIndex = 5;
            this.gridColumn4.Width = 163;
            // 
            // colRefresh
            // 
            this.colRefresh.AppearanceHeader.Options.UseTextOptions = true;
            this.colRefresh.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.colRefresh.Caption = " 操作";
            this.colRefresh.ColumnEdit = this.rpicRefresh;
            this.colRefresh.FieldName = "image";
            this.colRefresh.Name = "colRefresh";
            this.colRefresh.UnboundType = DevExpress.Data.UnboundColumnType.Object;
            this.colRefresh.Visible = true;
            this.colRefresh.VisibleIndex = 6;
            // 
            // rpicRefresh
            // 
            this.rpicRefresh.Name = "rpicRefresh";
            this.rpicRefresh.Click += new System.EventHandler(this.rpicRefresh_Click);
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Font = new System.Drawing.Font("微软雅黑", 10F);
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmColumnsSetting,
            this.tsmRestore});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(163, 52);
            // 
            // tsmColumnsSetting
            // 
            this.tsmColumnsSetting.Name = "tsmColumnsSetting";
            this.tsmColumnsSetting.Size = new System.Drawing.Size(162, 24);
            this.tsmColumnsSetting.Text = "列设置";
            this.tsmColumnsSetting.Click += new System.EventHandler(this.tsmColumnsSetting_Click);
            // 
            // tsmRestore
            // 
            this.tsmRestore.Name = "tsmRestore";
            this.tsmRestore.Size = new System.Drawing.Size(162, 24);
            this.tsmRestore.Text = "还原默认设置";
            this.tsmRestore.Click += new System.EventHandler(this.tsmRestore_Click);
            // 
            // FormProductQuery
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(649, 519);
            this.Controls.Add(this.gd);
            this.Controls.Add(this.panelControl2);
            this.Controls.Add(this.panelControl1);
            this.Name = "FormProductQuery";
            this.Text = "选择产品";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormProductQuery_FormClosing);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.FormProductQuery_KeyDown);
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).EndInit();
            this.panelControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.bteSearch.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl2)).EndInit();
            this.panelControl2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.chkShowZero.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gd)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bds)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gv)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rluexquatku)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rpicRefresh)).EndInit();
            this.contextMenuStrip1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.PanelControl panelControl1;
        private DevExpress.XtraEditors.PanelControl panelControl2;
        private DevExpress.XtraEditors.SimpleButton btnJoin;
        private DevExpress.XtraEditors.SimpleButton btnCancel;
        private DevExpress.XtraGrid.GridControl gd;
        private DevExpress.XtraGrid.Views.Grid.GridView gv;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn1;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn2;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn3;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn4;
        private DevExpress.XtraEditors.SimpleButton btnQuery;
        private DevExpress.XtraEditors.ButtonEdit bteSearch;
        private System.Windows.Forms.BindingSource bds;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn5;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem tsmColumnsSetting;
        private System.Windows.Forms.ToolStripMenuItem tsmRestore;
        private DevExpress.XtraGrid.Columns.GridColumn colxquatku;
        private DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit rluexquatku;
        private DevExpress.XtraGrid.Columns.GridColumn colRefresh;
        private DevExpress.XtraEditors.Repository.RepositoryItemPictureEdit rpicRefresh;
        private DevExpress.XtraEditors.CheckEdit chkShowZero;
    }
}