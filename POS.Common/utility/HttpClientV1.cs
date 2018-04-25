﻿using POS.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Cache;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Script.Serialization;

namespace POS.Common.utility
{
    public class HttpClientV1
    {
        static JavaScriptSerializer js = new JavaScriptSerializer();
        static object obj = new object();

        public static ManualResetEvent allDone = new ManualResetEvent(false);
        const int BUFFER_SIZE = 1024;
        const int DefaultTimeout = 2 * 60 * 1000; // 2 minutes timeout

        static ApplicationLogger logger = new ApplicationLogger(typeof(HttpClientV1).Name);
        public HttpClientV1()
        {
            js.MaxJsonLength = 2147483644;
        }

        private static void TimeoutCallback(object state, bool timedOut)
        {
            if (timedOut)
            {
                HttpWebRequest request = state as HttpWebRequest;
                if (request != null)
                {
                    request.Abort();
                }
            }
        }
        public static string Get(Uri url, int? timeout)
        {
            string result = null;
            HttpWebRequest request = null;
            WebResponse response = null;
            Stream stream = null;
            try
            {
                System.Net.ServicePointManager.DefaultConnectionLimit = 200;
                System.GC.Collect();
                request = (System.Net.HttpWebRequest)System.Net.HttpWebRequest.Create(url);
                request.KeepAlive = false;
                if (timeout.HasValue)
                {
                    request.Timeout = timeout.Value;
                    request.ReadWriteTimeout = timeout.Value;
                }
                request.Proxy = null;
                request.Method = "GET";

                response = request.GetResponse();
                stream = response.GetResponseStream();
                using (StreamReader sr = new StreamReader(stream, Encoding.UTF8))
                {
                    result = sr.ReadToEnd();
                }
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (stream != null)
                {
                    stream.Close();
                    stream.Dispose();
                }
                if (response != null)
                {
                    response.Close();
                    response = null;
                }
                if (request != null)
                {
                    request.Abort();
                    request = null;
                }
            }

        }

