using POS.BLL;
using POS.Common;
using POS.Common.utility;
using POS.Control;
using POS.Helper;
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

namespace POS.Setting
{
    public partial class FormDBMap : BaseForm
    {
        DBBLL bll = new DBBLL();
        public FormDBMap()
        {
            InitializeComponent();
        }

        private void FormDBMap_SizeChanged(object sender, EventArgs e)
        {
            splitterControl1.SplitPosition = this.Width / 2;
        }

        private void FormDBMap_Load(object sender, EventArgs e)
        {
            lueLocalTable.Properties.DataSource = AppConst.dicDB;
            lueLocalTable_Query.Properties.DataSource = AppConst.dicDB;
            if (!Directory.Exists(AppConst.dbMapPath))
            {
                //创建数据库文件目录
                Directory.CreateDirectory(AppConst.dbMapPath);
            }
            txtServiceTable.Text = "D_1002_1919";
        }

        #region 加载本地表结构
        private void lueLocalTable_EditValueChanged(object sender, EventArgs e)
        {
            if (lueLocalTable.EditValue != null && !string.IsNullOrEmpty(lueLocalTable.EditValue.ToString()))
            {
                bdsLocal.DataSource = AppConst.dicDBStruct[lueLocalTable.EditValue.ToString()].OrderByDescending(r => r.Caption).ToList();
            }
            else
            {
                bdsLocal.DataSource = null;
            }
        }
        #endregion

