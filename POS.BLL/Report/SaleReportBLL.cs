using POS.DAL.Report;
using POS.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace POS.BLL.Report
{
    /// <summary>
    /// 销售报表
    /// </summary>
    public class SaleReportBLL
    {
        SaleReportDAL saleReportDAL = new SaleReportDAL();
        public List<SaleReportModel> GetSaleReport(string posnono, DateTime startDate, DateTime endDate)
        {
            try
            {
                return saleReportDAL.GetSaleReport(posnono, startDate, endDate);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
    }
}
