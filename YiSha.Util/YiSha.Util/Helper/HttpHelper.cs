using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.RegularExpressions;
using YiSha.Util.Extension;

namespace YiSha.Util.Helper
{
    /// <summary>
    /// Http连接操作帮助类
    /// </summary>
    public class HttpHelper
    {
        #region 是否是网址

        public static bool IsUrl(string url)
        {
            url = url.ParseToString().ToLower();
            return url.StartsWith("http://") || url.StartsWith("https://");
        }

        #endregion

        #region 模拟GET

        /// <summary>
        /// GET请求
        /// </summary>
        /// <param name="url">The URL.</param>
        /// <param name="postDataStr">The post data string.</param>
        /// <returns>System.String.</returns>
        public static string HttpGet(string url, int timeout = 10 * 1000)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "GET";
            request.ContentType = "text/html;charset=UTF-8";
            request.Timeout = timeout;

            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            using Stream myResponseStream = response.GetResponseStream();
            using StreamReader myStreamReader = new StreamReader(myResponseStream!, Encoding.GetEncoding("utf-8"));
            return myStreamReader.ReadToEnd();
        }

        #endregion

        #region 模拟POST

        /// <summary>
        /// POST请求
        /// </summary>
        /// <param name="posturl">The posturl.</param>
        /// <param name="postData">The post data.</param>
        /// <returns>System.String.</returns>
        public static string HttpPost(string posturl, string postData, string contentType = "application/x-www-form-urlencoded", int timeout = 10 * 1000)
        {
            Encoding encoding = Encoding.GetEncoding("utf-8");
            byte[] data = encoding.GetBytes(postData);
            // 准备请求...
            // 设置参数
            var request = WebRequest.Create(posturl) as HttpWebRequest;
            CookieContainer cookieContainer = new CookieContainer();
            request.CookieContainer = cookieContainer;
            request.AllowAutoRedirect = true;
            request.Method = "POST";
            request.ContentType = contentType;
            request.Timeout = timeout;
            request.ContentLength = data.Length;

            using var outstream = request.GetRequestStream();
            outstream.Write(data, 0, data.Length);

            //发送请求并获取相应回应数据
            var response = request.GetResponse() as HttpWebResponse;
            //直到request.GetResponse()程序才开始向目标网页发送Post请求
            using var instream = response.GetResponseStream();
            using var sr = new StreamReader(instream!, encoding);
            //返回结果网页（html）代码
            return sr.ReadToEnd();
        }

        /// <summary>
        /// 模拟httpPost提交表单
        /// </summary>
        /// <param name="url">POS请求的网址</param>
        /// <param name="data">表单里的参数和值</param>
        /// <param name="encoder">页面编码</param>
        public static string CreateAutoSubmitForm(string url, Dictionary<string, string> data, Encoding encoder)
        {
            StringBuilder html = new StringBuilder();
            html.AppendLine("<html>");
            html.AppendLine("<head>");
            html.AppendFormat("<meta http-equiv=\"Content-Type\" content=\"text/html; charset={0}\" />", encoder.BodyName);
            html.AppendLine("</head>");
            html.AppendLine("<body onload=\"OnLoadSubmit();\">");
            html.AppendFormat("<form id=\"pay_form\" action=\"{0}\" method=\"post\">", url);
            foreach (KeyValuePair<string, string> kvp in data)
            {
                html.AppendFormat("<input type=\"hidden\" name=\"{0}\" id=\"{0}\" value=\"{1}\" />", kvp.Key, kvp.Value);
            }
            html.AppendLine("</form>");
            html.AppendLine("<script type=\"text/javascript\">");
            html.AppendLine("<!--");
            html.AppendLine("function OnLoadSubmit()");
            html.AppendLine("{");
            html.AppendLine("document.getElementById(\"pay_form\").submit();");
            html.AppendLine("}");
            html.AppendLine("//-->");
            html.AppendLine("</script>");
            html.AppendLine("</body>");
            html.AppendLine("</html>");
            return html.ToString();
        }

