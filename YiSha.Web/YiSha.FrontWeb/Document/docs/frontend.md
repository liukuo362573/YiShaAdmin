## 前端组件

YiShaAdmin封装了一些常用的浏览器JS组件方法。

名称   |  使用  |  介绍  
-      |-      |-
[表格](#表格)   |  $('#id').ysTable(options)    | bootstrap table表格封装处理 |
[表格树](#表格树) |  $('#id').ysTreeTable(options)    | bootstrap table tree表格树封装处理 |
[树形选择](#树形选择) |   $('#id').ysTree(options)    |  ztree封装处理  |
-      |-      |-
[单选框](#单选框)   |  $('#id').ysRadioBox(options)    |  基础控件封装  |
[复选框](#复选框)   |  $('#id').ysCheckBox(options)     |  基础控件封装  |
[下拉框](#下拉框) |  $('#id').ysComboBox(options)   |  select2封装处理  |
[树形下拉框](#树形下拉框)   |  $('#id').ysComboBoxTree(options)    |  ztree封装处理  |
-      |-      |-
[弹出层](#弹出层)   |  ys.methodName    |  layer封装处理 |
[校验](#校验)   |  $('#id').validate(options)   |  jquery validation 封装处理  |
[通用方法](#通用方法)   |  ys.methodName    |  通用方法封装处理  |

## 表格

表格组件基于bootstrap table组件进行封装，轻松实现数据表格。

表格初始化$('#id').ysTable(options)

表格参数(Table options)

参数   |  类型  |  默认值  |  描述
-      |-      |-        |-
url    |  String | Null   | 请求后台的Url
method | String | Null | 请求方式
toolbar | String | Null |
striped | String | Null |
cache | String | Null |
pagination | String | Null |
columns | Array |  | Null  | 默认空数组，在JS里面定义参考列的各项(Column options)
queryParams | String | Null | 附加在Url后面的QueryString参数
sortable
sortStable
sortName | String | Null | 排序列名称
sortOrder | String | Null | 排序方式
sidePagination
pageNumber
pageSize
pageList
search
strictSearch
showColumns
showRefresh
showToggle
minimumCountColumns
clickToSelect
uniqueId | String | Null | 每一行的唯一标识，一般为主键列
cardView
detailView
totalField
dataField
onLoadSuccess
onLoadError
onDblClickRow

表格列的参数(Column options)

## 表格树

表格组件基于bootstrap table组件进行封装，轻松实现数据表格。

表格初始化$('#id').ysTable(options)

表格参数(Table options)

参数   |  类型  |  默认值  |  描述
-      |-      |-        |-
url    |  String | Null   | 请求后台的Url

## 树形选择

树形选择组件基于ztree组件进行封装。

树形选择初始化$('#id').ysTree(options)

常用参数(Tree options)

参数   |  类型  |  默认值  |  描述
-      |-      |-        |-
async    |  Boolean | false   | 是否是异步请求
async    |  Boolean | false   | 是否是异步请求
maxHeight    |  String | 200px   |  容器的最大高度，内容超出这个高度，容器会显示垂直滚动条
expandLevel    |  Number | 0   | 展开的级数，0表示展开一级，1表示展开二级，以此类推
check    |  Object |    | enable属性表示是否显示复选框
view    |  Object |    | selectedMulti属性表示你是否可以多选

常用方法

方法   |  参数  |   描述
-      |-      |-     
getValue    |  target   | 获取组件的值（第一个参数为组件）<br />使用示例 $('#id').ysComboBoxTree('getValue')
setValue    |  target,value   | 设置组件的值（第一个参数为组件，第二个为值）<br />使用示例 $('#id').ysComboBoxTree('setValue', value)

## 单选框

单选框初始化  $('#id').ysRadioBox(options)

常用参数 (RadioBox options)

参数   |  类型  |  默认值  |  描述
-      |-      |-        |-
url    |  String |    | 数据源url，支持ajax获取数据源
key    |  String | Key   | 数据源里面有很多属性的时候，key表示对应选项的值
value    |  String | Value   | 数据源里面有很多属性的时候，value表示对应的选项，界面上看到的数据
data    |  Object |    | 当url为空的时候，可以设置此属性作为组件的数据源
dataName    |  String |  Result  | 当数据源中的某一个属性作为实际要绑定的组件的数据源，dataName就是那个属性的值

常用方法

方法   |  参数  |   描述
-      |-      |-     
getValue    |  target   | 获取组件的值（第一个参数为组件）<br />使用示例 $('#id').ysRadioBox('getValue')
setValue    |  target,value   | 设置组件的值（第一个参数为组件，第二个为值）<br />使用示例 $('#id').ysRadioBox('setValue', value)

## 复选框

复选框初始化 $('#id').ysCheckBox(options)

常用参数 (CheckBox options)

参数   |  类型  |  默认值  |  描述
-      |-      |-        |-
url    |  String | false   | 数据源url，支持ajax获取数据源
key    |  String | Key   | 数据源里面有很多属性的时候，key表示对应选项的值
value    |  String | Value   | 数据源里面有很多属性的时候，value表示对应的选项，界面上看到的数据
data    |  Object |    | 当url为空的时候，可以设置此属性作为组件的数据源
dataName    |  String |  Result  | 当数据源中的某一个属性作为实际要绑定的组件的数据源，dataName就是那个属性的值

常用方法

方法   |  参数  |   描述
-      |-      |-     
getValue    |  target   | 获取组件的值（第一个参数为组件）<br />使用示例 $('#id').ysCheckBox('getValue')
setValue    |  target,value   | 设置组件的值（第一个参数为组件，第二个为值）<br />使用示例 $('#id').ysCheckBox('setValue', value)

## 下拉框

下拉框组件基于select2组件进行封装。

下拉框初始化 $('#id').ysComboBox(options)

常用参数 (ComboBox options)

参数   |  类型  |  默认值  |  描述
-      |-      |-        |-
url    |  String | false   | 数据源url，支持ajax获取数据源
key    |  String | 200px   | 数据源里面有很多属性的时候，key表示对应选项的值
value    |  String | 0   | 数据源里面有很多属性的时候，value表示对应的选项，界面上看到的数据
maxHeight    |  String |  160px  | 容器的最大高度，内容超出这个高度，容器会显示垂直滚动条
class    |  String |    | 下拉框的样式
multiple    |  Boolean |  false  | 组件是否可以多选，true 可以多选，false 不能多选
data    |  Object |    | 当url为空的时候，可以设置此属性作为组件的数据源
dataName    |  String |  Result  | 当数据源中的某一个属性作为实际要绑定的组件的数据源，dataName就是那个属性的值
onChange    |  Function |    | 当组件选项改变的时候触发

常用方法

方法   |  参数  |   描述
-      |-      |-     
getValue    |  target   | 获取组件的值（第一个参数为组件）<br />使用示例 $('#id').ysComboBox('getValue')
setValue    |  target,value   | 设置组件的值（第一个参数为组件，第二个为值）<br />使用示例 $('#id').ysComboBox('setValue', value)

## 树形下拉框

树形下拉框组件基于ztree组件进行封装。

树形下拉框初始化 $('#id').ysComboBoxTree(options)

常用参数 (ComboBoxTree options)

参数   |  类型  |  默认值  |  描述
-      |-      |-        |-
url    |  String |    | 数据源url，支持ajax获取数据源
async    |  Boolean | false   | 是否是异步请求
maxHeight    |  String | 200px   |  容器的最大高度，内容超出这个高度，容器会显示垂直滚动条
expandLevel    |  Number | 0   | 展开的级数，0表示展开一级，1表示展开二级，以此类推
check    |  Object |    | enable属性表示是否显示复选框
view    |  Object |    | selectedMulti属性表示你是否可以多选

常用方法

方法   |  参数  |   描述
-      |-      |-     
getValue    |  target   | 获取组件的值（第一个参数为组件）<br />使用示例 $('#id').ysComboBoxTree('getValue')
setValue    |  target,value   | 设置组件的值（第一个参数为组件，第二个为值）<br />使用示例 $('#id').ysComboBoxTree('setValue', value)

## 弹出层

弹出层组件基于 <code>layer</code> 组件进行封装，提供了弹出、消息、提示、确认、遮罩处理等功能。

- 提供成功、错误、警告弹出层
```javascript
    ys.msgSuccess('成功')
	ys.msgError('错误或失败')
	ys.msgWarning('警告')	
```
- 提供成功、错误、警告弹出层，需要点击关闭
```javascript
    ys.alertSuccess('成功')
	ys.alertError('错误或失败')
	ys.alertWarning('警告')	
```
- 提供页面弹出层
```javascript
    ys.openDialog({title:'页面标题',url:'页面url',width:'768px',height:'600px',callback})
	ys.closeDialog()
```
- 提供遮罩层
```javascript
    ys.showLoading('显示内容')
	ys.closeLoading()
```
- 修改页面数据示例，Index页面调用showSaveForm方法弹出修改Form页面，Form页面调用saveForm方法保存数据到服务端

1.Index 页面
```javascript
    function showSaveForm(bAdd) {
        var id = 0;
        if (!bAdd) {
            var selectedRow = $("#gridTable").bootstrapTable("getSelections");
            if (!ys.checkRowEdit(selectedRow)) {
                return;
            }
            else {
                id = selectedRow[0].Id;
            }
        }
        ys.openDialog({
            title: id > 0 ? "编辑文章" : "添加文章",
            url: '@Url.Content("~/OrganizationManage/News/NewsForm")' + '?id=' + id,
            width: "768px",
            height: "600px",
            callback: function (index, layero) {
                var iframeWin = window[layero.find('iframe')[0]['name']];
                iframeWin.saveForm(index);
            }
        });
    }
```

2.Form 页面
```javascript
    function saveForm(index) {
        if ($("#form").validate().form()) {
            var postData = $("#form").getWeb'#id's({ Id: id });
            ys.ajax({
                url: '@Url.Content("~/OrganizationManage/News/SaveFormJson")',
                type: "post",
                data: postData,
                success: function (obj) {
                    if (obj.Tag == 1) {
                        ys.msgSuccess(obj.Message);
                        parent.refreshGrid();
                        parent.layer.close(index);
                    }
                    else {
                        ys.msgError(obj.Message);
                    }
                }
            });
        }
    }
```


## 校验

校验组件沿用jquery validation组件验证逻辑。

- 校验初始化
```javascript
    $("#form").validate({
        rules: {
            userName: { required: true },
            password: {
                required: true,
                minlength: 6,
                maxlength: 20
            },
            mobile: { isPhone: true},
            email: { email: true}
        }
    });
```

- 保存时校验

```javascript
	$("#form").validate().form()
```

## 通用方法

表格组件基于bootstrap table组件进行封装，轻松实现数据表格。

表格初始化$('#id').ysTable(options)

表格参数(Table options)

参数   |  类型  |  默认值  |  描述
-      |-      |-        |-
url    |  String | Null   | 请求后台的Url