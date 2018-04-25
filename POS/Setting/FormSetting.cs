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
using POS.Model;
using POS.Common;
using System.IO;
using POS.Common.Enum;
using System.Threading.Tasks;
using System.Threading;
using System.Configuration;
using System.Collections.Specialized;
using System.Drawing.Printing;

namespace POS.Setting
{
    public partial class FormSetting : BaseForm
    {
        CnkuBLL cnkuBLL = new CnkuBLL();
        PossettingBLL possettingBLL = new PossettingBLL();
        UpgradeModel upgrade = null;
        bool isShowAutoSync = false;
        public FormSetting()
        {
            InitializeComponent();
            Init();

            bool enable = GetPermission(Functions.Setting, false);
            btnBackups.Enabled = enable;
            btnApply_g.Enabled = enable;
            btnConfirm_g.Enabled = enable;
            btnApply_p.Enabled = enable;
            btnConfirm_p.Enabled = enable;
            btnApply_s.Enabled = enable;
            btnConfirm_s.Enabled = enable;
        }

        private void Init()
        {
            if (bool.TryParse(ConfigurationManager.AppSettings["isShowAutoSync"], out isShowAutoSync))
            {
                if (isShowAutoSync)
                {
                    chkSyncData.Visible = true;
                }
                else
                {
                    chkSyncData.Visible = false;
                }
            }
            else
            {
                chkSyncData.Visible = false;
            }

            lblVersion.Text = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();
            cboPorts.Properties.Items.AddRange(CustomerDisplayHelper.GetPortNames());
            lueDefCnku.Properties.DataSource = cnkuBLL.GetCnku(RuntimeObject.CurrentUser.xls).ToList();
            List<PossettingModel> possettings = possettingBLL.GetPossetting();
            PossettingModel entity = possettings.Where(r => r.xpname == AppConst.Shop_Name).FirstOrDefault();
            if (entity != null)
            {
                txtShopName.Text = entity.xpvalue;
            }
            entity = possettings.Where(r => r.xpname == AppConst.Shop_Address).FirstOrDefault();
            if (entity != null)
            {
                txtShopAddress.Text = entity.xpvalue;
            }
            entity = possettings.Where(r => r.xpname == AppConst.Consumer_Pho).FirstOrDefault();
            if (entity != null)
            {
                txtConsumer.Text = entity.xpvalue;
            }
            entity = possettings.Where(r => r.xpname == AppConst.SecretKey).FirstOrDefault();
            if (entity != null)
            {
                txtKey.Text = entity.xpvalue;
            }
            Dictionary<object, object> machineIDs = GetPayCodeMachineIDs();
            if (machineIDs != null)
            {
                var query = machineIDs.Where(r => r.Key.ToString() == RuntimeObject.CurrentUser.xls).FirstOrDefault();
                if (query.Value != null)
                {
                    txtDeviceid.Text = query.Value.ToString();
                }
            }
            entity = possettings.Where(r => r.xpname == AppConst.SyncData_Type).FirstOrDefault();
            if (entity != null)
            {
                cboSyncData.SelectedIndex = int.Parse(entity.xpvalue);
            }
            entity = possettings.Where(r => r.xpname == AppConst.SyncData_TimeSpan).FirstOrDefault();
            if (entity != null)
            {
                sptSyncData_TimeSpan.EditValue = entity.xpvalue;
            }
            entity = possettings.Where(r => r.xpname == AppConst.DEF_CKC).FirstOrDefault();
            if (entity != null)
            {
                lueDefCnku.EditValue = entity.xpvalue;
            }
            entity = possettings.Where(r => r.xpname == AppConst.PaymentChannel).FirstOrDefault();
            if (entity != null)
            {
                rdoPaymentChannel.SelectedIndex = int.Parse(entity.xpvalue);
            }
            else
            {
                rdoPaymentChannel.SelectedIndex = 0;
            }
            entity = possettings.Where(r => r.xpname == AppConst.Print_Kind).FirstOrDefault();
            if (entity != null)
            {
                tsPrintkind.IsOn = bool.Parse(entity.xpvalue);
            }
            entity = possettings.Where(r => r.xpname == AppConst.Print_Num).FirstOrDefault();
            if (entity != null)
            {
                rdoPrintNum.SelectedIndex = int.Parse(entity.xpvalue);
            }
            entity = possettings.Where(r => r.xpname == AppConst.Is_Customer_Display).FirstOrDefault();
            if (entity != null)
            {
                tsCustomerDisplay.IsOn = bool.Parse(entity.xpvalue);
            }
            entity = possettings.Where(r => r.xpname == AppConst.Customer_Addr).FirstOrDefault();
            if (entity != null)
            {
                cboPorts.EditValue = entity.xpvalue;
            }
            entity = possettings.Where(r => r.xpname == AppConst.BaudRate).FirstOrDefault();
            if (entity != null)
            {
                txtBaudRate.EditValue = entity.xpvalue;
            }
            entity = possettings.Where(r => r.xpname == AppConst.Cashbox_Port).FirstOrDefault();
            if (entity != null)
            {
                txtCashbox_Port.EditValue = entity.xpvalue;
            }
            entity = possettings.Where(r => r.xpname == AppConst.Cashbox_Order).FirstOrDefault();
            if (entity != null)
            {
                txtCashbox_Order.EditValue = entity.xpvalue;
            }
            entity = possettings.Where(r => r.xpname == AppConst.IsPrintBill).FirstOrDefault();
            if (entity != null)
            {
                tsPrint.IsOn = bool.Parse(entity.xpvalue);
            }

            //获取当前打印机
            for (int i = 0; i < PrinterSettings.InstalledPrinters.Count; i++)
            {
                cboPrintName.Properties.Items.Add(PrinterSettings.InstalledPrinters[i]);
                cboVoucherPrintName.Properties.Items.Add(PrinterSettings.InstalledPrinters[i]);
            }

            entity = possettings.Where(r => r.xpname == AppConst.DefaultPrinter).FirstOrDefault();
            if (entity != null)
            {
                cboPrintName.EditValue = entity.xpvalue;
            }
            entity = possettings.Where(r => r.xpname == AppConst.DefaultVoucherPrinter).FirstOrDefault();
            if (entity != null)
            {
                cboVoucherPrintName.EditValue = entity.xpvalue;
            }

        }
        private void FormSetting_Load(object sender, EventArgs e)
        {
            UserModel user = RuntimeObject.CurrentUser;
            DateTime currentTime = DateTime.Now;
            bool result = SyncHelperBLL.CheckConnect(out currentTime, user.bookID, user.username, user.password);
            if (result)
            {
                try
                {
                    upgrade = AutoUpdaterBLL.GetUpgradeInfo();
                    lblVersion_New.Text = upgrade.version;
                }
                catch (Exception)
                {
                    MessagePopup.ShowInformation("获取服务器版本号失败");
                    return;
                }
            }
            else
            {
                lblVersion_New.Text = string.Empty;
            }
        }

