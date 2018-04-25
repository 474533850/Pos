using POS.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web.Script.Serialization;
using System.Threading;
using POS.Common;
using System.Configuration;
using System.Web;
using POS.Common.Enum;
using POS.Common.utility;

namespace POS.DAL.Sync
{
    public class SyncHelperDALV1 : BaseDAL
    {
        //private static string baseUrl = "http://www.xc200.com/pos/pull";
        //private static string baseUrl = "http://192.168.1.238/pos/pull";
        //private static string baseUrl_Push = "http://192.168.1.238/pos/push";
        private static Uri baseUrl_Pull = null;
        private static Uri baseUrl_Push = null;
        static JavaScriptSerializer js = new JavaScriptSerializer();
        //static ManualResetEvent wait_Sync = new ManualResetEvent(false);
        static Mutex metxt = new Mutex(false);
        static Uri baseUrl = new Uri(ConfigurationManager.AppSettings["serviceUrl"].ToString());
        HttpClient httpClient = new HttpClient();
        // static ApplicationLogger logger = new ApplicationLogger(typeof(SyncHelperDAL).Name);

        #region 获取同步数据url
        /// <summary>
        /// 获取同步数据url
        /// </summary>
        /// <param name="sid">账套号</param>
        /// <param name="mod">表名</param>
        /// <param name="xls">分部代码</param>
        /// <param name="usercode">用户代码</param>
        /// <param name="version">版本号</param>
        /// <returns></returns>
        private static string GetUrl(string sid, string mod, string xls, string usercode, object version, int id = 0, int page = 1, int pageSize = 20)
        {
            try
            {
                string pageSet = ConfigurationManager.AppSettings["pageSize"].ToString();
                int size = 0;
                if (int.TryParse(pageSet, out size))
                {
                    pageSize = size;
                }
            }
            catch (Exception)
            {
            }
            string urlFormat = string.Format("{0}?sid={1}&mod={2}&xls={3}&usercode={4}&version={5}&id={6}&page={7}&pageSize={8}", baseUrl_Pull, sid, mod, xls, usercode, version ?? "", id,page,pageSize);
            return urlFormat;
        }
        #endregion

