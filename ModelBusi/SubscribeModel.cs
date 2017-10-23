using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WeChat.Entity;

namespace WeChat.ModelBusi
{
    public class SubscribeModel
    {
        /// <summary>
        /// 获取推送信息
        /// </summary>
        /// <returns></returns>
        public static List<SubcribeInfoEn> getSubscribeInfo()
        {
            using(DBSession db=new DBSession())
            {
                string sql = "select * from wechat_subscribe where TriggerStatus=1 and isinvalid=0";
                return db.QueryEntity<SubcribeInfoEn>(sql);
            }
        }

        public static bool updateSubscirbeInfo(int id)
        {
            using (DBSession db = new DBSession())
            {
                string sql = "update wechat_subscribe set TriggerStatus=2,pushtime=sysdate where id=" + id;
                return db.ExecuteSignle(sql) == 0 ? true : false;
            }

        }
    }
}