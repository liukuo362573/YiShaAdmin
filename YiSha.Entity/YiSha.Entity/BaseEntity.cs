using System;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel;
using System.Threading.Tasks;
using YiSha.Util;
using YiSha.Web.Code;
using YiSha.IdGenerator;

namespace YiSha.Entity
{
    /// <summary>
    /// 数据库实体的基类，所有的数据库实体属性类型都是可空值类型，为了在做条件查询的时候进行判断
    /// 虽然是可空值类型，null的属性值，在底层会根据属性类型赋值默认值，字符串是string.empty，数值是0，日期是0001-01-01
    /// </summary>
    public class BaseEntity
    {
        /// <summary>
        /// 所有表的主键
        /// long返回到前端js的时候，会丢失精度，所以转成字符串
        /// </summary>
        [JsonConverter(typeof(StringJsonConverter))]
        public long? Id { get; set; }

        /// <summary>
        /// 是否删除 1是，0否
        /// </summary>
        [JsonIgnore]
        public int? BaseIsDelete { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        [JsonConverter(typeof(DateTimeJsonConverter))]
        [Description("创建时间")]
        public DateTime? BaseCreateTime { get; set; }

        /// <summary>
        /// 修改时间
        /// </summary>
        [JsonConverter(typeof(DateTimeJsonConverter))]
        [Description("修改时间")]
        public DateTime? BaseModifyTime { get; set; }

        /// <summary>
        /// 创建人ID
        /// </summary>
        public long? BaseCreatorId { get; set; }

        /// <summary>
        /// 修改人ID
        /// </summary>
        public long? BaseModifierId { get; set; }

        /// <summary>
        /// 版本
        /// </summary>
        public int? BaseVersion { get; set; }

        /// <summary>
        /// WebApi没有Cookie和Session，所以需要传入Token来标识用户身份
        /// </summary>
        [NotMapped]
        public string Token { get; set; }

        public async Task Create()
        {
            this.Id = IdGeneratorHelper.Instance.GetId();

            this.BaseIsDelete = 0;
            this.BaseVersion = 0;
            if (this.BaseCreateTime == null)
            {
                this.BaseCreateTime = DateTime.Now;
            }
            if (this.BaseModifyTime == null)
            {
                this.BaseModifyTime = DateTime.Now;
            }
            if (this.BaseCreatorId == null || this.BaseModifierId == null)
            {
                OperatorInfo user = await Operator.Instance.Current(Token);
                if (user != null)
                {
                    this.BaseCreatorId = user.UserId;
                    this.BaseModifierId = user.UserId;
                }
                else
                {
                    if (this.BaseCreatorId == null)
                    {
                        this.BaseCreatorId = 0;
                    }
                    if (this.BaseModifierId == null)
                    {
                        this.BaseModifierId = 0;
                    }
                }
            }
        }
        public async Task Modify()
        {
            this.BaseModifyTime = DateTime.Now;

            if (this.BaseCreatorId == null || this.BaseModifierId == null)
            {
                OperatorInfo user = await Operator.Instance.Current();
                if (user != null)
                {
                    this.BaseModifierId = user.UserId;
                }
                else
                {
                    if (this.BaseModifierId == null)
                    {
                        this.BaseModifierId = 0;
                    }
                }
            }
        }
    }

    public class BaseEntityExtension
    {
        public static string[] BaseFields = new string[]
        {
            "id",
            "base_is_delete",
            "base_create_time",
            "base_modify_time",
            "base_creator_id",
            "base_modifier_id",
            "base_version"
        };
    }


}
