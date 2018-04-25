using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Linq;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using System.Net;
using System.IO;
using POS.Helper;
using POS.Common;
using System.Diagnostics;
using POS.BLL;

namespace POS
{
    public partial class FormDownloadProgress : BaseForm
    {
        private string url;
        public FormDownloadProgress(string url)
        {
            InitializeComponent();
            this.url = url;
        }

        private void FormDownloadProgress_Load(object sender, EventArgs e)
        {
            Download();
        }

        void Download()
        {
            System.Net.ServicePointManager.DefaultConnectionLimit = 200;
            System.GC.Collect();
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "GET";
            request.Timeout = 1000 * 60 * 5;
            request.ReadWriteTimeout = 1000 * 60 * 5;
            request.Proxy = null;
            request.BeginGetResponse(AsyncCallbackResult, request);
        }

        private void AsyncCallbackResult(IAsyncResult ar)
        {
            WebRequest request = null;
            WebResponse response = null;
            Stream responseStream = null;
            //创建本地文件写入流
            Stream stream = null;
            try
            {
                request = ar.AsyncState as WebRequest;
                response = request.EndGetResponse(ar);
                responseStream = response.GetResponseStream();
                long totalBytes = response.ContentLength;
                this.Invoke((MethodInvoker)delegate { progressBar.Properties.Maximum = (int)totalBytes; });
                long totalDownloadBytes = 0;
                byte[] bytes = new byte[1024];
                int size = responseStream.Read(bytes, 0, bytes.Length);
                if (!Directory.Exists(AppConst.UpgradeDirectory))
                {
                    Directory.CreateDirectory(AppConst.UpgradeDirectory);
                }
                System.IO.File.SetAttributes(AppConst.UpgradeDirectory, System.IO.FileAttributes.Normal);
                DelectDir(AppConst.UpgradeDirectory);
                List<string> fileNames = Directory.GetFileSystemEntries(AppConst.UpgradeDirectory).ToList();
                // string path = Path.Combine(AppConst.UpgradeDirectory, AppConst.UpgradeFileName);
                string path = string.Empty;
                //if (File.Exists(path))
                //{
                //    File.Delete(path);
                //}

                if (fileNames.FirstOrDefault() == null)
                {
                    path = Path.Combine(AppConst.UpgradeDirectory, ("1" + ".zip"));
                }
                else
                {
                    path = Path.Combine(AppConst.UpgradeDirectory, (fileNames.Count + 1 + ".zip"));
                }

                stream = new FileStream(path, FileMode.Create);
                while (size > 0)
                {
                    stream.Write(bytes, 0, size);
                    totalDownloadBytes += size;
                    this.Invoke((MethodInvoker)delegate { progressBar.EditValue = totalDownloadBytes; });
                    size = responseStream.Read(bytes, 0, bytes.Length);
                }

                string basePath = AppDomain.CurrentDomain.BaseDirectory;
                string updater_exe_path = Path.Combine(Path.Combine(basePath, "AutoUpdater"), "AutoUpdater.exe");
                if (!File.Exists(updater_exe_path))
                {
                    if (this.InvokeRequired)
                    {
                        this.Invoke((MethodInvoker)delegate
                        {
                            MessagePopup.ShowError("升级程序已损坏！");
                        });
                    }
                    else
                    {
                        MessagePopup.ShowError("升级程序已损坏！");
                    }

                    this.Close();
                }
                Process.Start(updater_exe_path);
                this.DialogResult = DialogResult.OK;
                // Process.GetCurrentProcess().Kill();
            }
            catch (Exception ex)
            {
                if (this.InvokeRequired)
                {
                    this.Invoke((MethodInvoker)delegate
                    {
                        MessagePopup.ShowError(string.Format("下载补丁失败,错误信息：{0}！", ex.Message));
                    });
                }
                else
                {
                    MessagePopup.ShowError(string.Format("下载补丁失败,错误信息：{0}！", ex.Message));
                }

                this.Close();
            }
            finally
            {
                if (stream != null)
                {
                    stream.Close();
                }
                if (responseStream != null)
                {
                    responseStream.Close();
                }
                if (response != null)
                {
                    response.Close();
                }
                if (request != null)
                {
                    request.Abort();
                }
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        #region 删除所有的临时文件
        public void DelectDir(string srcPath)
        {
            try
            {
                DirectoryInfo dir = new DirectoryInfo(srcPath);
                FileSystemInfo[] fileinfo = dir.GetFileSystemInfos();  //返回目录中所有文件和子目录
                foreach (FileSystemInfo i in fileinfo)
                {
                    if (i is DirectoryInfo)            //判断是否文件夹
                    {
                        DirectoryInfo subdir = new DirectoryInfo(i.FullName);
                        subdir.Delete(true);          //删除子目录和文件
                    }
                    else
                    {
                        File.Delete(i.FullName);      //删除指定文件
                    }
                }
            }
            catch (Exception)
            {
            }
        }
        #endregion
    }
}