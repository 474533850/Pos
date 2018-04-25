using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Linq;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using System.Configuration;
using POS.Helper;
using System.Diagnostics;

namespace POS.Setting
{
    public partial class FormServerSetting : BaseForm
    {
        public FormServerSetting()
        {
            InitializeComponent();
        }


        private void FormServerSetting_Load(object sender, EventArgs e)
        {
            txtServerUrl.Text = ConfigurationManager.AppSettings["serviceUrl"].ToString();
        }
        private void btnConfirm_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtServerUrl.Text.Trim()))
            {
                MessagePopup.ShowInformation("服务器地址不能为空！");
                return;
            }
            Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            config.AppSettings.Settings["serviceUrl"].Value = txtServerUrl.Text.Trim();
            config.Save(ConfigurationSaveMode.Modified);
            ConfigurationManager.RefreshSection("appSettings");
            Restart();
        }

        private void Restart()
        {
            Application.ExitThread();
            Application.Exit();
            Application.Restart();
            Process.GetCurrentProcess().Kill();
        }

    }
}