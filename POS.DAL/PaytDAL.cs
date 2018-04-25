using POS.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using System.Text;

namespace POS.DAL
{
    /// <summary>
    /// 账户
    /// </summary>
    public class PaytDAL
    {
        #region 获取账户
        /// <summary>
        /// 获取账户
        /// </summary>
        /// <returns></returns>
        public List<PaytModel> GetPayt()
        {
            string cmdText = "select * from payt";

            try
            {
                SQLiteDataReader dataReader = SQLiteHelper.ExecuteReader(SQLiteHelper.connectionString, CommandType.Text, cmdText);
                List<PaytModel> list = new List<PaytModel>();

                while (dataReader.Read())
                {
                    PaytModel entity = new PaytModel();
                    entity.payttype = dataReader["payttype"].ToString();
                    entity.paytcode = dataReader["paytcode"].ToString();
                    entity.paytname = dataReader["paytname"].ToString();
                    list.Add(entity);
                }
                dataReader.Close();
                return list;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion
    }
}
