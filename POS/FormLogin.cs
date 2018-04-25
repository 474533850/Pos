using DevExpress.XtraEditors.Controls;
using POS.BLL;
using POS.Common;
using POS.Common.utility;
using POS.Helper;
using POS.Model;
using POS.Setting;
using POS.Shifts;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace POS
{
    public partial class FormLogin : BaseForm
    {
        static ApplicationLogger logger = new ApplicationLogger(typeof(FormLogin).Name);
        UserBLL userBLL = new UserBLL();
        PosBanBLL posBanBLL = new PosBanBLL();
        CnkuBLL cnkuBLL = new CnkuBLL();
        PossettingBLL possettingBLL = new PossettingBLL();
        DBBLL dbBLL = new DBBLL();

        ConfigModel config = null;
        string xls = string.Empty;
        string xlsname = string.Empty;
        public FormLogin()
        {
            InitializeComponent();
            Init();
            UpdateDB();
            SyncHelperBLL.ReturnUploadstatus();
        }

        [DllImport("Kernel32.dll")]
        public static extern bool SetLocalTime(ref SYSTEMTIME Time);
        [DllImport("Kernel32.dll")]
        public static extern void GetLocalTime(ref SYSTEMTIME Time);

        private void Init()
        {
            config = ConfigHelper.LoadConfig(AppConst.ConfigFilePath);
            xls = config.xls;
            xlsname = config.xlsname;
            lueCnku.EditValue = null;
            lueCnku.Properties.DataSource = cnkuBLL.GetCnku(xls);
            lueUserCode.Properties.DataSource = userBLL.GetUsers(xls);
            PossettingModel entity = possettingBLL.GetPossettingByKey(AppConst.DEF_CKC);
            if (entity != null)
            {
                lueCnku.EditValue = entity.xpvalue;
            }
            Localizer.Active = new CustomXtraEditorsLocalizer();
        }

        #region 升级数据库
        /// <summary>
        /// 升级数据库
        /// </summary>
        private void UpdateDB()
        {
            string dbVersion = dbBLL.GetDBVersion();
            if (dbVersion != AppConst.current_dbVersion)
            {
                if (AppConst.current_dbVersion == "1.0.0.1")
                {
                    try
                    {
                        dbBLL.UpdateDBV1(AppConst.current_dbVersion);
                    }
                    catch (Exception ex)
                    {
                        logger.Info(ex.Message);
                        MessagePopup.ShowError("同步数据库失败！");
                        return;
                    }
                }
                if (AppConst.current_dbVersion == "1.0.0.2")
                {
                    try
                    {
                        dbBLL.UpdateDBV2(AppConst.current_dbVersion);
                    }
                    catch (Exception ex)
                    {
                        logger.Info(ex.Message);
                        MessagePopup.ShowError("同步数据库失败！");
                        return;
                    }
                }
                if (AppConst.current_dbVersion == "1.0.0.3")
                {
                    try
                    {
                        dbBLL.UpdateDBV3(AppConst.current_dbVersion);
                    }
                    catch (Exception ex)
                    {
                        logger.Info(ex.Message);
                        MessagePopup.ShowError("同步数据库失败！");
                        return;
                    }
                }
                if (AppConst.current_dbVersion == "1.0.0.4")
                {
                    try
                    {
                        dbBLL.UpdateDBV4(AppConst.current_dbVersion);
                    }
                    catch (Exception ex)
                    {
                        logger.Info(ex.Message);
                        MessagePopup.ShowError("同步数据库失败！");
                        return;
                    }
                }
                if (AppConst.current_dbVersion == "1.0.0.5")
                {
                    try
                    {
                        dbBLL.UpdateDBV5(AppConst.current_dbVersion);
                    }
                    catch (Exception ex)
                    {
                        logger.Info(ex.Message);
                        MessagePopup.ShowError("同步数据库失败！");
                        return;
                    }
                }

            }
        }
        #endregion

        private void btnLogin_Click(object sender, EventArgs e)
        {
            if (!Checked()) return;
            bool result = SetLocalTime();
            try
            {
                CnkuModel cnku = lueCnku.GetSelectedDataRow() as CnkuModel;
                string cnkucode = string.Empty;
                string cnkuname = string.Empty;
                if (cnku != null)
                {
                    cnkucode = cnku.cnkucode;
                    cnkuname = cnku.cnkuname;
                }
                string password = txtPassword.Text.Trim() == string.Empty ? string.Empty : MD5Helper.GetMd5Hash(txtPassword.Text.Trim());
                UserModel user = userBLL.Login(lueUserCode.EditValue.ToString(), password);
                if (user != null)
                {
                    string posnono = posBanBLL.IsShift();
                    if (string.IsNullOrEmpty(posnono))
                    {
                        FormBegin frm = new FormBegin();
                        frm.ShowDialog();
                        PosbanModel entity = new PosbanModel();
                        entity.xposset = string.Empty;
                        entity.posnono = ClientCodeHelper.GenerateClientCode();
                        entity.posposi = xls;
                        entity.posbcode = string.Empty;
                        entity.xopcode = user.usercode;
                        entity.xopname = user.username;
                        entity.xjienow = frm.Money;
                        posBanBLL.AddPosBan(entity);
                        posnono = entity.posnono;

                    }
                    user.xls = xls;
                    user.xlsname = xlsname;
                    user.cnkucode = cnkucode;
                    user.cnkuname = cnkuname;
                    user.posnono = posnono;
                    user.bookID = config.sid;
                    user.password = txtPassword.Text.Trim() == string.Empty ? string.Empty : MD5Helper.GetMd5Hash(txtPassword.Text.Trim());
                    user.passwordExpress = txtPassword.Text.Trim();
                    RuntimeObject.CurrentUser = user;
                    DateTime currentTime = DateTime.Now;
                    if (result)
                    {
                        //SyncData(AppConst.AllTableNames, 0, string.Empty, false);
                    }
                    this.DialogResult = DialogResult.OK;
                }
                else
                {
                    MessagePopup.ShowInformation("工号或者密码不正确！");
                }
            }
            catch (Exception ex)
            {
                string str = GetExceptionMsg(ex, e.ToString());
                logger.Info(str);
                MessagePopup.ShowError(string.Format("未知错误！{0}", ex.Message));
            }

        }

        private bool Checked()
        {
            bool success = true;
            if (lueCnku.EditValue == null || string.IsNullOrEmpty(lueCnku.Text.Trim()))
            {
                MessagePopup.ShowInformation("请选择仓库！");
                success = false;
            }
            else if (lueUserCode.EditValue == null || string.IsNullOrEmpty(lueUserCode.EditValue.ToString().Trim()))
            {
                MessagePopup.ShowInformation("请输入工号！");
                success = false;
            }
            //else if (string.IsNullOrEmpty(txtPassword.Text.Trim()))
            //{
            //    MessagePopup.ShowInformation("请输入密码！");
            //    success = false;
            //}
            return success;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private bool SetLocalTime()
        {
            DateTime serviceTime = DateTime.Now;
            bool result = CheckConnect(out serviceTime, "正在获取服务器时间");

            if (result)
            {
                DateTime currentTime = DateTime.Now;

                TimeSpan timeSpan = serviceTime - currentTime;

                if (Math.Abs(timeSpan.TotalMinutes) >= 5)
                {
                    MessagePopup.ShowInformation("当前系统时间和服务器时间不一致，系统时间即将被重置！");
                    try
                    {
                        //转换System.DateTime到SYSTEMTIME
                        SYSTEMTIME st = new SYSTEMTIME();
                        st.FromDateTime(serviceTime);
                        //调用Win32 API设置系统时间
                        SetLocalTime(ref st);
                    }
                    catch (Exception)
                    {
                        MessagePopup.ShowError("设置系统时间失败，请手动设置！");
                    }
                }
            }
            return result;
        }

        private void btnSetting_Click(object sender, EventArgs e)
        {
            //FormServerSetting frm = new FormServerSetting();
            //frm.ShowDialog();

            FormDBMap frm = new FormDBMap();
            frm.ShowDialog();
        }

        static string GetExceptionMsg(Exception ex, string backStr)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("****************************异常文本****************************");
            sb.AppendLine("【出现时间】：" + DateTime.Now.ToString());
            if (ex != null)
            {
                sb.AppendLine("【异常类型】：" + ex.GetType().Name);
                sb.AppendLine("【异常信息】：" + ex.Message);
                sb.AppendLine("【堆栈调用】：" + ex.StackTrace);
            }
            else
            {
                sb.AppendLine("【未处理异常】：" + backStr);
            }
            sb.AppendLine("***************************************************************");
            return sb.ToString();
        }
    }
}
