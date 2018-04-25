using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.IO;
using POS.Common;
using System.Collections;
using POS.Model;

namespace POS.DAL
{
    public class SQLiteHelper
    {
        private static readonly string sqlitePath = Path.Combine(AppConst.sqliteDirectory, AppConst.dbName);
        public static readonly string connectionString = string.Format("Data Source={0}", sqlitePath);

        private static readonly object obj = new object();

        public static SQLiteConnection DbConnection
        {
            get
            {
                try
                {
                    return new SQLiteConnection(connectionString);
                }
                catch (Exception ex)
                {
                    return new SQLiteConnection(connectionString);
                    throw ex;
                }
            }
        }

        /// <summary>
        /// 预备连接
        /// </summary>
        /// <param name="conn">数据库连接对象</param>
        /// <param name="sqlcmd">连接命令对象</param>
        /// <param name="trans">连接的事务</param>
        /// <param name="cmdType">命令类型</param>
        /// <param name="cmdText">命令文本内容</param>
        /// <param name="cmdparam">命令内容带的参数</param>
        public static void preparationConnion(SQLiteConnection conn, SQLiteCommand cmd, SQLiteTransaction trans, CommandType cmdType, string cmdText, SQLiteParameter[] cmdparam)
        {
            if (conn.State != ConnectionState.Open)
            {
                conn.Open();
            }
            cmd.Connection = conn;
            cmd.CommandType = cmdType;
            cmd.CommandText = cmdText;

            if (trans != null)
            {
                cmd.Transaction = trans;
            }

            if (cmdparam != null)
            {
                foreach (SQLiteParameter param in cmdparam)
                {
                    cmd.Parameters.Add(param);
                }
            }

        }


        //SqlDatareader读取数据
        public static SQLiteDataReader ExecuteReader(string connString, CommandType cmdType, string cmdText, params SQLiteParameter[] param)
        {
            Monitor.Enter(obj);

            SQLiteCommand cmd = new SQLiteCommand();
            // SQLiteConnection conn = new SQLiteConnection(connString);
            SQLiteConnection conn = DbConnection;
            try
            {
                preparationConnion(conn, cmd, null, cmdType, cmdText, param);
                SQLiteDataReader reader = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                cmd.Parameters.Clear();
                return reader;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                Monitor.Exit(obj);
            }
        }

        /// <summary>
        /// 返回受影响的行数
        /// </summary>
        /// <param name="connString"></param>
        /// <param name="cmdType"></param>
        /// <param name="cmdText"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public static int ExecuteNonQuery(string connString, CommandType cmdType, string cmdText, params SQLiteParameter[] param)
        {
            Monitor.Enter(obj);

            SQLiteCommand cmd = new SQLiteCommand();
            SQLiteConnection conn = DbConnection;
            try
            {
                //using (conn = new SQLiteConnection(connString))
                //{
                preparationConnion(conn, cmd, null, cmdType, cmdText, param);
                int val = cmd.ExecuteNonQuery(CommandBehavior.CloseConnection);
                cmd.Parameters.Clear();
                return val;
                //}
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                conn.Close();
                Monitor.Exit(obj);
            }
        }

        /// <summary>
        /// 返回首行首列的值
        /// </summary>
        /// <param name="connString"></param>
        /// <param name="cmdType"></param>
        /// <param name="cmdText"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public static object ExecuteScalar(string connString, CommandType cmdType, string cmdText, params SQLiteParameter[] param)
        {
            Monitor.Enter(obj);

            SQLiteCommand cmd = new SQLiteCommand();
            SQLiteConnection conn = DbConnection;
            try
            {
                //using (conn = new SQLiteConnection(connString))
                //{
                preparationConnion(conn, cmd, null, cmdType, cmdText, param);
                object val = cmd.ExecuteScalar();
                cmd.Parameters.Clear();
                return val;
                //}
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                conn.Close();
                Monitor.Exit(obj);
            }
        }

        #region 执行多条SQL语句，实现数据库事务。
        /// <summary>
        /// 执行多条SQL语句，实现数据库事务。
        /// </summary>
        /// <param name="sqlStringList">SQL语句的哈希表（key为sql语句，value是该语句的SqlParameter[]）</param>
        public static bool ExecuteSqlTran(List<SqlExpressionModel> sqlStringList)
        {
            Monitor.Enter(obj);
            SQLiteConnection conn = DbConnection;
            using (SQLiteCommand cmd = conn.CreateCommand())
            {
                conn.Open();
                SQLiteTransaction sqltran = conn.BeginTransaction();
                cmd.Transaction = sqltran;
                try
                {
                    foreach (SqlExpressionModel item in sqlStringList)
                    {
                        string cmdText =item.CmdText;
                        SQLiteParameter[] param = item.Parameters == null ? null : (SQLiteParameter[])item.Parameters;
                        preparationConnion(conn, cmd, null, CommandType.Text, cmdText, param);
                        int val = cmd.ExecuteNonQuery();
                        cmd.Parameters.Clear();
                    }
                    sqltran.Commit();
                    return true;
                }
                catch (Exception ex)
                {
                    sqltran.Rollback();
                    throw ex;
                }
                finally
                {
                    conn.Close();
                    Monitor.Exit(obj);
                }
            }
        }
        #endregion
    }
}
