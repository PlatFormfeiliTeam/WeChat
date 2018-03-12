using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using WeChat.Common;
using WeChat.Entity;

namespace WeChat.ModelBusi
{
    public class SubscribeModel
    {
        /// <summary>
        /// 获取推送任务
        /// </summary>
        /// <returns></returns>
        public static List<SubcribeInfoEn> getSubscribeTask()
        {
            using(DBSession db=new DBSession())
            {
                string sql = "select * from wechat_subscribe where TriggerStatus=1 and isinvalid=0";
                return db.QueryEntity<SubcribeInfoEn>(sql);
            }
        }
        /// <summary>
        /// 订单_获取最新的N条订阅条信息
        /// </summary>
        /// <returns></returns>
        public static DataTable getSubscribeInfo_Order(string starttime, string endtime, string flag, int pagesize, int lastnum, string cusno, int userId)
        {
            try
            {
                using (DBSession db = new DBSession())
                {
                    string strWhere = " (subws.codetype=1 or subws.codetype=2) and ";
                    if (!string.IsNullOrEmpty(cusno))
                    {
                        strWhere += " subws.cusno='" + cusno + "' and ";
                    }
                    if (!string.IsNullOrEmpty(starttime))
                    {
                        strWhere += " subws.SUBSTIME>to_date('" + starttime + "','yyyy-mm-dd hh24:mi:ss') and ";
                    }
                    if (!string.IsNullOrEmpty(endtime))
                    {
                        strWhere += " subws.SUBSTIME<to_date('" + endtime + " 23:59:59','yyyy-mm-dd hh24:mi:ss') and ";
                    }
                    if (flag == "已触发")
                    {
                        strWhere += " (subws.TRIGGERSTATUS=1 or subws.TRIGGERSTATUS=2) and";
                    }
                    else if (flag == "未触发")
                    {
                        strWhere += " subws.TRIGGERSTATUS=0 and";
                    }
                    if (userId > 0)
                    {
                        strWhere += " subws.userid=" + userId + " and";
                    }
                    strWhere += " subws.isinvalid=0";
                    string sql = @"select lo.busiunitname,lo.busitype,lo.divideno,lo.repwayid,lo.contractno,lo.goodsnum,lo.goodsgw,to_char(lo.declstatus) as declstatus,to_char(lo.inspstatus) as inspstatus,lo.logisticsname, 
                            ws.cusno,ws.triggerstatus, ws.substype,ws.status as substatus ,'' as sublogstatus,ws.statusvalue,sb.name as businame,sr.name as repwayname from wechat_subscribe ws 
                            left join list_order lo on ws.cusno=lo.cusno 
                            left join cusdoc.sys_busitype sb on lo.busitype=sb.code 
                            left join cusdoc.sys_repway sr on lo.repwayid=sr.code where ws.cusno in (
                            select cusno from ( select rownum as rown ,tab.* from 
                            (select * from 
                                (select cusno,substime, ROW_NUMBER() OVER(partition by cusno order by substime desc) as rnum from  wechat_subscribe subws 
                                where {0}  and subws.cusno is not null)
                            newws where newws.rnum=1 order by newws.substime desc ) tab where rownum<={1}) t1 where t1.rown>{2})
                            and (ws.codetype=1 or ws.codetype=2) and ws.isinvalid=0 order by ws.cusno,ws.substype,ws.statusvalue";
                    sql = string.Format(sql, strWhere, lastnum + pagesize, lastnum);
                    return db.QuerySignle(sql);
                }
            }
            catch(Exception ex)
            {
                LogHelper.Write("SubscribeModel_getSubscribeInfo_Order:" + ex.Message);
                return null;
            }
            
        }

