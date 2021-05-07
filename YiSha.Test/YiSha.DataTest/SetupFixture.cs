using NUnit.Framework;
using YiSha.Util.Model;

namespace YiSha.DataTest
{
    [SetUpFixture]
    public class SetupFixture
    {
        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            GlobalContext.SystemConfig = new SystemConfig
            {
                DbProvider = "MySql",
                DbConnectionString = "server=localhost;database=YiShaAdmin;user=root;password=123456;port=3306;",
                DbCommandTimeout = 180,
                DbBackup = "DataBase"
            };
        }

        [OneTimeTearDown]
        public void OneTimeTearDown() { }
    }
}