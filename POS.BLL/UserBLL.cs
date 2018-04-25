using POS.DAL;
using POS.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS.BLL
{
    public class UserBLL
    {
        UserDAL dal = new UserDAL();
        public UserModel Login(string userCode, string password)
        {
            try
            {
                return dal.Login(userCode, password);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #region 获取所有的用户
        /// <summary>
        /// 获取所有的用户
        /// </summary>
        /// <param name="userCode"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public List<UserModel> GetUsers(string xls)
        {
            try
            {
                return dal.GetUsers(xls);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion
    }
}
