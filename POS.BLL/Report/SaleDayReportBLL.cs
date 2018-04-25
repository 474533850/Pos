using POS.DAL.Report;
using POS.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace POS.BLL.Report
{
    /// <summary>
    /// 销售日结报表
    /// </summary>
    public class SaleDayReportBLL
    {
        SaleDayReportDAL saleDayReportDAL = new SaleDayReportDAL();
        public List<SaleReportModel> GetSaleDayReport(DateTime startDate,DateTime endDate, out decimal debts)
        {
            try
            {
                return saleDayReportDAL.GetSaleDayReport(startDate, endDate,out debts);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        /// <summary>
        /// 日结支付明细
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        public List<BillpaytModel> GetSaleDayBillpayt(DateTime startDate, DateTime endDate)
        {
            try
            {
                return saleDayReportDAL.GetSaleDayBillpayt(startDate, endDate);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        }
}
