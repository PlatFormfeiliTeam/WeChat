<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="DeclareList.aspx.cs" Inherits="WeChat.Page.BusiOpera.DeclareList" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<%-- <meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>--%>
    <meta charset="utf-8">
    <meta name="viewport" content="initial-scale=1, maximum-scale=1">
    <title>报关单查询</title>
    <link href="/css/iconfont/iconfont.css" rel="stylesheet" />
    <link rel="stylesheet" href="//g.alicdn.com/msui/sm/0.6.2/css/sm.min.css">
    <%--<link rel="stylesheet" href="//g.alicdn.com/msui/sm/0.6.2/css/??sm.min.css,sm-extend.min.css">--%>
    <script type='text/javascript' src='//g.alicdn.com/sj/lib/zepto/zepto.min.js' charset='utf-8'></script>

    <style>
        .bar{
            height:6rem;
        }
        .button{
            height:2.2em;
            font-size:.85rem;
        }
        .button.button-fill{
            line-height:2.2em;
        }
        .list-block{
            margin:.3rem 0;
        }
        .bar input[type=search]{
            height:2rem;
        }
        .bar-nav~.content{
            top:7rem;
        }
        .search-input input{
            padding:.5rem;
        }
        /*.list-group .item-inner:after{
            background-color:gray;
        }*/
    </style>

    <script type="text/javascript">
        $(function () {
            $("#txt_startdate").calendar({});
            $("#txt_enddate").calendar({});

            $(document).on('click', '.open-tabs-modal', function () {
                $.modal({
                    title: '',
                    text: '<div class="tabs">' +
                            '<div class="tab active" id="tab1">Lorem ipsum dolor sit amet, consectetur adipiscing elit. Nullam convallis nunc non dolor euismod feugiat. Sed at sapien nisl. Ut et tincidunt metus. Suspendisse nec risus vel sapien placerat tincidunt. Nunc pulvinar urna tortor.</div>' +
                            '<div class="tab" id="tab2">Vivamus feugiat diam velit. Maecenas aliquet egestas lacus, eget pretium massa mattis non. Donec volutpat euismod nisl in posuere. Vestibulum ante ipsum primis in faucibus orci luctus et ultrices posuere cubilia Curae</div>' +
                          '</div>',
                    buttons: [
                     {
                         text: '重置',
                         onClick: function () {
                             //$.alert('You clicked first button!')
                         }
                     }
                    ]
                })
            });


            $.init();
        });
        
    </script>
</head>
<body>

    <div class="page-group">
        <div id="page-infinite-scroll-bottom" class="page page-current">
            <header class="bar bar-nav">
                <div class="search-input">                    
                    <div class="row"> 
                        <label style="float:left; width:25%; margin-left:4%; height:2rem; padding-top:.8rem">报关单号：</label>
                        <input style="float:left; width:70%;padding:.5rem; " type="search" id='txt_hscode' placeholder='请输入18位或9位报关单号...'/>
                    </div>
                    <div class="row">
                        <div style="float:left; width:25%; margin-left:4%; height:2rem; padding-top:.8rem">申报日期：</div>
                        <div style="float:left; width:28%;"><input type="search" id='txt_startdate' placeholder='起始日期' data-toggle='date'/></div>
                        <div style="float:left; width:3%; height:2rem; padding-top:.8rem;margin-left:1%;margin-right:1%;">~</div>
                        <div style="float:left; width:28%;"><input type="search" id='txt_enddate' placeholder='结束日期' data-toggle='date'/></div>                        
                        <div style="float:left; width:9%; margin-left:1%;"><a href="#" class="open-tabs-modal"><i class="iconfont" style="font-size:1.65rem;color:gray;">&#xe6ca;</i></a></div>
                    </div>                    
                </div>                
                <a href="#" id="search_a" class="open-preloader-title button button-fill"><span class="icon icon-search"></span>&nbsp;查询</a>   
            </header>
            <div data-toggle='date' />
           <%-- <div class="content infinite-scroll native-scroll" data-distance="100">
                <div id="div_list"></div>
                <!-- 加载提示符 -->
                <div class="infinite-scroll-preloader">
                  <div class="preloader"></div>
                </div>
            </div>--%>
        </div>
    </div>  

    <script type='text/javascript' src='//g.alicdn.com/msui/sm/0.6.2/js/sm.min.js' charset='utf-8'></script>   
   <%-- <script type='text/javascript' src='//g.alicdn.com/msui/sm/0.6.2/js/sm-extend.min.js' charset='utf-8'></script>
    <script type="text/javascript" src="//g.alicdn.com/msui/sm/0.6.2/js/sm-city-picker.min.js" charset="utf-8"></script>--%>
</body>
</html>
