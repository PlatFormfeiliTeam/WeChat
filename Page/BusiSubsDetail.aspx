<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="BusiSubsDetail.aspx.cs" Inherits="WeChat.Page.BusiSubsDetail" %>

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
            $.ajax({
                type: 'post',
                url: 'BusiSubsDetail.aspx/getInfo',
                dataType: 'json',
                contentType: 'application/json; charset=utf-8',
                cache: false,
                async: false,
                success: function (data) {
                    var obj = eval("(" + data.d + ")");
                    var str = '<div class="row">' +
                              '<div class="col-40">经营单位：</div>' +
                              '<div class="col-60">' + (obj[0]["BUSIUNITNAME"] == null ? "" : obj[0]["BUSIUNITNAME"]) + '</div>' +
                            '</div>' +
                            '<div class="row">' +
                              '<div class="col-40">业务类型：</div>' +
                              '<div class="col-60">' + (obj[0]["BUSITYPENAME"] == null ? "" : obj[0]["BUSITYPENAME"]) + '</div>' +
                            '</div>' +
                            '<div class="row">' +
                              '<div class="col-40">企业编号：</div>' +
                              '<div class="col-60">' + (obj[0]["CUSNO"] == null ? "" : obj[0]["CUSNO"]) + '</div>' +
                            '</div>' +
                            '<div class="row">' +
                              '<div class="col-40">分单号：</div>' +
                              '<div class="col-60">' + (obj[0]["DIVIDENO"] == null ? "" : obj[0]["DIVIDENO"]) + '</div>' +
                            '</div>' +
                            '<div class="row">' +
                              '<div class="col-40">申报方式：</div>' +
                              '<div class="col-60">' + (obj[0]["REPWAYNAME"] == null ? "" : obj[0]["REPWAYNAME"]) + '</div>' +
                            '</div>' +
                            '<div class="row">' +
                              '<div class="col-40">合同号：</div>' +
                              '<div class="col-60">' + (obj[0]["CONTRACTNO"] == null ? "" : obj[0]["CONTRACTNO"]) + '</div>' +
                            '</div>' +
                            '<div class="row">' +
                              '<div class="col-40">件数毛重：</div>' +
                              '<div class="col-60">' + (obj[0]["GOODSNUM"] == null ? "" : obj[0]["GOODSNUM"]) + '/' + obj[0]["GOODSGW"] + '</div>' +
                            '</div>' +
                            '<div class="row">' +
                              '<div class="col-40">报关查验：</div>' +
                              '<div class="col-60">' + (obj[0]["ISCHECK"] == null ? "" : obj[0]["ISCHECK"]) + '</div>' +
                            '</div>' +
                            '<div class="row">' +
                              '<div class="col-40">查验图片：</div>' +
                              '<div class="col-60">' + (obj[0]["CHECKPIC"] == null ? "" : obj[0]["CHECKPIC"]) + '</div>' +
                            '</div>' +
                            '<div class="row">' +
                              '<div class="col-40">报关状态：</div>' +
                              '<div class="col-60">' + (obj[0]["DECLSTATUS"] == null ? "" : obj[0]["DECLSTATUS"]) + '</div>' +
                            '</div>' +
                            '<div class="row">' +
                              '<div class="col-40">报检查验：</div>' +
                              '<div class="col-60">' + (obj[0]["INSPISCHECK"] == null ? "" : obj[0]["INSPISCHECK"]) + '</div>' +
                            '</div>' +
                            '<div class="row">' +
                              '<div class="col-40">是否法检：</div>' +
                              '<div class="col-60">' + (obj[0]["LAWFLAG"] == null ? "" : obj[0]["LAWFLAG"]) + '</div>' +
                            '</div>' +
                            '<div class="row">' +
                              '<div class="col-40">报检状态：</div>' +
                              '<div class="col-60">' + (obj[0]["INSPSTATUS"] == null ? "" : obj[0]["INSPSTATUS"]) + '</div>' +
                            '</div>' +
                            '<div class="row">' +
                              '<div class="col-40">物流状态：</div>' +
                              '<div class="col-60">' + (obj[0]["LOGISTICSNAME"] == null ? "" : obj[0]["LOGISTICSNAME"]) + '</div>' +
                            '</div>';
                    $("#cont").append(str);
                }
            })
        })
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