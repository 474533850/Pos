using POS.DAL;
using POS.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace POS.BLL
{
    public class PosBanBLL
    {
        PosBanDAL posBanDAL = new PosBanDAL();

        #region 添加当班记录
        /// <summary>
        /// 添加当班记录
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public bool AddPosBan(PosbanModel entity)
        {
            try
            {
                return posBanDAL.AddPosBan(entity);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        #endregion

        #region 修改当班记录
        /// <summary>
        /// 修改当班记录
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public bool UpdatePosban(PosbanModel entity)
        {
            try
            {
                return posBanDAL.UpdatePosban(entity);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        /// <summary>
        /// 修改当班记录
        /// </summary>
        /// <param name="reduceMoney">减少金额</param>
        /// <param name="posnono">单据的当班号</param>
        /// <returns></returns>
        public bool UpdatePosban(decimal reduceMoney, string posnono)
        {
            try
            {
                return posBanDAL.UpdatePosban(reduceMoney, posnono);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        #endregion

        #region 获取单条当班记录
        /// <summary>
        /// 获取单条当班记录
        /// </summary>
        /// <param name="posnono"></param>
        /// <returns>当班单号</returns>
        public PosbanModel GetPosbanByNO(string posnono)
        {
            try
            {
                return posBanDAL.GetPosbanByNO(posnono);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        #endregion

        #region 判断是否交班
        /// <summary>
        /// 判断是否交班
        /// </summary>
        /// <returns>不为空为未交班</returns>
        public string IsShift()
        {
            try
            {
                return posBanDAL.IsShift();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        #endregion

        #region 获取交班详情
        /// <summary>
        ///获取交班详情
        /// </summary>
        /// <returns></returns>
        public PosbanDetailModel GetPosbanDetail(string posnono,out int appendCount)
        {
            try
            {
                return posBanDAL.GetPosbanDetail(posnono,out appendCount);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        #endregion
    }
}
