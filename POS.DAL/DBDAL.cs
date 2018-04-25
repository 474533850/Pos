using POS.Common.Enum;
using POS.Common.utility;
using POS.Model;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Web.Script.Serialization;

namespace POS.DAL
{
    public class DBDAL
    {
        JavaScriptSerializer js = new JavaScriptSerializer();

        #region 获取当前数据版本号
        /// <summary>
        /// 获取当前数据版本号
        /// </summary>
        /// <returns></returns>
        public string GetDBVersion()
        {
            string cmdText = "select max(versionNO) from version";
            string version = (string)SQLiteHelper.ExecuteScalar(SQLiteHelper.connectionString, CommandType.Text, cmdText);
            return version;
        }
        #endregion

        #region 检查数据库表的列是否存在
        /// <summary>
        /// 检查数据库表的列是否存在
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="columnName"></param>
        /// <returns></returns>
        private bool CheckColumnExists(String tableName, String columnName)
        {
            try
            {
                string cmdText = "select count(*) from sqlite_master where type = 'table' and name = @tableName and sql like @columnName";
                SQLiteParameter[] parameters = new SQLiteParameter[2];
                parameters[0] = new SQLiteParameter("tableName", DbType.String);
                parameters[0].Value = tableName;
                parameters[1] = new SQLiteParameter("columnName", DbType.String);
                parameters[1].Value = "%" + columnName + "%";

                bool result = (long)SQLiteHelper.ExecuteScalar(SQLiteHelper.connectionString, CommandType.Text, cmdText, parameters) > 0;
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region 检查数据库表是否存在
        /// <summary>
        /// 检查数据库表是否存在
        /// </summary>
        /// <param name="tableName"></param>
        /// <returns></returns>
        private bool CheckTableExists(String tableName)
        {
            try
            {
                string cmdText = "select count(*) from sqlite_master where type = 'table' and name = @tableName";
                SQLiteParameter[] parameters = new SQLiteParameter[1];
                parameters[0] = new SQLiteParameter("tableName", DbType.String);
                parameters[0].Value = tableName;

                bool result = (long)SQLiteHelper.ExecuteScalar(SQLiteHelper.connectionString, CommandType.Text, cmdText, parameters) > 0;
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region POS单、收款单添加上传数据状态列
        /// <summary>
        /// POS单、收款单添加上传数据状态列
        /// </summary>
        /// <param name="dbVersion"></param>
        /// <returns></returns>
        public bool UpdateDBV1(string dbVersion)
        {
            List<SqlExpressionModel> sqlStringList = new List<SqlExpressionModel>();
            string cmdText = string.Empty;
            SqlExpressionModel sqlExp = null;
            //POS单添加上传状态
            if (!CheckColumnExists("poshh", "uploadstatus"))
            {
                cmdText = "alter table poshh add uploadstatus VARCHAR (20)";
                sqlExp = new SqlExpressionModel();
                sqlExp.CmdText = cmdText;
                sqlStringList.Add(sqlExp);
            }
            //客户收款单添加上传状态
            if (!CheckColumnExists("ofhh", "uploadstatus"))
            {
                cmdText = "alter table ofhh add uploadstatus VARCHAR (20)";
                sqlExp = new SqlExpressionModel();
                sqlExp.CmdText = cmdText;
                sqlStringList.Add(sqlExp);
            }
            //会员添加上传状态
            if (!CheckColumnExists("clnt", "uploadstatus"))
            {
                cmdText = "alter table clnt add uploadstatus VARCHAR (20)";
                sqlExp = new SqlExpressionModel();
                sqlExp.CmdText = cmdText;
                sqlStringList.Add(sqlExp);
            }
            //pos单添加积分抵扣现金
            if (!CheckColumnExists("poshh", "deductiblecash"))
            {
                cmdText = "alter table poshh add deductiblecash DECIMAL";
                sqlExp = new SqlExpressionModel();
                sqlExp.CmdText = cmdText;
                sqlStringList.Add(sqlExp);
            }

            cmdText = "update version set versionNO=@versionNO";
            SQLiteParameter[] param = new SQLiteParameter[1];
            param[0] = new SQLiteParameter("versionNO", DbType.String);
            param[0].Value = dbVersion;
            sqlExp = new SqlExpressionModel();
            sqlExp.CmdText = cmdText;
            sqlExp.Parameters = param;
            sqlStringList.Add(sqlExp);

            cmdText = "update poshh set uploadstatus=@uploadstatus where xstate=@xstate";
            param = new SQLiteParameter[2];
            param[0] = new SQLiteParameter("uploadstatus", DbType.String);
            param[0].Value = (int)UploadStatus.NotUploaded;
            param[1] = new SQLiteParameter("xstate", DbType.String);
            param[1].Value = "换进";
            sqlExp = new SqlExpressionModel();
            sqlExp.CmdText = cmdText;
            sqlExp.Parameters = param;
            sqlStringList.Add(sqlExp);

            try
            {
                SQLiteHelper.ExecuteSqlTran(sqlStringList);
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region 当班记录添加上传数据状态列
        /// <summary>
        ///当班记录添加上传数据状态列
        /// </summary>
        /// <param name="dbVersion"></param>
        /// <returns></returns>
        public bool UpdateDBV2(string dbVersion)
        {
            List<SqlExpressionModel> sqlStringList = new List<SqlExpressionModel>();
            string cmdText = string.Empty;
            SqlExpressionModel sqlExp = null;
            //当班记录添加上传状态
            if (!CheckColumnExists("posban", "uploadstatus"))
            {
                cmdText = "alter table posban add uploadstatus VARCHAR (20)";
                sqlExp = new SqlExpressionModel();
                sqlExp.CmdText = cmdText;
                sqlStringList.Add(sqlExp);
            }


            cmdText = "update version set versionNO=@versionNO";
            SQLiteParameter[] param = new SQLiteParameter[1];
            param[0] = new SQLiteParameter("versionNO", DbType.String);
            param[0].Value = dbVersion;
            sqlExp = new SqlExpressionModel();
            sqlExp.CmdText = cmdText;
            sqlExp.Parameters = param;
            sqlStringList.Add(sqlExp);

            try
            {
                SQLiteHelper.ExecuteSqlTran(sqlStringList);
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region pos单添加历史记录列
        /// <summary>
        ///pos单添加历史记录列
        /// </summary>
        /// <param name="dbVersion"></param>
        /// <returns></returns>
        public bool UpdateDBV3(string dbVersion)
        {
            List<SqlExpressionModel> sqlStringList = new List<SqlExpressionModel>();
            string cmdText = string.Empty;
            SqlExpressionModel sqlExp = null;
            //当班记录添加上传状态
            if (!CheckColumnExists("poshh", "isHistory"))
            {
                cmdText = "alter table poshh add isHistory BOOLEAN";
                sqlExp = new SqlExpressionModel();
                sqlExp.CmdText = cmdText;
                sqlStringList.Add(sqlExp);
            }

            cmdText = "update poshh set isHistory=1";
            sqlExp = new SqlExpressionModel();
            sqlExp.CmdText = cmdText;
            sqlStringList.Add(sqlExp);

            cmdText = "update version set versionNO=@versionNO";
            SQLiteParameter[] param = new SQLiteParameter[1];
            param[0] = new SQLiteParameter("versionNO", DbType.String);
            param[0].Value = dbVersion;
            sqlExp = new SqlExpressionModel();
            sqlExp.CmdText = cmdText;
            sqlExp.Parameters = param;
            sqlStringList.Add(sqlExp);

            cmdText = "update poshh set uploadstatus=@uploadstatus where SID is not null";
            param = new SQLiteParameter[1];
            param[0] = new SQLiteParameter("uploadstatus", DbType.String);
            param[0].Value = (int)UploadStatus.Uploaded;
            sqlExp = new SqlExpressionModel();
            sqlExp.CmdText = cmdText;
            sqlExp.Parameters = param;
            sqlStringList.Add(sqlExp);

            try
            {
                SQLiteHelper.ExecuteSqlTran(sqlStringList);
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region pos表体添加换货状态列
        /// <summary>
        ///pos表体添加换货状态列
        /// </summary>
        /// <param name="dbVersion"></param>
        /// <returns></returns>
        public bool UpdateDBV4(string dbVersion)
        {
            List<SqlExpressionModel> sqlStringList = new List<SqlExpressionModel>();
            string cmdText = string.Empty;
            SqlExpressionModel sqlExp = null;
            //当班记录添加上传状态
            if (!CheckColumnExists("posbb", "xchg"))
            {
                cmdText = "alter table posbb add xchg VARCHAR (20)";
                sqlExp = new SqlExpressionModel();
                sqlExp.CmdText = cmdText;
                sqlStringList.Add(sqlExp);
            }


            cmdText = "update version set versionNO=@versionNO";
            SQLiteParameter[] param = new SQLiteParameter[1];
            param[0] = new SQLiteParameter("versionNO", DbType.String);
            param[0].Value = dbVersion;
            sqlExp = new SqlExpressionModel();
            sqlExp.CmdText = cmdText;
            sqlExp.Parameters = param;
            sqlStringList.Add(sqlExp);

            try
            {
                SQLiteHelper.ExecuteSqlTran(sqlStringList);
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region 添加会员日表
        /// <summary>
        ///添加会员日表
        /// </summary>
        /// <param name="dbVersion"></param>
        /// <returns></returns>
        public bool UpdateDBV5(string dbVersion)
        {
            List<SqlExpressionModel> sqlStringList = new List<SqlExpressionModel>();
            string cmdText = string.Empty;
            SqlExpressionModel sqlExp = null;

            //会员日
            if (!CheckTableExists("clntday"))
            {
                cmdText = @"CREATE TABLE clntday (
                            SID       INT,
                            xday      INT,
                            xmonth    VARCHAR(30),
                            xstart    BOOLAN,
                            xtableid  INT,
                            xsubid    INT,
                            xversion  FLOAT
                        );";
                sqlExp = new SqlExpressionModel();
                sqlExp.CmdText = cmdText;
                sqlStringList.Add(sqlExp);
            }
            else
            {
                if (!CheckColumnExists("clntday", "xmonth"))
                {
                    cmdText = "alter table clntday add xmonth VARCHAR(30)";
                    sqlExp = new SqlExpressionModel();
                    sqlExp.CmdText = cmdText;
                    sqlStringList.Add(sqlExp);
                }
            }
            if (CheckTableExists("clntprice"))
            {
                cmdText = "drop table clntprice";
                sqlExp = new SqlExpressionModel();
                sqlExp.CmdText = cmdText;
                sqlStringList.Add(sqlExp);
            }
            //会员日分类等级折扣
            if (!CheckTableExists("clntdrule"))
            {
                cmdText = @"CREATE TABLE clntdrule (
                            SID       INT,
                            goodtype  VARCHAR(40),
                            uclssprics VARCHAR(200),
                            classtype    VARCHAR(40),
                            xtimes    DECIMAL,
                            xtableid  INT,
                            xsubid    INT,
                            xversion  FLOAT
                        );";
                sqlExp = new SqlExpressionModel();
                sqlExp.CmdText = cmdText;
                sqlStringList.Add(sqlExp);
            }

            //pos单添加是否为会员日列
            if (!CheckColumnExists("poshh", "isClntDay"))
            {
                cmdText = "alter table poshh add isClntDay BOOLEAN";
                sqlExp = new SqlExpressionModel();
                sqlExp.CmdText = cmdText;
                sqlStringList.Add(sqlExp);
            }

            //pos表体添加赠送积分的倍数
            if (!CheckColumnExists("posbb", "xtimes"))
            {
                cmdText = "alter table posbb add xtimes DECIMAL";
                sqlExp = new SqlExpressionModel();
                sqlExp.CmdText = cmdText;
                sqlStringList.Add(sqlExp);
            }

            try
            {
                SQLiteHelper.ExecuteSqlTran(sqlStringList);
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        public List<DBStructModel> GetServiceDBStruct(string url)
        {
            try
            {
                string r = HttpClient.Get(url, null);
                List<DBStructModel> syncResult = js.Deserialize<List<DBStructModel>>(r);
                syncResult = syncResult.Where(p => p.Column !=null && p.Column != "").ToList();
                return syncResult;
            }
            catch (Exception ex)
            {
                //string str = GetExceptionMsg(ex, ex.ToString());
                //logger.Info(str);
                throw ex;
            }
        }
    }
}
