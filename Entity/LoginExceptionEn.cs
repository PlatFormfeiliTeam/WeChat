using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WeChat.Entity
{
    public class LoginExceptionEn
    {
        public int ID { get; set; }

        public string LoginOpenId { get; set; }

        public string LoginNickName { get; set; }

        public string UserCode { get; set; }

        public string UserName { get; set; }

        public string OldOpenId { get; set; }

        public string OldNickName { get; set; }

        public DateTime? CreateTime { get; set; }

        public int IsSend { get; set; }

        public DateTime? SendTime { get; set; }
    }
}