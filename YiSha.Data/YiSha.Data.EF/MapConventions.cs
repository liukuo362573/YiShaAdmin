using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace YiSha.Data.EF
{
    /// <summary>
    /// 主键约定，把属性Id当做数据库主键
    /// </summary>
    public class PrimaryKeyConvention
    {
        public static void SetPrimaryKey(ModelBuilder modelBuilder, string entityName)
        {
            modelBuilder.Entity(entityName).HasKey("Id");
        }
    }

    /// <summary>
    /// 列名约定，比如属性ParentId，映射到数据库字段parent_id
    /// </summary>
    public class ColumnConvention
    {
        public static void SetColumnName(ModelBuilder modelBuilder, string entityName, string propertyName)
        {
            StringBuilder sbField = new StringBuilder();
            char[] charArr = propertyName.ToCharArray();

            int iCapital = 0; // 把属性第一个开始的大写字母转成小写，直到遇到了第1个小写字母，因为数据库里面是小写的
            while (iCapital < charArr.Length)
            {
                if (charArr[iCapital] >= 'A' && charArr[iCapital] <= 'Z')
                {
                    charArr[iCapital] = (char)(charArr[iCapital] + 32);
                }
                else
                {
                    break;
                }
                iCapital++;
            }

            for (int i = 0; i < charArr.Length; i++)
            {
                if (charArr[i] >= 'A' && charArr[i] <= 'Z')
                {
                    charArr[i] = (char)(charArr[i] + 32);
                    sbField.Append("_" + charArr[i]);
                }
                else
                {
                    sbField.Append(charArr[i]);
                }
            }
            modelBuilder.Entity(entityName).Property(propertyName).HasColumnName(sbField.ToString());
        }
    }
}
