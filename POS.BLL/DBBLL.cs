using POS.DAL;
using POS.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace POS.BLL
{
    public class DBBLL
    {
        DBDAL dbDAL = new DBDAL();

        #region 获取当前数据版本号
        /// <summary>
        /// 获取当前数据版本号
        /// </summary>
        /// <returns></returns>
        public string GetDBVersion()
        {
            try
            {
                return dbDAL.GetDBVersion();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region POS单、收款单添加上传数据状态列
        /// <summary>
        /// POS单、收款单添加上传数据状态列
        /// </summary>
        /// <param name="dbVersion"></param>
        /// <returns></returns>
        public bool UpdateDBV1(string dbVersion)
        {
            try
            {
                return dbDAL.UpdateDBV1(dbVersion);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region 当班记录添加上传数据状态列
        /// <summary>
        /// 当班记录添加上传数据状态列
        /// </summary>
        /// <param name="dbVersion"></param>
        /// <returns></returns>
        public bool UpdateDBV2(string dbVersion)
        {
            try
            {
                return dbDAL.UpdateDBV2(dbVersion);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region pos单添加历史记录列
        /// <summary>
        ///pos单添加历史记录列
        /// </summary>
        /// <param name="dbVersion"></param>
        /// <returns></returns>
        public bool UpdateDBV3(string dbVersion)
        {
            try
            {
                return dbDAL.UpdateDBV3(dbVersion);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region pos表体添加换货状态列
        /// <summary>
        ///pos表体添加换货状态列
        /// </summary>
        /// <param name="dbVersion"></param>
        /// <returns></returns>
        public bool UpdateDBV4(string dbVersion)
        {
            try
            {
                return dbDAL.UpdateDBV4(dbVersion);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region 添加会员日表
        /// <summary>
        ///添加会员日表
        /// </summary>
        /// <param name="dbVersion"></param>
        /// <returns></returns>
        public bool UpdateDBV5(string dbVersion)
        {
            try
            {
                return dbDAL.UpdateDBV5(dbVersion);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        public List<DBStructModel> GetServiceDBStruct(string url)
        {
            try
            {
                return dbDAL.GetServiceDBStruct(url);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
