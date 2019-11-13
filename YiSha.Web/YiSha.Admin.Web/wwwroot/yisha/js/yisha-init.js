/*页面的ready函数执行之后再执行*/
$(function () {
    // checkbox 事件绑定
    if ($(".check-box").length > 0) {
        $(".check-box").iCheck({
            checkboxClass: 'icheckbox-blue',
            radioClass: 'iradio-blue',
        });
    }

    // radio 事件绑定
    if ($(".radio-box").length > 0) {
        $(".radio-box").iCheck({
            checkboxClass: 'icheckbox-blue',
            radioClass: 'iradio-blue',
        });
    }

    // laydate 时间控件绑定
    if ($(".select-time").length > 10) {
        layui.use('laydate', function () {
            var laydate = layui.laydate;
            var startDate = laydate.render({
                elem: '#startTime',
                max: $('#endTime').val(),
                theme: 'molv',
                trigger: 'click',
                done: function (value, date) {
                    // 结束时间大于开始时间
                    if (value !== '') {
                        endDate.config.min.year = date.year;
                        endDate.config.min.month = date.month - 1;
                        endDate.config.min.date = date.date;
                    } else {
                        endDate.config.min.year = '';
                        endDate.config.min.month = '';
                        endDate.config.min.date = '';
                    }
                }
            });
            var endDate = laydate.render({
                elem: '#endTime',
                min: $('#startTime').val(),
                theme: 'molv',
                trigger: 'click',
                done: function (value, date) {
                    // 开始时间小于结束时间
                    if (value !== '') {
                        startDate.config.max.year = date.year;
                        startDate.config.max.month = date.month - 1;
                        startDate.config.max.date = date.date;
                    } else {
                        startDate.config.max.year = '';
                        startDate.config.max.month = '';
                        startDate.config.max.date = '';
                    }
                }
            });
        });
    }

    // tree 关键字搜索绑定
    if ($("#keyword").length > 0) {
        $("#keyword").bind("focus", function focusKey(e) {
            if ($("#keyword").hasClass("empty")) {
                $("#keyword").removeClass("empty");
            }
        }).bind("blur", function blurKey(e) {
            if ($("#keyword").val() === "") {
                $("#keyword").addClass("empty");
            }
            $.tree.searchNode(e);
        }).bind("input propertychange", $.tree.searchNode);
    }

    // bootstrap table tree 表格树 展开/折叠
    var expandFlag = false;
    $("#btnExpandAll").click(function () {
        if (expandFlag) {
            $('#gridTable').bootstrapTreeTable('expandAll');
        } else {
            $('#gridTable').bootstrapTreeTable('collapseAll');
        }
        expandFlag = expandFlag ? false : true;
    });

    // 复选框后按钮样式状态变更
    $("#gridTable").on("check.bs.table uncheck.bs.table check-all.bs.table uncheck-all.bs.table", function () {
        var ids = $("#gridTable").bootstrapTable("getSelections");
        $('#btnDelete').toggleClass('disabled', !ids.length);
        $('#btnEdit').toggleClass('disabled', ids.length != 1);
    });

    // select2复选框事件绑定
    if ($.fn.select2 !== undefined) {
        $("select.form-control.select2").each(function () {
            $(this).select2().on("change", function () {
                $(this).valid();
            });
        });
    }

    $("#searchDiv").keyup(function (e) {
        if (e.which === 13) {
            $("#btnSearch").click();
        }
    });

    // 校验按钮权限，没有权限的按钮就隐藏
    if (top.getButtonAuthority) {
        var buttonList = [];
        $('#toolbar').find('a').each(function (i, ele) {
            buttonList.push(ele.id);
        });
        var removeButtonList = top.getButtonAuthority(window.location.href, buttonList);
        if (removeButtonList) {
            $.each(removeButtonList, function (i, val) {
                $("#" + val).remove();
            });
        }
    }

    // input,select 的id赋值给name，因为jquery.validation验证组件使用的是name
    $("input:text, input:password, input:radio, select").each(function (i, ele) {
        if (ele.id) {
            $(ele).attr("name", ele.id);
        }
    });
});

function resetToolbarStatus() {
    if ($('#btnDelete')) {
        $('#btnDelete').toggleClass('disabled');
    }
    if ($('#btnEdit')) {
        $('#btnEdit').toggleClass('disabled');
    }
}

function createMenuItem(dataUrl, menuName) {
    var dataIndex = ys.getGuid,
        flag = true;
    if (dataUrl == undefined || $.trim(dataUrl).length == 0) return false;
    var topWindow = $(window.parent.document);
    // 选项卡菜单已存在
    $('.menuTab', topWindow).each(function () {
        if ($(this).data('id') == dataUrl) {
            if (!$(this).hasClass('active')) {
                $(this).addClass('active').siblings('.menuTab').removeClass('active');
                $('.page-tabs-content').animate({ marginLeft: "" }, "fast");
                // 显示tab对应的内容区
                $('.mainContent .YiSha_iframe', topWindow).each(function () {
                    if ($(this).data('id') == dataUrl) {
                        $(this).show().siblings('.YiSha_iframe').hide();
                        return false;
                    }
                });
            }
            flag = false;
            return false;
        }
    });
    // 选项卡菜单不存在
    if (flag) {
        var str = '<a href="javascript:;" class="active menuTab" data-id="' + dataUrl + '">' + menuName + ' <i class="fa fa-times-circle"></i></a>';
        $('.menuTab', topWindow).removeClass('active');

        // 添加选项卡对应的iframe
        var str1 = '<iframe class="YiSha_iframe" name="iframe' + dataIndex + '" width="100%" height="100%" src="' + dataUrl + '" frameborder="0" data-id="' + dataUrl + '" seamless></iframe>';
        $('.mainContent', topWindow).find('iframe.YiSha_iframe').hide().parents('.mainContent').append(str1);

        // 添加选项卡
        $('.menuTabs .page-tabs-content', topWindow).append(str);
    }
    return false;
}
