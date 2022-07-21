namespace YiSha.Util.IDGenerator
{
    /// <summary>
    /// 生成数据库主键Id
    /// </summary>
    public class IDGeneratorHelper
    {
        private int SnowFlakeWorkerId = GlobalContext.SystemConfig.SnowFlakeWorkerId;

        private Snowflake snowflake;

        private static readonly IDGeneratorHelper instance = new IDGeneratorHelper();

        private IDGeneratorHelper()
        {
            snowflake = new Snowflake(SnowFlakeWorkerId, 0, 0);
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