        #region 手动同步数据
        private void btnSyncData_Click(object sender, EventArgs e)
        {
            if (MessagePopup.ShowQuestion("确定开始同步数据？") == DialogResult.Yes)
            {
                SyncData(AppConst.AllTableNames);
            }
        }
        #endregion

        #region 是否客显
        private void tsCustomerDisplay_Toggled(object sender, EventArgs e)
        {
            layCustomerDisplay1.Visibility =
            layCustomerDisplay2.Visibility =
            layCustomerDisplay3.Visibility =
            layCustomerDisplay4.Visibility =
            layCustomerDisplay5.Visibility =
            layCustomerDisplay6.Visibility =
            layCustomerDisplay7.Visibility =
            tsCustomerDisplay.IsOn ? DevExpress.XtraLayout.Utils.LayoutVisibility.Always : DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
        }
        #endregion

        #region 确定

        private void btnConfirm_g_Click(object sender, EventArgs e)
        {
            if (Checked())
            {
                if (Save_g())
                {
                    this.DialogResult = DialogResult.OK;
                }
                else
                {
                    MessagePopup.ShowError("保存失败！");
                }
            }
        }

        private void btnApply_g_Click(object sender, EventArgs e)
        {
            if (Checked())
            {
                if (Save_g())
                {
                    MessagePopup.ShowInformation("保存成功！");
                }
                else
                {
                    MessagePopup.ShowError("保存失败！");
                }
            }
        }

        private bool Checked()
        {
            if (string.IsNullOrEmpty(txtKey.Text.Trim()))
            {
                MessagePopup.ShowInformation("密钥不能为空！");
                return false;
            }

            return true;
        }

        private void btnConfirm_p_Click(object sender, EventArgs e)
        {
            if (Save_p())
            {
                this.DialogResult = DialogResult.OK;
            }
            else
            {
                MessagePopup.ShowError("保存失败！");
            }
        }

        private void btnApply_p_Click(object sender, EventArgs e)
        {
            if (Save_p())
            {
                MessagePopup.ShowInformation("保存成功！");
            }
            else
            {
                MessagePopup.ShowError("保存失败！");
            }
        }

        private void btnConfirm_s_Click(object sender, EventArgs e)
        {
            if (Save_s())
            {
                this.DialogResult = DialogResult.OK;
            }
            else
            {
                MessagePopup.ShowError("保存失败！");
            }
        }