        #endregion

        #region 预定义方法或者变更

        //默认的编码
        private Encoding _encoding = Encoding.Default;

        //HttpWebRequest对象用来发起请求
        private HttpWebRequest _request = null;

        //获取影响流的数据对象
        private HttpWebResponse _response = null;

        /// <summary>
        /// 根据相传入的数据，得到相应页面数据
        /// </summary>
        /// <param name="strPostdata">传入的数据Post方式,get方式传NUll或者空字符串都可以</param>
        /// <returns>string类型的响应数据</returns>
        private HttpResult GetHttpRequestData(HttpItem objhttpitem)
        {
            //返回参数
            HttpResult result = new HttpResult();
            try
            {
                #region 得到请求的response

                using (_response = (HttpWebResponse)_request.GetResponse())
                {
                    result.Header = _response.Headers;
                    if (_response.Cookies != null)
                    {
                        result.CookieCollection = _response.Cookies;
                    }
                    if (_response.Headers["set-cookie"] != null)
                    {
                        result.Cookie = _response.Headers["set-cookie"];
                    }

                    MemoryStream stream;
                    //GZIIP处理
                    if (_response.ContentEncoding.Equals("gzip", StringComparison.InvariantCultureIgnoreCase))
                    {
                        //开始读取流并设置编码方式
                        //new GZipStream(response.GetResponseStream(), CompressionMode.Decompress).CopyTo(_stream, 10240);
                        //.net4.0以下写法
                        stream = GetMemoryStream(new GZipStream(_response.GetResponseStream()!, CompressionMode.Decompress));
                    }
                    else
                    {
                        //开始读取流并设置编码方式
                        //response.GetResponseStream().CopyTo(_stream, 10240);
                        //.net4.0以下写法
                        stream = GetMemoryStream(_response.GetResponseStream());
                    }
                    //获取Byte
                    byte[] rawResponse = stream.ToArray();
                    //是否返回Byte类型数据
                    if (objhttpitem.ResultType == ResultType.Byte)
                    {
                        result.ResultByte = rawResponse;
                    }
                    //从这里开始我们要无视编码了
                    if (_encoding == null)
                    {
                        string temp = Encoding.Default.GetString(rawResponse, 0, rawResponse.Length);
                        //<meta(.*?)charset([\s]?)=[^>](.*?)>
                        Match meta = Regex.Match(temp, "<meta([^<]*)charset=([^<]*)[\"']", RegexOptions.IgnoreCase | RegexOptions.Multiline);
                        string charter = meta.Groups.Count > 2 ? meta.Groups[2].Value : string.Empty;
                        charter = charter.Replace("\"", string.Empty).Replace("'", string.Empty).Replace(";", string.Empty);
                        if (charter.Length > 0)
                        {
                            charter = charter.ToLower().Replace("iso-8859-1", "gbk");
                            _encoding = Encoding.GetEncoding(charter);
                        }
                        else
                        {
                            if (_response.CharacterSet.ToLower().Trim() == "iso-8859-1")
                            {
                                _encoding = Encoding.GetEncoding("gbk");
                            }
                            else
                            {
                                if (string.IsNullOrEmpty(_response.CharacterSet.Trim()))
                                {
                                    _encoding = Encoding.UTF8;
                                }
                                else
                                {
                                    _encoding = Encoding.GetEncoding(_response.CharacterSet);
                                }
                            }
                        }
                    }
                    //得到返回的HTML
                    result.Html = _encoding.GetString(rawResponse);
                    //最后释放流
                    stream.Close();
                }

                #endregion
            }
            catch (WebException ex)
            {
                //这里是在发生异常时返回的错误信息
                result.Html = "String Error";
                _response = (HttpWebResponse)ex.Response;
            }
            if (objhttpitem.IsToLower)
            {
                result.Html = result.Html.ToLower();
            }
            return result;
        }

