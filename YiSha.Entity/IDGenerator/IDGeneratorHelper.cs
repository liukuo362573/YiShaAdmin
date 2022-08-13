using YiSha.Util;

namespace YiSha.Entity.IDGenerator
{
    /// <summary>
    /// 生成数据库主键Id
    /// </summary>
    public class IDGeneratorHelper
    {
        private Snowflake snowflake;

        private static readonly IDGeneratorHelper instance = new IDGeneratorHelper();

        private IDGeneratorHelper()
        {
            var snowFlakeWorkerId = GlobalContext.SystemConfig.SnowFlakeWorkerId;
            snowflake = new Snowflake(snowFlakeWorkerId, 0, 0);
        }

        public static IDGeneratorHelper Instance
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
