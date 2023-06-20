using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace YiSha.Util
{
    /// <summary>
    /// Int64 序列化
    /// </summary>
    public class JsonLong : JsonConverter<long>
    {
        public override long Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType == JsonTokenType.String)
            {
                if (long.TryParse(reader.GetString(), out long data)) return data;
            }
            return reader.GetInt64();
        }

        public override void Write(Utf8JsonWriter writer, long value, JsonSerializerOptions options)
        {
            var zhCN = new CultureInfo("zh-CN");//时区
            var setValue = value.ToString(zhCN);
            writer?.WriteStringValue(setValue);
        }
    }
}
