using YiSha.Util;

namespace YiSha.IdGenerator
{
    /// <summary>
    /// 生成数据库主键Id
    /// </summary>
    public class IdGeneratorHelper
    {
        private int SnowFlakeWorkerId = GlobalContext.SystemConfig.SnowFlakeWorkerId;

        private Snowflake snowflake;

        private static readonly IdGeneratorHelper instance = new IdGeneratorHelper();

        private IdGeneratorHelper()
        {
            snowflake = new Snowflake(SnowFlakeWorkerId, 0, 0);
        }
        public static IdGeneratorHelper Instance
        {
            get
            {
                return instance;
            }
        }
        public long GetId()
        {
            return snowflake.NextId();
        }
    }
}
