using Microsoft.AspNetCore.Mvc;
using YiSha.Admin.Web.Filter;
using YiSha.Util;
using YiSha.Util.Model;

namespace YiSha.Admin.Web.Controllers
{
    /// <summary>
    /// 文件控制器
    /// </summary>
    public class FileController : BaseController
    {
        /// <summary>
        /// 上传单个文件
        /// </summary>
        /// <param name="fileModule"></param>
        /// <param name="fileList"></param>
        /// <returns></returns>
        [HttpPost]
        [AuthorizeFilter]
        public async Task<TData<string>> UploadFile(int fileModule, IFormCollection fileList)
        {
            TData<string> obj = await FileHelper.UploadFile(fileModule, fileList.Files);
            return obj;
        }

        /// <summary>
        /// 删除单个文件
        /// </summary>
        /// <param name="fileModule"></param>
        /// <param name="filePath">文件路径</param>
        /// <returns></returns>
        [HttpPost]
        [AuthorizeFilter]
        public TData<string> DeleteFile(int fileModule, string filePath)
        {
            TData<string> obj = FileHelper.DeleteFile(fileModule, filePath);
            return obj;
        }

        /// <summary>
        /// 下载文件
        /// </summary>
        /// <param name="filePath">文件路径</param>
        /// <param name="delete"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        [HttpGet]
        public FileContentResult DownloadFile(string filePath, int delete = 1)
        {
            var obj = FileHelper.DownloadFile(filePath, delete);
            if (obj.Tag == 1)
            {
                return obj.Data;
            }
            else
            {
                throw new Exception("下载失败：" + obj.Message);
            }
        }
    }
}
