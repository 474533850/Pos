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
    /// 分部
    /// </summary>
    public class LsDAL
    {
        #region 获取分部
        /// <summary>
        /// 获取分部
        /// </summary>
        /// <returns></returns>
        public List<LsModel> GetLs()
        {
            string cmdText = "select xlstype,xls,xlsname from ls";

            try
            {
                SQLiteDataReader dataReader = SQLiteHelper.ExecuteReader(SQLiteHelper.connectionString, CommandType.Text, cmdText);
                List<LsModel> list = new List<LsModel>();

                while (dataReader.Read())
                {
                    LsModel entity = new LsModel();
                    entity.xlstype = dataReader["xlstype"].ToString();
                    entity.xls = dataReader["xls"].ToString();
                    entity.xlsname = dataReader["xlsname"].ToString();
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
