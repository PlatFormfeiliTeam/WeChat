using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WeChat.Entity;
using WeChat.Common;

namespace WeChat.ModelBusi
{
    public class UserModel
    {

        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="name"></param>
        /// <param name="pwd"></param>
        /// <param name="customer"></param>
        /// <returns></returns>
        public static WGUserEn Login(string name,string pwd, string customer,string openid,string nickname)
        {
            using(DBSession db=new DBSession())
            {
                pwd = pwd.ToSHA1();
                string sql = @"select su.id as GWYUSERID,su.name GWYUSERCODE,su.realname as GWYUSERNAME,csc.code as CUSTOMERCODE,csc.iscompany,csc.iscustomer,csc.isreceiver from sys_user su 
                            left join cusdoc.sys_customer csc on su.customerid=csc.id where su.name='{0}' and su.password='{1}' and csc.code='{2}' and su.enabled=1";
                sql = string.Format(sql, name, pwd, customer.ToUpper());
                WGUserEn user = db.QuerySignleEntity<WGUserEn>(sql);
                if(user!=null && !string.IsNullOrEmpty(user.GwyUserCode))
                {
                    user.WCOpenID = openid;
                    user.WCNickName = nickname;
                    sql = @"insert into wechat_user(id,GWYUSERCODE,GWYUSERNAME,WCOpenID,WCNickName,iscompany,iscustomer,isreceiver,customercode,createdate,gwyuserid) 
                        values(wechat_user_id.nextval,'{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}',sysdate,{8})";
                    sql = string.Format(sql, user.GwyUserCode, user.GwyUserName, user.WCOpenID, user.WCNickName, user.IsCompany, user.IsCustomer, user.IsReceiver,
                        user.CustomerCode, user.GwyUserID);
                    db.ExecuteSignle(sql);
                }
                return user;
            }
        }

        /// <summary>
        /// 根据opneid查询关联账号
        /// </summary>
        /// <param name="openid"></param>
        /// <returns></returns>
        public static WGUserEn getWeChatUser(string openid)
        {
            using(DBSession db=new DBSession())
            {
                string sql = "select * from wechat_user where wcopenid='" + openid + "'";
                WGUserEn wuser= db.QuerySignleEntity<WGUserEn>(sql);
                return wuser;
            }
        }

    }
}