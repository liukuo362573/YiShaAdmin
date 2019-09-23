using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YiSha.Business.SystemManage;
using YiSha.Util;
using YiSha.Util.Model;

namespace YiSha.Business.AutoJob
{
    public class DatabasesBackupJob : IJobTask
    {
        public async Task<TData> Start()
        {
            TData obj = new TData();
            string backupPath = string.Empty;// GlobalContext.SystemConfig.DatabaseBackup;
            if (string.IsNullOrEmpty(backupPath))
            {
                backupPath = GlobalContext.HostingEnvironment.ContentRootPath + Path.DirectorySeparatorChar + "Database";
            }
            if (string.IsNullOrEmpty(backupPath))
            {
                obj.Message = "未配置数据库备份路径";
            }
            else
            {
                if (!Directory.Exists(backupPath))
                {
                    Directory.CreateDirectory(backupPath);
                }

                string info = await new DatabaseTableBLL().DatabaseBackup(backupPath);

                obj.Tag = 1;
                obj.Message = "备份路径：" + info;
            }
            return obj;
        }
    }
}
