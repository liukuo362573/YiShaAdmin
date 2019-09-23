using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using YiSha.Util.Model;

namespace YiSha.Business.AutoJob
{
    public interface IJobTask
    {
        Task<TData> Start();
    }
}
