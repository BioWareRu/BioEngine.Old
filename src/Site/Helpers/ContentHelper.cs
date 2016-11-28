using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace BioEngine.Site.Helpers
{
    public class ContentHelper
    {
        private static readonly Regex ImgRegex = new Regex("<img.+?src=[\\\"\'](.+?)[\\\"\'].*?>",
            RegexOptions.IgnoreCase);

        private static readonly Regex StripTagsRegex = new Regex("<.*?>");

        public static string GetImageUrl(string content)
        {
            var result = ImgRegex.Match(content);
            return result.Success ? result.Groups[1].Value : null;
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
    }
}