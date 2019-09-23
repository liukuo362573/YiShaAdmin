using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using YiSha.Util;
using YiSha.Util.Extension;
using YiSha.Util.Model;
using YiSha.Entity.OrganizationManage;
using YiSha.Model.Param.OrganizationManage;
using YiSha.Service.OrganizationManage;
using YiSha.Business.SystemManage;

namespace YiSha.Business.OrganizationManage
{
    public class NewsBLL
    {
        private AreaBLL areaBLL = new AreaBLL();
        private NewsService newsService = new NewsService();

        #region 获取数据
        public async Task<TData<List<NewsEntity>>> GetList(NewsListParam param)
        {
            TData<List<NewsEntity>> obj = new TData<List<NewsEntity>>();
            areaBLL.SetAreaParam(param);
            obj.Result = await newsService.GetList(param);
            obj.TotalCount = obj.Result.Count;
            obj.Tag = 1;
            return obj;
        }

        public async Task<TData<List<NewsEntity>>> GetPageList(NewsListParam param, Pagination pagination)
        {
            TData<List<NewsEntity>> obj = new TData<List<NewsEntity>>();
            areaBLL.SetAreaParam(param);
            obj.Result = await newsService.GetPageList(param, pagination);
            obj.TotalCount = pagination.TotalCount;
            obj.Tag = 1;
            return obj;
        }

        public async Task<TData<List<NewsEntity>>> GetPageContentList(NewsListParam param, Pagination pagination)
        {
            TData<List<NewsEntity>> obj = new TData<List<NewsEntity>>();
            obj.Result = await newsService.GetPageContentList(param, pagination);
            obj.TotalCount = pagination.TotalCount;
            obj.Tag = 1;
            return obj;
        }

        public async Task<TData<NewsEntity>> GetEntity(long id)
        {
            TData<NewsEntity> obj = new TData<NewsEntity>();
            obj.Result = await newsService.GetEntity(id);
            areaBLL.SetAreaId(obj.Result);
            obj.Tag = 1;
            return obj;
        }

        public async Task<TData<int>> GetMaxSort()
        {
            TData<int> obj = new TData<int>();
            obj.Result = await newsService.GetMaxSort();
            obj.Tag = 1;
            return obj;
        }
        #endregion

        #region 提交数据
        public async Task<TData<string>> SaveForm(NewsEntity entity)
        {
            TData<string> obj = new TData<string>();
            areaBLL.SetAreaEntity(entity);
            await newsService.SaveForm(entity);
            obj.Result = entity.Id.ParseToString();
            obj.Tag = 1;
            return obj;
        }

        public async Task<TData> DeleteForm(string ids)
        {
            TData obj = new TData();
            await newsService.DeleteForm(ids);
            obj.Tag = 1;
            return obj;
        }
        #endregion

        #region 私有方法
        #endregion
    }
}
