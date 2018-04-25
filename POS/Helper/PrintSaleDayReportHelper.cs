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
    public class PrintSaleDayReportHelper
    {
        public static void Print(PosModel posModel)
        {
            if (posModel != null)
            {
                PossettingBLL possettingBLL = new PossettingBLL();
                List<PossettingModel> possettings = possettingBLL.GetPossetting();
                PossettingModel entity = possettings.Where(r => r.xpname == AppConst.Print_Kind).FirstOrDefault();
                string reportName = "销售日结";
                string printName =string.Format("{0}{1}",reportName, "58mm");
                int printNum = 1;
                if (entity != null)
                {
                    if (!bool.Parse(entity.xpvalue))
                    {
                        printName = string.Format("{0}{1}", reportName, "80mm");
                    }
                }
                entity = possettings.Where(r => r.xpname == AppConst.Print_Num).FirstOrDefault();
                if (entity != null)
                {
                    printNum = int.Parse(entity.xpvalue) + 1;
                }
                try
                {
                    XtraReport report = new XtraReport();
                    report.BeginInit();
                    string reportFolder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "report");
                    string reportPath = Path.Combine(reportFolder, printName + ".repx");
                    report.LoadLayout(reportPath);
                    report.EndInit();
                    //绑定
                    List<PosModel> dataList = new List<PosModel>();
                    dataList.Add(posModel);
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

                    entity = possettings.Where(r => r.xpname == AppConst.DefaultPrinter).FirstOrDefault();
                    if (entity != null)
                    {
                        report.PrinterName = entity.xpvalue;
                    }

                    for (int i = 0; i < printNum; i++)
                    {
                        //report.ShowPreview();
                        report.Print(report.PrinterName);
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }
    }
}