        #region 加载服务器表结构
        private void btnLoadServerStructure_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtServiceTable.Text))
            {
                MessagePopup.ShowInformation("请填写服务器数据表！");
                return;
            }
            string cmdText = string.Format(@"select T.[Column],case when T.[Column]='ID' then '原数据ID' 
                                            when T.[Column]='XSUBID' then 'xsubid'
                                            when T.[Column]='XTABLEID' then 'xtableid'
                                            when T.[Column]='version' then '数据版本' 
                                            else c.FIELDCAP end as Caption from
                                            (
                                            SELECT col.name as [Column]
                                            FROM dbo.syscolumns col  
                                            LEFT  JOIN dbo.systypes t ON col.xtype = t.xusertype  
                                            inner JOIN dbo.sysobjects obj ON col.id = obj.id  
                                                                      AND obj.xtype = 'U'  
                                                                      AND obj.status >= 0  
                                            LEFT  JOIN dbo.syscomments comm ON col.cdefault = comm.id   
                                            WHERE   obj.name = '{0}'  
                                            )T left join {1} c on c.FIELDNAME = T.[Column]", txtServiceTable.Text.Trim(), txtServiceTable.Text.Trim().Replace('D', 'S'));

            string urlStr = string.Format("http://www.dunbiao.com/dataout.jsp?COMP=1919&USER=admin&PASSWORD=aaa&SHOWSQL={0}", cmdText);
            List<DBStructModel> data = bll.GetServiceDBStruct(urlStr);
            bdsService.DataSource = data.Where(r => r.Caption != "序号" && r.Column != "XC").OrderByDescending(r => r.Caption).ToList();
        }
        #endregion

        #region 向上移动
        private void btnUP_L_Click(object sender, EventArgs e)
        {
            //得到当前选中行的索引  
            int rowIndex = gvLocal.FocusedRowHandle;

            List<string> list = new List<string>();
            //把当前选中行的数据存入list数组中
            for (int i = 0; i < gvLocal.Columns.Count; i++)
            {
                list.Add(gvLocal.GetRowCellValue(rowIndex, gvLocal.Columns[i].FieldName).ToString());
            }

            for (int j = 0; j < gvLocal.Columns.Count; j++)
            {
                gvLocal.SetRowCellValue(rowIndex, gvLocal.Columns[j].FieldName, gvLocal.GetRowCellValue(rowIndex - 1, gvLocal.Columns[j].FieldName));
                gvLocal.SetRowCellValue(rowIndex - 1, gvLocal.Columns[j].FieldName, list[j]);
            }

            gvLocal.FocusedRowHandle = rowIndex - 1;
            if (gvLocal.FocusedRowHandle == 0)
            {
                btnUP_L.Enabled = false;
            }
            else
            {
                btnDown_L.Enabled = true;
            }
        }
        private void btnUP_S_Click(object sender, EventArgs e)
        {
            //得到当前选中行的索引  
            int rowIndex = gvServer.FocusedRowHandle;

            List<string> list = new List<string>();
            //把当前选中行的数据存入list数组中
            for (int i = 0; i < gvServer.Columns.Count; i++)
            {
                list.Add(gvServer.GetRowCellValue(rowIndex, gvServer.Columns[i].FieldName).ToString());
            }

            for (int j = 0; j < gvServer.Columns.Count; j++)
            {
                gvServer.SetRowCellValue(rowIndex, gvServer.Columns[j].FieldName, gvServer.GetRowCellValue(rowIndex - 1, gvServer.Columns[j].FieldName));
                gvServer.SetRowCellValue(rowIndex - 1, gvServer.Columns[j].FieldName, list[j]);
            }

            gvServer.FocusedRowHandle = rowIndex - 1;
            if (gvServer.FocusedRowHandle == 0)
            {
                btnUP_S.Enabled = false;
            }
            else
            {
                btnDown_S.Enabled = true;
            }
        }
        #endregion

        #region 向下移动
        private void btnDown_L_Click(object sender, EventArgs e)
        {
            //得到当前选中行的索引 
            int rowIndex = gvLocal.FocusedRowHandle;

            List<string> list = new List<string>();
            for (int i = 0; i < gvLocal.Columns.Count; i++)
            {
                //把当前选中行的数据存入list数组中  
                list.Add(gvLocal.GetRowCellValue(rowIndex, gvLocal.Columns[i].FieldName).ToString());
            }

            for (int j = 0; j < gvLocal.Columns.Count; j++)
            {
                gvLocal.SetRowCellValue(rowIndex, gvLocal.Columns[j].FieldName, gvLocal.GetRowCellValue(rowIndex + 1, gvLocal.Columns[j].FieldName));
                gvLocal.SetRowCellValue(rowIndex + 1, gvLocal.Columns[j].FieldName, list[j]);
            }
            gvLocal.FocusedRowHandle = rowIndex + 1;

            if (gvLocal.FocusedRowHandle == gvLocal.RowCount - 1)
            {
                btnDown_L.Enabled = false;
            }
            else
            {
                btnUP_L.Enabled = true;
            }
        }
        private void btnDown_S_Click(object sender, EventArgs e)
        {
            //得到当前选中行的索引 
            int rowIndex = gvServer.FocusedRowHandle;

            List<string> list = new List<string>();
            for (int i = 0; i < gvServer.Columns.Count; i++)
            {
                //把当前选中行的数据存入list数组中  
                list.Add(gvServer.GetRowCellValue(rowIndex, gvServer.Columns[i].FieldName).ToString());
            }

            for (int j = 0; j < gvServer.Columns.Count; j++)
            {
                gvServer.SetRowCellValue(rowIndex, gvServer.Columns[j].FieldName, gvServer.GetRowCellValue(rowIndex + 1, gvServer.Columns[j].FieldName));
                gvServer.SetRowCellValue(rowIndex + 1, gvServer.Columns[j].FieldName, list[j]);
            }
            gvServer.FocusedRowHandle = rowIndex + 1;

            if (gvServer.FocusedRowHandle == gvServer.RowCount - 1)
            {
                btnDown_S.Enabled = false;
            }
            else
            {
                btnUP_S.Enabled = true;
            }
        }
        #endregion

        #region 控制方向键按钮的状态
        private void gvLocal_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
            int rowIndex = gvLocal.FocusedRowHandle;
            if (rowIndex == 0)
            {
                btnUP_L.Enabled = false;
                btnDown_L.Enabled = true;
            }
            else if (rowIndex == gvLocal.RowCount - 1)
            {
                btnUP_L.Enabled = true;
                btnDown_L.Enabled = false;
            }
            else
            {
                btnUP_L.Enabled = true;
                btnDown_L.Enabled = true;
            }
        }
        private void gvServer_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
            int rowIndex = gvServer.FocusedRowHandle;
            if (rowIndex == 0)
            {
                btnUP_S.Enabled = false;
                btnDown_S.Enabled = true;
            }
            else if (rowIndex == gvServer.RowCount - 1)
            {
                btnUP_S.Enabled = true;
                btnDown_S.Enabled = false;
            }
            else
            {
                btnUP_S.Enabled = true;
                btnDown_S.Enabled = true;
            }
        }
        #endregion

        #region 删除当前行
        private void btnDel_L_Click(object sender, EventArgs e)
        {
            if (bdsLocal.Current != null)
            {
                bdsLocal.RemoveCurrent();
            }
            else
            {
                MessagePopup.ShowInformation("请选择一条数据！");
            }
        }
        private void btnDel_S_Click(object sender, EventArgs e)
        {
            if (bdsService.Current != null)
            {
                bdsService.RemoveCurrent();
            }
            else
            {
                MessagePopup.ShowInformation("请选择一条数据！");
            }
        }
        #endregion

        #region 自定义行号

        private void gvLocal_CustomDrawRowIndicator(object sender, DevExpress.XtraGrid.Views.Grid.RowIndicatorCustomDrawEventArgs e)
        {
            if (e.Info.IsRowIndicator && e.RowHandle >= 0)
            {
                e.Info.DisplayText = (e.RowHandle + 1).ToString();
            }
        }

        private void gvServer_CustomDrawRowIndicator(object sender, DevExpress.XtraGrid.Views.Grid.RowIndicatorCustomDrawEventArgs e)
        {
            if (e.Info.IsRowIndicator && e.RowHandle >= 0)
            {
                e.Info.DisplayText = (e.RowHandle + 1).ToString();
            }
        }
        #endregion

        #region 保存表映射关系
        private void btnSave_Click(object sender, EventArgs e)
        {
            bdsLocal.EndEdit();
            bdsService.EndEdit();
            if (gvLocal.RowCount == 0)
            {
                MessagePopup.ShowInformation("本地表结构不能为空！");
            }
            else if (gvServer.RowCount == 0)
            {
                MessagePopup.ShowInformation("服务器表结构不能为空！");
            }
            else
            {
                string file = Path.Combine(AppConst.dbMapPath, lueLocalTable.EditValue.ToString() + "&" + txtServiceTable.Text.Trim() + ".xml");
                if (File.Exists(file))
                {
                    if (MessagePopup.ShowQuestion("是否覆盖当前配置文件？") != DialogResult.Yes)
                    {
                        return;
                    }
                }
                List<DBRelation> datas = new List<DBRelation>();
                List<DBStructModel> dbLocal = bdsLocal.DataSource as List<DBStructModel>;
                List<DBStructModel> dbServer = bdsService.DataSource as List<DBStructModel>;
                if (dbLocal == null)
                {
                    MessagePopup.ShowInformation("本地表结构不能为空！");
                    return;
                }
                if (dbServer == null)
                {
                    MessagePopup.ShowInformation("服务器表结构不能为空！");
                    return;
                }
                for (int i = 0; i < dbLocal.Count; i++)
                {
                    if (dbServer.Count > i)
                    {
                        DBRelation item = new DBRelation();
                        item.LColumn = dbLocal[i].Column;
                        item.LCaption = dbLocal[i].Caption;

                        item.SColumn = dbServer[i].Column;
                        item.SCaption = dbServer[i].Caption;
                        datas.Add(item);
                    }
                }
                try
                {
                    ConfigHelper.SaveConfig<List<DBRelation>>(file, datas);
                    MessagePopup.ShowInformation("保存成功！");
                }
                catch (Exception)
                {
                    MessagePopup.ShowError("保存失败！");
                }
            }
        }
        #endregion

        #region 查找表

        private void btnQuery_Click(object sender, EventArgs e)
        {
            if (lueLocalTable_Query.EditValue != null)
            {
                foreach (Control.Control control in flpDBMap.Controls)
                {
                    if (control.Name == lueLocalTable_Query.EditValue.ToString())
                    {
                        this.ActiveControl = control.Controls[0];
                        break;
                    }
                }
            }
        }
        #endregion

        #region 加载已配置好的表结构
        private void xtraTabControl1_SelectedPageChanged(object sender, DevExpress.XtraTab.TabPageChangedEventArgs e)
        {
            if (xtraTabControl1.SelectedTabPageIndex == 1)
            {
                DevExpress.Utils.WaitDialogForm dlg = new DevExpress.Utils.WaitDialogForm("正在加载表结构，请稍后……", new Size(250, 100));
                dlg.Show();
                try
                {
                    flpDBMap.Controls.Clear();
                    DirectoryInfo d = new DirectoryInfo(AppConst.dbMapPath);
                    FileInfo[] files = d.GetFiles();
                    foreach (FileInfo file in files)
                    {
                        Control.Control control = new Control.Control();
                        string[] array = file.Name.Split('&');
                        control.LocalTable = array[0];
                        control.Name = array[0];
                        control.ServerTable = array[0];
                        control.Data = ConfigHelper.LoadConfig<DBRelation>(file.FullName);
                        flpDBMap.Controls.Add(control);
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    dlg.Close();
                }
            }
        }
        #endregion

    }
}
