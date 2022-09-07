using Microsoft.AspNetCore.Mvc;
using YiSha.Admin.WebApi.Filter;
using YiSha.Entity;
using YiSha.Model.Entity.OrganizationManage;
using YiSha.Model.Param;
using YiSha.Model.Param.OrganizationManage;
using YiSha.Util;
using YiSha.Model;

namespace YiSha.Admin.WebApi.Controllers
{
    /// <summary>
    /// 新闻数据控制器
    /// </summary>
    [Route("[controller]/[action]")]
    [ApiController]
    [AuthorizeFilter]
    public class NewsController : Controller
    {
        /// <summary>
        /// 数据库上下文
        /// </summary>
        private MyDbContext DbCmd { get; }

        /// <summary>
        /// 新闻数据控制器
        /// </summary>
        public NewsController(MyDbContext myDbContext)
        {
            this.DbCmd = myDbContext;
        }

        /// <summary>
        /// 获取文章列表
        /// </summary>
        /// <param name="newsTitle">文章标题</param>
        /// <param name="newsType">文章类别</param>
        /// <param name="newsTag">文章标签</param>
        /// <param name="provinceId">省份ID</param>
        /// <param name="cityId">城市ID</param>
        /// <param name="countyId">区域ID</param>
        /// <param name="pageIndex">页索引</param>
        /// <param name="pageSize">页大小</param>
        /// <returns></returns>
        [HttpGet]
        public TData<List<NewsEntity>> GetPageList(string? newsTitle = default, int newsType = default, string newsTag = default,
            int provinceId = default, int cityId = default, int countyId = default,
            int pageIndex = 0, int pageSize = 10)
        {
            var newsEntities = (from news in DbCmd.SysNews
                                where (newsTitle == default || news.NewsTitle.Contains(newsTitle))
                                       && (newsType == default || news.NewsType == newsType)
                                       && (newsTag == default || news.NewsTag.Contains(newsTag))
                                       && (provinceId == default || news.ProvinceId == provinceId)
                                       && (cityId == default || news.CityId == cityId)
                                       && (countyId == default || news.CountyId == countyId)
                                orderby news.NewsSort ascending
                                select new NewsEntity
                                {
                                    NewsTitle = news.NewsTitle,
                                    NewsContent = news.NewsContent,
                                    NewsTag = news.NewsTag,
                                    ThumbImage = news.ThumbImage,
                                    NewsAuthor = news.NewsAuthor,
                                    NewsSort = news.NewsSort,
                                    NewsDate = news.NewsDate,
                                    NewsType = news.NewsType,
                                    ViewTimes = news.ViewTimes,
                                    ProvinceId = news.ProvinceId,
                                    CityId = news.CityId,
                                    CountyId = news.CountyId,
                                    ProvinceName = "",
                                    CityName = "",
                                    CountryName = "",
                                    AreaId = "",
                                    BaseVersion = news.BaseVersion,
                                    BaseModifyTime = news.BaseModifyTime,
                                    BaseModifierId = news.BaseModifierId,
                                    BaseCreateTime = news.BaseCreateTime,
                                    BaseCreatorId = news.BaseCreatorId,
                                    Id = news.Id,
                                    Token = news.Token,
                                });
            var obj = new TData<List<NewsEntity>>();
            obj.Data = newsEntities.Skip(pageSize * (pageIndex - 1)).Take(pageSize).ToList();
            obj.Total = newsEntities.Count();
            obj.Tag = 1;
            return obj;
        }

        /// <summary>
        /// 获取文章列表
        /// </summary>
        /// <param name="param"></param>
        /// <param name="pagination"></param>
        /// <returns></returns>
        [HttpGet]
        public TData<List<NewsEntity>> GetPageContentList([FromQuery] NewsListParam param, [FromQuery] Pagination pagination)
        {
            TData<List<NewsEntity>> obj = new TData<List<NewsEntity>>();
            return obj;
        }

        /// <summary>
        /// 获取文章内容
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public TData<NewsEntity> GetForm([FromQuery] long id)
        {
            TData<NewsEntity> obj = new TData<NewsEntity>();
            return obj;
        }

        /// <summary>
        /// 保存文章
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        public TData<string> SaveForm([FromBody] NewsEntity entity)
        {
            TData<string> obj = new TData<string>();
            return obj;
        }

        /// <summary>
        /// SaveViewTimes
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpPost]
        public TData<string> SaveViewTimes([FromBody] IdParam param)
        {
            TData<string> obj = new TData<string>();
            TData<NewsEntity> objNews = new TData<NewsEntity>();
            NewsEntity newsEntity = new NewsEntity();
            if (objNews.Data != null)
            {
                newsEntity.Id = param.Id.Value;
                newsEntity.ViewTimes = objNews.Data.ViewTimes;
                newsEntity.ViewTimes++;
            }
            else
            {
                obj = new TData<string>();
                obj.Message = "文章不存在";
            }
            return obj;
        }
    }
}
