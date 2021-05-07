using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YiSha.Entity.OrganizationManage;
using YiSha.Model.Param.OrganizationManage;
using YiSha.Model.Result;
using YiSha.Service.OrganizationManage;
using YiSha.Util.Extension;
using YiSha.Util.Helper;
using YiSha.Util.Model;
using YiSha.Web.Code;

namespace YiSha.Business.OrganizationManage
{
    public class DepartmentBLL
    {
        private readonly DepartmentService _departmentService = new();

        private readonly UserService _userService = new();

        #region 获取数据

        public async Task<TData<List<DepartmentEntity>>> GetList(DepartmentListParam param)
        {
            var list = await _departmentService.GetList(param);
            var operatorInfo = await Operator.Instance.Current();
            if (operatorInfo.IsSystem != 1)
            {
                var childrenDepartmentIdList = await GetChildrenDepartmentIdList(list, operatorInfo.DepartmentId!.Value);
                list = list.Where(p => childrenDepartmentIdList.Contains(p.Id!.Value)).ToList();
            }
            var userList = await _userService.GetList(new UserListParam { UserIds = string.Join(",", list.Select(p => p.PrincipalId).ToArray()) });
            foreach (var entity in list)
            {
                entity.PrincipalName = entity.PrincipalId > 0 ? userList.Where(p => p.Id == entity.PrincipalId).Select(p => p.RealName).FirstOrDefault() : string.Empty;
            }
            return new() { Data = list, Tag = 1 };
        }

        public async Task<TData<List<ZtreeInfo>>> GetZtreeDepartmentList(DepartmentListParam param)
        {
            var departmentList = await _departmentService.GetList(param);
            var operatorInfo = await Operator.Instance.Current();
            if (operatorInfo.IsSystem != 1)
            {
                var childrenDepartmentIdList = await GetChildrenDepartmentIdList(departmentList, operatorInfo.DepartmentId!.Value);
                departmentList = departmentList.Where(p => childrenDepartmentIdList.Contains(p.Id!.Value)).ToList();
            }
            var list = departmentList.Select(department => new ZtreeInfo
            {
                id = department.Id,
                pId = department.ParentId,
                name = department.DepartmentName
            }).ToList();
            return new() { Data = list, Tag = 1 };
        }

        public async Task<TData<List<ZtreeInfo>>> GetZtreeUserList(DepartmentListParam param)
        {
            var list = new List<ZtreeInfo>();
            var departmentList = await _departmentService.GetList(param);
            var operatorInfo = await Operator.Instance.Current();
            if (operatorInfo.IsSystem != 1)
            {
                var childrenDepartmentIdList = await GetChildrenDepartmentIdList(departmentList, operatorInfo.DepartmentId!.Value);
                departmentList = departmentList.Where(p => childrenDepartmentIdList.Contains(p.Id!.Value)).ToList();
            }
            var userList = await _userService.GetList(null);
            foreach (var department in departmentList)
            {
                list.Add(new ZtreeInfo
                {
                    id = department.Id,
                    pId = department.ParentId,
                    name = department.DepartmentName
                });
                var userIdList = userList.Where(t => t.DepartmentId == department.Id).Select(t => t.Id!.Value);
                list.AddRange(userList.Where(t => userIdList.Contains(t.Id!.Value)).Select(user => new ZtreeInfo
                {
                    id = user.Id,
                    pId = department.Id,
                    name = user.RealName
                }));
            }
            return new() { Data = list, Tag = 1 };
        }

        public async Task<TData<DepartmentEntity>> GetEntity(long id)
        {
            return new() { Data = await _departmentService.GetEntity(id), Tag = 1 };
        }

        public async Task<TData<int>> GetMaxSort()
        {
            return new() { Data = await _departmentService.GetMaxSort(), Tag = 1 };
        }

        #endregion

        #region 提交数据

        public async Task<TData<string>> SaveForm(DepartmentEntity entity)
        {
            if (_departmentService.ExistDepartmentName(entity))
            {
                return new() { Tag = 0, Message = "部门名称已经存在" };
            }
            await _departmentService.SaveForm(entity);
            return new() { Data = entity.Id.ParseToString(), Tag = 1 };
        }

        public async Task<TData> DeleteForm(string ids)
        {
            if (_departmentService.ExistChildrenDepartment(TextHelper.SplitToArray<long>(ids, ',')))
            {
                return new() { Tag = 0, Message = "该部门下面有子部门" };
            }
            await _departmentService.DeleteForm(ids);
            return new() { Tag = 1 };
        }

        #endregion

        #region 公共方法

        /// <summary>
        /// 获取当前部门及下面所有的部门
        /// </summary>
        public async Task<List<long>> GetChildrenDepartmentIdList(List<DepartmentEntity> departmentList, long departmentId)
        {
            departmentList ??= await _departmentService.GetList(null);
            var departmentIdList = new List<long> { departmentId };
            GetChildrenDepartmentIdList(departmentList, departmentId, departmentIdList);
            return departmentIdList;
        }

        #endregion

        #region 私有方法

        /// <summary>
        /// 获取该部门下面所有的子部门
        /// </summary>
        private void GetChildrenDepartmentIdList(List<DepartmentEntity> departmentList, long departmentId, List<long> departmentIdList)
        {
            var children = departmentList.Where(p => p.ParentId == departmentId).Select(p => p.Id!.Value);
            if (!children.Any()) return;
            departmentIdList.AddRange(children);
            foreach (long id in children)
            {
                GetChildrenDepartmentIdList(departmentList, id, departmentIdList);
            }
        }

        #endregion
    }
}