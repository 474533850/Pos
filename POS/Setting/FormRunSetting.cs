using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Linq;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using POS.Helper;
using POS.Model;
using POS.Common.utility;
using System.IO;
using POS.Common;
using POS.BLL;
using System.Configuration;

namespace POS.Setting
{
    public partial class FormRunSetting : BaseForm
    {
        LsBLL lsBLL = new LsBLL();
        PossettingBLL possettingBLL = new PossettingBLL();
        ApplicationLogger logger = new ApplicationLogger(typeof(FormRunSetting).Name);
        public FormRunSetting()
        {
            InitializeComponent();
            tab.ShowTabHeader = DevExpress.Utils.DefaultBoolean.False;
            this.AcceptButton = btnNext;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Application.ExitThread();
        }

        #region 确认
        private void btnConfirm_Click(object sender, EventArgs e)
        {
            if (txtSID.Text.Trim() == string.Empty)
            {
                MessagePopup.ShowInformation("请输入账套！");
            }
            else if (lueDepartment.EditValue == null || string.IsNullOrEmpty(lueDepartment.Text.Trim()))
            {
                MessagePopup.ShowInformation("请选择分部！");
            }
            else
            {
                ConfigModel config = new ConfigModel();
                config.sid = txtSID.Text.Trim();
                config.xls = lueDepartment.EditValue.ToString();
                config.xlsname = lueDepartment.Text.Trim();

                string configPath = Path.Combine(AppConst.ConfigPath, AppConst.ConfigName);
                ConfigHelper.SaveConfig(configPath, config);

                this.DialogResult = DialogResult.OK;
            }
        }
        #endregion

        #region 上一步
        private void btnPre_Click(object sender, EventArgs e)
        {
            tab.SelectedTabPage = xtraTabPage1;
        }
        #endregion

        #region 下一步
        private void btnNext_Click(object sender, EventArgs e)
        {
            if (txtSID.Text.Trim() == string.Empty)
            {
                MessagePopup.ShowInformation("请输入账套！");
            }
            else if (string.IsNullOrEmpty(memoKey.Text.Trim()))
            {
                MessagePopup.ShowInformation("请输入密钥！");
            }
            else
            {
                tab.SelectedTabPage = xtraTabPage2;
                List<PossettingModel> possettings = new List<PossettingModel>();
                PossettingModel entity = new PossettingModel();
                entity.issys = false;
                entity.xpname = AppConst.SecretKey;
                entity.xpvalue = memoKey.Text.Trim();
                possettings.Add(entity);
                possettingBLL.AddPossetting(possettings);
                SyncDepartment();
            }
        }
        #endregion

        #region 同步分部

        private void SyncDepartment()
        {
            DateTime currentTime = DateTime.Now;
            string username = ConfigurationManager.AppSettings["username"].ToString();
            string password = ConfigurationManager.AppSettings["password"].ToString();
            if (SyncHelperBLL.CheckConnect(out currentTime, txtSID.Text.Trim(), username, password))
            {
                DevExpress.Utils.WaitDialogForm dlg = new DevExpress.Utils.WaitDialogForm("正在同步分部信息，请稍后……", new Size(250, 100));
                dlg.Show();
                try
                {
                    SyncHelperBLL.SyncData(txtSID.Text.Trim(), "", "",username,password, new List<string> { "ls" });
                }
                catch (Exception ex)
                {
                    dlg.Close();
                    logger.Info(ex.Message);
                    MessagePopup.ShowInformation("同步分部信息失败,请重试！");
                }
                finally
                {
                    dlg.Close();
                    System.Threading.Thread.Sleep(1000);
                    lueDepartment.Properties.DataSource = lsBLL.GetLs();
                }
            }
            else
            {
                MessagePopup.ShowError(AppConst.Connect_Msg);
            }
        }
        #endregion

        private void tab_SelectedPageChanged(object sender, DevExpress.XtraTab.TabPageChangedEventArgs e)
        {
            if (tab.SelectedTabPageIndex == 0)
            {
                this.AcceptButton = btnNext;
            }
            else
            {
                this.AcceptButton = btnConfirm;
            }
        }
    }
}