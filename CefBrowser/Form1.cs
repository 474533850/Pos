using CefSharp.WinForms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Configuration;

namespace CefBrowser
{
    public partial class Form1 : Form
    {
        ChromiumWebBrowser webBrower = null;
        string[] args = null;
        string url = string.Empty;
        string userName = string.Empty;
        string password = string.Empty;
        public Form1(string[] args)
        {
            InitializeComponent();
            this.args = args;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //args = new string[3];
            //args[0] = "http://yun.xc200.com/xc.jsp?id=1754";
            //args[1] = "w001";
            //args[2] = "123";
            //MessageBox.Show(args[0]);
            //MessageBox.Show(args[1]);
            //MessageBox.Show(args[2]);
            url = "http://xc200.com";
            if (args != null && args.Length > 0)
            {
                url = args[0];
                if (args.Length > 1 && args[1] != null)
                {
                    userName = args[1];
                }
                if (args.Length == 3)
                {
                    password = args[2];
                }

                Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                config.AppSettings.Settings["serviceUrl"].Value = url;
                //config.AppSettings.Settings["userName"].Value = userName;
                //config.AppSettings.Settings["password"].Value = password;
                config.Save(ConfigurationSaveMode.Modified);
                ConfigurationManager.RefreshSection("appSettings");
            }
            else
            {
                //url = "http://yun.xc200.com/xc.jsp?id=1465";
                string baseUrl = ConfigurationManager.AppSettings["serviceUrl"].ToString().Trim();
                if (!string.IsNullOrEmpty(baseUrl))
                {
                    url = baseUrl;
                }
                //userName = ConfigurationManager.AppSettings["userName"].ToString().Trim();
                //password = ConfigurationManager.AppSettings["password"].ToString().Trim();
            }
            webBrower = new ChromiumWebBrowser(url);
            webBrower.Dock = DockStyle.Fill;//填充方式
            this.Controls.Add(webBrower);
            webBrower.FrameLoadEnd += WebBrower_FrameLoadEnd;
            webBrower.MenuHandler = new MenuHandler();
            webBrower.DownloadHandler = new DownloadHandler();
        }

        private void WebBrower_FrameLoadEnd(object sender, CefSharp.FrameLoadEndEventArgs e)
        {
            if (args != null)
            {
                webBrower.GetBrowser().MainFrame.ExecuteJavaScriptAsync(string.Format("document.getElementById('WORKCODE').value='{0}'", userName));
                webBrower.GetBrowser().MainFrame.ExecuteJavaScriptAsync(string.Format("document.getElementById('PASSWORD').value='{0}'", password));
            }
        }
    }
}
