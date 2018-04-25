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
using POS.Common.Enum;

namespace POS.Client
{
    public partial class FormClientUpdate : BaseForm
    {
        //当前的会员
        public ClntModel currentClient { get; set; }

        ClientBLL clientBLL = new ClientBLL();

        public FormClientUpdate()
        {
            InitializeComponent();
        }

        private void FormClientUpdate_Load(object sender, EventArgs e)
        {
            InitData();
        }

        private void InitData()
        {
            lueclntclss.Properties.DataSource = clientBLL.GetClntclss();

            txtclntcode.EditValue = currentClient.clntcode;
            txtclntname.EditValue = currentClient.clntname;
            txtxpho.EditValue = currentClient.xpho;
            lueclntclss.EditValue = currentClient.clntclss;
            txtxzhe.EditValue = currentClient.xzhe;
            dtexbro.EditValue = currentClient.xbro;
            if (!string.IsNullOrEmpty(currentClient.xintime))
            {
                txtxintime.EditValue = Convert.ToDateTime(currentClient.xintime);
            }
            txtxadd.EditValue = currentClient.xadd;
            metxnotes.EditValue = currentClient.xnotes;

        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (!GetPermission(Functions.EClnt))
            {
                return;
            }
            if (!Checked()) return;
            try
            {
                ClntModel clnt = SetData();
                if (clientBLL.ExistClntPhone(txtxpho.Text.Trim(), clnt.ID))
                {
                    MessagePopup.ShowInformation("会员电话已被其他会员占用！");
                    txtxpho.Focus();
                    return;
                }
                bool result = clientBLL.UpdateClient(clnt);
                if (result)
                {
                    //实时同步单据抵扣会员积分
                    SyncData(new List<string> { "clnt" },0,string.Empty,false);
                    MessagePopup.ShowInformation("修改成功！");
                    this.DialogResult = DialogResult.OK;
                }
                else
                {
                    MessagePopup.ShowError("修改失败！");
                }
            }
            catch (Exception ex)
            {
                MessagePopup.ShowError(string.Format("未知错误！{0}", ex.Message));
            }
        }

        #region 检查是否通过
        private bool Checked()
        {

            if (string.IsNullOrEmpty(txtclntname.Text.Trim()))
            {
                MessagePopup.ShowInformation("请输入会员姓名！");
                txtclntname.Focus();
                return false;
            }
            if (string.IsNullOrEmpty(txtxpho.Text.Trim()))
            {
                MessagePopup.ShowInformation("请输入会员电话！");
                txtxpho.Focus();
                return false;
            }
            if (dtexbro.EditValue == null)
            {
                MessagePopup.ShowInformation("请输入会员生日！");
                txtxpho.Focus();
                return false;
            }
            return true;
        }
        #endregion

        #region 赋值
        private ClntModel SetData()
        {
            ClntModel clnt = new ClntModel();
            clnt.ID = currentClient.ID;
            clnt.clntname = txtclntname.Text.Trim();
            clnt.xpho = txtxpho.Text.Trim();

            if (lueclntclss.EditValue != null)
            {
                clnt.clntclss = lueclntclss.Text.Trim();
            }
            if (dtexbro.EditValue != null)
            {
                clnt.xbro = dtexbro.DateTime;
            }
            clnt.xadd = txtxadd.Text.Trim();
            clnt.xnotes = metxnotes.Text.Trim();
            return clnt;
        }
        #endregion
    }
}