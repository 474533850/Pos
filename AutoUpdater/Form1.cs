using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AutoUpdater
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Task task = Task.Factory.StartNew(() =>
            {
                AutoUpdateHelper.AutoUpdater.Update("POS");
            });
            task.ContinueWith(r =>
            {
                MessageBox.Show("升级完成!");
                string basePaht = AppDomain.CurrentDomain.BaseDirectory;
                //主程序目录
                string rootPath = Path.GetDirectoryName(basePaht.TrimEnd('\\'));
                string path = Path.Combine(rootPath, "POS" + ".exe");
                Process.Start(path);
                Application.ExitThread(); Application.Exit(); Process.GetCurrentProcess().Kill();
            });

            //AutoUpdateHelper.AutoUpdater.Update("POS");
            //Application.ExitThread(); Application.Exit(); Process.GetCurrentProcess().Kill();
        }
    }
}
