using POS.DAL;
using POS.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace POS.BLL
{
    public class POSBLL
    {
        POSDAL posDAL = new POSDAL();

        #region 查询Pos单_分页
        /// <summary>
        /// 查询Pos单_分页
        /// </summary>
        /// <param name="page">当前页数</param>
        /// <param name="pageSize">每页显示多少条</param>
        /// <returns></returns>
        public List<PoshhModel> GetPOS(int page, int pageSize, string key, string goodKey, string clntKey, DateTime? startDate, DateTime? endDate, bool isAll, int uploadstatus,out long totalPage)
        {
            try
            {
                return posDAL.GetPOS(page, pageSize, key,goodKey, clntKey, startDate, endDate, isAll, uploadstatus, out totalPage);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        #endregion

        #region 添加Pos单
        /// <summary>
        /// 添加Pos单
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public bool AddPOS(PoshhModel entity, out string billno)
        {
            try
            {
                return posDAL.AddPOS(entity, out billno);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        #endregion

        #region 查询Pos挂单单据
        /// <summary>
        /// 查询Pos挂单单据
        /// </summary>
        /// <returns></returns>
        public List<PoshhModel> GetPOSPending()
        {
            try
            {
                return posDAL.GetPOSPending();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        #endregion

        #region 查询一张Pos单据
        /// <summary>
        /// 查询一张Pos单据
        /// </summary>
        /// <returns></returns>
        public PoshhModel GetPOSByID(Guid ID)
        {
            try
            {
                return posDAL.GetPOSByID(ID);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        /// <summary>
        /// 查询一张Pos单据
        /// </summary>
        /// <returns></returns>
        public PoshhModel GetPOSByPNO(string billno)
        {
            try
            {
                return posDAL.GetPOSByPNO(billno);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        #endregion

            #region  修改挂单
            /// <summary>
            /// 修改挂单
            /// </summary>
            /// <param name="entity"></param>
            /// <returns></returns>
        public bool UpdatePosbb(PosbbModel entity)
        {
            try
            {
                return posDAL.UpdatePosbb(entity);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        #endregion

        #region 作废
        /// <summary>
        /// 作废
        /// </summary>
        /// <returns></returns>
        public bool Invalid(Guid ID, string remark)
        {
            try
            {
                return posDAL.Invalid(ID, remark);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        #endregion

        #region 挂单（追加）
        /// <summary>
        /// 挂单（追加）
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public bool Append(PoshhModel entity)
        {
            try
            {
                return posDAL.Append(entity);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        #endregion

        #region 取单收款
        /// <summary>
        /// 取单收款
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public bool PendingReceipt(PoshhModel entity)
        {
            try
            {
                return posDAL.PendingReceipt(entity);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        #endregion

        #region 退货
        /// <summary>
        /// 退货
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public bool Returned(PoshhModel entity, Guid ID)
        {
            try
            {
                return posDAL.Returned(entity, ID);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        #endregion

        #region 快速开单
        /// <summary>
        /// 快速开单
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public bool FastBilling(PoshhModel entity, out string billno)
        {
            try
            {
                return posDAL.FastBilling(entity, out billno);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        #endregion

        #region 快速开单（追加）
        /// <summary>
        /// 快速开单（追加）
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public bool FastBillingAppend(PoshhModel entity)
        {
            try
            {
                return posDAL.FastBillingAppend(entity);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        #endregion

        #region 换货
        /// <summary>
        /// 换货
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public bool ChangeGoods(PoshhModel entity, Guid ID, out string billno)
        {
            try
            {
                return posDAL.ChangeGoods(entity, ID, out billno);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        #endregion
    }
}
