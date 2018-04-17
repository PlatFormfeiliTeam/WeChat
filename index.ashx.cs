
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using WeChat.Common;
using WeChat.ModelWeChat;

namespace WeChat
{
    /// <summary>
    /// index 的摘要说明
    /// </summary>
    public class index : IHttpHandler
    {
        public void ProcessRequest(HttpContext context)
        {
            LogHelper.Write("----------------------分割线111--------------------------");
            //接通测试
            //InterfaceTest();
            //事件响应
            string postString = response(context);
            HandleModel.Handle(postString);
            
        }
        /// <summary>
        /// 获取请求
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string response(HttpContext context)
        {
            string postString = string.Empty;
            if (context.Request.HttpMethod.ToUpper() == "POST")
            {
                using (Stream stream = context.Request.InputStream)
                {
                    Byte[] postBytes = new Byte[stream.Length];
                    stream.Read(postBytes, 0, (Int32)stream.Length);
                    postString = Encoding.UTF8.GetString(postBytes);
                }
            }
            return postString;
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }


        #region 开发者测试
        //成为开发者url测试，返回echoStr
        public void InterfaceTest()
        {
            string token = "kaiser2012";
            Write("--------------------------------------");
            if (string.IsNullOrEmpty(token))
            {
                return;
            }

            string echoString = HttpContext.Current.Request.QueryString["echoStr"];
            string signature = HttpContext.Current.Request.QueryString["signature"];
            string timestamp = HttpContext.Current.Request.QueryString["timestamp"];
            string nonce = HttpContext.Current.Request.QueryString["nonce"];
            Write(@"echoString:" + echoString + "\r\nsignature:" + signature + "\r\ntimestamp:" + timestamp + "\r\nnonce:" + nonce);

            string[] ArrTmp = { token, timestamp, nonce };
            Array.Sort(ArrTmp);//字典排序  
            string tmpStr = string.Join("", ArrTmp);
            tmpStr = ToSHA1(tmpStr);//对该字符串进行sha1加密  
            Write(@"tmpStr:" + tmpStr);
            if (tmpStr == signature)//开发者获得加密后的字符串可与signature对比，标识该请求来源于微信。开发者通过检验signature对请求进行校验，若确认此次GET请求来自微信服务器，请原样返回echostr参数内容，则接入生效，否则接入失败  
            {
                if (!string.IsNullOrEmpty(echoString))
                {
                    HttpContext.Current.Response.Write(echoString);
                    HttpContext.Current.Response.End();
                }
            }
        }
        public void Write(string str)
        {
            string path = System.Web.HttpContext.Current.Server.MapPath(".") + @"\log.txt";
            if(!File.Exists(path))
            {
                File.Create(path);
            }
            FileStream fs = new FileStream(path, FileMode.Append);
            StreamWriter sw = new StreamWriter(fs);
            //开始写入
            sw.WriteLine(str);
            //清空缓冲区
            sw.Flush();
            //关闭流
            sw.Close();
            fs.Close();
        }

        public string ToSHA1(string value)
        {
            var buffer = Encoding.UTF8.GetBytes(value);
            var data = SHA1.Create().ComputeHash(buffer);
            var sb = new StringBuilder();
            foreach (var t in data)
            {
                sb.Append(t.ToString("x2"));
            }
            return sb.ToString();
        }
        #endregion



    }
}