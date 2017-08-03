using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using BioEngine.Common.DB;
using BioEngine.Common.Interfaces;
using BioEngine.Routing;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BioEngine.Content.Helpers
{
    public class ContentHelper : IContentHelperInterface
    {
        private readonly BWContext _dbContext;
        private readonly IUrlHelper _urlHelper;

        public ContentHelper(BWContext dbContext, IUrlHelper urlHelper)
        {
            _dbContext = dbContext;
            _urlHelper = urlHelper;

            _placeholders = new List<ContentPlaceholder>()
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
        }

        private static readonly Regex StripTagsRegex = new Regex("<.*?>");

        public static string GetDescription(string content, int lenght = 20)
        {
            if (string.IsNullOrEmpty(content)) return content;
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

        public string StripTags(string html)
        {
            return string.IsNullOrEmpty(html) ? html : StripTagsRegex.Replace(html, string.Empty).Trim();
        }

        private readonly List<ContentPlaceholder> _placeholders;


        public async Task<string> ReplacePlaceholders(string text)
        {
            foreach (var contentPlaceholder in _placeholders)
            {
                var matches = contentPlaceholder.Regex.Matches(text);
                if (matches.Count > 0)
                {
                    foreach (Match match in matches)
                    {
                        var replacement = await contentPlaceholder.Replace(match, contentPlaceholder.UrlOnly) ?? "n/a";

                        text = text.Replace(match.Value, replacement);
                    }
                }
            }
            return text;
        }

        private async Task<string> ReplaceGame(Match match, bool urlOnly)
        {
            var gameUrl = match.Groups[1].Value;
            var game = await _dbContext.Games.FirstOrDefaultAsync(x => x.Url == gameUrl);
            if (game == null) return null;
            var url = _urlHelper.Base().ParentUrl(game, true);
            return urlOnly ? url.ToString() : $"<a href=\"{url}\" title=\"{game.Title}\">{game.Title}</a>";
        }

        private async Task<string> ReplaceDeveloper(Match match, bool urlOnly)
        {
            var developerUrl = match.Groups[1].Value;
            var developer = await _dbContext.Developers.FirstOrDefaultAsync(x => x.Url == developerUrl);
            if (developer == null) return null;
            var url = _urlHelper.Base().ParentUrl(developer, true);
            return urlOnly ? url.ToString() : $"<a href=\"{url}\" title=\"{developer.Name}\">{developer.Name}</a>";
        }

        private async Task<string> ReplaceNews(Match match, bool urlOnly)
        {
            var newsId = int.Parse(match.Groups[1].Value);
            if (newsId <= 0) return null;
            var news = await _dbContext.News.FirstOrDefaultAsync(x => x.Id == newsId);
            if (news == null) return null;
            var url = _urlHelper.News().PublicUrl(news, true);
            return urlOnly ? url.ToString() : $"<a href=\"{url}\" title=\"{news.Title}\">{news.Title}</a>";
        }

        private async Task<string> ReplaceTwitter(Match match, bool urlOnly)
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

            return await Task.FromResult(html);
        }

        private async Task<string> ReplaceVideo(Match match, bool urlOnly)
        {
            var fileId = int.Parse(match.Groups[1].Value);
            var file = await _dbContext.Files.FirstOrDefaultAsync(x => x.Id == fileId);
            var result = !string.IsNullOrEmpty(file?.YtId)
                ? $"<iframe width=\"560\" height=\"315\" src=\"//www.youtube.com/embed/{file.YtId}\" frameborder=\"0\" allowfullscreen></iframe>"
                : null;
            return result;
        }

        private async Task<string> ReplaceHttp(Match match, bool urlOnly)
        {
            return await Task.FromResult("src=\"");
        }

        private async Task<string> ReplaceGallery(Match match, bool urlOnly)
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
            var pic = await _dbContext.GalleryPics.FirstOrDefaultAsync(x => x.Id == galleryId);
            if (pic == null) return null;


            var picUrl = _urlHelper.Gallery().PublicUrl(pic, true) + "#nanogallery/nanoGallery/0/" + pic.Id;
            if (urlOnly)
            {
                return picUrl;
            }
            var thumbUrl = _urlHelper.Gallery().ThumbUrl(pic, width, height);
            return $"<a href='{picUrl}' title='{pic.Desc}'><img src='{thumbUrl}' alt='{pic.Desc}' /></a>";
        }

        private async Task<string> ReplaceArticle(Match match, bool urlOnly)
        {
            var articleId = int.Parse(match.Groups[1].Value);
            if (articleId <= 0) return null;
            var article = await _dbContext.Articles.FirstOrDefaultAsync(x => x.Id == articleId);
            if (article == null) return null;
            var url = _urlHelper.Articles().PublicUrl(article, true);
            return urlOnly ? url.ToString() : $"<a href=\"{url}\" title=\"{article.Title}\">{article.Title}</a>";
        }

        private async Task<string> ReplaceFile(Match match, bool urlOnly)
        {
            var fileId = int.Parse(match.Groups[1].Value);
            if (fileId <= 0) return null;
            var file = await _dbContext.Files.FirstOrDefaultAsync(x => x.Id == fileId);
            if (file == null) return null;
            var url = _urlHelper.Files().PublicUrl(file, true);
            return urlOnly ? url.ToString() : $"<a href=\"{url}\" title=\"{file.Title}\">{file.Title}</a>";
        }
    }

    internal struct ContentPlaceholder
    {
        public Regex Regex { get; }
        public bool UrlOnly { get; }

        public readonly Func<Match, bool, Task<string>> Replace;

        public ContentPlaceholder(Regex regex, bool urlOnly,
            Func<Match, bool, Task<string>> replaceFunc)
        {
            Regex = regex;
            UrlOnly = urlOnly;
            Replace = replaceFunc;
        }
    }
}