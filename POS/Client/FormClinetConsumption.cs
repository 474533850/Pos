using DevExpress.XtraPrinting;
using DevExpress.XtraPrinting.NativeBricks;
using DevExpress.XtraReports.UI;
using POS.BLL;
using POS.Common;
using POS.Common.Enum;
using POS.Common.utility;
using POS.Helper;
using POS.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web.Script.Serialization;
using System.Windows.Forms;

namespace POS.Client
{
    /// <summary>
    /// 会员消费历史
    /// </summary>
    public partial class FormClinetConsumption : BaseForm
    {
        int pageSize = 10;
        int currentPage = 1;
        long totalPage = 0;
        string clntcode = string.Empty;
        Uri baseUrl = new Uri(ConfigurationManager.AppSettings["serviceUrl"].ToString());

        JavaScriptSerializer js = new JavaScriptSerializer();

        public FormClinetConsumption(string clntcode)
        {
            InitializeComponent();
            panelBottom_SizeChanged(null, null);
            this.clntcode = clntcode;
            chkisAll.Checked = true;
            dteStart.DateTime = DateTime.Now;
            dteEnd.DateTime = DateTime.Now;
            Query();
            btnPage.Text = string.Format("{0}/{1}", currentPage, totalPage);
            Clear();
        }

        #region 分页按钮居中
        private void panelBottom_SizeChanged(object sender, EventArgs e)
        {
            this.panelPage.Left = (panelBottom.Width - panelPage.Width) / 2;
        }
        #endregion

        #region 搜索
        private void btnQuery_Click(object sender, EventArgs e)
        {
            Query();
        }

        private void Query()
        {

           // DateTime? startTime = null;
           // DateTime? endTime = null;
            string startTime = null;
            string endTime = null;
            if (!chkisAll.Checked)
            {
                startTime = dteStart.DateTime.Date.ToString("yyyy-MM-dd");
                endTime = dteEnd.DateTime.Date.ToString("yyyy-MM-dd");
            }
            Uri url = new Uri(baseUrl,
                string.Format("pos/poslog?sid={0}&clntcode={1}&startDate={2}&endDate={3}&page={4}&pageSize={5}&xls={6} "
                , RuntimeObject.CurrentUser.bookID, clntcode, startTime, endTime, currentPage, pageSize, RuntimeObject.CurrentUser.xls));
            string queryString = url.Query;
            if (!string.IsNullOrEmpty(queryString))
            {
                url = new Uri(url.ToString() + "&sign=" + GlobalHelper.GetSign(queryString, GetSecretKey()));
            }
            AsyncCallbackResult(url.ToString());
        }

        private void AsyncCallbackResult(string url)
        {
            if (CheckConnect())
            {
                DevExpress.Utils.WaitDialogForm dlg = null;
                try
                {
                    dlg = new DevExpress.Utils.WaitDialogForm("正在查询数据，请稍后……", new Size(250, 100));
                    dlg.Show();
                    //SyncResultModel<PoshhModel> data = HttpClient.GetAasync<PoshhModel>(url,null);
                    HttpClient.GetAasync<PoshhModel>(url, null, syncResult =>
                    {
                        SyncResultModel<PoshhModel> data = syncResult;
                        if (data.rows != null && data.rows.Count > 0)
                        {
                            foreach (var item in data.rows)
                            {
                                item.xintime = DateTime.Parse(item.xintime).ToString("yy-MM-dd HH:mm");
                            }

                            if (InvokeRequired)
                            {
                                this.Invoke((MethodInvoker)delegate { bdsPos.DataSource = data.rows; gvPOS.ViewCaption = " "; });
                            }
                            else
                            {
                                bdsPos.DataSource = data.rows;
                                gvPOS.ViewCaption =" ";
                            }
                        }
                        else
                        {
                            if (InvokeRequired)
                            {
                                this.Invoke((MethodInvoker)delegate { bdsPos.DataSource = null; gvPOS.ViewCaption = "没有消费记录"; });
                            }
                            else
                            {
                                bdsPos.DataSource = null;
                                gvPOS.ViewCaption = "没有消费记录";
                            }
                        }
                        totalPage = (data.total % pageSize) == 0 ? (data.total / pageSize) : (data.total / pageSize + 1);
                        if (InvokeRequired)
                        {
                            this.Invoke((MethodInvoker)delegate { btnPage.Text = string.Format("{0}/{1}", currentPage, totalPage); });
                        }
                        else
                        {
                            btnPage.Text = string.Format("{0}/{1}", currentPage, totalPage);
                        }
                    });

                    //this.Invoke((MethodInvoker)delegate
                    //{
                    //    btnPage.Text = string.Format("{0}/{1}", currentPage, totalPage);
                    //});
                }
                catch (Exception ex)
                {
                    MessagePopup.ShowError("获取会员消费记录失败！");
                }
                finally
                {
                    dlg.Close();
                }
            }
        }
        #endregion

