<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SubscribeList_decl.aspx.cs" Inherits="WeChat.Page.MyBusiness.SubscribeList_decl" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
    <meta name="viewport" content="initial-scale=1, maximum-scale=1" />
    <link href="/css/iconfont/iconfont.css" rel="stylesheet" />
    <link rel="stylesheet" href="//g.alicdn.com/msui/sm/0.6.2/css/sm.min.css" />
    <script type='text/javascript' src='//g.alicdn.com/sj/lib/zepto/zepto.min.js' charset='utf-8'></script>
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
            margin:0.2rem 0 0 0 ;
        }
        .bar .button {
            top:0;
        }
        .bar-nav
        {
            height:5rem;
        }
        .bar-nav ~ .content {
            top: 5rem;
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

        /************************************************ 更多查询*********************************/
        .morediv{
            width: 98%;
            left: 1%;
            right: 1%;
            margin-left: 0px;
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
            $("#txt_startdate").calendar({});
            $("#txt_enddate").calendar({});

            $('.infinite-scroll-preloader').hide();
            //查询
            $(document).on('click', '#search_a', function (e) {
                $("#subcontent").html("");
                $.showPreloader('加载中...');
                lastnum = 0;
                $('.infinite-scroll-preloader').show();
                $.attachInfiniteScroll($('.infinite-scroll'));
                setTimeout(function () {
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
                }, 500);
                $.hidePreloader();
            })
            //无限滚动 注册'infinite'事件处理函数
            $(document).on('infinite', "#pageone", function () {
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
        

        function loadData(pagesize, lastnum) {
            $.ajax({
                url: 'SubscribeList_decl.aspx/QuerySubscribeInfo',
                contentType: "application/json; charset=utf-8",
                type: 'post',
                dataType: 'json',
                data: "{'starttime':'" + $("#txt_startdate").val() +
                        "','endtime':'" + $("#txt_enddate").val() +
                        "','istigger':'" + $("#picker_tigger").val() +
                        "','declcode':'" + $("#txt_code").val() +
                        "','pagesize':" + pagesize +
                        ",'lastnum':" + lastnum + "}",
                cache: false,
                async: false,//默认是true，异步；false为同步，此方法执行完在执行下面代码
                success: function (data) {
                    var obj = eval("(" + data.d + ")");//将字符串转为json
                    for (var i = 0; i < obj.length; i++) {
                        var obj = eval("(" + data.d + ")");//将字符串转为json
                        var str = '<div class="list-block" id="' + obj[i]["CODE"] + '" ondblclick="subscribe(' + obj[i]["CODE"] + ')">' +
                                    '<ul>' +
                                        '<li class="item-content">' +
                                            '<div class="item-inner">' +
                                                '<div class="my-title">' + obj[i]["DECLARATIONCODE"] + '</div>' +
                                                '<div class="my-after">' + obj[i]["GOODSNUM"] + '/' + obj[i]["GOODSGW"] + '</div>' +
                                                '<div class="my-after">' + obj[i]["MODIFYFLAG"] + '</div>' +
                                            '</div>' +
                                        '</li>' +
                                        '<li class="item-content">' +
                                            '<div class="item-inner">' +
                                                '<div class="my-title">' + obj[i]["TRANSNAME"] + '</div>' +
                                                '<div class="my-after">' + obj[i]["TRADENAME"] + '</div>' +
                                                '<div class="my-after">' + obj[i]["CUSTOMSSTATUS"] + '</div>' +
                                            '</div>' +
                                        '</li>' +
                                        '<li class="item-content">' +
                                            '<div class="item-inner">' +
                                                '<div class="my-title">' + obj[i]["SUBSTATUS"] +  '</div>' +
                                                '<div class="my-after"></div>' +
                                                '<div class="my-after"></div>' +
                                            '</div>' +
                                        '</li>' +
                                    '</ul>' +
                                  '</div>';
                        $("#subcontent").append(str);
                    }
                }
            })
        }
    </script>
</head>
<body>
  <div class="page-group">
        <div id="pageone" class="page page-current">
            <%--search --%>
            <header class="bar bar-nav">
                <div class="search-input">                    
                    <div class="row">
                        <div class="col-50"><input type="search" id='txt_startdate' placeholder='订阅起始日期'/></div>
                        <div class="col-50"><input type="search" id='txt_enddate' placeholder='订阅结束日期'/></div>
                    </div>    
                    <div class="row"> 
                        <div class="col-50"><input type="text" id='picker_tigger' placeholder='是否触发'/></div>
                        <div class="col-50"><input type="text" id='txt_code' placeholder='报关单号'/></div>
                    </div> 
                    <div class="row"> 
                        <div class="col-100"><a href="#" id="search_a" class="open-preloader-title button button-fill">查  询</a> </div>
                    </div>                
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
    <script type="text/javascript">
        //创建picker
        $("#picker_tigger").picker({
            toolbarTemplate: '<header class="bar bar-nav">\
  <button class="button button-link pull-right close-picker">确定</button>\
  <h1 class="title">请选择</h1>\
  </header>',
            cols: [
              {
                  textAlign: 'center',
                  values: ['全部', '未触发', '已触发']
              }
            ]
        });
    </script> 
</body>
</html>
