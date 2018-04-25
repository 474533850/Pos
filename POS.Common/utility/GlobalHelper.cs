using POS.Model;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Web;

namespace POS.Common.utility
{
    public class GlobalHelper
    {
        private static Uri baseUrl_Pull = null;
        private static Uri baseUrl_Push = null;
        public static Uri GetBaseUrl()
        {
            return new Uri(ConfigurationManager.AppSettings["serviceUrl"].ToString());
        }

        #region 获取下载数据连接
        /// <summary>
        /// 获取下载数据连接
        /// </summary>
        /// <returns></returns>
        public static Uri GetBaseUrl_Pull()
        {
            baseUrl_Pull = new Uri(GetBaseUrl(), @"pos/pull");
            return baseUrl_Pull;

        }
        /// <summary>
        /// 获取下载数据连接
        /// </summary>
        /// <param name="sid"></param>
        /// <param name="usercode"></param>
        /// <param name="password"></param>
        /// <param name="sql"></param>
        /// <returns></returns>
        public static string GetBaseUrl_Pull(string sid, string username,string password,string sql)
        {
            string url = string.Format("{0}?COMP={1}&USER={2}&PASSWORD={3}&SHOWSQL={4}", GetBaseUrl(),sid, username, password,sql);
            return url;

        }
        #endregion

        #region 获取上传数据连接
        /// <summary>
        /// 获取上传数据连接
        /// </summary>
        /// <returns></returns>
        public static Uri GetbaseUrl_Push()
        {
            baseUrl_Push = new Uri(GetBaseUrl(), @"pos/push");
            return baseUrl_Push;
        }
        #endregion

        #region 获取需要上传数据的参数
        /// <summary>
        /// 获取需要上传数据的参数
        /// </summary>
        /// <param name="sid"></param>
        /// <param name="mod"></param>
        /// <param name="xls"></param>
        /// <param name="usercode"></param>
        /// <param name="json"></param>
        /// <returns></returns>
        public static string GetPostData(string sid, string mod, string xls, string usercode, string json, string secretKey)
        {
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add("sid", sid);
            dic.Add("mod", mod);
            dic.Add("xls", xls);
            dic.Add("usercode", usercode);
            dic.Add("data", json);
            dic = dic.OrderBy(r => r.Key).ToDictionary(r => r.Key, o => o.Value);
            StringBuilder str = new StringBuilder();
            foreach (KeyValuePair<string, object> item in dic)
            {
                str.AppendFormat("{0}={1}&", item.Key, item.Value);
            }
            str.Append("key=");
            string key = str.Append(secretKey).ToString().Trim();
            string sign = MD5Helper.GetMd5Hash(key).ToLower();
            string postData = string.Format("sid={0}&mod={1}&xls={2}&usercode={3}&data={4}&sign={5}", sid, mod, xls, usercode, HttpUtility.UrlEncode(json, Encoding.UTF8), sign);
            return postData;
        }
        public static string GetPostData(Dictionary<string, object> dic, string secretKey)
        {
            dic = dic.OrderBy(r => r.Key).ToDictionary(r => r.Key, o => o.Value);
            StringBuilder str = new StringBuilder();
            StringBuilder url = new StringBuilder();
            foreach (KeyValuePair<string, object> item in dic)
            {
                str.AppendFormat("{0}={1}&", item.Key, item.Value);
                url.AppendFormat("{0}={1}&", item.Key, item.Key == "data"? HttpUtility.UrlEncode(item.Value.ToString(), Encoding.UTF8) : item.Value);
            }
            str.Append("key=");
            string key = str.Append(secretKey).ToString().Trim();
            string sign = MD5Helper.GetMd5Hash(key).ToLower();
            url.AppendFormat("sign={0}",sign);
            string postData = url.ToString();
            return postData;
        }
        #endregion

        #region 获取签名
        /// <summary>
        /// 获取签名 必须添加分部代码
        /// </summary>
        /// <param name="queryString">url 参数部分</param>
        /// <param name="secretKey">密钥</param>
        /// <returns></returns>
        public static string GetSign(string queryString,string secretKey)
        {
            Dictionary<string, object> dic = GlobalHelper.GetQueryString(queryString);
            dic = dic.OrderBy(r => r.Key).ToDictionary(r => r.Key, o => o.Value);
            StringBuilder str = new StringBuilder();
            foreach (KeyValuePair<string, object> item in dic)
            {
                str.AppendFormat("{0}={1}&", item.Key, item.Value);
            }
            str.Append("key=");
            string key = str.Append(secretKey).ToString().Trim();
            string sign = MD5Helper.GetMd5Hash(key).ToLower();
            return sign;
        }
        #endregion

        /// <summary>
        /// 将查询字符串解析转换为名值集合.
        /// </summary>
        /// <param name="queryString"></param>
        /// <returns></returns>
        public static Dictionary<string, object> GetQueryString(string queryString)
        {
            return GetQueryString(queryString, null, true);
        }

        #region 将查询字符串解析转换为名值集合
        /// <summary>
        /// 将查询字符串解析转换为名值集合.
        /// </summary>
        /// <param name="queryString"></param>
        /// <param name="encoding"></param>
        /// <param name="isEncoded"></param>
        /// <returns></returns>
        private static Dictionary<string, object> GetQueryString(string queryString, Encoding encoding, bool isEncoded)
        {
            queryString = queryString.Replace("?", "");
            Dictionary<string, object> result = new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase);
            if (!string.IsNullOrEmpty(queryString))
            {
                int count = queryString.Length;
                for (int i = 0; i < count; i++)
                {
                    int startIndex = i;
                    int index = -1;
                    while (i < count)
                    {
                        char item = queryString[i];
                        if (item == '=')
                        {
                            if (index < 0)
                            {
                                index = i;
                            }
                        }
                        else if (item == '&')
                        {
                            break;
                        }
                        i++;
                    }
                    string key = null;
                    string value = null;
                    if (index >= 0)
                    {
                        key = queryString.Substring(startIndex, index - startIndex);
                        value = queryString.Substring(index + 1, (i - index) - 1);
                    }
                    else
                    {
                        key = queryString.Substring(startIndex, i - startIndex);
                    }
                    if (isEncoded)
                    {
                        result[MyUrlDeCode(key, encoding)] = MyUrlDeCode(value, encoding);
                    }
                    else
                    {
                        result[key] = value;
                    }
                    if ((i == (count - 1)) && (queryString[i] == '&'))
                    {
                        result[key] = string.Empty;
                    }
                }
            }
            return result;
        }
        #endregion

        #region 解码URL
        /// <summary>
        /// 解码URL.
        /// </summary>
        /// <param name="encoding">null为自动选择编码</param>
        /// <param name="str"></param>
        /// <returns></returns>
        private static string MyUrlDeCode(string str, Encoding encoding)
        {
            if (encoding == null)
            {
                Encoding utf8 = Encoding.UTF8;
                //首先用utf-8进行解码                     
                string code = HttpUtility.UrlDecode(str.ToUpper(), utf8);
                //将已经解码的字符再次进行编码.
                string encode = HttpUtility.UrlEncode(code, utf8).ToUpper();
                if (str == encode)
                    encoding = Encoding.UTF8;
                else
                    encoding = Encoding.GetEncoding("gb2312");
            }
            return HttpUtility.UrlDecode(str, encoding);
        }
        #endregion
    }
}
