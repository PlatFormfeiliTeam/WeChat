using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WeChat.Entity
{
    /// <summary>
    /// 微信关务云关联账号
    /// </summary>
    public class WGUserEn
    {
        /// <summary>
        /// 微信id
        /// </summary>
        public string WCOpenID { get; set; }
        /// <summary>
        /// 微信账号
        /// </summary>
        public string WCNickName { get; set; }
        /// <summary>
        /// 关务云ID
        /// </summary>
        public int GwyUserID { get; set; }
        /// <summary>
        /// 关务云账号
        /// </summary>
        public string GwyUserCode { get; set; }
        /// <summary>
        /// 关务云账号名称
        /// </summary>
        public string GwyUserName { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime? CreateDate { get; set; }
        /// <summary>
        /// 是否生产企业
        /// </summary>
        public int IsCompany { get; set; }
        /// <summary>
        /// 是否委托单位
        /// </summary>
        public int IsCustomer { get; set; }
        /// <summary>
        /// 是否接单单位
        /// </summary>
        public int IsReceiver { get; set; }
        /// <summary>
        /// 客商代码
        /// </summary>
        public string CustomerCode { get; set; }

    }
}