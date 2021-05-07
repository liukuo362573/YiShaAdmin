using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.SS.Util;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection;
using YiSha.Util.Extension;
using YiSha.Util.Model;

namespace YiSha.Util.Helper
{
    /// <summary>
    /// List导出到Excel文件
    /// </summary>
    public static class ExcelHelper
    {
        #region List导出到Excel文件

        /// <summary>
        /// List导出到Excel文件
        /// </summary>
        public static string ExportToExcel<T>(string sFileName, string sHeaderText, List<T> list, string[] columns = null) where T : new()
        {
            if (list == null) throw new ArgumentNullException(nameof(list));
            sFileName = $"{SecurityHelper.GetGuid()}_{sFileName}";
            var sRoot = GlobalContext.HostingEnvironment.ContentRootPath;
            var partDirectory = $"Resource{Path.DirectorySeparatorChar}Export{Path.DirectorySeparatorChar}Excel";
            var sDirectory = Path.Combine(sRoot, partDirectory);
            var sFilePath = Path.Combine(sDirectory, sFileName);
            if (!Directory.Exists(sDirectory))
            {
                Directory.CreateDirectory(sDirectory);
            }
            using var ms = CreateExportMemoryStream(list, sHeaderText, columns);
            using var fs = new FileStream(sFilePath, FileMode.Create, FileAccess.Write);
            fs.Write(ms.ToArray(), 0, ms.ToArray().Length);
            return partDirectory + Path.DirectorySeparatorChar + sFileName;
        }

