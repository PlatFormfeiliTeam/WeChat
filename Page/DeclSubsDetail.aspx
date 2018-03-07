<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="DeclSubsDetail.aspx.cs" Inherits="WeChat.Page.DeclSubsDetail" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title>订阅详情</title>
    <meta name="viewport" content="initial-scale=1, maximum-scale=1" />
    <script type='text/javascript' src='//g.alicdn.com/sj/lib/zepto/zepto.min.js' charset='utf-8'></script>
    <link rel="stylesheet" href="//g.alicdn.com/msui/sm/0.6.2/css/sm.min.css" />
    <style type="text/css">
        .row
        {
            font-size:small;
            margin:0.5rem;
        }
        .row .col-40
        {
            text-align:right;
            width:20%;
        }
        .row .col-60
        {
            color:blue;
        }
    </style>
    <script type="text/javascript">
        $(function () {

            var params = GetRequest();

            $.ajax({
                type: 'post',
                url: 'DeclSubsDetail.aspx/getInfo',
                dataType: 'json',
                contentType: 'application/json; charset=utf-8',
                data: "{'code':'" + params["code"] + "'}",
                cache: false,
                async: false,
                success: function (data) {
                    var obj = eval("(" + data.d + ")");
                    var str = '<div class="row">' +
                              '<div class="col-40">报关单号：</div>' +
                              '<div class="col-60">' + (obj[0]["DECLARATIONCODE"] == null ? "" : obj[0]["DECLARATIONCODE"]) + '</div>' +
                            '</div>' +
                            '<div class="row">' +
                              '<div class="col-40">业务类型：</div>' +
                              '<div class="col-60">' + getname("BUSITYPE", obj[0]["BUSITYPE"]) + '</div>' +
                            '</div>' +
                            '<div class="row">' +
                              '<div class="col-40">监管方式：</div>' +
                              '<div class="col-60">' + (obj[0]["TRADEMETHOD"] == null ? "" : obj[0]["TRADEMETHOD"]) + '</div>' +
                            '</div>' +
                            '<div class="row">' +
                              '<div class="col-40">收发货人：</div>' +
                              '<div class="col-60">' + (obj[0]["CONSIGNEESHIPPERNAME"] == null ? "" : obj[0]["CONSIGNEESHIPPERNAME"]) + '</div>' +
                            '</div>' +
                            '<div class="row">' +
                              '<div class="col-40">合同号：</div>' +
                              '<div class="col-60">' + (obj[0]["CONTRACTNO"] == null ? "" : obj[0]["CONTRACTNO"]) + '</div>' +
                            '</div>' +
                            '<div class="row">' +
                              '<div class="col-40">申报日期：</div>' +
                              '<div class="col-60">' + (obj[0]["REPTIME"] == null ? "" : obj[0]["REPTIME"]) + '</div>' +
                            '</div>' +
                            '<div class="row">' +
                              '<div class="col-40">运输工具：</div>' +
                              '<div class="col-60">' + (obj[0]["TRANSNAME"] == null ? "" : obj[0]["TRANSNAME"]) + '/' + obj[0]["GOODSGW"] + '</div>' +
                            '</div>' +
                            '<div class="row">' +
                              '<div class="col-40">件数毛重：</div>' +
                              '<div class="col-60">' + (obj[0]["GOODSNUM"] == null ? "" : obj[0]["GOODSNUM"]) + '/' + (obj[0]["GOODSGW"] == null ? "" : obj[0]["GOODSGW"]) + '</div>' +
                            '</div>' +
                            '<div class="row">' +
                              '<div class="col-40">删改单：</div>' +
                              '<div class="col-60">' + getname("MODIFYFLAG", obj[0]["MODIFYFLAG"]) + '</div>' +
                            '</div>' +
                            '<div class="row">' +
                              '<div class="col-40">提运单号：</div>' +
                              '<div class="col-60">' + (obj[0]["BLNO"] == null ? "" : obj[0]["BLNO"]) + '</div>' +
                            '</div>' +
                            '<div class="row">' +
                              '<div class="col-40">企业编号：</div>' +
                              '<div class="col-60">' + (obj[0]["CUSNO"] == null ? "" : obj[0]["CUSNO"]) + '</div>' +
                            '</div>' +
                            '<div class="row">' +
                              '<div class="col-40">海关状态：</div>' +
                              '<div class="col-60">' + (obj[0]["CUSTOMSSTATUS"] == null ? "" : obj[0]["CUSTOMSSTATUS"]) + '</div>' +
                            '</div>';
                    $("#cont").append(str);
                }
            })
        })
        function GetRequest() {
            var url = location.search; //获取url中"?"符后的字串   
            var theRequest = new Object();
            if (url.indexOf("?") != -1) {
                var str = url.substr(1);
                strs = str.split("&");
                for (var i = 0; i < strs.length; i++) {
                    theRequest[strs[i].split("=")[0]] = unescape(strs[i].split("=")[1]);
                }
            }
            return theRequest;
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

    </script>
</head>
<body>
    <div class="content">
      <div class="content-padded grid-demo" id="cont">
    
      </div>
    </div>
</body>
</html>
<div class="content">
  <div class="content-padded grid-demo">
    
  </div>
</div>