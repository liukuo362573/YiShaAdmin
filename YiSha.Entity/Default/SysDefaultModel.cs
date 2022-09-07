using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using YiSha.Common.IDGenerator;

namespace YiSha.Entity.Default
{
    /// <summary>
    /// 系统表默认字段
    /// </summary>
    public class SysDefaultModel
    {
        /// <summary>
        /// 所有表的主键
        /// long返回到前端js的时候，会丢失精度，所以转成字符串
        /// </summary>
        [Key, Column("Id"), DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        [Column("BaseCreateTime"), Description("创建时间")]
        public DateTime BaseCreateTime { get; set; }

        /// <summary>
        /// 创建人ID
        /// </summary>
        [Column("BaseCreatorId")]
        public long BaseCreatorId { get; set; }

        /// <summary>
        /// 数据更新版本，控制并发
        /// </summary>
        [Column("BaseVersion")]
        public int BaseVersion { get; set; }

        /// <summary>
        /// 修改时间
        /// </summary>
        [Column("BaseModifyTime"), Description("修改时间")]
        public DateTime BaseModifyTime { get; set; }

        /// <summary>
        /// 修改人ID
        /// </summary>
        [Column("BaseModifierId")]
        public long BaseModifierId { get; set; }

        /// <summary>
        /// 是否删除(1是，0否)
        /// </summary>
        [Column("BaseIsDelete"), JsonIgnore]
        public int BaseIsDelete { get; set; }

        /// <summary>
        /// WebApi 没有 Cookie 和 Session，所以需要传入 Token 来标识用户身份
        /// </summary>
        [NotMapped]
        public string Token { get; set; }

        /// <summary>
        /// 创建
        /// </summary>
        public async Task Create()
        {
            this.Id = IDGeneratorHelper.Instance.GetId();

            this.BaseIsDelete = 0;

            if (this.BaseCreateTime == default)
            {
                this.BaseCreateTime = DateTime.Now;
            }

            //if (this.BaseCreatorId == default)
            //{
            //    var user = await Operator.Instance.Current(Token);
            //    this.BaseCreatorId = user != null ? user.UserId : 0;
            //}
        }

        /// <summary>
        /// 调整
        /// </summary>
        public async Task Modify()
        {
            this.BaseVersion = 0;
            this.BaseModifyTime = DateTime.Now;

            //if (this.BaseModifierId == default)
            //{
            //    var user = await Operator.Instance.Current();
            //    this.BaseModifierId = user != null ? user.UserId : 0;
            //}
        }
    }

    /// <summary>
    /// 基础字段
    /// </summary>
    public class BaseField
    {
        /// <summary>
        /// 基础字段List
        /// </summary>
        public static string[] BaseFieldList { get; } = new string[]
        {
            "Id",
            "BaseIsDelete",
            "BaseCreateTime",
            "BaseModifyTime",
            "BaseCreatorId",
            "BaseModifierId",
            "BaseVersion",
        };
    }
}
