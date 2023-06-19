using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using YiSha.Util.Extension;

namespace YiSha.Util
{
    /// <summary>
    /// JsonHelper
    /// </summary>
    public static class JsonHelper
    {
        /// <summary>
        /// ToObject
        /// </summary>
        public static T ToObject<T>(this string Json)
        {
            Json = Json.Replace("&nbsp;", "");
            return Json == null ? default(T) : JsonConvert.DeserializeObject<T>(Json);
        }

        /// <summary>
        /// ToJObject
        /// </summary>
        public static JObject ToJObject(this string Json)
        {
            return Json == null ? JObject.Parse("{}") : JObject.Parse(Json.Replace("&nbsp;", ""));
        }
    }
}
