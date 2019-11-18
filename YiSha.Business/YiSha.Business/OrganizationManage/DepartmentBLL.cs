using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YiSha.Entity.OrganizationManage;
using YiSha.Enum.OrganizationManage;
using YiSha.Model;
using YiSha.Model.Result;
using YiSha.Model.Param.OrganizationManage;
using YiSha.Service.OrganizationManage;
using YiSha.Util;
using YiSha.Util.Model;
using YiSha.Util.Extension;

namespace YiSha.Business.OrganizationManage
{
    public class DepartmentBLL
    {
        private DepartmentService departmentService = new DepartmentService();
        private UserService userService = new UserService();
        private UserBelongService userBelongService = new UserBelongService();

        #region 获取数据
        public async Task<TData<List<DepartmentEntity>>> GetList(DepartmentListParam param)
        {
            TData<List<DepartmentEntity>> obj = new TData<List<DepartmentEntity>>();
            obj.Result = await departmentService.GetList(param);
            List<UserEntity> userList = await userService.GetList(new UserListParam { UserIds = string.Join(",", obj.Result.Select(p => p.PrincipalId).ToArray()) });
            foreach (DepartmentEntity entity in obj.Result)
            {
                if (entity.PrincipalId > 0)
                {
                    entity.PrincipalName = userList.Where(p => p.Id == entity.PrincipalId).Select(p => p.RealName).FirstOrDefault();
                }
                else
                {
                    entity.PrincipalName = string.Empty;
                }
            }
            obj.Tag = 1;
            return obj;
        }

        public async Task<TData<List<ZtreeInfo>>> GetZtreeDepartmentList(DepartmentListParam param)
        {
            var obj = new TData<List<ZtreeInfo>>();
            obj.Result = new List<ZtreeInfo>();
            List<DepartmentEntity> list = await departmentService.GetList(param);
            foreach (DepartmentEntity department in list)
            {
                obj.Result.Add(new ZtreeInfo
                {
                    id = department.Id,
                    pId = department.ParentId,
                    name = department.DepartmentName
                });
            }
            obj.Tag = 1;
            return obj;
        }

        public async Task<TData<List<ZtreeInfo>>> GetZtreeUserList(DepartmentListParam param)
        {
            var obj = new TData<List<ZtreeInfo>>();
            obj.Result = new List<ZtreeInfo>();
            List<UserEntity> userList = await userService.GetList(null);
            List<DepartmentEntity> departmentList = await departmentService.GetList(param);
            foreach (DepartmentEntity department in departmentList)
            {
                obj.Result.Add(new ZtreeInfo
                {
                    id = department.Id,
                    pId = department.ParentId,
                    name = department.DepartmentName
                });
                List<long> userIdList = userList.Where(t => t.DepartmentId == department.Id).Select(t => t.Id.Value).ToList();
                foreach (UserEntity user in userList.Where(t => userIdList.Contains(t.Id.Value)))
                {
                    obj.Result.Add(new ZtreeInfo
                    {
                        id = user.Id,
                        pId = department.Id,
                        name = user.RealName
                    });
                }
            }
            obj.Tag = 1;
            return obj;
        }

        public async Task<TData<DepartmentEntity>> GetEntity(long id)
        {
            TData<DepartmentEntity> obj = new TData<DepartmentEntity>();
            obj.Result = await departmentService.GetEntity(id);
            obj.Tag = 1;
            return obj;
        }

        public async Task<TData<int>> GetMaxSort()
        {
            TData<int> obj = new TData<int>();
            obj.Result = await departmentService.GetMaxSort();
            obj.Tag = 1;
            return obj;
        }

        public async Task<List<long>> GetDepartmentIdList(long departmentId)
        {
            List<DepartmentEntity> departmentList = await departmentService.GetList(null);
            List<long> departmentIdList = new List<long>();
            departmentIdList.Add(departmentId);
            GetDepartmentIdList(departmentList, departmentId, departmentIdList);
            return departmentIdList;
        }
        #endregion

        #region 提交数据
        public async Task<TData<string>> SaveForm(DepartmentEntity entity)
        {
            TData<string> obj = new TData<string>();
            if (departmentService.ExistDepartmentName(entity))
            {
                obj.Message = "部门名称已经存在！";
                return obj;
            }
            await departmentService.SaveForm(entity);
            obj.Result = entity.Id.ParseToString();
            obj.Tag = 1;
            return obj;
        }

        public async Task<TData> DeleteForm(string ids)
        {
            TData obj = new TData();
            foreach (long id in TextHelper.SplitToArray<long>(ids, ','))
            {
                if (departmentService.ExistChildrenDepartment(id))
                {
                    obj.Message = "该部门下面有子部门！";
                    return obj;
                }
            }
            await departmentService.DeleteForm(ids);
            obj.Tag = 1;
            return obj;
        }
        #endregion

        #region 私有方法
        private void GetDepartmentIdList(List<DepartmentEntity> departmentList, long departmentId, List<long> departmentIdList)
        {
            var children = departmentList.Where(p => p.ParentId == departmentId).Select(p => p.Id.Value).ToList();
            if (children.Count > 0)
            {
                departmentIdList.AddRange(children);
                foreach (long id in children)
                {
                    GetDepartmentIdList(departmentList, id, departmentIdList);
                }
            }
        }
        #endregion
    }
}
