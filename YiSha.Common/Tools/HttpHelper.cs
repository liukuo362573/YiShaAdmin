﻿using System.IO.Compression;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.RegularExpressions;
using YiSha.Util;

namespace YiSha.Common
{
    /// <summary>
    /// Http连接操作帮助类
    /// </summary>
    public class HttpHelper
    {
        #region 是否是网址

        /// <summary>
        /// 是否是网址
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static bool IsUrl(string url)
        {
            url = url.ToStr().ToLower();
            if (url.StartsWith("http://") || url.StartsWith("https://"))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        #endregion 是否是网址

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
            Stream myResponseStream = response.GetResponseStream();
            StreamReader myStreamReader = new StreamReader(myResponseStream, Encoding.GetEncoding("utf-8"));
            string retString = myStreamReader.ReadToEnd();
            myStreamReader.Close();
            myResponseStream.Close();

            return retString;
        }

        #endregion 模拟GET

        #region 模拟POST

        /// <summary>
        /// POST请求
        /// </summary>
        /// <param name="posturl">The posturl.</param>
        /// <param name="postData">The post data.</param>
        /// <returns>System.String.</returns>
        public static string HttpPost(string posturl, string postData, string contentType = "application/x-www-form-urlencoded", int timeout = 10 * 1000)
        {
            Stream outstream = null;
            Stream instream = null;
            StreamReader sr = null;
            HttpWebResponse response = null;
            HttpWebRequest request = null;
            Encoding encoding = Encoding.GetEncoding("utf-8");
            byte[] data = encoding.GetBytes(postData);
            // 准备请求...
            try
            {
                // 设置参数
                request = WebRequest.Create(posturl) as HttpWebRequest;
                CookieContainer cookieContainer = new CookieContainer();
                request.CookieContainer = cookieContainer;
                request.AllowAutoRedirect = true;
                request.Method = "POST";
                request.ContentType = contentType;
                request.Timeout = timeout;
                request.ContentLength = data.Length;
                outstream = request.GetRequestStream();
                outstream.Write(data, 0, data.Length);
                outstream.Close();
                //发送请求并获取相应回应数据
                response = request.GetResponse() as HttpWebResponse;
                //直到request.GetResponse()程序才开始向目标网页发送Post请求
                instream = response.GetResponseStream();
                sr = new StreamReader(instream, encoding);
                //返回结果网页（html）代码
                string content = sr.ReadToEnd();
                string err = string.Empty;
                return content;
            }
            catch (Exception ex)
            {
                string err = ex.Message;
                return string.Empty;
            }
        }

        /// <summary>
        /// 模拟httpPost提交表单
        /// </summary>
        /// <param name="url">POS请求的网址</param>
        /// <param name="data">表单里的参数和值</param>
        /// <param name="encoder">页面编码</param>
        /// <returns></returns>
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

        #endregion 模拟POST

        #region 预定义方法或者变更

        /// <summary>
        /// 默认的编码
        /// </summary>
        private Encoding encoding = Encoding.Default;

        /// <summary>
        /// HttpWebRequest对象用来发起请求
        /// </summary>
        private HttpWebRequest request = null;

        /// <summary>
        /// 获取影响流的数据对象
        /// </summary>
        private HttpWebResponse response = null;