        /// <summary>
        /// 4.0以下.net版本取数据使用
        /// </summary>
        /// <param name="streamResponse">流</param>
        private static MemoryStream GetMemoryStream(Stream streamResponse)
        {
            MemoryStream stream = new MemoryStream();
            int length = 256;
            byte[] buffer = new byte[length];
            int bytesRead = streamResponse.Read(buffer, 0, length);
            // write the required bytes
            while (bytesRead > 0)
            {
                stream.Write(buffer, 0, bytesRead);
                bytesRead = streamResponse.Read(buffer, 0, length);
            }
            return stream;
        }

        /// <summary>
        /// 为请求准备参数
        /// </summary>
        ///<param name="httpItem">参数列表</param>
        /// <param name="_Encoding">读取数据时的编码方式</param>
        private void SetRequest(HttpItem httpItem)
        {
            // 验证证书
            SetCer(httpItem);
            // 设置代理
            SetProxy(httpItem);
            //请求方式Get或者Post
            _request.Method = httpItem.Method;
            _request.Timeout = httpItem.Timeout;
            _request.ReadWriteTimeout = httpItem.ReadWriteTimeout;
            //Accept
            _request.Accept = httpItem.Accept;
            //ContentType返回类型
            _request.ContentType = httpItem.ContentType;
            //UserAgent客户端的访问类型，包括浏览器版本和操作系统信息
            _request.UserAgent = httpItem.UserAgent;
            // 编码
            SetEncoding(httpItem);
            //设置Cookie
            SetCookie(httpItem);
            //来源地址
            _request.Referer = httpItem.Referer;
            //是否执行跳转功能
            _request.AllowAutoRedirect = httpItem.Allowautoredirect;
            //设置Post数据
            SetPostData(httpItem);
            //设置最大连接
            if (httpItem.Connectionlimit > 0)
            {
                _request.ServicePoint.ConnectionLimit = httpItem.Connectionlimit;
            }
        }

        /// <summary>
        /// 设置证书
        /// </summary>
        private void SetCer(HttpItem objhttpItem)
        {
            if (objhttpItem.CerPath?.Length > 0)
            {
                //这一句一定要写在创建连接的前面。使用回调的方法进行证书验证。
                ServicePointManager.ServerCertificateValidationCallback = CheckValidationResult;
                //初始化对像，并设置请求的URL地址
                _request = (HttpWebRequest)WebRequest.Create(GetUrl(objhttpItem.Url));
                //创建证书文件
                X509Certificate objx509 = new X509Certificate(objhttpItem.CerPath);
                //添加到请求里
                _request.ClientCertificates.Add(objx509);
            }
            else
            {
                //初始化对像，并设置请求的URL地址
                _request = (HttpWebRequest)WebRequest.Create(GetUrl(objhttpItem.Url));
            }
        }

        /// <summary>
        /// 设置编码
        /// </summary>
        /// <param name="objhttpItem">Http参数</param>
        private void SetEncoding(HttpItem objhttpItem)
        {
            if (string.IsNullOrEmpty(objhttpItem.Encoding) || objhttpItem.Encoding.ToLower().Trim() == "null")
            {
                //读取数据时的编码方式
                _encoding = null;
            }
            else
            {
                //读取数据时的编码方式
                _encoding = Encoding.GetEncoding(objhttpItem.Encoding);
            }
        }

        /// <summary>
        /// 设置Cookie
        /// </summary>
        /// <param name="objhttpItem">Http参数</param>
        private void SetCookie(HttpItem objhttpItem)
        {
            if (objhttpItem.Cookie?.Length > 0)
            {
                //Cookie
                _request.Headers[HttpRequestHeader.Cookie] = objhttpItem.Cookie;
            }
            //设置Cookie
            if (objhttpItem.CookieCollection != null)
            {
                _request.CookieContainer = new CookieContainer();
                _request.CookieContainer.Add(objhttpItem.CookieCollection);
            }
        }

