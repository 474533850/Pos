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
using POS.Helper;
using POS.Common;
using DevExpress.XtraBars.Alerter;
using System.Threading.Tasks;
using POS.Model;
using System.Web.Script.Serialization;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraSplashScreen;
using POS.Common.utility;
using System.Configuration;
using System.Threading;
using System.Text.RegularExpressions;
using DevExpress.Utils;
using System.Diagnostics;
using POS.Common.Enum;

namespace POS
{
    public partial class BaseForm : DevExpress.XtraEditors.XtraForm
    {
        static PossettingBLL possettingBLL = new PossettingBLL();
        static JavaScriptSerializer js = new JavaScriptSerializer();
        public static Uri baseUrl_Push = null;
        private object obj = new object();
        public Form ParentFrom { get; set; }
        public BaseForm()
        {
            InitializeComponent();
            this.Load += BaseForm_Load;
            // CheckForIllegalCrossThreadCalls = false;
        }

        private void BaseForm_Load(object sender, EventArgs e)
        {
            if (!IsDesignMode())
            {
                baseUrl_Push = GlobalHelper.GetbaseUrl_Push();
            }
        }

        public static bool IsDesignMode()
        {
            bool returnFlag = false;

#if DEBUG
            if (LicenseManager.UsageMode == LicenseUsageMode.Designtime)
            {
                returnFlag = true;
            }
            else if (Process.GetCurrentProcess().ProcessName == "devenv")
            {
                returnFlag = true;
            }
#endif

            return returnFlag;
        }
        /// <summary>
        /// 读取报表导出文件名
        /// </summary>
        /// <param name="reportName"></param>
        /// <returns></returns>
        public string GetReportExportFileName(string reportName)
        {
            return string.Format("{0}({1})", reportName, DateTime.Now.ToString("yyyy-MM-dd HH_mm_ss"));
        }

        #region 表格导出Excel
        public void ExportToXls(GridView gv, string name)
        {
            if (gv.RowCount > 0)
            {
                System.Windows.Forms.SaveFileDialog saveFileDialog = new SaveFileDialog();
                saveFileDialog.Filter = "导出Excel (*.xls)|*.xls";
                saveFileDialog.FilterIndex = 0;
                saveFileDialog.RestoreDirectory = true;
                saveFileDialog.FileName = GetReportExportFileName(name);
                saveFileDialog.Title = "导出文件";

                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    DevExpress.XtraPrinting.XlsExportOptionsEx opEx = new DevExpress.XtraPrinting.XlsExportOptionsEx();
                    opEx.ExportType = DevExpress.Export.ExportType.WYSIWYG;
                    gv.ExportToXls(saveFileDialog.FileName, opEx);
                }
            }
        }
        #endregion


        #region 检查网络是否能连接到服务器
        /// <summary>
        /// 检查网络是否能连接到服务器
        /// </summary>
        /// <returns></returns>
        public static bool CheckConnect(bool isShowMsg = true)
        {
            DevExpress.Utils.WaitDialogForm dlg = null;
            int second = 10;
            try
            {
                //string formatStr = "正在连接服务器，请稍后……{0}s";
                //string msg = string.Format(formatStr, second);
                string msg = "正在连接服务器，请稍后……";
                //new Thread((ThreadStart)delegate
                //{
                dlg = new DevExpress.Utils.WaitDialogForm(msg, new Size(250, 100));
                dlg.Show();
                //    dlg.Show();
                //    while (second > 0)
                //    {
                //        Thread.Sleep(1000);
                //        second--;
                //        msg = string.Format(formatStr, second);
                //        dlg.SetCaption(msg);
                //    }
                //}).Start();
                UserModel user = RuntimeObject.CurrentUser;
                DateTime currentTime = DateTime.Now;
                bool result = SyncHelperBLL.CheckConnect(out currentTime, user.bookID, user.username, user.password, second);
                if (!result)
                {
                    // dlg.Invoke((EventHandler)delegate { dlg.Close(); });
                    if (isShowMsg)
                    {
                        MessagePopup.ShowError(AppConst.Connect_Msg);
                    }
                }
                //WaitFormClose(dlg);
                return result;
            }
            catch (Exception)
            {
                // WaitFormClose(dlg);
                // dlg.Invoke((EventHandler)delegate { dlg.Close(); });
                if (isShowMsg)
                {
                    MessagePopup.ShowError(AppConst.Connect_Msg);
                }
                return false;
            }
            finally
            {
                dlg.Close();
            }
        }

