using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WeChat.Entity
{
    /// <summary>
    /// 事件消息
    /// </summary>
    public class ReqEventEn : ReqBaseEn
    {
        public string Event { get; set; }
        public string EventKey { get; set; }
    }
}