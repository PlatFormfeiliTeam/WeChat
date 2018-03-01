<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SignOut.aspx.cs" Inherits="WeChat.Page.SignOut" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title>解绑绑定</title>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <meta name="viewport" content="initial-scale=1, maximum-scale=1" />
    <script type='text/javascript' src='//g.alicdn.com/sj/lib/zepto/zepto.min.js' charset='utf-8'></script>
    <link rel="stylesheet" href="//g.alicdn.com/msui/sm/0.6.2/css/sm.min.css" />
    <script type="text/javascript">
        $(function(){
            $.ajax({
                type: 'post',
                url: 'SignOut.aspx/getUserInfo',
                dataType: 'json',
                contentType: 'application/json; charset=utf-8',
                cache: false,
                async: false,
                success: function (data) {
                    var obj = eval("(" + data.d + ")");
                    if (obj.flag == "true") {
                        $("#name").val(obj.code);
                        $("#pwd").val(obj.pwd);
                        $("#customer").val(obj.customercode);
                    }
                    else {
                        $("#errorinfo").html(obj.url);
                    }
                }
            })
        })
        function cancle() {
            $("#name").val("");
            $("#pwd").val("");
            $("#customer").val("");
        }
        function signout() {
            var name = $("#name").val();
            var pwd = $("#pwd").val();
            var customer = $("#customer").val();
            $.ajax({
                type: 'post',
                url: 'SignOut.aspx/UserSignOut',
                dataType: 'json',
                contentType: 'application/json; charset=utf-8',
                data: "{'name':'" + name + "','pwd':'" + pwd + "','customer':'" + customer + "'}",
                cache: false,
                async: false,
                success: function (data) {
                    var obj = eval("(" + data.d + ")");
                    if (obj.flag == "true") {
                        $("#errorinfo").html(obj.url);
                    }
                    else {
                        $("#errorinfo").html(obj.url);
                    }
                }

            })
        }
    </script>
</head>
<body>
    <div class="content">
      <div class="list-block">
        <ul>
          <!-- Text inputs -->
          <li>
            <div class="item-content">
              <div class="item-media"><i class="icon icon-form-name"></i></div>
              <div class="item-inner">
                <div class="item-title label">姓名</div>
                <div class="item-input">
                  <input id="name" type="text" placeholder="请输入姓名" />
                </div>
              </div>
            </div>
          </li>
          <li>
            <div class="item-content">
              <div class="item-media"><i class="icon icon-form-password"></i></div>
              <div class="item-inner">
                <div class="item-title label">密码</div>
                <div class="item-input">
                  <input id="pwd" type="password" placeholder="请输入密码" class="" />
                </div>
              </div>
            </div>
          </li>
          <li>
            <div class="item-content">
              <div class="item-media"><i class="icon icon-form-name"></i></div>
              <div class="item-inner">
                <div class="item-title label">客商</div>
                <div class="item-input">
                  <input id="customer" type="text" placeholder="请输入客商代码" />
                </div>
              </div>
            </div>
          </li>
          <li>
            <div class="item-content">
              <div class="item-media"><i class="icon icon-form-name"></i></div>
              <div class="item-inner">
                <div class="label" id="errorinfo" style="color:red"></div>
              </div>
            </div>
          </li>
        </ul>
      </div>
      <div class="content-block">
        <div class="row">
          <%--<div class="col-50"><a href="javascript:cancle()" class="button button-big button-fill" style="background-color: gray">取消</a></div>--%>
          <div class="col-100"><a href="javascript:signout()" class="button button-big button-fill" style="background-color: #3d4145;">解除</a></div>
        </div>
      </div>
    </div>
    
</body>
</html>