        public static bool CheckConnect(out DateTime serviceTime, string msgStr = "正在连接服务器", bool isShowMsg = false)
        {
            DevExpress.Utils.WaitDialogForm dlg = null;
            int second = 10;
            try
            {
                //string formatStr = msgStr + ("，请稍后……{0}s");
                //string msg = string.Format(formatStr, second);
                string msg = "正在连接服务器，请稍后……";
                dlg = new DevExpress.Utils.WaitDialogForm(msg, new Size(250, 100));
                dlg.Show();
                //new Thread((ThreadStart)delegate
                //{
                //    dlg = new DevExpress.Utils.WaitDialogForm(msg, new Size(250, 100));
                //    dlg.Show();
                //    while (second > 0)
                //    {
                //        Thread.Sleep(1000);
                //        second--;
                //        msg = string.Format(formatStr, second);
                //        dlg.SetCaption(msg);
                //    }
                //}).Start();
                UserModel user = RuntimeObject.CurrentUser;
                bool result = SyncHelperBLL.CheckConnect(out serviceTime, user.bookID, user.username, user.password, second);
                //  bool result = SyncHelperBLL.CheckConnect(out serviceTime, second);
                if (!result)
                {
                    if (isShowMsg)
                    {
                        MessagePopup.ShowError(AppConst.Connect_Msg);
                    }
                }
                //WaitFormClose(dlg);
                return result;
            }
            catch (Exception ex)
            {
                // WaitFormClose(dlg);
                //dlg.Invoke((EventHandler)delegate { dlg.Close(); });
                if (isShowMsg)
                {
                    MessagePopup.ShowError(AppConst.Connect_Msg);
                }
                serviceTime = DateTime.Now;
                return false;
            }
            finally
            {
                //try
                //{
                //    dlg.Invoke((MethodInvoker)delegate { dlg.Close(); });
                //}
                //catch (Exception)
                //{
                //}
                dlg.Close();
            }
        }

        private static void WaitFormClose(WaitDialogForm dlg)
        {
            try
            {
                if (dlg != null)
                {
                    //if (dlg.InvokeRequired)
                    //{
                    //    dlg.Invoke((MethodInvoker)delegate { dlg.Close(); });
                    //}
                    //else
                    //{
                    //    dlg.Close();
                    //}
                    dlg.Close();
                }
            }
            catch (Exception ex)
            {
            }
        }
        #endregion

        private void BaseForm_KeyUp(object sender, KeyEventArgs e)
        {
            if (this.Name == "FormLock")
            {
                return;
            }
            if (e.KeyCode == Keys.Escape)
            {
                this.Close();
            }
        }

        #region 同步单据数据
        public void SyncPOS(Form parentFrom)
        {
            Task.Factory.StartNew(() =>
            {
                UserModel user = RuntimeObject.CurrentUser;
                DateTime currentTime = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                if (SyncHelperBLL.CheckConnect(out currentTime, user.bookID, user.username, user.password))
                {
                    try
                    {
                        SyncHelperBLL.SyncData(RuntimeObject.CurrentUser.bookID, RuntimeObject.CurrentUser.xls, RuntimeObject.CurrentUser.usercode, new List<string> { "poshh", "ojie2", "jjie2", "tickoffmx", "ku2" });
                    }
                    catch (Exception ex)
                    {
                        Alert(parentFrom);

                    }
                }
                else
                {
                    Alert(parentFrom);
                }
            });

        }
        #endregion