        /// <summary>
        /// 设置Post数据
        /// </summary>
        /// <param name="objhttpItem">Http参数</param>
        private void SetPostData(HttpItem objhttpItem)
        {
            //验证在得到结果时是否有传入数据
            if (_request.Method.Trim().ToLower().Contains("post"))
            {
                //写入Byte类型
                if (objhttpItem.PostDataType == PostDataType.Byte)
                {
                    //验证在得到结果时是否有传入数据
                    if (objhttpItem.PostdataByte != null && objhttpItem.PostdataByte.Length > 0)
                    {
                        _request.ContentLength = objhttpItem.PostdataByte.Length;
                        _request.GetRequestStream().Write(objhttpItem.PostdataByte, 0, objhttpItem.PostdataByte.Length);
                    }
                } //写入文件
                else if (objhttpItem.PostDataType == PostDataType.FilePath)
                {
                    StreamReader r = new StreamReader(objhttpItem.Postdata, _encoding);
                    byte[] buffer = Encoding.Default.GetBytes(r.ReadToEnd());
                    r.Close();
                    _request.ContentLength = buffer.Length;
                    _request.GetRequestStream().Write(buffer, 0, buffer.Length);
                }
                else
                {
                    //验证在得到结果时是否有传入数据
                    if (objhttpItem.Postdata?.Length > 0)
                    {
                        byte[] buffer = Encoding.Default.GetBytes(objhttpItem.Postdata);
                        _request.ContentLength = buffer.Length;
                        _request.GetRequestStream().Write(buffer, 0, buffer.Length);
                    }
                }
            }
        }

        /// <summary>
        /// 设置代理
        /// </summary>
        /// <param name="objhttpItem">参数对象</param>
        private void SetProxy(HttpItem objhttpItem)
        {
            if (objhttpItem.ProxyUserName?.Length > 0 || objhttpItem.ProxyPwd?.Length > 0 || objhttpItem.ProxyIp?.Length > 0)
            {
                //设置代理服务器
                WebProxy myProxy = new WebProxy(objhttpItem.ProxyIp, false);
                //建议连接
                myProxy.Credentials = new NetworkCredential(objhttpItem.ProxyUserName, objhttpItem.ProxyPwd);
                //给当前请求对象
                _request.Proxy = myProxy;
                //设置安全凭证
                _request.Credentials = CredentialCache.DefaultNetworkCredentials;
            }
        }

        /// <summary>
        /// 回调验证证书问题
        /// </summary>
        /// <param name="sender">流对象</param>
        /// <param name="certificate">证书</param>
        /// <param name="chain">X509Chain</param>
        /// <param name="errors">SslPolicyErrors</param>
        /// <returns>bool</returns>
        public bool CheckValidationResult(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors errors)
        {
            // 总是接受
            return true;
        }

        #endregion

        #region 普通类型

        /// <summary>
        /// 传入一个正确或不正确的URl，返回正确的URL
        /// </summary>
        public static string GetUrl(string url)
        {
            if (!(url.Contains("http://") || url.Contains("https://")))
            {
                url = "http://" + url;
            }
            return url;
        }

        ///<summary>
        ///采用https协议访问网络,根据传入的URl地址，得到响应的数据字符串。
        ///</summary>
        ///<param name="httpItem">参数列表</param>
        ///<returns>String类型的数据</returns>
        public HttpResult GetHtml(HttpItem httpItem)
        {
            //准备参数
            SetRequest(httpItem);
            //调用专门读取数据的类
            return GetHttpRequestData(httpItem);
        }

        public static bool IsUrl(Uri url)
        {
            throw new NotImplementedException();
        }

        public static string HttpGet(Uri url, int timeout)
        {
            throw new NotImplementedException();
        }

