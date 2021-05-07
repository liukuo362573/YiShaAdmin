using YiSha.Util.Model;

namespace YiSha.IdGenerator
{
    /// <summary>
    /// 生成数据库主键Id
    /// </summary>
    public class IdGeneratorHelper
    {
        private readonly int _snowFlakeWorkerId = GlobalContext.SystemConfig.SnowFlakeWorkerId;

        private readonly Snowflake _snowflake;

        private IdGeneratorHelper()
        {
            _snowflake = new Snowflake(_snowFlakeWorkerId, 0);
        }

        public static IdGeneratorHelper Instance { get; } = new();

        public long GetId()
        {
            return _snowflake.NextId();
        }
    }
}