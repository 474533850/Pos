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
    /// 仓库
    /// </summary>
    public class CnkuDAL
    {
        #region 获取分部仓库
        /// <summary>
        /// 获取分部仓库
        /// </summary>
        /// <returns></returns>
        public List<CnkuModel> GetCnku(string xls)
        {
            string cmdText = "select cnkutype,cnkucode,cnkuname,xls,xlsname from cnku where xls=@xls";

            SQLiteDataReader dataReader = null;
            try
            {
                SQLiteParameter[] parameters = new SQLiteParameter[1];
                parameters[0] = new SQLiteParameter("xls", DbType.String);
                parameters[0].Value = xls;
                dataReader = SQLiteHelper.ExecuteReader(SQLiteHelper.connectionString, CommandType.Text, cmdText, parameters);
                List<CnkuModel> list = new List<CnkuModel>();

                while (dataReader.Read())
                {
                    CnkuModel entity = new CnkuModel();
                    entity.cnkutype = dataReader["cnkutype"].ToString();
                    entity.cnkucode = dataReader["cnkucode"].ToString();
                    entity.cnkuname = dataReader["cnkuname"].ToString();
                    entity.xls = dataReader["xls"].ToString();
                    entity.xlsname = dataReader["xlsname"].ToString();
                    list.Add(entity);
                }
                return list;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (dataReader != null)
                {
                    dataReader.Close();
                }
            }
        }
        #endregion
    }
}
