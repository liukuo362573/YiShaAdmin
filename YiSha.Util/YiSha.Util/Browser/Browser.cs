// Copyright (c) 2019 Sarin Na Wangkanai, All Rights Reserved.
// The Apache v2. See License.txt in the project root for license information.

using System;

namespace YiSha.Util.Browser
{
    public class BaseBrowser
    {
        public string Name { get; set; }

        public string Maker { get; set; }

        public BrowserType Type { get; set; } = BrowserType.Generic;

        public Version Version { get; set; }

        protected BaseBrowser() { }

        public BaseBrowser(BrowserType browserType) => Type = browserType;

        public BaseBrowser(BrowserType browserType, Version version) : this(browserType) => Version = version;

        protected static Version ToVersion(string version)
        {
            version = RemoveWhitespace(version);
            return Version.TryParse(version, out var parsedVersion) ? parsedVersion : new Version(0, 0);
        }

        public static string RemoveWhitespace(string version) => version != null && version.Contains(" ") ? version.Replace(" ", "") : version;
    }

    public enum BrowserType
    {
        IE,
        Chrome,
        Safari,
        Firefox,
        Edge,
        Opera,
        Generic
    }

    public static class BrowserHelper
    {
        public static string GetBrowserInfo(string userAgent)
        {
            var ie = new InternetExplorer(userAgent);
            if (ie.Type == BrowserType.IE) return $"{ie.Type.ToString()} {ie.Version}";
            var firefox = new Firefox(userAgent);
            if (firefox.Type == BrowserType.Firefox) return $"{firefox.Type.ToString()} {firefox.Version}";
            var edge = new Edge(userAgent);
            if (edge.Type == BrowserType.Edge) return $"{edge.Type.ToString()} {edge.Version}";
            var opera = new Opera(userAgent);
            if (opera.Type == BrowserType.Opera) return $"{opera.Type.ToString()} {opera.Version}";
            var chrome = new Chrome(userAgent);
            if (chrome.Type == BrowserType.Chrome) return $"{chrome.Type.ToString()} {chrome.Version}";
            var safari = new Safari(userAgent);
            if (safari.Type == BrowserType.Safari) return $"{safari.Type.ToString()} {safari.Version}";
            return string.Empty;
        }
    }
}