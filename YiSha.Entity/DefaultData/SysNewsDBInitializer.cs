using YiSha.Entity.Models;
using YiSha.Util;

namespace YiSha.Entity.DefaultData
{
    /// <summary>
    /// 新闻表数据初始化
    /// </summary>
    internal class SysNewsDBInitializer
    {
        /// <summary>
        /// 获取数据
        /// </summary>
        public static List<SysNews> GetData
        {
            get
            {
                var lists = new List<SysNews>();

                lists.Add(new SysNews()
                {
                    Id = 34571912667467776,
                    BaseIsDelete = 0,
                    BaseCreateTime = "2022-01-01 00:00:00".ToDate(),
                    BaseModifyTime = "2022-01-01 00:00:00".ToDate(),
                    BaseCreatorId = 16508640061130151,
                    BaseModifierId = 16508640061130151,
                    BaseVersion = 0,
                    NewsTitle = "UHC健康会",
                    NewsContent = "<p>UHC 健康会，您的健康管家，为您的健康保驾护航。</p><p>\n        <img src='http://localhost:5001/api/Resource/News/2019/07/31/8722abb613cd46b4af5b4ded7ddf5fad.jpg' data-filename='/' style='width: 550px;'>\n    </p><p>\n        <img src='http://localhost:5001/api/Resource/News/2019/07/31/1ffc4edd922e4cb195744c13f9eec636.jpg' data-filename='/' style='width: 550px;'>\n    </p><p>\n        <img src='http://localhost:5001/api/Resource/News/2019/07/31/e0728828482542f099ab79ba7d3ef701.jpg' data-filename='/' style='width: 550px;'>\n\n    </p><p>\n        <img src='http://localhost:5001/api/Resource/News/2019/07/31/57b1153fbf514d9384ba9837a46737cf.jpg' data-filename='/' style='width: 550px;'>\n        <br>\n    </p>\n    <p>小程序码</p><p>\n        <img src='http://localhost:5001/api/Resource/News/2019/07/31/46ece527595a408e9e62b2334374b560.jpg' data-filename='/' style='width: 430px;'>\n        <br>\n    </p><p><br></p>",
                    NewsTag = "微信小程序，健康会",
                    ProvinceId = 0,
                    CityId = 0,
                    CountyId = 0,
                    ThumbImage = "http://localhost:5001/api/Resource/News/2019/07/31/eee642de4d3443779c0670e0da8eeed7.png",
                    NewsAuthor = "管理员",
                    NewsSort = 1,
                    NewsDate = "2022-01-01 00:00:00".ToDate(),
                    NewsType = 1,
                    ViewTimes = 138,
                });

                lists.Add(new SysNews()
                {
                    Id = 76797547762421760,
                    BaseIsDelete = 0,
                    BaseCreateTime = "2022-01-01 00:00:00".ToDate(),
                    BaseModifyTime = "2022-01-01 00:00:00".ToDate(),
                    BaseCreatorId = 16508640061130151,
                    BaseModifierId = 16508640061130151,
                    BaseVersion = 0,
                    NewsTitle = "58名师",
                    NewsContent = "<p>汇聚同城的各科教师，找老师，就上58名师！</p><p><img src='http://localhost:5001/api/Resource/News/2019/07/31/b0316da26b5546d1bf07fcd05e8889e0.png' data-filename='/' style='width: 270px;'></p><p><br></p><p><img src='http://localhost:5001/api/Resource/News/2019/07/31/ad812ef0248a4fcb94edfa69d1ee5a66.png' data-filename='/' style='width: 269px;'></p><p><img src='http://localhost:5001/api/Resource/News/2019/07/31/b2921e1e08e04b0d92b29485356eacb1.png' data-filename='/' style='width: 273px;'></p><p><img src='http://localhost:5001/api/Resource/News/2019/07/31/a1ee188e3d3b462c8f401a9205ddd10e.png' data-filename='/' style='width: 274px;'></p><p>小程序码</p><p><img src='http://localhost:5001/api/Resource/News/2019/07/31/1c30e2bb681448bab52e566993b82472.jpg' data-filename='/' style='width: 430px;'><br></p><p><br></p>",
                    NewsTag = "微信小程序，找老师",
                    ProvinceId = 340000,
                    CityId = 340100,
                    CountyId = 340172,
                    ThumbImage = "http://localhost:5001/api/Resource/News/2019/07/31/a627c3eed0ca428391fa62a841652ea4.png",
                    NewsAuthor = "管理员",
                    NewsSort = 2,
                    NewsDate = "2022-01-01 00:00:00".ToDate(),
                    NewsType = 1,
                    ViewTimes = 35,
                });

                return lists;
            }
        }
    }
}
