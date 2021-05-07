// Copyright (c) 2019 Sarin Na Wangkanai, All Rights Reserved.
// The Apache v2. See License.txt in the project root for license information.

using System;

namespace YiSha.Util.Browser
{
    public class Opera : BaseBrowser
    {
        public Opera(string agent)
        {
            if (string.IsNullOrEmpty(agent))
            {
                throw new ArgumentNullException(nameof(agent));
            }

            var lower = agent.ToLower();
            var opera12 = BrowserType.Opera.ToString().ToLower();

            if (lower.Contains(opera12))
            {
                var first = lower.IndexOf("version");
                var version = lower[(first + "version".Length + 1)..];
                Version = ToVersion(version);
                Type = BrowserType.Opera;
            }

            var opera15 = "opr";
            if (lower.Contains(opera15))
            {
                var first = lower.IndexOf(opera15);
                var version = lower[(first + opera15.Length + 1)..];
                Version = ToVersion(version);
                Type = BrowserType.Opera;
            }
        }
    }
}