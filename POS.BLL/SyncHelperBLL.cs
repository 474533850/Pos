using POS.DAL.Sync;
using POS.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace POS.BLL
{
    public class SyncHelperBLL
    {
        #region 同步数据
        /// <summary>
        /// 同步数据
        /// </summary>
        /// <param name="sid">账套号</param>
        /// <param name="xls">分部代码</param>
        /// <param name="usercode">用户代码</param>
        /// <returns></returns>

        public static bool SyncData(string sid, string xls, string usercode, List<string> tables, int id=0, Guid? posid = null,string goodcode="",string clntcode="")
        {
            try
            {
                return SyncHelperDAL.SyncData(sid, xls, usercode, tables, id,posid,goodcode, clntcode);
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        public static bool SyncData(string sid, string xls, string usercode, string username, string password, List<string> tables, int id = 0, Guid? posid = null, string goodcode = "", string clntcode = "")
        {
            try
            {
                return SyncHelperDAL.SyncData(sid, xls, usercode, username, password,tables, id, posid, goodcode, clntcode);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region 检查网络是否能连接到服务器
        public static bool CheckConnect(out DateTime currentTime, string sid, string username, string password, int second=10)
        {
            currentTime = DateTime.Now;
            try
            {
                bool result = SyncHelperDAL.CheckConnect(out currentTime,sid,username,password, second);
                return result;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        #endregion

        #region 检查签名是否正确
        public static bool Cksign(string sid,string xls)
        {
            try
            {
                bool result = SyncHelperDAL.Cksign(sid,xls);
                return result;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        #endregion

        #region 获取会员积分结余
        public static decimal Getjifen(string sid, string userCode, string xls)
        {
            try
            {
                return SyncHelperDAL.Getjifen(sid, userCode,xls);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region 获取预存款余额
        public static decimal Getyck(string sid, string userCode, string xls)
        {
            try
            {
                return SyncHelperDAL.Getyck(sid, userCode,xls);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region 获取同步的消息
        public static List<SyncInfoModel> GetSyncInfo()
        {

            try
            {
                return SyncHelperDAL.GetSyncInfo();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region 修改同步的消息为已读
        public static bool UpdateSyncInfo()
        {
            try
            {
                return SyncHelperDAL.UpdateSyncInfo();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region 上传错误日志
        public static void UploadErrLog(string msg, string sid, string xls, string usercode)
        {
            try
            {
                SyncHelperDAL.UploadErrLog(msg, sid, xls, usercode);
            }
            catch (Exception ex)
            {
            }
        }
        #endregion

        #region 恢复单据的修改状态
        /// <summary>
        /// 恢复单据的修改状态
        /// </summary>
        public static void ReturnUploadstatus()
        {
            try
            {
                SyncHelperDAL.ReturnUploadstatus();
            }
            catch (Exception ex)
            {
            }
        }
        #endregion
    }
}
