// Copyright (c) 2019 Sarin Na Wangkanai, All Rights Reserved.
// The Apache v2. See License.txt in the project root for license information.

namespace YiSha.Util.Browser
{
    public class InternetExplorer : BaseBrowser
    {
        private readonly string _agent;

        public InternetExplorer(string agent)
        {
            _agent = agent.ToLower();

            var ie10 = "msie";
            var rv = "rv:";
            if (_agent.Contains(ie10))
            {
                var first = _agent.IndexOf(ie10);
                var cut = _agent.Substring(first + ie10.Length + 1);
                var version = cut.Substring(0, cut.IndexOf(';'));
                Version = ToVersion(version);
                Type = BrowserType.IE;
            }

            if (_agent.Contains("ie 11.0"))
            {
                Type = BrowserType.IE;
                Version = new System.Version("11.0");
            }

            if (_agent.Contains(rv) && _agent.Contains("trident"))
            {
                var first = _agent.IndexOf(rv);
                var last = _agent.IndexOf(")", first);
                if (first > 0 && last > 0)
                {
                    Type = BrowserType.IE;
                    var version = _agent.Substring(first + rv.Length, last - first - rv.Length);
                    Version = new System.Version(version);
                }
            }
        }
    }
}
