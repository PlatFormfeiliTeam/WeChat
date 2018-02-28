<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="NewSubscribeList_busi.aspx.cs" Inherits="WeChat.Page.MyBusiness.NewSubscribeList_busi" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title>业务订阅</title>
    <meta name="viewport" content="initial-scale=1, maximum-scale=1" />
    <link href="/css/iconfont/iconfont.css" rel="stylesheet" />
    <link rel="stylesheet" href="//g.alicdn.com/msui/sm/0.6.2/css/sm.min.css" />
    <script type='text/javascript' src='//g.alicdn.com/sj/lib/zepto/zepto.min.js' charset='utf-8'></script>
    <link rel="stylesheet" href="/css/extraSearch.css?t=<%=ConfigurationManager.AppSettings["Version"]%>" />  
    <script type="text/javascript" src="/js/extraSearch.js?t=<%=ConfigurationManager.AppSettings["Version"]%>" ></script>
     <style>
         body{
             font-size:small;
         }
        .bar input[type=search]{
             margin:0;
             height:1.4rem;
        }
        .row
        {
            margin:0.1rem 0 0 0 ;
        }
        .bar .button {
            top:0;
        }
        .bar-nav
        {
            height:5.5rem;
        }
        .bar-nav ~ .content {
            top: 5.5rem;
        }
        .list-block {
            margin:0;
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

        /************************************************ 更多查询*********************************/
        .morediv{
            width: 98%;
            left: 1%;
            right: 1%;
            margin-left: 0px;
        }    
        #page-infinite-scroll-bottom .search-input input{
            border-radius:0;font-size:13px;
        }

    </style>
    <script type="text/javascript">
        
        // 加载flag
        var loading = false;
        //最后一条信息
        var lastnum = 0;
        // 最多可加载的条目
        var maxItems = 100;
        // 每次加载添加多少条目
        var pagesize = 6;
        $(function () {
            //----------------------------------------------------------------------------------------------------------------查询条件
            //查询条件初始化
            initsearch_condition();
            //高级查询
            initSerach_SubscribeBusi();
            //列名
            $("#btn_gridname_m").click(function () {
                showGridName();
            });
            //重置
            $("#btn_reset_m").click(function () {
                $("#txt_subscribetime_s").val(""); $("#txt_subscribetime_e").val("");
                $("#txt_subscribetime_s").calendar({}); $("#txt_subscribetime_e").calendar({});//否则之前选的那天  不能再次选中

                $("#txt_busiunit").val(""); $("#txt_customareacode").val("");
                $("#picker_trigger").picker("setValue", ["未触发"]); 

                $("#txt_busitype").val("");
                $("#txt_ordercode").val("");
                $("#txt_cusno").val("");
                $("#txt_divideno").val("");
                $("#txt_contractno").val("");
                $("#txt_submittime_s").val("");
                $("#txt_submittime_e").val("");
            });

            $('.infinite-scroll-preloader').hide();
            //查询
            $(document).on('click', '#btn_search_m', function (e) {
                queryData();
            });

            //无限滚动 注册'infinite'事件处理函数
            $(document).on('infinite', "#page-infinite-scroll-bottom", function () {
                // 如果正在加载，则退出
                if (loading) return;
                // 设置flag
                loading = true;
                //显示加载栏
                $('.infinite-scroll-preloader').show();
                // 模拟1s的加载过程
                setTimeout(function () {
                    // 重置加载flag
                    loading = false;
                    if (lastnum >= maxItems || lastnum % pagesize != 0) {
                        // 加载完毕，则注销无限加载事件，以防不必要的加载
                        $.detachInfiniteScroll($('.infinite-scroll'));
                        // 删除加载提示符
                        $('.infinite-scroll-preloader').hide();
                        $.toast("已经加载到最后");
                        return;
                    }
                    // 添加新条目
                    loadData(pagesize, lastnum);

                    if (lastnum == $('#subcontent .list-block').length) {
                        $.detachInfiniteScroll($('.infinite-scroll'));// 加载完毕，则注销无限加载事件，以防不必要的加载     
                        $('.infinite-scroll-preloader').hide();

                        $.toast("已经加载到最后");
                        return;
                    }
                    // 更新最后加载的序号
                    lastnum = $('#subcontent .list-block').length;
                    //容器发生改变,如果是js滚动，需要刷新滚动
                    $.refreshScroller();
                }, 500);
            });
            $.init();
        })
        
        function queryData()
        {
            $("#subcontent").html("");
            $.showPreloader('加载中...');
            lastnum = 0;
            $('.infinite-scroll-preloader').show();
            $.attachInfiniteScroll($('.infinite-scroll'));
            setTimeout(function () {
                $.closeModal();
                loadData(pagesize, lastnum);//加载数据
                lastnum = $('#subcontent .list-block').length;//获取数据条数
                $.refreshScroller();//刷新滚动条
                $('.infinite-scroll-bottom').scrollTop(0);//滚动条置顶

                if (lastnum < pagesize) {
                    $.detachInfiniteScroll($('.infinite-scroll-bottom'));// 加载完毕，则注销无限加载事件，以防不必要的加载     
                    $('.infinite-scroll-preloader').hide();
                    if (lastnum == 0) { $.toast("没有符合的数据！"); }
                    else { $.toast("已经加载到最后"); }
                }
                $.hidePreloader();
            }, 500);
        }

        function loadData(pagesize, lastnum) {
            $.ajax({
                url: 'NewSubscribeList_busi.aspx/QuerySubscribeInfo',
                contentType: "application/json; charset=utf-8",
                type: 'post',
                dataType: 'json',
                data: "{'subscribestart':'" + $("#txt_subscribetime_s").val() +
                    "','subscribeend':'" + $("#txt_subscribetime_e").val() +
                    "','busiunit':'" + $("#txt_busiunit").val() +
                    "','istigger':'" + $("#picker_trigger").val() +
                    "','busitype':\"" + $("#txt_busitype").val() +
                    "\",'ordercode':'" + $("#txt_ordercode").val() +
                    "','cusno':'" + $("#txt_cusno").val() +
                    "','divideno':'" + $("#txt_divideno").val() +
                    "','contract':'" + $("#txt_contractno").val() +
                    "','submitstart':'" + $("#txt_submittime_s").val() +
                    "','submitend':'" + $("#txt_submittime_e").val() +
                    "','pagesize':" + pagesize +
                    ",'lastnum':" + lastnum +
                    "}",
                cache: false,
                async: false, //默认是true，异步；false为同步，此方法执行完在执行下面代码
                success: function (data) {
                    if (data.d == null || data.d == "")
                        return;
                    var obj = eval("(" + data.d + ")"); //将字符串转为json
                    for (var i = 0; i < obj.length; i++) {
                        var obj = eval("(" + data.d + ")"); //将字符串转为json
                        $("#span_sum").text(obj[0]["SUM"]);
                        var str = '<div class="list-block" id="' +
                            obj[i]["CUSNO"] +
                            '">' +
                            '<ul>' +
                            '<li class="item-content">' +
                            '<div class="item-inner">' +
                            '<div class="my-title">' +
                            (obj[i]["BUSIUNITNAME"] == null ? "" : obj[i]["BUSIUNITNAME"]) +
                            '</div>' +
                            '<div class="my-after">' +
                            (obj[i]["BUSINAME"] == null ? "" : obj[i]["BUSINAME"]) +
                            '</div>' +
                            '<div class="my-after">' +
                            (obj[i]["CUSNO"] == null ? "" : obj[i]["CUSNO"]) +
                            '</div>' +
                            '</div>' +
                            '</li>' +
                            '<li class="item-content">' +
                            '<div class="item-inner">' +
                            '<div class="my-title">' +
                            (obj[i]["DIVIDENO"] == null ? "" : obj[i]["DIVIDENO"]) +
                            '</div>' +
                            '<div class="my-after">' +
                            (obj[i]["REPWAYNAME"] == null ? "" : obj[i]["REPWAYNAME"]) +
                            '</div>' +
                            '<div class="my-after">' +
                            (obj[i]["CONTRACTNO"] == null ? "" : obj[i]["CONTRACTNO"]) +
                            '</div>' +
                            '</div>' +
                            '</li>' +
                            '<li class="item-content">' +
                            '<div class="item-inner">' +
                            '<div class="my-title">' +
                            (obj[i]["GOODSNUM"] == null ? "" : obj[i]["GOODSNUM"]) +
                            '/' +
                            (obj[i]["GOODSGW"] == null ? "" : obj[i]["GOODSGW"]) +
                            '</div>' +
                            '<div class="my-after">' +
                            (obj[i]["DECLSTATUS"] == null ? "" : obj[i]["DECLSTATUS"]) +
                            '</div>' +
                            '<div class="my-after">' +
                            (obj[i]["INSPSTATUS"] == null ? "" : obj[i]["INSPSTATUS"]) +
                            '</div>' +
                            '</div>' +
                            '</li>' +
                            '<li class="item-content">' +
                            '<div class="item-inner">' +
                            '<div class="my-title">' +
                            (obj[i]["PUSHTIME"] == null ? "" : obj[i]["PUSHTIME"]) +
                            '</div>' +
                            '<div class="my-after">' +
                            (obj[i]["STATUS"] == null ? "" : obj[i]["STATUS"]) +
                            '</div>' +
                            '<div class="my-after" style="color:blue;cursor:pointer;" onclick="delSubscribeInfo(' + obj[i]["ID"] + ',' + obj[i]["TRIGGERSTATUS"] + ')">' +
                            (obj[i]["TRIGGERSTATUS"] == 0 ? "删除" : "") +
                            '</div>' +
                            '</div>' +
                            '</li>' +
                            '</ul>' +
                            '</div>';
                        $("#subcontent").append(str);
                    }
                }
            });
        }

        function showGridName() {
            var strname = '<div class="list-block" >' +
                            '<ul>' +
                            '<li class="item-content">' +
                            '<div class="item-inner">' +
                            '<div class="my-title">经营单位</div>' +
                            '<div class="my-after">业务类型</div>' +
                            '<div class="my-after">企业编号</div>' +
                            '</div>' +
                            '</li>' +
                            '<li class="item-content">' +
                            '<div class="item-inner">' +
                            '<div class="my-title">分单号</div>' +
                            '<div class="my-after">申报方式</div>' +
                            '<div class="my-after">合同号</div>' +
                            '</div>' +
                            '</li>' +
                            '<li class="item-content">' +
                            '<div class="item-inner">' +
                            '<div class="my-title">件数/毛重</div>' +
                            '<div class="my-after">报关状态</div>' +
                            '<div class="my-after">报检状态</div>' +
                            '</div>' +
                            '</li>' +
                            '<li class="item-content">' +
                            '<div class="item-inner">' +
                            '<div class="my-title">推送时间</div>' +
                            '<div class="my-after">订阅状态</div>' +
                            '<div class="my-after">删除按钮</div>' +
                            '</div>' +
                            '</li>' +
                            '</ul>' +
                            '</div>';
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

        //查询条件初始化
        function initsearch_condition() {
            $("#picker_trigger").picker({
                toolbarTemplate: '<header class="bar bar-nav">\
                      <button class="button button-link pull-right close-picker">确定</button>\
                      <h1 class="title">请选择触发情况</h1>\
                      </header>',
                cols: [
                  {
                      textAlign: 'center',
                      values: ['未触发', '已触发', '已推送']
                  }
                ]
            });


            //初始化时间控件
            var before = new Date();
            before.setDate(before.getDate() - 3);
            var beforeday = before.Format("yyyy-MM-dd");

            var now = new Date();
            var today = now.Format("yyyy-MM-dd");

            $("#txt_subscribetime_s").val(beforeday);
            $("#txt_subscribetime_s").calendar({ value: [beforeday] });

            $("#txt_subscribetime_e").val(today);
            $("#txt_subscribetime_e").calendar({ value: [today] });

        }


        function delSubscribeInfo(id, status) {
            if (status != 0)
                return;
            $.ajax({
                url: "NewSubscribeList_busi.aspx/DeleteSubscribeInfo",
                contentType: "application/json; charset=utf-8",
                type: "post",
                data: "{'id':'" + id + "'}",
                dataType: "json",
                cache: false,
                async: false, //默认是true，异步；false为同步，此方法执行完在执行下面代码
                success: function (data) {
                    if (data.d == true) {
                        $.toast("删除成功");
                        queryData();
                    }
                    else {
                        $.toast("删除失败");
                    }
                }
            })
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
                        <div class="col-33" style="width:20%;font-size:13px;margin-top:.3rem;">订阅时间:</div>
                        <div class="col-33" style="width:33%;margin-left:0;"><input type="search" id='txt_subscribetime_s'/></div>
                        <div class="col-33" style="width:33%;margin-left:0;"><input type="search" id='txt_subscribetime_e'/></div>
                        <div class="col-10" style="width:10%;margin-left:0;">
                            <input id="btn_more_m" type="button" value="..." class="open-tabs-modal" style="background-color:#3D4145;color:#ffffff;border-radius:0;border:0;"  />
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-70" style="width:75%;margin-left:4%;"><input type="search" id='txt_busiunit' placeholder='经营单位'/></div>
                        <div class="col-30" style="width:21%;margin-left:0;"><input type="search" id='picker_trigger' placeholder='未触发'/></div>
                    </div> 
                    <div class="row">
                        <div class="col-25" style="width:21%;"><input id="btn_gridname_m" type="button"  value="列名" style="background-color:#808080;color:#ffffff;border-radius:0;border:0;" /></div>
                        <div class="col-60" style="width:54%;margin-left:0;"><input id="btn_search_m" type="button" value="查询" style="background-color:#3D4145;color:#ffffff;border-radius:0;border:0;" /></div>
                        <div class="col-25" style="width:21%;margin-left:0;"><input id="btn_reset_m" type="button" value="重置" style="background-color:#808080;color:#ffffff;border-radius:0;border:0;" /></div>
                    </div> 
                    <input type="hidden" id='txt_busitype'/>
                    <input type="hidden" id='txt_ordercode'/> 
                    <input type="hidden" id='txt_cusno'/>  
                    <input type="hidden" id='txt_divideno'/> 
                    <input type="hidden" id='txt_contractno'/>
                    <input type="hidden" id='txt_submittime_s'/>
                    <input type="hidden" id='txt_submittime_e'/>                   
                </div>  
                <div id="div_tbar" style="font-size:13px;margin:.2rem 0;">
                    <span style="color:#929292">共计</span>
                    <span id="span_sum">0</span>
                    <span style="color:#929292">笔</span>
                </div>
            </header>
            
             <!-- 这里是页面内容区 -->
            <div class="content infinite-scroll infinite-scroll-bottom" data-distance="100" id="scroll-bottom-one">
                <div id ="subcontent"></div>
                <!-- 加载提示符 -->
                <div class="infinite-scroll-preloader"> 
                    <div class="preloader"></div>
                </div>
            </div>
            
        </div>
    </div>  
<script type='text/javascript' src='//g.alicdn.com/msui/sm/0.6.2/js/sm.min.js' charset='utf-8'></script>  
    
</body>
</html>
