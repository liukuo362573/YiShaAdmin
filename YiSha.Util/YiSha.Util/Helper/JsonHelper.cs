using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using YiSha.Util.Extension;

namespace YiSha.Util.Helper
{
    #region JsonHelper

    public static class JsonHelper
    {
        public static T ToObject<T>(this string json)
        {
            if (string.IsNullOrEmpty(json))
            {
                throw new ArgumentNullException(nameof(json));
            }
            json = json.Replace("&nbsp;", "");
            return JsonConvert.DeserializeObject<T>(json);
        }

        public static JObject ToJObject(this string json)
        {
            return json == null ? JObject.Parse("{}") : JObject.Parse(json.Replace("&nbsp;", ""));
        }
    }

    #endregion JsonHelper

    #region JsonConverter

    /// <summary>
    /// Json数据返回到前端js的时候，把数值很大的long类型转成字符串
    /// </summary>
    public class StringJsonConverter : JsonConverter
    {
        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            return reader?.Value.ParseToLong();
        }

        public override bool CanConvert(Type objectType)
        {
            return true;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            if (writer == null)
            {
                throw new ArgumentNullException(nameof(writer));
            }
            if (value == null)
            {
                writer.WriteNull();
            }
            else
            {
                writer.WriteValue(value.ToString());
            }
        }
    }

    /// <summary>
    /// DateTime类型序列化的时候，转成指定的格式
    /// </summary>
    public class DateTimeJsonConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return true;
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            return reader?.Value.ParseToString().ParseToDateTime();
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            if (writer == null)
            {
                throw new ArgumentNullException(nameof(writer));
            }
            if (value == null)
            {
                writer.WriteNull();
                return;
            }
            if (!(value is DateTime dt))
            {
                writer.WriteNull();
                return;
            }
            writer.WriteValue(dt.ToString("yyyy-MM-dd HH:mm:ss"));
        }
    }

    #endregion JsonConverter
}