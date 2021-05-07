// Copyright (c) 2019 Sarin Na Wangkanai, All Rights Reserved.
// The Apache v2. See License.txt in the project root for license information.

using System;

namespace YiSha.Util.Browser
{
    public class Safari : BaseBrowser
    {
        public Safari(string agent)
        {
            if (string.IsNullOrEmpty(agent))
            {
                throw new ArgumentNullException(nameof(agent));
            }

            var lower = agent.ToLower();
            var safari = BrowserType.Safari.ToString().ToLower();

            if (lower.Contains(safari))
            {
                var first = lower.IndexOf(safari);
                var version = lower[(first + safari.Length + 1)..];
                Version = ToVersion(version);
                Type = BrowserType.Safari;
            }
        }
    }
}