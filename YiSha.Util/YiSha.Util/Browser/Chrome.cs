// Copyright (c) 2019 Sarin Na Wangkanai, All Rights Reserved.
// The Apache v2. See License.txt in the project root for license information.

using System;

namespace YiSha.Util.Browser
{
    public class Chrome : BaseBrowser
    {
        public Chrome(string agent)
        {
            if (string.IsNullOrEmpty(agent))
            {
                throw new ArgumentNullException(nameof(agent));
            }

            var lower = agent.ToLower();
            var chrome = BrowserType.Chrome.ToString().ToLower();

            if (lower.Contains(chrome))
            {
                var first = lower.IndexOf(chrome);
                var cut = lower[(first + chrome.Length + 1)..];
                var version = cut.Substring(0, cut.IndexOf(' '));
                Version = ToVersion(version);
                Type = BrowserType.Chrome;
            }
        }
    }
}