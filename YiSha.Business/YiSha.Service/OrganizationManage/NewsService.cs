using System;
using System.Linq;
using System.Linq.Expressions;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Data.Common;
using System.Text;
using YiSha.Util;
using YiSha.Util.Extension;
using YiSha.Util.Model;
using YiSha.Data;
using YiSha.Data.Repository;
using YiSha.Entity.OrganizationManage;
using YiSha.Model.Param.OrganizationManage;

namespace YiSha.Service.OrganizationManage
{
    public class NewsService : RepositoryFactory
    {
        #region 获取数据
        public async Task<List<NewsEntity>> GetList(NewsListParam param)
        {
            var strSql = new StringBuilder();
            List<DbParameter> filter = ListFilter(param, strSql);
            var list = await this.BaseRepository().FindList<NewsEntity>(strSql.ToString(), filter.ToArray());
            return list.ToList();
        }

        public async Task<List<NewsEntity>> GetPageList(NewsListParam param, Pagination pagination)
        {
            var strSql = new StringBuilder();
            List<DbParameter> filter = ListFilter(param, strSql);
            var list = await this.BaseRepository().FindList<NewsEntity>(strSql.ToString(), filter.ToArray(), pagination);
            return list.ToList();
        }

        public async Task<List<NewsEntity>> GetPageContentList(NewsListParam param, Pagination pagination)
        {
            var strSql = new StringBuilder();
            List<DbParameter> filter = ListFilter(param, strSql, true);
            var list = await this.BaseRepository().FindList<NewsEntity>(strSql.ToString(), filter.ToArray(), pagination);
            return list.ToList();
        }

        public async Task<NewsEntity> GetEntity(long id)
        {
            return await this.BaseRepository().FindEntity<NewsEntity>(id);
        }

        public async Task<int> GetMaxSort()
        {
            object result = await this.BaseRepository().FindObject("SELECT MAX(NewsSort) FROM SysNews");
            int sort = result.ParseToInt();
            sort++;
            return sort;
        }
        #endregion

        #region 提交数据
        public async Task SaveForm(NewsEntity entity)
        {
            if (entity.Id.IsNullOrZero())
            {
                await entity.Create();
                await this.BaseRepository().Insert<NewsEntity>(entity);
            }
            else
            {
                await entity.Modify();
                await this.BaseRepository().Update<NewsEntity>(entity);
            }
        }

        public async Task DeleteForm(string ids)
        {
            long[] idArr = TextHelper.SplitToArray<long>(ids, ',');
            await this.BaseRepository().Delete<NewsEntity>(idArr);
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
                    parameter.Add(DbParameterExtension.CreateDbParameter("@NewsTitle", '%' + param.NewsTitle + '%'));
                }
                if (param.NewsType > 0)
                {
                    strSql.Append(" AND a.NewsType = @NewsType");
                    parameter.Add(DbParameterExtension.CreateDbParameter("@NewsType", param.NewsType));
                }
                if (!string.IsNullOrEmpty(param.NewsTag))
                {
                    strSql.Append(" AND a.NewsTag like @NewsTag");
                    parameter.Add(DbParameterExtension.CreateDbParameter("@NewsTag", '%' + param.NewsTag + '%'));
                }
                if (param.ProvinceId > 0)
                {
                    strSql.Append(" AND a.ProvinceId = @ProvinceId");
                    parameter.Add(DbParameterExtension.CreateDbParameter("@ProvinceId", param.ProvinceId));
                }
                if (param.CityId > 0)
                {
                    strSql.Append(" AND a.CityId = @CityId");
                    parameter.Add(DbParameterExtension.CreateDbParameter("@CityId", param.CityId));
                }
                if (param.CountyId > 0)
                {
                    strSql.Append(" AND a.CountId = @CountId");
                    parameter.Add(DbParameterExtension.CreateDbParameter("@CountyId", param.CountyId));
                }
            }
            return parameter;
        }
        #endregion
    }
}
