using POS.DAL;
using POS.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace POS.BLL
{
    /// <summary>
    /// 账户
    /// </summary>
    public class PaytBLL
    {
        PaytDAL paytDAL = new PaytDAL();

        #region 获取账户
        /// <summary>
        /// 获取账户
        /// </summary>
        /// <returns></returns>
        public List<PaytModel> GetPayt()
        {
            try
            {
                return paytDAL.GetPayt();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion
    }
}
