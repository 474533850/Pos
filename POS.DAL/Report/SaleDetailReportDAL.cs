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
    /// 零售明细表
    /// </summary>
    public class SaleDetailReportDAL
    {
        public List<SaleDetailReportModel> Get(string billNO, string goodKey, string clntKey, DateTime? startDate, DateTime? endDate, bool isAll)
        {
            SQLiteParameter[] parameters = null;

            Dictionary<string, string> stateDic = EnumHelper.GetEnumDictionary(typeof(PosState));
            List<string> state = new List<string>();
            state.Add(stateDic[Enum.GetName(typeof(PosState), PosState.Deal)]);
            state.Add(stateDic[Enum.GetName(typeof(PosState), PosState.Returned)]);
            state.Add(stateDic[Enum.GetName(typeof(PosState), PosState.Additional)]);
            state.Add(stateDic[Enum.GetName(typeof(PosState), PosState.Change)]);

            StringBuilder cmdText = new StringBuilder();
            cmdText.AppendFormat("select *,b.xpoints as xpointsb from poshh a ");
            cmdText.AppendLine("inner join posbb b on a.ID = b.XID ");
            cmdText.AppendFormat("where xstate in ('{0}') ", string.Join("','", state));
            if (!isAll)
            {
                cmdText.AppendLine(" and datetime(xintime) >= @startDate and datetime(xintime)<@endDate");
            }
            if (billNO != string.Empty)
            {
                cmdText.AppendFormat(" and lower(billno) like '{0}' ", "%" + billNO.ToLower() + "%");
            }

            if (goodKey != string.Empty)
            {
                cmdText.AppendFormat(" and (lower(b.goodcode) like '{0}' or lower(b.goodname) like  '{0}' or lower(b.xbarcode) like  '{0}')", "%" + goodKey.ToLower() + "%");
            }
            if (clntKey != string.Empty)
            {
                cmdText.AppendFormat(" and (lower(a.clntname) like '{0}' or lower(a.clntcode) like  '{0}')", "%" + clntKey.ToLower() + "%");
            }
            cmdText.AppendLine(" order by datetime(xintime) desc ");


            if (!isAll)
            {
                parameters = new SQLiteParameter[2];
                parameters[0] = new SQLiteParameter("startDate", DbType.DateTime);
                parameters[0].Value = startDate.Value.Date;
                parameters[1] = new SQLiteParameter("endDate", DbType.DateTime);
                parameters[1].Value = endDate.Value.Date.AddDays(1);
            }

            SQLiteDataReader dataReader = SQLiteHelper.ExecuteReader(SQLiteHelper.connectionString, CommandType.Text, cmdText.ToString(), parameters);

            List<SaleDetailReportModel> datas = new List<SaleDetailReportModel>();

            while (dataReader.Read())
            {
                SaleDetailReportModel entity = new SaleDetailReportModel();
                entity.xlsname = dataReader["xlsname"].ToString().Trim();
                entity.xinname = dataReader["xinname"].ToString().Trim();
                entity.xterm = dataReader["xterm"].ToString().Trim();
                entity.billno = dataReader["billno"].ToString().Trim();
                entity.xstate = dataReader["xstate"].ToString().Trim();
                entity.clntname = dataReader["clntname"].ToString().Trim();
                entity.xhezhe = decimal.Parse(dataReader["xhezhe"].ToString().Trim());
                entity.xpay = decimal.Parse(dataReader["xpay"].ToString().Trim());
                entity.xintime = DateTime.Parse(dataReader["xintime"].ToString()).ToString("MM-dd HH:mm");

                entity.goodcode = dataReader["goodcode"].ToString().Trim();
                entity.xbarcode = dataReader["xbarcode"].ToString().Trim();
                decimal xpoints = 0;
                if (decimal.TryParse(dataReader["xpointsb"].ToString().Trim(), out xpoints))
                {
                    entity.xpointsb = xpoints;
                    entity.goodname = dataReader["goodname"].ToString().Trim() + "【兑】";
                }
                else
                {
                    entity.goodname = dataReader["goodname"].ToString().Trim();
                }
                xpoints = 0;
                if (decimal.TryParse(dataReader["xpoints"].ToString().Trim(), out xpoints))
                {
                    entity.xpoints = xpoints;
                }
                int xsendjf = 0;
                if (int.TryParse(dataReader["xsendjf"].ToString().Trim(), out xsendjf))
                {
                    entity.xsendjf = xsendjf;
                }
                entity.goodunit = dataReader["goodunit"].ToString().Trim();
                entity.unitname = dataReader["unitname"].ToString().Trim();
                entity.unitrate = (dataReader["unitrate"] != null && dataReader["unitrate"].ToString().Trim() != string.Empty)
                    ? decimal.Parse(dataReader["unitrate"].ToString().Trim())
                    : (decimal?)null;
                entity.unitquat = (dataReader["unitquat"] != null && dataReader["unitquat"].ToString().Trim() != string.Empty)
                    ? decimal.Parse(dataReader["unitquat"].ToString().Trim())
                    : 0;
                entity.goodkind1 = dataReader["goodkind1"].ToString().Trim();
                entity.goodkind2 = dataReader["goodkind2"].ToString().Trim();
                entity.goodkind3 = dataReader["goodkind3"].ToString().Trim();
                entity.goodkind4 = dataReader["goodkind4"].ToString().Trim();
                entity.goodkind5 = dataReader["goodkind5"].ToString().Trim();
                entity.goodkind6 = dataReader["goodkind6"].ToString().Trim();
                entity.goodkind7 = dataReader["goodkind7"].ToString().Trim();
                entity.goodkind8 = dataReader["goodkind8"].ToString().Trim();
                entity.goodkind9 = dataReader["goodkind9"].ToString().Trim();
                entity.goodkind10 = dataReader["goodkind10"].ToString().Trim();
                StringBuilder goodtm = new StringBuilder();
                goodtm.Append(entity.goodkind1);
                goodtm.Append(entity.goodkind2);
                goodtm.Append(entity.goodkind3);
                goodtm.Append(entity.goodkind4);
                goodtm.Append(entity.goodkind5);
                goodtm.Append(entity.goodkind6);
                goodtm.Append(entity.goodkind7);
                goodtm.Append(entity.goodkind8);
                goodtm.Append(entity.goodkind9);
                goodtm.Append(entity.goodkind10);
                entity.goodtm = goodtm.ToString();
                entity.cnkucode = dataReader["cnkucode"].ToString().Trim();
                entity.cnkuname = dataReader["cnkuname"].ToString().Trim();
                entity.xquat = decimal.Parse(dataReader["xquat"].ToString().Trim());
                entity.xtquat = (dataReader["xtquat"] != null && dataReader["xtquat"].ToString().Trim() != string.Empty)
                    ? decimal.Parse(dataReader["xtquat"].ToString().Trim())
                    : (decimal?)null;
                entity.xpricold = decimal.Parse(dataReader["xpricold"].ToString().Trim());
                entity.xzhe = decimal.Parse(dataReader["xzhe"].ToString().Trim());
                entity.xpric = decimal.Parse(dataReader["xpric"].ToString().Trim());
                entity.xallp = decimal.Parse(dataReader["xallp"].ToString().Trim());
                entity.xsalestype = dataReader["xsalestype"].ToString().Trim();
                entity.xsalesid = (dataReader["xsalesid"] != null && dataReader["xsalesid"].ToString().Trim() != string.Empty)
                   ? int.Parse(dataReader["xsalesid"].ToString().Trim())
                   : (int?)null;
                entity.xpoints = (dataReader["xpoints"] != null && dataReader["xpoints"].ToString().Trim() != string.Empty)
                   ? decimal.Parse(dataReader["xpoints"].ToString().Trim())
                   : (decimal?)null;
                entity.xtaxr = (dataReader["xtaxr"] != null && dataReader["xtaxr"].ToString().Trim() != string.Empty)
                  ? decimal.Parse(dataReader["xtaxr"].ToString().Trim())
                  : 0;
                entity.xtax = (dataReader["xtax"] != null && dataReader["xtax"].ToString().Trim() != string.Empty)
                 ? decimal.Parse(dataReader["xtax"].ToString().Trim())
                 : 0;
                entity.xprict = (dataReader["xprict"] != null && dataReader["xprict"].ToString().Trim() != string.Empty)
                 ? decimal.Parse(dataReader["xprict"].ToString().Trim())
                 : 0;
                entity.xallpt = (dataReader["xallpt"] != null && dataReader["xallpt"].ToString().Trim() != string.Empty)
                ? decimal.Parse(dataReader["xallpt"].ToString().Trim())
                : 0;
                entity.xchg = dataReader["xchg"].ToString().Trim();
                entity.xtimes = (dataReader["xtimes"] != null && dataReader["xtimes"].ToString().Trim() != string.Empty)
                 ? decimal.Parse(dataReader["xtimes"].ToString().Trim())
                 : (decimal?)null;
                datas.Add(entity);
            }
            dataReader.Close();
            return datas;
        }
    }
}
