using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WeChat.Entity.Enum
{
    public enum LogisticsStatusEnum
    {
        /// <summary>
        /// 初始状态，无意义
        /// </summary>
        InitialStatus = 0,
        /// <summary>
        /// 抽单受理（抽单状态——已派单）
        /// </summary>
        DrawBillAccepted = 5,
        /// <summary>
        /// 抽单完成（抽单状态——已签收）
        /// </summary>
        DrawBillFinished = 10,
        /// <summary>
        /// 转关申报完成（口岸报关——转关放行）
        /// </summary>
        TransDeclFinished = 20,
        /// <summary>
        /// 口岸报关完成（口岸报关——报关放行）
        /// </summary>
        DeclarationFinished = 30,
        /// <summary>
        /// 口岸报检完成（报检状态——商检放行）
        /// </summary>
        InspectionFinished = 40,
        /// <summary>
        /// 已派车（运输状态——已排班）
        /// </summary>
        SendCar = 45,
        /// <summary>
        /// 提货完成（运输状态——已提货）
        /// </summary>
        PickupFinished = 50,
        /// <summary>
        /// 运输中（运输状态——车辆出发）
        /// </summary>
        InTransit = 60,
        /// <summary>
        /// 运输完成（运输状态——到达中转站）
        /// </summary>
        TransportFinished = 70,
        /// <summary>
        /// 送货完成（运输状态——运输完成）
        /// </summary>
        DeliveryFinished = 80
    }
}