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
    /// 当班记录
    /// </summary>
    public class PosBanDAL : BaseDAL
    {
        #region 添加当班记录
        /// <summary>
        /// 添加当班记录
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public bool AddPosBan(PosbanModel entity)
        {
            try
            {
                //找上期结留
                string sql = "select xjiehav from posban order by datetime(xtime2) desc limit 1";

                decimal? xjiehav = (decimal?)SQLiteHelper.ExecuteScalar(SQLiteHelper.connectionString, CommandType.Text, sql);

                SQLiteParameter[] parameters = new SQLiteParameter[15];

                StringBuilder cmdText = new StringBuilder();
                cmdText.AppendLine("INSERT INTO posban(");
                cmdText.AppendLine("ID,xposset,posnono,posposi,posbcode,xopcode,xopname,xtime1,");
                cmdText.AppendLine("xjielst,xjiepos,xjienow,xjiehav,xjieget,xjieok,xversion)");
                cmdText.AppendLine("VALUES(");
                cmdText.AppendLine("@ID,@xposset,@posnono,@posposi,@posbcode,@xopcode,@xopname,@xtime1,");
                cmdText.AppendLine("@xjielst,@xjiepos,@xjienow,@xjiehav,@xjieget,@xjieok,@xversion)");

                parameters[0] = new SQLiteParameter("xposset", DbType.String);
                parameters[0].Value = entity.xposset;
                parameters[1] = new SQLiteParameter("posnono", DbType.String);
                parameters[1].Value = entity.posnono;
                parameters[2] = new SQLiteParameter("posposi", DbType.String);
                parameters[2].Value = entity.posposi;
                parameters[3] = new SQLiteParameter("posbcode", DbType.String);
                parameters[3].Value = entity.posbcode;
                parameters[4] = new SQLiteParameter("xopcode", DbType.String);
                parameters[4].Value = entity.xopcode;
                parameters[5] = new SQLiteParameter("xopname", DbType.String);
                parameters[5].Value = entity.xopname;
                parameters[6] = new SQLiteParameter("xtime1", DbType.String);
                parameters[6].Value = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                parameters[7] = new SQLiteParameter("xjielst", DbType.Decimal);
                parameters[7].Value = xjiehav ?? 0;
                parameters[8] = new SQLiteParameter("xjiepos", DbType.Decimal);
                parameters[8].Value = 0;
                parameters[9] = new SQLiteParameter("xjienow", DbType.Decimal);
                parameters[9].Value = entity.xjienow;
                parameters[10] = new SQLiteParameter("xjiehav", DbType.Decimal);
                parameters[10].Value = 0;
                parameters[11] = new SQLiteParameter("xjieget", DbType.Decimal);
                parameters[11].Value = 0;
                parameters[12] = new SQLiteParameter("xjieok", DbType.Boolean);
                parameters[12].Value = false;
                parameters[13] = new SQLiteParameter("xversion", DbType.Double);
                parameters[13].Value = GetTimeStamp();
                parameters[14] = new SQLiteParameter("ID", DbType.String);
                parameters[14].Value = Guid.NewGuid();

                int result = SQLiteHelper.ExecuteNonQuery(SQLiteHelper.connectionString, CommandType.Text, cmdText.ToString(), parameters);

                return result > 0;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region 修改当班记录
        /// <summary>
        /// 修改当班记录
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public bool UpdatePosban(PosbanModel entity)
        {
            try
            {
                SQLiteParameter[] parameters = new SQLiteParameter[6];

                string cmdText = @"update posban set xtime2=@xtime2,xjiepos=@xjiepos,xjiehav=@xjiehav,xjieget=@xjieget,
                                   xjieok=@xjieok where posnono=@posnono";

                parameters[0] = new SQLiteParameter("xtime2", DbType.String);
                parameters[0].Value = entity.xtime2;
                parameters[1] = new SQLiteParameter("xjiepos", DbType.Decimal);
                parameters[1].Value = entity.xjiepos;
                parameters[2] = new SQLiteParameter("xjiehav", DbType.Decimal);
                parameters[2].Value = entity.xjiehav;
                parameters[3] = new SQLiteParameter("xjieget", DbType.Decimal);
                parameters[3].Value = entity.xjieget;
                parameters[4] = new SQLiteParameter("xjieok", DbType.Boolean);
                parameters[4].Value = entity.xjieok;
                parameters[5] = new SQLiteParameter("posnono", DbType.String);
                parameters[5].Value = entity.posnono;

                int result = SQLiteHelper.ExecuteNonQuery(SQLiteHelper.connectionString, CommandType.Text, cmdText, parameters);

                return result > 0;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 修改当班记录
        /// </summary>
        /// <param name="reduceMoney">减少金额</param>
        /// <param name="posnono">单据的当班号</param>
        /// <returns></returns>
        public bool UpdatePosban(decimal reduceMoney,string posnono)
        {
            try
            {
                SQLiteParameter[] parameters = new SQLiteParameter[3];

                string cmdText = @"update posban set xjiepos=xjiepos-@reduceMoney,xjieget=@reduceMoney,uploadstatus=@uploadstatus where posnono=@posnono";

                parameters[0] = new SQLiteParameter("reduceMoney", DbType.Decimal);
                parameters[0].Value = reduceMoney;
                parameters[1] = new SQLiteParameter("posnono", DbType.String);
                parameters[1].Value = posnono;
                parameters[2] = new SQLiteParameter("uploadstatus", DbType.String);
                parameters[2].Value = (int)UploadStatus.NotUploaded;

                int result = SQLiteHelper.ExecuteNonQuery(SQLiteHelper.connectionString, CommandType.Text, cmdText, parameters);

                return result > 0;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region 获取单条当班记录
        /// <summary>
        /// 获取单条当班记录
        /// </summary>
        /// <param name="posnono"></param>
        /// <returns>当班单号</returns>
        public PosbanModel GetPosbanByNO(string posnono)
        {
            SQLiteParameter[] parameters = new SQLiteParameter[1];
            StringBuilder cmdText = new StringBuilder();
            cmdText.AppendLine("select xtime1,xjieok");
            cmdText.AppendLine("from posban");
            cmdText.AppendLine("where posnono =@posnono");
            parameters[0] = new SQLiteParameter("posnono", DbType.String);
            parameters[0].Value = posnono;
            SQLiteDataReader dataReader = null;
            try
            {
                dataReader = SQLiteHelper.ExecuteReader(SQLiteHelper.connectionString, CommandType.Text, cmdText.ToString(), parameters);
                PosbanModel posban = null;
                while (dataReader.Read())
                {
                    posban = new PosbanModel();
                    posban.xtime1 = dataReader["xtime1"].ToString();
                    posban.xjieok = bool.Parse(dataReader["xjieok"].ToString());
                }
                return posban;
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

        #region 判断是否交班
        /// <summary>
        /// 判断是否交班
        /// </summary>
        /// <returns>不为空为未交班</returns>
        public string IsShift()
        {
            string sql = "select posnono from posban where xjieok=0";
            string posnono = (string)SQLiteHelper.ExecuteScalar(SQLiteHelper.connectionString, CommandType.Text, sql);
            return posnono;
        }
        #endregion

        #region 获取交班详情
        /// <summary>
        ///获取交班详情
        /// </summary>
        /// <returns></returns>

        public PosbanDetailModel GetPosbanDetail(string posnono, out int appendCount)
        {
            PosbanDetailModel entity = new PosbanDetailModel();

            Dictionary<string, string> stateDic = EnumHelper.GetEnumDictionary(typeof(PosState));
            Dictionary<string, string> payTypeDic = EnumHelper.GetEnumDictionary(typeof(PayType));
            string sql = string.Empty;
            SQLiteParameter[] parameters = null;

            sql = "select a.ID,a.xstate,b.paytname as paytype,b.xpay,a.xhenojie,b.xnote1,a.deductiblecash from poshh a left join billpayt b on a.ID = b.XID where posnono=@posnono";
            parameters = new SQLiteParameter[1];
            parameters[0] = new SQLiteParameter("posnono", DbType.String);
            parameters[0].Value = posnono;

            SQLiteDataReader dataReader = SQLiteHelper.ExecuteReader(SQLiteHelper.connectionString, CommandType.Text, sql, parameters);

            List<PoshhModel> poshhs = new List<PoshhModel>();

            while (dataReader.Read())
            {
                PoshhModel poshh = new PoshhModel();
                poshh.ID = new Guid(dataReader["ID"].ToString().Trim());
                poshh.xstate = dataReader["xstate"].ToString().Trim();
                poshh.paytype = dataReader["paytype"].ToString().Trim();
                poshh.xpay = string.IsNullOrEmpty(dataReader["xpay"].ToString()) ? 0 : decimal.Parse(dataReader["xpay"].ToString().Trim());
                poshh.xhenojie = string.IsNullOrEmpty(dataReader["xhenojie"].ToString()) ? 0 : decimal.Parse(dataReader["xhenojie"].ToString().Trim());
                poshh.xnote = dataReader["xnote1"].ToString().Trim();
                poshh.deductiblecash = (dataReader["deductiblecash"] == null || dataReader["deductiblecash"].ToString() == string.Empty) ? 0 : decimal.Parse(dataReader["deductiblecash"].ToString().Trim());
                poshhs.Add(poshh);
            }
            dataReader.Close();
            appendCount = poshhs.Where(r => r.xstate == stateDic[Enum.GetName(typeof(PosState), PosState.Additional)]).Count();

            //退货总额
            var refundQuery = poshhs.Where(r => r.xstate == stateDic[Enum.GetName(typeof(PosState), PosState.Returned)]).ToList();
            decimal totalRefund = refundQuery.Sum(r => r.xpay);
            List<string> posStates = new List<string>();
            posStates.Add(stateDic[Enum.GetName(typeof(PosState), PosState.Deal)]);
            posStates.Add(stateDic[Enum.GetName(typeof(PosState), PosState.Change)]);

            var query_Cash = poshhs.Where(r => posStates.Contains(r.xstate)
                                && r.paytype == payTypeDic[Enum.GetName(typeof(PayType), PayType.Cash)]).ToList();
            entity.TotalCash = query_Cash.Sum(r => r.xpay)
                               - refundQuery.Where(r => r.paytype == payTypeDic[Enum.GetName(typeof(PayType), PayType.Cash)]).Sum(r => r.xpay);
            entity.CashCount = query_Cash.Select(r=>r.ID).Distinct().Count();

            var query_Deposit = poshhs.Where(r => posStates.Contains(r.xstate)
                             && r.paytype == payTypeDic[Enum.GetName(typeof(PayType), PayType.Deposit)] && r.xnote != "积分兑换").ToList();

            entity.TotalDeposit = query_Deposit.Sum(r => r.xpay)
                             - refundQuery.Where(r => r.paytype == payTypeDic[Enum.GetName(typeof(PayType), PayType.Deposit)] && r.xnote != "积分兑换").Sum(r => r.xpay);
            entity.DepositCount = query_Deposit.Distinct().Count();

            var query_Deductible = poshhs.Where(r => posStates.Contains(r.xstate)
                               && r.paytype == payTypeDic[Enum.GetName(typeof(PayType), PayType.Deposit)] && r.xnote == "积分兑换").ToList();
            entity.Money_Deductible = query_Deductible.Sum(r => r.xpay)
                             - refundQuery.Where(r => r.paytype == payTypeDic[Enum.GetName(typeof(PayType), PayType.Deposit)] && r.xnote == "积分兑换").Sum(r => r.xpay);
            entity.Count_Deductible = query_Deductible.Select(r => r.ID).Distinct().Count();

            var query_WeChat = poshhs.Where(r => posStates.Contains(r.xstate)
                            && r.paytype == payTypeDic[Enum.GetName(typeof(PayType), PayType.WeChat)]).ToList();
            entity.TotalWeChat = query_WeChat.Sum(r => r.xpay)
                             - refundQuery.Where(r => r.paytype == payTypeDic[Enum.GetName(typeof(PayType), PayType.WeChat)]).Sum(r => r.xpay);
            entity.WeChatCount = query_WeChat.Distinct().Count();

            var query_Alipay = poshhs.Where(r => posStates.Contains(r.xstate)
                        && r.paytype == payTypeDic[Enum.GetName(typeof(PayType), PayType.Alipay)]).ToList();
            entity.TotalAlipay = query_Alipay.Sum(r => r.xpay)
                            - refundQuery.Where(r => r.paytype == payTypeDic[Enum.GetName(typeof(PayType), PayType.Alipay)]).Sum(r => r.xpay);
            entity.AlipayCount = query_Alipay.Distinct().Count();

            var query_UnionpayCard = poshhs.Where(r => posStates.Contains(r.xstate)
                       && r.xnote == payTypeDic[Enum.GetName(typeof(PayType), PayType.UnionpayCard)]).ToList();
            entity.TotalUnionpayCard = query_UnionpayCard.Sum(r => r.xpay)
                          - refundQuery.Where(r => r.xnote == payTypeDic[Enum.GetName(typeof(PayType), PayType.UnionpayCard)]).Sum(r => r.xpay);
            entity.UnionpayCardCount = query_UnionpayCard.Distinct().Count();
            var query_Check = poshhs.Where(r => posStates.Contains(r.xstate)
                      && r.paytype == payTypeDic[Enum.GetName(typeof(PayType), PayType.Check)]).ToList();
            entity.Check = query_Check.Sum(r => r.xpay)
                        - refundQuery.Where(r => r.paytype == payTypeDic[Enum.GetName(typeof(PayType), PayType.Check)]).Sum(r => r.xpay);
            entity.CheckCount = query_Check.Distinct().Count();

            var query_Coupon = poshhs.Where(r => posStates.Contains(r.xstate)
                      && r.paytype == payTypeDic[Enum.GetName(typeof(PayType), PayType.Coupon)]).ToList();
            entity.TotalCoupon = query_Coupon.Sum(r => r.xpay)
                          - refundQuery.Where(r => r.paytype == payTypeDic[Enum.GetName(typeof(PayType), PayType.Coupon)]).Sum(r => r.xpay);
            entity.TotalCouponQuantity = query_Coupon.Distinct().Count();

            var query_Debts= poshhs.Where(r => posStates.Contains(r.xstate) && r.xhenojie > 0).ToList();
            entity.Debts = query_Debts.Sum(r => r.xhenojie);
            entity.DebtsCount = query_Debts.Distinct().Count();

            entity.TotalAmount = poshhs.Where(r => posStates.Contains(r.xstate)).Sum(r => r.xpay) - totalRefund;
            entity.TotalRefund = totalRefund;
            entity.TotalInvalid = poshhs.Where(r => r.xstate == stateDic[Enum.GetName(typeof(PosState), PosState.Invalid)]).Sum(r => r.xpay);
            entity.PosCount = poshhs.Where(r => posStates.Contains(r.xstate)).Select(r => r.ID).Distinct().Count();
            entity.ReturnCount = poshhs.Where(r => r.xstate == stateDic[Enum.GetName(typeof(PosState), PosState.Returned)]).Select(r => r.ID).Distinct().Count();
            entity.InvalidCount = poshhs.Where(r => r.xstate == stateDic[Enum.GetName(typeof(PosState), PosState.Invalid)]).Select(r => r.ID).Distinct().Count();

            #region 挂单总数
            sql = "select count(*) from poshh where xstate=@xstate";
            parameters = new SQLiteParameter[1];
            parameters[0] = new SQLiteParameter("xstate", DbType.String);
            parameters[0].Value = stateDic[Enum.GetName(typeof(PosState), PosState.Pending)];
            entity.PendingCount = (long)SQLiteHelper.ExecuteScalar(SQLiteHelper.connectionString, CommandType.Text, sql, parameters);
            #endregion

            #region  促销统计
            sql = string.Format(@"select b.*,a.clntcode from poshh a
                    inner join posbb b on a.ID = b.XID
                    where posnono=@posnono and xstate in ('{0}')", string.Join("','", posStates));
            parameters = new SQLiteParameter[1];
            parameters[0] = new SQLiteParameter("posnono", DbType.String);
            parameters[0].Value = posnono;

            dataReader = SQLiteHelper.ExecuteReader(SQLiteHelper.connectionString, CommandType.Text, sql, parameters);

            List<PosbbModel> posbbs = new List<PosbbModel>();

            while (dataReader.Read())
            {
                PosbbModel posbb = new PosbbModel();
                posbb.XID = new Guid(dataReader["XID"].ToString().Trim());
                posbb.clntcode = dataReader["clntcode"].ToString().Trim();
                posbb.xquat = decimal.Parse(dataReader["xquat"].ToString().Trim());
                posbb.xtquat = (dataReader["xtquat"] == null || dataReader["xtquat"].ToString() == string.Empty) ? 0 : decimal.Parse(dataReader["xtquat"].ToString().Trim());
                posbb.xpoints = (dataReader["xpoints"] == null || dataReader["xpoints"].ToString() == string.Empty) ? (decimal?)null : decimal.Parse(dataReader["xpoints"].ToString().Trim());
                posbb.xpricold = decimal.Parse(dataReader["xpricold"].ToString().Trim());
                posbb.xzhe = decimal.Parse(dataReader["xzhe"].ToString().Trim());
                posbb.xpric = decimal.Parse(dataReader["xpric"].ToString().Trim());
                posbb.xsalestype = dataReader["xsalestype"].ToString().Trim();
                posbb.xsalesid = (dataReader["xsalesid"] == null || dataReader["xsalesid"].ToString() == string.Empty) ? (int?)null : int.Parse(dataReader["xsalesid"].ToString().Trim());
                posbbs.Add(posbb);
            }
            dataReader.Close();

            var query = posbbs.Where(r => r.clntcode == string.Empty && r.xsalesid.HasValue == false && r.xzhe < 100);
            entity.Money_Manual = query.Sum(r => (r.xpricold - r.xpric) * (r.xquat - r.xtquat.Value));
            entity.Quantity_Manual = query.Sum(r => (r.xquat - r.xtquat.Value));
            entity.Count_Manual = query.Select(r => r.XID).Distinct().Count();

            query = posbbs.Where(r => r.clntcode != string.Empty && r.xsalesid.HasValue == false && r.xzhe < 100);
            entity.Money_Clnt = query.Sum(r => (r.xpricold - r.xpric) * (r.xquat - r.xtquat.Value));
            entity.Quantity_Clnt = query.Sum(r => (r.xquat - r.xtquat.Value));
            entity.Count_Clnt = query.Select(r => r.XID).Distinct().Count();

            query = posbbs.Where(r => r.xsalesid.HasValue && r.xzhe < 100);
            entity.Money_Sale = query.Sum(r => (r.xpricold - r.xpric) * (r.xquat - r.xtquat.Value));
            entity.Quantity_Sale = query.Sum(r => (r.xquat - r.xtquat.Value));
            entity.Count_Sale = query.Select(r => r.XID).Distinct().Count();

            query = posbbs.Where(r => r.xpoints.HasValue && r.xpoints > 0);
            entity.Money_Exchange = query.Sum(r => r.xpricold * r.xquat);
            entity.Quantity_Exchange = query.Sum(r => (r.xquat - r.xtquat.Value));
            entity.Count_Exchange = query.Select(r => r.XID).Distinct().Count();
            #endregion

            #region  充值统计
            sql = @"select a.* from ofbb a
                    inner join ofhh b on a.XID = b.ID where b.xnote=@posnono";
            parameters = new SQLiteParameter[1];
            parameters[0] = new SQLiteParameter("posnono", DbType.String);
            parameters[0].Value = posnono;

            dataReader = SQLiteHelper.ExecuteReader(SQLiteHelper.connectionString, CommandType.Text, sql, parameters);

            List<OfbbModel> ofbbs = new List<OfbbModel>();

            while (dataReader.Read())
            {
                OfbbModel ofbb = new OfbbModel();
                ofbb.xztype = dataReader["xztype"].ToString().Trim();
                ofbb.xfee = decimal.Parse(dataReader["xfee"].ToString().Trim());
                ofbbs.Add(ofbb);
            }
            dataReader.Close();

            entity.Cash_Recharge = ofbbs.Where(r => r.xztype == payTypeDic[Enum.GetName(typeof(PayType), PayType.Cash)]).Sum(r => r.xfee);
            entity.WeChat_Recharge = ofbbs.Where(r => r.xztype == payTypeDic[Enum.GetName(typeof(PayType), PayType.WeChat)]).Sum(r => r.xfee);
            entity.Alipay_Recharge = ofbbs.Where(r => r.xztype == payTypeDic[Enum.GetName(typeof(PayType), PayType.Alipay)]).Sum(r => r.xfee);
            entity.UnionpayCard_Recharge = ofbbs.Where(r => r.xztype == payTypeDic[Enum.GetName(typeof(PayType), PayType.UnionpayCard)]).Sum(r => r.xfee);
            entity.Total_Recharge = entity.Cash_Recharge + entity.WeChat_Recharge + entity.Alipay_Recharge + entity.UnionpayCard_Recharge;
            #endregion
            return entity;
        }
        #endregion
    }
}
