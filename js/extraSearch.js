//现场报关高级查询
function initSerach_SiteDeclare() {
    $(document).on('click', '.open-tabs-modal', function () {
        $.modal({
            text: 
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
                            '<div class="col-25">委托日期:</div>' +
                            '<div class="col-33" style="width:35%;margin-left:0rem;"><input type="text" id="subdatestart" placeholder="起始" /></div>' +
                            '<div class="col-33" style="width:36%;margin-left:0rem;"><input type="text" id="subdateend" placeholder="结束"/></div>' +
                        '</div>' +
                        '<div class="row">' +
                            '<div class="col-25">放行日期:</div>' +
                            '<div class="col-33" style="width:35%;margin-left:0rem;"><input type="text" id="passdatestart" placeholder="起始"/></div>' +
                            '<div class="col-33" style="width:36%;margin-left:0rem;"><input type="text" id="passdateend" placeholder="结束"/></div>' +
                        '</div>' +
                        '<div style="margin-top:.5rem;" class="row">' +
                            '<div class="col-20" ><a href="#" id="btn_more_cancel" class="button">取消</a></div>' +
                            '<div class="col-60" style="width:59.99%; margin-left:0rem;"><a href="#" id="btn_more_sure" style="background-color: #3d4145;" class="button">确&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;认</a></div>' +
                            '<div class="col-20" style="margin-left:0rem;"><a href="#" id="btn_more_reset" class="button">重置</a></div>' +
                        '</div>' +
                    '</div>',
            extraClass: 'moredivsitedecl'//避免直接设置.modal的样式，从而影响其他toast的提示
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

        //----------------------------------------------------------------------------------------------------------------------赋值
        if ($("#txt_busitype").val() != "") {
            $("#busitype .labcontent").each(function () {
                if ($("#txt_busitype").val().replace(getbusitypeid($(this).html()), "") != $("#txt_busitype").val()) {
                    $(this).addClass("labhover");
                }
            });

            var inall = true;
            $("#busi_in .labcontent").each(function () {
                if ($(this).css("color") != "rgb(0, 0, 255)") { inall = false; }
            });
            if (inall) { $("#busi_in .labtitle").addClass("labhover"); }

            var outall = true;
            $("#busi_out .labcontent").each(function () {
                if ($(this).css("color") != "rgb(0, 0, 255)") { outall = false; }
            });
            if (outall) { $("#busi_out .labtitle").addClass("labhover"); }
        }

        if ($("#txt_modifyflag").val() == "删单") { $("#mod_delete").addClass("labhover"); }
        if ($("#txt_modifyflag").val() == "改单") { $("#mod_update").addClass("labhover"); }
        if ($("#txt_modifyflag").val() == "改单完成") { $("#mod_finish").addClass("labhover"); }

        if ($("#txt_auditflag").val() == "稽核") { $("#audit").addClass("labhover"); }

        $("#busiunit").val($("#txt_busiunit").val());
        $("#ordercode").val($("#txt_ordercode").val());
        $("#cusno").val($("#txt_cusno").val());
        $("#divideno").val($("#txt_divideno").val());
        $("#contractno").val($("#txt_contractno").val());

        ////初始化时间控件
        //var before = new Date();
        //before.setDate(before.getDate() - 3);
        //var beforeday = before.Format("yyyy-MM-dd");

        //var now = new Date();
        //var today = now.Format("yyyy-MM-dd");

        if ($("#txt_submittime_s").val() != "" || $("#txt_submittime_e").val() != "") {

            if ($("#txt_submittime_s").val() != "") {
                $("#subdatestart").val($("#txt_submittime_s").val());
                $("#subdatestart").calendar({ value: [$("#txt_submittime_s").val()] });
            }
            if ($("#txt_submittime_e").val() != "") {
                $("#subdateend").val($("#txt_submittime_e").val());
                $("#subdateend").calendar({ value: [$("#txt_submittime_e").val()] });
            }

        } else {
            //$("#subdatestart").val(beforeday);
            //$("#subdatestart").calendar({ value: [beforeday] });

            //$("#subdateend").val(today);
            //$("#subdateend").calendar({ value: [today] });

            $("#subdatestart").val("");
            $("#subdatestart").calendar({ });

            $("#subdateend").val("");
            $("#subdateend").calendar({ });
        }

        if ($("#txt_sitepasstime_s").val() != "" || $("#txt_sitepasstime_e").val() != "") {

            if ($("#txt_sitepasstime_s").val() != "") {
                $("#passdatestart").val($("#txt_sitepasstime_s").val());
                $("#passdatestart").calendar({ value: [$("#txt_sitepasstime_s").val()] });
            }
            if ($("#txt_sitepasstime_e").val() != "") {
                $("#passdateend").val($("#txt_sitepasstime_e").val());
                $("#passdateend").calendar({ value: [$("#txt_sitepasstime_e").val()] });
            }

        } else {
            //$("#passdatestart").val(beforeday);
            //$("#passdatestart").calendar({ value: [beforeday] });

            //$("#passdateend").val(today);
            //$("#passdateend").calendar({ value: [today] });

            $("#passdatestart").val("");
            $("#passdatestart").calendar({ });

            $("#passdateend").val("");
            $("#passdateend").calendar({ });

        }

        //----------------------------------------------------------------------------------------------------------------------按钮事件

        $("#btn_more_cancel").click(function () {
            $.closeModal(".moredivsitedecl");
        });

        $("#btn_more_sure").click(function () {
            var busitypeid = "";
            $("#busitype .labcontent").each(function () {
                if ($(this).css("color") == "rgb(0, 0, 255)") {
                    busitypeid = busitypeid + "'" + getbusitypeid($(this).html()) + "',";
                }
            });
            if (busitypeid != "") {
                busitypeid = busitypeid.substr(0, busitypeid.length - 1);
            }

            var modifyflag = ""; var auditflag = "";
            $("#modify .labcontent").each(function () {
                if ($(this).attr("id") == "mod_delete" && $(this).css("color") == "rgb(0, 0, 255)") {
                    modifyflag = $(this).html();
                }
                if ($(this).attr("id") == "mod_update" && $(this).css("color") == "rgb(0, 0, 255)") {
                    modifyflag = $(this).html();
                }
                if ($(this).attr("id") == "mod_finish" && $(this).css("color") == "rgb(0, 0, 255)") {
                    modifyflag = $(this).html();
                }
                if ($(this).attr("id") == "audit" && $(this).css("color") == "rgb(0, 0, 255)") {
                    auditflag = $(this).html();
                }
            });

            $("#txt_busitype").val(busitypeid);
            $("#txt_modifyflag").val(modifyflag);
            $("#txt_auditflag").val(auditflag);

            $("#txt_busiunit").val($("#busiunit").val());
            $("#txt_ordercode").val($("#ordercode").val());
            $("#txt_cusno").val($("#cusno").val());
            $("#txt_divideno").val($("#divideno").val());
            $("#txt_contractno").val($("#contractno").val());
            $("#txt_submittime_s").val($("#subdatestart").val());
            $("#txt_submittime_e").val($("#subdateend").val());
            $("#txt_sitepasstime_s").val($("#passdatestart").val());
            $("#txt_sitepasstime_e").val($("#passdateend").val());

            $.closeModal(".moredivsitedecl");
        });

        $("#btn_more_reset").click(function () {
            $("#txt_siteapplytime_s").val(""); $("#txt_siteapplytime_e").val("");
            $("#txt_siteapplytime_s").calendar({}); $("#txt_siteapplytime_e").calendar({});//否则之前选的那天  不能再次选中

            $("#txt_declcode").val(""); $("#txt_customareacode").val("");
            $("#picker_is_pass").picker("setValue", ["全部"]); $("#picker_ischeck").picker("setValue", ["全部"]);

            $("#txt_busitype").val("");
            $("#txt_modifyflag").val("");
            $("#txt_auditflag").val("");

            $(".labcontent").each(function () { $(this).removeClass("labhover"); });
            $(".labtitle").each(function () { $(this).removeClass("labhover"); });

            $("#txt_busiunit").val(""); $("#busiunit").val("");
            $("#txt_ordercode").val(""); $("#ordercode").val("");
            $("#txt_cusno").val(""); $("#cusno").val("");
            $("#txt_divideno").val(""); $("#divideno").val("");
            $("#txt_contractno").val(""); $("#contractno").val("");
            $("#txt_submittime_s").val(""); $("#subdatestart").val(""); $("#subdatestart").calendar({});
            $("#txt_submittime_e").val(""); $("#subdateend").val(""); $("#subdateend").calendar({});
            $("#txt_sitepasstime_s").val(""); $("#passdatestart").val(""); $("#passdatestart").calendar({});
            $("#txt_sitepasstime_e").val(""); $("#passdateend").val(""); $("#passdateend").calendar({});

            $.closeModal(".moredivsitedecl");
        });
    });
}

//现场报检高级查询
function initSerach_SiteInspection() {
    $(document).on('click', '.open-tabs-modal', function () {
        $.modal({
            text: '<div class="content-padded grid-demo">' +
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
                            '<div class="col-100" style="width:92%;"><input type="text" id="approvalcode" placeholder="流水号"/></div>' +
                        '</div>' +
                        '<div class="row">' +
                            '<div class="col-25">委托日期:</div>' +
                            '<div class="col-33" style="width:35%;margin-left:0rem;"><input type="text" id="subdatestart" placeholder="起始" /></div>' +
                            '<div class="col-33" style="width:36%;margin-left:0rem;"><input type="text" id="subdateend" placeholder="结束"/></div>' +
                        '</div>' +
                        '<div class="row">' +
                            '<div class="col-25">放行日期:</div>' +
                            '<div class="col-33" style="width:35%;margin-left:0rem;"><input type="text" id="passdatestart" placeholder="起始"/></div>' +
                            '<div class="col-33" style="width:36%;margin-left:0rem;"><input type="text" id="passdateend" placeholder="结束"/></div>' +
                        '</div>' +
                        '<div style="margin-top:.3rem;" class="row">' +
                            '<div class="col-20" ><a href="#" id="btn_more_cancel" class="button">取消</a></div>' +
                            '<div class="col-60" style="width:59.99%; margin-left:0rem;"><a href="#" id="btn_more_sure" style="background-color: #3d4145;" class="button">确&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;认</a></div>' +
                            '<div class="col-20" style="margin-left:0rem;"><a href="#" id="btn_more_reset" class="button">重置</a></div>' +
                        '</div>' +
                    '</div>' ,
            extraClass: 'moredivsiteinsp'//避免直接设置.modal的样式，从而影响其他toast的提示
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

        //----------------------------------------------------------------------------------------------------------------------赋值
        if ($("#txt_busitype").val() != "") {
            $("#busitype .labcontent").each(function () {
                if ($("#txt_busitype").val().replace(getbusitypeid($(this).html()), "") != $("#txt_busitype").val()) {
                    $(this).addClass("labhover");
                }
            });

            var inall = true;
            $("#busi_in .labcontent").each(function () {
                if ($(this).css("color") != "rgb(0, 0, 255)") { inall = false; }
            });
            if (inall) { $("#busi_in .labtitle").addClass("labhover"); }

            var outall = true;
            $("#busi_out .labcontent").each(function () {
                if ($(this).css("color") != "rgb(0, 0, 255)") { outall = false; }
            });
            if (outall) { $("#busi_out .labtitle").addClass("labhover"); }
        }

        if ($("#txt_lawflag").val() == "法检") { $("#flag_law").addClass("labhover"); }
        if ($("#txt_isneedclearance").val() == "需通关单") { $("#flag_clearance").addClass("labhover"); }
        if ($("#txt_isfumigation").val() == "熏蒸") { $("#mod_fumigation").addClass("labhover"); }

        if ($("#txt_modifyflag").val() == "删单") { $("#mod_delete").addClass("labhover"); }
        if ($("#txt_modifyflag").val() == "改单") { $("#mod_update").addClass("labhover"); }
        if ($("#txt_modifyflag").val() == "改单完成") { $("#mod_finish").addClass("labhover"); }        

        $("#busiunit").val($("#txt_busiunit").val());
        $("#contractno").val($("#txt_contractno").val());
        $("#ordercode").val($("#txt_ordercode").val());
        $("#cusno").val($("#txt_cusno").val());
        $("#divideno").val($("#txt_divideno").val());
        $("#customarea").val($("#txt_customareacode").val());
        $("#approvalcode").val($("#txt_approvalcode").val());

        ////初始化时间控件
        //var before = new Date();
        //before.setDate(before.getDate() - 3);
        //var beforeday = before.Format("yyyy-MM-dd");

        //var now = new Date();
        //var today = now.Format("yyyy-MM-dd");

        if ($("#txt_submittime_s").val() != "" || $("#txt_submittime_e").val() != "") {

            if ($("#txt_submittime_s").val() != "") {
                $("#subdatestart").val($("#txt_submittime_s").val());
                $("#subdatestart").calendar({ value: [$("#txt_submittime_s").val()] });
            }
            if ($("#txt_submittime_e").val() != "") {
                $("#subdateend").val($("#txt_submittime_e").val());
                $("#subdateend").calendar({ value: [$("#txt_submittime_e").val()] });
            }

        } else {
            //$("#subdatestart").val(beforeday);
            //$("#subdatestart").calendar({ value: [beforeday] });

            //$("#subdateend").val(today);
            //$("#subdateend").calendar({ value: [today] });

            $("#subdatestart").val("");
            $("#subdatestart").calendar({ });

            $("#subdateend").val("");
            $("#subdateend").calendar({ });
        }

        if ($("#txt_sitepasstime_s").val() != "" || $("#txt_sitepasstime_e").val() != "") {

            if ($("#txt_sitepasstime_s").val() != "") {
                $("#passdatestart").val($("#txt_sitepasstime_s").val());
                $("#passdatestart").calendar({ value: [$("#txt_sitepasstime_s").val()] });
            }
            if ($("#txt_sitepasstime_e").val() != "") {
                $("#passdateend").val($("#txt_sitepasstime_e").val());
                $("#passdateend").calendar({ value: [$("#txt_sitepasstime_e").val()] });
            }

        } else {
            //$("#passdatestart").val(beforeday);
            //$("#passdatestart").calendar({ value: [beforeday] });

            //$("#passdateend").val(today);
            //$("#passdateend").calendar({ value: [today] });

            $("#passdatestart").val("");
            $("#passdatestart").calendar({ value: [""] });

            $("#passdateend").val("");
            $("#passdateend").calendar({ value: [""] });

        }

        //----------------------------------------------------------------------------------------------------------------------按钮事件

        $("#btn_more_cancel").click(function () {
            $.closeModal(".moredivsiteinsp");
        });

        $("#btn_more_sure").click(function () {
            var busitypeid = "";
            $("#busitype .labcontent").each(function () {
                if ($(this).css("color") == "rgb(0, 0, 255)") {
                    busitypeid = busitypeid + "'" + getbusitypeid($(this).html()) + "',";
                }
            });
            if (busitypeid != "") {
                busitypeid = busitypeid.substr(0, busitypeid.length - 1);
            }

            var lawflag = ""; var isneedclearance = ""; var isfumigation = "";
            $("#otherflag .labcontent").each(function () {
                if ($(this).attr("id") == "flag_law" && $(this).css("color") == "rgb(0, 0, 255)") {
                    lawflag = $(this).html();
                }
                if ($(this).attr("id") == "flag_clearance" && $(this).css("color") == "rgb(0, 0, 255)") {
                    isneedclearance = $(this).html();
                }
                if ($(this).attr("id") == "mod_fumigation" && $(this).css("color") == "rgb(0, 0, 255)") {
                    isfumigation = $(this).html();
                }
            });

            var modifyflag = "";
            $("#modify .labcontent").each(function () {
                if ($(this).attr("id") == "mod_delete" && $(this).css("color") == "rgb(0, 0, 255)") {
                    modifyflag = $(this).html();
                }
                if ($(this).attr("id") == "mod_update" && $(this).css("color") == "rgb(0, 0, 255)") {
                    modifyflag = $(this).html();
                }
                if ($(this).attr("id") == "mod_finish" && $(this).css("color") == "rgb(0, 0, 255)") {
                    modifyflag = $(this).html();
                }
            });

            $("#txt_busitype").val(busitypeid); $("#txt_lawflag").val(lawflag); $("#txt_isneedclearance").val(isneedclearance); $("#txt_isfumigation").val(isfumigation);
            $("#txt_modifyflag").val(modifyflag);            

            $("#txt_busiunit").val($("#busiunit").val());
            $("#txt_contractno").val($("#contractno").val());
            $("#txt_ordercode").val($("#ordercode").val());
            $("#txt_cusno").val($("#cusno").val());
            $("#txt_divideno").val($("#divideno").val());
            $("#txt_customareacode").val($("#customarea").val());
            $("#txt_approvalcode").val($("#approvalcode").val());

            $("#txt_submittime_s").val($("#subdatestart").val());
            $("#txt_submittime_e").val($("#subdateend").val());
            $("#txt_sitepasstime_s").val($("#passdatestart").val());
            $("#txt_sitepasstime_e").val($("#passdateend").val());

            $.closeModal(".moredivsiteinsp");
        });

        $("#btn_more_reset").click(function () {
            $("#txt_inspsiteapplytime_s").val(""); $("#txt_inspsiteapplytime_e").val("");
            $("#txt_inspsiteapplytime_s").calendar({}); $("#txt_inspsiteapplytime_e").calendar({});//否则之前选的那天  不能再次选中

            $("#txt_inspcode").val(""); 
            $("#picker_is_pass").picker("setValue", ["全部"]); $("#picker_ischeck").picker("setValue", ["全部"]);

            $("#txt_busitype").val(""); $("#txt_lawflag").val(""); $("#txt_isneedclearance").val(""); $("#txt_isfumigation").val("");
            $("#txt_modifyflag").val("");
            

            $(".labcontent").each(function () { $(this).removeClass("labhover"); });
            $(".labtitle").each(function () { $(this).removeClass("labhover"); });

            $("#txt_busiunit").val(""); $("#busiunit").val("");
            $("#txt_contractno").val(""); $("#contractno").val("");
            $("#txt_ordercode").val(""); $("#ordercode").val("");
            $("#txt_cusno").val(""); $("#cusno").val("");
            $("#txt_divideno").val(""); $("#divideno").val("");
            $("#txt_customareacode").val(""); $("#customarea").val("");
            $("#txt_approvalcode").val(); $("#approvalcode").val("");

            $("#txt_submittime_s").val(""); $("#subdatestart").val(""); $("#subdatestart").calendar({});
            $("#txt_submittime_e").val(""); $("#subdateend").val(""); $("#subdateend").calendar({});
            $("#txt_sitepasstime_s").val(""); $("#passdatestart").val(""); $("#passdatestart").calendar({});
            $("#txt_sitepasstime_e").val(""); $("#passdateend").val(""); $("#passdateend").calendar({});

            $.closeModal(".moredivsiteinsp");
        });


    });
}

//报关高级查询
function initSerach_Declare() {
    $(document).on('click', '.open-tabs-modal', function () {
        $.modal({
            text:
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
                            '<div class="col-25">委托日期:</div>' +
                            '<div class="col-33" style="width:35%;margin-left:0rem;"><input type="text" id="subdatestart" placeholder="起始" /></div>' +
                            '<div class="col-33" style="width:36%;margin-left:0rem;"><input type="text" id="subdateend" placeholder="结束"/></div>' +
                        '</div>' +
                        '<div class="row">' +
                            '<div class="col-25">放行日期:</div>' +
                            '<div class="col-33" style="width:35%;margin-left:0rem;"><input type="text" id="passdatestart" placeholder="起始"/></div>' +
                            '<div class="col-33" style="width:36%;margin-left:0rem;"><input type="text" id="passdateend" placeholder="结束"/></div>' +
                        '</div>' +
                        '<div style="margin-top:.3rem;" class="row">' +
                            '<div class="col-20" ><a href="#" id="btn_more_cancel" class="button">取消</a></div>' +
                            '<div class="col-60" style="width:59.99%; margin-left:0rem;"><a href="#" id="btn_more_sure" style="background-color: #3d4145;" class="button">确&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;认</a></div>' +
                            '<div class="col-20" style="margin-left:0rem;"><a href="#" id="btn_more_reset" class="button">重置</a></div>' +
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

        //----------------------------------------------------------------------------------------------------------------------赋值
        if ($("#txt_busitype").val() != "") {
            $("#busitype .labcontent").each(function () {
                if ($("#txt_busitype").val().replace(getbusitypeid($(this).html()), "") != $("#txt_busitype").val()) {
                    $(this).addClass("labhover");
                }
            });

            var inall = true;
            $("#busi_in .labcontent").each(function () {
                if ($(this).css("color") != "rgb(0, 0, 255)") { inall = false; }
            });
            if (inall) { $("#busi_in .labtitle").addClass("labhover"); }

            var outall = true;
            $("#busi_out .labcontent").each(function () {
                if ($(this).css("color") != "rgb(0, 0, 255)") { outall = false; }
            });
            if (outall) { $("#busi_out .labtitle").addClass("labhover"); }
        }

        if ($("#txt_ischeck").val() == "查验") { $("#flag_checked").addClass("labhover"); }
        if ($("#txt_ischeck").val() == "未查验") { $("#flag_uncheck").addClass("labhover"); }

        if ($("#txt_ispass").val() == "放行") { $("#flag_passed").addClass("labhover"); }
        if ($("#txt_ispass").val() == "未放行") { $("#flag_unpass").addClass("labhover"); }

        $("#busiunit").val($("#txt_busiunit").val());
        $("#ordercode").val($("#txt_ordercode").val());
        $("#cusno").val($("#txt_cusno").val());
        $("#tradeway").val($("#txt_tradeway").val());
        $("#contractno").val($("#txt_contractno").val());
        $("#blno").val($("#txt_blno").val());
        
        ////初始化时间控件
        //var before = new Date();
        //before.setDate(before.getDate() - 3);
        //var beforeday = before.Format("yyyy-MM-dd");

        //var now = new Date();
        //var today = now.Format("yyyy-MM-dd");

        if ($("#txt_submittime_s").val() != "" || $("#txt_submittime_e").val() != "") {

            if ($("#txt_submittime_s").val() != "") {
                $("#subdatestart").val($("#txt_submittime_s").val());
                $("#subdatestart").calendar({ value: [$("#txt_submittime_s").val()] });
            }
            if ($("#txt_submittime_e").val() != "") {
                $("#subdateend").val($("#txt_submittime_e").val());
                $("#subdateend").calendar({ value: [$("#txt_submittime_e").val()] });
            }
            
        } else {
            //$("#subdatestart").val(beforeday);
            //$("#subdatestart").calendar({ value: [beforeday] });

            //$("#subdateend").val(today);
            //$("#subdateend").calendar({ value: [today] });

            $("#subdatestart").val("");
            $("#subdatestart").calendar({ });

            $("#subdateend").val("");
            $("#subdateend").calendar({ });
        }

        if ($("#txt_sitepasstime_s").val() != "" || $("#txt_sitepasstime_e").val() != "") {

            if ($("#txt_sitepasstime_s").val() != "") {
                $("#passdatestart").val($("#txt_sitepasstime_s").val());
                $("#passdatestart").calendar({ value: [$("#txt_sitepasstime_s").val()] });
            }
            if ($("#txt_sitepasstime_e").val() != "") {
                $("#passdateend").val($("#txt_sitepasstime_e").val());
                $("#passdateend").calendar({ value: [$("#txt_sitepasstime_e").val()] });
            }

        } else {
            //$("#passdatestart").val(beforeday);
            //$("#passdatestart").calendar({ value: [beforeday] });

            //$("#passdateend").val(today);
            //$("#passdateend").calendar({ value: [today] });

            $("#passdatestart").val("");
            $("#passdatestart").calendar({ });

            $("#passdateend").val("");
            $("#passdateend").calendar({ });
        }

        //----------------------------------------------------------------------------------------------------------------------按钮事件

        $("#btn_more_cancel").click(function () {
            $.closeModal(".morediv");
        });

        $("#btn_more_sure").click(function () {
            var busitypeid = "";
            $("#busitype .labcontent").each(function () {
                if ($(this).css("color") == "rgb(0, 0, 255)") {
                    busitypeid = busitypeid + "'" + getbusitypeid($(this).html()) + "',";
                }
            });
            if (busitypeid != "") {
                busitypeid = busitypeid.substr(0, busitypeid.length - 1);
            }

            var ischeck = ""; var ispass = "";
            $("#otherflag .labcontent").each(function () {
                if ($(this).attr("id") == "flag_checked" && $(this).css("color") == "rgb(0, 0, 255)") {
                    ischeck = $(this).html();
                }
                if ($(this).attr("id") == "flag_uncheck" && $(this).css("color") == "rgb(0, 0, 255)") {
                    ischeck = $(this).html();
                }
                if ($(this).attr("id") == "flag_passed" && $(this).css("color") == "rgb(0, 0, 255)") {
                    ispass = $(this).html();
                }
                if ($(this).attr("id") == "flag_unpass" && $(this).css("color") == "rgb(0, 0, 255)") {
                    ispass = $(this).html();
                }
            });

            $("#txt_busitype").val(busitypeid);
            $("#txt_ischeck").val(ischeck);
            $("#txt_ispass").val(ispass);

            $("#txt_busiunit").val($("#busiunit").val());
            $("#txt_ordercode").val($("#ordercode").val()); 
            $("#txt_cusno").val($("#cusno").val()); 
            $("#txt_tradeway").val($("#tradeway").val()); 
            $("#txt_contractno").val($("#contractno").val()); 
            $("#txt_blno").val($("#blno").val()); 
            $("#txt_submittime_s").val($("#subdatestart").val());
            $("#txt_submittime_e").val($("#subdateend").val()); 
            $("#txt_sitepasstime_s").val($("#passdatestart").val()); 
            $("#txt_sitepasstime_e").val($("#passdateend").val()); 

            $.closeModal(".morediv");
        });

        $("#btn_more_reset").click(function () {
            $("#txt_reptime_s").val(""); $("#txt_reptime_e").val("");
            $("#txt_reptime_s").calendar({}); $("#txt_reptime_e").calendar({});//否则之前选的那天  不能再次选中

            $("#txt_declcode").val("");
            $("#picker_CUSTOMSSTATUS").picker("setValue", ["全部"]); $("#picker_MODIFYFLAG").picker("setValue", ["全部"]);

            $("#txt_busitype").val("");
            $("#txt_ischeck").val("");
            $("#txt_ispass").val("");

            $(".labcontent").each(function () { $(this).removeClass("labhover"); });
            $(".labtitle").each(function () { $(this).removeClass("labhover"); });

            $("#txt_busiunit").val(""); $("#busiunit").val("");
            $("#txt_ordercode").val(""); $("#ordercode").val("");
            $("#txt_cusno").val(""); $("#cusno").val("");
            $("#txt_tradeway").val(""); $("#tradeway").val("");
            $("#txt_contractno").val(""); $("#contractno").val("");
            $("#txt_blno").val(""); $("#blno").val("");
            $("#txt_submittime_s").val(""); $("#subdatestart").val(""); $("#subdatestart").calendar({});
            $("#txt_submittime_e").val(""); $("#subdateend").val(""); $("#subdateend").calendar({});
            $("#txt_sitepasstime_s").val(""); $("#passdatestart").val(""); $("#passdatestart").calendar({});
            $("#txt_sitepasstime_e").val(""); $("#passdateend").val(""); $("#passdateend").calendar({});

            $.closeModal(".morediv");
        });

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

//获取业务类型ID
function getbusitypeid(busitypename) {
    var busitypeid = "";

    switch (busitypename) {
        case "空运进口":
            busitypeid = "11";
            break;
        case "空运出口":
            busitypeid = "10";
            break;
        case "海运进口":
            busitypeid = "21";
            break;
        case "海运出口":
            busitypeid = "20";
            break;
        case "陆运进口":
            busitypeid = "31";
            break;
        case "陆运出口":
            busitypeid = "30";
            break;
        case "国内进口":
            busitypeid = "41";
            break;
        case "国内出口":
            busitypeid = "40";
            break;
        case "特殊进口":
            busitypeid = "51";
            break;
        case "特殊出口":
            busitypeid = "50";
            break;
    }

    return busitypeid;
}

/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
/////////////////////////////我的订单模块/////////////////////////////////////////////////////////////////////////////
//我的订单高级查询
function initSerach_MyOrder() {
    $(document).on('click', '.open-tabs-modal', function () {
        $.modal({
            text:
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
                            '<div class="col-25">放行日期:</div>' +
                            '<div class="col-33" style="width:35%;margin-left:0rem;"><input type="text" id="passdatestart" placeholder="起始"/></div>' +
                            '<div class="col-33" style="width:36%;margin-left:0rem;"><input type="text" id="passdateend" placeholder="结束"/></div>' +
                        '</div>' +
                        '<div style="margin-top:.5rem;" class="row">' +
                            '<div class="col-20" ><a href="#" id="btn_more_cancel" class="button">取消</a></div>' +
                            '<div class="col-60" style="width:59.99%; margin-left:0rem;"><a href="#" id="btn_more_sure" style="background-color: #3d4145;" class="button">确&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;认</a></div>' +
                            '<div class="col-20" style="margin-left:0rem;"><a href="#" id="btn_more_reset" class="button">重置</a></div>' +
                        '</div>' +
                    '</div>',
            extraClass: 'moredivmyorder'//避免直接设置.modal的样式，从而影响其他toast的提示
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

        //----------------------------------------------------------------------------------------------------------------------赋值
        if ($("#txt_busitype").val() != "") {
            $("#busitype .labcontent").each(function () {
                if ($("#txt_busitype").val().replace(getbusitypeid($(this).html()), "") != $("#txt_busitype").val()) {
                    $(this).addClass("labhover");
                }
            });

            var inall = true;
            $("#busi_in .labcontent").each(function () {
                if ($(this).css("color") != "rgb(0, 0, 255)") { inall = false; }
            });
            if (inall) { $("#busi_in .labtitle").addClass("labhover"); }

            var outall = true;
            $("#busi_out .labcontent").each(function () {
                if ($(this).css("color") != "rgb(0, 0, 255)") { outall = false; }
            });
            if (outall) { $("#busi_out .labtitle").addClass("labhover"); }
        }

        if ($("#txt_modifyflag").val() == "删单") { $("#mod_delete").addClass("labhover"); }
        if ($("#txt_modifyflag").val() == "改单") { $("#mod_update").addClass("labhover"); }
        if ($("#txt_modifyflag").val() == "改单完成") { $("#mod_finish").addClass("labhover"); }

        if ($("#txt_auditflag").val() == "稽核") { $("#audit").addClass("labhover"); }

        $("#busiunit").val($("#txt_busiunit").val());
        $("#ordercode").val($("#txt_ordercode").val());
        $("#cusno").val($("#txt_cusno").val());
        $("#divideno").val($("#txt_divideno").val());
        $("#contractno").val($("#txt_contractno").val());

       

        

        if ($("#txt_sitepasstime_s").val() != "" || $("#txt_sitepasstime_e").val() != "") {

            if ($("#txt_sitepasstime_s").val() != "") {
                $("#passdatestart").val($("#txt_sitepasstime_s").val());
                $("#passdatestart").calendar({ value: [$("#txt_sitepasstime_s").val()] });
            }
            if ($("#txt_sitepasstime_e").val() != "") {
                $("#passdateend").val($("#txt_sitepasstime_e").val());
                $("#passdateend").calendar({ value: [$("#txt_sitepasstime_e").val()] });
            }

        } else {
            
            $("#passdatestart").val("");
            $("#passdatestart").calendar({});

            $("#passdateend").val("");
            $("#passdateend").calendar({});

        }

        //----------------------------------------------------------------------------------------------------------------------按钮事件

        $("#btn_more_cancel").click(function () {
            $.closeModal(".moredivmyorder");
        });

        $("#btn_more_sure").click(function () {
            var busitypeid = "";
            $("#busitype .labcontent").each(function () {
                if ($(this).css("color") == "rgb(0, 0, 255)") {
                    busitypeid = busitypeid + "'" + getbusitypeid($(this).html()) + "',";
                }
            });
            if (busitypeid != "") {
                busitypeid = busitypeid.substr(0, busitypeid.length - 1);
            }

            var modifyflag = ""; var auditflag = "";
            $("#modify .labcontent").each(function () {
                if ($(this).attr("id") == "mod_delete" && $(this).css("color") == "rgb(0, 0, 255)") {
                    modifyflag = $(this).html();
                }
                if ($(this).attr("id") == "mod_update" && $(this).css("color") == "rgb(0, 0, 255)") {
                    modifyflag = $(this).html();
                }
                if ($(this).attr("id") == "mod_finish" && $(this).css("color") == "rgb(0, 0, 255)") {
                    modifyflag = $(this).html();
                }
                if ($(this).attr("id") == "audit" && $(this).css("color") == "rgb(0, 0, 255)") {
                    auditflag = $(this).html();
                }
            });

            $("#txt_busitype").val(busitypeid);
            $("#txt_modifyflag").val(modifyflag);
            $("#txt_auditflag").val(auditflag);

            $("#txt_busiunit").val($("#busiunit").val());
            $("#txt_ordercode").val($("#ordercode").val());
            $("#txt_cusno").val($("#cusno").val());
            $("#txt_divideno").val($("#divideno").val());
            $("#txt_contractno").val($("#contractno").val());
            $("#txt_sitepasstime_s").val($("#passdatestart").val());
            $("#txt_sitepasstime_e").val($("#passdateend").val());

            $.closeModal(".moredivmyorder");
        });

        $("#btn_more_reset").click(function () {
            $("#txt_submittime_s").val(""); $("#txt_submittime_e").val("");
            $("#txt_submittime_s").calendar({}); $("#txt_submittime_e").calendar({});//否则之前选的那天  不能再次选中

            $("#txt_declcode").val(""); $("#txt_customareacode").val("");
            $("#picker_is_pass").picker("setValue", ["全部"]); $("#picker_ischeck").picker("setValue", ["全部"]);

            $("#txt_busitype").val("");
            $("#txt_modifyflag").val("");
            $("#txt_auditflag").val("");

            $(".labcontent").each(function () { $(this).removeClass("labhover"); });
            $(".labtitle").each(function () { $(this).removeClass("labhover"); });

            $("#txt_busiunit").val(""); $("#busiunit").val("");
            $("#txt_ordercode").val(""); $("#ordercode").val("");
            $("#txt_cusno").val(""); $("#cusno").val("");
            $("#txt_divideno").val(""); $("#divideno").val("");
            $("#txt_contractno").val(""); $("#contractno").val("");
            $("#txt_sitepasstime_s").val(""); $("#passdatestart").val(""); $("#passdatestart").calendar({});
            $("#txt_sitepasstime_e").val(""); $("#passdateend").val(""); $("#passdateend").calendar({});

            $.closeModal(".moredivmyorder");
        });
    });
}

//业务订阅清单
function initSerach_SubscribeBusi() {
    $(document).on('click', '.open-tabs-modal', function () {
        $.modal({
            text:
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
                        '<div class="row">' +
                            '<div class="col-50"><input type="text" id="ordercode" placeholder="订单编号"/></div>' +
                            '<div class="col-50" style="margin-left:0rem;"><input type="text" id="cusno" placeholder="企业编号"/></div>' +
                        '</div>' +
                        '<div class="row">' +
                            '<div class="col-50"><input type="text" id="divideno" placeholder="分单号"/></div>' +
                            '<div class="col-50" style="margin-left:0rem;"><input type="text" id="contractno" placeholder="合同号"/></div>' +
                        '</div>' +
                        '<div class="row">' +
                            '<div class="col-25">委托日期:</div>' +
                            '<div class="col-33" style="width:35%;margin-left:0rem;"><input type="text" id="submitdatestart" placeholder="起始"/></div>' +
                            '<div class="col-33" style="width:36%;margin-left:0rem;"><input type="text" id="submitdateend" placeholder="结束"/></div>' +
                        '</div>' +
                        '<div style="margin-top:.5rem;" class="row">' +
                            '<div class="col-20" ><a href="#" id="btn_more_cancel" class="button">取消</a></div>' +
                            '<div class="col-60" style="width:59.99%; margin-left:0rem;"><a href="#" id="btn_more_sure" style="background-color: #3d4145;" class="button">确&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;认</a></div>' +
                            '<div class="col-20" style="margin-left:0rem;"><a href="#" id="btn_more_reset" class="button">重置</a></div>' +
                        '</div>' +
                    '</div>',
            extraClass: 'moredivmyorder'//避免直接设置.modal的样式，从而影响其他toast的提示
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
        

        //----------------------------------------------------------------------------------------------------------------------赋值
        if ($("#txt_busitype").val() != "") {
            $("#busitype .labcontent").each(function () {
                if ($("#txt_busitype").val().replace(getbusitypeid($(this).html()), "") != $("#txt_busitype").val()) {
                    $(this).addClass("labhover");
                }
            });

            var inall = true;
            $("#busi_in .labcontent").each(function () {
                if ($(this).css("color") != "rgb(0, 0, 255)") { inall = false; }
            });
            if (inall) { $("#busi_in .labtitle").addClass("labhover"); }

            var outall = true;
            $("#busi_out .labcontent").each(function () {
                if ($(this).css("color") != "rgb(0, 0, 255)") { outall = false; }
            });
            if (outall) { $("#busi_out .labtitle").addClass("labhover"); }
        }

       

        $("#ordercode").val($("#txt_ordercode").val());
        $("#cusno").val($("#txt_cusno").val());
        $("#divideno").val($("#txt_divideno").val());
        $("#contractno").val($("#txt_contractno").val());





        if ($("#txt_submittime_s").val() != "" || $("#txt_submittime_e").val() != "") {

            if ($("#txt_submittime_s").val() != "") {
                $("#submitdatestart").val($("#txt_submittime_s").val());
                $("#submitdatestart").calendar({ value: [$("#txt_submittime_s").val()] });
            }
            if ($("#txt_submittime_e").val() != "") {
                $("#submitdateend").val($("#txt_submittime_e").val());
                $("#submitdateend").calendar({ value: [$("#txt_submittime_e").val()] });
            }

        } else {

            $("#submitdatestart").val("");
            $("#submitdatestart").calendar({});

            $("#submitdateend").val("");
            $("#submitdateend").calendar({});

        }

        //----------------------------------------------------------------------------------------------------------------------按钮事件

        $("#btn_more_cancel").click(function () {
            $.closeModal(".moredivmyorder");
        });

        $("#btn_more_sure").click(function () {
            var busitypeid = "";
            $("#busitype .labcontent").each(function () {
                if ($(this).css("color") == "rgb(0, 0, 255)") {
                    busitypeid = busitypeid + "'" + getbusitypeid($(this).html()) + "',";
                }
            });
            if (busitypeid != "") {
                busitypeid = busitypeid.substr(0, busitypeid.length - 1);
            }

           

            $("#txt_busitype").val(busitypeid);

            $("#txt_ordercode").val($("#ordercode").val());
            $("#txt_cusno").val($("#cusno").val());
            $("#txt_divideno").val($("#divideno").val());
            $("#txt_contractno").val($("#contractno").val());
            $("#txt_submittime_s").val($("#submitdatestart").val());
            $("#txt_submittime_e").val($("#submitdateend").val());

            $.closeModal(".moredivmyorder");
        });

        $("#btn_more_reset").click(function () {
            $("#txt_submittime_s").val(""); $("#txt_submittime_e").val("");
            $("#txt_submittime_s").calendar({}); $("#txt_submittime_e").calendar({});//否则之前选的那天  不能再次选中

            $("#txt_declcode").val(""); $("#txt_customareacode").val("");
            $("#picker_is_pass").picker("setValue", ["全部"]); $("#picker_ischeck").picker("setValue", ["全部"]);

            $("#txt_busitype").val("");

            $(".labcontent").each(function () { $(this).removeClass("labhover"); });
            $(".labtitle").each(function () { $(this).removeClass("labhover"); });

            $("#txt_ordercode").val(""); $("#ordercode").val("");
            $("#txt_cusno").val(""); $("#cusno").val("");
            $("#txt_divideno").val(""); $("#divideno").val("");
            $("#txt_contractno").val(""); $("#contractno").val("");
            $("#txt_submittime_s").val(""); $("#submitdatestart").val(""); $("#submitdatestart").calendar({});
            $("#txt_submittime_e").val(""); $("#submitdateend").val(""); $("#submitdateend").calendar({});

            $.closeModal(".moredivmyorder");
        });
    });
}

