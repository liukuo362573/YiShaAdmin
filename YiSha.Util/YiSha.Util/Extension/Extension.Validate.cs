using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;

namespace YiSha.Util.Extension
{
    public static partial class Extensions
    {
        public static bool IsEmpty(this object value)
        {
            return value == null || value.ParseToString().Length == 0;
        }

        public static bool IsNullOrZero(this object value)
        {
            return value == null || value.ParseToString().Trim() == "0";
        }

        public static bool IsAjaxRequest(this HttpRequest request)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }
            return request.Headers != null && request.Headers["X-Requested-With"] == "XMLHttpRequest";
        }

        public static bool TryAny<TSource>(this IEnumerable<TSource> source)
        {
            return source != null && source.Any();
        }
    }
}