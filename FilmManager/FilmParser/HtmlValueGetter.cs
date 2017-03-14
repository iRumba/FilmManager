using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using HtmlAgilityPack;

namespace FilmParser
{
    public static class HtmlValueGetter
    {
        public static string[] GetValues(this HtmlValueGetterInfo getterInfo, string url)
        {
            return getterInfo.GetValues(CreateDocument(url));
        }

        public static string[] GetValues(this HtmlValueGetterInfo getterInfo, HtmlDocument document)
        {
            var listElements = document.DocumentNode.SelectNodes(getterInfo.ElementSearchingString);
            IEnumerable<string> listValues = null;
            if (!string.IsNullOrWhiteSpace(getterInfo.Attribute))
                listValues = listElements.Select(e => e.Attributes[getterInfo.Attribute].Value);
            else
                listValues = listElements.Select(e => e.InnerText);
            if (getterInfo.UseRegex)
                listValues = listValues.Select(e => Regex.Match(e, getterInfo.RegexMatch).Value);
            return listValues.ToArray();
        }

        public static string GetValue(HtmlDocument document, HtmlValueGetterInfo getterInfo)
        {
            var element = document.DocumentNode.SelectNodes(getterInfo.ElementSearchingString).First();
            var elementValue = string.Empty;
            if (!string.IsNullOrWhiteSpace(getterInfo.Attribute))
                elementValue = element.Attributes[getterInfo.Attribute].Value;
            else
                elementValue = element.InnerText;
            if (getterInfo.UseRegex)
                elementValue = Regex.Match(elementValue, getterInfo.RegexMatch).Value;
            return elementValue;
        }

        public static string GetValue(this HtmlValueGetterInfo getterInfo, string url)
        {
            return getterInfo.GetValue(CreateDocument(url));
        }

        public static string GetValue(this HtmlValueGetterInfo getterInfo, HtmlDocument document)
        {
            return GetValue(document, getterInfo);
        }

        public static string GetValue(string url, HtmlValueGetterInfo getterInfo)
        {
            return GetValue(CreateDocument(url), getterInfo);
        }

        public static HtmlDocument CreateDocument(string url)
        {
            var res = new HtmlDocument();
            res.LoadHtml(GetHtml(url));
            return res;
        }

        public static string GetHtml(string url)
        {
            var client = (HttpWebRequest)WebRequest.Create(url);
            client.UserAgent = "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/56.0.2924.87 Safari/537.36";
            client.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8";
            client.Headers[HttpRequestHeader.AcceptCharset] = "utf-8";
            client.Headers[HttpRequestHeader.AcceptEncoding] = "gzip, deflate";
            client.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;
            client.Headers[HttpRequestHeader.AcceptLanguage] = "ru";

            var resp = (HttpWebResponse)client.GetResponse();

            var res = string.Empty;
            Encoding enc;
            string charset = null;

            if (!string.IsNullOrWhiteSpace(resp.CharacterSet))
                enc = Encoding.GetEncoding(resp.CharacterSet);
            else if (!string.IsNullOrWhiteSpace(resp.ContentEncoding))
                enc = Encoding.GetEncoding(resp.ContentEncoding);
            else if (!string.IsNullOrWhiteSpace(resp.ContentType) &&
                !string.IsNullOrWhiteSpace(charset = Regex.Match(resp.ContentType, "(?<=charset=)[^;]*", RegexOptions.IgnoreCase).Value.Trim()))
                enc = Encoding.GetEncoding(charset);
            else
                enc = Encoding.UTF8;

            using (var reader = new StreamReader(resp.GetResponseStream(), enc, true))
            {
                res = reader.ReadToEnd();
            }
            return res;
        }
    }
}
