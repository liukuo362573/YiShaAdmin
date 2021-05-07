﻿using Microsoft.AspNetCore.Html;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using YiSha.Util.Extension;
using YiSha.Util.Model;

namespace YiSha.Util.Helper
{
    /// <summary>
    /// Adapted from https://github.com/madskristensen/BundlerMinifier/wiki/Unbundling-scripts-for-debugging
    ///
    /// Areas modified:
    ///  - Modified it to make it work with aspnetcore.
    ///  - Accept both scripts and styles.
    ///  - Read config from wwwroot
    ///  - Accept baseFolder since DI not suited for static methods
    ///  - Style nitpicks
    /// </summary>
    public static class BundlerHelper
    {
        private static readonly long _version = DateTime.Now.Ticks;

        public static HtmlString Render(string baseFolder, string bundlePath)
        {
            if (bundlePath == null) throw new ArgumentNullException(nameof(bundlePath));
            string configFile = Path.Combine(baseFolder, "bundleconfig.json");
            Bundle bundle = GetBundle(configFile, bundlePath);
            if (bundle == null) return null;

            // Clean up the bundle to remove the virtual folder that aspnetcore provides.
            List<string> inputFiles = bundle.InputFiles;
            if (GlobalContext.SystemConfig.Debug)
            {
                inputFiles = inputFiles.Select(file => file.Replace("wwwroot", GlobalContext.SystemConfig.VirtualDirectory.ParseToString())).ToList();
            }
            List<string> outputString = bundlePath.EndsWith(".js") ? inputFiles.Select(inputFile => $"<script src='{inputFile}?v=" + _version + "' type='text/javascript' ></script>").ToList() : inputFiles.Select(inputFile => $"<link rel='stylesheet' href='{inputFile}?v=" + _version + "' />").ToList();

            return new HtmlString(string.Join("\n", outputString));
        }

        private static Bundle GetBundle(string configFile, string bundlePath)
        {
            if (GlobalContext.SystemConfig.Debug)
            {
                if (GlobalContext.SystemConfig.VirtualDirectory?.Length > 0)
                {
                    bundlePath = bundlePath.Replace(GlobalContext.SystemConfig.VirtualDirectory, string.Empty);
                }
                FileInfo file = new FileInfo(configFile);
                if (!file.Exists) return null;

                IEnumerable<Bundle> bundles = JsonConvert.DeserializeObject<IEnumerable<Bundle>>(File.ReadAllText(configFile));
                return (from b in bundles where b.OutputFileName.EndsWith(bundlePath, StringComparison.InvariantCultureIgnoreCase) select b).FirstOrDefault();
            }
            return new Bundle
            {
                InputFiles = new List<string> { bundlePath },
                OutputFileName = bundlePath
            };
        }

        private class Bundle
        {
            [JsonProperty("outputFileName")]
            public string OutputFileName { get; set; }

            [JsonProperty("inputFiles")]
            public List<string> InputFiles { get; set; } = new();
        }
    }
}