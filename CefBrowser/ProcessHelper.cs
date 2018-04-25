﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CefBrowser
{
    public class ProcessHelper
    {
        /// <summary>
        /// 当前进程名
        /// </summary>
        private const string CURRENT_PROCESS_NAME = "CefBrowser";

        /// <summary>
        /// 当前exe扩展名
        /// </summary>
        private const string CURRENT_FILE_EXT = "exe";

        /// <summary>
        /// 检查是否修改了文件名
        /// </summary>
        /// <returns>如果修改，返回false；没有修改，返回true</returns>
        public static bool CheckFileName(Type assembleObj)
        {
            string fileName = System.IO.Path.GetFileName(assembleObj.Assembly.Location);
            if (fileName.Equals(CURRENT_PROCESS_NAME + "." + CURRENT_FILE_EXT, StringComparison.CurrentCultureIgnoreCase))
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// 检测进程数
        /// </summary>
        /// <param name="proCount"></param>
        /// <param name="processName"></param>
        /// <returns></returns>
        public static bool CheckProcess(int proCount)
        {
            System.Diagnostics.Process[] processes = System.Diagnostics.Process.GetProcessesByName(CURRENT_PROCESS_NAME);
            if (processes.Length > proCount)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
    }
}