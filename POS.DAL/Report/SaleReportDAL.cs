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
    /// 销售报表
    /// </summary>
    public class SaleReportDAL
    {
        public List<SaleReportModel> GetSaleReport(string posnono, DateTime startDate, DateTime endDate)
        {
            Dictionary<string, string> stateDic = EnumHelper.GetEnumDictionary(typeof(PosState));
            List<string> state = new List<string>();
            state.Add(stateDic[Enum.GetName(typeof(PosState), PosState.Deal)]);
            state.Add(stateDic[Enum.GetName(typeof(PosState), PosState.Change)]);
            state.Add(stateDic[Enum.GetName(typeof(PosState), PosState.Returned)]);
            //成交单据
            StringBuilder cmdText = new StringBuilder();
            cmdText.AppendLine("select goodname,goodtype3,sum(xquat) as xquat,sum(xallp) as xallp from (");
            cmdText.AppendLine("select distinct b.ID,b.goodname,c.goodtype3, case when xstate=@xstate2 then -b.xquat else b.xquat end as xquat,");
            cmdText.AppendLine("case when xstate=@xstate2 then -b.xallp else case when b.xpoints>0 then 0 else b.xallp end end as xallp  from poshh a");
            //cmdText.AppendLine("case when xstate=@xstate1 then case when b.xpoints>0 then 0 else b.xallp end when xstate=@xstate2 then -b.xallp end as xallp  from poshh a");
            cmdText.AppendLine("inner join posbb b on a.ID = b.XID");
            cmdText.AppendLine("inner join good c on trim(b.goodcode) = trim(c.goodcode)");
            if (posnono != string.Empty)
            {
                cmdText.AppendLine("where posnono=@posnono ");
            }
            else
            {
                cmdText.AppendLine("where datetime(xintime) >= @startDate and datetime(xintime)<@endDate");
            }
            cmdText.AppendFormat("and xstate in ('{0}') ", string.Join("','", state));
            cmdText.AppendLine(")t group by goodname,goodtype3");

            SQLiteParameter[] parameters = null;
            if (posnono != string.Empty)
            {
                parameters = new SQLiteParameter[2];
                parameters[0] = new SQLiteParameter("posnono", DbType.String);
                parameters[0].Value = posnono;
                parameters[1] = new SQLiteParameter("xstate2", DbType.String);
                parameters[1].Value = stateDic[Enum.GetName(typeof(PosState), PosState.Returned)];
            }
            else
            {
                parameters = new SQLiteParameter[3];
                parameters[0] = new SQLiteParameter("xstate2", DbType.String);
                parameters[0].Value = stateDic[Enum.GetName(typeof(PosState), PosState.Returned)];
                parameters[1] = new SQLiteParameter("startDate", DbType.DateTime);
                parameters[1].Value = startDate.Date;
                parameters[2] = new SQLiteParameter("endDate", DbType.DateTime);
                parameters[2].Value = endDate.Date.AddDays(1); ;
            }

            SQLiteDataReader dataReader = SQLiteHelper.ExecuteReader(SQLiteHelper.connectionString, CommandType.Text, cmdText.ToString(), parameters);
            List<SaleReportModel> datas = new List<SaleReportModel>();
            while (dataReader.Read())
            {
                SaleReportModel entity = new SaleReportModel();
                entity.goodname = dataReader["goodname"].ToString().Trim();
                entity.goodtype = dataReader["goodtype3"].ToString().Trim();
                entity.Quantity = decimal.Parse(dataReader["xquat"].ToString().Trim());
                entity.Total = decimal.Parse(dataReader["xallp"].ToString().Trim());
                datas.Add(entity);
            }
            dataReader.Close();
            return datas;
        }
    }
}
