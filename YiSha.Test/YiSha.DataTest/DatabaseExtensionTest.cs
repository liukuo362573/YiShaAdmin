using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YiSha.Data.Extension;
using YiSha.Data.Repository;
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
            RoleService roleService = new RoleService();
            RoleListParam roleListParam = new RoleListParam { };
            Pagination pagination = new Pagination
            {
                Sort = "RoleSort asc"
            };
            List<RoleEntity> list = await roleService.GetPageList(roleListParam, pagination);
            Assert.IsTrue(list[0].RoleSort < list[1].RoleSort);
        }

        /// <summary>
        /// 测试多字段排序
        /// </summary>
        /// <returns></returns>
        [Test]
        public async Task MultiSortTest()
        {
            RoleService roleService = new RoleService();
            RoleListParam roleListParam = new RoleListParam { };
            Pagination pagination = new Pagination
            {
                Sort = "Id desc,RoleSort asc"
            };
            List<RoleEntity> list = await roleService.GetPageList(roleListParam, pagination);
            Assert.IsTrue(list[0].Id > list[1].Id);
        }

        /// <summary>
        /// 测试如何获取Linq查询时对应的Sql
        /// </summary>
        /// <returns></returns>
        [Test]
        public async Task LinqGetSqlTest()
        {
            string sort = "RoleSort";
            bool isAsc = false;
            int pageSize = 10;
            int pageIndex = 1;
            RepositoryFactory repositoryFactory = new RepositoryFactory();
            var tempData = repositoryFactory.BaseRepository().Db.DbContext.Set<RoleEntity>().AsQueryable();
            tempData = tempData.AppendSort(sort, isAsc);
            tempData = tempData.Skip(pageSize * (pageIndex - 1)).Take(pageSize).AsQueryable();
            string strSql = tempData.GetSql();
            Assert.IsTrue(strSql.ToUpper().Contains("SELECT"));
        }
    }
}