        /// <summary>
        /// List导出到Excel的MemoryStream
        /// </summary>
        /// <param name="list">数据源</param>
        /// <param name="sHeaderText">表头文本</param>
        /// <param name="columns">需要导出的属性</param>
        private static MemoryStream CreateExportMemoryStream<T>(IReadOnlyList<T> list, string sHeaderText, string[] columns = null) where T : new()
        {
            var workbook = new HSSFWorkbook();
            var sheet = workbook.CreateSheet();

            var properties = ReflectionHelper.GetProperties(typeof(T), columns);
            if (columns == null)
            {
                properties = properties.Where(it => it.GetCustomAttributes(true).OfType<DescriptionAttribute>().Any()).ToArray();
            }

            var dateStyle = workbook.CreateCellStyle();
            var format = workbook.CreateDataFormat();
            dateStyle.DataFormat = format.GetFormat("yyyy-MM-dd");

            #region 取得每列的列宽（最大宽度）

            var arrColWidth = new int[properties.Length];
            for (var columnIndex = 0; columnIndex < properties.Length; columnIndex++)
            {
                //GBK对应的code page是CP936
                arrColWidth[columnIndex] = properties[columnIndex].Name.Length;
            }

            #endregion

            for (var rowIndex = 0; rowIndex < list.Count; rowIndex++)
            {
                #region 新建表，填充表头，填充列头，样式

                if (rowIndex is 65535 or 0)
                {
                    if (rowIndex != 0)
                    {
                        sheet = workbook.CreateSheet();
                    }

                    #region 表头及样式

                    {
                        var headerRow = sheet.CreateRow(0);
                        headerRow.HeightInPoints = 25;
                        headerRow.CreateCell(0).SetCellValue(sHeaderText);

                        var headStyle = workbook.CreateCellStyle();
                        headStyle.Alignment = HorizontalAlignment.Center;
                        var font = workbook.CreateFont();
                        font.FontHeightInPoints = 20;
                        font.Boldweight = 700;
                        headStyle.SetFont(font);

                        headerRow.GetCell(0).CellStyle = headStyle;

                        sheet.AddMergedRegion(new CellRangeAddress(0, 0, 0, properties.Length - 1));
                    }

                    #endregion

                    #region 列头及样式

                    {
                        var headerRow = sheet.CreateRow(1);
                        var headStyle = workbook.CreateCellStyle();
                        headStyle.Alignment = HorizontalAlignment.Center;
                        var font = workbook.CreateFont();
                        font.FontHeightInPoints = 10;
                        font.Boldweight = 700;
                        headStyle.SetFont(font);

                        for (var columnIndex = 0; columnIndex < properties.Length; columnIndex++)
                        {
                            // 类属性如果有Description就用Description当做列名
                            var customAttribute = (DescriptionAttribute)Attribute.GetCustomAttribute(properties[columnIndex], typeof(DescriptionAttribute));
                            var description = properties[columnIndex].Name;
                            if (customAttribute != null)
                            {
                                description = customAttribute.Description;
                            }
                            headerRow.CreateCell(columnIndex).SetCellValue(description);
                            headerRow.GetCell(columnIndex).CellStyle = headStyle;

                            //设置列宽
                            sheet.SetColumnWidth(columnIndex, (arrColWidth[columnIndex] + 1) * 256);
                        }
                    }

                    #endregion
                }

                #endregion

                #region 填充内容

                var contentStyle = workbook.CreateCellStyle();
                contentStyle.Alignment = HorizontalAlignment.Left;
                var dataRow = sheet.CreateRow(rowIndex + 2); // 前面2行已被占用
                for (var columnIndex = 0; columnIndex < properties.Length; columnIndex++)
                {
                    var newCell = dataRow.CreateCell(columnIndex);
                    newCell.CellStyle = contentStyle;

                    var drValue = properties[columnIndex].GetValue(list[rowIndex], null).ParseToString();
                    switch (properties[columnIndex].PropertyType.ToString())
                    {
                        case "System.String":
                            newCell.SetCellValue(drValue);
                            break;

                        case "System.DateTime":
                        case "System.Nullable`1[System.DateTime]":
                            newCell.SetCellValue(drValue.ParseToDateTime());
                            newCell.CellStyle = dateStyle; //格式化显示
                            break;

                        case "System.Boolean":
                        case "System.Nullable`1[System.Boolean]":
                            newCell.SetCellValue(drValue.ParseToBool());
                            break;

                        case "System.Byte":
                        case "System.Nullable`1[System.Byte]":
                        case "System.Int16":
                        case "System.Nullable`1[System.Int16]":
                        case "System.Int32":
                        case "System.Nullable`1[System.Int32]":
                            newCell.SetCellValue(drValue.ParseToInt());
                            break;

                        case "System.Int64":
                        case "System.Nullable`1[System.Int64]":
                            newCell.SetCellValue(drValue.ParseToString());
                            break;

                        case "System.Double":
                        case "System.Nullable`1[System.Double]":
                            newCell.SetCellValue(drValue.ParseToDouble());
                            break;

                        case "System.Decimal":
                        case "System.Nullable`1[System.Decimal]":
                            newCell.SetCellValue(drValue.ParseToDouble());
                            break;

                        case "System.DBNull":
                            newCell.SetCellValue(string.Empty);
                            break;

                        default:
                            newCell.SetCellValue(string.Empty);
                            break;
                    }
                }

                #endregion
            }

            using var ms = new MemoryStream();
            workbook.Write(ms);
            workbook.Close();
            ms.Flush();
            ms.Position = 0;
            return ms;
        }

        #endregion

        #region Excel导入

