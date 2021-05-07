// Copyright (c) 2019 Sarin Na Wangkanai, All Rights Reserved.
// The Apache v2. See License.txt in the project root for license information.

using System;

namespace YiSha.Util.Browser
{
    public class InternetExplorer : BaseBrowser
    {
        public InternetExplorer(string agent)
        {
            if (string.IsNullOrEmpty(agent))
            {
                throw new ArgumentNullException(nameof(agent));
            }

            var lower = agent.ToLower();

            var ie10 = "msie";
            var rv = "rv:";
            if (lower.Contains(ie10))
            {
                var first = lower.IndexOf(ie10);
                var cut = lower[(first + ie10.Length + 1)..];
                var version = cut.Substring(0, cut.IndexOf(';'));
                Version = ToVersion(version);
                Type = BrowserType.IE;
            }

            if (lower.Contains("ie 11.0"))
            {
                Type = BrowserType.IE;
                Version = new Version("11.0");
            }

            if (lower.Contains(rv) && lower.Contains("trident"))
            {
                var first = lower.IndexOf(rv);
                var last = lower.IndexOf(")", first);
                if (first > 0 && last > 0)
                {
                    Type = BrowserType.IE;
                    var version = lower.Substring(first + rv.Length, last - first - rv.Length);
                    Version = new Version(version);
                }
            }
        }
    }
}