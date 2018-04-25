using DevExpress.Skins;
using POS.BLL;
using POS.Common;
using POS.Common.utility;
using POS.Helper;
using POS.Model;
using POS.Sale;
using POS.Setting;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace POS
{
    static class Program
    {
        static ApplicationLogger logger = new ApplicationLogger(typeof(Program).Name);
        static UpgradeModel upgrade = new UpgradeModel();
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            //检测应用程序名，不允许修改成其他文件名
            if (ProcessHelper.CheckFileName(typeof(Program)))
            {
                //检测进程数，只能开启一个进程
                if (ProcessHelper.CheckProcess(1))
                {
                    Application.EnableVisualStyles();
                    Application.SetCompatibleTextRenderingDefault(false);
                    //处理未捕获的异常   
                    Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);
                    //处理UI线程异常 
                    Application.ThreadException += Application_ThreadException;
                    //处理非UI线程异常 
                    AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
                    //改变所有的组件字体
                    // DevExpress.Utils.AppearanceObject.DefaultFont = new Font("Tahoma", 12);
                    //默认皮肤
                    SkinManager.EnableFormSkins();
                    string skinName = Properties.Settings.Default.SkinName;
                    if (string.IsNullOrEmpty(skinName))
                    {
                        DevExpress.LookAndFeel.UserLookAndFeel.Default.SetSkinStyle("Xmas 2008 Blue");
                    }
                    else
                    {
                        DevExpress.LookAndFeel.UserLookAndFeel.Default.SetSkinStyle(skinName);
                    }
                    DevExpress.XtraSplashScreen.SplashScreenManager.ShowForm(typeof(FormSplashScreen), true, true);
                    Upgrade();
                    SetDB();
                    //创建配置文件夹
                    if (!Directory.Exists(AppConst.ConfigPath))
                    {
                        Directory.CreateDirectory(AppConst.ConfigPath);
                    }

                    if (File.Exists(AppConst.ConfigFilePath))
                    {
                        ConfigModel config = ConfigHelper.LoadConfig(AppConst.ConfigFilePath);
                        if (config != null)
                        {
                            Login(config.sid, config.xls);
                        }
                        else
                        {
                            Setting();
                        }
                    }
                    else
                    {
                        Setting();
                    }
                }
                else
                {
                    //MessagePopup.ShowInformation("只允许打开一个应用程序,请勿重开！");
                    if (DialogResult.Yes == MessagePopup.ShowQuestion("只允许打开一个应用程序，是否关闭其他进程？"))
                    {
                        if (!ProcessHelper.KillProcess())
                        {
                            MessagePopup.ShowError("关闭其他进程失败！");
                            return;
                        }
                    }
                }
            }
            else
            {
                MessagePopup.ShowError("不允许修改文件名！");
                return;
            }
        }

        #region 设置分部
        /// <summary>
        /// 设置分部
        /// </summary>
        private static void Setting()
        {
            //打开分部设置界面
            FormRunSetting frm = new FormRunSetting();
            if (frm.ShowDialog() == DialogResult.OK)
            {
                ConfigModel config = ConfigHelper.LoadConfig(AppConst.ConfigFilePath);
                if (config != null)
                {
                    Login(config.sid, config.xls);
                }
                else
                {
                    Application.ExitThread();
                }
            }
            else
            {
                Application.ExitThread();
            }
        }
        #endregion

        #region 登录
        private static void Login(string sid, string xls)
        {
            DevExpress.XtraSplashScreen.SplashScreenManager.CloseForm();
            DateTime currentTime = DateTime.Now;
            string username = ConfigurationManager.AppSettings["username"].ToString();
            string password = ConfigurationManager.AppSettings["password"].ToString();

            if (SyncHelperBLL.CheckConnect(out currentTime,sid,username,password))
            {
                //DateTime startTime = POS.Properties.Settings.Default.StartTime;
                //if (startTime == DateTime.MinValue)
                //{
                //    startTime = currentTime;
                //    POS.Properties.Settings.Default.StartTime = currentTime;
                //}
                //TimeSpan timeSpan = currentTime.Subtract(startTime);
                //if (timeSpan.Days>15)
                //{
                //    MessagePopup.ShowInformation("试用的天数已到期！");
                //    return;
                //}

                //同步用户
                SyncUser(sid, xls);
            }
            FormLogin frm = new FormLogin();
            if (frm.ShowDialog() == DialogResult.OK)
            {
                Application.Run(new FormMain());
            }
        }
        #endregion

        #region 同步用户
        private static void SyncUser(string sid, string xls)
        {
            DevExpress.Utils.WaitDialogForm dlg = new DevExpress.Utils.WaitDialogForm("正在同步用户和仓库数据，请稍后……", new Size(250, 100));
            dlg.Show();
            try
            {
                bool ck = SyncHelperBLL.Cksign(sid, xls);
                if (!ck)
                {
                    MessagePopup.ShowInformation("验证失败，请检查密钥是否正确！");
                    return;
                }
                SyncHelperBLL.SyncData(sid, xls, "", new List<string> { "user", "cnku" });
                System.Threading.Thread.Sleep(2000);
            }
            catch (Exception ex)
            {
                dlg.Close();
                logger.Info(ex.Message);
                MessagePopup.ShowInformation("同步用户和仓库数据失败,请重试！");
            }
            finally
            {
                dlg.Close();
            }
        }
        #endregion

        #region 拷贝数据库文件
        /// <summary>
        /// 拷贝数据库文件
        /// </summary>

        private static void SetDB()
        {
            if (!Directory.Exists(AppConst.sqliteDirectory))
            {
                //创建数据库文件目录
                Directory.CreateDirectory(AppConst.sqliteDirectory);
            }
            string sqlitePath = Path.Combine(AppConst.sqliteDirectory, AppConst.dbName);
            if (!File.Exists(sqlitePath))
            {
                File.Copy(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, AppConst.dbName), sqlitePath);
            }
        }
        #endregion

        #region 在线升级
        private static void Upgrade()
        {
            DateTime currentTime = DateTime.Now;

            //bool result = SyncHelperBLL.CheckConnect(out currentTime);
            //if (result)
            //{
            //    try
            //    {
            //        upgrade = AutoUpdaterBLL.GetUpgradeInfo();
            //    }
            //    catch (Exception ex)
            //    {
            //        MessagePopup.ShowInformation("获取服务器版本号失败");
            //        return;
            //    }
            //    string currentVersion = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();
            //    if (currentVersion != upgrade.version)
            //    {
            //        if (MessagePopup.ShowQuestion("发现新版本,确定下载更新程序？") == DialogResult.Yes)
            //        {
            //            FormDownloadProgress frm = new FormDownloadProgress(upgrade.url);
            //            frm.ShowDialog();
            //        }
            //    }
            //}
        }
        #endregion
        private static void Application_ThreadException(object sender, System.Threading.ThreadExceptionEventArgs e)
        {
            string str = GetExceptionMsg(e.Exception, e.ToString());
            logger.Info(str);
            UploadErrLog("UI:"+str);
            if (e.Exception.GetType() == typeof(WebException))
            {
                MessageBox.Show(AppConst.Connect_Msg);
            }
            else if (e.GetType() == typeof(SignException))
            {
                MessageBox.Show("验证失败，请检查密钥是否正确！");
            }
            else
            {
                MessageBox.Show("系统出现未知异常，请重启系统！");
                ApplicationHelper.Restart();
            }
        }

        private static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            Exception ex = e.ExceptionObject as Exception;
            string str = GetExceptionMsg(ex, e.ToString());
            logger.Info(str);
            UploadErrLog("非UI"+ str);
            if (ex.GetType() == typeof(WebException))
            {
                MessageBox.Show(AppConst.Connect_Msg);
            }
            else if (ex.GetType() == typeof(SignException))
            {
                MessageBox.Show("验证失败，请检查密钥是否正确！");
            }
            else
            {
                MessageBox.Show("系统出现未知异常，请重启系统！");
                ApplicationHelper.Restart();
            }
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
        private static void UploadErrLog(string msg)
        {
            string str = "当前版本：" + System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString()+System.Environment.NewLine;
            str += msg;
            ConfigModel config = ConfigHelper.LoadConfig(AppConst.ConfigFilePath);
            SyncHelperBLL.UploadErrLog(str, config.sid, config.xls, string.Empty);
        }
    }
}
