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
using POS.BLL;
using POS.Common.utility;
using System.Web.Script.Serialization;
using System.Web;
using System.Configuration;
using POS.Common.Enum;
using POS.Common;

namespace POS.Client
{
    public partial class FormClientAdd : BaseForm
    {
        ClientBLL clientBLL = new ClientBLL();
        JavaScriptSerializer js = new JavaScriptSerializer();
        static ApplicationLogger logger = new ApplicationLogger(typeof(FormClientAdd).Name);

        public ClntModel CurrentClient { get { return client; } }

        private ClntModel client;
       
        public FormClientAdd()
        {
            InitializeComponent();
            Init();
        }

        private void Init()
        {
            dteXbro.EditValue = DateTime.Now;
            txtClntCode.Text = ClientCodeHelper.GenerateClientCode();
            lueClntType.Properties.DataSource = clientBLL.GetClnttype();
            lueClntType.EditValue = "门店会员";
        }

        #region 确认
        private void btnConfirm_Click(object sender, EventArgs e)
        {
            if (!GetPermission(Functions.AClnt))
            {
                return;
            }
            if (!Checked()) return;

            if (CheckConnect())
            {
                client = SetData();
                try
                {
                    ClntModel clnt = AddClnt(client);
                    if (clnt != null)
                    {
                        client.clntcode = clnt.clntcode;
                        client.SID = clnt.SID;
                        client.xtableid = clnt.xtableid;
                        clientBLL.AddClient(client);
                        this.DialogResult = DialogResult.OK;
                    }
                    else
                    {
                        MessagePopup.ShowError("添加失败！");
                    }
                }
                catch (Exception ex)
                {
                    MessagePopup.ShowError(string.Format("未知错误！{0}", ex.Message));
                }
            }
        }
        #endregion

        #region 检查是否通过
        private bool Checked()
        {
            if (string.IsNullOrEmpty(txtClntCode.Text.Trim()))
            {
                MessagePopup.ShowInformation("请输入会员卡号！");
                txtClntCode.Focus();
                return false;
            }
            if (!string.IsNullOrEmpty(txtPassword.Text.Trim()))
            {
                if (txtPassword.Text.Trim() != txtConPassword.Text.Trim())
                {
                    MessagePopup.ShowInformation("两次输入会员密码不一致！");
                    txtPassword.Focus();
                    return false;
                }

            }
            if (lueClntType.EditValue == null)
            {
                MessagePopup.ShowInformation("请选择会员类别！");
                return false;
            }
            if (string.IsNullOrEmpty(txtClntName.Text.Trim()))
            {
                MessagePopup.ShowInformation("请输入会员姓名！");
                txtClntName.Focus();
                return false;
            }
            if (string.IsNullOrEmpty(txtXpho.Text.Trim()))
            {
                MessagePopup.ShowInformation("请输入会员电话！");
                txtXpho.Focus();
                return false;
            }
            if (dteXbro.EditValue == null)
            {
                MessagePopup.ShowInformation("请输入会员生日！");
                dteXbro.Focus();
                return false;
            }
            if (clientBLL.ExistClntPhone(txtXpho.Text.Trim(), null))
            {
                MessagePopup.ShowInformation("会员电话已被其他会员占用！");
                txtXpho.Focus();
                return false;
            }
            return true;
        }
        #endregion

        #region 赋值

        private ClntModel SetData()
        {
            ClntModel clnt = new ClntModel();
            clnt.ID = Guid.NewGuid();
            clnt.clntcode = txtClntCode.Text.Trim();
            clnt.password = txtPassword.Text.Trim();
            if (lueClntType.EditValue != null)
            {
                clnt.clnttype = lueClntType.Text.Trim();
            }
            clnt.clntname = txtClntName.Text.Trim();
            clnt.xpho = txtXpho.Text.Trim();
            if (dteXbro.EditValue != null)
            {
                clnt.xbro = DateTime.Parse(dteXbro.DateTime.ToString("yyyy-MM-dd"));
            }
            clnt.xadd = txtXadd.Text.Trim();
            clnt.xnotes = memoXnotes.Text.Trim();
            //分部
            clnt.xls = RuntimeObject.CurrentUser.xls;
            clnt.xlsname = RuntimeObject.CurrentUser.xlsname;
            clnt.xinname = RuntimeObject.CurrentUser.usercode;
            ClntclssModel defaultClntclss = clientBLL.GetDefaultClntclss();
            if (defaultClntclss != null)
            {
                clnt.clntclss = defaultClntclss.clntclss;
                clnt.xzhe = defaultClntclss.xzhe;
            }
            else
            {
                clnt.clntclss = string.Empty;
                clnt.xzhe = 100;
            }
            clnt.xversion = double.Parse(GetTimeStamp());

            return clnt;
        }
        #endregion

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private ClntModel AddClnt(ClntModel clnt)
        {
            List<ClntModel> clnts = new List<ClntModel>();
            clnts.Add(clnt);
            var query = from p in clnts
                        select new
                        {
                            ID = p.ID,
                            SID = p.SID,
                            clnttype = p.clnttype,
                            clntcode = p.clntcode,
                            clntname = p.clntname,
                            clntclss = p.clntclss,
                            xbro = p.xbro.HasValue ? p.xbro.Value.ToString("yyyy-MM-dd") : string.Empty,
                            xpho = p.xpho,
                            xintime = p.xintime,
                            xadd = p.xadd,
                            xnotes = p.xnotes,
                            xls = p.xls,
                            xlsname = p.xlsname,
                            xversion = p.xversion,
                        };

            string json = js.Serialize(query);
            UserModel user = RuntimeObject.CurrentUser;
            //string postData = "sid=" + RuntimeObject.CurrentUser.bookID;
            //postData += "&mod=" + "clnt";
            //postData += "&xls=" + RuntimeObject.CurrentUser.xls;
            //postData += "&usercode=" + RuntimeObject.CurrentUser.usercode;
            //postData += "&data=" + HttpUtility.UrlEncode(json, Encoding.UTF8);
            
            string postData = GlobalHelper.GetPostData(user.bookID, "clnt", user.xls, user.usercode, json, GetSecretKey());
            byte[] byteArray = Encoding.UTF8.GetBytes(postData);

            DevExpress.Utils.WaitDialogForm dlg = new DevExpress.Utils.WaitDialogForm("正在处理数据，请稍后……", new Size(250, 100));
            dlg.Show();
            try
            {
                SyncResultModel<ClntModel> syncResult = HttpClient.PostAasync<ClntModel>(baseUrl_Push.ToString(), null, postData);
                List<ClntModel> datas = syncResult.datas;
                if (datas != null && datas.Count > 0)
                {
                    return datas.FirstOrDefault();
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
                return null;
            }
            finally
            {
                dlg.Close();
            }
        }

    }
}