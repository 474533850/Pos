using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace POS.Common.utility
{
    public class ApplicationHelper
    {
        /// <summary>
        /// 重启
        /// </summary>
        private static void Restart()
        {
            Application.ExitThread();
            Application.Exit();
            Application.Restart();
            Process.GetCurrentProcess().Kill();
        }
    }
}
