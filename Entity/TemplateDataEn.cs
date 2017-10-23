using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WeChat.Entity
{
    /// <summary>
    /// 微信模板消息实体
    /// </summary>
    public class TemplateDataEn
    {
        public string touser { get; set; }
        public string template_id { get; set; }
        public string topcolor { get; set; }
        public string url { get; set; }
        public object data { get; set; }
    }
}