<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SubscribeDetail.aspx.cs" Inherits="WeChat.Page.MyBusiness.SubscribeDetail" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>订阅详情</title>
    <meta name="viewport" content="initial-scale=1, maximum-scale=1" />
    <script type='text/javascript' src='//g.alicdn.com/sj/lib/zepto/zepto.min.js' charset='utf-8'></script>
    <link rel="stylesheet" href="//g.alicdn.com/msui/sm/0.6.2/css/sm.min.css" />
    <link rel="stylesheet" href="/css/extraSearch.css?t=<%=ConfigurationManager.AppSettings["Version"]%>" />
    <script type="text/javascript" src="/js/BusiInfoDetail.js?t=<%=ConfigurationManager.AppSettings["Version"]%>"></script>
    <script type="text/javascript">
        $(function () {
            var params = GetRequest();
            var dd = params["code"];
            if (params["code"] != "") getBusiInfo_company(params["code"]);
            else $.popup('.popup-detail');
            //getBusiInfo_company("18020800393");
        });

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
    </script>
</head>
<body>
    <div class="popup popup-detail">
        <div class="content" style="bottom: 60px;">
            <div class="buttons-tab">
                <a href="#tab1" class="tab-link active button">报关信息</a>
                <a href="#tab2" class="tab-link button">报检信息</a>
                <a href="#tab3" class="tab-link button">物流信息</a>
            </div>
            <div class="content-block">
                <div class="tabs">
                    <div id="tab1" class="tab active">
                        <div class="content-block " id="pop_tab_decl"></div>
                    </div>
                    <div id="tab2" class="tab">
                        <div class="content-block" id="pop_tab_insp"></div>
                    </div>
                    <div id="tab3" class="tab">
                        <div class="content-block" id="pop_tab_logistics"></div>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <script type='text/javascript' src='//g.alicdn.com/msui/sm/0.6.2/js/sm.min.js' charset='utf-8'></script>
</body>
</html>
