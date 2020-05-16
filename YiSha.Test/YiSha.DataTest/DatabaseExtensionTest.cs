using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
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
        public async Task TestSort()
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
        public async Task TestMultiSort()
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
    }
}
