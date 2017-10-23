using System;
using System.Collections.Generic ;
using System.Linq;
using System.Web ;

namespace WeChat.Entity
{
    /// <summary>
    /// 微信用户实体
    /// </summary>
    public class UserInfoEn
    {
        /// <summary>
        /// 全局凭证唯一Id
        /// </summary>
        public string OpenID { get; set; }

        /// <summary>
        /// 公众号Id
        /// </summary>
        public string PublicId { get; set; }
        /// <summary>
        /// 用户Id
        /// </summary>
        public string UserID { get; set; }
        /// <summary>
        /// 昵称
        /// </summary>
        public string NickName { get; set; }

        /// <summary>
        /// 性别 1是男 0是女
        /// </summary>
        public int Sex { get; set; }

        /// <summary>
        /// 是否关注 1是关注 
        /// </summary>
        public int Subscribe { get; set; }
        /// <summary>
        /// 国家
        /// </summary>
        public string Country { get; set; }

        /// <summary>
        /// 地区
        /// </summary>
        public string Province { get; set; }
        /// <summary>
        /// 城市
        /// </summary>
        public string City { get; set; }
        /// <summary>
        /// 关注时间
        /// </summary>
        public DateTime CreateDate { get; set; }
        /// <summary>
        /// 用户头像
        /// </summary>
        public string HeadimgUrl { get; set; }
        /// <summary>
        /// 第三方平台Id，可为空
        /// </summary>
        public string UnionID { get; set; }
        /// <summary>
        /// 用户取消关注时间
        /// </summary>
        public DateTime Un_Subscribe_Time { get; set; }
    }
}