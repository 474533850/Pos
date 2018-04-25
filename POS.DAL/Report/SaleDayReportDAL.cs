using POS.Common.Enum;
using POS.Common.utility;
using POS.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using System.Text;

namespace POS.DAL.Report
{
    /// <summary>
    /// 销售日结报表
    /// </summary>
    public class SaleDayReportDAL
    {
        #region 修改前
        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="startDate"></param>
        ///// <param name="endDate"></param>
        ///// <param name="debts">挂单金额</param>
        ///// <returns></returns>
        //public List<SaleReportModel> GetSaleDayReport(DateTime startDate, DateTime endDate, out decimal debts)
        //{
        //    Dictionary<string, string> stateDic = EnumHelper.GetEnumDictionary(typeof(PosState));
        //    //成交单据
        //    StringBuilder cmdText = new StringBuilder();
        //    //cmdText.AppendLine("select goodtype3,sum(xquat) as xquat,sum(xallp) as xallp,sum(xrpay) as xrpay from (");
        //    cmdText.AppendLine("select distinct b.ID,b.goodname,c.goodtype3, xstate,case when xstate=@xstate2 then -b.xquat else b.xquat end as xquat,");
        //    cmdText.AppendLine("case when xstate=@xstate2 then -b.xallp else b.xallp end as xallp,xrpay,a.xhenojie from poshh a");
        //    cmdText.AppendLine("inner join posbb b on a.ID = b.XID");
        //    cmdText.AppendLine("inner join good c on trim(b.goodcode) = trim(c.goodcode)");
        //    cmdText.AppendLine("where (xstate=@xstate1 or xstate=@xstate2) and datetime(xintime) >= @startDate and datetime(xintime)<@endDate and (b.xpoints is null or b.xpoints=0)");
        //    //  cmdText.AppendLine(")t group by goodtype3");

        //    SQLiteParameter[] parameters = new SQLiteParameter[4];
        //    parameters[0] = new SQLiteParameter("startDate", DbType.DateTime);
        //    parameters[0].Value = startDate.Date;
        //    parameters[1] = new SQLiteParameter("endDate", DbType.DateTime);
        //    parameters[1].Value = endDate.Date.AddDays(1); ;
        //    parameters[2] = new SQLiteParameter("xstate1", DbType.String);
        //    parameters[2].Value = stateDic[Enum.GetName(typeof(PosState), PosState.Deal)]; ;
        //    parameters[3] = new SQLiteParameter("xstate2", DbType.String);
        //    parameters[3].Value = stateDic[Enum.GetName(typeof(PosState), PosState.Returned)];

        //    SQLiteDataReader dataReader = SQLiteHelper.ExecuteReader(SQLiteHelper.connectionString, CommandType.Text, cmdText.ToString(), parameters);
        //    List<SaleReportModel> datas = new List<SaleReportModel>();
        //    while (dataReader.Read())
        //    {
        //        SaleReportModel entity = new SaleReportModel();
        //        entity.goodtype = dataReader["goodtype3"].ToString().Trim();
        //        entity.xstate = dataReader["xstate"].ToString().Trim();
        //        entity.Quantity = string.IsNullOrEmpty(dataReader["xquat"].ToString().Trim()) ? 0 : decimal.Parse(dataReader["xquat"].ToString().Trim());
        //        entity.Total = string.IsNullOrEmpty(dataReader["xallp"].ToString().Trim()) ? 0 : decimal.Parse(dataReader["xallp"].ToString().Trim());
        //        entity.xrpay = string.IsNullOrEmpty(dataReader["xrpay"].ToString().Trim()) ? 0:decimal.Parse(dataReader["xrpay"].ToString().Trim());
        //        entity.debts = string.IsNullOrEmpty(dataReader["xhenojie"].ToString().Trim()) ? 0 : decimal.Parse(dataReader["xhenojie"].ToString().Trim());
        //        datas.Add(entity);
        //    }
        //    debts = datas.Where(r => r.xstate == stateDic[Enum.GetName(typeof(PosState), PosState.Deal)] && r.debts > 0).Sum(r => r.debts);
        //    var query = (from p in datas
        //                 group p by new { p.goodtype } into g
        //                 select new SaleReportModel
        //                 {
        //                     goodtype = g.Key.goodtype,
        //                     Quantity = g.Sum(r => r.Quantity),
        //                     Total = g.Sum(r => r.Total),
        //                 }).ToList();
        //    return query;
        //}
        ///// <summary>
        ///// 日结支付明细
        ///// </summary>
        ///// <param name="startDate"></param>
        ///// <param name="endDate"></param>
        ///// <returns></returns>
        //public List<BillpaytModel> GetSaleDayBillpayt(DateTime startDate, DateTime endDate)
        //{
        //    Dictionary<string, string> stateDic = EnumHelper.GetEnumDictionary(typeof(PosState));
        //    //成交单据
        //    string cmdText = @"select paytname,case when xstate=@xstate1 then a.xpay when xstate=@xstate2 then -a.xpay end as xpay from billpayt a
        //                        inner join poshh b on a.XID = b.ID
        //                        where (xstate=@xstate1 or xstate=@xstate2)
        //                        and datetime(xintime) >= @startDate and datetime(xintime)<@endDate";

