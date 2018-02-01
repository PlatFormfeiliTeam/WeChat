using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Xml;
using WeChat.Common;
using WeChat.Entity;
using WeChat.ModelBusi;

namespace WeChat.ModelWeChat
{
    /// <summary>
    /// 菜单事件处理
    /// </summary>
    public  class  HandleModel
    {
        public static RequestMsgEn req = null;
        /// <summary>
        /// 处理菜单事件
        /// </summary>
        /// <param name="postStr"></param>
        public static void Handle(string postStr)
        {
            
            try
            {
                req = JsonHelper.XmlToEntity_my<RequestMsgEn>(postStr);
            }
            catch(Exception e)
            {
                LogHelper.Write("Request转Entity错误：" + e.Message);
            }
            if (req == null)
                return;

            string responseContent = ReturnMessage();

            HttpContext.Current.Response.ContentEncoding = Encoding.UTF8;
            HttpContext.Current.Response.Write(responseContent);
        }

        public static string ReturnMessage()
        {
            string responseContent = "";
            
            if (req.MsgType!=null)
            {
                switch (req.MsgType)
                {
                    case "event":
                        responseContent=EventHandle();//事件处理
                        break;
                    case "text":
                        responseContent=TextHandle();//接受文本消息处理
                        break;
                    case "image":
                        break;
                    case "link":
                        break;
                    case "location":
                        break;
                    default:
                        break;
                }
            }
            return responseContent;
        }
        //事件
        public static string EventHandle()
        {
            string responseContent = "";
            if (req.Event!=null)
            {
                //菜单单击事件
                if (req.Event.Equals("CLICK", StringComparison.OrdinalIgnoreCase))
                {
                    if (req.EventKey.Equals("V1001_GOOD", StringComparison.OrdinalIgnoreCase))//click_one
                    {
                        //ReGetOpenId();
                        if(UserModel.DeleteUser(req.FromUserName))
                        {
                            responseContent = string.Format(ReplyType.Message_Text,
                                req.FromUserName,
                                req.ToUserName,
                                DateTime.Now.Ticks,
                                "注销成功");
                        }
                        else
                        {
                            responseContent = string.Format(ReplyType.Message_Text,
                                req.FromUserName,
                                req.ToUserName,
                                DateTime.Now.Ticks,
                                "注销失败");
                        }
                    }
                }
                else if (req.Event.Equals("subscribe", StringComparison.OrdinalIgnoreCase))
                {
                    WebNoticeModel wnm = new WebNoticeModel();
                    List<WebNoticeEn> list = wnm.getTwoNotice();
                    string news = "";
                    foreach(WebNoticeEn w in list)
                    {
                        news += string.Format(ReplyType.Message_News_Item, w.TypeName + "：" + w.Title, w.Title, "", "http://223.68.174.213:8012/Home/IndexNoticeDetail?fekx4+uqU5g=");
                    }
                    responseContent = string.Format(ReplyType.Message_News_Main,
                             req.FromUserName,
                             req.ToUserName,
                             DateTime.Now.Ticks,
                             "3",
                              string.Format(ReplyType.Message_News_Item, "欢迎关注<关务云>公众账号", "关于我们",
                              "http://1w838262n7.imwork.net/image/banner_feiliks.png",
                              "") + news);
                    LogHelper.Write("subscribe:" + responseContent);
                }
                else if (req.Event.Equals("unsubscribe", StringComparison.OrdinalIgnoreCase))
                {
                    UserModel.DeleteUser(req.FromUserName);
                }
            }
            return responseContent;
        }
        //接受文本消息
        public static string TextHandle()
        {
            string responseContent = "";
            if (!string.IsNullOrEmpty(req.Content))
            {
                responseContent = string.Format(ReplyType.Message_Text,
                    req.FromUserName,
                    req.ToUserName, 
                    DateTime.Now.Ticks,
                    "欢迎使用微信公共账号，您输入的内容为：" + req.Content + "\r\n<a href=\"http://www.baidu.com\">点击进入</a>");
            }
            else
            {
                responseContent = string.Format(ReplyType.Message_Text,
                    req.FromUserName,
                    req.ToUserName,
                    DateTime.Now.Ticks,
                    "您什么都没输入，没法帮您啊，%>_<%。");
            }
            return responseContent;
        }

      

       

    }
}