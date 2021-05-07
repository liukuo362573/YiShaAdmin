// Copyright (c) 2019 Sarin Na Wangkanai, All Rights Reserved.
// The Apache v2. See License.txt in the project root for license information.

using System;

namespace YiSha.Util.Browser
{
    public class Firefox : BaseBrowser
    {
        public Firefox(string agent)
        {
            if (string.IsNullOrEmpty(agent))
            {
                throw new ArgumentNullException(nameof(agent));
            }

            var lower = agent.ToLower();
            var firefox = BrowserType.Firefox.ToString().ToLower();

            if (lower.Contains(firefox))
            {
                var first = lower.IndexOf(firefox);
                var version = lower[(first + firefox.Length + 1)..];
                Version = ToVersion(version);
                Type = BrowserType.Firefox;
            }
        }
    }
}