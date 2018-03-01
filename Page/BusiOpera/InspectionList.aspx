<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="InspectionList.aspx.cs" Inherits="WeChat.Page.BusiOpera.InspectionList" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<%-- <meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>--%>
    <meta charset="utf-8">
    <meta name="viewport" content="initial-scale=1, maximum-scale=1">
    <title>报检单查询</title>
    <link href="/css/iconfont/iconfont.css?t=<%=ConfigurationManager.AppSettings["Version"]%>" rel="stylesheet" />
    <link rel="stylesheet" href="//g.alicdn.com/msui/sm/0.6.2/css/sm.min.css">
    <link rel="stylesheet" href="//g.alicdn.com/msui/sm/0.6.2/css/??sm.min.css,sm-extend.min.css">
    <script type='text/javascript' src='//g.alicdn.com/sj/lib/zepto/zepto.min.js' charset='utf-8'></script>

    <link rel="stylesheet" href="/css/extraSearch.css?t=<%=ConfigurationManager.AppSettings["Version"]%>" />    
    <script type="text/javascript" src="/js/extraSearch.js?t=<%=ConfigurationManager.AppSettings["Version"]%>" ></script>

    <style>
        #page-infinite-scroll-bottom .bar input[type=search]{
             margin:.2rem 0;
        }
        #page-infinite-scroll-bottom .bar .button {
            top:0;
        }
        #page-infinite-scroll-bottom .bar-nav ~ .content {
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
        .filediv .modal-title{
            font-weight:800;
        }
         /************************************************ 查询列表名称*********************************/
        .girdnamediv {
            width: 98%;
            left: 1%;
            right: 1%;
            margin-left: 0px;
            text-align: left;
            top: 3.1rem;
        } 
        .girdnamediv .modal-inner{
           padding:0px;
        }  
    </style>

    <script type="text/javascript">
        $(function () {
            //---------------------------------------------------------------------------------------------------------------列表名称
            function showGridName() {
                var strname = '<div class="list-block" style="margin:0;font-size:small;">'
                            + '<ul>'
                                + '<li class="item-content" style="min-height:1.3rem;height:1.3rem;">'
                                    + '<div class="item-inner row" style="min-height:1.3rem;height:1.3rem;">'//border-top:2px solid #0894EC;border-left:2px solid #0894EC;border-right:2px solid #0894EC;
                                        + '<div class="item-title col-50">报检单号</div>'
                                        + '<div class="item-title col-33">业务类型</div>'
                                        + '<div class="item-title col-20">监管方式</div>'
                                    + '</div>'
                                + '</li>'
                                + '<li class="item-content" style="min-height:1.3rem;height:1.3rem;">'
                                    + '<div class="item-inner row" style="min-height:1.3rem;height:1.3rem;">'//border-top:2px solid #0894EC;border-left:2px solid #0894EC;border-right:2px solid #0894EC;
                                        + '<div class="item-title col-50">收发货人</div>'
                                        + '<div class="item-title col-33">合同号</div>'
                                        + '<div class="item-title col-20">申报日期</div>'
                                    + '</div>'
                                + '</li>'
                                + '<li class="item-content" style="min-height:1.3rem;height:1.3rem;">'
                                    + '<div class="item-inner row" style="min-height:1.3rem;height:1.3rem;">'//border-top:2px solid #0894EC;border-left:2px solid #0894EC;border-right:2px solid #0894EC;
                                        + '<div class="item-title col-50">报检流水号</div>'
                                        + '<div class="item-title col-33">件数/毛重</div>'
                                        + '<div class="item-title col-20">删改单</div>'
                                    + '</div>'
                                + '</li>'
                                + '<li class="item-content" style="min-height:1.3rem;height:1.3rem;">'
                                    + '<div class="item-inner row" style="min-height:1.3rem;height:1.3rem;">'//border:2px solid #0894EC;
                                        + '<div class="item-title col-50">通关单号</div>'
                                        + '<div class="item-title col-33">企业编号</div>'
                                        + '<div class="item-title col-20">国检状态</div>'
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

            //----------------------------------------------------------------------------------------------------------------查询条件
            initsearch_condition();
            initSerach_inspection();

            //---------------------------------------------------------------------------------------------------------------------
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
                $("#txt_reptime_s").val(""); $("#txt_reptime_e").val("");
                $("#txt_reptime_s").calendar({}); $("#txt_reptime_e").calendar({});//否则之前选的那天  不能再次选中

                $("#txt_inspcode").val("");
                $("#picker_MODIFYFLAG").picker("setValue", ["全部"]);

                $("#txt_busitype").val(""); $("#txt_ischeck").val(""); $("#txt_ispass").val("");
                $("#txt_lawflag").val(""); $("#txt_isneedclearance").val("");

                $("#txt_busiunit").val(""); $("#txt_contractno").val("");
                $("#txt_ordercode").val(""); $("#txt_cusno").val("");
                $("#txt_divideno").val(""); $("#txt_customareacode").val(""); $("#txt_approvalcode").val();
                
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

            //条码扫描
            $("#btn_barcode").click(function () {
                wx.ready(function () {
                    wx.scanQRCode({
                        needResult: 1, // 默认为0，扫描结果由微信处理，1则直接返回扫描结果，
                        scanType: ["qrCode", "barCode"], // 可以指定扫二维码还是一维码，默认二者都有
                        success: function (res) {
                            var result = res.resultStr; // 当needResult 为 1 时，扫码返回的结果
                            var serial = result.split(",");
                            var serialNumber = serial[serial.length - 1];
                            $("#txt_inspcode").val(serialNumber);
                        }
                    });

                });
                //初始化jsapi接口 状态
                wx.error(function (res) {
                    alert("调用微信jsapi返回的状态:" + res.errMsg);
                });
            });

            //删改单维护
            $("#ModifyEdit_a").click(function () {
                var curcount = parseInt($("#span_curchose").text());
                if (curcount != 1) {
                    $.toast("请选择一笔删改单记录");
                    return;
                }

                var preinspcode = "";
                $("#div_list .list-block").each(function () {
                    if ($(this).children("ul").css('background-color') == "rgb(193, 221, 241)") {
                        preinspcode = $(this)[0].id;
                        //alert($(this).children("ul").children().eq(2).children("div").children().eq(2).text());
                    }
                });
                //if (preinspcode == "") {
                //    $.toast("请选择需要维护的记录");
                //    return;
                //}

                var buttons1 = [
                    {
                        text: '<font style="font-weight:800">删改单维护，请选择</font>',
                        label: true
                    },
                    {
                        text: '删单', bold: true, //color: 'danger',
                        onClick: function () {
                            modifySave(preinspcode, "删单", 1);
                        }
                    },
                    {
                        text: '改单', bold: true,
                        onClick: function () {
                            modifySave(preinspcode, "改单", 2);
                        }
                    },
                    {
                        text: '改单完成', bold: true,
                        onClick: function () {
                            modifySave(preinspcode, "改单完成", 3);
                        }
                    }];
                var buttons2 = [
                          {
                              text: '取消', bg: 'danger'
                          }
                ];
                var groups = [buttons1, buttons2];
                $.actions(groups);
            });

            //关联报检单查询
            $("#Ass_a").click(function () {
                var curcount = parseInt($("#span_curchose").text());
                if (curcount != 1) {
                    $.toast("请选择一笔关联报检单记录");
                    return;
                }

                var preinspcode = "";
                $("#div_list .list-block").each(function () {
                    if ($(this).children("ul").css('background-color') == "rgb(193, 221, 241)") {
                        preinspcode = $(this)[0].id;
                    }
                });
                //if (preinspcode == "") {
                //    $.toast("请选择需要关联的记录");
                //    return;
                //}

                var busitypename_con = $("#div_list #" + preinspcode).children("ul").children().eq(0).children("div").children().eq(1).text();
                if (busitypename_con != "国内出口" && busitypename_con != "国内进口") {
                    $.toast("请选择国内业务");
                    return;
                }

                $.ajax({
                    type: "post", //要用post方式                 
                    url: "InspectionList.aspx/AssCon",//方法所在页面和方法名
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    data: "{'preinspcode':'" + preinspcode + "'}",
                    cache: false,
                    async: false,
                    success: function (data) {
                        var obj = eval("(" + data.d + ")");//将字符串转为json

                        if (obj.length <= 0) {
                            $.toast("没有关联报检单");
                            return;
                        }

                        var buttons1 = [
                            {
                                text: '<font style="font-weight:800">关联报检单信息</font>',
                                label: true
                            },
                            {
                                text: '<div class="list-block" style="margin:0;font-size:13px;color:black;">'
                                        + '<ul>'
                                           + '<li class="item-content" style="min-height:1.3rem;height:1.3rem;">' +
                                                  '<div class="item-inner row" style="min-height:1.3rem;height:1.3rem;">'
                                                    + '<div class="item-title col-50">' + (obj[i]["INSPECTIONCODE"] == null ? "" : obj[i]["INSPECTIONCODE"]) + '</div>'
                                                    + '<div class="item-title col-33">' + getname("BUSITYPE", obj[i]["BUSITYPE"]) + '</div>'
                                                    + '<div class="item-title col-20">' + (obj[i]["TRADEWAYNAME"] == null ? "" : obj[i]["TRADEWAYNAME"]) + '</div>'
                                                + '</div>'
                                           + '</li>'
                                    + '</ul>'
                                + '</div>'
                            },
                            {
                                text: '<div class="list-block" style="margin:0;font-size:13px;color:black;">'
                                        + '<ul>'
                                            + '<li class="item-content" style="min-height:1.3rem;height:1.3rem;">'
                                                + '<div class="item-inner row" style="min-height:1.3rem;height:1.3rem;">'
                                                    + '<div class="item-title col-50">' + (obj[i]["BUSIUNITNAME"] == null ? "" : obj[i]["BUSIUNITNAME"]) + '</div>'
                                                    + '<div class="item-title col-33">' + (obj[i]["CONTRACTNO"] == null ? "" : obj[i]["CONTRACTNO"]) + '</div>'
                                                    + '<div class="item-title col-20">' + (obj[i]["REPTIME"] == null ? "" : obj[i]["REPTIME"]) + '</div>'
                                                + '</div>'
                                            + '</li>'
                                    + '</ul>'
                                + '</div>'
                            },
                            {
                                text: '<div class="list-block" style="margin:0;font-size:13px;color:black;">'
                                        + '<ul>'
                                            + '<li class="item-content" style="min-height:1.3rem;height:1.3rem;">'
                                                + '<div class="item-inner row" style="min-height:1.3rem;height:1.3rem;">'
                                                    + '<div class="item-title col-50">' + (obj[i]["APPROVALCODE"] == null ? "" : obj[i]["APPROVALCODE"]) + '</div>'
                                                    + '<div class="item-title col-33">' + (obj[i]["GOODSNUM"] == null ? "" : obj[i]["GOODSNUM"]) + '/' + (obj[i]["GOODSGW"] == null ? "" : obj[i]["GOODSGW"]) + '</div>'
                                                    + '<div class="item-title col-20">' + getname("MODIFYFLAG", obj[i]["MODIFYFLAG"]) + '</div>'
                                                + '</div>'
                                            + '</li>'
                                    + '</ul>'
                                + '</div>'
                            },
                            {
                                text: '<div class="list-block" style="margin:0;font-size:13px;color:black;">'
                                        + '<ul>'
                                            + '<li class="item-content" style="min-height:1.3rem;height:1.3rem;">'
                                                + '<div class="item-inner row" style="min-height:1.3rem;height:1.3rem;">'
                                                    + '<div class="item-title col-50">' + (obj[i]["CLEARANCECODE"] == null ? "" : obj[i]["CLEARANCECODE"]) + '</div>'
                                                    + '<div class="item-title col-33">' + (obj[i]["CUSNO"] == null ? "" : obj[i]["CUSNO"]) + '</div>'
                                                    + '<div class="item-title col-20">' + (obj[i]["INSPSTATUS"] == null ? "" : obj[i]["INSPSTATUS"]) + '</div>'
                                                + '</div>'
                                            + '</li>'
                                    + '</ul>'
                                + '</div>'
                            }
                        ];
                        //var buttons2 = [
                        //            {
                        //                text: '返回', bg: 'danger'
                        //            }
                        //];
                        var groups = [buttons1];//, buttons2
                        $.actions(groups);


                    },
                    error: function (XMLHttpRequest, textStatus, errorThrown) {//请求失败处理函数
                        //alert(XMLHttpRequest.status);
                        //alert(XMLHttpRequest.readyState);
                        //alert(textStatus);
                        alert('error...状态文本值：' + textStatus + " 异常信息：" + errorThrown);
                    }
                });

            });

            //文件调阅
            $("#FileConsult_a").click(function () {
                var curcount = parseInt($("#span_curchose").text());
                if (curcount != 1) {
                    $.toast("请选择一笔文件调阅记录");
                    return;
                }

                var preinspcode = "";
                $("#div_list .list-block").each(function () {
                    if ($(this).children("ul").css('background-color') == "rgb(193, 221, 241)") {
                        preinspcode = $(this)[0].id;
                    }
                });
                //if (preinspcode == "") {
                //    $.toast("请选择需要调阅的记录");
                //    return;
                //}

                $.modal({
                    title: '文件调阅',
                    text: '<div class="content-block row" style="margin:1rem 0;">' +
                                '<div class="col-25"><a href="#" id="filecancel" class="button" style="background-color: gray;color:white;border-radius:0;border:0;vertical-align:middle;">返回</div>' +
                                '<div class="col-40" style="margin-left:0;"><a href="#" id="filedecl" class="button" style="background-color: #3d4145;color:white;border-radius:0;border:0;vertical-align:middle;">报检文件</a></div>' +
                                '<div class="col-40" style="margin-left:0;"><a href="#" id="filecheck" class="button" style="background-color: gray;color:white;border-radius:0;border:0;vertical-align:middle;">查验文件</a></div>' +
                            '</div>',
                    extraClass: 'filediv'//避免直接设置.modal的样式，从而影响其他toast的提示
                });

                $("#filecancel").click(function () {
                    $.closeModal(".filediv");
                });

                $("#filedecl").click(function () {
                    $.closeModal(".filediv");

                    $.ajax({
                        type: "post", //要用post方式                 
                        url: "InspectionList.aspx/FileConsult",//方法所在页面和方法名
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
                        data: "{'preinspcode':'" + preinspcode + "'}",
                        cache: false,
                        async: false,
                        success: function (data) {
                            if (data.d == "") {
                                $.toast("报检单文件不存在");
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
                        },
                        error: function (XMLHttpRequest, textStatus, errorThrown) {//请求失败处理函数
                            //alert(XMLHttpRequest.status);
                            //alert(XMLHttpRequest.readyState);
                            //alert(textStatus);
                            alert('error...状态文本值：' + textStatus + " 异常信息：" + errorThrown);
                        }
                    });

                });

                $("#filecheck").click(function () {
                    $.closeModal(".filediv");

                    $.ajax({
                        type: "post", //要用post方式                 
                        url: "InspectionList.aspx/picfileconsult",//方法所在页面和方法名
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
                        data: "{'preinspcode':'" + preinspcode + "'}",
                        cache: false,
                        async: false,//默认是true，异步；false为同步，此方法执行完在执行下面代码
                        success: function (data) {
                            var obj = eval("(" + data.d + ")");//将字符串转为json

                            if (obj.length <= 0) {
                                $.toast("没有查验图片");
                                return;
                            }

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
            
            $.init();
            //----------------------------------------------------------------------------------------------------------------------------------------
            function loaddata(itemsPerLoad, lastIndex) {
                $.ajax({
                    type: "post", //要用post方式                 
                    url: "InspectionList.aspx/BindList",//方法所在页面和方法名
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    data: "{'reptime_s':'" + $("#txt_reptime_s").val() + "','reptime_e':'" + $("#txt_reptime_e").val() + "','inspcode':'" + $("#txt_inspcode").val()
                        + "','modifyflag':'" + $("#picker_MODIFYFLAG").val() + "','busitype':\"" + $("#txt_busitype").val()+ "\",'ischeck':'" + $("#txt_ischeck").val() 
                        + "','ispass':'" + $("#txt_ispass").val() + "','lawflag':'" + $("#txt_lawflag").val() + "','isneedclearance':'" + $("#txt_isneedclearance").val()
                        + "','busiunit':'" + $("#txt_busiunit").val() + "','contractno':'" + $("#txt_contractno").val() + "','ordercode':'" + $("#txt_ordercode").val()
                        + "','cusno':'" + $("#txt_cusno").val() + "','divideno':'" + $("#txt_divideno").val() + "','customareacode':'" + $("#txt_customareacode").val()
                        + "','approvalcode':'" + $("#txt_approvalcode").val() + "','submittime_s':'" + $("#txt_submittime_s").val() + "','submittime_e':'" + $("#txt_submittime_e").val()
                        + "','sitepasstime_s':'" + $("#txt_sitepasstime_s").val() + "','sitepasstime_e':'" + $("#txt_sitepasstime_e").val()
                        + "','start':" + lastIndex + ",'itemsPerLoad':" + itemsPerLoad + "}",
                    cache: false,
                    async: false,//默认是true，异步；false为同步，此方法执行完在执行下面代码
                    success: function (data) {
                        var objdata = eval("(" + data.d + ")");//将字符串转为json                        
                        var obj = objdata[0]["data"];
                        $("#span_sum").text(objdata[0]["sum"]);

                        var tb = "";
                        for (var i = 0; i < obj.length; i++) {
                            tb = '<div class="list-block" id="' + (obj[i]["CODE"] == null ? "" : obj[i]["CODE"]) + '">'
                                    + '<ul id="order_' + (obj[i]["ORDERCODE"] == null ? "" : obj[i]["ORDERCODE"]) + '">'
                                        + '<li class="item-content">'
                                             + '<div class="item-inner row">'
                                                + '<div class="item-title col-50">' + (obj[i]["INSPECTIONCODE"] == null ? "" : obj[i]["INSPECTIONCODE"]) + '</div>'
                                                + '<div class="item-title col-33">' + getname("BUSITYPE", obj[i]["BUSITYPE"]) + '</div>'
                                                + '<div class="item-title col-20">' + (obj[i]["TRADEWAYNAME"] == null ? "" : obj[i]["TRADEWAYNAME"]) + '</div>'
                                            + '</div>'
                                        + '</li>'
                                        + '<li class="item-content">'
                                            + '<div class="item-inner row">'
                                                + '<div class="item-title col-50">' + (obj[i]["BUSIUNITNAME"] == null ? "" : obj[i]["BUSIUNITNAME"]) + '</div>'
                                                + '<div class="item-title col-33">' + (obj[i]["CONTRACTNO"] == null ? "" : obj[i]["CONTRACTNO"]) + '</div>'
                                                + '<div class="item-title col-20">' + (obj[i]["REPTIME"] == null ? "" : obj[i]["REPTIME"]) + '</div>'
                                            + '</div>'
                                        + '</li>'
                                        + '<li class="item-content">'
                                            + '<div class="item-inner row">'
                                                + '<div class="item-title col-50">' + (obj[i]["APPROVALCODE"] == null ? "" : obj[i]["APPROVALCODE"]) + '</div>'
                                                + '<div class="item-title col-33">' + (obj[i]["GOODSNUM"] == null ? "" : obj[i]["GOODSNUM"]) + '/' + (obj[i]["GOODSGW"] == null ? "" : obj[i]["GOODSGW"]) + '</div>'
                                                + '<div class="item-title col-20">' + getname("MODIFYFLAG", obj[i]["MODIFYFLAG"]) + '</div>'
                                            + '</div>'
                                        + '</li>'
                                        + '<li class="item-content">'
                                            + '<div class="item-inner row">'
                                                + '<div class="item-title col-50">' + (obj[i]["CLEARANCECODE"] == null ? "" : obj[i]["CLEARANCECODE"]) + '</div>'
                                                + '<div class="item-title col-33">' + (obj[i]["CUSNO"] == null ? "" : obj[i]["CUSNO"]) + '</div>'
                                                + '<div class="item-title col-20">' + (obj[i]["INSPSTATUS"] == null ? "" : obj[i]["INSPSTATUS"]) + '</div>'
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

        function initsearch_condition() {
            $("#picker_MODIFYFLAG").picker({
                toolbarTemplate: '<header class="bar bar-nav">\
                      <button class="button button-link pull-right close-picker">确定</button>\
                      <h1 class="title">请选择删改单</h1>\
                      </header>',
                cols: [
                  {
                      textAlign: 'center',
                      values: ['全部', '正常', '删单', '改单', '改单完成']
                  }
                ]
            });

            //初始化时间控件
            var before = new Date();
            before.setDate(before.getDate() - 3);
            var beforeday = before.Format("yyyy-MM-dd");

            var now = new Date();
            var today = now.Format("yyyy-MM-dd");

            $("#txt_reptime_s").val(beforeday);
            $("#txt_reptime_s").calendar({ value: [beforeday] });

            $("#txt_reptime_e").val(today);
            $("#txt_reptime_e").calendar({ value: [today] });

        }

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

        function modifySave(preinspcode, modifystr, modifyflag) {

            $.confirm('请确认是否保存为 <font color=blue>' + modifystr + '</font>?',
                function () {//OK事件
                    $.ajax({
                        type: "post", //要用post方式                 
                        url: "InspectionList.aspx/ModifySave",//方法所在页面和方法名
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
                        data: "{'preinspcode':'" + preinspcode + "','modifyflag':" + modifyflag + "}",
                        cache: false,
                        async: false,//默认是true，异步；false为同步，此方法执行完在执行下面代码
                        success: function (data) {
                            if (data.d == "success") {
                                $.toast("保存成功");
                                $("#div_list #" + preinspcode).children("ul").children().eq(2).children("div").children().eq(2).text(modifystr);
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
                },
                function () { }//cancel事件
              );
        }        

    </script>

</head>
<body>
    <div class="page-group">
        <div id="page-infinite-scroll-bottom" class="page page-current">
            <%--search --%>
            <header class="bar bar-nav">
                <div class="search-input">                 
                    <div class="row"> 
                        <div class="col-25" style="width:25%;font-size:13px;margin-top:.8rem;">报检时间始/末:</div>
                        <div class="col-33" style="width:30%;margin-left:0;"><input type="search" id='txt_reptime_s'/></div>
                        <div class="col-33" style="width:30%;margin-left:0;"><input type="search" id='txt_reptime_e'/></div>
                        <div class="col-10" style="width:10%;margin-left:0;margin-top:.2rem;">
                            <input id="btn_more_m" type="button" value="..." class="open-tabs-modal" style="background-color:#3D4145;color:#ffffff;border-radius:0;border:0;"  />
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-50" style="width:48%;"><input type="search" id='txt_inspcode' placeholder='报检单号'/></div>
                        <div class="col-40" style="width:40%;margin-left:0;"><input type="search" id='picker_MODIFYFLAG' placeholder='删改单'/></div>
                        <div class="col-10" style="width:6%;margin-left:0;"><a href="#" id="btn_barcode"><i class="iconfont" style="font-size:1.3rem;color:gray;">&#xe608;</i></a></div>
                    </div> 
                    <div class="row">
                        <div class="col-25" style="width:21%;"><input id="btn_gridname_m" type="button" value="列名" style="background-color:#808080;color:#ffffff;border-radius:0;border:0;" /></div>
                        <div class="col-60" style="width:54%;margin-left:0;"><input id="btn_search_m" type="button" value="查询" style="background-color:#3D4145;color:#ffffff;border-radius:0;border:0;" /></div>
                        <div class="col-25" style="width:21%;margin-left:0;"><input id="btn_reset_m" type="button" value="重置" style="background-color:#808080;color:#ffffff;border-radius:0;border:0;" /></div>
                    </div> 
                    <input type="hidden" id='txt_busitype'/><input type="hidden" id='txt_ischeck'/><input type="hidden" id='txt_ispass'/>
                    <input type="hidden" id='txt_lawflag'/><input type="hidden" id='txt_isneedclearance'/>
                    <input type="hidden" id='txt_busiunit'/> <input type="hidden" id='txt_contractno'/> 
                    <input type="hidden" id='txt_ordercode'/><input type="hidden" id='txt_cusno'/>  
                    <input type="hidden" id='txt_divideno'/> <input type="hidden" id='txt_customareacode'/>
                    <input type="hidden" id='txt_approvalcode'/>  
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
                <a class="tab-item external" href="#" id="Ass_a"><%--active--%>
                    <span class="icon icon-menu"></span>
                    <span class="tab-label">关联报检单</span>
                </a>
                <a class="tab-item external" href="#" id="ModifyEdit_a">
                    <span class="icon icon-edit"></span>
                    <span class="tab-label">删改单维护</span>
                    <%--<span class="badge">2</span>--%>
                </a>
                <a class="tab-item external" href="#" id="FileConsult_a">
                    <span class="icon icon-message"></span>
                    <span class="tab-label">文件调阅<input type="hidden" id="hd_AdminUrl" value='<%= System.Configuration.ConfigurationManager.AppSettings["AdminUrl"] %>' /></span>
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




    <script type='text/javascript' src='//g.alicdn.com/msui/sm/0.6.2/js/sm.min.js' charset='utf-8'></script>   
   <%-- <script type='text/javascript' src='//g.alicdn.com/msui/sm/0.6.2/js/sm-extend.min.js' charset='utf-8'></script>--%>
    <script src="/js/sm-extend.min.js"></script>
</body>

    <script type='text/javascript' src='http://res.wx.qq.com/open/js/jweixin-1.2.0.js'></script>
    <script type='text/javascript'>


        var conf = [];
        $.ajax({
            type: "post", //要用post方式                 
            url: "InspectionList.aspx/getConf",//方法所在页面和方法名
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
            jsApiList: ['scanQRCode']
        });

    </script>

</html>