        #region 异步获取服务端数据
        /// <summary>
        /// 异步获取服务端数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="url"></param>
        /// <param name="timeout"></param>
        /// <returns></returns>
        public static SyncResultModel<T> GetAasync1<T>(string url, int? timeout) where T : class
        {
            SyncResultModel<T> result = null;
            HttpWebRequest request = null;
            WebResponse response = null;
            Stream stream = null;
            try
            {
                System.Net.ServicePointManager.DefaultConnectionLimit = 200;
                System.GC.Collect();
                request = (System.Net.HttpWebRequest)System.Net.HttpWebRequest.Create(url);
                //request.KeepAlive = false;
                if (timeout.HasValue)
                {
                    request.Timeout = timeout.Value;
                    request.ReadWriteTimeout = timeout.Value;
                }
                request.Proxy = null;
                request.Method = "GET";
                var task = Task<WebResponse>.Factory.FromAsync(request.BeginGetResponse, request.EndGetResponse, null);
                response = task.Result;
                stream = response.GetResponseStream();
                using (StreamReader sr = new StreamReader(stream, Encoding.UTF8))
                {
                    string data = sr.ReadToEnd();
                    result = js.Deserialize<SyncResultModel<T>>(data);
                }
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (stream != null)
                {
                    stream.Close();
                    stream.Dispose();
                }
                if (response != null)
                {
                    response.Close();
                    response = null;
                }
                if (request != null)
                {
                    request.Abort();
                    result = null;
                }
            }
        }
        // static ManualResetEvent allDone = new ManualResetEvent(false);
        public static void GetAasyncV2<T>(string url, int? timeout, Action<SyncResultModel<T>> act) where T : class
        {
            HttpWebRequest request = null;
            try
            {
                System.Net.ServicePointManager.DefaultConnectionLimit = 200;
                System.GC.Collect();
                request = (System.Net.HttpWebRequest)System.Net.HttpWebRequest.Create(url);
                request.KeepAlive = false;
                if (timeout.HasValue)
                {
                    request.Timeout = timeout.Value;
                    request.ReadWriteTimeout = timeout.Value;
                }
                request.Proxy = null;
                request.Method = "GET";
                StateObject<SyncResultModel<T>> state = new StateObject<SyncResultModel<T>>();
                state.Action = act;
                state.HttpWebRequest = request;
                request.BeginGetResponse(AsyncCallback<T>, state);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static void GetAasync<T>(string url, int? timeout, Action<SyncResultModel<T>> act) where T : class
        {
            allDone = new ManualResetEvent(false);
            HttpWebRequest request = null;
            //WebResponse response = null;
            try
            {
                System.Net.ServicePointManager.DefaultConnectionLimit = 200;
                System.Net.ServicePointManager.CheckCertificateRevocationList = false;
                System.GC.Collect();
                request = (System.Net.HttpWebRequest)System.Net.HttpWebRequest.Create(url);
                request.KeepAlive = false;
                //忽略缓存，完全使用服务器满足请求
                request.CachePolicy = new RequestCachePolicy(RequestCacheLevel.NoCacheNoStore);
                request.ServicePoint.Expect100Continue = false;
                request.ServicePoint.ConnectionLimit = 500;
                if (timeout.HasValue)
                {
                    request.Timeout = timeout.Value;
                    request.ReadWriteTimeout = timeout.Value;
                }
                request.Proxy = null;
                request.Method = "GET";
                StateObject<SyncResultModel<T>> state = new StateObject<SyncResultModel<T>>();
                state.Action = act;
                state.HttpWebRequest = request;
                IAsyncResult result = request.BeginGetResponse(AsyncCallback<T>, state);

                ThreadPool.RegisterWaitForSingleObject(result.AsyncWaitHandle, new WaitOrTimerCallback(TimeoutCallback), request, DefaultTimeout, true);
                allDone.WaitOne();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            //finally
            //{
            //    if (response != null)
            //    {
            //        response.Close();
            //    }
            //    if (request != null)
            //    {
            //        request.Abort();
            //    }
            //}
        }
        private static void AsyncCallback<T>(IAsyncResult ar) where T : class
        {
            lock (obj)
            {
                WebRequest request = null;
                WebResponse response = null;
                Stream stream = null;
                try
                {
                    StateObject<SyncResultModel<T>> state = ar.AsyncState as StateObject<SyncResultModel<T>>;
                    request = state.HttpWebRequest as WebRequest;
                    response = request.EndGetResponse(ar);
                    stream = response.GetResponseStream();
                    using (StreamReader sr = new StreamReader(stream, Encoding.UTF8))
                    {
                        string result = sr.ReadToEnd();
                        SyncResultModel<T> data = js.Deserialize<SyncResultModel<T>>(result);
                        state.Action?.Invoke(data);
                    }
                }
                catch (Exception ex)
                {
                    logger.Info(ex.Message);
                    //throw ex;
                }
                finally
                {
                    if (stream != null)
                    {
                        stream.Close();
                        stream.Dispose();
                    }
                    if (response != null)
                    {
                        response.Close();
                        response = null;
                    }
                    if (request != null)
                    {
                        request.Abort();
                        request = null;
                    }
                    allDone.Set();
                }
            }
        }
        #endregion

        #region 异步提交数据
        public static SyncResultModel<T> PostAasync<T>(string url, int? timeout, string postdata) where T : class
        {
            allDone = new ManualResetEvent(false);
            SyncResultModel<T> result = null;
            HttpWebRequest request = null;
            WebResponse response = null;
            try
            {
                System.Net.ServicePointManager.DefaultConnectionLimit = 200;
                System.Net.ServicePointManager.CheckCertificateRevocationList = false;
                System.GC.Collect();
                request = WebRequest.Create(new Uri(url)) as HttpWebRequest;
                request.KeepAlive = false;
                //忽略缓存，完全使用服务器满足请求
                request.CachePolicy = new RequestCachePolicy(RequestCacheLevel.NoCacheNoStore);
                request.ServicePoint.Expect100Continue = false;
                request.ServicePoint.ConnectionLimit = 500;
                if (timeout.HasValue)
                {
                    request.Timeout = timeout.Value;
                    request.ReadWriteTimeout = timeout.Value;
                }
                request.Proxy = null;
                request.Method = "POST";
                request.ContentType = "application/x-www-form-urlencoded;charset=utf8";

                byte[] byteArray = Encoding.UTF8.GetBytes(postdata);
                request.ContentLength = byteArray.Length;

                using (var requestStream = Task<Stream>.Factory.FromAsync(request.BeginGetRequestStream, request.EndGetRequestStream, request))
                {
                    requestStream.Result.Write(byteArray, 0, byteArray.Length);
                }

                //response = Task<WebResponse>.Factory.FromAsync(request.BeginGetResponse, request.EndGetResponse, request).Result;
                //var stream = response.GetResponseStream();
                //using (StreamReader sr = new StreamReader(stream, Encoding.UTF8))
                //{
                //    string data = sr.ReadToEnd();
                //    result = js.Deserialize<SyncResultModel<T>>(data);
                //}
                //return result;

                Task<WebResponse> t = Task.Factory.FromAsync<WebResponse>(request.BeginGetResponse, request.EndGetResponse, null);
                ThreadPool.RegisterWaitForSingleObject((t as IAsyncResult).AsyncWaitHandle, TimeoutCallback, request, DefaultTimeout, true);
                t.ContinueWith(task =>
                {
                    try
                    {
                        WebResponse response1 = task.Result;
                        Stream responseStream = response1.GetResponseStream();
                        using (StreamReader sr = new StreamReader(responseStream, Encoding.UTF8))
                        {
                            string data = sr.ReadToEnd();
                            result = js.Deserialize<SyncResultModel<T>>(data);
                        }
                    }
                    catch (Exception ex)
                    {
                    }
                    finally
                    {
                        allDone.Set();
                    }
                });
                allDone.WaitOne();
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (response != null)
                {
                    response.Close();
                    response = null;
                }
                if (request != null)
                {
                    request.Abort();
                    result = null;
                }
            }
        }
        #endregion

    }

    internal class StateObject<T>
    {
        public HttpWebRequest HttpWebRequest { set; get; }
        public Action<T> Action { set; get; }

    }
}