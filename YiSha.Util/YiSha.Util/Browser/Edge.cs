// Copyright (c) 2019 Sarin Na Wangkanai, All Rights Reserved.
// The Apache v2. See License.txt in the project root for license information.

namespace YiSha.Util.Browser
{
    public class Edge : BaseBrowser
    {
        private readonly string _agent;

        public Edge(string agent)
        {
            _agent = agent.ToLower();
            var edge = BrowserType.Edge.ToString().ToLower();

            if (_agent.Contains(edge))
            {
                var first = _agent.IndexOf(edge);
                var version = _agent.Substring(first + edge.Length + 1);
                Version = ToVersion(version);
                Type = BrowserType.Edge;
            }
        }
    }
}
