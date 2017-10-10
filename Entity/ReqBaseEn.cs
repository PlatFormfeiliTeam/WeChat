using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WeChat.Entity
{
    /// <summary>
    /// 基础消息
    /// </summary>
    public class ReqBaseEn
    {
        public string ToUserName { get; set; }
        public string FromUserName { get; set; }

        public string MsgType { get; set; }

        public DateTime CreateTime { get; set; }

        public string MsgId { get; set; }
    }
}