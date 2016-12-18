using System;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;

namespace BioEngine.Site.Helpers
{
    public class ContentHelper
    {
        private static readonly Regex ImgRegex = new Regex("<img.+?src=[\\\"\'](.+?)[\\\"\'].*?>",
            RegexOptions.IgnoreCase);

        private static readonly Regex StripTagsRegex = new Regex("<.*?>");

        public static Uri GetImageUrl(string content)
        {
            Uri uri = null;
            var result = ImgRegex.Match(content);
            if (result.Success)
            {
                uri = new Uri(result.Groups[1].Value);
            }
            return uri;
        }

        public static string GetDescription(string content, int lenght = 20)
        {
            var words =
                StripTagsRegex.Replace(content, string.Empty)
                    .Trim()
                    .Replace(Environment.NewLine, " ")
                    .Replace("&nbsp;", " ")
                    .Replace("  ", " ")
                    .Split(' ');
            var desc = string.Join(" ", words.Take(lenght).ToList());
            if (words.Length > lenght)
                desc += "...";

            return desc;
        }

        public static bool GetSizeAndMime(Uri imgUrl, out long size, out string mimeType)
        {
            var request = WebRequest.Create(imgUrl);
            var response = request.GetResponseAsync().Result;
            var success =
                !(!response.Headers.AllKeys.Contains("Content-Length") ||
                  !response.Headers.AllKeys.Contains("Content-Type"));
            if (!success)
            {
                size = 0;
                mimeType = string.Empty;
            }
            else
            {
                size = long.Parse(response.Headers["Content-Length"]);
                mimeType = response.Headers["Content-Type"];
            }
            return success;
        }
    }
}