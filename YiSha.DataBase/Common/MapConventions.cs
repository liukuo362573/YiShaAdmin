using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace YiSha.DataBase.Common
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
}
