using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WeChat.Common;
using WeChat.Entity;
using WeChat.ModelBusi;

namespace WeChat.ModelWeChat
{
    /// <summary>
    /// 模板消息推送
    /// </summary>
    public static class TemplateModel
    {
        public static bool taskFlag = false;
        public static void ExcuteSubcirbePush()
        {
            if (taskFlag)
                return;
            taskFlag = true;//已经运行
            LogHelper.Write("进入订阅执行...");
            while (taskFlag)
            {
                List<SubcribeInfoEn> sublist = SubscribeModel.getSubscribeTask();
                foreach (SubcribeInfoEn sub in sublist)
                {
                    var data = new
                    {
                        type = new TemplateDataItem(sub.SubsType,"#ff0000"),
                        cusno = new TemplateDataItem(sub.Cusno),
                        tiggertime = new TemplateDataItem(sub.TriggerTime.ToString()),
                        status = new TemplateDataItem(sub.Status)
                    };
                    //var obj = JsonHelper.SerializeObject(data);
                   
                    if (sub.SubsType == "物流状态")
                    {
                        sub.TemplateId = "2W7nYI371TSk18pLLubXelXz59wA3yMxoWq6o9uLYXY";
                    }
                    if (sub.SubsType == "报关状态")
                    {
                        sub.TemplateId = "PDpzPNCQdKFyyxTXCxZphl9Vor2mkgfUf-CLqPlLk8E";
                    }
                    if (sub.SubsType == "业务状态")
                    {
                        sub.TemplateId = "82bKjSd9Iyxdi0JPZMvUZ3zwmuleev6PfXimPfyb7aE";
                    }

                    SendMassMsgResultEn msg = SendTemplateMessage(TokenModel.AccessToken, sub.Openid, sub.TemplateId, data, "http://weixin.qq.com/download");
                    if (msg.errcode == "0")
                    {
                        SubscribeModel.updateSubscirbeInfo(sub.Id);
                    }
                }
                System.Threading.Thread.Sleep(5000);
            }
        }

        /// <summary>
        /// 模板消息仅用于公众号向用户发送重要的服务通知，只能用于符合其要求的服务场景中，如信用卡刷卡通知，商品购买成功通知等。
        /// 不支持广告等营销类消息以及其它所有可能对用户造成骚扰的消息。
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="accessToken">访问凭证</param>
        /// <param name="openId"></param>
        /// <param name="templateId">在公众平台线上模板库中选用模板获得ID</param>
        /// <param name="data"></param>
        /// <param name="url">，URL置空，则在发送后，点击模板消息会进入一个空白页面（ios），或无法点击（android）。</param>
        /// <param name="topcolor"></param>
        /// <returns></returns>
        public static SendMassMsgResultEn SendTemplateMessage(string accessToken, string openId, string templateId, object data, string url, string topcolor = "#173177")
        {
            var postUrl = string.Format("https://api.weixin.qq.com/cgi-bin/message/template/send?access_token={0}", accessToken);
            var msgData = new TemplateDataEn()
            {
                touser = openId,
                template_id = templateId,
                topcolor = topcolor,
                url = url,
                data = data
            };
            string postData = JsonHelper.SerializeObject(msgData);
            LogHelper.Write("postData：" + postData.ToString());
            string str = WeChatHelper.SendHttpRequest(postUrl, postData);
            LogHelper.Write("订阅消息回执：" + str);
            SendMassMsgResultEn result = JsonHelper.DeserializeJsonToObject<SendMassMsgResultEn>(str);
            return result;
        }

        public class TemplateDataItem
        {
            public string value { get; set; }
            public string color { get; set; }
            public TemplateDataItem(string val, string col = "#00ff00")
            {
                value = val;
                color = col;
            }
        }
    }
}