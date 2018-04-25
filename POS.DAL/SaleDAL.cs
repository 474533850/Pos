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
    /// 促销
    /// </summary>
    public class SaleDAL
    {
        #region 获取促销活动
        /// <summary>
        /// 获取促销活动
        /// </summary>
        /// <param name="clntCode">客户代码</param>
        /// <param name="goodcode">货品代码</param>
        /// <param name="xtableid">货品ID</param>
        /// <returns></returns>
        public SaleModel GetSale(string clntCode, GoodModel good, int xtableid)
        {
            StringBuilder cmdText = new StringBuilder();
            SQLiteParameter[] parameters = new SQLiteParameter[3];
            parameters[0] = new SQLiteParameter("dateTime", DbType.DateTime);
            parameters[0].Value = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            parameters[1] = new SQLiteParameter("goodcode", DbType.String);
            parameters[1].Value = good.goodcode;
            parameters[2] = new SQLiteParameter("xtableid", DbType.Int32);
            parameters[2].Value = xtableid;

            cmdText.AppendLine("select * from(");
            #region 促销活动货品
            //a类型没有sku
            cmdText.AppendLine("select a.*,c.goodcode,b.xintime from salegood a");
            cmdText.AppendLine("inner join sale b on a.xsaleid = b.xtableid");
            cmdText.AppendLine("inner join good c on a.xgoodid=c.xtableID");
            cmdText.AppendLine("left join goodpric d on c.xtableid = d.xsubid");
            cmdText.AppendLine("where d.SID is null and datetime(b.xtime1)<=@dateTime");
            cmdText.AppendLine("and datetime(b.xtime2)>=@dateTime and xstart=1");
            cmdText.AppendLine("and trim(c.goodcode)=@goodcode and a.xgtype='a' and a.xgoodid = @xtableid");
            cmdText.AppendLine("union all");
            //a类型下面所有的sku
            cmdText.AppendLine("select a.SID,a.xtype,a.xsaleid,a.xgtype,d.xtableid as xgoodid,a.xtableid,a.xsubid,a.xversion,c.goodcode,b.xintime from salegood a");
            cmdText.AppendLine("inner join sale b on a.xsaleid = b.xtableid");
            cmdText.AppendLine("inner join good c on a.xgoodid=c.xtableID");
            cmdText.AppendLine("inner join goodpric d on c.xtableid = d.xsubid");
            cmdText.AppendLine("where datetime(b.xtime1)<=@dateTime");
            cmdText.AppendLine("and datetime(b.xtime2)>=@dateTime and xstart=1 and d.xtableid = @xtableid");
            cmdText.AppendLine("and trim(c.goodcode)=@goodcode and a.xgtype='a' ");
            cmdText.AppendLine("union all");
            //单个SKU
            cmdText.AppendLine("select a.*,d.goodcode,b.xintime from salegood a");
            cmdText.AppendLine("inner join sale b on a.xsaleid = b.xtableid");
            cmdText.AppendLine("inner join goodpric c on a.xgoodid = c.xtableID");
            cmdText.AppendLine("inner join good d on d.xtableID=c.xsubid");
            cmdText.AppendLine("where datetime(b.xtime1)<=@dateTime");
            cmdText.AppendLine("and datetime(b.xtime2)>=@dateTime and xstart=1 and a.xgoodid = @xtableid");
            cmdText.AppendLine("and trim(d.goodcode)=@goodcode and a.xgtype='s' ");
            cmdText.AppendLine(") t order by t.xintime desc limit 1");


            SQLiteDataReader dataReader = SQLiteHelper.ExecuteReader(SQLiteHelper.connectionString, CommandType.Text, cmdText.ToString(), parameters);
            List<SalegoodModel> salegoodList = new List<SalegoodModel>();
            while (dataReader.Read())
            {
                SalegoodModel salegood = new SalegoodModel();
                salegood.xtype = dataReader["xtype"].ToString().Trim();
                salegood.xsaleid = int.Parse(dataReader["xsaleid"].ToString().Trim());
                salegood.xgoodid = int.Parse(dataReader["xgoodid"].ToString().Trim());
                salegood.goodcode = dataReader["goodcode"].ToString().Trim();
                salegood.xtableid = dataReader["xtableid"].ToString().Trim() == string.Empty ? (int?)null : int.Parse(dataReader["xtableid"].ToString().Trim());
                salegood.xsubid = dataReader["xsubid"].ToString().Trim() == string.Empty ? (int?)null : int.Parse(dataReader["xsubid"].ToString().Trim());
                salegood.xintime = dataReader["xintime"].ToString().Trim();
                salegoodList.Add(salegood);
            }
            dataReader.Close();
            #endregion

            //当前货品搞促销
            if (salegoodList.Count > 0 && salegoodList.FirstOrDefault() != null)
            {
                SalegoodModel salegood = salegoodList.OrderByDescending(r => r.xintime).FirstOrDefault();

                #region 促销活动
                cmdText = new StringBuilder();
                cmdText.AppendLine("select * from sale where xtableid = @xtableid");
                parameters = new SQLiteParameter[1];
                parameters[0] = new SQLiteParameter("xtableid", DbType.String);
                parameters[0].Value = salegood.xsaleid;
                dataReader = SQLiteHelper.ExecuteReader(SQLiteHelper.connectionString, CommandType.Text, cmdText.ToString(), parameters);
                List<SaleModel> saleList = new List<SaleModel>();
                while (dataReader.Read())
                {
                    SaleModel saleModel = new SaleModel();
                    saleModel.xtype = dataReader["xtype"].ToString().Trim();
                    saleModel.xname = dataReader["xname"].ToString().Trim();
                    saleModel.xkind = dataReader["xkind"].ToString().Trim();
                    saleModel.xunit = dataReader["xunit"].ToString().Trim();
                    if (dataReader["xrule"] != null && !string.IsNullOrEmpty(dataReader["xrule"].ToString()))
                    {
                        saleModel.xrule = int.Parse(dataReader["xrule"].ToString().Trim());
                    }

                    saleModel.xhalfnum = dataReader["xhalfnum"].ToString().Trim();
                    saleModel.xchsclss = dataReader["xchsclss"].ToString().Trim();
                    saleModel.xchstype = dataReader["xchstype"].ToString().Trim();
                    if (dataReader["xguest"] != null && !string.IsNullOrEmpty(dataReader["xguest"].ToString()))
                    {
                        saleModel.xguest = bool.Parse(dataReader["xguest"].ToString().Trim());
                    }
                    saleModel.xtableid = dataReader["xtableid"].ToString().Trim() == string.Empty ? (int?)null : int.Parse(dataReader["xtableid"].ToString().Trim());
                    saleModel.xsubid = dataReader["xsubid"].ToString().Trim() == string.Empty ? (int?)null : int.Parse(dataReader["xsubid"].ToString().Trim());
                    if (good.unitname == good.goodunit || saleModel.xtype != "t")
                    {
                        saleList.Add(saleModel);
                    }
                }
                dataReader.Close();
                #endregion

                SaleModel sale = saleList.FirstOrDefault();
                if (sale == null)
                {
                    return null;
                }

                #region 促销赠品
                cmdText = new StringBuilder();
                //cmdText.AppendLine("select a.*,b.goodcode from salegoodX a");
                //cmdText.AppendLine("inner join good b on a.xgoodid=b.xtableid");
                //cmdText.AppendLine("where a.xsaleid=@xtableid");
                //a类型没有sku
                cmdText.AppendLine("select a.*,c.goodcode from salegoodX a");
                cmdText.AppendLine("inner join sale b on a.xsaleid = b.xtableid");
                cmdText.AppendLine("inner join good c on a.xgoodid=c.xtableID");
                cmdText.AppendLine("left join goodpric d on c.xtableid = d.xsubid");
                cmdText.AppendLine("where d.SID is null");
                cmdText.AppendLine("and ifnull(a.xgtype,'a')='a' and a.xsaleid = @xtableid");
                cmdText.AppendLine("union all");
                //a类型下面所有的sku
                cmdText.AppendLine("select a.SID,a.xtype,a.xsaleid,a.xgtype,d.xtableid as xgoodid,a.xno,a.xtableid,a.xsubid,a.xversion,c.goodcode from salegoodX a");
                cmdText.AppendLine("inner join sale b on a.xsaleid = b.xtableid");
                cmdText.AppendLine("inner join good c on a.xgoodid=c.xtableID");
                cmdText.AppendLine("inner join goodpric d on c.xtableid = d.xsubid");
                cmdText.AppendLine("where ifnull(a.xgtype,'a')='a' and a.xsaleid = @xtableid");
                cmdText.AppendLine("union all");
                //单个SKU
                cmdText.AppendLine("select a.*,d.goodcode from salegoodX a");
                cmdText.AppendLine("inner join sale b on a.xsaleid = b.xtableid");
                cmdText.AppendLine("inner join goodpric c on a.xgoodid = c.xtableID");
                cmdText.AppendLine("inner join good d on d.xtableID=c.xsubid");
                cmdText.AppendLine("where a.xgtype='s' and a.xsaleid = @xtableid");

                dataReader = SQLiteHelper.ExecuteReader(SQLiteHelper.connectionString, CommandType.Text, cmdText.ToString(), parameters);
                List<SalegoodXModel> salegoodXList = new List<SalegoodXModel>();
                while (dataReader.Read())
                {
                    SalegoodXModel salegoodX = new SalegoodXModel();
                    salegoodX.xtype = dataReader["xtype"].ToString().Trim();
                    salegoodX.xsaleid = int.Parse(dataReader["xsaleid"].ToString().Trim());
                    salegoodX.xgoodid = int.Parse(dataReader["xgoodid"].ToString().Trim());
                    salegoodX.goodcode = dataReader["goodcode"].ToString().Trim();
                    salegoodX.xtableid = int.Parse(dataReader["xtableid"].ToString().Trim());
                    salegoodX.xsubid = dataReader["xsubid"].ToString().Trim() == string.Empty ? 0 : int.Parse(dataReader["xsubid"].ToString().Trim());
                    salegoodXList.Add(salegoodX);
                }
                dataReader.Close();
                #endregion

                #region 活动规则
                cmdText = new StringBuilder();
                cmdText.AppendLine("select * from salerule a");
                cmdText.AppendLine("where xsubid=@xtableid");
                dataReader = SQLiteHelper.ExecuteReader(SQLiteHelper.connectionString, CommandType.Text, cmdText.ToString(), parameters);
                List<SaleruleModel> saleruleList = new List<SaleruleModel>();
                while (dataReader.Read())
                {
                    SaleruleModel salerule = new SaleruleModel();
                    salerule.xhave = decimal.Parse(dataReader["xhave"].ToString().Trim());
                    salerule.xdo = decimal.Parse(dataReader["xdo"].ToString().Trim());
                    salerule.xfen = decimal.Parse(dataReader["xfen"].ToString().Trim());
                    salerule.xtableid = int.Parse(dataReader["xtableid"].ToString().Trim());
                    salerule.xsubid = dataReader["xsubid"].ToString().Trim() == string.Empty ? 0 : int.Parse(dataReader["xsubid"].ToString().Trim());
                    saleruleList.Add(salerule);
                }
                dataReader.Close();
                #endregion

                sale.salegoodXs = salegoodXList;
                sale.salerules = saleruleList;

                //游客不允许参加促销
                if (!sale.xguest)
                {
                    if (string.IsNullOrEmpty(clntCode))
                    {
                        return null;
                    }
                    else
                    {
                        //根据等级、类别判断会员是否能参加促销
                        ClientDAL clientDAL = new ClientDAL();
                        ClntModel client = clientDAL.GetClientByCode(clntCode);
                        if (sale.xchsclss.Contains(client.clntclss)
                            || sale.xchstype.Contains(client.clnttype))
                        {
                            return sale;
                        }
                        else
                        {
                            //当前会员不符合参加促销活动
                            return null;
                        }
                    }
                }
                else
                {
                    //全部顾客都可以参加促销
                    return sale;
                }

            }
            else
            {
                //当前货品没有搞促销
                return null;
            }
        }
        #endregion

        #region 获取一个促销活动
        /// <summary>
        /// 获取一个促销活动
        /// </summary>
        /// <param name="saleID">促销活动ID</param>
        /// <returns></returns>
        public SaleModel GetSaleByID(int saleID)
        {
            #region 促销活动
            StringBuilder cmdText = new StringBuilder();
            cmdText.AppendLine("select * from sale where xtableid = @xtableid");
            SQLiteParameter[] parameters = new SQLiteParameter[1];
            parameters[0] = new SQLiteParameter("xtableid", DbType.String);
            parameters[0].Value = saleID;
            SQLiteDataReader dataReader = SQLiteHelper.ExecuteReader(SQLiteHelper.connectionString, CommandType.Text, cmdText.ToString(), parameters);
            List<SaleModel> saleList = new List<SaleModel>();
            while (dataReader.Read())
            {
                SaleModel saleModel = new SaleModel();
                saleModel.xtype = dataReader["xtype"].ToString().Trim();
                saleModel.xname = dataReader["xname"].ToString().Trim();
                saleModel.xkind = dataReader["xkind"].ToString().Trim();
                saleModel.xunit = dataReader["xunit"].ToString().Trim();
                if (dataReader["xrule"] != null && !string.IsNullOrEmpty(dataReader["xrule"].ToString()))
                {
                    saleModel.xrule = int.Parse(dataReader["xrule"].ToString().Trim());
                }

                saleModel.xhalfnum = dataReader["xhalfnum"].ToString().Trim();
                saleModel.xchsclss = dataReader["xchsclss"].ToString().Trim();
                saleModel.xchstype = dataReader["xchstype"].ToString().Trim();
                if (dataReader["xguest"] != null && !string.IsNullOrEmpty(dataReader["xguest"].ToString()))
                {
                    saleModel.xguest = bool.Parse(dataReader["xguest"].ToString().Trim());
                }
                saleModel.xtableid = dataReader["xtableid"].ToString().Trim() == string.Empty ? (int?)null : int.Parse(dataReader["xtableid"].ToString().Trim());
                saleModel.xsubid = dataReader["xsubid"].ToString().Trim() == string.Empty ? (int?)null : int.Parse(dataReader["xsubid"].ToString().Trim());
                saleList.Add(saleModel);
            }
            dataReader.Close();
            #endregion

            SaleModel sale = saleList.FirstOrDefault();

            #region 促销赠品
            cmdText = new StringBuilder();
            //a类型没有sku
            cmdText.AppendLine("select a.*,c.goodcode from salegoodX a");
            cmdText.AppendLine("inner join sale b on a.xsaleid = b.xtableid");
            cmdText.AppendLine("inner join good c on a.xgoodid=c.xtableID");
            cmdText.AppendLine("left join goodpric d on c.xtableid = d.xsubid");
            cmdText.AppendLine("where d.SID is null");
            cmdText.AppendLine("and ifnull(a.xgtype,'a')='a' and a.xsaleid = @xtableid");
            cmdText.AppendLine("union all");
            //a类型下面所有的sku
            cmdText.AppendLine("select a.SID,a.xtype,a.xsaleid,a.xgtype,d.xtableid as xgoodid,a.xno,a.xtableid,a.xsubid,a.xversion,c.goodcode from salegoodX a");
            cmdText.AppendLine("inner join sale b on a.xsaleid = b.xtableid");
            cmdText.AppendLine("inner join good c on a.xgoodid=c.xtableID");
            cmdText.AppendLine("inner join goodpric d on c.xtableid = d.xsubid");
            cmdText.AppendLine("where ifnull(a.xgtype,'a')='a' and a.xsaleid = @xtableid");
            cmdText.AppendLine("union all");
            //单个SKU
            cmdText.AppendLine("select a.*,d.goodcode from salegoodX a");
            cmdText.AppendLine("inner join sale b on a.xsaleid = b.xtableid");
            cmdText.AppendLine("inner join goodpric c on a.xgoodid = c.xtableID");
            cmdText.AppendLine("inner join good d on d.xtableID=c.xsubid");
            cmdText.AppendLine("where a.xgtype='s' and a.xsaleid = @xtableid");

            dataReader = SQLiteHelper.ExecuteReader(SQLiteHelper.connectionString, CommandType.Text, cmdText.ToString(), parameters);
            List<SalegoodXModel> salegoodXList = new List<SalegoodXModel>();
            while (dataReader.Read())
            {
                SalegoodXModel salegoodX = new SalegoodXModel();
                salegoodX.xtype = dataReader["xtype"].ToString().Trim();
                salegoodX.xsaleid = int.Parse(dataReader["xsaleid"].ToString().Trim());
                salegoodX.xgoodid = int.Parse(dataReader["xgoodid"].ToString().Trim());
                salegoodX.goodcode = dataReader["goodcode"].ToString().Trim();
                salegoodX.xtableid = int.Parse(dataReader["xtableid"].ToString().Trim());
                salegoodX.xsubid = dataReader["xsubid"].ToString().Trim() == string.Empty ? 0 : int.Parse(dataReader["xsubid"].ToString().Trim());
                salegoodXList.Add(salegoodX);
            }
            dataReader.Close();
            #endregion

            #region 活动规则
            cmdText = new StringBuilder();
            cmdText.AppendLine("select * from salerule a");
            cmdText.AppendLine("where xsubid=@xtableid");
            dataReader = SQLiteHelper.ExecuteReader(SQLiteHelper.connectionString, CommandType.Text, cmdText.ToString(), parameters);
            List<SaleruleModel> saleruleList = new List<SaleruleModel>();
            while (dataReader.Read())
            {
                SaleruleModel salerule = new SaleruleModel();
                salerule.xhave = decimal.Parse(dataReader["xhave"].ToString().Trim());
                salerule.xdo = decimal.Parse(dataReader["xdo"].ToString().Trim());
                salerule.xfen = decimal.Parse(dataReader["xfen"].ToString().Trim());
                salerule.xtableid = int.Parse(dataReader["xtableid"].ToString().Trim());
                salerule.xsubid = dataReader["xsubid"].ToString().Trim() == string.Empty ? 0 : int.Parse(dataReader["xsubid"].ToString().Trim());
                saleruleList.Add(salerule);
            }
            dataReader.Close();
            #endregion

            sale.salegoodXs = salegoodXList;
            sale.salerules = saleruleList;

            return sale;

        }
        #endregion

        #region 获取满足当前时间段所有的促销活动
        public List<SaleModel> GetAllCurrentSales()
        {
            StringBuilder cmdText = new StringBuilder();
            cmdText.AppendLine("select * from sale ");
            cmdText.AppendLine("where datetime(xtime1)<=@dateTime");
            cmdText.AppendLine("and datetime(xtime2)>=@dateTime and xstart=1 order by xintime desc");
            SQLiteParameter[] parameters = new SQLiteParameter[1];
            parameters[0] = new SQLiteParameter("dateTime", DbType.DateTime);
            parameters[0].Value = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

            SQLiteDataReader dataReader = SQLiteHelper.ExecuteReader(SQLiteHelper.connectionString, CommandType.Text, cmdText.ToString(), parameters);
            List<SaleModel> saleList = new List<SaleModel>();
            while (dataReader.Read())
            {
                SaleModel saleModel = new SaleModel();
                saleModel.xtype = dataReader["xtype"].ToString().Trim();
                saleModel.xname = dataReader["xname"].ToString().Trim();
                saleModel.xkind = dataReader["xkind"].ToString().Trim();
                saleModel.xunit = dataReader["xunit"].ToString().Trim();
                if (dataReader["xrule"] != null && !string.IsNullOrEmpty(dataReader["xrule"].ToString()))
                {
                    saleModel.xrule = int.Parse(dataReader["xrule"].ToString().Trim());
                }
                saleModel.xtime1 = dataReader["xtime1"].ToString().Trim();
                saleModel.xtime2 = dataReader["xtime2"].ToString().Trim();
                saleModel.xhalfnum = dataReader["xhalfnum"].ToString().Trim();
                saleModel.xchsclss = dataReader["xchsclss"].ToString().Trim();
                saleModel.xchstype = dataReader["xchstype"].ToString().Trim();
                if (dataReader["xguest"] != null && !string.IsNullOrEmpty(dataReader["xguest"].ToString()))
                {
                    saleModel.xguest = bool.Parse(dataReader["xguest"].ToString().Trim());
                }
                saleModel.xtableid = dataReader["xtableid"].ToString().Trim() == string.Empty ? (int?)null : int.Parse(dataReader["xtableid"].ToString().Trim());
                saleModel.xsubid = dataReader["xsubid"].ToString().Trim() == string.Empty ? (int?)null : int.Parse(dataReader["xsubid"].ToString().Trim());
                saleList.Add(saleModel);
            }
            dataReader.Close();
            return saleList;
        }

        public List<SaleModel> GetAllCurrentSalesDetail()
        {
            StringBuilder cmdText = new StringBuilder();
            cmdText.AppendLine("select * from sale ");
            cmdText.AppendLine("where datetime(xtime1)<=@dateTime");
            cmdText.AppendLine("and datetime(xtime2)>=@dateTime and xstart=1 order by xtime1 desc");
            SQLiteParameter[] parameters = new SQLiteParameter[1];
            parameters[0] = new SQLiteParameter("dateTime", DbType.DateTime);
            parameters[0].Value = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

            SQLiteDataReader dataReader = SQLiteHelper.ExecuteReader(SQLiteHelper.connectionString, CommandType.Text, cmdText.ToString(), parameters);
            List<SaleModel> saleList = new List<SaleModel>();
            while (dataReader.Read())
            {
                SaleModel saleModel = new SaleModel();
                saleModel.xtype = dataReader["xtype"].ToString().Trim();
                saleModel.xname = dataReader["xname"].ToString().Trim();
                saleModel.xkind = dataReader["xkind"].ToString().Trim();
                saleModel.xunit = dataReader["xunit"].ToString().Trim();
                if (dataReader["xrule"] != null && !string.IsNullOrEmpty(dataReader["xrule"].ToString()))
                {
                    saleModel.xrule = int.Parse(dataReader["xrule"].ToString().Trim());
                }
                saleModel.xtime1 = dataReader["xtime1"].ToString().Trim();
                saleModel.xtime2 = dataReader["xtime2"].ToString().Trim();
                saleModel.xhalfnum = dataReader["xhalfnum"].ToString().Trim();
                saleModel.xchsclss = dataReader["xchsclss"].ToString().Trim();
                saleModel.xchstype = dataReader["xchstype"].ToString().Trim();
                if (dataReader["xguest"] != null && !string.IsNullOrEmpty(dataReader["xguest"].ToString()))
                {
                    saleModel.xguest = bool.Parse(dataReader["xguest"].ToString().Trim());
                }
                saleModel.xtableid = dataReader["xtableid"].ToString().Trim() == string.Empty ? (int?)null : int.Parse(dataReader["xtableid"].ToString().Trim());
                saleModel.xsubid = dataReader["xsubid"].ToString().Trim() == string.Empty ? (int?)null : int.Parse(dataReader["xsubid"].ToString().Trim());
                saleList.Add(saleModel);
            }
            dataReader.Close();

            foreach (var item in saleList)
            {
                cmdText = new StringBuilder();
                parameters = new SQLiteParameter[1];
                parameters[0] = new SQLiteParameter("xsaleid", DbType.Int32);
                parameters[0].Value = item.xtableid;

                cmdText.AppendLine("select * from(");
                #region 促销活动货品
                //a类型没有sku
                cmdText.AppendLine("select a.*,c.goodcode,c.goodname,c.goodunit,");
                cmdText.AppendLine("d.goodkind1,d.goodkind2,d.goodkind3,d.goodkind4,d.goodkind5,");
                cmdText.AppendLine("d.goodkind6,d.goodkind7,d.goodkind8,d.goodkind9,d.goodkind10");
                cmdText.AppendLine("from salegood a");
                cmdText.AppendLine("inner join good c on a.xgoodid=c.xtableID");
                cmdText.AppendLine("left join goodpric d on c.xtableid = d.xsubid");
                cmdText.AppendLine("where d.SID is null and xsaleid=@xsaleid");
                cmdText.AppendLine("union all");
                //a类型下面所有的sku
                cmdText.AppendLine("select a.SID,a.xtype,a.xsaleid,a.xgtype,d.xtableid as xgoodid,a.xtableid,a.xsubid,a.xversion,c.goodcode,c.goodname,c.goodunit,");
                cmdText.AppendLine("d.goodkind1,d.goodkind2,d.goodkind3,d.goodkind4,d.goodkind5,");
                cmdText.AppendLine("d.goodkind6,d.goodkind7,d.goodkind8,d.goodkind9,d.goodkind10");
                cmdText.AppendLine("from salegood a");
                cmdText.AppendLine("inner join good c on a.xgoodid=c.xtableID");
                cmdText.AppendLine("inner join goodpric d on c.xtableid = d.xsubid");
                cmdText.AppendLine("where xsaleid=@xsaleid");
                cmdText.AppendLine("union all");
                //单个SKU
                cmdText.AppendLine("select a.*,d.goodcode,d.goodname,d.goodunit,");
                cmdText.AppendLine("c.goodkind1,c.goodkind2,c.goodkind3,c.goodkind4,c.goodkind5,");
                cmdText.AppendLine("c.goodkind6,c.goodkind7,c.goodkind8,c.goodkind9,c.goodkind10");
                cmdText.AppendLine("from salegood a");
                cmdText.AppendLine("inner join goodpric c on a.xgoodid = c.xtableID");
                cmdText.AppendLine("inner join good d on d.xtableID=c.xsubid");
                cmdText.AppendLine("where xsaleid=@xsaleid");
                cmdText.AppendLine(")");


                dataReader = SQLiteHelper.ExecuteReader(SQLiteHelper.connectionString, CommandType.Text, cmdText.ToString(), parameters);
                List<SalegoodModel> salegoodList = new List<SalegoodModel>();

                while (dataReader.Read())
                {
                    List<string> goodkinds = new List<string>();
                    SalegoodModel salegood = new SalegoodModel();
                    salegood.xtype = dataReader["xtype"].ToString().Trim();
                    salegood.xsaleid = int.Parse(dataReader["xsaleid"].ToString().Trim());
                    salegood.xgoodid = int.Parse(dataReader["xgoodid"].ToString().Trim());
                    salegood.goodcode = dataReader["goodcode"].ToString().Trim();
                    salegood.goodname = dataReader["goodname"].ToString().Trim();
                    salegood.goodunit = dataReader["goodunit"].ToString().Trim();
                    salegood.xtableid = dataReader["xtableid"].ToString().Trim() == string.Empty ? (int?)null : int.Parse(dataReader["xtableid"].ToString().Trim());
                    salegood.xsubid = dataReader["xsubid"].ToString().Trim() == string.Empty ? (int?)null : int.Parse(dataReader["xsubid"].ToString().Trim());
                    if (!string.IsNullOrEmpty(dataReader["goodkind1"].ToString().Trim()))
                    {
                        salegood.goodkind1 = dataReader["goodkind1"].ToString().Trim();
                        goodkinds.Add(dataReader["goodkind1"].ToString().Trim());
                    }
                    if (!string.IsNullOrEmpty(dataReader["goodkind2"].ToString().Trim()))
                    {
                        salegood.goodkind2 = dataReader["goodkind2"].ToString().Trim();
                        goodkinds.Add(dataReader["goodkind2"].ToString().Trim());
                    }
                    if (!string.IsNullOrEmpty(dataReader["goodkind3"].ToString().Trim()))
                    {
                        salegood.goodkind3 = dataReader["goodkind3"].ToString().Trim();
                        goodkinds.Add(dataReader["goodkind3"].ToString().Trim());
                    }
                    if (!string.IsNullOrEmpty(dataReader["goodkind4"].ToString().Trim()))
                    {
                        salegood.goodkind4 = dataReader["goodkind4"].ToString().Trim();
                        goodkinds.Add(dataReader["goodkind4"].ToString().Trim());
                    }
                    if (!string.IsNullOrEmpty(dataReader["goodkind5"].ToString().Trim()))
                    {
                        salegood.goodkind5 = dataReader["goodkind5"].ToString().Trim();
                        goodkinds.Add(dataReader["goodkind5"].ToString().Trim());
                    }
                    if (!string.IsNullOrEmpty(dataReader["goodkind6"].ToString().Trim()))
                    {
                        salegood.goodkind6 = dataReader["goodkind6"].ToString().Trim();
                        goodkinds.Add(dataReader["goodkind6"].ToString().Trim());
                    }
                    if (!string.IsNullOrEmpty(dataReader["goodkind7"].ToString().Trim()))
                    {
                        salegood.goodkind7 = dataReader["goodkind7"].ToString().Trim();
                        goodkinds.Add(dataReader["goodkind7"].ToString().Trim());
                    }
                    if (!string.IsNullOrEmpty(dataReader["goodkind8"].ToString().Trim()))
                    {
                        salegood.goodkind8 = dataReader["goodkind8"].ToString().Trim();
                        goodkinds.Add(dataReader["goodkind8"].ToString().Trim());
                    }
                    if (!string.IsNullOrEmpty(dataReader["goodkind9"].ToString().Trim()))
                    {
                        salegood.goodkind9 = dataReader["goodkind9"].ToString().Trim();
                        goodkinds.Add(dataReader["goodkind9"].ToString().Trim());
                    }
                    if (!string.IsNullOrEmpty(dataReader["goodkind10"].ToString().Trim()))
                    {
                        salegood.goodkind10 = dataReader["goodkind10"].ToString().Trim();
                        goodkinds.Add(dataReader["goodkind10"].ToString().Trim());
                    }
                    if (goodkinds.Count > 0)
                    {
                        salegood.goodkind = string.Join(",", goodkinds);
                    }
                    salegoodList.Add(salegood);
                }
                dataReader.Close();
                item.salegoods = salegoodList;
                #endregion

                #region 促销赠品
                cmdText = new StringBuilder();
                //a类型没有sku
                cmdText.AppendLine("select a.*,c.goodcode,c.goodname,c.goodunit,");
                cmdText.AppendLine("d.goodkind1,d.goodkind2,d.goodkind3,d.goodkind4,d.goodkind5,");
                cmdText.AppendLine("d.goodkind6,d.goodkind7,d.goodkind8,d.goodkind9,d.goodkind10");
                cmdText.AppendLine("from salegoodX a");
                cmdText.AppendLine("inner join sale b on a.xsaleid = b.xtableid");
                cmdText.AppendLine("inner join good c on a.xgoodid=c.xtableID");
                cmdText.AppendLine("left join goodpric d on c.xtableid = d.xsubid");
                cmdText.AppendLine("where d.SID is null");
                cmdText.AppendLine("and ifnull(a.xgtype,'a')='a' and a.xsaleid = @xsaleid");
                cmdText.AppendLine("union all");
                //a类型下面所有的sku
                cmdText.AppendLine("select a.SID,a.xtype,a.xsaleid,a.xgtype,d.xtableid as xgoodid,a.xno,a.xtableid,a.xsubid,a.xversion,c.goodcode,c.goodname,c.goodunit,");
                cmdText.AppendLine("d.goodkind1,d.goodkind2,d.goodkind3,d.goodkind4,d.goodkind5,");
                cmdText.AppendLine("d.goodkind6,d.goodkind7,d.goodkind8,d.goodkind9,d.goodkind10");
                cmdText.AppendLine("from salegoodX a");
                cmdText.AppendLine("inner join sale b on a.xsaleid = b.xtableid");
                cmdText.AppendLine("inner join good c on a.xgoodid=c.xtableID");
                cmdText.AppendLine("inner join goodpric d on c.xtableid = d.xsubid");
                cmdText.AppendLine("where ifnull(a.xgtype,'a')='a' and a.xsaleid = @xsaleid");
                cmdText.AppendLine("union all");
                //单个SKU
                cmdText.AppendLine("select a.*,d.goodcode,d.goodname,d.goodunit,");
                cmdText.AppendLine("c.goodkind1,c.goodkind2,c.goodkind3,c.goodkind4,c.goodkind5,");
                cmdText.AppendLine("c.goodkind6,c.goodkind7,c.goodkind8,c.goodkind9,c.goodkind10");
                cmdText.AppendLine("from salegoodX a");
                cmdText.AppendLine("inner join sale b on a.xsaleid = b.xtableid");
                cmdText.AppendLine("inner join goodpric c on a.xgoodid = c.xtableID");
                cmdText.AppendLine("inner join good d on d.xtableID=c.xsubid");
                cmdText.AppendLine("where a.xgtype='s' and a.xsaleid = @xsaleid");

                dataReader = SQLiteHelper.ExecuteReader(SQLiteHelper.connectionString, CommandType.Text, cmdText.ToString(), parameters);
                List<SalegoodXModel> salegoodXList = new List<SalegoodXModel>();
                while (dataReader.Read())
                {
                    List<string> goodkinds = new List<string>();
                    SalegoodXModel salegoodX = new SalegoodXModel();
                    salegoodX.xtype = dataReader["xtype"].ToString().Trim();
                    salegoodX.xsaleid = int.Parse(dataReader["xsaleid"].ToString().Trim());
                    salegoodX.xgoodid = int.Parse(dataReader["xgoodid"].ToString().Trim());
                    salegoodX.goodname = dataReader["goodname"].ToString().Trim();
                    salegoodX.goodunit = dataReader["goodunit"].ToString().Trim();
                    salegoodX.goodcode = dataReader["goodcode"].ToString().Trim();
                    salegoodX.xtableid = int.Parse(dataReader["xtableid"].ToString().Trim());
                    salegoodX.xsubid = dataReader["xsubid"].ToString().Trim() == string.Empty ? 0 : int.Parse(dataReader["xsubid"].ToString().Trim());
                    if (!string.IsNullOrEmpty(dataReader["goodkind1"].ToString().Trim()))
                    {
                        salegoodX.goodkind1 = dataReader["goodkind1"].ToString().Trim();
                        goodkinds.Add(dataReader["goodkind1"].ToString().Trim());
                    }
                    if (!string.IsNullOrEmpty(dataReader["goodkind2"].ToString().Trim()))
                    {
                        salegoodX.goodkind2 = dataReader["goodkind2"].ToString().Trim();
                        goodkinds.Add(dataReader["goodkind2"].ToString().Trim());
                    }
                    if (!string.IsNullOrEmpty(dataReader["goodkind3"].ToString().Trim()))
                    {
                        salegoodX.goodkind3 = dataReader["goodkind3"].ToString().Trim();
                        goodkinds.Add(dataReader["goodkind3"].ToString().Trim());
                    }
                    if (!string.IsNullOrEmpty(dataReader["goodkind4"].ToString().Trim()))
                    {
                        salegoodX.goodkind4 = dataReader["goodkind4"].ToString().Trim();
                        goodkinds.Add(dataReader["goodkind4"].ToString().Trim());
                    }
                    if (!string.IsNullOrEmpty(dataReader["goodkind5"].ToString().Trim()))
                    {
                        salegoodX.goodkind5 = dataReader["goodkind5"].ToString().Trim();
                        goodkinds.Add(dataReader["goodkind5"].ToString().Trim());
                    }
                    if (!string.IsNullOrEmpty(dataReader["goodkind6"].ToString().Trim()))
                    {
                        salegoodX.goodkind6 = dataReader["goodkind6"].ToString().Trim();
                        goodkinds.Add(dataReader["goodkind6"].ToString().Trim());
                    }
                    if (!string.IsNullOrEmpty(dataReader["goodkind7"].ToString().Trim()))
                    {
                        salegoodX.goodkind7 = dataReader["goodkind7"].ToString().Trim();
                        goodkinds.Add(dataReader["goodkind7"].ToString().Trim());
                    }
                    if (!string.IsNullOrEmpty(dataReader["goodkind8"].ToString().Trim()))
                    {
                        salegoodX.goodkind8 = dataReader["goodkind8"].ToString().Trim();
                        goodkinds.Add(dataReader["goodkind8"].ToString().Trim());
                    }
                    if (!string.IsNullOrEmpty(dataReader["goodkind9"].ToString().Trim()))
                    {
                        salegoodX.goodkind9 = dataReader["goodkind9"].ToString().Trim();
                        goodkinds.Add(dataReader["goodkind9"].ToString().Trim());
                    }
                    if (!string.IsNullOrEmpty(dataReader["goodkind10"].ToString().Trim()))
                    {
                        salegoodX.goodkind10 = dataReader["goodkind10"].ToString().Trim();
                        goodkinds.Add(dataReader["goodkind10"].ToString().Trim());
                    }
                    if (goodkinds.Count > 0)
                    {
                        salegoodX.goodkind = string.Join(",", goodkinds);
                    }
                    salegoodXList.Add(salegoodX);
                }
                dataReader.Close();
                #endregion

                #region 促销规则
                cmdText = new StringBuilder();
                cmdText.AppendLine("select * from salerule where xsubid=@xsaleid");
                dataReader = SQLiteHelper.ExecuteReader(SQLiteHelper.connectionString, CommandType.Text, cmdText.ToString(), parameters);
                List<SaleruleModel> saleruleList = new List<SaleruleModel>();
                while (dataReader.Read())
                {
                    SaleruleModel salerule = new SaleruleModel();
                    salerule.xhave = decimal.Parse(dataReader["xhave"].ToString().Trim());
                    salerule.xdo = decimal.Parse(dataReader["xdo"].ToString().Trim());
                    salerule.xfen = decimal.Parse(dataReader["xfen"].ToString().Trim());
                    salerule.xtableid = dataReader["xtableid"].ToString().Trim() == string.Empty ? (int?)null : int.Parse(dataReader["xtableid"].ToString().Trim());
                    salerule.xsubid = dataReader["xsubid"].ToString().Trim() == string.Empty ? (int?)null : int.Parse(dataReader["xsubid"].ToString().Trim());
                    var query = (from p in salegoodXList where p.xno == salerule.xdo select p).ToList();
                    salerule.SalegoodXs = query;
                    saleruleList.Add(salerule);
                }
                dataReader.Close();
                item.salerules = saleruleList;
                #endregion
            }
            return saleList;
        }
        #endregion

        #region 获取所有的促销活动
        public List<SaleModel> GetAllSales()
        {
            StringBuilder cmdText = new StringBuilder();
            cmdText.AppendLine("select * from sale ");

            SQLiteDataReader dataReader = SQLiteHelper.ExecuteReader(SQLiteHelper.connectionString, CommandType.Text, cmdText.ToString());
            List<SaleModel> saleList = new List<SaleModel>();
            while (dataReader.Read())
            {
                SaleModel saleModel = new SaleModel();
                saleModel.xtype = dataReader["xtype"].ToString().Trim();
                saleModel.xname = dataReader["xname"].ToString().Trim();
                saleModel.xkind = dataReader["xkind"].ToString().Trim();
                saleModel.xunit = dataReader["xunit"].ToString().Trim();
                if (dataReader["xrule"] != null && !string.IsNullOrEmpty(dataReader["xrule"].ToString()))
                {
                    saleModel.xrule = int.Parse(dataReader["xrule"].ToString().Trim());
                }

                saleModel.xhalfnum = dataReader["xhalfnum"].ToString().Trim();
                saleModel.xchsclss = dataReader["xchsclss"].ToString().Trim();
                saleModel.xchstype = dataReader["xchstype"].ToString().Trim();
                if (dataReader["xguest"] != null && !string.IsNullOrEmpty(dataReader["xguest"].ToString()))
                {
                    saleModel.xguest = bool.Parse(dataReader["xguest"].ToString().Trim());
                }
                saleModel.xtableid = dataReader["xtableid"].ToString().Trim() == string.Empty ? (int?)null : int.Parse(dataReader["xtableid"].ToString().Trim());
                saleModel.xsubid = dataReader["xsubid"].ToString().Trim() == string.Empty ? (int?)null : int.Parse(dataReader["xsubid"].ToString().Trim());
                saleList.Add(saleModel);
            }
            dataReader.Close();
            return saleList;
        }
        #endregion

        #region 获取货品满足所有的促销活动
        /// <summary>
        /// 获取货品满足所有的促销活动
        /// </summary>
        /// <param name="clntCode">客户代码</param>
        /// <param name="goodcode">货品代码</param>
        /// <param name="xtableid">货品ID</param>
        /// <returns></returns>
        public List<int> GetSaleIDs(string clntCode, string goodcode, int xtableid)
        {
            StringBuilder cmdText = new StringBuilder();
            SQLiteParameter[] parameters = new SQLiteParameter[3];
            parameters[0] = new SQLiteParameter("dateTime", DbType.DateTime);
            parameters[0].Value = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            parameters[1] = new SQLiteParameter("goodcode", DbType.String);
            parameters[1].Value = goodcode;
            parameters[2] = new SQLiteParameter("xtableid", DbType.Int32);
            parameters[2].Value = xtableid;

            cmdText.AppendLine("select * from(");
            #region 促销活动货品
            //a类型没有sku
            cmdText.AppendLine("select a.xsaleid,b.xguest from salegood a");
            cmdText.AppendLine("inner join sale b on a.xsaleid = b.xtableid");
            cmdText.AppendLine("inner join good c on a.xgoodid=c.xtableID");
            cmdText.AppendLine("left join goodpric d on c.xtableid = d.xsubid");
            cmdText.AppendLine("where d.SID is null and datetime(b.xtime1)<=@dateTime");
            cmdText.AppendLine("and datetime(b.xtime2)>=@dateTime and xstart=1");
            cmdText.AppendLine("and trim(c.goodcode)=@goodcode and a.xgtype='a' and a.xgoodid = @xtableid");
            cmdText.AppendLine("union all");
            //a类型下面所有的sku
            cmdText.AppendLine("select a.xsaleid,b.xguest from salegood a");
            cmdText.AppendLine("inner join sale b on a.xsaleid = b.xtableid");
            cmdText.AppendLine("inner join good c on a.xgoodid=c.xtableID");
            cmdText.AppendLine("inner join goodpric d on c.xtableid = d.xsubid");
            cmdText.AppendLine("where datetime(b.xtime1)<=@dateTime");
            cmdText.AppendLine("and datetime(b.xtime2)>=@dateTime and xstart=1 and d.xtableid = @xtableid");
            cmdText.AppendLine("and trim(c.goodcode)=@goodcode and a.xgtype='a' ");
            cmdText.AppendLine("union all");
            //单个SKU
            cmdText.AppendLine("select a.xsaleid,b.xguest from salegood a");
            cmdText.AppendLine("inner join sale b on a.xsaleid = b.xtableid");
            cmdText.AppendLine("inner join goodpric c on a.xgoodid = c.xtableID");
            cmdText.AppendLine("inner join good d on d.xtableID=c.xsubid");
            cmdText.AppendLine("where datetime(b.xtime1)<=@dateTime");
            cmdText.AppendLine("and datetime(b.xtime2)>=@dateTime and xstart=1 and a.xgoodid = @xtableid");
            cmdText.AppendLine("and trim(d.goodcode)=@goodcode and a.xgtype='s' ");
            cmdText.AppendLine(") t");

            SQLiteDataReader dataReader = SQLiteHelper.ExecuteReader(SQLiteHelper.connectionString, CommandType.Text, cmdText.ToString(), parameters);
            List<SaleModel> saleList = new List<SaleModel>();
            while (dataReader.Read())
            {
                SaleModel sale = new SaleModel();
                sale.xtableid = int.Parse(dataReader["xsaleid"].ToString().Trim());
                sale.xguest = bool.Parse(dataReader["xguest"].ToString().Trim());
                saleList.Add(sale);
            }
            dataReader.Close();
            #endregion

            List<SaleModel> sales = new List<SaleModel>();
            var query1 = (from p in saleList where p.xguest select p).ToList();
            sales.AddRange(query1);
            if (string.IsNullOrEmpty(clntCode))
            {
                ClientDAL clientDAL = new ClientDAL();
                ClntModel client = clientDAL.GetClientByCode(clntCode);
                if (client != null)
                {
                    var query2 = (from p in saleList
                                  where p.xguest == false && (p.xchsclss.Contains(client.clntclss)
                     || p.xchstype.Contains(client.clnttype))
                                  select p).ToList();

                    sales.AddRange(query2);
                }
            }
            var result = sales.Where(r => r.xtableid.HasValue).Select(r => r.xtableid.Value).ToList();
            return result;

        }
        #endregion

        #region 获取会员日
        /// <summary>
        /// 获取会员日
        /// </summary>
        /// <param name="day"></param>
        /// <returns></returns>
        public ClntDayModel GetClntDay(int day,int month)
        {
            string cmdText = "select * from clntday where xday =@xday and xstart=1";
            SQLiteParameter[] parameters = new SQLiteParameter[1];
            parameters[0] = new SQLiteParameter("xday", DbType.Int16);
            parameters[0].Value = day;

            List<ClntDayModel> clntdays = new List<ClntDayModel>();

            SQLiteDataReader dataReader = SQLiteHelper.ExecuteReader(SQLiteHelper.connectionString, CommandType.Text, cmdText.ToString(), parameters);
            while (dataReader.Read())
            {
                ClntDayModel entity = new ClntDayModel();
                entity.xday = int.Parse(dataReader["xday"].ToString().Trim());
                entity.xmonth = dataReader["xmonth"].ToString().Trim().Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries).Select(Int32.Parse).ToList();
                entity.xtableid = dataReader["xtableid"].ToString().Trim() == string.Empty ? (int?)null : int.Parse(dataReader["xtableid"].ToString().Trim());
                entity.xsubid = dataReader["xsubid"].ToString().Trim() == string.Empty ? (int?)null : int.Parse(dataReader["xsubid"].ToString().Trim());
                clntdays.Add(entity);
            }
            dataReader.Close();

            var query = from p in clntdays where p.xmonth.Contains(month) select p;
            ClntDayModel clntday = query.FirstOrDefault();
            if (clntday != null)
            {
                cmdText = "select * from clntdrule where xsubid=@xtableid";
                parameters = new SQLiteParameter[1];
                parameters[0] = new SQLiteParameter("xtableid", DbType.Int32);
                parameters[0].Value = clntday.xtableid;

                List<ClntdRuleModel> clntprices = new List<Model.ClntdRuleModel>();
                dataReader = SQLiteHelper.ExecuteReader(SQLiteHelper.connectionString, CommandType.Text, cmdText.ToString(), parameters);
                while (dataReader.Read())
                {
                    ClntdRuleModel clntprice = new ClntdRuleModel();
                    clntprice.goodtype = dataReader["goodtype"].ToString().Trim();
                    clntprice.uclssprics = dataReader["uclssprics"].ToString().Trim();
                    clntprice.classtype = dataReader["classtype"].ToString().Trim();
                    clntprice.xtimes = dataReader["xtimes"].ToString().Trim() == string.Empty ? 1 : decimal.Parse(dataReader["xtimes"].ToString().Trim());
                    clntprice.xtableid = dataReader["xtableid"].ToString().Trim() == string.Empty ? (int?)null : int.Parse(dataReader["xtableid"].ToString().Trim());
                    clntprice.xsubid = dataReader["xsubid"].ToString().Trim() == string.Empty ? (int?)null : int.Parse(dataReader["xsubid"].ToString().Trim());
                    clntprices.Add(clntprice);
                }
                clntday.clntprices = clntprices;
                dataReader.Close();
            }
            return clntday;
        }
        #endregion
    }
}
