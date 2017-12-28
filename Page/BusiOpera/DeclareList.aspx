<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="DeclareList.aspx.cs" Inherits="WeChat.Page.BusiOpera.DeclareList" %>
<%@ Import Namespace="System.Configuration" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<%-- <meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>--%>
    <meta charset="utf-8">
    <meta name="viewport" content="initial-scale=1, maximum-scale=1">
    <title>报关单查询</title>
    <link href="/css/iconfont/iconfont.css?t=<%=ConfigurationManager.AppSettings["Version"]%>" rel="stylesheet" />
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
        #page-infinite-scroll-bottom .bar-nav ~ .content {
            top: 5rem;
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
        /************************************************ 更多查询*********************************/
        .morediv{
            width: 98%;
            left: 1%;
            right: 1%;
            margin-left: 0px;
        }    

        /************************************************ 报关订阅*********************************/
        #pop_sub_decl
        {
            background-color:#1D2E3C;
            color:white;
            margin-top:1rem;
        }
        #pop_sub_decl .row
        {
            background-color:#456581;
            border-top:solid 1px white;
            margin:0rem;
            font-size:initial;
            vertical-align:middle;
            height:2rem;
            line-height:2rem;
        }
        #pop_sub_decl .col-33
        {
            padding-top:0.2rem;
        }
       
        #pop_sub_decl .myrow
        {
            line-height:2rem;
            padding-left:4%;
        }
        #pop_sub_decl input
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
            //----------------------------------------------------------------------------------------------------------------查询条件
            $("#txt_startdate").calendar({});
            $("#txt_enddate").calendar({});          
            $(document).on('click', '.open-tabs-modal', function () {
                $.modal({
                    title: '<b>更多查询</b>',
                    text: '<div class="list-block" style="margin:0;">' +
                              '<ul>' +
                                '<li>' +
                                  '<div class="item-content">' +
                                    '<div class="item-inner">' +
                                      //'<div class="item-title label">进出口</div>' +
                                      '<div class="item-input"><input type="text" placeholder="选择进出口类型" id="picker_inout_type" value="' + $("#txt_inout_type").val() + '" readonly/></div>' +
                                    '</div>' +
                                  '</div>' +
                                '</li>' +
                                '<li>' +
                                  '<div class="item-content">' +
                                    '<div class="item-inner">' +
                                      //'<div class="item-title label">业务类型</div>' +
                                      '<div class="item-input"><input type="text" placeholder="选择业务类型" id="picker_busitype" value="' + $("#txt_busitype").val() + '" readonly/></div>' +
                                    '</div>' +
                                  '</div>' +
                                '</li>' +
                                '<li>' +
                                  '<div class="item-content">' +
                                    '<div class="item-inner">' +
                                     // '<div class="item-title label">删改单</div>' +
                                      '<div class="item-input"><input type="text" placeholder="选择删改单" id="picker_modifyflag" value="' + $("#txt_modifyflag").val() + '" readonly/></div>' +
                                    '</div>' +
                                  '</div>' +
                                '</li>' +
                                '<li>' +
                                  '<div class="item-content">' +
                                    '<div class="item-inner">' +
                                     // '<div class="item-title label">海关状态</div>' +
                                      '<div class="item-input"><input type="text" placeholder="选择海关状态" id="picker_customsstatus" value="' + $("#txt_customsstatus").val() + '" readonly/></div>' +
                                    '</div>' +
                                  '</div>' +
                                '</li>' +
                              '</ul>' +
                            '</div>',
                    buttons: [
                     {
                         text: '确认', bold: true,
                         onClick: function () {
                             $("#txt_inout_type").val($("#picker_inout_type").val());
                             $("#txt_busitype").val($("#picker_busitype").val());
                             $("#txt_modifyflag").val($("#picker_modifyflag").val());
                             $("#txt_customsstatus").val($("#picker_customsstatus").val());
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
                             $("#txt_declcode").val(""); $("#txt_startdate").val(""); $("#txt_enddate").val("");
                             $("#txt_startdate").calendar({}); $("#txt_enddate").calendar({});//否则之前选的那天  不能再次选中

                             $("#txt_inout_type").val(""); $("#picker_inout_type").val("")
                             $("#txt_busitype").val(""); $("#picker_busitype").val("")
                             $("#txt_modifyflag").val(""); $("#picker_modifyflag").val("");
                             $("#txt_customsstatus").val(""); $("#picker_customsstatus").val("");
                         }
                     }
                    ],
                    extraClass: 'morediv'//避免直接设置.modal的样式，从而影响其他toast的提示
                });

                $("#picker_inout_type").picker({
                    toolbarTemplate: '<header class="bar bar-nav">\
                      <button class="button button-link pull-right close-picker">确定</button>\
                      <h1 class="title">请选择进出口类型</h1>\
                      </header>',
                    cols: [
                      {
                          textAlign: 'center',
                          values: ['全部', '进口', '出口']
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
                          values: ['全部', '空运业务', '海运业务', '陆运业务', '国内业务', '特殊区域']
                      }
                    ]
                });
                $("#picker_modifyflag").picker({
                    toolbarTemplate: '<header class="bar bar-nav">\
                      <button class="button button-link pull-right close-picker">确定</button>\
                      <h1 class="title">请选择删改单</h1>\
                      </header>',
                    cols: [
                      {
                          textAlign: 'center',
                          values: ['正常', '删单', '改单', '改单完成']
                      }
                    ]
                });
                $("#picker_customsstatus").picker({
                    toolbarTemplate: '<header class="bar bar-nav">\
                      <button class="button button-link pull-right close-picker">确定</button>\
                      <h1 class="title">请选择海关状态</h1>\
                      </header>',
                    cols: [
                      {
                          textAlign: 'center',
                          values: ['未结关', '已结关', '未放行', '已放行']
                      }
                    ]
                });

            });

            //---------------------------------------------------------------------------------------------------------------------
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
                    $.closeModal();
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
                            $("#txt_declcode").val(serialNumber);
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
                var predelcode = "";
                $("#div_list .list-block").each(function () {
                    if ($(this).children("ul").css('background-color') == "rgb(193, 221, 241)") {
                        predelcode = $(this)[0].id;
                        //alert($(this).children("ul").children().eq(2).children("div").children().eq(2).text());
                    }
                });
                if (predelcode == "") {
                    $.toast("请选择需要维护的记录");
                    return;
                }

                var buttons1 = [
                    {
                        text: '<font style="font-weight:800">删改单维护，请选择</font>',
                        label: true
                    },
                    {
                        text: '删单', bold: true, //color: 'danger',
                        onClick: function () {
                            modifySave(predelcode, "删单", 1);
                        }
                    },
                    {
                        text: '改单', bold: true,
                        onClick: function () {
                            modifySave(predelcode, "改单", 2);
                        }
                    },
                    {
                        text: '改单完成', bold: true,
                        onClick: function () {
                            modifySave(predelcode, "改单完成", 3);
                        }
                    }];
                var buttons2 = [
                          {
                              text: '取消',bg: 'danger'
                          }
                ];
                var groups = [buttons1, buttons2];
                $.actions(groups);
            });

            //关联报关单查询
            $("#Ass_a").click(function () {
                var predelcode = "";
                $("#div_list .list-block").each(function () {
                    if ($(this).children("ul").css('background-color') == "rgb(193, 221, 241)") {
                        predelcode = $(this)[0].id;
                    }
                });
                if (predelcode == "") {
                    $.toast("请选择需要关联的记录");
                    return;
                }

                var busitypename_con = $("#div_list #" + predelcode).children("ul").children().eq(0).children("div").children().eq(1).text();
                if (busitypename_con != "国内出口" && busitypename_con != "国内进口") {
                    $.toast("请选择国内业务");
                    return;
                }

                $.ajax({
                    type: "post", //要用post方式                 
                    url: "DeclareList.aspx/AssCon",//方法所在页面和方法名
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    data: "{'predelcode':'" + predelcode + "'}",
                    cache: false,
                    async: false,
                    success: function (data) {
                        var obj = eval("(" + data.d + ")");//将字符串转为json

                        if (obj.length <= 0) {
                            $.toast("没有关联报关单");
                            return;
                        }

                        /*var tb_con = "";
                        tb_con = '<div class="list-block">'
                                + '<ul>'
                                    + '<li class="item-content">'
                                         + '<div class="item-inner row">'
                                            + '<div class="item-title col-50">' + obj[0]["DECLARATIONCODE"] + '</div>'
                                            + '<div class="item-title col-33">' + getname("BUSITYPE", obj[0]["BUSITYPE"]) + '</div>'
                                            + '<div class="item-title col-15">' + obj[0]["TRADEMETHOD"] + '</div>'
                                        + '</div>'
                                    + '</li>'
                                    + '<li class="item-content">'
                                        + '<div class="item-inner row">'
                                            + '<div class="item-title col-50">' + obj[0]["CONSIGNEESHIPPERNAME"] + '</div>'
                                            + '<div class="item-title col-33">' + obj[0]["CONTRACTNO"] + '</div>'
                                            + '<div class="item-title col-15">' + (obj[0]["REPTIME"] == null ? "" : obj[0]["REPTIME"]) + '</div>'
                                        + '</div>'
                                    + '</li>'
                                    + '<li class="item-content">'
                                        + '<div class="item-inner row">'
                                            + '<div class="item-title col-50">' + obj[0]["TRANSNAME"] + '</div>'
                                            + '<div class="item-title col-33">' + obj[0]["GOODSNUM"] + '/' + obj[0]["GOODSGW"] + '</div>'
                                            + '<div class="item-title col-15">' + getname("MODIFYFLAG", obj[0]["MODIFYFLAG"]) + '</div>'
                                        + '</div>'
                                    + '</li>'
                                    + '<li class="item-content">'
                                        + '<div class="item-inner row">'
                                            + '<div class="item-title col-50">' + obj[0]["BLNO"] + '</div>'
                                            + '<div class="item-title col-33">' + obj[0]["CUSNO"] + '</div>'
                                            + '<div class="item-title col-15">' + obj[0]["CUSTOMSSTATUS"] + '</div>'
                                        + '</div>'
                                    + '</li>'
                                + '</ul>'
                         + '</div>';
                         */

                        var buttons1 = [
                            {
                                text: '<font style="font-weight:800">关联报关单信息</font>',
                                label: true
                            },
                            {
                                text: '<div class="list-block" style="margin:0;font-size:13px;color:black;">'
                                        + '<ul>'
                                           + '<li class="item-content" style="min-height:1.3rem;height:1.3rem;">' +
                                                  '<div class="item-inner row" style="min-height:1.3rem;height:1.3rem;">'
                                                    + '<div class="item-title col-50">' + (obj[0]["DECLARATIONCODE"] == null ? "" : obj[0]["DECLARATIONCODE"]) + '</div>'
                                                    + '<div class="item-title col-33">' + getname("BUSITYPE", obj[0]["BUSITYPE"]) + '</div>'
                                                    + '<div class="item-title col-20">' + (obj[0]["TRADEMETHOD"] == null ? "" : obj[0]["TRADEMETHOD"]) + '</div>'
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
                                                    + '<div class="item-title col-50">' + (obj[0]["CONSIGNEESHIPPERNAME"] == null ? "" : obj[0]["CONSIGNEESHIPPERNAME"]) + '</div>'
                                                    + '<div class="item-title col-33">' + (obj[0]["CONTRACTNO"] == null ? "" : obj[0]["CONTRACTNO"]) + '</div>'
                                                    + '<div class="item-title col-20">' + (obj[0]["REPTIME"] == null ? "" : obj[0]["REPTIME"]) + '</div>'
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
                                                    + '<div class="item-title col-50">' + (obj[0]["TRANSNAME"] == null ? "" : obj[0]["TRANSNAME"]) + '</div>'
                                                    + '<div class="item-title col-33">' + (obj[0]["GOODSNUM"] == null ? "" : obj[0]["GOODSNUM"]) + '/' + (obj[0]["GOODSGW"] == null ? "" : obj[0]["GOODSGW"]) + '</div>'
                                                    + '<div class="item-title col-20">' + getname("MODIFYFLAG", obj[0]["MODIFYFLAG"]) + '</div>'
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
                                                    + '<div class="item-title col-50">' + (obj[0]["BLNO"] == null ? "" : obj[0]["BLNO"]) + '</div>'
                                                    + '<div class="item-title col-33">' + (obj[0]["CUSNO"] == null ? "" : obj[0]["CUSNO"]) + '</div>'
                                                    + '<div class="item-title col-20">' + (obj[0]["CUSTOMSSTATUS"] == null ? "" : obj[0]["CUSTOMSSTATUS"]) + '</div>'
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

            //报关单调阅
            $("#FileConsult_a").click(function () {
                var predelcode = "";
                $("#div_list .list-block").each(function () {
                    if ($(this).children("ul").css('background-color') == "rgb(193, 221, 241)") {
                        predelcode = $(this)[0].id;
                    }
                });
                if (predelcode == "") {
                    $.toast("请选择需要调阅的记录");
                    return;
                }

                $.ajax({
                    type: "post", //要用post方式                 
                    url: "DeclareList.aspx/FileConsult",//方法所在页面和方法名
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    data: "{'predelcode':'" + predelcode + "'}",
                    cache: false,
                    async: false,
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
                    },
                    error: function (XMLHttpRequest, textStatus, errorThrown) {//请求失败处理函数
                        //alert(XMLHttpRequest.status);
                        //alert(XMLHttpRequest.readyState);
                        //alert(textStatus);
                        alert('error...状态文本值：' + textStatus + " 异常信息：" + errorThrown);
                    }
                });

            });

            //报关订阅
            $("#Subs_decl_a").click(function () {
                var predeclcode = ""; var div_ordercode = ""; var declcode = ""; var cusno = "";
                $("#div_list .list-block").each(function () {
                    if ($(this).children("ul").css('background-color') == "rgb(193, 221, 241)") {

                        predeclcode = $(this)[0].id; div_ordercode = $(this).children("ul")[0].id;

                        declcode = $(this).children("ul").children().eq(0).children("div").children().eq(0).text();
                        cusno = $(this).children("ul").children().eq(3).children("div").children().eq(1).text();
                    }
                });
                if (predeclcode == "") {
                    $.toast("请选择需要订阅的记录");
                    return;
                }
                $.popup("#popup-subscribe-decl");

                $(document).on('open', '#popup-subscribe-decl', function () {
                    $("input[name='chk_status']").prop('checked', false);
                });
                $(document).on('opened', '#popup-subscribe-decl', function () {
                    $("#Pop_hd_predeclcode").val(predeclcode);
                    $("#Pop_hd_ordercode").val(div_ordercode.substring(6));

                    $("#Pop_hd_declcode").val(declcode);
                    $("#Pop_hd_cusno").val(cusno);
                });
            });

            $("#Pop_Subscribe").click(function () {
                var predeclcode = $("#Pop_hd_predeclcode").val(); var ordercode = $("#Pop_hd_ordercode").val();

                var declcode = $("#Pop_hd_declcode").val();
                var cusno = $("#Pop_hd_cusno").val();

                var status = "";
                $("input[name='chk_status']:checked").each(function (index, item) {
                    status += $(this).val() + ",";
                });

                if (status == "") {
                    $.toast("请选择需要订阅的状态");
                    return;
                }

                $.ajax({
                    type: 'post',
                    url: '/Page/MyBusiness/MyBusiness.aspx/SubscribeStatus',
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    data: "{'type':'报关状态','status':'" + status + "','cusno':'" + cusno + "','declarationcode':'" + declcode +
                        "','userid':'" + userid + "','username':'" + username + "','openid':'" + openid + "'}",
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
                    url: "DeclareList.aspx/BindList",//方法所在页面和方法名
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    data: "{'declcode':'" + $("#txt_declcode").val() + "','startdate':'" + $("#txt_startdate").val() + "','enddate':'" + $("#txt_enddate").val()
                        + "','inouttype':'" + $("#txt_inout_type").val() + "','busitype':'" + $("#txt_busitype").val() + "','modifyflag':'" + $("#txt_modifyflag").val()
                        + "','customsstatus':'" + $("#txt_customsstatus").val()
                        + "','start':" + lastIndex + ",'itemsPerLoad':" + itemsPerLoad + "}",
                    cache: false,
                    async: false,//默认是true，异步；false为同步，此方法执行完在执行下面代码
                    success: function (data) {
                        var obj = eval("(" + data.d + ")");//将字符串转为json

                        var tb = ""; 
                        for (var i = 0; i < obj.length; i++) {
                            tb = '<div class="list-block" id="' + (obj[i]["CODE"] == null ? "" : obj[i]["CODE"]) + '">'
                                    + '<ul id="order_' + (obj[i]["ORDERCODE"] == null ? "" : obj[i]["ORDERCODE"]) + '">'
                                        + '<li class="item-content">'
                                             + '<div class="item-inner row">'
                                                + '<div class="item-title col-50">' + (obj[i]["DECLARATIONCODE"] == null ? "" : obj[i]["DECLARATIONCODE"]) + '</div>'
                                                + '<div class="item-title col-33">' + getname("BUSITYPE", obj[i]["BUSITYPE"]) + '</div>'
                                                + '<div class="item-title col-20">' + (obj[i]["TRADEMETHOD"] == null ? "" : obj[i]["TRADEMETHOD"]) + '</div>'
                                            + '</div>'
                                        + '</li>'
                                        + '<li class="item-content">'
                                            + '<div class="item-inner row">'
                                                + '<div class="item-title col-50">' + (obj[i]["CONSIGNEESHIPPERNAME"] == null ? "" : obj[i]["CONSIGNEESHIPPERNAME"]) + '</div>'
                                                + '<div class="item-title col-33">' + (obj[i]["CONTRACTNO"] == null ? "" : obj[i]["CONTRACTNO"]) + '</div>'
                                                + '<div class="item-title col-20">' + (obj[i]["REPTIME"] == null ? "" : obj[i]["REPTIME"]) + '</div>'
                                            + '</div>'
                                        + '</li>'
                                        + '<li class="item-content">'
                                            + '<div class="item-inner row">'
                                                + '<div class="item-title col-50">' + (obj[i]["TRANSNAME"] == null ? "" : obj[i]["TRANSNAME"]) + '</div>'
                                                + '<div class="item-title col-33">' + (obj[i]["GOODSNUM"] == null ? "" : obj[i]["GOODSNUM"]) + '/' + (obj[i]["GOODSGW"] == null ? "" : obj[i]["GOODSGW"]) + '</div>'
                                                + '<div class="item-title col-20">' + getname("MODIFYFLAG", obj[i]["MODIFYFLAG"]) + '</div>'
                                            + '</div>'
                                        + '</li>'
                                        + '<li class="item-content">'
                                            + '<div class="item-inner row">'
                                                + '<div class="item-title col-50">' + (obj[i]["BLNO"] == null ? "" : obj[i]["BLNO"]) + '</div>'
                                                + '<div class="item-title col-33">' + (obj[i]["CUSNO"] == null ? "" : obj[i]["CUSNO"]) + '</div>'
                                                + '<div class="item-title col-20">' + (obj[i]["CUSTOMSSTATUS"] == null ? "" : obj[i]["CUSTOMSSTATUS"]) + '</div>'
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

        function modifySave(predelcode, modifystr, modifyflag) {

            $.confirm('请确认是否保存为 <font color=blue>' + modifystr + '</font>?',
                function () {//OK事件
                    $.ajax({
                        type: "post", //要用post方式                 
                        url: "DeclareList.aspx/ModifySave",//方法所在页面和方法名
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
                        data: "{'predelcode':'" + predelcode + "','modifyflag':" + modifyflag+ "}",
                        cache: false,
                        async: false,//默认是true，异步；false为同步，此方法执行完在执行下面代码
                        success: function (data) {
                            if (data.d == "success") {
                                $.toast("保存成功");
                                $("#div_list #" + predelcode).children("ul").children().eq(2).children("div").children().eq(2).text(modifystr);
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
                        <div class="col-85"><input type="search" id='txt_declcode' placeholder='请输入18位或后9位报关单号...'/></div>
                        <div class="col-15"><a href="#" id="btn_barcode"><i class="iconfont" style="font-size:1.3rem;color:gray;">&#xe608;</i></a></div>
                    </div>
                    <div class="row">
                        <div class="col-40"><input type="search" id='txt_startdate' placeholder='申报起始日期'/></div>
                        <div class="col-5">~</div>
                        <div class="col-40"><input type="search" id='txt_enddate' placeholder='申报结束日期'/></div>
                        <div class="col-15"><a href="#" class="open-tabs-modal"><i class="iconfont" style="font-size:1.3rem;color:gray;">&#xe6ca;</i></a></div>
                    </div>                    
                </div>  
                <input type="hidden" id='txt_inout_type'/><input type="hidden" id='txt_busitype'/><input type="hidden" id='txt_modifyflag'/>  <input type="hidden" id='txt_customsstatus'/>                
                <a href="#" id="search_a" class="open-preloader-title button button-fill">查询</a>   
            </header>

            <%--工具栏 --%>
            <nav class="bar bar-tab">
                <a class="tab-item external" href="#" id="Ass_a"><%--active--%>
                    <span class="icon icon-menu"></span>
                    <span class="tab-label">关联报关单信息</span>
                </a>
                <a class="tab-item external" href="#" id="ModifyEdit_a">
                    <span class="icon icon-edit"></span>
                    <span class="tab-label">删改单维护</span>
                    <%--<span class="badge">2</span>--%>
                </a>
                <a class="tab-item external" href="#" id="FileConsult_a">
                    <span class="icon icon-message"></span>
                    <span class="tab-label">报关单调阅</span>
                </a>
                <a class="tab-item external" href="#" id="Subs_decl_a">
                    <span class="icon icon-card"></span>
                    <span class="tab-label">报关订阅</span>
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

    <!--popup 订阅弹出页-->
    <div class="popup popup-subscribe" id="popup-subscribe-decl" >
        <div class="content" >
            <div id="pop_sub_decl">
                <div class="myrow">
                    报关状态订阅<input type="hidden" id="Pop_hd_predeclcode" value="" /><input type="hidden" id="Pop_hd_ordercode" value="" />
                    <input type="hidden" id="Pop_hd_declcode" value="" /><input type="hidden" id="Pop_hd_cusno" value="" />
                </div>
                <div class="row">
                    <div class="col-66">申报完成</div>
                    <div class="col-33"><input type="checkbox" name="chk_status" value="申报完成"/></div>
                </div>
                <div class="row">
                    <div class="col-66">已放行</div>
                    <div class="col-33"><input type="checkbox" name="chk_status" value="已放行"/></div>
                </div>
                <div class="row">
                    <div class="col-66">已结关</div>
                    <div class="col-33"><input type="checkbox" name="chk_status" value="已结关"/></div>
                </div>
                <div class="row">
                    <div class="col-66">改单完成</div>
                    <div class="col-33"><input type="checkbox" name="chk_status" value="改单完成"/></div>
                </div>
                <div class="row">
                    <div class="col-66">删单完成</div>
                    <div class="col-33"><input type="checkbox" name="chk_status" value="删单完成"/></div>
                </div>
            </div>
            <div class="content-block row">
                <div class="col-50"><a href="#" class="button button-fill" id="Pop_Subscribe">确  认</a></div>
                <div class="col-50"><a href="#" class="close-popup button button-fill">返  回</a></div>
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
            url: "DeclareList.aspx/getConf",//方法所在页面和方法名
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
