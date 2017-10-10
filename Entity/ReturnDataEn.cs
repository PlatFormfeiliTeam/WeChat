using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WeChat.Entity
{
    public class ReturnDataEn<T>
    {
        /// 接口返回数据状态true成功|false失败
        /// </summary>
        public bool ResponseState { get; set; }
        /// <summary>
        /// 接口返回正确数据
        /// </summary>
        public T ResponseData { get; set; }
        /// <summary>
        /// 接口返回错误时间
        /// </summary>
        public AppErrorEn ErrorData;
    }
}