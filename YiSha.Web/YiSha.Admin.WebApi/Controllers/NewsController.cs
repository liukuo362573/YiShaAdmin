﻿using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using YiSha.Admin.WebApi.Filter;
using YiSha.Business.OrganizationManage;
using YiSha.Entity.OrganizationManage;
using YiSha.Model.Param;
using YiSha.Model.Param.OrganizationManage;
using YiSha.Util.Model;

namespace YiSha.Admin.WebApi.Controllers
{
    [Route("[controller]/[action]"), ApiController, AuthorizeFilter]
    public class NewsController : ControllerBase
    {
        private readonly NewsBLL _newsBLL = new();

        #region 获取数据

        /// <summary>
        /// 获取文章列表
        /// </summary>
        /// <param name="param"></param>
        /// <param name="pagination"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<TData<List<NewsEntity>>> GetPageList([FromQuery] NewsListParam param, [FromQuery] Pagination pagination)
        {
            TData<List<NewsEntity>> obj = await _newsBLL.GetPageList(param, pagination);
            return obj;
        }

        /// <summary>
        /// 获取文章列表
        /// </summary>
        /// <param name="param"></param>
        /// <param name="pagination"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<TData<List<NewsEntity>>> GetPageContentList([FromQuery] NewsListParam param, [FromQuery] Pagination pagination)
        {
            TData<List<NewsEntity>> obj = await _newsBLL.GetPageContentList(param, pagination);
            return obj;
        }

        /// <summary>
        /// 获取文章内容
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<TData<NewsEntity>> GetForm([FromQuery] long id)
        {
            TData<NewsEntity> obj = await _newsBLL.GetEntity(id);
            return obj;
        }

        #endregion

        #region 提交数据

        /// <summary>
        /// 保存文章
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<TData<string>> SaveForm([FromBody] NewsEntity entity)
        {
            TData<string> obj = await _newsBLL.SaveForm(entity);
            return obj;
        }

        [HttpPost]
        public async Task<TData<string>> SaveViewTimes([FromBody] IdParam param)
        {
            TData<string> obj = null;
            TData<NewsEntity> objNews = await _newsBLL.GetEntity(param.Id.Value);
            NewsEntity newsEntity = new NewsEntity();
            if (objNews.Data != null)
            {
                newsEntity.Id = param.Id.Value;
                newsEntity.ViewTimes = objNews.Data.ViewTimes;
                newsEntity.ViewTimes++;
                obj = await _newsBLL.SaveForm(newsEntity);
            }
            else
            {
                obj = new TData<string>();
                obj.Message = "文章不存在";
            }
            return obj;
        }

        #endregion
    }
}