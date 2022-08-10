using System.Text.Json;
using System.Text.Json.Serialization;

namespace YiSha.Util
{
    /// <summary>
    /// Json 工具
    /// </summary>
    public static class JsonHelper
    {
        public static T? ToObject<T>(this string Json)
        {
            Json = Json.Replace("&nbsp;", "");
            return Json == null ? default : Newtonsoft.Json.JsonConvert.DeserializeObject<T>(Json);
        }

        public static Newtonsoft.Json.Linq.JObject ToJObject(this string Json)
        {
            return Json == null ? Newtonsoft.Json.Linq.JObject.Parse("{}") : Newtonsoft.Json.Linq.JObject.Parse(Json.Replace("&nbsp;", ""));
        }
    }

    /// <summary>
    /// Long Json 格式化
    /// </summary>
    public class LongJsonConverter : JsonConverter<long>
    {
        /// <summary>
        /// 读取
        /// </summary>
        /// <param name="reader">读</param>
        /// <param name="typeToConvert">类型</param>
        /// <param name="options">选项</param>
        /// <returns></returns>
        public override long Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType == JsonTokenType.String)
            {
                if (long.TryParse(reader.GetString(), out long data)) return data;
            }
            return reader.GetInt64();
        }

        /// <summary>
        /// 写入
        /// </summary>
        /// <param name="writer">写</param>
        /// <param name="value">值</param>
        /// <param name="options">选项</param>
        public override void Write(Utf8JsonWriter writer, long value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value.ToString());
        }
    }

    /// <summary>
    /// 时间 Json 格式化
    /// </summary>
    public class DateTimeJsonConverter : JsonConverter<DateTime>
    {
        /// <summary>
        /// 读取
        /// </summary>
        /// <param name="reader">读</param>
        /// <param name="typeToConvert">类型</param>
        /// <param name="options">选项</param>
        /// <returns></returns>
        public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType == JsonTokenType.String)
            {
                if (DateTime.TryParse(reader.GetString(), out DateTime data)) return data;
            }
            return reader.GetDateTime();
        }

        /// <summary>
        /// 写入
        /// </summary>
        /// <param name="writer">写</param>
        /// <param name="value">值</param>
        /// <param name="options">选项</param>
        public override void Write(Utf8JsonWriter writer, DateTime value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value.ToString("yyyy-MM-dd HH:mm:ss"));
        }
    }
}
