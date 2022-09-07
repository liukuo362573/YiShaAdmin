(function ($) {
    "use strict";
    $.fn.ysTree = function (option, param) {
        if (typeof option == 'string') {
            return $.fn.ysTree.methods[option](this, param);
        }
        var _option = $.extend({}, $.fn.ysTree.defaults, option || {});
        var target = $(this);
        var id = target.attr("id");

        // 显示垂直滚动条
        target.css("overflow-y", "auto").css("max-height", _option.maxHeight);

        ys.ajax({
            url: _option.url,
            async: _option.async,
            success: function (data) {
                var tree = $.fn.zTree.init($("#" + id), _option, data.Data);
                for (var level = 0; level <= _option.expandLevel; level++) {
                    var nodes = tree.getNodesByParam("level", level);
                    for (var i = 0; i < nodes.length; i++) {
                        tree.expandNode(nodes[i], true, false, false);
                    }
                }
            }
        });
        return target;
    };
    $.fn.ysTree.methods = {
        getCheckedNodes: function (target, column) {
            var zTreeObj = $.fn.zTree.getZTreeObj($(target).attr("id"));
            var _column = ys.isNullOrEmpty(column) ? "id" : column;
            var nodes = zTreeObj.getCheckedNodes(true);
            return $.map(nodes, function (row) {
                return row[_column];
            }).join();
        },
        setCheckedNodes: function (target, ids) {
            if (!ys.isNullOrEmpty(ids)) {
                var _ids = ids.split(',');
                var zTreeObj = $.fn.zTree.getZTreeObj($(target).attr("id"));
                zTreeObj.cancelSelectedNode();//先取消所有的选中状态
                $.each(_ids, function (i, id) {
                    var node = zTreeObj.getNodeByParam("id", id);
                    zTreeObj.checkNode(node, true, false, true);
                });
            }
        },
        setCheckedNodesByName: function (target, names) {
            if (!ys.isNullOrEmpty(names)) {
                var _names = names.split(',');
                var zTreeObj = $.fn.zTree.getZTreeObj($(target).attr("id"));
                zTreeObj.cancelSelectedNode();//先取消所有的选中状态
                $.each(_names, function (i, name) {
                    var node = zTreeObj.getNodeByParam("name", name);
                    zTreeObj.checkNode(node, true, false, true);
                });
            }
        }
    };
    $.fn.ysTree.defaults = {
        url: '',
        async: false,
        maxHeight: "300px",
        expandLevel: 0,
        check: { "enable": false },
        view: { selectedMulti: false, nameIsHTML: true },
        data: { simpleData: { enable: true } },
        callback: {}
    };

    // 下拉框，里面展示的数据是树形，和ysComboBox对应
    $.fn.ysComboBoxTree = function (option, param) {
        if (typeof option == 'string') {
            return $.fn.ysComboBoxTree.methods[option](this, param);
        }
        var _option = $.extend({}, $.fn.ysComboBoxTree.defaults, option || {});

        var target = $(this);
        var id = target.attr("id");

        var eleInputId = id + "_input";
        var eleTreeId = id + "_tree";

        // 样式需要改成通用的
        target.css("position", "relative");
        var html = "<input id='" + eleInputId + "' name='" + eleInputId + "' readonly='readonly' type='text' class='form-control' />";
        html += "<div id='" + eleTreeId + "' class='ztree treeSelect-panel' style='overflow-y: auto;max-height:" + _option.maxHeight + ";border:1px solid #e5e6e7;margin-top:1px;display:none'></div>";
        $(html).appendTo(target);

        $("#" + eleInputId).click(function () {
            var targetTree = $("#" + eleTreeId);
            if (targetTree.is(":hidden")) {
                targetTree.show();
            }
            else {
                targetTree.hide();
            }
        });

        ys.ajax({
            url: _option.url,
            async: _option.async,
            success: function (data) {
                var targetTree = $("#" + eleTreeId);
                var targetInput = $("#" + eleInputId);

                // 用户定义的onClick回调
                var customOnClick = _option.callback.customOnClick;
                // OnClick callback
                _option.callback.onClick = function (event, treeId, treeNode) {
                    var wholeName = '';
                    var wholeId = '';
                    var parentNode = treeNode;
                    while (parentNode != null) {
                        wholeName = parentNode.name + '>' + wholeName;
                        wholeId = parentNode.id + ',' + wholeId;
                        parentNode = parentNode.getParentNode();
                    }
                    wholeName = ys.trimEnd(wholeName, '>');
                    wholeId = ys.trimEnd(wholeId, ',');

                    target.attr("data-key", wholeId);
                    target.attr("data-value", wholeName);

                    targetInput.val(wholeName);
                    targetTree.hide();

                    if (customOnClick) {
                        customOnClick(event, treeId, treeNode);
                    }
                };

                target.ztree = $.fn.zTree.init($("#" + eleTreeId), _option, data.Data);
                if (_option.expandLevel >= 0) {
                    for (var level = 0; level <= _option.expandLevel; level++) {
                        var nodes = target.ztree.getNodesByParam("level", level);
                        for (var i = 0; i < nodes.length; i++) {
                            target.ztree.expandNode(nodes[i], true, false, false);
                        }
                    }
                }
            }
        });

        $(document).click(function (e) {
            var e = e ? e : window.event;
            var tar = e.srcElement || e.target;
            if (!$(tar).hasClass('form-control')) {
                var tarId = $(tar).attr("id");
                if (ys.isNullOrEmpty(tarId) || tarId.indexOf("_tree") == -1) {
                    var targetTree = $("#" + eleTreeId);
                    targetTree.hide();
                    e.stopPropagation();
                }
            }
        });

        return target;
    };
    $.fn.ysComboBoxTree.methods = {
        getValue: function (target) {
            return $(target).attr("data-key");
        },
        setValue: function (target, value) {
            var lastId = '0'; // 取最下面的一个值
            if (value) {
                var arr = value.toString().split(',');
                lastId = arr[arr.length - 1];
            }
            var id = target.attr("id");
            var eleTreeId = id + "_tree";
            var zTreeObj = $.fn.zTree.getZTreeObj(eleTreeId);
            var node = zTreeObj.getNodeByParam("id", lastId);
            if (node != null) {
                zTreeObj.cancelSelectedNode();//先取消所有的选中状态
                zTreeObj.selectNode(node, true);//将指定ID的节点选中
                zTreeObj.expandNode(node, true, false);//将指定ID节点展开
                zTreeObj.setting.callback.onClick('setValue', zTreeObj.setting.treeId, node); //触发onclick
            }
            return $(target);
        }
    };
    $.fn.ysComboBoxTree.defaults = {
        url: '',
        async: false,
        maxHeight: "200px",
        expandLevel: 0,
        check: { "enable": false },
        view: { selectedMulti: false, nameIsHTML: true },
        data: { simpleData: { enable: true } },
        callback: {}
    };

})(jQuery);
