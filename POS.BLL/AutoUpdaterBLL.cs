using POS.DAL;
using POS.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace POS.BLL
{
   public class AutoUpdaterBLL
    {
        /// <summary>
        /// 获取升级信息
        /// </summary>
        public static UpgradeModel GetUpgradeInfo()
        {
            try
            {
                return AutoUpdaterDAL.GetUpgradeInfo();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
