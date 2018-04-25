using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace AutoUpdateHelper
{
    public class AutoUpdater
    {
        #region 升级
        public static void Update(string mainExeName)
        {
            //查找主程序的进程
            var pros = Process.GetProcessesByName(mainExeName);
            if (pros.Count() > 0)
            {
                foreach (Process process in pros)
                {
                    process.Kill();
                }
            }
            System.Threading.Thread.Sleep(500);
            string basePaht = AppDomain.CurrentDomain.BaseDirectory;
            //主程序目录
            string rootPath = Path.GetDirectoryName(basePaht.TrimEnd('\\'));
            //临时目录
            string tempFolderPaht = Path.Combine(rootPath, "TempFolder");
            List<string> filenames = Directory.GetFileSystemEntries(tempFolderPaht).ToList();
            string file = filenames.OrderByDescending(r => Path.GetFileName(r)).FirstOrDefault();

            //if (filenames != null && filenames.Length > 0)
            //{
            //    foreach (string file in filenames)
            //    {
            if (file != null)
            {
                if (File.Exists(file))
                {
                    //解压、覆盖
                    try
                    {
                        FileCompress.UnZip(file, rootPath);
                        //break;
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                }
            }
           // System.Threading.Thread.Sleep(1000);
            //    }
            //}

            //更新主程序完成，启动主程序
            //string path = Path.Combine(rootPath, mainExeName + ".exe");
            //MessageBox.Show(path);
            //Process.Start(path);
        }
        #endregion

    }
}
