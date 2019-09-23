using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using YiSha.Business.OrganizationManage;
using YiSha.Business.SystemManage;
using YiSha.Entity.OrganizationManage;
using YiSha.Model.Param.OrganizationManage;
using YiSha.Model.Result.SystemManage;
using YiSha.Util.Model;

namespace YiSha.FrontWeb.Controllers
{
    public class NewsController : Controller
    {
        private NewsBLL newsBLL = new NewsBLL();
        private DataDictBLL dataDictBLL = new DataDictBLL();

        #region 视图功能
        public async Task<IActionResult> Index(NewsListParam param, Pagination pagination)
        {
            pagination.PageSize = 6;
            param.NewsType = param.NewsType ?? 1;
            if (pagination.Sort == "Id")
            {
                pagination.Sort = "NewsDate";
            }
            TData<List<NewsEntity>> objNews = await newsBLL.GetPageList(param, pagination);
            TData<List<DataDictInfo>> objDataDict = await dataDictBLL.GetDataDictList();
            List<NewsEntity> list = objNews.Result;
            List<DataDictDetailInfo> newsTypeList = objDataDict.Result.Where(p => p.DictType == "NewsType").Select(p => p.Detail).FirstOrDefault();

            ViewBag.NewsList = list;
            ViewBag.NewsType = param.NewsType;
            ViewBag.NewsTypeList = newsTypeList;

            ViewBag.SortBy = pagination.Sort + ":" + pagination.SortType.Trim();
            ViewBag.Pager = HtmlPager.MiniPager("news", pagination, string.Format("/News/Index?NewsType={0}&NewsTag={1}", param.NewsType, param.NewsTag));
            return View();
        }

        public async Task<IActionResult> Detail(long id)
        {
            TData<NewsEntity> obj = await newsBLL.GetEntity(id);
            if (obj.Result == null)
            {

            }
            return View(obj.Result);
        }
        #endregion

        #region 获取数据
        [HttpGet]
        public async Task<IActionResult> GetListJson(NewsListParam param)
        {
            TData<List<NewsEntity>> obj = await newsBLL.GetList(param);
            return Json(obj);
        }

        [HttpGet]
        public async Task<IActionResult> GetPageListJson(NewsListParam param, Pagination pagination)
        {
            TData<List<NewsEntity>> obj = await newsBLL.GetPageList(param, pagination);
            return Json(obj);
        }

        [HttpGet]
        public async Task<IActionResult> GetFormJson(long id)
        {
            TData<NewsEntity> obj = await newsBLL.GetEntity(id);
            return Json(obj);
        }
        #endregion

        #region 提交数据
        [HttpPost]
        public async Task<IActionResult> SaveViewTimesJson(long id)
        {
            TData<string> obj = null;
            TData<NewsEntity> objNews = await newsBLL.GetEntity(id);
            NewsEntity newsEntity = objNews.Result;
            if (newsEntity != null)
            {
                newsEntity.ViewTimes++;
                obj = await newsBLL.SaveForm(newsEntity);
            }
            else
            {
                obj = new TData<string>();
                obj.Message = "文章不存在";
            }
            return Json(obj);
        }
        #endregion
    }
}