using POS.Common.utility;
using POS.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using System.Text;

namespace POS.DAL
{
    public class UserDAL
    {
        #region 登录
        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="userCode"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public UserModel Login(string userCode, string password)
        {
            SQLiteParameter[] parameters = new SQLiteParameter[2];

            string cmdText = "select usercode,username from [user] where usercode=@usercode and ifnull(password,'')=@password";
            parameters[0] = new SQLiteParameter("usercode", DbType.String);
            parameters[0].Value = userCode;
            parameters[1] = new SQLiteParameter("password", DbType.String);
            parameters[1].Value = password;

            try
            {
                SQLiteDataReader dataReader = SQLiteHelper.ExecuteReader(SQLiteHelper.connectionString, CommandType.Text, cmdText, parameters);
                UserModel user = null;
                while (dataReader.Read())
                {
                    user = new UserModel();
                    user.usercode = dataReader["usercode"].ToString();
                    user.username = dataReader["username"].ToString();
                }
                dataReader.Close();
                return user;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        #endregion

        #region 获取所有的用户
        /// <summary>
        /// 获取所有的用户
        /// </summary>
        /// <param name="userCode"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public List<UserModel> GetUsers(string xls)
        {
            string cmdText = "select usercode,username from [user] where xls=@xls";
            SQLiteParameter[] parameters = new SQLiteParameter[1];
            parameters[0] = new SQLiteParameter("xls", DbType.String);
            parameters[0].Value = xls;
            try
            {
                SQLiteDataReader dataReader = SQLiteHelper.ExecuteReader(SQLiteHelper.connectionString, CommandType.Text, cmdText,parameters);
               List< UserModel> users = new List<UserModel>();
                while (dataReader.Read())
                {
                    UserModel user = new UserModel();
                    user.usercode = dataReader["usercode"].ToString();
                    user.username = dataReader["username"].ToString();
                    users.Add(user);
                }
                dataReader.Close();
                return users;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        #endregion
    }
}
