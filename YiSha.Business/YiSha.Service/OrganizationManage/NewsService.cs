using System.Collections.Generic;
using System.Data.Common;
using System.Text;
using System.Threading.Tasks;
using YiSha.Data.Extension;
using YiSha.Data.Repository;
using YiSha.Entity.OrganizationManage;
using YiSha.Model.Param.OrganizationManage;
using YiSha.Util.Extension;
using YiSha.Util.Helper;
using YiSha.Util.Model;

namespace YiSha.Service.OrganizationManage
{
    public class NewsService : RepositoryFactory
    {
        #region 获取数据

        public async Task<List<NewsEntity>> GetList(NewsListParam param)
        {
            var builder = new StringBuilder();
            var listFilter = ListFilter(param, builder).ToArray();
            return await BaseRepository().FindList<NewsEntity>(builder.ToString(), listFilter);
        }

        public async Task<List<NewsEntity>> GetPageList(NewsListParam param, Pagination pagination)
        {
            var builder = new StringBuilder();
            var listFilter = ListFilter(param, builder).ToArray();
            return await BaseRepository().FindList<NewsEntity>(builder.ToString(), pagination, listFilter);
        }

        public async Task<List<NewsEntity>> GetPageContentList(NewsListParam param, Pagination pagination)
        {
            var builder = new StringBuilder();
            var listFilter = ListFilter(param, builder, true).ToArray();
            return await BaseRepository().FindList<NewsEntity>(builder.ToString(), pagination, listFilter);
        }

        public async Task<NewsEntity> GetEntity(long id)
        {
            return await BaseRepository().FindEntity<NewsEntity>(id);
        }

        public async Task<int> GetMaxSort()
        {
            var result = await BaseRepository().FindEntity<int>("SELECT MAX(NewsSort) FROM SysNews");
            return result + 1;
        }

        #endregion

        #region 提交数据

        public async Task SaveForm(NewsEntity entity)
        {
            if (entity.Id.IsNullOrZero())
            {
                await entity.Create();
                await BaseRepository().Insert(entity);
            }
            else
            {
                await entity.Modify();
                await BaseRepository().Update(entity);
            }
        }

        public async Task DeleteForm(string ids)
        {
            var idArr = TextHelper.SplitToArray<object>(ids, ',');
            await BaseRepository().Delete<NewsEntity>(idArr);
        }

        #endregion

        #region 私有方法

        private List<DbParameter> ListFilter(NewsListParam param, StringBuilder builder, bool bNewsContent = false)
        {
            builder.Append(@"SELECT  a.Id,
                                    a.BaseModifyTime,
                                    a.BaseModifierId,
                                    a.NewsTitle,
                                    a.ThumbImage,
                                    a.NewsTag,
                                    a.NewsAuthor,
                                    a.NewsSort,
                                    a.NewsDate,
                                    a.NewsType,
                                    a.ProvinceId,
                                    a.CityId,
                                    a.CountyId,
                                    a.ViewTimes");
            if (bNewsContent)
            {
                builder.Append(",a.NewsContent");
            }
            builder.Append(@" FROM SysNews a WHERE 1 = 1");
            var parameter = new List<DbParameter>();
            if (param != null)
            {
                if (param.NewsTitle?.Length > 0)
                {
                    builder.Append(" AND a.NewsTitle like @NewsTitle");
                    parameter.Add(DbParameterHelper.CreateDbParameter("@NewsTitle", '%' + param.NewsTitle + '%'));
                }
                if (param.NewsType > 0)
                {
                    builder.Append(" AND a.NewsType = @NewsType");
                    parameter.Add(DbParameterHelper.CreateDbParameter("@NewsType", param.NewsType));
                }
                if (param.NewsTag?.Length > 0)
                {
                    builder.Append(" AND a.NewsTag like @NewsTag");
                    parameter.Add(DbParameterHelper.CreateDbParameter("@NewsTag", '%' + param.NewsTag + '%'));
                }
                if (param.ProvinceId > 0)
                {
                    builder.Append(" AND a.ProvinceId = @ProvinceId");
                    parameter.Add(DbParameterHelper.CreateDbParameter("@ProvinceId", param.ProvinceId));
                }
                if (param.CityId > 0)
                {
                    builder.Append(" AND a.CityId = @CityId");
                    parameter.Add(DbParameterHelper.CreateDbParameter("@CityId", param.CityId));
                }
                if (param.CountyId > 0)
                {
                    builder.Append(" AND a.CountId = @CountId");
                    parameter.Add(DbParameterHelper.CreateDbParameter("@CountyId", param.CountyId));
                }
            }
            return parameter;
        }

        #endregion
    }
}