        #endregion
    }

    /// <summary>
    /// Http请求参考类
    /// </summary>
    public class HttpItem
    {
        /// <summary>
        /// 请求URL必须填写
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// 请求方式默认为GET方式
        /// </summary>
        public string Method { get; set; } = "GET";

        /// <summary>
        /// 默认请求超时时间
        /// </summary>
        public int Timeout { get; set; } = 100000;

        /// <summary>
        /// 默认写入Post数据超时间
        /// </summary>
        public int ReadWriteTimeout { get; set; } = 30000;

        /// <summary>
        /// 请求标头值 默认为text/html, application/xhtml+xml, */*
        /// </summary>
        public string Accept { get; set; } = "text/html, application/xhtml+xml, */*";

        /// <summary>
        /// 请求返回类型默认 text/html
        /// </summary>
        public string ContentType { get; set; } = "text/html";

        /// <summary>
        /// 客户端访问信息默认Mozilla/5.0 (compatible; MSIE 9.0; Windows NT 6.1; Trident/5.0)
        /// </summary>
        public string UserAgent { get; set; } = "Mozilla/5.0 (compatible; MSIE 9.0; Windows NT 6.1; Trident/5.0)";

        /// <summary>
        /// 返回数据编码默认为NUll,可以自动识别
        /// </summary>
        public string Encoding { get; set; } = string.Empty;

        /// <summary>
        /// Post的数据类型
        /// </summary>
        public PostDataType PostDataType { get; set; } = PostDataType.String;

        /// <summary>
        /// Post请求时要发送的字符串Post数据
        /// </summary>
        public string Postdata { get; set; }

        /// <summary>
        /// Post请求时要发送的Byte类型的Post数据
        /// </summary>
        public byte[] PostdataByte { get; set; } = null;

        /// <summary>
        /// Cookie对象集合
        /// </summary>
        public CookieCollection CookieCollection { get; set; } = null;

        /// <summary>
        /// 请求时的Cookie
        /// </summary>
        public string Cookie { get; set; } = string.Empty;

        /// <summary>
        /// 来源地址，上次访问地址
        /// </summary>
        public string Referer { get; set; } = string.Empty;

        /// <summary>
        /// 证书绝对路径
        /// </summary>
        public string CerPath { get; set; } = string.Empty;

        /// <summary>
        /// 是否设置为全文小写
        /// </summary>
        public bool IsToLower { get; set; } = true;

        /// <summary>
        /// 支持跳转页面，查询结果将是跳转后的页面
        /// </summary>
        public bool Allowautoredirect { get; set; } = true;

        /// <summary>
        /// 最大连接数
        /// </summary>
        public int Connectionlimit { get; set; } = 1024;

        /// <summary>
        /// 代理Proxy 服务器用户名
        /// </summary>
        public string ProxyUserName { get; set; } = string.Empty;

        /// <summary>
        /// 代理 服务器密码
        /// </summary>
        public string ProxyPwd { get; set; } = string.Empty;

        /// <summary>
        /// 代理 服务IP
        /// </summary>
        public string ProxyIp { get; set; } = string.Empty;

        /// <summary>
        /// 设置返回类型String和Byte
        /// </summary>
        public ResultType ResultType { get; set; } = ResultType.String;
    }

    /// <summary>
    /// Http返回参数类
    /// </summary>
    public class HttpResult
    {
        /// <summary>
        /// Http请求返回的Cookie
        /// </summary>
        public string Cookie { get; set; } = string.Empty;

        /// <summary>
        /// Cookie对象集合
        /// </summary>
        public CookieCollection CookieCollection { get; set; }

        /// <summary>
        /// 返回的String类型数据 只有ResultType.String时才返回数据，其它情况为空
        /// </summary>
        public string Html { get; set; } = string.Empty;

        /// <summary>
        /// 返回的Byte数组 只有ResultType.Byte时才返回数据，其它情况为空
        /// </summary>
        public byte[] ResultByte { get; set; }

        //header对象
        public WebHeaderCollection Header { get; set; } = new WebHeaderCollection();
    }

    /// <summary>
    /// 返回类型
    /// </summary>
    public enum ResultType
    {
        String, //表示只返回字符串

        Byte //表示返回字符串和字节流
    }

    /// <summary>
    /// Post的数据格式默认为string
    /// </summary>
    public enum PostDataType
    {
        String, //字符串

        Byte, //字符串和字节流

        FilePath //表示传入的是文件
    }
}