/**
 * @author: YL
 * @version: v1.0.0
 */
!function ($) {
    'use strict';
    $.extend($.fn.bootstrapTable.defaults, {
        treeShowField: null,
        idField: 'id',
        parentIdField: 'pid',
        onGetNodes: function (row, data) {
            var that = this;
            var nodes = [];
            $.each(data, function (i, item) {
                if (row[that.options.idField] === item[that.options.parentIdField]) {
                    nodes.push(item);
                }
            });
            return nodes;
        },
        onCheckRoot: function (row, data) {
            var that = this;
            return !row[that.options.parentIdField];
        }
    });

    var BootstrapTable = $.fn.bootstrapTable.Constructor,
        _initRow = BootstrapTable.prototype.initRow,
        _initHeader = BootstrapTable.prototype.initHeader;

    // td
    BootstrapTable.prototype.initHeader = function () {
        var that = this;
        _initHeader.apply(that, Array.prototype.slice.apply(arguments));
        var treeShowField = that.options.treeShowField;
        if (treeShowField) {
            $.each(this.header.fields, function (i, field) {
                if (treeShowField === field) {
                    that.treeEnable = true;
                    return false;
                }
            });
        }
    };

    var initTr = function (item, idx, data, parentDom) {
        var that = this;
        var nodes = that.options.onGetNodes.apply(that, [item, data]);
        item._nodes = nodes;
        parentDom.append(_initRow.apply(that, [item, idx, data, parentDom]));

        // init sub node
        var len = nodes.length - 1;
        for (var i = 0; i <= len; i++) {
            var node = nodes[i];
            node._level = item._level + 1;
            node._parent = item;
            if (i === len)
                node._last = 1;
            // jquery.treegrid.js
            that.options.rowStyle = function (item, idx) {
                var id = item[that.options.idField] ? item[that.options.idField] : 0;
                var pid = item[that.options.parentIdField] ? item[that.options.parentIdField] : 0;
                return {
                    classes: 'treegrid-' + id + ' treegrid-parent-' + pid
                };
            };
            initTr.apply(that, [node, $.inArray(node, data), data, parentDom]);
        }
    };

    // tr
    BootstrapTable.prototype.initRow = function (item, idx, data, parentDom) {
        var that = this;
        if (that.treeEnable) {
            // init root node
            if (that.options.onCheckRoot.apply(that, [item, data])) {
                if (item._level === undefined) {
                    item._level = 0;
                }
                // jquery.treegrid.js
                that.options.rowStyle = function (item, idx) {
                    var x = item[that.options.idField] ? item[that.options.idField] : 0;
                    return {
                        classes: 'treegrid-' + x
                    };
                };
                initTr.apply(that, [item, idx, data, parentDom]);
                return true;
            }
            return false;
        }
        return _initRow.apply(that, Array.prototype.slice.apply(arguments));
    };
}(jQuery);