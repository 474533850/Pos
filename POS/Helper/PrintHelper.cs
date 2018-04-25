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
using System.Text.RegularExpressions;

namespace POS.Helper
{
    public class PrintHelper
    {
        public static void Print(PoshhModel currentPoshh)
        {
            if (currentPoshh != null)
            {
                PossettingBLL possettingBLL = new PossettingBLL();
                List<PossettingModel> possettings = possettingBLL.GetPossetting();
                PossettingModel entity = possettings.Where(r => r.xpname == AppConst.Print_Kind).FirstOrDefault();
                string printName = "58mm";
                int printNum = 1;
                if (entity != null)
                {
                    if (!bool.Parse(entity.xpvalue))
                    {
                        printName = "80mm";
                    }
                }
                entity = possettings.Where(r => r.xpname == AppConst.Print_Num).FirstOrDefault();
                if (entity != null)
                {
                    printNum = int.Parse(entity.xpvalue) + 1;
                }

                PosModel posModel = new PosModel();
                posModel.xlsname = currentPoshh.xlsname;
                posModel.billno = currentPoshh.billno;
                posModel.totalCount = string.Format("共{0}项", currentPoshh.Posbbs.Count);
                // posModel.xpay = currentPoshh.xpay;
                posModel.xpay = CalcMoneyHelper.Subtract(currentPoshh.xheallp, (currentPoshh.xhezhe+currentPoshh.xrpay));
                posModel.xhezhe = currentPoshh.xhezhe;
                if (!string.IsNullOrEmpty(currentPoshh.xpho))
                {
                    posModel.clntcode = Regex.Replace(currentPoshh.xpho, "(\\d{3})\\d{4}(\\d{4})", "$1****$2");
                }
                posModel.xsendjf = currentPoshh.xsendjf;
     
                UserModel user = RuntimeObject.CurrentUser;
                DateTime dt = DateTime.Now;
                if (SyncHelperBLL.CheckConnect(out dt, user.bookID, user.username, user.password))
                {
                    posModel.totalXsendjf = SyncHelperBLL.Getjifen(user.bookID, currentPoshh.clntcode,user.xls).ToString();
                }
                else
                {
                    posModel.totalXsendjf = "待查";
                }
                entity = possettings.Where(r => r.xpname == AppConst.Shop_Address).FirstOrDefault();
                if (entity != null)
                {
                    posModel.address = entity.xpvalue;
                }
                entity = possettings.Where(r => r.xpname == AppConst.Consumer_Pho).FirstOrDefault();
                if (entity != null)
                {
                    posModel.telephone = entity.xpvalue;
                }

                Dictionary<string, string> payTypeDic = EnumHelper.GetEnumDictionary(typeof(PayType));
                posModel.paytype = payTypeDic[Enum.GetName(typeof(PayType), PayType.Alipay)] + ":";
                if (currentPoshh.payts != null)
                {
                    IEnumerable<BillpaytModel> pay = currentPoshh.payts.Where(r => r.paytname == payTypeDic[Enum.GetName(typeof(PayType), PayType.Cash)]);
                    if (pay != null)
                    {
                        posModel.cash = pay.Where(r => r.xreceipt.HasValue).Sum(r => r.xreceipt)>0? pay.Where(r => r.xreceipt.HasValue).Sum(r => r.xreceipt.Value) : pay.Sum(r => r.xpay);
                        //pay.xreceipt.HasValue ? pay.xreceipt.Value : pay.xpay;
                    }
                    pay = currentPoshh.payts.Where(r => r.paytname == payTypeDic[Enum.GetName(typeof(PayType), PayType.Deposit)] && r.xnote1 != "积分兑换");
                    if (pay != null)
                    {
                        posModel.deposit = pay.Sum(r => r.xpay);
                    }
                    pay = currentPoshh.payts.Where(r => r.paytname == payTypeDic[Enum.GetName(typeof(PayType), PayType.Deposit)] && r.xnote1 == "积分兑换");
                    if (pay != null)
                    {
                        posModel.deductible = pay.Sum(r => r.xpay);
                    }
                    pay = currentPoshh.payts.Where(r => r.paytname == payTypeDic[Enum.GetName(typeof(PayType), PayType.Alipay)]);
                    if (pay != null)
                    {
                        posModel.alipay = pay.Sum(r => r.xpay);
                        posModel.paytype = payTypeDic[Enum.GetName(typeof(PayType), PayType.Alipay)] + ":";
                    }
                    pay = currentPoshh.payts.Where(r => r.paytname == payTypeDic[Enum.GetName(typeof(PayType), PayType.WeChat)]);
                    if (pay != null)
                    {
                        posModel.alipay = pay.Sum(r => r.xpay);
                        posModel.paytype = payTypeDic[Enum.GetName(typeof(PayType), PayType.WeChat)] + ":";
                    }
                    pay = currentPoshh.payts.Where(r => r.xnote1 == payTypeDic[Enum.GetName(typeof(PayType), PayType.UnionpayCard)]);
                    if (pay != null)
                    {
                        posModel.unionpaycard = pay.Sum(r => r.xpay);
                    }
                    pay = currentPoshh.payts.Where(r => r.paytname == payTypeDic[Enum.GetName(typeof(PayType), PayType.Coupon)]);
                    if (pay != null)
                    {
                        posModel.coupon = pay.Sum(r => r.xpay);
                    }
                    pay = currentPoshh.payts.Where(r => r.paytname == payTypeDic[Enum.GetName(typeof(PayType), PayType.Check)]);
                    if (pay != null)
                    {
                        posModel.check = pay.Sum(r => r.xpay);
                    }
                }
                decimal totalPay = posModel.deposit + posModel.cash + posModel.alipay + posModel.unionpaycard + posModel.coupon + posModel.deductible + posModel.check;
                if (totalPay == 0)
                {
                    posModel.balance = 0;
                }
                else
                {
                    posModel.balance = CalcMoneyHelper.Subtract(totalPay, posModel.xpay);
                }

                posModel.xintime = Convert.ToDateTime(currentPoshh.xintime).ToString("MM-dd HH:mm:ss");
                posModel.Details = (from p in currentPoshh.Posbbs
                                    select new PosDetailModel
                                    {
                                        goodcode = p.goodcode,
                                        goodname = p.goodname,
                                        xquat = p.xquat,
                                        xpric = p.xpric,
                                        xallp = p.xallp
                                    }).ToList();

                try
                {
                    XtraReport report = new XtraReport();
                    //report.ShowPrintMarginsWarning = false;
                    //report.PrintingSystem.ShowMarginsWarning = false;
                    report.BeginInit();
                    string reportFolder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "report");
                    string reportPath = Path.Combine(reportFolder, printName + ".repx");
                    report.LoadLayout(reportPath);
                    report.EndInit();
                    //绑定
                    List<PosModel> dataList = new List<PosModel>();
                    dataList.Add(posModel);
                    //report.Parameters["IsPrintRemark"].Value = barcode.IsPrintRemark;
                    report.DataSource = dataList;

                    report.RequestParameters = false;
                    report.CreateDocument(false);
                    // float maxvalue = 0;
                    //BrickEnumerator be = report.Pages[report.Pages.Count - 1].GetEnumerator();
                    //while (be.MoveNext())
                    //{
                    //    RectangleF bounds = report.Pages[report.Pages.Count - 1].GetBrickBounds(be.CurrentBrick);
                    //    maxvalue = bounds.Bottom > maxvalue ? bounds.Bottom : maxvalue;
                    //}
                    //report.PaperKind = System.Drawing.Printing.PaperKind.Custom;
                    //report.PageHeight = report.PageHeight * (report.Pages.Count - 1) - ((report.Margins.Top + report.Margins.Bottom) * (report.Pages.Count - 2)) + ((int)(maxvalue / 3 * 0.96f));
                    //while (report.Pages.Count > 1)
                    //{
                    //    report.CreateDocument(false);
                    //    report.PageHeight += 100;
                    //    report.CreateDocument(false);
                    //}
                    //report.CreateDocument(false);
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
