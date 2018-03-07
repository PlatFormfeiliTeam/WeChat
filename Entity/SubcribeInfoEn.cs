using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WeChat.Entity
{
    /// <summary>
    /// 业务订阅信息实体
    /// </summary>
    public class SubcribeInfoEn
    {
        /// <summary>
        /// 
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// 订单编号
        /// </summary>
        public string OrderCode { get; set; }
        /// <summary>
        /// 报关单号
        /// </summary>
        public string DeclarationCode { get; set; }
        /// <summary>
        /// 企业编号
        /// </summary>
        public string Cusno { get; set; }
        /// <summary>
        /// 订阅人ID
        /// </summary>
        public int UserId { get; set; }
        /// <summary>
        /// 订阅人名称
        /// </summary>
        public string UserName { get; set; }
        /// <summary>
        /// 订阅时间
        /// </summary>
        public DateTime SubsTime { get; set; }
        /// <summary>
        /// 订阅类型
        /// </summary>
        public string SubsType { get; set; }
        /// <summary>
        /// 订阅内容
        /// </summary>
        public string Status { get; set; }
        /// <summary>
        /// 触发状态（0已订阅，1已触发，2已推送）
        /// </summary>
        public int TriggerStatus { get; set; }
        /// <summary>
        /// 模板id
        /// </summary>
        public string TemplateId { get; set; }
        /// <summary>
        /// 触发时间
        /// </summary>
        public DateTime TriggerTime { get; set; }
        /// <summary>
        /// 推送时间
        /// </summary>
        public DateTime PushTime { get; set; }
        /// <summary>
        /// 微信号
        /// </summary>
        public string Openid { get; set; }
        /// <summary>
        /// 状态值
        /// </summary>
        public int StatusValue { get; set; }
        
    }
}