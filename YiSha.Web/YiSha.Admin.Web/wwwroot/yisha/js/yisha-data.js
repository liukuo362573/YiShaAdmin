; (function (window) {
    "use strict";

    // 存放数据库里面所有的数据字典，获取字典类型列表或是字典值 例如top.getDataDict('NewsType')或top.getDataDictValue('NewsType' , 1)
    var dataDict = {};
    // 存放当前用户所拥有的权限
    var dataAuthority = {};

    function initDataDict() {
        ys.ajax({
            url: ctx + 'SystemManage/DataDict/GetDataDictListJson',
            type: "get",
            success: function (obj) {
                if (obj.Tag == 1) {
                    for (var i = 0; i < obj.Result.length; i++) {
                        dataDict[obj.Result[i].DictType] = obj.Result[i].Detail;
                    }
                }
            }
        });
    }
    function getDataDict(dictType) {
        return dataDict[dictType];
    }
    function getDataDictValue(dictType, dictKey) {
        if (dataDict[dictType]) {
            for (var i = 0; i < dataDict[dictType].length; i++) {
                if (dataDict[dictType][i].DictKey == dictKey) {
                    return dataDict[dictType][i].DictValue;
                }
            }
        }
        return '';
    }

    function initDataAuthority() {
        ys.ajax({
            url: ctx + 'SystemManage/DataDict/GetDataDictListJson',
            type: "get",
            success: function (obj) {
                if (obj.Tag == 1) {
                    for (var i = 0; i < obj.Result.length; i++) {
                        dataDict[obj.Result[i].DictType] = obj.Result[i].Detail;
                    }
                }
            }
        });
    }

    initDataDict();
    initDataAuthority();

    // 公开方法
    window.getDataDict = getDataDict;
    window.getDataDictValue = getDataDictValue;
})(window);