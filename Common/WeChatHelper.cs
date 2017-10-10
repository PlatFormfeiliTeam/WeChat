using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;

namespace WeChat.Common
{
    public class WeChatHelper
    {
        public static string loadUrl(string url,string data)
        {
            try
            {
                HttpWebRequest hwr = WebRequest.Create(url) as HttpWebRequest;
                hwr.Method = "POST";
                hwr.ContentType = "application/x-www-form-urlencoded";
                byte[] payload;
                payload = System.Text.Encoding.UTF8.GetBytes(data);
                hwr.ContentLength = payload.Length;
                Stream writer = hwr.GetRequestStream();
                writer.Write(payload, 0, payload.Length);
                writer.Close();
                var result = hwr.GetResponse() as HttpWebResponse;
                LogHelper.Write("result:" + result.StatusCode.ToString());
                return result.StatusCode.ToString();
            }
            catch (Exception e)
            {
                LogHelper.Write("loadUrl异常：" + e.Message);
                return e.Message;
            }
        }

    }
}