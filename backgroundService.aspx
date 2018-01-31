<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="backgroundService.aspx.cs" Inherits="WeChat.backgroundService" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <div style="float:left"><asp:Button ID="Button1" runat="server" Text="创建菜单" OnClick="Button1_Click" /></div>
        <div>&nbsp;&nbsp;<asp:Label ID="Label1" runat="server" Text=""></asp:Label></div>
    </div>
    <div style="line-height:50px;">&nbsp;</div>
    <div style="clear:both">
        <div style="float:left">
        <asp:Button ID="Button2" runat="server" Text="订阅服务" OnClick="Button2_Click" />
            </div>
        <div>&nbsp;&nbsp;<asp:Label ID="Label2" runat="server" Text="服务停止"></asp:Label></div>
    </div>
        <div style="clear:both">
        <div style="float:left">
        <asp:Button ID="Button3" runat="server" Text="urlEncode" OnClick="Button3_Click" />
            </div>
        <div>&nbsp;&nbsp;<asp:TextBox ID="TextBox1" runat="server"></asp:TextBox><asp:TextBox ID="TextBox2" Width="300" runat="server"></asp:TextBox></div>
            
    </div>
    </form>
</body>
</html>
