using POS.Common.utility;
using POS.Model;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Web.Script.Serialization;

namespace POS.DAL
{
    public class AutoUpdaterDAL
    {
        static Uri baseUrl = new Uri(ConfigurationManager.AppSettings["serviceUrl"].ToString());
        static JavaScriptSerializer js = new JavaScriptSerializer();
        /// <summary>
        /// 获取升级信息
        /// </summary>
        public static UpgradeModel GetUpgradeInfo()
        {
            try
            {
                string urlStr = string.Format(@"pos/upgrade.json");
                Uri url = new Uri(baseUrl, urlStr);
                string r = HttpClient.Get(url, null);
                UpgradeModel syncResult = js.Deserialize<UpgradeModel>(r);
                return syncResult;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
