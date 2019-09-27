; (function ($) {
    "use strict";
    $.fn.ysTable = function (option, param) {
        //如果是调用方法
        if (typeof option == 'string') {
            return $.fn.ysTable.methods[option](this, param);
        }

        //如果是初始化组件
        var _option = $.extend({}, $.fn.ysTable.defaults, option || {});
        var target = $(this);
        target.bootstrapTable(_option);
        return target;
    };

    $.fn.ysTable.methods = {
        search: function (target) {
            // 从第一页开始
            target.bootstrapTable('refresh', { pageNumber: 1 });
        },
        getPagination: function (target, params) {
            var pagination = {
                pageSize: params.limit,                         //页面大小
                pageIndex: (params.offset / params.limit) + 1,   //页码
                sort: params.sort,      //排序列名
                sortType: params.order //排位命令（desc，asc）
            };
            return pagination;
        }
    };

    $.fn.ysTable.defaults = {
        method: 'GET',                      // 请求方式（*）
        toolbar: '#toolbar',                // 工具按钮用哪个容器
        striped: true,                      // 是否显示行间隔色
        cache: false,                       // 是否使用缓存，默认为true，所以一般情况下需要设置一下这个属性（*）
        pagination: true,                   // 是否显示分页（*）
        sortable: true,                     // 是否启用排序
        sortStable: true,                   // 设置为 true 将获得稳定的排序
        sortName: 'Id',                     // 排序列名称
        sortOrder: "desc",                  // 排序方式
        sidePagination: "server",           // 分页方式：client客户端分页，server服务端分页（*）
        pageNumber: 1,                      // 初始化加载第一页，默认第一页,并记录
        pageSize: 10,                       // 每页的记录行数（*）
        pageList: "10, 25, 50, 100",        // 可供选择的每页的行数（*）
        search: false,                      // 是否显示表格搜索
        strictSearch: true,
        showColumns: true,                  // 是否显示所有的列（选择显示的列）
        showRefresh: true,                  // 是否显示刷新按钮
        showToggle: true,                   // 是否显示详细视图和列表视图的切换按钮
        minimumCountColumns: 2,             // 最少允许的列数
        clickToSelect: true,                // 是否启用点击选中行
        height: undefined,                  // 行高，如果没有设置height属性，表格自动根据记录条数觉得表格高度
        uniqueId: "Id",                     // 每一行的唯一标识，一般为主键列
        cardView: false,                    // 是否显示详细视图
        detailView: false,                  // 是否显示父子表
        totalField: 'TotalCount',
        dataField: 'Result',
        columns: [],
        queryParams: {},
        onLoadSuccess: function (data) {
            if (data) {
                if (data.Tag != 1) {
                    ys.alertError(data.Message);
                }
            }
        },
        onLoadError: function (status, s) {
            if (s.statusText != "abort") {
                ys.alertError("数据加载失败！");
            }
        }
    };
})(window.jQuery);