        /// <summary>
        /// 根据相传入的数据，得到相应页面数据
        /// </summary>
        /// <param name="strPostdata">传入的数据Post方式,get方式传NUll或者空字符串都可以</param>
        /// <returns>string类型的响应数据</returns>
        private HttpResult GetHttpRequestData(HttpItem httpItem)
        {
            //返回参数
            HttpResult result = new HttpResult();
            try
            {
                #region 得到请求的response

                using (response = (HttpWebResponse)request.GetResponse())
                {
                    result.Header = response.Headers;
                    if (response.Cookies != null)
                    {
                        result.CookieCollection = response.Cookies;
                    }
                    if (response.Headers["set-cookie"] != null)
                    {
                        result.Cookie = response.Headers["set-cookie"];
                    }

                    MemoryStream _stream = new MemoryStream();
                    //GZIIP处理
                    if (response.ContentEncoding != null && response.ContentEncoding.Equals("gzip", StringComparison.InvariantCultureIgnoreCase))
                    {
                        //开始读取流并设置编码方式
                        //new GZipStream(response.GetResponseStream(), CompressionMode.Decompress).CopyTo(_stream, 10240);
                        //.net4.0以下写法
                        _stream = GetMemoryStream(new GZipStream(response.GetResponseStream(), CompressionMode.Decompress));
                    }
                    else
                    {
                        //开始读取流并设置编码方式
                        //response.GetResponseStream().CopyTo(_stream, 10240);
                        //.net4.0以下写法
                        _stream = GetMemoryStream(response.GetResponseStream());
                    }
                    //获取Byte
                    byte[] RawResponse = _stream.ToArray();
                    //是否返回Byte类型数据
                    if (httpItem.ResultType == ResultType.Byte)
                    {
                        result.ResultByte = RawResponse;
                    }
                    //从这里开始我们要无视编码了
                    if (encoding == null)
                    {
                        string temp = Encoding.Default.GetString(RawResponse, 0, RawResponse.Length);
                        //<meta(.*?)charset([\s]?)=[^>](.*?)>
                        Match meta = Regex.Match(temp, "<meta([^<]*)charset=([^<]*)[\"']", RegexOptions.IgnoreCase | RegexOptions.Multiline);
                        string charter = (meta.Groups.Count > 2) ? meta.Groups[2].Value : string.Empty;
                        charter = charter.Replace("\"", string.Empty).Replace("'", string.Empty).Replace(";", string.Empty);
                        if (charter.Length > 0)
                        {
                            charter = charter.ToLower().Replace("iso-8859-1", "gbk");
                            encoding = Encoding.GetEncoding(charter);
                        }
                        else
                        {
                            if (response.CharacterSet != null && response.CharacterSet.ToLower().Trim() == "iso-8859-1")
                            {
                                encoding = Encoding.GetEncoding("gbk");
                            }
                            else
                            {
                                if (string.IsNullOrEmpty(response.CharacterSet.Trim()))
                                {
                                    encoding = Encoding.UTF8;
                                }
                                else
                                {
                                    encoding = Encoding.GetEncoding(response.CharacterSet);
                                }
                            }
                        }
                    }
                    //得到返回的HTML
                    result.Html = encoding.GetString(RawResponse);
                    //最后释放流
                    _stream.Close();
                }

                #endregion 得到请求的response
            }
            catch (WebException ex)
            {
                //这里是在发生异常时返回的错误信息
                result.Html = "String Error";
                response = (HttpWebResponse)ex.Response;
            }
            if (httpItem.IsToLower)
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
            MemoryStream _stream = new MemoryStream();
            int Length = 256;
            Byte[] buffer = new Byte[Length];
            int bytesRead = streamResponse.Read(buffer, 0, Length);
            // write the required bytes
            while (bytesRead > 0)
            {
                _stream.Write(buffer, 0, bytesRead);
                bytesRead = streamResponse.Read(buffer, 0, Length);
            }
            return _stream;
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
            request.Method = httpItem.Method;
            request.Timeout = httpItem.Timeout;
            request.ReadWriteTimeout = httpItem.ReadWriteTimeout;
            //Accept
            request.Accept = httpItem.Accept;
            //ContentType返回类型
            request.ContentType = httpItem.ContentType;
            //UserAgent客户端的访问类型，包括浏览器版本和操作系统信息
            request.UserAgent = httpItem.UserAgent;
            // 编码
            SetEncoding(httpItem);
            //设置Cookie
            SetCookie(httpItem);
            //来源地址
            request.Referer = httpItem.Referer;
            //是否执行跳转功能
            request.AllowAutoRedirect = httpItem.Allowautoredirect;
            //设置Post数据
            SetPostData(httpItem);
            //设置最大连接
            if (httpItem.Connectionlimit > 0)
            {
                request.ServicePoint.ConnectionLimit = httpItem.Connectionlimit;
            }
        }

