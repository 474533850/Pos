using POS.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using System.Text;

namespace POS.DAL
{
    public class PossettingDAL : BaseDAL
    {
        #region 添加系统设置
        /// <summary>
        /// 添加系统设置
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public bool AddPossetting(List<PossettingModel> entitys)
        {
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
                        foreach (PossettingModel item in entitys)
                        {
                            string cmdText = "select count(*) from possetting where xpname=@xpname";
                            SQLiteParameter[] parameters = new SQLiteParameter[1];
                            parameters[0] = new SQLiteParameter("xpname", DbType.String);
                            parameters[0].Value = item.xpname;
                            cmd.Parameters.Clear();
                            cmd.CommandText = cmdText;
                            cmd.Parameters.AddRange(parameters);
                            long result = (long)cmd.ExecuteScalar();

                            if (result == 0)
                            {
                                parameters = new SQLiteParameter[5];
                                parameters[0] = new SQLiteParameter("issys", DbType.Boolean);
                                parameters[0].Value = item.issys;
                                parameters[1] = new SQLiteParameter("xpname", DbType.String);
                                parameters[1].Value = item.xpname;
                                parameters[2] = new SQLiteParameter("xpvalue", DbType.String);
                                parameters[2].Value = item.xpvalue;
                                parameters[3] = new SQLiteParameter("usercode", DbType.String);
                                parameters[3].Value = item.usercode;
                                parameters[4] = new SQLiteParameter("xversion", DbType.Double);
                                parameters[4].Value = GetTimeStamp();

                                cmdText = @"INSERT INTO possetting (
                                               issys,
                                               xpname,
                                               xpvalue,
                                               usercode,
                                               xversion
                                           )
                                           VALUES (
                                               @issys,
                                               @xpname,
                                               @xpvalue,
                                               @usercode,
                                               @xversion
                                           )";

                                cmd.Parameters.Clear();
                                cmd.CommandText = cmdText.ToString();
                                cmd.Parameters.AddRange(parameters);
                                cmd.ExecuteNonQuery();
                            }
                            else
                            {
                                cmdText = "update possetting set xpvalue=@xpvalue where xpname=@xpname";
                                parameters = new SQLiteParameter[2];
                                parameters[0] = new SQLiteParameter("xpvalue", DbType.String);
                                parameters[0].Value = item.xpvalue;
                                parameters[1] = new SQLiteParameter("xpname", DbType.String);
                                parameters[1].Value = item.xpname;
                                cmd.Parameters.Clear();
                                cmd.CommandText = cmdText.ToString();
                                cmd.Parameters.AddRange(parameters);
                                cmd.ExecuteNonQuery();
                            }

                        }
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

        #region 获取系统设置
        /// <summary>
        /// 获取系统设置
        /// </summary>
        /// <returns></returns>
        public List<PossettingModel> GetPossetting()
        {
            string cmdText = "select issys,xpname,xpvalue,usercode from possetting";

            try
            {
                SQLiteDataReader dataReader = SQLiteHelper.ExecuteReader(SQLiteHelper.connectionString, CommandType.Text, cmdText);
                List<PossettingModel> list = new List<PossettingModel>();

                while (dataReader.Read())
                {
                    PossettingModel possetting = new PossettingModel();
                    possetting.issys = bool.Parse(dataReader["issys"].ToString());
                    possetting.xpname = dataReader["xpname"].ToString();
                    possetting.xpvalue = dataReader["xpvalue"].ToString();
                    possetting.usercode = dataReader["usercode"].ToString();
                    list.Add(possetting);
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

        #region 获取单系统设置
        /// <summary>
        /// 获取单系统设置
        /// </summary>
        /// <returns></returns>
        public PossettingModel GetPossettingByKey(string key)
        {
            string cmdText = "select issys,xpname,xpvalue,usercode from possetting where xpname=@xpname";
            SQLiteParameter[] parameters = new SQLiteParameter[1];
            parameters[0] = new SQLiteParameter("xpname", DbType.String);
            parameters[0].Value = key;
            try
            {
                SQLiteDataReader dataReader = SQLiteHelper.ExecuteReader(SQLiteHelper.connectionString, CommandType.Text, cmdText, parameters);
                List<PossettingModel> list = new List<PossettingModel>();

                while (dataReader.Read())
                {
                    PossettingModel possetting = new PossettingModel();
                    possetting.issys = bool.Parse(dataReader["issys"].ToString());
                    possetting.xpname = dataReader["xpname"].ToString();
                    possetting.xpvalue = dataReader["xpvalue"].ToString();
                    possetting.usercode = dataReader["usercode"].ToString();
                    list.Add(possetting);
                }
                dataReader.Close();
                return list.FirstOrDefault();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion
    }
}
