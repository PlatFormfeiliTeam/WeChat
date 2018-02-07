//现场报关高级查询
function initSerach_SiteDeclare() {
    $(document).on('click', '.open-tabs-modal', function () {
        $.modal({
            text: '<div class="content">' +
                    '<div class="content-padded grid-demo">' +
                        '<div id="busitype">' +
                            '<div id="busi_in" class="row">' +
                                '<div class="col-25 labtitle">进口业务</div>' +
                                '<div class="col-25 labcontent">空运进口</div>' +
                                '<div class="col-25 labcontent">海运进口</div>' +
                                '<div class="col-25 labcontent">陆运进口</div>' +
                            '</div>' +
                            '<div id="busi_out" class="row">' +
                                '<div class="col-25 labtitle">出口业务</div>' +
                                '<div class="col-25 labcontent">空运出口</div>' +
                                '<div class="col-25 labcontent">海运出口</div>' +
                                '<div class="col-25 labcontent">陆运出口</div>' +
                            '</div>' +
                            '<div id="busi_other" class="row">' +
                                '<div class="col-25 labcontent">国内进口</div>' +
                                '<div class="col-25 labcontent">国内出口</div>' +
                                '<div class="col-25 labcontent">特殊进口</div>' +
                                '<div class="col-25 labcontent">特殊出口</div>' +
                            '</div>' +
                        '</div>' +
                        '<div id="modify" class="row">' +
                            '<div id="mod_delete" class="col-25 labcontent">删单</div>' +
                            '<div id="mod_update" class="col-25 labcontent">改单</div>' +
                            '<div id="mod_finish" class="col-25 labcontent">改单完成</div>' +
                            '<div id="audit" class="col-25 labcontent">稽核</div>' +
                        '</div>' +
                        '<div class="row">' +
                            '<div class="col-100" style="width:92%"><input type="text" id="busiunit" placeholder="经营单位"/></div>' +
                        '</div>' +
                        '<div class="row">' +
                            '<div class="col-50"><input type="text" id="ordercode" placeholder="订单编号"/></div>' +
                            '<div class="col-50" style="margin-left:0rem;"><input type="text" id="cusno" placeholder="企业编号"/></div>' +
                        '</div>' +
                        '<div class="row">' +
                            '<div class="col-50"><input type="text" id="divideno" placeholder="分单号"/></div>' +
                            '<div class="col-50" style="margin-left:0rem;"><input type="text" id="contractno" placeholder="合同号"/></div>' +
                        '</div>' +
                        '<div class="row">' +
                            '<div class="col-20">委托日期</div>' +
                            '<div class="col-40"><input type="date" id="subdatestart" placeholder="起始" /></div>' +
                            '<div class="col-40" style="margin-left:0rem;"><input type="date" id="subdateend" placeholder="结束"/></div>' +
                        '</div>' +
                        '<div class="row">' +
                            '<div class="col-20">放行日期</div>' +
                            '<div class="col-40"><input type="date" id="passdatestart" placeholder="起始"/></div>' +
                            '<div class="col-40" style="margin-left:0rem;"><input type="date" id="passdateend" placeholder="结束"/></div>' +
                        '</div>' +
                        '<div style="margin-top:0.3rem" class="row">' +
                            '<div class="col-20" ><a href="#" class="button">取消</a></div>' +
                            '<div class="col-60" style="width:59.99%; margin-left:0rem;"><a href="#" style="background-color: #3d4145;" class="button">确&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;认</a></div>' +
                            '<div class="col-20" style="margin-left:0rem;"><a href="#" class="button">重置</a></div>' +
                        '</div>' +
                    '</div>' +
                '</div>',
            extraClass: 'morediv'//避免直接设置.modal的样式，从而影响其他toast的提示
        });

        //绑定labtitle事件
        $("#busi_in .labtitle").click(function () {
            $(this).toggleClass("labhover");
            if ($(this).css("color") == "rgb(0, 0, 255)") {
                $("#busi_in .labcontent").each(function () {
                    $(this).addClass("labhover");
                })
            }
            else {
                $("#busi_in .labcontent").each(function () {
                    $(this).removeClass("labhover");
                })
            }
        });
        $("#busi_out .labtitle").click(function () {
            $(this).toggleClass("labhover");
            if ($(this).css("color") == "rgb(0, 0, 255)") {
                $("#busi_out .labcontent").each(function () {
                    $(this).addClass("labhover");
                })
            }
            else {
                $("#busi_out .labcontent").each(function () {
                    $(this).removeClass("labhover");
                })
            }
        });
        //绑定labcontent事件
        $("#busi_in .labcontent").click(function () {
            $(this).toggleClass("labhover");
            var flag = true;
            $("#busi_in .labcontent").each(function () {
                if ($(this).css("color") != "rgb(0, 0, 255)") {
                    flag = false;
                    return;
                }
            })
            if (flag) {
                $("#busi_in .labtitle").addClass("labhover");
            }
            else {
                $("#busi_in .labtitle").removeClass("labhover");
            }
        });
        $("#busi_out .labcontent").click(function () {
            $(this).toggleClass("labhover");
            var flag = true;
            $("#busi_out .labcontent").each(function () {
                if ($(this).css("color") != "rgb(0, 0, 255)") {
                    flag = false;
                    return;
                }
            })
            if (flag) {
                $("#busi_out .labtitle").addClass("labhover");
            }
            else {
                $("#busi_out .labtitle").removeClass("labhover");
            }
        });
        $("#busi_other .labcontent").click(function () {
            $(this).toggleClass("labhover");
        });
        $("#modify .labcontent").click(function () {
            $(this).toggleClass("labhover");
            if ($(this).attr("id") != "audit" && $(this).css("color") == "rgb(0, 0, 255)") {
                $("#mod_delete").removeClass("labhover");
                $("#mod_update").removeClass("labhover");
                $("#mod_finish").removeClass("labhover");
                $(this).addClass("labhover");
            }
        });

        //初始化时间控件
        var before = new Date();
        before.setDate(before.getDate() - 3);
        var beforeday = before.Format("yyyy-MM-dd");
        //拼装完整日期格式  
        $("#subdatestart").val(beforeday);
        $("#passdatestart").val(beforeday);

        var now = new Date();
        var today = now.Format("yyyy-MM-dd");
        //完成赋值  
        $("#subdateend").val(today);
        $("#passdateend").val(today);

    });
}
//现场报检高级查询
function initSerach_SiteInspection() {
    $(document).on('click', '.open-tabs-modal', function () {
        $.modal({
            text: '<div class="content">' +
                    '<div class="content-padded grid-demo">' +
                        '<div id="busitype">' +
                            '<div id="busi_in" class="row">' +
                                '<div class="col-25 labtitle">进口业务</div>' +
                                '<div class="col-25 labcontent">空运进口</div>' +
                                '<div class="col-25 labcontent">海运进口</div>' +
                                '<div class="col-25 labcontent">陆运进口</div>' +
                            '</div>' +
                            '<div id="busi_out" class="row">' +
                                '<div class="col-25 labtitle">出口业务</div>' +
                                '<div class="col-25 labcontent">空运出口</div>' +
                                '<div class="col-25 labcontent">海运出口</div>' +
                                '<div class="col-25 labcontent">陆运出口</div>' +
                            '</div>' +
                            '<div id="busi_other" class="row">' +
                                '<div class="col-25 labcontent">国内进口</div>' +
                                '<div class="col-25 labcontent">国内出口</div>' +
                                '<div class="col-25 labcontent">特殊进口</div>' +
                                '<div class="col-25 labcontent">特殊出口</div>' +
                            '</div>' +
                        '</div>' +
                        '<div id="otherflag" class="row">' +
                            '<div id="flag_law" class="col-25 labcontent">法检业务</div>' +
                            '<div id="flag_clearance" class="col-25 labcontent">需通关单</div>' +
                            '<div id="mod_fumigation" class="col-25 labcontent">熏蒸业务</div>' +
                        '</div>' +
                        '<div id="modify" class="row">' +
                            '<div id="mod_delete" class="col-25 labcontent">删单</div>' +
                            '<div id="mod_update" class="col-25 labcontent">改单</div>' +
                            '<div id="mod_finish" class="col-25 labcontent">改单完成</div>' +
                        '</div>' +
                        '<div class="row">' +
                            '<div class="col-50" ><input type="text" id="busiunit" placeholder="经营单位"/></div>' +
                            '<div class="col-50" style="margin-left:0rem;"><input type="text" id="contractno" placeholder="合同号"/></div>' +
                        '</div>' +
                        '<div class="row">' +
                            '<div class="col-50"><input type="text" id="ordercode" placeholder="订单编号"/></div>' +
                            '<div class="col-50" style="margin-left:0rem;"><input type="text" id="cusno" placeholder="企业编号"/></div>' +
                        '</div>' +
                        '<div class="row">' +
                            '<div class="col-50"><input type="text" id="divideno" placeholder="分单号"/></div>' +
                            '<div class="col-50" style="margin-left:0rem;"><input type="text" id="customarea" placeholder="申报关区"/></div>' +
                        '</div>' +
                        '<div class="row">' +
                            '<div class="col-20">委托日期</div>' +
                            '<div class="col-40"><input type="date" id="subdatestart" placeholder="起始" /></div>' +
                            '<div class="col-40" style="margin-left:0rem;"><input type="date" id="subdateend" placeholder="结束"/></div>' +
                        '</div>' +
                        '<div class="row">' +
                            '<div class="col-20">放行日期</div>' +
                            '<div class="col-40"><input type="date" id="passdatestart" placeholder="起始"/></div>' +
                            '<div class="col-40" style="margin-left:0rem;"><input type="date" id="passdateend" placeholder="结束"/></div>' +
                        '</div>' +
                        '<div style="margin-top:0.3rem" class="row">' +
                            '<div class="col-20" ><a href="#" class="button">取消</a></div>' +
                            '<div class="col-60" style="width:59.99%; margin-left:0rem;"><a href="#" style="background-color: #3d4145;" class="button">确&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;认</a></div>' +
                            '<div class="col-20" style="margin-left:0rem;"><a href="#" class="button">重置</a></div>' +
                        '</div>' +
                    '</div>' +
                '</div>',
            extraClass: 'morediv'//避免直接设置.modal的样式，从而影响其他toast的提示
        });

        //绑定labtitle事件
        $("#busi_in .labtitle").click(function () {
            $(this).toggleClass("labhover");
            if ($(this).css("color") == "rgb(0, 0, 255)") {
                $("#busi_in .labcontent").each(function () {
                    $(this).addClass("labhover");
                })
            }
            else {
                $("#busi_in .labcontent").each(function () {
                    $(this).removeClass("labhover");
                })
            }
        });
        $("#busi_out .labtitle").click(function () {
            $(this).toggleClass("labhover");
            if ($(this).css("color") == "rgb(0, 0, 255)") {
                $("#busi_out .labcontent").each(function () {
                    $(this).addClass("labhover");
                })
            }
            else {
                $("#busi_out .labcontent").each(function () {
                    $(this).removeClass("labhover");
                })
            }
        });
        //绑定labcontent事件
        $("#busi_in .labcontent").click(function () {
            $(this).toggleClass("labhover");
            var flag = true;
            $("#busi_in .labcontent").each(function () {
                if ($(this).css("color") != "rgb(0, 0, 255)") {
                    flag = false;
                    return;
                }
            })
            if (flag) {
                $("#busi_in .labtitle").addClass("labhover");
            }
            else {
                $("#busi_in .labtitle").removeClass("labhover");
            }
        });
        $("#busi_out .labcontent").click(function () {
            $(this).toggleClass("labhover");
            var flag = true;
            $("#busi_out .labcontent").each(function () {
                if ($(this).css("color") != "rgb(0, 0, 255)") {
                    flag = false;
                    return;
                }
            })
            if (flag) {
                $("#busi_out .labtitle").addClass("labhover");
            }
            else {
                $("#busi_out .labtitle").removeClass("labhover");
            }
        });
        $("#busi_other .labcontent").click(function () {
            $(this).toggleClass("labhover");
        });
        $("#modify .labcontent").click(function () {
            $(this).toggleClass("labhover");
            if ($(this).attr("id") != "audit" && $(this).css("color") == "rgb(0, 0, 255)") {
                $("#mod_delete").removeClass("labhover");
                $("#mod_update").removeClass("labhover");
                $("#mod_finish").removeClass("labhover");
                $(this).addClass("labhover");
            }
        }); 
        $("#otherflag .labcontent").click(function () {
            $(this).toggleClass("labhover");
        });

        //初始化时间控件
        var before = new Date();
        before.setDate(before.getDate() - 3);
        var beforeday = before.Format("yyyy-MM-dd");
        //拼装完整日期格式  
        $("#subdatestart").val(beforeday);
        $("#passdatestart").val(beforeday);

        var now = new Date();
        var today = now.Format("yyyy-MM-dd");
        //完成赋值  
        $("#subdateend").val(today);
        $("#passdateend").val(today);

    });
}