        #region 同步数据
        /// <summary>
        /// 
        /// </summary>
        /// <param name="tables">表名称</param>
        /// <param name="id">同步单条记录的ID</param>
        /// <param name="posID">pos单据ID</param>
        /// <param name="msg">消息</param>
        /// <param name="isShowMsg">是否显示提示消息</param>
        /// <returns></returns>
        public bool SyncData(List<string> tables, int id = 0, string msg = "", bool isShowMsg = true, Guid? posID = null, string goodcode = "", string clntcode = "")
        {
            if (CheckConnect())
            {
                DevExpress.Utils.WaitDialogForm dlg = new DevExpress.Utils.WaitDialogForm("正在同步数据，请稍后……", new Size(250, 100));
                dlg.Show();
                try
                {
                    bool ck = SyncHelperBLL.Cksign(RuntimeObject.CurrentUser.bookID, RuntimeObject.CurrentUser.xls);
                    if (!ck)
                    {
                        MessagePopup.ShowInformation("验证失败，请检查密钥是否正确！");
                        return false;
                    }
                    bool result = SyncHelperBLL.SyncData(RuntimeObject.CurrentUser.bookID, RuntimeObject.CurrentUser.xls, RuntimeObject.CurrentUser.usercode, tables, id, posID, goodcode, clntcode);
                    dlg.Close();
                    if (!result)
                    {
                        MessagePopup.ShowInformation("网络不给力！");
                    }
                    if (isShowMsg)
                        MessagePopup.ShowInformation("同步数据成功！");
                    return true;
                }
                catch (Exception ex)
                {
                    dlg.Close();
                    if (string.IsNullOrEmpty(msg))
                    {
                        msg = string.Format("同步数据失败,错误信息：{0}！", ex.Message);
                    }
                    MessagePopup.ShowInformation(msg);
                    return false;
                }
                finally
                {
                    dlg.Close();
                }
            }
            else
            {
                return false;
            }
        }
        #endregion

        public void Alert(Form parentFrom, string msg = "")
        {
            try
            {
                System.Action action = () =>
                   {
                       AlertControl control = new AlertControl();
                       if (string.IsNullOrEmpty(msg))
                       {
                           msg = AppConst.Sync_Cashier_Msg;
                       }
                       AlertInfo alertInfo = new AlertInfo("温馨提示", msg);
                       control.FormLocation = AlertFormLocation.BottomRight;
                       control.Show(ParentForm, alertInfo);
                   };

                parentFrom.Invoke(action);
            }
            catch (Exception ex)
            {
            }
        }

        #region 获取贝壳机具号
        /// <summary>
        /// 获取贝壳机具号
        /// </summary>
        /// <returns></returns>
        public Dictionary<object, object> GetPayCodeMachineIDs()
        {
            PossettingModel entity = possettingBLL.GetPossettingByKey(AppConst.PAYCORE_MACHINE_IDS);
            if (entity != null && !string.IsNullOrEmpty(entity.xpvalue))
            {
                Dictionary<object, object> machineIDs = js.Deserialize<Dictionary<object, object>>(entity.xpvalue);
                return machineIDs;
            }
            else
            {
                return null;
            }
        }
        #endregion

        #region 获取移动支付通道,0为贝壳通道，1为原生通道
        /// <summary>
        /// 获取移动支付通道,0为贝壳通道，1为原生通道
        /// </summary>
        /// <returns></returns>
        public int GetPaymentChannel()
        {
            int result = 0;
            PossettingModel entity = possettingBLL.GetPossettingByKey(AppConst.PaymentChannel);
            if (entity != null && !string.IsNullOrEmpty(entity.xpvalue))
            {
                result = js.Deserialize<int>(entity.xpvalue);

            }
            return result;
        }
        #endregion

        #region 等待窗口
        public void CreateWaitDialog()
        {
            try
            {
                SplashScreenManager.ShowForm(typeof(DevExpress.XtraWaitForm.DemoWaitForm), false, true);
                SplashScreenManager.Default.SetWaitFormCaption("请稍候...");
                SplashScreenManager.Default.SetWaitFormDescription("正在加载窗体资源...");
            }
            catch (Exception)
            {
                CloseWaitDialog();
            }
        }

        public void SetWaitDialogCaption(string description)
        {
            if (SplashScreenManager.Default != null)
                SplashScreenManager.Default.SetWaitFormDescription(description);
        }

        public void CloseWaitDialog()
        {
            if (SplashScreenManager.Default != null) SplashScreenManager.CloseForm();
        }

        #endregion