        /// <summary>
        /// Excel导入
        /// </summary>
        public static List<T> ImportFromExcel<T>(string filePath, int fieldRow = 1) where T : new()
        {
            if (filePath == null) throw new ArgumentNullException(nameof(filePath));
            var absoluteFilePath = GlobalContext.HostingEnvironment.ContentRootPath + filePath.Replace(Path.AltDirectorySeparatorChar, Path.DirectorySeparatorChar);
            var list = new List<T>();
            HSSFWorkbook hssfWorkbook = null;
            XSSFWorkbook xssWorkbook = null;
            ISheet sheet;
            using (var file = new FileStream(absoluteFilePath, FileMode.Open, FileAccess.Read))
            {
                switch (Path.GetExtension(filePath))
                {
                    case ".xls":
                        hssfWorkbook = new HSSFWorkbook(file);
                        sheet = hssfWorkbook.GetSheetAt(0);
                        break;

                    case ".xlsx":
                        xssWorkbook = new XSSFWorkbook(file);
                        sheet = xssWorkbook.GetSheetAt(0);
                        break;

                    default: throw new Exception("不支持的文件格式");
                }
            }
            var columnRow = sheet.GetRow(fieldRow);
            var mapPropertyInfoDict = new Dictionary<int, PropertyInfo>();
            for (var j = 0; j < columnRow.LastCellNum; j++)
            {
                var cell = columnRow.GetCell(j);
                var propertyInfo = MapPropertyInfo<T>(cell.ParseToString());
                if (propertyInfo != null)
                {
                    mapPropertyInfoDict.Add(j, propertyInfo);
                }
            }

            for (var i = sheet.FirstRowNum + 2; i <= sheet.LastRowNum; i++)
            {
                var row = sheet.GetRow(i);
                var entity = new T();
                for (int j = row.FirstCellNum; j < columnRow.LastCellNum; j++)
                {
                    if (!mapPropertyInfoDict.ContainsKey(j) || row.GetCell(j) == null)
                    {
                        continue;
                    }
                    var propertyInfo = mapPropertyInfoDict[j];
                    switch (propertyInfo.PropertyType.ToString())
                    {
                        case "System.DateTime":
                        case "System.Nullable`1[System.DateTime]":
                            mapPropertyInfoDict[j].SetValue(entity, row.GetCell(j).ParseToString().ParseToDateTime());
                            break;

                        case "System.Boolean":
                        case "System.Nullable`1[System.Boolean]":
                            mapPropertyInfoDict[j].SetValue(entity, row.GetCell(j).ParseToString().ParseToBool());
                            break;

                        case "System.Byte":
                        case "System.Nullable`1[System.Byte]":
                            mapPropertyInfoDict[j].SetValue(entity, byte.Parse(row.GetCell(j).ParseToString()));
                            break;

                        case "System.Int16":
                        case "System.Nullable`1[System.Int16]":
                            mapPropertyInfoDict[j].SetValue(entity, short.Parse(row.GetCell(j).ParseToString()));
                            break;

                        case "System.Int32":
                        case "System.Nullable`1[System.Int32]":
                            mapPropertyInfoDict[j].SetValue(entity, row.GetCell(j).ParseToString().ParseToInt());
                            break;

                        case "System.Int64":
                        case "System.Nullable`1[System.Int64]":
                            mapPropertyInfoDict[j].SetValue(entity, row.GetCell(j).ParseToString().ParseToLong());
                            break;

                        case "System.Double":
                        case "System.Nullable`1[System.Double]":
                            mapPropertyInfoDict[j].SetValue(entity, row.GetCell(j).ParseToString().ParseToDouble());
                            break;

                        case "System.Decimal":
                        case "System.Nullable`1[System.Decimal]":
                            mapPropertyInfoDict[j].SetValue(entity, row.GetCell(j).ParseToString().ParseToDecimal());
                            break;

                        case "System.String":
                            mapPropertyInfoDict[j].SetValue(entity, row.GetCell(j).ParseToString());
                            break;

                        default:
                            mapPropertyInfoDict[j].SetValue(entity, row.GetCell(j).ParseToString());
                            break;
                    }
                }
                list.Add(entity);
            }
            hssfWorkbook?.Close();
            xssWorkbook?.Close();
            return list;
        }

        /// <summary>
        /// 查找Excel列名对应的实体属性
        /// </summary>
        private static PropertyInfo MapPropertyInfo<T>(string columnName) where T : new()
        {
            var propertyList = ReflectionHelper.GetProperties(typeof(T));
            var propertyInfo = propertyList.FirstOrDefault(p => p.Name == columnName);
            if (propertyInfo != null)
            {
                return propertyInfo;
            }
            return (
                from it in propertyList
                let attributes = it.GetCustomAttributes<DescriptionAttribute>(false).ToArray()
                where attributes.Length > 0 && attributes[0].Description == columnName
                select it
            ).FirstOrDefault();
        }

        #endregion
    }
}