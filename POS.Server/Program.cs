using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Windows.Forms;

namespace POS.Server
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Uri url = new Uri(ConfigurationManager.AppSettings["serviceUrl"].ToString());
            url = new Uri(url, string.Format("xc.jsp?id={0}", 1754));
            System.Diagnostics.Process.Start(url.ToString());
            //Application.Run(new Form1());
        }
    }
}
