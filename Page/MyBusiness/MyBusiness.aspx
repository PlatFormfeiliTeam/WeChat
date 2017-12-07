<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="MyBusiness.aspx.cs" Inherits="WeChat.Page.MyBusiness.MyBusiness" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <meta name="viewport" content="initial-scale=1, maximum-scale=1" />
    <link href="/css/iconfont/iconfont.css" rel="stylesheet" />

    <link rel="stylesheet" href="//g.alicdn.com/msui/sm/0.6.2/css/sm.min.css" />
    <script type='text/javascript' src='//g.alicdn.com/sj/lib/zepto/zepto.min.js' charset='utf-8'></script>
    <link rel="stylesheet" href="//g.alicdn.com/msui/sm/0.6.2/css/sm-extend.min.css" />
    <%--<script type='text/javascript' src='//g.alicdn.com/msui/sm/0.6.2/js/sm-extend.min.js' charset='utf-8'></script>--%>
    <style type="text/css">
        #scroll-bottom-one {
            top: 7.8rem;
        }
         .bar input[type=search]{
             margin:.1rem 0;
         }
        .content-block
        {
            margin:0rem 0;
            padding:0.2rem;
            color:#6d6d72;
        }
        .content-padded
        {
            margin:0.2rem;
        }
        input
        {
            font-family:"微软雅黑";
            width:6rem;
            height: 1.5rem;
            border: none;
            border-radius: .15rem;
            font-size: .8rem;
        }
        .row
        {
            font-size:small;
            overflow:hidden;
            margin-top:0.3rem;
            margin-left:-2%;
        }
        
        .list-block {
            margin: 0.25rem 0;
        }
        .list-block .item-inner {
            padding-right:0.25rem;
            padding-top:0.1rem;
            padding-bottom:0.1rem;
            min-height:1.2rem;
            font-size:small;
        }
        .list-block .item-content {
            min-height:1.2rem;
        }
        .list-block .my-title {
            width:40%;
            overflow:hidden;
            white-space:nowrap;
            text-overflow:ellipsis;
            text-align:center;
        }
        .list-block .my-after {
            width:30%;
            overflow:hidden;
            white-space:nowrap;
            text-overflow:ellipsis;
            text-align:center;
        }
        .lab
        {
            color:white;
            font-family:"微软雅黑";
            font-size:initial;
        }
        .picker-items-col-wrapper
        {
            width:12rem;
        }
        .button
        {
            font-size:15px;
            height:25px;
            line-height:1.35rem;
            vertical-align:middle;
        }
        .button.button-fill
        {
            line-height:28px;
        }
        .popup .list-block
        {
            margin:0.1rem 0 0 0;
            background-color:white;
        }
        .popup .row
        {
            margin-top:0.1rem;
        }
        .popup-subscribe .content-block
        {
            background-color:#1D2E3C;
            color:white;
            margin-top:1rem;
        }
        .popup-subscribe .row
        {
            background-color:#456581;
            border-top:solid 1px white;
            margin:0rem;
            font-size:initial;
            vertical-align:middle;
            height:2rem;
            line-height:2rem;
        }
        .popup-subscribe .col-33
        {
            padding-top:0.2rem;
        }
        .popup-subscribe .col-50
        {
            margin-left:2%;
        }
        .myrow
        {
            line-height:2rem;
            padding-left:4%;
        }
    </style>

    <script type="text/javascript">
        var userid = "22", username = "test", openid = "oLVv71Ma9dCp5zhoJWuAHTDKAx4A";
        // 加载flag
        var loading = false;
        // 最多可加载的条目
        var maxItems = 100;
        // 每次加载添加多少条目
        var itemsPerLoad = 6;
        var lastIndex = 0;
        var subCusno = "", subDeclarationCode = "";//订阅的单号
        //按钮查询
        function loadData(itemsPerLoad, lastIndex) {
            $.ajax({
                type: "post", //要用post方式                 
                //url: "MyBusiness.aspx?action=QueryData",//进入load根据action匹配方法
                url: "MyBusiness.aspx/QueryData",//通过static+[webmethod],直接访问指定的方法
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                data: "{'declstatus':'" + $("#picker_declstatus").val() +
                        "','inspstatus':'" + $("#picker_inspstatus").val() +
                        "','inout':'" + $("#picker_inout").val() +
                        "','busitype':'" + $("#picker_busitype").val() +
                        "','customs':'" + $("#picker_customs").val() +
                        "','sitedeclare':'" + $("#picker_sitedeclare").val() +
                        "','logisticsstatus':'" + $("#picker_logisticsstatus").val() +
                        "','starttime':'" + $("#txt_startdate").val() +
                        "','endtime':'" + $("#txt_enddate").val() +
                        "','itemsperload':" + itemsPerLoad +
                        ",'lastindex':" + lastIndex + "}",
                cache: false,
                async: false,//默认是true，异步；false为同步，此方法执行完在执行下面代码
                success: function (data) {
                    var obj = eval("(" + data.d + ")");//将字符串转为json
                    for (var i = 0; i < obj.length; i++) {
                        var str = '<div class="list-block" id="' + obj[i]["CODE"] + ',' + obj[i]["CUSNO"] + '" >' +
                                    '<ul>' +
                                        '<li class="item-content">' +
                                            '<div class="item-inner">' +
                                                '<div class="my-title">' + (obj[i]["BUSIUNITNAME"] == null ? "" : obj[i]["BUSIUNITNAME"]) + '</div>' +
                                                '<div class="my-after">' + (obj[i]["BUSITYPENAME"] == null ? "" : obj[i]["BUSITYPENAME"]) + '</div>' +
                                                '<div class="my-after">' + (obj[i]["CUSNO"] == null ? "" : obj[i]["CUSNO"]) + '</div>' +
                                            '</div>' +
                                        '</li>' +
                                        '<li class="item-content">' +
                                            '<div class="item-inner">' +
                                                '<div class="my-title">' + (obj[i]["DIVIDENO"] == null ? "" : obj[i]["DIVIDENO"]) + '</div>' +
                                                '<div class="my-after">' + (obj[i]["REPWAYNAME"] == null ? "" : obj[i]["REPWAYNAME"]) + '</div>' +
                                                '<div class="my-after">' + (obj[i]["CONTRACTNO"] == null ? "" : obj[i]["CONTRACTNO"]) + '</div>' +
                                            '</div>' +
                                        '</li>' +
                                        '<li class="item-content">' +
                                            '<div class="item-inner">' +
                                                '<div class="my-title">' + (obj[i]["GOODSNUM"] == null ? "" : obj[i]["GOODSNUM"]) + '/' + obj[i]["GOODSGW"] + '</div>' +
                                                '<div class="my-after">' + (obj[i]["ISCHECK"] == null ? "" : obj[i]["ISCHECK"]) + '</div>' +
                                                '<div class="my-after">' + (obj[i]["CHECKPIC"] == null ? "" : obj[i]["CHECKPIC"]) + '</div>' +
                                            '</div>' +
                                        '</li>' +
                                        '<li class="item-content">' +
                                            '<div class="item-inner">' +
                                                '<div class="my-title">' + (obj[i]["DECLSTATUS"] == null ? "" : obj[i]["DECLSTATUS"]) + '</div>' +
                                                '<div class="my-after">' + (obj[i]["INSPCHECK"] == null ? "" : obj[i]["INSPCHECK"]) + '</div>' +
                                                '<div class="my-after">' + (obj[i]["LAWFLAG"] == null ? "" : obj[i]["LAWFLAG"]) + '</div>' +
                                            '</div>' +
                                        '</li>' +
                                        '<li class="item-content">' +
                                            '<div class="item-inner">' +
                                                '<div class="my-title">' + (obj[i]["INSPSTATUS"] == null ? "" : obj[i]["INSPSTATUS"]) + '</div>' +
                                                '<div class="my-after">' + (obj[i]["LOGISTICSNAME"] == null ? "" : obj[i]["LOGISTICSNAME"]) + '</div>' +
                                                '<div class="my-after"></div>' +
                                            '</div>' +
                                        '</li>' +
                                    '</ul>' +
                                  '</div>';
                        $("#busicontent").append(str);
                    }
                }
            });
        }
        //自定义长按事件——可用
        //$.fn.longPress = function (fn) {
        //    var timeout = undefined;
        //    var $this = this;
        //    for (var i = 0; i < $this.length; i++) {
        //        $this[i].addEventListener('touchstart', function (event) {
        //            timeout = setTimeout(fn, 800);  //长按时间超过800ms，则执行传入的方法
        //        }, false);
        //        $this[i].addEventListener('touchend', function (event) {
        //            clearTimeout(timeout);  //长按时间少于800ms，不会执行传入的方法
        //        }, false);
        //    }
        //}
       
        //查询
        $(function() {
            $('.infinite-scroll-preloader').hide();
            //FastClick.attach(document.body);
            //查询
            $(document).on('click',
                '#button_one',
                function() {
                    $("#busicontent").html("");
                    //$.showPreloader('加载中...');
                    lastIndex = 0;
                    $('.infinite-scroll-preloader').show();
                    $.attachInfiniteScroll($('.infinite-scroll'));
                    //setTimeout(function() {
                            loadData(itemsPerLoad, lastIndex); //加载数据
                            lastIndex = $('#busicontent .list-block').length; //获取数据条数
                            $.refreshScroller(); //刷新滚动条
                            $('.infinite-scroll-bottom').scrollTop(0); //滚动条置顶

                            if (lastIndex < itemsPerLoad) {
                                $.detachInfiniteScroll($('.infinite-scroll-bottom')); // 加载完毕，则注销无限加载事件，以防不必要的加载     
                                $('.infinite-scroll-preloader').hide();
                                if (lastIndex == 0) {
                                    $.toast("没有符合的数据！");
                                } else {
                                    $.toast("已经加载到最后");
                                }
                            }
                    //    },
                    //    500);
                    //$.hidePreloader();
                })

            //物流状态——按钮切换
            $(document).on('click',
                '.iconfont',
                function() {
                    $("#choudan").hide();
                    $("#zhuanguan").hide();
                    $("#baojian").hide();
                    $("#yunshu").hide();
                    $("#icon_choudan").parent().css('color', '#6D6D72');
                    $("#icon_zhuanguan").parent().css('color', '#6D6D72');
                    $("#icon_baojian").parent().css('color', '#6D6D72');
                    $("#icon_yunshu").parent().css('color', '#6D6D72');
                    $(this).parent().css('color', '#0894EC');
                    var id = $(this).attr("id");
                    id = id.substring(5);
                    $("#" + id).show();
                });
            //无限滚动 注册'infinite'事件处理函数
            $(document).on('infinite',
                "#router1",
                function() {
                    // 如果正在加载，则退出
                    if (loading) return;
                    // 设置flag
                    loading = true;
                    //显示加载栏
                    $('.infinite-scroll-preloader').show();
                    // 模拟1s的加载过程
                    setTimeout(function() {
                            // 重置加载flag
                            loading = false;
                            if (lastIndex >= maxItems || lastIndex % itemsPerLoad != 0) {
                                // 加载完毕，则注销无限加载事件，以防不必要的加载
                                $.detachInfiniteScroll($('.infinite-scroll'));
                                // 删除加载提示符
                                $('.infinite-scroll-preloader').hide();
                                $.toast("已经加载到最后");
                                return;
                            }
                            // 添加新条目
                            loadData(itemsPerLoad, lastIndex);

                            if (lastIndex == $('#busicontent .list-block').length) {
                                $.detachInfiniteScroll($('.infinite-scroll')); // 加载完毕，则注销无限加载事件，以防不必要的加载     
                                $('.infinite-scroll-preloader').hide();

                                $.toast("已经加载到最后");
                                return;
                            }
                            // 更新最后加载的序号
                            lastIndex = $('#busicontent .list-block').length;
                            //容器发生改变,如果是js滚动，需要刷新滚动
                            $.refreshScroller();
                        },
                        500);
                });
            //选中业务变色
            $("#busicontent").on('click',
                '.list-block',
                function(e) { // $("#div_list")也可以换成$(document)，是基于父容器的概念   

                    if ($(this).children("ul").css('background-color') == "rgb(193, 221, 241)") {
                        $(this).children("ul").css('background-color', '#fff');
                    } else {
                        $("#busicontent .list-block ul").css('background-color', '#fff');
                        $(this).children("ul").css('background-color', '#C1DDF1');
                    }
                });


            //功能菜单
            $(document).on('click',
                '.tab-item',
                function(e) {
                    $(".tab-item").removeClass("active");
                    $(this).addClass("active");
                })

            //打开详情弹出框
            $(document).on('click',
                '.open-detail',
                function() {
                    $.showPreloader('加载中...');
                    //清空弹出窗信息
                    $("#pop_tab_decl").html("");
                    $("#pop_tab_insp").html("");
                    $("#pop_tab_logistics").html("");
                    var ordercode = "";
                    $("#busicontent .list-block").each(function() {

                        if ($(this).children("ul").css('background-color') == "rgb(193, 221, 241)") {
                            ordercode = $(this)[0].id;
                            //console.log(ordercode);
                            subCusno = ordercode.split(",")[1];
                            ordercode = ordercode.split(",")[0];
                            //console.log(ordercode);
                            //console.log(subCusno);
                        }
                    });
                    if (ordercode == "") {
                        $.toast("请选择需要调阅的记录");
                        $.hidePreloader();
                        return;
                    }
                    $.ajax({
                        type: 'post',
                        url: 'MyBusiness.aspx/QueryOrderDetail',
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
                        data: "{'code':'" + ordercode + "'}",
                        cache: false,
                        async: false, //默认是true，异步；false为同步，此方法执行完在执行下面代码
                        success: function(data) {
                            var obj = eval("(" + data.d + ")");
                            //1、报关信息
                            var orderTable = obj.OrderTable;
                            var declTable = obj.DeclTable;
                            var inspTable = obj.InspTable;
                            var logisticsTable = obj.LogisticsTable;
                            if (orderTable != null && orderTable.length > 0) {
                                var declstr = '<div class="content-padded grid-demo" >' +
                                    '<div class="row">' +
                                    '<div class="col-20">委托时间</div>' +
                                    '<div class="col-40">' +
                                    (orderTable[0]["SUBMITTIME"] == null ? "" : orderTable[0]["SUBMITTIME"]) +
                                    '</div>' +
                                    '<div class="col-20">' +
                                    (orderTable[0]["SUBMITUSERNAME"] == null ? "" : orderTable[0]["SUBMITUSERNAME"]) +
                                    '</div>' +
                                    '</div>' +
                                    '<div class="row">' +
                                    '<div class="col-20">制单完成</div>' +
                                    '<div class="col-40">' +
                                    (orderTable[0]["MOENDTIME"] == null ? "" : orderTable[0]["MOENDTIME"]) +
                                    '</div>' +
                                    '<div class="col-20">' +
                                    (orderTable[0]["MOENDNAME"] == null ? "" : orderTable[0]["MOENDNAME"]) +
                                    '</div>' +
                                    '</div>' +
                                    '<div class="row">' +
                                    '<div class="col-20">审核完成</div>' +
                                    '<div class="col-40">' +
                                    (orderTable[0]["COENDTIME"] == null ? "" : orderTable[0]["COENDTIME"]) +
                                    '</div>' +
                                    '<div class="col-20">' +
                                    (orderTable[0]["COENDNAME"] == null ? "" : orderTable[0]["COENDNAME"]) +
                                    '</div>' +
                                    '</div>' +
                                    '<div class="row">' +
                                    '<div class="col-20">预录完成</div>' +
                                    '<div class="col-40">' +
                                    (orderTable[0]["PREENDTIME"] == null ? "" : orderTable[0]["PREENDTIME"]) +
                                    '</div>' +
                                    '<div class="col-20">' +
                                    (orderTable[0]["PREENDNAME"] == null ? "" : orderTable[0]["PREENDNAME"]) +
                                    '</div>' +
                                    '</div>' +
                                    '<div class="row">' +
                                    '<div class="col-20">申报完成</div>' +
                                    '<div class="col-40">' +
                                    (orderTable[0]["REPENDTIME"] == null ? "" : orderTable[0]["REPENDTIME"]) +
                                    '</div>' +
                                    '<div class="col-20">' +
                                    (orderTable[0]["REPENDNAME"] == null ? "" : orderTable[0]["REPENDNAME"]) +
                                    '</div>' +
                                    '</div>' +
                                    '<div class="row">' +
                                    '<div class="col-20">现场报关</div>' +
                                    '<div class="col-40">' +
                                    (orderTable[0]["SITEAPPLYTIME"] == null ? "" : orderTable[0]["SITEAPPLYTIME"]) +
                                    '</div>' +
                                    '<div class="col-20">' +
                                    (orderTable[0]["SITEAPPLYUSERNAME"] == null ? "" : orderTable[0]["SITEAPPLYUSERNAME"]) +
                                    '</div>' +
                                    '</div>' +
                                    '<div class="row">' +
                                    '<div class="col-20">查验维护</div>' +
                                    '<div class="col-40">' +
                                    (orderTable[0]["SUBMITTIME"] == null ? "" : orderTable[0]["SUBMITTIME"]) +
                                    '</div>' +
                                    '<div class="col-20">' +
                                    (orderTable[0]["SUBMITUSERNAME"] == null ? "" : orderTable[0]["SUBMITUSERNAME"]) +
                                    '</div>' +
                                    '</div>' +
                                    '<div class="row">' +
                                    '<div class="col-20">现场放行</div>' +
                                    '<div class="col-40">' +
                                    (orderTable[0]["SITEPASSTIME"] == null ? "" : orderTable[0]["SITEPASSTIME"]) +
                                    '</div>' +
                                    '<div class="col-20">' +
                                    (orderTable[0]["SITEPASSUSERNAME"] == null ? "" : orderTable[0]["SITEPASSUSERNAME"]) +
                                    '</div>' +
                                    '</div>' +
                                    '<div class="row">' +
                                    '<div class="col-20">查验图片</div>' +
                                    '<div class="col-40">' +
                                    (orderTable[0]["CHECKPIC"] == null ? "" : orderTable[0]["CHECKPIC"]) +
                                    '</div>' +
                                    '<div class="col-20"></div>' +
                                    '</div>' +
                                    '</div>';
                                declstr +=
                                    '<div style="width:100%;background-color:#DBDBDB;line-height:0.2rem;">&nbsp;</div>';
                                declstr += '<div style=" height: 100%;background-color:#DBDBDB;">';
                                if (declTable != null) {
                                    for (var i = 0; i < declTable.length; i++) {
                                        declstr += '<div class="list-block" id="' +
                                            (declTable[i]["DECLARATIONCODE"] == null ? "" : declTable[i]["DECLARATIONCODE"] == null) +
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
                                    '<div class="col-40">' +
                                    (orderTable[0]["SUBMITTIME"] == null ? "" : orderTable[0]["SUBMITTIME"]) +
                                    '</div>' +
                                    '<div class="col-20">' +
                                    (orderTable[0]["SUBMITUSERNAME"] == null ? "" : orderTable[0]["SUBMITUSERNAME"]) +
                                    '</div>' +
                                    '</div>' +
                                    '<div class="row">' +
                                    '<div class="col-20">制单完成</div>' +
                                    '<div class="col-40">' +
                                    (orderTable[0]["INSPMOENDTIME"] == null ? "" : orderTable[0]["INSPMOENDTIME"]) +
                                    '</div>' +
                                    '<div class="col-20">' +
                                    (orderTable[0]["INSPMOENDNAME"] == null ? "" : orderTable[0]["INSPMOENDNAME"]) +
                                    '</div>' +
                                    '</div>' +
                                    '<div class="row">' +
                                    '<div class="col-20">审核完成</div>' +
                                    '<div class="col-40">' +
                                    (orderTable[0]["INSPCOENDTIME"] == null ? "" : orderTable[0]["INSPCOENDTIME"]) +
                                    '</div>' +
                                    '<div class="col-20">' +
                                    (orderTable[0]["INSPCOENDNAME"] == null ? "" : orderTable[0]["INSPCOENDNAME"]) +
                                    '</div>' +
                                    '</div>' +
                                    '<div class="row">' +
                                    '<div class="col-20">预录完成</div>' +
                                    '<div class="col-40">' +
                                    (orderTable[0]["INSPPREENDTIME"] == null ? "" : orderTable[0]["INSPPREENDTIME"]) +
                                    '</div>' +
                                    '<div class="col-20">' +
                                    (orderTable[0]["INSPPREENDNAME"] == null ? "" : orderTable[0]["INSPPREENDNAME"]) +
                                    '</div>' +
                                    '</div>' +
                                    '<div class="row">' +
                                    '<div class="col-20">申报完成</div>' +
                                    '<div class="col-40">' +
                                    (orderTable[0]["INSPREPENDTIME"] == null ? "" : orderTable[0]["INSPREPENDTIME"]) +
                                    '</div>' +
                                    '<div class="col-20">' +
                                    (orderTable[0]["INSPREPENDNAME"] == null ? "" : orderTable[0]["INSPREPENDNAME"]) +
                                    '</div>' +
                                    '</div>' +
                                    '<div class="row">' +
                                    '<div class="col-20">现场报检</div>' +
                                    '<div class="col-40">' +
                                    (orderTable[0]["INSPSITEAPPLYTIME"] == null ? "" : orderTable[0]["INSPSITEAPPLYTIME"]) +
                                    '</div>' +
                                    '<div class="col-20">' +
                                    (orderTable[0]["INSPSITEAPPLYUSERNAME"] == null ? "" : orderTable[0]["INSPSITEAPPLYUSERNAME"]) +
                                    '</div>' +
                                    '</div>' +
                                    '<div class="row">' +
                                    '<div class="col-20">查验维护</div>' +
                                    '<div class="col-40">' +
                                    (orderTable[0]["INSPSUBMITTIME"] == null ? "" : orderTable[0]["INSPSUBMITTIME"]) +
                                    '</div>' +
                                    '<div class="col-20">' +
                                    (orderTable[0]["INSPSUBMITUSERNAME"] == null ? "" : orderTable[0]["INSPSUBMITUSERNAME"]) +
                                    '</div>' +
                                    '</div>' +
                                    '<div class="row">' +
                                    '<div class="col-20">报检放行</div>' +
                                    '<div class="col-40">' +
                                    (orderTable[0]["INSPSITEPASSTIME"] == null ? "" : orderTable[0]["INSPSITEPASSTIME"]) +
                                    '</div>' +
                                    '<div class="col-20">' +
                                    (orderTable[0]["INSPSITEPASSUSERNAME"] == null ? "" : orderTable[0]["INSPSITEPASSUSERNAME"]) +
                                    '</div>' +
                                    '</div>' +
                                    '<div class="row">' +
                                    '<div class="col-20">查验图片</div>' +
                                    '<div class="col-40">' +
                                    (orderTable[0]["INSPCHECKPIC"] == null ? "" : orderTable[0]["INSPCHECKPIC"]) +
                                    '</div>' +
                                    '<div class="col-20"></div>' +
                                    '</div>' +
                                    '</div>';
                                inspstr +=
                                    '<div style="width:100%;background-color:#DBDBDB;line-height:0.2rem;">&nbsp;</div>';
                                inspstr += '<div style=" height: 100%;background-color:#DBDBDB;">';
                                if (inspTable != null) {
                                    for (var i = 0; i < inspTable.length; i++) {
                                        inspstr += '<div class="list-block">';
                                        inspstr += '<div class="row">';
                                        inspstr += '<div class="col-40">' + (inspTable[i]["APPROVALCODE"] == null ? "" : inspTable[i]["APPROVALCODE"]) + '</div>';
                                        inspstr += '<div class="col-40">' + (inspTable[i]["INSPSTATUS"] == null ? "" : inspTable[i]["INSPSTATUS"]) + '</div>';
                                        inspstr += '<div class="col-20">' + (inspTable[i]["MODIFYFLAG"] == null ? "" : inspTable[i]["MODIFYFLAG"]) + '</div>';
                                        inspstr += '</div>';
                                        inspstr += '<div class="row">';
                                        inspstr += '<div class="col-40">' + (inspTable[i]["INSPECTIONCODE"] == null ? "" : inspTable[i]["INSPECTIONCODE"]) + '</div>';
                                        inspstr += '<div class="col-40">' + (inspTable[i]["CLEARANCECODE"] == null ? "" : inspTable[i]["CLEARANCECODE"]) + '</div>';
                                        inspstr += '<div class="col-20"></div>';
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
                                    '<div class="col-40">时间</div>' +
                                    '<div class="col-25">操作人</div>' +
                                    '<div class="col-35">状态值</div>' +
                                    '</div>'
                                var choudan = '<div id="choudan">';
                                var zhuanguan = '<div id="zhuanguan" style="display:none">';
                                var baojian = '<div id="baojian" style="display:none">';
                                var yunshu = '<div id="yunshu" style="display:none">';
                                for (var i = 0; i < logisticsTable.length; i++) {
                                    if (logisticsTable[i]["OPERATE_TYPE"] == "抽单状态") {
                                        choudan += '<div class="row">' +
                                            '<div class="col-40">' +
                                            (logisticsTable[i]["OPERATE_DATE"] == null ? "" : logisticsTable[i]["OPERATE_DATE"]) +
                                            '</div>' +
                                            '<div class="col-25">' +
                                            (logisticsTable[i]["OPERATER"] == null ? "" : logisticsTable[i]["OPERATER"]) +
                                            '</div>' +
                                            '<div class="col-35">' +
                                            (logisticsTable[i]["OPERATE_RESULT"] == null ? "" : logisticsTable[i]["OPERATE_RESULT"]) +
                                            '</div>' +
                                            '</div>';
                                    } else if (logisticsTable[i]["OPERATE_TYPE"] == "报关申报状态" ||
                                        logisticsTable[i]["OPERATE_TYPE"] == "转关申报状态") {
                                        zhuanguan += '<div class="row">' +
                                            '<div class="col-40">' +
                                            (logisticsTable[i]["OPERATE_DATE"] == null ? "" : logisticsTable[i]["OPERATE_DATE"]) +
                                            '</div>' +
                                            '<div class="col-25">' +
                                            (logisticsTable[i]["OPERATER"] == null ? "" : logisticsTable[i]["OPERATER"]) +
                                            '</div>' +
                                            '<div class="col-35">' +
                                            (logisticsTable[i]["OPERATE_RESULT"] == null ? "" : logisticsTable[i]["OPERATE_RESULT"]) +
                                            '</div>' +
                                            '</div>';
                                    } else if (logisticsTable[i]["OPERATE_TYPE"] == "商检状态") {
                                        baojian += '<div class="row">' +
                                            '<div class="col-40">' +
                                            (logisticsTable[i]["OPERATE_DATE"] == null ? "" : logisticsTable[i]["OPERATE_DATE"]) +
                                            '</div>' +
                                            '<div class="col-25">' +
                                            (logisticsTable[i]["OPERATER"] == null ? "" : logisticsTable[i]["OPERATER"]) +
                                            '</div>' +
                                            '<div class="col-35">' +
                                            (logisticsTable[i]["OPERATE_RESULT"] == null ? "" : logisticsTable[i]["OPERATE_RESULT"]) +
                                            '</div>' +
                                            '</div>';
                                    } else if (logisticsTable[i]["OPERATE_TYPE"] == "运输状态") {
                                        yunshu += '<div class="row">' +
                                            '<div class="col-40">' +
                                            (logisticsTable[i]["OPERATE_DATE"] == null ? "" : logisticsTable[i]["OPERATE_DATE"]) +
                                            '</div>' +
                                            '<div class="col-25">' +
                                            (logisticsTable[i]["OPERATER"] == null ? "" : logisticsTable[i]["OPERATER"]) +
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


                });
            //选中报关单变色
            $("#pop_tab_decl").on('click',
                '.list-block',
                function(e) {

                    if ($(this).css('background-color') == "rgb(193, 221, 241)") {
                        $(this).css('background-color', '#fff');
                    } else {
                        $("#pop_tab_decl .list-block ").css('background-color', '#fff');
                        $(this).css('background-color', '#C1DDF1');
                    }
                });
            $.init();

        });
        //长按事件
        //$('#pop_tab_decl .list-block').longPress(function (e) {
        //});
        //打开业务订阅弹出框
        $(document).on('click',
            '.open-subscribe',
            function() {
                var cusno = "";
                $("#busicontent .list-block").each(function() {

                    if ($(this).children("ul").css('background-color') == "rgb(193, 221, 241)") {
                        cusno = $(this)[0].id;
                        cusno = cusno.split(",")[1];
                    }
                });
                if (cusno == "") {
                    $.toast("请选择需要订阅的记录");
                    return;
                }
                subCusno = cusno;
                $.popup("#popup-subscribe-log");
            });
        //打开预制单订阅弹出框
        $(document).on('click',
            '#btn-subs-decl',
            function() {
                var declcode = "";
                $("#pop_tab_decl .list-block").each(function() {

                    if ($(this).css('background-color') == "rgb(193, 221, 241)") {
                        console.log($(this));
                        declcode = $(this)[0].id;
                        //console.log(declcode);
                    }
                });
                if (declcode == "") {
                    $.toast("请选择需要订阅的记录");
                    return;
                }
                subDeclarationCode = declcode;
                $.popup("#popup-subscribe-decl");
            });
        //订阅消息
        function subscribe(type) {
            var status = "";
            var input;
            if (type == "订单状态")
            {
                if (subCusno == "") {
                    $.toast("请选择需要订阅的业务");
                    return;
                }
                else
                {
                    var tab = $(".popup-subscribe .tab").css("display");
                    if (tab == "block")
                    {
                        type = "业务状态";
                        input = $("#pop_sub_order input");
                    }
                    else
                    {
                        type = "物流状态";
                        input = $("#pop_sub_log input");
                    }
                }
            }
            if (type == "报关状态")
            {
                if (subDeclarationCode == "") {
                    $.toast("请选择需要订阅的报关单");
                    return;
                }
                else {
                    input = $("#pop_sub_decl input");
                }
            }
            for (var i = 0; i < input.length; i++) {
                if (input[i].checked) {
                    status += input[i].value + ",";
                }
            }
            if (status == "")
            {
                $.toast("请选择需要订阅的状态");
                return;
            }
            $.ajax({
                type: 'post',
                url: 'MyBusiness.aspx/SubscribeStatus',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                data: "{'type':'" +
                    type +
                    "','status':'" +
                    status +
                    "','cusno':'" +
                    subCusno +
                    "','declarationcode':'" +
                    subDeclarationCode +
                    "','userid':'" +
                    userid +
                    "','username':'" +
                    username +
                    "','openid':'" +
                    openid +
                    "'}",
                cache: false,
                async: false, //默认是true，异步；false为同步，此方法执行完在执行下面代码
                success: function(data) {
                    $.toast(data.d);
                }
            });
        }

        //返回后Checked置空
        $(document).on('click', '#backto', function () {
            var input = $('.popup input');
            for (var i = 0; i < input.length; i++) {
                if (input[i].checked) {
                    input[i].checked = false;
                }
            }
            $("[href='#sub_tab1']").addClass("active"); $("#sub_tab1").addClass("active");
            $("[href='#sub_tab2']").removeClass("active"); $("#sub_tab2").removeClass("active");
        });

        //报关单调阅
        function showDeclPdf() {
            var ordercode = "";
            $("#busicontent .list-block").each(function () {

                if ($(this).children("ul").css('background-color') == "rgb(193, 221, 241)") {

                    ordercode = $(this)[0].id;
                    //yangyang.zhao
                    ordercode = ordercode.split(",")[0];
                    //console.log(ordercode);
                }
            });
            if (ordercode == "") {
                $.toast("请选择需要调阅的记录");
                return;
            }
            $.ajax({
                type: 'post',
                url: 'MyBusiness.aspx/GetDeclPdf',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                data: "{'orderCode':'" + ordercode + "'}",
                cache: false,
                async: false,//默认是true，异步；false为同步，此方法执行完在执行下面代码
                success: function (data) {
                    if (data.d == "") {
                        $.toast("报关单文件不存在");
                        return;
                    }

                    var picstr = data.d;
                    var picarr = picstr.split(',');

                    var imgs = "[";
                    for (var i = 0; i < picarr.length; i++) {
                        if (picarr[i] != "") { imgs += "'/TempFile/tempPic/" + picarr[i] + "',"; }
                    }
                    imgs += "]";

                    var myPhotoBrowserStandalone = $.photoBrowser({
                        photos: eval(imgs)
                    });
                    myPhotoBrowserStandalone.open();
                }
            })
            //$.popup(".popup-declpdf");
        }
        //查验图片调阅
        function showCheckPic() {
            var ordercode = "";
            $("#busicontent .list-block").each(function () {

                if ($(this).children("ul").css('background-color') == "rgb(193, 221, 241)") {
                    ordercode = $(this)[0].id;
                    ordercode = ordercode.split(",")[0];
                }
            });
            if (ordercode == "") {
                $.toast("请选择需要调阅的记录");
                return;
            }

            $.ajax({
                type: "post", //要用post方式                 
                url: "/page/BusiOpera/SiteDeclareList.aspx/picfileconsult",//方法所在页面和方法名
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                data: "{'ordercode':'" + ordercode + "'}",
                cache: false,
                async: false,//默认是true，异步；false为同步，此方法执行完在执行下面代码
                success: function (data) {
                    var obj = eval("(" + data.d + ")");//将字符串转为json
                    var imgs = "[";
                    for (var i = 0; i < obj.length; i++) {
                        imgs += "'" + $("#hd_AdminUrl").val() + "file/" + obj[i]["FILENAME"] + "',";
                    }
                    imgs += "]";

                    var myPhotoBrowserStandalone = $.photoBrowser({
                        photos: eval(imgs)
                    });
                    myPhotoBrowserStandalone.open();
                }
            });
        }

        function reset() {
            //报关状态重置
            $("#picker_declstatus").remove();
            $("#picker_declstatus1").append("<input type='search' id='picker_declstatus' placeholder='报关状态'/>");
            util.picker_declstatus();
            //报检状态重置
            $("#picker_inspstatus").remove();
            $("#picker_inspstatus1").append("<input type='search' id='picker_inspstatus' placeholder='报检状态'/>");
            util.picker_inspstatus();
            //进口出口重置
            $("#picker_inout").remove();
            $("#picker_inout1").append("<input type='search' id='picker_inout' placeholder='进口出口'/>");
            util.picker_inout();
            //业务类型重置
            $("#picker_busitype").remove();
            $("#picker_busitype1").append("<input type='search' id='picker_busitype' placeholder='业务类型'/>");
            util.picker_busitype();
            //申报现场（改为输入框）重置
            $("#picker_customs").val("");
            //$("#picker_customs").remove();
            //$("#picker_customs1").append("<input type='search' id='picker_customs' placeholder='申报现场'/>");
            //util.picker_customs();
            //现场报关重置
            $("#picker_sitedeclare").remove();
            $("#picker_sitedeclare1").append("<input type='search' id='picker_sitedeclare' placeholder='现场报关'/>");
            util.picker_sitedeclare();
            //物流状态重置
            $("#picker_logisticsstatus").remove();
            $("#picker_logisticsstatus1").append("<input type='search' id='picker_logisticsstatus' placeholder='物流状态'/>");
            util.picker_logisticsstatus();
            //开始时间重置
            $("#txt_startdate").remove();
            $("#txt_startdate1").append("<input type='search' id='txt_startdate' placeholder='委托起始日期'/>");
            $("#txt_startdate").calendar({});
            //结束时间重置
            $("#txt_enddate").remove();
            $("#txt_enddate1").append("<input type='search' id='txt_enddate' placeholder='委托结束日期'/>");
            $("#txt_enddate").calendar({});


            console.log($("#picker_declstatus"));
            
            //window.location.reload();
        }


    </script>
</head>
<body>

<form id="form1" runat="server">

    <div class="page-group">
        <!-- page1 消息订阅-->
        <div class="page page-current"  id="router1">
            
            <!-- 标题栏 -->
            <%--<header class="bar bar-nav">
                <a  style=" width:4rem;" class="icon icon-search pull-right open-panel">查询</a>
                <h1 class="title">我的业务</h1>
            </header>--%>
            
            
            <header class="bar bar-nav">
                
                <div class="search-input">
                    <div class="row">
                        <div class="col-25" id="picker_declstatus1"><input type="search" id='picker_declstatus' placeholder='报关状态'/></div> 
                        <div class="col-25" id="picker_inspstatus1"><input type="search" id='picker_inspstatus' placeholder='报检状态'/></div>
                        <div class="col-25" id="picker_inout1"><input type="search" id='picker_inout' placeholder='进口出口'/></div> 
                        <div class="col-25" id="picker_busitype1"><input type="search" id='picker_busitype' placeholder='业务类型'/></div>
                    </div> 
                    <div class="row">
                        <div class="col-33" id="picker_customs1"><input type="text" id='picker_customs' placeholder='申报现场'/></div>
                        <div class="col-33" id="picker_sitedeclare1"><input type="search" id='picker_sitedeclare' placeholder='现场报关'/></div> 
                        <div class="col-33" id="picker_logisticsstatus1"><input type="search" id='picker_logisticsstatus' placeholder='物流状态'/></div>
                    </div>
                    <div class="row">
                        <div class="col-40" id="txt_startdate1"><input type="search" id='txt_startdate' placeholder='委托起始日期'/></div>
                        <div class="col-5">~</div>
                        <div class="col-40" id="txt_enddate1"><input type="search" id='txt_enddate' placeholder='委托结束日期'/></div>
                        <div class="col-15"><i class="iconfont" style="font-size:1.2rem;" onclick="reset()" >&#xe604;</i></div>
                    </div>
                    <a href="#" class="open-preloader-title button  button-fill" id="button_one">查&nbsp;&nbsp;询</a>
                </div>
                    
            </header>
            
            

            <!-- 工具栏 -->
            <nav class="bar bar-tab">
                <a class="tab-item open-detail" href="#">
                    <span class="icon icon-menu"></span>
                    <span class="tab-label">业务详情</span>
                </a>
                <a class="tab-item  open-subscribe" href="#">
                    <span class="icon icon-edit"></span>
                    <span class="tab-label">消息订阅</span>
                </a>
                <a class="tab-item open-declpdf"  href="javascript:showDeclPdf()">
                    <span class="icon icon-star" ></span>
                    <span class="tab-label">报关单调阅</span>
                </a>
                <a class="tab-item "  href="javascript:showCheckPic()"> 
                    <span class="icon icon-picture"></span>
                    <span class="tab-label">查验图片调阅</span>
                </a>
                <input type="hidden" id="hd_AdminUrl" value='<%= System.Configuration.ConfigurationManager.AppSettings["AdminUrl"] %>' />
                <%--<a class="tab-item " href="SubscribeList.aspx">
                    <span class="icon icon-menu"></span>
                    <span class="tab-label">订阅清单</span>
                </a>--%>                
            </nav>
            
            

            <!-- 这里是页面内容区 -->
            <div class="content infinite-scroll infinite-scroll-bottom" data-distance="100" id="scroll-bottom-one">
                <div id ="busicontent"></div>
                <!-- 加载提示符 -->
                <div class="infinite-scroll-preloader"> 
                    <div class="preloader"></div>
                </div>
                
            </div>
        </div>
    </div>
    <!-- popup, 右侧弹出的查询条件 -->
    <div class="panel-overlay"></div>
    <!-- Left Panel with Reveal effect -->
    <div style="background-color:#2C2C37" class="panel panel-right panel-reveal">
        <div class="content-block">
            <div class="content-padded">
                <div class="row">
                    <div class="col-95"><span class="lab">报关状态：</span><input type="text" id='picker_declstatus'/></div>
                </div>
                <div class="row">
                    <div class="col-95"><span class="lab">报检状态：</span><input type="text" id='picker_inspstatus'/></div>                    
                </div>
                <div class="row">
                    <div class="col-95"><span class="lab">进口出口：</span><input type="text" id='picker_inout'/></div>
                </div>
                <div class="row">
                    <div class="col-95"><span class="lab">业务类型：</span><input type="text" id='picker_busitype'/></div>                    
                </div>
                <div class="row">
                    <div class="col-95"><span class="lab">申报现场：</span><input type="text" id='picker_customs'/></div>
                </div>
                <div class="row">
                    <div class="col-95"><span class="lab">现场报关：</span><input type="text" id='picker_sitedeclare'/></div>                    
                </div>
                <div class="row">
                    <div class="col-95"><span class="lab">物流状态：</span><input type="text" id='picker_logisticsstatus'/></div>
                </div>
                <div class="row">
                    <div class="col-95"><span class="lab">委托时间：</span><input type="text"  id='picker_starttime'/></div>
                </div>
                <div class="row">
                    <div class="col-95"><span class="lab">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;至：</span><input type="text"  id='picker_endtime'/></div>
                </div>
            </div>
            <p><a href="#" class="open-preloader-title button  button-fill" id="button_one">查&nbsp;&nbsp;询</a></p>
            <%--<p><a href="#" class="button button-round">重&nbsp;&nbsp;&nbsp;&nbsp;置</a></p>--%>
        </div>
    </div>

    <!--popup 详情弹出页-->
    <div class="popup popup-detail" >
        <div class="content" style="bottom:60px;">
          <div class="buttons-tab">
            <a href="#tab1" class="tab-link active button">报关信息</a>
            <a href="#tab2" class="tab-link button">报检信息</a>
            <a href="#tab3" class="tab-link button">物流信息</a>
          </div>
          <div class="content-block">
            <div class="tabs">
              <div id="tab1" class="tab active">
                <div class="content-block " id="pop_tab_decl"></div>
                <div style="margin-left:25%;width:50%;" ><a href="#" id="btn-subs-decl" style="font-size:30px;" class="button button-success button-fill">报关订阅</a></div>
              </div>
              <div id="tab2" class="tab">
                <div class="content-block" id="pop_tab_insp"></div>
              </div>
              <div id="tab3" class="tab">
                <div class="content-block" id="pop_tab_logistics"></div>
              </div>
            </div>
          </div>
        </div>
       <div style="bottom:0.2rem; position:absolute;width:96%;margin-left:2%"><a href="#" class="close-popup button button-fill">返  回</a></div>
    </div>
    <!--popup 订阅弹出页-->
    <div class="popup popup-subscribe" id="popup-subscribe-decl" >
        <div class="content" >
          <div class="content-block" id="pop_sub_decl">
                    <div class="myrow">报关状态订阅</div>
                    <div class="row">
                        <div class="col-66">申报完成</div>
                        <div class="col-33"><input type="checkbox" value="申报完成"/></div>
                    </div>
                    <div class="row">
                        <div class="col-66">已放行</div>
                        <div class="col-33"><input type="checkbox" value="已放行"/></div>
                    </div>
                    <div class="row">
                        <div class="col-66">已结关</div>
                        <div class="col-33"><input type="checkbox" value="已结关"/></div>
                    </div>
                    <div class="row">
                        <div class="col-66">改单完成</div>
                        <div class="col-33"><input type="checkbox" value="改单完成"/></div>
                    </div>
                    <div class="row">
                        <div class="col-66">删单完成</div>
                        <div class="col-33"><input type="checkbox" value="删单完成"/></div>
                    </div>
                </div>
            <div class="row" style="background-color:white;margin-top:1rem">
                <div class="col-50"><a href="javascript:subscribe('报关状态')" class="button button-fill">确  认</a></div>
                <div class="col-50"><a href="#" class="close-popup button button-fill" id="backto">返  回</a></div>
            </div>
        </div>
    </div>
    <div class="popup popup-subscribe" id="popup-subscribe-log" >
        <div class="content" >
          <div class="buttons-tab">
            <a href="#sub_tab1" class="tab-link active button">业务状态</a>
            <a href="#sub_tab2" class="tab-link button">物流状态</a>
          </div>
          <div class="content-block">
            <div class="tabs">
              <div id="sub_tab1" class="tab active">
                <div class="content-block" id="pop_sub_order">
                    <div class="myrow">业务状态订阅</div>
                    <div class="row">
                        <div class="col-66">订单受理</div>
                        <div class="col-33"><input type="checkbox" value="订单受理"/></div>
                    </div>
                    <div class="row">
                        <div class="col-66">申报开始</div>
                        <div class="col-33"><input type="checkbox" value="申报开始"/></div>
                    </div>
                    <div class="row">
                        <div class="col-66">提前申报完成</div>
                        <div class="col-33"><input type="checkbox" value="提前申报完成"/></div>
                    </div>
                    <div class="row">
                        <div class="col-66">申报完成</div>
                        <div class="col-33"><input type="checkbox" value="申报完成"/></div>
                    </div>
                    <div class="row">
                        <div class="col-66">现场报关</div>
                        <div class="col-33"><input type="checkbox" value="现场报关"/></div>
                    </div>
                    <div class="row">
                        <div class="col-66">现场放行</div>
                        <div class="col-33"><input type="checkbox" value="现场放行"/></div>
                    </div>
                </div>
              </div>
              <div id="sub_tab2" class="tab">
                <div class="content-block" id="pop_sub_log">
                    <div class="myrow">物流状态订阅</div>
                    <div class="row">
                        <div class="col-66">抽单完成</div>
                        <div class="col-33"><input type="checkbox" value="抽单完成"/></div>
                    </div>
                    <div class="row">
                        <div class="col-66">已派车</div>
                        <div class="col-33"><input type="checkbox" value="已派车"/></div>
                    </div>
                    <div class="row">
                        <div class="col-66">运输完成</div>
                        <div class="col-33"><input type="checkbox" value="运输完成"/></div>
                    </div>
                    <div class="row">
                        <div class="col-66">送货完成</div>
                        <div class="col-33"><input type="checkbox" value="送货完成"/></div>
                    </div>
                </div>
              </div>
            </div>
          </div>
            <div class="row" style="background-color:white;margin-top:1rem">
                <div class="col-50"><a href="javascript:subscribe('订单状态')" class="button button-fill">确  认</a></div>
                <div class="col-50"><a href="#" class="close-popup button button-fill" id="backto">返  回</a></div>
            </div>
        </div>
    </div>   


    <%--<div class="popup popup-subscribe" >
        <div class="content" >
          <div class="buttons-tab">
            <a href="#sub_tab1" class="tab-link active button">报关状态</a>
            <a href="#sub_tab2" class="tab-link button">物流状态</a>
          </div>
          <div class="content-block">
            <div class="tabs">
              <div id="sub_tab1" class="tab active">
                <div class="content-block" id="pop_sub_decl">
                    <div class="myrow">报关状态订阅</div>
                    <div class="row">
                        <div class="col-66">申报完成</div>
                        <div class="col-33"><input type="checkbox" value="申报完成"/></div>
                    </div>
                    <div class="row">
                        <div class="col-66">已放行</div>
                        <div class="col-33"><input type="checkbox" value="已放行"/></div>
                    </div>
                    <div class="row">
                        <div class="col-66">已结关</div>
                        <div class="col-33"><input type="checkbox" value="已结关"/></div>
                    </div>
                    <div class="row">
                        <div class="col-66">改单完成</div>
                        <div class="col-33"><input type="checkbox" value="改单完成"/></div>
                    </div>
                    <div class="row">
                        <div class="col-66">删单完成</div>
                        <div class="col-33"><input type="checkbox" value="删单完成"/></div>
                    </div>
                </div>
              </div>
              <div id="sub_tab2" class="tab">
                <div class="content-block" id="pop_sub_log">
                    <div class="myrow">物流状态订阅</div>
                    <div class="row">
                        <div class="col-66">抽单完成</div>
                        <div class="col-33"><input type="checkbox" value="抽单完成"/></div>
                    </div>
                    <div class="row">
                        <div class="col-66">已派车</div>
                        <div class="col-33"><input type="checkbox" value="已派车"/></div>
                    </div>
                    <div class="row">
                        <div class="col-66">运输完成</div>
                        <div class="col-33"><input type="checkbox" value="运输完成"/></div>
                    </div>
                    <div class="row">
                        <div class="col-66">送货完成</div>
                        <div class="col-33"><input type="checkbox" value="送货完成"/></div>
                    </div>
                </div>
              </div>
            </div>
          </div>
          <div class="content-block">
            <div class="row" style="background-color:white">
                <div class="col-50"><a href="javascript:subscribe()" class="button button-fill">确  认</a></div>
                <div class="col-50"><a href="#" class="close-popup button button-fill">返  回</a></div>
            </div>
              </div>
        </div>
    </div>--%>
   

    <!-- 默认必须要执行$.init(),实际业务里一般不会在HTML文档里执行，通常是在业务页面代码的最后执行 -->
    
    <script type='text/javascript' src='//g.alicdn.com/msui/sm/0.6.2/js/sm.min.js' charset='utf-8'></script>
     <script src="/js/sm-extend.min.js"></script>
    <script type="text/javascript">
        //初始化控件的值
        var myDate = new Date();
        var nowDate = myDate.toLocaleDateString();
        $("#picker_starttime").val("2017/03/04");
        $("#picker_endtime").val(nowDate);        
        //创建picker
        
        var util = {
            picker_declstatus: function() {
                $("#picker_declstatus").picker({
                    toolbarTemplate: '<header class="bar bar-nav">\
  <button class="button button-link pull-right close-picker">确定</button>\
  <h1 class="title">请选择</h1>\
  </header>',
                    cols: [
                        {
                            textAlign: 'center',
                            values: ['全部', '申报完结', '现场申报', '现场放行']
                        }
                    ]

                });
            },
            picker_inspstatus: function() {
                $("#picker_inspstatus").picker({
                    toolbarTemplate: '<header class="bar bar-nav">\
  <button class="button button-link pull-right close-picker">确定</button>\
  <h1 class="title">请选择</h1>\
  </header>',
                    cols: [
                        {
                            textAlign: 'center',
                            values: ['全部', '申报完结', '现场报检', '现场放行']
                        }
                    ]
                });
            },
            picker_inout: function() {
                $("#picker_inout").picker({
                    toolbarTemplate: '<header class="bar bar-nav">\
  <button class="button button-link pull-right close-picker">确定</button>\
  <h1 class="title">请选择</h1>\
  </header>',
                    cols: [
                        {
                            textAlign: 'center',
                            values: ['全部', '进口', '出口']
                        }
                    ]
                });
            },
            picker_busitype: function() {
                $("#picker_busitype").picker({
                    toolbarTemplate: '<header class="bar bar-nav">\
  <button class="button button-link pull-right close-picker">确定</button>\
  <h1 class="title">请选择</h1>\
  </header>',
                    cols: [
                        {
                            textAlign: 'center',
                            values: ['全部（不含国内）', '国内业务', '特殊区域', '空运业务', '陆运业务', '海运业务']
                        }
                    ]
                });
            },
            picker_customs: function() {
                $("#picker_customs").picker({
                    toolbarTemplate: '<header class="bar bar-nav">\
  <button class="button button-link pull-right close-picker">确定</button>\
  <h1 class="title">请选择</h1>\
  </header>',
                    cols: [
                        {
                            textAlign: 'center',
                            values: ['全部', '2369']
                        }
                    ]
                });
            },
            picker_sitedeclare: function() {
                $("#picker_sitedeclare").picker({
                    toolbarTemplate: '<header class="bar bar-nav">\
  <button class="button button-link pull-right close-picker">确定</button>\
  <h1 class="title">请选择</h1>\
  </header>',
                    cols: [
                        {
                            textAlign: 'center',
                            values: ['全部', '需现场申报']
                        }
                    ]
                });
            },
            picker_logisticsstatus: function() {
                $("#picker_logisticsstatus").picker({
                    toolbarTemplate: '<header class="bar bar-nav">\
  <button class="button button-link pull-right close-picker">确定</button>\
  <h1 class="title">请选择</h1>\
  </header>',
                    cols: [
                        {
                            textAlign: 'center',
                            values: ['全部', '待抽单', '抽单完成', '未派车', '已派车', '未运抵', '运输完成', '未送货', '送货完成']
                        }
                    ]
                });
            }

        }
        util.picker_declstatus();
        util.picker_inspstatus();
        util.picker_inout();
        util.picker_busitype();
        //申报现场改为输入框
        //util.picker_customs();
        util.picker_sitedeclare();
        util.picker_logisticsstatus();
        
  //      $("#picker_declstatus").picker({
  //          toolbarTemplate: '<header class="bar bar-nav">\
  //<button class="button button-link pull-right close-picker">确定</button>\
  //<h1 class="title">请选择</h1>\
  //</header>', 
  //          cols: [
  //            {
  //                textAlign: 'center',
  //                values: ['全部', '申报完结', '现场申报', '现场放行']
  //            }
  //          ]
            
  //      });
  //      $("#picker_inspstatus").picker({
  //          toolbarTemplate: '<header class="bar bar-nav">\
  //<button class="button button-link pull-right close-picker">确定</button>\
  //<h1 class="title">请选择</h1>\
  //</header>',
  //          cols: [
  //            {
  //                textAlign: 'center',
  //                values: ['全部', '申报完结', '现场报检','现场放行']
  //            }
  //          ]
  //      });
  //      $("#picker_inout").picker({
  //          toolbarTemplate: '<header class="bar bar-nav">\
  //<button class="button button-link pull-right close-picker">确定</button>\
  //<h1 class="title">请选择</h1>\
  //</header>',
  //          cols: [
  //            {
  //                textAlign: 'center',
  //                values: ['全部', '进口', '出口']
  //            }
  //          ]
  //      });
  //      $("#picker_busitype").picker({
  //          toolbarTemplate: '<header class="bar bar-nav">\
  //<button class="button button-link pull-right close-picker">确定</button>\
  //<h1 class="title">请选择</h1>\
  //</header>',
  //          cols: [
  //            {
  //                textAlign: 'center',
  //                values: ['全部（不含国内）', '国内业务', '特殊区域','空运业务','陆运业务','海运业务']
  //            }
  //          ]
  //      });
  //      $("#picker_customs").picker({
  //          toolbarTemplate: '<header class="bar bar-nav">\
  //<button class="button button-link pull-right close-picker">确定</button>\
  //<h1 class="title">请选择</h1>\
  //</header>',
  //          cols: [
  //            {
  //                textAlign: 'center',
  //                values: ['全部', '2369']
  //            }
  //          ]
  //      });
  //      $("#picker_sitedeclare").picker({
  //          toolbarTemplate: '<header class="bar bar-nav">\
  //<button class="button button-link pull-right close-picker">确定</button>\
  //<h1 class="title">请选择</h1>\
  //</header>',
  //          cols: [
  //            {
  //                textAlign: 'center',
  //                values: ['全部', '需现场申报']
  //            }
  //          ]
  //      });
  //      $("#picker_logisticsstatus").picker({
  //          toolbarTemplate: '<header class="bar bar-nav">\
  //<button class="button button-link pull-right close-picker">确定</button>\
  //<h1 class="title">请选择</h1>\
  //</header>',
  //          cols: [
  //            {
  //                textAlign: 'center',
  //                values: ['全部', '待抽单', '抽单完成', '未派车', '已派车', '未运抵', '运输完成', '未送货', '送货完成']
  //            }
  //          ]
  //      });
        
        $("#picker_starttime").calendar({
            value: [nowDate],
            dateFormat:'yyyy/mm/dd'
        });
        $("#picker_endtime").calendar({
            value: [nowDate],
            dateFormat: 'yyyy/mm/dd'
        });

        $("#txt_startdate").calendar({});
        $("#txt_enddate").calendar({});
       
    </script>
</form>
</body>
   

</html>
