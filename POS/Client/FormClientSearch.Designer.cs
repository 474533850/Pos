namespace POS.Client
{
    partial class FormClientSearch
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormClientSearch));
            DevExpress.Utils.SerializableAppearanceObject serializableAppearanceObject1 = new DevExpress.Utils.SerializableAppearanceObject();
            DevExpress.Utils.SerializableAppearanceObject serializableAppearanceObject2 = new DevExpress.Utils.SerializableAppearanceObject();
            this.bteSearch = new DevExpress.XtraEditors.ButtonEdit();
            this.bdsData = new System.Windows.Forms.BindingSource(this.components);
            this.gd = new DevExpress.XtraGrid.GridControl();
            this.gv = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.gridColumn1 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn2 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn3 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn4 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.rlueJF = new DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit();
            this.gridColumn5 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.rlueBlance = new DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit();
            this.gridColumn6 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colRefresh = new DevExpress.XtraGrid.Columns.GridColumn();
            this.rpicRefresh = new DevExpress.XtraEditors.Repository.RepositoryItemPictureEdit();
            ((System.ComponentModel.ISupportInitialize)(this.bteSearch.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bdsData)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gd)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gv)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rlueJF)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rlueBlance)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rpicRefresh)).BeginInit();
            this.SuspendLayout();
            // 
            // bteSearch
            // 
            this.bteSearch.Location = new System.Drawing.Point(14, 33);
            this.bteSearch.Name = "bteSearch";
            this.bteSearch.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 12F);
            this.bteSearch.Properties.Appearance.Options.UseFont = true;
            this.bteSearch.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Glyph, "", -1, true, true, false, DevExpress.XtraEditors.ImageLocation.MiddleCenter, ((System.Drawing.Image)(resources.GetObject("bteSearch.Properties.Buttons"))), new DevExpress.Utils.KeyShortcut(System.Windows.Forms.Keys.None), serializableAppearanceObject1, "", null, null, true),
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Glyph, "", -1, true, true, true, DevExpress.XtraEditors.ImageLocation.MiddleCenter, ((System.Drawing.Image)(resources.GetObject("bteSearch.Properties.Buttons1"))), new DevExpress.Utils.KeyShortcut(System.Windows.Forms.Keys.None), serializableAppearanceObject2, "", null, null, true)});
            this.bteSearch.Properties.NullValuePrompt = "输入卡号/手机/姓名搜索会员";
            this.bteSearch.Properties.NullValuePromptShowForEmptyValue = true;
            this.bteSearch.Properties.ShowNullValuePromptWhenFocused = true;
            this.bteSearch.Size = new System.Drawing.Size(533, 38);
            this.bteSearch.TabIndex = 0;
            this.bteSearch.ButtonClick += new DevExpress.XtraEditors.Controls.ButtonPressedEventHandler(this.btnEditSearch_ButtonClick);
            this.bteSearch.KeyDown += new System.Windows.Forms.KeyEventHandler(this.bteSearch_KeyDown);
            // 
            // gd
            // 
            this.gd.DataSource = this.bdsData;
            this.gd.EmbeddedNavigator.Margin = new System.Windows.Forms.Padding(2);
            this.gd.Location = new System.Drawing.Point(14, 76);
            this.gd.MainView = this.gv;
            this.gd.Margin = new System.Windows.Forms.Padding(2);
            this.gd.Name = "gd";
            this.gd.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.rlueJF,
            this.rlueBlance,
            this.rpicRefresh});
            this.gd.Size = new System.Drawing.Size(534, 299);
            this.gd.TabIndex = 8;
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
            this.gridColumn4,
            this.gridColumn5,
            this.gridColumn6,
            this.colRefresh});
            this.gv.GridControl = this.gd;
            this.gv.Name = "gv";
            this.gv.OptionsBehavior.Editable = false;
            this.gv.OptionsCustomization.AllowColumnMoving = false;
            this.gv.OptionsCustomization.AllowFilter = false;
            this.gv.OptionsCustomization.AllowQuickHideColumns = false;
            this.gv.OptionsMenu.EnableColumnMenu = false;
            this.gv.OptionsMenu.EnableFooterMenu = false;
            this.gv.OptionsMenu.EnableGroupPanelMenu = false;
            this.gv.OptionsMenu.ShowAutoFilterRowItem = false;
            this.gv.OptionsView.ColumnHeaderAutoHeight = DevExpress.Utils.DefaultBoolean.True;
            this.gv.OptionsView.RowAutoHeight = true;
            this.gv.OptionsView.ShowGroupPanel = false;
            this.gv.RowHeight = 40;
            this.gv.RowClick += new DevExpress.XtraGrid.Views.Grid.RowClickEventHandler(this.gv_RowClick);
            this.gv.CustomUnboundColumnData += new DevExpress.XtraGrid.Views.Base.CustomColumnDataEventHandler(this.gv_CustomUnboundColumnData);
            // 
            // gridColumn1
            // 
            this.gridColumn1.AppearanceCell.Options.UseTextOptions = true;
            this.gridColumn1.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumn1.AppearanceHeader.Options.UseTextOptions = true;
            this.gridColumn1.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumn1.Caption = "卡号";
            this.gridColumn1.FieldName = "clntcode";
            this.gridColumn1.Name = "gridColumn1";
            this.gridColumn1.OptionsColumn.AllowEdit = false;
            this.gridColumn1.Visible = true;
            this.gridColumn1.VisibleIndex = 0;
            this.gridColumn1.Width = 106;
            // 
            // gridColumn2
            // 
            this.gridColumn2.AppearanceCell.Options.UseTextOptions = true;
            this.gridColumn2.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumn2.AppearanceHeader.Options.UseTextOptions = true;
            this.gridColumn2.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumn2.Caption = "姓名";
            this.gridColumn2.FieldName = "clntname";
            this.gridColumn2.Name = "gridColumn2";
            this.gridColumn2.OptionsColumn.AllowEdit = false;
            this.gridColumn2.Visible = true;
            this.gridColumn2.VisibleIndex = 1;
            this.gridColumn2.Width = 91;
            // 
            // gridColumn3
            // 
            this.gridColumn3.AppearanceCell.Options.UseTextOptions = true;
            this.gridColumn3.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumn3.AppearanceHeader.Options.UseTextOptions = true;
            this.gridColumn3.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumn3.Caption = "手机";
            this.gridColumn3.FieldName = "xpho";
            this.gridColumn3.Name = "gridColumn3";
            this.gridColumn3.OptionsColumn.AllowEdit = false;
            this.gridColumn3.Visible = true;
            this.gridColumn3.VisibleIndex = 2;
            this.gridColumn3.Width = 105;
            // 
            // gridColumn4
            // 
            this.gridColumn4.AppearanceCell.Options.UseTextOptions = true;
            this.gridColumn4.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumn4.AppearanceHeader.Options.UseTextOptions = true;
            this.gridColumn4.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumn4.Caption = "积分";
            this.gridColumn4.ColumnEdit = this.rlueJF;
            this.gridColumn4.FieldName = "clntcode";
            this.gridColumn4.Name = "gridColumn4";
            this.gridColumn4.OptionsColumn.AllowEdit = false;
            this.gridColumn4.Visible = true;
            this.gridColumn4.VisibleIndex = 3;
            this.gridColumn4.Width = 72;
            // 
            // rlueJF
            // 
            this.rlueJF.AutoHeight = false;
            this.rlueJF.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.rlueJF.Columns.AddRange(new DevExpress.XtraEditors.Controls.LookUpColumnInfo[] {
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("xjie", "积分")});
            this.rlueJF.DisplayMember = "xjie";
            this.rlueJF.Name = "rlueJF";
            this.rlueJF.NullText = "";
            this.rlueJF.ValueMember = "clntcode";
            // 
            // gridColumn5
            // 
            this.gridColumn5.AppearanceCell.Options.UseTextOptions = true;
            this.gridColumn5.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumn5.AppearanceHeader.Options.UseTextOptions = true;
            this.gridColumn5.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumn5.Caption = "余额";
            this.gridColumn5.ColumnEdit = this.rlueBlance;
            this.gridColumn5.FieldName = "clntcode";
            this.gridColumn5.Name = "gridColumn5";
            this.gridColumn5.OptionsColumn.AllowEdit = false;
            this.gridColumn5.Visible = true;
            this.gridColumn5.VisibleIndex = 4;
            this.gridColumn5.Width = 78;
            // 
            // rlueBlance
            // 
            this.rlueBlance.AutoHeight = false;
            this.rlueBlance.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.rlueBlance.Columns.AddRange(new DevExpress.XtraEditors.Controls.LookUpColumnInfo[] {
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("xjie", "余额")});
            this.rlueBlance.DisplayMember = "xjie";
            this.rlueBlance.Name = "rlueBlance";
            this.rlueBlance.NullText = "";
            this.rlueBlance.ValueMember = "clntcode";
            // 
            // gridColumn6
            // 
            this.gridColumn6.AppearanceCell.Options.UseTextOptions = true;
            this.gridColumn6.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumn6.AppearanceHeader.Options.UseTextOptions = true;
            this.gridColumn6.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumn6.Caption = "折扣";
            this.gridColumn6.FieldName = "xzhe";
            this.gridColumn6.Name = "gridColumn6";
            this.gridColumn6.OptionsColumn.AllowEdit = false;
            this.gridColumn6.Width = 59;
            // 
            // colRefresh
            // 
            this.colRefresh.AppearanceCell.Options.UseTextOptions = true;
            this.colRefresh.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.colRefresh.AppearanceHeader.Options.UseTextOptions = true;
            this.colRefresh.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.colRefresh.Caption = "操作";
            this.colRefresh.ColumnEdit = this.rpicRefresh;
            this.colRefresh.FieldName = "image";
            this.colRefresh.Name = "colRefresh";
            this.colRefresh.UnboundType = DevExpress.Data.UnboundColumnType.Object;
            this.colRefresh.Visible = true;
            this.colRefresh.VisibleIndex = 5;
            this.colRefresh.Width = 64;
            // 
            // rpicRefresh
            // 
            this.rpicRefresh.Name = "rpicRefresh";
            this.rpicRefresh.Click += new System.EventHandler(this.rpicRefresh_Click);
            // 
            // FormClientSearch
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(559, 386);
            this.Controls.Add(this.gd);
            this.Controls.Add(this.bteSearch);
            this.Name = "FormClientSearch";
            this.Text = "搜索会员";
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.FormClientSearch_KeyDown);
            ((System.ComponentModel.ISupportInitialize)(this.bteSearch.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bdsData)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gd)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gv)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rlueJF)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rlueBlance)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rpicRefresh)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.ButtonEdit bteSearch;
        private System.Windows.Forms.BindingSource bdsData;
        private DevExpress.XtraGrid.GridControl gd;
        private DevExpress.XtraGrid.Views.Grid.GridView gv;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn1;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn2;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn3;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn4;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn5;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn6;
        private DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit rlueJF;
        private DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit rlueBlance;
        private DevExpress.XtraGrid.Columns.GridColumn colRefresh;
        private DevExpress.XtraEditors.Repository.RepositoryItemPictureEdit rpicRefresh;
    }
}