//报关高级查询
function initSerach_Declare() {
    $(document).on('click', '.open-tabs-modal', function () {
        $.modal({
            text: '<div class="content">' +
                    '<div class="content-padded grid-demo">' +
                        '<div id="busitype">' +
                            '<div id="busi_in" class="row">' +
                                '<div class="col-25 labtitle">进口业务</div>' +
                                '<div class="col-25 labcontent">空运进口</div>' +
                                '<div class="col-25 labcontent">海运进口</div>' +
                                '<div class="col-25 labcontent">陆运进口</div>' +
                            '</div>' +
                            '<div id="busi_out" class="row">' +
                                '<div class="col-25 labtitle">出口业务</div>' +
                                '<div class="col-25 labcontent">空运出口</div>' +
                                '<div class="col-25 labcontent">海运出口</div>' +
                                '<div class="col-25 labcontent">陆运出口</div>' +
                            '</div>' +
                            '<div id="busi_other" class="row">' +
                                '<div class="col-25 labcontent">国内进口</div>' +
                                '<div class="col-25 labcontent">国内出口</div>' +
                                '<div class="col-25 labcontent">特殊进口</div>' +
                                '<div class="col-25 labcontent">特殊出口</div>' +
                            '</div>' +
                        '</div>' +
                        '<div id="otherflag" class="row">' +
                            '<div id="flag_checked" class="col-25 labcontent">查验</div>' +
                            '<div id="flag_uncheck" class="col-25 labcontent">未查验</div>' +
                            '<div id="flag_passed" class="col-25 labcontent">放行</div>' +
                            '<div id="flag_unpass" class="col-25 labcontent">未放行</div>' +
                        '</div>' +
                        '<div class="row">' +
                            '<div class="col-100" style="width:92%"><input type="text" id="busiunit" placeholder="经营单位"/></div>' +
                        '</div>' +
                        '<div class="row">' +
                            '<div class="col-50"><input type="text" id="ordercode" placeholder="订单编号"/></div>' +
                            '<div class="col-50" style="margin-left:0rem;"><input type="text" id="cusno" placeholder="企业编号"/></div>' +
                        '</div>' +
                        '<div class="row">' +
                            '<div class="col-50"><input type="text" id="tradeway" placeholder="监管方式"/></div>' +
                            '<div class="col-50" style="margin-left:0rem;"><input type="text" id="contractno" placeholder="合同号"/></div>' +
                        '</div>' +
                        '<div class="row">' +
                            '<div class="col-100" style="width:92%"><input type="text" id="blno" placeholder="提运单号"/></div>' +
                        '</div>' +
                        '<div class="row">' +
                            '<div class="col-20">委托日期</div>' +
                            '<div class="col-40"><input type="date" id="subdatestart" placeholder="起始" /></div>' +
                            '<div class="col-40" style="margin-left:0rem;"><input type="date" id="subdateend" placeholder="结束"/></div>' +
                        '</div>' +
                        '<div class="row">' +
                            '<div class="col-20">放行日期</div>' +
                            '<div class="col-40"><input type="date" id="passdatestart" placeholder="起始"/></div>' +
                            '<div class="col-40" style="margin-left:0rem;"><input type="date" id="passdateend" placeholder="结束"/></div>' +
                        '</div>' +
                        '<div style="margin-top:0.3rem" class="row">' +
                            '<div class="col-20" ><a href="#" class="button">取消</a></div>' +
                            '<div class="col-60" style="width:59.99%; margin-left:0rem;"><a href="#" style="background-color: #3d4145;" class="button">确&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;认</a></div>' +
                            '<div class="col-20" style="margin-left:0rem;"><a href="#" class="button">重置</a></div>' +
                        '</div>' +
                    '</div>' +
                '</div>',
            extraClass: 'morediv'//避免直接设置.modal的样式，从而影响其他toast的提示
        });

        //绑定labtitle事件
        $("#busi_in .labtitle").click(function () {
            $(this).toggleClass("labhover");
            if ($(this).css("color") == "rgb(0, 0, 255)") {
                $("#busi_in .labcontent").each(function () {
                    $(this).addClass("labhover");
                })
            }
            else {
                $("#busi_in .labcontent").each(function () {
                    $(this).removeClass("labhover");
                })
            }
        });
        $("#busi_out .labtitle").click(function () {
            $(this).toggleClass("labhover");
            if ($(this).css("color") == "rgb(0, 0, 255)") {
                $("#busi_out .labcontent").each(function () {
                    $(this).addClass("labhover");
                })
            }
            else {
                $("#busi_out .labcontent").each(function () {
                    $(this).removeClass("labhover");
                })
            }
        });
        //绑定labcontent事件
        $("#busi_in .labcontent").click(function () {
            $(this).toggleClass("labhover");
            var flag = true;
            $("#busi_in .labcontent").each(function () {
                if ($(this).css("color") != "rgb(0, 0, 255)") {
                    flag = false;
                    return;
                }
            })
            if (flag) {
                $("#busi_in .labtitle").addClass("labhover");
            }
            else {
                $("#busi_in .labtitle").removeClass("labhover");
            }
        });
        $("#busi_out .labcontent").click(function () {
            $(this).toggleClass("labhover");
            var flag = true;
            $("#busi_out .labcontent").each(function () {
                if ($(this).css("color") != "rgb(0, 0, 255)") {
                    flag = false;
                    return;
                }
            })
            if (flag) {
                $("#busi_out .labtitle").addClass("labhover");
            }
            else {
                $("#busi_out .labtitle").removeClass("labhover");
            }
        });
        $("#busi_other .labcontent").click(function () {
            $(this).toggleClass("labhover");
        });
        $("#otherflag .labcontent").click(function () {
            $(this).toggleClass("labhover");
            if ($(this).attr("id") == "flag_checked" && $(this).css("color") == "rgb(0, 0, 255)") {
                $("#flag_uncheck").removeClass("labhover");
            }
            if ($(this).attr("id") == "flag_uncheck" && $(this).css("color") == "rgb(0, 0, 255)") {
                $("#flag_checked").removeClass("labhover");
            }
            if ($(this).attr("id") == "flag_passed" && $(this).css("color") == "rgb(0, 0, 255)") {
                $("#flag_unpass").removeClass("labhover");
            }
            if ($(this).attr("id") == "flag_unpass" && $(this).css("color") == "rgb(0, 0, 255)") {
                $("#flag_passed").removeClass("labhover");
            }
        });

        //初始化时间控件
        var before = new Date();
        before.setDate(before.getDate() - 3);
        var beforeday = before.Format("yyyy-MM-dd");
        //拼装完整日期格式  
        $("#subdatestart").val(beforeday);
        $("#passdatestart").val(beforeday);

        var now = new Date();
        var today = now.Format("yyyy-MM-dd");
        //完成赋值  
        $("#subdateend").val(today);
        $("#passdateend").val(today);

    });
}

