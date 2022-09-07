﻿namespace YiSha.Model
{
    /// <summary>
    /// 数据传输对象
    /// </summary>
    public class TData
    {
        /// <summary>
        /// 操作结果，Tag为1代表成功，0代表失败，其他的验证返回结果，可根据需要设置
        /// </summary>
        public int Tag { get; set; } = 0;

        /// <summary>
        /// 提示信息或异常信息
        /// </summary>
        public string Message { get; set; } = "";

        /// <summary>
        /// 扩展 Message
        /// </summary>
        public string Description { get; set; } = "";
    }

    /// <summary>
    /// 数据传输对象
    /// </summary>
    public class TData<T> : TData
    {
        /// <summary>
        /// 列表的记录数
        /// </summary>
        public int Total { get; set; } = 0;

        /// <summary>
        /// 数据
        /// </summary>
        public T? Data { get; set; } = default;
    }
}
