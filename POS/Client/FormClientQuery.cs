using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Linq;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using POS.Model;
using POS.BLL;
using POS.Helper;

namespace POS.Client
{
    public partial class FormClientQuery : BaseForm
    {
        ClientBLL clientBLL = new ClientBLL();
        /// <summary>
        /// 当前选择的会员
        /// </summary>
        public ClntModel currentClient { get; set; }
        public FormClientQuery(List<ClntModel> clinets,string key)
        {
            InitializeComponent();
            bdsData.DataSource = clinets;
            txtSearch.Text = key;
            Init();
            this.ActiveControl = gd;
            this.gv.GotFocus += Gv_GotFocus;
            this.gv.LostFocus += Gv_LostFocus;
        }

        private void Init()
        {
            rlueJF.DataSource = clientBLL.GetJjie2();
            rlueBlance.DataSource = clientBLL.GetOjie2();
        }
        private void Gv_LostFocus(object sender, EventArgs e)
        {
            this.AcceptButton = null;
        }

        private void Gv_GotFocus(object sender, EventArgs e)
        {
            this.AcceptButton = btnConfirm;
        }

        private void Search()
        {
            if (string.IsNullOrEmpty((txtSearch.Text.Trim())))
            {
                gv.Focus();
                MessagePopup.ShowInformation("请输入搜索条件！");
                txtSearch.Focus();
                return;
            }

            DevExpress.Utils.WaitDialogForm dlg = new DevExpress.Utils.WaitDialogForm("正在查询会员信息，请稍后……", new Size(250, 100));
            dlg.Show();
            List<ClntModel> clinets = clientBLL.GetClientByKey(txtSearch.Text.Trim());
            bdsData.DataSource = clinets;
            dlg.Close();
        }

        private void txtSearch_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                Search();
            }
        }
        private void btnQuery_Click(object sender, EventArgs e)
        {
            Search();
        }

        private void btnConfirm_Click(object sender, EventArgs e)
        {
            ClntModel client = bdsData.Current as ClntModel;
            if (client != null)
            {
                List<Ojie2Model> jfs = (rlueBlance.DataSource as List<Ojie2Model>);
                var query = jfs.Where(r => r.clntcode == client.clntcode).FirstOrDefault();
                if (query != null)
                {
                    client.balance = query.xjie;
                }
                else
                {
                    client.balance = 0;
                }
                List<Jjie2Model> integrals = (rlueJF.DataSource as List<Jjie2Model>);
                var integral = integrals.Where(r => r.clntcode == client.clntcode).FirstOrDefault();
                if (integral != null)
                {
                    client.integral = integral.xjie;
                }
                else
                {
                    client.integral = 0;
                }
                currentClient = client;
                this.DialogResult = DialogResult.OK;
            }
        }

        private void gv_RowClick(object sender, DevExpress.XtraGrid.Views.Grid.RowClickEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                if (gv.FocusedColumn == colRefresh)
                {
                    rpicRefresh_Click(null, null);
                }
                else
                {
                    if (e.Clicks == 2)
                    {
                        btnConfirm_Click(null, null);
                    }
                }
            }
        }

        private void gv_CustomUnboundColumnData(object sender, DevExpress.XtraGrid.Views.Base.CustomColumnDataEventArgs e)
        {
            if (e.Column == colRefresh)
            {
                e.Value = DevExpress.Images.ImageResourceCache.Default.GetImage("images/actions/refresh_16x16.png");
            }
        }

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

        private void FormClientQuery_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Down)
            {
                if (this.ActiveControl.Parent != null && this.ActiveControl.Parent.GetType() == typeof(DevExpress.XtraEditors.TextEdit))
                {
                    gv.Focus();
                }
            }
            else if (e.KeyCode == Keys.Up)
            {
                if (gv.FocusedRowHandle == 0)
                {
                    txtSearch.Focus();
                }
            }
        }
    }
}