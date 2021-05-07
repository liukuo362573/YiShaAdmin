using System.Collections.Generic;
using System.Threading.Tasks;
using YiSha.Entity.OrganizationManage;
using YiSha.Model.Param.OrganizationManage;
using YiSha.Service.OrganizationManage;
using YiSha.Util.Extension;
using YiSha.Util.Model;

namespace YiSha.Business.OrganizationManage
{
    public class PositionBLL
    {
        private readonly PositionService _positionService = new();

        #region 获取数据

        public async Task<TData<List<PositionEntity>>> GetList(PositionListParam param)
        {
            return new() { Data = await _positionService.GetList(param), Tag = 1 };
        }

        public async Task<TData<List<PositionEntity>>> GetPageList(PositionListParam param, Pagination pagination)
        {
            return new()
            {
                Data = await _positionService.GetPageList(param, pagination),
                Total = pagination.TotalCount,
                Tag = 1
            };
        }

        public async Task<TData<PositionEntity>> GetEntity(long id)
        {
            return new() { Data = await _positionService.GetEntity(id), Tag = 1 };
        }

        public async Task<TData<int>> GetMaxSort()
        {
            return new() { Data = await _positionService.GetMaxSort(), Tag = 1 };
        }

        #endregion

        #region 提交数据

        public async Task<TData<string>> SaveForm(PositionEntity entity)
        {
            if (_positionService.ExistPositionName(entity))
            {
                return new() { Tag = 0, Message = "职位名称已经存在" };
            }
            await _positionService.SaveForm(entity);
            return new() { Data = entity.Id.ParseToString(), Tag = 1 };
        }

        public async Task<TData> DeleteForm(string ids)
        {
            await _positionService.DeleteForm(ids);
            return new() { Tag = 1 };
        }

        #endregion
    }
}