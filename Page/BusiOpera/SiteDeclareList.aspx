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
    <%--<link rel="stylesheet" href="//g.alicdn.com/msui/sm/0.6.2/css/??sm.min.css,sm-extend.min.css">--%>
    <script type='text/javascript' src='//g.alicdn.com/sj/lib/zepto/zepto.min.js' charset='utf-8'></script>

    <style>
        .bar input[type=search]{
             margin:.2rem 0;
        }
        .bar .button {
            top:0;
        }
        .bar-nav ~ .content {
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
    </style>

    <script type="text/javascript">
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

            $("#div_list").on('click', '.list-block', function (e) {// $("#div_list")也可以换成$(document)，是基于父容器的概念   
                if ($(this).children("ul").css('background-color') == "rgb(193, 221, 241)") {
                    $(this).children("ul").css('background-color', '#fff');
                } else {
                    $("#div_list .list-block ul").css('background-color', '#fff');
                    $(this).children("ul").css('background-color', '#C1DDF1');
                }
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

                            tb = '<div class="list-block" id="' + obj[i]["CODE"] + '">'
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
            if (key == "ISCHECK" || key == "CHECKPIC") {
                switch (value) {
                    case 0: str = "否"; break;
                    case 1: str = "是"; break;
                    default: str = "否"; break;
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
                      values: ['未放行', '已放行']
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

                             //$("input[name='radio_type']").attr('checked', false); $("#txt_morecon").val("");//因每次窗口都是新开的，可以不用置空，置空隐藏值即可
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
                    $('input[name="radio_type"][value="' + $("#txt_radio_type_hidden").val() + '"]').attr("checked", true);
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
            <header class="bar bar-nav">
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
                    <span class="tab-label">查验图片</span>
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
   <%-- <script type='text/javascript' src='//g.alicdn.com/msui/sm/0.6.2/js/sm-extend.min.js' charset='utf-8'></script>
    <script type="text/javascript" src="//g.alicdn.com/msui/sm/0.6.2/js/sm-city-picker.min.js" charset="utf-8"></script>--%>

</body>
</html>
