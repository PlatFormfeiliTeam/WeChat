//业务操作——打开详情弹出框
function getBusiInfo_customer(ordercode)
{
    if (ordercode == "") {
        $.toast("请选择需要查看的记录");
        return;
    }
    $.showPreloader('加载中...');
    //清空弹出窗信息
    $("#pop_tab_decl").html("");
    $("#pop_tab_insp").html("");
    $("#pop_tab_logistics").html("");
    
    
    $.ajax({
        type: 'post',
        url: '../MyBusiness/MyBusiness.aspx/QueryOrderDetail',
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        data: "{'code':'" + ordercode + "'}",
        cache: false,
        async: false, //默认是true，异步；false为同步，此方法执行完在执行下面代码
        success: function (data) {
            var obj = eval("(" + data.d + ")");
            console.log(obj);
            //1、报关信息
            var orderTable = obj.OrderTable;
            var declTable = obj.DeclTable;
            var inspTable = obj.InspTable;
            var logisticsTable = obj.LogisticsTable;
            if (orderTable != null && orderTable.length > 0) {
                var declstr = '<div class="content-padded grid-demo" >' +
                    '<div class="row">' +
                    '<div class="col-20">委托时间</div>' +
                    '<div class="col-60">' +
                    (orderTable[0]["SUBMITTIME"] == null ? "" : orderTable[0]["SUBMITTIME"]) +
                    '</div>' +
                    '<div class="col-20">' +
                    (orderTable[0]["SUBMITUSERNAME"] == null ? "" : orderTable[0]["SUBMITUSERNAME"]) +
                    '</div>' +
                    '</div>' +
                    '<div class="row">' +
                    '<div class="col-20">制单完成</div>' +
                    '<div class="col-60">' +
                    (orderTable[0]["MOENDTIME"] == null ? "" : orderTable[0]["MOENDTIME"]) +
                    '</div>' +
                    '<div class="col-20">' +
                    (orderTable[0]["MOENDNAME"] == null ? "" : orderTable[0]["MOENDNAME"]) +
                    '</div>' +
                    '</div>' +
                    '<div class="row">' +
                    '<div class="col-20">审核完成</div>' +
                    '<div class="col-60">' +
                    (orderTable[0]["COENDTIME"] == null ? "" : orderTable[0]["COENDTIME"]) +
                    '</div>' +
                    '<div class="col-20">' +
                    (orderTable[0]["COENDNAME"] == null ? "" : orderTable[0]["COENDNAME"]) +
                    '</div>' +
                    '</div>' +
                    '<div class="row">' +
                    '<div class="col-20">预录完成</div>' +
                    '<div class="col-60">' +
                    (orderTable[0]["PREENDTIME"] == null ? "" : orderTable[0]["PREENDTIME"]) +
                    '</div>' +
                    '<div class="col-20">' +
                    (orderTable[0]["PREENDNAME"] == null ? "" : orderTable[0]["PREENDNAME"]) +
                    '</div>' +
                    '</div>' +
                    '<div class="row">' +
                    '<div class="col-20">申报完成</div>' +
                    '<div class="col-60">' +
                    (orderTable[0]["REPENDTIME"] == null ? "" : orderTable[0]["REPENDTIME"]) +
                    '</div>' +
                    '<div class="col-20">' +
                    (orderTable[0]["REPENDNAME"] == null ? "" : orderTable[0]["REPENDNAME"]) +
                    '</div>' +
                    '</div>' +
                    '<div class="row">' +
                    '<div class="col-20">现场报关</div>' +
                    '<div class="col-60">' +
                    (orderTable[0]["SITEAPPLYTIME"] == null ? "" : orderTable[0]["SITEAPPLYTIME"]) +
                    '</div>' +
                    '<div class="col-20">' +
                    (orderTable[0]["SITEAPPLYUSERNAME"] == null ? "" : orderTable[0]["SITEAPPLYUSERNAME"]) +
                    '</div>' +
                    '</div>' +
                    '<div class="row">' +
                    '<div class="col-20">查验维护</div>' +
                    '<div class="col-60">' +
                    (orderTable[0]["DECLCHECKTIME"] == null ? "" : orderTable[0]["DECLCHECKTIME"]) +
                    '</div>' +
                    '<div class="col-20">' +
                    (orderTable[0]["SUBMITUSERNAME"] == null ? "" : orderTable[0]["SUBMITUSERNAME"]) +
                    '</div>' +
                    '</div>' +
                    '<div class="row">' +
                    '<div class="col-20">现场稽核</div>' +
                    '<div class="col-60">' +
                    (orderTable[0]["AUDITFLAGTIME"] == null ? "" : orderTable[0]["AUDITFLAGTIME"]) +
                    '</div>' +
                    '<div class="col-20">' +
                    (orderTable[0]["AUDITFLAGNAME"] == null ? "" : orderTable[0]["AUDITFLAGNAME"]) +
                    '</div>' +
                    '</div>' +
                    '<div class="row">' +
                    '<div class="col-20">现场放行</div>' +
                    '<div class="col-60">' +
                    (orderTable[0]["SITEPASSTIME"] == null ? "" : orderTable[0]["SITEPASSTIME"]) +
                    '</div>' +
                    '<div class="col-20">' +
                    (orderTable[0]["SITEPASSUSERNAME"] == null ? "" : orderTable[0]["SITEPASSUSERNAME"]) +
                    '</div>' +
                    '</div>' +
                    '<div class="row">' +
                    '<div class="col-20">查验图片</div>' +
                    '<div class="col-60">' +
                    (orderTable[0]["CHECKPIC"] == null ? "" : orderTable[0]["CHECKPIC"]) +
                    '</div>' +
                    '</div>' +
                    '</div>';
                declstr += '<div>';
                if (declTable != null) {
                    for (var i = 0; i < declTable.length; i++) {
                        declstr += '<div style="border-top: 1px solid #000;" class="list-block" id="' +
                            (declTable[i]["DECLARATIONCODE"] == null ? "" : declTable[i]["DECLARATIONCODE"]) +
                            '">';
                        declstr += '<div class="row">';
                        declstr += '<div class="col-40">' + (declTable[i]["DECLARATIONCODE"] == null ? "" : declTable[i]["DECLARATIONCODE"]) + '</div>';
                        declstr += '<div class="col-40">' +
                            declTable[i]["GOODSNUM"] +
                            '/' +
                            declTable[i]["GOODSGW"] +
                            '</div>';
                        declstr += '<div class="col-20">' + (declTable[i]["MODIFYFLAG"] == null ? "" : declTable[i]["MODIFYFLAG"]) + '</div>';
                        declstr += '</div>';
                        declstr += '<div class="row">';
                        declstr += '<div class="col-40">' + (declTable[i]["TRANSNAME"] == null ? "" : declTable[i]["TRANSNAME"]) + '</div>';
                        declstr += '<div class="col-40">' + (declTable[i]["TRADENAME"] == null ? "" : declTable[i]["TRADENAME"]) + '</div>';
                        declstr += '<div class="col-20">' + (declTable[i]["CUSTOMSSTATUS"] == null ? "" : declTable[i]["CUSTOMSSTATUS"]) + '</div>';
                        declstr += '</div>';
                        declstr += '</div>';
                    }
                }
                declstr += '</div>';
                $("#pop_tab_decl").append(declstr);
            }
            //2、报检信息
            if (orderTable != null &&
                (orderTable[0]["ENTRUSTTYPE"] == "02" || orderTable[0]["ENTRUSTTYPE"] == "03")) {
                var inspstr = '<div class="content-padded grid-demo" >' +
                    '<div class="row">' +
                    '<div class="col-20">委托时间</div>' +
                    '<div class="col-60">' +
                    (orderTable[0]["SUBMITTIME"] == null ? "" : orderTable[0]["SUBMITTIME"]) +
                    '</div>' +
                    '<div class="col-20">' +
                    (orderTable[0]["SUBMITUSERNAME"] == null ? "" : orderTable[0]["SUBMITUSERNAME"]) +
                    '</div>' +
                    '</div>' +
                    '<div class="row">' +
                    '<div class="col-20">制单完成</div>' +
                    '<div class="col-60">' +
                    (orderTable[0]["INSPMOENDTIME"] == null ? "" : orderTable[0]["INSPMOENDTIME"]) +
                    '</div>' +
                    '<div class="col-20">' +
                    (orderTable[0]["INSPMOENDNAME"] == null ? "" : orderTable[0]["INSPMOENDNAME"]) +
                    '</div>' +
                    '</div>' +
                    '<div class="row">' +
                    '<div class="col-20">审核完成</div>' +
                    '<div class="col-60">' +
                    (orderTable[0]["INSPCOENDTIME"] == null ? "" : orderTable[0]["INSPCOENDTIME"]) +
                    '</div>' +
                    '<div class="col-20">' +
                    (orderTable[0]["INSPCOENDNAME"] == null ? "" : orderTable[0]["INSPCOENDNAME"]) +
                    '</div>' +
                    '</div>' +
                    '<div class="row">' +
                    '<div class="col-20">预录完成</div>' +
                    '<div class="col-60">' +
                    (orderTable[0]["INSPPREENDTIME"] == null ? "" : orderTable[0]["INSPPREENDTIME"]) +
                    '</div>' +
                    '<div class="col-20">' +
                    (orderTable[0]["INSPPREENDNAME"] == null ? "" : orderTable[0]["INSPPREENDNAME"]) +
                    '</div>' +
                    '</div>' +
                    '<div class="row">' +
                    '<div class="col-20">申报完成</div>' +
                    '<div class="col-60">' +
                    (orderTable[0]["INSPREPENDTIME"] == null ? "" : orderTable[0]["INSPREPENDTIME"]) +
                    '</div>' +
                    '<div class="col-20">' +
                    (orderTable[0]["INSPREPENDNAME"] == null ? "" : orderTable[0]["INSPREPENDNAME"]) +
                    '</div>' +
                    '</div>' +
                    '<div class="row">' +
                    '<div class="col-20">现场报检</div>' +
                    '<div class="col-60">' +
                    (orderTable[0]["INSPSITEAPPLYTIME"] == null ? "" : orderTable[0]["INSPSITEAPPLYTIME"]) +
                    '</div>' +
                    '<div class="col-20">' +
                    (orderTable[0]["INSPSITEAPPLYUSERNAME"] == null ? "" : orderTable[0]["INSPSITEAPPLYUSERNAME"]) +
                    '</div>' +
                    '</div>' +
                    '<div class="row">' +
                    '<div class="col-20">查验维护</div>' +
                    '<div class="col-60">' +
                    (orderTable[0]["INSPCHECKTIME"] == null ? "" : orderTable[0]["INSPCHECKTIME"]) +
                    '</div>' +
                    '<div class="col-20">' +
                    (orderTable[0]["INSPSUBMITUSERNAME"] == null ? "" : orderTable[0]["INSPSUBMITUSERNAME"]) +
                    '</div>' +
                    '</div>' +
                    '<div class="row">' +
                    '<div class="col-20">熏蒸维护</div>' +
                    '<div class="col-60">' +
                    (orderTable[0]["FUMIGATIONTIME"] == null ? "" : orderTable[0]["FUMIGATIONTIME"]) +
                    '</div>' +
                    '<div class="col-20">' +
                    (orderTable[0]["FUMIGATIONNAME"] == null ? "" : orderTable[0]["FUMIGATIONNAME"]) +
                    '</div>' +
                    '</div>' +
                    '<div class="row">' +
                    '<div class="col-20">报检放行</div>' +
                    '<div class="col-60">' +
                    (orderTable[0]["INSPSITEPASSTIME"] == null ? "" : orderTable[0]["INSPSITEPASSTIME"]) +
                    '</div>' +
                    '<div class="col-20">' +
                    (orderTable[0]["INSPSITEPASSUSERNAME"] == null ? "" : orderTable[0]["INSPSITEPASSUSERNAME"]) +
                    '</div>' +
                    '</div>' +
                    '<div class="row">' +
                    '<div class="col-20">查验图片</div>' +
                    '<div class="col-60">' +
                    (orderTable[0]["INSPCHECKPIC"] == null ? "" : orderTable[0]["INSPCHECKPIC"]) +
                    '</div>' +
                    '</div>' +
                    '</div>';
                inspstr += '<div>';
                if (inspTable != null) {
                    for (var i = 0; i < inspTable.length; i++) {
                        inspstr += '<div style="border-top: 1px solid #000;" class="list-block">';
                        inspstr += '<div class="row">';
                        inspstr += '<div class="col-40">' + (inspTable[i]["INSPECTIONCODE"] == null ? "" : inspTable[i]["INSPECTIONCODE"]) + '</div>';
                        inspstr += '<div class="col-40">' + (inspTable[i]["APPROVALCODE"] == null ? "" : inspTable[i]["APPROVALCODE"]) + '</div>';

                        inspstr += '</div>';
                        inspstr += '<div class="row">';
                        inspstr += '<div class="col-40">' + (inspTable[i]["CLEARANCECODE"] == null ? "" : inspTable[i]["CLEARANCECODE"]) + '</div>';
                        inspstr += '<div class="col-40">' + (inspTable[i]["MODIFYFLAG"] == null ? "" : inspTable[i]["MODIFYFLAG"]) + '</div>';
                        inspstr += '<div class="col-20">' + (inspTable[i]["INSPSTATUS"] == null ? "" : inspTable[i]["INSPSTATUS"]) + '</div>';
                        inspstr += '</div>';
                        inspstr += '</div>';
                    }
                }
                inspstr += '</div>';
                $("#pop_tab_insp").append(inspstr);
            }
            //物流状态
            if (logisticsTable != null && logisticsTable.length > 0) {
                var logstr = '<div style="text-align:center;">物流状态（' +
                    orderTable[0]["BUSITYPE"] +
                    '）</div>';
                logstr += '<div class="row">' +
                    '<div class="col-25" style="color:#0894EC"><i id="icon_choudan" class="iconfont" >&#xe63f;抽单</i></div>' +
                    '<div class="col-25"><i id="icon_zhuanguan" class="iconfont" >&#xe63f;转关</i></div>' +
                    '<div class="col-25"><i id="icon_baojian" class="iconfont" >&#xe63f;报检</i></div>' +
                    '<div class="col-25"><i id="icon_yunshu" class="iconfont" >&#xe63f;运输</i></div>' +
                    '</div>';
                logstr +=
                    '<div style="width:100%;background-color:#DBDBDB;line-height:0.2rem;">&nbsp;</div>';
                logstr += '<div id="logistics" style="background-color:white;">';
                logstr += '<div class="row">' +
                    '<div class="col-60">时间</div>' +
                    '<div class="col-35">状态值</div>' +
                    '</div>'
                var choudan = '<div id="choudan">';
                var zhuanguan = '<div id="zhuanguan" style="display:none">';
                var baojian = '<div id="baojian" style="display:none">';
                var yunshu = '<div id="yunshu" style="display:none">';
                for (var i = 0; i < logisticsTable.length; i++) {
                    if (logisticsTable[i]["OPERATE_TYPE"] == "抽单状态") {
                        choudan += '<div class="row">' +
                            '<div class="col-25">' +
                            (logisticsTable[i]["OPERATER"] == null ? "" : logisticsTable[i]["OPERATER"]) +
                            '</div>' +
                            '<div class="col-60">' +
                            (logisticsTable[i]["OPERATE_DATE"] == null ? "" : logisticsTable[i]["OPERATE_DATE"]) +
                            '</div>' +
                            '<div class="col-35">' +
                            (logisticsTable[i]["OPERATE_RESULT"] == null ? "" : logisticsTable[i]["OPERATE_RESULT"]) +
                            '</div>' +
                            '</div>';
                    } else if (logisticsTable[i]["OPERATE_TYPE"] == "报关申报状态" ||
                        logisticsTable[i]["OPERATE_TYPE"] == "转关申报状态") {
                        zhuanguan += '<div class="row">' +
                            '<div class="col-25">' +
                            (logisticsTable[i]["OPERATER"] == null ? "" : logisticsTable[i]["OPERATER"]) +
                            '</div>' +
                            '<div class="col-60">' +
                            (logisticsTable[i]["OPERATE_DATE"] == null ? "" : logisticsTable[i]["OPERATE_DATE"]) +
                            '</div>' +
                            '<div class="col-35">' +
                            (logisticsTable[i]["OPERATE_RESULT"] == null ? "" : logisticsTable[i]["OPERATE_RESULT"]) +
                            '</div>' +
                            '</div>';
                    } else if (logisticsTable[i]["OPERATE_TYPE"] == "商检状态") {
                        baojian += '<div class="row">' +
                            '<div class="col-25">' +
                            (logisticsTable[i]["OPERATER"] == null ? "" : logisticsTable[i]["OPERATER"]) +
                            '</div>' +
                            '<div class="col-60">' +
                            (logisticsTable[i]["OPERATE_DATE"] == null ? "" : logisticsTable[i]["OPERATE_DATE"]) +
                            '</div>' +
                            '<div class="col-35">' +
                            (logisticsTable[i]["OPERATE_RESULT"] == null ? "" : logisticsTable[i]["OPERATE_RESULT"]) +
                            '</div>' +
                            '</div>';
                    } else if (logisticsTable[i]["OPERATE_TYPE"] == "运输状态") {
                        yunshu += '<div class="row">' +
                            '<div class="col-25">' +
                            (logisticsTable[i]["OPERATER"] == null ? "" : logisticsTable[i]["OPERATER"]) +
                            '</div>' +
                            '<div class="col-60">' +
                            (logisticsTable[i]["OPERATE_DATE"] == null ? "" : logisticsTable[i]["OPERATE_DATE"]) +
                            '</div>' +
                            '<div class="col-35">' +
                            (logisticsTable[i]["OPERATE_RESULT"] == null ? "" : logisticsTable[i]["OPERATE_RESULT"]) +
                            '</div>' +
                            '</div>';
                    }

                }
                choudan += '</div>';
                zhuanguan += '</div>';
                baojian += '</div>';
                yunshu += '</div>';
                logstr += choudan;
                logstr += zhuanguan;
                logstr += baojian;
                logstr += yunshu;
                logstr += "</div>";
                $("#pop_tab_logistics").append(logstr);
            }
        }
    });
    $.hidePreloader();
    $.popup('.popup-detail');
}