        private void btnApply_s_Click(object sender, EventArgs e)
        {
            if (Save_s())
            {
                MessagePopup.ShowInformation("保存成功！");
            }
            else
            {
                MessagePopup.ShowError("保存失败！");
            }
        }

        private bool Save_g()
        {
            List<PossettingModel> possettings = new List<PossettingModel>();
            PossettingModel entity = new PossettingModel();
            entity.issys = false;
            entity.xpname = AppConst.Shop_Name;
            entity.xpvalue = txtShopName.Text.Trim();
            possettings.Add(entity);
            entity = new PossettingModel();
            entity.issys = false;
            entity.xpname = AppConst.Shop_Address;
            entity.xpvalue = txtShopAddress.Text.Trim();
            possettings.Add(entity);
            entity = new PossettingModel();
            entity.issys = false;
            entity.xpname = AppConst.Consumer_Pho;
            entity.xpvalue = txtConsumer.Text.Trim();
            possettings.Add(entity);
            //entity = new PossettingModel();
            //entity.issys = false;
            //entity.xpname = AppConst.Deviceid;
            //entity.xpvalue = txtDeviceid.Text.Trim();
            //possettings.Add(entity);
            entity = new PossettingModel();
            entity.issys = false;
            entity.xpname = AppConst.SyncData_Type;
            entity.xpvalue = cboSyncData.SelectedIndex.ToString();
            possettings.Add(entity);
            entity = new PossettingModel();
            entity.issys = false;
            entity.xpname = AppConst.SyncData_TimeSpan;
            entity.xpvalue = sptSyncData_TimeSpan.Text.Trim().ToString();
            possettings.Add(entity);
            entity = new PossettingModel();
            entity.issys = false;
            entity.xpname = AppConst.DEF_CKC;
            entity.xpvalue = lueDefCnku.EditValue == null ? string.Empty : lueDefCnku.EditValue.ToString();
            possettings.Add(entity);
            entity = new PossettingModel();
            entity.issys = false;
            entity.xpname = AppConst.IsPrintBill;
            entity.xpvalue = tsPrint.IsOn.ToString();
            possettings.Add(entity);
            entity = new PossettingModel();
            entity.issys = false;
            entity.xpname = AppConst.SecretKey;
            entity.xpvalue = txtKey.Text.Trim();
            possettings.Add(entity);
            entity = new PossettingModel();
            entity.issys = false;
            entity.xpname = AppConst.PaymentChannel;
            entity.xpvalue = rdoPaymentChannel.SelectedIndex.ToString();
            possettings.Add(entity);
            possettingBLL.AddPossetting(possettings);
            return possettingBLL.AddPossetting(possettings);
        }

        private bool Save_p()
        {
            List<PossettingModel> possettings = new List<PossettingModel>();
            PossettingModel entity = new PossettingModel();
            entity.issys = false;
            entity.xpname = AppConst.Print_Kind;
            entity.xpvalue = tsPrintkind.IsOn.ToString();
            possettings.Add(entity);
            entity = new PossettingModel();
            entity.issys = false;
            entity.xpname = AppConst.Print_Num;
            entity.xpvalue = rdoPrintNum.SelectedIndex.ToString();
            possettings.Add(entity);

            possettings.Add(entity);

            return possettingBLL.AddPossetting(possettings);

        }

        private bool Save_s()
        {
            List<PossettingModel> possettings = new List<PossettingModel>();
            PossettingModel entity = new PossettingModel();
            entity.issys = false;
            entity.xpname = AppConst.Is_Customer_Display;
            entity.xpvalue = tsCustomerDisplay.IsOn.ToString();
            possettings.Add(entity);
            entity = new PossettingModel();
            entity.issys = false;
            entity.xpname = AppConst.Customer_Addr;
            entity.xpvalue = cboPorts.EditValue == null ? string.Empty : cboPorts.EditValue.ToString();
            possettings.Add(entity);
            entity = new PossettingModel();
            entity.issys = false;
            entity.xpname = AppConst.BaudRate;
            entity.xpvalue = txtBaudRate.Text.Trim();
            possettings.Add(entity);
            entity = new PossettingModel();
            entity.issys = false;
            entity.xpname = AppConst.Cashbox_Port;
            entity.xpvalue = txtCashbox_Port.Text.Trim();
            possettings.Add(entity);
            entity = new PossettingModel();
            entity.issys = false;
            entity.xpname = AppConst.Cashbox_Order;
            entity.xpvalue = txtCashbox_Order.Text.Trim();
            if (cboPrintName.EditValue != null && !string.IsNullOrEmpty(cboPrintName.EditValue.ToString().Trim()))
            {
                entity = new PossettingModel();
                entity.issys = false;
                entity.xpname = AppConst.DefaultPrinter;
                entity.xpvalue = cboPrintName.Text.Trim().ToString();
                possettings.Add(entity);
            }

            if (cboVoucherPrintName.EditValue != null && !string.IsNullOrEmpty(cboVoucherPrintName.EditValue.ToString().Trim()))
            {
                entity = new PossettingModel();
                entity.issys = false;
                entity.xpname = AppConst.DefaultVoucherPrinter;
                entity.xpvalue = cboVoucherPrintName.Text.Trim().ToString();
                possettings.Add(entity);
            }

            return possettingBLL.AddPossetting(possettings);

        }
        #endregion

