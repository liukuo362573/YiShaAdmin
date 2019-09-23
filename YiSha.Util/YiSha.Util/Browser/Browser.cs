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

        public BaseBrowser() { }
        public BaseBrowser(BrowserType browserType)
            => Type = browserType;
        public BaseBrowser(BrowserType browserType, Version version)
            : this(browserType)
            => Version = version;

        public BaseBrowser(string name)
        {
            BrowserType type;

            if (!System.Enum.TryParse(name, true, out type))
                throw new BrowserNotFoundException(name, "not found");

            Type = type;
        }
        public Version ToVersion(string version)
        {
            version = RemoveWhitespace(version);

            return Version.TryParse(version, out var parsedVersion) ?
                   parsedVersion :
                   new Version(0, 0);
        }
        public string RemoveWhitespace(string version) => version.Contains(" ") ? version.Replace(" ", "") : version;
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

    public class BrowserNotFoundException : ArgumentException
    {
        private readonly string _invalidBrowserName; // unrecognized browser name
        public virtual string InvalidBrowserName => _invalidBrowserName;
        private static string DefaultMessage => "Browser Not Supported";

        public BrowserNotFoundException()
            : base(DefaultMessage) { }
        public BrowserNotFoundException(string message)
            : base(message) { }
        public BrowserNotFoundException(string paramName, string message)
            : base(message, paramName) { }
        public BrowserNotFoundException(string message, Exception innerException)
            : base(message, innerException) { }
        public BrowserNotFoundException(string message, string invalidBrowserName, Exception innerException)
            : base(message, innerException)
        {
            _invalidBrowserName = invalidBrowserName;
        }
        public BrowserNotFoundException(string paramName, string invalidBrowserName, string message)
            : base(message, paramName)
        {
            _invalidBrowserName = invalidBrowserName;
        }

        public override string Message
        {
            get
            {
                var s = base.Message;
                if (_invalidBrowserName != null)
                    return s + Environment.NewLine + InvalidBrowserName;

                return s;
            }
        }
    }
}