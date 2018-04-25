using POS.DAL;
using POS.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace POS.BLL
{
    /// <summary>
    /// 促销
    /// </summary>
    public class SaleBLL
    {
        SaleDAL saleDAL = new SaleDAL();

        #region 获取促销活动
        /// <summary>
        /// 获取促销活动
        /// </summary>
        /// <param name="clntCode">客户代码</param>
        /// <param name="goodcode">货品代码</param>
        /// <returns></returns>
        public SaleModel GetSale(string clntCode, GoodModel good, int xtableid)
        {
            try
            {
                return saleDAL.GetSale(clntCode, good, xtableid);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        #endregion

        #region 获取一个促销活动
        /// <summary>
        /// 获取一个促销活动
        /// </summary>
        /// <param name="saleID">促销活动ID</param>
        /// <returns></returns>
        public SaleModel GetSaleByID(int saleID)
        {
            try
            {
                return saleDAL.GetSaleByID(saleID);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        #endregion

        #region 获取满足当前时间段所有的促销活动
        public List<SaleModel> GetAllCurrentSales()
        {
            try
            {
                return saleDAL.GetAllCurrentSales();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public List<SaleModel> GetAllCurrentSalesDetail()
        {
            try
            {
                return saleDAL.GetAllCurrentSalesDetail();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region 获取货品满足所有的促销活动
        /// <summary>
        /// 获取货品满足所有的促销活动
        /// </summary>
        /// <param name="clntCode">客户代码</param>
        /// <param name="goodcode">货品代码</param>
        /// <param name="xtableid">货品ID</param>
        /// <returns></returns>
        public List<int> GetSaleIDs(string clntCode, string goodcode, int xtableid)
        {
            try
            {
                return saleDAL.GetSaleIDs(clntCode, goodcode, xtableid);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        #endregion

        #region 获取所有的促销活动
        public List<SaleModel> GetAllSales()
        {
            try
            {
                return saleDAL.GetAllSales();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        #endregion

        #region 获取会员日
        /// <summary>
        /// 获取会员日
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public ClntDayModel GetClntDay(int day, int month)
        {
            try
            {
                return saleDAL.GetClntDay(day,month);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        #endregion
    }
}