        /// <summary>
        /// 设置证书
        /// </summary>
        /// <param name="httpItem"></param>
        private void SetCer(HttpItem httpItem)
        {
            if (!string.IsNullOrEmpty(httpItem.CerPath))
            {
                //这一句一定要写在创建连接的前面。使用回调的方法进行证书验证。
                ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(CheckValidationResult);
                //初始化对像，并设置请求的URL地址
                request = (HttpWebRequest)WebRequest.Create(GetUrl(httpItem.URL));
                //创建证书文件
                X509Certificate objx509 = new X509Certificate(httpItem.CerPath);
                //添加到请求里
                request.ClientCertificates.Add(objx509);
            }
            else
            {
                //初始化对像，并设置请求的URL地址
                request = (HttpWebRequest)WebRequest.Create(GetUrl(httpItem.URL));
            }
        }

        /// <summary>
        /// 设置编码
        /// </summary>
        /// <param name="httpItem">Http参数</param>
        private void SetEncoding(HttpItem httpItem)
        {
            if (string.IsNullOrEmpty(httpItem.Encoding) || httpItem.Encoding.ToLower().Trim() == "null")
            {
                //读取数据时的编码方式
                encoding = null;
            }
            else
            {
                //读取数据时的编码方式
                encoding = System.Text.Encoding.GetEncoding(httpItem.Encoding);
            }
        }

        /// <summary>
        /// 设置Cookie
        /// </summary>
        /// <param name="httpItem">Http参数</param>
        private void SetCookie(HttpItem httpItem)
        {
            if (!string.IsNullOrEmpty(httpItem.Cookie))
            {
                //Cookie
                request.Headers[HttpRequestHeader.Cookie] = httpItem.Cookie;
            }
            //设置Cookie
            if (httpItem.CookieCollection != null)
            {
                request.CookieContainer = new CookieContainer();
                request.CookieContainer.Add(httpItem.CookieCollection);
            }
        }

        /// <summary>
        /// 设置Post数据
        /// </summary>
        /// <param name="httpItem">Http参数</param>
        private void SetPostData(HttpItem httpItem)
        {
            //验证在得到结果时是否有传入数据
            if (request.Method.Trim().ToLower().Contains("post"))
            {
                //写入Byte类型
                if (httpItem.PostDataType == PostDataType.Byte)
                {
                    //验证在得到结果时是否有传入数据
                    if (httpItem.PostdataByte != null && httpItem.PostdataByte.Length > 0)
                    {
                        request.ContentLength = httpItem.PostdataByte.Length;
                        request.GetRequestStream().Write(httpItem.PostdataByte, 0, httpItem.PostdataByte.Length);
                    }
                }//写入文件
                else if (httpItem.PostDataType == PostDataType.FilePath)
                {
                    StreamReader r = new StreamReader(httpItem.Postdata, encoding);
                    byte[] buffer = Encoding.Default.GetBytes(r.ReadToEnd());
                    r.Close();
                    request.ContentLength = buffer.Length;
                    request.GetRequestStream().Write(buffer, 0, buffer.Length);
                }
                else
                {
                    //验证在得到结果时是否有传入数据
                    if (!string.IsNullOrEmpty(httpItem.Postdata))
                    {
                        byte[] buffer = Encoding.Default.GetBytes(httpItem.Postdata);
                        request.ContentLength = buffer.Length;
                        request.GetRequestStream().Write(buffer, 0, buffer.Length);
                    }
                }
            }
        }

