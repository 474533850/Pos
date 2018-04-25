using POS.DAL;
using POS.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace POS.BLL
{
    public class PossettingBLL
    {
        PossettingDAL possettingDAL = new PossettingDAL();

        #region 添加系统设置
        /// <summary>
        /// 添加系统设置
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public bool AddPossetting(List<PossettingModel> entitys)
        {
            try
            {
                return possettingDAL.AddPossetting(entitys);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region 获取系统设置
        /// <summary>
        /// 获取系统设置
        /// </summary>
        /// <returns></returns>
        public List<PossettingModel> GetPossetting()
        {
            try
            {
                return possettingDAL.GetPossetting();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region 获取单系统设置
        /// <summary>
        /// 获取单系统设置
        /// </summary>
        /// <returns></returns>
        public PossettingModel GetPossettingByKey(string key)
        {
            try
            {
                return possettingDAL.GetPossettingByKey(key);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion
    }
}
