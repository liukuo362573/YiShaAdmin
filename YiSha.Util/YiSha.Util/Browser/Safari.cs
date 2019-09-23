// Copyright (c) 2019 Sarin Na Wangkanai, All Rights Reserved.
// The Apache v2. See License.txt in the project root for license information.

namespace YiSha.Util.Browser
{
    public class Safari : BaseBrowser
    {
        private readonly string _agent;

        public Safari(string agent)
        {
            _agent = agent.ToLower();
            var safari = BrowserType.Safari.ToString().ToLower();

            if (_agent.Contains(safari))
            {
                var first = _agent.IndexOf(safari);
                var version = _agent.Substring(first + safari.Length + 1);
                Version = ToVersion(version);
                Type = BrowserType.Safari;
            }
        }
    }
}
