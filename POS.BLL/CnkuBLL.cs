using POS.DAL;
using POS.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace POS.BLL
{
    public class CnkuBLL
    {
        CnkuDAL cnkuDAL = new CnkuDAL();

        #region 获取分部仓库
        /// <summary>
        /// 获取分部仓库
        /// </summary>
        /// <returns></returns>
        public List<CnkuModel> GetCnku(string xls)
        {
            try
            {
                return cnkuDAL.GetCnku(xls);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        #endregion
    }
}
