using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using YiSha.Entity;
using YiSha.Model;
using YiSha.Model.Entity.OrganizationManage;
using YiSha.Util;

namespace YiSha.Admin.WebApi.Controllers
{
    /// <summary>
    /// 新闻数据控制器
    /// </summary>
    [Authorize]
    [Route("[controller]/[action]")]
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
        public TData<List<NewsEntity>> GetPageList(string? newsTitle = default, int newsType = default, string? newsTag = default,
            int provinceId = default, int cityId = default, int countyId = default,
            int pageIndex = 1, int pageSize = 10)
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
                                    //ProvinceName = news.ProvinceName,
                                    //CityName = news.CityName,
                                    //CountryName = news.CountryName,
                                    //AreaId = news.AreaId,
                                    BaseVersion = news.BaseVersion,
                                    BaseModifyTime = news.BaseModifyTime,
                                    BaseModifierId = news.BaseModifierId,
                                    BaseCreateTime = news.BaseCreateTime,
                                    BaseCreatorId = news.BaseCreatorId,
                                    Id = news.Id,
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
        /// <param name="newsTitle">文章标题</param>
        /// <param name="newsType">文章类型</param>
        /// <param name="newsTag">文章标签</param>
        /// <param name="pageIndex">页索引</param>
        /// <param name="pageSize">页大小</param>
        /// <returns></returns>
        [HttpGet]
        public TData<List<NewsEntity>> GetPageContentList(string? newsTitle = default, int newsType = default, string? newsTag = default,
            int pageIndex = 1, int pageSize = 10)
        {
            var newSysNews = (from news in DbCmd.SysNews
                              where (newsTitle == default || news.NewsTitle.Contains(newsTitle))
                                     && (newsType == default || news.NewsType == newsType)
                                     && (newsTag == default || news.NewsTag.Contains(newsTag))
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
                                  //ProvinceName = news.ProvinceName,
                                  //CityName = news.CityName,
                                  //CountryName = news.CountryName,
                                  //AreaId = news.AreaId,
                                  BaseVersion = news.BaseVersion,
                                  BaseModifyTime = news.BaseModifyTime,
                                  BaseModifierId = news.BaseModifierId,
                                  BaseCreateTime = news.BaseCreateTime,
                                  BaseCreatorId = news.BaseCreatorId,
                                  Id = news.Id,
                              });
            var obj = new TData<List<NewsEntity>>();
            obj.Data = newSysNews.Skip(pageSize * (pageIndex - 1)).Take(pageSize).ToList();
            obj.Total = newSysNews.Count();
            obj.Tag = 1;
            return obj;
        }

        /// <summary>
        /// 获取文章内容
        /// </summary>
        /// <param name="id">Id</param>
        /// <returns></returns>
        [HttpGet]
        public TData<NewsEntity> GetForm(long id = default)
        {
            var newSysNews = (from news in DbCmd.SysNews
                              where (id == default || news.Id == id)
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
                                  //ProvinceName = news.ProvinceName,
                                  //CityName = news.CityName,
                                  //CountryName = news.CountryName,
                                  //AreaId = news.AreaId,
                                  BaseVersion = news.BaseVersion,
                                  BaseModifyTime = news.BaseModifyTime,
                                  BaseModifierId = news.BaseModifierId,
                                  BaseCreateTime = news.BaseCreateTime,
                                  BaseCreatorId = news.BaseCreatorId,
                                  Id = news.Id,
                              });
            var obj = new TData<NewsEntity>();
            obj.Data = newSysNews.FirstOrDefault();
            obj.Total = newSysNews.Count();
            obj.Tag = 1;
            return obj;
        }

        /// <summary>
        /// 保存文章
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        public TData<string> SaveForm(NewsEntity entity)
        {
            DbCmd.SysNews.Add(entity);
            var count = DbCmd.SaveChanges();
            var obj = new TData<string>();
            obj.Data = entity.Id.ToStr();
            obj.Total = 0;
            obj.Tag = 1;
            return obj;
        }

        /// <summary>
        /// SaveViewTimes
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public TData<string> SaveViewTimes(long id = default)
        {
            TData<string> obj = new TData<string>();
            TData<NewsEntity> objNews = new TData<NewsEntity>();
            NewsEntity newsEntity = new NewsEntity();
            if (objNews.Data != null)
            {
                newsEntity.Id = id;
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
