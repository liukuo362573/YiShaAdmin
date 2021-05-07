using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using YiSha.Util.Helper;
using YiSha.Util.Model;

namespace YiSha.Admin.Web.Controllers
{
    public class FileController : BaseController
    {
        /// <summary>
        /// 上传单个文件
        /// </summary>
        [HttpPost]
        public async Task<TData<string>> UploadFile(int fileModule, IFormCollection fileList)
        {
            return await FileHelper.UploadFile(fileModule, fileList.Files);
        }

        /// <summary>
        /// 删除单个文件
        /// </summary>
        [HttpPost]
        public TData<string> DeleteFile(int fileModule, string filePath)
        {
            return FileHelper.DeleteFile(fileModule, filePath);
        }

        /// <summary>
        /// 下载文件
        /// </summary>
        [HttpGet]
        public FileContentResult DownloadFile(string filePath, int delete = 1)
        {
            var obj = FileHelper.DownloadFile(filePath, delete);
            if (obj.Tag == 1)
            {
                return obj.Data;
            }
            throw new Exception("下载失败：" + obj.Message);
        }
    }
}