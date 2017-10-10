using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Net;
namespace WeChat.Common
{
    public class LogHelper
    {
        public static void Write(string str)
        {
            FileStream fs = new FileStream(System.Web.HttpContext.Current.Server.MapPath(".") + "\\Log\\log.txt", FileMode.Append);
            StreamWriter sw = new StreamWriter(fs);
            //开始写入
            sw.WriteLine(str + "-----" + DateTime.Now.ToString() + "\r\n");
            //清空缓冲区
            sw.Flush();
            //关闭流
            sw.Close();
            fs.Close();
        }
    }
}