//我的订单——打开详情弹出框
function getBusiInfo_company(ordercode) {
    if (ordercode == "") {
        $.toast("请选择需要查看的记录");
        return;
    }
    $.showPreloader('加载中...');
    //清空弹出窗信息
    $("#pop_tab_decl").html("");
    $("#pop_tab_insp").html("");
    $("#pop_tab_logistics").html("");
   
    
    $.ajax({
        type: 'post',
        url: 'MyBusiness.aspx/QueryOrderDetail',
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        data: "{'code':'" + ordercode + "'}",
        cache: false,
        async: false, //默认是true，异步；false为同步，此方法执行完在执行下面代码
        success: function (data) {
            var obj = eval("(" + data.d + ")");
            console.log(obj);
            //1、报关信息
            var orderTable = obj.OrderTable;
            var declTable = obj.DeclTable;
            var inspTable = obj.InspTable;
            var logisticsTable = obj.LogisticsTable;
            if (orderTable != null && orderTable.length > 0) {
                var declstr = '<div class="content-padded grid-demo" >' +
                    '<div class="row">' +
                    '<div class="col-20">委托时间</div>' +
                    '<div class="col-60">' +
                    (orderTable[0]["SUBMITTIME"] == null ? "" : orderTable[0]["SUBMITTIME"]) +
                    '</div>' +
                    '</div>' +
                    '<div class="row">' +
                    '<div class="col-20">制单完成</div>' +
                    '<div class="col-60">' +
                    (orderTable[0]["MOENDTIME"] == null ? "" : orderTable[0]["MOENDTIME"]) +
                    '</div>' +
                    '</div>' +
                    '<div class="row">' +
                    '<div class="col-20">审核完成</div>' +
                    '<div class="col-60">' +
                    (orderTable[0]["COENDTIME"] == null ? "" : orderTable[0]["COENDTIME"]) +
                    '</div>' +
                    '</div>' +
                    '<div class="row">' +
                    '<div class="col-20">预录完成</div>' +
                    '<div class="col-60">' +
                    (orderTable[0]["PREENDTIME"] == null ? "" : orderTable[0]["PREENDTIME"]) +
                    '</div>' +
                    '</div>' +
                    '<div class="row">' +
                    '<div class="col-20">申报完成</div>' +
                    '<div class="col-60">' +
                    (orderTable[0]["REPENDTIME"] == null ? "" : orderTable[0]["REPENDTIME"]) +
                    '</div>' +
                    '</div>' +
                    '<div class="row">' +
                    '<div class="col-20">现场报关</div>' +
                    '<div class="col-60">' +
                    (orderTable[0]["SITEAPPLYTIME"] == null ? "" : orderTable[0]["SITEAPPLYTIME"]) +
                    '</div>' +
                    '</div>' +
                    '<div class="row">' +
                    '<div class="col-20">查验维护</div>' +
                    '<div class="col-60">' +
                    (orderTable[0]["DECLCHECKTIME"] == null ? "" : orderTable[0]["DECLCHECKTIME"]) +
                    '</div>' +
                    '</div>' +
                    '<div class="row">' +
                    '<div class="col-20">现场稽核</div>' +
                    '<div class="col-60">' +
                    (orderTable[0]["AUDITFLAGTIME"] == null ? "" : orderTable[0]["AUDITFLAGTIME"]) +
                    '</div>' +
                    '</div>' +
                    '<div class="row">' +
                    '<div class="col-20">现场放行</div>' +
                    '<div class="col-60">' +
                    (orderTable[0]["SITEPASSTIME"] == null ? "" : orderTable[0]["SITEPASSTIME"]) +
                    '</div>' +
                    '</div>' +
                    '<div class="row">' +
                    '<div class="col-20">查验图片</div>' +
                    '<div class="col-60">' +
                    (orderTable[0]["CHECKPIC"] == null ? "" : orderTable[0]["CHECKPIC"]) +
                    '</div>' +
                    '</div>' +
                    '</div>';
                declstr += '<div>';
                if (declTable != null) {
                    for (var i = 0; i < declTable.length; i++) {
                        declstr += '<div style="border-top: 1px solid #000;" class="list-block" id="' +
                            (declTable[i]["DECLARATIONCODE"] == null ? "" : declTable[i]["DECLARATIONCODE"]) +
                            '">';
                        declstr += '<div class="row">';
                        declstr += '<div class="col-40">' + (declTable[i]["DECLARATIONCODE"] == null ? "" : declTable[i]["DECLARATIONCODE"]) + '</div>';
                        declstr += '<div class="col-40">' +
                            declTable[i]["GOODSNUM"] +
                            '/' +
                            declTable[i]["GOODSGW"] +
                            '</div>';
                        declstr += '<div class="col-20">' + (declTable[i]["MODIFYFLAG"] == null ? "" : declTable[i]["MODIFYFLAG"]) + '</div>';
                        declstr += '</div>';
                        declstr += '<div class="row">';
                        declstr += '<div class="col-40">' + (declTable[i]["TRANSNAME"] == null ? "" : declTable[i]["TRANSNAME"]) + '</div>';
                        declstr += '<div class="col-40">' + (declTable[i]["TRADENAME"] == null ? "" : declTable[i]["TRADENAME"]) + '</div>';
                        declstr += '<div class="col-20">' + (declTable[i]["CUSTOMSSTATUS"] == null ? "" : declTable[i]["CUSTOMSSTATUS"]) + '</div>';
                        declstr += '</div>';
                        declstr += '</div>';
                    }
                }
                declstr += '</div>';
                $("#pop_tab_decl").append(declstr);
            }
            //2、报检信息
            if (orderTable != null &&
                (orderTable[0]["ENTRUSTTYPE"] == "02" || orderTable[0]["ENTRUSTTYPE"] == "03")) {
                var inspstr = '<div class="content-padded grid-demo" >' +
                    '<div class="row">' +
                    '<div class="col-20">委托时间</div>' +
                    '<div class="col-60">' +
                    (orderTable[0]["SUBMITTIME"] == null ? "" : orderTable[0]["SUBMITTIME"]) +
                    '</div>' +
                    '</div>' +
                    '<div class="row">' +
                    '<div class="col-20">制单完成</div>' +
                    '<div class="col-60">' +
                    (orderTable[0]["INSPMOENDTIME"] == null ? "" : orderTable[0]["INSPMOENDTIME"]) +
                    '</div>' +
                    '</div>' +
                    '<div class="row">' +
                    '<div class="col-20">审核完成</div>' +
                    '<div class="col-60">' +
                    (orderTable[0]["INSPCOENDTIME"] == null ? "" : orderTable[0]["INSPCOENDTIME"]) +
                    '</div>' +
                    '</div>' +
                    '<div class="row">' +
                    '<div class="col-20">预录完成</div>' +
                    '<div class="col-60">' +
                    (orderTable[0]["INSPPREENDTIME"] == null ? "" : orderTable[0]["INSPPREENDTIME"]) +
                    '</div>' +
                    '</div>' +
                    '<div class="row">' +
                    '<div class="col-20">申报完成</div>' +
                    '<div class="col-60">' +
                    (orderTable[0]["INSPREPENDTIME"] == null ? "" : orderTable[0]["INSPREPENDTIME"]) +
                    '</div>' +
                    '</div>' +
                    '<div class="row">' +
                    '<div class="col-20">现场报检</div>' +
                    '<div class="col-60">' +
                    (orderTable[0]["INSPSITEAPPLYTIME"] == null ? "" : orderTable[0]["INSPSITEAPPLYTIME"]) +
                    '</div>' +
                    '</div>' +
                    '<div class="row">' +
                    '<div class="col-20">查验维护</div>' +
                    '<div class="col-60">' +
                    (orderTable[0]["INSPCHECKTIME"] == null ? "" : orderTable[0]["INSPCHECKTIME"]) +
                    '</div>' +
                    '</div>' +
                    '<div class="row">' +
                    '<div class="col-20">熏蒸维护</div>' +
                    '<div class="col-60">' +
                    (orderTable[0]["FUMIGATIONTIME"] == null ? "" : orderTable[0]["FUMIGATIONTIME"]) +
                    '</div>' +
                    '</div>' +
                    '<div class="row">' +
                    '<div class="col-20">报检放行</div>' +
                    '<div class="col-60">' +
                    (orderTable[0]["INSPSITEPASSTIME"] == null ? "" : orderTable[0]["INSPSITEPASSTIME"]) +
                    '</div>' +
                    '</div>' +
                    '<div class="row">' +
                    '<div class="col-20">查验图片</div>' +
                    '<div class="col-60">' +
                    (orderTable[0]["INSPCHECKPIC"] == null ? "" : orderTable[0]["INSPCHECKPIC"]) +
                    '</div>' +
                    '</div>' +
                    '</div>';
                inspstr += '<div>';
                if (inspTable != null) {
                    for (var i = 0; i < inspTable.length; i++) {
                        inspstr += '<div style="border-top: 1px solid #000;" class="list-block">';
                        inspstr += '<div class="row">';
                        inspstr += '<div class="col-40">' + (inspTable[i]["INSPECTIONCODE"] == null ? "" : inspTable[i]["INSPECTIONCODE"]) + '</div>';
                        inspstr += '<div class="col-40">' + (inspTable[i]["APPROVALCODE"] == null ? "" : inspTable[i]["APPROVALCODE"]) + '</div>';

                        inspstr += '</div>';
                        inspstr += '<div class="row">';
                        inspstr += '<div class="col-40">' + (inspTable[i]["CLEARANCECODE"] == null ? "" : inspTable[i]["CLEARANCECODE"]) + '</div>';
                        inspstr += '<div class="col-40">' + (inspTable[i]["MODIFYFLAG"] == null ? "" : inspTable[i]["MODIFYFLAG"]) + '</div>';
                        inspstr += '<div class="col-20">' + (inspTable[i]["INSPSTATUS"] == null ? "" : inspTable[i]["INSPSTATUS"]) + '</div>';
                        inspstr += '</div>';
                        inspstr += '</div>';
                    }
                }
                inspstr += '</div>';
                $("#pop_tab_insp").append(inspstr);
            }
            //物流状态
            if (logisticsTable != null && logisticsTable.length > 0) {
                var logstr = '<div style="text-align:center;">物流状态（' +
                    orderTable[0]["BUSITYPE"] +
                    '）</div>';
                logstr += '<div class="row">' +
                    '<div class="col-25" style="color:#0894EC"><i id="icon_choudan" class="iconfont" >&#xe63f;抽单</i></div>' +
                    '<div class="col-25"><i id="icon_zhuanguan" class="iconfont" >&#xe63f;转关</i></div>' +
                    '<div class="col-25"><i id="icon_baojian" class="iconfont" >&#xe63f;报检</i></div>' +
                    '<div class="col-25"><i id="icon_yunshu" class="iconfont" >&#xe63f;运输</i></div>' +
                    '</div>';
                logstr +=
                    '<div style="width:100%;background-color:#DBDBDB;line-height:0.2rem;">&nbsp;</div>';
                logstr += '<div id="logistics" style="background-color:white;">';
                logstr += '<div class="row">' +
                    '<div class="col-60">时间</div>' +
                    '<div class="col-35">状态值</div>' +
                    '</div>'
                var choudan = '<div id="choudan">';
                var zhuanguan = '<div id="zhuanguan" style="display:none">';
                var baojian = '<div id="baojian" style="display:none">';
                var yunshu = '<div id="yunshu" style="display:none">';
                for (var i = 0; i < logisticsTable.length; i++) {
                    if (logisticsTable[i]["OPERATE_TYPE"] == "抽单状态") {
                        choudan += '<div class="row">' +
                            '<div class="col-60">' +
                            (logisticsTable[i]["OPERATE_DATE"] == null ? "" : logisticsTable[i]["OPERATE_DATE"]) +
                            '</div>' +
                            '<div class="col-35">' +
                            (logisticsTable[i]["OPERATE_RESULT"] == null ? "" : logisticsTable[i]["OPERATE_RESULT"]) +
                            '</div>' +
                            '</div>';
                    } else if (logisticsTable[i]["OPERATE_TYPE"] == "报关申报状态" ||
                        logisticsTable[i]["OPERATE_TYPE"] == "转关申报状态") {
                        zhuanguan += '<div class="row">' +
                            '<div class="col-60">' +
                            (logisticsTable[i]["OPERATE_DATE"] == null ? "" : logisticsTable[i]["OPERATE_DATE"]) +
                            '</div>' +
                            '<div class="col-35">' +
                            (logisticsTable[i]["OPERATE_RESULT"] == null ? "" : logisticsTable[i]["OPERATE_RESULT"]) +
                            '</div>' +
                            '</div>';
                    } else if (logisticsTable[i]["OPERATE_TYPE"] == "商检状态") {
                        baojian += '<div class="row">' +
                            '<div class="col-60">' +
                            (logisticsTable[i]["OPERATE_DATE"] == null ? "" : logisticsTable[i]["OPERATE_DATE"]) +
                            '</div>' +
                            '<div class="col-35">' +
                            (logisticsTable[i]["OPERATE_RESULT"] == null ? "" : logisticsTable[i]["OPERATE_RESULT"]) +
                            '</div>' +
                            '</div>';
                    } else if (logisticsTable[i]["OPERATE_TYPE"] == "运输状态") {
                        yunshu += '<div class="row">' +
                            '<div class="col-60">' +
                            (logisticsTable[i]["OPERATE_DATE"] == null ? "" : logisticsTable[i]["OPERATE_DATE"]) +
                            '</div>' +
                            '<div class="col-35">' +
                            (logisticsTable[i]["OPERATE_RESULT"] == null ? "" : logisticsTable[i]["OPERATE_RESULT"]) +
                            '</div>' +
                            '</div>';
                    }

                }
                choudan += '</div>';
                zhuanguan += '</div>';
                baojian += '</div>';
                yunshu += '</div>';
                logstr += choudan;
                logstr += zhuanguan;
                logstr += baojian;
                logstr += yunshu;
                logstr += "</div>";
                $("#pop_tab_logistics").append(logstr);
            }
        }
    });
    $.hidePreloader();
    $.popup('.popup-detail');
}
