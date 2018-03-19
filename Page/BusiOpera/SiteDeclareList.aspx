<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SiteDeclareList.aspx.cs" Inherits="WeChat.Page.BusiOpera.SiteDeclareList" %>
<%@ Import Namespace="System.Configuration" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<%--<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>--%>
    <meta charset="utf-8">
    <meta name="viewport" content="initial-scale=1, maximum-scale=1">
    <title>现场报关</title>
    <link href="/css/iconfont/iconfont.css?t=<%=ConfigurationManager.AppSettings["Version"]%>" rel="stylesheet" />
    <link rel="stylesheet" href="//g.alicdn.com/msui/sm/0.6.2/css/sm.min.css">
    <link rel="stylesheet" href="//g.alicdn.com/msui/sm/0.6.2/css/??sm.min.css,sm-extend.min.css">
    <script type='text/javascript' src='//g.alicdn.com/sj/lib/zepto/zepto.min.js' charset='utf-8'></script>

    <link rel="stylesheet" href="/css/extraSearch.css?t=<%=ConfigurationManager.AppSettings["Version"]%>" />    
    <link rel="stylesheet" href="/css/SubscribeInfo.css?t=<%=ConfigurationManager.AppSettings["Version"]%>" />
    <script type="text/javascript" src="/js/extraSearch.js?t=<%=ConfigurationManager.AppSettings["Version"]%>" ></script>
    <script type="text/javascript" src="/js/SubscribeInfo.js?t=<%=ConfigurationManager.AppSettings["Version"]%>"></script>
    <style>
        #page-infinite-scroll-bottom .bar input[type=search]{
             margin:.2rem 0;
        }
        #page-infinite-scroll-bottom .bar .button {
            top:0;
        }
        #page-infinite-scroll-bottom .bar-nav~.content{
            top: 6.2rem;
        }
        #page-infinite-scroll-bottom .search-input input{
            border-radius:0;font-size:13px;
        }
        #div_list .list-block{
            font-size:13px;
            margin:.2rem 0;
        }
        #div_list .list-block .item-content{
            height:1.3rem;
            min-height:1.3rem;
        }
        #div_list .list-block .item-inner{
            height:1.3rem;
            min-height:1.3rem;
        }
        .picker-items-col.picker-items-col-center{
             width: 100%;
        }
        .picker-items-col-wrapper
        {
           width: 100%;
        }
         /************************************************ *********************************/
         .sitediv .modal-inner{
             padding:0;
         }
        .sitediv .modal-title{
            text-align:right;
        }
        .sitediv .modal-title+.modal-text{
            margin-top:0;
        }

        .picdiv .modal-inner{
             padding:0;
         }
        .picdiv .modal-title{
            text-align:right;
        }
        .picdiv .modal-title+.modal-text{
            margin-top:0;
        }
        /************************************************ 查询列表名称*********************************/
        .girdnamediv {
            width: 98%;
            left: 1%;
            right: 1%;
            margin-left: 0px;
            text-align: left;
            top: 3.2rem;
        } 
        .girdnamediv .modal-inner{
           padding:0px;
        }  

         /************************************************ 报关订阅*********************************/
        #popup-subscribe-log_con
        {
            background-color:#1D2E3C;
            color:white;
            margin-top:1rem;
        }
        #popup-subscribe-log_con .row
        {
            background-color:#456581;
            border-top:solid 1px white;
            margin:0rem;
            font-size:initial;
            vertical-align:middle;
            height:2rem;
            line-height:2rem;
        }
        #popup-subscribe-log_con .col-33
        {
            padding-top:0.2rem;
        }
       
        #popup-subscribe-log_con .myrow
        {
            line-height:2rem;
            padding-left:4%;
        }
        #popup-subscribe-log_con input
        {
            font-family:"微软雅黑";
            width:6rem;
            height: 1.5rem;
            border: none;
            border-radius: .15rem;
            font-size: .8rem;
        }

        .float-button {
            position: fixed;
            bottom: 120px;
            right: 0px;
            z-index:300;
        }
    </style>

    <script type="text/javascript">
        var v_userid = "", v_username = "";// var v_userid = "763", v_username = "昆山吉时报关有限公司";

        $(function () {
            //---------------------------------------------------------------------------------------------------------------获取当前登录人及姓名
            $.ajax({
                type: "post", //要用post方式                 
                url: "SiteDeclareList.aspx/getcuruser",//方法所在页面和方法名
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                data: "{}",
                cache: false,
                async: false,//默认是true，异步；false为同步，此方法执行完在执行下面代码
                success: function (data) {
                    var obj = eval("(" + data.d + ")");
                    if (obj.length == 1) {
                        v_userid = obj[0]["USERID"]; v_username = obj[0]["USERNAME"];
                    }
                },
                error: function (XMLHttpRequest, textStatus, errorThrown) {//请求失败处理函数
                    //alert(XMLHttpRequest.status);
                    //alert(XMLHttpRequest.readyState);
                    //alert(textStatus);
                    alert('error...状态文本值：' + textStatus + " 异常信息：" + errorThrown);
                }
            });
            //---------------------------------------------------------------------------------------------------------------列表名称
            function showGridName() {
                var strname = '<div class="list-block" style="margin:0;font-size:small;">'
                            + '<ul>'
                                + '<li class="item-content" style="min-height:1.1rem;height:1.1rem;">'
                                    + '<div class="item-inner row" style="min-height:1.1rem;height:1.1rem;">'//border-top:2px solid #0894EC;border-left:2px solid #0894EC;border-right:2px solid #0894EC;
                                        + '<div class="item-title col-40">收发货人</div>'
                                        + '<div class="item-title col-25">业务类型</div>'
                                        + '<div class="item-title col-33">订单编号</div>'
                                    + '</div>'
                                + '</li>'
                                + '<li class="item-content" style="min-height:1.1rem;height:1.1rem;">'
                                    + '<div class="item-inner row" style="min-height:1.1rem;height:1.1rem;">'//border-top:2px solid #0894EC;border-left:2px solid #0894EC;border-right:2px solid #0894EC;
                                        + '<div class="item-title col-40">总分单号</div>'
                                        + '<div class="item-title col-25">申报方式</div>'
                                        + '<div class="item-title col-33">企业编号</div>'
                                    + '</div>'
                                + '</li>'
                                + '<li class="item-content" style="min-height:1.1rem;height:1.1rem;">'
                                    + '<div class="item-inner row" style="min-height:1.1rem;height:1.1rem;">'//border-top:2px solid #0894EC;border-left:2px solid #0894EC;border-right:2px solid #0894EC;
                                        + '<div class="item-title col-40">现场报关</div>'
                                        + '<div class="item-title col-25">件数/毛重</div>'
                                        + '<div class="item-title col-33">合同号</div>'
                                    + '</div>'
                                + '</li>'
                                + '<li class="item-content" style="min-height:1.1rem;height:1.1rem;">'
                                    + '<div class="item-inner row" style="min-height:1.1rem;height:1.1rem;">'//border-top:2px solid #0894EC;border-left:2px solid #0894EC;border-right:2px solid #0894EC;
                                        + '<div class="item-title col-40">查验维护</div>'
                                        + '<div class="item-title col-25">查验/稽核</div>'
                                        + '<div class="item-title col-33">两单关联</div>'
                                    + '</div>'
                                + '</li>'
                                + '<li class="item-content" style="min-height:1.1rem;height:1.1rem;">'
                                    + '<div class="item-inner row" style="min-height:1.1rem;height:1.1rem;">'//border:2px solid #0894EC;
                                        + '<div class="item-title col-40">现场放行</div>'
                                        + '<div class="item-title col-25">查验图片</div>'
                                        + '<div class="item-title col-33">多单关联</div>'
                                    + '</div>'
                                + '</li>'
                            + '</ul>'
                        + '</div>';
                $.modal({
                    //title: '<b>更多查询</b>',
                    text: strname,
                    //buttons: [{ text: '取消', bold: true, onClick: function () { } }],
                    extraClass: 'girdnamediv'//避免直接设置.modal的样式，从而影响其他toast的提示
                });

                $(document).on('click', '.girdnamediv', function () {
                    $.closeModal(".girdnamediv");
                });

            }

            initsearch_condition();
            initSerach_SiteDeclare();

            var loading = false;
            var itemsPerLoad = 10;// 每次加载添加多少条目                
            var maxItems = 100;// 最多可加载的条目
            var lastIndex = 0;//$('.list-block').length;//.list-container li     
            
            $("#btn_gridname_m").click(function () {
                showGridName();
            });

            $("#btn_search_m").click(function () {
                select();
            });
            $("#btn_reset_m").click(function () {
                $("#txt_siteapplytime_s").val(""); $("#txt_siteapplytime_e").val("");
                $("#txt_siteapplytime_s").calendar({}); $("#txt_siteapplytime_e").calendar({});//否则之前选的那天  不能再次选中

                $("#txt_declcode").val(""); $("#txt_customareacode").val("");
                $("#picker_is_pass").picker("setValue", ["全部"]); $("#picker_ischeck").picker("setValue", ["全部"]);

                $("#txt_busitype").val("");
                $("#txt_modifyflag").val("");
                $("#txt_auditflag").val("");

                $("#txt_busiunit").val(""); 
                $("#txt_ordercode").val(""); 
                $("#txt_cusno").val("");
                $("#txt_divideno").val(""); 
                $("#txt_contractno").val(""); 
                $("#txt_submittime_s").val(""); 
                $("#txt_submittime_e").val(""); 
                $("#txt_sitepasstime_s").val(""); 
                $("#txt_sitepasstime_e").val(""); 
            });


            function select() {
                $.showPreloader('加载中...');
                setTimeout(function () {
                    $.closeModal();
                    //首次查询需要置为初始值
                    $('#div_list').html(""); $("#span_curchose").text(0);
                    loading = false; itemsPerLoad = 10; lastIndex = 0;
                    var scroller = $('.native-scroll');

                    //首次查询，需要加载监听事件及加载符号
                    $('.infinite-scroll-preloader').show();
                    $.attachInfiniteScroll($('.infinite-scroll'));

                    loaddata(itemsPerLoad, lastIndex);
                    lastIndex = $('#div_list .list-block').length;// 更新最后加载的序号
                    $.refreshScroller();
                    scroller.scrollTop(0); //首次查询后，滚动条需要置为初始值0

                    if (lastIndex < itemsPerLoad) {
                        $.detachInfiniteScroll($('.infinite-scroll'));// 加载完毕，则注销无限加载事件，以防不必要的加载     
                        $('.infinite-scroll-preloader').hide();

                        if (lastIndex == 0) { $.toast("没有符合的数据！"); }
                        else { $.toast("已经加载到最后"); }
                    }

                    $.hidePreloader();
                }, 500);
            }

            //无限滚动
            $(document).on("pageInit", "#page-infinite-scroll-bottom", function (e, id, page) {
                $('.infinite-scroll-preloader').hide();

                $(page).on('infinite', function () {
                    if (loading) return;// 如果正在加载，则退出                    
                    loading = true;// 设置flag

                    $('.infinite-scroll-preloader').show();
                    $.attachInfiniteScroll($('.infinite-scroll'));

                    setTimeout(function () {
                        loading = false;// 重置加载flag
                        if (lastIndex >= maxItems || lastIndex % itemsPerLoad != 0) {
                            $.detachInfiniteScroll($('.infinite-scroll'));// 加载完毕，则注销无限加载事件，以防不必要的加载     
                            $('.infinite-scroll-preloader').hide();

                            $.toast("已经加载到最后");
                            return;
                        }

                        loaddata(itemsPerLoad, lastIndex);

                        if (lastIndex == $('#div_list .list-block').length) {
                            $.detachInfiniteScroll($('.infinite-scroll'));// 加载完毕，则注销无限加载事件，以防不必要的加载     
                            $('.infinite-scroll-preloader').hide();

                            $.toast("已经加载到最后");
                            return;
                        }
                        lastIndex = $('#div_list .list-block').length;// 更新最后加载的序号
                        $.refreshScroller();

                    }, 500);
                });
            });

            //$("#div_list").on('click', '.list-block', function (e) {// $("#div_list")也可以换成$(document)，是基于父容器的概念   
            //    if ($(this).children("ul").css('background-color') == "rgb(193, 221, 241)") {
            //        $(this).children("ul").css('background-color', '#fff');
            //    } else {
            //        $("#div_list .list-block ul").css('background-color', '#fff');
            //        $(this).children("ul").css('background-color', '#C1DDF1');
            //    }
            //});

            $("#div_list").on('click', '.list-block', function (e) {// $("#div_list")也可以换成$(document)，是基于父容器的概念   
                var curcount = parseInt($("#span_curchose").text());
                if ($(this).children("ul").css('background-color') == "rgb(193, 221, 241)") {
                    $(this).children("ul").css('background-color', '#fff');
                    curcount = curcount - 1;
                } else {
                    $(this).children("ul").css('background-color', '#C1DDF1');
                    curcount++;
                }
                $("#span_curchose").text(curcount);
            });

            //现场报关放行
            $("#Siteapply_Pass_a").click(function () {
                var curcount = parseInt($("#span_curchose").text());
                if (curcount <= 0) {
                    $.toast("请选择需要报关放行的记录");
                    return;
                }

                var arr_divid = new Array();
                $("#div_list .list-block").each(function () {
                    if ($(this).children("ul").css('background-color') == "rgb(193, 221, 241)") {
                        arr_divid.push($(this)[0].id.substring(6));////order_
                    }
                });
                var arr_divids = JSON.stringify(arr_divid);//将数组转为JSON字符串

                $.modal({
                    title: '<i id="sitecancel" class="iconfont" style="font-size:1.3rem;">&#xea4f;</i>',
                    text: '<span style="font-weight:800;">现场报关放行</span>' +
                            '<div class="content-block row" style="margin:0;padding:.75rem;">' +
                                '<div class="col-50"><a href="#" id="siteapply" class="button" style="background-color: #3d4145;color:white;border-radius:0;border:0;vertical-align:middle;">现场报关</a></div>' +
                                '<div class="col-50"><a href="#" id="sitepass" class="button" style="background-color: gray;color:white;border-radius:0;border:0;vertical-align:middle;">现场放行</a></div>' +
                            '</div>',
                    //title: '现场报关放行',
                    //text: '<div class="content-block row" style="margin:1rem 0;padding:0;">' +
                    //            '<div class="col-20"><a href="#" id="sitecancel" class="button" style="padding:0 0;background-color: gray;color:white;border-radius:0;border:0;vertical-align:middle;">返回</div>' +
                    //            '<div class="col-40"><a href="#" id="siteapply" class="button" style="background-color: #3d4145;color:white;border-radius:0;border:0;vertical-align:middle;">现场报关</a></div>' +
                    //            '<div class="col-40"><a href="#" id="sitepass" class="button" style="background-color: gray;color:white;border-radius:0;border:0;vertical-align:middle;">现场放行</a></div>' +
                    //        '</div>',
                    extraClass: 'sitediv'//避免直接设置.modal的样式，从而影响其他toast的提示
                });

                $("#sitecancel").click(function () {
                    $.closeModal(".sitediv");
                });

                $("#siteapply").click(function () {
                    $.closeModal(".sitediv");

                    $.confirm('请确认是否需要<font color=blue>现场报关</font>?',
                        function () {//OK事件
                            $.ajax({
                                type: "post", //要用post方式                 
                                url: "SiteDeclareList.aspx/Siteapplyall",//方法所在页面和方法名
                                contentType: "application/json; charset=utf-8",
                                dataType: "json",
                                data: "{'ordercode':'" + arr_divids + "'}",
                                cache: false,
                                async: false,//默认是true，异步；false为同步，此方法执行完在执行下面代码
                                success: function (data) {
                                    var obj = eval("(" + data.d + ")");

                                    var count_s = 0, count_e = 0, count_r = 0;
                                    for (var i = 0; i < obj.length; i++) {
                                        if (obj[i]["ISEXISTS"] == "Y") { count_r++; }
                                        else {
                                            if (obj[i]["FLAG"] == "S") {
                                                count_s++;
                                                $("#div_list #order_" + obj[i]["ORDERCODE"]).children("ul").children().eq(2).children("div").children().eq(0).text(obj[i]["CURTIME"]);//更新现场报关时间
                                            }
                                            if (obj[i]["FLAG"] == "E") { count_e++; }
                                        }
                                    }

                                    var msg = "";//"当前选中" + arr_divid.length + "笔";
                                    if (count_s > 0) { msg = msg + ",成功" + count_s + "笔"; }
                                    if (count_e > 0) { msg = msg + ",失败" + count_e + "笔"; }
                                    if (count_r > 0) { msg = msg + ",已存在" + count_r + "笔"; }

                                    if (msg == "") { msg = "系统异常，请重新登录！"; }
                                    else { msg = msg.substr(1);}
                                    $.toast(msg);
                                },
                                error: function (XMLHttpRequest, textStatus, errorThrown) {//请求失败处理函数
                                    //alert(XMLHttpRequest.status);
                                    //alert(XMLHttpRequest.readyState);
                                    //alert(textStatus);
                                    alert('error...状态文本值：' + textStatus + " 异常信息：" + errorThrown);
                                }
                            });
                        },
                        function () { }//cancel事件
                      );
                });

                $("#sitepass").click(function () {
                    $.closeModal(".sitediv");

                    $.confirm('请确认是否需要<font color=blue>现场放行</font>?',
                        function () {//OK事件
                            $.ajax({
                                type: "post", //要用post方式                 
                                url: "SiteDeclareList.aspx/Passall",//方法所在页面和方法名
                                contentType: "application/json; charset=utf-8",
                                dataType: "json",
                                data: "{'ordercode':'" + arr_divids + "'}",
                                cache: false,
                                async: false,//默认是true，异步；false为同步，此方法执行完在执行下面代码
                                success: function (data) {
                                    var obj = eval("(" + data.d + ")");

                                    var count_s = 0, count_e = 0, count_r = 0;
                                    for (var i = 0; i < obj.length; i++) {
                                        if (obj[i]["ISEXISTS"] == "Y") { count_r++; }
                                        else {
                                            if (obj[i]["FLAG"] == "S") {
                                                count_s++;
                                                $("#div_list #order_" + obj[i]["ORDERCODE"]).children("ul").children().eq(4).children("div").children().eq(0).text(obj[i]["CURTIME"]);//更新现场放行时间
                                            }
                                            if (obj[i]["FLAG"] == "E") { count_e++; }
                                        }
                                    }

                                    var msg = "";//"当前选中" + arr_divid.length + "笔";
                                    if (count_s > 0) { msg = msg + ",成功" + count_s + "笔"; }
                                    if (count_e > 0) { msg = msg + ",失败" + count_e + "笔"; }
                                    if (count_r > 0) { msg = msg + ",已存在" + count_r + "笔"; }

                                    if (msg == "") { msg = "系统异常，请重新登录！"; }
                                    else { msg = msg.substr(1); }
                                    $.toast(msg);
                                },
                                error: function (XMLHttpRequest, textStatus, errorThrown) {//请求失败处理函数
                                    //alert(XMLHttpRequest.status);
                                    //alert(XMLHttpRequest.readyState);
                                    //alert(textStatus);
                                    alert('error...状态文本值：' + textStatus + " 异常信息：" + errorThrown);
                                }
                            });
                        },
                        function () { }//cancel事件
                      );
                });
            });

            /*
            //现场报关
            $("#Siteapply_a").click(function () {
                var divid = "";//order_
                $("#div_list .list-block").each(function () {
                    if ($(this).children("ul").css('background-color') == "rgb(193, 221, 241)") {
                        divid = $(this)[0].id;
                        //alert($(this).children("ul").children().eq(2).children("div").children().eq(0).text());
                    }
                });
                if (divid == "") {
                    $.toast("请选择需要现场报关的记录");
                    return;
                }
                if ($("#div_list #" + divid).children("ul").children().eq(2).children("div").children().eq(0).text() != "") {
                    $.toast("该笔记录已经现场报关");
                    return;
                }


                $.confirm('请确认是否需要<font color=blue>现场报关</font>?',
                function () {//OK事件
                    $.ajax({
                        type: "post", //要用post方式                 
                        url: "SiteDeclareList.aspx/Siteapply",//方法所在页面和方法名
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
                        data: "{'ordercode':'" + divid.substring(6) + "'}",
                        cache: false,
                        async: false,//默认是true，异步；false为同步，此方法执行完在执行下面代码
                        success: function (data) {
                            if (data.d != "") {
                                $.toast("现场报关成功");
                                $("#div_list #" + divid).children("ul").children().eq(2).children("div").children().eq(0).text(data.d);//更新现场报关时间
                            } else {
                                $.toast("现场报关失败");
                            }
                        },
                        error: function (XMLHttpRequest, textStatus, errorThrown) {//请求失败处理函数
                            //alert(XMLHttpRequest.status);
                            //alert(XMLHttpRequest.readyState);
                            //alert(textStatus);
                            alert('error...状态文本值：' + textStatus + " 异常信息：" + errorThrown);
                        }
                    });
                },
                function () { }//cancel事件
              );

            });*/

            //报关单详细
            $("#Detail_a").click(function () {
                var curcount = parseInt($("#span_curchose").text());
                if (curcount != 1) {
                    $.toast("请选择一笔报关详细记录");
                    return;
                }

                var divid = "";//order_
                $("#div_list .list-block").each(function () {
                    if ($(this).children("ul").css('background-color') == "rgb(193, 221, 241)") {
                        divid = $(this)[0].id;
                        //alert($(this).children("ul").children().eq(4).children("div").children().eq(0).text());
                    }
                });
                //if (divid == "") {
                //    $.toast("请选择需要查看的记录");
                //    return;
                //}

                var strconHTML = "";

                $.ajax({
                    type: "post", //要用post方式                 
                    url: "SiteDeclareList.aspx/Detail",//方法所在页面和方法名
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    data: "{'ordercode':'" + divid.substring(6) + "'}",
                    cache: false,
                    async: false,
                    success: function (data) {
                        var obj = eval("(" + data.d + ")");//将字符串转为json
                       
                        var jsonorder = obj[0].json_order;
                        var jsondecl = obj[0].json_decl;

                        //strconHTML = '<font style="font-weight:800;font-size:.9rem;">报关详细信息</font>';
                        strconHTML = '<font class="title"><b>报关详细信息</b></font>';
                        strconHTML += '<div class="list-block" style="margin:0;margin-top:2rem;margin-buttom:1.5rem;font-size:13px;color:black;">'
                                        + '<ul>'
                                           + '<li class="item-content" style="min-height:1.3rem;height:1.3rem;">' +
                                                  '<div class="item-inner row" style="min-height:1.3rem;height:1.3rem;">'
                                                    + '<div class="item-title col-25">委托时间</div>'
                                                    + '<div class="item-title col-50">' + (jsonorder[0]["SUBMITTIME"] == null ? "" : jsonorder[0]["SUBMITTIME"]) + '</div>'
                                                    + '<div class="item-title col-25">' + (jsonorder[0]["SUBMITUSERNAME"] == null ? "" : jsonorder[0]["SUBMITUSERNAME"]) + '</div>'
                                                + '</div>'
                                           + '</li>'
                                            + '<li class="item-content" style="min-height:1.3rem;height:1.3rem;">'
                                                + '<div class="item-inner row" style="min-height:1.3rem;height:1.3rem;">'
                                                    + '<div class="item-title col-25">制单完成</div>'
                                                    + '<div class="item-title col-50">' + (jsonorder[0]["MOENDTIME"] == null ? "" : jsonorder[0]["MOENDTIME"]) + '</div>'
                                                    + '<div class="item-title col-25">' + (jsonorder[0]["MOENDNAME"] == null ? "" : jsonorder[0]["MOENDNAME"]) + '</div>'
                                                + '</div>'
                                            + '</li>'                                            
                                            + '<li class="item-content" style="min-height:1.3rem;height:1.3rem;">'
                                                + '<div class="item-inner row" style="min-height:1.3rem;height:1.3rem;">'
                                                    + '<div class="item-title col-25">审核完成</div>'
                                                    + '<div class="item-title col-50">' + (jsonorder[0]["COENDTIME"] == null ? "" : jsonorder[0]["COENDTIME"]) + '</div>'
                                                    + '<div class="item-title col-25">' + (jsonorder[0]["COENDNAME"] == null ? "" : jsonorder[0]["COENDNAME"]) + '</div>'
                                                + '</div>'
                                            + '</li>'
                                            + '<li class="item-content" style="min-height:1.3rem;height:1.3rem;">'
                                                + '<div class="item-inner row" style="min-height:1.3rem;height:1.3rem;">'
                                                    + '<div class="item-title col-25">预录完成</div>'
                                                    + '<div class="item-title col-50">' + (jsonorder[0]["PREENDTIME"] == null ? "" : jsonorder[0]["PREENDTIME"]) + '</div>'
                                                    + '<div class="item-title col-25">' + (jsonorder[0]["PREENDNAME"] == null ? "" : jsonorder[0]["PREENDNAME"]) + '</div>'
                                                + '</div>'
                                            + '</li>'
                                            + '<li class="item-content" style="min-height:1.3rem;height:1.3rem;">'
                                                + '<div class="item-inner row" style="min-height:1.3rem;height:1.3rem;">'
                                                    + '<div class="item-title col-25">申报完成</div>'
                                                    + '<div class="item-title col-50">' + (jsonorder[0]["REPENDTIME"] == null ? "" : jsonorder[0]["REPENDTIME"]) + '</div>'
                                                    + '<div class="item-title col-25">' + (jsonorder[0]["REPENDNAME"] == null ? "" : jsonorder[0]["REPENDNAME"]) + '</div>'
                                                + '</div>'
                                            + '</li>'
                                            + '<li class="item-content" style="min-height:1.3rem;height:1.3rem;">'
                                                + '<div class="item-inner row" style="min-height:1.3rem;height:1.3rem;">'
                                                    + '<div class="item-title col-25">报关查验</div>'
                                                    + '<div class="item-title col-50">' + (jsonorder[0]["DECLCHECKTIME"] == null ? "" : jsonorder[0]["DECLCHECKTIME"]) + '</div>'
                                                    + '<div class="item-title col-25">' + (jsonorder[0]["DECLCHECKNAME"] == null ? "" : jsonorder[0]["DECLCHECKNAME"]) + '</div>'
                                                + '</div>'
                                            + '</li>'
                                            + '<li class="item-content" style="min-height:1.3rem;height:1.3rem;">'
                                                + '<div class="item-inner row" style="min-height:1.3rem;height:1.3rem;">'
                                                    + '<div class="item-title col-25">报关稽核</div>'
                                                    + '<div class="item-title col-50">' + (jsonorder[0]["AUDITFLAGTIME"] == null ? "" : jsonorder[0]["AUDITFLAGTIME"]) + '</div>'
                                                    + '<div class="item-title col-25">' + (jsonorder[0]["AUDITFLAGNAME"] == null ? "" : jsonorder[0]["AUDITFLAGNAME"]) + '</div>'
                                                + '</div>'
                                            + '</li>'
                                            + '<li class="item-content" style="min-height:1.3rem;height:1.3rem;">'
                                                + '<div class="item-inner row" style="min-height:1.3rem;height:1.3rem;">'
                                                    + '<div class="item-title col-25">现场报关</div>'
                                                    + '<div class="item-title col-50">' + (jsonorder[0]["SITEAPPLYTIME"] == null ? "" : jsonorder[0]["SITEAPPLYTIME"]) + '</div>'
                                                    + '<div class="item-title col-25">' + (jsonorder[0]["SITEAPPLYUSERNAME"] == null ? "" : jsonorder[0]["SITEAPPLYUSERNAME"]) + '</div>'
                                                + '</div>'
                                            + '</li>'
                                            + '<li class="item-content" style="min-height:1.3rem;height:1.3rem;">'
                                                + '<div class="item-inner row" style="min-height:1.3rem;height:1.3rem;">'
                                                    + '<div class="item-title col-25">现场放行</div>'
                                                    + '<div class="item-title col-50">' + (jsonorder[0]["SITEPASSTIME"] == null ? "" : jsonorder[0]["SITEPASSTIME"]) + '</div>'
                                                    + '<div class="item-title col-25">' + (jsonorder[0]["SITEPASSUSERNAME"] == null ? "" : jsonorder[0]["SITEPASSUSERNAME"]) + '</div>'
                                                + '</div>'
                                            + '</li>'
                                            + '<li class="item-content" style="min-height:1.3rem;height:1.3rem;">'
                                                + '<div class="item-inner row" style="min-height:1.3rem;height:1.3rem;">'
                                                    + '<div class="item-title col-25">检验图片</div>'
                                                    + '<div class="item-title col-50">' + getname("CHECKPIC", jsonorder[0]["CHECKPIC"]) + '</div>'
                                                    + '<div class="item-title col-25"></div>'
                                                + '</div>'
                                            + '</li>'
                                        + '</ul>'
                                    + '</div>';

                   
                        strconHTML += '<div class="list-block" style="margin:0;font-size:14px;color:black;">'
                                        + '<ul>'
                                            + '<li class="item-content" style="min-height:1.3rem;height:1.3rem;">'
                                                + '<div class="item-inner row" style="min-height:1.3rem;height:1.3rem;border-top:2px solid #0894EC;border-left:2px solid #0894EC;border-right:2px solid #0894EC;">'
                                                    + '<div class="item-title col-50">报关单号</div>'
                                                    + '<div class="item-title col-25">件数/毛重</div>'
                                                    + '<div class="item-title col-25">海关状态</div>'
                                                + '</div>'
                                            + '</li>'
                                            + '<li class="item-content" style="min-height:1.3rem;height:1.3rem;">'
                                                + '<div class="item-inner row" style="min-height:1.3rem;height:1.3rem;border:2px solid #0894EC;">'
                                                    + '<div class="item-title col-50">运输工具名称</div>'
                                                    + '<div class="item-title col-25">监管方式</div>'
                                                    + '<div class="item-title col-25">删改单</div>'
                                                + '</div>'
                                            + '</li>'
                                        + '</ul>'
                                    + '</div>';
                    


                        for (var i = 0; i < jsondecl.length; i++) {
                            strconHTML += '<div class="list-block" style="margin:0;font-size:13px;color:black;">'
                                            + '<ul>'
                                                + '<li class="item-content" style="min-height:1.3rem;height:1.3rem;">'
                                                    + '<div class="item-inner row" style="min-height:1.3rem;height:1.3rem;">'
                                                        + '<div class="item-title col-50">' + (jsondecl[i]["DECLARATIONCODE"] == null ? "" : jsondecl[i]["DECLARATIONCODE"]) + '</div>'
                                                        + '<div class="item-title col-25">' + (jsondecl[i]["GOODSNUM"] == null ? "" : jsondecl[i]["GOODSNUM"]) + '/'
                                                                                            + (jsondecl[i]["GOODSGW"] == null ? "" : jsondecl[i]["GOODSGW"])
                                                        + '</div>'
                                                        + '<div class="item-title col-25">' + (jsondecl[i]["CUSTOMSSTATUS"] == null ? "" : jsondecl[i]["CUSTOMSSTATUS"]) + '</div>'
                                                    + '</div>'
                                                + '</li>'
                                                + '<li class="item-content" style="min-height:1.3rem;height:1.3rem;">'
                                                    + '<div class="item-inner row" style="min-height:1.3rem;height:1.3rem;">'
                                                        + '<div class="item-title col-50">' + (jsondecl[i]["TRANSNAME"] == null ? "" : jsondecl[i]["TRANSNAME"]) + '</div>'
                                                        + '<div class="item-title col-25">' + (jsondecl[i]["TRADECODE"] == null ? "" : jsondecl[i]["TRADECODE"]) + '</div>'
                                                        + '<div class="item-title col-25">' + getname("MODIFYFLAG", jsondecl[i]["MODIFYFLAG"]) + '</div>'
                                                    + '</div>'
                                                + '</li>'
                                            + '</ul>'
                                        + '</div>';
                        }

                    },
                    error: function (XMLHttpRequest, textStatus, errorThrown) {//请求失败处理函数
                        //alert(XMLHttpRequest.status);
                        //alert(XMLHttpRequest.readyState);
                        //alert(textStatus);
                        alert('error...状态文本值：' + textStatus + " 异常信息：" + errorThrown);
                    }
                });

                var popupHTML = '<div class="popup">' +
                                 '<div class="content">' +//data-type='native'                                                                               
                                        strconHTML +
                                        //'<div class="content-block"><a href="#" class="close-popup button button-fill button-danger">返回</a></div>' +
                                        '<div class="content-block"><a href="#" class="close-popup button button-fill" style="background-color: #3d4145;border-radius:0;color:white;border:0;">返回</a></div>' +
                                 '</div>' +
                             '</div>';
                $.popup(popupHTML);

            });

            /*
            //报关单放行
            $("#Pass_a").click(function () {
                var divid = "";//order_
                $("#div_list .list-block").each(function () {
                    if ($(this).children("ul").css('background-color') == "rgb(193, 221, 241)") {
                        divid = $(this)[0].id;
                        //alert($(this).children("ul").children().eq(4).children("div").children().eq(0).text());
                    }
                });
                if (divid == "") {
                    $.toast("请选择需要放行的记录");
                    return;
                }
                if ($("#div_list #" + divid).children("ul").children().eq(4).children("div").children().eq(0).text() != "") {
                    $.toast("该笔记录已经放行，不能再放行");
                    return;
                }


                $.confirm('请确认是否需要<font color=blue>放行</font>?',
                    function () {//OK事件
                        $.ajax({
                            type: "post", //要用post方式                 
                            url: "SiteDeclareList.aspx/Pass",//方法所在页面和方法名
                            contentType: "application/json; charset=utf-8",
                            dataType: "json",
                            data: "{'ordercode':'" + divid.substring(6) + "'}",
                            cache: false,
                            async: false,//默认是true，异步；false为同步，此方法执行完在执行下面代码
                            success: function (data) {
                                if (data.d != "") {
                                    $.toast("放行成功");
                                    $("#div_list #" + divid).children("ul").children().eq(4).children("div").children().eq(0).text(data.d);//更新交接时间
                                } else {
                                    $.toast("放行失败");
                                }
                            },
                            error: function (XMLHttpRequest, textStatus, errorThrown) {//请求失败处理函数
                                //alert(XMLHttpRequest.status);
                                //alert(XMLHttpRequest.readyState);
                                //alert(textStatus);
                                alert('error...状态文本值：' + textStatus + " 异常信息：" + errorThrown);
                            }
                        });
                    },
                    function () { }//cancel事件
                  );

            });
            */

            //查验标志
            $("#Check_a").click(function () {
                var curcount = parseInt($("#span_curchose").text());
                if (curcount != 1) {
                    $.toast("请选择一笔查验标志记录");
                    return;
                }

                var divid = "";//order_
                $("#div_list .list-block").each(function () {
                    if ($(this).children("ul").css('background-color') == "rgb(193, 221, 241)") {
                        divid = $(this)[0].id;
                        //alert($(this).children("ul").children().eq(2).children("div").children().eq(0).text());
                    }
                });
                //if (divid == "") {
                //    $.toast("请选择需要查验标志的记录");
                //    return;
                //}

                var strconHTML = "";
                strconHTML = '<font class="title"><b>报关查验稽核维护</b></font>';

                strconHTML += '<div class="list-block" style="margin:0; margin-top:2rem;margin-buttom:2rem;margin-left:4%;margin-right:4%;line-height:1.5rem;font-size:.7rem">' +
                                    '<div class="row"> ' +
                                           //'<div class="col-50"><a href="#" class="close-popup button button-fill button-danger">返回</div>' +
                                           //'<div class="col-50"><a href="#" id="check_audit_save" class="button button-fill">保存</a></div>' +
                                            '<div class="col-50"><a href="#" class="close-popup button button-fill"  style="background-color: gray;border-radius:0;color:white;border:0;">返回</div>' +
                                           '<div class="col-50" style="margin-left:0rem;"><a href="#" id="check_audit_save" class="button button-fill"  style="background-color: #3d4145;border-radius:0;color:white;border:0;">保存</a></div>' +
                                    '</div>' +
                                    '<div class="row"> ' +
                                        '<div class="col-33">订单编号：</div>' +
                                        '<div class="col-66">' + divid.substring(6) + '</div>' +
                                    '</div> ' +
                                    '<div class="row"> ' +
                                        '<div class="col-33">企业编号：</div>' +
                                        '<div class="col-66">' + $("#div_list #" + divid).children("ul").children().eq(1).children("div").children().eq(2).text() + '</div>' +
                                    '</div> ' +
                                    '<hr style="height:1px;border:none;border-top:1px dashed black;"/>' +
                                    '<div class="row"> ' +
                                        '<div class="col-33">查验时间：</div>' +
                                        '<div class="col-66"><input type="text" style="background:#c7c7cc;height:1.2rem;font-size:.7rem" id="txt_declchecktime" readonly /></div>' +
                                    '</div> ' +
                                        '<div class="row"> ' +
                                        '<div class="col-33">查验人员：<input type="hidden" id="txt_declcheckid" readonly /></div>' +
                                        '<div class="col-66"><input type="text" style="background:#c7c7cc;height:1.2rem;font-size:.7rem" id="txt_declcheckname" readonly /></div>' +
                                    '</div> ' +
                                    '<div class="row"> ' +
                                        '<div class="col-33">查验备注：</div>' +
                                        '<div class="col-66"><input type="text" style="background:#c7c7cc;height:1.2rem;font-size:.7rem" id="txt_declcheckremark" readonly /></div>' +//:#ffffff
                                    '</div> ' +
                                '</div>';
                strconHTML += '<div class="list-block" style="margin:0;font-size:.7rem;margin-left:4%;margin-right:4%;">' +
                                 '<ul style="background:#e8e8e8;">' +
                                   '<li>' +
                                       '<div class="row" style="height:2rem"> ' +
                                           '<div class="col-50" style="margin-top:.5rem;">箱号</div>' +
                                           '<div class="col-25" style="margin-top:.5rem;">箱型</div>' +
                                           '<div class="col-25" style="margin-top:.5rem;">查验选择</div>' +
                                       '</div> ' +
                                   '</li>' +
                                '</ul>' +
                           '</div>';

                $.ajax({
                    type: "post", //要用post方式                 
                    url: "SiteDeclareList.aspx/declcontainerdata",//方法所在页面和方法名
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    data: "{'ordercode':'" + divid.substring(6) + "'}",
                    cache: false,
                    async: false,
                    success: function (data) {
                        var obj = eval("(" + data.d + ")");//将字符串转为json

                        for (var i = 0; i < obj.length; i++) {
                            strconHTML += '<div class="list-block" style="margin:0;font-size:.7rem;margin-left:4%;margin-right:4%;">' +
                                              '<ul style="background:#e8e8e8;">' +
                                                '<li>' +
                                                    '<div class="row"> ' +
                                                       '<div class="col-50" style="margin-top:.5rem;">' + (obj[i]["CONTAINERNO"] == null ? "" : obj[i]["CONTAINERNO"]) + '</div>' +
                                                       '<div class="col-25" style="margin-top:.5rem;">' + (obj[i]["CONTAINERSIZEE"] == null ? "" : obj[i]["CONTAINERSIZEE"]) + '</div>' +
                                                       '<div class="col-25">' +
                                                           '<label class="label-checkbox item-content">' +
                                                               '<input type="checkbox" name="checkbox_type" value="">' +
                                                               '<div class="item-media"><i class="icon icon-form-checkbox"></i></div>' +
                                                           '</label>' +
                                                       '</div>' +
                                                   '</div> ' +
                                               '</li>' +
                                            '</ul>' +
                                       '</div>';
                        }

                    },
                    error: function (XMLHttpRequest, textStatus, errorThrown) {//请求失败处理函数
                        //alert(XMLHttpRequest.status);
                        //alert(XMLHttpRequest.readyState);
                        //alert(textStatus);
                        alert('error...状态文本值：' + textStatus + " 异常信息：" + errorThrown);
                    }
                });

                var strconHTML2 = '<div class="list-block" style="margin:0; margin-top:2rem;margin-buttom:2rem;margin-left:4%;margin-right:4%;line-height:1.5rem;font-size:.7rem">' +
                                    '<hr style="height:1px;border:none;border-top:1px dashed black;"/>' +
                                    '<div class="row"> ' +
                                        '<div class="col-33">稽核时间：</div>' +
                                        '<div class="col-66"><input type="text" style="background:#c7c7cc;height:1.2rem;font-size:.7rem" id="txt_auditflagtime" readonly /></div>' +
                                    '</div> ' +
                                        '<div class="row"> ' +
                                        '<div class="col-33">稽核人员：<input type="hidden" id="txt_auditflagid" readonly /></div>' +
                                        '<div class="col-66"><input type="text" style="background:#c7c7cc;height:1.2rem;font-size:.7rem" id="txt_auditflagname" readonly /></div>' +
                                    '</div> ' +
                                    '<div class="row"> ' +
                                        '<div class="col-33">稽核内容：</div>' +
                                        '<div class="col-66"><input type="text" style="background:#c7c7cc;height:1.2rem;font-size:.7rem" id="txt_auditcontent" readonly /></div>' +//:#ffffff
                                    '</div> ' +
                                '</div>';

                
                //var ButtonHTML = '<div class="content-block">' +
                //                        '<div class="row"> ' +
                //                            '<div class="col-50"><a href="#" class="close-popup button button-fill button-danger">返回</div>' +
                //                            '<div class="col-50"><a href="#" id="check_audit_save" class="button button-fill">保存</a></div>' +
                //                        '</div>' +
                //                    '</div>';
                var popupHTML = '<div class="popup" style="background:#e8e8e8;">' +
                                 '<div class="content">' +//data-type='native'                                                                               
                                        strconHTML +
                                        strconHTML2 +
                                        //ButtonHTML +
                                 '</div>' +
                             '</div>';

                $.popup(popupHTML);

                $.ajax({
                    type: "post", //要用post方式                 
                    url: "SiteDeclareList.aspx/loadcheckdata",//方法所在页面和方法名
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    data: "{'ordercode':'" + divid.substring(6) + "'}",
                    cache: false,
                    async: false,
                    success: function (data) {
                        var obj = eval("(" + data.d + ")");//将字符串转为json

                        if (obj.length > 0) {
                            $("#txt_declchecktime").val(obj[0]["DECLCHECKTIME"] == null ? "" : obj[0]["DECLCHECKTIME"]);//初始化日期时间
                            $("#txt_declcheckid").val(obj[0]["DECLCHECKID"] == null ? "" : obj[0]["DECLCHECKID"]);//当前登录人id
                            $("#txt_declcheckname").val(obj[0]["DECLCHECKNAME"] == null ? "" : obj[0]["DECLCHECKNAME"]);//当前登录人name
                            $("#txt_declcheckremark").val(obj[0]["DECLCHECKREMARK"] == null ? "" : obj[0]["DECLCHECKREMARK"]);//查验备注赋值

                            if ($("#txt_declchecktime").val() != "") {
                                $("#txt_declcheckremark").removeAttr('readonly'); $("#txt_declcheckremark").css('background','#ffffff');
                            }

                            $("#txt_auditflagtime").val(obj[0]["AUDITFLAGTIME"] == null ? "" : obj[0]["AUDITFLAGTIME"]);//初始化日期时间
                            $("#txt_auditflagid").val(obj[0]["AUDITFLAGID"] == null ? "" : obj[0]["AUDITFLAGID"]);//当前登录人id
                            $("#txt_auditflagname").val(obj[0]["AUDITFLAGNAME"] == null ? "" : obj[0]["AUDITFLAGNAME"]);//当前登录人name
                            $("#txt_auditcontent").val(obj[0]["AUDITCONTENT"] == null ? "" : obj[0]["AUDITCONTENT"]);//稽核内容赋值

                            if ($("#txt_auditflagtime").val() != "") {
                                $("#txt_auditcontent").removeAttr('readonly'); $("#txt_auditcontent").css('background', '#ffffff');
                            }
                        }

                    },
                    error: function (XMLHttpRequest, textStatus, errorThrown) {//请求失败处理函数
                        //alert(XMLHttpRequest.status);
                        //alert(XMLHttpRequest.readyState);
                        //alert(textStatus);
                        alert('error...状态文本值：' + textStatus + " 异常信息：" + errorThrown);
                    }
                });

                $("#txt_declchecktime").click(function () {
                    if ($("#txt_declchecktime").val() == "") {
                        $("#txt_declchecktime").val(getNowDate());//当前时间
                        $("#txt_declcheckid").val(v_userid);//当前登录人id
                        $("#txt_declcheckname").val(v_username);//当前登录人name
                        $("#txt_declcheckremark").removeAttr('readonly'); $("#txt_declcheckremark").css('background', '#ffffff');
                    } else {
                        $.confirm('请确认是否需要<font color=blue>撤销查验</font>?',
                        function () {//OK事件
                            $("#txt_declchecktime").val("");
                            $("#txt_declcheckid").val("");
                            $("#txt_declcheckname").val("");
                            $("#txt_declcheckremark").val("");
                            $("#txt_declcheckremark").prop('readonly', 'readonly'); $("#txt_declcheckremark").css('background', '#c7c7cc');
                        });
                    }                    
                });

                $("#txt_auditflagtime").click(function () {
                    if ($("#txt_auditflagtime").val() == "") {
                        $("#txt_auditflagtime").val(getNowDate());
                        $("#txt_auditflagid").val(v_userid);//当前登录人id
                        $("#txt_auditflagname").val(v_username);//当前登录人name
                        $("#txt_auditcontent").removeAttr('readonly'); $("#txt_auditcontent").css('background', '#ffffff');
                    } else {
                        $.confirm('请确认是否需要<font color=blue>撤销稽核</font>?',
                        function () {//OK事件
                            $("#txt_auditflagtime").val("");
                            $("#txt_auditflagid").val("");
                            $("#txt_auditflagname").val("");
                            $("#txt_auditcontent").val("");
                            $("#txt_auditcontent").prop('readonly', 'readonly'); $("#txt_auditcontent").css('background', '#c7c7cc');
                        });
                    }
                });

                $("#check_audit_save").click(function () {
                    $.confirm('请确认是否<font color=blue>保存</font>?',
                         function () {//OK事件
                             $.ajax({
                                 type: "post", //要用post方式                 
                                 url: "SiteDeclareList.aspx/check_audit_save",//方法所在页面和方法名
                                 contentType: "application/json; charset=utf-8",
                                 dataType: "json",
                                 data: "{'ordercode':'" + divid.substring(6)
                                     + "','checktime':'" + $("#txt_declchecktime").val() + "','checkname':'" + $("#txt_declcheckname").val()
                                     + "','checkid':'" + $("#txt_declcheckid").val() + "','checkremark':'" + $("#txt_declcheckremark").val()
                                     + "','auditflagtime':'" + $("#txt_auditflagtime").val() + "','auditflagname':'" + $("#txt_auditflagname").val()
                                     + "','auditflagid':'" + $("#txt_auditflagid").val() + "','auditcontent':'" + $("#txt_auditcontent").val()
                                     + "'}",
                                 cache: false,
                                 async: false,//默认是true，异步；false为同步，此方法执行完在执行下面代码
                                 success: function (data) {
                                     var obj = eval("(" + data.d + ")");
                                     if (obj.length == 1) {
                                         $.toast("保存成功");
                                         $("#div_list #" + divid).children("ul").children().eq(3).children("div").children().eq(0).text(obj[0]["DECLCHECKTIME"]);//更新查验时间
                                         $("#div_list #" + divid).children("ul").children().eq(3).children("div").children().eq(1).text(getname2("ISCHECK", obj[0]["ISCHECK"], obj[0]["AUDITFLAG"]));                                         
                                         $("#div_list #" + divid).children("ul").children().eq(4).children("div").children().eq(1).text(getname("CHECKPIC", obj[0]["CHECKPIC"]));

                                         //obj[0]["AUDITFLAGTIME"]//更新稽核时间
                                     } else {
                                         $.toast("保存失败");
                                     }
                                 },
                                 error: function (XMLHttpRequest, textStatus, errorThrown) {//请求失败处理函数
                                     //alert(XMLHttpRequest.status);
                                     //alert(XMLHttpRequest.readyState);
                                     //alert(textStatus);
                                     alert('error...状态文本值：' + textStatus + " 异常信息：" + errorThrown);
                                 }
                             });
                         });
                });
            });

            //查验图片
            $("#Picture_a").click(function () {
                var curcount = parseInt($("#span_curchose").text());
                if (curcount != 1) {
                    $.toast("请选择一笔查验图片记录");
                    return;
                }

                var divid = "";//order_
                $("#div_list .list-block").each(function () {
                    if ($(this).children("ul").css('background-color') == "rgb(193, 221, 241)") {
                        divid = $(this)[0].id;
                    }
                }); 
                //if (divid == "") {
                //    $.toast("请选择需要查验图片的记录");
                //    return;
                //}

                var concheck = $("#div_list #" + divid).children("ul").children().eq(3).children("div").children().eq(1).text();

                if (concheck != getname2("ISCHECK", 1, 0) && concheck != getname2("ISCHECK", 1, 1)) {
                    $.toast("无查验标志，不能使用查验图片功能");
                    return;
                }


                $.modal({
                    title: '<i id="picfilecancel" class="iconfont" style="font-size:1.3rem;">&#xea4f;</i>',
                    text: '<span style="font-weight:800;">查验图片</span>' +
                            '<div class="content-block row" style="margin:0;padding:.75rem;">' +
                                '<div class="col-50"><a href="#" id="picfileupload" class="button" style="background-color: #3d4145;color:white;border-radius:0;border:0;vertical-align:middle;">上传</a></div>' +
                                '<div class="col-50"><a href="#" id="picfileconsult" class="button" style="background-color: gray;color:white;border-radius:0;border:0;vertical-align:middle;">调阅</a></div>' +
                            '</div>',
                    //title: '查验图片',
                    //text: '<div class="content-block row" style="margin:1rem 0;">' +
                    //            //'<div class="col-33"><a href="#" id="picfilecancel" class="button button-fill">返回</div>' +
                    //            //'<div class="col-33"><a href="#" id="picfileupload" class="button button-fill">上传</a></div>' +
                    //            //'<div class="col-33"><a href="#" id="picfileconsult" class="button button-fill">调阅</a></div>' +

                    //            '<div class="col-33"><a href="#" id="picfilecancel" class="button" style="background-color: gray;color:white;border-radius:0;border:0;vertical-align:middle;">返回</div>' +
                    //            '<div class="col-33"><a href="#" id="picfileupload" class="button" style="background-color: #3d4145;color:white;border-radius:0;border:0;vertical-align:middle;">上传</a></div>' +
                    //            '<div class="col-33"><a href="#" id="picfileconsult" class="button" style="background-color: gray;color:white;border-radius:0;border:0;vertical-align:middle;">调阅</a></div>' +
                    //        '</div>',
                    extraClass: 'picdiv'//避免直接设置.modal的样式，从而影响其他toast的提示
                });

                $("#picfilecancel").click(function () {
                    $.closeModal(".picdiv");
                });

                $("#picfileupload").click(function () {
                    $.closeModal(".picdiv");

                    wx.ready(function () {
                        wx.chooseImage({
                            count: 5, // 默认9
                            sizeType: ['original', 'compressed'], // 可以指定是原图还是压缩图，默认二者都有
                            sourceType: ['album', 'camera'], // 可以指定来源是相册还是相机，默认二者都有
                            success: function (res) {　 　　　　　              
                                var localIds = res.localIds;//把图片本地的id信息，用于上传图片到微信浏览器时使用                                            
                                syncUpload(localIds);                               
                            }
                        });

                        var syncUpload = function (localIds) {
                            var localId = localIds.pop();
                            wx.uploadImage({
                                localId: localId,
                                isShowProgressTips: 1,
                                success: function (res) {
                                    savefile(res.serverId); // 返回图片的服务器端ID
                                    if (localIds.length > 0) {
                                        syncUpload(localIds);
                                    }
                                }
                            });
                        };

                    });
                    //初始化jsapi接口 状态
                    wx.error(function (res) {
                        alert("调用微信jsapi返回的状态:" + res.errMsg);
                    });

                    function savefile(serverId) {
                        $.ajax({
                            type: "post", //要用post方式                 
                            url: "SiteDeclareList.aspx/SaveFile",//方法所在页面和方法名
                            contentType: "application/json; charset=utf-8",
                            dataType: "json",
                            data: "{'mediaIds':'" + serverId + "','ordercode':'" + divid.substring(6) + "'}",
                            cache: false,
                            async: true,//默认是true，异步；false为同步，此方法执行完在执行下面代码
                            success: function (data) {
                                if (data.d == "success") {
                                    $.toast("上传成功");
                                    //修改页面查验图片标志
                                    $("#div_list #" + divid).children("ul").children().eq(4).children("div").children().eq(1).text(getname("CHECKPIC", 1));
                                } else {
                                    $.toast("上传失败");
                                }
                            }
                        });
                    }

                });

                $("#picfileconsult").click(function () {

                    $.closeModal(".picdiv");

                    if ($("#div_list #" + divid).children("ul").children().eq(4).children("div").children().eq(1).text() != getname("CHECKPIC", 1)) {
                        $.toast("没有查验图片");
                        return;
                    }

                    $.ajax({
                        type: "post", //要用post方式                 
                        url: "SiteDeclareList.aspx/picfileconsult",//方法所在页面和方法名
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
                        data: "{'ordercode':'" + divid.substring(6) + "'}",
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

                });

            });

            $("#Subs_log_a").click(function () {
                var curcount = parseInt($("#span_curchose").text());
                if (curcount != 1) {
                    $.toast("请选择一笔订阅记录");
                    return;
                }

                var divid = "";//order_
                var cusno = "";
                $("#div_list .list-block").each(function () {
                    if ($(this).children("ul").css('background-color') == "rgb(193, 221, 241)") {
                        divid = $(this)[0].id;
                        cusno = $(this).children("ul").children().eq(1).children("div").children().eq(2).text();
                    }
                });
                //if (divid == "") {
                //    $.toast("请选择需要订阅的记录");
                //    return;
                //}
                $.popup("#popup-subscribe-log");

                $(document).on('open', '#popup-subscribe-log', function () {
                    $("input[name='chk_orderstatus']").prop('checked', false);
                    $("input[name='chk_logisticstatus']").prop('checked', false);

                    $("[href='#sub_tab1']").addClass("active"); $("#sub_tab1").addClass("active");
                    $("[href='#sub_tab2']").removeClass("active"); $("#sub_tab2").removeClass("active");
                });

                $(document).on('opened', '#popup-subscribe-log', function () {
                    $("#Pop_hd_ordercode").val(divid.substring(6));
                    $("#Pop_hd_cusno").val(cusno);
                });
            });

            $("#Pop_Subscribe_log").click(function () {
                var ordercode = $("#Pop_hd_ordercode").val();
                var cusno = $("#Pop_hd_cusno").val();
                var type = "", status = "";

                var tab = $("#popup-subscribe-log_con .tab").css("display");
                if (tab == "block") {
                    type = "业务状态";

                    $("input[name='chk_orderstatus']:checked").each(function (index, item) {
                        status += $(this).val() + ",";
                    });
                }
                else {
                    type = "物流状态";

                    $("input[name='chk_logisticstatus']:checked").each(function (index, item) {
                        status += $(this).val() + ",";
                    });
                }

                if (status == "") {
                    $.toast("请选择需要订阅的" + type + "");
                    return;
                }

                $.ajax({
                    type: 'post',
                    url: '/Page/MyBusiness/MyBusiness.aspx/SubscribeStatus',
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    data: "{'type':'" + type + "','status':'" + status + "','cusno':'" + cusno + "','declarationcode':'','ordercode':'" + ordercode + "'}",
                    cache: false,
                    async: false,//默认是true，异步；false为同步，此方法执行完在执行下面代码
                    success: function (data) {
                        $.toast(data.d);
                    }
                });

            });

            //订阅清单
            //$("#Subscribe_a").click(function () {
            //    window.location.href = "/Page/MyBusiness/NewSubscribeList_busi.aspx";
            //});
            //打开订阅详情弹出框
            $(document).on('click', '.open-subinfo_busi', function () {

                subinfoload_busi();

            });

            $.init();
            //----------------------------------------------------------------------------------------------------------------------------------------
            function loaddata(itemsPerLoad, lastIndex) {
                $.ajax({
                    type: "post", //要用post方式                 
                    url: "SiteDeclareList.aspx/BindList",//方法所在页面和方法名
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    data: "{'siteapplytime_s':'" + $("#txt_siteapplytime_s").val() + "','siteapplytime_e':'" + $("#txt_siteapplytime_e").val() + "','declcode':'" + $("#txt_declcode").val()
                        + "','customareacode':'" + $("#txt_customareacode").val() + "','ispass':'" + $("#picker_is_pass").val() + "','ischeck':'" + $("#picker_ischeck").val()
                        + "','busitype':\"" + $("#txt_busitype").val() + "\",'modifyflag':'" + $("#txt_modifyflag").val() + "','auditflag':'" + $("#txt_auditflag").val()
                        + "','busiunit':'" + $("#txt_busiunit").val() + "','ordercode':'" + $("#txt_ordercode").val() + "','cusno':'" + $("#txt_cusno").val()
                        + "','divideno':'" + $("#txt_divideno").val() + "','contractno':'" + $("#txt_contractno").val() + "','submittime_s':'" + $("#txt_submittime_s").val()
                        + "','submittime_e':'" + $("#txt_submittime_e").val() + "','sitepasstime_s':'" + $("#txt_sitepasstime_s").val() + "','sitepasstime_e':'" + $("#txt_sitepasstime_e").val()
                        + "','start':" + lastIndex + ",'itemsPerLoad':" + itemsPerLoad + "}",
                    cache: false,
                    async: false,//默认是true，异步；false为同步，此方法执行完在执行下面代码
                    success: function (data) {
                        if (data.d == "") {
                            $("#span_sum").text("0");
                            $('#div_list').append("");
                            return;
                        }

                        var objdata = eval("(" + data.d + ")");//将字符串转为json                        
                        var obj = objdata[0]["data"];
                        $("#span_sum").text(objdata[0]["sum"]); 

                        var tb = "";
                        var blno_busi = "";//分提单号
                        for (var i = 0; i < obj.length; i++) {

                            blno_busi = "";
                            switch (obj[i]["BUSITYPE"]) {
                                case "10": case "11":
                                    if (obj[i]["TOTALNO"] == null && obj[i]["DIVIDENO"] == null) {
                                        blno_busi = "";
                                    }
                                    if (obj[i]["TOTALNO"] == null && obj[i]["DIVIDENO"] != null) {
                                        blno_busi = '_' + obj[i]["DIVIDENO"];
                                    }
                                    if (obj[i]["TOTALNO"] != null && obj[i]["DIVIDENO"] == null) {
                                        blno_busi = obj[i]["TOTALNO"];
                                    }
                                    if (obj[i]["TOTALNO"] != null && obj[i]["DIVIDENO"] != null) {
                                        blno_busi = obj[i]["TOTALNO"] + '_' + obj[i]["DIVIDENO"];
                                    }
                                    break;
                                case "20": case "21":
                                    blno_busi = obj[i]["SECONDLADINGBILLNO"] == null ? "" : obj[i]["SECONDLADINGBILLNO"];
                                    break;
                                case "30": case "31":
                                    blno_busi = obj[i]["LANDLADINGNO"] == null ? "" : obj[i]["LANDLADINGNO"];
                                    break;
                                case "40": case "41":
                                    blno_busi = obj[i]["ASSOCIATEPEDECLNO"] == null ? "" : obj[i]["ASSOCIATEPEDECLNO"];
                                    break;
                            }

                            tb = '<div class="list-block" id="order_' + obj[i]["CODE"] + '">'
                                    + '<ul>'
                                        + '<li class="item-content">'
                                             + '<div class="item-inner row">'
                                                + '<div class="item-title col-40">' + obj[i]["BUSIUNITNAME"] + '</div>'
                                                + '<div class="item-title col-25">' + getname("BUSITYPE", obj[i]["BUSITYPE"]) + '</div>'
                                                + '<div class="item-title col-33">' + obj[i]["CODE"] + '</div>'
                                            + '</div>'
                                        + '</li>'
                                        + '<li class="item-content">'
                                            + '<div class="item-inner row">'
                                                + '<div class="item-title col-40">' + blno_busi + '</div>'
                                                + '<div class="item-title col-25">' + (obj[i]["REPWAYNAME"] == null ? "" : obj[i]["REPWAYNAME"]) + '</div>'
                                                + '<div class="item-title col-33">' + (obj[i]["CUSNO"] == null ? "" : obj[i]["CUSNO"]) + '</div>'
                                            + '</div>'
                                        + '</li>'
                                        + '<li class="item-content">'
                                            + '<div class="item-inner row">'
                                                + '<div class="item-title col-40">' + (obj[i]["SITEAPPLYTIME"] == null ? "" : obj[i]["SITEAPPLYTIME"]) + '</div>'
                                                + '<div class="item-title col-25">' + obj[i]["GOODSNUM"] + '/' + obj[i]["GOODSGW"] + '</div>'
                                                + '<div class="item-title col-33">' + (obj[i]["CONTRACTNO"] == null ? "" : obj[i]["CONTRACTNO"]) + '</div>'
                                            + '</div>'
                                        + '</li>'
                                        + '<li class="item-content">'
                                            + '<div class="item-inner row">'
                                                + '<div class="item-title col-40">' + (obj[i]["DECLCHECKTIME"] == null ? "" : obj[i]["DECLCHECKTIME"]) + '</div>'
                                                + '<div class="item-title col-25">' + getname2("ISCHECK", obj[i]["ISCHECK"], obj[i]["AUDITFLAG"]) + '</div>'
                                                + '<div class="item-title col-33">' + (obj[i]["ASSOCIATENO"] == null ? "" : obj[i]["ASSOCIATENO"]) + '</div>'
                                            + '</div>'
                                        + '</li>'
                                        + '<li class="item-content">'
                                            + '<div class="item-inner row">'
                                                + '<div class="item-title col-40">' + (obj[i]["SITEPASSTIME"] == null ? "" : obj[i]["SITEPASSTIME"]) + '</div>'
                                                + '<div class="item-title col-25">' + getname("CHECKPIC", obj[i]["CHECKPIC"]) + '</div>'
                                                + '<div class="item-title col-33">' + (obj[i]["CORRESPONDNO"] == null ? "" : obj[i]["CORRESPONDNO"]) + '</div>'
                                            + '</div>'
                                        + '</li>'
                                    + '</ul>'
                             + '</div>';

                            $('#div_list').append(tb);
                            tb = ""; objname = "";
                        }

                    },
                    error: function (XMLHttpRequest, textStatus, errorThrown) {//请求失败处理函数
                        //alert(XMLHttpRequest.status);
                        //alert(XMLHttpRequest.readyState);
                        //alert(textStatus);
                        alert('error...状态文本值：' + textStatus + " 异常信息：" + errorThrown);
                    }
                });
            }

        });

        function getname(key, value) {
            var str = "";
            if (key == "BUSITYPE") {
                switch (value) {
                    case "10": str = "空运出口"; break;
                    case "11": str = "空运进口"; break;
                    case "20": str = "海运出口"; break;
                    case "21": str = "海运进口"; break;
                    case "30": str = "陆运出口"; break;
                    case "31": str = "陆运进口"; break;
                    case "40": str = "国内出口"; break;
                    case "41": str = "国内进口"; break;
                    case "50": str = "特殊出口"; break;
                    case "51": str = "特殊进口"; break;
                }
            }
            if (key == "CHECKPIC") {
                switch (value) {
                    case 0: str = ""; break;//无查验图
                    case 1: str = "有查验图"; break;
                    default: str = ""; break;//无查验图
                }
            }
            if (key == "MODIFYFLAG") {
                switch (value) {
                    case 0: str = "正常"; break;
                    case 1: str = "删单"; break;
                    case 2: str = "改单"; break;
                    case 3: str = "改单完成"; break;
                }
            }
            return str;
        }

        function getname2(key, value, value2) {
            var restr = ""; var str = ""; var str2 = "";
            if (key == "ISCHECK") {

                switch (value) {
                    case 0: str = ""; break;//未查验
                    case 1: str = "查验"; break;//已查验
                    default: str = ""; break;//未查验
                }

                switch (value2) {
                    case 0: str2 = ""; break;//未稽核
                    case 1: str2 = "稽核"; break;//已稽核
                    default: str2 = ""; break;//未稽核
                }

                if (str == "" && str2 == "") { restr = ""; }
                if (str == "" && str2 != "") { restr = "/" + str2; }
                if (str != "" && str2 == "") { restr = str; }
                if (str != "" && str2 != "") { restr = str + "/" + str2; }

            }
            return restr;
        }
        
        function initsearch_condition() {
            $("#picker_is_pass").picker({
                toolbarTemplate: '<header class="bar bar-nav">\
                      <button class="button button-link pull-right close-picker">确定</button>\
                      <h1 class="title">请选择放行情况</h1>\
                      </header>',
                cols: [
                  {
                      textAlign: 'center',
                      values: ['全部', '未放行', '已放行']
                  }
                ]
            });

            $("#picker_ischeck").picker({
                toolbarTemplate: '<header class="bar bar-nav">\
                      <button class="button button-link pull-right close-picker">确定</button>\
                      <h1 class="title">请选择查验情况</h1>\
                      </header>',
                cols: [
                  {
                      textAlign: 'center',
                      values: ['全部', '查验', '未查验']
                  }
                ]
            });

            //初始化时间控件
            //var before = new Date();
            //before.setDate(before.getDate() - 3);
            //var beforeday = before.Format("yyyy-MM-dd");

            //var now = new Date();
            //var today = now.Format("yyyy-MM-dd");

            //$("#txt_siteapplytime_s").val(beforeday);
            //$("#txt_siteapplytime_s").calendar({ value: [beforeday] });

            //$("#txt_siteapplytime_e").val(today);
            //$("#txt_siteapplytime_e").calendar({ value: [today] });
            $("#txt_siteapplytime_s").calendar({});
            $("#txt_siteapplytime_e").calendar({});
            
        }

        function getNowDate() {
            var nd = new Date();
            var y = nd.getFullYear();
            var m = nd.getMonth() + 1;
            var d = nd.getDate();
            var h = nd.getHours();
            var mi = nd.getMinutes();

            if (m <= 9) m = "0" + m;
            if (d <= 9) d = "0" + d;
            if (h <= 9) h = "0" + h;
            if (mi <= 9) mi = "0" + mi;

            return y + "" + m + "" + d + " " + h + ":" + mi;
        }

        //清除选中
        function clearSelect() {
            $("#div_list .list-block").each(function () {
                if ($(this).children("ul").css('background-color') == "rgb(193, 221, 241)") {
                    $(this).children("ul").css('background-color', '#fff');
                    $("#span_curchose").text(0);
                }
            });
        }

    </script>

</head>
<body>
    <div class="page-group">
        <div id="page-infinite-scroll-bottom" class="page page-current">
            <%--search --%>
            <header class="bar bar-nav" style="height:5.5rem;"> <%--style="height:5rem;"--%><%--暂时不用，就是查询背景色第二行--%>
                <div class="search-input">                    
                    <div class="row"> 
                        <div class="col-33" style="width:33%;font-size:13px;margin-top:.8rem;">现场报关日期始/末:</div>
                        <div class="col-33" style="width:26%;margin-left:0;"><input type="search" id='txt_siteapplytime_s'/></div>
                        <div class="col-33" style="width:26%;margin-left:0;"><input type="search" id='txt_siteapplytime_e'/></div>
                        <div class="col-10" style="width:10%;margin-left:0;margin-top:.2rem;">
                            <input id="btn_more_m" type="button" value="..." class="open-tabs-modal" style="background-color:#3D4145;color:#ffffff;border-radius:0;border:0;"  />
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-50" style="width:46%;"><input type="search" id='txt_declcode' placeholder='报关单号'/></div>
                        <div class="col-20" style="width:19%;margin-left:0;"><input type="search" id='txt_customareacode' placeholder='申报关区'/></div>
                        <div class="col-15" style="width:15%;margin-left:0;"><input type="search" id='picker_is_pass' placeholder='放行'/></div>
                        <div class="col-15" style="width:15%;margin-left:0;"><input type="search" id='picker_ischeck' placeholder='查验'/></div>
                    </div> 
                    <div class="row">
                        <div class="col-25" style="width:21%;"><input id="btn_gridname_m" type="button" value="列名" style="background-color:#808080;color:#ffffff;border-radius:0;border:0;" /></div>
                        <div class="col-60" style="width:54%;margin-left:0;"><input id="btn_search_m" type="button" value="查询" style="background-color:#3D4145;color:#ffffff;border-radius:0;border:0;" /></div>
                        <div class="col-25" style="width:21%;margin-left:0;"><input id="btn_reset_m" type="button" value="重置" style="background-color:#808080;color:#ffffff;border-radius:0;border:0;" /></div>
                    </div> 
                    <input type="hidden" id='txt_busitype'/><input type="hidden" id='txt_modifyflag'/><input type="hidden" id='txt_auditflag'/>
                    <input type="hidden" id='txt_busiunit'/>
                    <input type="hidden" id='txt_ordercode'/> <input type="hidden" id='txt_cusno'/>  
                    <input type="hidden" id='txt_divideno'/> <input type="hidden" id='txt_contractno'/>
                    <input type="hidden" id='txt_submittime_s'/><input type="hidden" id='txt_submittime_e'/>
                    <input type="hidden" id='txt_sitepasstime_s'/><input type="hidden" id='txt_sitepasstime_e'/>                   
                </div>  
                <div id="div_tbar" style="font-size:13px;margin:.2rem 0;">
                    <font color="#929292">共计</font>
                    <span id="span_sum">0</span>
                    <font color="#929292">笔,当前选中</font>
                    <span id="span_curchose">0</span>
                    <font color="#929292">笔</font>
                </div>  
            </header>

            <%--工具栏 --%>
            <nav class="bar bar-tab">
                <%--<a class="tab-item external" href="#" id="Siteapply_a">active
                    <span class="icon icon-friends"></span>
                    <span class="tab-label">现场报关</span>
                </a>--%>
                <a class="tab-item external" href="#" id="Siteapply_Pass_a">
                    <span class="icon icon-friends"></span>
                    <span class="tab-label">报关放行</span>
                </a>
                <a class="tab-item external" href="#" id="Detail_a">
                    <span class="icon icon-message"></span>
                    <span class="tab-label">报关详细</span>
                    <%--<span class="badge">2</span>--%>
                </a>
               <%-- <a class="tab-item external" href="#" id="Pass_a">
                    <span class="icon icon-cart"></span>
                    <span class="tab-label">报关放行</span>
                </a>--%>
                <a class="tab-item external" href="#" id="Check_a">
                    <span class="icon icon-check"></span>
                    <span class="tab-label">查验稽核</span>
                </a>
                <a class="tab-item external" href="#" id="Picture_a">
                    <span class="icon icon-picture"></span>
                    <span class="tab-label">查验图片<input type="hidden" id="hd_AdminUrl" value='<%= System.Configuration.ConfigurationManager.AppSettings["AdminUrl"] %>' /></span>
                </a>
                <a class="tab-item external" href="#" id="Subs_log_a">
                    <span class="icon icon-card"></span>
                    <span class="tab-label">消息订阅</span>
                </a>
                <a class="tab-item open-subinfo_busi" href="#" id="Subscribe_a"> 
                    <span class="icon icon-menu"></span>
                    <span class="tab-label">订阅清单</span>
                </a>
            </nav>

           <%--body --%>
            <div class="content infinite-scroll native-scroll" data-distance="100">
                <div id="div_list"></div>

                <!-- 加载提示符 -->
                <div class="infinite-scroll-preloader">
                  <div class="preloader"></div>
                </div>
                <div class="float-button" onclick="clearSelect()"><img src="../../image/clearbtn.png" /></div>
            </div>
            
        </div>
    </div>  

    <div class="popup popup-subscribe" id="popup-subscribe-log" >
        <div class="content" >
            <div class="buttons-tab">
                <input type="hidden" id="Pop_hd_ordercode" value="" /><input type="hidden" id="Pop_hd_cusno" value="" />
                <a href="#sub_tab1" class="tab-link active button">业务状态</a>
                <a href="#sub_tab2" class="tab-link button">物流状态</a>
            </div>
            <div class="content-block" id="popup-subscribe-log_con">
                <div class="tabs">
                    <div id="sub_tab1" class="tab active">
                        <div class="myrow">业务状态订阅</div>
                        <div class="row">
                            <div class="col-66">订单受理</div>
                            <div class="col-33"><input type="checkbox" name="chk_orderstatus" value="订单受理"/></div>
                        </div>
                        <div class="row">
                            <div class="col-66">申报开始</div>
                            <div class="col-33"><input type="checkbox" name="chk_orderstatus" value="申报开始"/></div>
                        </div>
                        <div class="row">
                            <div class="col-66">提前申报完成</div>
                            <div class="col-33"><input type="checkbox" name="chk_orderstatus" value="提前申报完成"/></div>
                        </div>
                        <div class="row">
                            <div class="col-66">申报完成</div>
                            <div class="col-33"><input type="checkbox" name="chk_orderstatus" value="申报完成"/></div>
                        </div>
                        <div class="row">
                            <div class="col-66">现场报关</div>
                            <div class="col-33"><input type="checkbox" name="chk_orderstatus" value="现场报关"/></div>
                        </div>
                        <div class="row">
                            <div class="col-66">现场放行</div>
                            <div class="col-33"><input type="checkbox" name="chk_orderstatus" value="现场放行"/></div>
                        </div>
                    </div>
                    <div id="sub_tab2" class="tab">
                        <div class="myrow">物流状态订阅</div>
                        <div class="row">
                            <div class="col-66">抽单完成</div>
                            <div class="col-33"><input type="checkbox" name="chk_logisticstatus" value="抽单完成"/></div>
                        </div>
                        <div class="row">
                            <div class="col-66">已派车</div>
                            <div class="col-33"><input type="checkbox" name="chk_logisticstatus" value="已派车"/></div>
                        </div>
                        <div class="row">
                            <div class="col-66">运输完成</div>
                            <div class="col-33"><input type="checkbox" name="chk_logisticstatus" value="运输完成"/></div>
                        </div>
                        <div class="row">
                            <div class="col-66">送货完成</div>
                            <div class="col-33"><input type="checkbox" name="chk_logisticstatus" value="送货完成"/></div>
                        </div>
                    </div>
                </div>
            </div>
            <div class="content-block row">
                <div class="col-50"><a href="#" class="close-popup button button-fill" style="background-color: gray;border-radius:0;color:white;border:0;">返  回</a></div>
                <div class="col-50"><a href="#" class="button button-fill" id="Pop_Subscribe_log" style="background-color: #3d4145;border-radius:0;color:white;border:0;">确  认</a></div>
            </div>
        </div>
    </div>   

    <!--popup 订阅详情弹出页-->
    <div class="popup pop-subscribeinfo">
        <div class="content" id="subscribeinfo" style="bottom: 3rem;">
        </div>
        <div style="bottom: 1.5rem; position: fixed; width: 80%; margin-left: 10%"><a href="#" class="close-popup button">返&nbsp;&nbsp;&nbsp;&nbsp;回</a></div>
    </div>

    <script type='text/javascript' src='//g.alicdn.com/msui/sm/0.6.2/js/sm.min.js' charset='utf-8'></script>   
    <%--<script type='text/javascript' src='//g.alicdn.com/msui/sm/0.6.2/js/sm-extend.min.js' charset='utf-8'></script>--%>
    <script src="/js/sm-extend.min.js"></script>
</body>

    
    <script type='text/javascript' src='http://res.wx.qq.com/open/js/jweixin-1.2.0.js'></script>
    <script type='text/javascript'>


        var conf = [];
        $.ajax({
            type: "post", //要用post方式                 
            url: "SiteDeclareList.aspx/getConf",//方法所在页面和方法名
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: "{'url':'" + window.location.href.split('#')[0] + "'}",
            cache: false,
            async: false,//默认是true，异步；false为同步，此方法执行完在执行下面代码
            success: function (data) {
                conf = eval("(" + data.d + ")");//将字符串转为json
            }
        });

        wx.config({
            debug: false,
            appId: conf.appid,
            timestamp: conf.timestamp,
            nonceStr: conf.noncestr,
            signature: conf.signature,
            jsApiList: ['chooseImage', 'previewImage', 'uploadImage', 'downloadImage']
        });

    </script>




</html>