        /// <summary>
        /// 订单_获取本人未推送的所有订阅信息
        /// </summary>
        /// <returns></returns>
        public static DataTable getSubscribeInfo_Order(int userId)
        {
            try
            {
                using (DBSession db = new DBSession())
                {
                    string strWhere = " (subws.codetype=1 or subws.codetype=2) and (subws.TRIGGERSTATUS=0 or subws.TRIGGERSTATUS=1) and ";
                    
                    if (userId > 0)
                    {
                        strWhere += " subws.userid=" + userId + " and";
                    }
                    strWhere += " subws.isinvalid=0";
                    string sql = @"select lo.busiunitname,lo.busitype,lo.divideno,lo.repwayid,lo.contractno,lo.goodsnum,lo.goodsgw,to_char(lo.declstatus) as declstatus,to_char(lo.inspstatus) as inspstatus,lo.logisticsname, 
                            ws.cusno,ws.triggerstatus, ws.substype,ws.status as substatus ,'' as sublogstatus,ws.statusvalue,sb.name as businame,sr.name as repwayname from wechat_subscribe ws 
                            left join list_order lo on ws.cusno=lo.cusno 
                            left join cusdoc.sys_busitype sb on lo.busitype=sb.code 
                            left join cusdoc.sys_repway sr on lo.repwayid=sr.code where ws.cusno in (
                            select cusno from ( select rownum as rown ,tab.* from 
                            (select * from 
                                (select cusno,substime, ROW_NUMBER() OVER(partition by cusno order by substime desc) as rnum from  wechat_subscribe subws 
                                where {0}  and subws.cusno is not null)
                            newws where newws.rnum=1 order by newws.substime desc ) tab where rownum<={1}) t1 where t1.rown>{2})
                            and (ws.codetype=1 or ws.codetype=2) and ws.isinvalid=0 order by ws.cusno,ws.substype,ws.statusvalue";
                    sql = string.Format(sql, strWhere, 100, 0);
                    return db.QuerySignle(sql);
                }
            }
            catch (Exception ex)
            {
                LogHelper.Write("SubscribeModel_getSubscribeInfo_Order:" + ex.Message);
                return null;
            }

        }

