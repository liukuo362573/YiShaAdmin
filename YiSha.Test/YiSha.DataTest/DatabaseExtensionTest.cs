using NUnit.Framework;
using YiSha.DataBase;
using YiSha.DataBase.Extension;
using YiSha.Entity.SystemManage;
using YiSha.Model.Param.SystemManage;
using YiSha.Service.SystemManage;
using YiSha.Util.Model;

namespace YiSha.DataTest
{
    public class DatabaseExtensionTest
    {
        /// <summary>
        /// 测试单字段排序
        /// </summary>
        /// <returns></returns>
        [Test]
        public async Task SortTest()
        {
            var roleService = new RoleService();
            var roleListParam = new RoleListParam { };
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
        /// <returns></returns>
        [Test]
        public async Task MultiSortTest()
        {
            var roleService = new RoleService();
            var roleListParam = new RoleListParam { };
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
        /// <returns></returns>
        [Test]
        public async Task LinqGetSqlTest()
        {
            var sort = "RoleSort";
            var isAsc = false;
            var pageSize = 10;
            var pageIndex = 1;
            var repository = new Repository();
            var tempData = repository.dbContext.Set<RoleEntity>().AsQueryable();
            tempData = DbExtension.AppendSort<RoleEntity>(tempData, sort, isAsc);
            tempData = tempData.Skip<RoleEntity>(pageSize * (pageIndex - 1)).Take<RoleEntity>(pageSize).AsQueryable();
            var strSql = DbExtension.GetSql<RoleEntity>(tempData);
            Assert.IsTrue(strSql.ToUpper().Contains("SELECT"));
        }
    }
}
