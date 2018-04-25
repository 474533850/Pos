using POS.Common;
using POS.Common.Enum;
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
    /// <summary>
    /// 收款单逻辑类
    /// </summary>
    public class OfhhDAL : BaseDAL
    {
        #region 添加收款单
        /// <summary>
        /// 添加收款单
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public bool AddOfhh(OfhhModel entity)
        {
            Dictionary<string, string> stateDic = EnumHelper.GetEnumDictionary(typeof(PosState));
            Dictionary<string, string> couponStateDic = EnumHelper.GetEnumDictionary(typeof(CouponState));

            using (SQLiteConnection con = new SQLiteConnection(SQLiteHelper.connectionString))
            {
                using (SQLiteCommand cmd = new SQLiteCommand())
                {
                    con.Open();
                    SQLiteTransaction sqltran = con.BeginTransaction();
                    cmd.Connection = con;
                    cmd.Transaction = sqltran;

                    try
                    {
                        #region 表头
                        SQLiteParameter[] parameters = new SQLiteParameter[11];

                        StringBuilder cmdText = new StringBuilder();
                        cmdText.AppendLine("INSERT INTO ofhh(");
                        cmdText.AppendLine("ID,xdate,clntcode,clntname,xnote,xinname,xintime,xversion,SID,xtableid,xnote)");
                        cmdText.AppendLine("VALUES(");
                        cmdText.AppendLine("@ID,@xdate,@clntcode,@clntname,@xnote,@xinname,@xintime,@xversion,@SID,@xtableid,@xnote)");
                      
                        entity.ID = entity.ID;
                        parameters[0] = new SQLiteParameter("ID", DbType.String);
                        parameters[0].Value = entity.ID;
                        parameters[1] = new SQLiteParameter("xdate", DbType.DateTime);
                        parameters[1].Value = DateTime.Now.ToString("yyyy-MM-dd");
                        parameters[2] = new SQLiteParameter("clntcode", DbType.String);
                        parameters[2].Value = entity.clntcode;
                        parameters[3] = new SQLiteParameter("clntname", DbType.String);
                        parameters[3].Value = entity.clntname;
                        parameters[4] = new SQLiteParameter("xnote", DbType.String);
                        parameters[4].Value = entity.xnote;
                        parameters[5] = new SQLiteParameter("xinname", DbType.String);
                        parameters[5].Value = entity.xinname;
                        parameters[6] = new SQLiteParameter("xintime", DbType.String);
                        parameters[6].Value = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                        parameters[7] = new SQLiteParameter("xversion", DbType.Double);
                        parameters[7].Value = entity.xversion;
                        parameters[8] = new SQLiteParameter("SID", DbType.Int32);
                        parameters[8].Value = entity.SID;
                        parameters[9] = new SQLiteParameter("xtableid", DbType.Int32);
                        parameters[9].Value = entity.xtableid;
                        parameters[10] = new SQLiteParameter("xnote", DbType.String);
                        parameters[10].Value = entity.xnote;

                        cmd.CommandText = cmdText.ToString();
                        cmd.Parameters.AddRange(parameters);
                        cmd.ExecuteNonQuery();
                        #endregion

                        #region 表体
                        foreach (OfbbModel item in entity.ofbbs)
                        {
                            cmd.Parameters.Clear();

                            AddOfbb(entity, out parameters, out cmdText, item);

                            cmd.CommandText = cmdText.ToString();
                            cmd.Parameters.AddRange(parameters);
                            cmd.ExecuteNonQuery();
                        }
                        #endregion

                        sqltran.Commit();
                        return true;
                    }
                    catch (Exception ex)
                    {
                        sqltran.Rollback();
                        throw ex;
                    }
                }
            }

        }
        #endregion

        #region 添加表体
        private static void AddOfbb(OfhhModel entity, out SQLiteParameter[] parameters, out StringBuilder cmdText, OfbbModel item)
        {
            parameters = new SQLiteParameter[13];

            cmdText = new StringBuilder();
            cmdText.AppendLine("INSERT INTO ofbb(");
            cmdText.AppendLine("ID,XID,xnoteb,xztype,xzstate,paybillno,xfee,");
            cmdText.AppendLine("xsubsidy,transno,xversion,SID,xtableid,xsubid)");
            cmdText.AppendLine("VALUES(");
            cmdText.AppendLine("@ID,@XID,@xnoteb,@xztype,@xzstate,@paybillno,@xfee,");
            cmdText.AppendLine("@xsubsidy,@transno,@xversion,@SID,@xtableid,@xsubid)");

            parameters[0] = new SQLiteParameter("ID", DbType.String);
            parameters[0].Value = item.ID;
            parameters[1] = new SQLiteParameter("XID", DbType.String);
            parameters[1].Value = entity.ID;
            parameters[2] = new SQLiteParameter("xnoteb", DbType.String);
            parameters[2].Value = item.xnoteb;
            parameters[3] = new SQLiteParameter("xztype", DbType.String);
            parameters[3].Value = item.xztype;
            parameters[4] = new SQLiteParameter("xzstate", DbType.String);
            parameters[4].Value = item.xzstate;
            parameters[5] = new SQLiteParameter("paybillno", DbType.String);
            parameters[5].Value = string.Empty;
            parameters[6] = new SQLiteParameter("xfee", DbType.Decimal);
            parameters[6].Value = item.xfee;
            parameters[7] = new SQLiteParameter("xsubsidy", DbType.Decimal);
            parameters[7].Value = item.xsubsidy;
            parameters[8] = new SQLiteParameter("transno", DbType.String);
            parameters[8].Value =item.transno;
            parameters[9] = new SQLiteParameter("xversion", DbType.Double);
            parameters[9].Value = item.xversion;
            parameters[10] = new SQLiteParameter("SID", DbType.Int32);
            parameters[10].Value =item.SID;
            parameters[11] = new SQLiteParameter("xtableid", DbType.Int32);
            parameters[11].Value = item.xtableid;
            parameters[12] = new SQLiteParameter("xsubid", DbType.Int32);
            parameters[12].Value = item.xsubid;
        }
        #endregion
    }
}
