using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WeChat.Entity;
using WeChat.Common;
using System.Data;

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
                pwd = pwd.Trim2().ToSHA1();
                string sql = @"select su.id as GWYUSERID,su.name GWYUSERCODE,su.realname as GWYUSERNAME,csc.code as CUSTOMERCODE,csc.hscode,csc.iscompany,csc.iscustomer,csc.isreceiver 
                            from sys_user su left join cusdoc.sys_customer csc on su.customerid=csc.id where su.name='{0}' and su.password='{1}' and csc.code='{2}' and su.enabled=1";
                sql = string.Format(sql, name.Trim2(), pwd, customer.Trim2().ToUpper());
                WGUserEn user = db.QuerySignleEntity<WGUserEn>(sql);
                if (user != null)
                {
                    user.WCOpenID = openid;
                    user.WCNickName = nickname;
                }
                return user;
            }
        }

        public static bool UserExsit(string name, string openid)
        {
            using (DBSession db = new DBSession())
            {
                string sql = "select wcopenid from wechat_user where gwyusercode='" + name + "'";
                DataTable dt = db.QuerySignle(sql);
                if (dt != null && dt.Rows.Count > 0)
                {
                    if (dt.Rows[0]["wcopenid"].ToString2() != openid)
                    {
                        return true;
                    }
                }
                return false;
            }
        }
        /// <summary>
        /// 新增账号
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public static bool SaveUser(WGUserEn user)
        {
            using (DBSession db = new DBSession())
            {
                string sql = "delete from wechat_user where WCOpenID='" + user.WCOpenID + "'";
                db.ExecuteSignle(sql);
                sql = @"insert into wechat_user(id,GWYUSERCODE,GWYUSERNAME,WCOpenID,WCNickName,iscompany,iscustomer,isreceiver,customercode,createdate,gwyuserid,hscode) 
                    values(wechat_user_id.nextval,'{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}',sysdate,{8},'{9}')";
                sql = string.Format(sql, user.GwyUserCode, user.GwyUserName, user.WCOpenID, user.WCNickName, user.IsCompany, user.IsCustomer, user.IsReceiver,
                    user.CustomerCode, user.GwyUserID, user.HSCode);
                return db.ExecuteSignle(sql) == 0 ? false : true;
            }
        }

        /// <summary>
        /// 注销账号
        /// </summary>
        /// <param name="openid"></param>
        /// <returns></returns>
        public static bool DeleteUser(string openid)
        {
            using (DBSession db = new DBSession())
            {
                string sql = "delete from wechat_user where WCOpenID='" + openid + "'";
                return db.ExecuteSignle(sql) == 0 ? false : true;
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
                string sql = "select * from wechat_user where wcopenid='" + openid + "' and rownum=1";
                WGUserEn wuser= db.QuerySignleEntity<WGUserEn>(sql);
                return wuser;
            }
        }

    }
}