        #region 获取回款记录
        /// <summary>
        /// 获取回款记录
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        public List<PosjhhModel> GetClntRepayments(string startDate, string endDate, string clntname = "")
        {
            List<PosjhhModel> posjhhs = new List<PosjhhModel>();
            if (CheckConnect())
            {
                ManualResetEvent wait = new ManualResetEvent(false);
                Uri baseUrl = new Uri(ConfigurationManager.AppSettings["serviceUrl"].ToString());

                Uri url = new Uri(baseUrl,
                   string.Format("pos/pull?sid={0}&mod=posjhh&xls={1}&clntname={2}&sdate={3}&edate={4} "
                   , RuntimeObject.CurrentUser.bookID, RuntimeObject.CurrentUser.xls, clntname, startDate, endDate));

                try
                {
                    string queryString = url.Query;
                    if (!string.IsNullOrEmpty(queryString))
                    {
                        url = new Uri(url.ToString() + "&sign=" + GlobalHelper.GetSign(queryString, GetSecretKey()));
                    }
                    HttpClient.GetAasync<PosjhhModel>(url.ToString(), null, syncResult =>
                     {
                         posjhhs = syncResult.datas;
                         wait.Set();
                     });
                    wait.WaitOne();
                    return posjhhs;
                }
                catch (Exception ex)
                {
                    wait.Set();
                    MessagePopup.ShowError("获取挂账结算明细账记录失败！");
                    return posjhhs;
                }
            }
            else
            {
                return posjhhs;
            }
        }
        #endregion

        #region 金额大写
        public static String ConvertToChinese(Decimal number)
        {
            var s = number.ToString("#L#E#D#C#K#E#D#C#J#E#D#C#I#E#D#C#H#E#D#C#G#E#D#C#F#E#D#C#.0B0A");
            var d = Regex.Replace(s, @"((?<=-|^)[^1-9]*)|((?'z'0)[0A-E]*((?=[1-9])|(?'-z'(?=[F-L\.]|$))))|((?'b'[F-L])(?'z'0)[0A-L]*((?=[1-9])|(?'-z'(?=[\.]|$))))", "${b}${z}");
            var r = Regex.Replace(d, ".", m => "负元空零壹贰叁肆伍陆柒捌玖空空空空空空空分角拾佰仟万亿兆京垓秭穰"[m.Value[0] - '-'].ToString());
            return r;
        }
        #endregion

        #region 获取时间戳
        /// <summary>  
        /// 获取时间戳  
        /// </summary>  
        /// <returns></returns>  
        public string GetTimeStamp()
        {
            TimeSpan ts = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0);
            return Convert.ToInt64(ts.TotalMilliseconds).ToString();
        }
        #endregion

        #region 获取权限
        /// <summary>  
        /// 获取权限  
        /// </summary>  
        /// <returns></returns>  
        public static bool GetPermission(Functions function, bool isShowMsg = true)
        {
            return true;
            string rightGroup = "店员组";
            bool result = false;
            PossettingModel entity = possettingBLL.GetPossettingByKey(AppConst.POS_AUTHS);
            if (entity != null && !string.IsNullOrEmpty(entity.xpvalue))
            {
                try
                {
                    Dictionary<object, Model.Permissions> rights = js.Deserialize<Dictionary<object, Model.Permissions>>(entity.xpvalue);
                    if (rights.ContainsKey(rightGroup))
                    {
                        Model.Permissions permission = rights[rightGroup];
                        if (!string.IsNullOrEmpty(permission.permission))
                        {
                            List<int> array = permission.permission.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries).Select(Int32.Parse).ToList();
                            if (array.Contains((int)function))
                            {
                                result = true;
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                }
            }
            if (!result)
            {
                if (isShowMsg)
                {
                    if (function == Functions.Print)
                    {
                        MessagePopup.ShowInformation("没有打印权限！");
                    }
                    else
                    {
                        MessagePopup.ShowInformation("没有该权限！");
                    }
                }
            }
            return result;
        }
        #endregion

        #region 获取密钥
        /// <summary>
        /// 获取密钥
        /// </summary>
        /// <returns></returns>
        public string GetSecretKey()
        {
            string secretKey = string.Empty;
            PossettingModel possetting = possettingBLL.GetPossettingByKey(AppConst.SecretKey);
            if (possetting != null)
            {
                secretKey = possetting.xpvalue.Trim();
            }
            return secretKey;
        }
        #endregion
    }
}