(function ($) {
    "use strict";
    $.fn.ysTreeTable = function (option, param) {
        //如果是调用方法
        if (typeof option == 'string') {
            return $.fn.ysTreeTable.methods[option](this, param);
        }

        //如果是初始化组件
        var _option = $.extend({}, $.fn.ysTreeTable.defaults, option || {});
        var target = $(this);
        target.bootstrapTreeTable(_option);
        return target;
    };

    $.fn.ysTreeTable.methods = {
        search: function (target, param) {
            return target.bootstrapTreeTable('refresh', param);
        },
        getRowById: function (target, id) {
            var tree = target.data('bootstrap.tree.table');
            for (var row in tree.data_obj) {
                if (tree.data_obj[row]) {
                    if (tree.data_obj[row].Id == id) {
                        return tree.data_obj[row];
                    }
                }
            }
        },
        expandRowById: function (target, id) {
            return target.bootstrapTreeTable('expandRow', id);
        }
    };

    $.fn.ysTreeTable.defaults = {
        method: 'GET',
        code: "Id",
        parentCode: "ParentId",
        bordered: true,
        expandColumn: '1',
        expandAll: false,
        expandFirst: true
    };
})(jQuery);