// Copyright (c) 2019 Sarin Na Wangkanai, All Rights Reserved.
// The Apache v2. See License.txt in the project root for license information.

namespace YiSha.Util.Browser
{
    public class Firefox : BaseBrowser
    {
        private readonly string _agent;

        public Firefox(string agent)
        {
            _agent = agent.ToLower();
            var firefox = BrowserType.Firefox.ToString().ToLower();

            if (_agent.Contains(firefox))
            {
                var first = _agent.IndexOf(firefox);
                var version = _agent.Substring(first + firefox.Length + 1);
                Version = ToVersion(version);
                Type = BrowserType.Firefox;
            }
        }
    }
}
