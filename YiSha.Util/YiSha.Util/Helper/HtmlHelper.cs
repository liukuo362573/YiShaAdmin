using System;
using System.Collections.Generic;

namespace YiSha.Util.Helper
{
    public static class HtmlHelper
    {
        /// <summary>
        /// Get part Content from HTML by apply prefix part and subfix part
        /// </summary>
        /// <param name="html">souce html</param>
        /// <param name="prefix">prefix</param>
        /// <param name="subfix">subfix</param>
        /// <returns>part content</returns>
        public static string Resove(string html, string prefix, string subfix)
        {
            if (string.IsNullOrEmpty(html))
            {
                throw new ArgumentNullException(nameof(html));
            }
            if (string.IsNullOrEmpty(prefix))
            {
                throw new ArgumentNullException(nameof(prefix));
            }
            int inl = html.IndexOf(prefix);
            if (inl != -1)
            {
                inl += prefix.Length;
                int inl2 = html.IndexOf(subfix, inl);
                return html[inl..inl2];
            }
            return null;
        }

        public static string ResoveReverse(string html, string subfix, string prefix)
        {
            if (string.IsNullOrEmpty(html))
            {
                throw new ArgumentNullException(nameof(html));
            }
            if (string.IsNullOrEmpty(prefix))
            {
                throw new ArgumentNullException(nameof(prefix));
            }
            int inl = html.IndexOf(subfix);
            if (inl == -1) return null;

            string subString = html.Substring(0, inl);
            int inl2 = subString.LastIndexOf(prefix);
            if (inl2 == -1) return null;

            return subString.Substring(inl2 + prefix.Length, subString.Length - inl2 - prefix.Length);
        }

        public static List<string> ResoveList(string html, string prefix, string subfix)
        {
            if (string.IsNullOrEmpty(html))
            {
                throw new ArgumentNullException(nameof(html));
            }
            if (string.IsNullOrEmpty(prefix))
            {
                throw new ArgumentNullException(nameof(prefix));
            }
            List<string> list = new List<string>();
            int index = prefix.Length * -1;
            do
            {
                index = html.IndexOf(prefix, index + prefix.Length);
                if (index == -1) break;

                index += prefix.Length;
                int index4 = html.IndexOf(subfix, index);
                string s78 = html[index..index4];
                list.Add(s78);
            }
            while (index > -1);
            return list;
        }
    }
}