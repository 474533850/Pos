using POS.DAL.Report;
using POS.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace POS.BLL.Report
{
    /// <summary>
    /// 零售明细表
    /// </summary>
    public class SaleDetailReportBLL
    {
        SaleDetailReportDAL saleDetailReportDAL = new SaleDetailReportDAL();
        public List<SaleDetailReportModel> Get(string billNO, string goodKey, string clntKey, DateTime? startDate, DateTime? endDate, bool isAll)
        {
            try
            {
                return saleDetailReportDAL.Get(billNO, goodKey, clntKey,startDate, endDate, isAll);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
    }
}
