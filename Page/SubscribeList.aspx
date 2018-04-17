<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SubscribeList.aspx.cs" Inherits="WeChat.Page.SubscribeList" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>订阅清单</title>
    <meta name="viewport" content="initial-scale=1, maximum-scale=1" />
    <link href="/css/iconfont/iconfont.css" rel="stylesheet" />
    <link rel="stylesheet" href="//g.alicdn.com/msui/sm/0.6.2/css/sm.min.css" />
    <script type='text/javascript' src='//g.alicdn.com/sj/lib/zepto/zepto.min.js' charset='utf-8'></script>

    <style type="text/css">
        .list-block {
            margin: 0 0 0 0;
        }

            .list-block .item-content {
                min-height: 1.2rem;
            }

            .list-block .item-inner {
                padding-top: 0.1rem;
                padding-bottom: 0.1rem;
                min-height: 1.2rem;
                font-size: small;
            }

            .list-block .my-title {
                width: 40%;
                overflow: hidden;
                white-space: nowrap;
                text-overflow: ellipsis;
                text-align: center;
            }

            .list-block .my-after {
                width: 30%;
                overflow: hidden;
                white-space: nowrap;
                text-overflow: ellipsis;
                text-align: center;
            }

        .button {
            border-radius: 0;
            background-color: gray;
            color: white;
            border: 0;
            vertical-align: middle;
            padding-top: 0.1rem;
            text-align: center;
        }
        .myscroll {
            position: absolute;
            left:0;
            right:0;
            bottom: 0.5rem;
            top: 2.2rem;
            overflow: auto;
        }
    </style>

    <script type="text/javascript">
        $(function () {
            $.showPreloader('加载中...');
            subinfoload_busi();
            subinfoload_decl();

            $.hidePreloader();
        })

        //业务订阅信息查询
        function subinfoload_busi() {

            $.ajax({
                url: 'SubscribeList.aspx/getBusiSubscribeInfo',
                contentType: "application/json; charset=utf-8",
                type: 'post',
                dataType: 'json',

                cache: false,
                async: false, //默认是true，异步；false为同步，此方法执行完在执行下面代码
                success: function (data) {
                    if (data.d == null || data.d == "" || data.d == "[]") {
                        return;
                    }
                    $("#subscribeinfo_busi").html("");
                    var obj = eval("(" + data.d + ")"); //将字符串转为json
                    for (var i = 0; i < obj.length; i++) {
                        var str = '<div class="list-block" style="border-bottom:solid 1px black" id="' +
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
                            '<div class="my-after" style="color:blue;cursor:pointer;" onclick="delSubscribeInfo(' + obj[i]["ID"] + ',' + obj[i]["TRIGGERSTATUS"] + ',1)">' +
                            (obj[i]["TRIGGERSTATUS"] == 0 ? "删除" : "") +
                            '</div>' +
                            '</div>' +
                            '</li>' +
                            '</ul>' +
                            '</div>';
                        $("#subscribeinfo_busi").append(str);
                    }
                }
            });
        }



        //加载数据
        function subinfoload_decl() {
            $.ajax({
                url: 'SubscribeList.aspx/getDeclSubscribeInfo',
                contentType: "application/json; charset=utf-8",
                type: 'post',
                dataType: 'json',
                cache: false,
                async: false, //默认是true，异步；false为同步，此方法执行完在执行下面代码
                success: function (data) {
                    if (data.d == null || data.d == "" || data.d == "[]") {
                        return;
                    }
                    $("#subscribeinfo_decl").html("");
                    var obj = eval("(" + data.d + ")"); //将字符串转为json
                    for (var i = 0; i < obj.length; i++) {
                        var str = '<div class="list-block" style="border-bottom:solid 1px black" id="' +
                            obj[i]["DECLARATIONCODE"] +
                            '" >' +
                            '<ul>' +
                            '<li class="item-content">' +
                            '<div class="item-inner">' +
                            '<div class="my-title">' +
                            obj[i]["DECLARATIONCODE"] +
                            '</div>' +
                            '<div class="my-after">' +
                            obj[i]["GOODSNUM"] +
                            '/' +
                            obj[i]["GOODSGW"] +
                            '</div>' +
                            '<div class="my-after">' +
                            obj[i]["MODIFYFLAG"] +
                            '</div>' +
                            '</div>' +
                            '</li>' +
                            '<li class="item-content">' +
                            '<div class="item-inner">' +
                            '<div class="my-title">' +
                            obj[i]["TRANSNAME"] +
                            '</div>' +
                            '<div class="my-after">' +
                            obj[i]["TRADENAME"] +
                            '</div>' +
                            '<div class="my-after">' +
                            obj[i]["CUSTOMSSTATUS"] +
                            '</div>' +
                            '</div>' +
                            '</li>' +
                            '<li class="item-content">' +
                            '<div class="item-inner">' +
                            '<div class="my-title">' +
                            (obj[i]["PUSHTIME"] == null ? "" : obj[i]["PUSHTIME"]) +
                            '</div>' +
                            '<div class="my-after">' +
                            obj[i]["STATUS"] +
                            '</div>' +
                            '<div class="my-after" style="color:blue;cursor:pointer;" onclick="delSubscribeInfo(' + obj[i]["ID"] + ',' + obj[i]["TRIGGERSTATUS"] + ',2)">' +
                            (obj[i]["TRIGGERSTATUS"] == 0 ? "删除" : "") +
                            '</div>' +
                            '</div>' +
                            '</li>' +
                            '</ul>' +
                            '</div>';
                        $("#subscribeinfo_decl").append(str);
                    }
                }
            });
        }

        //删除订阅信息
        function delSubscribeInfo(id, status, kind) {
            if (status != 0)
                return;
            $.confirm('请确认是否<font color=blue>删除订阅</font>?', function () {//OK事件
                $.ajax({
                    url: "SubscribeList.aspx/DeleteSubscribeInfo",
                    contentType: "application/json; charset=utf-8",
                    type: "post",
                    data: "{'id':'" + id + "'}",
                    dataType: "json",
                    cache: false,
                    async: false, //默认是true，异步；false为同步，此方法执行完在执行下面代码
                    success: function (data) {
                        if (data.d == true) {
                            $.toast("删除成功");
                            if (kind == 1) subinfoload_busi();
                            else if (kind == 2) subinfoload_busi();
                        }
                        else {
                            $.toast("删除失败");
                        }
                    }
                });
            });
        }

    </script>
</head>
<body>
    <div class="content">
        <div class="buttons-tab">
            <a href="#tab1" class="tab-link active button">业务订阅清单</a>
            <a href="#tab2" class="tab-link button">报关单订阅清单</a>
        </div>
        <div >
            <div class="tabs">
                <div id="tab1" class="tab active">
                    <div class="myscroll" id="subscribeinfo_busi"></div>
                </div>
                <div id="tab2" class="tab">
                    <div class="myscroll" id="subscribeinfo_decl"></div>
                </div>
            </div>
        </div>
    </div>
    <script type='text/javascript' src='//g.alicdn.com/msui/sm/0.6.2/js/sm.min.js' charset='utf-8'></script>

    <script src="/js/sm-extend.min.js"></script>
</body>
</html>
