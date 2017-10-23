using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WeChat.Entity
{
    /// <summary>
    /// 业务咨询实体
    /// </summary>
    public class WebNoticeEn
    {
        /// <summary>
        /// 
        /// </summary>
        public int ID { get; set; }
        /// <summary>
        /// 标题
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// 内容
        /// </summary>
        public string Content { get; set; }
        /// <summary>
        /// 类别：newcategory.id
        /// </summary>
        public int Type { get; set; }
        /// <summary>
        /// 类别名称
        /// </summary>
        public string TypeName { get; set; }
        /// <summary>
        /// 排序
        /// </summary>
        public int SortIndex { get; set; }
        /// <summary>
        /// 修改人id
        /// </summary>
        public int UpdateID { get; set; }
        /// <summary>
        /// 修改人名称
        /// </summary>
        public string UpdateName { get; set; }
        /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime UpdateTime { get; set; }
        /// <summary>
        /// 发布日期
        /// </summary>
        public DateTime PublishDate { get; set; }
        /// <summary>
        /// 本文来源
        /// </summary>
        public string ReferenceSource { get; set; }
        /// <summary>
        /// 本文来源
        /// </summary>
        public string Attachment { get; set; }
    }
}