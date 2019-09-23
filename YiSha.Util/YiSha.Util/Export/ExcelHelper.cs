using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.SS.Util;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Web;
using YiSha.Util.Extension;

namespace YiSha.Util.Export
{
    /// <summary>
    /// List导出到Excel文件
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ExcelHelper<T>
    {
        #region List导出到Excel文件
        /// <summary>
        /// List导出到Excel文件
        /// </summary>
        /// <param name="sFileName"></param>
        /// <param name="sHeaderText"></param>
        /// <param name="list"></param>
        public string ExportToFile(string sFileName, string sHeaderText, List<T> list, string[] columns)
        {
            sFileName = string.Format("{0}_{1}", SecurityHelper.GetGuid(), sFileName);
            string sRoot = GlobalContext.HostingEnvironment.ContentRootPath;
            string partDirectory = string.Format("Resource{0}Export{0}Excel", Path.DirectorySeparatorChar);
            string sDirectory = Path.Combine(sRoot, partDirectory);
            string sFilePath = Path.Combine(sDirectory, sFileName);
            if (!Directory.Exists(sDirectory))
            {
                Directory.CreateDirectory(sDirectory);
            }
            using (MemoryStream ms = Export(list, sHeaderText, columns))
            {
                using (FileStream fs = new FileStream(sFilePath, FileMode.Create, FileAccess.Write))
                {
                    byte[] data = ms.ToArray();
                    fs.Write(data, 0, data.Length);
                    fs.Flush();
                }
            }
            return partDirectory + Path.DirectorySeparatorChar + sFileName;
        }
        #endregion

        #region List导出到Excel的MemoryStream  
        /// <summary>  
        /// List导出到Excel的MemoryStream  
        /// </summary>  
        /// <param name="list">数据源</param>  
        /// <param name="sHeaderText">表头文本</param>  
        /// <param name="columns">需要导出的属性</param>  
        private MemoryStream Export(List<T> list, string sHeaderText, string[] columns)
        {
            HSSFWorkbook workbook = new HSSFWorkbook();
            ISheet sheet = workbook.CreateSheet();

            Type type = typeof(T);
            PropertyInfo[] properties = ReflectionHelper.GetProperties(type, columns);

            ICellStyle dateStyle = workbook.CreateCellStyle();
            IDataFormat format = workbook.CreateDataFormat();
            dateStyle.DataFormat = format.GetFormat("yyyy-MM-dd");

            #region 取得每列的列宽（最大宽度）
            int[] arrColWidth = new int[properties.Length];
            for (int columnIndex = 0; columnIndex < properties.Length; columnIndex++)
            {
                //GBK对应的code page是CP936
                arrColWidth[columnIndex] = Encoding.GetEncoding(936).GetBytes(properties[columnIndex].Name).Length;
            }
            #endregion
            for (int rowIndex = 0; rowIndex < list.Count; rowIndex++)
            {
                #region 新建表，填充表头，填充列头，样式
                if (rowIndex == 65535 || rowIndex == 0)
                {
                    if (rowIndex != 0)
                    {
                        sheet = workbook.CreateSheet();
                    }

                    #region 表头及样式
                    {
                        IRow headerRow = sheet.CreateRow(0);
                        headerRow.HeightInPoints = 25;
                        headerRow.CreateCell(0).SetCellValue(sHeaderText);

                        ICellStyle headStyle = workbook.CreateCellStyle();
                        headStyle.Alignment = HorizontalAlignment.Center;
                        IFont font = workbook.CreateFont();
                        font.FontHeightInPoints = 20;
                        font.Boldweight = 700;
                        headStyle.SetFont(font);

                        headerRow.GetCell(0).CellStyle = headStyle;

                        sheet.AddMergedRegion(new CellRangeAddress(0, 0, 0, properties.Count() - 1));
                    }
                    #endregion

                    #region 列头及样式
                    {
                        IRow headerRow = sheet.CreateRow(1);
                        ICellStyle headStyle = workbook.CreateCellStyle();
                        headStyle.Alignment = HorizontalAlignment.Center;
                        IFont font = workbook.CreateFont();
                        font.FontHeightInPoints = 10;
                        font.Boldweight = 700;
                        headStyle.SetFont(font);

                        for (int columnIndex = 0; columnIndex < properties.Length; columnIndex++)
                        {
                            // 类属性如果有Description就用Description当做列名
                            DescriptionAttribute customAttribute = (DescriptionAttribute)Attribute.GetCustomAttribute(properties[columnIndex], typeof(DescriptionAttribute));
                            string description = properties[columnIndex].Name;
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
                ICellStyle contentStyle = workbook.CreateCellStyle();
                contentStyle.Alignment = HorizontalAlignment.Left;
                IRow dataRow = sheet.CreateRow(rowIndex + 2); // 前面2行已被占用
                for (int columnIndex = 0; columnIndex < properties.Length; columnIndex++)
                {
                    ICell newCell = dataRow.CreateCell(columnIndex);
                    newCell.CellStyle = contentStyle;

                    string drValue = properties[columnIndex].GetValue(list[rowIndex], null).ParseToString();
                    switch (properties[columnIndex].PropertyType.ToString())
                    {
                        case "System.String"://字符串类型  
                            newCell.SetCellValue(drValue);
                            break;

                        case "System.DateTime"://日期类型  
                        case "System.Nullable`1[System.DateTime]":
                            DateTime dateV;
                            DateTime.TryParse(drValue, out dateV);
                            newCell.SetCellValue(dateV);

                            newCell.CellStyle = dateStyle;//格式化显示  
                            break;
                        case "System.Boolean"://布尔型  
                        case "System.Nullable`1[System.Boolean]":
                            bool boolV = false;
                            bool.TryParse(drValue, out boolV);
                            newCell.SetCellValue(boolV);
                            break;
                        case "System.Int16"://整型  
                        case "System.Nullable`1[System.Int16]":
                        case "System.Int32":
                        case "System.Nullable`1[System.Int32]":
                        case "System.Int64":
                        case "System.Nullable`1[System.Int64]":
                        case "System.Byte":
                        case "System.Nullable`1[System.Byte]":
                            int intV = 0;
                            int.TryParse(drValue, out intV);
                            newCell.SetCellValue(intV);
                            break;
                        case "System.Decimal"://浮点型  
                        case "System.Nullable`1[System.Decimal]":
                        case "System.Double":
                        case "System.Nullable`1[System.Double]":
                            double doubV = 0;
                            double.TryParse(drValue, out doubV);
                            newCell.SetCellValue(doubV);
                            break;
                        case "System.DBNull"://空值处理  
                            newCell.SetCellValue(string.Empty);
                            break;
                        default:
                            newCell.SetCellValue(string.Empty);
                            break;
                    }
                }
                #endregion
            }

            using (MemoryStream ms = new MemoryStream())
            {
                workbook.Write(ms);
                ms.Flush();
                ms.Position = 0;
                return ms;
            }
        }
        #endregion

        #region Excel导入到DataTable
        /// <summary>
        /// Excel导入到DataTable
        /// </summary>
        /// <param name="sFileName"></param>
        /// <returns></returns>
        public DataTable Import(string sFileName)
        {
            DataTable dt = new DataTable();

            HSSFWorkbook hssfworkbook;
            using (FileStream file = new FileStream(sFileName, FileMode.Open, FileAccess.Read))
            {
                hssfworkbook = new HSSFWorkbook(file);
            }
            ISheet sheet = hssfworkbook.GetSheetAt(0);
            System.Collections.IEnumerator rows = sheet.GetRowEnumerator();

            IRow headerRow = sheet.GetRow(0);
            int cellCount = headerRow.LastCellNum;

            for (int j = 0; j < cellCount; j++)
            {
                ICell cell = headerRow.GetCell(j);
                dt.Columns.Add(cell.ToString());
            }

            for (int i = (sheet.FirstRowNum + 1); i <= sheet.LastRowNum; i++)
            {
                IRow row = sheet.GetRow(i);
                DataRow dataRow = dt.NewRow();

                for (int j = row.FirstCellNum; j < cellCount; j++)
                {
                    if (row.GetCell(j) != null)
                    {
                        dataRow[j] = row.GetCell(j).ToString();
                    }
                }
                dt.Rows.Add(dataRow);
            }
            return dt;
        }
        #endregion
    }
}