        #region 检查网络是否能连接到服务器
        public static bool CheckConnect(out DateTime currentTime)
        {
            currentTime = DateTime.Now;
            try
            {
                // Uri baseUrl = new Uri(ConfigurationManager.AppSettings["serviceUrl"].ToString());
                Uri url = new Uri(baseUrl, @"pos/cnet");
                string result = HttpClient.Get(url, 5000).Trim();
                currentTime = ConvertStringToDateTime(result);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        #endregion

        #region 获取会员积分结余
        public static decimal Getjifen(string sid, string userCode)
        {
            try
            {
                decimal result;
                string urlStr = string.Format(@"pos/jifen?sid={0}&clntcode={1}", sid, userCode);
                Uri url = new Uri(baseUrl, urlStr);
                string r = HttpClient.Get(url, null);
                SyncResultModel<object> syncResult = js.Deserialize<SyncResultModel<object>>(r);
                if (decimal.TryParse(syncResult.data, out result)) { }
                return result;
            }
            catch (Exception ex)
            {
                //string str = GetExceptionMsg(ex, ex.ToString());
                //logger.Info(str);
                throw ex;
            }
        }
        #endregion

        #region 获取预存款余额
        public static decimal Getyck(string sid, string userCode)
        {
            try
            {
                decimal result;
                string urlStr = string.Format(@"pos/yck?sid={0}&clntcode={1}", sid, userCode);
                Uri url = new Uri(baseUrl, urlStr);
                string r = HttpClient.Get(url, null);
                SyncResultModel<object> syncResult = js.Deserialize<SyncResultModel<object>>(r);
                if (decimal.TryParse(syncResult.data, out result)) { }
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region 同步数据
        /// <summary>
        /// 同步数据
        /// </summary>
        /// <param name="sid">账套号</param>
        /// <param name="xls">分部代码</param>
        /// <param name="usercode">用户代码</param>
        /// <returns></returns>

        public static bool SyncData(string sid, string xls, string usercode, List<string> tables, int id = 0)
        {
            try
            {
                //Uri baseUrl = new Uri(ConfigurationManager.AppSettings["serviceUrl"].ToString());
                baseUrl_Pull = new Uri(baseUrl, @"pos/pull");
                baseUrl_Push = new Uri(baseUrl, @"pos/push");

                js.MaxJsonLength = 2147483644;
                object obj = new object();
                Monitor.Enter(obj);
                if (tables.Contains("ls"))
                {
                    //if (!metxt.WaitOne(TimeSpan.FromSeconds(10), false))
                    //{ }
                    metxt.WaitOne();
                    SyncDepartment(sid, usercode);
                    metxt.ReleaseMutex();
                    //wait_Sync.WaitOne();
                }
                if (tables.Contains("user"))
                {
                    metxt.WaitOne();
                    SyncUser(sid, xls, usercode);
                    metxt.ReleaseMutex();
                    //wait_Sync.WaitOne();
                }
                if (tables.Contains("cnku"))
                {
                    metxt.WaitOne();
                    SyncCnku(sid, xls, usercode);
                    metxt.ReleaseMutex();
                    //wait_Sync.WaitOne();
                }
                if (tables.Contains("ofhh"))
                {
                    metxt.WaitOne();
                    SyncOfhh(sid, xls, usercode);
                    metxt.ReleaseMutex();
                    //wait_Sync.WaitOne();
                }
                if (tables.Contains("poshh"))
                {
                    metxt.WaitOne();
                    SyncPoshh(sid, xls, usercode);
                    metxt.ReleaseMutex();
                    //wait_Sync.WaitOne();
                }
                if (tables.Contains("ku2"))
                {
                    metxt.WaitOne();
                    Syncku2(sid, xls, usercode, id);
                    metxt.ReleaseMutex();
                    //wait_Sync.WaitOne();
                }
                if (tables.Contains("goodtype1"))
                {
                    metxt.WaitOne();
                    SyncGoodtype1(sid, xls, usercode);
                    metxt.ReleaseMutex();
                    //wait_Sync.WaitOne();
                }
                if (tables.Contains("goodtype2"))
                {
                    metxt.WaitOne();
                    SyncGoodtype2(sid, xls, usercode);
                    metxt.ReleaseMutex();
                    //wait_Sync.WaitOne();
                }
                if (tables.Contains("goodtype3"))
                {
                    metxt.WaitOne();
                    SyncGoodtype3(sid, xls, usercode);
                    metxt.ReleaseMutex();
                    //wait_Sync.WaitOne();
                }
                if (tables.Contains("goodkeys"))
                {
                    metxt.WaitOne();
                    SyncGoodkeys(sid, xls, usercode);
                    metxt.ReleaseMutex();
                    //wait_Sync.WaitOne();
                }
                if (tables.Contains("good"))
                {
                    metxt.WaitOne();
                    SyncGood(sid, xls, usercode);
                    metxt.ReleaseMutex();
                    //wait_Sync.WaitOne();
                }
                if (tables.Contains("goodpric"))
                {
                    metxt.WaitOne();
                    SyncGoodpric(sid, xls, usercode);
                    metxt.ReleaseMutex();
                    //wait_Sync.WaitOne();
                }
                if (tables.Contains("goodpric2"))
                {
                    metxt.WaitOne();
                    SyncGoodpric2(sid, xls, usercode);
                    metxt.ReleaseMutex();
                    //wait_Sync.WaitOne();
                }
                if (tables.Contains("gbarcode"))
                {
                    metxt.WaitOne();
                    SyncGbarcode(sid, xls, usercode);
                    metxt.ReleaseMutex();
                    //wait_Sync.WaitOne();
                }
                if (tables.Contains("goodkind"))
                {
                    metxt.WaitOne();
                    SyncGoodkind(sid, xls, usercode);
                    metxt.ReleaseMutex();
                    //wait_Sync.WaitOne();
                }
                if (tables.Contains("clnttype"))
                {
                    metxt.WaitOne();
                    SyncClnttype(sid, xls, usercode);
                    metxt.ReleaseMutex();
                    //wait_Sync.WaitOne();
                }
                if (tables.Contains("clnt"))
                {
                    metxt.WaitOne();
                    SyncClnt(sid, xls, usercode);
                    metxt.ReleaseMutex();
                    //wait_Sync.WaitOne();
                }
                if (tables.Contains("clntclss"))
                {
                    metxt.WaitOne();
                    SyncClntclss(sid, xls, usercode);
                    metxt.ReleaseMutex();
                    //wait_Sync.WaitOne();
                }
                if (tables.Contains("jjie2"))
                {
                    metxt.WaitOne();
                    SyncJjie2(sid, xls, usercode);
                    metxt.ReleaseMutex();
                    //wait_Sync.WaitOne();
                }
                if (tables.Contains("ojie2"))
                {
                    metxt.WaitOne();
                    SyncOjie2(sid, xls, usercode);
                    metxt.ReleaseMutex();
                    //wait_Sync.WaitOne();
                }
                if (tables.Contains("sale"))
                {
                    metxt.WaitOne();
                    SyncSale(sid, xls, usercode);
                    metxt.ReleaseMutex();
                    //wait_Sync.WaitOne();
                }
                if (tables.Contains("salegood"))
                {
                    metxt.WaitOne();
                    SyncSalegood(sid, xls, usercode);
                    metxt.ReleaseMutex();
                    //wait_Sync.WaitOne();
                }
                if (tables.Contains("salegoodX"))
                {
                    metxt.WaitOne();
                    SyncSalegoodX(sid, xls, usercode);
                    metxt.ReleaseMutex();
                    //wait_Sync.WaitOne();
                }
                if (tables.Contains("salerule"))
                {
                    metxt.WaitOne();
                    SyncSalerule(sid, xls, usercode);
                    metxt.ReleaseMutex();
                    //wait_Sync.WaitOne();
                }
                if (tables.Contains("posbantype"))
                {
                    metxt.WaitOne();
                    SyncPosbantype(sid, xls, usercode);
                    metxt.ReleaseMutex();
                    //wait_Sync.WaitOne();
                }
                if (tables.Contains("posbb"))
                {
                    //  SyncPosbb(sid, xls, usercode);
                    //wait_Sync.WaitOne();
                }
                if (tables.Contains("posban"))
                {
                    metxt.WaitOne();
                    SyncPosban(sid, xls, usercode);
                    metxt.ReleaseMutex();
                    //wait_Sync.WaitOne();
                }
                if (tables.Contains("possetting"))
                {
                    metxt.WaitOne();
                    SyncPossetting(sid, xls, usercode);
                    metxt.ReleaseMutex();
                    //wait_Sync.WaitOne();
                }
                if (tables.Contains("tickoff"))
                {
                    metxt.WaitOne();
                    SyncTickoff(sid, xls, usercode);
                    metxt.ReleaseMutex();
                    //wait_Sync.WaitOne();
                }
                if (tables.Contains("tickoffmx"))
                {
                    metxt.WaitOne();
                    SyncTickoffmx(sid, xls, usercode);
                    metxt.ReleaseMutex();
                    //wait_Sync.WaitOne();
                }
                if (tables.Contains("payt"))
                {
                    metxt.WaitOne();
                    SyncPayt(sid, xls, usercode);
                    metxt.ReleaseMutex();
                    //wait_Sync.WaitOne();
                }
                if (!string.IsNullOrEmpty(xls))
                {
                    metxt.WaitOne();
                    SyncDel(sid, xls, usercode);
                    metxt.ReleaseMutex();
                    //wait_Sync.WaitOne();
                }

                Monitor.Exit(obj);
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region 同步删除本地的数据
        private static void SyncDel(string sid, string xls, string usercode)
        {
            StringBuilder cmdText = new StringBuilder();
            cmdText.AppendLine("select xpvalue from possetting where xpname=@xpname");
            SQLiteParameter[] parameters = new SQLiteParameter[1];
            parameters[0] = new SQLiteParameter("xpname", DbType.String);
            parameters[0].Value = AppConst.SyncDel_MaxVersion;
            object version = SQLiteHelper.ExecuteScalar(SQLiteHelper.connectionString, CommandType.Text, cmdText.ToString(), parameters);
            string url = GetUrl(sid, "delids", xls, usercode, version);
            //SyncResultModel<SyncDelDetailModel> syncResult = 
            try
            {
                HttpClient.GetAasync<SyncDelDetailModel>(url, null, syncResult =>
                   {
                       AsyncCallbackDel(syncResult);
                   });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private static void AsyncCallbackDel(SyncResultModel<SyncDelDetailModel> syncResult)
        {
            List<SyncDelDetailModel> datas = syncResult.datas;
            datas = (from p in datas.Where(r => AppConst.AllTableNames.Contains(r.xtbname)) select p).ToList();
            if (datas.Count > 0)
            {
                #region 同步到数据库
                //using (SQLiteConnection con = new SQLiteConnection(SQLiteHelper.connectionString))
                //{
                SQLiteConnection con = SQLiteHelper.DbConnection;
                using (SQLiteCommand cmd = new SQLiteCommand())
                {
                    con.Open();
                    SQLiteTransaction sqltran = con.BeginTransaction();
                    cmd.Connection = con;
                    cmd.Transaction = sqltran;

                    try
                    {
                        string cmdText = string.Empty;
                        SQLiteParameter[] parameters = null;
                        foreach (SyncDelDetailModel item in datas)
                        {
                            cmdText = string.Format("DELETE FROM {0} where SID=@SID", item.xtbname);
                            parameters = new SQLiteParameter[1];
                            parameters[0] = new SQLiteParameter("SID", DbType.Int32);
                            parameters[0].Value = item.xid;

                            cmd.Parameters.Clear();
                            cmd.CommandText = cmdText.ToString();
                            cmd.Parameters.AddRange(parameters);
                            cmd.ExecuteNonQuery();
                        }

                        cmdText = "select count(*) from possetting where xpname=@xpname";
                        parameters = new SQLiteParameter[1];
                        parameters[0] = new SQLiteParameter("xpname", DbType.String);
                        parameters[0].Value = AppConst.SyncDel_MaxVersion;
                        cmd.Parameters.Clear();
                        cmd.CommandText = cmdText;
                        cmd.Parameters.AddRange(parameters);
                        long result = (long)cmd.ExecuteScalar();

                        SQLiteParameter[] parameters_Add = new SQLiteParameter[4];
                        parameters_Add[0] = new SQLiteParameter("issys", DbType.Boolean);
                        parameters_Add[0].Value = false;
                        parameters_Add[1] = new SQLiteParameter("xpname", DbType.String);
                        parameters_Add[1].Value = AppConst.SyncDel_MaxVersion;
                        parameters_Add[2] = new SQLiteParameter("xpvalue", DbType.String);
                        parameters_Add[2].Value = datas.Max(r => r.xversion);
                        parameters_Add[3] = new SQLiteParameter("xversion", DbType.Double);
                        parameters_Add[3].Value = GetTimeStamp();
                        if (result == 0)
                        {
                            string cmdText_Add = @"INSERT INTO possetting (
                                               issys,
                                               xpname,
                                               xpvalue,
                                               xversion
                                           )
                                           VALUES (
                                               @issys,
                                               @xpname,
                                               @xpvalue,
                                               @xversion
                                           )";

                            cmd.CommandText = cmdText_Add;
                            cmd.Parameters.AddRange(parameters_Add);
                            cmd.ExecuteNonQuery();
                        }
                        else
                        {
                            cmdText = "update possetting set xpvalue=@xpvalue where xpname=@xpname";
                            parameters = new SQLiteParameter[2];
                            parameters[0] = new SQLiteParameter("xpvalue", DbType.String);
                            parameters[0].Value = datas.Max(r => r.xversion);
                            parameters[1] = new SQLiteParameter("xpname", DbType.String);
                            parameters[1].Value = AppConst.SyncDel_MaxVersion;
                            cmd.Parameters.Clear();
                            cmd.CommandText = cmdText.ToString();
                            cmd.Parameters.AddRange(parameters);
                            cmd.ExecuteNonQuery();
                        }
                        sqltran.Commit();
                    }
                    catch (Exception ex)
                    {
                        sqltran.Rollback();
                        throw ex;
                    }
                    finally
                    {
                        con.Close();
                        //wait_Sync.Set();
                    }
                }
                //}
                #endregion
            }
            //wait_Sync.Set();
        }
        #endregion

        #region 同步分部
        private static void SyncDepartment(string sid, string usercode)
        {
            StringBuilder cmdText = new StringBuilder();
            cmdText.AppendLine("select max(xversion) from ls");
            object version = SQLiteHelper.ExecuteScalar(SQLiteHelper.connectionString, CommandType.Text, cmdText.ToString());
            string url = GetUrl(sid, "ls", "", usercode, version);
            //   SyncResultModel<LsModel> syncResult = HttpClient.GetAasync<LsModel>(url, null);
            try
            {
                HttpClient.GetAasync<LsModel>(url, null, syncResult =>
                   {
                       AsyncCallbackDepartment(syncResult);
                   });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private static void AsyncCallbackDepartment(SyncResultModel<LsModel> syncResult)
        {
            List<LsModel> datas = syncResult.datas;
            if (datas.Count > 0)
            {
                #region 同步到数据库
                //using (SQLiteConnection con = new SQLiteConnection(SQLiteHelper.connectionString))
                //{
                SQLiteConnection con = SQLiteHelper.DbConnection;
                using (SQLiteCommand cmd = new SQLiteCommand())
                {
                    con.Open();
                    SQLiteTransaction sqltran = con.BeginTransaction();
                    cmd.Connection = con;
                    cmd.Transaction = sqltran;

                    try
                    {
                        foreach (LsModel item in datas)
                        {
                            cmd.Parameters.Clear();
                            StringBuilder cmdText = new StringBuilder();
                            cmdText.AppendLine("select count(*) from ls where SID=@SID");
                            SQLiteParameter[] parameters = new SQLiteParameter[1];
                            parameters[0] = new SQLiteParameter("SID", DbType.Int32);
                            parameters[0].Value = item.SID;
                            cmd.CommandText = cmdText.ToString();
                            cmd.Parameters.AddRange(parameters);
                            long result = (long)cmd.ExecuteScalar();

                            parameters = new SQLiteParameter[22];
                            parameters[0] = new SQLiteParameter("SID", DbType.Int32);
                            parameters[0].Value = item.SID;
                            parameters[1] = new SQLiteParameter("xlstype", DbType.String);
                            parameters[1].Value = item.xlstype;
                            parameters[2] = new SQLiteParameter("xls", DbType.String);
                            parameters[2].Value = item.xls;
                            parameters[3] = new SQLiteParameter("xlsname", DbType.String);
                            parameters[3].Value = item.xlsname;
                            parameters[4] = new SQLiteParameter("xlsp", DbType.String);
                            parameters[4].Value = item.xlsp;
                            parameters[5] = new SQLiteParameter("xlsnamep", DbType.String);
                            parameters[5].Value = item.xlsnamep;
                            parameters[6] = new SQLiteParameter("xdotype", DbType.String);
                            parameters[6].Value = item.xdotype;
                            parameters[7] = new SQLiteParameter("xstate", DbType.String);
                            parameters[7].Value = item.xstate;
                            parameters[8] = new SQLiteParameter("lsclass", DbType.String);
                            parameters[8].Value = item.lsclass;
                            parameters[9] = new SQLiteParameter("xpost1", DbType.String);
                            parameters[9].Value = item.xpost1;
                            parameters[10] = new SQLiteParameter("xpost2", DbType.String);
                            parameters[10].Value = item.xpost2;
                            parameters[11] = new SQLiteParameter("xpost3", DbType.String);
                            parameters[11].Value = item.xpost3;
                            parameters[12] = new SQLiteParameter("xaddr", DbType.String);
                            parameters[12].Value = item.xaddr;
                            parameters[13] = new SQLiteParameter("xfax", DbType.String);
                            parameters[13].Value = item.xfax;
                            parameters[14] = new SQLiteParameter("xinname", DbType.String);
                            parameters[14].Value = item.xinname;
                            parameters[15] = new SQLiteParameter("xintime", DbType.String);
                            parameters[15].Value = item.xintime;
                            parameters[16] = new SQLiteParameter("xlastlogintime", DbType.String);
                            parameters[16].Value = item.xlastlogintime;
                            parameters[17] = new SQLiteParameter("xjytime1", DbType.String);
                            parameters[17].Value = item.xjytime1;
                            parameters[18] = new SQLiteParameter("xjytime2", DbType.String);
                            parameters[18].Value = item.xjytime2;
                            parameters[19] = new SQLiteParameter("xtableid", DbType.Int32);
                            parameters[19].Value = item.xtableid;
                            parameters[20] = new SQLiteParameter("xsubid", DbType.Int32);
                            parameters[20].Value = item.xsubid;
                            parameters[21] = new SQLiteParameter("xversion", DbType.Double);
                            parameters[21].Value = item.xversion;

                            if (result == 0)
                            {
                                cmdText = new StringBuilder();
                                cmdText.AppendLine("INSERT INTO ls (");
                                cmdText.AppendLine("SID,xlstype,xls,xlsname,xlsp,xlsnamep,xdotype,");
                                cmdText.AppendLine("xstate,lsclass,xpost1,xpost2,xpost3,xaddr,xfax,xinname,");
                                cmdText.AppendLine("xintime,xlastlogintime,xjytime1,xjytime2,xtableid,xsubid,xversion)");
                                cmdText.AppendLine("VALUES(");
                                cmdText.AppendLine("@SID,@xlstype,@xls,@xlsname,@xlsp,@xlsnamep,@xdotype,");
                                cmdText.AppendLine("@xstate,@lsclass,@xpost1,@xpost2,@xpost3,@xaddr,@xfax,@xinname,");
                                cmdText.AppendLine("@xintime,@xlastlogintime,@xjytime1,@xjytime2,@xtableid,@xsubid,@xversion)");

                                cmd.Parameters.Clear();
                                cmd.CommandText = cmdText.ToString();
                                cmd.Parameters.AddRange(parameters);
                                cmd.ExecuteNonQuery();
                            }
                            else
                            {
                                string sql = @"UPDATE ls
                                               SET xlstype = @xlstype,
                                                   xls = @xls,
                                                   xlsname = @xlsname,
                                                   xlsp = @xlsp,
                                                   xlsnamep = @xlsnamep,
                                                   xdotype = @xdotype,
                                                   xstate = @xstate,
                                                   lsclass = @lsclass,
                                                   xpost1 = @xpost1,
                                                   xpost2 = @xpost2,
                                                   xpost3 = @xpost3,
                                                   xaddr = @xaddr,
                                                   xfax = @xfax,
                                                   xinname = @xinname,
                                                   xintime = @xintime,
                                                   xlastlogintime = @xlastlogintime,
                                                   xjytime1 = @xjytime1,
                                                   xjytime2 = @xjytime2,
                                                   xtableid = @xtableid,
                                                   xsubid = @xsubid,
                                                   xversion = @xversion
                                             WHERE SID = @SID ";

                                cmd.Parameters.Clear();
                                cmd.CommandText = sql;
                                cmd.Parameters.AddRange(parameters);
                                cmd.ExecuteNonQuery();
                            }
                        }

                        sqltran.Commit();
                    }
                    catch (Exception ex)
                    {
                        sqltran.Rollback();
                        throw ex;
                    }
                    finally
                    {
                        con.Close();
                        //wait_Sync.Set();
                    }
                }
                //}
                #endregion
            }
            //wait_Sync.Set();
        }
        #endregion

        #region 同步用户
        public static void SyncUser(string sid, string xls, string usercode)
        {
            StringBuilder cmdText = new StringBuilder();
            cmdText.AppendLine("select max(xversion) from user");
            object version = SQLiteHelper.ExecuteScalar(SQLiteHelper.connectionString, CommandType.Text, cmdText.ToString());
            string url = GetUrl(sid, "user", xls, usercode, version);
            //SyncResultModel<UserModel> syncResult = HttpClient.GetAasync<UserModel>(url, null);
            //AsyncCallbackUser(syncResult);
            try
            {
                HttpClient.GetAasync<UserModel>(url, null, syncResult =>
                {
                    AsyncCallbackUser(syncResult);
                });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private static void AsyncCallbackUser(SyncResultModel<UserModel> syncResult)
        {
            List<UserModel> datas = syncResult.datas;
            if (datas.Count > 0)
            {
                #region 同步到数据库
                //using (SQLiteConnection con = new SQLiteConnection(SQLiteHelper.connectionString))
                //{
                SQLiteConnection con = SQLiteHelper.DbConnection;
                using (SQLiteCommand cmd = new SQLiteCommand())
                {
                    con.Open();
                    SQLiteTransaction sqltran = con.BeginTransaction();
                    cmd.Connection = con;
                    cmd.Transaction = sqltran;

                    try
                    {
                        foreach (UserModel item in datas)
                        {
                            cmd.Parameters.Clear();
                            string cmdText = "select count(*) from user where SID=@SID";
                            SQLiteParameter[] parameters = new SQLiteParameter[1];
                            parameters[0] = new SQLiteParameter("SID", DbType.Int32);
                            parameters[0].Value = item.SID;
                            cmd.CommandText = cmdText;
                            cmd.Parameters.AddRange(parameters);
                            long result = (long)cmd.ExecuteScalar();

                            parameters = new SQLiteParameter[9];
                            parameters[0] = new SQLiteParameter("SID", DbType.Int32);
                            parameters[0].Value = item.SID;
                            parameters[1] = new SQLiteParameter("usercode", DbType.String);
                            parameters[1].Value = item.usercode;
                            parameters[2] = new SQLiteParameter("username", DbType.String);
                            parameters[2].Value = item.username;
                            parameters[3] = new SQLiteParameter("xtableid", DbType.Int32);
                            parameters[3].Value = item.xtableid;
                            parameters[4] = new SQLiteParameter("xsubid", DbType.Int32);
                            parameters[4].Value = item.xsubid;
                            parameters[5] = new SQLiteParameter("password", DbType.String);
                            parameters[5].Value = item.password;
                            parameters[6] = new SQLiteParameter("xversion", DbType.Double);
                            parameters[6].Value = item.xversion;
                            parameters[7] = new SQLiteParameter("xls", DbType.String);
                            parameters[7].Value = item.xls;
                            parameters[8] = new SQLiteParameter("xlsname", DbType.String);
                            parameters[8].Value = item.xlsname;

                            if (result == 0)
                            {
                                cmdText = @"INSERT INTO user (
                                         SID,
                                         usercode,
                                         username,
                                         xtableid,
                                         xsubid,
                                         password,
                                         xversion,
                                         xls,
                                         xlsname
                                     )
                                     VALUES (
                                         @SID,
                                         @usercode,
                                         @username,
                                         @xtableid,
                                         @xsubid,
                                         @password,
                                         @xversion,
                                         @xls,
                                         @xlsname
                                        )";

                                cmd.Parameters.Clear();
                                cmd.CommandText = cmdText.ToString();
                                cmd.Parameters.AddRange(parameters);
                                cmd.ExecuteNonQuery();
                            }
                            else
                            {
                                cmdText = @"UPDATE user
                                               SET usercode = @usercode,
                                                   username = @username,
                                                   xtableid = @xtableid,
                                                   xsubid = @xsubid,
                                                   password = @password,
                                                   xversion = @xversion,
                                                   xls=@xls,
                                                   xlsname=@xlsname
                                             WHERE SID = @SID";

                                cmd.Parameters.Clear();
                                cmd.CommandText = cmdText;
                                cmd.Parameters.AddRange(parameters);
                                cmd.ExecuteNonQuery();
                            }
                        }

                        sqltran.Commit();
                    }
                    catch (Exception ex)
                    {
                        sqltran.Rollback();
                        throw ex;
                    }
                    finally
                    {
                        con.Close();
                        //wait_Sync.Set();
                    }
                }
                // }
                #endregion
            }
            //wait_Sync.Set();
        }
        #endregion

        #region 同步仓库
        public static void SyncCnku(string sid, string xls, string usercode)
        {
            StringBuilder cmdText = new StringBuilder();
            cmdText.AppendLine("select max(xversion) from cnku where xls=@xls");
            SQLiteParameter[] parameters = new SQLiteParameter[1];
            parameters[0] = new SQLiteParameter("xls", DbType.String);
            parameters[0].Value = xls;
            object version = SQLiteHelper.ExecuteScalar(SQLiteHelper.connectionString, CommandType.Text, cmdText.ToString(), parameters);
            string url = GetUrl(sid, "cnku", xls, usercode, version);
            //SyncResultModel<CnkuModel> syncResult = HttpClient.GetAasync<CnkuModel>(url, null);
            //AsyncCallbackCnku(syncResult);
            try
            {
                HttpClient.GetAasync<CnkuModel>(url, null, syncResult =>
                   {
                       AsyncCallbackCnku(syncResult);
                   });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private static void AsyncCallbackCnku(SyncResultModel<CnkuModel> syncResult)
        {
            List<CnkuModel> datas = syncResult.datas;
            if (datas.Count > 0)
            {
                #region 同步到数据库
                //using (SQLiteConnection con = new SQLiteConnection(SQLiteHelper.connectionString))
                //{
                SQLiteConnection con = SQLiteHelper.DbConnection;
                using (SQLiteCommand cmd = new SQLiteCommand())
                {
                    con.Open();
                    SQLiteTransaction sqltran = con.BeginTransaction();
                    cmd.Connection = con;
                    cmd.Transaction = sqltran;

                    try
                    {
                        foreach (CnkuModel item in datas)
                        {
                            cmd.Parameters.Clear();
                            string cmdText = "select count(*) from cnku where SID=@SID";
                            SQLiteParameter[] parameters = new SQLiteParameter[1];
                            parameters[0] = new SQLiteParameter("SID", DbType.Int32);
                            parameters[0].Value = item.SID;
                            cmd.CommandText = cmdText;
                            cmd.Parameters.AddRange(parameters);
                            long result = (long)cmd.ExecuteScalar();

                            parameters = new SQLiteParameter[9];
                            parameters[0] = new SQLiteParameter("SID", DbType.Int32);
                            parameters[0].Value = item.SID;
                            parameters[1] = new SQLiteParameter("cnkutype", DbType.String);
                            parameters[1].Value = item.cnkutype;
                            parameters[2] = new SQLiteParameter("cnkucode", DbType.String);
                            parameters[2].Value = item.cnkucode;
                            parameters[3] = new SQLiteParameter("cnkuname", DbType.String);
                            parameters[3].Value = item.cnkuname;
                            parameters[4] = new SQLiteParameter("xls", DbType.String);
                            parameters[4].Value = item.xls;
                            parameters[5] = new SQLiteParameter("xlsname", DbType.String);
                            parameters[5].Value = item.xlsname;
                            parameters[6] = new SQLiteParameter("xtableid", DbType.Int32);
                            parameters[6].Value = item.xtableid;
                            parameters[7] = new SQLiteParameter("xsubid", DbType.Int32);
                            parameters[7].Value = item.xsubid;
                            parameters[8] = new SQLiteParameter("xversion", DbType.Double);
                            parameters[8].Value = item.xversion;

                            if (result == 0)
                            {
                                cmdText = @"INSERT INTO cnku (
                                             SID,
                                             cnkutype,
                                             cnkucode,
                                             cnkuname,
                                             xls,
                                             xlsname,
                                             xtableid,
                                             xsubid,
                                             xversion
                                         )
                                         VALUES (
                                             @SID,
                                             @cnkutype,
                                             @cnkucode,
                                             @cnkuname,
                                             @xls,
                                             @xlsname,
                                             @xtableid,
                                             @xsubid,
                                             @xversion
                                         )";

                                cmd.Parameters.Clear();
                                cmd.CommandText = cmdText.ToString();
                                cmd.Parameters.AddRange(parameters);
                                cmd.ExecuteNonQuery();
                            }
                            else
                            {
                                cmdText = @"UPDATE cnku
                                           SET cnkutype = @cnkutype,
                                               cnkucode = @cnkucode,
                                               cnkuname = @cnkuname,
                                               xls = @xls,
                                               xlsname = @xlsname,
                                               xtableid = @xtableid,
                                               xsubid = @xsubid,
                                               xversion = @xversion
                                         WHERE SID = @SID";

                                cmd.Parameters.Clear();
                                cmd.CommandText = cmdText;
                                cmd.Parameters.AddRange(parameters);
                                cmd.ExecuteNonQuery();
                            }
                        }

                        sqltran.Commit();
                    }
                    catch (Exception ex)
                    {
                        sqltran.Rollback();
                        throw ex;
                    }
                    finally
                    {
                        con.Close();
                        //wait_Sync.Set();
                    }
                }
                //}
                #endregion
            }
            //wait_Sync.Set();
        }
        #endregion

        #region 收款单
        private static void SyncOfhh(string sid, string xls, string usercode)
        {
            //上传
            StringBuilder cmdText = new StringBuilder();
            cmdText.AppendLine("select * from ofhh where SID is null");

            SQLiteDataReader dataReader = SQLiteHelper.ExecuteReader(SQLiteHelper.connectionString, CommandType.Text, cmdText.ToString());

            List<OfhhModel> ofhhs = new List<OfhhModel>();
            while (dataReader.Read())
            {
                OfhhModel ofhh = new OfhhModel();
                ofhh.SID = (dataReader["SID"] == null || string.IsNullOrEmpty(dataReader["SID"].ToString())) ? (int?)null : int.Parse(dataReader["SID"].ToString().Trim());
                ofhh.ID = new Guid(dataReader["ID"].ToString().Trim());
                ofhh.xdate = DateTime.Parse(dataReader["xdate"].ToString().Trim());
                ofhh.clntcode = dataReader["clntcode"].ToString().Trim();
                ofhh.clntname = dataReader["clntname"].ToString().Trim();
                ofhh.xnote = dataReader["xnote"].ToString().Trim();
                ofhh.xinname = dataReader["xinname"].ToString().Trim();
                ofhh.xintime = dataReader["xintime"].ToString().Trim();
                ofhh.xversion = double.Parse(dataReader["xversion"].ToString());
                ofhhs.Add(ofhh);
            }
            dataReader.Close();

            foreach (var item in ofhhs)
            {
                SQLiteParameter[] parameters = new SQLiteParameter[1];
                parameters[0] = new SQLiteParameter("ID", DbType.String);
                parameters[0].Value = item.ID;

                #region 表体
                cmdText = new StringBuilder();
                cmdText.AppendLine("select * from ofbb where XID = @ID");

                SQLiteDataReader dataReader_detail = SQLiteHelper.ExecuteReader(SQLiteHelper.connectionString, CommandType.Text, cmdText.ToString(), parameters);

                List<OfbbModel> ofbbs = new List<OfbbModel>();
                while (dataReader_detail.Read())
                {
                    OfbbModel ofbb = new OfbbModel();
                    ofbb.SID = (dataReader_detail["SID"] == null || string.IsNullOrEmpty(dataReader_detail["SID"].ToString())) ? (int?)null : int.Parse(dataReader_detail["SID"].ToString().Trim());
                    ofbb.ID = new Guid(dataReader_detail["ID"].ToString().Trim());
                    ofbb.xnoteb = dataReader_detail["xnoteb"].ToString().Trim();
                    ofbb.xztype = dataReader_detail["xztype"].ToString().Trim();
                    ofbb.xzstate = dataReader_detail["xzstate"].ToString().Trim();
                    ofbb.transno = dataReader_detail["transno"].ToString().Trim();
                    ofbb.xfee = decimal.Parse(dataReader_detail["xfee"].ToString().Trim());
                    ofbb.xsubsidy = decimal.Parse(dataReader_detail["xsubsidy"].ToString().Trim());
                    ofbbs.Add(ofbb);
                }
                dataReader_detail.Close();
                item.ofbbs = ofbbs;
                #endregion
            }

            if (ofhhs.Count > 0)
            {
                var query = from p in ofhhs
                            select new
                            {
                                ID = p.ID,
                                SID = p.SID,
                                xdate = p.xdate.ToString("yyyy-MM-dd"),
                                clntcode = p.clntcode,
                                clntname = p.clntname,
                                xnote = p.xnote,
                                xinname = p.xinname,
                                xintime = p.xintime,
                                xversion = p.xversion,
                                ofbbs = from d in p.ofbbs
                                        select new
                                        {
                                            ID = d.ID,
                                            SID = d.SID,
                                            xnoteb = d.xnoteb,
                                            xztype = d.xztype,
                                            xzstate = d.xzstate,
                                            xfee = d.xfee,
                                            xversion = p.xversion,
                                        }
                            };

                string json = js.Serialize(query);
                string postData = "sid=" + sid;
                postData += "&mod=" + "ofhh";
                postData += "&xls=" + xls;
                postData += "&usercode=" + usercode;
                postData += "&data=" + HttpUtility.UrlEncode(json, Encoding.UTF8);
                try
                {
                    SyncResultModel<OfhhModel> syncResult = HttpClient.PostAasync<OfhhModel>(baseUrl_Push.ToString(), null, postData);
                    AsyncCallbackOfhh_UP(syncResult);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }
        private static void AsyncCallbackOfhh_UP(SyncResultModel<OfhhModel> syncResult)
        {
            List<OfhhModel> datas = syncResult.datas;
            if (datas.Count > 0)
            {
                #region 同步到数据库
                //using (SQLiteConnection con = new SQLiteConnection(SQLiteHelper.connectionString))
                //{
                SQLiteConnection con = SQLiteHelper.DbConnection;
                using (SQLiteCommand cmd = new SQLiteCommand())
                {
                    con.Open();
                    SQLiteTransaction sqltran = con.BeginTransaction();
                    cmd.Connection = con;
                    cmd.Transaction = sqltran;

                    try
                    {
                        SQLiteParameter[] parameters = null;
                        foreach (OfhhModel item in datas)
                        {
                            parameters = new SQLiteParameter[2];
                            parameters[0] = new SQLiteParameter("SID", DbType.Int32);
                            parameters[0].Value = item.SID;
                            parameters[1] = new SQLiteParameter("ID", DbType.String);
                            parameters[1].Value = item.ID;

                            string sql = @"UPDATE ofhh
                                               SET SID = @SID
                                             WHERE ID = @ID";

                            cmd.Parameters.Clear();
                            cmd.CommandText = sql;
                            cmd.Parameters.AddRange(parameters);
                            cmd.ExecuteNonQuery();

                            if (item.ofbbs != null)
                            {
                                foreach (OfbbModel p in item.ofbbs)
                                {
                                    sql = @"UPDATE ofbb
                                               SET SID = @SID
                                             WHERE XID = @ID";

                                    cmd.Parameters.Clear();
                                    cmd.CommandText = sql;
                                    cmd.Parameters.AddRange(parameters);
                                    cmd.ExecuteNonQuery();
                                }
                            }

                        }
                        sqltran.Commit();
                    }
                    catch (Exception ex)
                    {
                        sqltran.Rollback();
                        throw ex;
                    }
                    finally
                    {
                        con.Close();
                        //wait_Sync.Set();
                    }
                }
                //}
                #endregion
            }
            //wait_Sync.Set();
        }
        #endregion

        #region 当前进销存帐
        private static void Syncku2(string sid, string xls, string usercode, int id = 0)
        {
            StringBuilder cmdText = new StringBuilder();
            //cmdText.AppendFormat("select max(xversion) from ku2");
            cmdText.AppendFormat("select xversion from(select xversion from ku2 order by xversion desc limit 0, 5) order by xversion asc limit 0, 1");
            object version = SQLiteHelper.ExecuteScalar(SQLiteHelper.connectionString, CommandType.Text, cmdText.ToString());
            string url = GetUrl(sid, "ku2", xls, usercode, version, id);
            //SyncResultModel<Ku2Model> syncResult = HttpClient.GetAasync<Ku2Model>(url, null);
            //AsyncCallbackKu2(syncResult);
            try
            {
                HttpClient.GetAasync<Ku2Model>(url, null, syncResult =>
                   {
                       AsyncCallbackKu2(syncResult);
                   });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private static void AsyncCallbackKu2(SyncResultModel<Ku2Model> syncResult)
        {
            List<Ku2Model> datas = syncResult.datas;
            if (datas.Count > 0)
            {
                #region 同步到数据库
                //using (SQLiteConnection con = new SQLiteConnection(SQLiteHelper.connectionString))
                //{
                SQLiteConnection con = SQLiteHelper.DbConnection;
                using (SQLiteCommand cmd = new SQLiteCommand())
                {
                    con.Open();
                    SQLiteTransaction sqltran = con.BeginTransaction();
                    cmd.Connection = con;
                    cmd.Transaction = sqltran;

                    try
                    {
                        foreach (Ku2Model item in datas)
                        {
                            cmd.Parameters.Clear();
                            string cmdText = "select count(*) from ku2 where SID=@SID";
                            SQLiteParameter[] parameters = new SQLiteParameter[1];
                            parameters[0] = new SQLiteParameter("SID", DbType.Int32);
                            parameters[0].Value = item.SID;
                            cmd.CommandText = cmdText.ToString();
                            cmd.Parameters.AddRange(parameters);
                            long result = (long)cmd.ExecuteScalar();

                            parameters = new SQLiteParameter[36];
                            parameters[0] = new SQLiteParameter("SID", DbType.Int32);
                            parameters[0].Value = item.SID;
                            parameters[1] = new SQLiteParameter("xls", DbType.String);
                            parameters[1].Value = item.xls;
                            parameters[2] = new SQLiteParameter("xlsname", DbType.String);
                            parameters[2].Value = item.xlsname;
                            parameters[3] = new SQLiteParameter("cnkucode", DbType.String);
                            parameters[3].Value = item.cnkucode;
                            parameters[4] = new SQLiteParameter("cnkuname", DbType.String);
                            parameters[4].Value = item.cnkuname;
                            parameters[5] = new SQLiteParameter("goodcode", DbType.String);
                            parameters[5].Value = item.goodcode;
                            parameters[6] = new SQLiteParameter("goodname", DbType.String);
                            parameters[6].Value = item.goodname;
                            parameters[7] = new SQLiteParameter("goodunit", DbType.String);
                            parameters[7].Value = item.goodunit;
                            parameters[8] = new SQLiteParameter("goodkind1", DbType.String);
                            parameters[8].Value = item.goodkind1;
                            parameters[9] = new SQLiteParameter("goodkind2", DbType.String);
                            parameters[9].Value = item.goodkind2;
                            parameters[10] = new SQLiteParameter("goodkind3", DbType.String);
                            parameters[10].Value = item.goodkind3;
                            parameters[11] = new SQLiteParameter("goodkind4", DbType.String);
                            parameters[11].Value = item.goodkind4;
                            parameters[12] = new SQLiteParameter("goodkind5", DbType.String);
                            parameters[12].Value = item.goodkind5;
                            parameters[13] = new SQLiteParameter("goodkind6", DbType.String);
                            parameters[13].Value = item.goodkind6;
                            parameters[14] = new SQLiteParameter("goodkind7", DbType.String);
                            parameters[14].Value = item.goodkind7;
                            parameters[15] = new SQLiteParameter("goodkind8", DbType.String);
                            parameters[15].Value = item.goodkind8;
                            parameters[16] = new SQLiteParameter("goodkind9", DbType.String);
                            parameters[16].Value = item.goodkind9;
                            parameters[17] = new SQLiteParameter("goodkind10", DbType.String);
                            parameters[17].Value = item.goodkind10;
                            parameters[18] = new SQLiteParameter("xpricqc", DbType.Decimal);
                            parameters[18].Value = item.xpricqc;
                            parameters[19] = new SQLiteParameter("xquatqc", DbType.Decimal);
                            parameters[19].Value = item.xquatqc;
                            parameters[20] = new SQLiteParameter("xallpqc", DbType.Decimal);
                            parameters[20].Value = item.xallpqc;
                            parameters[21] = new SQLiteParameter("xpricin", DbType.Decimal);
                            parameters[21].Value = item.xpricin;
                            parameters[22] = new SQLiteParameter("xquatin", DbType.Decimal);
                            parameters[22].Value = item.xquatin;
                            parameters[23] = new SQLiteParameter("xallpin", DbType.Decimal);
                            parameters[23].Value = item.xallpin;
                            parameters[24] = new SQLiteParameter("xpricot", DbType.Decimal);
                            parameters[24].Value = item.xpricot;
                            parameters[25] = new SQLiteParameter("xquatot", DbType.Decimal);
                            parameters[25].Value = item.xquatot;
                            parameters[26] = new SQLiteParameter("xallpot", DbType.Decimal);
                            parameters[26].Value = item.xallpot;
                            parameters[27] = new SQLiteParameter("xchenot", DbType.Decimal);
                            parameters[27].Value = item.xchenot;
                            parameters[28] = new SQLiteParameter("xlirnot", DbType.Decimal);
                            parameters[28].Value = item.xlirnot;
                            parameters[29] = new SQLiteParameter("xpricku", DbType.Decimal);
                            parameters[29].Value = item.xpricku;
                            parameters[30] = new SQLiteParameter("xquatku", DbType.Decimal);
                            parameters[30].Value = item.xquatku;
                            parameters[31] = new SQLiteParameter("xallpku", DbType.Decimal);
                            parameters[31].Value = item.xallpku;
                            parameters[32] = new SQLiteParameter("xlastime", DbType.String);
                            parameters[32].Value = item.xlastime;
                            parameters[33] = new SQLiteParameter("xtableid", DbType.Int32);
                            parameters[33].Value = item.xtableid;
                            parameters[34] = new SQLiteParameter("xsubid", DbType.Int32);
                            parameters[34].Value = item.xsubid;
                            parameters[35] = new SQLiteParameter("xversion", DbType.Double);
                            parameters[35].Value = item.xversion;

                            if (result == 0)
                            {
                                cmdText = @"INSERT INTO ku2 (
                                            SID,
                                            xls,
                                            xlsname,
                                            cnkucode,
                                            cnkuname,
                                            goodcode,
                                            goodname,
                                            goodunit,
                                            goodkind1,
                                            goodkind2,
                                            goodkind3,
                                            goodkind4,
                                            goodkind5,
                                            goodkind6,
                                            goodkind7,
                                            goodkind8,
                                            goodkind9,
                                            goodkind10,
                                            xpricqc,
                                            xquatqc,
                                            xallpqc,
                                            xpricin,
                                            xquatin,
                                            xallpin,
                                            xpricot,
                                            xquatot,
                                            xallpot,
                                            xchenot,
                                            xlirnot,
                                            xpricku,
                                            xquatku,
                                            xallpku,
                                            xlastime,
                                            xtableid,
                                            xsubid,
                                            xversion
                                        )
                                        VALUES (
                                            @SID,
                                            @xls,
                                            @xlsname,
                                            @cnkucode,
                                            @cnkuname,
                                            @goodcode,
                                            @goodname,
                                            @goodunit,
                                            @goodkind1,
                                            @goodkind2,
                                            @goodkind3,
                                            @goodkind4,
                                            @goodkind5,
                                            @goodkind6,
                                            @goodkind7,
                                            @goodkind8,
                                            @goodkind9,
                                            @goodkind10,
                                            @xpricqc,
                                            @xquatqc,
                                            @xallpqc,
                                            @xpricin,
                                            @xquatin,
                                            @xallpin,
                                            @xpricot,
                                            @xquatot,
                                            @xallpot,
                                            @xchenot,
                                            @xlirnot,
                                            @xpricku,
                                            @xquatku,
                                            @xallpku,
                                            @xlastime,
                                            @xtableid,
                                            @xsubid,
                                            @xversion
                                        )";

                                cmd.Parameters.Clear();
                                cmd.CommandText = cmdText.ToString();
                                cmd.Parameters.AddRange(parameters);
                                cmd.ExecuteNonQuery();
                            }
                            else
                            {
                                cmdText = @"UPDATE ku2
                                               SET xls = @xls,
                                                   xlsname = @xlsname,
                                                   cnkucode = @cnkucode,
                                                   cnkuname = @cnkuname,
                                                   goodcode = @goodcode,
                                                   goodname = @goodname,
                                                   goodunit = @goodunit,
                                                   goodkind1 = @goodkind1,
                                                   goodkind2 = @goodkind2,
                                                   goodkind3 = @goodkind3,
                                                   goodkind4 = @goodkind4,
                                                   goodkind5 = @goodkind5,
                                                   goodkind6 = @goodkind6,
                                                   goodkind7 = @goodkind7,
                                                   goodkind8 = @goodkind8,
                                                   goodkind9 = @goodkind9,
                                                   goodkind10 = @goodkind10,
                                                   xpricqc = @xpricqc,
                                                   xquatqc = @xquatqc,
                                                   xallpqc = @xallpqc,
                                                   xpricin = @xpricin,
                                                   xquatin = @xquatin,
                                                   xallpin = @xallpin,
                                                   xpricot = @xpricot,
                                                   xquatot = @xquatot,
                                                   xallpot = @xallpot,
                                                   xchenot = @xchenot,
                                                   xlirnot = @xlirnot,
                                                   xpricku = @xpricku,
                                                   xquatku = @xquatku,
                                                   xallpku = @xallpku,
                                                   xlastime = @xlastime,
                                                   xtableid = @xtableid,
                                                   xsubid = @xsubid,
                                                   xversion = @xversion
                                             WHERE SID = @SID ";

                                cmd.Parameters.Clear();
                                cmd.CommandText = cmdText;
                                cmd.Parameters.AddRange(parameters);
                                cmd.ExecuteNonQuery();
                            }
                        }

                        sqltran.Commit();
                    }
                    catch (Exception ex)
                    {
                        sqltran.Rollback();
                        throw ex;
                    }
                    finally
                    {
                        con.Close();
                        //wait_Sync.Set();
                    }
                }
                // }
                #endregion
            }
            //wait_Sync.Set();
        }
        #endregion

        #region 货品大类
        public static void SyncGoodtype1(string sid, string xls, string usercode)
        {
            StringBuilder cmdText = new StringBuilder();
            cmdText.AppendLine("select max(xversion) from goodtype1");
            object version = SQLiteHelper.ExecuteScalar(SQLiteHelper.connectionString, CommandType.Text, cmdText.ToString());
            string url = GetUrl(sid, "goodtype1", xls, usercode, version);
            //SyncResultModel<Goodtype1Model> syncResult = HttpClient.GetAasync<Goodtype1Model>(url, null);
            //AsyncCallbackGoodtype1(syncResult);
            try
            {
                HttpClient.GetAasync<Goodtype1Model>(url, null, syncResult =>
                  {
                      AsyncCallbackGoodtype1(syncResult);
                  });
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private static void AsyncCallbackGoodtype1(SyncResultModel<Goodtype1Model> syncResult)
        {
            List<Goodtype1Model> datas = syncResult.datas;
            if (datas.Count > 0)
            {
                #region 同步到数据库
                //using (SQLiteConnection con = new SQLiteConnection(SQLiteHelper.connectionString))
                //{
                SQLiteConnection con = SQLiteHelper.DbConnection;
                using (SQLiteCommand cmd = new SQLiteCommand())
                {
                    con.Open();
                    SQLiteTransaction sqltran = con.BeginTransaction();
                    cmd.Connection = con;
                    cmd.Transaction = sqltran;

                    try
                    {
                        foreach (Goodtype1Model item in datas)
                        {
                            cmd.Parameters.Clear();
                            string cmdText = "select count(*) from goodtype1 where SID=@SID";
                            SQLiteParameter[] parameters = new SQLiteParameter[1];
                            parameters[0] = new SQLiteParameter("SID", DbType.Int32);
                            parameters[0].Value = item.SID;
                            cmd.CommandText = cmdText;
                            cmd.Parameters.AddRange(parameters);
                            long result = (long)cmd.ExecuteScalar();

                            parameters = new SQLiteParameter[6];
                            parameters[0] = new SQLiteParameter("SID", DbType.Int32);
                            parameters[0].Value = item.SID;
                            parameters[1] = new SQLiteParameter("goodtype1", DbType.String);
                            parameters[1].Value = item.goodtype1;
                            parameters[2] = new SQLiteParameter("uclssprics", DbType.String);
                            parameters[2].Value = item.uclssprics;
                            parameters[3] = new SQLiteParameter("xtableid", DbType.Int32);
                            parameters[3].Value = item.xtableid;
                            parameters[4] = new SQLiteParameter("xsubid", DbType.Int32);
                            parameters[4].Value = item.xsubid;
                            parameters[5] = new SQLiteParameter("xversion", DbType.Double);
                            parameters[5].Value = item.xversion;

                            if (result == 0)
                            {
                                cmdText = @"INSERT INTO goodtype1 (
                                          SID,
                                          goodtype1,
                                          uclssprics,
                                          xtableid,
                                          xsubid,
                                          xversion
                                      )
                                      VALUES (
                                          @SID,
                                          @goodtype1,
                                          @uclssprics,
                                          @xtableid,
                                          @xsubid,
                                          @xversion
                                      )";

                                cmd.Parameters.Clear();
                                cmd.CommandText = cmdText.ToString();
                                cmd.Parameters.AddRange(parameters);
                                cmd.ExecuteNonQuery();
                            }
                            else
                            {
                                cmdText = @"UPDATE goodtype1
                                               SET goodtype1 = @goodtype1,
                                                   uclssprics = @uclssprics,
                                                   xtableid = @xtableid,
                                                   xsubid = @xsubid,
                                                   xversion = @xversion
                                             WHERE SID = @SID";

                                cmd.Parameters.Clear();
                                cmd.CommandText = cmdText;
                                cmd.Parameters.AddRange(parameters);
                                cmd.ExecuteNonQuery();
                            }
                        }

                        sqltran.Commit();
                    }
                    catch (Exception ex)
                    {
                        sqltran.Rollback();
                        throw ex;
                    }
                    finally
                    {
                        //wait_Sync.Set();
                    }
                }
                // }
                #endregion
            }
            //wait_Sync.Set();
        }
        #endregion

        #region 货品中类
        public static void SyncGoodtype2(string sid, string xls, string usercode)
        {
            StringBuilder cmdText = new StringBuilder();
            cmdText.AppendLine("select max(xversion) from goodtype2");
            object version = SQLiteHelper.ExecuteScalar(SQLiteHelper.connectionString, CommandType.Text, cmdText.ToString());
            string url = GetUrl(sid, "goodtype2", xls, usercode, version);
            //SyncResultModel<Goodtype2Model> syncResult = HttpClient.GetAasync<Goodtype2Model>(url, null);
            //AsyncCallbackGoodtype2(syncResult);
            try
            {
                HttpClient.GetAasync<Goodtype2Model>(url, null, syncResult =>
                    {
                        AsyncCallbackGoodtype2(syncResult);
                    });
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        private static void AsyncCallbackGoodtype2(SyncResultModel<Goodtype2Model> syncResult)
        {
            List<Goodtype2Model> datas = syncResult.datas;
            if (datas.Count > 0)
            {
                #region 同步到数据库
                //using (SQLiteConnection con = new SQLiteConnection(SQLiteHelper.connectionString))
                //{
                SQLiteConnection con = SQLiteHelper.DbConnection;
                using (SQLiteCommand cmd = new SQLiteCommand())
                {
                    con.Open();
                    SQLiteTransaction sqltran = con.BeginTransaction();
                    cmd.Connection = con;
                    cmd.Transaction = sqltran;

                    try
                    {
                        foreach (Goodtype2Model item in datas)
                        {
                            cmd.Parameters.Clear();
                            string cmdText = "select count(*) from goodtype2 where SID=@SID";
                            SQLiteParameter[] parameters = new SQLiteParameter[1];
                            parameters[0] = new SQLiteParameter("SID", DbType.Int32);
                            parameters[0].Value = item.SID;
                            cmd.CommandText = cmdText;
                            cmd.Parameters.AddRange(parameters);
                            long result = (long)cmd.ExecuteScalar();

                            parameters = new SQLiteParameter[7];
                            parameters[0] = new SQLiteParameter("SID", DbType.Int32);
                            parameters[0].Value = item.SID;
                            parameters[1] = new SQLiteParameter("goodtype1", DbType.String);
                            parameters[1].Value = item.goodtype1;
                            parameters[2] = new SQLiteParameter("goodtype2", DbType.String);
                            parameters[2].Value = item.goodtype2;
                            parameters[3] = new SQLiteParameter("uclssprics", DbType.String);
                            parameters[3].Value = item.uclssprics;
                            parameters[4] = new SQLiteParameter("xtableid", DbType.Int32);
                            parameters[4].Value = item.xtableid;
                            parameters[5] = new SQLiteParameter("xsubid", DbType.Int32);
                            parameters[5].Value = item.xsubid;
                            parameters[6] = new SQLiteParameter("xversion", DbType.Double);
                            parameters[6].Value = item.xversion;

                            if (result == 0)
                            {
                                cmdText = @"INSERT INTO goodtype2 (
                                          SID,
                                          goodtype1,
                                          goodtype2,
                                          uclssprics,
                                          xtableid,
                                          xsubid,
                                          xversion
                                      )
                                      VALUES (
                                          @SID,
                                          @goodtype1,
                                          @goodtype2,
                                          @uclssprics,
                                          @xtableid,
                                          @xsubid,
                                          @xversion
                                      )";

                                cmd.Parameters.Clear();
                                cmd.CommandText = cmdText.ToString();
                                cmd.Parameters.AddRange(parameters);
                                cmd.ExecuteNonQuery();
                            }
                            else
                            {
                                cmdText = @"UPDATE goodtype2
                                               SET goodtype1 = @goodtype1,
                                                   goodtype2 = @goodtype2,
                                                   uclssprics = @uclssprics,
                                                   xtableid = @xtableid,
                                                   xsubid = @xsubid,
                                                   xversion = @xversion
                                             WHERE SID = @SID";

                                cmd.Parameters.Clear();
                                cmd.CommandText = cmdText;
                                cmd.Parameters.AddRange(parameters);
                                cmd.ExecuteNonQuery();
                            }
                        }

                        sqltran.Commit();
                    }
                    catch (Exception ex)
                    {
                        sqltran.Rollback();
                        throw ex;
                    }
                    finally
                    {
                        //wait_Sync.Set();
                    }
                }
                // }
                #endregion
            }
            //wait_Sync.Set();
        }
        #endregion

        #region 货品小类
        public static void SyncGoodtype3(string sid, string xls, string usercode)
        {
            StringBuilder cmdText = new StringBuilder();
            cmdText.AppendLine("select max(xversion) from goodtype3");
            object version = SQLiteHelper.ExecuteScalar(SQLiteHelper.connectionString, CommandType.Text, cmdText.ToString());
            string url = GetUrl(sid, "goodtype3", xls, usercode, version);

            //SyncResultModel<Goodtype3Model> syncResult = HttpClient.GetAasync<Goodtype3Model>(url, null);
            //AsyncCallbackGoodtype3(syncResult);
            try
            {
                HttpClient.GetAasync<Goodtype3Model>(url, null, syncResult =>
                    {
                        AsyncCallbackGoodtype3(syncResult);
                    });
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        private static void AsyncCallbackGoodtype3(SyncResultModel<Goodtype3Model> syncResult)
        {
            List<Goodtype3Model> datas = syncResult.datas;

            if (datas.Count > 0)
            {
                #region 同步到数据库
                //using (SQLiteConnection con = new SQLiteConnection(SQLiteHelper.connectionString))
                //{
                SQLiteConnection con = SQLiteHelper.DbConnection;
                using (SQLiteCommand cmd = new SQLiteCommand())
                {
                    con.Open();
                    SQLiteTransaction sqltran = con.BeginTransaction();
                    cmd.Connection = con;
                    cmd.Transaction = sqltran;

                    try
                    {
                        foreach (Goodtype3Model item in datas)
                        {
                            cmd.Parameters.Clear();
                            string cmdText = "select count(*) from goodtype3 where SID=@SID";
                            SQLiteParameter[] parameters = new SQLiteParameter[1];
                            parameters[0] = new SQLiteParameter("SID", DbType.Int32);
                            parameters[0].Value = item.SID;
                            cmd.CommandText = cmdText;
                            cmd.Parameters.AddRange(parameters);
                            long result = (long)cmd.ExecuteScalar();

                            parameters = new SQLiteParameter[8];
                            parameters[0] = new SQLiteParameter("SID", DbType.Int32);
                            parameters[0].Value = item.SID;
                            parameters[1] = new SQLiteParameter("goodtype1", DbType.String);
                            parameters[1].Value = item.goodtype1;
                            parameters[2] = new SQLiteParameter("goodtype2", DbType.String);
                            parameters[2].Value = item.goodtype2;
                            parameters[3] = new SQLiteParameter("goodtype3", DbType.String);
                            parameters[3].Value = item.goodtype3;
                            parameters[4] = new SQLiteParameter("xtableid", DbType.Int32);
                            parameters[4].Value = item.xtableid;
                            parameters[5] = new SQLiteParameter("xsubid", DbType.Int32);
                            parameters[5].Value = item.xsubid;
                            parameters[6] = new SQLiteParameter("xversion", DbType.Double);
                            parameters[6].Value = item.xversion;
                            parameters[7] = new SQLiteParameter("uclssprics", DbType.String);
                            parameters[7].Value = item.uclssprics;

                            if (result == 0)
                            {
                                cmdText = @"INSERT INTO goodtype3 (
                                          SID,
                                          goodtype1,
                                          goodtype2,
                                          goodtype3,
                                          uclssprics,
                                          xtableid,
                                          xsubid,
                                          xversion
                                      )
                                      VALUES (
                                          @SID,
                                          @goodtype1,
                                          @goodtype2,
                                          @goodtype3,
                                          @uclssprics,
                                          @xtableid,
                                          @xsubid,
                                          @xversion
                                      )";

                                cmd.Parameters.Clear();
                                cmd.CommandText = cmdText.ToString();
                                cmd.Parameters.AddRange(parameters);
                                cmd.ExecuteNonQuery();
                            }
                            else
                            {
                                cmdText = @"UPDATE goodtype3
                                               SET goodtype1 = @goodtype1,
                                                   goodtype2 = @goodtype2,
                                                   goodtype3 = @goodtype3,
                                                   uclssprics = @uclssprics,
                                                   xtableid = @xtableid,
                                                   xsubid = @xsubid,
                                                   xversion = @xversion
                                             WHERE SID = @SID";

                                cmd.Parameters.Clear();
                                cmd.CommandText = cmdText;
                                cmd.Parameters.AddRange(parameters);
                                cmd.ExecuteNonQuery();
                            }
                        }

                        sqltran.Commit();
                    }
                    catch (Exception ex)
                    {
                        sqltran.Rollback();
                        throw ex;
                    }
                    finally
                    {
                        //wait_Sync.Set();
                    }
                }
                // }
                #endregion
            }
            //wait_Sync.Set();
        }
        #endregion

        #region 货品类型
        public static void SyncGoodkeys(string sid, string xls, string usercode)
        {
            StringBuilder cmdText = new StringBuilder();
            cmdText.AppendLine("select max(xversion) from goodkeys");
            object version = SQLiteHelper.ExecuteScalar(SQLiteHelper.connectionString, CommandType.Text, cmdText.ToString());
            string url = GetUrl(sid, "goodkeys", xls, usercode, version);
            //SyncResultModel<GoodkeysModel> syncResult = HttpClient.GetAasync<GoodkeysModel>(url, null);
            //AsyncCallbackGoodkeys(syncResult);
            try
            {
                HttpClient.GetAasync<GoodkeysModel>(url, null, syncResult =>
                   {
                       AsyncCallbackGoodkeys(syncResult);
                   });
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        private static void AsyncCallbackGoodkeys(SyncResultModel<GoodkeysModel> syncResult)
        {
            List<GoodkeysModel> datas = syncResult.datas;
            if (datas.Count > 0)
            {
                #region 同步到数据库
                //using (SQLiteConnection con = new SQLiteConnection(SQLiteHelper.connectionString))
                //{
                SQLiteConnection con = SQLiteHelper.DbConnection;
                using (SQLiteCommand cmd = new SQLiteCommand())
                {
                    con.Open();
                    SQLiteTransaction sqltran = con.BeginTransaction();
                    cmd.Connection = con;
                    cmd.Transaction = sqltran;

                    try
                    {
                        foreach (GoodkeysModel item in datas)
                        {
                            cmd.Parameters.Clear();
                            string cmdText = "select count(*) from goodkeys where SID=@SID";
                            SQLiteParameter[] parameters = new SQLiteParameter[1];
                            parameters[0] = new SQLiteParameter("SID", DbType.Int32);
                            parameters[0].Value = item.SID;
                            cmd.CommandText = cmdText;
                            cmd.Parameters.AddRange(parameters);
                            long result = (long)cmd.ExecuteScalar();

                            parameters = new SQLiteParameter[5];
                            parameters[0] = new SQLiteParameter("SID", DbType.Int32);
                            parameters[0].Value = item.SID;
                            parameters[1] = new SQLiteParameter("goodkeys", DbType.String);
                            parameters[1].Value = item.goodkeys;
                            parameters[2] = new SQLiteParameter("xtableid", DbType.Int32);
                            parameters[2].Value = item.xtableid;
                            parameters[3] = new SQLiteParameter("xsubid", DbType.Int32);
                            parameters[3].Value = item.xsubid;
                            parameters[4] = new SQLiteParameter("xversion", DbType.Double);
                            parameters[4].Value = item.xversion;

                            if (result == 0)
                            {
                                cmdText = @"INSERT INTO goodkeys (
                                             SID,
                                             goodkeys,
                                             xtableid,
                                             xsubid,
                                             xversion
                                         )
                                         VALUES (
                                             @SID,
                                             @goodkeys,
                                             @xtableid,
                                             @xsubid,
                                             @xversion
                                         )";

                                cmd.Parameters.Clear();
                                cmd.CommandText = cmdText.ToString();
                                cmd.Parameters.AddRange(parameters);
                                cmd.ExecuteNonQuery();
                            }
                            else
                            {
                                cmdText = @"UPDATE goodkeys
                                           SET goodkeys = @goodkeys,
                                               xtableid = @xtableid,
                                               xsubid = @xsubid,
                                               xversion = @xversion
                                         WHERE SID = @SID";

                                cmd.Parameters.Clear();
                                cmd.CommandText = cmdText;
                                cmd.Parameters.AddRange(parameters);
                                cmd.ExecuteNonQuery();
                            }
                        }

                        sqltran.Commit();
                    }
                    catch (Exception ex)
                    {
                        sqltran.Rollback();
                        throw ex;
                    }
                    finally
                    {
                        con.Close();
                        //wait_Sync.Set();
                    }
                }
                // }
                #endregion
            }
            //wait_Sync.Set();
        }
        #endregion

        #region 货品信息
        private static void SyncGood(string sid, string xls, string usercode)
        {
            StringBuilder cmdText = new StringBuilder();
            cmdText.AppendFormat("select max(xversion) from good");
            object version = SQLiteHelper.ExecuteScalar(SQLiteHelper.connectionString, CommandType.Text, cmdText.ToString());
            string url = GetUrl(sid, "good", xls, usercode, version);
            //SyncResultModel<GoodModel> result = HttpClient.GetAasync<GoodModel>(url, null);
            //AsyncCallbackGood(result);
            try
            {
                HttpClient.GetAasync<GoodModel>(url, null, syncResult =>
                  {
                      AsyncCallbackGood(syncResult);
                  });
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        private static void AsyncCallbackGood(SyncResultModel<GoodModel> syncResult)
        {
            List<GoodModel> datas = syncResult.datas;

            if (datas.Count > 0)
            {
                #region 同步到数据库
                //using (SQLiteConnection con = new SQLiteConnection(SQLiteHelper.connectionString))
                //{
                SQLiteConnection con = SQLiteHelper.DbConnection;
                using (SQLiteCommand cmd = new SQLiteCommand())
                {
                    con.Open();
                    SQLiteTransaction sqltran = con.BeginTransaction();
                    cmd.Connection = con;
                    cmd.Transaction = sqltran;

                    try
                    {
                        foreach (GoodModel item in datas)
                        {
                            cmd.Parameters.Clear();
                            string cmdText = "select count(*) from good where SID=@SID";
                            SQLiteParameter[] parameters = new SQLiteParameter[1];
                            parameters[0] = new SQLiteParameter("SID", DbType.Int32);
                            parameters[0].Value = item.SID;
                            cmd.CommandText = cmdText.ToString();
                            cmd.Parameters.AddRange(parameters);
                            long result = (long)cmd.ExecuteScalar();

                            parameters = new SQLiteParameter[21];
                            parameters[0] = new SQLiteParameter("SID", DbType.Int32);
                            parameters[0].Value = item.SID;
                            parameters[1] = new SQLiteParameter("goodtype1", DbType.String);
                            parameters[1].Value = item.goodtype1;
                            parameters[2] = new SQLiteParameter("goodtype2", DbType.String);
                            parameters[2].Value = item.goodtype2;
                            parameters[3] = new SQLiteParameter("goodtype3", DbType.String);
                            parameters[3].Value = item.goodtype3;
                            parameters[4] = new SQLiteParameter("goodcode", DbType.String);
                            parameters[4].Value = item.goodcode;
                            parameters[5] = new SQLiteParameter("goodname", DbType.String);
                            parameters[5].Value = item.goodname;
                            parameters[6] = new SQLiteParameter("goodunit", DbType.String);
                            parameters[6].Value = item.goodunit;
                            parameters[7] = new SQLiteParameter("xmulunit", DbType.String);
                            parameters[7].Value = item.xmulunit;
                            parameters[8] = new SQLiteParameter("xshow", DbType.Boolean);
                            parameters[8].Value = item.xshow;
                            parameters[9] = new SQLiteParameter("goodkeys", DbType.String);
                            parameters[9].Value = item.goodkeys;
                            parameters[10] = new SQLiteParameter("xprico", DbType.Decimal);
                            parameters[10].Value = item.xprico;
                            parameters[11] = new SQLiteParameter("xweight", DbType.Decimal);
                            parameters[11].Value = item.xweight;
                            parameters[12] = new SQLiteParameter("xsendjf", DbType.Decimal);
                            parameters[12].Value = item.xsendjf;
                            parameters[13] = new SQLiteParameter("xchagjf", DbType.Decimal);
                            parameters[13].Value = item.xchagjf;
                            parameters[14] = new SQLiteParameter("xls", DbType.String);
                            parameters[14].Value = item.xls;
                            parameters[15] = new SQLiteParameter("xlsname", DbType.String);
                            parameters[15].Value = item.xlsname;
                            parameters[16] = new SQLiteParameter("xtableid", DbType.Int32);
                            parameters[16].Value = item.xtableid;
                            parameters[17] = new SQLiteParameter("xsubid", DbType.Int32);
                            parameters[17].Value = item.xsubid;
                            parameters[18] = new SQLiteParameter("xjpiny", DbType.String);
                            parameters[18].Value = item.xjpiny;
                            parameters[19] = new SQLiteParameter("xqpiny", DbType.String);
                            parameters[19].Value = item.xqpiny;
                            parameters[20] = new SQLiteParameter("xversion", DbType.Double);
                            parameters[20].Value = item.xversion;

                            if (result == 0)
                            {
                                cmdText = @"INSERT INTO good (
                                             SID,
                                             goodtype1,
                                             goodtype2,
                                             goodtype3,
                                             goodcode,
                                             goodname,
                                             xjpiny,
                                             xqpiny,
                                             goodunit,
                                             xmulunit,
                                             xshow,
                                             goodkeys,
                                             xprico,
                                             xweight,
                                             xsendjf,
                                             xchagjf,
                                             xls,
                                             xlsname,
                                             xtableid,
                                             xsubid,
                                             xversion
                                         )
                                         VALUES (
                                             @SID,
                                             @goodtype1,
                                             @goodtype2,
                                             @goodtype3,
                                             @goodcode,
                                             @goodname,
                                             @xjpiny,
                                             @xqpiny,
                                             @goodunit,
                                             @xmulunit,
                                             @xshow,
                                             @goodkeys,
                                             @xprico,
                                             @xweight,
                                             @xsendjf,
                                             @xchagjf,
                                             @xls,
                                             @xlsname,
                                             @xtableid,
                                             @xsubid,
                                             @xversion
                                         )";

                                cmd.Parameters.Clear();
                                cmd.CommandText = cmdText.ToString();
                                cmd.Parameters.AddRange(parameters);
                                cmd.ExecuteNonQuery();
                            }
                            else
                            {
                                cmdText = @"UPDATE good
                                           SET goodtype1 = @goodtype1,
                                               goodtype2 = @goodtype2,
                                               goodtype3 = @goodtype3,
                                               goodcode = @goodcode,
                                               goodname = @goodname,
                                               xjpiny=@xjpiny,
                                               xqpiny=@xqpiny,
                                               goodunit = @goodunit,
                                               xmulunit = @xmulunit,
                                               xshow = @xshow,
                                               goodkeys = @goodkeys,
                                               xprico = @xprico,
                                               xweight = @xweight,
                                               xsendjf = @xsendjf,
                                               xchagjf = @xchagjf,
                                               xls = @xls,
                                               xlsname = @xlsname,
                                               xtableid = @xtableid,
                                               xsubid = @xsubid,
                                               xversion = @xversion
                                         WHERE SID = @SID ";

                                cmd.Parameters.Clear();
                                cmd.CommandText = cmdText;
                                cmd.Parameters.AddRange(parameters);
                                cmd.ExecuteNonQuery();
                            }
                        }

                        sqltran.Commit();
                    }
                    catch (Exception ex)
                    {
                        sqltran.Rollback();
                        throw ex;
                    }
                    finally
                    {
                        con.Close();
                        //wait_Sync.Set();
                    }
                }
                //}
                #endregion
            }
            //wait_Sync.Set();
        }
        #endregion

        #region 货品价格
        private static void SyncGoodpric(string sid, string xls, string usercode)
        {
            StringBuilder cmdText = new StringBuilder();
            cmdText.AppendFormat("select max(xversion) from goodpric");
            object version = SQLiteHelper.ExecuteScalar(SQLiteHelper.connectionString, CommandType.Text, cmdText.ToString());
            string url = GetUrl(sid, "goodpric", xls, usercode, version);
            //SyncResultModel<GoodpricModel> syncResult = HttpClient.GetAasync<GoodpricModel>(url, null);
            //AsyncCallbackGoodpric(syncResult);
            try
            {
                HttpClient.GetAasync<GoodpricModel>(url, null, syncResult =>
                   {
                       AsyncCallbackGoodpric(syncResult);
                   });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private static void AsyncCallbackGoodpric(SyncResultModel<GoodpricModel> syncResult)
        {
            List<GoodpricModel> datas = syncResult.datas;
            if (datas.Count > 0)
            {
                #region 同步到数据库
                // using (SQLiteConnection con = new SQLiteConnection(SQLiteHelper.connectionString))
                //{
                SQLiteConnection con = SQLiteHelper.DbConnection;
                using (SQLiteCommand cmd = new SQLiteCommand())
                {
                    con.Open();
                    SQLiteTransaction sqltran = con.BeginTransaction();
                    cmd.Connection = con;
                    cmd.Transaction = sqltran;

                    try
                    {
                        foreach (GoodpricModel item in datas)
                        {
                            cmd.Parameters.Clear();
                            string cmdText = "select count(*) from goodpric where SID=@SID";
                            SQLiteParameter[] parameters = new SQLiteParameter[1];
                            parameters[0] = new SQLiteParameter("SID", DbType.Int32);
                            parameters[0].Value = item.SID;
                            cmd.CommandText = cmdText.ToString();
                            cmd.Parameters.AddRange(parameters);
                            long result = (long)cmd.ExecuteScalar();

                            parameters = new SQLiteParameter[17];
                            parameters[0] = new SQLiteParameter("SID", DbType.Int32);
                            parameters[0].Value = item.SID;
                            parameters[1] = new SQLiteParameter("xpric", DbType.Decimal);
                            parameters[1].Value = item.xpric;
                            parameters[2] = new SQLiteParameter("goodkind1", DbType.String);
                            parameters[2].Value = item.goodkind1;
                            parameters[3] = new SQLiteParameter("goodkind2", DbType.String);
                            parameters[3].Value = item.goodkind2;
                            parameters[4] = new SQLiteParameter("goodkind3", DbType.String);
                            parameters[4].Value = item.goodkind3;
                            parameters[5] = new SQLiteParameter("goodkind4", DbType.String);
                            parameters[5].Value = item.goodkind4;
                            parameters[6] = new SQLiteParameter("goodkind5", DbType.String);
                            parameters[6].Value = item.goodkind5;
                            parameters[7] = new SQLiteParameter("goodkind6", DbType.String);
                            parameters[7].Value = item.goodkind6;
                            parameters[8] = new SQLiteParameter("goodkind7", DbType.String);
                            parameters[8].Value = item.goodkind7;
                            parameters[9] = new SQLiteParameter("goodkind8", DbType.String);
                            parameters[9].Value = item.goodkind8;
                            parameters[10] = new SQLiteParameter("goodkind9", DbType.String);
                            parameters[10].Value = item.goodkind9;
                            parameters[11] = new SQLiteParameter("goodkind10", DbType.String);
                            parameters[11].Value = item.goodkind10;
                            parameters[12] = new SQLiteParameter("xtableid", DbType.Int32);
                            parameters[12].Value = item.xtableid;
                            parameters[13] = new SQLiteParameter("xsubid", DbType.Int32);
                            parameters[13].Value = item.xsubid;
                            parameters[14] = new SQLiteParameter("xsendjf", DbType.Decimal);
                            parameters[14].Value = item.xsendjf;
                            parameters[15] = new SQLiteParameter("xchagjf", DbType.Decimal);
                            parameters[15].Value = item.xchagjf;
                            parameters[16] = new SQLiteParameter("xversion", DbType.Double);
                            parameters[16].Value = item.xversion;

                            if (result == 0)
                            {
                                cmdText = @"INSERT INTO goodpric (
                                             SID,
                                             xpric,
                                             goodkind1,
                                             goodkind2,
                                             goodkind3,
                                             goodkind4,
                                             goodkind5,
                                             goodkind6,
                                             goodkind7,
                                             goodkind8,
                                             goodkind9,
                                             goodkind10,
                                             xtableid,
                                             xsubid,
                                             xsendjf,
                                             xchagjf,
                                             xversion
                                         )
                                         VALUES (
                                             @SID,
                                             @xpric,
                                             @goodkind1,
                                             @goodkind2,
                                             @goodkind3,
                                             @goodkind4,
                                             @goodkind5,
                                             @goodkind6,
                                             @goodkind7,
                                             @goodkind8,
                                             @goodkind9,
                                             @goodkind10,
                                             @xtableid,
                                             @xsubid,
                                             @xsendjf,
                                             @xchagjf,
                                             @xversion
                                         )";

                                cmd.Parameters.Clear();
                                cmd.CommandText = cmdText.ToString();
                                cmd.Parameters.AddRange(parameters);
                                cmd.ExecuteNonQuery();
                            }
                            else
                            {
                                cmdText = @"UPDATE goodpric
                                           SET xpric = @xpric,
                                               goodkind1 = @goodkind1,
                                               goodkind2 = @goodkind2,
                                               goodkind3 = @goodkind3,
                                               goodkind4 = @goodkind4,
                                               goodkind5 = @goodkind5,
                                               goodkind6 = @goodkind6,
                                               goodkind7 = @goodkind7,
                                               goodkind8 = @goodkind8,
                                               goodkind9 = @goodkind9,
                                               goodkind10 = @goodkind10,
                                               xtableid = @xtableid,
                                               xsubid = @xsubid,
                                               xsendjf=@xsendjf,
                                               xchagjf=@xchagjf,
                                               xversion = @xversion
                                         WHERE SID = @SID";

                                cmd.Parameters.Clear();
                                cmd.CommandText = cmdText;
                                cmd.Parameters.AddRange(parameters);
                                cmd.ExecuteNonQuery();
                            }
                        }

                        sqltran.Commit();
                    }
                    catch (Exception ex)
                    {
                        sqltran.Rollback();
                        throw ex;
                    }
                    finally
                    {
                        con.Close();
                        //wait_Sync.Set();
                    }
                }
                // }
                #endregion
            }
            //wait_Sync.Set();
        }
        #endregion

        #region 等级价格
        private static void SyncGoodpric2(string sid, string xls, string usercode)
        {
            StringBuilder cmdText = new StringBuilder();
            cmdText.AppendFormat("select max(xversion) from goodpric2");
            object version = SQLiteHelper.ExecuteScalar(SQLiteHelper.connectionString, CommandType.Text, cmdText.ToString());
            string url = GetUrl(sid, "goodpric2", xls, usercode, version);
            //SyncResultModel<Goodpric2Model> syncResult = HttpClient.GetAasync<Goodpric2Model>(url, null);
            //AsyncCallbackGoodpric2(syncResult);
            try
            {

                HttpClient.GetAasync<Goodpric2Model>(url, null, syncResult =>
                {
                    AsyncCallbackGoodpric2(syncResult);
                });
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private static void AsyncCallbackGoodpric2(SyncResultModel<Goodpric2Model> syncResult)
        {
            List<Goodpric2Model> datas = syncResult.datas;
            if (datas.Count > 0)
            {
                #region 同步到数据库
                //using (SQLiteConnection con = new SQLiteConnection(SQLiteHelper.connectionString))
                //{
                SQLiteConnection con = SQLiteHelper.DbConnection;
                using (SQLiteCommand cmd = new SQLiteCommand())
                {
                    con.Open();
                    SQLiteTransaction sqltran = con.BeginTransaction();
                    cmd.Connection = con;
                    cmd.Transaction = sqltran;

                    try
                    {
                        foreach (Goodpric2Model item in datas)
                        {
                            cmd.Parameters.Clear();
                            string cmdText = "select count(*) from goodpric2 where SID=@SID";
                            SQLiteParameter[] parameters = new SQLiteParameter[1];
                            parameters[0] = new SQLiteParameter("SID", DbType.Int32);
                            parameters[0].Value = item.SID;
                            cmd.CommandText = cmdText.ToString();
                            cmd.Parameters.AddRange(parameters);
                            long result = (long)cmd.ExecuteScalar();

                            parameters = new SQLiteParameter[6];
                            parameters[0] = new SQLiteParameter("SID", DbType.Int32);
                            parameters[0].Value = item.SID;
                            parameters[1] = new SQLiteParameter("clntclss", DbType.String);
                            parameters[1].Value = item.clntclss;
                            parameters[2] = new SQLiteParameter("xpric", DbType.Decimal);
                            parameters[2].Value = item.xpric;
                            parameters[3] = new SQLiteParameter("xtableid", DbType.Int32);
                            parameters[3].Value = item.xtableid;
                            parameters[4] = new SQLiteParameter("xsubid", DbType.Int32);
                            parameters[4].Value = item.xsubid;
                            parameters[5] = new SQLiteParameter("xversion", DbType.Double);
                            parameters[5].Value = item.xversion;

                            if (result == 0)
                            {
                                cmdText = @"INSERT INTO goodpric2 (
                                              SID,
                                              clntclss,
                                              xpric,
                                              xtableid,
                                              xsubid,
                                              xversion
                                          )
                                          VALUES (
                                              @SID,
                                              @clntclss,
                                              @xpric,
                                              @xtableid,
                                              @xsubid,
                                              @xversion
                                          )";

                                cmd.Parameters.Clear();
                                cmd.CommandText = cmdText.ToString();
                                cmd.Parameters.AddRange(parameters);
                                cmd.ExecuteNonQuery();
                            }
                            else
                            {
                                cmdText = @"UPDATE goodpric2
                                           SET clntclss = @clntclss,
                                               xpric = @xpric,
                                               xtableid = @xtableid,
                                               xsubid = @xsubid,
                                               xversion = @xversion
                                         WHERE SID = @SID";

                                cmd.Parameters.Clear();
                                cmd.CommandText = cmdText;
                                cmd.Parameters.AddRange(parameters);
                                cmd.ExecuteNonQuery();
                            }
                        }

                        sqltran.Commit();
                    }
                    catch (Exception ex)
                    {
                        sqltran.Rollback();
                        throw ex;
                    }
                    finally
                    {
                        con.Close();
                        //wait_Sync.Set();
                    }
                }
                // }
                #endregion
            }
            //wait_Sync.Set();
        }
        #endregion

        #region 一品多码
        private static void SyncGbarcode(string sid, string xls, string usercode)
        {
            StringBuilder cmdText = new StringBuilder();
            cmdText.AppendFormat("select max(xversion) from gbarcode");
            object version = SQLiteHelper.ExecuteScalar(SQLiteHelper.connectionString, CommandType.Text, cmdText.ToString());
            string url = GetUrl(sid, "gbarcode", xls, usercode, version);
            //SyncResultModel<GbarcodeModel> syncResult = HttpClient.GetAasync<GbarcodeModel>(url, null);
            //AsyncCallbackGbarcode(syncResult);
            try
            {
                HttpClient.GetAasync<GbarcodeModel>(url, null, syncResult =>
               {
                   AsyncCallbackGbarcode(syncResult);
               });
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private static void AsyncCallbackGbarcode(SyncResultModel<GbarcodeModel> syncResult)
        {
            List<GbarcodeModel> datas = syncResult.datas;
            if (datas.Count > 0)
            {
                #region 同步到数据库
                //using (SQLiteConnection con = new SQLiteConnection(SQLiteHelper.connectionString))
                //{
                SQLiteConnection con = SQLiteHelper.DbConnection;
                using (SQLiteCommand cmd = new SQLiteCommand())
                {
                    con.Open();
                    SQLiteTransaction sqltran = con.BeginTransaction();
                    cmd.Connection = con;
                    cmd.Transaction = sqltran;

                    try
                    {
                        foreach (GbarcodeModel item in datas)
                        {
                            cmd.Parameters.Clear();
                            string cmdText = "select count(*) from gbarcode where SID=@SID";
                            SQLiteParameter[] parameters = new SQLiteParameter[1];
                            parameters[0] = new SQLiteParameter("SID", DbType.Int32);
                            parameters[0].Value = item.SID;
                            cmd.CommandText = cmdText.ToString();
                            cmd.Parameters.AddRange(parameters);
                            long result = (long)cmd.ExecuteScalar();

                            parameters = new SQLiteParameter[8];
                            parameters[0] = new SQLiteParameter("SID", DbType.Int32);
                            parameters[0].Value = item.SID;
                            parameters[1] = new SQLiteParameter("xbarcode", DbType.String);
                            parameters[1].Value = item.xbarcode;
                            parameters[2] = new SQLiteParameter("goodunit", DbType.String);
                            parameters[2].Value = item.goodunit;
                            parameters[3] = new SQLiteParameter("xtype", DbType.String);
                            parameters[3].Value = item.xtype;
                            parameters[4] = new SQLiteParameter("xtableid", DbType.Int32);
                            parameters[4].Value = item.xtableid;
                            parameters[5] = new SQLiteParameter("xsubid", DbType.Int32);
                            parameters[5].Value = item.xsubid;
                            parameters[6] = new SQLiteParameter("xversion", DbType.Double);
                            parameters[6].Value = item.xversion;
                            parameters[7] = new SQLiteParameter("xpric", DbType.Decimal);
                            parameters[7].Value = item.xpric;


                            if (result == 0)
                            {
                                cmdText = @"INSERT INTO gbarcode (
                                             SID,
                                             xbarcode,
                                             goodunit,
                                             xtype,
                                             xtableid,
                                             xsubid,
                                             xpric,
                                             xversion
                                         )
                                         VALUES (
                                             @SID,
                                             @xbarcode,
                                             @goodunit,
                                             @xtype,
                                             @xtableid,
                                             @xsubid,
                                             @xpric,
                                             @xversion
                                         )";

                                cmd.Parameters.Clear();
                                cmd.CommandText = cmdText.ToString();
                                cmd.Parameters.AddRange(parameters);
                                cmd.ExecuteNonQuery();
                            }
                            else
                            {
                                cmdText = @"UPDATE gbarcode
                                           SET xbarcode = @xbarcode,
                                               goodunit = @goodunit,
                                               xtype = @xtype,
                                               xtableid = @xtableid,
                                               xsubid = @xsubid,
                                               xpric=@xpric,
                                               xversion = @xversion
                                         WHERE SID = @SID";

                                cmd.Parameters.Clear();
                                cmd.CommandText = cmdText;
                                cmd.Parameters.AddRange(parameters);
                                cmd.ExecuteNonQuery();
                            }
                        }

                        sqltran.Commit();
                    }
                    catch (Exception ex)
                    {
                        sqltran.Rollback();
                        throw ex;
                    }
                    finally
                    {
                        con.Close();
                        //wait_Sync.Set();
                    }
                }
                //}
                #endregion
            }
            //wait_Sync.Set();
        }
        #endregion

        #region 货品规格
        private static void SyncGoodkind(string sid, string xls, string usercode)
        {
            StringBuilder cmdText = new StringBuilder();
            cmdText.AppendFormat("select max(xversion) from goodkind");
            object version = SQLiteHelper.ExecuteScalar(SQLiteHelper.connectionString, CommandType.Text, cmdText.ToString());
            string url = GetUrl(sid, "goodkind", xls, usercode, version);
            //SyncResultModel<GoodkindModel> syncResult = HttpClient.GetAasync<GoodkindModel>(url, null);
            //AsyncCallbackGoodkind(syncResult);
            try
            {
                HttpClient.GetAasync<GoodkindModel>(url, null, syncResult =>
                {
                    AsyncCallbackGoodkind(syncResult);
                });
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private static void AsyncCallbackGoodkind(SyncResultModel<GoodkindModel> syncResult)
        {
            List<GoodkindModel> datas = syncResult.datas;
            if (datas.Count > 0)
            {
                #region 同步到数据库
                //using (SQLiteConnection con = new SQLiteConnection(SQLiteHelper.connectionString))
                //{
                SQLiteConnection con = SQLiteHelper.DbConnection;
                using (SQLiteCommand cmd = new SQLiteCommand())
                {
                    con.Open();
                    SQLiteTransaction sqltran = con.BeginTransaction();
                    cmd.Connection = con;
                    cmd.Transaction = sqltran;

                    try
                    {
                        foreach (GoodkindModel item in datas)
                        {
                            cmd.Parameters.Clear();
                            string cmdText = "select count(*) from goodkind where SID=@SID";
                            SQLiteParameter[] parameters = new SQLiteParameter[1];
                            parameters[0] = new SQLiteParameter("SID", DbType.Int32);
                            parameters[0].Value = item.SID;
                            cmd.CommandText = cmdText.ToString();
                            cmd.Parameters.AddRange(parameters);
                            long result = (long)cmd.ExecuteScalar();

                            parameters = new SQLiteParameter[7];
                            parameters[0] = new SQLiteParameter("SID", DbType.Int32);
                            parameters[0].Value = item.SID;
                            parameters[1] = new SQLiteParameter("xno", DbType.String);
                            parameters[1].Value = item.xno;
                            parameters[2] = new SQLiteParameter("goodkind", DbType.String);
                            parameters[2].Value = item.goodkind;
                            parameters[3] = new SQLiteParameter("goodkinds", DbType.String);
                            parameters[3].Value = item.goodkinds;
                            parameters[4] = new SQLiteParameter("xtableid", DbType.Int32);
                            parameters[4].Value = item.xtableid;
                            parameters[5] = new SQLiteParameter("xsubid", DbType.Int32);
                            parameters[5].Value = item.xsubid;
                            parameters[6] = new SQLiteParameter("xversion", DbType.Double);
                            parameters[6].Value = item.xversion;

                            if (result == 0)
                            {
                                cmdText = @"INSERT INTO goodkind (
                                             SID,
                                             xno,
                                             goodkind,
                                             goodkinds,
                                             xtableid,
                                             xsubid,
                                             xversion
                                         )
                                         VALUES (
                                             @SID,
                                             @xno,
                                             @goodkind,
                                             @goodkinds,
                                             @xtableid,
                                             @xsubid,
                                             @xversion
                                         )";

                                cmd.Parameters.Clear();
                                cmd.CommandText = cmdText.ToString();
                                cmd.Parameters.AddRange(parameters);
                                cmd.ExecuteNonQuery();
                            }
                            else
                            {
                                cmdText = @"UPDATE goodkind
                                           SET xno = @xno,
                                               goodkind = @goodkind,
                                               goodkinds = @goodkinds,
                                               xtableid = @xtableid,
                                               xsubid = @xsubid,
                                               xversion = @xversion
                                         WHERE SID = @SID";

                                cmd.Parameters.Clear();
                                cmd.CommandText = cmdText;
                                cmd.Parameters.AddRange(parameters);
                                cmd.ExecuteNonQuery();
                            }
                        }

                        sqltran.Commit();
                    }
                    catch (Exception ex)
                    {
                        sqltran.Rollback();
                        throw ex;
                    }
                    finally
                    {
                        con.Close();
                        //wait_Sync.Set();
                    }
                }
                //}
                #endregion
            }
            //wait_Sync.Set();
        }
        #endregion

        #region 顾客类别
        private static void SyncClnttype(string sid, string xls, string usercode)
        {
            StringBuilder cmdText = new StringBuilder();
            cmdText.AppendFormat("select max(xversion) from clnttype");
            object version = SQLiteHelper.ExecuteScalar(SQLiteHelper.connectionString, CommandType.Text, cmdText.ToString());
            string url = GetUrl(sid, "clnttype", xls, usercode, version);
            //SyncResultModel<ClnttypeModel> syncResult = HttpClient.GetAasync<ClnttypeModel>(url, null);
            //AsyncCallbackClnttype(syncResult);
            try
            {
                HttpClient.GetAasync<ClnttypeModel>(url, null, syncResult =>
               {
                   AsyncCallbackClnttype(syncResult);
               });
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private static void AsyncCallbackClnttype(SyncResultModel<ClnttypeModel> syncResult)
        {
            List<ClnttypeModel> datas = syncResult.datas;
            if (datas.Count > 0)
            {
                #region 同步到数据库
                //using (SQLiteConnection con = new SQLiteConnection(SQLiteHelper.connectionString))
                //{
                SQLiteConnection con = SQLiteHelper.DbConnection;
                using (SQLiteCommand cmd = new SQLiteCommand())
                {
                    con.Open();
                    SQLiteTransaction sqltran = con.BeginTransaction();
                    cmd.Connection = con;
                    cmd.Transaction = sqltran;

                    try
                    {
                        foreach (ClnttypeModel item in datas)
                        {
                            cmd.Parameters.Clear();
                            string cmdText = "select count(*) from clnttype where SID=@SID";
                            SQLiteParameter[] parameters = new SQLiteParameter[1];
                            parameters[0] = new SQLiteParameter("SID", DbType.Int32);
                            parameters[0].Value = item.SID;
                            cmd.CommandText = cmdText.ToString();
                            cmd.Parameters.AddRange(parameters);
                            long result = (long)cmd.ExecuteScalar();

                            parameters = new SQLiteParameter[5];
                            parameters[0] = new SQLiteParameter("SID", DbType.Int32);
                            parameters[0].Value = item.SID;
                            parameters[1] = new SQLiteParameter("clnttype", DbType.String);
                            parameters[1].Value = item.clnttype;
                            parameters[2] = new SQLiteParameter("xtableid", DbType.Int32);
                            parameters[2].Value = item.xtableid;
                            parameters[3] = new SQLiteParameter("xsubid", DbType.Int32);
                            parameters[3].Value = item.xsubid;
                            parameters[4] = new SQLiteParameter("xversion", DbType.Double);
                            parameters[4].Value = item.xversion;

                            if (result == 0)
                            {
                                cmdText = @"INSERT INTO clnttype (
                                             SID,
                                             clnttype,
                                             xtableid,
                                             xsubid,
                                             xversion
                                         )
                                         VALUES (
                                             @SID,
                                             @clnttype,
                                             @xtableid,
                                             @xsubid,
                                             @xversion
                                         )";

                                cmd.Parameters.Clear();
                                cmd.CommandText = cmdText.ToString();
                                cmd.Parameters.AddRange(parameters);
                                cmd.ExecuteNonQuery();
                            }
                            else
                            {
                                cmdText = @"UPDATE clnttype
                                           SET clnttype = @clnttype,
                                               xtableid = @xtableid,
                                               xsubid = @xsubid,
                                               xversion = @xversion
                                         WHERE SID = @SID";

                                cmd.Parameters.Clear();
                                cmd.CommandText = cmdText;
                                cmd.Parameters.AddRange(parameters);
                                cmd.ExecuteNonQuery();
                            }
                        }

                        sqltran.Commit();
                    }
                    catch (Exception ex)
                    {
                        sqltran.Rollback();
                        throw ex;
                    }
                    finally
                    {
                        con.Close();
                        //wait_Sync.Set();
                    }
                }
                //}
                #endregion
            }
            //wait_Sync.Set();
        }
        #endregion

        #region 顾客等级
        private static void SyncClntclss(string sid, string xls, string usercode)
        {
            StringBuilder cmdText = new StringBuilder();
            cmdText.AppendFormat("select max(xversion) from clntclss");
            object version = SQLiteHelper.ExecuteScalar(SQLiteHelper.connectionString, CommandType.Text, cmdText.ToString());
            string url = GetUrl(sid, "clntclss", xls, usercode, version);
            //SyncResultModel<ClntclssModel> syncResult = HttpClient.GetAasync<ClntclssModel>(url, null);
            //AsyncCallbackClntclss(syncResult);
            try
            {
                HttpClient.GetAasync<ClntclssModel>(url, null, syncResult =>
               {
                   AsyncCallbackClntclss(syncResult);
               });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private static void AsyncCallbackClntclss(SyncResultModel<ClntclssModel> syncResult)
        {
            List<ClntclssModel> datas = syncResult.datas;
            if (datas.Count > 0)
            {
                #region 同步到数据库
                //using (SQLiteConnection con = new SQLiteConnection(SQLiteHelper.connectionString))
                //{
                SQLiteConnection con = SQLiteHelper.DbConnection;
                using (SQLiteCommand cmd = new SQLiteCommand())
                {
                    con.Open();
                    SQLiteTransaction sqltran = con.BeginTransaction();
                    cmd.Connection = con;
                    cmd.Transaction = sqltran;

                    try
                    {
                        foreach (ClntclssModel item in datas)
                        {
                            cmd.Parameters.Clear();
                            string cmdText = "select count(*) from clntclss where SID=@SID";
                            SQLiteParameter[] parameters = new SQLiteParameter[1];
                            parameters[0] = new SQLiteParameter("SID", DbType.Int32);
                            parameters[0].Value = item.SID;
                            cmd.CommandText = cmdText.ToString();
                            cmd.Parameters.AddRange(parameters);
                            long result = (long)cmd.ExecuteScalar();

                            parameters = new SQLiteParameter[7];
                            parameters[0] = new SQLiteParameter("SID", DbType.Int32);
                            parameters[0].Value = item.SID;
                            parameters[1] = new SQLiteParameter("clntclss", DbType.String);
                            parameters[1].Value = item.clntclss;
                            parameters[2] = new SQLiteParameter("xzhe", DbType.Decimal);
                            parameters[2].Value = item.xzhe;
                            parameters[3] = new SQLiteParameter("xdefault", DbType.Boolean);
                            parameters[3].Value = item.xdefault;
                            parameters[4] = new SQLiteParameter("xtableid", DbType.Int32);
                            parameters[4].Value = item.xtableid;
                            parameters[5] = new SQLiteParameter("xsubid", DbType.Int32);
                            parameters[5].Value = item.xsubid;
                            parameters[6] = new SQLiteParameter("xversion", DbType.Double);
                            parameters[6].Value = item.xversion;

                            if (result == 0)
                            {
                                cmdText = @"INSERT INTO clntclss (
                                             SID,
                                             clntclss,
                                             xzhe,
                                             xdefault,
                                             xtableid,
                                             xsubid,
                                             xversion
                                         )
                                         VALUES (
                                             @SID,
                                             @clntclss,
                                             @xzhe,
                                             @xdefault,
                                             @xtableid,
                                             @xsubid,
                                             @xversion
                                         )";

                                cmd.Parameters.Clear();
                                cmd.CommandText = cmdText.ToString();
                                cmd.Parameters.AddRange(parameters);
                                cmd.ExecuteNonQuery();
                            }
                            else
                            {
                                cmdText = @"UPDATE clntclss
                                           SET clntclss = @clntclss,
                                               xzhe = @xzhe,
                                               xdefault = @xdefault,
                                               xtableid = @xtableid,
                                               xsubid = @xsubid,
                                               xversion = @xversion
                                         WHERE SID = @SID";

                                cmd.Parameters.Clear();
                                cmd.CommandText = cmdText;
                                cmd.Parameters.AddRange(parameters);
                                cmd.ExecuteNonQuery();
                            }
                        }

                        sqltran.Commit();
                    }
                    catch (Exception ex)
                    {
                        sqltran.Rollback();
                        throw ex;
                    }
                    finally
                    {
                        con.Close();
                        //wait_Sync.Set();
                    }
                }
                //}
                #endregion
            }
            //wait_Sync.Set();
        }
        #endregion

        #region 顾客信息
        private static void SyncClnt(string sid, string xls, string usercode)
        {
            StringBuilder cmdText = new StringBuilder();
            cmdText.AppendLine("select max(xversion) from clnt where SID is not null");
            object version = SQLiteHelper.ExecuteScalar(SQLiteHelper.connectionString, CommandType.Text, cmdText.ToString());
            string url = GetUrl(sid, "clnt", xls, usercode, version);
            //SyncResultModel<ClntModel> syncResult = HttpClient.GetAasync<ClntModel>(url, null);
            //AsyncCallbackClnt(syncResult);
            try
            {
                HttpClient.GetAasync<ClntModel>(url, null, syncResult =>
                    {
                        AsyncCallbackClnt(syncResult);
                    });
            }
            catch (Exception ex)
            {
                throw ex;
            }

            //上传
            cmdText = new StringBuilder();
            cmdText.AppendLine("select * from clnt where SID is null or isUpdate =1");

            SQLiteDataReader dataReader = SQLiteHelper.ExecuteReader(SQLiteHelper.connectionString, CommandType.Text, cmdText.ToString());
            List<ClntModel> clnts = new List<ClntModel>();
            while (dataReader.Read())
            {
                ClntModel clnt = new ClntModel();
                clnt.SID = (dataReader["SID"] == null || string.IsNullOrEmpty(dataReader["SID"].ToString())) ? (int?)null : int.Parse(dataReader["SID"].ToString());
                clnt.ID = new Guid(dataReader["ID"].ToString());
                clnt.clnttype = dataReader["clnttype"].ToString();
                clnt.clntcode = dataReader["clntcode"].ToString();
                clnt.clntname = dataReader["clntname"].ToString();
                clnt.clntclss = dataReader["clntclss"].ToString();
                clnt.xls = dataReader["xls"].ToString();
                clnt.xlsname = dataReader["xlsname"].ToString();
                clnt.xbro = dataReader["xbro"].ToString() == string.Empty ? (DateTime?)null : DateTime.Parse(dataReader["xbro"].ToString());
                clnt.xpho = dataReader["xpho"].ToString();
                clnt.xintime = dataReader["xintime"].ToString();
                clnt.xadd = dataReader["xadd"].ToString();
                clnt.xnotes = dataReader["xnotes"].ToString();
                clnt.xversion = double.Parse(dataReader["xversion"].ToString());
                clnts.Add(clnt);
            }
            dataReader.Close();
            if (clnts.Count > 0)
            {

                var query = from p in clnts
                            select new
                            {
                                ID = p.ID,
                                SID = p.SID,
                                clnttype = p.clnttype,
                                clntcode = p.clntcode,
                                clntname = p.clntname,
                                clntclss = p.clntclss,
                                xbro = p.xbro.HasValue ? p.xbro.Value.ToString("yyyy-MM-dd") : string.Empty,
                                xpho = p.xpho,
                                xintime = p.xintime,
                                xadd = p.xadd,
                                xnotes = p.xnotes,
                                xls = p.xls,
                                xlsname = p.xlsname,
                                xversion = p.xversion,
                            };
                string json = js.Serialize(query);
                string postData = "sid=" + sid;
                postData += "&mod=" + "clnt";
                postData += "&xls=" + xls;
                postData += "&usercode=" + usercode;
                postData += "&data=" + HttpUtility.UrlEncode(json, Encoding.UTF8);
                byte[] byteArray = Encoding.UTF8.GetBytes(postData);

                try
                {
                    SyncResultModel<ClntModel> syncResult = HttpClient.PostAasync<ClntModel>(baseUrl_Push.ToString(), null, postData);
                    AsyncCallbackClnt_UP(syncResult);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        private static void AsyncCallbackClnt(SyncResultModel<ClntModel> syncResult)
        {
            List<ClntModel> datas = syncResult.datas;

            if (datas.Count == 0)
            {
                //wait_Sync.Set();
            }
            else
            {
                #region 同步到数据库
                //using (SQLiteConnection con = new SQLiteConnection(SQLiteHelper.connectionString))
                //{
                SQLiteConnection con = SQLiteHelper.DbConnection;
                using (SQLiteCommand cmd = new SQLiteCommand())
                {
                    con.Open();
                    SQLiteTransaction sqltran = con.BeginTransaction();
                    cmd.Connection = con;
                    cmd.Transaction = sqltran;

                    try
                    {
                        foreach (ClntModel item in datas)
                        {
                            cmd.Parameters.Clear();
                            string cmdText = "select count(*) from clnt where SID=@SID";
                            SQLiteParameter[] parameters = new SQLiteParameter[1];
                            parameters[0] = new SQLiteParameter("SID", DbType.Int32);
                            parameters[0].Value = item.SID;
                            cmd.CommandText = cmdText.ToString();
                            cmd.Parameters.AddRange(parameters);
                            long result = (long)cmd.ExecuteScalar();

                            parameters = new SQLiteParameter[42];
                            parameters[0] = new SQLiteParameter("SID", DbType.Int32);
                            parameters[0].Value = item.SID;
                            parameters[1] = new SQLiteParameter("ID", DbType.String);
                            parameters[1].Value = Guid.NewGuid();
                            parameters[2] = new SQLiteParameter("xls", DbType.String);
                            parameters[2].Value = item.xls;
                            parameters[3] = new SQLiteParameter("clnttype", DbType.String);
                            parameters[3].Value = item.clnttype;
                            parameters[4] = new SQLiteParameter("clntcode", DbType.String);
                            parameters[4].Value = item.clntcode;
                            parameters[5] = new SQLiteParameter("clntname", DbType.String);
                            parameters[5].Value = item.clntname;
                            parameters[6] = new SQLiteParameter("clntclss", DbType.String);
                            parameters[6].Value = item.clntclss;
                            parameters[7] = new SQLiteParameter("clntcodep", DbType.String);
                            parameters[7].Value = item.clntcodep;
                            parameters[8] = new SQLiteParameter("clntnamep", DbType.String);
                            parameters[8].Value = item.clntnamep;
                            parameters[9] = new SQLiteParameter("xuser", DbType.String);
                            parameters[9].Value = item.xuser;
                            parameters[10] = new SQLiteParameter("xlsj", DbType.String);
                            parameters[10].Value = item.xlsj;
                            parameters[11] = new SQLiteParameter("xlsnamej", DbType.String);
                            parameters[11].Value = item.xlsnamej;
                            parameters[12] = new SQLiteParameter("xmaill", DbType.String);
                            parameters[12].Value = item.xmaill;
                            parameters[13] = new SQLiteParameter("xpho", DbType.String);
                            parameters[13].Value = item.xpho;
                            parameters[14] = new SQLiteParameter("xname", DbType.String);
                            parameters[14].Value = item.xname;
                            parameters[15] = new SQLiteParameter("xbro", DbType.DateTime);
                            parameters[15].Value = item.xbro;
                            parameters[16] = new SQLiteParameter("xarea1", DbType.String);
                            parameters[16].Value = item.xarea1;
                            parameters[17] = new SQLiteParameter("xarea2", DbType.String);
                            parameters[17].Value = item.xarea2;
                            parameters[18] = new SQLiteParameter("xarea3", DbType.String);
                            parameters[18].Value = item.xarea3;
                            parameters[19] = new SQLiteParameter("xadd", DbType.String);
                            parameters[19].Value = item.xadd;
                            parameters[20] = new SQLiteParameter("xqq", DbType.String);
                            parameters[20].Value = item.xqq;
                            parameters[21] = new SQLiteParameter("xmyidx", DbType.String);
                            parameters[21].Value = item.xmyidx;
                            parameters[22] = new SQLiteParameter("xistelcheck", DbType.Boolean);
                            parameters[22].Value = item.xistelcheck;
                            parameters[23] = new SQLiteParameter("xisemlcheck", DbType.Boolean);
                            parameters[23].Value = item.xisemlcheck;
                            parameters[24] = new SQLiteParameter("xwxid", DbType.String);
                            parameters[24].Value = item.xwxid;
                            parameters[25] = new SQLiteParameter("xstop", DbType.Boolean);
                            parameters[25].Value = item.xstop;
                            parameters[26] = new SQLiteParameter("clntcodej", DbType.String);
                            parameters[26].Value = item.clntcodej;
                            parameters[27] = new SQLiteParameter("clntnamej", DbType.String);
                            parameters[27].Value = item.clntnamej;
                            parameters[28] = new SQLiteParameter("xlpath", DbType.String);
                            parameters[28].Value = item.xlpath;
                            parameters[29] = new SQLiteParameter("xlnum", DbType.Int32);
                            parameters[29].Value = item.xlnum;
                            parameters[30] = new SQLiteParameter("xinname", DbType.String);
                            parameters[30].Value = item.xinname;
                            parameters[31] = new SQLiteParameter("xintime", DbType.String);
                            parameters[31].Value = item.xintime;
                            parameters[32] = new SQLiteParameter("xlastlogintime", DbType.String);
                            parameters[32].Value = item.xlastlogintime;
                            parameters[33] = new SQLiteParameter("xls", DbType.String);
                            parameters[33].Value = item.xls;
                            parameters[34] = new SQLiteParameter("xlsname", DbType.String);
                            parameters[34].Value = item.xlsname;
                            parameters[35] = new SQLiteParameter("sharecode", DbType.String);
                            parameters[35].Value = item.sharecode;
                            parameters[36] = new SQLiteParameter("xnotes", DbType.String);
                            parameters[36].Value = item.xnotes;
                            parameters[37] = new SQLiteParameter("xtableid", DbType.Int32);
                            parameters[37].Value = item.xtableid;
                            parameters[38] = new SQLiteParameter("xsubid", DbType.Int32);
                            parameters[38].Value = item.xsubid;
                            parameters[39] = new SQLiteParameter("password", DbType.String);
                            parameters[39].Value = item.password;
                            parameters[40] = new SQLiteParameter("xcontime", DbType.Int32);
                            parameters[40].Value = item.xcontime;
                            parameters[41] = new SQLiteParameter("xversion", DbType.Double);
                            parameters[41].Value = item.xversion;


                            if (result == 0)
                            {
                                cmdText = @"INSERT INTO clnt (
                                             ID,
                                             SID,
                                             clnttype,
                                             clntcode,
                                             clntname,
                                             clntclss,
                                             clntcodep,
                                             clntnamep,
                                             xuser,
                                             xlsj,
                                             xlsnamej,
                                             xmaill,
                                             xpho,
                                             xname,
                                             xbro,
                                             xarea1,
                                             xarea2,
                                             xarea3,
                                             xadd,
                                             xqq,
                                             xmyidx,
                                             xistelcheck,
                                             xisemlcheck,
                                             xwxid,
                                             xstop,
                                             clntcodej,
                                             clntnamej,
                                             xlpath,
                                             xlnum,
                                             xinname,
                                             xintime,
                                             xlastlogintime,
                                             xls,
                                             xlsname,
                                             sharecode,
                                             xnotes,
                                             xtableid,
                                             xsubid,
                                             password,
                                             xcontime,
                                             xversion
                                         )
                                         VALUES (
                                             @ID,
                                             @SID,
                                             @clnttype,
                                             @clntcode,
                                             @clntname,
                                             @clntclss,
                                             @clntcodep,
                                             @clntnamep,
                                             @xuser,
                                             @xlsj,
                                             @xlsnamej,
                                             @xmaill,
                                             @xpho,
                                             @xname,
                                             @xbro,
                                             @xarea1,
                                             @xarea2,
                                             @xarea3,
                                             @xadd,
                                             @xqq,
                                             @xmyidx,
                                             @xistelcheck,
                                             @xisemlcheck,
                                             @xwxid,
                                             @xstop,
                                             @clntcodej,
                                             @clntnamej,
                                             @xlpath,
                                             @xlnum,
                                             @xinname,
                                             @xintime,
                                             @xlastlogintime,
                                             @xls,
                                             @xlsname,
                                             @sharecode,
                                             @xnotes,
                                             @xtableid,
                                             @xsubid,
                                             @password,
                                             @xcontime,
                                             @xversion
                                         )";

                                cmd.Parameters.Clear();
                                cmd.CommandText = cmdText.ToString();
                                cmd.Parameters.AddRange(parameters);
                                cmd.ExecuteNonQuery();
                            }
                            else
                            {
                                string sql = @"UPDATE clnt
                                               SET clnttype = @clnttype,
                                                   clntcode = @clntcode,
                                                   clntname = @clntname,
                                                   clntclss = @clntclss,
                                                   clntcodep = @clntcodep,
                                                   clntnamep = @clntnamep,
                                                   xuser = @xuser,
                                                   xlsj = @xlsj,
                                                   xlsnamej = @xlsnamej,
                                                   xmaill = @xmaill,
                                                   xpho = @xpho,
                                                   xname = @xname,
                                                   xbro = @xbro,
                                                   xarea1 = @xarea1,
                                                   xarea2 = @xarea2,
                                                   xarea3 = @xarea3,
                                                   xadd = @xadd,
                                                   xqq = @xqq,
                                                   xmyidx = @xmyidx,
                                                   xistelcheck = @xistelcheck,
                                                   xisemlcheck = @xisemlcheck,
                                                   xwxid = @xwxid,
                                                   xstop = @xstop,
                                                   clntcodej = @clntcodej,
                                                   clntnamej = @clntnamej,
                                                   xlpath = @xlpath,
                                                   xlnum = @xlnum,
                                                   xinname = @xinname,
                                                   xintime = @xintime,
                                                   xlastlogintime = @xlastlogintime,
                                                   xls = @xls,
                                                   xlsname = @xlsname,
                                                   sharecode = @sharecode,
                                                   xnotes = @xnotes,
                                                   xtableid = @xtableid,
                                                   xsubid = @xsubid,
                                                   password = @password,
                                                   xcontime=@xcontime,
                                                   xversion = @xversion
                                             WHERE SID = @SID";

                                cmd.Parameters.Clear();
                                cmd.CommandText = sql;
                                cmd.Parameters.AddRange(parameters);
                                cmd.ExecuteNonQuery();
                            }
                        }

                        sqltran.Commit();
                    }
                    catch (Exception ex)
                    {
                        sqltran.Rollback();
                        throw ex;
                    }
                    finally
                    {
                        con.Close();
                        //wait_Sync.Set();
                    }
                }
                // }
                #endregion
            }
        }
        private static void AsyncCallbackClnt_UP(SyncResultModel<ClntModel> syncResult)
        {
            List<ClntModel> datas = syncResult.datas;
            if (datas.Count > 0)
            {
                #region 同步到数据库
                //using (SQLiteConnection con = new SQLiteConnection(SQLiteHelper.connectionString))
                //{
                SQLiteConnection con = SQLiteHelper.DbConnection;
                using (SQLiteCommand cmd = new SQLiteCommand())
                {
                    con.Open();
                    SQLiteTransaction sqltran = con.BeginTransaction();
                    cmd.Connection = con;
                    cmd.Transaction = sqltran;
                    SQLiteParameter[] parameters = null;
                    try
                    {
                        foreach (ClntModel item in datas)
                        {
                            string sql = string.Empty;
                            if (string.IsNullOrEmpty(item.clntcode))
                            {
                                parameters = new SQLiteParameter[2];
                                parameters[0] = new SQLiteParameter("SID", DbType.Int32);
                                parameters[0].Value = item.SID;
                                parameters[1] = new SQLiteParameter("ID", DbType.String);
                                parameters[1].Value = item.ID;
                                sql = @"UPDATE clnt
                                               SET SID = @SID,
                                                   isUpdate=0
                                             WHERE ID = @ID";
                            }
                            else
                            {
                                parameters = new SQLiteParameter[3];
                                parameters[0] = new SQLiteParameter("SID", DbType.Int32);
                                parameters[0].Value = item.SID;
                                parameters[1] = new SQLiteParameter("ID", DbType.String);
                                parameters[1].Value = item.ID;
                                parameters[2] = new SQLiteParameter("clntcode", DbType.String);
                                parameters[2].Value = item.clntcode;
                                sql = @"UPDATE clnt
                                               SET SID = @SID,
                                                   isUpdate=0,
                                                   clntcode=@clntcode
                                             WHERE ID = @ID";
                            }



                            cmd.Parameters.Clear();
                            cmd.CommandText = sql;
                            cmd.Parameters.AddRange(parameters);
                            cmd.ExecuteNonQuery();
                        }
                        sqltran.Commit();
                    }
                    catch (Exception ex)
                    {
                        sqltran.Rollback();
                        throw ex;
                    }
                    finally
                    {
                        con.Close();
                        //wait_Sync.Set();
                    }
                }
                // }
                #endregion
            }
            //wait_Sync.Set();
        }
        #endregion

        #region 积分结余
        private static void SyncJjie2(string sid, string xls, string usercode)
        {
            StringBuilder cmdText = new StringBuilder();
            cmdText.AppendFormat("select max(xversion) from jjie2");
            object version = SQLiteHelper.ExecuteScalar(SQLiteHelper.connectionString, CommandType.Text, cmdText.ToString());
            string url = GetUrl(sid, "jjie2", xls, usercode, version);
            //SyncResultModel<Jjie2Model> syncResult = HttpClient.GetAasync<Jjie2Model>(url, null);
            //AsyncCallbackJjie2(syncResult);
            try
            {
                HttpClient.GetAasync<Jjie2Model>(url, null, syncResult =>
                    {
                        AsyncCallbackJjie2(syncResult);
                    });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private static void AsyncCallbackJjie2(SyncResultModel<Jjie2Model> syncResult)
        {
            List<Jjie2Model> datas = syncResult.datas;
            if (datas.Count > 0)
            {
                #region 同步到数据库
                //using (SQLiteConnection con = new SQLiteConnection(SQLiteHelper.connectionString))
                //{
                SQLiteConnection con = SQLiteHelper.DbConnection;
                using (SQLiteCommand cmd = new SQLiteCommand())
                {
                    con.Open();
                    SQLiteTransaction sqltran = con.BeginTransaction();
                    cmd.Connection = con;
                    cmd.Transaction = sqltran;

                    try
                    {
                        foreach (Jjie2Model item in datas)
                        {
                            cmd.Parameters.Clear();
                            string cmdText = "select count(*) from jjie2 where SID=@SID";
                            SQLiteParameter[] parameters = new SQLiteParameter[1];
                            parameters[0] = new SQLiteParameter("SID", DbType.Int32);
                            parameters[0].Value = item.SID;
                            cmd.CommandText = cmdText.ToString();
                            cmd.Parameters.AddRange(parameters);
                            long result = (long)cmd.ExecuteScalar();

                            parameters = new SQLiteParameter[10];
                            parameters[0] = new SQLiteParameter("SID", DbType.Int32);
                            parameters[0].Value = item.SID;
                            parameters[1] = new SQLiteParameter("clntcode", DbType.String);
                            parameters[1].Value = item.clntcode;
                            parameters[2] = new SQLiteParameter("clntname", DbType.String);
                            parameters[2].Value = item.clntname;
                            parameters[3] = new SQLiteParameter("xlast", DbType.Decimal);
                            parameters[3].Value = item.xlast;
                            parameters[4] = new SQLiteParameter("xdo", DbType.Decimal);
                            parameters[4].Value = item.xdo;
                            parameters[5] = new SQLiteParameter("xpay", DbType.Decimal);
                            parameters[5].Value = item.xpay;
                            parameters[6] = new SQLiteParameter("xjie", DbType.Decimal);
                            parameters[6].Value = item.xjie;
                            parameters[7] = new SQLiteParameter("xtableid", DbType.Int32);
                            parameters[7].Value = item.xtableid;
                            parameters[8] = new SQLiteParameter("xsubid", DbType.Int32);
                            parameters[8].Value = item.xsubid;
                            parameters[9] = new SQLiteParameter("xversion", DbType.Double);
                            parameters[9].Value = item.xversion;

                            if (result == 0)
                            {
                                cmdText = @"INSERT INTO jjie2 (
                                              SID,
                                              clntcode,
                                              clntname,
                                              xlast,
                                              xdo,
                                              xpay,
                                              xjie,
                                              xtableid,
                                              xsubid,
                                              xversion
                                          )
                                          VALUES (
                                              @SID,
                                              @clntcode,
                                              @clntname,
                                              @xlast,
                                              @xdo,
                                              @xpay,
                                              @xjie,
                                              @xtableid,
                                              @xsubid,
                                              @xversion
                                          )";

                                cmd.Parameters.Clear();
                                cmd.CommandText = cmdText.ToString();
                                cmd.Parameters.AddRange(parameters);
                                cmd.ExecuteNonQuery();
                            }
                            else
                            {
                                cmdText = @"UPDATE jjie2
                                           SET clntcode = @clntcode,
                                               clntname = @clntname,
                                               xlast = @xlast,
                                               xdo = @xdo,
                                               xpay = @xpay,
                                               xjie = @xjie,
                                               xtableid = @xtableid,
                                               xsubid = @xsubid,
                                               xversion = @xversion
                                         WHERE SID = @SID";

                                cmd.Parameters.Clear();
                                cmd.CommandText = cmdText;
                                cmd.Parameters.AddRange(parameters);
                                cmd.ExecuteNonQuery();
                            }
                        }

                        sqltran.Commit();
                    }
                    catch (Exception ex)
                    {
                        sqltran.Rollback();
                        throw ex;
                    }
                    finally
                    {
                        con.Close();
                        //wait_Sync.Set();
                    }
                }
                //}
                #endregion
            }
            //wait_Sync.Set();
        }
        #endregion

        #region 预存款结余
        private static void SyncOjie2(string sid, string xls, string usercode)
        {
            StringBuilder cmdText = new StringBuilder();
            cmdText.AppendFormat("select max(xversion) from ojie2");
            object version = SQLiteHelper.ExecuteScalar(SQLiteHelper.connectionString, CommandType.Text, cmdText.ToString());
            string url = GetUrl(sid, "ojie2", xls, usercode, version);
            //SyncResultModel<Ojie2Model> syncResult = HttpClient.GetAasync<Ojie2Model>(url, null);
            //AsyncCallbackOjie2(syncResult);
            try
            {
                HttpClient.GetAasync<Ojie2Model>(url, null, syncResult =>
                {
                    AsyncCallbackOjie2(syncResult);
                });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private static void AsyncCallbackOjie2(SyncResultModel<Ojie2Model> syncResult)
        {
            List<Ojie2Model> datas = syncResult.datas;

            if (datas.Count > 0)
            {
                #region 同步到数据库
                //using (SQLiteConnection con = new SQLiteConnection(SQLiteHelper.connectionString))
                //{
                SQLiteConnection con = SQLiteHelper.DbConnection;
                using (SQLiteCommand cmd = new SQLiteCommand())
                {
                    con.Open();
                    SQLiteTransaction sqltran = con.BeginTransaction();
                    cmd.Connection = con;
                    cmd.Transaction = sqltran;

                    try
                    {
                        foreach (Ojie2Model item in datas)
                        {
                            cmd.Parameters.Clear();
                            string cmdText = "select count(*) from ojie2 where SID=@SID";
                            SQLiteParameter[] parameters = new SQLiteParameter[1];
                            parameters[0] = new SQLiteParameter("SID", DbType.Int32);
                            parameters[0].Value = item.SID;
                            cmd.CommandText = cmdText.ToString();
                            cmd.Parameters.AddRange(parameters);
                            long result = (long)cmd.ExecuteScalar();

                            parameters = new SQLiteParameter[15];
                            parameters[0] = new SQLiteParameter("SID", DbType.Int32);
                            parameters[0].Value = item.SID;
                            parameters[1] = new SQLiteParameter("clntcode", DbType.String);
                            parameters[1].Value = item.clntcode;
                            parameters[2] = new SQLiteParameter("clntname", DbType.String);
                            parameters[2].Value = item.clntname;
                            parameters[3] = new SQLiteParameter("xlast", DbType.Decimal);
                            parameters[3].Value = item.xlast;
                            parameters[4] = new SQLiteParameter("xpay", DbType.Decimal);
                            parameters[4].Value = item.xpay;
                            parameters[5] = new SQLiteParameter("xhk", DbType.Decimal);
                            parameters[5].Value = item.xhk;
                            parameters[6] = new SQLiteParameter("xdo", DbType.Decimal);
                            parameters[6].Value = item.xdo;
                            parameters[7] = new SQLiteParameter("xzhe", DbType.Decimal);
                            parameters[7].Value = item.xzhe;
                            parameters[8] = new SQLiteParameter("xjie", DbType.Decimal);
                            parameters[8].Value = item.xjie;
                            parameters[9] = new SQLiteParameter("xxflast", DbType.Decimal);
                            parameters[9].Value = item.xxflast;
                            parameters[10] = new SQLiteParameter("xxfnow", DbType.Decimal);
                            parameters[10].Value = item.xxfnow;
                            parameters[11] = new SQLiteParameter("xxfjie", DbType.Decimal);
                            parameters[11].Value = item.xxfjie;
                            parameters[12] = new SQLiteParameter("xtableid", DbType.Int32);
                            parameters[12].Value = item.xtableid;
                            parameters[13] = new SQLiteParameter("xsubid", DbType.Int32);
                            parameters[13].Value = item.xsubid;
                            parameters[14] = new SQLiteParameter("xversion", DbType.Double);
                            parameters[14].Value = item.xversion;

                            if (result == 0)
                            {
                                cmdText = @"INSERT INTO ojie2 (
                                              SID,
                                              clntcode,
                                              clntname,
                                              xlast,
                                              xpay,
                                              xhk,
                                              xdo,
                                              xzhe,
                                              xjie,
                                              xxflast,
                                              xxfnow,
                                              xxfjie,
                                              xtableid,
                                              xsubid,
                                              xversion
                                          )
                                          VALUES (
                                              @SID,
                                              @clntcode,
                                              @clntname,
                                              @xlast,
                                              @xpay,
                                              @xhk,
                                              @xdo,
                                              @xzhe,
                                              @xjie,
                                              @xxflast,
                                              @xxfnow,
                                              @xxfjie,
                                              @xtableid,
                                              @xsubid,
                                              @xversion
                                          )";

                                cmd.Parameters.Clear();
                                cmd.CommandText = cmdText.ToString();
                                cmd.Parameters.AddRange(parameters);
                                cmd.ExecuteNonQuery();
                            }
                            else
                            {
                                cmdText = @"UPDATE ojie2
                                           SET clntcode = @clntcode,
                                               clntname = @clntname,
                                               xlast = @xlast,
                                               xpay = @xpay,
                                               xhk = @xhk,
                                               xdo = @xdo,
                                               xzhe = @xzhe,
                                               xjie = @xjie,
                                               xxflast = @xxflast,
                                               xxfnow = @xxfnow,
                                               xxfjie = @xxfjie,
                                               xtableid = @xtableid,
                                               xsubid = @xsubid,
                                               xversion = @xversion
                                         WHERE SID = @SID";

                                cmd.Parameters.Clear();
                                cmd.CommandText = cmdText;
                                cmd.Parameters.AddRange(parameters);
                                cmd.ExecuteNonQuery();
                            }
                        }

                        sqltran.Commit();
                    }
                    catch (Exception ex)
                    {
                        sqltran.Rollback();
                        throw ex;
                    }
                    finally
                    {
                        con.Close();
                        //wait_Sync.Set();
                    }
                }
                //}
                #endregion
            }
            //wait_Sync.Set();
        }
        #endregion

        #region 促销活动
        private static void SyncSale(string sid, string xls, string usercode)
        {
            StringBuilder cmdText = new StringBuilder();
            cmdText.AppendFormat("select max(xversion) from sale");
            object version = SQLiteHelper.ExecuteScalar(SQLiteHelper.connectionString, CommandType.Text, cmdText.ToString());
            string url = GetUrl(sid, "sale", xls, usercode, version);
            //SyncResultModel<SaleModel> syncResult = HttpClient.GetAasync<SaleModel>(url, null);
            //AsyncCallbackSale(syncResult);
            try
            {
                HttpClient.GetAasync<SaleModel>(url, null, syncResult =>
                {
                    AsyncCallbackSale(syncResult);
                });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private static void AsyncCallbackSale(SyncResultModel<SaleModel> syncResult)
        {
            List<SaleModel> datas = syncResult.datas;
            if (datas.Count > 0)
            {
                #region 同步到数据库
                //using (SQLiteConnection con = new SQLiteConnection(SQLiteHelper.connectionString))
                //{
                SQLiteConnection con = SQLiteHelper.DbConnection;
                using (SQLiteCommand cmd = new SQLiteCommand())
                {
                    con.Open();
                    SQLiteTransaction sqltran = con.BeginTransaction();
                    cmd.Connection = con;
                    cmd.Transaction = sqltran;

                    try
                    {
                        foreach (SaleModel item in datas)
                        {
                            cmd.Parameters.Clear();
                            string cmdText = "select count(*) from sale where SID=@SID";
                            SQLiteParameter[] parameters = new SQLiteParameter[1];
                            parameters[0] = new SQLiteParameter("SID", DbType.Int32);
                            parameters[0].Value = item.SID;
                            cmd.CommandText = cmdText.ToString();
                            cmd.Parameters.AddRange(parameters);
                            long result = (long)cmd.ExecuteScalar();

                            parameters = new SQLiteParameter[25];
                            parameters[0] = new SQLiteParameter("SID", DbType.Int32);
                            parameters[0].Value = item.SID;
                            parameters[1] = new SQLiteParameter("xtype", DbType.String);
                            parameters[1].Value = item.xtype;
                            parameters[2] = new SQLiteParameter("xname", DbType.String);
                            parameters[2].Value = item.xname;
                            parameters[3] = new SQLiteParameter("xkind", DbType.String);
                            parameters[3].Value = item.xkind;
                            parameters[4] = new SQLiteParameter("xunit", DbType.String);
                            parameters[4].Value = item.xunit;
                            parameters[5] = new SQLiteParameter("xrule", DbType.String);
                            parameters[5].Value = item.xrule;
                            parameters[6] = new SQLiteParameter("xhalfnum", DbType.String);
                            parameters[6].Value = item.xhalfnum;
                            parameters[7] = new SQLiteParameter("xgood", DbType.Decimal);
                            parameters[7].Value = item.xgood;
                            parameters[8] = new SQLiteParameter("xbys", DbType.Boolean);
                            parameters[8].Value = item.xbys;
                            parameters[9] = new SQLiteParameter("xdict", DbType.String);
                            parameters[9].Value = item.xdict;
                            parameters[10] = new SQLiteParameter("xdictm", DbType.Decimal);
                            parameters[10].Value = item.xdictm;
                            parameters[11] = new SQLiteParameter("xchssend", DbType.String);
                            parameters[11].Value = item.xchssend;
                            parameters[12] = new SQLiteParameter("xchsclss", DbType.String);
                            parameters[12].Value = item.xchsclss;
                            parameters[13] = new SQLiteParameter("xchstype", DbType.String);
                            parameters[13].Value = item.xchstype;
                            parameters[14] = new SQLiteParameter("xtime1", DbType.String);
                            parameters[14].Value = item.xtime1;
                            parameters[15] = new SQLiteParameter("xtime2", DbType.String);
                            parameters[15].Value = item.xtime2;
                            parameters[16] = new SQLiteParameter("xstart", DbType.Boolean);
                            parameters[16].Value = item.xstart;
                            parameters[17] = new SQLiteParameter("xinname", DbType.String);
                            parameters[17].Value = item.xinname;
                            parameters[18] = new SQLiteParameter("xintime", DbType.String);
                            parameters[18].Value = item.xintime;
                            parameters[19] = new SQLiteParameter("xls", DbType.String);
                            parameters[19].Value = item.xls;
                            parameters[20] = new SQLiteParameter("xlsname", DbType.String);
                            parameters[20].Value = item.xlsname;
                            parameters[21] = new SQLiteParameter("xguest", DbType.Boolean);
                            parameters[21].Value = item.xguest;
                            parameters[22] = new SQLiteParameter("xtableid", DbType.Int32);
                            parameters[22].Value = item.xtableid;
                            parameters[23] = new SQLiteParameter("xsubid", DbType.Int32);
                            parameters[23].Value = item.xsubid;
                            parameters[24] = new SQLiteParameter("xversion", DbType.Double);
                            parameters[24].Value = item.xversion;

                            if (result == 0)
                            {
                                cmdText = @"INSERT INTO sale (
                                             SID,
                                             xtype,
                                             xname,
                                             xkind,
                                             xunit,
                                             xrule,
                                             xhalfnum,
                                             xgood,
                                             xbys,
                                             xdict,
                                             xdictm,
                                             xchssend,
                                             xchsclss,
                                             xchstype,
                                             xtime1,
                                             xtime2,
                                             xstart,
                                             xinname,
                                             xintime,
                                             xls,
                                             xlsname,
                                             xguest,
                                             xtableid,
                                             xsubid,
                                             xversion
                                         )
                                         VALUES (
                                             @SID,
                                             @xtype,
                                             @xname,
                                             @xkind,
                                             @xunit,
                                             @xrule,
                                             @xhalfnum,
                                             @xgood,
                                             @xbys,
                                             @xdict,
                                             @xdictm,
                                             @xchssend,
                                             @xchsclss,
                                             @xchstype,
                                             @xtime1,
                                             @xtime2,
                                             @xstart,
                                             @xinname,
                                             @xintime,
                                             @xls,
                                             @xlsname,
                                             @xguest,
                                             @xtableid,
                                             @xsubid,
                                             @xversion
                                         )";

                                cmd.Parameters.Clear();
                                cmd.CommandText = cmdText.ToString();
                                cmd.Parameters.AddRange(parameters);
                                cmd.ExecuteNonQuery();
                            }
                            else
                            {
                                cmdText = @"UPDATE sale
                                           SET xtype = @xtype,
                                               xname = @xname,
                                               xkind = @xkind,
                                               xunit = @xunit,
                                               xrule = @xrule,
                                               xhalfnum = @xhalfnum,
                                               xgood = @xgood,
                                               xbys = @xbys,
                                               xdict = @xdict,
                                               xdictm = @xdictm,
                                               xchssend = @xchssend,
                                               xchsclss = @xchsclss,
                                               xchstype = @xchstype,
                                               xtime1 = @xtime1,
                                               xtime2 = @xtime2,
                                               xstart = @xstart,
                                               xinname = @xinname,
                                               xintime = @xintime,
                                               xls = @xls,
                                               xlsname = @xlsname,
                                               xguest = @xguest,
                                               xtableid = @xtableid,
                                               xsubid = @xsubid,
                                               xversion = @xversion
                                         WHERE SID = @SID";

                                cmd.Parameters.Clear();
                                cmd.CommandText = cmdText;
                                cmd.Parameters.AddRange(parameters);
                                cmd.ExecuteNonQuery();
                            }
                        }

                        string cmdText1 = "INSERT INTO syncInfo (tableName,isRead)VALUES(@tableName,0)";

                        SQLiteParameter[] parameters1 = new SQLiteParameter[1];
                        parameters1[0] = new SQLiteParameter("tableName", DbType.String);
                        parameters1[0].Value = "sale";

                        cmd.Parameters.Clear();
                        cmd.CommandText = cmdText1;
                        cmd.Parameters.AddRange(parameters1);
                        cmd.ExecuteNonQuery();

                        sqltran.Commit();
                    }
                    catch (Exception ex)
                    {
                        sqltran.Rollback();
                        throw ex;
                    }
                    finally
                    {
                        con.Close();
                        //wait_Sync.Set();
                    }
                }
                //}
                #endregion
            }
            //wait_Sync.Set();
        }
        #endregion

        #region 促销活动货品
        private static void SyncSalegood(string sid, string xls, string usercode)
        {
            StringBuilder cmdText = new StringBuilder();
            cmdText.AppendFormat("select max(xversion) from salegood");
            object version = SQLiteHelper.ExecuteScalar(SQLiteHelper.connectionString, CommandType.Text, cmdText.ToString());
            string url = GetUrl(sid, "salegood", xls, usercode, version);
            //SyncResultModel<SalegoodModel> syncResult = HttpClient.GetAasync<SalegoodModel>(url, null);
            //AsyncCallbackSalegood(syncResult);
            try
            {
                HttpClient.GetAasync<SalegoodModel>(url, null, syncResult =>
                {
                    AsyncCallbackSalegood(syncResult);
                });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private static void AsyncCallbackSalegood(SyncResultModel<SalegoodModel> syncResult)
        {
            List<SalegoodModel> datas = syncResult.datas;
            if (datas.Count > 0)
            {
                #region 同步到数据库
                //using (SQLiteConnection con = new SQLiteConnection(SQLiteHelper.connectionString))
                //{
                SQLiteConnection con = SQLiteHelper.DbConnection;
                using (SQLiteCommand cmd = new SQLiteCommand())
                {
                    con.Open();
                    SQLiteTransaction sqltran = con.BeginTransaction();
                    cmd.Connection = con;
                    cmd.Transaction = sqltran;

                    try
                    {
                        foreach (SalegoodModel item in datas)
                        {
                            cmd.Parameters.Clear();
                            string cmdText = "select count(*) from salegood where SID=@SID";
                            SQLiteParameter[] parameters = new SQLiteParameter[1];
                            parameters[0] = new SQLiteParameter("SID", DbType.Int32);
                            parameters[0].Value = item.SID;
                            cmd.CommandText = cmdText.ToString();
                            cmd.Parameters.AddRange(parameters);
                            long result = (long)cmd.ExecuteScalar();

                            parameters = new SQLiteParameter[8];
                            parameters[0] = new SQLiteParameter("SID", DbType.Int32);
                            parameters[0].Value = item.SID;
                            parameters[1] = new SQLiteParameter("xtype", DbType.String);
                            parameters[1].Value = item.xtype;
                            parameters[2] = new SQLiteParameter("xsaleid", DbType.Int32);
                            parameters[2].Value = item.xsaleid;
                            parameters[3] = new SQLiteParameter("xgtype", DbType.String);
                            parameters[3].Value = item.xgtype;
                            parameters[4] = new SQLiteParameter("xgoodid", DbType.Int32);
                            parameters[4].Value = item.xgoodid;
                            parameters[5] = new SQLiteParameter("xtableid", DbType.Int32);
                            parameters[5].Value = item.xtableid;
                            parameters[6] = new SQLiteParameter("xsubid", DbType.Int32);
                            parameters[6].Value = item.xsubid;
                            parameters[7] = new SQLiteParameter("xversion", DbType.Double);
                            parameters[7].Value = item.xversion;

                            if (result == 0)
                            {
                                cmdText = @"INSERT INTO salegood (
                                             SID,
                                             xtype,
                                             xsaleid,
                                             xgtype,
                                             xgoodid,
                                             xtableid,
                                             xsubid,
                                             xversion
                                         )
                                         VALUES (
                                             @SID,
                                             @xtype,
                                             @xsaleid,
                                             @xgtype,
                                             @xgoodid,
                                             @xtableid,
                                             @xsubid,
                                             @xversion
                                         )";

                                cmd.Parameters.Clear();
                                cmd.CommandText = cmdText.ToString();
                                cmd.Parameters.AddRange(parameters);
                                cmd.ExecuteNonQuery();
                            }
                            else
                            {
                                cmdText = @"UPDATE salegood
                                           SET xtype = @xtype,
                                               xsaleid = @xsaleid,
                                               xgtype = @xgtype,
                                               xgoodid = @xgoodid,
                                               xtableid = @xtableid,
                                               xsubid = @xsubid,
                                               xversion = @xversion
                                         WHERE SID = @SID";

                                cmd.Parameters.Clear();
                                cmd.CommandText = cmdText;
                                cmd.Parameters.AddRange(parameters);
                                cmd.ExecuteNonQuery();
                            }
                        }

                        sqltran.Commit();
                    }
                    catch (Exception ex)
                    {
                        sqltran.Rollback();
                        throw ex;
                    }
                    finally
                    {
                        con.Close();
                        //wait_Sync.Set();
                    }
                }
                //}
                #endregion
            }
            //wait_Sync.Set();
        }
        #endregion

        #region 促销赠品
        private static void SyncSalegoodX(string sid, string xls, string usercode)
        {
            StringBuilder cmdText = new StringBuilder();
            cmdText.AppendFormat("select max(xversion) from salegoodX");
            object version = SQLiteHelper.ExecuteScalar(SQLiteHelper.connectionString, CommandType.Text, cmdText.ToString());
            string url = GetUrl(sid, "salegoodX", xls, usercode, version);
            //SyncResultModel<SalegoodXModel> syncResult = HttpClient.GetAasync<SalegoodXModel>(url, null);
            //AsyncCallbackSalegoodX(syncResult);
            try
            {
                HttpClient.GetAasync<SalegoodXModel>(url, null, syncResult =>
                {
                    AsyncCallbackSalegoodX(syncResult);
                });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private static void AsyncCallbackSalegoodX(SyncResultModel<SalegoodXModel> syncResult)
        {
            List<SalegoodXModel> datas = syncResult.datas;
            if (datas.Count > 0)
            {
                #region 同步到数据库
                //using (SQLiteConnection con = new SQLiteConnection(SQLiteHelper.connectionString))
                //{
                SQLiteConnection con = SQLiteHelper.DbConnection;
                using (SQLiteCommand cmd = new SQLiteCommand())
                {
                    con.Open();
                    SQLiteTransaction sqltran = con.BeginTransaction();
                    cmd.Connection = con;
                    cmd.Transaction = sqltran;

                    try
                    {
                        foreach (SalegoodXModel item in datas)
                        {
                            cmd.Parameters.Clear();
                            string cmdText = "select count(*) from salegoodX where SID=@SID";
                            SQLiteParameter[] parameters = new SQLiteParameter[1];
                            parameters[0] = new SQLiteParameter("SID", DbType.Int32);
                            parameters[0].Value = item.SID;
                            cmd.CommandText = cmdText.ToString();
                            cmd.Parameters.AddRange(parameters);
                            long result = (long)cmd.ExecuteScalar();

                            parameters = new SQLiteParameter[9];
                            parameters[0] = new SQLiteParameter("SID", DbType.Int32);
                            parameters[0].Value = item.SID;
                            parameters[1] = new SQLiteParameter("xtype", DbType.String);
                            parameters[1].Value = item.xtype;
                            parameters[2] = new SQLiteParameter("xsaleid", DbType.Int32);
                            parameters[2].Value = item.xsaleid;
                            parameters[3] = new SQLiteParameter("xgtype", DbType.String);
                            parameters[3].Value = item.xgtype;
                            parameters[4] = new SQLiteParameter("xgoodid", DbType.Int32);
                            parameters[4].Value = item.xgoodid;
                            parameters[5] = new SQLiteParameter("xno", DbType.Int32);
                            parameters[5].Value = item.xno;
                            parameters[6] = new SQLiteParameter("xtableid", DbType.Int32);
                            parameters[6].Value = item.xtableid;
                            parameters[7] = new SQLiteParameter("xsubid", DbType.Int32);
                            parameters[7].Value = item.xsubid;
                            parameters[8] = new SQLiteParameter("xversion", DbType.Double);
                            parameters[8].Value = item.xversion;

                            if (result == 0)
                            {
                                cmdText = @"INSERT INTO salegoodX (
                                              SID,
                                              xtype,
                                              xsaleid,
                                              xgtype,
                                              xgoodid,
                                              xno,
                                              xtableid,
                                              xsubid,
                                              xversion
                                          )
                                          VALUES (
                                              @SID,
                                              @xtype,
                                              @xsaleid,
                                              @xgtype,
                                              @xgoodid,
                                              @xno,
                                              @xtableid,
                                              @xsubid,
                                              @xversion
                                          )";

                                cmd.Parameters.Clear();
                                cmd.CommandText = cmdText.ToString();
                                cmd.Parameters.AddRange(parameters);
                                cmd.ExecuteNonQuery();
                            }
                            else
                            {
                                cmdText = @"UPDATE salegoodX
                                           SET xtype = @xtype,
                                               xsaleid = @xsaleid,
                                               xgtype = @xgtype,
                                               xgoodid = @xgoodid,
                                               xno = @xno,
                                               xtableid = @xtableid,
                                               xsubid = @xsubid,
                                               xversion = @xversion
                                         WHERE SID = @SID";

                                cmd.Parameters.Clear();
                                cmd.CommandText = cmdText;
                                cmd.Parameters.AddRange(parameters);
                                cmd.ExecuteNonQuery();
                            }
                        }

                        sqltran.Commit();
                    }
                    catch (Exception ex)
                    {
                        sqltran.Rollback();
                        throw ex;
                    }
                    finally
                    {
                        con.Close();
                        //wait_Sync.Set();
                    }
                }
                // }
                #endregion
            }
            //wait_Sync.Set();
        }
        #endregion

        #region 活动规则
        private static void SyncSalerule(string sid, string xls, string usercode)
        {
            StringBuilder cmdText = new StringBuilder();
            cmdText.AppendFormat("select max(xversion) from salerule");
            object version = SQLiteHelper.ExecuteScalar(SQLiteHelper.connectionString, CommandType.Text, cmdText.ToString());
            string url = GetUrl(sid, "salerule", xls, usercode, version);
            //SyncResultModel<SaleruleModel> syncResult = HttpClient.GetAasync<SaleruleModel>(url, null);
            //AsyncCallbackSalerule(syncResult);
            try
            {
                HttpClient.GetAasync<SaleruleModel>(url, null, syncResult =>
                {
                    AsyncCallbackSalerule(syncResult);
                });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private static void AsyncCallbackSalerule(SyncResultModel<SaleruleModel> syncResult)
        {
            List<SaleruleModel> datas = syncResult.datas;
            if (datas.Count > 0)
            {
                #region 同步到数据库
                //using (SQLiteConnection con = new SQLiteConnection(SQLiteHelper.connectionString))
                //{
                SQLiteConnection con = SQLiteHelper.DbConnection;
                using (SQLiteCommand cmd = new SQLiteCommand())
                {
                    con.Open();
                    SQLiteTransaction sqltran = con.BeginTransaction();
                    cmd.Connection = con;
                    cmd.Transaction = sqltran;

                    try
                    {
                        foreach (SaleruleModel item in datas)
                        {
                            cmd.Parameters.Clear();
                            string cmdText = "select count(*) from salerule where SID=@SID";
                            SQLiteParameter[] parameters = new SQLiteParameter[1];
                            parameters[0] = new SQLiteParameter("SID", DbType.Int32);
                            parameters[0].Value = item.SID;
                            cmd.CommandText = cmdText.ToString();
                            cmd.Parameters.AddRange(parameters);
                            long result = (long)cmd.ExecuteScalar();

                            parameters = new SQLiteParameter[7];
                            parameters[0] = new SQLiteParameter("SID", DbType.Int32);
                            parameters[0].Value = item.SID;
                            parameters[1] = new SQLiteParameter("xhave", DbType.Decimal);
                            parameters[1].Value = item.xhave;
                            parameters[2] = new SQLiteParameter("xdo", DbType.Decimal);
                            parameters[2].Value = item.xdo;
                            parameters[3] = new SQLiteParameter("xfen", DbType.Decimal);
                            parameters[3].Value = item.xfen;
                            parameters[4] = new SQLiteParameter("xtableid", DbType.Int32);
                            parameters[4].Value = item.xtableid;
                            parameters[5] = new SQLiteParameter("xsubid", DbType.Int32);
                            parameters[5].Value = item.xsubid;
                            parameters[6] = new SQLiteParameter("xversion", DbType.Double);
                            parameters[6].Value = item.xversion;

                            if (result == 0)
                            {
                                cmdText = @"INSERT INTO salerule (
                                             SID,
                                             xhave,
                                             xdo,
                                             xfen,
                                             xtableid,
                                             xsubid,
                                             xversion
                                         )
                                         VALUES (
                                             @SID,
                                             @xhave,
                                             @xdo,
                                             @xfen,
                                             @xtableid,
                                             @xsubid,
                                             @xversion
                                         )";

                                cmd.Parameters.Clear();
                                cmd.CommandText = cmdText.ToString();
                                cmd.Parameters.AddRange(parameters);
                                cmd.ExecuteNonQuery();
                            }
                            else
                            {
                                cmdText = @"UPDATE salerule
                                           SET xhave = @xhave,
                                               xdo = @xdo,
                                               xfen = @xfen,
                                               xtableid = @xtableid,
                                               xsubid = @xsubid,
                                               xversion = @xversion
                                         WHERE SID = @SID";

                                cmd.Parameters.Clear();
                                cmd.CommandText = cmdText;
                                cmd.Parameters.AddRange(parameters);
                                cmd.ExecuteNonQuery();
                            }
                        }

                        sqltran.Commit();
                    }
                    catch (Exception ex)
                    {
                        sqltran.Rollback();
                        throw ex;
                    }
                    finally
                    {
                        con.Close();
                        //wait_Sync.Set();
                    }
                }
                //}
                #endregion
            }
            //wait_Sync.Set();
        }
        #endregion

        #region 班次
        private static void SyncPosbantype(string sid, string xls, string usercode)
        {
            StringBuilder cmdText = new StringBuilder();
            cmdText.AppendFormat("select max(xversion) from posbantype");
            object version = SQLiteHelper.ExecuteScalar(SQLiteHelper.connectionString, CommandType.Text, cmdText.ToString());
            string url = GetUrl(sid, "posbantype", xls, usercode, version);
            //SyncResultModel<PosbantypeModel> syncResult = HttpClient.GetAasync<PosbantypeModel>(url, null);
            //AsyncCallbackPosbantype(syncResult);
            try
            {
                HttpClient.GetAasync<PosbantypeModel>(url, null, syncResult =>
                {
                    AsyncCallbackPosbantype(syncResult);
                });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private static void AsyncCallbackPosbantype(SyncResultModel<PosbantypeModel> syncResult)
        {
            List<PosbantypeModel> datas = syncResult.datas;
            if (datas.Count > 0)
            {
                #region 同步到数据库
                //using (SQLiteConnection con = new SQLiteConnection(SQLiteHelper.connectionString))
                //{
                SQLiteConnection con = SQLiteHelper.DbConnection;
                using (SQLiteCommand cmd = new SQLiteCommand())
                {
                    con.Open();
                    SQLiteTransaction sqltran = con.BeginTransaction();
                    cmd.Connection = con;
                    cmd.Transaction = sqltran;

                    try
                    {
                        foreach (PosbantypeModel item in datas)
                        {
                            cmd.Parameters.Clear();
                            string cmdText = "select count(*) from posbantype where SID=@SID";
                            SQLiteParameter[] parameters = new SQLiteParameter[1];
                            parameters[0] = new SQLiteParameter("SID", DbType.Int32);
                            parameters[0].Value = item.SID;
                            cmd.CommandText = cmdText.ToString();
                            cmd.Parameters.AddRange(parameters);
                            long result = (long)cmd.ExecuteScalar();

                            parameters = new SQLiteParameter[8];
                            parameters[0] = new SQLiteParameter("SID", DbType.Int32);
                            parameters[0].Value = item.SID;
                            parameters[1] = new SQLiteParameter("posbcode", DbType.String);
                            parameters[1].Value = item.posbcode;
                            parameters[2] = new SQLiteParameter("xtime1", DbType.String);
                            parameters[2].Value = item.xtime1;
                            parameters[3] = new SQLiteParameter("xtime2", DbType.String);
                            parameters[3].Value = item.xtime2;
                            parameters[4] = new SQLiteParameter("xnote", DbType.String);
                            parameters[4].Value = item.xnote;
                            parameters[5] = new SQLiteParameter("xtableid", DbType.Int32);
                            parameters[5].Value = item.xtableid;
                            parameters[6] = new SQLiteParameter("xsubid", DbType.Int32);
                            parameters[6].Value = item.xsubid;
                            parameters[7] = new SQLiteParameter("xversion", DbType.Double);
                            parameters[7].Value = item.xversion;

                            if (result == 0)
                            {
                                cmdText = @"INSERT INTO posbantype (
                                               SID,
                                               posbcode,
                                               xtime1,
                                               xtime2,
                                               xnote,
                                               xtableid,
                                               xsubid,
                                               xversion
                                           )
                                           VALUES (
                                               @SID,
                                               @posbcode,
                                               @xtime1,
                                               @xtime2,
                                               @xnote,
                                               @xtableid,
                                               @xsubid,
                                               @xversion
                                           )";

                                cmd.Parameters.Clear();
                                cmd.CommandText = cmdText.ToString();
                                cmd.Parameters.AddRange(parameters);
                                cmd.ExecuteNonQuery();
                            }
                            else
                            {
                                cmdText = @"UPDATE posbantype
                                           SET posbcode = @posbcode,
                                               xtime1 = @xtime1,
                                               xtime2 = @xtime2,
                                               xnote = @xnote,
                                               xtableid = @xtableid,
                                               xsubid = @xsubid,
                                               xversion = @xversion
                                         WHERE SID = @SID";

                                cmd.Parameters.Clear();
                                cmd.CommandText = cmdText;
                                cmd.Parameters.AddRange(parameters);
                                cmd.ExecuteNonQuery();
                            }
                        }

                        sqltran.Commit();
                    }
                    catch (Exception ex)
                    {
                        sqltran.Rollback();
                        throw ex;
                    }
                    finally
                    {
                        con.Close();
                        //wait_Sync.Set();
                    }
                }
                //}
                #endregion
            }
            //wait_Sync.Set();
        }
        #endregion

        #region POS单据表头
        static double maxVersion = 0;
        static List<Guid> pohhIDs = new List<Guid>();
        private static void SyncPoshh(string sid, string xls, string usercode)
        {
            Dictionary<string, string> stateDic = EnumHelper.GetEnumDictionary(typeof(PosState));
            //上传
            StringBuilder cmdText = new StringBuilder();
            cmdText.AppendLine("select xpvalue from possetting where xpname=@xpname");
            SQLiteParameter[] parameters = new SQLiteParameter[1];
            parameters[0] = new SQLiteParameter("xpname", DbType.String);
            parameters[0].Value = AppConst.SyncPOS_MaxVersion;
            object version = SQLiteHelper.ExecuteScalar(SQLiteHelper.connectionString, CommandType.Text, cmdText.ToString(), parameters);

            cmdText = new StringBuilder();
            parameters = new SQLiteParameter[1];
            parameters[0] = new SQLiteParameter("xstate", DbType.String);
            parameters[0].Value = stateDic[Enum.GetName(typeof(PosState), PosState.Additional)];
            cmdText.AppendLine("select * from poshh where SID is null and xstate !=@xstate");
            if (version != null && version.ToString() != string.Empty)
            {
                cmdText.AppendLine("or (xversion>@xversion and xstate !=@xstate)");
                parameters = new SQLiteParameter[2];
                parameters[0] = new SQLiteParameter("xversion", DbType.Double);
                parameters[0].Value = version == null ? 0 : double.Parse(version.ToString());
                parameters[1] = new SQLiteParameter("xstate", DbType.String);
                parameters[1].Value = stateDic[Enum.GetName(typeof(PosState), PosState.Additional)];
            }

            SQLiteDataReader dataReader = SQLiteHelper.ExecuteReader(SQLiteHelper.connectionString, CommandType.Text, cmdText.ToString(), parameters);

            List<PoshhModel> poshhs = new List<PoshhModel>();
            while (dataReader.Read())
            {
                PoshhModel poshh = new PoshhModel();
                poshh.SID = (dataReader["SID"] == null || string.IsNullOrEmpty(dataReader["SID"].ToString())) ? (int?)null : int.Parse(dataReader["SID"].ToString().Trim());
                poshh.ID = new Guid(dataReader["ID"].ToString().Trim());
                poshh.billno = dataReader["billno"].ToString().Trim();
                poshh.posnono = dataReader["posnono"].ToString().Trim();
                poshh.xtype = dataReader["xtype"].ToString().Trim();
                poshh.xstate = dataReader["xstate"].ToString().Trim();
                poshh.paytype = dataReader["paytype"].ToString().Trim();
                poshh.clntcode = dataReader["clntcode"].ToString().Trim();
                poshh.clntname = dataReader["clntname"].ToString().Trim();
                poshh.xheallp = decimal.Parse(dataReader["xheallp"].ToString().Trim());
                poshh.xpay = decimal.Parse(dataReader["xpay"].ToString().Trim());
                poshh.xhezhe = decimal.Parse(dataReader["xhezhe"].ToString().Trim());
                poshh.xhenojie = decimal.Parse(dataReader["xhenojie"].ToString().Trim());
                poshh.xnote = dataReader["xnote"].ToString().Trim();
                poshh.xls = dataReader["xls"].ToString().Trim();
                poshh.xlsname = dataReader["xlsname"].ToString().Trim();
                poshh.pbillno = dataReader["pbillno"].ToString().Trim();
                poshh.xintime = dataReader["xintime"].ToString();
                poshh.xinname = dataReader["xinname"].ToString();
                poshh.xdate = Convert.ToDateTime(dataReader["xdate"].ToString()).Date;
                poshh.xpoints = (dataReader["xpoints"] == null || string.IsNullOrEmpty(dataReader["xpoints"].ToString())) ? (decimal?)null : decimal.Parse(dataReader["xpoints"].ToString());
                poshh.xrpay = (dataReader["xrpay"] == null || string.IsNullOrEmpty(dataReader["xrpay"].ToString())) ? 0 : decimal.Parse(dataReader["xrpay"].ToString());
                poshh.xsendjf = (dataReader["xsendjf"] == null || string.IsNullOrEmpty(dataReader["xsendjf"].ToString())) ? (int?)null : int.Parse(dataReader["xsendjf"].ToString());
                poshh.xhenojie = (dataReader["xsendjf"] == null || string.IsNullOrEmpty(dataReader["xhenojie"].ToString())) ? 0 : decimal.Parse(dataReader["xhenojie"].ToString());
                poshh.xtableid = (dataReader["xtableid"] == null || string.IsNullOrEmpty(dataReader["xtableid"].ToString())) ? (int?)null : int.Parse(dataReader["xtableid"].ToString());
                poshh.xversion = double.Parse(dataReader["xversion"].ToString());
                if (!pohhIDs.Contains(poshh.ID))
                {
                    pohhIDs.Add(poshh.ID);
                    poshhs.Add(poshh);
                }
            }
            dataReader.Close();
            if (poshhs.Count > 0)
            {
                maxVersion = poshhs.Max(r => r.xversion);
            }
            foreach (var item in poshhs)
            {
                //if (item.xstate == stateDic[Enum.GetName(typeof(PosState), PosState.Invalid)])
                //{
                //    continue;
                //}
                parameters = new SQLiteParameter[1];
                parameters[0] = new SQLiteParameter("ID", DbType.String);
                parameters[0].Value = item.ID;

                #region 表体
                cmdText = new StringBuilder();
                cmdText.AppendLine("select * from posbb where XID = @ID");

                SQLiteDataReader dataReader_detail = SQLiteHelper.ExecuteReader(SQLiteHelper.connectionString, CommandType.Text, cmdText.ToString(), parameters);

                List<PosbbModel> posbbs = new List<PosbbModel>();
                while (dataReader_detail.Read())
                {
                    PosbbModel posbb = new PosbbModel();
                    posbb.SID = (dataReader_detail["SID"] == null || string.IsNullOrEmpty(dataReader_detail["SID"].ToString())) ? (int?)null : int.Parse(dataReader_detail["SID"].ToString().Trim());
                    posbb.ID = new Guid(dataReader_detail["ID"].ToString().Trim());
                    posbb.goodcode = dataReader_detail["goodcode"].ToString().Trim();
                    posbb.goodname = dataReader_detail["goodname"].ToString().Trim();
                    posbb.goodunit = dataReader_detail["goodunit"].ToString().Trim();
                    posbb.unitname = dataReader_detail["unitname"].ToString().Trim();
                    posbb.unitrate = (dataReader_detail["unitrate"] != null && dataReader_detail["unitrate"].ToString().Trim() != string.Empty)
                        ? decimal.Parse(dataReader_detail["unitrate"].ToString().Trim())
                        : (decimal?)null;
                    posbb.unitquat = (dataReader_detail["unitquat"] != null && dataReader_detail["unitquat"].ToString().Trim() != string.Empty)
                        ? decimal.Parse(dataReader_detail["unitquat"].ToString().Trim())
                        : 0;
                    posbb.goodkind1 = dataReader_detail["goodkind1"].ToString().Trim();
                    posbb.goodkind2 = dataReader_detail["goodkind2"].ToString().Trim();
                    posbb.goodkind3 = dataReader_detail["goodkind3"].ToString().Trim();
                    posbb.goodkind4 = dataReader_detail["goodkind4"].ToString().Trim();
                    posbb.goodkind5 = dataReader_detail["goodkind5"].ToString().Trim();
                    posbb.goodkind6 = dataReader_detail["goodkind6"].ToString().Trim();
                    posbb.goodkind7 = dataReader_detail["goodkind7"].ToString().Trim();
                    posbb.goodkind8 = dataReader_detail["goodkind8"].ToString().Trim();
                    posbb.goodkind9 = dataReader_detail["goodkind9"].ToString().Trim();
                    posbb.goodkind10 = dataReader_detail["goodkind10"].ToString().Trim();
                    posbb.cnkucode = dataReader_detail["cnkucode"].ToString().Trim();
                    posbb.cnkuname = dataReader_detail["cnkuname"].ToString().Trim();
                    posbb.xquat = decimal.Parse(dataReader_detail["xquat"].ToString().Trim());
                    posbb.xtquat = (dataReader_detail["xtquat"] != null && dataReader_detail["xtquat"].ToString().Trim() != string.Empty)
                        ? decimal.Parse(dataReader_detail["xtquat"].ToString().Trim())
                        : (decimal?)null;
                    //posbb.xpricold = decimal.Parse(dataReader_detail["xpricold"].ToString().Trim());
                    posbb.xzhe = decimal.Parse(dataReader_detail["xzhe"].ToString().Trim());
                    // posbb.xpric = decimal.Parse(dataReader_detail["xpric"].ToString().Trim());
                    posbb.xallp = decimal.Parse(dataReader_detail["xallp"].ToString().Trim());
                    posbb.xtaxr = decimal.Parse(dataReader_detail["xtaxr"].ToString().Trim());
                    posbb.xtax = decimal.Parse(dataReader_detail["xtax"].ToString().Trim());
                    //posbb.xprict = decimal.Parse(dataReader_detail["xprict"].ToString().Trim());
                    posbb.xallpt = decimal.Parse(dataReader_detail["xallpt"].ToString().Trim());
                    if (posbb.unitrate.HasValue && posbb.unitrate.Value > 0)
                    {
                        decimal xpricold = decimal.Parse(dataReader_detail["xpricold"].ToString().Trim());
                        decimal xpric = decimal.Parse(dataReader_detail["xpric"].ToString().Trim());
                        decimal xprict = decimal.Parse(dataReader_detail["xprict"].ToString().Trim());
                        posbb.xpricold = xpricold / posbb.unitrate.Value;
                        posbb.xpric = xpric / posbb.unitrate.Value;
                        posbb.xprict = xprict / posbb.unitrate.Value;
                    }
                    else
                    {
                        posbb.xpricold = decimal.Parse(dataReader_detail["xpricold"].ToString().Trim());
                        posbb.xpric = decimal.Parse(dataReader_detail["xpric"].ToString().Trim());
                        posbb.xprict = decimal.Parse(dataReader_detail["xprict"].ToString().Trim());
                    }
                    posbb.xsalesid = (dataReader_detail["xsalesid"] == null || string.IsNullOrEmpty(dataReader_detail["xsalesid"].ToString())) ? (int?)null : int.Parse(dataReader_detail["xsalesid"].ToString());
                    posbb.xsalestype = dataReader_detail["xsalestype"].ToString();
                    posbb.xpoints = (dataReader_detail["xpoints"] == null || string.IsNullOrEmpty(dataReader_detail["xpoints"].ToString())) ? (decimal?)null : decimal.Parse(dataReader_detail["xpoints"].ToString());
                    posbb.xtableid = (dataReader_detail["xtableid"] == null || string.IsNullOrEmpty(dataReader_detail["xtableid"].ToString())) ? (int?)null : int.Parse(dataReader_detail["xtableid"].ToString());
                    posbb.xversion = double.Parse(dataReader_detail["xversion"].ToString());
                    posbbs.Add(posbb);
                }
                dataReader_detail.Close();
                item.Posbbs = posbbs;
                #endregion

                #region 支付明细
                cmdText = new StringBuilder();
                cmdText.AppendLine("select * from billpayt where XID = @ID");

                SQLiteDataReader dataReader_pay = SQLiteHelper.ExecuteReader(SQLiteHelper.connectionString, CommandType.Text, cmdText.ToString(), parameters);

                List<BillpaytModel> billpayts = new List<BillpaytModel>();
                while (dataReader_pay.Read())
                {
                    BillpaytModel payt = new BillpaytModel();
                    payt.SID = (dataReader_pay["SID"] == null || string.IsNullOrEmpty(dataReader_pay["SID"].ToString())) ? (int?)null : int.Parse(dataReader_pay["SID"].ToString().Trim());
                    payt.ID = new Guid(dataReader_pay["ID"].ToString().Trim());
                    payt.paytcode = dataReader_pay["paytcode"].ToString().Trim();
                    payt.paytname = dataReader_pay["paytname"].ToString().Trim();
                    payt.xpay = decimal.Parse(dataReader_pay["xpay"].ToString().Trim());
                    payt.xnote1 = dataReader_pay["xnote1"].ToString().Trim();
                    payt.xnote2 = dataReader_pay["xnote2"].ToString().Trim();
                    payt.billflag = dataReader_pay["billflag"].ToString().Trim();
                    payt.xtableid = (dataReader_pay["xtableid"] == null || string.IsNullOrEmpty(dataReader_pay["xtableid"].ToString())) ? (int?)null : int.Parse(dataReader_pay["xtableid"].ToString());
                    payt.xversion = double.Parse(dataReader_pay["xversion"].ToString());
                    billpayts.Add(payt);
                }
                dataReader_pay.Close();
                item.payts = billpayts;
                #endregion
            }
            if (poshhs.Count > 0)
            {
                var query = from p in poshhs
                            select new
                            {
                                ID = p.ID,
                                SID = p.SID,
                                billno = p.billno,
                                posnono = p.posnono,
                                xtype = p.xtype,
                                xstate = p.xstate,
                                xdate = p.xdate.ToString("yyyy-MM-dd"),
                                //paytype = p.paytype,
                                clntcode = p.clntcode,
                                clntname = p.clntname,
                                xheallp = p.xheallp,
                                xpay = p.xpay,
                                xhezhe = p.xhezhe,
                                xnote = p.xnote,
                                xls = p.xls,
                                xlsname = p.xlsname,
                                pbillno = p.pbillno,
                                xintime = p.xintime,
                                xinname = p.xinname,
                                xrpay = p.xrpay,
                                xpoints = p.xpoints,
                                xsendjf = p.xsendjf,
                                xhenojie = p.xhenojie,
                                xversion = p.xversion,
                                xtableid = p.xtableid,
                                posbbs = p.Posbbs != null ? from d in p.Posbbs
                                                            select new
                                                            {
                                                                ID = d.ID,
                                                                SID = d.SID,
                                                                goodcode = d.goodcode,
                                                                goodname = d.goodname,
                                                                goodunit = d.goodunit,
                                                                unitname = d.unitname,
                                                                unitrate = d.unitrate,
                                                                unitquat = d.unitquat,
                                                                goodkind1 = d.goodkind1,
                                                                goodkind2 = d.goodkind2,
                                                                goodkind3 = d.goodkind3,
                                                                goodkind4 = d.goodkind4,
                                                                goodkind5 = d.goodkind5,
                                                                goodkind6 = d.goodkind6,
                                                                goodkind7 = d.goodkind7,
                                                                goodkind8 = d.goodkind8,
                                                                goodkind9 = d.goodkind9,
                                                                goodkind10 = d.goodkind10,
                                                                cnkucode = d.cnkucode,
                                                                cnkuname = d.cnkuname,
                                                                xquat = d.xquat,
                                                                xtquat = d.xtquat,
                                                                xpricold = d.xpricold,
                                                                xzhe = d.xzhe,
                                                                xpric = d.xpric,
                                                                xallp = d.xallp,
                                                                xpoints = d.xpoints,
                                                                xtaxr = d.xtaxr,
                                                                xtax = d.xtax,
                                                                xprict = d.xprict,
                                                                xallpt = d.xallpt,
                                                                xsalestype = d.xsalestype,
                                                                xsalesid = d.xsalesid,
                                                                xversion = p.xversion,
                                                                xtableid = d.xtableid,
                                                            } : null,
                                payts = p.payts != null ? from d in p.payts
                                                          select new
                                                          {
                                                              ID = d.ID,
                                                              SID = d.SID,
                                                              paytcode = d.paytcode,
                                                              paytname = d.paytname,
                                                              xpay = d.xpay,
                                                              xnote1 = d.xnote1,
                                                              xnote2 = d.xnote2,
                                                              billflag = d.billflag,
                                                              xtableid = d.xtableid,
                                                              xversion = p.xversion,
                                                          } : null
                            };

                string json = js.Serialize(query);
                string postData = "sid=" + sid;
                postData += "&mod=" + "poshh";
                postData += "&xls=" + xls;
                postData += "&usercode=" + usercode;
                postData += "&data=" + HttpUtility.UrlEncode(json, Encoding.UTF8);

                try
                {
                    SyncResultModel<PoshhModel> syncResult = HttpClient.PostAasync<PoshhModel>(baseUrl_Push.ToString(), null, postData);
                    AsyncCallbackPoshh_UP(syncResult);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }
        private static void AsyncCallbackPoshh_UP(SyncResultModel<PoshhModel> syncResult)
        {
            List<PoshhModel> datas = syncResult.datas;
            if (datas.Count > 0)
            {
                #region 同步到数据库
                //using (SQLiteConnection con = new SQLiteConnection(SQLiteHelper.connectionString))
                //{
                SQLiteConnection con = SQLiteHelper.DbConnection;
                using (SQLiteCommand cmd = new SQLiteCommand())
                {
                    con.Open();
                    SQLiteTransaction sqltran = con.BeginTransaction();
                    cmd.Connection = con;
                    cmd.Transaction = sqltran;

                    try
                    {
                        SQLiteParameter[] parameters = null;

                        foreach (PoshhModel item in datas)
                        {
                            parameters = new SQLiteParameter[3];
                            parameters[0] = new SQLiteParameter("SID", DbType.Int32);
                            parameters[0].Value = item.SID;
                            parameters[1] = new SQLiteParameter("ID", DbType.String);
                            parameters[1].Value = item.ID;
                            parameters[2] = new SQLiteParameter("xtableid", DbType.Int32);
                            parameters[2].Value = item.xtableid;

                            string sql = @"UPDATE poshh
                                               SET SID = @SID,
                                                   xtableid=@xtableid
                                             WHERE ID = @ID";

                            cmd.Parameters.Clear();
                            cmd.CommandText = sql;
                            cmd.Parameters.AddRange(parameters);
                            cmd.ExecuteNonQuery();

                            if (item.Posbbs != null)
                            {
                                foreach (PosbbModel p in item.Posbbs)
                                {
                                    parameters = new SQLiteParameter[3];
                                    parameters[0] = new SQLiteParameter("SID", DbType.Int32);
                                    parameters[0].Value = p.SID;
                                    parameters[1] = new SQLiteParameter("ID", DbType.String);
                                    parameters[1].Value = item.ID;
                                    parameters[2] = new SQLiteParameter("xtableid", DbType.Int32);
                                    parameters[2].Value = p.xtableid;

                                    sql = @"UPDATE posbb
                                               SET SID = @SID,
                                                    xtableid=@xtableid
                                             WHERE XID = @ID";

                                    cmd.Parameters.Clear();
                                    cmd.CommandText = sql;
                                    cmd.Parameters.AddRange(parameters);
                                    cmd.ExecuteNonQuery();
                                }
                            }

                            if (item.payts != null)
                            {
                                foreach (BillpaytModel b in item.payts)
                                {
                                    parameters = new SQLiteParameter[3];
                                    parameters[0] = new SQLiteParameter("SID", DbType.Int32);
                                    parameters[0].Value = b.SID;
                                    parameters[1] = new SQLiteParameter("ID", DbType.String);
                                    parameters[1].Value = item.ID;
                                    parameters[2] = new SQLiteParameter("xtableid", DbType.Int32);
                                    parameters[2].Value = b.xtableid;
                                    sql = @"UPDATE billpayt
                                               SET SID = @SID,
                                                   xtableid=@xtableid
                                             WHERE XID = @ID";

                                    cmd.Parameters.Clear();
                                    cmd.CommandText = sql;
                                    cmd.Parameters.AddRange(parameters);
                                    cmd.ExecuteNonQuery();
                                }
                            }
                        }
                        string cmdText = "select count(*) from possetting where xpname=@xpname";
                        parameters = new SQLiteParameter[1];
                        parameters[0] = new SQLiteParameter("xpname", DbType.String);
                        parameters[0].Value = AppConst.SyncPOS_MaxVersion;
                        cmd.Parameters.Clear();
                        cmd.CommandText = cmdText;
                        cmd.Parameters.AddRange(parameters);
                        long result = (long)cmd.ExecuteScalar();

                        SQLiteParameter[] parameters_Add = new SQLiteParameter[4];
                        parameters_Add[0] = new SQLiteParameter("issys", DbType.Boolean);
                        parameters_Add[0].Value = false;
                        parameters_Add[1] = new SQLiteParameter("xpname", DbType.String);
                        parameters_Add[1].Value = AppConst.SyncPOS_MaxVersion;
                        parameters_Add[2] = new SQLiteParameter("xpvalue", DbType.String);
                        parameters_Add[2].Value = maxVersion;
                        parameters_Add[3] = new SQLiteParameter("xversion", DbType.Double);
                        parameters_Add[3].Value = GetTimeStamp();
                        if (result == 0)
                        {
                            string cmdText_Add = @"INSERT INTO possetting (
                                               issys,
                                               xpname,
                                               xpvalue,
                                               xversion
                                           )
                                           VALUES (
                                               @issys,
                                               @xpname,
                                               @xpvalue,
                                               @xversion
                                           )";

                            cmd.CommandText = cmdText_Add;
                            cmd.Parameters.AddRange(parameters_Add);
                            cmd.ExecuteNonQuery();
                        }
                        else
                        {
                            cmdText = "update possetting set xpvalue=@xpvalue where xpname=@xpname";
                            parameters = new SQLiteParameter[2];
                            parameters[0] = new SQLiteParameter("xpvalue", DbType.String);
                            parameters[0].Value = maxVersion;
                            parameters[1] = new SQLiteParameter("xpname", DbType.String);
                            parameters[1].Value = AppConst.SyncPOS_MaxVersion;
                            cmd.Parameters.Clear();
                            cmd.CommandText = cmdText.ToString();
                            cmd.Parameters.AddRange(parameters);
                            cmd.ExecuteNonQuery();
                        }
                        sqltran.Commit();
                    }
                    catch (Exception ex)
                    {
                        pohhIDs.Clear();
                        sqltran.Rollback();
                        throw ex;
                    }
                    finally
                    {
                        foreach (PoshhModel item in datas)
                        {
                            if (pohhIDs.Contains(item.ID))
                            {
                                pohhIDs.Remove(item.ID);
                            }
                        }
                        con.Close();
                        //wait_Sync.Set();
                    }
                }
                //}
                #endregion
            }
            //wait_Sync.Set();
        }
        #endregion

        #region POS单据表体
        private static void SyncPosbb(string sid, string xls, string usercode)
        {
            //上传
            StringBuilder cmdText = new StringBuilder();
            cmdText.AppendLine("select * from posbb where SID is null");

            SQLiteDataReader dataReader = SQLiteHelper.ExecuteReader(SQLiteHelper.connectionString, CommandType.Text, cmdText.ToString());

            List<PosbbModel> posbbs = new List<PosbbModel>();
            while (dataReader.Read())
            {
                PosbbModel posbb = new PosbbModel();
                posbb.ID = new Guid(dataReader["ID"].ToString().Trim());
                posbb.goodcode = dataReader["goodcode"].ToString().Trim();
                posbb.goodname = dataReader["goodname"].ToString().Trim();
                posbb.goodunit = dataReader["goodunit"].ToString().Trim();
                posbb.unitname = dataReader["unitname"].ToString().Trim();
                posbb.unitrate = (dataReader["unitrate"] != null && dataReader["unitrate"].ToString().Trim() != string.Empty)
                    ? decimal.Parse(dataReader["unitrate"].ToString().Trim())
                    : (decimal?)null;
                posbb.unitquat = (dataReader["unitquat"] != null && dataReader["unitquat"].ToString().Trim() != string.Empty)
                    ? decimal.Parse(dataReader["unitquat"].ToString().Trim())
                    : 0;
                posbb.goodkind1 = dataReader["goodkind1"].ToString().Trim();
                posbb.goodkind2 = dataReader["goodkind2"].ToString().Trim();
                posbb.goodkind3 = dataReader["goodkind3"].ToString().Trim();
                posbb.goodkind4 = dataReader["goodkind4"].ToString().Trim();
                posbb.goodkind5 = dataReader["goodkind5"].ToString().Trim();
                posbb.goodkind6 = dataReader["goodkind6"].ToString().Trim();
                posbb.goodkind7 = dataReader["goodkind7"].ToString().Trim();
                posbb.goodkind8 = dataReader["goodkind8"].ToString().Trim();
                posbb.goodkind9 = dataReader["goodkind9"].ToString().Trim();
                posbb.goodkind10 = dataReader["goodkind10"].ToString().Trim();
                posbb.cnkucode = dataReader["cnkucode"].ToString().Trim();
                posbb.cnkuname = dataReader["cnkuname"].ToString().Trim();
                posbb.xquat = decimal.Parse(dataReader["xquat"].ToString().Trim());
                posbb.xtquat = (dataReader["xtquat"] != null && dataReader["xtquat"].ToString().Trim() != string.Empty)
                    ? decimal.Parse(dataReader["xtquat"].ToString().Trim())
                    : (decimal?)null;
                posbb.xpricold = decimal.Parse(dataReader["xpricold"].ToString().Trim());
                posbb.xzhe = decimal.Parse(dataReader["xzhe"].ToString().Trim());
                posbb.xpric = decimal.Parse(dataReader["xpric"].ToString().Trim());
                posbb.xallp = decimal.Parse(dataReader["xallp"].ToString().Trim());
                posbbs.Add(posbb);
            }
            dataReader.Close();
            if (posbbs.Count > 0)
            {
                var query = from p in posbbs
                            select new
                            {
                                ID = p.ID,
                                goodcode = p.goodcode,
                                goodname = p.goodname,
                                goodunit = p.goodunit,
                                unitname = p.unitname,
                                unitrate = p.unitrate,
                                unitquat = p.unitquat,
                                goodkind1 = p.goodkind1,
                                goodkind2 = p.goodkind2,
                                goodkind3 = p.goodkind3,
                                goodkind4 = p.goodkind4,
                                goodkind5 = p.goodkind5,
                                goodkind6 = p.goodkind6,
                                goodkind7 = p.goodkind7,
                                goodkind8 = p.goodkind8,
                                goodkind9 = p.goodkind9,
                                goodkind10 = p.goodkind10,
                                cnkucode = p.cnkucode,
                                cnkuname = p.cnkuname,
                                xquat = p.xquat,
                                xtquat = p.xtquat,
                                xpricold = p.xpricold,
                                xzhe = p.xzhe,
                                xpric = p.xpric,
                                xallp = p.xallp
                            };
                string json = js.Serialize(query);
                string postData = "sid=" + sid;
                postData += "&mod=" + "posbb";
                postData += "&xls=" + xls;
                postData += "&usercode=" + usercode;
                postData += "&data=" + json;
                byte[] byteArray = Encoding.UTF8.GetBytes(postData);
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(baseUrl_Push);
                request.Method = "POST";
                request.ContentType = "application/x-www-form-urlencoded";
                Stream newStream = request.GetRequestStream();
                newStream.Write(byteArray, 0, byteArray.Length);
                newStream.Close();
                request.BeginGetResponse(AsyncCallbackPosbb_UP, request);
            }
        }
        private static void AsyncCallbackPosbb_UP(IAsyncResult ar)
        {
            List<PosbbModel> datas = new List<PosbbModel>();
            WebRequest request = ar.AsyncState as WebRequest;
            var response = request.EndGetResponse(ar);
            var stream = response.GetResponseStream();
            using (StreamReader sr = new StreamReader(stream, Encoding.UTF8))
            {
                string result = sr.ReadToEnd();
                SyncResultModel<PosbbModel> data = js.Deserialize<SyncResultModel<PosbbModel>>(result);
                datas = data.datas;
            }
            response.Close();
            if (datas.Count > 0)
            {
                #region 同步到数据库
                //using (SQLiteConnection con = new SQLiteConnection(SQLiteHelper.connectionString))
                //{
                SQLiteConnection con = SQLiteHelper.DbConnection;
                using (SQLiteCommand cmd = new SQLiteCommand())
                {
                    con.Open();
                    SQLiteTransaction sqltran = con.BeginTransaction();
                    cmd.Connection = con;
                    cmd.Transaction = sqltran;

                    try
                    {
                        foreach (PosbbModel item in datas)
                        {
                            SQLiteParameter[] parameters = new SQLiteParameter[2];
                            parameters[0] = new SQLiteParameter("SID", DbType.Int32);
                            parameters[0].Value = item.SID;
                            parameters[1] = new SQLiteParameter("ID", DbType.String);
                            parameters[1].Value = item.ID;

                            string sql = @"UPDATE posbb
                                               SET SID = @SID
                                             WHERE ID = @ID";

                            cmd.Parameters.Clear();
                            cmd.CommandText = sql;
                            cmd.Parameters.AddRange(parameters);
                            cmd.ExecuteNonQuery();
                        }
                        sqltran.Commit();
                    }
                    catch (Exception ex)
                    {
                        sqltran.Rollback();
                        throw ex;
                    }
                    finally
                    {
                        con.Close();
                        //wait_Sync.Set();
                    }
                }
                //}
                #endregion
            }
            //wait_Sync.Set();
        }
        #endregion

        #region 当班记录
        private static void SyncPosban(string sid, string xls, string usercode)
        {
            //上传
            StringBuilder cmdText = new StringBuilder();
            cmdText.AppendLine("select * from posban where SID is null and xjieok=1");

            SQLiteDataReader dataReader = SQLiteHelper.ExecuteReader(SQLiteHelper.connectionString, CommandType.Text, cmdText.ToString());

            List<PosbanModel> posbans = new List<PosbanModel>();
            while (dataReader.Read())
            {
                PosbanModel posban = new PosbanModel();
                posban.ID = new Guid(dataReader["ID"].ToString().Trim());
                posban.xposset = dataReader["xposset"].ToString().Trim();
                posban.posnono = dataReader["posnono"].ToString().Trim();
                posban.posposi = dataReader["posposi"].ToString().Trim();
                posban.posbcode = dataReader["posbcode"].ToString().Trim();
                posban.xopcode = dataReader["xopcode"].ToString().Trim();
                posban.xopname = dataReader["xopname"].ToString().Trim();
                posban.xtime1 = dataReader["xtime1"].ToString().Trim();
                posban.xtime2 = dataReader["xtime2"].ToString().Trim();
                posban.xjielst = decimal.Parse(dataReader["xjielst"].ToString().Trim());
                posban.xjiepos = decimal.Parse(dataReader["xjiepos"].ToString().Trim());
                posban.xjienow = decimal.Parse(dataReader["xjienow"].ToString().Trim());
                posban.xjiehav = decimal.Parse(dataReader["xjiehav"].ToString().Trim());
                posban.xjieget = decimal.Parse(dataReader["xjieget"].ToString().Trim());
                posban.xjieok = bool.Parse(dataReader["xjieok"].ToString().Trim());
                posbans.Add(posban);
            }
            dataReader.Close();
            if (posbans.Count > 0)
            {
                var query = from p in posbans
                            select new
                            {
                                ID = p.ID,
                                xposset = p.xposset,
                                posnono = p.posnono,
                                posposi = p.posposi,
                                posbcode = p.posbcode,
                                xopcode = p.xopcode,
                                xopname = p.xopname,
                                xtime1 = p.xtime1,
                                xtime2 = p.xtime2,
                                xjielst = p.xjielst,
                                xjiepos = p.xjiepos,
                                xjienow = p.xjienow,
                                xjiehav = p.xjiehav,
                                xjieget = p.xjieget,
                                xjieok = p.xjieok
                            };
                string json = js.Serialize(query);
                string postData = "sid=" + sid;
                postData += "&mod=" + "posban";
                postData += "&xls=" + xls;
                postData += "&usercode=" + usercode;
                postData += "&data=" + HttpUtility.UrlEncode(json, Encoding.UTF8);
                byte[] byteArray = Encoding.UTF8.GetBytes(postData);

                try
                {
                    SyncResultModel<PosbanModel> syncResult = HttpClient.PostAasync<PosbanModel>(baseUrl_Push.ToString(), null, postData);
                    AsyncCallbackPosban_UP(syncResult);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }
        private static void AsyncCallbackPosban_UP(SyncResultModel<PosbanModel> syncResult)
        {
            List<PosbanModel> datas = syncResult.datas;
            if (datas.Count > 0)
            {
                #region 同步到数据库
                //using (SQLiteConnection con = new SQLiteConnection(SQLiteHelper.connectionString))
                //{
                SQLiteConnection con = SQLiteHelper.DbConnection;
                using (SQLiteCommand cmd = new SQLiteCommand())
                {
                    con.Open();
                    SQLiteTransaction sqltran = con.BeginTransaction();
                    cmd.Connection = con;
                    cmd.Transaction = sqltran;

                    try
                    {
                        foreach (PosbanModel item in datas)
                        {
                            SQLiteParameter[] parameters = new SQLiteParameter[2];
                            parameters[0] = new SQLiteParameter("SID", DbType.Int32);
                            parameters[0].Value = item.SID;
                            parameters[1] = new SQLiteParameter("ID", DbType.String);
                            parameters[1].Value = item.ID;

                            string sql = @"UPDATE posban
                                               SET SID = @SID
                                             WHERE ID = @ID";

                            cmd.Parameters.Clear();
                            cmd.CommandText = sql;
                            cmd.Parameters.AddRange(parameters);
                            cmd.ExecuteNonQuery();
                        }
                        sqltran.Commit();
                    }
                    catch (Exception ex)
                    {
                        sqltran.Rollback();
                        throw ex;
                    }
                    finally
                    {
                        con.Close();
                        //wait_Sync.Set();
                    }
                }
                //}
                #endregion
            }
            //wait_Sync.Set();
        }
        #endregion

        #region POS设置
        private static void SyncPossetting(string sid, string xls, string usercode)
        {
            StringBuilder cmdText = new StringBuilder();
            cmdText.AppendFormat("select max(xversion) from possetting where issys=1");
            object version = SQLiteHelper.ExecuteScalar(SQLiteHelper.connectionString, CommandType.Text, cmdText.ToString());
            string url = GetUrl(sid, "possetting", xls, usercode, version);
            //SyncResultModel<PossettingModel> syncResult = HttpClient.GetAasync<PossettingModel>(url, null);
            //AsyncCallbackPossetting(syncResult);
            try
            {
                HttpClient.GetAasync<PossettingModel>(url, null, syncResult =>
                {
                    AsyncCallbackPossetting(syncResult);
                });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private static void AsyncCallbackPossetting(SyncResultModel<PossettingModel> syncResult)
        {
            List<PossettingModel> datas = syncResult.datas;
            if (datas.Count > 0)
            {
                #region 同步到数据库
                //using (SQLiteConnection con = new SQLiteConnection(SQLiteHelper.connectionString))
                //{
                SQLiteConnection con = SQLiteHelper.DbConnection;
                using (SQLiteCommand cmd = new SQLiteCommand())
                {
                    con.Open();
                    SQLiteTransaction sqltran = con.BeginTransaction();
                    cmd.Connection = con;
                    cmd.Transaction = sqltran;

                    try
                    {
                        foreach (PossettingModel item in datas)
                        {
                            cmd.Parameters.Clear();
                            string cmdText = "select count(*) from possetting where SID=@SID or xpname=@xpname";
                            SQLiteParameter[] parameters = new SQLiteParameter[2];
                            parameters[0] = new SQLiteParameter("SID", DbType.Int32);
                            parameters[0].Value = item.SID;
                            parameters[1] = new SQLiteParameter("xpname", DbType.String);
                            parameters[1].Value = item.xpname;
                            cmd.CommandText = cmdText.ToString();
                            cmd.Parameters.AddRange(parameters);
                            long result = (long)cmd.ExecuteScalar();

                            parameters = new SQLiteParameter[8];
                            parameters[0] = new SQLiteParameter("SID", DbType.Int32);
                            parameters[0].Value = item.SID;
                            parameters[1] = new SQLiteParameter("issys", DbType.Boolean);
                            parameters[1].Value = item.issys;
                            parameters[2] = new SQLiteParameter("xpname", DbType.String);
                            parameters[2].Value = item.xpname;
                            parameters[3] = new SQLiteParameter("xpvalue", DbType.String);
                            parameters[3].Value = item.xpvalue;
                            parameters[4] = new SQLiteParameter("usercode", DbType.String);
                            parameters[4].Value = item.usercode;
                            parameters[5] = new SQLiteParameter("xtableid", DbType.Int32);
                            parameters[5].Value = item.xtableid;
                            parameters[6] = new SQLiteParameter("xsubid", DbType.Int32);
                            parameters[6].Value = item.xsubid;
                            parameters[7] = new SQLiteParameter("xversion", DbType.Double);
                            parameters[7].Value = item.xversion;

                            if (result == 0)
                            {
                                cmdText = @"INSERT INTO possetting (
                                               SID,
                                               issys,
                                               xpname,
                                               xpvalue,
                                               usercode,
                                               xtableid,
                                               xsubid,
                                               xversion
                                           )
                                           VALUES (
                                               @SID,
                                               @issys,
                                               @xpname,
                                               @xpvalue,
                                               @usercode,
                                               @xtableid,
                                               @xsubid,
                                               @xversion
                                           )";

                                cmd.Parameters.Clear();
                                cmd.CommandText = cmdText.ToString();
                                cmd.Parameters.AddRange(parameters);
                                cmd.ExecuteNonQuery();
                            }
                            else
                            {
                                cmdText = @"UPDATE possetting
                                           SET issys = @issys,
                                               xpname = @xpname,
                                               xpvalue = @xpvalue,
                                               usercode = @usercode,
                                               xtableid = @xtableid,
                                               xsubid = @xsubid,
                                               xversion = @xversion
                                         WHERE SID = @SID or xpname=@xpname";

                                cmd.Parameters.Clear();
                                cmd.CommandText = cmdText;
                                cmd.Parameters.AddRange(parameters);
                                cmd.ExecuteNonQuery();
                            }
                        }

                        sqltran.Commit();
                    }
                    catch (Exception ex)
                    {
                        sqltran.Rollback();
                        throw ex;
                    }
                    finally
                    {
                        con.Close();
                        //wait_Sync.Set();
                    }
                }
                //}
                #endregion
            }
            //wait_Sync.Set();
        }
        #endregion

        #region 线下优惠券
        private static void SyncTickoff(string sid, string xls, string usercode)
        {
            StringBuilder cmdText = new StringBuilder();
            cmdText.AppendFormat("select max(xversion) from tickoff");
            object version = SQLiteHelper.ExecuteScalar(SQLiteHelper.connectionString, CommandType.Text, cmdText.ToString());
            string url = GetUrl(sid, "tickoff", xls, usercode, version);
            //SyncResultModel<TickoffModel> syncResult = HttpClient.GetAasync<TickoffModel>(url, null);
            //AsyncCallbackTickoff(syncResult);
            try
            {
                HttpClient.GetAasync<TickoffModel>(url, null, syncResult =>
                {
                    AsyncCallbackTickoff(syncResult);
                });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private static void AsyncCallbackTickoff(SyncResultModel<TickoffModel> syncResult)
        {
            List<TickoffModel> datas = syncResult.datas;
            if (datas.Count > 0)
            {
                #region 同步到数据库
                //using (SQLiteConnection con = new SQLiteConnection(SQLiteHelper.connectionString))
                //{
                SQLiteConnection con = SQLiteHelper.DbConnection;
                using (SQLiteCommand cmd = new SQLiteCommand())
                {
                    con.Open();
                    SQLiteTransaction sqltran = con.BeginTransaction();
                    cmd.Connection = con;
                    cmd.Transaction = sqltran;

                    try
                    {
                        foreach (TickoffModel item in datas)
                        {
                            cmd.Parameters.Clear();
                            string cmdText = "select count(*) from tickoff where SID=@SID";
                            SQLiteParameter[] parameters = new SQLiteParameter[1];
                            parameters[0] = new SQLiteParameter("SID", DbType.Int32);
                            parameters[0].Value = item.SID;
                            cmd.CommandText = cmdText.ToString();
                            cmd.Parameters.AddRange(parameters);
                            long result = (long)cmd.ExecuteScalar();

                            parameters = new SQLiteParameter[20];
                            parameters[0] = new SQLiteParameter("SID", DbType.Int32);
                            parameters[0].Value = item.SID;
                            parameters[1] = new SQLiteParameter("xname", DbType.String);
                            parameters[1].Value = item.xname;
                            parameters[2] = new SQLiteParameter("xallp", DbType.Decimal);
                            parameters[2].Value = item.xallp;
                            parameters[3] = new SQLiteParameter("xcount", DbType.Int32);
                            parameters[3].Value = item.xcount;
                            parameters[4] = new SQLiteParameter("xnotime", DbType.Boolean);
                            parameters[4].Value = item.xnotime;
                            parameters[5] = new SQLiteParameter("xtime1", DbType.String);
                            parameters[5].Value = item.xtime1;
                            parameters[6] = new SQLiteParameter("xtime2", DbType.String);
                            parameters[6].Value = item.xtime2;
                            parameters[7] = new SQLiteParameter("xafter", DbType.Int32);
                            parameters[7].Value = item.xafter;
                            parameters[8] = new SQLiteParameter("xend", DbType.String);
                            parameters[8].Value = item.xend;
                            parameters[9] = new SQLiteParameter("goodtype", DbType.String);
                            parameters[9].Value = item.goodtype;
                            parameters[10] = new SQLiteParameter("goodbank", DbType.String);
                            parameters[10].Value = item.goodbank;
                            parameters[11] = new SQLiteParameter("xminallp", DbType.Decimal);
                            parameters[11].Value = item.xminallp;
                            parameters[12] = new SQLiteParameter("xinname", DbType.String);
                            parameters[12].Value = item.xinname;
                            parameters[13] = new SQLiteParameter("xintime", DbType.String);
                            parameters[13].Value = item.xintime;
                            parameters[14] = new SQLiteParameter("xnote", DbType.String);
                            parameters[14].Value = item.xnote;
                            parameters[15] = new SQLiteParameter("xls", DbType.String);
                            parameters[15].Value = item.xls;
                            parameters[16] = new SQLiteParameter("xlsname", DbType.String);
                            parameters[16].Value = item.xlsname;
                            parameters[17] = new SQLiteParameter("xtableid", DbType.Int32);
                            parameters[17].Value = item.xtableid;
                            parameters[18] = new SQLiteParameter("xsubid", DbType.Int32);
                            parameters[18].Value = item.xsubid;
                            parameters[19] = new SQLiteParameter("xversion", DbType.Double);
                            parameters[19].Value = item.xversion;

                            if (result == 0)
                            {
                                cmdText = @"INSERT INTO tickoff (
                                            SID,
                                            xname,
                                            xallp,
                                            xcount,
                                            xnotime,
                                            xtime1,
                                            xtime2,
                                            xafter,
                                            xend,
                                            goodtype,
                                            goodbank,
                                            xminallp,
                                            xinname,
                                            xintime,
                                            xnote,
                                            xls,
                                            xlsname,
                                            xtableid,
                                            xsubid,
                                            xversion
                                        )
                                        VALUES (
                                            @SID,
                                            @xname,
                                            @xallp,
                                            @xcount,
                                            @xnotime,
                                            @xtime1,
                                            @xtime2,
                                            @xafter,
                                            @xend,
                                            @goodtype,
                                            @goodbank,
                                            @xminallp,
                                            @xinname,
                                            @xintime,
                                            @xnote,
                                            @xls,
                                            @xlsname,
                                            @xtableid,
                                            @xsubid,
                                            @xversion
                                        )";

                                cmd.Parameters.Clear();
                                cmd.CommandText = cmdText.ToString();
                                cmd.Parameters.AddRange(parameters);
                                cmd.ExecuteNonQuery();
                            }
                            else
                            {
                                cmdText = @"UPDATE tickoff
                                           SET xname = @xname,
                                               xallp = @xallp,
                                               xcount = @xcount,
                                               xnotime = @xnotime,
                                               xtime1 = @xtime1,
                                               xtime2 = @xtime2,
                                               xafter = @xafter,
                                               xend = @xend,
                                               goodtype = @goodtype,
                                               goodbank = @goodbank,
                                               xminallp = @xminallp,
                                               xinname = @xinname,
                                               xintime = @xintime,
                                               xnote = @xnote,
                                               xls = @xls,
                                               xlsname = @xlsname,
                                               xtableid = @xtableid,
                                               xsubid = @xsubid,
                                               xversion = @xversion
                                         WHERE SID = @SID";

                                cmd.Parameters.Clear();
                                cmd.CommandText = cmdText;
                                cmd.Parameters.AddRange(parameters);
                                cmd.ExecuteNonQuery();
                            }
                        }

                        sqltran.Commit();
                    }
                    catch (Exception ex)
                    {
                        sqltran.Rollback();
                        throw ex;
                    }
                    finally
                    {
                        con.Close();
                        //wait_Sync.Set();
                    }
                }
                //}
                #endregion
            }
            //wait_Sync.Set();
        }
        #endregion

        #region 线下优惠券明细
        private static void SyncTickoffmx(string sid, string xls, string usercode)
        {
            StringBuilder cmdText = new StringBuilder();
            cmdText.AppendFormat("select max(xversion) from tickoffmx");
            object version = SQLiteHelper.ExecuteScalar(SQLiteHelper.connectionString, CommandType.Text, cmdText.ToString());
            string url = GetUrl(sid, "tickoffmx", xls, usercode, version);
            //SyncResultModel<TickoffmxModel> syncResult = HttpClient.GetAasync<TickoffmxModel>(url, null);
            //AsyncCallbackTickoffmx(syncResult);

            try
            {
                HttpClient.GetAasync<TickoffmxModel>(url, null, syncResult =>
                {
                    AsyncCallbackTickoffmx(syncResult);
                });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private static void AsyncCallbackTickoffmx(SyncResultModel<TickoffmxModel> syncResult)
        {
            List<TickoffmxModel> datas = syncResult.datas;

            if (datas.Count > 0)
            {
                #region 同步到数据库
                //using (SQLiteConnection con = new SQLiteConnection(SQLiteHelper.connectionString))
                //{
                SQLiteConnection con = SQLiteHelper.DbConnection;
                using (SQLiteCommand cmd = new SQLiteCommand())
                {
                    con.Open();
                    SQLiteTransaction sqltran = con.BeginTransaction();
                    cmd.Connection = con;
                    cmd.Transaction = sqltran;

                    try
                    {
                        foreach (TickoffmxModel item in datas)
                        {
                            cmd.Parameters.Clear();
                            string cmdText = "select count(*) from tickoffmx where SID=@SID";
                            SQLiteParameter[] parameters = new SQLiteParameter[1];
                            parameters[0] = new SQLiteParameter("SID", DbType.Int32);
                            parameters[0].Value = item.SID;
                            cmd.CommandText = cmdText.ToString();
                            cmd.Parameters.AddRange(parameters);
                            long result = (long)cmd.ExecuteScalar();

                            parameters = new SQLiteParameter[20];
                            parameters[0] = new SQLiteParameter("SID", DbType.Int32);
                            parameters[0].Value = item.SID;
                            parameters[1] = new SQLiteParameter("xname", DbType.String);
                            parameters[1].Value = item.xname;
                            parameters[2] = new SQLiteParameter("xallp", DbType.Decimal);
                            parameters[2].Value = item.xallp;
                            parameters[3] = new SQLiteParameter("clntcode", DbType.String);
                            parameters[3].Value = item.clntcode;
                            parameters[4] = new SQLiteParameter("clntname", DbType.String);
                            parameters[4].Value = item.clntname;
                            parameters[5] = new SQLiteParameter("xnotime", DbType.Boolean);
                            parameters[5].Value = item.xnotime;
                            parameters[6] = new SQLiteParameter("xtime1", DbType.String);
                            parameters[6].Value = item.xtime1;
                            parameters[7] = new SQLiteParameter("xtime2", DbType.String);
                            parameters[7].Value = item.xtime2;
                            parameters[8] = new SQLiteParameter("xafter", DbType.Int32);
                            parameters[8].Value = item.xafter;
                            parameters[9] = new SQLiteParameter("xend", DbType.String);
                            parameters[9].Value = item.xend;
                            parameters[10] = new SQLiteParameter("xminallp", DbType.Decimal);
                            parameters[10].Value = item.xminallp;
                            parameters[11] = new SQLiteParameter("xusetime", DbType.String);
                            parameters[11].Value = item.xusetime;
                            parameters[12] = new SQLiteParameter("xinname", DbType.String);
                            parameters[12].Value = item.xinname;
                            parameters[13] = new SQLiteParameter("xintime", DbType.String);
                            parameters[13].Value = item.xintime;
                            parameters[14] = new SQLiteParameter("xstate", DbType.String);
                            parameters[14].Value = item.xstate;
                            parameters[15] = new SQLiteParameter("xopusetime", DbType.String);
                            parameters[15].Value = item.xopusetime;
                            parameters[16] = new SQLiteParameter("xcode", DbType.String);
                            parameters[16].Value = item.xcode;
                            parameters[17] = new SQLiteParameter("xtableid", DbType.Int32);
                            parameters[17].Value = item.xtableid;
                            parameters[18] = new SQLiteParameter("xsubid", DbType.Int32);
                            parameters[18].Value = item.xsubid;
                            parameters[19] = new SQLiteParameter("xversion", DbType.Double);
                            parameters[19].Value = item.xversion;

                            if (result == 0)
                            {
                                cmdText = @"INSERT INTO tickoffmx (
                                              SID,
                                              xcode,
                                              xname,
                                              xallp,
                                              clntcode,
                                              clntname,
                                              xnotime,
                                              xtime1,
                                              xtime2,
                                              xafter,
                                              xend,
                                              xminallp,
                                              xusetime,
                                              xinname,
                                              xintime,
                                              xstate,
                                              xopusetime,
                                              xtableid,
                                              xsubid,
                                              xversion
                                          )
                                          VALUES (
                                              @SID,
                                              @xcode,
                                              @xname,
                                              @xallp,
                                              @clntcode,
                                              @clntname,
                                              @xnotime,
                                              @xtime1,
                                              @xtime2,
                                              @xafter,
                                              @xend,
                                              @xminallp,
                                              @xusetime,
                                              @xinname,
                                              @xintime,
                                              @xstate,
                                              @xopusetime,
                                              @xtableid,
                                              @xsubid,
                                              @xversion
                                            )";

                                cmd.Parameters.Clear();
                                cmd.CommandText = cmdText.ToString();
                                cmd.Parameters.AddRange(parameters);
                                cmd.ExecuteNonQuery();
                            }
                            else
                            {
                                cmdText = @"UPDATE tickoffmx
                                           SET xcode = @xcode,
                                               xname = @xname,
                                               xallp = @xallp,
                                               clntcode = @clntcode,
                                               clntname = @clntname,
                                               xnotime = @xnotime,
                                               xtime1 = @xtime1,
                                               xtime2 = @xtime2,
                                               xafter = @xafter,
                                               xend = @xend,
                                               xminallp = @xminallp,
                                               xusetime = @xusetime,
                                               xinname = @xinname,
                                               xintime = @xintime,
                                               xstate = @xstate,
                                               xopusetime = @xopusetime,
                                               xtableid = @xtableid,
                                               xsubid = @xsubid,
                                               xversion = @xversion
                                         WHERE SID = @SID 
                                        ";

                                cmd.Parameters.Clear();
                                cmd.CommandText = cmdText;
                                cmd.Parameters.AddRange(parameters);
                                cmd.ExecuteNonQuery();
                            }
                        }

                        sqltran.Commit();
                    }
                    catch (Exception ex)
                    {
                        sqltran.Rollback();
                        throw ex;
                    }
                    finally
                    {
                        con.Close();
                        //wait_Sync.Set();
                    }
                }
                //}
                #endregion
            }
            //wait_Sync.Set();
        }
        #endregion

        #region  账户
        private static void SyncPayt(string sid, string xls, string usercode)
        {
            StringBuilder cmdText = new StringBuilder();
            cmdText.AppendFormat("select max(xversion) from payt");
            object version = SQLiteHelper.ExecuteScalar(SQLiteHelper.connectionString, CommandType.Text, cmdText.ToString());
            string url = GetUrl(sid, "payt", xls, usercode, version);
            //SyncResultModel<PaytModel> syncResult = HttpClient.GetAasync<PaytModel>(url, null);
            //AsyncCallbackPayt(syncResult);
            try
            {
                HttpClient.GetAasync<PaytModel>(url, null, syncResult =>
                {
                    AsyncCallbackPayt(syncResult);
                });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private static void AsyncCallbackPayt(SyncResultModel<PaytModel> syncResult)
        {
            List<PaytModel> datas = syncResult.datas;
            if (datas.Count > 0)
            {
                #region 同步到数据库
                //using (SQLiteConnection con = new SQLiteConnection(SQLiteHelper.connectionString))
                //{
                SQLiteConnection con = SQLiteHelper.DbConnection;
                using (SQLiteCommand cmd = new SQLiteCommand())
                {
                    con.Open();
                    SQLiteTransaction sqltran = con.BeginTransaction();
                    cmd.Connection = con;
                    cmd.Transaction = sqltran;

                    try
                    {
                        foreach (PaytModel item in datas)
                        {
                            cmd.Parameters.Clear();
                            string cmdText = "select count(*) from payt where SID=@SID";
                            SQLiteParameter[] parameters = new SQLiteParameter[1];
                            parameters[0] = new SQLiteParameter("SID", DbType.Int32);
                            parameters[0].Value = item.SID;
                            cmd.CommandText = cmdText.ToString();
                            cmd.Parameters.AddRange(parameters);
                            long result = (long)cmd.ExecuteScalar();

                            parameters = new SQLiteParameter[8];
                            parameters[0] = new SQLiteParameter("SID", DbType.Int32);
                            parameters[0].Value = item.SID;
                            parameters[1] = new SQLiteParameter("payttype", DbType.String);
                            parameters[1].Value = item.payttype;
                            parameters[2] = new SQLiteParameter("paytcode", DbType.String);
                            parameters[2].Value = item.paytcode;
                            parameters[3] = new SQLiteParameter("paytname", DbType.String);
                            parameters[3].Value = item.paytname;
                            parameters[4] = new SQLiteParameter("xnote", DbType.String);
                            parameters[4].Value = item.xnote;
                            parameters[5] = new SQLiteParameter("xtableid", DbType.Int32);
                            parameters[5].Value = item.xtableid;
                            parameters[6] = new SQLiteParameter("xsubid", DbType.Int32);
                            parameters[6].Value = item.xsubid;
                            parameters[7] = new SQLiteParameter("xversion", DbType.Double);
                            parameters[7].Value = item.xversion;

                            if (result == 0)
                            {
                                cmdText = @"INSERT INTO payt (
                                             SID,
                                             payttype,
                                             paytcode,
                                             paytname,
                                             xnote,
                                             xtableid,
                                             xsubid,
                                             xversion
                                         )
                                         VALUES (
                                             @SID,
                                             @payttype,
                                             @paytcode,
                                             @paytname,
                                             @xnote,
                                             @xtableid,
                                             @xsubid,
                                             @xversion
                                         )";

                                cmd.Parameters.Clear();
                                cmd.CommandText = cmdText.ToString();
                                cmd.Parameters.AddRange(parameters);
                                cmd.ExecuteNonQuery();
                            }
                            else
                            {
                                cmdText = @"UPDATE payt
                                           SET payttype = @payttype,
                                               paytcode = @paytcode,
                                               paytname = @paytname,
                                               xnote = @xnote,
                                               xtableid = @xtableid,
                                               xsubid = @xsubid,
                                               xversion = @xversion
                                         WHERE SID = @SID";

                                cmd.Parameters.Clear();
                                cmd.CommandText = cmdText;
                                cmd.Parameters.AddRange(parameters);
                                cmd.ExecuteNonQuery();
                            }
                        }

                        sqltran.Commit();
                    }
                    catch (Exception ex)
                    {
                        sqltran.Rollback();
                        throw ex;
                    }
                    finally
                    {
                        con.Close();
                        //wait_Sync.Set();
                    }
                }
                //}
                #endregion
            }
            //wait_Sync.Set();
        }
        #endregion

        #region 获取同步的消息
        public static List<SyncInfoModel> GetSyncInfo()
        {
            string cmdText = "select * from syncInfo where isRead=0";

            try
            {
                SQLiteDataReader dataReader = SQLiteHelper.ExecuteReader(SQLiteHelper.connectionString, CommandType.Text, cmdText.ToString());
                List<SyncInfoModel> syncInfoList = new List<SyncInfoModel>();
                while (dataReader.Read())
                {
                    SyncInfoModel syncInfo = new SyncInfoModel();
                    syncInfo.tableName = dataReader["tableName"].ToString();
                    syncInfo.SID = dataReader["SID"].ToString() == string.Empty ? 0 : int.Parse(dataReader["SID"].ToString());
                    syncInfoList.Add(syncInfo);
                }

                return syncInfoList;
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
            string cmdText = "UPDATE syncInfo SET isRead = 1 WHERE isRead = 0";

            try
            {
                int result = SQLiteHelper.ExecuteNonQuery(SQLiteHelper.connectionString, CommandType.Text, cmdText);

                return result > 0;
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
                DateTime time = DateTime.Now;
                if (CheckConnect(out time))
                {
                    Uri baseUrl_pusherr = new Uri(baseUrl, @"pos/pusherr");
                    string postData = "sid=" + sid;
                    postData += "&xls=" + xls;
                    postData += "&err=" + HttpUtility.UrlEncode(msg, Encoding.UTF8);
                    postData += "&usercode=" + usercode;
                    HttpClient.PostAasync<object>(baseUrl_pusherr.ToString(), null, postData);
                }
            }
            catch (Exception ex)
            {
            }
        }
        #endregion
    }

}