        //    SQLiteParameter[] parameters = new SQLiteParameter[4];
        //    parameters[0] = new SQLiteParameter("startDate", DbType.DateTime);
        //    parameters[0].Value = startDate.Date;
        //    parameters[1] = new SQLiteParameter("endDate", DbType.DateTime);
        //    parameters[1].Value = endDate.Date.AddDays(1); ;
        //    parameters[2] = new SQLiteParameter("xstate1", DbType.String);
        //    parameters[2].Value = stateDic[Enum.GetName(typeof(PosState), PosState.Deal)]; ;
        //    parameters[3] = new SQLiteParameter("xstate2", DbType.String);
        //    parameters[3].Value = stateDic[Enum.GetName(typeof(PosState), PosState.Returned)];

        //    SQLiteDataReader dataReader = SQLiteHelper.ExecuteReader(SQLiteHelper.connectionString, CommandType.Text, cmdText.ToString(), parameters);
        //    List<BillpaytModel> datas = new List<BillpaytModel>();
        //    while (dataReader.Read())
        //    {
        //        BillpaytModel entity = new BillpaytModel();
        //        entity.paytname = dataReader["paytname"].ToString().Trim();
        //        entity.xpay = decimal.Parse(dataReader["xpay"].ToString().Trim());
        //        datas.Add(entity);
        //    }
        //    return datas;
        //}
        #endregion

