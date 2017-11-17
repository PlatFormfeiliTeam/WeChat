using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Web;
using WeChat.Common;

namespace WeChat.ModelWeChat
{
    public class SignatureModel
    {
        public static string getSignature(string url)
        {
            SignatureModel sm = new SignatureModel();

            string appid = Jsapi_ticketModel.AppID;
            string jsapi_ticket = Jsapi_ticketModel.Jsapi_ticket;
            string noncestr =  GetRandomString(16);//随机数16位
            string timestamp = TimeStampModel.GetTimeStamp(DateTime.Now, 10);//时间戳 10位 

            string str = "jsapi_ticket=" + jsapi_ticket + "&noncestr=" + noncestr + "&timestamp=" + timestamp + "&url=" + url;
            string signature = GetPwd(str);

            return "{\"appid\":\"" + appid + "\",\"signature\":\"" + signature + "\",\"noncestr\":\"" + noncestr + "\",\"timestamp\":\"" + timestamp + "\"}";
        }

        ///<summary>
        ///生成随机字符串 
        ///</summary>
        ///<param name="length">目标字符串的长度</param>
        ///<param name="useNum">是否包含数字，1=包含，默认为包含</param>
        ///<param name="useLow">是否包含小写字母，1=包含，默认为包含</param>
        ///<param name="useUpp">是否包含大写字母，1=包含，默认为包含</param>
        ///<param name="useSpe">是否包含特殊字符，1=包含，默认为不包含</param>
        ///<param name="custom">要包含的自定义字符，直接输入要包含的字符列表</param>
        ///<returns>指定长度的随机字符串</returns>
        public static string GetRandomString(int length, bool useNum = true, bool useLow = true, bool useUpp = true, bool useSpe = false, string custom = "")
        {
            byte[] b = new byte[4];
            new System.Security.Cryptography.RNGCryptoServiceProvider().GetBytes(b);
            Random r = new Random(BitConverter.ToInt32(b, 0));
            string s = null, str = custom;
            if (useNum == true) { str += "0123456789"; }
            if (useLow == true) { str += "abcdefghijklmnopqrstuvwxyz"; }
            if (useUpp == true) { str += "ABCDEFGHIJKLMNOPQRSTUVWXYZ"; }
            if (useSpe == true) { str += "!\"#$%&'()*+,-./:;<=>?@[\\]^_`{|}~"; }
            for (int i = 0; i < length; i++)
            {
                s += str.Substring(r.Next(0, str.Length - 1), 1);
            }
            return s;
        }

        #region SHA1加密
        public static string GetPwd(string str)
        {
            byte[] cleanBytes = System.Text.Encoding.Default.GetBytes(str);
            byte[] hashedBytes = System.Security.Cryptography.SHA1.Create().ComputeHash(cleanBytes);
            return BitConverter.ToString(hashedBytes).Replace("-", "");
        }
        #endregion

    }
}