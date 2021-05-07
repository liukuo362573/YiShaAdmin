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
        private readonly AreaBLL _areaBLL = new();
        private readonly NewsService _newsService = new();

        #region 获取数据

        public async Task<TData<List<NewsEntity>>> GetList(NewsListParam param)
        {
            TData<List<NewsEntity>> obj = new TData<List<NewsEntity>>();
            _areaBLL.SetAreaParam(param);
            obj.Data = await _newsService.GetList(param);
            obj.Total = obj.Data.Count;
            obj.Tag = 1;
            return obj;
        }

        public async Task<TData<List<NewsEntity>>> GetPageList(NewsListParam param, Pagination pagination)
        {
            TData<List<NewsEntity>> obj = new TData<List<NewsEntity>>();
            _areaBLL.SetAreaParam(param);
            obj.Data = await _newsService.GetPageList(param, pagination);
            obj.Total = pagination.TotalCount;
            obj.Tag = 1;
            return obj;
        }

        public async Task<TData<List<NewsEntity>>> GetPageContentList(NewsListParam param, Pagination pagination)
        {
            TData<List<NewsEntity>> obj = new TData<List<NewsEntity>>();
            obj.Data = await _newsService.GetPageContentList(param, pagination);
            obj.Total = pagination.TotalCount;
            obj.Tag = 1;
            return obj;
        }

        public async Task<TData<NewsEntity>> GetEntity(long id)
        {
            TData<NewsEntity> obj = new TData<NewsEntity>();
            obj.Data = await _newsService.GetEntity(id);
            _areaBLL.SetAreaId(obj.Data);
            obj.Tag = 1;
            return obj;
        }

        public async Task<TData<int>> GetMaxSort()
        {
            TData<int> obj = new TData<int>();
            obj.Data = await _newsService.GetMaxSort();
            obj.Tag = 1;
            return obj;
        }

        #endregion

        #region 提交数据

        public async Task<TData<string>> SaveForm(NewsEntity entity)
        {
            TData<string> obj = new TData<string>();
            _areaBLL.SetAreaEntity(entity);
            await _newsService.SaveForm(entity);
            obj.Data = entity.Id.ParseToString();
            obj.Tag = 1;
            return obj;
        }

        public async Task<TData> DeleteForm(string ids)
        {
            TData obj = new TData();
            await _newsService.DeleteForm(ids);
            obj.Tag = 1;
            return obj;
        }

        #endregion

        #region 私有方法

        #endregion
    }
}