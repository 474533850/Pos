using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace POS.Helper
{
  public class ApplicationHelper
    {
        /// <summary>
        /// 重启
        /// </summary>
        public static void Restart()
        {
            Application.ExitThread();
            Application.Exit();
            Application.Restart();
            Process.GetCurrentProcess().Kill();
        }
    }
}
