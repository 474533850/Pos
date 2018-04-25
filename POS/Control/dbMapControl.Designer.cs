namespace POS.Control
{
    partial class Control
    {
        /// <summary> 
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region 组件设计器生成的代码

        /// <summary> 
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.panelControl1 = new DevExpress.XtraEditors.PanelControl();
            this.txtServiceTable = new DevExpress.XtraEditors.TextEdit();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.lueLocalTable = new DevExpress.XtraEditors.LookUpEdit();
            this.labelControl17 = new DevExpress.XtraEditors.LabelControl();
            this.gdDBMap = new DevExpress.XtraGrid.GridControl();
            this.gvDBMap = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.gridColumn1 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn2 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn3 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn4 = new DevExpress.XtraGrid.Columns.GridColumn();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).BeginInit();
            this.panelControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtServiceTable.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lueLocalTable.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gdDBMap)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvDBMap)).BeginInit();
            this.SuspendLayout();
            // 
            // panelControl1
            // 
            this.panelControl1.Controls.Add(this.txtServiceTable);
            this.panelControl1.Controls.Add(this.labelControl1);
            this.panelControl1.Controls.Add(this.lueLocalTable);
            this.panelControl1.Controls.Add(this.labelControl17);
            this.panelControl1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelControl1.Location = new System.Drawing.Point(0, 0);
            this.panelControl1.Name = "panelControl1";
            this.panelControl1.Size = new System.Drawing.Size(554, 64);
            this.panelControl1.TabIndex = 1;
            // 
            // txtServiceTable
            // 
            this.txtServiceTable.EditValue = "";
            this.txtServiceTable.Location = new System.Drawing.Point(386, 23);
            this.txtServiceTable.Margin = new System.Windows.Forms.Padding(2);
            this.txtServiceTable.Name = "txtServiceTable";
            this.txtServiceTable.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 12F);
            this.txtServiceTable.Properties.Appearance.Options.UseFont = true;
            this.txtServiceTable.Properties.ReadOnly = true;
            this.txtServiceTable.Size = new System.Drawing.Size(159, 26);
            this.txtServiceTable.TabIndex = 22;
            // 
            // labelControl1
            // 
            this.labelControl1.Appearance.Font = new System.Drawing.Font("Tahoma", 12F);
            this.labelControl1.Location = new System.Drawing.Point(270, 23);
            this.labelControl1.Margin = new System.Windows.Forms.Padding(2);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(112, 19);
            this.labelControl1.TabIndex = 21;
            this.labelControl1.Text = "服务器数据表：";
            // 
            // lueLocalTable
            // 
            this.lueLocalTable.Location = new System.Drawing.Point(106, 23);
            this.lueLocalTable.Name = "lueLocalTable";
            this.lueLocalTable.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 12F);
            this.lueLocalTable.Properties.Appearance.Options.UseFont = true;
            this.lueLocalTable.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.lueLocalTable.Properties.Columns.AddRange(new DevExpress.XtraEditors.Controls.LookUpColumnInfo[] {
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("Value", "名称")});
            this.lueLocalTable.Properties.DisplayMember = "Value";
            this.lueLocalTable.Properties.NullText = "";
            this.lueLocalTable.Properties.ReadOnly = true;
            this.lueLocalTable.Properties.ValueMember = "Key";
            this.lueLocalTable.Size = new System.Drawing.Size(140, 26);
            this.lueLocalTable.TabIndex = 19;
            // 
            // labelControl17
            // 
            this.labelControl17.Appearance.Font = new System.Drawing.Font("Tahoma", 12F);
            this.labelControl17.Location = new System.Drawing.Point(10, 23);
            this.labelControl17.Margin = new System.Windows.Forms.Padding(2);
            this.labelControl17.Name = "labelControl17";
            this.labelControl17.Size = new System.Drawing.Size(96, 19);
            this.labelControl17.TabIndex = 20;
            this.labelControl17.Text = "本地数据表：";
            // 
            // gdDBMap
            // 
            this.gdDBMap.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gdDBMap.EmbeddedNavigator.Margin = new System.Windows.Forms.Padding(2);
            this.gdDBMap.Location = new System.Drawing.Point(0, 64);
            this.gdDBMap.MainView = this.gvDBMap;
            this.gdDBMap.Margin = new System.Windows.Forms.Padding(2);
            this.gdDBMap.Name = "gdDBMap";
            this.gdDBMap.Size = new System.Drawing.Size(554, 303);
            this.gdDBMap.TabIndex = 9;
            this.gdDBMap.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gvDBMap});
            // 
            // gvDBMap
            // 
            this.gvDBMap.ColumnPanelRowHeight = 30;
            this.gvDBMap.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.gridColumn1,
            this.gridColumn2,
            this.gridColumn3,
            this.gridColumn4});
            this.gvDBMap.GridControl = this.gdDBMap;
            this.gvDBMap.IndicatorWidth = 30;
            this.gvDBMap.Name = "gvDBMap";
            this.gvDBMap.OptionsBehavior.Editable = false;
            this.gvDBMap.OptionsCustomization.AllowColumnMoving = false;
            this.gvDBMap.OptionsCustomization.AllowFilter = false;
            this.gvDBMap.OptionsCustomization.AllowQuickHideColumns = false;
            this.gvDBMap.OptionsDetail.EnableMasterViewMode = false;
            this.gvDBMap.OptionsMenu.EnableColumnMenu = false;
            this.gvDBMap.OptionsMenu.EnableFooterMenu = false;
            this.gvDBMap.OptionsMenu.EnableGroupPanelMenu = false;
            this.gvDBMap.OptionsMenu.ShowAutoFilterRowItem = false;
            this.gvDBMap.OptionsView.ColumnHeaderAutoHeight = DevExpress.Utils.DefaultBoolean.True;
            this.gvDBMap.OptionsView.RowAutoHeight = true;
            this.gvDBMap.OptionsView.ShowGroupPanel = false;
            this.gvDBMap.RowHeight = 25;
            // 
            // gridColumn1
            // 
            this.gridColumn1.AppearanceCell.Options.UseTextOptions = true;
            this.gridColumn1.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumn1.AppearanceHeader.Options.UseTextOptions = true;
            this.gridColumn1.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumn1.Caption = "本地_字段名称";
            this.gridColumn1.FieldName = "LColumn";
            this.gridColumn1.Name = "gridColumn1";
            this.gridColumn1.OptionsColumn.AllowEdit = false;
            this.gridColumn1.Visible = true;
            this.gridColumn1.VisibleIndex = 0;
            this.gridColumn1.Width = 138;
            // 
            // gridColumn2
            // 
            this.gridColumn2.AppearanceCell.Options.UseTextOptions = true;
            this.gridColumn2.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumn2.AppearanceHeader.Options.UseTextOptions = true;
            this.gridColumn2.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumn2.Caption = "本地_字段说明";
            this.gridColumn2.FieldName = "LCaption";
            this.gridColumn2.Name = "gridColumn2";
            this.gridColumn2.OptionsColumn.AllowEdit = false;
            this.gridColumn2.Visible = true;
            this.gridColumn2.VisibleIndex = 1;
            this.gridColumn2.Width = 153;
            // 
            // gridColumn3
            // 
            this.gridColumn3.AppearanceCell.Options.UseTextOptions = true;
            this.gridColumn3.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumn3.AppearanceHeader.Options.UseTextOptions = true;
            this.gridColumn3.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumn3.Caption = "服务器_字段名称";
            this.gridColumn3.FieldName = "SColumn";
            this.gridColumn3.Name = "gridColumn3";
            this.gridColumn3.Visible = true;
            this.gridColumn3.VisibleIndex = 2;
            this.gridColumn3.Width = 112;
            // 
            // gridColumn4
            // 
            this.gridColumn4.AppearanceCell.Options.UseTextOptions = true;
            this.gridColumn4.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumn4.AppearanceHeader.Options.UseTextOptions = true;
            this.gridColumn4.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumn4.Caption = "服务器_字段说明";
            this.gridColumn4.FieldName = "SCaption";
            this.gridColumn4.Name = "gridColumn4";
            this.gridColumn4.Visible = true;
            this.gridColumn4.VisibleIndex = 3;
            this.gridColumn4.Width = 119;
            // 
            // DBMapControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.gdDBMap);
            this.Controls.Add(this.panelControl1);
            this.Name = "DBMapControl";
            this.Size = new System.Drawing.Size(554, 367);
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).EndInit();
            this.panelControl1.ResumeLayout(false);
            this.panelControl1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtServiceTable.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lueLocalTable.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gdDBMap)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvDBMap)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.PanelControl panelControl1;
        private DevExpress.XtraEditors.TextEdit txtServiceTable;
        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraEditors.LookUpEdit lueLocalTable;
        private DevExpress.XtraEditors.LabelControl labelControl17;
        private DevExpress.XtraGrid.GridControl gdDBMap;
        private DevExpress.XtraGrid.Views.Grid.GridView gvDBMap;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn1;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn2;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn3;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn4;
    }
}
