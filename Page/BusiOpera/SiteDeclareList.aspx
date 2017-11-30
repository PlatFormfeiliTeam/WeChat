<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SiteDeclareList.aspx.cs" Inherits="WeChat.Page.BusiOpera.SiteDeclareList" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<%--<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>--%>
    <meta charset="utf-8">
    <meta name="viewport" content="initial-scale=1, maximum-scale=1">
    <title>现场报关</title>
    <link href="/css/iconfont/iconfont.css" rel="stylesheet" />
    <link rel="stylesheet" href="//g.alicdn.com/msui/sm/0.6.2/css/sm.min.css">
    <link rel="stylesheet" href="//g.alicdn.com/msui/sm/0.6.2/css/??sm.min.css,sm-extend.min.css">
    <script type='text/javascript' src='//g.alicdn.com/sj/lib/zepto/zepto.min.js' charset='utf-8'></script>


    <style>
        #page-infinite-scroll-bottom .bar input[type=search]{
             margin:.2rem 0;
        }
        #page-infinite-scroll-bottom .bar .button {
            top:0;
        }
        #page-infinite-scroll-bottom .bar-nav~.content{
            top: 5rem;
        }
        #div_list .list-block{
            font-size:14px;
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
        /************************************************ 更多查询*********************************/
        .morediv{
            width: 98%;
            left: 1%;
            right: 1%;
            margin-left: 0px;
        }        
        .morediv .modal-inner{
            height:8.5rem;
        }

        /* 更多查询 第一个ul 样式*/
        .morediv .modal-inner .list-block:first-child ul:first-child li{
          float:left; width:25%;
        }
        .morediv .modal-inner .list-block:first-child label.label-checkbox i.icon-form-checkbox{
            width:.8rem;height:.8rem;
        }
        .morediv .modal-inner .list-block:first-child .item-text{
            height:1.1rem;font-size:.62rem;
        }
        .morediv .modal-inner .list-block:first-child label{
          padding-left:0px;
        }
        .morediv .modal-inner .list-block:first-child label .item-inner{
           margin-left:.2rem;padding-right:2px;
        }
        /************************************************ 查验标志*********************************/
        .checkdiv{
            width: 98%;
            left: 1%;
            right: 1%;
            margin-left: 0px;
        }  
        .checkdiv .modal-inner{
            height:15rem;
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

    </style>

    <script type="text/javascript">
        var userid = "763", username = "昆山吉时报关有限公司", openid = "ohNOmwZOt0tNr9WN7s1i7dHqOQnU";

        $(function () {
            initsearch_condition();

            var loading = false;
            var itemsPerLoad = 10;// 每次加载添加多少条目                
            var maxItems = 100;// 最多可加载的条目
            var lastIndex = 0;//$('.list-block').length;//.list-container li       

            $(document).on('click', '.open-preloader-title', function () {
                select();
            });

            function select() {
                $.showPreloader('加载中...');
                setTimeout(function () {
                    //首次查询需要置为初始值
                    $('#div_list').html("");
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

            $("#div_list").on('click', '.list-block', function (e) {// $("#div_list")也可以换成$(document)，是基于父容器的概念   
                if ($(this).children("ul").css('background-color') == "rgb(193, 221, 241)") {
                    $(this).children("ul").css('background-color', '#fff');
                } else {
                    $("#div_list .list-block ul").css('background-color', '#fff');
                    $(this).children("ul").css('background-color', '#C1DDF1');
                }
            });

            //报关单交接
            $("#Handover_a").click(function () {
                var divid = "";//order_
                $("#div_list .list-block").each(function () {
                    if ($(this).children("ul").css('background-color') == "rgb(193, 221, 241)") {
                        divid = $(this)[0].id;
                        //alert($(this).children("ul").children().eq(2).children("div").children().eq(0).text());
                    }
                });
                if (divid == "") {
                    $.toast("请选择需要交接的记录");
                    return;
                }
                if ($("#div_list #" + divid).children("ul").children().eq(2).children("div").children().eq(0).text() != "") {
                    $.toast("该笔记录已经交接，不能再次交接");
                    return;
                }


                $.confirm('请确认是否需要<font color=blue>交接</font>?',
                function () {//OK事件
                    $.ajax({
                        type: "post", //要用post方式                 
                        url: "SiteDeclareList.aspx/Handover",//方法所在页面和方法名
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
                        data: "{'ordercode':'" + divid.substring(6) + "'}",
                        cache: false,
                        async: false,//默认是true，异步；false为同步，此方法执行完在执行下面代码
                        success: function (data) {
                            if (data.d != "") {
                                $.toast("交接成功");
                                $("#div_list #" + divid).children("ul").children().eq(2).children("div").children().eq(0).text(data.d);//更新交接时间
                            } else {
                                $.toast("交接失败");
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

            //报关单详细
            $("#Detail_a").click(function () {
                var divid = "";//order_
                $("#div_list .list-block").each(function () {
                    if ($(this).children("ul").css('background-color') == "rgb(193, 221, 241)") {
                        divid = $(this)[0].id;
                        //alert($(this).children("ul").children().eq(4).children("div").children().eq(0).text());
                    }
                });
                if (divid == "") {
                    $.toast("请选择需要查看的记录");
                    return;
                }

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
                                                    + '<div class="item-title col-25">报关交接</div>'
                                                    + '<div class="item-title col-50">' + (jsonorder[0]["HANDOVERTIME"] == null ? "" : jsonorder[0]["HANDOVERTIME"]) + '</div>'
                                                    + '<div class="item-title col-25">' + (jsonorder[0]["HANDOVERUSERNAME"] == null ? "" : jsonorder[0]["HANDOVERUSERNAME"]) + '</div>'
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
                                                    + '<div class="item-title col-25">现场报关</div>'
                                                    + '<div class="item-title col-50">' + (jsonorder[0]["SITEAPPLYTIME"] == null ? "" : jsonorder[0]["SITEAPPLYTIME"]) + '</div>'
                                                    + '<div class="item-title col-25">' + (jsonorder[0]["SITEAPPLYUSERNAME"] == null ? "" : jsonorder[0]["SITEAPPLYUSERNAME"]) + '</div>'
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
                                                    + '<div class="item-title col-25">查验维护</div>'
                                                    + '<div class="item-title col-50">' + (jsonorder[0]["DECLCHECKTIME"] == null ? "" : jsonorder[0]["DECLCHECKTIME"]) + '</div>'
                                                    + '<div class="item-title col-25">' + (jsonorder[0]["DECLCHECKNAME"] == null ? "" : jsonorder[0]["DECLCHECKNAME"]) + '</div>'
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
                                                    + '<div class="item-title col-25">现场放行</div>'
                                                    + '<div class="item-title col-50">' + (jsonorder[0]["SITEPASSTIME"] == null ? "" : jsonorder[0]["SITEPASSTIME"]) + '</div>'
                                                    + '<div class="item-title col-25">' + (jsonorder[0]["SITEPASSUSERNAME"] == null ? "" : jsonorder[0]["SITEPASSUSERNAME"]) + '</div>'
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
                                                    + '<div class="item-title col-25">件数</div>'
                                                    + '<div class="item-title col-25">毛重</div>'
                                                + '</div>'
                                            + '</li>'
                                            + '<li class="item-content" style="min-height:1.3rem;height:1.3rem;">'
                                                + '<div class="item-inner row" style="min-height:1.3rem;height:1.3rem;border-top:2px solid #0894EC;border-left:2px solid #0894EC;border-right:2px solid #0894EC;">'
                                                    + '<div class="item-title col-50">运输工具名称</div>'
                                                    + '<div class="item-title col-25">监管方式</div>'
                                                    + '<div class="item-title col-25">删改单</div>'
                                                + '</div>'
                                            + '</li>'
                                            + '<li class="item-content" style="min-height:1.3rem;height:1.3rem;">'
                                                + '<div class="item-inner row" style="min-height:1.3rem;height:1.3rem;border:2px solid #0894EC;">'
                                                    + '<div class="item-title col-50">海关状态</div>'
                                                    + '<div class="item-title col-25"></div>'
                                                    + '<div class="item-title col-25"></div>'
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
                                                        + '<div class="item-title col-25">' + (jsondecl[i]["GOODSNUM"] == null ? "" : jsondecl[i]["GOODSNUM"]) + '</div>'
                                                        + '<div class="item-title col-25">' + (jsondecl[i]["GOODSGW"] == null ? "" : jsondecl[i]["GOODSGW"]) + '</div>'
                                                    + '</div>'
                                                + '</li>'
                                                + '<li class="item-content" style="min-height:1.3rem;height:1.3rem;">'
                                                    + '<div class="item-inner row" style="min-height:1.3rem;height:1.3rem;">'
                                                        + '<div class="item-title col-50">' + (jsondecl[i]["TRANSNAME"] == null ? "" : jsondecl[i]["TRANSNAME"]) + '</div>'
                                                        + '<div class="item-title col-25">' + (jsondecl[i]["TRADECODE"] == null ? "" : jsondecl[i]["TRADECODE"]) + '</div>'
                                                        + '<div class="item-title col-25">' + getname("MODIFYFLAG", jsondecl[i]["MODIFYFLAG"]) + '</div>'
                                                    + '</div>'
                                                + '</li>'
                                                + '<li class="item-content" style="min-height:1.3rem;height:1.3rem;">'
                                                    + '<div class="item-inner row" style="min-height:1.3rem;height:1.3rem;">'
                                                        + '<div class="item-title col-50">' + (jsondecl[i]["CUSTOMSSTATUS"] == null ? "" : jsondecl[i]["CUSTOMSSTATUS"]) + '</div>'
                                                        + '<div class="item-title col-25"></div>'
                                                        + '<div class="item-title col-25"></div>'
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
                                        '<div class="content-block"><a href="#" class="close-popup button button-fill button-danger">返回</a></div>' +
                                 '</div>' +
                             '</div>';
                $.popup(popupHTML);

            });

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

            //查验标志
            $("#Check_a").click(function () {
                var divid = "";//order_
                $("#div_list .list-block").each(function () {
                    if ($(this).children("ul").css('background-color') == "rgb(193, 221, 241)") {
                        divid = $(this)[0].id;
                        //alert($(this).children("ul").children().eq(2).children("div").children().eq(0).text());
                    }
                });
                if (divid == "") {
                    $.toast("请选择需要查验标志的记录");
                    return;
                }
               
                var strconHTML = "";
                strconHTML = '<font class="title"><b>报关查验维护</b></font>';

                strconHTML += '<div class="list-block" style="margin:0; margin-top:2rem;margin-buttom:2rem;margin-left:4%;margin-right:4%;line-height:1.5rem;font-size:.7rem">' +
                                    '<div class="row"> ' +
                                        '<div class="col-33">订单编号：</div>' +
                                        '<div class="col-66">' + divid.substring(6) + '</div>' +
                                    '</div> ' +
                                    '<div class="row"> ' +
                                        '<div class="col-33">企业编号：</div>' +
                                        '<div class="col-66">' + $("#div_list #" + divid).children("ul").children().eq(1).children("div").children().eq(2).text() + '</div>' +
                                    '</div> ' +
                                    '<div class="row"> ' +
                                        '<div class="col-33">查验维护时间：</div>' +
                                        '<div class="col-66"><input type="text" style="background:#c7c7cc;height:1.2rem;font-size:.7rem" id="txt_declchecktime" readonly /></div>' +
                                    '</div> ' +
                                        '<div class="row"> ' +
                                        '<div class="col-33">查验维护人员：<input type="hidden" id="txt_declcheckid" readonly /></div>' +
                                        '<div class="col-66"><input type="text" style="background:#c7c7cc;height:1.2rem;font-size:.7rem" id="txt_declcheckname" readonly /></div>' +
                                    '</div> ' +
                                '</div>';
                strconHTML += '<div class="list-block" style="margin:0;font-size:.7rem;margin-left:4%;margin-right:4%;">' +
                                 '<ul>' +
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
                                              '<ul>' +
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

                var strconButton = '<div class="content-block">' +
                                        '<div class="row"> ' +
                                            '<div class="col-33"><a href="#" id="checkcancel" class="button button-fill button-warning">撤销标志</a></div>' +
                                            '<div class="col-33"><a href="#" id="checksave" class="button button-fill">查验标志</a></div>' +
                                            '<div class="col-33"><a href="#" class="close-popup button button-fill button-danger">返回</a></div>' +
                                        '</div>' +
                                    '</div>';

                var popupHTML = '<div class="popup">' +
                                 '<div class="content">' +//data-type='native'                                                                               
                                        strconHTML +
                                        strconButton +
                                 '</div>' +
                             '</div>';

                $.popup(popupHTML);
                
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
                
                //$("#txt_declchecktime").datetimePicker({ value: [y, m, d, h, mi] });//此行不用 ，用下一行代码，因为是只读，不允许操作
                $("#txt_declchecktime").val(y + "" + m + "" + d + " " + h + ":" + mi);//初始化日期时间

                $("#txt_declcheckid").val("763");//当前登录人id
                $("#txt_declcheckname").val("昆山吉时报关有限公司");//当前登录人name

                $("#checkcancel").click(function () {//初始化注册事件，必须是在HTML生成之后才能注册，否则无效
                    $.confirm('请确认是否需要<font color=blue>撤销查验</font>?',
                         function () {//OK事件
                             $.ajax({
                                 type: "post", //要用post方式                 
                                 url: "SiteDeclareList.aspx/checkcancel",//方法所在页面和方法名
                                 contentType: "application/json; charset=utf-8",
                                 dataType: "json",
                                 data: "{'ordercode':'" + divid.substring(6) + "'}",
                                 cache: false,
                                 async: false,//默认是true，异步；false为同步，此方法执行完在执行下面代码
                                 success: function (data) {
                                     if (data.d == "sucess") {
                                         $.toast("撤销成功");
                                         $("#div_list #" + divid).children("ul").children().eq(3).children("div").children().eq(0).text("");//更新查验时间
                                         $("#div_list #" + divid).children("ul").children().eq(3).children("div").children().eq(1).text("否");
                                     } else {
                                         $.toast("撤销失败");
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

                $("#checksave").click(function () {//初始化注册事件，必须是在HTML生成之后才能注册，否则无效
                    $.confirm('请确认是否需要<font color=blue>查验</font>?',
                        function () {//OK事件
                            $.ajax({
                                type: "post", //要用post方式                 
                                url: "SiteDeclareList.aspx/checksave",//方法所在页面和方法名
                                contentType: "application/json; charset=utf-8",
                                dataType: "json",
                                data: "{'ordercode':'" + divid.substring(6) + "','checktime':'" + $("#txt_declchecktime").val()
                                    + "','checkname':'" + $("#txt_declcheckname").val() + "','checkid':'" + $("#txt_declcheckid").val() + "'}",
                                cache: false,
                                async: false,//默认是true，异步；false为同步，此方法执行完在执行下面代码
                                success: function (data) {
                                    if (data.d != "") {
                                        $.toast("查验成功");
                                        $("#div_list #" + divid).children("ul").children().eq(3).children("div").children().eq(0).text(data.d);//更新查验时间
                                        $("#div_list #" + divid).children("ul").children().eq(3).children("div").children().eq(1).text("是");
                                    } else {
                                        $.toast("查验失败");
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

            });

            //查验图片
            $("#Picture_a").click(function () {
                var divid = "";//order_
                $("#div_list .list-block").each(function () {
                    if ($(this).children("ul").css('background-color') == "rgb(193, 221, 241)") {
                        divid = $(this)[0].id;
                    }
                }); 
                if (divid == "") {
                    $.toast("请选择需要查验图片的记录");
                    return;
                }

                if ($("#div_list #" + divid).children("ul").children().eq(3).children("div").children().eq(1).text() != "是") {
                    $.toast("无查验标志，不能使用查验图片功能");
                    return;
                }                

                $.modal({
                    title: '查验图片',
                    text: '<div class="content-block row">' +
                                '<div class="col-50"><a href="#" id="picfileupload" class="button button-fill">上传</a></div>' +
                                '<div class="col-50"><a href="#" id="picfileconsult" class="button button-fill">调阅</a></div>' +
                            '</div>',
                    extraClass: 'picdiv'//避免直接设置.modal的样式，从而影响其他toast的提示
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
                                    $("#div_list #" + divid).children("ul").children().eq(4).children("div").children().eq(1).text("是");
                                } else {
                                    $.toast("上传失败");
                                }
                            }
                        });
                    }

                });

                $("#picfileconsult").click(function () {

                    $.closeModal(".picdiv");

                    if ($("#div_list #" + divid).children("ul").children().eq(4).children("div").children().eq(1).text() != "是") {
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
                var divid = "";//order_
                var cusno = "";
                $("#div_list .list-block").each(function () {
                    if ($(this).children("ul").css('background-color') == "rgb(193, 221, 241)") {
                        divid = $(this)[0].id;
                        cusno = $(this).children("ul").children().eq(1).children("div").children().eq(2).text();
                    }
                });
                if (divid == "") {
                    $.toast("请选择需要订阅的记录");
                    return;
                }
                $.popup("#popup-subscribe-log");

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
                    data: "{'type':'" + type + "','status':'" + status + "','cusno':'" + cusno + "','declarationcode':'','userid':'" + userid + "','username':'" + username + "','openid':'" + openid + "'}",
                    cache: false,
                    async: false,//默认是true，异步；false为同步，此方法执行完在执行下面代码
                    success: function (data) {
                        $.toast(data.d);
                    }
                });

            });

            $.init();
            //----------------------------------------------------------------------------------------------------------------------------------------
            function loaddata(itemsPerLoad, lastIndex) {
                $.ajax({
                    type: "post", //要用post方式                 
                    url: "SiteDeclareList.aspx/BindList",//方法所在页面和方法名
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    data: "{'inout_type':'" + $("#picker_inout_type").val() + "','issiterep':'" + $("#picker_is_siterep").val() + "','busitype':'" + $("#picker_busitype").val()
                        + "','ispass':'" + $("#picker_is_pass").val() + "','startdate':'" + $("#txt_startdate").val() + "','enddate':'" + $("#txt_enddate").val()
                        + "','radiotype':'" + $("#txt_radio_type_hidden").val() + "','morecon':'" + $("#txt_morecon_hidden").val() 
                        + "','start':" + lastIndex + ",'itemsPerLoad':" + itemsPerLoad + "}",
                    cache: false,
                    async: false,//默认是true，异步；false为同步，此方法执行完在执行下面代码
                    success: function (data) {
                        var obj = eval("(" + data.d + ")");//将字符串转为json

                        

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
                                                + '<div class="item-title col-25">' + obj[i]["REPWAYNAME"] + '</div>'
                                                + '<div class="item-title col-33">' + obj[i]["CUSNO"] + '</div>'
                                            + '</div>'
                                        + '</li>'
                                        + '<li class="item-content">'
                                            + '<div class="item-inner row">'
                                                + '<div class="item-title col-40">' + (obj[i]["HANDOVERTIME"] == null ? "" : obj[i]["HANDOVERTIME"]) + '</div>'
                                                + '<div class="item-title col-25">' + obj[i]["GOODSNUM"] + '/' + obj[i]["GOODSGW"] + '</div>'
                                                + '<div class="item-title col-33">' + obj[i]["CONTRACTNO"] + '</div>'
                                            + '</div>'
                                        + '</li>'
                                        + '<li class="item-content">'
                                            + '<div class="item-inner row">'
                                                + '<div class="item-title col-40">' + (obj[i]["DECLCHECKTIME"] == null ? "" : obj[i]["DECLCHECKTIME"]) + '</div>'
                                                + '<div class="item-title col-25">' + getname("ISCHECK", obj[i]["ISCHECK"]) + '</div>'
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
            if (key == "ISCHECK") {
                switch (value) {
                    case 0: str = "未查验"; break;
                    case 1: str = "已查验"; break;
                    default: str = "未查验"; break;
                }
            }
            if (key == "CHECKPIC") {
                switch (value) {
                    case 0: str = "无查验图"; break;
                    case 1: str = "有查验图"; break;
                    default: str = "无查验图"; break;
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
        
        function initsearch_condition() {
            $("#picker_inout_type").picker({
                toolbarTemplate: '<header class="bar bar-nav">\
                      <button class="button button-link pull-right close-picker">确定</button>\
                      <h1 class="title">请选择进出口</h1>\
                      </header>',
                cols: [
                  {
                      textAlign: 'center',
                      values: ['全部', '进口', '出口']
                  }
                ]
            });

            $("#picker_is_siterep").picker({
                toolbarTemplate: '<header class="bar bar-nav">\
                      <button class="button button-link pull-right close-picker">确定</button>\
                      <h1 class="title">请选择现场申报</h1>\
                      </header>',
                cols: [
                  {
                      textAlign: 'center',
                      values: ['全部', '仅现场']
                  }
                ]
            });

            $("#picker_busitype").picker({
                toolbarTemplate: '<header class="bar bar-nav">\
                      <button class="button button-link pull-right close-picker">确定</button>\
                      <h1 class="title">请选择业务类型</h1>\
                      </header>',
                cols: [
                  {
                      textAlign: 'center',
                      values: ['全部', '空运', '海运', '陆运', '国内', '特殊区域']
                  }
                ]
            });

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

            $("#txt_startdate").calendar({});
            $("#txt_enddate").calendar({});

            $(document).on('click', '.open-tabs-modal', function () {
                $.modal({
                    title: '更多查询',
                    text: '<div class="list-block" style="margin:0;">' +
                              '<ul>' +
                                '<li>' +
                                    '<label class="label-checkbox item-content">' +
                                        '<input type="radio" name="radio_type" value="报关单号" checked>' +
                                        '<div class="item-media"><i class="icon icon-form-checkbox"></i></div>' +
                                        '<div class="item-inner">' +
                                            '<div class="item-text">报关单号</div>' +
                                        '</div>' +
                                    '</label>' +
                                '</li>' +
                                '<li>' +
                                    '<label class="label-checkbox item-content">' +
                                        '<input type="radio" name="radio_type" value="收发货人">' +
                                        '<div class="item-media"><i class="icon icon-form-checkbox"></i></div>' +
                                        '<div class="item-inner">' +
                                            '<div class="item-text">收发货人</div>' +
                                        '</div>' +
                                    '</label>' +
                                '</li>' +
                                '<li>' +
                                    '<label class="label-checkbox item-content">' +
                                        '<input type="radio" name="radio_type" value="客户编号">' +
                                        '<div class="item-media"><i class="icon icon-form-checkbox"></i></div>' +
                                        '<div class="item-inner">' +
                                            '<div class="item-text">客户编号</div>' +
                                        '</div>' +
                                    '</label>' +
                                '</li>' +
                                '<li>' +
                                    '<label class="label-checkbox item-content">' +
                                        '<input type="radio" name="radio_type" value="业务编号">' +
                                        '<div class="item-media"><i class="icon icon-form-checkbox"></i></div>' +
                                        '<div class="item-inner">' +
                                            '<div class="item-text">业务编号</div>' +
                                        '</div>' +
                                    '</label>' +
                                '</li>' +
                            '</ul>' +
                        '</div>' +
                        '<div class="list-block" style="margin:0;">' +
                            '<input type="text" style="background:#fff;height:1.5rem;font-size:.62rem" id="txt_morecon" value="' + $("#txt_morecon_hidden").val() + '" />' +
                            '<p id="p_morecon" style="font-size:.62rem;text-align:left;">注意：请输入报关单后9位或18位报关号</p>' +
                        '</div>',
                    buttons: [
                     {
                         text: '确认', bold: true,
                         onClick: function () {
                             $("#txt_radio_type_hidden").val($("input[name='radio_type']:checked").val());//单选类别
                             $("#txt_morecon_hidden").val($("#txt_morecon").val());//文本框值
                             //select();//暂时不要用，默认点查询按钮才查询
                         }
                     },
                     {
                         text: '取消', bold: true,
                         onClick: function () { }
                     },
                     {
                         text: '重置', bold: true,
                         onClick: function () {
                             $("#picker_inout_type").picker("setValue", ["全部"]); $("#picker_is_siterep").picker("setValue", ["仅现场"]);
                             $("#picker_busitype").picker("setValue", ["全部"]); $("#picker_is_pass").picker("setValue", ["未放行"]);
                             $("#txt_startdate").val(""); $("#txt_enddate").val("");
                             $("#txt_startdate").calendar({}); $("#txt_enddate").calendar({});//否则之前选的那天  不能再次选中

                             //$("input[name='radio_type']").prop('checked', false); $("#txt_morecon").val("");//因每次窗口都是新开的，可以不用置空，置空隐藏值即可
                             $("#txt_radio_type_hidden").val(""); $("#txt_morecon_hidden").val("");
                         }
                     }
                    ],
                    extraClass: 'morediv'//避免直接设置.modal的样式，从而影响其他toast的提示
                });
                
                $('input[name="radio_type"]').change(function (e) {
                    var radio_type_checked = $("input[name='radio_type']:checked").val();
                    if (radio_type_checked == "报关单号") { $("#p_morecon").text("注意：请输入报关单后9位或18位报关号"); }
                    if (radio_type_checked == "收发货人") { $("#p_morecon").text("注意：请输入4位以上连续名称"); }
                    if (radio_type_checked == "客户编号") { $("#p_morecon").text("注意：请输入后5位以上的号码"); }
                    if (radio_type_checked == "业务编号") { $("#p_morecon").text("注意：请输入后5位以上的编号"); }
                });

                //radio 初始化上次 点击 确认后 选中的值（文本框的默认值上面直接绑定在value上了）
                if ($("#txt_radio_type_hidden").val() != "") {
                    $('input[name="radio_type"][value="' + $("#txt_radio_type_hidden").val() + '"]').prop("checked", true);
                    $('input[name="radio_type"]').trigger('change');//触发change事件
                }

            });
        }

    </script>

</head>
<body>
    <div class="page-group">
        <div id="page-infinite-scroll-bottom" class="page page-current">
            <%--search --%>
            <header class="bar bar-nav"> <%--style="height:5rem;"--%><%--暂时不用，就是查询背景色第二行--%>
                <div class="search-input">                    
                    <div class="row"> 
                        <div class="col-25"><input type="search" id='picker_inout_type' placeholder='进出口'/></div> <%--value="全部"--%>
                        <div class="col-25"><input type="search" id='picker_is_siterep' placeholder='现场申报'/></div><%--value="仅现场"--%>
                        <div class="col-25"><input type="search" id='picker_busitype' placeholder='业务类型'/></div> <%--value="全部"--%>
                        <div class="col-25"><input type="search" id='picker_is_pass' placeholder='放行情况'/></div> <%--value="未放行"--%>
                    </div>
                    <div class="row">
                        <div class="col-40"><input type="search" id='txt_startdate' placeholder='委托起始日期'/></div>
                        <div class="col-5">~</div>
                        <div class="col-40"><input type="search" id='txt_enddate' placeholder='委托结束日期'/></div>
                        <div class="col-15"><a href="#" class="open-tabs-modal"><i class="iconfont" style="font-size:1.3rem;color:gray;">&#xe6ca;</i></a></div>
                    </div>                    
                </div>  
                <input type="hidden" id='txt_radio_type_hidden'/><input type="hidden" id='txt_morecon_hidden'/>              
                <a href="#" id="search_a" class="open-preloader-title button button-fill">查询</a>   
            </header>

            <%--工具栏 --%>
            <nav class="bar bar-tab">
                <a class="tab-item external" href="#" id="Handover_a"><%--active--%>
                    <span class="icon icon-friends"></span>
                    <span class="tab-label">报关单交接</span>
                </a>
                <a class="tab-item external" href="#" id="Detail_a">
                    <span class="icon icon-message"></span>
                    <span class="tab-label">报关单详细</span>
                    <%--<span class="badge">2</span>--%>
                </a>
                <a class="tab-item external" href="#" id="Pass_a">
                    <span class="icon icon-cart"></span>
                    <span class="tab-label">报关放行</span>
                </a>
                <a class="tab-item external" href="#" id="Check_a">
                    <span class="icon icon-check"></span>
                    <span class="tab-label">查验标志</span>
                </a>
                <a class="tab-item external" href="#" id="Picture_a">
                    <span class="icon icon-picture"></span>
                    <span class="tab-label">查验图片<input type="hidden" id="hd_AdminUrl" value='<%= System.Configuration.ConfigurationManager.AppSettings["AdminUrl"] %>' /></span>
                </a>
                <a class="tab-item external" href="#" id="Subs_log_a">
                    <span class="icon icon-card"></span>
                    <span class="tab-label">消息订阅</span>
                </a>
            </nav>

           <%--body --%>
            <div class="content infinite-scroll native-scroll" data-distance="100">
                <div id="div_list"></div>

                <!-- 加载提示符 -->
                <div class="infinite-scroll-preloader">
                  <div class="preloader"></div>
                </div>
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
                <div class="col-50"><a href="#" class="button button-fill" id="Pop_Subscribe_log">确  认</a></div>
                <div class="col-50"><a href="#" class="close-popup button button-fill">返  回</a></div>
            </div>
        </div>
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
