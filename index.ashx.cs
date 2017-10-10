using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using WeChat.Common;
using WeChat.WXModel;

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
            LogHelper.Write("----------------------分割线--------------------------");
            if (TokenModel.token == null)
            {
                LogHelper.Write("获取新的token");
                TokenModel.getAccesss_Token();
            }
            //Response.Write("<script>location.href='Test.html'</script>");
            if(flag)
            {
                createMenu();
                flag = false;
            }
            string postString = string.Empty;
            if (HttpContext.Current.Request.HttpMethod.ToUpper() == "POST")
            {
                
                using (Stream stream = HttpContext.Current.Request.InputStream)
                {
                    Byte[] postBytes = new Byte[stream.Length];
                    stream.Read(postBytes, 0, (Int32)stream.Length);
                    postString = Encoding.UTF8.GetString(postBytes);
                    HandleModel.Handle(postString);
                    LogHelper.Write(postString);
                }
            }
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }

        /// <summary>
        /// 创建菜单
        /// </summary>
        private void createMenu()
        {
            string data = getMenu();
            string url = string.Format("https://api.weixin.qq.com/cgi-bin/menu/create?access_token={0}", TokenModel.token.access_token);
            WeChatHelper.loadUrl(url, data);
           
        }
        /// <summary>
        /// 获取菜单数据
        /// </summary>
        /// <returns></returns>
        private string getMenu()
        {
            try
            {
                FileStream fs1 = new FileStream(System.Web.HttpContext.Current.Server.MapPath(".") + "\\file\\menu.txt", FileMode.Open);
                StreamReader sr = new StreamReader(fs1, Encoding.GetEncoding("GBK"));
                string menu = sr.ReadToEnd();
                sr.Close();
                fs1.Close();
                return menu;
            }
            catch(Exception e)
            {
                LogHelper.Write("getMenu异常：" + e.Message);
                return "";
            }
           
        }
    }
}