        #region 备份本地数据
        private void btnBackups_Click(object sender, EventArgs e)
        {
            string filePath = Path.Combine(AppConst.sqliteDirectory, AppConst.dbName);
            if (File.Exists(filePath))
            {
                System.Windows.Forms.SaveFileDialog saveFileDialog = new SaveFileDialog();
                saveFileDialog.Filter = "(*.db3)|*.db3";
                saveFileDialog.FilterIndex = 0;
                saveFileDialog.RestoreDirectory = true;
                saveFileDialog.FileName = "Pos";
                saveFileDialog.Title = "备份文件";

                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    File.Copy(filePath, saveFileDialog.FileName, true);
                }
            }
            else
            {
                MessagePopup.ShowError("数据库文件不存在！");
            }
        }
        #endregion

        #region 客显
        private void CustomerDisplay_Click(object sender, EventArgs e)
        {
            if (cboPorts.SelectedIndex == -1)
            {
                MessagePopup.ShowError("请选择客显地址！");
                return;
            }
            SimpleButton btn = sender as SimpleButton;
            if (btn != null)
            {
                string[] array = btn.Text.Trim().Split('：');
                CustomerDisplayHelper disliay;
                try
                {
                    disliay = new CustomerDisplayHelper(cboPorts.Text, int.Parse(txtBaudRate.Text));

                    switch (array[0].Trim())
                    {
                        case "清除":
                            disliay.DispiayType = CustomerDispiayType.Clear;
                            break;
                        case "单价":
                            disliay.DispiayType = CustomerDispiayType.Price;
                            break;
                        case "总价":
                            disliay.DispiayType = CustomerDispiayType.Total;
                            break;
                        case "找零":
                            disliay.DispiayType = CustomerDispiayType.Change;
                            break;
                    }
                    string dispiay = array.Length == 1 ? string.Empty : array[1];
                    disliay.DisplayData(dispiay);

                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }
            }
        }
        #endregion

        #region 版本更新
        private void btnVersionUpdate_Click(object sender, EventArgs e)
        {
            string currentVersion = lblVersion.Text.Trim();
            if (lblVersion_New.Text.Trim() != currentVersion)
            {
                if (CheckConnect())
                {
                    if (upgrade != null)
                    {

                        if (MessagePopup.ShowQuestion("确定下载更新程序？") == DialogResult.Yes)
                        {
                            FormDownloadProgress frm = new FormDownloadProgress(upgrade.url);
                            frm.ShowDialog();
                        }
                    }
                }
            }
            else
            {
                MessagePopup.ShowInformation("已是最新版本！");
            }
        }
        #endregion

        #region 自动同步数据
        private void chkSyncData_CheckedChanged(object sender, EventArgs e)
        {
            if (chkSyncData.Checked)
            {
                Task.Factory.StartNew(() =>
                {
                    int i = 0;
                    bool flag = chkSyncData.Checked;
                    while (flag)
                    {
                        if (CheckConnect(false))
                        {
                            i++;
                            try
                            {
                                SyncHelperBLL.SyncData(RuntimeObject.CurrentUser.bookID, RuntimeObject.CurrentUser.xls, RuntimeObject.CurrentUser.usercode, AppConst.AllTableNames);
                                this.Invoke((MethodInvoker)delegate
                                {
                                    chkSyncData.Text = string.Format("自动同步数据 同步数据成功{0}次", i);
                                });
                            }
                            catch (Exception ex)
                            {
                                string msg = string.Format("同步数据失败,错误信息：{0}！", ex.Message);
                                if (InvokeRequired)
                                {
                                    this.Invoke((MethodInvoker)delegate { MessagePopup.ShowInformation(msg); });
                                }
                            }
                            finally
                            {
                            }
                        }
                        flag = chkSyncData.Checked;
                        Thread.Sleep(1000);
                    }
                });
            }
            else
            {
                chkSyncData.Text = "自动同步数据";
            }
        }
        #endregion

        private void btnUpdateServer_Click(object sender, EventArgs e)
        {
            FormServerSetting frm = new FormServerSetting();
            frm.ShowDialog();
        }

        private void FormSetting_FormClosing(object sender, FormClosingEventArgs e)
        {
            chkSyncData.Checked = false;
        }
    }
}