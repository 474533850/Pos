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
    /// 货品逻辑类
    /// </summary>
    public class GoodDAL
    {
        #region 模糊搜索货品
        /// <summary>
        /// 模糊搜索货品
        /// </summary>
        /// <param name="key">条码、品名、货品代码</param>
        /// <param name="xls">分部代码</param>
        /// <returns></returns>
        public List<GoodModel> GetGoodByKey(string key)
        {
            SQLiteParameter[] parameters = new SQLiteParameter[1];
            parameters[0] = new SQLiteParameter("@key", DbType.String);
            parameters[0].Value = "%" + key.ToLower() + "%";

            List<GoodModel> goodList = new List<GoodModel>();

            #region 有条码通用
            string cmdText = @"select distinct a.*,b.xbarcode,b.goodunit as unitname,b.xtype,b.xpric from good a
                                inner join gbarcode b on a.xtableid = b.xsubid and xtype = '通用'
                                where lower(a.goodcode) like @key or (a.goodname) like  @key or lower(b.xbarcode) like  @key
                                or lower(a.xjpiny) like @key or lower(a.xqpiny) like @key and (a.xshow == 0 or a.xshow)";

            SQLiteDataReader dataReader = SQLiteHelper.ExecuteReader(SQLiteHelper.connectionString, CommandType.Text, cmdText, parameters);
            while (dataReader.Read())
            {
                GoodModel good = new GoodModel();
                good.ID = Guid.NewGuid();
                good.goodtype1 = dataReader["goodtype1"].ToString().Trim();
                good.goodtype2 = dataReader["goodtype2"].ToString().Trim();
                good.goodtype3 = dataReader["goodtype3"].ToString().Trim();
                good.goodcode = dataReader["goodcode"].ToString().Trim();
                good.goodname = dataReader["goodname"].ToString().Trim();
                good.goodunit = dataReader["goodunit"].ToString().Trim();
                if (!string.IsNullOrEmpty(dataReader["xmulunit"].ToString().Trim()))
                {
                    good.xmulunit = dataReader["xmulunit"].ToString().Trim();
                }
                good.goodkeys = dataReader["goodkeys"].ToString().Trim();
                if (dataReader["xpric"] != null && !string.IsNullOrEmpty(dataReader["xpric"].ToString()))
                {
                    decimal xpric = 0;
                    if (decimal.TryParse(dataReader["xpric"].ToString(), out xpric))
                    {
                        if (xpric > 0)
                        {
                            good.xprico = xpric;
                        }
                        else
                        {
                            good.xprico = (dataReader["xprico"] == null || dataReader["xprico"].ToString().Trim() == string.Empty) ? 0 : decimal.Parse(dataReader["xprico"].ToString().Trim());
                        }
                    }
                }
                else
                {
                    good.xprico = (dataReader["xprico"] == null || dataReader["xprico"].ToString().Trim() == string.Empty) ? 0 : decimal.Parse(dataReader["xprico"].ToString().Trim());
                }
                good.xsendjf = (dataReader["xsendjf"] == null || dataReader["xsendjf"].ToString().Trim() == string.Empty) ? 0 : decimal.Parse(dataReader["xsendjf"].ToString().Trim());
                good.xchagjf = (dataReader["xchagjf"] == null || dataReader["xchagjf"].ToString().Trim() == string.Empty) ? 0 : decimal.Parse(dataReader["xchagjf"].ToString().Trim());
                good.xls = dataReader["xls"].ToString().Trim();
                good.xlsname = dataReader["xlsname"].ToString().Trim();
                good.xtableid = int.Parse(dataReader["xtableid"].ToString().Trim());
                good.xsubid = dataReader["xsubid"].ToString().Trim() == string.Empty ? 0 : int.Parse(dataReader["xsubid"].ToString().Trim());

                good.xbarcode = dataReader["xbarcode"].ToString().Trim();
                good.unitname = dataReader["unitname"].ToString().Trim();
                good.key = good.goodcode + good.goodunit;
                goodList.Add(good);
            }
            dataReader.Close();
            #endregion

            #region 有条码SKU
            cmdText = @"select distinct a.*,b.xbarcode,b.goodunit as unitname,b.xtype,c.xpric,goodkind1,
                        goodkind2,goodkind3,goodkind4,goodkind5,goodkind6,
                        goodkind7,goodkind8,goodkind9,goodkind10,c.xtableid as cxtableid,c.xsendjf as cxsendjf,c.xchagjf as cxchagjf from good a
                        inner join goodpric c on a.xtableid = c.xsubid
                        inner join gbarcode b on c.xtableid =b.xsubid and xtype='SKU'
                        where lower(a.goodcode) like @key or lower(a.goodname) like @key or lower(b.xbarcode) like @key 
                        or lower(a.xjpiny) like @key or lower(a.xqpiny) like @key and (a.xshow == 0 or a.xshow)";

            dataReader = SQLiteHelper.ExecuteReader(SQLiteHelper.connectionString, CommandType.Text, cmdText, parameters);

            while (dataReader.Read())
            {
                GoodModel good = new GoodModel();
                good.ID = Guid.NewGuid();
                good.goodtype1 = dataReader["goodtype1"].ToString().Trim();
                good.goodtype2 = dataReader["goodtype2"].ToString().Trim();
                good.goodtype3 = dataReader["goodtype3"].ToString().Trim();
                good.goodcode = dataReader["goodcode"].ToString().Trim();
                good.goodname = dataReader["goodname"].ToString().Trim();
                good.goodunit = dataReader["goodunit"].ToString().Trim();
                if (!string.IsNullOrEmpty(dataReader["xmulunit"].ToString().Trim()))
                {
                    good.xmulunit = dataReader["xmulunit"].ToString().Trim();
                }
                good.goodkeys = dataReader["goodkeys"].ToString().Trim();
                good.xprico = (dataReader["xpric"] == null || dataReader["xpric"].ToString().Trim() == string.Empty) ? 0 : decimal.Parse(dataReader["xpric"].ToString().Trim());
                good.xsendjf = (dataReader["cxsendjf"] == null || dataReader["cxsendjf"].ToString().Trim() == string.Empty) ? 0 : decimal.Parse(dataReader["cxsendjf"].ToString().Trim());
                good.xchagjf = (dataReader["cxchagjf"] == null || dataReader["cxchagjf"].ToString().Trim() == string.Empty) ? 0 : decimal.Parse(dataReader["cxchagjf"].ToString().Trim());
                good.xls = dataReader["xls"].ToString().Trim();
                good.xlsname = dataReader["xlsname"].ToString().Trim();
                good.xtableid = int.Parse(dataReader["cxtableid"].ToString().Trim());

                good.xbarcode = dataReader["xbarcode"].ToString().Trim();
                good.unitname = dataReader["unitname"].ToString().Trim();

                List<string> goodkinds = new List<string>();
                if (!string.IsNullOrEmpty(dataReader["goodkind1"].ToString().Trim()))
                {
                    good.goodkind1 = dataReader["goodkind1"].ToString().Trim();
                    goodkinds.Add(dataReader["goodkind1"].ToString().Trim());
                }
                if (!string.IsNullOrEmpty(dataReader["goodkind2"].ToString().Trim()))
                {
                    good.goodkind2 = dataReader["goodkind2"].ToString().Trim();
                    goodkinds.Add(dataReader["goodkind2"].ToString().Trim());
                }
                if (!string.IsNullOrEmpty(dataReader["goodkind3"].ToString().Trim()))
                {
                    good.goodkind3 = dataReader["goodkind3"].ToString().Trim();
                    goodkinds.Add(dataReader["goodkind3"].ToString().Trim());
                }
                if (!string.IsNullOrEmpty(dataReader["goodkind4"].ToString().Trim()))
                {
                    good.goodkind4 = dataReader["goodkind4"].ToString().Trim();
                    goodkinds.Add(dataReader["goodkind4"].ToString().Trim());
                }
                if (!string.IsNullOrEmpty(dataReader["goodkind5"].ToString().Trim()))
                {
                    good.goodkind5 = dataReader["goodkind5"].ToString().Trim();
                    goodkinds.Add(dataReader["goodkind5"].ToString().Trim());
                }
                if (!string.IsNullOrEmpty(dataReader["goodkind6"].ToString().Trim()))
                {
                    good.goodkind6 = dataReader["goodkind6"].ToString().Trim();
                    goodkinds.Add(dataReader["goodkind6"].ToString().Trim());
                }
                if (!string.IsNullOrEmpty(dataReader["goodkind7"].ToString().Trim()))
                {
                    good.goodkind7 = dataReader["goodkind7"].ToString().Trim();
                    goodkinds.Add(dataReader["goodkind7"].ToString().Trim());
                }
                if (!string.IsNullOrEmpty(dataReader["goodkind8"].ToString().Trim()))
                {
                    good.goodkind8 = dataReader["goodkind8"].ToString().Trim();
                    goodkinds.Add(dataReader["goodkind8"].ToString().Trim());
                }
                if (!string.IsNullOrEmpty(dataReader["goodkind9"].ToString().Trim()))
                {
                    good.goodkind9 = dataReader["goodkind9"].ToString().Trim();
                    goodkinds.Add(dataReader["goodkind9"].ToString().Trim());
                }
                if (!string.IsNullOrEmpty(dataReader["goodkind10"].ToString().Trim()))
                {
                    good.goodkind10 = dataReader["goodkind10"].ToString().Trim();
                    goodkinds.Add(dataReader["goodkind10"].ToString().Trim());
                }
                if (goodkinds.Count > 0)
                {
                    good.goodkind = string.Join(",", goodkinds);
                    good.key = good.goodcode + good.goodunit + good.goodkind;
                }
                else
                {
                    good.key = good.goodcode + good.goodunit;
                }
                goodList.Add(good);
            }
            dataReader.Close();
            #endregion

            #region 没有条码的货品
            cmdText = @"select distinct a.*,xpric,goodkind1,
                        goodkind2,goodkind3,goodkind4,goodkind5,goodkind6,
                        goodkind7,goodkind8,goodkind9,goodkind10,b.xtableid as bxtableid,b.xsendjf as bxsendjf,b.xchagjf as bxchagjf from good a
                        left join goodpric b on b.xsubid = a.xtableid
                        where lower(a.goodcode) like @key or lower(a.goodname) like @key 
                        or lower(a.xjpiny) like @key or lower(a.xqpiny) like @key and (a.xshow == 0 or a.xshow)";

            dataReader = SQLiteHelper.ExecuteReader(SQLiteHelper.connectionString, CommandType.Text, cmdText, parameters);
            List<GoodModel> goodList_NoBar = new List<GoodModel>();
            while (dataReader.Read())
            {
                GoodModel good = new GoodModel();
                good.ID = Guid.NewGuid();
                good.goodtype1 = dataReader["goodtype1"].ToString().Trim();
                good.goodtype2 = dataReader["goodtype2"].ToString().Trim();
                good.goodtype3 = dataReader["goodtype3"].ToString().Trim();
                good.goodcode = dataReader["goodcode"].ToString().Trim();
                good.goodname = dataReader["goodname"].ToString().Trim();
                good.goodunit = dataReader["goodunit"].ToString().Trim();
                good.unitname = good.goodunit;
                if (!string.IsNullOrEmpty(dataReader["xmulunit"].ToString().Trim()))
                {
                    good.xmulunit = dataReader["xmulunit"].ToString().Trim();
                }
                good.goodkeys = dataReader["goodkeys"].ToString().Trim();
                List<string> goodkinds = new List<string>();
                if (dataReader["bxtableid"] != null && dataReader["bxtableid"].ToString() != string.Empty)
                {
                    good.xprico = (dataReader["xpric"] == null || dataReader["xpric"].ToString().Trim() == string.Empty) ? 0 : decimal.Parse(dataReader["xpric"].ToString().Trim());
                    if (!string.IsNullOrEmpty(dataReader["goodkind1"].ToString().Trim()))
                    {
                        good.goodkind1 = dataReader["goodkind1"].ToString().Trim();
                        goodkinds.Add(dataReader["goodkind1"].ToString().Trim());
                    }
                    if (!string.IsNullOrEmpty(dataReader["goodkind2"].ToString().Trim()))
                    {
                        good.goodkind2 = dataReader["goodkind2"].ToString().Trim();
                        goodkinds.Add(dataReader["goodkind2"].ToString().Trim());
                    }
                    if (!string.IsNullOrEmpty(dataReader["goodkind3"].ToString().Trim()))
                    {
                        good.goodkind3 = dataReader["goodkind3"].ToString().Trim();
                        goodkinds.Add(dataReader["goodkind3"].ToString().Trim());
                    }
                    if (!string.IsNullOrEmpty(dataReader["goodkind4"].ToString().Trim()))
                    {
                        good.goodkind4 = dataReader["goodkind4"].ToString().Trim();
                        goodkinds.Add(dataReader["goodkind4"].ToString().Trim());
                    }
                    if (!string.IsNullOrEmpty(dataReader["goodkind5"].ToString().Trim()))
                    {
                        good.goodkind5 = dataReader["goodkind5"].ToString().Trim();
                        goodkinds.Add(dataReader["goodkind5"].ToString().Trim());
                    }
                    if (!string.IsNullOrEmpty(dataReader["goodkind6"].ToString().Trim()))
                    {
                        good.goodkind6 = dataReader["goodkind6"].ToString().Trim();
                        goodkinds.Add(dataReader["goodkind6"].ToString().Trim());
                    }
                    if (!string.IsNullOrEmpty(dataReader["goodkind7"].ToString().Trim()))
                    {
                        good.goodkind7 = dataReader["goodkind7"].ToString().Trim();
                        goodkinds.Add(dataReader["goodkind7"].ToString().Trim());
                    }
                    if (!string.IsNullOrEmpty(dataReader["goodkind8"].ToString().Trim()))
                    {
                        good.goodkind8 = dataReader["goodkind8"].ToString().Trim();
                        goodkinds.Add(dataReader["goodkind8"].ToString().Trim());
                    }
                    if (!string.IsNullOrEmpty(dataReader["goodkind9"].ToString().Trim()))
                    {
                        good.goodkind9 = dataReader["goodkind9"].ToString().Trim();
                        goodkinds.Add(dataReader["goodkind9"].ToString().Trim());
                    }
                    if (!string.IsNullOrEmpty(dataReader["goodkind10"].ToString().Trim()))
                    {
                        good.goodkind10 = dataReader["goodkind10"].ToString().Trim();
                        goodkinds.Add(dataReader["goodkind10"].ToString().Trim());
                    }
                    good.xtableid = int.Parse(dataReader["bxtableid"].ToString().Trim());
                    good.xsendjf = (dataReader["bxsendjf"] == null || dataReader["bxsendjf"].ToString().Trim() == string.Empty) ? 0 : decimal.Parse(dataReader["bxsendjf"].ToString().Trim());
                    good.xchagjf = (dataReader["bxchagjf"] == null || dataReader["bxchagjf"].ToString().Trim() == string.Empty) ? 0 : decimal.Parse(dataReader["bxchagjf"].ToString().Trim());
                }
                else
                {
                    good.xprico = (dataReader["xprico"] == null || dataReader["xprico"].ToString().Trim() == string.Empty) ? 0 : decimal.Parse(dataReader["xprico"].ToString().Trim());
                    good.xtableid = int.Parse(dataReader["xtableid"].ToString().Trim());
                    good.xsendjf = (dataReader["xsendjf"] == null || dataReader["xsendjf"].ToString().Trim() == string.Empty) ? 0 : decimal.Parse(dataReader["xsendjf"].ToString().Trim());
                    good.xchagjf = (dataReader["xchagjf"] == null || dataReader["xchagjf"].ToString().Trim() == string.Empty) ? 0 : decimal.Parse(dataReader["xchagjf"].ToString().Trim());
                }
                good.xls = dataReader["xls"].ToString().Trim();
                good.xlsname = dataReader["xlsname"].ToString().Trim();

                if (goodkinds.Count > 0)
                {
                    good.goodkind = string.Join(",", goodkinds);
                    good.key = good.goodcode + good.goodunit + good.goodkind;
                }
                else
                {
                    good.key = good.goodcode + good.goodunit;
                }

                goodList_NoBar.Add(good);
            }
            dataReader.Close();
            #endregion

            //有条码的货品ID
            List<int> goodIDs = new List<int>();

            goodIDs.AddRange(goodList.Where(r => r.xtableid.HasValue).Select(r => r.xtableid.Value));

            var query = (from a in goodList_NoBar
                         where !goodIDs.Contains(a.xtableid.Value)
                         //join b in goodIDs on a.xtableid equals b into temp
                         // from t in temp.DefaultIfEmpty()
                         //where t == (int?)null
                         select a).ToList();
            goodList.AddRange(query);

            var result = goodList.OrderBy(r => r.goodcode).Distinct().ToList();

            return result;
        }
        #endregion

        #region 根据货品ID取单条货品
        /// <summary>
        /// 根据货品ID取单条货品
        /// </summary>
        /// <param name="goodid">货品ID</param>
        /// <returns></returns>
        public GoodModel GetGoodByID(int goodid,string goodcode)
        {
            #region 货品
            string goodSql = "select * from good where xtableid=@goodid and goodcode=@goodcode";
            SQLiteParameter[] parameters = new SQLiteParameter[2];
            parameters[0] = new SQLiteParameter("goodid", DbType.Int32);
            parameters[0].Value = goodid;
            parameters[1] = new SQLiteParameter("goodcode", DbType.String);
            parameters[1].Value = goodcode;

            SQLiteDataReader dataReader = SQLiteHelper.ExecuteReader(SQLiteHelper.connectionString, CommandType.Text, goodSql, parameters);
            List<GoodModel> goodList = new List<GoodModel>();
            while (dataReader.Read())
            {
                GoodModel good = new GoodModel();
                good.ID = Guid.NewGuid();
                good.goodtype1 = dataReader["goodtype1"].ToString().Trim();
                good.goodtype2 = dataReader["goodtype2"].ToString().Trim();
                good.goodtype3 = dataReader["goodtype3"].ToString().Trim();
                good.goodcode = dataReader["goodcode"].ToString().Trim();
                good.goodname = dataReader["goodname"].ToString().Trim();
                good.goodunit = dataReader["goodunit"].ToString().Trim();
                if (!string.IsNullOrEmpty(dataReader["xmulunit"].ToString().Trim()))
                {
                    good.xmulunit = dataReader["xmulunit"].ToString().Trim();
                }
                good.goodkeys = dataReader["goodkeys"].ToString().Trim();
                good.xprico = (dataReader["xprico"] == null || dataReader["xprico"].ToString().Trim() == string.Empty) ? 0 : decimal.Parse(dataReader["xprico"].ToString().Trim());
                good.xsendjf = (dataReader["xsendjf"] == null || dataReader["xsendjf"].ToString().Trim() == string.Empty) ? 0 : decimal.Parse(dataReader["xsendjf"].ToString().Trim());
                good.xchagjf = (dataReader["xchagjf"] == null || dataReader["xchagjf"].ToString().Trim() == string.Empty) ? 0 : decimal.Parse(dataReader["xchagjf"].ToString().Trim());
                good.xls = dataReader["xls"].ToString().Trim();
                good.xlsname = dataReader["xlsname"].ToString().Trim();
                good.xtableid = int.Parse(dataReader["xtableid"].ToString().Trim());
                good.xsubid = dataReader["xsubid"].ToString().Trim() == string.Empty ? 0 : int.Parse(dataReader["xsubid"].ToString().Trim());
                goodList.Add(good);
            }
            dataReader.Close();
            #endregion

            #region 货品价格
            string goodpricSql = @"select b.*,a.goodkind1,a.goodkind2,a.goodkind3,a.goodkind4,a.goodkind5,a.goodkind6,
                                   a.goodkind7,a.goodkind8,a.goodkind9,a.goodkind10,a.xpric from goodpric a 
                                   inner join  good b on a.xsubid = b.xtableid 
                                   where a.xtableid=@goodid and b.goodcode=@goodcode";

            dataReader = SQLiteHelper.ExecuteReader(SQLiteHelper.connectionString, CommandType.Text, goodpricSql, parameters);

            while (dataReader.Read())
            {
                GoodModel good = new GoodModel();
                good.ID = Guid.NewGuid();
                good.goodtype1 = dataReader["goodtype1"].ToString().Trim();
                good.goodtype2 = dataReader["goodtype2"].ToString().Trim();
                good.goodtype3 = dataReader["goodtype3"].ToString().Trim();
                good.goodcode = dataReader["goodcode"].ToString().Trim();
                good.goodname = dataReader["goodname"].ToString().Trim();
                good.goodunit = dataReader["goodunit"].ToString().Trim();
                if (!string.IsNullOrEmpty(dataReader["xmulunit"].ToString().Trim()))
                {
                    good.xmulunit = dataReader["xmulunit"].ToString().Trim();
                }
                good.goodkeys = dataReader["goodkeys"].ToString().Trim();
                // good.xprico = (dataReader["xprico"] == null || dataReader["xprico"].ToString().Trim() == string.Empty) ? 0 : decimal.Parse(dataReader["xprico"].ToString().Trim());
                good.xsendjf = (dataReader["xsendjf"] == null || dataReader["xsendjf"].ToString().Trim() == string.Empty) ? 0 : decimal.Parse(dataReader["xsendjf"].ToString().Trim());
                good.xchagjf = (dataReader["xchagjf"] == null || dataReader["xchagjf"].ToString().Trim() == string.Empty) ? 0 : decimal.Parse(dataReader["xchagjf"].ToString().Trim());
                good.xls = dataReader["xls"].ToString().Trim();
                good.xlsname = dataReader["xlsname"].ToString().Trim();
                good.xtableid = int.Parse(dataReader["xtableid"].ToString().Trim());
                good.xsubid = dataReader["xsubid"].ToString().Trim() == string.Empty ? (int?)null : int.Parse(dataReader["xsubid"].ToString().Trim());

                good.xprico = (dataReader["xpric"] == null || dataReader["xpric"].ToString().Trim() == string.Empty) ? 0 : decimal.Parse(dataReader["xpric"].ToString().Trim());

                List<string> goodkinds = new List<string>();
                if (!string.IsNullOrEmpty(dataReader["goodkind1"].ToString().Trim()))
                {
                    good.goodkind1 = dataReader["goodkind1"].ToString().Trim();
                    goodkinds.Add(dataReader["goodkind1"].ToString().Trim());
                }
                if (!string.IsNullOrEmpty(dataReader["goodkind2"].ToString().Trim()))
                {
                    good.goodkind2 = dataReader["goodkind2"].ToString().Trim();
                    goodkinds.Add(dataReader["goodkind2"].ToString().Trim());
                }
                if (!string.IsNullOrEmpty(dataReader["goodkind3"].ToString().Trim()))
                {
                    good.goodkind3 = dataReader["goodkind3"].ToString().Trim();
                    goodkinds.Add(dataReader["goodkind3"].ToString().Trim());
                }
                if (!string.IsNullOrEmpty(dataReader["goodkind4"].ToString().Trim()))
                {
                    good.goodkind4 = dataReader["goodkind4"].ToString().Trim();
                    goodkinds.Add(dataReader["goodkind4"].ToString().Trim());
                }
                if (!string.IsNullOrEmpty(dataReader["goodkind5"].ToString().Trim()))
                {
                    good.goodkind5 = dataReader["goodkind5"].ToString().Trim();
                    goodkinds.Add(dataReader["goodkind5"].ToString().Trim());
                }
                if (!string.IsNullOrEmpty(dataReader["goodkind6"].ToString().Trim()))
                {
                    good.goodkind6 = dataReader["goodkind6"].ToString().Trim();
                    goodkinds.Add(dataReader["goodkind6"].ToString().Trim());
                }
                if (!string.IsNullOrEmpty(dataReader["goodkind7"].ToString().Trim()))
                {
                    good.goodkind7 = dataReader["goodkind7"].ToString().Trim();
                    goodkinds.Add(dataReader["goodkind7"].ToString().Trim());
                }
                if (!string.IsNullOrEmpty(dataReader["goodkind8"].ToString().Trim()))
                {
                    good.goodkind8 = dataReader["goodkind8"].ToString().Trim();
                    goodkinds.Add(dataReader["goodkind8"].ToString().Trim());
                }
                if (!string.IsNullOrEmpty(dataReader["goodkind9"].ToString().Trim()))
                {
                    good.goodkind9 = dataReader["goodkind9"].ToString().Trim();
                    goodkinds.Add(dataReader["goodkind9"].ToString().Trim());
                }
                if (!string.IsNullOrEmpty(dataReader["goodkind10"].ToString().Trim()))
                {
                    good.goodkind10 = dataReader["goodkind10"].ToString().Trim();
                    goodkinds.Add(dataReader["goodkind10"].ToString().Trim());
                }

                if (goodkinds.Count > 0)
                {
                    good.goodkind = string.Join(",", goodkinds);
                }
                goodList.Add(good);
            }
            dataReader.Close();
            #endregion
            return goodList.FirstOrDefault();
        }
        #endregion

        #region 根据货品编码取单条货品
        /// <summary>
        /// 根据货品编码取单条货品
        /// </summary>
        /// <param name="goodcode">货品编码</param>
        /// <returns></returns>
        public GoodModel GetGoodByCode(string goodcode)
        {
            #region 货品
            string goodSql = "select * from good where goodcode=@goodcode";
            SQLiteParameter[] parameters = new SQLiteParameter[1];
            parameters[0] = new SQLiteParameter("goodcode", DbType.String);
            parameters[0].Value = goodcode;

            SQLiteDataReader dataReader = SQLiteHelper.ExecuteReader(SQLiteHelper.connectionString, CommandType.Text, goodSql, parameters);
            List<GoodModel> goodList = new List<GoodModel>();
            while (dataReader.Read())
            {
                GoodModel good = new GoodModel();
                good.ID = Guid.NewGuid();
                good.goodtype1 = dataReader["goodtype1"].ToString().Trim();
                good.goodtype2 = dataReader["goodtype2"].ToString().Trim();
                good.goodtype3 = dataReader["goodtype3"].ToString().Trim();
                good.goodcode = dataReader["goodcode"].ToString().Trim();
                good.goodname = dataReader["goodname"].ToString().Trim();
                good.goodunit = dataReader["goodunit"].ToString().Trim();
                if (!string.IsNullOrEmpty(dataReader["xmulunit"].ToString().Trim()))
                {
                    good.xmulunit = dataReader["xmulunit"].ToString().Trim();
                }
                good.goodkeys = dataReader["goodkeys"].ToString().Trim();
                good.xprico = (dataReader["xprico"] == null || dataReader["xprico"].ToString().Trim() == string.Empty) ? 0 : decimal.Parse(dataReader["xprico"].ToString().Trim());
                good.xsendjf = (dataReader["xsendjf"] == null || dataReader["xsendjf"].ToString().Trim() == string.Empty) ? 0 : decimal.Parse(dataReader["xsendjf"].ToString().Trim());
                good.xchagjf = (dataReader["xchagjf"] == null || dataReader["xchagjf"].ToString().Trim() == string.Empty) ? 0 : decimal.Parse(dataReader["xchagjf"].ToString().Trim());
                good.xls = dataReader["xls"].ToString().Trim();
                good.xlsname = dataReader["xlsname"].ToString().Trim();
                good.xtableid = int.Parse(dataReader["xtableid"].ToString().Trim());
                good.xsubid = dataReader["xsubid"].ToString().Trim() == string.Empty ? 0 : int.Parse(dataReader["xsubid"].ToString().Trim());
                goodList.Add(good);
            }
            dataReader.Close();
            #endregion
            return goodList.FirstOrDefault();
        }
        #endregion

        #region 获取兑换货品 
        /// <summary>
        /// 获取兑换货品
        /// </summary>
        /// <returns></returns>
        public List<GoodModel> GetExchangeGoods()
        {
            string cmdText = @"select a.*,xpric,goodkind1,
                        goodkind2,goodkind3,goodkind4,goodkind5,goodkind6,
                        goodkind7,goodkind8,goodkind9,goodkind10,b.xtableid as bxtableid,b.xchagjf as bxchagjf from good a
                        left join goodpric b on b.xsubid = a.xtableid
                        where a.xchagjf>0 or b.xchagjf>0 and (a.xshow == 0 or a.xshow)";

            SQLiteDataReader dataReader = SQLiteHelper.ExecuteReader(SQLiteHelper.connectionString, CommandType.Text, cmdText);
            List<GoodModel> goodList = new List<GoodModel>();
            while (dataReader.Read())
            {
                GoodModel good = new GoodModel();
                good.ID = Guid.NewGuid();
                good.goodtype1 = dataReader["goodtype1"].ToString().Trim();
                good.goodtype2 = dataReader["goodtype2"].ToString().Trim();
                good.goodtype3 = dataReader["goodtype3"].ToString().Trim();
                good.goodcode = dataReader["goodcode"].ToString().Trim();
                good.goodname = dataReader["goodname"].ToString().Trim();
                good.goodunit = dataReader["goodunit"].ToString().Trim();
                if (!string.IsNullOrEmpty(dataReader["xmulunit"].ToString().Trim()))
                {
                    good.xmulunit = dataReader["xmulunit"].ToString().Trim();
                }
                good.goodkeys = dataReader["goodkeys"].ToString().Trim();
                List<string> goodkinds = new List<string>();
                if (dataReader["bxtableid"] != null && dataReader["bxtableid"].ToString() != string.Empty)
                {
                    good.xprico = (dataReader["xpric"] == null || dataReader["xpric"].ToString().Trim() == string.Empty) ? 0 : decimal.Parse(dataReader["xpric"].ToString().Trim());
                    if (!string.IsNullOrEmpty(dataReader["goodkind1"].ToString().Trim()))
                    {
                        good.goodkind1 = dataReader["goodkind1"].ToString().Trim();
                        goodkinds.Add(dataReader["goodkind1"].ToString().Trim());
                    }
                    if (!string.IsNullOrEmpty(dataReader["goodkind2"].ToString().Trim()))
                    {
                        good.goodkind2 = dataReader["goodkind2"].ToString().Trim();
                        goodkinds.Add(dataReader["goodkind2"].ToString().Trim());
                    }
                    if (!string.IsNullOrEmpty(dataReader["goodkind3"].ToString().Trim()))
                    {
                        good.goodkind3 = dataReader["goodkind3"].ToString().Trim();
                        goodkinds.Add(dataReader["goodkind3"].ToString().Trim());
                    }
                    if (!string.IsNullOrEmpty(dataReader["goodkind4"].ToString().Trim()))
                    {
                        good.goodkind4 = dataReader["goodkind4"].ToString().Trim();
                        goodkinds.Add(dataReader["goodkind4"].ToString().Trim());
                    }
                    if (!string.IsNullOrEmpty(dataReader["goodkind5"].ToString().Trim()))
                    {
                        good.goodkind5 = dataReader["goodkind5"].ToString().Trim();
                        goodkinds.Add(dataReader["goodkind5"].ToString().Trim());
                    }
                    if (!string.IsNullOrEmpty(dataReader["goodkind6"].ToString().Trim()))
                    {
                        good.goodkind6 = dataReader["goodkind6"].ToString().Trim();
                        goodkinds.Add(dataReader["goodkind6"].ToString().Trim());
                    }
                    if (!string.IsNullOrEmpty(dataReader["goodkind7"].ToString().Trim()))
                    {
                        good.goodkind7 = dataReader["goodkind7"].ToString().Trim();
                        goodkinds.Add(dataReader["goodkind7"].ToString().Trim());
                    }
                    if (!string.IsNullOrEmpty(dataReader["goodkind8"].ToString().Trim()))
                    {
                        good.goodkind8 = dataReader["goodkind8"].ToString().Trim();
                        goodkinds.Add(dataReader["goodkind8"].ToString().Trim());
                    }
                    if (!string.IsNullOrEmpty(dataReader["goodkind9"].ToString().Trim()))
                    {
                        good.goodkind9 = dataReader["goodkind9"].ToString().Trim();
                        goodkinds.Add(dataReader["goodkind9"].ToString().Trim());
                    }
                    if (!string.IsNullOrEmpty(dataReader["goodkind10"].ToString().Trim()))
                    {
                        good.goodkind10 = dataReader["goodkind10"].ToString().Trim();
                        goodkinds.Add(dataReader["goodkind10"].ToString().Trim());
                    }
                    good.xtableid = int.Parse(dataReader["bxtableid"].ToString().Trim());
                    good.xchagjf = (dataReader["bxchagjf"] == null || dataReader["bxchagjf"].ToString().Trim() == string.Empty) ? 0 : decimal.Parse(dataReader["bxchagjf"].ToString().Trim());
                }
                else
                {
                    good.xprico = (dataReader["xprico"] == null || dataReader["xprico"].ToString().Trim() == string.Empty) ? 0 : decimal.Parse(dataReader["xprico"].ToString().Trim());
                    good.xtableid = int.Parse(dataReader["xtableid"].ToString().Trim());
                    good.xchagjf = (dataReader["xchagjf"] == null || dataReader["xchagjf"].ToString().Trim() == string.Empty) ? 0 : decimal.Parse(dataReader["xchagjf"].ToString().Trim());
                }
                good.xsendjf = (dataReader["xsendjf"] == null || dataReader["xsendjf"].ToString().Trim() == string.Empty) ? 0 : decimal.Parse(dataReader["xsendjf"].ToString().Trim());
                good.xls = dataReader["xls"].ToString().Trim();
                good.xlsname = dataReader["xlsname"].ToString().Trim();
                good.Quantity = 1;
                good.totalxchagjf = good.xchagjf;
                if (goodkinds.Count > 0)
                {
                    good.goodkind = string.Join(",", goodkinds);
                    good.key = good.goodcode + good.goodunit + string.Join(",", goodkinds);
                }
                else
                {
                    good.key = good.goodcode + good.goodunit;
                }
                goodList.Add(good);
            }
            dataReader.Close();
            return goodList.Where(r => r.xchagjf > 0).ToList();
        }
        #endregion

        #region 获取会员货品等级价格
        /// <summary>
        /// 获取会员货品等级价格
        /// </summary>
        /// <param name="goodid">货品ID</param>
        /// <returns></returns>
        public Goodpric2Model GetGoodpric2(int goodid, string clntclss)
        {
            #region 货品
            string goodSql = "select clntclss,xpric from goodpric2 where xsubid=@goodid and clntclss=@clntclss";
            SQLiteParameter[] parameters = new SQLiteParameter[2];
            parameters[0] = new SQLiteParameter("goodid", DbType.Int32);
            parameters[0].Value = goodid;
            parameters[1] = new SQLiteParameter("clntclss", DbType.String);
            parameters[1].Value = clntclss;

            SQLiteDataReader dataReader = SQLiteHelper.ExecuteReader(SQLiteHelper.connectionString, CommandType.Text, goodSql, parameters);
            List<Goodpric2Model> goodList = new List<Goodpric2Model>();
            while (dataReader.Read())
            {
                Goodpric2Model goodPric2 = new Goodpric2Model();
                goodPric2.clntclss = dataReader["clntclss"].ToString().Trim();
                goodPric2.xpric = dataReader["xpric"].ToString().Trim() == string.Empty ? 0 : decimal.Parse(dataReader["xpric"].ToString().Trim());
                goodList.Add(goodPric2);
            }
            dataReader.Close();
            #endregion

            return goodList.FirstOrDefault();
        }
        #endregion

        #region 获取货品大类
        /// <summary>
        /// 获取货品大类
        /// </summary>
        /// <param name="goodtype">货品类型</param>
        /// <returns></returns>
        public Goodtype1Model GetGoodtype1(string goodtype)
        {
            string goodSql = "select * from goodtype1 where goodtype1=@goodtype";
            SQLiteParameter[] parameters = new SQLiteParameter[1];
            parameters[0] = new SQLiteParameter("goodtype", DbType.String);
            parameters[0].Value = goodtype;

            SQLiteDataReader dataReader = SQLiteHelper.ExecuteReader(SQLiteHelper.connectionString, CommandType.Text, goodSql, parameters);
            List<Goodtype1Model> goodTypeList = new List<Goodtype1Model>();
            while (dataReader.Read())
            {
                Goodtype1Model goodType = new Goodtype1Model();
                goodType.goodtype1 = dataReader["goodtype1"].ToString().Trim();
                goodType.uclssprics = dataReader["uclssprics"].ToString().Trim();
                goodType.xtableid = (dataReader["xtableid"] == null || dataReader["xtableid"].ToString().Trim() == string.Empty) ? (int?)null : int.Parse(dataReader["xtableid"].ToString().Trim());
                goodTypeList.Add(goodType);
            }
            dataReader.Close();
            return goodTypeList.FirstOrDefault();
        }
        #endregion

        #region 获取货品中类
        /// <summary>
        /// 获取货品中类
        /// </summary>
        /// <param name="goodtype">货品类型</param>
        /// <returns></returns>
        public Goodtype2Model GetGoodtype2(string goodtype)
        {
            string goodSql = "select * from goodtype2 where goodtype2=@goodtype";
            SQLiteParameter[] parameters = new SQLiteParameter[1];
            parameters[0] = new SQLiteParameter("goodtype", DbType.String);
            parameters[0].Value = goodtype;

            SQLiteDataReader dataReader = SQLiteHelper.ExecuteReader(SQLiteHelper.connectionString, CommandType.Text, goodSql, parameters);
            List<Goodtype2Model> goodTypeList = new List<Goodtype2Model>();
            while (dataReader.Read())
            {
                Goodtype2Model goodType = new Goodtype2Model();
                goodType.goodtype1 = dataReader["goodtype1"].ToString().Trim();
                goodType.goodtype2 = dataReader["goodtype2"].ToString().Trim();
                goodType.uclssprics = dataReader["uclssprics"].ToString().Trim();
                goodType.xtableid = (dataReader["xtableid"] == null || dataReader["xtableid"].ToString().Trim() == string.Empty) ? (int?)null : int.Parse(dataReader["xtableid"].ToString().Trim());
                goodTypeList.Add(goodType);
            }
            dataReader.Close();
            return goodTypeList.FirstOrDefault();
        }
        #endregion

        #region 获取货品小类
        /// <summary>
        /// 获取货品中类
        /// </summary>
        /// <param name="goodtype">货品类型</param>
        /// <returns></returns>
        public Goodtype3Model GetGoodtype3(string goodtype)
        {
            string goodSql = "select * from goodtype3 where goodtype3=@goodtype";
            SQLiteParameter[] parameters = new SQLiteParameter[1];
            parameters[0] = new SQLiteParameter("goodtype", DbType.String);
            parameters[0].Value = goodtype;

            SQLiteDataReader dataReader = SQLiteHelper.ExecuteReader(SQLiteHelper.connectionString, CommandType.Text, goodSql, parameters);
            List<Goodtype3Model> goodTypeList = new List<Goodtype3Model>();
            while (dataReader.Read())
            {
                Goodtype3Model goodType = new Goodtype3Model();
                goodType.goodtype1 = dataReader["goodtype1"].ToString().Trim();
                goodType.goodtype2 = dataReader["goodtype2"].ToString().Trim();
                goodType.uclssprics = dataReader["uclssprics"].ToString().Trim();
                goodType.xtableid = (dataReader["xtableid"] == null || dataReader["xtableid"].ToString().Trim() == string.Empty) ? (int?)null : int.Parse(dataReader["xtableid"].ToString().Trim());
                goodTypeList.Add(goodType);
            }
            dataReader.Close();
            return goodTypeList.FirstOrDefault();
        }
        #endregion

        #region  获取当前仓库的库存
        /// <summary>
        /// 获取当前仓库的库存
        /// </summary>
        /// <param name="xls"></param>
        /// <param name="cnkucode"></param>
        /// <returns></returns>
        public List<Ku2Model> GetKu2(string xls, string cnkucode)
        {
            SQLiteParameter[] parameters = new SQLiteParameter[2];
            parameters[0] = new SQLiteParameter("xls", DbType.String);
            parameters[0].Value = xls;
            parameters[1] = new SQLiteParameter("cnkucode", DbType.String);
            parameters[1].Value = cnkucode;

            string cmdText = @"SELECT SID,xls,xlsname,cnkucode,cnkuname,goodcode,goodname,
                               goodunit,goodkind1,goodkind2,goodkind3,goodkind4,
                               goodkind5,goodkind6,goodkind7,goodkind8,goodkind9,
                               goodkind10,xquatku,xversion
                          FROM ku2 where xls=@xls and cnkucode=@cnkucode";

            List<Ku2Model> ku2s = new List<Ku2Model>();
            SQLiteDataReader dataReader = SQLiteHelper.ExecuteReader(SQLiteHelper.connectionString, CommandType.Text, cmdText, parameters);
            while (dataReader.Read())
            {
                Ku2Model ku = new Ku2Model();
                ku.SID =int.Parse(dataReader["SID"].ToString().Trim());
                ku.goodcode = dataReader["goodcode"].ToString().Trim();
                ku.goodname = dataReader["goodname"].ToString().Trim();
                ku.goodunit = dataReader["goodunit"].ToString().Trim();
                ku.xls = dataReader["xls"].ToString().Trim();
                ku.xlsname = dataReader["xlsname"].ToString().Trim();
                ku.xquatku = decimal.Parse(dataReader["xquatku"].ToString().Trim());
                List<string> goodkinds = new List<string>();
                if (!string.IsNullOrEmpty(dataReader["goodkind1"].ToString().Trim()))
                {
                    ku.goodkind1 = dataReader["goodkind1"].ToString().Trim();
                    goodkinds.Add(dataReader["goodkind1"].ToString().Trim());
                }
                if (!string.IsNullOrEmpty(dataReader["goodkind2"].ToString().Trim()))
                {
                    ku.goodkind2 = dataReader["goodkind2"].ToString().Trim();
                    goodkinds.Add(dataReader["goodkind2"].ToString().Trim());
                }
                if (!string.IsNullOrEmpty(dataReader["goodkind3"].ToString().Trim()))
                {
                    ku.goodkind3 = dataReader["goodkind3"].ToString().Trim();
                    goodkinds.Add(dataReader["goodkind3"].ToString().Trim());
                }
                if (!string.IsNullOrEmpty(dataReader["goodkind4"].ToString().Trim()))
                {
                    ku.goodkind4 = dataReader["goodkind4"].ToString().Trim();
                    goodkinds.Add(dataReader["goodkind4"].ToString().Trim());
                }
                if (!string.IsNullOrEmpty(dataReader["goodkind5"].ToString().Trim()))
                {
                    ku.goodkind5 = dataReader["goodkind5"].ToString().Trim();
                    goodkinds.Add(dataReader["goodkind5"].ToString().Trim());
                }
                if (!string.IsNullOrEmpty(dataReader["goodkind6"].ToString().Trim()))
                {
                    ku.goodkind6 = dataReader["goodkind6"].ToString().Trim();
                    goodkinds.Add(dataReader["goodkind6"].ToString().Trim());
                }
                if (!string.IsNullOrEmpty(dataReader["goodkind7"].ToString().Trim()))
                {
                    ku.goodkind7 = dataReader["goodkind7"].ToString().Trim();
                    goodkinds.Add(dataReader["goodkind7"].ToString().Trim());
                }
                if (!string.IsNullOrEmpty(dataReader["goodkind8"].ToString().Trim()))
                {
                    ku.goodkind8 = dataReader["goodkind8"].ToString().Trim();
                    goodkinds.Add(dataReader["goodkind8"].ToString().Trim());
                }
                if (!string.IsNullOrEmpty(dataReader["goodkind9"].ToString().Trim()))
                {
                    ku.goodkind9 = dataReader["goodkind9"].ToString().Trim();
                    goodkinds.Add(dataReader["goodkind9"].ToString().Trim());
                }
                if (!string.IsNullOrEmpty(dataReader["goodkind10"].ToString().Trim()))
                {
                    ku.goodkind10 = dataReader["goodkind10"].ToString().Trim();
                    goodkinds.Add(dataReader["goodkind10"].ToString().Trim());
                }
                ku.xversion = Double.Parse(dataReader["xversion"].ToString().Trim());
                if (goodkinds.Count > 0)
                {
                    ku.key = ku.goodcode + ku.goodunit + string.Join(",", goodkinds);
                }
                else
                {
                    ku.key = ku.goodcode + ku.goodunit;
                }
                ku2s.Add(ku);
            }
            dataReader.Close();
            return ku2s;
        }
        #endregion

        #region  获取所有仓库的库存
        /// <summary>
        /// 获取所有仓库的库存
        /// </summary>
        /// <param name="xls"></param>
        /// <param name="cnkucode"></param>
        /// <returns></returns>
        public List<Ku2Model> GetKu2()
        {
            string cmdText = @"SELECT goodcode,goodname,
                               goodunit,goodkind1,goodkind2,goodkind3,goodkind4,
                               goodkind5,goodkind6,goodkind7,goodkind8,goodkind9,
                               goodkind10,xquatku
                          FROM ku2";

            List<Ku2Model> ku2s = new List<Ku2Model>();
            SQLiteDataReader dataReader = SQLiteHelper.ExecuteReader(SQLiteHelper.connectionString, CommandType.Text, cmdText);
            while (dataReader.Read())
            {
                Ku2Model ku = new Ku2Model();
                ku.goodcode = dataReader["goodcode"].ToString().Trim();
                ku.goodname = dataReader["goodname"].ToString().Trim();
                ku.goodunit = dataReader["goodunit"].ToString().Trim();
                ku.xquatku = decimal.Parse(dataReader["xquatku"].ToString().Trim());
                List<string> goodkinds = new List<string>();
                if (!string.IsNullOrEmpty(dataReader["goodkind1"].ToString().Trim()))
                {
                    ku.goodkind1 = dataReader["goodkind1"].ToString().Trim();
                    goodkinds.Add(dataReader["goodkind1"].ToString().Trim());
                }
                if (!string.IsNullOrEmpty(dataReader["goodkind2"].ToString().Trim()))
                {
                    ku.goodkind2 = dataReader["goodkind2"].ToString().Trim();
                    goodkinds.Add(dataReader["goodkind2"].ToString().Trim());
                }
                if (!string.IsNullOrEmpty(dataReader["goodkind3"].ToString().Trim()))
                {
                    ku.goodkind3 = dataReader["goodkind3"].ToString().Trim();
                    goodkinds.Add(dataReader["goodkind3"].ToString().Trim());
                }
                if (!string.IsNullOrEmpty(dataReader["goodkind4"].ToString().Trim()))
                {
                    ku.goodkind4 = dataReader["goodkind4"].ToString().Trim();
                    goodkinds.Add(dataReader["goodkind4"].ToString().Trim());
                }
                if (!string.IsNullOrEmpty(dataReader["goodkind5"].ToString().Trim()))
                {
                    ku.goodkind5 = dataReader["goodkind5"].ToString().Trim();
                    goodkinds.Add(dataReader["goodkind5"].ToString().Trim());
                }
                if (!string.IsNullOrEmpty(dataReader["goodkind6"].ToString().Trim()))
                {
                    ku.goodkind6 = dataReader["goodkind6"].ToString().Trim();
                    goodkinds.Add(dataReader["goodkind6"].ToString().Trim());
                }
                if (!string.IsNullOrEmpty(dataReader["goodkind7"].ToString().Trim()))
                {
                    ku.goodkind7 = dataReader["goodkind7"].ToString().Trim();
                    goodkinds.Add(dataReader["goodkind7"].ToString().Trim());
                }
                if (!string.IsNullOrEmpty(dataReader["goodkind8"].ToString().Trim()))
                {
                    ku.goodkind8 = dataReader["goodkind8"].ToString().Trim();
                    goodkinds.Add(dataReader["goodkind8"].ToString().Trim());
                }
                if (!string.IsNullOrEmpty(dataReader["goodkind9"].ToString().Trim()))
                {
                    ku.goodkind9 = dataReader["goodkind9"].ToString().Trim();
                    goodkinds.Add(dataReader["goodkind9"].ToString().Trim());
                }
                if (!string.IsNullOrEmpty(dataReader["goodkind10"].ToString().Trim()))
                {
                    ku.goodkind10 = dataReader["goodkind10"].ToString().Trim();
                    goodkinds.Add(dataReader["goodkind10"].ToString().Trim());
                }
                if (goodkinds.Count > 0)
                {
                    ku.key = ku.goodcode + ku.goodunit + string.Join(",", goodkinds);
                }
                else
                {
                    ku.key = ku.goodcode + ku.goodunit;
                }
                ku2s.Add(ku);
            }
            dataReader.Close();
            var query = ( from p in ku2s
                        group p by new { p.key} into g
                        select new Ku2Model
                        {
                            key=g.Key.key,
                            xquatku=g.Sum(r=>r.xquatku)
                        }).ToList();
            return query;
        }
        #endregion

        #region  获取当个商品所有的库存
        /// <summary>
        /// 获取当个商品所有的库存
        /// </summary>
        /// <param name="key">货品代码货品名称</param>
        /// <returns></returns>
        public List<Ku2Model> GetAllKu2(GoodModel good)
        {
            SQLiteParameter[] parameters = new SQLiteParameter[11];
            parameters[0] = new SQLiteParameter("goodcode", DbType.String);
            parameters[0].Value = good.goodcode;
            parameters[1] = new SQLiteParameter("goodkind1", DbType.String);
            parameters[1].Value = good.goodkind1 == null ?string.Empty: good.goodkind1;
            parameters[2] = new SQLiteParameter("goodkind2", DbType.String);
            parameters[2].Value = good.goodkind2 == null ? string.Empty : good.goodkind2;
            parameters[3] = new SQLiteParameter("goodkind3", DbType.String);
            parameters[3].Value = good.goodkind3 == null ? string.Empty : good.goodkind3;
            parameters[4] = new SQLiteParameter("goodkind4", DbType.String);
            parameters[4].Value = good.goodkind4 == null ? string.Empty : good.goodkind4;
            parameters[5] = new SQLiteParameter("goodkind5", DbType.String);
            parameters[5].Value = good.goodkind5 == null ? string.Empty : good.goodkind5;
            parameters[6] = new SQLiteParameter("goodkind6", DbType.String);
            parameters[6].Value = good.goodkind6 == null ? string.Empty : good.goodkind6;
            parameters[7] = new SQLiteParameter("goodkind7", DbType.String);
            parameters[7].Value = good.goodkind7 == null ? string.Empty : good.goodkind7;
            parameters[8] = new SQLiteParameter("goodkind8", DbType.String);
            parameters[8].Value = good.goodkind8 == null ? string.Empty : good.goodkind8;
            parameters[9] = new SQLiteParameter("goodkind9", DbType.String);
            parameters[9].Value = good.goodkind9 == null ? string.Empty : good.goodkind9;
            parameters[10] = new SQLiteParameter("goodkind10", DbType.String);
            parameters[10].Value = good.goodkind10 == null ? string.Empty : good.goodkind10;

            string cmdText = @"SELECT SID,xls,xlsname,cnkucode,cnkuname,goodcode,goodname,
                               goodunit,xquatku,xlastime
                          FROM ku2 where goodcode = @goodcode 
                          and ifnull(goodkind1,'')=@goodkind1
                        and ifnull(goodkind2,'')=@goodkind2
                        and ifnull(goodkind3,'')=@goodkind3
                        and ifnull(goodkind4,'')=@goodkind4
                        and ifnull(goodkind5,'')=@goodkind5
                        and ifnull(goodkind6,'')=@goodkind6
                        and ifnull(goodkind7,'')=@goodkind7
                        and ifnull(goodkind8,'')=@goodkind8
                        and ifnull(goodkind9,'')=@goodkind9
                        and ifnull(goodkind10,'')=@goodkind10";

            List<Ku2Model> ku2s = new List<Ku2Model>();
            SQLiteDataReader dataReader = SQLiteHelper.ExecuteReader(SQLiteHelper.connectionString, CommandType.Text, cmdText, parameters);
            while (dataReader.Read())
            {
                Ku2Model ku = new Ku2Model();
                ku.SID = int.Parse(dataReader["SID"].ToString().Trim());
                ku.goodcode = dataReader["goodcode"].ToString().Trim();
                ku.goodname = dataReader["goodname"].ToString().Trim();
                ku.cnkucode = dataReader["cnkucode"].ToString().Trim();
                ku.cnkuname = dataReader["cnkuname"].ToString().Trim();
                ku.goodunit = dataReader["goodunit"].ToString().Trim();
                ku.xls = dataReader["xls"].ToString().Trim();
                ku.xlsname = dataReader["xlsname"].ToString().Trim();
                ku.xquatku = decimal.Parse(dataReader["xquatku"].ToString().Trim());
                ku.xlastime = dataReader["xlastime"].ToString().Trim();
                ku2s.Add(ku);
            }
            dataReader.Close();
            return ku2s;
        }
        #endregion
    }
}
