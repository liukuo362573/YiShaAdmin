# Excel Plus

## 它是什么?

`excel-plus` 是基于 [Apache POI](https://poi.apache.org/) 框架的一款扩展封装小库，让我们在开发中更快速的完成导入导出的需求。
尽管很多人会提出 `poi` 能干这事儿为什么还要封装一层呢？

`excel-plus`很大程度上简化了代码、让使用者更轻松的
读、写 Excel 文档，也不用去关心格式兼容等问题，很多时候我们在代码中会写很多的 `for` 循环，各种 `getXXXIndex`
来获取行或列让代码变的更臃肿。多个项目之间打一枪换一个地方，代码 Copy 来 Copy 去十分凌乱，
如果你也在开发中遇到类似的问题，那么 `excel-plus` 是你值得一试的工具。

## 不是什么

`excel-plus` 不是万能的，比如你想合并某几列，或者让第三行的某一列设置样式或特殊格式，
很抱歉它是做不到的，因为这让事情复杂化了，即便支持也会像原始的 POI API 一样让人痛恶。
如果真的需要，你可能需要在网络上寻找一些 `Utils` 结尾的工具类自行编写了，祝你好运 :P

> 如果你在使用过程中遇到什么问题或者建议可以发一个 [issue](https://github.com/biezhi/excel-plus/issues/new) 告诉我

## 特性

- 基于 Java 8 开发
- 简洁的 API 操作
- 注解驱动
- 可配置列顺序
- 支持按模板导出
- 支持过滤行数据
- 支持校验行数据
- 支持数据类型转换
- 支持自定义列样式
- 支持一行代码下载 Excel 文件
- 支持 Excel 2003、2007、CSV 格式

# 快速开始

## 引入依赖

加入以下 `maven` 依赖到你的 `pom.xml` 文件中，该项目使用的 `poi` 版本是 **3.17**，
如果你的项目已经存在，请注意删除或者排除依赖。

```xml
<dependency>
    <groupId>io.github.biezhi</groupId>
    <artifactId>excel-plus</artifactId>
    <version>0.1.6.RELEASE</version>
</dependency>
```

> **注意**：这里的版本号请使用 `maven` 仓库较新版本，可在 Github 的 README 中看到。

## 导入导出

下面是我们的 Java 模型类，用于存储 Excel 的行数据。

```java
// 卡密 Model
public class CardSecret {

    @ExcelField(order = 0, columnName = "运营商类型", 
                convertType = CardTypeConverter.class)
    private Integer cardType;

    @ExcelField(order = 1, columnName = "卡密")
    private String secret;

    @ExcelField(order = 2, columnName = "面额")
    private BigDecimal amount;

    @ExcelField(order = 3, columnName = "过期时间", datePattern = "yyyy年MM月dd日")
    private Date expiredDate;
    
    // 可跳过索引为 4 的列
    @ExcelField(order = 5, columnName = "使用情况", convertType = UsedTypeConverter.class)
    private Boolean used;

    // getter setter 省略
}
```

这里的 `cardType` 是数据库中存储的运营商类型，1对应的是**移动**，其他对应的是**联通**。
这时候我们需要编写一个转换器将数字类型的结果转换为Excel中语义化的中文。

```java
// 运营商类型转换器
public class CardTypeConverter implements Converter<Integer> {

    @Override
    public String write(Integer value) {
        return value.equals(1) ? "联通" : "移动";
    }

    @Override
    public Integer read(String value) {
        return value.equals("联通") ? 1 : 2;
    }
}
```

使用 `ExcelPlus` 导出卡密列表。

```java
ExcelPlus excelPlus = new ExcelPlus();
List<CardSecret> cardSecrets = new ArrayList<>();
cardSecrets.add(new CardSecret(1, "vlfdzepjmlz2y43z7er4", new BigDecimal("20"), false));
cardSecrets.add(new CardSecret(2, "rasefq2rzotsmx526z6g", new BigDecimal("10"), false));
cardSecrets.add(new CardSecret(2, "2ru44qut6neykb2380wt", new BigDecimal("50"), true));
cardSecrets.add(new CardSecret(1, "srcb4c9fdqzuykd6q4zl", new BigDecimal("15"), false));

excelPlus.export(cardSecrets).writeAsFile(new File("卡密列表.xlsx"));
```

这样就完成了一个列表数据导出为 Excel 的例子，通常我们的数据都是从数据库查询出来的。
下面这个例子是从Excel 中读取数据到 Java List 容器，如果你想存储在 Set 里我相信你可以做到。

```java
Reader reader = Reader.create().excelFile(new File("卡密列表.xlsx"));
List<CardSecret> cardList = excelPlus.read(CardSecret.class, reader).asList();
```

没错，就是这么简单！如果有更加复杂或自定义的需求可以看下面的进阶使用。

# 进阶使用

## 读取过滤

有时候我们需要对读取的行数据做一下过滤，这时候就可以使用 `filter` 函数来筛选出合适的数据项。

```java
excelPlus.read(CardSecret.class, reader)
         .filter(cardSecret -> !cardSecret.getSecret().isEmpty())
         .asList();
```

## 读取校验

有一种场景是当 Excel 中的某一行或者几行数据不满足条件时候，我们记录下这些异常数据，并提示给调用方（比如 Web 浏览器）。
下面这个示例校验每行数据中的 `amount` 是否 `< 20`，如果满足则返回一个校验失败的错误信息，然后我们将错误内容输出到控制台，
实际工作中你可能将它们交由前端处理。

```java
ReaderResult<CardSecret> result = excelPlus.read(new File("卡密列表.xls"), CardSecret.class)
         .valid((index, cardSecret) -> {
            if(!cardSecret.getUsed()){
                return ValidRow.ok();
            }
            return ValidRow.fail("已经被使用");
         });

if (!result.isValid()) {
    result.errors().forEach(System.out::println);
} else {
    System.out.println(result.asList().size());
}
```

> 当然你可以将 `valid` 校验代码块封装一下看起来更流畅 :P

## 导出样式

大多数情况下我们是无需设置样式的，在 `excel-plus` 中提供了设置表头和列的样式 API。
在某些需求下可能需要设置字体大小、颜色、居中等，你可以像下面的代码这样干。
如果你对样式的操作不熟悉可以参考 POI 的列设置[文档](https://poi.apache.org/spreadsheet/quick-guide.html#Creating+Date+Cells)。

```java
// 构建数据
List<CardSecret> cardSecrets = this.buildCardSecrets();

Exporter<CardSecret> exporter = Exporter.create(cardSecrets);
exporter.headerStyle(workbook -> {
    CellStyle headerStyle = workbook.createCellStyle();
    headerStyle.setAlignment(HorizontalAlignment.LEFT);

    headerStyle.setFillForegroundColor(HSSFColor.HSSFColorPredefined.WHITE.getIndex());
    headerStyle.setFillPattern(FillPatternType.SOLID_FOREGROUND);

    Font headerFont = workbook.createFont();
    headerFont.setFontHeightInPoints((short) 12);
    headerFont.setBold(true);
    headerStyle.setFont(headerFont);
    return headerStyle;
});

excelPlus.export(exporter)
                .writeAsFile(new File("卡密列表.xlsx"));
```

## 浏览器下载

为了方便我们将数据库查询的数据直接输出到浏览器弹出下载，`excel-plus` 也做了一点 _手脚_ 让你一行代码就可以搞定。

```java
excelPlus.export(exporter)
         .writeAsResponse(ResponseWrapper.create(servletResponse, "xxx表格.xls"))
```

只需要将 `HttpServletResponse` 对象传入，并输入导出的文件名称，其他的都见鬼去吧。

## 模板导出

有时候我们需要导出的 Excel 表格样式比较复杂，可以事先设置好一个模板表格，数据为空，
由程序向模板中填入数据，然后导出即可，这样就满足了美观的需求。

```java
List<CardSecret> cardSecrets = this.buildCardSecrets();
excelPlus.export(Exporter.create(cardSecrets).byTemplate("tpl.xls")).writeAsFile(new File("template_rows.xls"));
```

> 需要注意的是这里的 `tpl.xls` 位于 `classpath` 路径下。

# API 介绍

## 核心对象

- `ExcelPlus`: 用于操作读取或导出 Excel 文档的类
- `Converter`: 数据类型转换的顶层接口
- `ReaderResult`: 存储读取到的列表，包含校验不通过的消息
- `Exporter`: 用于存储导出 Excel 文档时的配置，如样式、模板位置等

## 注解使用

该项目中有 4 个注解，分别是 `ExcelField`、`ExcelSheet`、`ReadField`、`WriteField`。
正常情况下你只会用到第一个注解，下面解释一下 `@ExcelField`。

<b>@ExcelField 注解</b>

| 选项        | 默认值               | 描述                                                               |
|-------------|----------------------|--------------------------------------------------------------------|
| `order`       | -1                   | 用于标识 Excel 中的列索引，从 0 开始，该选项适用于读取或写入 Excel    |
| `columnName`  | 必选                 | 导出Excel时的列名称，如：状态、姓名、手机号                        |
| `datePattern` | 空                   | 日期格式化的 `pattern`，对 `Date`、`LocalDate`、`LocalDateTime` 生效        |
| `convertType` | `EmptyConverter.class` | 数据类型转换的类 Class，实现自 Converter 接口，实现类需有无参构造函数 |

> `@ReadField` 和 `@WriteField` 是针对读取和写入的顺序不一致、日期格式不一致时的覆盖型注解，一般用不到。

<b>@ExcelSheet 注解</b>

用于标识导出的工作表名称，默认是 `Sheet0`，无特殊需求用不到。

# 常见问题

等你有了我就写上来行不？

在使用过程中遇到什么问题或者建议可以发一个 [issue](https://github.com/biezhi/excel-plus/issues/new)

# 版本更新

<b>v0.1.2</b>

1. 导出数字支持
2. 修复日期格式化 bug
3. 修复列索引错误

<b>v0.1.1</b>

1. 优化读取性能
2. 重构 POI 读取方式

<b>v0.0.4</b>

1. 添加 `sheetName` 方法在运行时读取工作表
2. 添加 `sheetIndex` 方法在运行时读取工作表

<b>v0.0.3</b>

1. 修复自定义 `SheetName` 读取失败

<b>v0.0.2</b>

1. 添加验证行记录各项指标
2. 区分验证成功和失败行
3. 可配置是否添加到验证成功行
4. 取消 `columnName` 选项为必选

<b>v0.0.1</b>

- 发布第一个版本
