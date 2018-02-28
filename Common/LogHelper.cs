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
        static string path = System.Web.HttpContext.Current.Server.MapPath(".") + @"\Log\";
        public static void Write(string str)
        {
            try
            {
                string fileName = DateTime.Now.ToShortDateString().Replace("/", "") + ".txt";
                if (!File.Exists(path + fileName))
                {
                    File.Create(path + fileName);
                }
                FileStream fs = new FileStream(path + fileName, FileMode.Append);
                StreamWriter sw = new StreamWriter(fs);
                //开始写入
                sw.WriteLine(str + "-----" + DateTime.Now.ToString() + "\r\n");
                //清空缓冲区
                sw.Flush();
                //关闭流
                sw.Close();
                fs.Close();
            }
            catch(Exception e)
            {

            }
            
        }
    }
}