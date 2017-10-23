using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WeChat.Entity
{
    /// <summary>
    /// 微信菜单请求实体
    /// </summary>
    public class RequestMsgEn
    {
        /// <summary>
        /// 开发者微信号
        /// </summary>
        public string ToUserName { get; set; }
        /// <summary>
        /// 发送方帐号
        /// </summary>
        public string FromUserName { get; set; }
        /// <summary>
        /// 消息类型（text、	image、	location、link、event）
        /// </summary>
        public string MsgType { get; set; }
        /// <summary>
        /// 消息创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }
        /// <summary>
        /// 消息id
        /// </summary>
        public string MsgId { get; set; }
        /// <summary>
        /// text-文本消息内容
        /// </summary>
        public string Content { get; set; }
        /// <summary>
        /// image-图片链接
        /// </summary>
        public string PicUrl { get; set; }
        /// <summary>
        /// event-事件类型，（subscribe(订阅)、unsubscribe(取消订阅)、CLICK(自定义菜单点击事件)）
        /// </summary>
        public string Event { get; set; }
        /// <summary>
        /// event-事件KEY值，与自定义菜单接口中KEY值对应
        /// </summary>
        public string EventKey { get; set; }
        /// <summary>
        /// location-地理位置纬度
        /// </summary>
        public string Location_X { get; set; }
        /// <summary>
        /// location-地理位置经度
        /// </summary>
        public string Location_Y { get; set; }
        /// <summary>
        /// location-地图缩放大小
        /// </summary>
        public string Scale { get; set; }
        /// <summary>
        /// location-地理位置信息
        /// </summary>
        public string Label { get; set; }
        /// <summary>
        /// link-消息标题
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// link-消息描述
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// link-消息链接
        /// </summary>
        public string Url { get; set; }


    }
}