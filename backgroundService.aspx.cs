using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WeChat.Common;
using WeChat.Entity;
using WeChat.ModelWeChat;

namespace WeChat
{
    public partial class backgroundService : System.Web.UI.Page
    {
        private static bool flag = false;
        private static System.Threading.Thread th = null;
        protected void Page_Load(object sender, EventArgs e)
        {
        }
        protected void Button1_Click(object sender, EventArgs e)
        {
            //创建菜单
            createMenu();
            System.Threading.Thread.Sleep(3000);

        }
        /// <summary>
        ///运行订阅
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Button2_Click(object sender, EventArgs e)
        {
            if(!flag)
            {
                th = new System.Threading.Thread(TemplateModel.ExcuteSubcirbePush);
                th.Start();
                flag = true;
                this.Label2.Text = "服务运行中";
            }
            else
            {
                if (th != null)
                {
                    TemplateModel.taskFlag = false;
                    th.Abort();
                    th = null;
                    flag = false;
                    this.Label2.Text = "服务停止";

                }
                
            }
        }
        protected void Button3_Click(object sender, EventArgs e)
        {
            //创建菜单
            this.TextBox2.Text = MyHttpHelper.UrlEncode(this.TextBox1.Text);

        }
        /// <summary>
        /// 创建菜单
        /// </summary>
        private void createMenu()
        {
            string data = getMenu();
            LogHelper.Write("menu数据:" + data);
            string url = string.Format("https://api.weixin.qq.com/cgi-bin/menu/create?access_token={0}", TokenModel.AccessToken);
            AppErrorEn error = JsonHelper.DeserializeJsonToObject<AppErrorEn>(WeChatHelper.SendHttpRequest(url, data));
            if (error.errcode == "0")
                this.Label1.Text = "创建成功";
            else
                this.Label1.Text = "创建失败";
        }
        /// <summary>
        /// 获取菜单数据
        /// </summary>
        /// <returns></returns>
        private string getMenu()
        {
            try
            {
                FileStream fs1 = new FileStream(System.Web.HttpContext.Current.Server.MapPath(".") + "\\file\\menu.txt", FileMode.Open);
                StreamReader sr = new StreamReader(fs1, Encoding.GetEncoding("GBK"));
                string menu = sr.ReadToEnd();
                sr.Close();
                fs1.Close();
                return menu;
            }
            catch (Exception e)
            {
                LogHelper.Write("getMenu异常：" + e.Message);
                return "";
            }
        }
       
    }
}