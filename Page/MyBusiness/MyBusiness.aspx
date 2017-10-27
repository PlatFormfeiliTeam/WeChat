<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="MyBusiness.aspx.cs" Inherits="WeChat.Page.MyBusiness.MyBusiness" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <meta name="viewport" content="initial-scale=1, maximum-scale=1" />
    <link rel="stylesheet" href="//g.alicdn.com/msui/sm/0.6.2/css/sm.min.css" />
    <script type='text/javascript' src='//g.alicdn.com/sj/lib/zepto/zepto.min.js' charset='utf-8'></script>
    <style type="text/css">
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
            overflow:hidden;
            margin-left:-2%;
            margin-top:0.3rem;
        }
        .lab
        {
            /*font-size:smaller;*/
            color:white;
            font-family:"微软雅黑";
        }
        .picker-items-col-wrapper
        {
            width:12rem;
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
        .button
        {
            color:white;
            border:2px solid white;
            background-color:#2B79FF;
            font-size:larger;
            font-family:"微软雅黑";
            height:1.5rem;
            line-height:1.3rem;
        }
    </style>

    <script type="text/javascript">
        // 加载flag
        var loading = false;
        // 最多可加载的条目
        var maxItems = 100;
        // 每次加载添加多少条目
        var itemsPerLoad = 6;
        var lastIndex = 0;
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
                        "','starttime':'" + $("#picker_starttime").val() +
                        "','endtime':'" + $("#picker_endtime").val() +
                        "','itemsperload':" + itemsPerLoad +
                        ",'lastindex':" + lastIndex + "}",
                cache: false,
                async: false,//默认是true，异步；false为同步，此方法执行完在执行下面代码
                success: function (data) {
                    var obj = eval("(" + data.d + ")");//将字符串转为json
                    for (var i = 0; i < obj.length; i++) {
                        var str = '<div class="list-block">' +
                                    '<ul>' +
                                        '<li class="item-content">' +
                                            '<div class="item-inner">' +
                                                '<div class="my-title">' + obj[i]["BUSIUNITNAME"] + '</div>' +
                                                '<div class="my-after">' + obj[i]["BUSITYPENAME"] + '</div>' +
                                                '<div class="my-after">' + obj[i]["CUSNO"] + '</div>' +
                                            '</div>' +
                                        '</li>' +
                                        '<li class="item-content">' +
                                            '<div class="item-inner">' +
                                                '<div class="my-title">' + obj[i]["DIVIDENO"] + '</div>' +
                                                '<div class="my-after">' + obj[i]["REPWAYNAME"] + '</div>' +
                                                '<div class="my-after">' + obj[i]["CONTRACTNO"] + '</div>' +
                                            '</div>' +
                                        '</li>' +
                                        '<li class="item-content">' +
                                            '<div class="item-inner">' +
                                                '<div class="my-title">' + obj[i]["GOODSNUM"] + '/' + obj[i]["GOODSGW"] + '</div>' +
                                                '<div class="my-after">' + obj[i]["ISCHECK"] + '</div>' +
                                                '<div class="my-after">' + obj[i]["CHECKPIC"] + '</div>' +
                                            '</div>' +
                                        '</li>' +
                                        '<li class="item-content">' +
                                            '<div class="item-inner">' +
                                                '<div class="my-title">' + obj[i]["DECLSTATUS"] + '</div>' +
                                                '<div class="my-after">' + obj[i]["INSPCHECK"] + '</div>' +
                                                '<div class="my-after">' + obj[i]["LAWFLAG"] + '</div>' +
                                            '</div>' +
                                        '</li>' +
                                        '<li class="item-content">' +
                                            '<div class="item-inner">' +
                                                '<div class="my-title">' + obj[i]["INSPSTATUS"] + '</div>' +
                                                '<div class="my-after">' + obj[i]["LOGISTICSSTATUS"] + '</div>' +
                                                '<div class="my-after">海关状态</div>' +
                                            '</div>' +
                                        '</li>' +
                                    '</ul>' +
                                  '</div>';
                        $("#busicontent").append(str);
                    }
                }
            });
        }

        $(function () {
            $('.infinite-scroll-preloader').hide();
            //FastClick.attach(document.body);
            //查询
            $(document).on('click', '#button_one', function () {
                $("#busicontent").html("");
                $.showPreloader('加载中...');
                lastIndex = 0;
                $('.infinite-scroll-preloader').show();
                $.attachInfiniteScroll($('.infinite-scroll'));
                setTimeout(function () {
                    loadData(itemsPerLoad, lastIndex);//加载数据
                    lastIndex = $('#busicontent .list-block').length;//获取数据条数
                    $.refreshScroller();//刷新滚动条
                    $('.infinite-scroll-bottom').scrollTop(0);//滚动条置顶

                    if (lastIndex < itemsPerLoad) {
                        $.detachInfiniteScroll($('.infinite-scroll-bottom'));// 加载完毕，则注销无限加载事件，以防不必要的加载     
                        $('.infinite-scroll-preloader').hide();

                        if (lastIndex == 0) { $.toast("没有符合的数据！"); }
                        else { $.toast("已经加载到最后"); }
                    }
                }, 1000);
                $.hidePreloader();
            })

            $(document).on("pageInit", "#router1", function (e, id, page) {
                //无限滚动 注册'infinite'事件处理函数
                $(page).on('infinite',  function () {
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
                        if (lastIndex >= maxItems || lastIndex % itemsPerLoad != 0) {
                            // 加载完毕，则注销无限加载事件，以防不必要的加载
                            $.detachInfiniteScroll($('.infinite-scroll'));
                            // 删除加载提示符
                            $('.infinite-scroll-preloader').hide();
                            return;
                        }
                        // 添加新条目
                        loadData(itemsPerLoad, lastIndex);
                        
                        if (lastIndex == $('#busicontent .list-block').length) {
                            $.detachInfiniteScroll($('.infinite-scroll'));// 加载完毕，则注销无限加载事件，以防不必要的加载     
                            $('.infinite-scroll-preloader').hide();

                            $.toast("已经加载到最后");
                            return;
                        }
                        // 更新最后加载的序号
                        lastIndex = $('#busicontent .list-block').length;
                        //容器发生改变,如果是js滚动，需要刷新滚动
                        $.refreshScroller();
                    }, 1000);
                });
            })
            $.init();

        })
    </script>
