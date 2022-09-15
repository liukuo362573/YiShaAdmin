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
        /// 表的主键
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
        [Column("BaseCreatorId"), Description("创建人ID")]
        public long BaseCreatorId { get; set; }

        /// <summary>
        /// 数据更新版本，控制并发
        /// </summary>
        [Column("BaseVersion"), Description("数据版本")]
        public int BaseVersion { get; set; }

        /// <summary>
        /// 修改时间
        /// </summary>
        [Column("BaseModifyTime"), Description("修改时间")]
        public DateTime BaseModifyTime { get; set; }

        /// <summary>
        /// 修改人ID
        /// </summary>
        [Column("BaseModifierId"), Description("修改人ID")]
        public long BaseModifierId { get; set; }

        /// <summary>
        /// 是否删除(1是，0否)
        /// </summary>
        [Column("BaseIsDelete"), JsonIgnore, Description("是否删除")]
        public int BaseIsDelete { get; set; }
    }
}
