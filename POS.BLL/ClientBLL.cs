using POS.DAL;
using POS.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace POS.BLL
{
    public class ClientBLL
    {
        ClientDAL clientDAL = new ClientDAL();

        #region 添加会员
        /// <summary>
        /// 添加会员
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public bool AddClient(ClntModel entity)
        {

            try
            {
                return clientDAL.AddClient(entity);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        #endregion

        #region 修改会员
        /// <summary>
        /// 修改会员
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public bool UpdateClient(ClntModel entity)
        {
            try
            {
                return clientDAL.UpdateClient(entity);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        #endregion

        #region 获取单个会员
        /// <summary>
        /// 获取单个会员
        /// </summary>
        /// <param name="ID"></param>
        /// <returns>会员ID</returns>
        public ClntModel GetClientByID(Guid ID)
        {
            try
            {
                return clientDAL.GetClientByID(ID);
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }
        /// <summary>
        /// 获取单个会员
        /// </summary>
        /// <param name="clntcode"></param>
        /// <returns>会员代码</returns>
        public ClntModel GetClientByCode(string clntcode)
        {
            try
            {
                return clientDAL.GetClientByCode(clntcode);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        #endregion

        #region 模糊搜索查找会员
        /// <summary>
        /// 模糊搜索查找会员
        /// </summary>
        /// <param name="key"></param>
        /// <returns>卡号、手机、会员姓名</returns>
        public List<ClntModel> GetClientByKey(string key)
        {
            try
            {
                return clientDAL.GetClientByKey(key);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        #endregion

        #region 会员统计
        /// <summary>
        /// 会员统计
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <param name="xls"></param>
        /// <returns></returns>
        public List<ClntModel> GetClientStatistics(DateTime startTime, DateTime endTime, string xls)
        {
            try
            {
                return clientDAL.GetClientStatistics(startTime, endTime, xls);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        #endregion
        #region 获取顾客类别
        /// <summary>
        /// 获取顾客类别
        /// </summary>
        /// <returns></returns>
        public List<ClnttypeModel> GetClnttype()
        {
            try
            {
                return clientDAL.GetClnttype();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        #endregion

        #region 获取顾客等级
        /// <summary>
        /// 获取顾客等级
        /// </summary>
        /// <returns></returns>
        public List<ClntclssModel> GetClntclss()
        {
            try
            {
                return clientDAL.GetClntclss();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        #endregion

        #region 获取一个顾客等级
        /// <summary>
        /// 获取一个顾客等级
        /// </summary>
        /// <returns></returns>
        public ClntclssModel GetClntclssByClass(string clntclss)
        {
            try
            {
                return clientDAL.GetClntclssByClass(clntclss);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        #endregion

        #region 获取会员积分结余
        /// <summary>
        /// 获取会员积分结余
        /// </summary>
        /// <param name="clintCode"></param>
        /// <returns>会员代码</returns>
        public decimal GetJjie2(string clntcode)
        {
            try
            {
                return clientDAL.GetJjie2(clntcode);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        #endregion

        #region 获取会员预存款结余
        /// <summary>
        /// 获取会员预存款结余
        /// </summary>
        /// <param name="clintCode"></param>
        /// <returns>会员代码</returns>
        public decimal GetOjie2(string clntcode)
        {
            try
            {
                return clientDAL.GetOjie2(clntcode);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        #endregion

        #region 判断是否存在会员电话号码
        /// <summary>
        /// 判断是否存在会员电话号码
        /// </summary>
        /// <param name="clnt"></param>
        /// <returns></returns>
        public bool ExistClntPhone(string pho, Guid? ID)
        {
            try
            {
                return clientDAL.ExistClntPhone(pho, ID);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        #endregion

        #region 获取所有会员积分结余
        /// <summary>
        /// 获取所有会员积分结余
        /// </summary>
        /// <param name="clintCode"></param>
        /// <returns>会员代码</returns>
        public List<Jjie2Model> GetJjie2()
        {
            try
            {
                return clientDAL.GetJjie2();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region 获取所有会员预存款结余
        /// <summary>
        /// 获取所有会员预存款结余
        /// </summary>
        /// <param name="clintCode"></param>
        /// <returns>会员代码</returns>
        public List<Ojie2Model> GetOjie2()
        {
            try
            {
                return clientDAL.GetOjie2();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region 获取默认顾客等级
        /// <summary>
        /// 获取默认顾客等级
        /// </summary>
        /// <returns></returns>
        public ClntclssModel GetDefaultClntclss()
        {
            try
            {
                return clientDAL.GetDefaultClntclss();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion
    }
}