        /// <summary>
        /// 设置代理
        /// </summary>
        /// <param name="httpItem">参数对象</param>
        private void SetProxy(HttpItem httpItem)
        {
            if (string.IsNullOrEmpty(httpItem.ProxyUserName) && string.IsNullOrEmpty(httpItem.ProxyPwd) && string.IsNullOrEmpty(httpItem.ProxyIp))
            {
                //不需要设置
            }
            else
            {
                //设置代理服务器
                WebProxy myProxy = new WebProxy(httpItem.ProxyIp, false);
                //建议连接
                myProxy.Credentials = new NetworkCredential(httpItem.ProxyUserName, httpItem.ProxyPwd);
                //给当前请求对象
                request.Proxy = myProxy;
                //设置安全凭证
                request.Credentials = CredentialCache.DefaultNetworkCredentials;
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

        #endregion 预定义方法或者变更

        #region 普通类型

        /// <summary>
        /// 传入一个正确或不正确的URl，返回正确的URL
        /// </summary>
        /// <param name="URL">url</param>
        /// <returns>
        /// </returns>
        public static string GetUrl(string URL)
        {
            if (!(URL.Contains("http://") || URL.Contains("https://")))
            {
                URL = "http://" + URL;
            }
            return URL;
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

        #endregion 普通类型
    }

    /// <summary>
    /// Http请求参考类
    /// </summary>
    public class HttpItem
    {
        private string _URL;

        /// <summary>
        /// 请求URL必须填写
        /// </summary>
        public string URL
        {
            get { return _URL; }
            set { _URL = value; }
        }

        private string _Method = "GET";

        /// <summary>
        /// 请求方式默认为GET方式
        /// </summary>
        public string Method
        {
            get { return _Method; }
            set { _Method = value; }
        }

        private int _Timeout = 100000;

        /// <summary>
        /// 默认请求超时时间
        /// </summary>
        public int Timeout
        {
            get { return _Timeout; }
            set { _Timeout = value; }
        }

        private int _ReadWriteTimeout = 30000;

        /// <summary>
        /// 默认写入Post数据超时间
        /// </summary>
        public int ReadWriteTimeout
        {
            get { return _ReadWriteTimeout; }
            set { _ReadWriteTimeout = value; }
        }

        private string _Accept = "text/html, application/xhtml+xml, */*";

        /// <summary>
        /// 请求标头值 默认为text/html, application/xhtml+xml, */*
        /// </summary>
        public string Accept
        {
            get { return _Accept; }
            set { _Accept = value; }
        }

        private string _ContentType = "text/html";

        /// <summary>
        /// 请求返回类型默认 text/html
        /// </summary>
        public string ContentType
        {
            get { return _ContentType; }
            set { _ContentType = value; }
        }

        private string _UserAgent = "Mozilla/5.0 (compatible; MSIE 9.0; Windows NT 6.1; Trident/5.0)";

        /// <summary>
        /// 客户端访问信息默认Mozilla/5.0 (compatible; MSIE 9.0; Windows NT 6.1; Trident/5.0)
        /// </summary>
        public string UserAgent
        {
            get { return _UserAgent; }
            set { _UserAgent = value; }
        }

        private string _Encoding = string.Empty;

        /// <summary>
        /// 返回数据编码默认为NUll,可以自动识别
        /// </summary>
        public string Encoding
        {
            get { return _Encoding; }
            set { _Encoding = value; }
        }

        private PostDataType _PostDataType = PostDataType.String;

        /// <summary>
        /// Post的数据类型
        /// </summary>
        public PostDataType PostDataType
        {
            get { return _PostDataType; }
            set { _PostDataType = value; }
        }

        private string _Postdata;

        /// <summary>
        /// Post请求时要发送的字符串Post数据
        /// </summary>
        public string Postdata
        {
            get { return _Postdata; }
            set { _Postdata = value; }
        }

        private byte[] _PostdataByte = null;

        /// <summary>
        /// Post请求时要发送的Byte类型的Post数据
        /// </summary>
        public byte[] PostdataByte
        {
            get { return _PostdataByte; }
            set { _PostdataByte = value; }
        }

        private CookieCollection cookiecollection = null;

        /// <summary>
        /// Cookie对象集合
        /// </summary>
        public CookieCollection CookieCollection
        {
            get { return cookiecollection; }
            set { cookiecollection = value; }
        }

        private string _Cookie = string.Empty;

        /// <summary>
        /// 请求时的Cookie
        /// </summary>
        public string Cookie
        {
            get { return _Cookie; }
            set { _Cookie = value; }
        }

        private string _Referer = string.Empty;

        /// <summary>
        /// 来源地址，上次访问地址
        /// </summary>
        public string Referer
        {
            get { return _Referer; }
            set { _Referer = value; }
        }

        private string _CerPath = string.Empty;

        /// <summary>
        /// 证书绝对路径
        /// </summary>
        public string CerPath
        {
            get { return _CerPath; }
            set { _CerPath = value; }
        }

        private Boolean isToLower = true;

        /// <summary>
        /// 是否设置为全文小写
        /// </summary>
        public Boolean IsToLower
        {
            get { return isToLower; }
            set { isToLower = value; }
        }

        private Boolean allowautoredirect = true;

        /// <summary>
        /// 支持跳转页面，查询结果将是跳转后的页面
        /// </summary>
        public Boolean Allowautoredirect
        {
            get { return allowautoredirect; }
            set { allowautoredirect = value; }
        }

        private int connectionlimit = 1024;

        /// <summary>
        /// 最大连接数
        /// </summary>
        public int Connectionlimit
        {
            get { return connectionlimit; }
            set { connectionlimit = value; }
        }

        private string proxyusername = string.Empty;

        /// <summary>
        /// 代理Proxy 服务器用户名
        /// </summary>
        public string ProxyUserName
        {
            get { return proxyusername; }
            set { proxyusername = value; }
        }

        private string proxypwd = string.Empty;

        /// <summary>
        /// 代理 服务器密码
        /// </summary>
        public string ProxyPwd
        {
            get { return proxypwd; }
            set { proxypwd = value; }
        }

        private string proxyip = string.Empty;

        /// <summary>
        /// 代理 服务IP
        /// </summary>
        public string ProxyIp
        {
            get { return proxyip; }
            set { proxyip = value; }
        }

        private ResultType resulttype = ResultType.String;

        /// <summary>
        /// 设置返回类型String和Byte
        /// </summary>
        public ResultType ResultType
        {
            get { return resulttype; }
            set { resulttype = value; }
        }
    }

    /// <summary>
    /// Http返回参数类
    /// </summary>
    public class HttpResult
    {
        private string _Cookie = string.Empty;

        /// <summary>
        /// Http请求返回的Cookie
        /// </summary>
        public string Cookie
        {
            get { return _Cookie; }
            set { _Cookie = value; }
        }

        private CookieCollection cookiecollection = null;

        /// <summary>
        /// Cookie对象集合
        /// </summary>
        public CookieCollection CookieCollection
        {
            get { return cookiecollection; }
            set { cookiecollection = value; }
        }

        private string html = string.Empty;

        /// <summary>
        /// 返回的String类型数据 只有ResultType.String时才返回数据，其它情况为空
        /// </summary>
        public string Html
        {
            get { return html; }
            set { html = value; }
        }

        private byte[] resultbyte = null;

        /// <summary>
        /// 返回的Byte数组 只有ResultType.Byte时才返回数据，其它情况为空
        /// </summary>
        public byte[] ResultByte
        {
            get { return resultbyte; }
            set { resultbyte = value; }
        }

        private WebHeaderCollection header = new WebHeaderCollection();

        /// <summary>
        /// header对象
        /// </summary>
        public WebHeaderCollection Header
        {
            get { return header; }
            set { header = value; }
        }
    }

    /// <summary>
    /// 返回类型
    /// </summary>
    public enum ResultType
    {
        /// <summary>
        /// 表示只返回字符串
        /// </summary>
        String,

        /// <summary>
        /// 表示返回字符串和字节流
        /// </summary>
        Byte,
    }

    /// <summary>
    /// Post的数据格式默认为string
    /// </summary>
    public enum PostDataType
    {
        /// <summary>
        /// 字符串
        /// </summary>
        String,

        /// <summary>
        /// 字符串和字节流
        /// </summary>
        Byte,

        /// <summary>
        /// 表示传入的是文件
        /// </summary>
        FilePath,
    }
}
