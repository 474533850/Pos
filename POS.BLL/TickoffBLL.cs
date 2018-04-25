using POS.DAL;
using POS.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace POS.BLL
{
    public class TickoffBLL
    {
        TickoffDAL tickoffDAL = new TickoffDAL();

        #region 获取一张优惠券
        /// <summary>
        /// 获取一张优惠券
        /// </summary>
        /// <param name="clntcode"></param>
        /// <returns>编码</returns>
        public TickoffmxModel GetTickoffmx(string xcode)
        {
            try
            {
                return tickoffDAL.GetTickoffmx(xcode);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region 修改优惠券
        /// <summary>
        /// 修改优惠券
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public bool UpdateClient(TickoffmxModel entity)
        {
            try
            {
                return tickoffDAL.UpdateClient(entity);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion
    }
}
