using System.Collections.Generic;
using System.Threading.Tasks;
using YiSha.Business.SystemManage;
using YiSha.Entity.OrganizationManage;
using YiSha.Model.Param.OrganizationManage;
using YiSha.Service.OrganizationManage;
using YiSha.Util.Extension;
using YiSha.Util.Model;

namespace YiSha.Business.OrganizationManage
{
    public class NewsBLL
    {
        private readonly AreaBLL _areaBll = new();

        private readonly NewsService _newsService = new();

        #region 获取数据

        public async Task<TData<List<NewsEntity>>> GetList(NewsListParam param)
        {
            _areaBll.SetAreaParam(param);
            var data = await _newsService.GetList(param);
            return new() { Data = data, Total = data.Count, Tag = 1 };
        }

        public async Task<TData<List<NewsEntity>>> GetPageList(NewsListParam param, Pagination pagination)
        {
            _areaBll.SetAreaParam(param);
            var list = await _newsService.GetPageList(param, pagination);
            return new() { Data = list, Total = pagination.TotalCount, Tag = 1 };
        }

        public async Task<TData<List<NewsEntity>>> GetPageContentList(NewsListParam param, Pagination pagination)
        {
            var list = await _newsService.GetPageContentList(param, pagination);
            return new() { Data = list, Total = pagination.TotalCount, Tag = 1 };
        }

        public async Task<TData<NewsEntity>> GetEntity(long id)
        {
            var entity = await _newsService.GetEntity(id);
            _areaBll.SetAreaId(entity);
            return new() { Data = entity, Tag = 1 };
        }

        public async Task<TData<int>> GetMaxSort()
        {
            return new() { Data = await _newsService.GetMaxSort(), Tag = 1 };
        }

        #endregion

        #region 提交数据

        public async Task<TData<string>> SaveForm(NewsEntity entity)
        {
            _areaBll.SetAreaEntity(entity);
            await _newsService.SaveForm(entity);
            return new() { Data = entity.Id.ParseToString(), Tag = 1 };
        }

        public async Task<TData> DeleteForm(string ids)
        {
            await _newsService.DeleteForm(ids);
            return new() { Tag = 1 };
        }

        #endregion
    }
}