        #region 全选日期
        private void chkisAll_CheckedChanged(object sender, EventArgs e)
        {
            dteStart.Enabled = !chkisAll.Checked;
            dteEnd.Enabled = !chkisAll.Checked;
        }
        #endregion

        #region 上一页
        private void btnPrevious_Click(object sender, EventArgs e)
        {
            if (currentPage > 1)
            {
                currentPage--;
                Query();
            }
        }
        #endregion

        #region 下一页
        private void btnNext_Click(object sender, EventArgs e)
        {
            if (currentPage < totalPage)
            {
                currentPage++;
                Query();
            }
        }
        #endregion

        #region 选择一张单据
        private void gvPOS_FocusedRowObjectChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowObjectChangedEventArgs e)
        {
            PoshhModel current = bdsPos.Current as PoshhModel;
            if (current != null)
            {
                lblquat.Text = current.Posbbs.Sum(r => r.xquat).ToString();
                lblTotalMoney.Text = current.xpay.ToString();

                bdsDetail.DataSource = current.Posbbs;
            }
            else
            {
                Clear();
            }
        }
        #endregion

        #region 清空明细
        private void Clear()
        {
            lblquat.Text = string.Empty; ;
            lblTotalMoney.Text = string.Empty;
            bdsDetail.DataSource = null;
        }
        #endregion

