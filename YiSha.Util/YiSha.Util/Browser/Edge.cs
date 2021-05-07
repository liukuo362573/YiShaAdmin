// Copyright (c) 2019 Sarin Na Wangkanai, All Rights Reserved.
// The Apache v2. See License.txt in the project root for license information.

using System;

namespace YiSha.Util.Browser
{
    public class Edge : BaseBrowser
    {
        public Edge(string agent)
        {
            if (string.IsNullOrEmpty(agent))
            {
                throw new ArgumentNullException(nameof(agent));
            }

            var lower = agent.ToLower();
            var edge = BrowserType.Edge.ToString().ToLower();

            if (lower.Contains(edge))
            {
                var first = lower.IndexOf(edge);
                var version = lower[(first + edge.Length + 1)..];
                Version = ToVersion(version);
                Type = BrowserType.Edge;
            }
        }
    }
}