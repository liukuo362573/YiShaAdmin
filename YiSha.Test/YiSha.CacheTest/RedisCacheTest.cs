using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Diagnostics;
using NUnit.Framework;
using YiSha.Cache.Factory;
using YiSha.Util;
using YiSha.Util.Model;
using YiSha.Business;
using YiSha.Business.SystemManage;
using YiSha.Entity.SystemManage;

namespace YiSha.CacheTest
{
    public class RedisCacheTest
    {
        [SetUp]
        public void Init()
        {
            GlobalContext.SystemConfig = new SystemConfig
            {
                DBProvider = "MySql",
                DBConnectionString = "server=localhost;database=YiShaAdmin;user=root;password=123456;port=3306;",

                CacheProvider = "Redis",
                RedisConnectionString = "127.0.0.1:6379"
            };
        }

        [Test]
        public void TestRedisSimple()
        {
            string key = "test_simple_key";
            string value = "test_simple_value";
            CacheFactory.Cache.SetCache<string>(key, value);

            Assert.AreEqual(value, CacheFactory.Cache.GetCache<string>(key));
        }

        [Test]
        public void TestRedisComplex()
        {
            string key = "test_complex_key";
            TData<string> value = new TData<string> { Tag = 1, Data = "测试Redis" };
            CacheFactory.Cache.SetCache<TData<string>>(key, value);

            var result = CacheFactory.Cache.GetCache<TData<string>>(key);
            if (result.Tag == 1)
            {
                Assert.Pass(nameof(TestRedisComplex));
            }
            else
            {
                Assert.Fail(nameof(TestRedisComplex));
            }
        }

        [Test]
        public async Task TestRedisPerformance()
        {
            LogLoginBLL logLoginBLL = new LogLoginBLL();
            var obj = await logLoginBLL.GetList(null);

            string key = "test_performance_key";
            Stopwatch sw = new Stopwatch();
            sw.Start();
            CacheFactory.Cache.SetCache<List<LogLoginEntity>>(key, obj.Data);
            sw.Stop();
            Console.WriteLine(nameof(TestRedisPerformance) + " Redis Write Time:" + sw.ElapsedMilliseconds + " ms");

            sw.Restart();
            var result = CacheFactory.Cache.GetCache<List<LogLoginEntity>>(key);
            sw.Stop();
            if (obj.Data.Count == result.Count)
            {
                Console.WriteLine(nameof(TestRedisPerformance) + " Redis Read Time:" + sw.ElapsedMilliseconds + " ms");
            }
            else
            {
                Assert.Fail(nameof(TestRedisPerformance));
            }
        }
    }
}