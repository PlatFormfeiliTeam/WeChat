using System;
using System.Collections.Generic ;
using System.Linq;
using System.Web ;

namespace WeChat.Entity
{
    public class UserInfoEn
    {
        /// <summary>
        /// 是否订阅该公众号0没有关注|1已关注
        /// </summary>
        public string subscribe { get; set; }
        /// <summary>
        /// 用户的标识，对当前公众号唯一
        /// </summary>
        public string openid { get; set; }
        /// <summary>
        /// 用户的昵称
        /// </summary>
        public string nickname { get; set; }
        /// <summary>
        /// 用户的性别0未知|1男性|2女性
        /// </summary>
        public string sex { get; set; }
        /// <summary>
        /// 用户所在城市
        /// </summary>
        public string city { get; set; }
        /// <summary>
        /// 用户所在国家
        /// </summary>
        public string country { get; set; }
        /// <summary>
        /// 用户所在省份
        /// </summary>
        public string province { get; set; }
        /// <summary>
        /// 用户的语言
        /// </summary>
        public string language { get; set; }
        /// <summary>
        /// 用户头像,最后一个数值代表正方形头像大小（有0、46、64、96、132数值可选，0代表640*640正方形头像）
        /// </summary>
        public string headimgurl { get; set; }
        /// <summary>
        /// 用户关注时间
        /// </summary>
        public string subscribe_time { get; set; }
        /// <summary>
        /// 绑定到微信开放平台唯一标识
        /// </summary>
        public string unionid { get; set; }
        /// <summary>
        /// 粉丝备注
        /// </summary>
        public string remark { get; set; }
        /// <summary>
        /// 用户所在的分组ID（兼容旧的用户分组接口）
        /// </summary>
        public string groupid { get; set; }
        /// <summary>
        /// 用户被打上的标签ID列表
        /// </summary>
        public List<string> tagid_list { get; set; }
    }
}