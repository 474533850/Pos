using POS.DAL;
using POS.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace POS.BLL
{
    public class GoodBLL
    {
        GoodDAL goodDAL = new GoodDAL();

        #region 模糊搜索货品
        /// <summary>
        /// 模糊搜索货品
        /// </summary>
        /// <param name="key">条码、品名、货品代码</param>
        /// <param name="xls">分部代码</param>
        /// <returns></returns>
        public List<GoodModel> GetGoodByKey(string key, string xls)
        {
            try
            {
                return goodDAL.GetGoodByKey(key);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region 根据货品ID取单条货品
        /// <summary>
        /// 根据货品ID取单条货品
        /// </summary>
        /// <param name="goodid">货品ID</param>
        /// <returns></returns>
        public GoodModel GetGoodByID(int goodid, string goodcode)
        {
            try
            {
                return goodDAL.GetGoodByID(goodid, goodcode);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        #endregion

        #region 根据货品编码取单条货品
        /// <summary>
        /// 根据货品编码取单条货品
        /// </summary>
        /// <param name="goodcode">货品编码</param>
        /// <returns></returns>
        public GoodModel GetGoodByCode(string goodcode)
        {
            try
            {
                return goodDAL.GetGoodByCode(goodcode);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        #endregion

        #region 获取兑换货品 
        /// <summary>
        /// 获取兑换货品
        /// </summary>
        /// <returns></returns>
        public List<GoodModel> GetExchangeGoods()
        {
            try
            {
                return goodDAL.GetExchangeGoods();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region 获取会员货品等级价格
        /// <summary>
        /// 获取会员货品等级价格
        /// </summary>
        /// <param name="goodid">货品ID</param>
        /// <returns></returns>
        public Goodpric2Model GetGoodpric2(int goodid, string clntclss)
        {
            try
            {
                return goodDAL.GetGoodpric2(goodid, clntclss);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        #endregion

        #region 获取货品大类
        /// <summary>
        /// 获取货品大类
        /// </summary>
        /// <param name="goodtype">货品类型</param>
        /// <returns></returns>
        public Goodtype1Model GetGoodtype1(string goodtype)
        {
            try
            {
                return goodDAL.GetGoodtype1(goodtype);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        #endregion

        #region 获取货品中类
        /// <summary>
        /// 获取货品中类
        /// </summary>
        /// <param name="goodtype">货品类型</param>
        /// <returns></returns>
        public Goodtype2Model GetGoodtype2(string goodtype)
        {
            try
            {
                return goodDAL.GetGoodtype2(goodtype);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        #endregion

        #region 获取货品小类
        /// <summary>
        /// 获取货品小类
        /// </summary>
        /// <param name="goodtype">货品类型</param>
        /// <returns></returns>
        public Goodtype3Model GetGoodtype3(string goodtype)
        {
            try
            {
                return goodDAL.GetGoodtype3(goodtype);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        #endregion

        #region  获取当前仓库的库存
        /// <summary>
        /// 获取当前仓库的库存
        /// </summary>
        /// <param name="xls"></param>
        /// <param name="cnkucode"></param>
        /// <returns></returns>
        public List<Ku2Model> GetKu2(string xls, string cnkucode)
        {
            try
            {
                return goodDAL.GetKu2(xls, cnkucode);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        #endregion

        #region  获取所有仓库的库存
        /// <summary>
        /// 获取所有仓库的库存
        /// </summary>
        /// <param name="xls"></param>
        /// <param name="cnkucode"></param>
        /// <returns></returns>
        public List<Ku2Model> GetKu2()
        {
            try
            {
                return goodDAL.GetKu2();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        #endregion

        #region  获取单个商品所有的库存
        /// <summary>
        /// 获取单个商品所有的库存
        /// </summary>
        /// <param name="key">货品代码货品名称</param>
        /// <returns></returns>
        public List<Ku2Model> GetAllKu2(GoodModel good)
        {
            try
            {
                return goodDAL.GetAllKu2(good);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        #endregion
    }
}