</head>
<body>
    <div class="page-group">
        <!-- page1 消息订阅-->
        <div class="page page-current"  id="router1">
            <!-- 标题栏 -->
            <header class="bar bar-nav">
                <a  style=" width:4rem;" class="icon icon-search pull-right open-panel">查询</a>
                <h1 class="title">我的业务</h1>
            </header>
            <!-- 工具栏 -->
            <nav class="bar bar-tab">
                <a class="tab-item  active" href="#router1">
                    <span class="icon icon-edit"></span>
                    <span class="tab-label">消息订阅</span>
                </a>
                <a class="tab-item " href="#router2">
                    <span class="icon icon-menu"></span>
                    <span class="tab-label">订阅清单</span>
                </a>
                <a class="tab-item " href="#router3">
                    <span class="icon icon-star"></span>
                    <span class="tab-label">报关单调阅</span>
                </a>
                <a class="tab-item " href="#router4">
                    <span class="icon icon-picture"></span>
                    <span class="tab-label">查验图片调阅</span>
                </a>
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
        
        <!-- page2 订阅清单-->
        <div class="page" id="router2">
            <header class="bar bar-nav">
                <a class="icon icon-me pull-left open-panel"></a>
                <h1 class="title">我的业务</h1>
            </header>
            <nav class="bar bar-tab">
                <a class="tab-item " href="#router1">
                    <span class="icon icon-edit"></span>
                    <span class="tab-label">消息订阅</span>
                </a>
                <a class="tab-item  active" href="#router2">
                    <span class="icon icon-menu"></span>
                    <span class="tab-label">订阅清单</span>
                </a>
                <a class="tab-item " href="#router3">
                    <span class="icon icon-star"></span>
                    <span class="tab-label">报关单调阅</span>
                </a>
                <a class="tab-item " href="#router4">
                    <span class="icon icon-picture"></span>
                    <span class="tab-label">查验图片调阅</span>
                </a>
            </nav>
            <div class="content">
                <div class="content-block">这里是content2</div>
            </div>
        </div>
        <!-- page3 报关单调阅-->
        <div class="page"  id="router3">
            <header class="bar bar-nav">
                <a class="icon icon-me pull-left open-panel"></a>
                <h1 class="title">我的业务</h1>
            </header>
            <nav class="bar bar-tab">
                <a class="tab-item " href="#router1">
                    <span class="icon icon-edit"></span>
                    <span class="tab-label">消息订阅</span>
                </a>
                <a class="tab-item " href="#router2">
                    <span class="icon icon-menu"></span>
                    <span class="tab-label">订阅清单</span>
                </a>
                <a class="tab-item  active" href="#router3">
                    <span class="icon icon-star"></span>
                    <span class="tab-label">报关单调阅</span>
                </a>
                <a class="tab-item " href="#router4">
                    <span class="icon icon-picture"></span>
                    <span class="tab-label">查验图片调阅</span>
                </a>
            </nav>
            <div class="content">
                <div class="content-block">这里是content3</div>
            </div>
        </div>
        <!-- page4 查验图片调阅-->
        <div class="page"  id="router4">
            <header class="bar bar-nav">
                <a class="icon icon-me pull-left open-panel"></a>
                <h1 class="title">我的业务</h1>
            </header>
            <nav class="bar bar-tab">
                <a class="tab-item " href="#router1">
                    <span class="icon icon-edit"></span>
                    <span class="tab-label">消息订阅</span>
                </a>
                <a class="tab-item " href="#router2">
                    <span class="icon icon-menu"></span>
                    <span class="tab-label">订阅清单</span>
                </a>
                <a class="tab-item " href="#router3">
                    <span class="icon icon-star"></span>
                    <span class="tab-label">报关单调阅</span>
                </a>
                <a class="tab-item  active" href="#router4">
                    <span class="icon icon-picture"></span>
                    <span class="tab-label">查验图片调阅</span>
                </a>
            </nav>
            <div class="content">
                <div class="content-block">这里是content4</div>
            </div>
        </div>

    </div>

    <!-- popup, panel 等放在这里 -->
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
                    <div class="col-95"><span class="lab">现场申报：</span><input type="text" id='picker_sitedeclare'/></div>                    
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
            <p><a href="#" class="open-preloader-title button button-round" id="button_one">查&nbsp;&nbsp;&nbsp;&nbsp;询</a></p>
            <%--<p><a href="#" class="button button-round">重&nbsp;&nbsp;&nbsp;&nbsp;置</a></p>--%>
        </div>
    </div>


    <!-- 默认必须要执行$.init(),实际业务里一般不会在HTML文档里执行，通常是在业务页面代码的最后执行 -->
    
    <script type='text/javascript' src='//g.alicdn.com/msui/sm/0.6.2/js/sm.min.js' charset='utf-8'></script>
    <script type="text/javascript">
        //初始化控件的值
        var myDate = new Date();
        var nowDate = myDate.toLocaleDateString();
        $("#picker_starttime").val(nowDate);
        $("#picker_endtime").val(nowDate);
        //创建picker
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
        $("#picker_inspstatus").picker({
            toolbarTemplate: '<header class="bar bar-nav">\
  <button class="button button-link pull-right close-picker">确定</button>\
  <h1 class="title">请选择</h1>\
  </header>',
            cols: [
              {
                  textAlign: 'center',
                  values: ['全部', '申报完结', '现场报检','现场放行']
              }
            ]
        });
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
        $("#picker_busitype").picker({
            toolbarTemplate: '<header class="bar bar-nav">\
  <button class="button button-link pull-right close-picker">确定</button>\
  <h1 class="title">请选择</h1>\
  </header>',
            cols: [
              {
                  textAlign: 'center',
                  values: ['全部（不含国内）', '国内业务', '特殊区域','空运业务','陆运业务']
              }
            ]
        });
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
        
        $("#picker_starttime").calendar({
            value: [nowDate],
            dateFormat:'yyyy/mm/dd'
        });
        $("#picker_endtime").calendar({
            value: [nowDate],
            dateFormat: 'yyyy/mm/dd'
        });



    </script>
</body>
   

</html>
