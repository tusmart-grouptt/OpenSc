﻿using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text.RegularExpressions;

namespace YoutubeExtractor
{
    internal static class HttpHelper
    {
        public static string DownloadString(string url)
        {
#if PORTABLE
            HttpWebRequest request = WebRequest.CreateHttp(url);
            request.Method = "GET";

            System.Threading.Tasks.Task<WebResponse> task = System.Threading.Tasks.Task.Factory.FromAsync(
                request.BeginGetResponse,
                asyncResult => request.EndGetResponse(asyncResult),
                null);

            return task.ContinueWith(t => ReadStreamFromResponse(t.Result)).Result;
#else
            using (var client = new WebClient())
            {
                client.Encoding = System.Text.Encoding.UTF8;
                return client.DownloadString(url);
            }
#endif
        }

        public static string HtmlDecode(string value)
        {
#if PORTABLE
            return System.Net.WebUtility.HtmlDecode(value);
#else
            return System.Web.HttpUtility.HtmlDecode(value);
#endif
        }

        public static IDictionary<string, string> ParseQueryString(string s)
        {
            // remove anything other than query string from url
            if (s.Contains("?"))
            {
                s = s.Substring(s.IndexOf('?') + 1);
            }

            var dictionary = new Dictionary<string, string>();

            foreach (string vp in Regex.Split(s, "&"))
            {
                string[] strings = Regex.Split(vp, "=");
                dictionary.Add(strings[0], strings.Length == 2 ? UrlDecode(strings[1]) : string.Empty);
            }

            return dictionary;
        }

        public static string UrlDecode(string url)
        {
#if PORTABLE
            return System.Net.WebUtility.UrlDecode(url);
#else
            return System.Web.HttpUtility.UrlDecode(url);
#endif
        }

        private static string ReadStreamFromResponse(WebResponse response)
        {
            using (Stream responseStream = response.GetResponseStream())
            {
                using (var sr = new StreamReader(responseStream))
                {
                    return sr.ReadToEnd();
                }
            }
        }
    }
}