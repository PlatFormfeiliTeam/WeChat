using System;
using System.Collections.Generic;
using System.EnterpriseServices;
using System.Linq;
using System.Web;

namespace WeChat.Entity.Enum
{
    public enum DeclStatusEnum
    {
        /// <summary>
        /// 草稿（订单生成)
        /// </summary>
        CreateOrder = 0,
        /// <summary>
        /// 已委托（订单提交）
        /// </summary>
        SubmitOrder = 10,
        /// <summary>
        /// 拆分时间（只做传值 不是真实状态）
        /// </summary>
        FileSplie = 13,
        /// <summary>
        /// 已受理（业务点编辑）
        /// </summary>
        AcceptOrder = 15,
        /// <summary>
        /// 制单中（制单开始,预制单是在业务界面点打开的时候创建的，刚创建时状态为空）
        /// </summary>
        MOStart = 20,
        /// <summary>
        /// 制单完成（制单完成）
        /// </summary>
        MOFinish = 30,
        /// <summary>
        /// 撤销制单完成（不是真正状态，仅做传值用）
        /// </summary>
        CancleMOFinish = 31,
        /// <summary>
        /// 审核中（审核开始）
        /// </summary>
        FCOStart = 40,
        /// <summary>
        /// 审核完成（审核完成）
        /// </summary>
        FCOFinish = 50,
        /// <summary>
        /// 撤销审核完成（不是真正状态，仅做传值用）
        /// </summary>
        CancleFCOFinish = 51,
        /// <summary>
        /// 复审完成(点击复审按钮)
        /// </summary>
        RECheck=55,
        /// <summary>
        /// 待预录（制单/审单发送）
        /// </summary>
        WaitPI = 60,
        /// <summary>
        /// 撤销待预录（撤销发送，不是真正状态，仅做传值用）
        /// </summary>
        CancleWaitPI = 61,
        /// <summary>
        /// 预录中（选择通道）
        /// </summary>
        PreingInput = 70,
        /// <summary>
        /// 预录完成（维护暂存编号的时候预录完成）
        /// </summary>
        PIFinish = 80,        
        /// <summary>
        /// 审核校验完成（维护审核校验人和时间的时候记录,是制输审的审核,并不是预录校验完成，预录校验完成有专门的字段）
        /// </summary>
        SCOFinish = 90,
        /// <summary>
        /// 申报中（点击申报按钮开始申报）
        /// </summary>
        Declare = 100,
        
        /// <summary>
        /// 提前转关生成（通过回执状态抓取， 提前转关生成就可以打印提前报关单，打印关联状态会变成提前申报完成，打印的单子通过解析会回填预录入号等信息）
        /// </summary>
        PreTransFinish = 105,
        /// <summary>
        /// 提前申报完成（提前转关生成就可以打印提前报关单，打印关联状态会变成提前申报完成，这儿不能用统一编号，因为可能统一编号在校验单上就会生成）
        /// </summary>
        PreDeclFinish = 110,
        /// <summary>
        /// 申报退单
        /// </summary>
        DeclareReturn = 115,
        /// <summary>
        /// 申报完成（维护报关单号或回执抓取到报关单号）
        /// </summary>
        DeclFinish = 120,
         /// <summary>
        /// 未完结（范围，所有小于申报完结的都是未完结）
        /// </summary>
        UnDeclOver = 129,
        /// <summary>
        /// 申报完结（报关单PDF关联时间）
        /// </summary>
        DeclOver = 130,
        /// <summary>
        /// 资料整理（只做传值 不是真实状态）
        /// </summary>
        FileSort=140,
        /// <summary>
        /// 现场报关（只做传值 不是真实状态）
        /// </summary>
        SiteApply=150,
        /// <summary>
        /// 现场放行（只做传值 不是真实状态）
        /// </summary>
        SitePass=160
    }
}