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
        var arr = [];
        for (var i = 0; i < dataDict[dictType].length; i++) {
            if (dataDict[dictType][i].DictStatus == 1) {
                arr.push(dataDict[dictType][i]);
            }
        }
        return arr;
    }
    function getDataDictValue(dictType, dictKey) {
        if (dataDict[dictType]) {
            for (var i = 0; i < dataDict[dictType].length; i++) {
                if (dataDict[dictType][i].DictKey == dictKey) {
                    if (dataDict[dictType][i].ListClass) {
                        return '<span class="badge badge-' + dataDict[dictType][i].ListClass + '">' + dataDict[dictType][i].DictValue + '</span>';
                    }
                    else {
                        return dataDict[dictType][i].DictValue;
                    }
                }
            }
        }
        return '';
    }

    function initDataAuthority() {
        ys.ajax({
            url: ctx + 'OrganizationManage/User/GetUserAuthorizeJson',
            type: "get",
            success: function (obj) {
                if (obj.Tag == 1) {
                    dataAuthority = obj.Result;
                }
            }
        });
    }
    function getButtonAuthority(url, buttonList) {
        var noAuthorize = [];
        if (dataAuthority) {
            // 超级用户不验证
            if (dataAuthority.IsSystem != 1) {
                var regex = /([a-zA-Z]+)Manage\/(.*)\//;  //match url like http://localhost:5000/OrganizationManage/User/UserIndex
                var matches = regex.exec(url);
                if (matches && matches.length >= 3) {
                    var module = matches[1];
                    var page = matches[2];
                    buttonList.forEach(function (btn, btnIndex) {
                        var authorize = module.toLowerCase() + ':' + page.toLowerCase() + ':' + btn.toString().replace('btn', '').toLowerCase();
                        var hasAuthority = false;

                        dataAuthority.MenuAuthorize.forEach(function (authority, authorityIndex) {
                            if (authority.Authorize == authorize) {
                                hasAuthority = true;
                                return false;
                            }
                        });

                        if (!hasAuthority) {
                            noAuthorize.push(btn);
                        }
                    });
                }
            }
        }
        return noAuthorize;
    }

    initDataDict();
    initDataAuthority();

    // 公开方法
    window.initDataDict = initDataDict;
    window.getDataDict = getDataDict;
    window.getDataDictValue = getDataDictValue;

    window.getButtonAuthority = getButtonAuthority;
})(window);