using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WeChat.Entity
{
    public class OpenidListEn
    {
        /// <summary>
        /// 关注该公众账号的总用户数
        /// </summary>
        public string total { get; set; }
        /// <summary>
        /// 拉取的OPENID个数，最大值为10000
        /// </summary>
        public string count { get; set; }
        /// <summary>
        /// 列表数据，OPENID的列表
        /// </summary>
        public List<string> openids { get; set; }
        /// <summary>
        /// 拉取列表的最后一个用户的OPENID
        /// </summary>
        public string next_openid { get; set; }
    }
}