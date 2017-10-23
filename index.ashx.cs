using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
        static bool flag = true;
        public void ProcessRequest(HttpContext context)
        {
            LogHelper.Write("----------------------分割线111--------------------------");
            
            //事件响应
            string postString = response(context);
            LogHelper.Write("postStr：" + postString);
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
    }
}