using YiSha.Util.Model;

namespace YiSha.Business.AutoJob
{
    public interface IJobTask
    {
        Task<TData> Start();
    }
}
