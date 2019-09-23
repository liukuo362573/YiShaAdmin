using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using YiSha.Util.Extension;
using Newtonsoft.Json;

namespace YiSha.FrontWeb.Controllers
{
    public class ToolController : Controller
    {
        public IActionResult Ip()
        {
            Dictionary<string, string> dict = new Dictionary<string, string>();
            string ip = HttpContext?.Connection?.RemoteIpAddress.ParseToString();

            if (HttpContext != null && HttpContext.Request != null)
            {
                foreach (var header in HttpContext.Request.Headers)
                {
                    dict.Add(header.Key, header.Value);
                }
            }
            ViewBag.Ip = ip;
            ViewBag.Header = dict;
            return View();
        }
    }
}