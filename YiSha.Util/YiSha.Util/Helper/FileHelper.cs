﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YiSha.Enum;
using YiSha.Util.Extension;
using YiSha.Util.Model;

namespace YiSha.Util.Helper
{
    public static class FileHelper
    {
        /// <summary>
        /// 创建文件
        /// </summary>
        public static void CreateFile(string path, string content)
        {
            if (!Directory.Exists(Path.GetDirectoryName(path)))
            {
                Directory.CreateDirectory(Path.GetDirectoryName(path));
            }
            using var sw = new StreamWriter(path, false, Encoding.UTF8);
            sw.Write(content);
        }

        /// <summary>
        /// 上传单个文件
        /// </summary>
        public static async Task<TData<string>> UploadFile(int fileModule, IFormFileCollection files)
        {
            string dirModule;
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
            TData objCheck;
            IFormFile file = files[0];
            switch (fileModule)
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

                case (int)UploadFileType.Import:
                    objCheck = CheckFileExtension(Path.GetExtension(file.FileName), ".xls|.xlsx");
                    if (objCheck.Tag != 1)
                    {
                        obj.Message = objCheck.Message;
                        return obj;
                    }
                    dirModule = UploadFileType.Import.ToString();
                    break;

                default:
                    obj.Message = "请指定上传到的模块";
                    return obj;
            }
            string fileExtension = TextHelper.GetCustomValue(Path.GetExtension(file.FileName), ".png");

            string newFileName = SecurityHelper.GetGuid() + fileExtension;
            string dir = "Resource" + Path.DirectorySeparatorChar + dirModule + Path.DirectorySeparatorChar + DateTime.Now.ToString("yyyy-MM-dd").Replace('-', Path.DirectorySeparatorChar) + Path.DirectorySeparatorChar;

            string absoluteDir = Path.Combine(GlobalContext.HostingEnvironment.ContentRootPath, dir);
            if (!Directory.Exists(absoluteDir))
            {
                Directory.CreateDirectory(absoluteDir);
            }
            var absoluteFileName = absoluteDir + newFileName;
            try
            {
                using var fs = File.Create(absoluteFileName);
                await file.CopyToAsync(fs);

                obj.Data = Path.AltDirectorySeparatorChar + ConvertDirectoryToHttp(dir) + newFileName;
                obj.Message = Path.GetFileNameWithoutExtension(TextHelper.GetCustomValue(file.FileName, newFileName));
                obj.Description = (file.Length / 1024).ToString(); // KB
                obj.Tag = 1;
            }
            catch (Exception ex)
            {
                obj.Message = ex.Message;
            }
            return obj;
        }

        /// <summary>
        /// 删除单个文件
        /// </summary>
        public static TData<string> DeleteFile(int fileModule, string filePath)
        {
            var obj = new TData<string>();
            string dirModule = fileModule.GetDescriptionByEnum<UploadFileType>();

            if (string.IsNullOrEmpty(filePath))
            {
                obj.Message = "请先选择文件！";
                return obj;
            }
            filePath = "Resource" + Path.DirectorySeparatorChar + dirModule + Path.DirectorySeparatorChar + filePath;
            string absoluteDir = Path.Combine(GlobalContext.HostingEnvironment.ContentRootPath, filePath);
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

        /// <summary>
        /// 下载文件
        /// </summary>
        public static TData<FileContentResult> DownloadFile(string filePath, int delete)
        {
            if (filePath == null) throw new ArgumentNullException(nameof(filePath));
            var obj = new TData<FileContentResult>();
            string absoluteFilePath = GlobalContext.HostingEnvironment.ContentRootPath + Path.DirectorySeparatorChar + filePath.Replace(Path.AltDirectorySeparatorChar, Path.DirectorySeparatorChar);
            byte[] fileBytes = File.ReadAllBytes(absoluteFilePath);
            if (delete == 1)
            {
                File.Delete(absoluteFilePath);
            }
            string fileNamePrefix = DateTime.Now.ToString("yyyyMMddHHmmss");
            string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(filePath);
            string title = string.Empty;
            if (fileNameWithoutExtension.Contains("_"))
            {
                title = fileNameWithoutExtension.Split('_')[1].Trim();
            }
            string fileExtensionName = Path.GetExtension(filePath);
            obj.Data = new FileContentResult(fileBytes, "application/octet-stream")
            {
                FileDownloadName = $"{fileNamePrefix}_{title}{fileExtensionName}"
            };
            obj.Tag = 1;
            return obj;
        }

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

        public static Dictionary<string, string> GetMimeTypes()
        {
            return new()
            {
                { ".txt", "text/plain" },
                { ".pdf", "application/pdf" },
                { ".doc", "application/vnd.ms-word" },
                { ".docx", "application/vnd.ms-word" },
                { ".xls", "application/vnd.ms-excel" },
                { ".xlsx", "application/vnd.openxmlformats officedocument.spreadsheetml.sheet" },
                { ".png", "image/png" },
                { ".jpg", "image/jpeg" },
                { ".jpeg", "image/jpeg" },
                { ".gif", "image/gif" },
                { ".csv", "text/csv" }
            };
        }

        public static void CreateDirectory(string directory)
        {
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }
        }

        public static void DeleteDirectory(string filePath)
        {
            try
            {
                if (Directory.Exists(filePath)) //如果存在这个文件夹删除之
                {
                    foreach (string d in Directory.GetFileSystemEntries(filePath))
                    {
                        if (File.Exists(d))
                        {
                            File.Delete(d); //直接删除其中的文件
                        }
                        else
                        {
                            DeleteDirectory(d); //递归删除子文件夹
                        }
                    }
                    Directory.Delete(filePath, true); //删除已空文件夹
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
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
            var obj = new TData();
            var allowArr = TextHelper.SplitToArray<string>(allowExtension?.ToLower(), '|');
            if (allowArr.Any(p => p.Trim() == fileExtension.ParseToString().ToLower()))
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