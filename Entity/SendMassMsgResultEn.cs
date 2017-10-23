using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WeChat.Entity
{
    /// <summary>
    /// 微信模板推送回执
    /// </summary>
    public class SendMassMsgResultEn
    {
        ///// <summary>
        ///// 公众号微信号
        ///// </summary>
        //public string ToUserName { get; set; }
        ///// <summary>
        ///// 接收模板消息的用户的openid
        ///// </summary>
        //public string FromUserName { get; set; }
        ///// <summary>
        ///// 消息类型是事件
        ///// </summary>
        //public string MsgType { get; set; }
        ///// <summary>
        ///// 消息创建时间
        ///// </summary>
        //public DateTime CreateTime { get; set; }
        ///// <summary>
        ///// 消息id
        ///// </summary>
        //public string MsgId { get; set; }
        ///// <summary>
        ///// 事件为模板消息发送结束
        ///// </summary>
        //public string Event { get; set; }
        ///// <summary>
        ///// 发送状态为
        ///// </summary>
        //public string Status { get; set; }

        /// <summary>
        /// 错误代码
        /// </summary>
        public string errcode { get; set; }
        /// <summary>
        /// 错误信息
        /// </summary>
        public string errmsg { get; set; }
        /// <summary>
        /// 信息ID
        /// </summary>
        public string msgid { get; set; }
    }
}