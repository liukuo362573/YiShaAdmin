using Microsoft.EntityFrameworkCore;

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
