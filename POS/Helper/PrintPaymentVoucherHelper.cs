using DevExpress.XtraReports.UI;
using POS.BLL;
using POS.Common;
using POS.Common.Enum;
using POS.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using POS.Common.utility;
using DevExpress.XtraPrinting;
using System.Drawing;

namespace POS.Helper
{
    /// <summary>
    /// 打印营业款缴交凭证
    /// </summary>
    public class PrintPaymentVoucherHelper
    {
        public static void Print(PaymentVoucherModel PaymentVoucher)
        {
            if (PaymentVoucher != null)
            {
                PossettingBLL possettingBLL = new PossettingBLL();
                List<PossettingModel> possettings = possettingBLL.GetPossetting();
                string printName = "营业款缴交凭证";

                try
                {
                    XtraReport report = new XtraReport();
                    report.BeginInit();
                    string reportFolder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "report");
                    string reportPath = Path.Combine(reportFolder, printName + ".repx");
                    report.LoadLayout(reportPath);
                    report.EndInit();
                    //绑定
                    List<PaymentVoucherModel> dataList = new List<PaymentVoucherModel>();
                    dataList.Add(PaymentVoucher);
                    report.DataSource = dataList;

                    report.RequestParameters = false;
                    report.CreateDocument(false);
                    float maxvalue = 0;
                    BrickEnumerator be = report.Pages[report.Pages.Count - 1].GetEnumerator();
                    while (be.MoveNext())
                    {
                        RectangleF bounds = report.Pages[report.Pages.Count - 1].GetBrickBounds(be.CurrentBrick);
                        maxvalue = bounds.Bottom > maxvalue ? bounds.Bottom : maxvalue;
                    }
                    report.CreateDocument(false);
                    report.ShowPrintMarginsWarning = false;
                    report.PrintingSystem.ShowMarginsWarning = false;

                    PossettingModel entity = possettings.Where(r => r.xpname == AppConst.DefaultVoucherPrinter).FirstOrDefault();
                    if (entity != null)
                    {
                        report.PrinterName = entity.xpvalue;
                    }
                    //report.ShowPreview();
                   report.Print(report.PrinterName);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }
    }
}