        #region 打印
        private void btnPrint_Click(object sender, EventArgs e)
        {
            // PrintHelper.Print(currentPoshh);
            //if (currentPoshh != null)
            //{
            //    PossettingBLL possettingBLL = new PossettingBLL();
            //    List<PossettingModel> possettings = possettingBLL.GetPossetting();
            //    PossettingModel entity = possettings.Where(r => r.xpname == AppConst.Print_Kind).FirstOrDefault();
            //    string printName = "58mm";
            //    int printNum = 1;
            //    if (entity != null)
            //    {
            //        if (!bool.Parse(entity.xpvalue))
            //        {
            //            printName = "80mm";
            //        }
            //    }
            //    entity = possettings.Where(r => r.xpname == AppConst.Print_Num).FirstOrDefault();
            //    if (entity != null)
            //    {
            //        printNum = int.Parse(entity.xpvalue) + 1;
            //    }

            //    PosModel posModel = new PosModel();
            //    posModel.xlsname = currentPoshh.xlsname;
            //    posModel.billno = currentPoshh.billno;
            //    posModel.totalCount =string.Format("共{0}项", currentPoshh.Posbbs.Count);
            //    posModel.xpay = currentPoshh.xpay;
            //    posModel.xhezhe = currentPoshh.xhezhe;
            //    Dictionary<string, string> payTypeDic = EnumHelper.GetEnumDictionary(typeof(PayType));
            //    BillpaytModel pay = currentPoshh.Billpayts.Where(r => r.paytname == payTypeDic[Enum.GetName(typeof(PayType), PayType.Cash)]).FirstOrDefault();
            //    if (pay !=null)
            //    {
            //        posModel.cash = pay.xpay;
            //    }
            //    pay = currentPoshh.Billpayts.Where(r => r.paytname == payTypeDic[Enum.GetName(typeof(PayType), PayType.Deposit)]).FirstOrDefault();
            //    if (pay != null)
            //    {
            //        posModel.deposit = pay.xpay;
            //    }
            //    pay = currentPoshh.Billpayts.Where(r => r.paytname == payTypeDic[Enum.GetName(typeof(PayType), PayType.Alipay)]).FirstOrDefault();
            //    posModel.paytype = payTypeDic[Enum.GetName(typeof(PayType), PayType.Alipay)];
            //    if (pay != null)
            //    {
            //        posModel.alipay = pay.xpay;
            //        posModel.paytype = payTypeDic[Enum.GetName(typeof(PayType), PayType.Alipay)];
            //    }
            //    pay = currentPoshh.Billpayts.Where(r => r.paytname == payTypeDic[Enum.GetName(typeof(PayType), PayType.WeChat)]).FirstOrDefault();
            //    if (pay != null)
            //    {
            //        posModel.alipay = pay.xpay;
            //        posModel.paytype = payTypeDic[Enum.GetName(typeof(PayType), PayType.WeChat)];
            //    }
            //    pay = currentPoshh.Billpayts.Where(r => r.paytname == payTypeDic[Enum.GetName(typeof(PayType), PayType.UnionpayCard)]).FirstOrDefault();
            //    if (pay != null)
            //    {
            //        posModel.alipay = pay.xpay;
            //        posModel.paytype = payTypeDic[Enum.GetName(typeof(PayType), PayType.UnionpayCard)];
            //    }
            //    decimal totalPay =posModel.deposit+ posModel.cash + posModel.alipay;
            //    posModel.balance = CalcMoneyHelper.Subtract(totalPay, posModel.xpay);
            //    posModel.xintime =Convert.ToDateTime(currentPoshh.xintime).ToString("MM-dd HH:mm:ss");
            //    posModel.Details = (from p in currentPoshh.Posbbs
            //                        select new PosDetailModel
            //                        {
            //                            goodcode = p.goodcode,
            //                            goodname = p.goodname,
            //                            xquat = p.xquat,
            //                            xpric = p.xpric,
            //                            xallp = p.xallp
            //                        }).ToList();

            //    try
            //    {
            //        XtraReport report = new XtraReport();
            //        report.ShowPrintMarginsWarning = false;
            //        report.PrintingSystem.ShowMarginsWarning = false;
            //        report.BeginInit();
            //        string reportFolder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "report");
            //        string reportPath = Path.Combine(reportFolder, printName + ".repx");
            //        report.LoadLayout(reportPath);
            //        report.EndInit();
            //        //绑定
            //        List<PosModel> dataList = new List<PosModel>();
            //        dataList.Add(posModel);
            //        //report.Parameters["IsPrintRemark"].Value = barcode.IsPrintRemark;
            //        report.DataSource = dataList;

            //        report.RequestParameters = false;
            //        report.CreateDocument(false);
            //        float maxvalue = 0;
            //        BrickEnumerator be = report.Pages[report.Pages.Count - 1].GetEnumerator();
            //        while (be.MoveNext())
            //        {
            //            RectangleF bounds = report.Pages[report.Pages.Count - 1].GetBrickBounds(be.CurrentBrick);
            //            maxvalue = bounds.Bottom > maxvalue ? bounds.Bottom : maxvalue;
            //        }
            //        report.PaperKind = System.Drawing.Printing.PaperKind.Custom;
            //        report.PageHeight = report.PageHeight * (report.Pages.Count - 1) - ((report.Margins.Top + report.Margins.Bottom) * (report.Pages.Count - 2)) + ((int)(maxvalue / 3 * 0.96f));
            //        while (report.Pages.Count > 1)
            //        {
            //            report.CreateDocument(false);
            //            report.PageHeight += 100;
            //            report.CreateDocument(false);
            //        }
            //        report.CreateDocument(false);

            //        for (int i = 0; i < printNum; i++)
            //        {
            //           // report.ShowPreview();
            //            report.Print();
            //        }
            //    }
            //    catch (Exception ex)
            //    {
            //        throw ex;
            //    }
            //}
        }
        #endregion

        #region 导出
        private void btnExport_Click(object sender, EventArgs e)
        {
            if (!GetPermission(Functions.Export))
            {
                return;
            }
            if (bdsDetail.List.Count > 0)
            {
                System.Windows.Forms.SaveFileDialog saveFileDialog = new SaveFileDialog();
                saveFileDialog.Filter = "导出Excel (*.xls)|*.xls";
                saveFileDialog.FilterIndex = 0;
                saveFileDialog.RestoreDirectory = true;
                saveFileDialog.FileName = GetReportExportFileName("会员消费记录");
                saveFileDialog.Title = "导出文件";

                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    DevExpress.XtraPrinting.XlsExportOptionsEx opEx = new DevExpress.XtraPrinting.XlsExportOptionsEx();
                    opEx.ExportType = DevExpress.Export.ExportType.WYSIWYG;
                    gvDetail.ExportToXls(saveFileDialog.FileName, opEx);
                }
            }
        }
        #endregion
    }
}
