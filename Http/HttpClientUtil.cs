using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Net;
using OL.Utils.Check;
using OL.Utils.Extensions;
using OL.Utils.Security;
using System.Net.Http.Headers;
using OL.Utils.Files;

namespace OL.Utils.Http
{
    public class HttpClientUtil
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="url"></param>
        /// <param name="postData"></param>
        /// <param name="headers"></param>
        /// <returns></returns>
        public static string HttpPost(string url, Dictionary<string, string> postData = null, Dictionary<string, string> headers = null)
        {
            CheckNull.ArgumentIsNullException(url,nameof(url));
            try
            {
                var handler = new HttpClientHandler() { AutomaticDecompression = System.Net.DecompressionMethods.None };
                using (var http = new HttpClient(handler))
                {

                    if (headers != null)
                    {
                        foreach (var header in headers)
                        {
                            http.DefaultRequestHeaders.Add(header.Key, header.Value);
                        }
                    }
                    using (HttpContent httpContent = new StringContent(BuildParam(postData), Encoding.UTF8))
                    {
                        httpContent.Headers.ContentType = new MediaTypeHeaderValue("application/x-www-form-urlencoded");
                        HttpResponseMessage response = http.PostAsync(url, httpContent).Result;
                        return response.Content.ReadAsStringAsync().Result;
                    }
                }
            }
            catch (Exception ex)
            {
                LogUtil.LogError("HttpPost:请检查网络或请求地址-" +  ex.Message );
                return "请检查网络或请求地址："+ex.Message;
            }
        }
        public static string HttpPostJson(string url, string postDataJson = "", Dictionary<string, string> headers = null)
        {
            CheckNull.ArgumentIsNullException(url, nameof(url));
            var handler = new HttpClientHandler() { AutomaticDecompression = System.Net.DecompressionMethods.None };
            using (var http = new HttpClient(handler))
            {
               
                if (headers != null)
                {
                    foreach (var header in headers)
                    {
                        http.DefaultRequestHeaders.Add(header.Key, header.Value);
                    }
                }
                using (HttpContent httpContent = new StringContent(postDataJson , Encoding.UTF8))
                {
                    httpContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                    HttpResponseMessage response = http.PostAsync(url, httpContent).Result;
                    return response.Content.ReadAsStringAsync().Result;
                }
            }
        }
        public static T HttpPost<T>(string url, Dictionary<string, string> postData = null, Dictionary<string, string> headers = null)
        {
             CheckNull.ArgumentIsNullException(url, nameof(url));
            var handler = new HttpClientHandler() { AutomaticDecompression = DecompressionMethods.None };
            using (var http = new HttpClient(handler))
            {
                if (headers != null)
                {
                    foreach (var header in headers)
                    {
                        http.DefaultRequestHeaders.Add(header.Key, header.Value);
                    }
                }
                using (HttpContent httpContent = new StringContent(BuildParam(postData), Encoding.UTF8))
                {
                    HttpResponseMessage response = http.PostAsync(url, httpContent).Result;
                    return response.Content.ReadAsStreamAsync().Result.ToObject<T>();
                }
            }
        }

        public static string HttpGet(string url, Dictionary<string, string> postData = null, Dictionary<string, string> headers = null)
        {
            CheckNull.ArgumentIsNullException(url, nameof(url));
            var handler = new HttpClientHandler() { AutomaticDecompression = DecompressionMethods.None };
            using (var http = new HttpClient(handler))
            {
                if (headers != null)
                {
                    foreach (var header in headers)
                    {
                        http.DefaultRequestHeaders.Add(header.Key, header.Value);
                    }
                }
                if (postData != null)
                {
                    url = url + "?" + BuildParam(postData);
                }
                HttpResponseMessage response = http.GetAsync(url).Result;
                return response.Content.ReadAsStringAsync().Result;
            }
        }

        public static T HttpGet<T>(string url, Dictionary<string, string> postData = null, Dictionary<string, string> headers = null)
        {
            CheckNull.ArgumentIsNullException(url, nameof(url));
            var handler = new HttpClientHandler() { AutomaticDecompression = DecompressionMethods.None };
            using (var http = new HttpClient(handler))
            {
                if (headers != null)
                {
                    foreach (var header in headers)
                    {
                        http.DefaultRequestHeaders.Add(header.Key, header.Value);
                    }
                }
                if (postData != null)
                {
                    url = url + "?" + BuildParam(postData);
                }
                HttpResponseMessage response = http.GetAsync(url).Result;
                return response.Content.ReadAsStreamAsync().Result.ToObject<T>();
            }
        }

        public static string BuildParam(List<KeyValuePair<string, string>> paramArray, Encoding encode = null)
        {
            string url = "";
            if (encode == null) encode = Encoding.UTF8;
            if (paramArray != null && paramArray.Count > 0)
            {
                var parms = "";
                foreach (var item in paramArray)
                {
                    parms += string.Format("{0}={1}&", EncoderUtil.UrlHttpUtilityEncoder(item.Key, encode), EncoderUtil.UrlHttpUtilityEncoder(item.Value, encode));
                }
                if (parms != "")
                {
                    parms = parms.TrimEnd('&');
                }
                url += parms;
            }
            return url;
        }

        public static string BuildParam(Dictionary<string, string> paramArray, Encoding encode = null)
        {
            string url = "";
            if (encode == null) encode = Encoding.UTF8;
            if (paramArray != null && paramArray.Count > 0)
            {
                var parms = "";
                foreach (var item in paramArray)
                {
                    parms += string.Format("{0}={1}&",  EncoderUtil.UrlHttpUtilityEncoder(item.Key, encode), EncoderUtil.UrlHttpUtilityEncoder(item.Value, encode));// ;
                }
                if (parms != "")
                {
                    parms = parms.TrimEnd('&');
                }
                url += parms;
            }
            return url;
        }
    }
}
