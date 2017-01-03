using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using BioEngine.Common.DB;
using BioEngine.Site.Components.Url;

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

        private static readonly List<ContentPlaceholder> Placeholders = new List<ContentPlaceholder>()
        {
            new ContentPlaceholder(new Regex("\\[game:([a-zA-Z0-9_]+)\\]"), false, ReplaceGame),
            new ContentPlaceholder(new Regex("\\[gameUrl:([a-zA-Z0-9_]+)\\]"), true, ReplaceGame),
            new ContentPlaceholder(new Regex("\\[developer:([a-zA-Z0-9_]+)\\]"), false, ReplaceDeveloper),
            new ContentPlaceholder(new Regex("\\[developerUrl:([a-zA-Z0-9_]+)\\]"), true, ReplaceDeveloper),
            new ContentPlaceholder(new Regex("\\[news:([0-9]+)\\]"), false, ReplaceNews),
            new ContentPlaceholder(new Regex("\\[newsUrl:([0-9]+)\\]"), true, ReplaceNews),
            new ContentPlaceholder(new Regex("\\[file:([0-9]+)\\]"), false, ReplaceFile),
            new ContentPlaceholder(new Regex("\\[fileUrl:([0-9]+)\\]"), true, ReplaceFile),
            new ContentPlaceholder(new Regex("\\[article:([0-9]+)\\]"), false, ReplaceArticle),
            new ContentPlaceholder(new Regex("\\[articleUrl:([0-9]+)\\]"), true, ReplaceArticle),
            new ContentPlaceholder(new Regex("\\[gallery:([0-9]+)\\]"), false, ReplaceGallery),
            new ContentPlaceholder(new Regex("\\[gallery:([0-9]+):([0-9]+):([0-9]+)\\]"), false, ReplaceGallery),
            new ContentPlaceholder(new Regex("\\[galleryUrl:([0-9]+)\\]"), true, ReplaceGallery),
            new ContentPlaceholder(new Regex("src=\"http:"), true, ReplaceHttp),
            new ContentPlaceholder(new Regex("\\[video id\\=([0-9]+?) uri\\=(.*?)\\](.*?)\\[\\/video\\]"), true,
                ReplaceVideo),
            new ContentPlaceholder(new Regex("\\[twitter:([0-9]+)\\]"), false, ReplaceTwitter),
        };


        public static string ReplacePlaceholders(string text, BWContext dbContext, UrlManager urlManager)
        {
            foreach (var contentPlaceholder in Placeholders)
            {
                var matches = contentPlaceholder.Regex.Matches(text);
                if (matches.Count > 0)
                {
                    foreach (Match match in matches)
                    {
                        var replacement = contentPlaceholder.Replace(match, contentPlaceholder.UrlOnly, dbContext,
                                              urlManager) ?? "n/a";

                        text = text.Replace(match.Value, replacement);
                    }
                }
            }
            return text;
        }

        private static string ReplaceGame(Match match, bool urlOnly, BWContext dbcontext,
            UrlManager urlManager)
        {
            var gameUrl = match.Groups[1].Value;
            var game = dbcontext.Games.FirstOrDefault(x => x.Url == gameUrl);
            if (game == null) return null;
            var url = urlManager.ParentUrl(game);
            return urlOnly ? url : $"<a href=\"{url}\" title=\"{game.Title}\">{game.Title}</a>";
        }

        private static string ReplaceDeveloper(Match match, bool urlOnly, BWContext dbcontext,
            UrlManager urlManager)
        {
            var developerUrl = match.Groups[1].Value;
            var developer = dbcontext.Developers.FirstOrDefault(x => x.Url == developerUrl);
            if (developer == null) return null;
            var url = urlManager.ParentUrl(developer);
            return urlOnly ? url : $"<a href=\"{url}\" title=\"{developer.Name}\">{developer.Name}</a>";
        }

        private static string ReplaceNews(Match match, bool urlOnly, BWContext dbcontext, UrlManager urlManager)
        {
            var newsId = int.Parse(match.Groups[1].Value);
            if (newsId <= 0) return null;
            var news = dbcontext.News.FirstOrDefault(x => x.Id == newsId);
            if (news == null) return null;
            var url = urlManager.News.PublicUrl(news);
            return urlOnly ? url : $"<a href=\"{url}\" title=\"{news.Title}\">{news.Title}</a>";
        }

        private static string ReplaceTwitter(Match match, bool urlOnly, BWContext dbcontext, UrlManager urlManager)
        {
            var id = match.Groups[1].Value;
            var html = @"
<div class='embed-twit' id='twitter" + id + @"'></div>
<script type='text/javascript'>
twttr.ready(function(){
twttr.widgets.createTweet('" + id + @"',document.getElementById('twitter" + id + @"'),
  {linkColor: '#55acee',conversation: 'none'  });            });
</script>
";

            return html;
        }

        private static string ReplaceVideo(Match match, bool urlOnly, BWContext dbcontext, UrlManager urlManager)
        {
            var fileId = int.Parse(match.Groups[1].Value);
            var file = dbcontext.Files.FirstOrDefault(x => x.Id == fileId);
            return !string.IsNullOrEmpty(file?.YtId)
                ? $"<iframe width=\"560\" height=\"315\" src=\"//www.youtube.com/embed/{file.YtId}\" frameborder=\"0\" allowfullscreen></iframe>"
                : null;
        }

        private static string ReplaceHttp(Match match, bool urlOnly, BWContext dbcontext, UrlManager urlManager)
        {
            return "src=\"";
        }

        private static string ReplaceGallery(Match match, bool urlOnly, BWContext dbcontext, UrlManager urlManager)
        {
            var width = 300;
            var height = 300;
            var galleryId = int.Parse(match.Groups[1].Value);
            if (match.Groups.Count == 4)
            {
                width = int.Parse(match.Groups[2].Value);
                height = int.Parse(match.Groups[3].Value);
            }
            if (galleryId <= 0) return null;
            var pic = dbcontext.GalleryPics.FirstOrDefault(x => x.Id == galleryId);
            if (pic == null) return null;


            var picUrl = urlManager.Gallery.PublicUrl(pic) + "#nanogallery/nanoGallery/0/" + pic.Id;
            if (urlOnly)
            {
                return picUrl;
            }
            var thumbUrl = urlManager.Gallery.ThumbUrl(pic, width, height);
            return $"<a href='{picUrl}' title='{pic.Desc}'><img src='{thumbUrl}' alt='{pic.Desc}' /></a>";
        }

        private static string ReplaceArticle(Match match, bool urlOnly, BWContext dbcontext, UrlManager urlManager)
        {
            var articleId = int.Parse(match.Groups[1].Value);
            if (articleId <= 0) return null;
            var article = dbcontext.Articles.FirstOrDefault(x => x.Id == articleId);
            if (article == null) return null;
            var url = urlManager.Articles.PublicUrl(article);
            return urlOnly ? url : $"<a href=\"{url}\" title=\"{article.Title}\">{article.Title}</a>";
        }

        private static string ReplaceFile(Match match, bool urlOnly, BWContext dbcontext, UrlManager urlManager)
        {
            var fileId = int.Parse(match.Groups[1].Value);
            if (fileId <= 0) return null;
            var file = dbcontext.Files.FirstOrDefault(x => x.Id == fileId);
            if (file == null) return null;
            var url = urlManager.Files.PublicUrl(file);
            return urlOnly ? url : $"<a href=\"{url}\" title=\"{file.Title}\">{file.Title}</a>";
        }
    }

    internal struct ContentPlaceholder
    {
        public Regex Regex { get; }
        public bool UrlOnly { get; }

        public readonly Func<Match, bool, BWContext, UrlManager, string> Replace;

        public ContentPlaceholder(Regex regex, bool urlOnly,
            Func<Match, bool, BWContext, UrlManager, string> replaceFunc)
        {
            Regex = regex;
            UrlOnly = urlOnly;
            Replace = replaceFunc;
        }
    }
}