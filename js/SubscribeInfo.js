//业务订阅信息查询
function subinfoload_busi() {
    $.showPreloader('加载中...');
    $.ajax({
        url: '../NewSubscribeList_busi.aspx/NewQuerySubscribeInfo',
        contentType: "application/json; charset=utf-8",
        type: 'post',
        dataType: 'json',
        
        cache: false,
        async: false, //默认是true，异步；false为同步，此方法执行完在执行下面代码
        success: function (data) {
            if (data.d == null || data.d == "" || data.d == "[]") {
                $.hidePreloader();
                return;
            }
            $("#subscribeinfo").html("");
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
                    '<div class="my-after" style="color:blue;cursor:pointer;" onclick="delSubscribeInfo(' + obj[i]["ID"] + ',' + obj[i]["TRIGGERSTATUS"] + ')">' +
                    (obj[i]["TRIGGERSTATUS"] == 0 ? "删除" : "") +
                    '</div>' +
                    '</div>' +
                    '</li>' +
                    '</ul>' +
                    '</div>';
                $("#subscribeinfo").append(str);
            }
            $.hidePreloader();
            $.popup(".pop-subscribeinfo");

        }
    });
}

//删除订阅信息
function delSubscribeInfo(id, status) {
    if (status != 0)
        return;
    $.confirm('请确认是否<font color=blue>删除订阅</font>?', function () {//OK事件
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
                    subinfoload_busi();
                }
                else {
                    $.toast("删除失败");
                }
            }
        });
    });

}

//加载数据
function subinfoload_decl() {
    $.showPreloader('加载中...');
    $.ajax({
        url: '../NewSubscribeList_decl.aspx/NewQuerySubscribeInfo',
        contentType: "application/json; charset=utf-8",
        type: 'post',
        dataType: 'json',
        cache: false,
        async: false, //默认是true，异步；false为同步，此方法执行完在执行下面代码
        success: function (data) {
            if (data.d == null || data.d == "" || data.d == "[]") {
                $.hidePreloader();
                return;
            }
            $("#subscribeinfo").html("");
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
                    '<div class="my-after" style="color:blue;cursor:pointer;" onclick="delSubscribeInfo(' + obj[i]["ID"] + ',' + obj[i]["TRIGGERSTATUS"] + ')">' +
                    (obj[i]["TRIGGERSTATUS"] == 0 ? "删除" : "") +
                    '</div>' +
                    '</div>' +
                    '</li>' +
                    '</ul>' +
                    '</div>';
                $("#subscribeinfo").append(str);
            }
            $.hidePreloader();
            $.popup(".pop-subscribeinfo");
        }
    });
}