        /// <summary>
        /// 订单_获取本人未推送的所有订阅信息
        /// </summary>
        /// <returns></returns>
        public static DataTable getNewSubscribeInfo_Order(string subscribestart, string subscribeend, string busiunit, string istigger, string busitype, string ordercode, string cusno,
            string divideno, string contract, string submitstart, string submitend, int pagesize, int lastnum, int userId, out string sum )
        {
            sum = "";
            try
            {
                using (DBSession db = new DBSession())
                {
                    string strWhere = " (ws.codetype=1 or ws.codetype=2) and ";
                    if (!string.IsNullOrEmpty(subscribestart)) strWhere += "ws.substime > to_date('" + subscribestart + "','yyyy-mm-dd hh24:mi:ss') and ";
                    if (!string.IsNullOrEmpty(subscribeend)) strWhere += "ws.substime < to_date('" + subscribeend + " 23:59:59','yyyy-mm-dd hh24:mi:ss') and ";

                    if (!string.IsNullOrEmpty(busiunit)) strWhere += " (lo.busiunitcode like '%" + busiunit + "%' or lo.busiunitname like '%" + busiunit + "%') and ";
                    
                    int TRIGGERSTATUS = 0;
                    if (istigger == "已触发") TRIGGERSTATUS = 1;
                    else if (istigger == "已推送") TRIGGERSTATUS = 2;
                    strWhere += " ws.TRIGGERSTATUS=" + TRIGGERSTATUS + " and ";

                    if (!string.IsNullOrEmpty(busitype)) strWhere += " lo.busitype in (" + busiunit + ") and ";
                    if (!string.IsNullOrEmpty(ordercode)) strWhere += " lo.code like '%" + ordercode + "%' and ";
                    if (!string.IsNullOrEmpty(cusno)) strWhere += " lo.cusno like '%" + cusno + "%' and ";
                    if (!string.IsNullOrEmpty(divideno)) strWhere += " lo.divideno like '%" + divideno + "%'  and ";
                    if (!string.IsNullOrEmpty(contract)) strWhere += " lo.contractno like '%" + contract + "%' and ";

                    if (!string.IsNullOrEmpty(submitstart)) strWhere += "lo.submittime > to_date('" + submitstart + "','yyyy-mm-dd hh24:mi:ss') and ";
                    if (!string.IsNullOrEmpty(submitend)) strWhere += "lo.submittime < to_date('" + submitend + " 23:59:59','yyyy-mm-dd hh24:mi:ss') and ";
                    if (userId > 0)
                    {
                        strWhere += " ws.userid=" + userId + " and";
                    }
                    strWhere += " ws.isinvalid=0 and lo.isinvalid=0";
                    string sql = @"with newt
                                    as(
                                    select * from 
                                        (select tab.*,rownum as rown from 
                                            (select lo.busiunitname,lo.busitype,lo.divideno,lo.repwayid,lo.contractno,lo.goodsnum,lo.goodsgw,to_char(lo.declstatus) as declstatus,
                                            to_char(lo.inspstatus) as inspstatus,lo.logisticsname,  ws.cusno,ws.triggerstatus, ws.substype,ws.status,ws.pushtime,ws.id,
                                            ws.statusvalue,ws.substime,'' as sum from wechat_subscribe ws left join list_order lo on ws.cusno=lo.cusno where {0} order by substime desc
                                        ) tab ) where rown>{1} and rown<={2})
                                        select newt.*,sb.name as businame,sr.name as repwayname from newt 
                                            left join cusdoc.sys_busitype sb on newt.busitype=sb.code 
                                            left join cusdoc.sys_repway sr on newt.repwayid=sr.code  order by newt.substime desc";
                    sql = string.Format(sql, strWhere, lastnum, lastnum + pagesize);
                    string sumSql = @"select count(1) from wechat_subscribe ws left join list_order lo on ws.cusno=lo.cusno where {0} ";
                    DataTable dt = db.QuerySignle(string.Format(sumSql, strWhere));
                    if (dt != null && dt.Rows.Count > 0) sum = dt.Rows[0][0].ToString2();
                    return db.QuerySignle(sql);
                }
            }
            catch (Exception ex)
            {
                LogHelper.Write("SubscribeModel_getSubscribeInfo_Order:" + ex.Message);
                return null;
            }

        }
        /// <summary>
        /// 预制单_获取最新的N条订阅条信息
        /// </summary>
        /// <returns></returns>
        public static DataTable getSubscribeInfo_Decl(string starttime, string endtime, string flag, int pagesize, int lastnum, string declarationcode,int userId)
        {
            try
            {
                using (DBSession db = new DBSession())
                {
                    string strWhere = " subws.codetype=3 and ";
                    if (!string.IsNullOrEmpty(declarationcode))
                    {
                        strWhere += " subws.declarationcode='" + declarationcode + "' and ";
                    }
                    if (!string.IsNullOrEmpty(starttime))
                    {
                        strWhere += " subws.SUBSTIME>to_date('" + starttime + "','yyyy-mm-dd hh24:mi:ss') and ";
                    }
                    if (!string.IsNullOrEmpty(endtime))
                    {
                        strWhere += " subws.SUBSTIME<to_date('" + endtime + " 23:59:59','yyyy-mm-dd hh24:mi:ss') and ";
                    }
                    if (flag == "已触发")
                    {
                        strWhere += " (subws.TRIGGERSTATUS=1 or subws.TRIGGERSTATUS=2) and";
                    }
                    else if (flag == "未触发")
                    {
                        strWhere += " subws.TRIGGERSTATUS=0 and";
                    }
                    if (userId > 0)
                    {
                        strWhere += " subws.userid=" + userId + " and ";
                    }
                    strWhere += " subws.isinvalid=0";
                    string sql = @"select ld.declarationcode,ld.goodsnum,ld.goodsgw,ld.tradecode,ld.modifyflag,ld.customsstatus,ld.transname,ws.declarationcode,
                            ws.cusno,ws.triggerstatus, ws.substype,ws.status as substatus ,ws.statusvalue,cbd.name as tradename from wechat_subscribe ws 
                            left join list_declaration ld on ws.declarationcode=ld.declarationcode 
                            left join cusdoc.base_decltradeway cbd on ld.tradecode=cbd.code where ws.declarationcode in (
                            select declarationcode from ( select rownum as rown ,tab.* from 
                            (select * from 
                                (select declarationcode,substime, ROW_NUMBER() OVER(partition by declarationcode order by substime desc) as rnum from  wechat_subscribe subws 
                            where {0} and declarationcode is not null)
                            newws where newws.rnum=1 order by newws.substime desc ) tab where rownum<={1}) t1 where t1.rown>{2}
                            ) and ws.codetype=3 and ws.isinvalid=0 and ws.declarationcode is not null order by ws.declarationcode,ws.substype,ws.statusvalue";
                    return db.QuerySignle(string.Format(sql, strWhere, lastnum + pagesize, lastnum));
                }
            }
            catch (Exception ex)
            {
                LogHelper.Write("SubscribeModel_getSubscribeInfo_Decl:" + ex.Message);
                return null;
            }

        }
        /// <summary>
        /// 预制单_获取本人未推送的所有订阅信息
        /// </summary>
        /// <returns></returns>
        public static DataTable getSubscribeInfo_Decl(int userId)
        {
            try
            {
                using (DBSession db = new DBSession())
                {
                    string strWhere = " subws.codetype=3 and (subws.TRIGGERSTATUS=0 or subws.TRIGGERSTATUS=1) and  ";
                    
                    if (userId > 0)
                    {
                        strWhere += " subws.userid=" + userId + " and ";
                    }
                    strWhere += " subws.isinvalid=0";
                    string sql = @"select ld.declarationcode,ld.goodsnum,ld.goodsgw,ld.tradecode,ld.modifyflag,ld.customsstatus,ld.transname,ws.declarationcode,
                            ws.cusno,ws.triggerstatus, ws.substype,ws.status as substatus ,ws.statusvalue,cbd.name as tradename from wechat_subscribe ws 
                            left join list_declaration ld on ws.declarationcode=ld.declarationcode 
                            left join cusdoc.base_decltradeway cbd on ld.tradecode=cbd.code where ws.declarationcode in (
                            select declarationcode from ( select rownum as rown ,tab.* from 
                            (select * from 
                                (select declarationcode,substime, ROW_NUMBER() OVER(partition by declarationcode order by substime desc) as rnum from  wechat_subscribe subws 
                            where {0} and declarationcode is not null)
                            newws where newws.rnum=1 order by newws.substime desc ) tab where rownum<={1}) t1 where t1.rown>{2}
                            ) and ws.codetype=3 and ws.isinvalid=0 and ws.declarationcode is not null order by ws.declarationcode,ws.substype,ws.statusvalue";
                    return db.QuerySignle(string.Format(sql, strWhere, 100, 0));
                }
            }
            catch (Exception ex)
            {
                LogHelper.Write("SubscribeModel_getSubscribeInfo_Decl:" + ex.Message);
                return null;
            }

        }