//报检高级查询
function initSerach_inspection() {
    $(document).on('click', '.open-tabs-modal', function () {
        $.modal({
            text: '<div class="content">' +
                    '<div class="content-padded grid-demo">' +
                        '<div id="busitype">' +
                            '<div id="busi_in" class="row">' +
                                '<div class="col-25 labtitle">进口业务</div>' +
                                '<div class="col-25 labcontent">空运进口</div>' +
                                '<div class="col-25 labcontent">海运进口</div>' +
                                '<div class="col-25 labcontent">陆运进口</div>' +
                            '</div>' +
                            '<div id="busi_out" class="row">' +
                                '<div class="col-25 labtitle">出口业务</div>' +
                                '<div class="col-25 labcontent">空运出口</div>' +
                                '<div class="col-25 labcontent">海运出口</div>' +
                                '<div class="col-25 labcontent">陆运出口</div>' +
                            '</div>' +
                            '<div id="busi_other" class="row">' +
                                '<div class="col-25 labcontent">国内进口</div>' +
                                '<div class="col-25 labcontent">国内出口</div>' +
                                '<div class="col-25 labcontent">特殊进口</div>' +
                                '<div class="col-25 labcontent">特殊出口</div>' +
                            '</div>' +
                        '</div>' +
                        '<div id="otherflag" class="row">' +
                            '<div id="flag_checked" class="col-25 labcontent">查验</div>' +
                            '<div id="flag_uncheck" class="col-25 labcontent">未查验</div>' +
                            '<div id="flag_passed" class="col-25 labcontent">放行</div>' +
                            '<div id="flag_unpass" class="col-25 labcontent">未放行</div>' +
                        '</div>' +
                        '<div id="otherflag2" class="row">' +
                            '<div id="flag_law" class="col-25 labcontent">法检业务</div>' +
                            '<div id="flag_clearance" class="col-25 labcontent">需通关单</div>' +
                        '</div>' +
                        '<div class="row">' +
                            '<div class="col-50"><input type="text" id="busiunit" placeholder="经营单位"/></div>' +
                            '<div class="col-50" style="margin-left:0rem;"><input type="text" id="contractno" placeholder="合同号"/></div>' +
                        '</div>' +
                        '<div class="row">' +
                            '<div class="col-50"><input type="text" id="ordercode" placeholder="订单编号"/></div>' +
                            '<div class="col-50" style="margin-left:0rem;"><input type="text" id="cusno" placeholder="企业编号"/></div>' +
                        '</div>' +
                        '<div class="row">' +
                            '<div class="col-50"><input type="text" id="divideno" placeholder="分单号"/></div>' +
                            '<div class="col-50" style="margin-left:0rem;"><input type="text" id="customarea" placeholder="申报关区"/></div>' +
                        '</div>' +
                        '<div class="row">' +
                            '<div class="col-20">委托日期</div>' +
                            '<div class="col-40"><input type="date" id="subdatestart" placeholder="起始" /></div>' +
                            '<div class="col-40" style="margin-left:0rem;"><input type="date" id="subdateend" placeholder="结束"/></div>' +
                        '</div>' +
                        '<div class="row">' +
                            '<div class="col-20">放行日期</div>' +
                            '<div class="col-40"><input type="date" id="passdatestart" placeholder="起始"/></div>' +
                            '<div class="col-40" style="margin-left:0rem;"><input type="date" id="passdateend" placeholder="结束"/></div>' +
                        '</div>' +
                        '<div style="margin-top:0.3rem" class="row">' +
                            '<div class="col-20" ><a href="#" class="button">取消</a></div>' +
                            '<div class="col-60" style="width:59.99%; margin-left:0rem;"><a href="#" style="background-color: #3d4145;" class="button">确&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;认</a></div>' +
                            '<div class="col-20" style="margin-left:0rem;"><a href="#" class="button">重置</a></div>' +
                        '</div>' +
                    '</div>' +
                '</div>',
            extraClass: 'morediv'//避免直接设置.modal的样式，从而影响其他toast的提示
        });

        //绑定labtitle事件
        $("#busi_in .labtitle").click(function () {
            $(this).toggleClass("labhover");
            if ($(this).css("color") == "rgb(0, 0, 255)") {
                $("#busi_in .labcontent").each(function () {
                    $(this).addClass("labhover");
                })
            }
            else {
                $("#busi_in .labcontent").each(function () {
                    $(this).removeClass("labhover");
                })
            }
        });
        $("#busi_out .labtitle").click(function () {
            $(this).toggleClass("labhover");
            if ($(this).css("color") == "rgb(0, 0, 255)") {
                $("#busi_out .labcontent").each(function () {
                    $(this).addClass("labhover");
                })
            }
            else {
                $("#busi_out .labcontent").each(function () {
                    $(this).removeClass("labhover");
                })
            }
        });
        //绑定labcontent事件
        $("#busi_in .labcontent").click(function () {
            $(this).toggleClass("labhover");
            var flag = true;
            $("#busi_in .labcontent").each(function () {
                if ($(this).css("color") != "rgb(0, 0, 255)") {
                    flag = false;
                    return;
                }
            })
            if (flag) {
                $("#busi_in .labtitle").addClass("labhover");
            }
            else {
                $("#busi_in .labtitle").removeClass("labhover");
            }
        });
        $("#busi_out .labcontent").click(function () {
            $(this).toggleClass("labhover");
            var flag = true;
            $("#busi_out .labcontent").each(function () {
                if ($(this).css("color") != "rgb(0, 0, 255)") {
                    flag = false;
                    return;
                }
            })
            if (flag) {
                $("#busi_out .labtitle").addClass("labhover");
            }
            else {
                $("#busi_out .labtitle").removeClass("labhover");
            }
        });
        $("#busi_other .labcontent").click(function () {
            $(this).toggleClass("labhover");
        });
        $("#otherflag2 .labcontent").click(function () {
            $(this).toggleClass("labhover");
        });
        $("#otherflag .labcontent").click(function () {
            $(this).toggleClass("labhover");
            if ($(this).attr("id") == "flag_checked" && $(this).css("color") == "rgb(0, 0, 255)") {
                $("#flag_uncheck").removeClass("labhover");
            }
            if ($(this).attr("id") == "flag_uncheck" && $(this).css("color") == "rgb(0, 0, 255)") {
                $("#flag_checked").removeClass("labhover");
            }
            if ($(this).attr("id") == "flag_passed" && $(this).css("color") == "rgb(0, 0, 255)") {
                $("#flag_unpass").removeClass("labhover");
            }
            if ($(this).attr("id") == "flag_unpass" && $(this).css("color") == "rgb(0, 0, 255)") {
                $("#flag_passed").removeClass("labhover");
            }
        });

        //初始化时间控件
        var before = new Date();
        before.setDate(before.getDate() - 3);
        var beforeday = before.Format("yyyy-MM-dd");
        //拼装完整日期格式  
        $("#subdatestart").val(beforeday);
        $("#passdatestart").val(beforeday);

        var now = new Date();
        var today = now.Format("yyyy-MM-dd");
        //完成赋值  
        $("#subdateend").val(today);
        $("#passdateend").val(today);

    });
}
//日期格式
Date.prototype.Format = function (fmt) {
    var o = {
        "M+": this.getMonth() + 1, //月份   
        "d+": this.getDate(), //日   
        "h+": this.getHours(), //小时   
        "m+": this.getMinutes(), //分   
        "s+": this.getSeconds(), //秒   
        "q+": Math.floor((this.getMonth() + 3) / 3), //季度   
        "S": this.getMilliseconds() //毫秒   
    };
    if (/(y+)/.test(fmt)) fmt = fmt.replace(RegExp.$1, (this.getFullYear() + "").substr(4 - RegExp.$1.length));
    for (var k in o)
        if (new RegExp("(" + k + ")").test(fmt)) fmt = fmt.replace(RegExp.$1, (RegExp.$1.length == 1) ? (o[k]) : (("00" + o[k]).substr(("" + o[k]).length)));
    return fmt;
}