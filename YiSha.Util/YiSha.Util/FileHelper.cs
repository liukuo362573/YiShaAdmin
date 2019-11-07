using System;
using System.IO;
using System.Web;
using System.Text;
using System.Linq;
using System.Text.RegularExpressions;
using YiSha.Util.Model;
using YiSha.Enum;
using YiSha.Util.Extension;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace YiSha.Util
{
    public class FileHelper
    {
        #region 创建文本文件
        /// <summary>
        /// 创建文件
        /// </summary>
        /// <param name="path"></param>
        /// <param name="content"></param>
        public static void CreateFile(string path, string content)
        {
            if (!Directory.Exists(Path.GetDirectoryName(path)))
            {
                Directory.CreateDirectory(Path.GetDirectoryName(path));
            }
            using (StreamWriter sw = new StreamWriter(path, false, Encoding.UTF8))
            {
                sw.Write(content);
            }
        }
        #endregion

        #region 上传单个文件
        /// <summary>
        /// 上传单个文件
        /// </summary>
        /// <param name="iFileModule"></param>
        /// <param name="fileCollection"></param>
        /// <returns></returns>
        public async static Task<TData<string>> UploadFile(int iFileModule, IFormFileCollection files)
        {
            string dirModule = string.Empty;
            TData<string> obj = new TData<string>();
            if (files == null || files.Count == 0)
            {
                obj.Message = "请先选择文件！";
                return obj;
            }
            if (files.Count > 1)
            {
                obj.Message = "一次只能上传一个文件！";
                return obj;
            }
            TData objCheck = null;
            IFormFile file = files[0];
            switch (iFileModule)
            {
                case (int)UploadFileType.Portrait:
                    objCheck = CheckFileExtension(Path.GetExtension(file.FileName), ".jpg|.jpeg|.gif|.png");
                    if (objCheck.Tag != 1)
                    {
                        obj.Message = objCheck.Message;
                        return obj;
                    }
                    dirModule = UploadFileType.Portrait.ToString();
                    break;

                case (int)UploadFileType.News:
                    if (file.Length > 5 * 1024 * 1024) // 5MB
                    {
                        obj.Message = "文件最大限制为 5MB";
                        return obj;
                    }
                    objCheck = CheckFileExtension(Path.GetExtension(file.FileName), ".jpg|.jpeg|.gif|.png");
                    if (objCheck.Tag != 1)
                    {
                        obj.Message = objCheck.Message;
                        return obj;
                    }
                    dirModule = UploadFileType.News.ToString();
                    break;

                default:
                    obj.Message = "请指定上传到的模块";
                    return obj;
            }
            string fileExtension = CommonHelper.GetCustomValueWhenEmpty(Path.GetExtension(file.FileName), ".png");

            string newFileName = SecurityHelper.GetGuid() + fileExtension;
            string dir = "Resource" + Path.DirectorySeparatorChar + dirModule + Path.DirectorySeparatorChar + DateTime.Now.ToString("yyyy-MM-dd").Replace('-', Path.DirectorySeparatorChar) + Path.DirectorySeparatorChar;

            string absoluteDir = Path.Combine(GlobalContext.HostingEnvironment.ContentRootPath, dir);
            string absoluteFileName = string.Empty;
            if (!Directory.Exists(absoluteDir))
            {
                Directory.CreateDirectory(absoluteDir);
            }
            absoluteFileName = absoluteDir + newFileName;
            try
            {
                using (FileStream fs = File.Create(absoluteFileName))
                {
                    await file.CopyToAsync(fs);
                    fs.Flush();
                }
                obj.Result = Path.AltDirectorySeparatorChar + ConvertDirectoryToHttp(dir) + newFileName;
                obj.Message = Path.GetFileNameWithoutExtension(CommonHelper.GetCustomValueWhenEmpty(file.FileName, newFileName));
                obj.Description = (file.Length / 1024).ToString(); // KB
                obj.Tag = 1;
            }
            catch (Exception ex)
            {
                obj.Message = ex.Message;
            }
            return obj;
        }
        #endregion

        #region 删除单个文件
        /// <summary>
        /// 删除单个文件
        /// </summary>
        /// <param name="iFileModule"></param>
        /// <param name="path"></param>
        /// <returns></returns>
        public static TData<string> DeleteFile(int iFileModule, string path)
        {
            TData<string> obj = new TData<string>();
            string dirModule = iFileModule.GetDescriptionByEnum<UploadFileType>();

            if (string.IsNullOrEmpty(path))
            {
                obj.Message = "请先选择文件！";
                return obj;
            }
            path = "Resource" + Path.DirectorySeparatorChar + dirModule + Path.DirectorySeparatorChar + path;
            string absoluteDir = Path.Combine(GlobalContext.HostingEnvironment.ContentRootPath, path);
            try
            {
                if (File.Exists(absoluteDir))
                {
                    File.Delete(absoluteDir);
                }
                else
                {
                    obj.Message = "文件不存在";
                }
                obj.Tag = 1;
            }
            catch (Exception ex)
            {
                obj.Message = ex.Message;
            }
            return obj;
        }
        #endregion

        #region 下载文件
        /// <summary>
        /// 下载文件
        /// </summary>
        /// <param name="httpContext"></param>
        /// <param name="sFileName"></param>
        /// <param name="sFilePath"></param>
        public static void DownLoadFile(HttpContext httpContext, string sFileName, string sFilePath)
        {
            string[] fileNameArr = sFileName.Split('_');
            sFileName = fileNameArr[fileNameArr.Length - 1];

            httpContext.Response.Headers["Content-Type"] = GetContentType(sFileName);
            httpContext.Response.Headers["Content-Disposition"] = "attachment; filename=" + HttpUtility.UrlEncode(sFileName);

            using (FileStream fs = new FileStream(sFilePath, FileMode.Open))
            {
                byte[] bytes = new byte[(int)fs.Length];
                fs.Read(bytes, 0, (int)fs.Length);

                httpContext.Response.Headers.ContentLength = fs.Length;
                httpContext.Response.Body.Write(bytes, 0, bytes.Length);
                httpContext.Response.Body.Flush();
            }
        }
        #endregion

        #region GetContentType
        public static string GetContentType(string path)
        {
            var types = GetMimeTypes();
            var ext = Path.GetExtension(path).ToLowerInvariant();
            var contentType = types[ext];
            if (string.IsNullOrEmpty(contentType))
            {
                contentType = "application/octet-stream";
            }
            return contentType;
        }
        #endregion

        #region GetMimeTypes
        public static Dictionary<string, string> GetMimeTypes()
        {
            return new Dictionary<string, string>
            {
                {".txt", "text/plain"},
                {".pdf", "application/pdf"},
                {".doc", "application/vnd.ms-word"},
                {".docx", "application/vnd.ms-word"},
                {".xls", "application/vnd.ms-excel"},
                {".xlsx", "application/vnd.openxmlformats officedocument.spreadsheetml.sheet"},
                {".png", "image/png"},
                {".jpg", "image/jpeg"},
                {".jpeg", "image/jpeg"},
                {".gif", "image/gif"},
                {".csv", "text/csv"}
            };
        }
        #endregion

        public static void CreateDirectory(string dir)
        {
            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }
        }

        public static void DeleteDirectory(string path)
        {
            try
            {
                if (Directory.Exists(path)) //如果存在这个文件夹删除之 
                {
                    foreach (string d in Directory.GetFileSystemEntries(path))
                    {
                        if (File.Exists(d))
                            File.Delete(d); //直接删除其中的文件                        
                        else
                            DeleteDirectory(d); //递归删除子文件夹 
                    }
                    Directory.Delete(path, true); //删除已空文件夹                 
                }
            }
            catch (Exception ex)
            {
                LogHelper.WriteWithTime(ex);
            }
        }

        public static string ConvertDirectoryToHttp(string directory)
        {
            directory = directory.ParseToString();
            directory = directory.Replace(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar);
            return directory;
        }

        public static string ConvertHttpToDirectory(string http)
        {
            http = http.ParseToString();
            http = http.Replace(Path.AltDirectorySeparatorChar, Path.DirectorySeparatorChar);
            return http;
        }

        public static TData CheckFileExtension(string fileExtension, string allowExtension)
        {
            TData obj = new TData();
            string[] allowArr = CommonHelper.SplitToArray<string>(allowExtension.ToLower(), '|');
            if (allowArr.Where(p => p.Trim() == fileExtension.ParseToString().ToLower()).Any())
            {
                obj.Tag = 1;
            }
            else
            {
                obj.Message = "只有文件扩展名是 " + allowExtension + " 的文件才能上传";
            }
            return obj;
        }
    }
}
