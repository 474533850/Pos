using POS.DAL;
using POS.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace POS.BLL
{
    /// <summary>
    /// 收款单逻辑类
    /// </summary>
    public class OfhhBLL
    {
        OfhhDAL ofhhDAL = new OfhhDAL();

        #region 添加收款单
        /// <summary>
        /// 添加收款单
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public bool AddOfhh(OfhhModel entity)
        {
            try
            {
                return ofhhDAL.AddOfhh(entity);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        #endregion
    }
}
