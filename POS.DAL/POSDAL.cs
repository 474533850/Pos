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
    /// POS单逻辑类
    /// </summary>
    public class POSDAL : BaseDAL
    {
        #region 查询Pos单_分页
        /// <summary>
        /// 查询Pos单_分页
        /// </summary>
        /// <param name="page">当前页数</param>
        /// <param name="pageSize">每页显示多少条</param>
        /// <returns></returns>
        public List<PoshhModel> GetPOS(int page, int pageSize, string key, string goodKey, string clntKey, DateTime? startDate, DateTime? endDate, bool isAll, int uploadstatus, out long totalPage)
        {
            //查询总条数
            SQLiteParameter[] parameters = null;
            Dictionary<string, string> stateDic = EnumHelper.GetEnumDictionary(typeof(PosState));
            List<string> state = new List<string>();
            state.Add(stateDic[Enum.GetName(typeof(PosState), PosState.Deal)]);
            state.Add(stateDic[Enum.GetName(typeof(PosState), PosState.Returned)]);
            state.Add(stateDic[Enum.GetName(typeof(PosState), PosState.Additional)]);
            state.Add(stateDic[Enum.GetName(typeof(PosState), PosState.Change)]);
            state.Add(stateDic[Enum.GetName(typeof(PosState), PosState.Invalid)]);

            StringBuilder sql = new StringBuilder();
            sql.AppendLine("select count(distinct a.ID) from poshh a ");
            if (goodKey != string.Empty)
            {
                sql.AppendLine("inner join posbb b on a.ID = b.XID ");
                sql.AppendLine("inner join good g on b.goodcode = g.goodcode ");
            }
            sql.AppendFormat("where xstate in ('{0}') ", string.Join("','", state));
            if (uploadstatus > 0)
            {
                sql.AppendFormat("and ifnull(uploadstatus,'') ='{0}' ", uploadstatus);
            }
            else if (uploadstatus == 0)
            {
                sql.AppendFormat("and (ifnull(uploadstatus,'') ='{0}' or ifnull(uploadstatus,'') ='{1}') ", uploadstatus, string.Empty);
            }
            if (!isAll)
            {
                sql.AppendLine(" and datetime(xintime) >= @startDate and datetime(xintime)<@endDate ");
            }
            if (key != string.Empty)
            {
                sql.AppendFormat(" and (lower(billno) like '{0}' or lower(clntcode) like '{0}') ", "%" + key.ToLower() + "%");
            }

            if (goodKey != string.Empty)
            {
                sql.AppendFormat(" and (lower(b.goodcode) like '{0}' or lower(g.goodname) like  '{0}' or lower(b.xbarcode) like  '{0}')", "%" + goodKey.ToLower() + "%");
            }
            if (clntKey != string.Empty)
            {
                sql.AppendFormat(" and (lower(a.clntname) like '{0}' or lower(a.clntcode) like  '{0}')", "%" + clntKey.ToLower() + "%");
            }

            if (!isAll)
            {
                parameters = new SQLiteParameter[2];
                parameters[0] = new SQLiteParameter("startDate", DbType.DateTime);
                parameters[0].Value = startDate.Value.Date;
                parameters[1] = new SQLiteParameter("endDate", DbType.DateTime);
                parameters[1].Value = endDate.Value.Date.AddDays(1);
            }

            long totalRow = (long)SQLiteHelper.ExecuteScalar(SQLiteHelper.connectionString, CommandType.Text, sql.ToString(), parameters);
            totalPage = (totalRow % pageSize) == 0 ? (totalRow / pageSize) : (totalRow / pageSize + 1);

            StringBuilder cmdText = new StringBuilder();
            cmdText.AppendFormat("select distinct a.* from poshh a ");
            if (goodKey != string.Empty)
            {
                cmdText.AppendLine("inner join posbb b on a.ID = b.XID ");
            }
            cmdText.AppendFormat("where xstate in ('{0}') ", string.Join("','", state));
            if (uploadstatus > 0)
            {
                cmdText.AppendFormat("and ifnull(uploadstatus,'') ='{0}' ", uploadstatus);
            }
            else if (uploadstatus == 0)
            {
                cmdText.AppendFormat("and (ifnull(uploadstatus,'') ='{0}' or ifnull(uploadstatus,'') ='{1}') ", uploadstatus, string.Empty);
            }
            if (!isAll)
            {
                cmdText.AppendLine(" and datetime(xintime) >= @startDate and datetime(xintime)<@endDate");
            }
            if (key != string.Empty)
            {
                cmdText.AppendFormat(" and (lower(billno) like '{0}' or lower(clntcode) like '{0}')", "%" + key.ToLower() + "%");
            }

            if (goodKey != string.Empty)
            {
                cmdText.AppendFormat(" and (lower(b.goodcode) like '{0}' or lower(b.goodname) like  '{0}' or lower(b.xbarcode) like  '{0}')", "%" + goodKey.ToLower() + "%");
            }
            if (clntKey != string.Empty)
            {
                cmdText.AppendFormat(" and (lower(a.clntname) like '{0}' or lower(a.clntcode) like  '{0}')", "%" + clntKey.ToLower() + "%");
            }
            cmdText.AppendLine(" order by datetime(xintime) desc limit @pageSize offset @pageSize*(@page-1)");


            if (!isAll)
            {
                parameters = new SQLiteParameter[4];
                parameters[0] = new SQLiteParameter("pageSize", DbType.Int32);
                parameters[0].Value = pageSize;
                parameters[1] = new SQLiteParameter("page", DbType.Int32);
                parameters[1].Value = page;
                parameters[2] = new SQLiteParameter("startDate", DbType.DateTime);
                parameters[2].Value = startDate.Value.Date;
                parameters[3] = new SQLiteParameter("endDate", DbType.DateTime);
                parameters[3].Value = endDate.Value.Date.AddDays(1);
            }
            else
            {
                parameters = new SQLiteParameter[2];
                parameters[0] = new SQLiteParameter("pageSize", DbType.Int32);
                parameters[0].Value = pageSize;
                parameters[1] = new SQLiteParameter("page", DbType.Int32);
                parameters[1].Value = page;
            }

            SQLiteDataReader dataReader = SQLiteHelper.ExecuteReader(SQLiteHelper.connectionString, CommandType.Text, cmdText.ToString(), parameters);

            List<PoshhModel> poshhs = new List<PoshhModel>();

            while (dataReader.Read())
            {
                PoshhModel poshh = new PoshhModel();
                poshh.ID = new Guid(dataReader["ID"].ToString().Trim());
                poshh.xlsname = dataReader["xlsname"].ToString().Trim();
                poshh.xterm = dataReader["xterm"].ToString().Trim();
                poshh.billno = dataReader["billno"].ToString().Trim();
                poshh.pbillno = dataReader["pbillno"].ToString().Trim();
                poshh.xstate = dataReader["xstate"].ToString().Trim();
                poshh.clntname = dataReader["clntname"].ToString().Trim();
                poshh.xhezhe = decimal.Parse(dataReader["xhezhe"].ToString().Trim());
                poshh.xpay = decimal.Parse(dataReader["xpay"].ToString().Trim());
                poshh.xintime = DateTime.Parse(dataReader["xintime"].ToString()).ToString("MM-dd HH:mm");
                if (dataReader["uploadstatus"] == null
                    || dataReader["uploadstatus"].ToString() == ((int)UploadStatus.NotUploaded).ToString()
                    || dataReader["uploadstatus"].ToString() == string.Empty)
                {
                    poshh.uploadstatus = "未上传";
                }
                else if (dataReader["uploadstatus"].ToString() == ((int)UploadStatus.Uploading).ToString())
                {
                    poshh.uploadstatus = "上传中";
                }
                else if (dataReader["uploadstatus"].ToString() == ((int)UploadStatus.Uploaded).ToString())
                {
                    poshh.uploadstatus = "已上传";
                }
                poshhs.Add(poshh);
            }
            dataReader.Close();
            return poshhs;
        }
        #endregion

        #region 查询Pos挂单单据
        /// <summary>
        /// 查询Pos挂单单据
        /// </summary>
        /// <returns></returns>
        public List<PoshhModel> GetPOSPending()
        {
            SQLiteParameter[] parameters = new SQLiteParameter[1];

            StringBuilder cmdText = new StringBuilder();
            cmdText.AppendLine("select ID,billno,xintime from poshh where xstate = @xstate order by xintime");

            Dictionary<string, string> stateDic = EnumHelper.GetEnumDictionary(typeof(PosState));

            parameters[0] = new SQLiteParameter("xstate", DbType.String);
            parameters[0].Value = string.Join(",", stateDic[Enum.GetName(typeof(PosState), PosState.Pending)]);

            SQLiteDataReader dataReader = SQLiteHelper.ExecuteReader(SQLiteHelper.connectionString, CommandType.Text, cmdText.ToString(), parameters);

            List<PoshhModel> poshhs = new List<PoshhModel>();

            while (dataReader.Read())
            {
                PoshhModel poshh = new PoshhModel();
                poshh.ID = new Guid(dataReader["ID"].ToString().Trim());
                poshh.billno = dataReader["billno"].ToString().Trim();
                poshh.xintime = dataReader["xintime"].ToString();
                poshhs.Add(poshh);
            }
            dataReader.Close();
            return poshhs;
        }
        #endregion

        #region 查询一张Pos单据
        /// <summary>
        /// 查询一张Pos单据
        /// </summary>
        /// <returns></returns>
        public PoshhModel GetPOSByID(Guid ID)
        {
            SQLiteParameter[] parameters = new SQLiteParameter[1];

            #region 表头
            StringBuilder cmdText = new StringBuilder();
            cmdText.AppendLine("select ID,billno,posnono,xtype,xstate,xdate,paytype,transno,clntcode,clntname,");
            cmdText.AppendLine("xheallp as xheallp,xpay,xhezhe,xhenojie,xnote,xls,xlsname,xinname,xintime,");
            cmdText.AppendLine("pbillno,xrpay,xpoints,xsendjf,xhenojie,isHistory,isClntDay from poshh where ID = @ID");

            parameters[0] = new SQLiteParameter("ID", DbType.String);
            parameters[0].Value = ID;

            SQLiteDataReader dataReader = SQLiteHelper.ExecuteReader(SQLiteHelper.connectionString, CommandType.Text, cmdText.ToString(), parameters);

            PoshhModel poshh = null;

            while (dataReader.Read())
            {
                poshh = new PoshhModel();
                poshh.ID = new Guid(dataReader["ID"].ToString().Trim());
                poshh.billno = dataReader["billno"].ToString().Trim();
                poshh.posnono = dataReader["posnono"].ToString().Trim();
                poshh.xtype = dataReader["xtype"].ToString().Trim();
                poshh.xstate = dataReader["xstate"].ToString().Trim();
                poshh.paytype = dataReader["paytype"].ToString().Trim();
                poshh.transno = dataReader["transno"].ToString().Trim();
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
                poshh.xrpay = (dataReader["xrpay"] == null || string.IsNullOrEmpty(dataReader["xrpay"].ToString())) ? 0 : decimal.Parse(dataReader["xrpay"].ToString());
                poshh.xpoints = (dataReader["xpoints"] != null && dataReader["xpoints"].ToString().Trim() != string.Empty)
                  ? decimal.Parse(dataReader["xpoints"].ToString().Trim())
                  : (decimal?)null;
                poshh.xsendjf = (dataReader["xsendjf"] != null && dataReader["xsendjf"].ToString().Trim() != string.Empty)
                 ? int.Parse(dataReader["xsendjf"].ToString().Trim())
                 : (int?)null;
                poshh.xhenojie = (dataReader["xhenojie"] != null && dataReader["xhenojie"].ToString().Trim() != string.Empty)
               ? decimal.Parse(dataReader["xhenojie"].ToString().Trim())
               : 0;
                poshh.isHistory = (dataReader["isHistory"] != null && dataReader["isHistory"].ToString().Trim() != string.Empty)
               ? bool.Parse(dataReader["isHistory"].ToString().Trim()) : false;
                poshh.isClntDay = (dataReader["isClntDay"] != null && dataReader["isClntDay"].ToString().Trim() != string.Empty)
              ? bool.Parse(dataReader["isClntDay"].ToString().Trim()) : false;

            }
            dataReader.Close();
            #endregion

            #region 表体
            cmdText = new StringBuilder();
            cmdText.AppendLine("select ID,XID,a.goodcode,b.goodname,b.goodunit,unitname,unitrate,unitquat,xbarcode,");
            cmdText.AppendLine("goodkind1,goodkind2,goodkind3,goodkind4,goodkind5,goodkind6,goodkind7,goodkind8,");
            cmdText.AppendLine("goodkind9,goodkind10,cnkucode,cnkuname,xquat,xtquat,xpricold,xzhe,");
            cmdText.AppendLine("xpric,xallp,xsalestype,xsalesid,goodXtableID,isGift,PID,xpoints,xtaxr,xtax,xprict,xallpt,xchg,xtimes from posbb a");
            cmdText.AppendLine("inner join good b on a.goodcode = b.goodcode");
            cmdText.AppendLine("where XID = @ID");

            dataReader = SQLiteHelper.ExecuteReader(SQLiteHelper.connectionString, CommandType.Text, cmdText.ToString(), parameters);
            List<PosbbModel> posbbs = new List<PosbbModel>();

            while (dataReader.Read())
            {
                PosbbModel posbb = new PosbbModel();
                posbb.ID = new Guid(dataReader["ID"].ToString().Trim());
                posbb.XID = new Guid(dataReader["XID"].ToString().Trim());
                posbb.goodcode = dataReader["goodcode"].ToString().Trim();
                posbb.xbarcode = dataReader["xbarcode"].ToString().Trim();
                decimal xpoints = 0;
                if (decimal.TryParse(dataReader["xpoints"].ToString().Trim(), out xpoints))
                {
                    posbb.goodname = dataReader["goodname"].ToString().Trim() + "【兑】";
                }
                else
                {
                    posbb.goodname = dataReader["goodname"].ToString().Trim();
                }
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
                StringBuilder goodtm = new StringBuilder();
                goodtm.Append(posbb.goodkind1);
                goodtm.Append(posbb.goodkind2);
                goodtm.Append(posbb.goodkind3);
                goodtm.Append(posbb.goodkind4);
                goodtm.Append(posbb.goodkind5);
                goodtm.Append(posbb.goodkind6);
                goodtm.Append(posbb.goodkind7);
                goodtm.Append(posbb.goodkind8);
                goodtm.Append(posbb.goodkind9);
                goodtm.Append(posbb.goodkind10);
                posbb.goodtm = goodtm.ToString();
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
                posbb.xsalestype = dataReader["xsalestype"].ToString().Trim();
                posbb.xsalesid = (dataReader["xsalesid"] != null && dataReader["xsalesid"].ToString().Trim() != string.Empty)
                   ? int.Parse(dataReader["xsalesid"].ToString().Trim())
                   : (int?)null;
                posbb.goodXtableID = (dataReader["goodXtableID"] != null && dataReader["goodXtableID"].ToString().Trim() != string.Empty)
                 ? int.Parse(dataReader["goodXtableID"].ToString().Trim())
                 : 0;
                posbb.isGift = (dataReader["isGift"] != null && dataReader["isGift"].ToString().Trim() != string.Empty)
                ? bool.Parse(dataReader["isGift"].ToString().Trim())
                : false;
                posbb.PID = (dataReader["PID"] != null && dataReader["PID"].ToString().Trim() != string.Empty)
                    ? Guid.Parse(dataReader["PID"].ToString().Trim())
                    : (Guid?)null;
                posbb.xpoints = (dataReader["xpoints"] != null && dataReader["xpoints"].ToString().Trim() != string.Empty)
                   ? decimal.Parse(dataReader["xpoints"].ToString().Trim())
                   : (decimal?)null;
                posbb.xtaxr = (dataReader["xtaxr"] != null && dataReader["xtaxr"].ToString().Trim() != string.Empty)
                  ? decimal.Parse(dataReader["xtaxr"].ToString().Trim())
                  : 0;
                posbb.xtax = (dataReader["xtax"] != null && dataReader["xtax"].ToString().Trim() != string.Empty)
                 ? decimal.Parse(dataReader["xtax"].ToString().Trim())
                 : 0;
                posbb.xprict = (dataReader["xprict"] != null && dataReader["xprict"].ToString().Trim() != string.Empty)
                 ? decimal.Parse(dataReader["xprict"].ToString().Trim())
                 : 0;
                posbb.xallpt = (dataReader["xallpt"] != null && dataReader["xallpt"].ToString().Trim() != string.Empty)
                ? decimal.Parse(dataReader["xallpt"].ToString().Trim())
                : 0;
                posbb.xchg = dataReader["xchg"].ToString().Trim();
                posbb.xtimes = (dataReader["xtimes"] != null && dataReader["xtimes"].ToString().Trim() != string.Empty)
              ? decimal.Parse(dataReader["xtimes"].ToString().Trim())
              : (decimal?)null;
                posbbs.Add(posbb);
            }
            dataReader.Close();
            poshh.Posbbs = posbbs;
            #endregion

            #region 支付明细
            cmdText = new StringBuilder();
            cmdText.AppendLine("select ID,XID,paytcode,paytname,xpay,xreceipt,xnote1,xnote2,billflag from billpayt where XID = @ID");

            dataReader = SQLiteHelper.ExecuteReader(SQLiteHelper.connectionString, CommandType.Text, cmdText.ToString(), parameters);
            List<BillpaytModel> billpayts = new List<BillpaytModel>();

            while (dataReader.Read())
            {
                BillpaytModel billpayt = new BillpaytModel();
                billpayt.ID = new Guid(dataReader["ID"].ToString().Trim());
                billpayt.XID = new Guid(dataReader["XID"].ToString().Trim());
                billpayt.paytcode = dataReader["paytcode"].ToString().Trim();
                billpayt.paytname = dataReader["paytname"].ToString().Trim();
                billpayt.xnote1 = dataReader["xnote1"].ToString().Trim();
                billpayt.xnote2 = dataReader["xnote2"].ToString().Trim();
                billpayt.billflag = dataReader["billflag"].ToString().Trim();
                billpayt.xpay = decimal.Parse(dataReader["xpay"].ToString().Trim());
                billpayt.xreceipt = (dataReader["xreceipt"] == null || string.IsNullOrEmpty(dataReader["xreceipt"].ToString())) ? 0 : decimal.Parse(dataReader["xreceipt"].ToString().Trim());
                billpayts.Add(billpayt);
            }
            dataReader.Close();
            poshh.payts = billpayts;
            #endregion

            return poshh;
        }
        /// <summary>
        /// 查询一张Pos单据
        /// </summary>
        /// <returns></returns>
        public PoshhModel GetPOSByPNO(string billno)
        {
            SQLiteParameter[] parameters = new SQLiteParameter[1];

            #region 表头
            StringBuilder cmdText = new StringBuilder();
            cmdText.AppendLine("select ID,billno,posnono,xtype,xstate,xdate,paytype,transno,clntcode,clntname,");
            cmdText.AppendLine("xheallp as xheallp,xpay,xhezhe,xhenojie,xnote,xls,xlsname,xinname,xintime,");
            cmdText.AppendLine("pbillno,xrpay,xpoints,xsendjf,xhenojie,isHistory from poshh where pbillno = @billno");

            parameters[0] = new SQLiteParameter("billno", DbType.String);
            parameters[0].Value = billno;

            SQLiteDataReader dataReader = SQLiteHelper.ExecuteReader(SQLiteHelper.connectionString, CommandType.Text, cmdText.ToString(), parameters);

            PoshhModel poshh = null;

            while (dataReader.Read())
            {
                poshh = new PoshhModel();
                poshh.ID = new Guid(dataReader["ID"].ToString().Trim());
                poshh.billno = dataReader["billno"].ToString().Trim();
                poshh.posnono = dataReader["posnono"].ToString().Trim();
                poshh.xtype = dataReader["xtype"].ToString().Trim();
                poshh.xstate = dataReader["xstate"].ToString().Trim();
                poshh.paytype = dataReader["paytype"].ToString().Trim();
                poshh.transno = dataReader["transno"].ToString().Trim();
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
                poshh.xrpay = (dataReader["xrpay"] == null || string.IsNullOrEmpty(dataReader["xrpay"].ToString())) ? 0 : decimal.Parse(dataReader["xrpay"].ToString());
                poshh.xpoints = (dataReader["xpoints"] != null && dataReader["xpoints"].ToString().Trim() != string.Empty)
                  ? decimal.Parse(dataReader["xpoints"].ToString().Trim())
                  : (decimal?)null;
                poshh.xsendjf = (dataReader["xsendjf"] != null && dataReader["xsendjf"].ToString().Trim() != string.Empty)
                 ? int.Parse(dataReader["xsendjf"].ToString().Trim())
                 : (int?)null;
                poshh.xhenojie = (dataReader["xhenojie"] != null && dataReader["xhenojie"].ToString().Trim() != string.Empty)
               ? decimal.Parse(dataReader["xhenojie"].ToString().Trim())
               : 0;
                poshh.isHistory = (dataReader["isHistory"] != null && dataReader["isHistory"].ToString().Trim() != string.Empty)
               ? bool.Parse(dataReader["isHistory"].ToString().Trim()) : false;
            }
            dataReader.Close();
            #endregion
            return poshh;
        }
        #endregion

        #region 添加Pos单
        /// <summary>
        /// 添加Pos单
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public bool AddPOS(PoshhModel entity, out string billno)
        {
            if (entity.Posbbs == null || entity.Posbbs.Count == 0)
            {
                billno = string.Empty;
                return false;
            }
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
                        SQLiteParameter[] parameters = new SQLiteParameter[27];

                        StringBuilder cmdText = new StringBuilder();
                        cmdText.AppendLine("INSERT INTO poshh(");
                        cmdText.AppendLine("ID,billno,posnono,xstate,paytype,clntcode,clntname,");
                        cmdText.AppendLine("xnote,xls,xlsname,xinname,xintime,xterm,pbillno,xdate,");
                        cmdText.AppendLine("xheallp,xpay,xhezhe,xhenojie,transno,xrpay,xpoints,xversion,xsendjf,xhenojie,deductiblecash,isClntDay)");
                        cmdText.AppendLine("VALUES(");
                        cmdText.AppendLine("@ID,@billno,@posnono,@xstate,@paytype,@clntcode,@clntname,");
                        cmdText.AppendLine("@xnote,@xls,@xlsname,@xinname,@xintime,@xterm,@pbillno,@xdate,");
                        cmdText.AppendLine("@xheallp,@xpay,@xhezhe,@xhenojie,@transno,@xrpay,@xpoints,@xversion,@xsendjf,@xhenojie,@deductiblecash,@isClntDay)");

                        billno = GetPosbbNO();
                        entity.ID = Guid.NewGuid();
                        parameters[0] = new SQLiteParameter("ID", DbType.String);
                        parameters[0].Value = entity.ID;
                        parameters[1] = new SQLiteParameter("billno", DbType.String);
                        parameters[1].Value = billno;
                        parameters[2] = new SQLiteParameter("posnono", DbType.String);
                        parameters[2].Value = entity.posnono;
                        parameters[3] = new SQLiteParameter("xstate", DbType.String);
                        parameters[3].Value = entity.xstate;
                        parameters[4] = new SQLiteParameter("clntcode", DbType.String);
                        parameters[4].Value = entity.clntcode;
                        parameters[5] = new SQLiteParameter("clntname", DbType.String);
                        parameters[5].Value = entity.clntname;
                        parameters[6] = new SQLiteParameter("xnote", DbType.String);
                        parameters[6].Value = entity.xnote;
                        parameters[7] = new SQLiteParameter("xls", DbType.String);
                        parameters[7].Value = entity.xls;
                        parameters[8] = new SQLiteParameter("xlsname", DbType.String);
                        parameters[8].Value = entity.xlsname;
                        parameters[9] = new SQLiteParameter("xinname", DbType.String);
                        parameters[9].Value = entity.xinname;
                        parameters[10] = new SQLiteParameter("xintime", DbType.String);
                        parameters[10].Value = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                        parameters[11] = new SQLiteParameter("xterm", DbType.String);
                        parameters[11].Value = entity.xstate == (stateDic[Enum.GetName(typeof(PosState), PosState.Deal)]) ? DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") : string.Empty;
                        parameters[12] = new SQLiteParameter("pbillno", DbType.String);
                        parameters[12].Value = entity.pbillno;
                        parameters[13] = new SQLiteParameter("xdate", DbType.DateTime);
                        parameters[13].Value = DateTime.Now.ToString("yyyy-MM-dd");
                        parameters[14] = new SQLiteParameter("xheallp", DbType.Decimal);
                        parameters[14].Value = entity.xheallp;
                        parameters[15] = new SQLiteParameter("xpay", DbType.Decimal);
                        parameters[15].Value = entity.xpay;
                        parameters[16] = new SQLiteParameter("xhezhe", DbType.Decimal);
                        parameters[16].Value = entity.xhezhe;
                        parameters[17] = new SQLiteParameter("xhenojie", DbType.Decimal);
                        parameters[17].Value = entity.xhenojie;
                        parameters[18] = new SQLiteParameter("xversion", DbType.Double);
                        parameters[18].Value = GetTimeStamp();
                        parameters[19] = new SQLiteParameter("paytype", DbType.String);
                        parameters[19].Value = entity.paytype;
                        parameters[20] = new SQLiteParameter("transno", DbType.String);
                        parameters[20].Value = entity.transno;
                        parameters[21] = new SQLiteParameter("xpoints", DbType.Decimal);
                        parameters[21].Value = entity.xpoints;
                        parameters[22] = new SQLiteParameter("xrpay", DbType.Decimal);
                        parameters[22].Value = entity.xrpay;
                        parameters[23] = new SQLiteParameter("xsendjf", DbType.Int32);
                        parameters[23].Value = entity.xsendjf;
                        parameters[24] = new SQLiteParameter("xhenojie", DbType.Decimal);
                        parameters[24].Value = entity.xhenojie;
                        parameters[25] = new SQLiteParameter("deductiblecash", DbType.Decimal);
                        parameters[25].Value = entity.deductiblecash;
                        parameters[26] = new SQLiteParameter("isClntDay", DbType.Boolean);
                        parameters[26].Value = entity.isClntDay;

                        cmd.CommandText = cmdText.ToString();
                        cmd.Parameters.AddRange(parameters);
                        cmd.ExecuteNonQuery();
                        #endregion

                        #region 表体
                        foreach (PosbbModel item in entity.Posbbs)
                        {
                            cmd.Parameters.Clear();

                            AddPosbb(entity, out parameters, out cmdText, item);

                            cmd.CommandText = cmdText.ToString();
                            cmd.Parameters.AddRange(parameters);
                            cmd.ExecuteNonQuery();
                        }
                        #endregion

                        #region 支付明细
                        if (entity.payts != null)
                        {
                            foreach (BillpaytModel item in entity.payts)
                            {
                                cmd.Parameters.Clear();

                                AddBillpayt(entity, out parameters, out cmdText, item);

                                cmd.CommandText = cmdText.ToString();
                                cmd.Parameters.AddRange(parameters);
                                cmd.ExecuteNonQuery();
                            }
                            Dictionary<string, string> payTypeDic = EnumHelper.GetEnumDictionary(typeof(PayType));
                            var deposit = entity.payts.Where(r => r.paytname == payTypeDic[Enum.GetName(typeof(PayType), PayType.Deposit)]).FirstOrDefault();
                            if (deposit != null)
                            {
                                //减掉用户的预存款
                                //cmd.Parameters.Clear();
                                //cmdText = new StringBuilder();
                                //cmdText.AppendLine("UPDATE ojie2 set ");
                            }
                            var coupon = entity.payts.Where(r => r.paytname == payTypeDic[Enum.GetName(typeof(PayType), PayType.Coupon)]).FirstOrDefault();
                            if (coupon != null)
                            {
                                parameters = new SQLiteParameter[5];

                                string sql = @"UPDATE tickoffmx set clntcode=@clntcode,clntname=@clntname,xstate=@xstate,xopusetime=@xopusetime
                                               where lower(xcode)=@xcode";

                                parameters[0] = new SQLiteParameter("clntcode", DbType.String);
                                parameters[0].Value = entity.clntcode;
                                parameters[1] = new SQLiteParameter("clntname", DbType.String);
                                parameters[1].Value = entity.clntname;
                                parameters[2] = new SQLiteParameter("xstate", DbType.String);
                                parameters[2].Value = (couponStateDic[Enum.GetName(typeof(CouponState), CouponState.Used)]);
                                parameters[3] = new SQLiteParameter("xopusetime", DbType.String);
                                parameters[3].Value = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                                parameters[4] = new SQLiteParameter("xcode", DbType.String);
                                parameters[4].Value = coupon.xnote1.ToLower();

                                cmd.CommandText = sql;
                                cmd.Parameters.AddRange(parameters);
                                cmd.ExecuteNonQuery();
                            }
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
        private static void AddPosbb(PoshhModel entity, out SQLiteParameter[] parameters, out StringBuilder cmdText, PosbbModel item)
        {
            parameters = new SQLiteParameter[40];

            cmdText = new StringBuilder();
            cmdText.AppendLine("INSERT INTO posbb(");
            cmdText.AppendLine("ID,XID,goodcode,goodname,goodunit,goodkind1,goodkind2,");
            cmdText.AppendLine("goodkind3,goodkind4,goodkind5,goodkind6,goodkind7,goodkind8,goodkind9,goodkind10,");
            cmdText.AppendLine("cnkucode,cnkuname,xquat,xtquat,xpricold,xzhe,xpric,xallp,unitname,unitrate,");
            cmdText.AppendLine("unitquat,xsalestype,xsalesid,goodXtableID,isGift,PID,xpoints,xtaxr,xtax,xprict,xallpt,xbarcode,xversion,xchg,xtimes)");
            cmdText.AppendLine("VALUES(");
            cmdText.AppendLine("@ID,@XID,@goodcode,@goodname,@goodunit,@goodkind1,@goodkind2,");
            cmdText.AppendLine("@goodkind3,@goodkind4,@goodkind5,@goodkind6,@goodkind7,@goodkind8,@goodkind9,@goodkind10,");
            cmdText.AppendLine("@cnkucode,@cnkuname,@xquat,@xtquat,@xpricold,@xzhe,@xpric,@xallp,@unitname,@unitrate,");
            cmdText.AppendLine("@unitquat,@xsalestype,@xsalesid,@goodXtableID,@isGift,@PID,@xpoints,@xtaxr,@xtax,@xprict,@xallpt,@xbarcode,@xversion,@xchg,@xtimes)");

            item.xtax = CalcMoneyHelper.Multiply(item.xallp, item.xtaxr);
            item.xallpt = CalcMoneyHelper.Add(item.xallp, item.xtax);
            item.xprict = CalcMoneyHelper.Divide(item.xallpt, item.unitquat);

            parameters[0] = new SQLiteParameter("ID", DbType.String);
            parameters[0].Value = item.ID == Guid.Empty ? Guid.NewGuid() : item.ID;
            parameters[1] = new SQLiteParameter("XID", DbType.String);
            parameters[1].Value = entity.ID;
            parameters[2] = new SQLiteParameter("goodcode", DbType.String);
            parameters[2].Value = item.goodcode;
            parameters[3] = new SQLiteParameter("goodname", DbType.String);
            parameters[3].Value = item.goodname;
            parameters[4] = new SQLiteParameter("goodunit", DbType.String);
            parameters[4].Value = item.goodunit;
            parameters[5] = new SQLiteParameter("goodkind1", DbType.String);
            parameters[5].Value = item.goodkind1;
            parameters[6] = new SQLiteParameter("goodkind2", DbType.String);
            parameters[6].Value = item.goodkind2;
            parameters[7] = new SQLiteParameter("goodkind3", DbType.String);
            parameters[7].Value = item.goodkind3;
            parameters[8] = new SQLiteParameter("goodkind4", DbType.String);
            parameters[8].Value = item.goodkind4;
            parameters[9] = new SQLiteParameter("goodkind5", DbType.String);
            parameters[9].Value = item.goodkind5;
            parameters[10] = new SQLiteParameter("goodkind6", DbType.String);
            parameters[10].Value = item.goodkind6;
            parameters[11] = new SQLiteParameter("goodkind7", DbType.String);
            parameters[11].Value = item.goodkind7;
            parameters[12] = new SQLiteParameter("goodkind8", DbType.String);
            parameters[12].Value = item.goodkind8;
            parameters[13] = new SQLiteParameter("goodkind9", DbType.String);
            parameters[13].Value = item.goodkind9;
            parameters[14] = new SQLiteParameter("goodkind10", DbType.String);
            parameters[14].Value = item.goodkind10;
            parameters[15] = new SQLiteParameter("cnkucode", DbType.String);
            parameters[15].Value = item.cnkucode;
            parameters[16] = new SQLiteParameter("cnkuname", DbType.String);
            parameters[16].Value = item.cnkuname;
            parameters[17] = new SQLiteParameter("xquat", DbType.Decimal);
            //parameters[17].Value = item.xquat;
            parameters[17].Value = item.xchg == "换进" ? 0 - item.unitrate * item.unitquat : item.unitrate * item.unitquat;
            parameters[18] = new SQLiteParameter("xtquat", DbType.Decimal);
            parameters[18].Value = item.xtquat;
            parameters[19] = new SQLiteParameter("xpricold", DbType.Decimal);
            parameters[19].Value = item.xpricold;
            parameters[20] = new SQLiteParameter("xzhe", DbType.Decimal);
            parameters[20].Value = item.xzhe;
            parameters[21] = new SQLiteParameter("xpric", DbType.Decimal);
            parameters[21].Value = item.xpric;
            parameters[22] = new SQLiteParameter("xallp", DbType.Decimal);
            parameters[22].Value = item.xchg == "换进" ? 0 - item.xallp : item.xallp;
            parameters[23] = new SQLiteParameter("unitname", DbType.String);
            parameters[23].Value = item.unitname;
            parameters[24] = new SQLiteParameter("unitrate", DbType.Decimal);
            parameters[24].Value = item.unitrate;
            parameters[25] = new SQLiteParameter("unitquat", DbType.Decimal);
            parameters[25].Value = item.xchg == "换进" ? 0 - item.unitquat : item.unitquat;
            parameters[26] = new SQLiteParameter("xsalestype", DbType.String);
            parameters[26].Value = item.xsalestype;
            parameters[27] = new SQLiteParameter("xsalesid", DbType.Int32);
            parameters[27].Value = item.xsalesid;
            parameters[28] = new SQLiteParameter("goodXtableID", DbType.Int32);
            parameters[28].Value = item.goodXtableID;
            parameters[29] = new SQLiteParameter("isGift", DbType.Boolean);
            parameters[29].Value = item.isGift;
            parameters[30] = new SQLiteParameter("PID", DbType.String);
            parameters[30].Value = item.PID;
            parameters[31] = new SQLiteParameter("xpoints", DbType.Decimal);
            parameters[31].Value = item.xchg == "换进" ? 0 - item.xpoints : item.xpoints;
            parameters[32] = new SQLiteParameter("xtaxr", DbType.Decimal);
            parameters[32].Value = item.xtaxr;
            parameters[33] = new SQLiteParameter("xtax", DbType.Decimal);
            parameters[33].Value = item.xchg == "换进" ? 0 - item.xtax : item.xtax;
            parameters[34] = new SQLiteParameter("xprict", DbType.Decimal);
            parameters[34].Value = item.xprict;
            parameters[35] = new SQLiteParameter("xallpt", DbType.Decimal);
            parameters[35].Value = item.xchg == "换进" ? 0 - item.xallpt : item.xallpt;
            parameters[36] = new SQLiteParameter("xbarcode", DbType.String);
            parameters[36].Value = item.xbarcode;
            parameters[37] = new SQLiteParameter("xversion", DbType.Double);
            parameters[37].Value = GetTimeStamp();
            parameters[38] = new SQLiteParameter("xchg", DbType.String);
            parameters[38].Value = item.xchg;
            parameters[39] = new SQLiteParameter("xtimes", DbType.Decimal);
            parameters[39].Value = item.xtimes;
        }
        #endregion

        #region 添加支付明细
        private static void AddBillpayt(PoshhModel entity, out SQLiteParameter[] parameters, out StringBuilder cmdText, BillpaytModel item)
        {
            parameters = new SQLiteParameter[10];

            cmdText = new StringBuilder();
            cmdText.AppendLine("INSERT INTO billpayt (ID,XID,paytcode,paytname,");
            cmdText.AppendLine("xpay,xnote1,xnote2,billflag,xversion,xreceipt)");
            cmdText.AppendLine("VALUES (@ID,@XID,@paytcode,@paytname,");
            cmdText.AppendLine("@xpay,@xnote1,@xnote2,@billflag,@xversion,@xreceipt)");

            parameters[0] = new SQLiteParameter("ID", DbType.String);
            parameters[0].Value = Guid.NewGuid();
            parameters[1] = new SQLiteParameter("XID", DbType.String);
            parameters[1].Value = entity.ID;
            parameters[2] = new SQLiteParameter("paytcode", DbType.String);
            parameters[2].Value = item.paytcode;
            parameters[3] = new SQLiteParameter("paytname", DbType.String);
            parameters[3].Value = item.paytname;
            parameters[4] = new SQLiteParameter("xpay", DbType.Decimal);
            parameters[4].Value = item.xpay;
            parameters[5] = new SQLiteParameter("xnote1", DbType.String);
            parameters[5].Value = item.xnote1;
            parameters[6] = new SQLiteParameter("xnote2", DbType.String);
            parameters[6].Value = item.xnote2;
            parameters[7] = new SQLiteParameter("billflag", DbType.String);
            parameters[7].Value = item.billflag;
            parameters[8] = new SQLiteParameter("xversion", DbType.Double);
            parameters[8].Value = GetTimeStamp();
            parameters[9] = new SQLiteParameter("xreceipt", DbType.Decimal);
            parameters[9].Value = item.xreceipt;
        }
        #endregion

        #region 生成POS单号
        /// <summary>
        /// 生成POS单号
        /// </summary>
        /// <returns></returns>
        private string GetPosbbNO()
        {
            SQLiteParameter[] parameters = new SQLiteParameter[1];
            //查询总条数
            string sql = "select count(*) from poshh where date(xintime) = @date";

            //Dictionary<string, string> stateDic = EnumHelper.GetEnumDictionary(typeof(PosState));

            parameters[0] = new SQLiteParameter("date", DbType.String);
            parameters[0].Value = DateTime.Now.ToString("yyyy-MM-dd");

            long totalRow = (long)SQLiteHelper.ExecuteScalar(SQLiteHelper.connectionString, CommandType.Text, sql, parameters);
            string currentNO = (totalRow + 1).ToString().PadLeft(4, '0');
            StringBuilder billno = new StringBuilder();
            billno.Append("PS");
            billno.Append(DateTime.Now.ToString("yyyyMMddHHmmssfff"));
            billno.Append(currentNO);
            return billno.ToString();

        }
        #endregion

        #region  修改挂单
        /// <summary>
        /// 修改挂单
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public bool UpdatePosbb(PosbbModel entity)
        {
            try
            {
                string cmdText = string.Empty;
                SQLiteParameter[] parameters = null;
                if (entity.xquat == 0)
                {
                    parameters = new SQLiteParameter[1];
                    //删除
                    cmdText = "DELETE FROM posbb where ID=@ID";
                    parameters[0] = new SQLiteParameter("ID", DbType.String);
                    parameters[0].Value = entity.ID;
                    SQLiteHelper.ExecuteNonQuery(SQLiteHelper.connectionString, CommandType.Text, cmdText, parameters);

                    cmdText = "select count(*) from posbb where XID=@XID";

                    parameters = new SQLiteParameter[1];
                    parameters[0] = new SQLiteParameter("XID", DbType.String);
                    parameters[0].Value = entity.XID;

                    long totalRow = (long)SQLiteHelper.ExecuteScalar(SQLiteHelper.connectionString, CommandType.Text, cmdText, parameters);

                    if (totalRow == 0)
                    {
                        Dictionary<string, string> stateDic = EnumHelper.GetEnumDictionary(typeof(PosState));

                        //把表头状态改成作废
                        //cmdText = "update poshh set xstate = @xstate where ID=@XID";
                        //parameters = new SQLiteParameter[2];
                        //parameters[0] = new SQLiteParameter("XID", DbType.String);
                        //parameters[0].Value = entity.XID;
                        //parameters[1] = new SQLiteParameter("xstate", DbType.String);
                        //parameters[1].Value = stateDic[Enum.GetName(typeof(PosState), PosState.Invalid)];
                        //SQLiteHelper.ExecuteNonQuery(SQLiteHelper.connectionString, CommandType.Text, cmdText, parameters);
                        Invalid(entity.XID, string.Empty);
                    }
                }
                else
                {
                    //修改
                    cmdText = "update posbb set xpric=@xpric,xquat=@xquat,xzhe=@xzhe,xallp=@xallp where ID=@ID";
                    parameters = new SQLiteParameter[5];
                    parameters[0] = new SQLiteParameter("xpric", DbType.Decimal);
                    parameters[0].Value = entity.xpric;
                    parameters[1] = new SQLiteParameter("xquat", DbType.Decimal);
                    parameters[1].Value = entity.xquat;
                    parameters[2] = new SQLiteParameter("xzhe", DbType.Decimal);
                    parameters[2].Value = entity.xzhe;
                    parameters[3] = new SQLiteParameter("xallp", DbType.Decimal);
                    parameters[3].Value = entity.xallp;
                    parameters[4] = new SQLiteParameter("ID", DbType.String);
                    parameters[4].Value = entity.ID;
                    SQLiteHelper.ExecuteNonQuery(SQLiteHelper.connectionString, CommandType.Text, cmdText, parameters);

                    cmdText = "update poshh set xversion=@xversion,uploadstatus=@uploadstatus where ID=@ID";
                    parameters = new SQLiteParameter[3];
                    parameters[0] = new SQLiteParameter("ID", DbType.String);
                    parameters[0].Value = entity.XID;
                    parameters[1] = new SQLiteParameter("xversion", DbType.String);
                    parameters[1].Value = GetTimeStamp();
                    parameters[2] = new SQLiteParameter("uploadstatus", DbType.String);
                    parameters[2].Value = (int)UploadStatus.NotUploaded;
                    SQLiteHelper.ExecuteNonQuery(SQLiteHelper.connectionString, CommandType.Text, cmdText, parameters);
                }
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region 作废
        /// <summary>
        /// 作废
        /// </summary>
        /// <returns></returns>
        public bool Invalid(Guid ID, string remark)
        {
            SQLiteTransaction sqltran = null;
            SQLiteConnection con = null;
            try
            {
                Dictionary<string, string> payTypeDic = EnumHelper.GetEnumDictionary(typeof(PayType));

                //获取优惠券编码
                string sql = "select xnote1 from billpayt where XID=XID and paytname=@paytname";
                SQLiteParameter[] parameters = new SQLiteParameter[2];
                parameters[0] = new SQLiteParameter("ID", DbType.String);
                parameters[0].Value = ID;
                parameters[1] = new SQLiteParameter("paytname", DbType.String);
                parameters[1].Value = payTypeDic[Enum.GetName(typeof(PayType), PayType.Coupon)];


                object xcode = SQLiteHelper.ExecuteScalar(SQLiteHelper.connectionString, CommandType.Text, sql, parameters);

                con = SQLiteHelper.DbConnection;
                using (SQLiteCommand cmd = new SQLiteCommand())
                {
                    con.Open();
                    sqltran = con.BeginTransaction();
                    cmd.Connection = con;
                    cmd.Transaction = sqltran;

                    string cmdText = "update poshh set xstate = @xstate,xnote=@xnote,pbillno='',xversion=@xversion,uploadstatus=@uploadstatus where ID=@ID";
                    Dictionary<string, string> stateDic = EnumHelper.GetEnumDictionary(typeof(PosState));
                    parameters = new SQLiteParameter[5];
                    parameters[0] = new SQLiteParameter("ID", DbType.String);
                    parameters[0].Value = ID;
                    parameters[1] = new SQLiteParameter("xstate", DbType.String);
                    parameters[1].Value = stateDic[Enum.GetName(typeof(PosState), PosState.Invalid)];
                    parameters[2] = new SQLiteParameter("xnote", DbType.String);
                    parameters[2].Value = remark;
                    parameters[3] = new SQLiteParameter("xversion", DbType.String);
                    parameters[3].Value = GetTimeStamp();
                    parameters[4] = new SQLiteParameter("uploadstatus", DbType.String);
                    parameters[4].Value = (int)UploadStatus.NotUploaded;

                    cmd.Parameters.Clear();
                    cmd.CommandText = cmdText;
                    cmd.Parameters.AddRange(parameters);
                    cmd.ExecuteNonQuery();

                    if (xcode != null && string.IsNullOrEmpty(xcode.ToString()))
                    {
                        Dictionary<string, string> couponStateDic = EnumHelper.GetEnumDictionary(typeof(CouponState));

                        cmdText = "update tickoffmx set clntcode='',clntname='',xopusetime='',xstate=@xstate where lower(xcode) = @xcode";
                        parameters = new SQLiteParameter[2];
                        parameters[0] = new SQLiteParameter("xstate", DbType.String);
                        parameters[0].Value = couponStateDic[Enum.GetName(typeof(CouponState), CouponState.Unused)];
                        parameters[1] = new SQLiteParameter("xcode", DbType.String);
                        parameters[1].Value = xcode;

                        cmd.Parameters.Clear();
                        cmd.CommandText = cmdText;
                        cmd.Parameters.AddRange(parameters);
                        cmd.ExecuteNonQuery();
                    }
                    sqltran.Commit();
                    return true;
                }
            }
            catch (Exception ex)
            {
                sqltran.Rollback();
                throw ex;
            }
            finally
            {
                con.Close();
            }
        }
        #endregion

        #region 挂单（追加）
        /// <summary>
        /// 挂单（追加）
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public bool Append(PoshhModel entity)
        {
            if (entity.Posbbs == null || entity.Posbbs.Count == 0)
            {
                return false;
            }
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
                        SQLiteParameter[] parameters = new SQLiteParameter[8];

                        StringBuilder cmdText = new StringBuilder();
                        cmdText.AppendLine("update poshh set xheallp=@xheallp,xpay=@xpay,xhezhe=@xhezhe,xhenojie=@xhenojie,xrpay=@xrpay,xversion=@xversion,uploadstatus=@uploadstatus where ID=@ID");

                        parameters[0] = new SQLiteParameter("ID", DbType.String);
                        parameters[0].Value = entity.ID;
                        parameters[1] = new SQLiteParameter("xheallp", DbType.Decimal);
                        parameters[1].Value = entity.xheallp;
                        parameters[2] = new SQLiteParameter("xpay", DbType.Decimal);
                        parameters[2].Value = entity.xpay;
                        parameters[3] = new SQLiteParameter("xhezhe", DbType.Decimal);
                        parameters[3].Value = entity.xhezhe;
                        parameters[4] = new SQLiteParameter("xhenojie", DbType.Decimal);
                        parameters[4].Value = entity.xhenojie;
                        parameters[5] = new SQLiteParameter("xversion", DbType.Double);
                        parameters[5].Value = GetTimeStamp();
                        parameters[6] = new SQLiteParameter("xrpay", DbType.Decimal);
                        parameters[6].Value = entity.xrpay;
                        parameters[7] = new SQLiteParameter("uploadstatus", DbType.String);
                        parameters[7].Value = (int)UploadStatus.NotUploaded;

                        cmd.CommandText = cmdText.ToString();
                        cmd.Parameters.AddRange(parameters);
                        cmd.ExecuteNonQuery();
                        #endregion

                        #region 表体
                        foreach (PosbbModel item in entity.Posbbs)
                        {
                            cmd.Parameters.Clear();

                            #region 修改前
                            //parameters = new SQLiteParameter[29];

                            //cmdText = new StringBuilder();
                            //cmdText.AppendLine("INSERT INTO posbb(");
                            //cmdText.AppendLine("ID,XID,goodcode,goodname,goodunit,goodkind1,goodkind2,");
                            //cmdText.AppendLine("goodkind3,goodkind4,goodkind5,goodkind6,goodkind7,goodkind8,goodkind9,goodkind10,");
                            //cmdText.AppendLine("cnkucode,cnkuname,xquat,xtquat,xpricold,xzhe,xpric,xallp,unitname,unitrate,unitquat,xsalestype,xsalesid,xversion)");
                            //cmdText.AppendLine("VALUES(");
                            //cmdText.AppendLine("@ID,@XID,@goodcode,@goodname,@goodunit,@goodkind1,@goodkind2,");
                            //cmdText.AppendLine("@goodkind3,@goodkind4,@goodkind5,@goodkind6,@goodkind7,@goodkind8,@goodkind9,@goodkind10,");
                            //cmdText.AppendLine("@cnkucode,@cnkuname,@xquat,@xtquat,@xpricold,@xzhe,@xpric,@xallp,@unitname,@unitrate,@unitquat,xsalestype,xsalesid,@xversion)");

                            //parameters[0] = new SQLiteParameter("ID", DbType.String);
                            //parameters[0].Value = Guid.NewGuid();
                            //parameters[1] = new SQLiteParameter("XID", DbType.String);
                            //parameters[1].Value = entity.ID;
                            //parameters[2] = new SQLiteParameter("goodcode", DbType.String);
                            //parameters[2].Value = item.goodcode;
                            //parameters[3] = new SQLiteParameter("goodname", DbType.String);
                            //parameters[3].Value = item.goodname;
                            //parameters[4] = new SQLiteParameter("goodunit", DbType.String);
                            //parameters[4].Value = item.goodunit;
                            //parameters[5] = new SQLiteParameter("goodkind1", DbType.String);
                            //parameters[5].Value = item.goodkind1;
                            //parameters[6] = new SQLiteParameter("goodkind2", DbType.String);
                            //parameters[6].Value = item.goodkind2;
                            //parameters[7] = new SQLiteParameter("goodkind3", DbType.String);
                            //parameters[7].Value = item.goodkind3;
                            //parameters[8] = new SQLiteParameter("goodkind4", DbType.String);
                            //parameters[8].Value = item.goodkind4;
                            //parameters[9] = new SQLiteParameter("goodkind5", DbType.String);
                            //parameters[9].Value = item.goodkind5;
                            //parameters[10] = new SQLiteParameter("goodkind6", DbType.String);
                            //parameters[10].Value = item.goodkind6;
                            //parameters[11] = new SQLiteParameter("goodkind7", DbType.String);
                            //parameters[11].Value = item.goodkind7;
                            //parameters[12] = new SQLiteParameter("goodkind8", DbType.String);
                            //parameters[12].Value = item.goodkind8;
                            //parameters[13] = new SQLiteParameter("goodkind9", DbType.String);
                            //parameters[13].Value = item.goodkind9;
                            //parameters[14] = new SQLiteParameter("goodkind10", DbType.String);
                            //parameters[14].Value = item.goodkind10;
                            //parameters[15] = new SQLiteParameter("cnkucode", DbType.String);
                            //parameters[15].Value = item.cnkucode;
                            //parameters[16] = new SQLiteParameter("cnkuname", DbType.String);
                            //parameters[16].Value = item.cnkuname;
                            //parameters[17] = new SQLiteParameter("xquat", DbType.Decimal);
                            //parameters[17].Value = item.xquat;
                            //parameters[18] = new SQLiteParameter("xtquat", DbType.Decimal);
                            //parameters[18].Value = item.xtquat;
                            //parameters[19] = new SQLiteParameter("xpricold", DbType.Decimal);
                            //parameters[19].Value = item.xpricold;
                            //parameters[20] = new SQLiteParameter("xzhe", DbType.Decimal);
                            //parameters[20].Value = item.xzhe;
                            //parameters[21] = new SQLiteParameter("xpric", DbType.Decimal);
                            //parameters[21].Value = item.xpric;
                            //parameters[22] = new SQLiteParameter("xallp", DbType.Decimal);
                            //parameters[22].Value = item.xallp;
                            //parameters[23] = new SQLiteParameter("unitname", DbType.String);
                            //parameters[23].Value = item.unitname;
                            //parameters[24] = new SQLiteParameter("unitrate", DbType.Decimal);
                            //parameters[24].Value = item.unitrate;
                            //parameters[25] = new SQLiteParameter("unitquat", DbType.Decimal);
                            //parameters[25].Value = item.unitquat;
                            //parameters[26] = new SQLiteParameter("xsalestype", DbType.String);
                            //parameters[26].Value = item.xsalestype;
                            //parameters[27] = new SQLiteParameter("xsalesid", DbType.Int32);
                            //parameters[27].Value = item.xsalesid;
                            //parameters[28] = new SQLiteParameter("xversion", DbType.Double);
                            //parameters[28].Value = GetTimeStamp();
                            #endregion
                            AddPosbb(entity, out parameters, out cmdText, item);

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

        #region 取单收款
        /// <summary>
        /// 取单收款
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public bool PendingReceipt(PoshhModel entity)
        {
            if (entity.Posbbs == null || entity.Posbbs.Count == 0)
            {
                return false;
            }
            Dictionary<string, string> stateDic = EnumHelper.GetEnumDictionary(typeof(PosState));

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
                        SQLiteParameter[] parameters = new SQLiteParameter[16];

                        StringBuilder cmdText = new StringBuilder();
                        cmdText.AppendLine("update poshh set xheallp=@xheallp,xpay=@xpay,xhezhe=@xhezhe,xhenojie=@xhenojie,xstate=@xstate,");
                        cmdText.AppendLine(" xterm=@xterm,xnote=@xnote,xrpay=@xrpay,xversion=@xversion,xsendjf=@xsendjf,clntcode=@clntcode,clntname=@clntname,uploadstatus=@uploadstatus,deductiblecash=@deductiblecash,isClntDay=@isClntDay where ID=@ID");

                        parameters[0] = new SQLiteParameter("ID", DbType.String);
                        parameters[0].Value = entity.ID;
                        parameters[1] = new SQLiteParameter("xheallp", DbType.Decimal);
                        parameters[1].Value = entity.xheallp;
                        parameters[2] = new SQLiteParameter("xpay", DbType.Decimal);
                        parameters[2].Value = entity.xpay;
                        parameters[3] = new SQLiteParameter("xhezhe", DbType.Decimal);
                        parameters[3].Value = entity.xhezhe;
                        parameters[4] = new SQLiteParameter("xhenojie", DbType.Decimal);
                        parameters[4].Value = entity.xhenojie;
                        parameters[5] = new SQLiteParameter("xstate", DbType.String);
                        parameters[5].Value = stateDic[Enum.GetName(typeof(PosState), PosState.Deal)];
                        parameters[6] = new SQLiteParameter("xterm", DbType.String);
                        parameters[6].Value = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                        parameters[7] = new SQLiteParameter("xnote", DbType.String);
                        parameters[7].Value = entity.xnote;
                        parameters[8] = new SQLiteParameter("xversion", DbType.Double);
                        parameters[8].Value = GetTimeStamp();
                        parameters[9] = new SQLiteParameter("xrpay", DbType.Decimal);
                        parameters[9].Value = entity.xrpay;
                        parameters[10] = new SQLiteParameter("xsendjf", DbType.Int32);
                        parameters[10].Value = entity.xsendjf;
                        parameters[11] = new SQLiteParameter("clntcode", DbType.String);
                        parameters[11].Value = entity.clntcode;
                        parameters[12] = new SQLiteParameter("clntname", DbType.String);
                        parameters[12].Value = entity.clntname;
                        parameters[13] = new SQLiteParameter("uploadstatus", DbType.String);
                        parameters[13].Value = (int)UploadStatus.NotUploaded;
                        parameters[14] = new SQLiteParameter("deductiblecash", DbType.Decimal);
                        parameters[14].Value = entity.deductiblecash;
                        parameters[15] = new SQLiteParameter("isClntDay", DbType.Boolean);
                        parameters[15].Value = entity.isClntDay;

                        cmd.CommandText = cmdText.ToString();
                        cmd.Parameters.AddRange(parameters);
                        cmd.ExecuteNonQuery();
                        #endregion

                        cmd.Parameters.Clear();
                        cmdText = new StringBuilder();
                        cmdText.AppendLine("delete from posbb where XID=@ID");
                        parameters = new SQLiteParameter[1];
                        parameters[0] = new SQLiteParameter("ID", DbType.String);
                        parameters[0].Value = entity.ID;
                        cmd.CommandText = cmdText.ToString();
                        cmd.Parameters.AddRange(parameters);
                        int result = cmd.ExecuteNonQuery();

                        #region 表体
                        foreach (PosbbModel item in entity.Posbbs)
                        {
                            cmd.Parameters.Clear();

                            #region 修改前
                            //parameters = new SQLiteParameter[29];

                            //cmdText = new StringBuilder();
                            //cmdText.AppendLine("INSERT INTO posbb(");
                            //cmdText.AppendLine("ID,XID,goodcode,goodname,goodunit,goodkind1,goodkind2,");
                            //cmdText.AppendLine("goodkind3,goodkind4,goodkind5,goodkind6,goodkind7,goodkind8,goodkind9,goodkind10,");
                            //cmdText.AppendLine("cnkucode,cnkuname,xquat,xtquat,xpricold,xzhe,xpric,xallp,unitname,unitrate,unitquat,xsalestype,xsalesid,xversion)");
                            //cmdText.AppendLine("VALUES(");
                            //cmdText.AppendLine("@ID,@XID,@goodcode,@goodname,@goodunit,@goodkind1,@goodkind2,");
                            //cmdText.AppendLine("@goodkind3,@goodkind4,@goodkind5,@goodkind6,@goodkind7,@goodkind8,@goodkind9,@goodkind10,");
                            //cmdText.AppendLine("@cnkucode,@cnkuname,@xquat,@xtquat,@xpricold,@xzhe,@xpric,@xallp,@unitname,@unitrate,@unitquat,xsalestype,@xsalesid,@xversion)");

                            //parameters[0] = new SQLiteParameter("ID", DbType.String);
                            //parameters[0].Value = Guid.NewGuid();
                            //parameters[1] = new SQLiteParameter("XID", DbType.String);
                            //parameters[1].Value = entity.ID;
                            //parameters[2] = new SQLiteParameter("goodcode", DbType.String);
                            //parameters[2].Value = item.goodcode;
                            //parameters[3] = new SQLiteParameter("goodname", DbType.String);
                            //parameters[3].Value = item.goodname;
                            //parameters[4] = new SQLiteParameter("goodunit", DbType.String);
                            //parameters[4].Value = item.goodunit;
                            //parameters[5] = new SQLiteParameter("goodkind1", DbType.String);
                            //parameters[5].Value = item.goodkind1;
                            //parameters[6] = new SQLiteParameter("goodkind2", DbType.String);
                            //parameters[6].Value = item.goodkind2;
                            //parameters[7] = new SQLiteParameter("goodkind3", DbType.String);
                            //parameters[7].Value = item.goodkind3;
                            //parameters[8] = new SQLiteParameter("goodkind4", DbType.String);
                            //parameters[8].Value = item.goodkind4;
                            //parameters[9] = new SQLiteParameter("goodkind5", DbType.String);
                            //parameters[9].Value = item.goodkind5;
                            //parameters[10] = new SQLiteParameter("goodkind6", DbType.String);
                            //parameters[10].Value = item.goodkind6;
                            //parameters[11] = new SQLiteParameter("goodkind7", DbType.String);
                            //parameters[11].Value = item.goodkind7;
                            //parameters[12] = new SQLiteParameter("goodkind8", DbType.String);
                            //parameters[12].Value = item.goodkind8;
                            //parameters[13] = new SQLiteParameter("goodkind9", DbType.String);
                            //parameters[13].Value = item.goodkind9;
                            //parameters[14] = new SQLiteParameter("goodkind10", DbType.String);
                            //parameters[14].Value = item.goodkind10;
                            //parameters[15] = new SQLiteParameter("cnkucode", DbType.String);
                            //parameters[15].Value = item.cnkucode;
                            //parameters[16] = new SQLiteParameter("cnkuname", DbType.String);
                            //parameters[16].Value = item.cnkuname;
                            //parameters[17] = new SQLiteParameter("xquat", DbType.Decimal);
                            //parameters[17].Value = item.xquat;
                            //parameters[18] = new SQLiteParameter("xtquat", DbType.Decimal);
                            //parameters[18].Value = item.xtquat;
                            //parameters[19] = new SQLiteParameter("xpricold", DbType.Decimal);
                            //parameters[19].Value = item.xpricold;
                            //parameters[20] = new SQLiteParameter("xzhe", DbType.Decimal);
                            //parameters[20].Value = item.xzhe;
                            //parameters[21] = new SQLiteParameter("xpric", DbType.Decimal);
                            //parameters[21].Value = item.xpric;
                            //parameters[22] = new SQLiteParameter("xallp", DbType.Decimal);
                            //parameters[22].Value = item.xallp;
                            //parameters[23] = new SQLiteParameter("unitname", DbType.String);
                            //parameters[23].Value = item.unitname;
                            //parameters[24] = new SQLiteParameter("unitrate", DbType.Decimal);
                            //parameters[24].Value = item.unitrate;
                            //parameters[25] = new SQLiteParameter("unitquat", DbType.Decimal);
                            //parameters[25].Value = item.unitquat;
                            //parameters[26] = new SQLiteParameter("xsalestype", DbType.String);
                            //parameters[26].Value = item.xsalestype;
                            //parameters[27] = new SQLiteParameter("xsalesid", DbType.Int32);
                            //parameters[27].Value = item.xsalesid;
                            //parameters[28] = new SQLiteParameter("xversion", DbType.Double);
                            //parameters[28].Value = GetTimeStamp();
                            #endregion

                            AddPosbb(entity, out parameters, out cmdText, item);

                            cmd.CommandText = cmdText.ToString();
                            cmd.Parameters.AddRange(parameters);
                            cmd.ExecuteNonQuery();
                        }
                        #endregion

                        #region 支付明细
                        foreach (BillpaytModel item in entity.payts)
                        {
                            cmd.Parameters.Clear();

                            AddBillpayt(entity, out parameters, out cmdText, item);

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

        #region 退货
        /// <summary>
        /// 退货
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public bool Returned(PoshhModel entity, Guid ID)
        {
            if (entity.Posbbs == null || entity.Posbbs.Count == 0)
            {
                return false;
            }
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
                        SQLiteParameter[] parameters = new SQLiteParameter[22];

                        StringBuilder cmdText = new StringBuilder();
                        cmdText.AppendLine("INSERT INTO poshh(");
                        cmdText.AppendLine("ID,billno,posnono,xstate,paytype,clntcode,clntname,");
                        cmdText.AppendLine("xnote,xls,xlsname,xinname,xintime,pbillno,xdate,");
                        cmdText.AppendLine("xheallp,xpay,xhezhe,xhenojie,transno,xversion,xsendjf,xrpay)");
                        cmdText.AppendLine("VALUES(");
                        cmdText.AppendLine("@ID,@billno,@posnono,@xstate,@paytype,@clntcode,@clntname,");
                        cmdText.AppendLine("@xnote,@xls,@xlsname,@xinname,@xintime,@pbillno,@xdate,");
                        cmdText.AppendLine("@xheallp,@xpay,@xhezhe,@xhenojie,@transno,@xversion,@xsendjf,@xrpay)");

                        entity.ID = Guid.NewGuid();
                        parameters[0] = new SQLiteParameter("ID", DbType.String);
                        parameters[0].Value = entity.ID;
                        parameters[1] = new SQLiteParameter("billno", DbType.String);
                        parameters[1].Value = GetPosbbNO();
                        parameters[2] = new SQLiteParameter("posnono", DbType.String);
                        parameters[2].Value = entity.posnono;
                        parameters[3] = new SQLiteParameter("xstate", DbType.String);
                        parameters[3].Value = entity.xstate;
                        parameters[4] = new SQLiteParameter("clntcode", DbType.String);
                        parameters[4].Value = entity.clntcode;
                        parameters[5] = new SQLiteParameter("clntname", DbType.String);
                        parameters[5].Value = entity.clntname;
                        parameters[6] = new SQLiteParameter("xnote", DbType.String);
                        parameters[6].Value = entity.xnote;
                        parameters[7] = new SQLiteParameter("xls", DbType.String);
                        parameters[7].Value = entity.xls;
                        parameters[8] = new SQLiteParameter("xlsname", DbType.String);
                        parameters[8].Value = entity.xlsname;
                        parameters[9] = new SQLiteParameter("xinname", DbType.String);
                        parameters[9].Value = entity.xinname;
                        parameters[10] = new SQLiteParameter("xintime", DbType.String);
                        parameters[10].Value = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                        parameters[11] = new SQLiteParameter("pbillno", DbType.String);
                        parameters[11].Value = entity.pbillno;
                        parameters[12] = new SQLiteParameter("xdate", DbType.DateTime);
                        parameters[12].Value = DateTime.Now.ToString("yyyy-MM-dd"); ;
                        parameters[13] = new SQLiteParameter("xheallp", DbType.Decimal);
                        parameters[13].Value = entity.xheallp;
                        parameters[14] = new SQLiteParameter("xpay", DbType.Decimal);
                        parameters[14].Value = entity.xpay;
                        parameters[15] = new SQLiteParameter("xhezhe", DbType.Decimal);
                        parameters[15].Value = entity.xhezhe;
                        parameters[16] = new SQLiteParameter("xhenojie", DbType.Decimal);
                        parameters[16].Value = entity.xhenojie;
                        parameters[17] = new SQLiteParameter("xversion", DbType.Double);
                        parameters[17].Value = GetTimeStamp();
                        parameters[18] = new SQLiteParameter("paytype", DbType.String);
                        parameters[18].Value = entity.paytype;
                        parameters[19] = new SQLiteParameter("transno", DbType.String);
                        parameters[19].Value = entity.transno;
                        parameters[20] = new SQLiteParameter("xsendjf", DbType.Int32);
                        parameters[20].Value = entity.xsendjf;
                        parameters[21] = new SQLiteParameter("xrpay", DbType.Decimal);
                        parameters[21].Value = entity.xrpay;

                        cmd.CommandText = cmdText.ToString();
                        cmd.Parameters.AddRange(parameters);
                        cmd.ExecuteNonQuery();

                        cmdText = new StringBuilder();
                        cmdText.AppendLine("update poshh set xversion=@xversion,uploadstatus=@uploadstatus where ID=@ID");
                        parameters = new SQLiteParameter[3];
                        parameters[0] = new SQLiteParameter("ID", DbType.String);
                        parameters[0].Value = ID;
                        parameters[1] = new SQLiteParameter("xversion", DbType.Double);
                        parameters[1].Value = GetTimeStamp();
                        parameters[2] = new SQLiteParameter("uploadstatus", DbType.String);
                        parameters[2].Value = (int)UploadStatus.NotUploaded;
                        #endregion

                        #region 表体
                        foreach (PosbbModel item in entity.Posbbs)
                        {
                            cmd.Parameters.Clear();

                            parameters = new SQLiteParameter[31];

                            cmdText = new StringBuilder();
                            cmdText.AppendLine("INSERT INTO posbb(");
                            cmdText.AppendLine("ID,XID,goodcode,goodname,goodunit,goodkind1,goodkind2,");
                            cmdText.AppendLine("goodkind3,goodkind4,goodkind5,goodkind6,goodkind7,goodkind8,goodkind9,goodkind10,");
                            cmdText.AppendLine("cnkucode,cnkuname,xquat,xpricold,xzhe,xpric,xallp,unitname,unitrate,unitquat,xtaxr,xtax,xprict,xallpt,xbarcode,xversion)");
                            cmdText.AppendLine("VALUES(");
                            cmdText.AppendLine("@ID,@XID,@goodcode,@goodname,@goodunit,@goodkind1,@goodkind2,");
                            cmdText.AppendLine("@goodkind3,@goodkind4,@goodkind5,@goodkind6,@goodkind7,@goodkind8,@goodkind9,@goodkind10,");
                            cmdText.AppendLine("@cnkucode,@cnkuname,@xquat,@xpricold,@xzhe,@xpric,@xallp,@unitname,@unitrate,@unitquat,@xtaxr,@xtax,@xprict,@xallpt,@xbarcode,@xversion)");

                            parameters[0] = new SQLiteParameter("ID", DbType.String);
                            parameters[0].Value = Guid.NewGuid();
                            parameters[1] = new SQLiteParameter("XID", DbType.String);
                            parameters[1].Value = entity.ID;
                            parameters[2] = new SQLiteParameter("goodcode", DbType.String);
                            parameters[2].Value = item.goodcode;
                            parameters[3] = new SQLiteParameter("goodname", DbType.String);
                            parameters[3].Value = item.goodname;
                            parameters[4] = new SQLiteParameter("goodunit", DbType.String);
                            parameters[4].Value = item.goodunit;
                            parameters[5] = new SQLiteParameter("goodkind1", DbType.String);
                            parameters[5].Value = item.goodkind1;
                            parameters[6] = new SQLiteParameter("goodkind2", DbType.String);
                            parameters[6].Value = item.goodkind2;
                            parameters[7] = new SQLiteParameter("goodkind3", DbType.String);
                            parameters[7].Value = item.goodkind3;
                            parameters[8] = new SQLiteParameter("goodkind4", DbType.String);
                            parameters[8].Value = item.goodkind4;
                            parameters[9] = new SQLiteParameter("goodkind5", DbType.String);
                            parameters[9].Value = item.goodkind5;
                            parameters[10] = new SQLiteParameter("goodkind6", DbType.String);
                            parameters[10].Value = item.goodkind6;
                            parameters[11] = new SQLiteParameter("goodkind7", DbType.String);
                            parameters[11].Value = item.goodkind7;
                            parameters[12] = new SQLiteParameter("goodkind8", DbType.String);
                            parameters[12].Value = item.goodkind8;
                            parameters[13] = new SQLiteParameter("goodkind9", DbType.String);
                            parameters[13].Value = item.goodkind9;
                            parameters[14] = new SQLiteParameter("goodkind10", DbType.String);
                            parameters[14].Value = item.goodkind10;
                            parameters[15] = new SQLiteParameter("cnkucode", DbType.String);
                            parameters[15].Value = item.cnkucode;
                            parameters[16] = new SQLiteParameter("cnkuname", DbType.String);
                            parameters[16].Value = item.cnkuname;
                            parameters[17] = new SQLiteParameter("xquat", DbType.Decimal);
                            parameters[17].Value = item.unitrate * item.unitquat;
                            parameters[18] = new SQLiteParameter("xpricold", DbType.Decimal);
                            parameters[18].Value = item.xpricold;
                            parameters[19] = new SQLiteParameter("xzhe", DbType.Decimal);
                            parameters[19].Value = item.xzhe;
                            parameters[20] = new SQLiteParameter("xpric", DbType.Decimal);
                            parameters[20].Value = item.xpric;
                            parameters[21] = new SQLiteParameter("xallp", DbType.Decimal);
                            parameters[21].Value = item.xallp;
                            parameters[22] = new SQLiteParameter("unitname", DbType.String);
                            parameters[22].Value = item.unitname;
                            parameters[23] = new SQLiteParameter("unitrate", DbType.Decimal);
                            parameters[23].Value = item.unitrate;
                            parameters[24] = new SQLiteParameter("unitquat", DbType.Decimal);
                            parameters[24].Value = item.xtquat;
                            parameters[25] = new SQLiteParameter("xtaxr", DbType.Decimal);
                            parameters[25].Value = item.xtaxr;
                            parameters[26] = new SQLiteParameter("xtax", DbType.Decimal);
                            parameters[26].Value = item.xtax;
                            parameters[27] = new SQLiteParameter("xprict", DbType.Decimal);
                            parameters[27].Value = item.xprict;
                            parameters[28] = new SQLiteParameter("xallpt", DbType.Decimal);
                            parameters[28].Value = item.xallpt;
                            parameters[29] = new SQLiteParameter("xbarcode", DbType.String);
                            parameters[29].Value = item.xbarcode;
                            parameters[30] = new SQLiteParameter("xversion", DbType.Double);
                            parameters[30].Value = GetTimeStamp();

                            cmd.CommandText = cmdText.ToString();
                            cmd.Parameters.AddRange(parameters);
                            cmd.ExecuteNonQuery();

                            //修改已退货数量
                            string sql = "update posbb set xtquat=@xtquat where ID=@ID";
                            parameters = new SQLiteParameter[2];
                            parameters[0] = new SQLiteParameter("ID", DbType.String);
                            parameters[0].Value = item.ID;
                            parameters[1] = new SQLiteParameter("xtquat", DbType.Decimal);
                            parameters[1].Value = item.xtquat;

                            cmd.CommandText = sql;
                            cmd.Parameters.AddRange(parameters);
                            cmd.ExecuteNonQuery();
                        }
                        #endregion

                        #region 支付明细
                        foreach (BillpaytModel item in entity.payts)
                        {
                            cmd.Parameters.Clear();

                            AddBillpayt(entity, out parameters, out cmdText, item);

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

        #region 快速开单
        /// <summary>
        /// 快速开单
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public bool FastBilling(PoshhModel entity, out string billno)
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
                        SQLiteParameter[] parameters = new SQLiteParameter[26];

                        StringBuilder cmdText = new StringBuilder();
                        cmdText.AppendLine("INSERT INTO poshh(");
                        cmdText.AppendLine("ID,billno,posnono,xstate,paytype,clntcode,clntname,");
                        cmdText.AppendLine("xnote,xls,xlsname,xinname,xintime,xterm,pbillno,xdate,");
                        cmdText.AppendLine("xheallp,xpay,xhezhe,xhenojie,transno,xrpay,xpoints,xversion,xsendjf,xhenojie,isClntDay)");
                        cmdText.AppendLine("VALUES(");
                        cmdText.AppendLine("@ID,@billno,@posnono,@xstate,@paytype,@clntcode,@clntname,");
                        cmdText.AppendLine("@xnote,@xls,@xlsname,@xinname,@xintime,@xterm,@pbillno,@xdate,");
                        cmdText.AppendLine("@xheallp,@xpay,@xhezhe,@xhenojie,@transno,@xrpay,@xpoints,@xversion,@xsendjf,@xhenojie,@isClntDay)");

                        billno = GetPosbbNO();
                        entity.ID = Guid.NewGuid();
                        parameters[0] = new SQLiteParameter("ID", DbType.String);
                        parameters[0].Value = entity.ID;
                        parameters[1] = new SQLiteParameter("billno", DbType.String);
                        parameters[1].Value = billno;
                        parameters[2] = new SQLiteParameter("posnono", DbType.String);
                        parameters[2].Value = entity.posnono;
                        parameters[3] = new SQLiteParameter("xstate", DbType.String);
                        parameters[3].Value = entity.xstate;
                        parameters[4] = new SQLiteParameter("clntcode", DbType.String);
                        parameters[4].Value = entity.clntcode;
                        parameters[5] = new SQLiteParameter("clntname", DbType.String);
                        parameters[5].Value = entity.clntname;
                        parameters[6] = new SQLiteParameter("xnote", DbType.String);
                        parameters[6].Value = entity.xnote;
                        parameters[7] = new SQLiteParameter("xls", DbType.String);
                        parameters[7].Value = entity.xls;
                        parameters[8] = new SQLiteParameter("xlsname", DbType.String);
                        parameters[8].Value = entity.xlsname;
                        parameters[9] = new SQLiteParameter("xinname", DbType.String);
                        parameters[9].Value = entity.xinname;
                        parameters[10] = new SQLiteParameter("xintime", DbType.String);
                        parameters[10].Value = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                        parameters[11] = new SQLiteParameter("xterm", DbType.String);
                        parameters[11].Value = entity.xstate == (stateDic[Enum.GetName(typeof(PosState), PosState.Deal)]) ? DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") : string.Empty;
                        parameters[12] = new SQLiteParameter("pbillno", DbType.String);
                        parameters[12].Value = entity.pbillno;
                        parameters[13] = new SQLiteParameter("xdate", DbType.DateTime);
                        parameters[13].Value = DateTime.Now.ToString("yyyy-MM-dd");
                        parameters[14] = new SQLiteParameter("xheallp", DbType.Decimal);
                        parameters[14].Value = entity.xheallp;
                        parameters[15] = new SQLiteParameter("xpay", DbType.Decimal);
                        parameters[15].Value = entity.xpay;
                        parameters[16] = new SQLiteParameter("xhezhe", DbType.Decimal);
                        parameters[16].Value = entity.xhezhe;
                        parameters[17] = new SQLiteParameter("xhenojie", DbType.Decimal);
                        parameters[17].Value = entity.xhenojie;
                        parameters[18] = new SQLiteParameter("xversion", DbType.Double);
                        parameters[18].Value = GetTimeStamp();
                        parameters[19] = new SQLiteParameter("paytype", DbType.String);
                        parameters[19].Value = entity.paytype;
                        parameters[20] = new SQLiteParameter("transno", DbType.String);
                        parameters[20].Value = entity.transno;
                        parameters[21] = new SQLiteParameter("xpoints", DbType.Decimal);
                        parameters[21].Value = entity.xpoints;
                        parameters[22] = new SQLiteParameter("xrpay", DbType.Decimal);
                        parameters[22].Value = entity.xrpay;
                        parameters[23] = new SQLiteParameter("xsendjf", DbType.Int32);
                        parameters[23].Value = entity.xsendjf;
                        parameters[24] = new SQLiteParameter("xhenojie", DbType.Decimal);
                        parameters[24].Value = entity.xhenojie;
                        parameters[25] = new SQLiteParameter("isClntDay", DbType.Boolean);
                        parameters[25].Value = entity.isClntDay;

                        cmd.CommandText = cmdText.ToString();
                        cmd.Parameters.AddRange(parameters);
                        cmd.ExecuteNonQuery();
                        #endregion

                        #region 支付明细
                        if (entity.payts != null)
                        {
                            foreach (BillpaytModel item in entity.payts)
                            {
                                cmd.Parameters.Clear();

                                AddBillpayt(entity, out parameters, out cmdText, item);

                                cmd.CommandText = cmdText.ToString();
                                cmd.Parameters.AddRange(parameters);
                                cmd.ExecuteNonQuery();
                            }
                            Dictionary<string, string> payTypeDic = EnumHelper.GetEnumDictionary(typeof(PayType));

                            var coupon = entity.payts.Where(r => r.paytname == payTypeDic[Enum.GetName(typeof(PayType), PayType.Coupon)]).FirstOrDefault();
                            if (coupon != null)
                            {
                                parameters = new SQLiteParameter[5];

                                string sql = @"UPDATE tickoffmx set clntcode=@clntcode,clntname=@clntname,xstate=@xstate,xopusetime=@xopusetime
                                               where lower(xcode)=@xcode";

                                parameters[0] = new SQLiteParameter("clntcode", DbType.String);
                                parameters[0].Value = entity.clntcode;
                                parameters[1] = new SQLiteParameter("clntname", DbType.String);
                                parameters[1].Value = entity.clntname;
                                parameters[2] = new SQLiteParameter("xstate", DbType.String);
                                parameters[2].Value = (couponStateDic[Enum.GetName(typeof(CouponState), CouponState.Used)]);
                                parameters[3] = new SQLiteParameter("xopusetime", DbType.String);
                                parameters[3].Value = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                                parameters[4] = new SQLiteParameter("xcode", DbType.String);
                                parameters[4].Value = coupon.xnote1.ToLower();

                                cmd.CommandText = sql;
                                cmd.Parameters.AddRange(parameters);
                                cmd.ExecuteNonQuery();
                            }
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

        #region 快速开单（追加）
        /// <summary>
        /// 快速开单（追加）
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public bool FastBillingAppend(PoshhModel entity)
        {
            if (entity.Posbbs == null || entity.Posbbs.Count == 0)
            {
                return false;
            }
            Dictionary<string, string> stateDic = EnumHelper.GetEnumDictionary(typeof(PosState));

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
                        SQLiteParameter[] parameters = new SQLiteParameter[4];

                        StringBuilder cmdText = new StringBuilder();
                        cmdText.AppendLine("update poshh set xstate=@xstate,xversion=@xversion,uploadstatus=@uploadstatus where ID=@ID");

                        parameters[0] = new SQLiteParameter("ID", DbType.String);
                        parameters[0].Value = entity.ID;
                        parameters[1] = new SQLiteParameter("xstate", DbType.String);
                        parameters[1].Value = stateDic[Enum.GetName(typeof(PosState), PosState.Deal)];
                        parameters[2] = new SQLiteParameter("xversion", DbType.Double);
                        parameters[2].Value = GetTimeStamp();
                        parameters[3] = new SQLiteParameter("uploadstatus", DbType.String);
                        parameters[3].Value = (int)UploadStatus.NotUploaded;

                        cmd.CommandText = cmdText.ToString();
                        cmd.Parameters.AddRange(parameters);
                        cmd.ExecuteNonQuery();
                        #endregion

                        #region 表体
                        foreach (PosbbModel item in entity.Posbbs)
                        {
                            cmd.Parameters.Clear();
                            AddPosbb(entity, out parameters, out cmdText, item);

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

        #region 换货
        /// <summary>
        /// 换货
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public bool ChangeGoods(PoshhModel entity, Guid ID, out string billno)
        {
            if (entity.Posbbs == null || entity.Posbbs.Count == 0)
            {
                billno = string.Empty;
                return false;
            }
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
                        SQLiteParameter[] parameters = new SQLiteParameter[28];

                        StringBuilder cmdText = new StringBuilder();
                        cmdText.AppendLine("INSERT INTO poshh(");
                        cmdText.AppendLine("ID,billno,posnono,xstate,paytype,clntcode,clntname,");
                        cmdText.AppendLine("xnote,xls,xlsname,xinname,xintime,xterm,pbillno,xdate,");
                        cmdText.AppendLine("xheallp,xpay,xhezhe,xhenojie,transno,xrpay,xpoints,xversion,xsendjf,xhenojie,pbillno,deductiblecash,isClntDay)");
                        cmdText.AppendLine("VALUES(");
                        cmdText.AppendLine("@ID,@billno,@posnono,@xstate,@paytype,@clntcode,@clntname,");
                        cmdText.AppendLine("@xnote,@xls,@xlsname,@xinname,@xintime,@xterm,@pbillno,@xdate,");
                        cmdText.AppendLine("@xheallp,@xpay,@xhezhe,@xhenojie,@transno,@xrpay,@xpoints,@xversion,@xsendjf,@xhenojie,@pbillno,@deductiblecash,@isClntDay)");

                        billno = GetPosbbNO();
                        entity.ID = Guid.NewGuid();
                        parameters[0] = new SQLiteParameter("ID", DbType.String);
                        parameters[0].Value = entity.ID;
                        parameters[1] = new SQLiteParameter("billno", DbType.String);
                        parameters[1].Value = billno;
                        parameters[2] = new SQLiteParameter("posnono", DbType.String);
                        parameters[2].Value = entity.posnono;
                        parameters[3] = new SQLiteParameter("xstate", DbType.String);
                        parameters[3].Value = stateDic[Enum.GetName(typeof(PosState), PosState.Change)];
                        parameters[4] = new SQLiteParameter("clntcode", DbType.String);
                        parameters[4].Value = entity.clntcode;
                        parameters[5] = new SQLiteParameter("clntname", DbType.String);
                        parameters[5].Value = entity.clntname;
                        parameters[6] = new SQLiteParameter("xnote", DbType.String);
                        parameters[6].Value = entity.xnote;
                        parameters[7] = new SQLiteParameter("xls", DbType.String);
                        parameters[7].Value = entity.xls;
                        parameters[8] = new SQLiteParameter("xlsname", DbType.String);
                        parameters[8].Value = entity.xlsname;
                        parameters[9] = new SQLiteParameter("xinname", DbType.String);
                        parameters[9].Value = entity.xinname;
                        parameters[10] = new SQLiteParameter("xintime", DbType.String);
                        parameters[10].Value = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                        parameters[11] = new SQLiteParameter("xterm", DbType.String);
                        parameters[11].Value = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                        parameters[12] = new SQLiteParameter("pbillno", DbType.String);
                        parameters[12].Value = entity.pbillno;
                        parameters[13] = new SQLiteParameter("xdate", DbType.DateTime);
                        parameters[13].Value = DateTime.Now.ToString("yyyy-MM-dd");
                        parameters[14] = new SQLiteParameter("xheallp", DbType.Decimal);
                        parameters[14].Value = entity.xheallp;
                        parameters[15] = new SQLiteParameter("xpay", DbType.Decimal);
                        parameters[15].Value = entity.xpay;
                        parameters[16] = new SQLiteParameter("xhezhe", DbType.Decimal);
                        parameters[16].Value = entity.xhezhe;
                        parameters[17] = new SQLiteParameter("xhenojie", DbType.Decimal);
                        parameters[17].Value = entity.xhenojie;
                        parameters[18] = new SQLiteParameter("xversion", DbType.Double);
                        parameters[18].Value = GetTimeStamp();
                        parameters[19] = new SQLiteParameter("paytype", DbType.String);
                        parameters[19].Value = entity.paytype;
                        parameters[20] = new SQLiteParameter("transno", DbType.String);
                        parameters[20].Value = entity.transno;
                        parameters[21] = new SQLiteParameter("xpoints", DbType.Decimal);
                        parameters[21].Value = entity.xpoints;
                        parameters[22] = new SQLiteParameter("xrpay", DbType.Decimal);
                        parameters[22].Value = entity.xrpay;
                        parameters[23] = new SQLiteParameter("xsendjf", DbType.Int32);
                        parameters[23].Value = entity.xsendjf;
                        parameters[24] = new SQLiteParameter("xhenojie", DbType.Decimal);
                        parameters[24].Value = entity.xhenojie;
                        parameters[25] = new SQLiteParameter("pbillno", DbType.String);
                        parameters[25].Value = entity.pbillno;
                        parameters[26] = new SQLiteParameter("deductiblecash", DbType.Decimal);
                        parameters[26].Value = entity.deductiblecash;
                        parameters[27] = new SQLiteParameter("isClntDay", DbType.Boolean);
                        parameters[27].Value = entity.isClntDay;


                        cmd.CommandText = cmdText.ToString();
                        cmd.Parameters.AddRange(parameters);
                        cmd.ExecuteNonQuery();
                        #endregion

                        //修改换货状态
                        //cmdText = new StringBuilder();
                        //cmdText.AppendLine("update poshh set xstate=@xstate, xversion=@xversion,uploadstatus=@uploadstatus where ID=@ID");
                        //parameters = new SQLiteParameter[4];
                        //parameters[0] = new SQLiteParameter("ID", DbType.String);
                        //parameters[0].Value = ID;
                        //parameters[1] = new SQLiteParameter("xstate", DbType.String);
                        //parameters[1].Value = stateDic[Enum.GetName(typeof(PosState), PosState.In_Storage)];
                        //parameters[2] = new SQLiteParameter("xversion", DbType.Double);
                        //parameters[2].Value = GetTimeStamp();
                        //parameters[3] = new SQLiteParameter("uploadstatus", DbType.String);
                        //parameters[3].Value = (int)UploadStatus.NotUploaded; ;
                        //cmd.CommandText = cmdText.ToString();
                        //cmd.Parameters.AddRange(parameters);
                        //cmd.ExecuteNonQuery();

                        #region 表体
                        foreach (PosbbModel item in entity.Posbbs)
                        {
                            cmd.Parameters.Clear();
                            item.ID = Guid.Empty;
                            AddPosbb(entity, out parameters, out cmdText, item);

                            cmd.CommandText = cmdText.ToString();
                            cmd.Parameters.AddRange(parameters);
                            cmd.ExecuteNonQuery();
                        }
                        #endregion

                        #region 支付明细
                        if (entity.payts != null)
                        {
                            foreach (BillpaytModel item in entity.payts)
                            {
                                cmd.Parameters.Clear();

                                AddBillpayt(entity, out parameters, out cmdText, item);

                                cmd.CommandText = cmdText.ToString();
                                cmd.Parameters.AddRange(parameters);
                                cmd.ExecuteNonQuery();
                            }
                            Dictionary<string, string> payTypeDic = EnumHelper.GetEnumDictionary(typeof(PayType));
                            var coupon = entity.payts.Where(r => r.paytname == payTypeDic[Enum.GetName(typeof(PayType), PayType.Coupon)]).FirstOrDefault();
                            if (coupon != null)
                            {
                                parameters = new SQLiteParameter[5];

                                string sql = @"UPDATE tickoffmx set clntcode=@clntcode,clntname=@clntname,xstate=@xstate,xopusetime=@xopusetime
                                               where lower(xcode)=@xcode";

                                parameters[0] = new SQLiteParameter("clntcode", DbType.String);
                                parameters[0].Value = entity.clntcode;
                                parameters[1] = new SQLiteParameter("clntname", DbType.String);
                                parameters[1].Value = entity.clntname;
                                parameters[2] = new SQLiteParameter("xstate", DbType.String);
                                parameters[2].Value = (couponStateDic[Enum.GetName(typeof(CouponState), CouponState.Used)]);
                                parameters[3] = new SQLiteParameter("xopusetime", DbType.String);
                                parameters[3].Value = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                                parameters[4] = new SQLiteParameter("xcode", DbType.String);
                                parameters[4].Value = coupon.xnote1.ToLower();

                                cmd.CommandText = sql;
                                cmd.Parameters.AddRange(parameters);
                                cmd.ExecuteNonQuery();
                            }
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
    }
}
