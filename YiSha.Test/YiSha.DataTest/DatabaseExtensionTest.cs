using NUnit.Framework;
using System;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YiSha.Data.Extension;
using YiSha.Data.Repository;
using YiSha.Entity.SystemManage;
using YiSha.Model.Param.SystemManage;
using YiSha.Service.SystemManage;
using YiSha.Util.Extension;
using YiSha.Util.Model;

namespace YiSha.DataTest
{
    public class DatabaseExtensionTest
    {
        /// <summary>
        /// 测试单字段排序
        /// </summary>
        [Test]
        public async Task SortTest()
        {
            var roleService = new RoleService();
            var roleListParam = new RoleListParam();
            var pagination = new Pagination
            {
                Sort = "RoleSort asc"
            };
            var list = await roleService.GetPageList(roleListParam, pagination);
            Assert.IsTrue(list[0].RoleSort < list[1].RoleSort);
        }

        /// <summary>
        /// 测试多字段排序
        /// </summary>
        [Test]
        public async Task MultiSortTest()
        {
            var roleService = new RoleService();
            var roleListParam = new RoleListParam();
            var pagination = new Pagination
            {
                Sort = "Id desc,RoleSort asc"
            };
            var list = await roleService.GetPageList(roleListParam, pagination);
            Assert.IsTrue(list[0].Id > list[1].Id);
        }

        /// <summary>
        /// 测试如何获取Linq查询时对应的Sql
        /// </summary>
        [Test]
        public void LinqGetSqlTest()
        {
            var sort = "RoleSort";
            var isAsc = false;
            var pageSize = 10;
            var pageIndex = 1;
            var repositoryFactory = new RepositoryFactory();
            var tempData = repositoryFactory.BaseRepository().Db.DbContext.Set<RoleEntity>().AsQueryable();
            tempData = tempData.AppendSort(sort, isAsc);
            tempData = tempData.Skip(pageSize * (pageIndex - 1)).Take(pageSize).AsQueryable();
            var sql = tempData.GetSql();
            Assert.IsTrue(sql.ToUpper().Contains("SELECT"));
        }

        [Test]
        public void IsElementryTypeTest()
        {
            Assert.IsTrue(typeof(string).IsElementaryType());
            Assert.IsTrue(typeof(int).IsElementaryType());
            Assert.IsTrue(typeof(decimal).IsElementaryType());
            Assert.IsTrue(typeof(float).IsElementaryType());
            Assert.IsTrue(typeof(StringComparison).IsElementaryType()); // enum
            Assert.IsTrue(typeof(int?).IsElementaryType());
            Assert.IsTrue(typeof(decimal?).IsElementaryType());
            Assert.IsTrue(typeof(StringComparison?).IsElementaryType());
            Assert.IsFalse(typeof(object).IsElementaryType());
            Assert.IsFalse(typeof(Point).IsElementaryType()); // struct in System.Drawing
            Assert.IsFalse(typeof(Point?).IsElementaryType());
            Assert.IsFalse(typeof(StringBuilder).IsElementaryType()); // reference type
        }

        [Test]
        public async Task GetElementryTypeValueTest()
        {
            var repositoryFactory = new RepositoryFactory();
            var sql = @"select UserName from SysUser where Id = @Id";
            var result = await repositoryFactory.BaseRepository().FindEntity<string>(sql, DbParameterHelper.CreateDbParameter("@Id", 16508640061130151));
            Assert.IsTrue(result == "admin");
        }

        [Test]
        public async Task GetElementryTypeValueWhenIsDbNullTest()
        {
            var repositoryFactory = new RepositoryFactory();
            var sql = @"select UserName from SysUser where Id = @Id";
            var result = await repositoryFactory.BaseRepository().FindEntity<string>(sql, DbParameterHelper.CreateDbParameter("@Id", 345));
            Assert.IsTrue(result == default);
        }

        [Test]
        public async Task GetDynamicValueTest()
        {
            var repositoryFactory = new RepositoryFactory();
            var sql = @"select UserName from SysUser where Id = @Id";
            var result = await repositoryFactory.BaseRepository().FindEntity<dynamic>(sql, DbParameterHelper.CreateDbParameter("@Id", 16508640061130151));
            Assert.IsTrue(result.UserName == "admin");
        }
    }
}