using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using YiSha.Util;
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
                DBProvider = "MySql",
                DBConnectionString = "server=47.92.169.229;database=yisha_admin;user=root;password=Yisha*20190901;port=3306;",
                DBCommandTimeout = 180,
                DBBackup = "DataBase"
            };
        }

        [OneTimeTearDown]
        public void OneTimeTearDown()
        {

        }
    }
}
