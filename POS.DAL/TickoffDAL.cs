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
    /// 优惠券
    /// </summary>
    public class TickoffDAL
    {
        #region 获取一张优惠券
        /// <summary>
        /// 获取一张优惠券
        /// </summary>
        /// <param name="clntcode"></param>
        /// <returns>编码</returns>
        public TickoffmxModel GetTickoffmx(string xcode)
        {

            SQLiteParameter[] parameters = new SQLiteParameter[1];
            StringBuilder cmdText = new StringBuilder();
            cmdText.AppendLine("select SID,xcode,xallp,xstate,xnotime,xtime1,xtime2");
            cmdText.AppendLine("from tickoffmx");
            cmdText.AppendLine("where lower(xcode) =@xcode");
            parameters[0] = new SQLiteParameter("xcode", DbType.String);
            parameters[0].Value = xcode.ToLower();

            try
            {
                SQLiteDataReader dataReader = SQLiteHelper.ExecuteReader(SQLiteHelper.connectionString, CommandType.Text, cmdText.ToString(), parameters);
                TickoffmxModel entity = null;
                while (dataReader.Read())
                {
                    entity = new TickoffmxModel();
                    entity.SID = dataReader["SID"].ToString() != string.Empty ? int.Parse(dataReader["SID"].ToString()) : (int?)null;
                    entity.xcode = dataReader["xcode"].ToString();
                    entity.xallp = (dataReader["xallp"] == null || string.IsNullOrEmpty(dataReader["xallp"].ToString())) ? 0 : decimal.Parse(dataReader["xallp"].ToString());
                    entity.xstate = dataReader["xstate"].ToString();
                    entity.xnotime = bool.Parse(dataReader["xnotime"].ToString());
                    entity.xtime1 = dataReader["xtime1"].ToString();
                    entity.xtime2 = dataReader["xtime2"].ToString();
                }
                dataReader.Close();
                return entity;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region 修改优惠券
        /// <summary>
        /// 修改优惠券
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public bool UpdateClient(TickoffmxModel entity)
        {

            try
            {
                SQLiteParameter[] parameters = new SQLiteParameter[7];

                string cmdText = @"UPDATE tickoffmx set clntcode=@clntcode,clntname=@clntname,xstate=@xstate,xopusetime=@xopusetime
                                   where SID=@SID";

                parameters[0] = new SQLiteParameter("clntcode", DbType.String);
                parameters[0].Value = entity.clntcode;
                parameters[1] = new SQLiteParameter("clntname", DbType.String);
                parameters[1].Value = entity.clntname;
                parameters[2] = new SQLiteParameter("xstate", DbType.String);
                parameters[2].Value = entity.xstate;
                parameters[3] = new SQLiteParameter("xopusetime", DbType.String);
                parameters[3].Value = entity.xopusetime;
                parameters[4] = new SQLiteParameter("SID", DbType.Int32);
                parameters[4].Value = entity.SID;

                int result = SQLiteHelper.ExecuteNonQuery(SQLiteHelper.connectionString, CommandType.Text, cmdText, parameters);

                return result > 0;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion
    }
}
