using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
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
            var strSql = new StringBuilder();
            List<DbParameter> filter = ListFilter(param, strSql);
            var list = await BaseRepository().FindList<NewsEntity>(strSql.ToString(), filter.ToArray());
            return list.ToList();
        }

        public async Task<List<NewsEntity>> GetPageList(NewsListParam param, Pagination pagination)
        {
            var strSql = new StringBuilder();
            List<DbParameter> filter = ListFilter(param, strSql);
            var list = await BaseRepository().FindList<NewsEntity>(strSql.ToString(), filter.ToArray(), pagination);
            return list.ToList();
        }

        public async Task<List<NewsEntity>> GetPageContentList(NewsListParam param, Pagination pagination)
        {
            var strSql = new StringBuilder();
            List<DbParameter> filter = ListFilter(param, strSql, true);
            var list = await BaseRepository().FindList<NewsEntity>(strSql.ToString(), filter.ToArray(), pagination);
            return list.ToList();
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
            long[] idArr = TextHelper.SplitToArray<long>(ids, ',');
            await BaseRepository().Delete<NewsEntity>(idArr);
        }

        #endregion

        #region 私有方法

        private List<DbParameter> ListFilter(NewsListParam param, StringBuilder strSql, bool bNewsContent = false)
        {
            strSql.Append(@"SELECT  a.Id,
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
                strSql.Append(",a.NewsContent");
            }
            strSql.Append(@"         FROM    SysNews a
                            WHERE   1 = 1");
            var parameter = new List<DbParameter>();
            if (param != null)
            {
                if (!string.IsNullOrEmpty(param.NewsTitle))
                {
                    strSql.Append(" AND a.NewsTitle like @NewsTitle");
                    parameter.Add(DbParameterHelper.CreateDbParameter("@NewsTitle", '%' + param.NewsTitle + '%'));
                }
                if (param.NewsType > 0)
                {
                    strSql.Append(" AND a.NewsType = @NewsType");
                    parameter.Add(DbParameterHelper.CreateDbParameter("@NewsType", param.NewsType));
                }
                if (!string.IsNullOrEmpty(param.NewsTag))
                {
                    strSql.Append(" AND a.NewsTag like @NewsTag");
                    parameter.Add(DbParameterHelper.CreateDbParameter("@NewsTag", '%' + param.NewsTag + '%'));
                }
                if (param.ProvinceId > 0)
                {
                    strSql.Append(" AND a.ProvinceId = @ProvinceId");
                    parameter.Add(DbParameterHelper.CreateDbParameter("@ProvinceId", param.ProvinceId));
                }
                if (param.CityId > 0)
                {
                    strSql.Append(" AND a.CityId = @CityId");
                    parameter.Add(DbParameterHelper.CreateDbParameter("@CityId", param.CityId));
                }
                if (param.CountyId > 0)
                {
                    strSql.Append(" AND a.CountId = @CountId");
                    parameter.Add(DbParameterHelper.CreateDbParameter("@CountyId", param.CountyId));
                }
            }
            return parameter;
        }

        #endregion
    }
}