using POS.BLL;
using POS.Common;
using POS.Common.utility;
using POS.Helper;
using POS.Model;
using POS.Sale;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace POS.Shifts
{
    public partial class FormPosBan : BaseForm
    {
        static ApplicationLogger logger = new ApplicationLogger(typeof(FormPosBan).Name);
        PosBanBLL posBanBLL = new PosBanBLL();
        string endDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        PosbanDetailModel posbanDetail = null;

        PossettingBLL possettingBLL = new PossettingBLL();
        //空单的数量
        int appendCount;
        public FormPosBan()
        {
            InitializeComponent();
            Init();
        }

        private void Init()
        {
            UserModel currentUser = RuntimeObject.CurrentUser;

            PosbanModel posban = posBanBLL.GetPosbanByNO(currentUser.posnono);

            posbanDetail = posBanBLL.GetPosbanDetail(currentUser.posnono, out appendCount);

            txtUserName.Text = string.Format("{0}(工号：{1})", currentUser.username, currentUser.usercode);
            PossettingModel possetting = possettingBLL.GetPossettingByKey(AppConst.Shop_Address);
            txtAddress.Text = (possetting == null || possetting.xpvalue == string.Empty) ? currentUser.xlsname : possetting.xpvalue;
            lblStartDate.Text = DateTime.Parse(posban.xtime1).ToString("yyyy-MM-dd HH:mm:ss");
            lblEndDate.Text = endDate;


            //金额统计
            lblTotalCash.Tag = posbanDetail.TotalCash
                + posbanDetail.TotalDeposit
                + posbanDetail.TotalWeChat
                + posbanDetail.TotalAlipay
                + posbanDetail.TotalUnionpayCard
                + posbanDetail.Check
                + posbanDetail.TotalCoupon
                + posbanDetail.Money_Deductible;

            lblTotalCash.Text = string.Format("￥{0}", lblTotalCash.Tag);
            lblCash.Text = string.Format("{1}笔；￥{0}", posbanDetail.TotalCash, posbanDetail.CashCount);
            lblDeposit.Text = string.Format("{1}笔；￥{0}", posbanDetail.TotalDeposit, posbanDetail.DepositCount);
            lblWeChat.Text = string.Format("{1}笔；￥{0}", posbanDetail.TotalWeChat, posbanDetail.WeChatCount);
            lblAlipay.Text = string.Format("{1}笔；￥{0}", posbanDetail.TotalAlipay, posbanDetail.AlipayCount);
            lblUnionpayCard.Text = string.Format("{1}笔；￥{0}", posbanDetail.TotalUnionpayCard, posbanDetail.UnionpayCardCount);
            lblCouponQuantity.Text = posbanDetail.TotalCouponQuantity.ToString();
            lblTotalCoupon.Text = string.Format("￥{0}", posbanDetail.TotalCoupon);
            lblDebts.Text = string.Format("{1}笔；￥{0}", posbanDetail.Debts, posbanDetail.DebtsCount);
            lblCheck.Text = string.Format("{1}笔；￥{0}", posbanDetail.Check, posbanDetail.CheckCount);

            //销售总额
            lblTotalAmount.Text = string.Format("￥{0}", posbanDetail.TotalAmount);
            lblAmount.Text = string.Format("￥{0}", posbanDetail.TotalAmount);
            lblRefund.Text = posbanDetail.TotalRefund.ToString();
            lblInvalid.Text = posbanDetail.TotalInvalid.ToString();

            //总单数据
            lblTotalOrderCount.Text = (posbanDetail.PosCount + posbanDetail.InvalidCount + posbanDetail.ReturnCount).ToString();
            lblPosCount.Text = posbanDetail.PosCount.ToString();
            lblReturnCount.Text = posbanDetail.ReturnCount.ToString();
            lblInvalidCount.Text = posbanDetail.InvalidCount.ToString();

            //挂单
            lblTotalPendingCount.Text = posbanDetail.PendingCount.ToString();
            lblPendingCount.Text = posbanDetail.PendingCount.ToString();

            //促销
            lblTotalSale.Text = string.Format("￥{0}", posbanDetail.Money_Manual + posbanDetail.Money_Clnt + posbanDetail.Money_Sale + posbanDetail.Money_Exchange);
            lblMoney_Manual.Text = string.Format("￥{0}", posbanDetail.Money_Manual);
            lblQuantity_Manual.Text = string.Format("{0}", posbanDetail.Quantity_Manual);
            lblCount_Manual.Text = string.Format("{0}", posbanDetail.Count_Manual);
            lblMoney_Clnt.Text = string.Format("￥{0}", posbanDetail.Money_Clnt);
            lblQuantity_Clnt.Text = string.Format("{0}", posbanDetail.Quantity_Clnt);
            lblCount_Clnt.Text = string.Format("{0}", posbanDetail.Count_Clnt);
            lblMoney_Sale.Text = string.Format("￥{0}", posbanDetail.Money_Sale);
            lblQuantity_Sale.Text = string.Format("{0}", posbanDetail.Quantity_Sale);
            lblCount_Sale.Text = string.Format("{0}", posbanDetail.Count_Sale);
            lblMoney_Exchange.Text = string.Format("￥{0}", posbanDetail.Money_Exchange);
            lblQuantity_Exchange.Text = string.Format("{0}", posbanDetail.Quantity_Exchange);
            lblCount_Exchange.Text = string.Format("{0}", posbanDetail.Count_Exchange);
            lblMoney_Cash.Text = string.Format("￥{0}", posbanDetail.Money_Deductible);
            lblQuantity_Cash.Text = string.Format("{0}", posbanDetail.Count_Deductible);

            //会员充值
            lblTotal_Recharge.Text = string.Format("￥{0}", posbanDetail.Total_Recharge);
            lblCash_Recharge.Text = string.Format("￥{0}", posbanDetail.Cash_Recharge);
            lblWeChat_Recharge.Text = string.Format("￥{0}", posbanDetail.WeChat_Recharge);
            lblAlipay_Recharge.Text = string.Format("￥{0}", posbanDetail.Alipay_Recharge);
            lblUnionpayCard_Recharge.Text = string.Format("￥{0}", posbanDetail.UnionpayCard_Recharge);

            //回款
            List<PosjhhModel> posjhhs = GetClntRepayments(lblStartDate.Text, lblEndDate.Text);
            decimal xpay = 0;
            if (posjhhs != null)
            {
                xpay = posjhhs.Sum(r => r.xpay);
                posbanDetail.Repayments = xpay;
            }
            SetData(xpay);
            //GetClntRepayments();
        }

        //private void GetClntRepayments()
        //{
        //    Uri baseUrl = new Uri(ConfigurationManager.AppSettings["serviceUrl"].ToString());
        //    List<PosjhhModel> posjhhs = new List<PosjhhModel>();
        //    Uri url = new Uri(baseUrl,
        //       string.Format("pos/pull?sid={0}&mod=posjhh&xls={1}&clntname={2}&sdate={3}&edate={4} "
        //       , RuntimeObject.CurrentUser.bookID, RuntimeObject.CurrentUser.xls, string.Empty, lblStartDate.Text, lblEndDate.Text));
        //    try
        //    {
        //        HttpClient.GetAasync<PosjhhModel>(url.ToString(), null, syncResult =>
        //        {
        //            posjhhs = syncResult.datas;
        //            decimal xpay = posjhhs.Sum(r => r.xpay);
        //            posbanDetail.Repayments = xpay;
        //            if (InvokeRequired)
        //            {
        //                this.Invoke((MethodInvoker)delegate { SetData(xpay); });
        //            }
        //            else
        //            {
        //                SetData(xpay);
        //            }
        //        });
        //    }
        //    catch (Exception ex)
        //    {
        //        MessagePopup.ShowError("获取挂账结算明细账记录失败！");
        //    }
        //}
        private void SetData(decimal xpay)
        {
            lblRepayments.Text = string.Format("￥{0}", xpay);
            if (lblTotalCash.Tag != null && lblTotalCash.Tag.ToString() != string.Empty)
            {
                lblTotalCash.Text = string.Format("￥{0}", decimal.Parse(lblTotalCash.Tag.ToString()) + xpay);
            }
        }

        #region 打印结果开关
        private void tSwitchPrint_Toggled(object sender, EventArgs e)
        {
            // chkConfirm.Visible = tSwitchPrint.IsOn;
        }
        #endregion

        #region 销售商品报表
        private void btnSaleReport_Click(object sender, EventArgs e)
        {
            FormSaleReport frm = new FormSaleReport();
            frm.ShowDialog();
        }
        #endregion

        #region 销售日结
        private void btnSaleDayReport_Click(object sender, EventArgs e)
        {
            FormSaleDayReport frm = new FormSaleDayReport();
            DialogResult dialogResult = frm.ShowDialog();
            if (dialogResult == DialogResult.OK)
            {
                btnSubmit_Click(null, null);
            }
            else if (dialogResult == DialogResult.No)
            {
                this.Close();
            }
        }
        #endregion

        #region 交班
        private void btnSubmit_Click(object sender, EventArgs e)
        {
            decimal jiehav = 0;
            if (!string.IsNullOrEmpty(txtJiehav.Text.Trim()))
            {
                if (!decimal.TryParse(txtJiehav.Text.Trim(), out jiehav))
                {
                    MessagePopup.ShowError("输入结留现金不正确！");
                    return;
                }
            }
            else if (appendCount > 0)
            {
                MessagePopup.ShowError(string.Format("存在{0}张销售单据没有追加商品,请把单据追加完成再交班！", appendCount));
                return;
            }
            DevExpress.Utils.WaitDialogForm dlg = new DevExpress.Utils.WaitDialogForm("正在处理交班信息，请稍后……", new Size(250, 100));
            dlg.Show();
            PosbanModel entity = new PosbanModel();
            entity.xtime2 = endDate;
            entity.xjiepos = posbanDetail.TotalAmount + posbanDetail.Repayments;
            entity.xjiehav = jiehav;
            entity.xjieget = posbanDetail.TotalAmount + posbanDetail.Repayments - jiehav;
            entity.xjieok = true;
            entity.posnono = RuntimeObject.CurrentUser.posnono;
            logger.Info("修改交班记录");
            posBanBLL.UpdatePosban(entity);
            logger.Info("修改交班记录成功");

            try
            {
                string pathRoot = Path.GetPathRoot(Application.StartupPath);
                string filePath = Path.Combine(AppConst.sqliteDirectory, AppConst.dbName);
                string targetPath = Path.Combine(pathRoot, "posBackup");
                if (!Directory.Exists(targetPath))
                {
                    Directory.CreateDirectory(targetPath);
                }
                File.SetAttributes(targetPath, System.IO.FileAttributes.Hidden);
                List<string> filenames = Directory.GetFileSystemEntries(targetPath).ToList();
                if (filenames.Count > 6)
                {
                    string file = filenames.OrderBy(r => Path.GetFileName(r)).FirstOrDefault();
                    if (File.Exists(file))
                    {
                        File.Delete(file);
                    }
                }
                string targetFileName = Path.Combine(targetPath, ("Pos" + DateTime.Now.ToString("yyyyMMddhhmmss") + ".db3"));
                File.Copy(filePath, targetFileName, true);
                logger.Info("开始上传数据");
                if (CheckConnect())
                {
                    //上传数据
                    bool result = SyncHelperBLL.SyncData(RuntimeObject.CurrentUser.bookID, RuntimeObject.CurrentUser.xls, RuntimeObject.CurrentUser.usercode, new List<string> { "clnt", "poshh", "posban", "billpayt" });
                    Thread.Sleep(2000);
                }
                logger.Info("上传数据成功");
            }
            catch (Exception)
            {
            }
            dlg.Close();
            if (tSwitchPrint.IsOn)
            {
                PosbanDetailModel data = new PosbanDetailModel();
                data.UserName = txtUserName.Text.Trim();
                data.Address = txtAddress.Text.Trim();
                data.DateTime = string.Format("{0}/{1}", lblStartDate.Text, lblEndDate.Text);
                data.TotalAmount = posbanDetail.TotalAmount;
                data.TotalOrderCount = posbanDetail.PosCount + posbanDetail.InvalidCount + posbanDetail.ReturnCount;
                data.TotalSale = posbanDetail.Money_Manual + posbanDetail.Money_Clnt + posbanDetail.Money_Sale + posbanDetail.Money_Exchange;
                data.Total_Recharge = posbanDetail.Total_Recharge;
                PrintPosBanHelper.Print(data);
            }
            Application.ExitThread();
            Application.Exit();
            Application.Restart();
            Process.GetCurrentProcess().Kill();
            // Restart();
        }
        #endregion

        private void Restart()
        {
            Thread thtmp = new Thread(new ParameterizedThreadStart(run));
            object appName = Application.ExecutablePath;
            Thread.Sleep(2000);
            thtmp.Start(appName);
        }

        private void run(Object obj)
        {
            Process ps = new Process();
            ps.StartInfo.FileName = obj.ToString();
            ps.Start();
        }
    }
}