        /// <summary>
        /// 预制单_获取最新的N条订阅条信息
        /// </summary>
        /// <returns></returns>
        public static DataTable getNewSubscribeInfo_Decl(string subscribestart, string subscribeend, string declarationcode, string istrigger, string busitype, string busiunit,
            string ordercode, string cusno, string contract, string submitstart, string submitend, int pagesize, int lastnum, int userId, out string sum)
        {
            sum = "0";
            try
            {
                using (DBSession db = new DBSession())
                {
                    string strWhere = " ws.codetype=3 and ";
                    if (!string.IsNullOrEmpty(subscribestart)) strWhere += "ws.substime > to_date('" + subscribestart + "','yyyy-mm-dd hh24:mi:ss') and ";
                    if (!string.IsNullOrEmpty(subscribeend)) strWhere += "ws.substime < to_date('" + subscribeend + " 23:59:59','yyyy-mm-dd hh24:mi:ss') and ";
                    if (!string.IsNullOrEmpty(declarationcode)) strWhere += " ld.declarationcode like '%" + declarationcode + "%' and ";

                    int TRIGGERSTATUS = 0;
                    if (istrigger == "已触发") TRIGGERSTATUS = 1;
                    else if (istrigger == "已推送") TRIGGERSTATUS = 2;
                    strWhere += " ws.TRIGGERSTATUS=" + TRIGGERSTATUS + " and ";

                    if (!string.IsNullOrEmpty(busitype)) strWhere += " lo.busitype in (" + busiunit + ") and ";
                    if (!string.IsNullOrEmpty(busiunit)) strWhere += " (lo.busiunitcode like '%" + busiunit + "%' or lo.busiunitname like '%" + busiunit + "%') and ";
                    if (!string.IsNullOrEmpty(ordercode)) strWhere += " lo.code like '%" + ordercode + "%' and ";
                    if (!string.IsNullOrEmpty(cusno)) strWhere += " lo.cusno like '%" + cusno + "%' and ";
                    if (!string.IsNullOrEmpty(contract)) strWhere += " lo.contractno like '%" + contract + "%' and ";
                    if (!string.IsNullOrEmpty(submitstart)) strWhere += "lo.submittime > to_date('" + submitstart + "','yyyy-mm-dd hh24:mi:ss') and ";
                    if (!string.IsNullOrEmpty(submitend)) strWhere += "lo.submittime < to_date('" + submitend + " 23:59:59','yyyy-mm-dd hh24:mi:ss') and ";
                    if (userId > 0)
                    {
                        strWhere += " ws.userid=" + userId + " and ";
                    }
                    strWhere += " ws.isinvalid=0 and lo.isinvalid=0 and ld.isinvalid=0";
                    string sql = @"with newt
                                      as(
                                      select * from 
                                          (select tab.*,rownum as rown from 
                                              (select ld.goodsnum,ld.goodsgw,ld.tradecode,ld.modifyflag,ld.customsstatus,ld.transname,
                                              ws.declarationcode, ws.cusno,ws.triggerstatus, ws.substype,ws.status ,ws.statusvalue,ws.pushtime,ws.id,
                                              ws.substime,'' as sum from wechat_subscribe ws 
                                              left join list_order lo on ws.cusno=lo.cusno
                                              left join list_declaration ld on ld.declarationcode=ws.declarationcode 
                                              where {0} order by ws.substime desc
                                          )tab ) where rown>{1} and rown<={2})
                                        select newt.*,cbd.name as tradename from newt 
                                            left join cusdoc.base_decltradeway cbd on newt.tradecode=cbd.code 
                                            order by newt.substime desc";
                    sql = string.Format(sql, strWhere, lastnum, lastnum + pagesize);
                    string sumSql = @"select count(1) from wechat_subscribe ws 
                                              left join list_order lo on ws.cusno=lo.cusno
                                              left join list_declaration ld on ld.declarationcode=ws.declarationcode 
                                              where {0}";
                    DataTable dt = db.QuerySignle(string.Format(sumSql, strWhere));
                    if (dt != null && dt.Rows.Count > 0) sum = dt.Rows[0][0].ToString2();
                    return db.QuerySignle(sql);
                }
            }
            catch (Exception ex)
            {
                LogHelper.Write("SubscribeModel_getSubscribeInfo_Decl:" + ex.Message);
                return null;
            }

        }
        /// <summary>
        /// 查询订阅详情
        /// </summary>
        /// <param name="starttime"></param>
        /// <param name="endtime"></param>
        /// <param name="flag"></param>
        /// <returns></returns>
        public static DataTable getSubscribeInfo(string ordercodes)
        {
            
            using (DBSession db = new DBSession())
            {
                string sql = @"select lo.busiunitname,lo.busitype,lo.cusno,lo.divideno,lo.repwayid,lo.contractno,lo.goodsnum,lo.goodsgw,lo.declstatus,lo.inspstatus,
                            lo.logisticsname, ws.substype,ws.status,ws.statusvalue,sb.name as businame,sr.name as repwayname from wechat_subscribe ws 
                            left join list_order lo on ws.cusno=lo.code 
                            left join cusdoc.sys_busitype sb on lo.busitype=sb.code 
                            left join cusdoc.sys_repway sr on lo.repwayid=sr.code where ws.cusno in ({0}) and ws.isinvalid=0 order by ws.cusno,ws.substype,ws.statusvalue";
                return db.QuerySignle(string.Format(sql, ordercodes));
            }
        }
        /// <summary>
        /// 信息已推送
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static bool updateSubscirbeInfo(int id)
        {
            using (DBSession db = new DBSession())
            {
                string sql = "update wechat_subscribe set TriggerStatus=2,pushtime=sysdate where id=" + id;
                return db.ExecuteSignle(sql) == 0 ? true : false;
            }

        }
        /// <summary>
        /// 新增订阅信息
        /// </summary>
        /// <param name="type"></param>
        /// <param name="status"></param>
        /// <param name="cusno"></param>
        /// <param name="declarationcode"></param>
        /// <param name="cusno"></param>
        /// <param name="userid"></param>
        /// <param name="username"></param>
        /// <param name="openid"></param>
        /// <param name="codetype"></param>
        /// <returns></returns>
        public static bool insertSubscribe(string type, string[] status, string cusno, string declarationcode, int userid, string username, string openid, string codetype)
        {
            try
            {
                string sql = @"insert into wechat_subscribe(id,cusno,declarationcode,userid,username,substime,substype,status,openid,statusvalue,codetype) 
                values(wechat_subscribe_id.nextval,'{0}','{1}','{2}','{3}',sysdate,'{4}','{5}','{6}','{7}','{8}')";
                List<string> sqls = new List<string>();
                for (int i = 0; i < status.Length; i++)
                {
                    string statusvalue = SwitchHelper.switchValue(type, status[i]);
                    sqls.Add(string.Format(sql, cusno, declarationcode, userid, username, type, status[i], openid, statusvalue, codetype));
                }
                using (DBSession db = new DBSession())
                {
                    return db.ExecuteBatch(sqls) > 0 ? true : false;
                }
            }
            catch(Exception ex)
            {
                LogHelper.Write("SubscribeModel_insertSubscribe:" + ex.Message + "——code:" + cusno + declarationcode);
                return false;
            }
            
        }
        /// <summary>
        /// 获取预制单订阅状态
        /// </summary>
        /// <param name="declarationcode"></param>
        /// <returns></returns>
        public static DataTable getDeclstatus(string declarationCode)
        {
            using(DBSession db=new DBSession())
            {
                string sql = "select customsstatus from list_declaration where declarationcode='" + declarationCode + "'";
                return db.QuerySignle(sql);
            }
        }
        /// <summary>
        /// 获取业务物流订阅状态
        /// </summary>
        /// <param name="cusno"></param>
        /// <returns></returns>
        public static DataTable getLogisticsstatus(string cusno)
        {
            using (DBSession db = new DBSession())
            {
                string sql = "select logisticsname from list_order where cusno='" + cusno + "'";
                return db.QuerySignle(sql);
            }
        }
        /// <summary>
        /// 获取业务报关状态
        /// </summary>
        /// <param name="cusno"></param>
        /// <returns></returns>
        public static DataTable getOrderstatus(string cusno)
        {
            using (DBSession db = new DBSession())
            {
                string sql = "select declstatus from list_order where code='" + cusno + "'";
                return db.QuerySignle(sql);
            }
        }
        /// <summary>
        /// 获取触发状态（0已订阅，1已触发，2已推送）
        /// </summary>
        /// <param name="cusno"></param>
        /// <returns></returns>
        public static DataTable GetTriggerstatus(string cusno, string checkedStatus, string type, string declarationcode, int userid)
        {
            using (DBSession db = new DBSession())
            {
                string sql;
                if (type.Equals("报关状态"))
                {
                    sql = "select triggerstatus from wechat_subscribe where cusno='" + cusno +
                                "'  and status = '" + checkedStatus + "' and substype = '" + type + "' and declarationcode = '" + declarationcode + "' and userid=" + userid;
                }
                else
                {
                    sql = "select triggerstatus from wechat_subscribe where cusno='" + cusno + "'  and status = '" + checkedStatus + "' and substype = '" + type + "'  and userid=" + userid;
                }
                 
                return db.QuerySignle(sql);
            }
        }

        /// <summary>
        /// 删除订阅信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static bool deleteSubscribe(string id)
        {
            using(DBSession db=new DBSession())
            {
                string sql = "delete from wechat_subscribe where id=" + id + " and triggerstatus=0";
                return db.ExecuteSignle(sql) == 0 ? false : true;
            }
        }
    }

}