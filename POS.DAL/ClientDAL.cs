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
    /// 会员逻辑类
    /// </summary>
    public class ClientDAL : BaseDAL
    {
        #region 添加会员
        /// <summary>
        /// 添加会员
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public bool AddClient(ClntModel entity)
        {

            try
            {
                //默认用户等级
                ClntclssModel defaultClntclss = GetDefaultClntclss();

                SQLiteParameter[] parameters = new SQLiteParameter[17];

                StringBuilder cmdText = new StringBuilder();
                cmdText.AppendLine("INSERT INTO clnt(");
                cmdText.AppendLine("ID,clntcode,password,clnttype,clntname,clntclss,xpho,");
                cmdText.AppendLine("xbro,xadd,xnotes,xls,xlsname,xinname,xintime,xversion,SID,xtableid)");
                cmdText.AppendLine("VALUES(");
                cmdText.AppendLine("@ID,@clntcode,@password,@clnttype,@clntname,@clntclss,@xpho,");
                cmdText.AppendLine("@xbro,@xadd,@xnotes,@xls,@xlsname,@xinname,@xintime,@xversion,@SID,@xtableid)");

                parameters[0] = new SQLiteParameter("clntcode", DbType.String);
                parameters[0].Value = entity.clntcode;
                parameters[1] = new SQLiteParameter("password", DbType.String);
                parameters[1].Value = entity.password;
                parameters[2] = new SQLiteParameter("clnttype", DbType.String);
                parameters[2].Value = entity.clnttype;
                parameters[3] = new SQLiteParameter("clntname", DbType.String);
                parameters[3].Value = entity.clntname;
                parameters[4] = new SQLiteParameter("clntclss", DbType.String);
                parameters[4].Value = entity.clntclss;
                parameters[5] = new SQLiteParameter("xpho", DbType.String);
                parameters[5].Value = entity.xpho;
                parameters[6] = new SQLiteParameter("xbro", DbType.DateTime);
                parameters[6].Value = entity.xbro;
                parameters[7] = new SQLiteParameter("xadd", DbType.String);
                parameters[7].Value = entity.xadd;
                parameters[8] = new SQLiteParameter("xnotes", DbType.String);
                parameters[8].Value = entity.xnotes;
                parameters[9] = new SQLiteParameter("xls", DbType.String);
                parameters[9].Value = entity.xls;
                parameters[10] = new SQLiteParameter("xlsname", DbType.String);
                parameters[10].Value = entity.xlsname;
                parameters[11] = new SQLiteParameter("xinname", DbType.String);
                parameters[11].Value = entity.xinname;
                parameters[12] = new SQLiteParameter("xintime", DbType.String);
                parameters[12].Value = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                parameters[13] = new SQLiteParameter("xversion", DbType.Double);
                parameters[13].Value = entity.xversion;
                parameters[14] = new SQLiteParameter("ID", DbType.String);
                parameters[14].Value = entity.ID;
                parameters[15] = new SQLiteParameter("SID", DbType.Int32);
                parameters[15].Value = entity.SID;
                parameters[16] = new SQLiteParameter("xtableid", DbType.Int32);
                parameters[16].Value = entity.xtableid;

                int result = SQLiteHelper.ExecuteNonQuery(SQLiteHelper.connectionString, CommandType.Text, cmdText.ToString(), parameters);

                return result > 0;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region 获取单个会员
        /// <summary>
        /// 获取单个会员
        /// </summary>
        /// <param name="ID"></param>
        /// <returns>会员ID</returns>
        public ClntModel GetClientByID(Guid ID)
        {

            SQLiteParameter[] parameters = new SQLiteParameter[1];
            StringBuilder cmdText = new StringBuilder();
            cmdText.AppendLine("select clnt.ID,clnt.SID,clnt.clnttype,clnt.clntcode,clnt.clntname,clnt.clntclss,");
            cmdText.AppendLine("clnt.xbro,clnt.xintime,clnt.xadd,clnt.xnotes,clnt.xpho,clnt.xcontime,clnt.password,c.xzhe,clnt.xlsname");
            cmdText.AppendLine("from clnt left join clntclss c on clnt.clntclss=c.clntclss");
            cmdText.AppendLine("where ID =@ID");
            parameters[0] = new SQLiteParameter("ID", DbType.String);
            parameters[0].Value = ID;

            SQLiteDataReader dataReader = null;
            try
            {
                dataReader = SQLiteHelper.ExecuteReader(SQLiteHelper.connectionString, CommandType.Text, cmdText.ToString(), parameters);
                ClntModel clnt = null;
                while (dataReader.Read())
                {
                    clnt = new ClntModel();
                    clnt.ID = new Guid(dataReader["ID"].ToString());
                    clnt.SID = dataReader["SID"].ToString() != string.Empty ? int.Parse(dataReader["SID"].ToString()) : 0;
                    clnt.clnttype = dataReader["clnttype"].ToString();
                    clnt.clntcode = dataReader["clntcode"].ToString();
                    clnt.clntname = dataReader["clntname"].ToString();
                    clnt.clntclss = dataReader["clntclss"].ToString();
                    clnt.xbro = dataReader["xbro"].ToString() == string.Empty ? (DateTime?)null : DateTime.Parse(dataReader["xbro"].ToString());
                    clnt.xpho = dataReader["xpho"].ToString();
                    clnt.xintime = dataReader["xintime"].ToString();
                    clnt.xadd = dataReader["xadd"].ToString();
                    clnt.xnotes = dataReader["xnotes"].ToString();
                    clnt.xcontime = (dataReader["xcontime"] == null || dataReader["xcontime"].ToString() == string.Empty) ? 0 : int.Parse(dataReader["xcontime"].ToString());
                    clnt.xzhe = (dataReader["xzhe"] == null || dataReader["xzhe"].ToString() == string.Empty) ? 100 : decimal.Parse(dataReader["xzhe"].ToString());
                    clnt.xlsname = dataReader["xlsname"].ToString();
                }
                return clnt;
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

        #region 获取单个会员
        /// <summary>
        /// 获取单个会员
        /// </summary>
        /// <param name="clntcode"></param>
        /// <returns>会员代码</returns>
        public ClntModel GetClientByCode(string clntcode)
        {

            SQLiteParameter[] parameters = new SQLiteParameter[1];
            StringBuilder cmdText = new StringBuilder();
            cmdText.AppendLine("select clnt.ID,clnt.SID,clnt.clnttype,clnt.clntcode,clnt.clntname,clnt.clntclss,");
            cmdText.AppendLine("clnt.xbro,clnt.xintime,clnt.xadd,clnt.xnotes,clnt.xpho,clnt.password,c.xzhe");
            cmdText.AppendLine("from clnt left join clntclss c on clnt.clntclss=c.clntclss");
            cmdText.AppendLine("where clntcode =@clntcode");
            parameters[0] = new SQLiteParameter("clntcode", DbType.String);
            parameters[0].Value = clntcode;

            SQLiteDataReader dataReader = null;
            try
            {
                dataReader = SQLiteHelper.ExecuteReader(SQLiteHelper.connectionString, CommandType.Text, cmdText.ToString(), parameters);
                ClntModel clnt = null;
                while (dataReader.Read())
                {
                    clnt = new ClntModel();
                    clnt.ID = new Guid(dataReader["ID"].ToString());
                    clnt.SID = dataReader["SID"].ToString() != string.Empty ? int.Parse(dataReader["SID"].ToString()) : (int?)null;
                    clnt.clnttype = dataReader["clnttype"].ToString();
                    clnt.clntcode = dataReader["clntcode"].ToString();
                    clnt.clntname = dataReader["clntname"].ToString();
                    clnt.clntclss = dataReader["clntclss"].ToString();
                    clnt.xbro = dataReader["xbro"].ToString() == string.Empty ? (DateTime?)null : DateTime.Parse(dataReader["xbro"].ToString());
                    clnt.xpho = dataReader["xpho"].ToString();
                    clnt.xzhe = dataReader["xzhe"].ToString() == string.Empty ? 100 : decimal.Parse(dataReader["xzhe"].ToString());
                    clnt.xintime = dataReader["xintime"].ToString();
                    clnt.xadd = dataReader["xadd"].ToString();
                    clnt.xnotes = dataReader["xnotes"].ToString();
                    clnt.password = dataReader["password"].ToString();
                }
                return clnt;
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

        #region 修改会员
        /// <summary>
        /// 修改会员
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public bool UpdateClient(ClntModel entity)
        {

            try
            {
                SQLiteParameter[] parameters = new SQLiteParameter[7];

                string cmdText = @"UPDATE clnt set clntname=@clntname,xpho=@xpho,clntclss=@clntclss,xbro=@xbro,
                                   xadd=@xadd,xnotes=@xnotes,isUpdate=1 where ID=@ID";

                parameters[0] = new SQLiteParameter("clntname", DbType.String);
                parameters[0].Value = entity.clntname;
                parameters[1] = new SQLiteParameter("xpho", DbType.String);
                parameters[1].Value = entity.xpho;
                parameters[2] = new SQLiteParameter("clntclss", DbType.String);
                parameters[2].Value = entity.clntclss;
                parameters[3] = new SQLiteParameter("xbro", DbType.DateTime);
                parameters[3].Value = entity.xbro;
                parameters[4] = new SQLiteParameter("xadd", DbType.String);
                parameters[4].Value = entity.xadd;
                parameters[5] = new SQLiteParameter("xnotes", DbType.String);
                parameters[5].Value = entity.xnotes;
                parameters[6] = new SQLiteParameter("ID", DbType.String);
                parameters[6].Value = entity.ID;

                int result = SQLiteHelper.ExecuteNonQuery(SQLiteHelper.connectionString, CommandType.Text, cmdText, parameters);

                return result > 0;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region 模糊搜索会员
        /// <summary>
        /// 模糊搜索会员
        /// </summary>
        /// <param name="key"></param>
        /// <returns>卡号、手机、会员姓名</returns>
        public List<ClntModel> GetClientByKey(string key)
        {

            SQLiteParameter[] parameters = new SQLiteParameter[1];
            StringBuilder cmdText = new StringBuilder();
            cmdText.AppendLine("select distinct clnt.ID,clnt.clntcode,clnt.clntname,clnt.clntclss,clnt.xpho,clnt.clnttype,");
            //cmdText.AppendLine("(select sum(ifnull(jjie2.xjie,0)) from jjie2 where jjie2.clntcode = clnt.clntcode) as integral,");
            //cmdText.AppendLine("(select sum(ifnull(ojie2.xjie,0)) from ojie2 where ojie2.clntcode = clnt.clntcode) as balance,");
            cmdText.AppendLine("clntclss.xzhe from clnt");
            //cmdText.AppendLine("left join jjie2 on clnt.clntcode = jjie2.clntcode");
            //cmdText.AppendLine("left join ojie2 on clnt.clntcode = ojie2.clntcode");
            cmdText.AppendLine("left join clntclss on clnt.clntclss = clntclss.clntclss");
            cmdText.AppendLine("where lower(clnt.clntcode) like @key or lower(clnt.xpho) like @key or  lower(clnt.clntname) like @key order by clnt.clntcode asc");
            parameters[0] = new SQLiteParameter("@key", DbType.String);
            parameters[0].Value = "%" + key.ToLower() + "%";

            SQLiteDataReader dataReader = null;
            try
            {
                dataReader = SQLiteHelper.ExecuteReader(SQLiteHelper.connectionString, CommandType.Text, cmdText.ToString(), parameters);
                List<ClntModel> clntList = new List<ClntModel>();
                while (dataReader.Read())
                {
                    ClntModel clnt = new ClntModel();
                    clnt.ID = new Guid(dataReader["ID"].ToString());
                    clnt.clntcode = dataReader["clntcode"].ToString();
                    clnt.clntname = dataReader["clntname"].ToString();
                    clnt.clntclss = dataReader["clntclss"].ToString();
                    clnt.clnttype = dataReader["clnttype"].ToString();
                    clnt.xpho = dataReader["xpho"].ToString();
                    //clnt.integral = dataReader["integral"].ToString() == string.Empty ? 0 : decimal.Parse(dataReader["integral"].ToString());
                    //clnt.balance = dataReader["balance"].ToString() == string.Empty ? 0 : decimal.Parse(dataReader["balance"].ToString());
                    clnt.xzhe = dataReader["xzhe"].ToString() == string.Empty ? 100 : decimal.Parse(dataReader["xzhe"].ToString());
                    clntList.Add(clnt);
                }
                //var query = (from p in clntList
                //            group p by new { p.ID,p.clntcode,p.clntname,p.clntclss,p.xpho,p.xzhe} into g
                //            select new ClntModel
                //            {
                //                ID=g.Key.ID,
                //                clntcode=g.Key.clntcode,
                //                clntname=g.Key.clntname,
                //                clntclss=g.Key.clntclss,
                //                xpho=g.Key.xpho,
                //                xzhe=g.Key.xzhe,
                //                integral=g.Sum(r=>r.integral),
                //                balance=g.Sum(r=>r.balance)
                //            }).ToList();
                return clntList.Where(r => r.clnttype == "门店会员").ToList();
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

        #region 会员统计
        /// <summary>
        /// 会员统计
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <param name="xls"></param>
        /// <returns></returns>
        public List<ClntModel> GetClientStatistics(DateTime startTime, DateTime endTime, string xls)
        {

            SQLiteParameter[] parameters = new SQLiteParameter[3];
            StringBuilder cmdText = new StringBuilder();
            cmdText.AppendLine("select distinct clnt.ID,clnt.clntcode,clnt.clntname,clnt.clntclss,clnt.xpho,");
            cmdText.AppendLine("(select sum(ifnull(jjie2.xjie,0)) from jjie2 where jjie2.clntcode = clnt.clntcode) as integral,");
            cmdText.AppendLine("(select sum(ifnull(ojie2.xjie,0)) from ojie2 where ojie2.clntcode = clnt.clntcode) as balance,");
            cmdText.AppendLine("clntclss.xzhe from clnt");
            cmdText.AppendLine("left join jjie2 on clnt.clntcode = jjie2.clntcode");
            cmdText.AppendLine("left join ojie2 on clnt.clntcode = ojie2.clntcode");
            cmdText.AppendLine("left join clntclss on clnt.clntclss = clntclss.clntclss");
            cmdText.AppendLine("where date(xintime) >= @startDate and date(xintime)<=@endDate and xls=@xls order by clnt.clntcode asc");
            parameters[0] = new SQLiteParameter("startDate", DbType.DateTime);
            parameters[0].Value = startTime.Date;
            parameters[1] = new SQLiteParameter("endDate", DbType.DateTime);
            parameters[1].Value = endTime.Date;
            parameters[2] = new SQLiteParameter("xls", DbType.String);
            parameters[2].Value = xls;

            SQLiteDataReader dataReader = null;
            try
            {
                dataReader = SQLiteHelper.ExecuteReader(SQLiteHelper.connectionString, CommandType.Text, cmdText.ToString(), parameters);
                List<ClntModel> clntList = new List<ClntModel>();
                while (dataReader.Read())
                {
                    ClntModel clnt = new ClntModel();
                    clnt.ID = new Guid(dataReader["ID"].ToString());
                    clnt.clntcode = dataReader["clntcode"].ToString();
                    clnt.clntname = dataReader["clntname"].ToString();
                    clnt.clntclss = dataReader["clntclss"].ToString();
                    clnt.xpho = dataReader["xpho"].ToString();
                    clnt.integral = dataReader["integral"].ToString() == string.Empty ? 0 : decimal.Parse(dataReader["integral"].ToString());
                    clnt.balance = dataReader["balance"].ToString() == string.Empty ? 0 : decimal.Parse(dataReader["balance"].ToString());
                    clnt.xzhe = dataReader["xzhe"].ToString() == string.Empty ? 100 : decimal.Parse(dataReader["xzhe"].ToString());
                    clntList.Add(clnt);
                }
                return clntList;
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

        #region 获取默认顾客等级
        /// <summary>
        /// 获取默认顾客等级
        /// </summary>
        /// <returns></returns>
        public ClntclssModel GetDefaultClntclss()
        {
            string cmdText = "select clntclss,xzhe from clntclss where xdefault=1";

            SQLiteDataReader dataReader = null;
            try
            {
                dataReader = SQLiteHelper.ExecuteReader(SQLiteHelper.connectionString, CommandType.Text, cmdText);
                ClntclssModel clntclss = null;
                while (dataReader.Read())
                {
                    clntclss = new ClntclssModel();
                    clntclss.clntclss = dataReader["clntclss"].ToString();
                    clntclss.xzhe = decimal.Parse(dataReader["xzhe"].ToString());
                }
                return clntclss;
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

        #region 获取顾客等级
        /// <summary>
        /// 获取顾客等级
        /// </summary>
        /// <returns></returns>
        public List<ClntclssModel> GetClntclss()
        {
            string cmdText = "select clntclss,xzhe from clntclss";

            SQLiteDataReader dataReader = null;
            try
            {
                dataReader = SQLiteHelper.ExecuteReader(SQLiteHelper.connectionString, CommandType.Text, cmdText);
                List<ClntclssModel> list = new List<ClntclssModel>();

                while (dataReader.Read())
                {
                    ClntclssModel clntclss = new ClntclssModel();
                    clntclss.clntclss = dataReader["clntclss"].ToString();
                    clntclss.xzhe = decimal.Parse(dataReader["xzhe"].ToString());
                    list.Add(clntclss);
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

        #region 获取一个顾客等级
        /// <summary>
        /// 获取一个顾客等级
        /// </summary>
        /// <returns></returns>
        public ClntclssModel GetClntclssByClass(string clntclss)
        {
            string cmdText = "select * from clntclss where clntclss=@clntclss";

            SQLiteParameter[] parameters = new SQLiteParameter[1];
            parameters[0] = new SQLiteParameter("clntclss", DbType.String);
            parameters[0].Value = clntclss;

            SQLiteDataReader dataReader = null;
            try
            {
                dataReader = SQLiteHelper.ExecuteReader(SQLiteHelper.connectionString, CommandType.Text, cmdText, parameters);
                ClntclssModel entity = null;
                while (dataReader.Read())
                {
                    entity = new ClntclssModel();
                    entity.clntclss = dataReader["clntclss"].ToString();
                    entity.xzhe = decimal.Parse(dataReader["xzhe"].ToString());
                    entity.xtableid = (dataReader["xtableid"] == null || string.IsNullOrEmpty(dataReader["xtableid"].ToString())) ? (int?)null : int.Parse(dataReader["xtableid"].ToString());
                }
                return entity;
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

        #region 获取顾客类别
        /// <summary>
        /// 获取顾客类别
        /// </summary>
        /// <returns></returns>
        public List<ClnttypeModel> GetClnttype()
        {
            string cmdText = "select SID,clnttype from clnttype";

            SQLiteDataReader dataReader = null;
            try
            {
                dataReader = SQLiteHelper.ExecuteReader(SQLiteHelper.connectionString, CommandType.Text, cmdText);
                List<ClnttypeModel> list = new List<ClnttypeModel>();
                while (dataReader.Read())
                {
                    ClnttypeModel clnttype = new ClnttypeModel();
                    clnttype.SID = int.Parse(dataReader["SID"].ToString());
                    clnttype.clnttype = dataReader["clnttype"].ToString();
                    list.Add(clnttype);
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

        #region 获取会员积分结余
        /// <summary>
        /// 获取会员积分结余
        /// </summary>
        /// <param name="clintCode"></param>
        /// <returns>会员代码</returns>
        public decimal GetJjie2(string clntcode)
        {

            SQLiteParameter[] parameters = new SQLiteParameter[1];

            string cmdText = "select sum(xjie) from jjie2 where clntcode =@clntcode";
            parameters[0] = new SQLiteParameter("clntcode", DbType.String);
            parameters[0].Value = clntcode;

            try
            {
                object balance = SQLiteHelper.ExecuteScalar(SQLiteHelper.connectionString, CommandType.Text, cmdText, parameters);
                if (balance != null && !string.IsNullOrEmpty(balance.ToString()))
                {
                    return decimal.Parse(balance.ToString());
                }
                else
                {
                    return 0;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region 获取会员预存款结余
        /// <summary>
        /// 获取会员预存款结余
        /// </summary>
        /// <param name="clintCode"></param>
        /// <returns>会员代码</returns>
        public decimal GetOjie2(string clntcode)
        {

            SQLiteParameter[] parameters = new SQLiteParameter[1];

            string cmdText = "select sum(xjie) from ojie2 where clntcode =@clntcode";
            parameters[0] = new SQLiteParameter("clntcode", DbType.String);
            parameters[0].Value = clntcode;

            try
            {
                object balance = SQLiteHelper.ExecuteScalar(SQLiteHelper.connectionString, CommandType.Text, cmdText, parameters);
                if (balance != null && !string.IsNullOrEmpty(balance.ToString()))
                {
                    return decimal.Parse(balance.ToString());
                }
                else
                {
                    return 0;
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region 判断是否存在会员电话号码
        /// <summary>
        /// 判断是否存在会员电话号码
        /// </summary>
        /// <param name="clnt"></param>
        /// <returns></returns>
        public bool ExistClntPhone(string pho, Guid? ID)
        {
            SQLiteParameter[] parameters = null;
            string cmdText = string.Empty;
            if (ID.HasValue)
            {
                parameters = new SQLiteParameter[2];
                parameters[0] = new SQLiteParameter("xpho", DbType.String);
                parameters[0].Value = pho;
                parameters[1] = new SQLiteParameter("ID", DbType.String);
                parameters[1].Value = ID;
                cmdText = "select count(*) from clnt where xpho=@xpho and ID !=@ID";
            }
            else
            {
                parameters = new SQLiteParameter[1];
                parameters[0] = new SQLiteParameter("xpho", DbType.String);
                parameters[0].Value = pho;
                cmdText = "select count(*) from clnt where xpho=@xpho";
            }
            long result = (long)SQLiteHelper.ExecuteScalar(SQLiteHelper.connectionString, CommandType.Text, cmdText.ToString(), parameters);
            return result > 0;
        }
        #endregion

        #region 获取所有会员积分结余
        /// <summary>
        /// 获取所有会员积分结余
        /// </summary>
        /// <param name="clintCode"></param>
        /// <returns>会员代码</returns>
        public List<Jjie2Model> GetJjie2()
        {
            SQLiteParameter[] parameters = new SQLiteParameter[1];

            string cmdText = "select sum(xjie) as xjie,clntcode from jjie2 group by clntcode";

            SQLiteDataReader dataReader = null;
            try
            {
                dataReader = SQLiteHelper.ExecuteReader(SQLiteHelper.connectionString, CommandType.Text, cmdText);
                List<Jjie2Model> jjie2List = new List<Jjie2Model>();
                while (dataReader.Read())
                {
                    Jjie2Model jjie2 = new Jjie2Model();
                    jjie2.clntcode = dataReader["clntcode"].ToString();
                    jjie2.xjie = decimal.Parse(dataReader["xjie"].ToString());
                    jjie2List.Add(jjie2);
                }
                return jjie2List;
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

        #region 获取所有会员预存款结余
        /// <summary>
        /// 获取所有会员预存款结余
        /// </summary>
        /// <param name="clintCode"></param>
        /// <returns>会员代码</returns>
        public List<Ojie2Model> GetOjie2()
        {
            string cmdText = "select sum(xjie)as xjie,clntcode from ojie2 group by clntcode";

            SQLiteDataReader dataReader = null;
            try
            {
                dataReader = SQLiteHelper.ExecuteReader(SQLiteHelper.connectionString, CommandType.Text, cmdText);
                List<Ojie2Model> jjie2List = new List<Ojie2Model>();
                while (dataReader.Read())
                {
                    Ojie2Model ojie2 = new Ojie2Model();
                    ojie2.clntcode = dataReader["clntcode"].ToString();
                    ojie2.xjie = decimal.Parse(dataReader["xjie"].ToString());
                    jjie2List.Add(ojie2);
                }
                return jjie2List;
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
