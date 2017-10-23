<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="HsCodeList.aspx.cs" Inherits="WeChat.Page.FrequentSearch.HsCodeList" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
   <%-- <meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>--%>
    <meta charset="utf-8">
    <meta name="viewport" content="initial-scale=1, maximum-scale=1">
    <title>HS编码查询</title>

    <link rel="stylesheet" href="//g.alicdn.com/msui/sm/0.6.2/css/sm.min.css">
    <%--<link rel="stylesheet" href="//g.alicdn.com/msui/sm/0.6.2/css/??sm.min.css,sm-extend.min.css">--%>
    <script type='text/javascript' src='//g.alicdn.com/sj/lib/zepto/zepto.min.js' charset='utf-8'></script>
    <style>
        .bar{
            height:6rem;
        }
        .button{
            height:2.2em;
            font-size:.85rem;
        }
        .button.button-fill{
            line-height:2.2em;
        }
        .list-block{
            margin:.3rem 0;
        }
        .bar input[type=search]{
            height:2rem;
        }
        .bar-nav~.content{
            top:7rem;
        }
    </style>
    
    <script type="text/javascript">

        var loading = false;
        var itemsPerLoad = 10;// 每次加载添加多少条目                
        var maxItems = 100;// 最多可加载的条目
        var lastIndex = 0;//$('.list-block').length;//.list-container li        

        $(function () {
            FastClick.attach(document.body);
            //----------------------------------------------------------------------------------------------------------------------------------zepo
            $(document).on('click', '.open-preloader-title', function () {
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
                    lastIndex = $('.list-block').length;// 更新最后加载的序号
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
            });

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

                        if (lastIndex == $('.list-block').length) {
                            $.detachInfiniteScroll($('.infinite-scroll'));// 加载完毕，则注销无限加载事件，以防不必要的加载     
                            $('.infinite-scroll-preloader').hide();

                            $.toast("已经加载到最后");
                            return;
                        }                        
                        lastIndex = $('.list-block').length;// 更新最后加载的序号
                        $.refreshScroller();

                    }, 500);
                });
            });

            $("#div_list").on('click', '.item-link', function (e) {// $("#div_list")也可以换成$(document)，是基于父容器的概念               

                /* var strconHTML = "";
                 var ele_id = e.currentTarget.id;
                 var id = ele_id.substring(3);
 
                 $.ajax({
                     type: "post", //要用post方式                 
                     url: "HsCodeList.aspx/BindList",//方法所在页面和方法名
                     contentType: "application/json; charset=utf-8",
                     dataType: "json",
                     data: "{'id':'" + id + "'}",
                     cache: false,
                     async: false,
                     success: function (data) {
                         var obj = eval("(" + data.d + ")");//将字符串转为json
 
                         strconHTML = '<div class="list-block">'
                             + '<ul class="list-container">'
                             + '<li class="item-content item-link" id="li_' + obj[0]["ID"] + '"><div class="item-inner"><div class="item-title">HS编码</div><div class="item-after"><font color="#0894ec">' + obj[0]["HSCODEEXTRACODE"] + '</font></div></div></li>'
                             + '<li class="item-content"><div class="item-inner"><div class="item-title">商品名称</div><div class="item-after">' + obj[0]["NAME"] + '</div></div></li>'
                             + '<li class="item-content"><div class="item-inner"><div class="item-title">计量单位</div><div class="item-after">' + obj[0]["LEGALUNIT"] + '/' + obj[i]["LEGALUNITNAME"] + '</div></div></li>'
                             + '</ul>'
                             + '</div>';
 
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
                                         '<div class="content-block">' +
                                               '<div id="div_con_popup">' +
                                                     strconHTML+
                                               '</div>' +
                                               '<div class="content-block"><a href="#" class="close-popup button button-fill button-danger">返回</a></div>' +
                                         '</div>' +
                                     '</div>' +
                                 '</div>';
                 $.popup(popupHTML);*/

                $.popup('.popup-detail');
            });

            //$("#div_list").on('touchend', '.item-link', function (e) {// $("#div_list")也可以换成$(document)，是基于父容器的概念
            //    $.popup('.popup-detail');
            //    e.preventDefault();//阻止“默认行为”
            //});


            $.init();
            //----------------------------------------------------------------------------------------------------------------------------------------jquery
            function loaddata(itemsPerLoad, lastIndex) {
                $.ajax({
                    type: "post", //要用post方式                 
                    url: "HsCodeList.aspx/BindList",//方法所在页面和方法名
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    data: "{'hscode':'" + $("#txt_hscode").val() + "','commodityname':'" + $("#txt_commodityname").val() + "','start':" + lastIndex + ",'itemsPerLoad':" + itemsPerLoad + "}",
                    cache: false,
                    async: false,//默认是true，异步；false为同步，此方法执行完在执行下面代码
                    success: function (data) {
                        var obj = eval("(" + data.d + ")");//将字符串转为json

                        var tb = ""; var objname = "";
                        for (var i = 0; i < obj.length; i++) {
                            //alert(obj[i]["NAME"]);
                            objname = obj[i]["NAME"].length >= 12 ? obj[i]["NAME"].substring(0, 12) + "..." : obj[i]["NAME"];

                            tb = '<div class="list-block">'// onclick=shownotice(' + obj[i]["ID"] + ')
                               + '<ul class="list-container">'
                               + '<li class="item-content item-link" id="li_' + obj[i]["ID"] + '"><div class="item-inner"><div class="item-title">HS编码</div><div class="item-after"><font color="#0894ec">' + obj[i]["HSCODEEXTRACODE"] + '</font></div></div></li>'
                               + '<li class="item-content"><div class="item-inner"><div class="item-title">商品名称</div><div class="item-after">' + objname + '</div></div></li>'
                               + '<li class="item-content"><div class="item-inner"><div class="item-title">计量单位</div><div class="item-after">' + obj[i]["LEGALUNIT"] + '/' + obj[i]["LEGALUNITNAME"] + '</div></div></li>'
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
    </script>
    
</head>
<body>    

    <div class="page-group">
        <div id="page-infinite-scroll-bottom" class="page page-current">
            <header class="bar bar-nav">
                <div class="search-input">
                    <input type="search" id='txt_hscode' placeholder='请输入HS编码...'/>
                </div>
                <div class="search-input">
                    <input type="search" id='txt_commodityname' placeholder='请输入商品名称...'/>
                </div>
                <a href="#" id="search_a" class="open-preloader-title button button-fill"><span class="icon icon-search"></span>&nbsp;查询</a>   
                <%--<br />   
                <a href="#" id="search_b" class="button button-fill"><span class="icon icon-search"></span>&nbsp;查询</a>      --%>
            </header>
            <div class="content infinite-scroll native-scroll" data-distance="100">
                <div id="div_list"></div>
                <!-- 加载提示符 -->
                <div class="infinite-scroll-preloader">
                  <div class="preloader"></div>
                </div>
            </div>
        </div>
    </div>

    <!-- Popup -->
    <div class="popup popup-detail" id="div_detail">
        <div class="content"> <%--data-type='native'--%>
            <div class="content-padded grid-demo">  
                <div class="row"><div class="col-100">HS编码：9803009000</div></div>                
                <div class="row"><div class="col-100">其他定制型软件</div></div>               
                <div class="row"><div class="col-100">计量单位：006/套</div></div>
            </div>
            

            <%--<div class="content-block-title">List block buttons</div>
            <div class="list-block">
            <ul>
                <li><a href="#" class="item-link list-button">List Button 1</a></li>
                <li><a href="#" class="item-link list-button">List Button 2</a></li>
                <li><a href="#" class="item-link list-button">List Button 3</a></li>
            </ul>
            </div>
            <div class="content-block-title">Inset list block buttons</div>   
            <ul>
                <li><a href="#" class="item-link list-button">List Button 1</a></li>
                <li><a href="#" class="item-link list-button">List Button 2</a></li>
                <li><a href="#" class="item-link list-button">List Button 3</a></li>
            </ul>   --%>
            <div class="content-block"><a href="#" class="close-popup button button-fill button-danger">返回</a></div>     
        </div>
    </div>
    
    <script type='text/javascript' src='//g.alicdn.com/msui/sm/0.6.2/js/sm.min.js' charset='utf-8'></script>   
    <%--<script type='text/javascript' src='//g.alicdn.com/msui/sm/0.6.2/js/??sm.min.js,sm-extend.min.js' charset='utf-8'></script>--%>
</body>
</html>
