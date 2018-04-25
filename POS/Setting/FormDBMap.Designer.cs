namespace POS.Setting
{
    partial class FormDBMap
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormDBMap));
            this.xtraTabControl1 = new DevExpress.XtraTab.XtraTabControl();
            this.xtraTabPage1 = new DevExpress.XtraTab.XtraTabPage();
            this.panelControl3 = new DevExpress.XtraEditors.PanelControl();
            this.gridControl2 = new DevExpress.XtraGrid.GridControl();
            this.bdsService = new System.Windows.Forms.BindingSource(this.components);
            this.gvServer = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.gridColumn3 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn4 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.panelControl5 = new DevExpress.XtraEditors.PanelControl();
            this.btnDel_S = new DevExpress.XtraEditors.SimpleButton();
            this.btnDown_S = new DevExpress.XtraEditors.SimpleButton();
            this.btnUP_S = new DevExpress.XtraEditors.SimpleButton();
            this.splitterControl1 = new DevExpress.XtraEditors.SplitterControl();
            this.panelControl2 = new DevExpress.XtraEditors.PanelControl();
            this.gridControl1 = new DevExpress.XtraGrid.GridControl();
            this.bdsLocal = new System.Windows.Forms.BindingSource(this.components);
            this.gvLocal = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.gridColumn1 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn2 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.panelControl6 = new DevExpress.XtraEditors.PanelControl();
            this.btnDown_L = new DevExpress.XtraEditors.SimpleButton();
            this.btnUP_L = new DevExpress.XtraEditors.SimpleButton();
            this.panelControl1 = new DevExpress.XtraEditors.PanelControl();
            this.btnSave = new DevExpress.XtraEditors.SimpleButton();
            this.btnLoadServerStructure = new DevExpress.XtraEditors.SimpleButton();
            this.txtServiceTable = new DevExpress.XtraEditors.TextEdit();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.lueLocalTable = new DevExpress.XtraEditors.LookUpEdit();
            this.labelControl17 = new DevExpress.XtraEditors.LabelControl();
            this.xtraTabPage2 = new DevExpress.XtraTab.XtraTabPage();
            this.flpDBMap = new System.Windows.Forms.FlowLayoutPanel();
            this.panelControl4 = new DevExpress.XtraEditors.PanelControl();
            this.lueLocalTable_Query = new DevExpress.XtraEditors.LookUpEdit();
            this.labelControl2 = new DevExpress.XtraEditors.LabelControl();
            this.btnQuery = new DevExpress.XtraEditors.SimpleButton();
            this.btnDel_L = new DevExpress.XtraEditors.SimpleButton();
            ((System.ComponentModel.ISupportInitialize)(this.xtraTabControl1)).BeginInit();
            this.xtraTabControl1.SuspendLayout();
            this.xtraTabPage1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl3)).BeginInit();
            this.panelControl3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridControl2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bdsService)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvServer)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl5)).BeginInit();
            this.panelControl5.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl2)).BeginInit();
            this.panelControl2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridControl1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bdsLocal)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvLocal)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl6)).BeginInit();
            this.panelControl6.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).BeginInit();
            this.panelControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtServiceTable.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lueLocalTable.Properties)).BeginInit();
            this.xtraTabPage2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl4)).BeginInit();
            this.panelControl4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.lueLocalTable_Query.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // xtraTabControl1
            // 
            this.xtraTabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.xtraTabControl1.Location = new System.Drawing.Point(0, 0);
            this.xtraTabControl1.Name = "xtraTabControl1";
            this.xtraTabControl1.SelectedTabPage = this.xtraTabPage1;
            this.xtraTabControl1.Size = new System.Drawing.Size(903, 532);
            this.xtraTabControl1.TabIndex = 0;
            this.xtraTabControl1.TabPages.AddRange(new DevExpress.XtraTab.XtraTabPage[] {
            this.xtraTabPage1,
            this.xtraTabPage2});
            this.xtraTabControl1.SelectedPageChanged += new DevExpress.XtraTab.TabPageChangedEventHandler(this.xtraTabControl1_SelectedPageChanged);
            // 
            // xtraTabPage1
            // 
            this.xtraTabPage1.Controls.Add(this.panelControl3);
            this.xtraTabPage1.Controls.Add(this.splitterControl1);
            this.xtraTabPage1.Controls.Add(this.panelControl2);
            this.xtraTabPage1.Controls.Add(this.panelControl1);
            this.xtraTabPage1.Name = "xtraTabPage1";
            this.xtraTabPage1.Size = new System.Drawing.Size(897, 503);
            this.xtraTabPage1.Text = "新建表映射关系";
            // 
            // panelControl3
            // 
            this.panelControl3.Controls.Add(this.gridControl2);
            this.panelControl3.Controls.Add(this.panelControl5);
            this.panelControl3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelControl3.Location = new System.Drawing.Point(442, 64);
            this.panelControl3.Name = "panelControl3";
            this.panelControl3.Size = new System.Drawing.Size(455, 439);
            this.panelControl3.TabIndex = 3;
            // 
            // gridControl2
            // 
            this.gridControl2.DataSource = this.bdsService;
            this.gridControl2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridControl2.EmbeddedNavigator.Margin = new System.Windows.Forms.Padding(2);
            this.gridControl2.Location = new System.Drawing.Point(2, 2);
            this.gridControl2.MainView = this.gvServer;
            this.gridControl2.Margin = new System.Windows.Forms.Padding(2);
            this.gridControl2.Name = "gridControl2";
            this.gridControl2.Size = new System.Drawing.Size(385, 435);
            this.gridControl2.TabIndex = 9;
            this.gridControl2.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gvServer});
            // 
            // gvServer
            // 
            this.gvServer.ColumnPanelRowHeight = 30;
            this.gvServer.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.gridColumn3,
            this.gridColumn4});
            this.gvServer.GridControl = this.gridControl2;
            this.gvServer.IndicatorWidth = 30;
            this.gvServer.Name = "gvServer";
            this.gvServer.OptionsCustomization.AllowColumnMoving = false;
            this.gvServer.OptionsCustomization.AllowFilter = false;
            this.gvServer.OptionsCustomization.AllowQuickHideColumns = false;
            this.gvServer.OptionsCustomization.AllowSort = false;
            this.gvServer.OptionsDetail.EnableMasterViewMode = false;
            this.gvServer.OptionsMenu.EnableColumnMenu = false;
            this.gvServer.OptionsMenu.EnableFooterMenu = false;
            this.gvServer.OptionsMenu.EnableGroupPanelMenu = false;
            this.gvServer.OptionsMenu.ShowAutoFilterRowItem = false;
            this.gvServer.OptionsView.ColumnHeaderAutoHeight = DevExpress.Utils.DefaultBoolean.True;
            this.gvServer.OptionsView.RowAutoHeight = true;
            this.gvServer.OptionsView.ShowGroupPanel = false;
            this.gvServer.RowHeight = 25;
            this.gvServer.CustomDrawRowIndicator += new DevExpress.XtraGrid.Views.Grid.RowIndicatorCustomDrawEventHandler(this.gvServer_CustomDrawRowIndicator);
            this.gvServer.FocusedRowChanged += new DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventHandler(this.gvServer_FocusedRowChanged);
            // 
            // gridColumn3
            // 
            this.gridColumn3.AppearanceCell.Options.UseTextOptions = true;
            this.gridColumn3.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumn3.AppearanceHeader.Options.UseTextOptions = true;
            this.gridColumn3.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumn3.Caption = "服务器_字段名称";
            this.gridColumn3.FieldName = "Column";
            this.gridColumn3.Name = "gridColumn3";
            this.gridColumn3.OptionsColumn.AllowEdit = false;
            this.gridColumn3.Visible = true;
            this.gridColumn3.VisibleIndex = 0;
            this.gridColumn3.Width = 165;
            // 
            // gridColumn4
            // 
            this.gridColumn4.AppearanceCell.Options.UseTextOptions = true;
            this.gridColumn4.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumn4.AppearanceHeader.Options.UseTextOptions = true;
            this.gridColumn4.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumn4.Caption = "字段说明";
            this.gridColumn4.FieldName = "Caption";
            this.gridColumn4.Name = "gridColumn4";
            this.gridColumn4.OptionsColumn.AllowEdit = false;
            this.gridColumn4.Visible = true;
            this.gridColumn4.VisibleIndex = 1;
            this.gridColumn4.Width = 184;
            // 
            // panelControl5
            // 
            this.panelControl5.Controls.Add(this.btnDel_S);
            this.panelControl5.Controls.Add(this.btnDown_S);
            this.panelControl5.Controls.Add(this.btnUP_S);
            this.panelControl5.Dock = System.Windows.Forms.DockStyle.Right;
            this.panelControl5.Location = new System.Drawing.Point(387, 2);
            this.panelControl5.Name = "panelControl5";
            this.panelControl5.Size = new System.Drawing.Size(66, 435);
            this.panelControl5.TabIndex = 1;
            // 
            // btnDel_S
            // 
            this.btnDel_S.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnDel_S.Appearance.Font = new System.Drawing.Font("Tahoma", 12F);
            this.btnDel_S.Appearance.Options.UseFont = true;
            this.btnDel_S.Image = ((System.Drawing.Image)(resources.GetObject("btnDel_S.Image")));
            this.btnDel_S.ImageLocation = DevExpress.XtraEditors.ImageLocation.MiddleCenter;
            this.btnDel_S.Location = new System.Drawing.Point(4, 128);
            this.btnDel_S.Margin = new System.Windows.Forms.Padding(2);
            this.btnDel_S.Name = "btnDel_S";
            this.btnDel_S.Size = new System.Drawing.Size(58, 34);
            this.btnDel_S.TabIndex = 25;
            this.btnDel_S.Click += new System.EventHandler(this.btnDel_S_Click);
            // 
            // btnDown_S
            // 
            this.btnDown_S.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnDown_S.Appearance.Font = new System.Drawing.Font("Tahoma", 12F);
            this.btnDown_S.Appearance.Options.UseFont = true;
            this.btnDown_S.Image = ((System.Drawing.Image)(resources.GetObject("btnDown_S.Image")));
            this.btnDown_S.ImageLocation = DevExpress.XtraEditors.ImageLocation.MiddleCenter;
            this.btnDown_S.Location = new System.Drawing.Point(4, 79);
            this.btnDown_S.Margin = new System.Windows.Forms.Padding(2);
            this.btnDown_S.Name = "btnDown_S";
            this.btnDown_S.Size = new System.Drawing.Size(58, 34);
            this.btnDown_S.TabIndex = 24;
            this.btnDown_S.Click += new System.EventHandler(this.btnDown_S_Click);
            // 
            // btnUP_S
            // 
            this.btnUP_S.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnUP_S.Appearance.Font = new System.Drawing.Font("Tahoma", 12F);
            this.btnUP_S.Appearance.Options.UseFont = true;
            this.btnUP_S.Image = ((System.Drawing.Image)(resources.GetObject("btnUP_S.Image")));
            this.btnUP_S.ImageLocation = DevExpress.XtraEditors.ImageLocation.MiddleCenter;
            this.btnUP_S.Location = new System.Drawing.Point(4, 30);
            this.btnUP_S.Margin = new System.Windows.Forms.Padding(2);
            this.btnUP_S.Name = "btnUP_S";
            this.btnUP_S.Size = new System.Drawing.Size(58, 34);
            this.btnUP_S.TabIndex = 24;
            this.btnUP_S.Click += new System.EventHandler(this.btnUP_S_Click);
            // 
            // splitterControl1
            // 
            this.splitterControl1.Location = new System.Drawing.Point(437, 64);
            this.splitterControl1.Name = "splitterControl1";
            this.splitterControl1.Size = new System.Drawing.Size(5, 439);
            this.splitterControl1.TabIndex = 2;
            this.splitterControl1.TabStop = false;
            // 
            // panelControl2
            // 
            this.panelControl2.Controls.Add(this.gridControl1);
            this.panelControl2.Controls.Add(this.panelControl6);
            this.panelControl2.Dock = System.Windows.Forms.DockStyle.Left;
            this.panelControl2.Location = new System.Drawing.Point(0, 64);
            this.panelControl2.Name = "panelControl2";
            this.panelControl2.Size = new System.Drawing.Size(437, 439);
            this.panelControl2.TabIndex = 1;
            // 
            // gridControl1
            // 
            this.gridControl1.DataSource = this.bdsLocal;
            this.gridControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridControl1.EmbeddedNavigator.Margin = new System.Windows.Forms.Padding(2);
            this.gridControl1.Location = new System.Drawing.Point(2, 2);
            this.gridControl1.MainView = this.gvLocal;
            this.gridControl1.Margin = new System.Windows.Forms.Padding(2);
            this.gridControl1.Name = "gridControl1";
            this.gridControl1.Size = new System.Drawing.Size(367, 435);
            this.gridControl1.TabIndex = 8;
            this.gridControl1.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gvLocal});
            // 
            // gvLocal
            // 
            this.gvLocal.ColumnPanelRowHeight = 30;
            this.gvLocal.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.gridColumn1,
            this.gridColumn2});
            this.gvLocal.GridControl = this.gridControl1;
            this.gvLocal.IndicatorWidth = 30;
            this.gvLocal.Name = "gvLocal";
            this.gvLocal.OptionsCustomization.AllowColumnMoving = false;
            this.gvLocal.OptionsCustomization.AllowFilter = false;
            this.gvLocal.OptionsCustomization.AllowQuickHideColumns = false;
            this.gvLocal.OptionsCustomization.AllowSort = false;
            this.gvLocal.OptionsDetail.EnableMasterViewMode = false;
            this.gvLocal.OptionsMenu.EnableColumnMenu = false;
            this.gvLocal.OptionsMenu.EnableFooterMenu = false;
            this.gvLocal.OptionsMenu.EnableGroupPanelMenu = false;
            this.gvLocal.OptionsMenu.ShowAutoFilterRowItem = false;
            this.gvLocal.OptionsView.ColumnHeaderAutoHeight = DevExpress.Utils.DefaultBoolean.True;
            this.gvLocal.OptionsView.RowAutoHeight = true;
            this.gvLocal.OptionsView.ShowGroupPanel = false;
            this.gvLocal.RowHeight = 25;
            this.gvLocal.CustomDrawRowIndicator += new DevExpress.XtraGrid.Views.Grid.RowIndicatorCustomDrawEventHandler(this.gvLocal_CustomDrawRowIndicator);
            this.gvLocal.FocusedRowChanged += new DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventHandler(this.gvLocal_FocusedRowChanged);
            // 
            // gridColumn1
            // 
            this.gridColumn1.AppearanceCell.Options.UseTextOptions = true;
            this.gridColumn1.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumn1.AppearanceHeader.Options.UseTextOptions = true;
            this.gridColumn1.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumn1.Caption = "本地_字段名称";
            this.gridColumn1.FieldName = "Column";
            this.gridColumn1.Name = "gridColumn1";
            this.gridColumn1.OptionsColumn.AllowEdit = false;
            this.gridColumn1.Visible = true;
            this.gridColumn1.VisibleIndex = 0;
            this.gridColumn1.Width = 165;
            // 
            // gridColumn2
            // 
            this.gridColumn2.AppearanceCell.Options.UseTextOptions = true;
            this.gridColumn2.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumn2.AppearanceHeader.Options.UseTextOptions = true;
            this.gridColumn2.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumn2.Caption = "字段说明";
            this.gridColumn2.FieldName = "Caption";
            this.gridColumn2.Name = "gridColumn2";
            this.gridColumn2.OptionsColumn.AllowEdit = false;
            this.gridColumn2.Visible = true;
            this.gridColumn2.VisibleIndex = 1;
            this.gridColumn2.Width = 184;
            // 
            // panelControl6
            // 
            this.panelControl6.Controls.Add(this.btnDel_L);
            this.panelControl6.Controls.Add(this.btnDown_L);
            this.panelControl6.Controls.Add(this.btnUP_L);
            this.panelControl6.Dock = System.Windows.Forms.DockStyle.Right;
            this.panelControl6.Location = new System.Drawing.Point(369, 2);
            this.panelControl6.Name = "panelControl6";
            this.panelControl6.Size = new System.Drawing.Size(66, 435);
            this.panelControl6.TabIndex = 9;
            // 
            // btnDown_L
            // 
            this.btnDown_L.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnDown_L.Appearance.Font = new System.Drawing.Font("Tahoma", 12F);
            this.btnDown_L.Appearance.Options.UseFont = true;
            this.btnDown_L.Image = ((System.Drawing.Image)(resources.GetObject("btnDown_L.Image")));
            this.btnDown_L.ImageLocation = DevExpress.XtraEditors.ImageLocation.MiddleCenter;
            this.btnDown_L.Location = new System.Drawing.Point(4, 79);
            this.btnDown_L.Margin = new System.Windows.Forms.Padding(2);
            this.btnDown_L.Name = "btnDown_L";
            this.btnDown_L.Size = new System.Drawing.Size(58, 34);
            this.btnDown_L.TabIndex = 24;
            this.btnDown_L.Click += new System.EventHandler(this.btnDown_L_Click);
            // 
            // btnUP_L
            // 
            this.btnUP_L.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnUP_L.Appearance.Font = new System.Drawing.Font("Tahoma", 12F);
            this.btnUP_L.Appearance.Options.UseFont = true;
            this.btnUP_L.Image = ((System.Drawing.Image)(resources.GetObject("btnUP_L.Image")));
            this.btnUP_L.ImageLocation = DevExpress.XtraEditors.ImageLocation.MiddleCenter;
            this.btnUP_L.Location = new System.Drawing.Point(4, 30);
            this.btnUP_L.Margin = new System.Windows.Forms.Padding(2);
            this.btnUP_L.Name = "btnUP_L";
            this.btnUP_L.Size = new System.Drawing.Size(58, 34);
            this.btnUP_L.TabIndex = 24;
            this.btnUP_L.Click += new System.EventHandler(this.btnUP_L_Click);
            // 
            // panelControl1
            // 
            this.panelControl1.Controls.Add(this.btnSave);
            this.panelControl1.Controls.Add(this.btnLoadServerStructure);
            this.panelControl1.Controls.Add(this.txtServiceTable);
            this.panelControl1.Controls.Add(this.labelControl1);
            this.panelControl1.Controls.Add(this.lueLocalTable);
            this.panelControl1.Controls.Add(this.labelControl17);
            this.panelControl1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelControl1.Location = new System.Drawing.Point(0, 0);
            this.panelControl1.Name = "panelControl1";
            this.panelControl1.Size = new System.Drawing.Size(897, 64);
            this.panelControl1.TabIndex = 0;
            // 
            // btnSave
            // 
            this.btnSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSave.Appearance.Font = new System.Drawing.Font("Tahoma", 12F);
            this.btnSave.Appearance.Options.UseFont = true;
            this.btnSave.Location = new System.Drawing.Point(760, 15);
            this.btnSave.Margin = new System.Windows.Forms.Padding(2);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(123, 34);
            this.btnSave.TabIndex = 23;
            this.btnSave.Text = "保存";
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnLoadServerStructure
            // 
            this.btnLoadServerStructure.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnLoadServerStructure.Appearance.Font = new System.Drawing.Font("Tahoma", 12F);
            this.btnLoadServerStructure.Appearance.Options.UseFont = true;
            this.btnLoadServerStructure.Location = new System.Drawing.Point(610, 15);
            this.btnLoadServerStructure.Margin = new System.Windows.Forms.Padding(2);
            this.btnLoadServerStructure.Name = "btnLoadServerStructure";
            this.btnLoadServerStructure.Size = new System.Drawing.Size(136, 34);
            this.btnLoadServerStructure.TabIndex = 23;
            this.btnLoadServerStructure.Text = "加载服务器表结构";
            this.btnLoadServerStructure.Click += new System.EventHandler(this.btnLoadServerStructure_Click);
            // 
            // txtServiceTable
            // 
            this.txtServiceTable.EditValue = "";
            this.txtServiceTable.Location = new System.Drawing.Point(421, 20);
            this.txtServiceTable.Margin = new System.Windows.Forms.Padding(2);
            this.txtServiceTable.Name = "txtServiceTable";
            this.txtServiceTable.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 12F);
            this.txtServiceTable.Properties.Appearance.Options.UseFont = true;
            this.txtServiceTable.Size = new System.Drawing.Size(174, 26);
            this.txtServiceTable.TabIndex = 22;
            // 
            // labelControl1
            // 
            this.labelControl1.Appearance.Font = new System.Drawing.Font("Tahoma", 12F);
            this.labelControl1.Location = new System.Drawing.Point(305, 23);
            this.labelControl1.Margin = new System.Windows.Forms.Padding(2);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(112, 19);
            this.labelControl1.TabIndex = 21;
            this.labelControl1.Text = "服务器数据表：";
            // 
            // lueLocalTable
            // 
            this.lueLocalTable.Location = new System.Drawing.Point(111, 20);
            this.lueLocalTable.Name = "lueLocalTable";
            this.lueLocalTable.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 12F);
            this.lueLocalTable.Properties.Appearance.Options.UseFont = true;
            this.lueLocalTable.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.lueLocalTable.Properties.Columns.AddRange(new DevExpress.XtraEditors.Controls.LookUpColumnInfo[] {
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("Value", "名称")});
            this.lueLocalTable.Properties.DisplayMember = "Value";
            this.lueLocalTable.Properties.NullText = "";
            this.lueLocalTable.Properties.ValueMember = "Key";
            this.lueLocalTable.Size = new System.Drawing.Size(160, 26);
            this.lueLocalTable.TabIndex = 19;
            this.lueLocalTable.EditValueChanged += new System.EventHandler(this.lueLocalTable_EditValueChanged);
            // 
            // labelControl17
            // 
            this.labelControl17.Appearance.Font = new System.Drawing.Font("Tahoma", 12F);
            this.labelControl17.Location = new System.Drawing.Point(15, 23);
            this.labelControl17.Margin = new System.Windows.Forms.Padding(2);
            this.labelControl17.Name = "labelControl17";
            this.labelControl17.Size = new System.Drawing.Size(96, 19);
            this.labelControl17.TabIndex = 20;
            this.labelControl17.Text = "本地数据表：";
            // 
            // xtraTabPage2
            // 
            this.xtraTabPage2.Controls.Add(this.flpDBMap);
            this.xtraTabPage2.Controls.Add(this.panelControl4);
            this.xtraTabPage2.Name = "xtraTabPage2";
            this.xtraTabPage2.Size = new System.Drawing.Size(897, 503);
            this.xtraTabPage2.Text = "查询表映射关系";
            // 
            // flpDBMap
            // 
            this.flpDBMap.AutoScroll = true;
            this.flpDBMap.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flpDBMap.Location = new System.Drawing.Point(0, 61);
            this.flpDBMap.Name = "flpDBMap";
            this.flpDBMap.Size = new System.Drawing.Size(897, 442);
            this.flpDBMap.TabIndex = 1;
            // 
            // panelControl4
            // 
            this.panelControl4.Controls.Add(this.lueLocalTable_Query);
            this.panelControl4.Controls.Add(this.labelControl2);
            this.panelControl4.Controls.Add(this.btnQuery);
            this.panelControl4.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelControl4.Location = new System.Drawing.Point(0, 0);
            this.panelControl4.Name = "panelControl4";
            this.panelControl4.Size = new System.Drawing.Size(897, 61);
            this.panelControl4.TabIndex = 0;
            // 
            // lueLocalTable_Query
            // 
            this.lueLocalTable_Query.Location = new System.Drawing.Point(124, 18);
            this.lueLocalTable_Query.Name = "lueLocalTable_Query";
            this.lueLocalTable_Query.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 12F);
            this.lueLocalTable_Query.Properties.Appearance.Options.UseFont = true;
            this.lueLocalTable_Query.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.lueLocalTable_Query.Properties.Columns.AddRange(new DevExpress.XtraEditors.Controls.LookUpColumnInfo[] {
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("Value", "名称")});
            this.lueLocalTable_Query.Properties.DisplayMember = "Value";
            this.lueLocalTable_Query.Properties.NullText = "";
            this.lueLocalTable_Query.Properties.ValueMember = "Key";
            this.lueLocalTable_Query.Size = new System.Drawing.Size(160, 26);
            this.lueLocalTable_Query.TabIndex = 25;
            // 
            // labelControl2
            // 
            this.labelControl2.Appearance.Font = new System.Drawing.Font("Tahoma", 12F);
            this.labelControl2.Location = new System.Drawing.Point(28, 21);
            this.labelControl2.Margin = new System.Windows.Forms.Padding(2);
            this.labelControl2.Name = "labelControl2";
            this.labelControl2.Size = new System.Drawing.Size(96, 19);
            this.labelControl2.TabIndex = 26;
            this.labelControl2.Text = "本地数据表：";
            // 
            // btnQuery
            // 
            this.btnQuery.Appearance.Font = new System.Drawing.Font("Tahoma", 12F);
            this.btnQuery.Appearance.Options.UseFont = true;
            this.btnQuery.Location = new System.Drawing.Point(302, 13);
            this.btnQuery.Margin = new System.Windows.Forms.Padding(2);
            this.btnQuery.Name = "btnQuery";
            this.btnQuery.Size = new System.Drawing.Size(136, 34);
            this.btnQuery.TabIndex = 24;
            this.btnQuery.Text = "查找表";
            this.btnQuery.Click += new System.EventHandler(this.btnQuery_Click);
            // 
            // btnDel_L
            // 
            this.btnDel_L.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnDel_L.Appearance.Font = new System.Drawing.Font("Tahoma", 12F);
            this.btnDel_L.Appearance.Options.UseFont = true;
            this.btnDel_L.Image = ((System.Drawing.Image)(resources.GetObject("btnDel_L.Image")));
            this.btnDel_L.ImageLocation = DevExpress.XtraEditors.ImageLocation.MiddleCenter;
            this.btnDel_L.Location = new System.Drawing.Point(4, 128);
            this.btnDel_L.Margin = new System.Windows.Forms.Padding(2);
            this.btnDel_L.Name = "btnDel_L";
            this.btnDel_L.Size = new System.Drawing.Size(58, 34);
            this.btnDel_L.TabIndex = 26;
            this.btnDel_L.Click += new System.EventHandler(this.btnDel_L_Click);
            // 
            // FormDBMap
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(903, 532);
            this.Controls.Add(this.xtraTabControl1);
            this.MaximizeBox = true;
            this.Name = "FormDBMap";
            this.Text = "数据库表映射关系";
            this.Load += new System.EventHandler(this.FormDBMap_Load);
            this.SizeChanged += new System.EventHandler(this.FormDBMap_SizeChanged);
            ((System.ComponentModel.ISupportInitialize)(this.xtraTabControl1)).EndInit();
            this.xtraTabControl1.ResumeLayout(false);
            this.xtraTabPage1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.panelControl3)).EndInit();
            this.panelControl3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridControl2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bdsService)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvServer)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl5)).EndInit();
            this.panelControl5.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.panelControl2)).EndInit();
            this.panelControl2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridControl1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bdsLocal)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvLocal)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl6)).EndInit();
            this.panelControl6.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).EndInit();
            this.panelControl1.ResumeLayout(false);
            this.panelControl1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtServiceTable.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lueLocalTable.Properties)).EndInit();
            this.xtraTabPage2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.panelControl4)).EndInit();
            this.panelControl4.ResumeLayout(false);
            this.panelControl4.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.lueLocalTable_Query.Properties)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraTab.XtraTabControl xtraTabControl1;
        private DevExpress.XtraTab.XtraTabPage xtraTabPage1;
        private DevExpress.XtraEditors.PanelControl panelControl1;
        private DevExpress.XtraTab.XtraTabPage xtraTabPage2;
        private DevExpress.XtraEditors.LookUpEdit lueLocalTable;
        private DevExpress.XtraEditors.LabelControl labelControl17;
        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraEditors.TextEdit txtServiceTable;
        private DevExpress.XtraEditors.SimpleButton btnSave;
        private DevExpress.XtraEditors.SimpleButton btnLoadServerStructure;
        private DevExpress.XtraEditors.PanelControl panelControl3;
        private DevExpress.XtraEditors.SplitterControl splitterControl1;
        private DevExpress.XtraEditors.PanelControl panelControl2;
        private DevExpress.XtraEditors.PanelControl panelControl5;
        private DevExpress.XtraGrid.GridControl gridControl1;
        private DevExpress.XtraGrid.Views.Grid.GridView gvLocal;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn1;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn2;
        private DevExpress.XtraEditors.SimpleButton btnDown_S;
        private DevExpress.XtraEditors.SimpleButton btnUP_S;
        private System.Windows.Forms.BindingSource bdsService;
        private System.Windows.Forms.BindingSource bdsLocal;
        private DevExpress.XtraGrid.GridControl gridControl2;
        private DevExpress.XtraGrid.Views.Grid.GridView gvServer;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn3;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn4;
        private DevExpress.XtraEditors.PanelControl panelControl4;
        private DevExpress.XtraEditors.SimpleButton btnQuery;
        private System.Windows.Forms.FlowLayoutPanel flpDBMap;
        private DevExpress.XtraEditors.LookUpEdit lueLocalTable_Query;
        private DevExpress.XtraEditors.LabelControl labelControl2;
        private DevExpress.XtraEditors.PanelControl panelControl6;
        private DevExpress.XtraEditors.SimpleButton btnDown_L;
        private DevExpress.XtraEditors.SimpleButton btnUP_L;
        private DevExpress.XtraEditors.SimpleButton btnDel_S;
        private DevExpress.XtraEditors.SimpleButton btnDel_L;
    }
}