using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Views.Grid;
using POS.BLL;
using POS.BLL.Report;
using POS.Common.Enum;
using POS.Common.utility;
using POS.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace POS.Sale
{
    /// <summary>
    /// 促销活动
    /// </summary>
    public partial class FormMessageCenter : BaseForm
    {
        SaleBLL saleBLL = new SaleBLL();
        Dictionary<string, string> tyeDic = EnumHelper.GetEnumDictionary(typeof(SaleType));
        public FormMessageCenter()
        {
            InitializeComponent();
            Init();
            gv.OptionsFind.AlwaysVisible = true;
        }


        void Init()
        {
            bdsData.DataSource = saleBLL.GetAllCurrentSalesDetail();
            gvRules_t.OptionsDetail.ShowDetailTabs = false;
            gvRules_t.OptionsDetail.EnableMasterViewMode = false;
            gvRules_a.OptionsDetail.ShowDetailTabs = false;
            gvRules_j.OptionsDetail.ShowDetailTabs = false;
            gvRules_j.OptionsDetail.EnableMasterViewMode = false;
            gvRules_p.OptionsDetail.ShowDetailTabs = false;
            gvRules_z.OptionsDetail.ShowDetailTabs = false;
            gvRules_z.OptionsDetail.EnableMasterViewMode = false;
            gvRules_a.RowClick += GvRules_RowClick;
            gvRules_j.RowClick += GvRules_RowClick;
            gvRules_p.RowClick += GvRules_RowClick;
            gvRules_z.RowClick += GvRules_RowClick;
            gvRules_t.RowClick += GvRules_RowClick;
            GetVersionInfo();
        }

        #region 双击打开、关闭促销活动
        private void gv_RowClick(object sender, DevExpress.XtraGrid.Views.Grid.RowClickEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                if (e.Clicks == 2)
                {
                    if (gv.GetMasterRowExpanded(e.RowHandle))
                    {
                        gv.CollapseMasterRow(e.RowHandle);
                    }
                    else
                    {
                        SaleModel sale = bdsData.Current as SaleModel;
                        if (sale != null)
                        {
                            GridLevelNode gridLevelNode1 = gridControl1.LevelTree.Nodes.FirstOrDefault();
                            //减价
                            if (sale.xtype == tyeDic[Enum.GetName(typeof(SaleType), SaleType.j)])
                            {
                                gridLevelNode1.LevelTemplate = gvRules_j;
                            }
                            //赠品
                            else if (sale.xtype == tyeDic[Enum.GetName(typeof(SaleType), SaleType.p)])
                            {
                                gridLevelNode1.LevelTemplate = gvRules_p;
                            }
                            //打折
                            else if (sale.xtype == tyeDic[Enum.GetName(typeof(SaleType), SaleType.z)])
                            {
                                gridLevelNode1.LevelTemplate = gvRules_z;
                            }
                            //加送
                            else if (sale.xtype == tyeDic[Enum.GetName(typeof(SaleType), SaleType.a)])
                            {
                                gridLevelNode1.LevelTemplate = gvRules_a;
                            }
                            //特价
                            else if (sale.xtype == tyeDic[Enum.GetName(typeof(SaleType), SaleType.t)])
                            {
                                gridLevelNode1.LevelTemplate = gvRules_t;
                            }
                        }
                        gv.ExpandMasterRow(e.RowHandle);
                    }
                }
            }
        }
        #endregion

        #region 自定义多明细关系名称
        private void gv_MasterRowGetRelationDisplayCaption(object sender, DevExpress.XtraGrid.Views.Grid.MasterRowGetRelationNameEventArgs e)
        {
            if (e.RelationName == "salegoods")
            {
                e.RelationName = "促销商品";
            }
            else if (e.RelationName == "salerules")
            {
                e.RelationName = "促销规则";
            }
        }
        #endregion

        #region 双击打开、关闭促销赠品
        private void GvRules_RowClick(object sender, DevExpress.XtraGrid.Views.Grid.RowClickEventArgs e)
        {
            GridView gv = sender as GridView;
            if (e.Button == MouseButtons.Left)
            {
                if (e.Clicks == 2)
                {
                    if (gv.GetMasterRowExpanded(e.RowHandle))
                    {
                        gv.CollapseMasterRow(e.RowHandle);
                    }
                    else
                    {
                        gv.ExpandMasterRow(e.RowHandle);
                    }
                }
            }
        }
        #endregion

        private void GetVersionInfo()
        {
            string filePath = Path.Combine(Application.StartupPath, "version.txt");
            if (File.Exists(filePath))
            {
                FileStream fs = null;
                StreamReader sr = null;
                try
                {
                    fs = new FileStream(filePath, FileMode.Open, FileAccess.Read);
                    sr = new StreamReader(fs);
                    sr.BaseStream.Seek(0, SeekOrigin.Begin);
                    string str = sr.ReadToEnd();
                    this.meVersion.Text = str;
                }
                catch (Exception)
                {
                }
                finally
                {
                    if (sr != null)
                    {
                        sr.Close();
                    }
                    if (fs != null)
                    {
                        fs.Close();
                    }
                }
            }
        }
    }
}
