using POS.DAL;
using POS.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace POS.BLL
{
    /// <summary>
    /// 分部
    /// </summary>
    public class LsBLL
    {
        LsDAL lsDAL = new LsDAL();

        #region 获取分部
        /// <summary>
        /// 获取分部
        /// </summary>
        /// <returns></returns>
        public List<LsModel> GetLs()
        {
            try
            {
                return lsDAL.GetLs();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        #endregion
    }
}