        /// <summary>
        /// 
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <param name="debts">挂单金额</param>
        /// <returns></returns>
        public List<SaleReportModel> GetSaleDayReport(DateTime startDate, DateTime endDate, out decimal debts)
        {
            Dictionary<string, string> stateDic = EnumHelper.GetEnumDictionary(typeof(PosState));
            List<string> state = new List<string>();
            state.Add(stateDic[Enum.GetName(typeof(PosState), PosState.Deal)]);
            state.Add(stateDic[Enum.GetName(typeof(PosState), PosState.Change)]);
            state.Add(stateDic[Enum.GetName(typeof(PosState), PosState.Returned)]);
            //成交单据
            StringBuilder cmdText = new StringBuilder();
            cmdText.AppendLine("select distinct b.ID,b.XID,b.goodname,c.goodtype3, xstate,case when xstate=@xstate2 then -b.xquat else b.xquat end as xquat,");
            cmdText.AppendLine("case when xstate=@xstate2 then -b.xallp else b.xallp end as xallp,xrpay,a.xhenojie from poshh a");
            cmdText.AppendLine("inner join posbb b on a.ID = b.XID");
            cmdText.AppendLine("inner join good c on trim(b.goodcode) = trim(c.goodcode)");
            cmdText.AppendFormat("where xstate in ('{0}') ", string.Join("','", state));
            cmdText.AppendLine("and datetime(xintime) >= @startDate and datetime(xintime)<@endDate and (b.xpoints is null or b.xpoints=0)");

            SQLiteParameter[] parameters = new SQLiteParameter[3];
            parameters[0] = new SQLiteParameter("startDate", DbType.DateTime);
            parameters[0].Value = startDate.Date;
            parameters[1] = new SQLiteParameter("endDate", DbType.DateTime);
            parameters[1].Value = endDate.Date.AddDays(1); ;
            //parameters[2] = new SQLiteParameter("xstate1", DbType.String);
            //parameters[2].Value = stateDic[Enum.GetName(typeof(PosState), PosState.Deal)];
            parameters[2] = new SQLiteParameter("xstate2", DbType.String);
            parameters[2].Value = stateDic[Enum.GetName(typeof(PosState), PosState.Returned)];

            SQLiteDataReader dataReader = SQLiteHelper.ExecuteReader(SQLiteHelper.connectionString, CommandType.Text, cmdText.ToString(), parameters);
            List<SaleReportModel> datas = new List<SaleReportModel>();
            while (dataReader.Read())
            {
                SaleReportModel entity = new SaleReportModel();
                entity.ID = new Guid(dataReader["XID"].ToString().Trim());
                entity.goodtype = dataReader["goodtype3"].ToString().Trim();
                entity.xstate = dataReader["xstate"].ToString().Trim();
                entity.Quantity = string.IsNullOrEmpty(dataReader["xquat"].ToString().Trim()) ? 0 : decimal.Parse(dataReader["xquat"].ToString().Trim());
                entity.Total = string.IsNullOrEmpty(dataReader["xallp"].ToString().Trim()) ? 0 : decimal.Parse(dataReader["xallp"].ToString().Trim());
                entity.xrpay = string.IsNullOrEmpty(dataReader["xrpay"].ToString().Trim()) ? 0 : decimal.Parse(dataReader["xrpay"].ToString().Trim());
                entity.debts = string.IsNullOrEmpty(dataReader["xhenojie"].ToString().Trim()) ? 0 : decimal.Parse(dataReader["xhenojie"].ToString().Trim());
                datas.Add(entity);
            }
            dataReader.Close();
            debts =(from p in datas.Where(r => r.xstate == stateDic[Enum.GetName(typeof(PosState), PosState.Deal)] && r.debts > 0) select new { ID=p.ID, debts =p.debts}).Distinct().Sum(r => r.debts);
            var query = (from p in datas
                         group p by new { p.goodtype } into g
                         select new SaleReportModel
                         {
                             goodtype = g.Key.goodtype,
                             Quantity = g.Sum(r => r.Quantity),
                             Total = g.Sum(r => r.Total),
                         }).ToList();
            return query;
        }
        /// <summary>
        /// 日结支付明细
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        public List<BillpaytModel> GetSaleDayBillpayt(DateTime startDate, DateTime endDate)
        {
            Dictionary<string, string> stateDic = EnumHelper.GetEnumDictionary(typeof(PosState));
            List<string> state = new List<string>();
            state.Add(stateDic[Enum.GetName(typeof(PosState), PosState.Deal)]);
            state.Add(stateDic[Enum.GetName(typeof(PosState), PosState.Change)]);
            state.Add(stateDic[Enum.GetName(typeof(PosState), PosState.Returned)]);
            //成交单据
            StringBuilder cmdText = new StringBuilder();
            cmdText.AppendLine("select paytname,case when xstate=@xstate2 then -a.xpay else a.xpay end as xpay,a.xnote1 from billpayt a");
            cmdText.AppendLine("inner join poshh b on a.XID = b.ID");
            cmdText.AppendFormat("where xstate in ('{0}') ", string.Join("','", state));
            cmdText.AppendLine("and datetime(xintime) >= @startDate and datetime(xintime)<@endDate");

            SQLiteParameter[] parameters = new SQLiteParameter[3];
            parameters[0] = new SQLiteParameter("startDate", DbType.DateTime);
            parameters[0].Value = startDate.Date;
            parameters[1] = new SQLiteParameter("endDate", DbType.DateTime);
            parameters[1].Value = endDate.Date.AddDays(1); ;
            //parameters[2] = new SQLiteParameter("xstate1", DbType.String);
            //parameters[2].Value = stateDic[Enum.GetName(typeof(PosState), PosState.Deal)];
            parameters[2] = new SQLiteParameter("xstate2", DbType.String);
            parameters[2].Value = stateDic[Enum.GetName(typeof(PosState), PosState.Returned)];

            SQLiteDataReader dataReader = SQLiteHelper.ExecuteReader(SQLiteHelper.connectionString, CommandType.Text, cmdText.ToString(), parameters);
            List<BillpaytModel> datas = new List<BillpaytModel>();
            while (dataReader.Read())
            {
                BillpaytModel entity = new BillpaytModel();
                entity.paytname = dataReader["paytname"].ToString().Trim();
                entity.xpay = decimal.Parse(dataReader["xpay"].ToString().Trim());
                entity.xnote1 = (dataReader["xnote1"].ToString().Trim());
                datas.Add(entity);
            }
            dataReader.Close();
            return datas;
        }
    }
}
