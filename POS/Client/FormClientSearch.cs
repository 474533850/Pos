using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Linq;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using POS.BLL;
using POS.Model;
using POS.Helper;

namespace POS.Client
{
    public partial class FormClientSearch : BaseForm
    {
        ClientBLL clientBLL = new ClientBLL();

        public ClntModel currentClient { get; set; }
        //是否会员充值
        private bool isRecharge = false;
        public FormClientSearch(bool isRecharge)
        {
            InitializeComponent();
            gd.Visible = false;
            this.isRecharge = isRecharge;
            Init();
        }

        private void Init()
        {
            rlueJF.DataSource = clientBLL.GetJjie2();
            rlueBlance.DataSource = clientBLL.GetOjie2();
        }
        private void btnEditSearch_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            if (e.Button.Index == 0)
            {
                if (bteSearch.EditValue != null)
                {
                    //清除
                    bteSearch.EditValue = null;
                    Search();
                }
            }
            else
            {
                //查询
                Search();
            }
        }
        private void bteSearch_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                Search();
            }
        }

        private void Search()
        {
            if (string.IsNullOrEmpty((bteSearch.Text.Trim())))
            {
                gv.Focus();
                MessagePopup.ShowInformation("请输入搜索条件！");
                bteSearch.Focus();
                return;
            }
            DevExpress.Utils.WaitDialogForm dlg = new DevExpress.Utils.WaitDialogForm("正在查询会员信息，请稍后……", new Size(250, 100));
            dlg.Show();
            List<ClntModel> clinets = clientBLL.GetClientByKey(bteSearch.Text.Trim());
            dlg.Close();
            bdsData.DataSource = clinets;
            if (clinets.Count == 0)
            {
                MessagePopup.ShowInformation("查询不到相关会员！");
            }
            else if (clinets.Count == 1)
            {
                //跳转到明细
                ToClientDetail(clinets.First());
            }
            else if (clinets.Count > 1)
            {
                gd.Visible = true;
            }
        }

        private void gv_RowClick(object sender, DevExpress.XtraGrid.Views.Grid.RowClickEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                if (gv.FocusedColumn == colRefresh)
                {
                    rpicRefresh_Click(null,null);
                }
                else
                {
                    ClntModel client = bdsData.Current as ClntModel;
                    if (client != null)
                    {
                        //跳转到明细
                        ToClientDetail(client);
                    }
                }
            }
        }

        #region 跳转到明细
        private void ToClientDetail(ClntModel client)
        {
                if (isRecharge)
                {
                    FormClientRecharge frm = new FormClientRecharge(client);
                    frm.ShowDialog();
                    this.DialogResult = DialogResult.OK;
                }
                else
                {
                    FormClientDetail frm = new FormClientDetail(client.ID);
                    if (frm.ShowDialog() == DialogResult.OK)
                    {
                        currentClient = frm.currentClient;
                        this.DialogResult = DialogResult.OK;
                    }
                }
        }
        #endregion

        #region 自定义刷新按钮
        private void gv_CustomUnboundColumnData(object sender, DevExpress.XtraGrid.Views.Base.CustomColumnDataEventArgs e)
        {
            if (e.Column == colRefresh)
            {
                e.Value = DevExpress.Images.ImageResourceCache.Default.GetImage("images/actions/refresh_16x16.png");
            }
        }
        #endregion

        #region 同步数据
        private void rpicRefresh_Click(object sender, EventArgs e)
        {
            if (gv.FocusedColumn == colRefresh)
            {
                ClntModel clnt = bdsData.Current as ClntModel;
                if (clnt != null)
                {
                    bool result = SyncData(new List<string> { "jjie2", "ojie2" }, 0, string.Empty, true, null, string.Empty, clnt.clntcode);
                    if (result)
                    {
                        Init();
                    }
                }
            }
        }
        #endregion

        private void FormClientSearch_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Down)
            {
                if (this.ActiveControl.Parent != null && this.ActiveControl.Parent.GetType() == typeof(DevExpress.XtraEditors.ButtonEdit))
                {
                    gv.Focus();
                }
            }
            else if (e.KeyCode == Keys.Up)
            {
                if (gv.FocusedRowHandle == 0)
                {
                    bteSearch.Focus();
                }
            }
        }
    }
}