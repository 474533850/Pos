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
using POS.Common;
using System.Threading;
using POS.Helper;
using System.Web.Script.Serialization;
using POS.Common.Enum;

namespace POS.Client
{
    public partial class FormClientDetail : BaseForm
    {

        //当前的会员
        public ClntModel currentClient { get; set; }

        ClientBLL clientBLL = new ClientBLL();
        PossettingBLL possettingBLL = new PossettingBLL();
        JavaScriptSerializer js = new JavaScriptSerializer();

        object[] parameters = null;

        private Guid ID;
        public FormClientDetail(Guid ID, params object[] parameters)
        {
            InitializeComponent();
            this.ID = ID;
            this.parameters = parameters;
            InitData(ID);

            if (parameters != null && parameters.Length > 0)
            {
                btnSubmit.Text = "取消会员";
                btnSubmit.Tag = false;
            }
            else
            {
                btnSubmit.Text = "选中会员";
                btnSubmit.Tag = true;
            }
        }

        private void InitData(Guid ID)
        {
            ClntModel clent = clientBLL.GetClientByID(ID);
            txtclntcode.EditValue = clent.clntcode;
            txtclntname.EditValue = clent.clntname;
            txtxpho.EditValue = clent.xpho;
            txtclntclss.EditValue = clent.clntclss;
            txtxzhe.EditValue = clent.xzhe;
            txtxcontime.EditValue = clent.xcontime;
            if (clent.xbro.HasValue)
            {
                txtxbro.EditValue = clent.xbro.Value.ToString("m");
            }

            if (!string.IsNullOrEmpty(clent.xintime))
            {
                txtxintime.EditValue = Convert.ToDateTime(clent.xintime);
            }
            txtxadd.EditValue = clent.xadd;
            metxnotes.EditValue = clent.xnotes;
            txtXlsName.EditValue = clent.xlsname;
            decimal ojie2 = clientBLL.GetOjie2(clent.clntcode);
            decimal jjie2 = clientBLL.GetJjie2(clent.clntcode);
            bteBalance.EditValue = ojie2;
            bteIntegral.EditValue = jjie2;
            currentClient = clent;
        }

        private bool ZBIsEnabled()
        {
            bool result = false;
            if (parameters != null && parameters.Length > 1)
            {
                if (bool.TryParse(parameters[1].ToString(),out result)) { }                
            }
            return result;
        }

        #region 提交
        private void btnSubmit_Click(object sender, EventArgs e)
        {
            if (btnSubmit.Tag != null && bool.Parse(btnSubmit.Tag.ToString()) == false)
            {
                currentClient = null;
            }
            this.DialogResult = DialogResult.OK;
        }
        #endregion

        #region 编辑
        private void btnModify_Click(object sender, EventArgs e)
        {
            if (!GetPermission(Functions.EClnt))
            {
                return;
            }
            FormClientUpdate frm = new FormClientUpdate();
            frm.currentClient = currentClient;
            if (frm.ShowDialog() == DialogResult.OK)
            {
                InitData(ID);
            }
        }
        #endregion

        #region 快捷键
        private void FormClientDetail_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F5)
            {
                //编辑
                btnModify_Click(null, null);
            }
            else if (e.KeyCode == Keys.F3)
            {
                //充值
                btnRecharge_Click(null, null);
            }
        }

        #endregion

        #region 会员消费历史
        private void btnConsumption_Click(object sender, EventArgs e)
        {
            if (CheckConnect())
            {
                FormClinetConsumption frm = new FormClinetConsumption(currentClient.clntcode);
                frm.ShowDialog();
            }
        }
        #endregion

        #region 充值

        private void btnRecharge_Click(object sender, EventArgs e)
        {
            if (!GetPermission(Functions.Recharge))
            {
                return;
            }
            if (!ZBIsEnabled())
            {
                return;
            }
            FormClientRecharge frm = new FormClientRecharge(currentClient);
            if (frm.ShowDialog() == DialogResult.OK)
            {
                System.Threading.Thread.Sleep(1000);
                decimal ojie2 = clientBLL.GetOjie2(currentClient.clntcode);
                bteBalance.EditValue = ojie2;
            }
        }

        private void bteBalance_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            btnRecharge_Click(null, null);
        }
        #endregion

        #region 积分兑换
        private void bteIntegral_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            if (!GetPermission(Functions.Cashier))
            {
                return;
            }
            if (!ZBIsEnabled())
            {
                return;
            }
            PossettingModel possetting = possettingBLL.GetPossettingByKey(AppConst.INTEGRAL_RULES);
            if (possetting != null)
            {
                if (!string.IsNullOrEmpty(possetting.xpvalue))
                {
                    Dictionary<object, object> uclsspricsDic = js.Deserialize<Dictionary<object, object>>(possetting.xpvalue);
                    var query = uclsspricsDic.Where(r => r.Key.ToString() == "type").FirstOrDefault();
                    decimal value = 0;
                    if (decimal.TryParse(query.Value.ToString(), out value))
                    {
                        if (value != 0)
                        {
                            FormClientExchange frm = new FormClientExchange(currentClient);
                            if (frm.ShowDialog() == DialogResult.OK)
                            {
                                Thread.Sleep(1000);
                                decimal jjie2 = clientBLL.GetJjie2(currentClient.clntcode);
                                bteIntegral.EditValue = jjie2;
                            }
                        }
                        else
                        {
                            MessagePopup.ShowInformation(AppConst.Open_integral_Msg);
                        }
                    }
                    else
                    {
                        MessagePopup.ShowInformation(AppConst.Open_integral_Msg);
                    }

                }
                else
                {
                    MessagePopup.ShowInformation(AppConst.Open_integral_Msg);
                }
            }
            else
            {
                MessagePopup.ShowInformation(AppConst.Open_integral_Msg);
            }
        }
        #endregion

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}