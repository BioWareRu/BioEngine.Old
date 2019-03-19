using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BioEngine.API.Components.REST;
using BioEngine.API.Components.REST.Errors;
using BioEngine.Common.Models;
using BioEngine.Data.Articles.Queries;
using BioEngine.Data.Base.Queries;
using BioEngine.Data.Files.Queries;
using BioEngine.Data.Gallery.Queries;
using BioEngine.Data.News.Queries;
using BioEngine.Routing;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace BioEngine.API.Controllers
{
    [Authorize]
    [ValidationExceptionsFilter]
    [UserExceptionFilter]
    [ExceptionFilter]
    [Route("v1/[controller]")]
    public class ExportController : Controller
    {
        private readonly IMediator _mediator;
        private readonly ILogger<ExportController> _logger;
        private readonly BioUrlManager _urlManager;

        public ExportController(IMediator mediator, ILogger<ExportController> logger, BioUrlManager urlManager)
        {
            _mediator = mediator;
            _logger = logger;
            _urlManager = urlManager;
        }

        [HttpGet]
        public async Task<ActionResult<Export>> Get()
        {
            var export = new Export();

            // developers
            var developers = (await _mediator.Send(new GetDevelopersQuery())).models;
            foreach (var developer in developers)
            {
                export.Developers.Add(new DeveloperExport(developer, _urlManager));
            }

            //games
            var games = (await _mediator.Send(new GetGamesQuery())).models;
            foreach (var game in games)
            {
                export.Games.Add(new GameExport(game, _urlManager));
            }

            //topics
            var topics = (await _mediator.Send(new GetTopicsQuery())).models;
            foreach (var topic in topics)
            {
                export.Topics.Add(new TopicExport(topic, _urlManager));
            }

            //news
            var news = (await _mediator.Send(new GetNewsQuery())).models;
            foreach (var newse in news)
            {
                export.News.Add(new NewsExport(newse));
            }

            //articles cats
            var articlesCats = (await _mediator.Send(new GetArticlesCategoriesQuery())).models;
            foreach (var cat in articlesCats)
            {
                export.ArticlesCats.Add(new ArticleCatExport(cat));
            }

            //articles
            var articles = (await _mediator.Send(new GetArticlesQuery())).models;
            foreach (var item in articles)
            {
                export.Articles.Add(new ArticleExport(item));
            }

            //files cats
            var filesCats = (await _mediator.Send(new GetFilesCategoriesQuery())).models;
            foreach (var cat in filesCats)
            {
                export.FilesCats.Add(new FileCatExport(cat));
            }

            //files
            var files = (await _mediator.Send(new GetFilesQuery())).models;
            foreach (var item in files)
            {
                if (!string.IsNullOrEmpty(item.YtId) || item.Link.Contains("files.bioware.ru"))
                {
                    export.Files.Add(new FileExport(item));
                }
            }

            //gallery cats
            var galleryCats = (await _mediator.Send(new GetGalleryCategoriesQuery())).models;
            foreach (var cat in galleryCats)
            {
                export.GalleryCats.Add(new GalleryCatExport(cat));
            }

            //gallery pics
            var galleryPics = (await _mediator.Send(new GetGalleryPicsQuery())).models;
            foreach (var item in galleryPics)
            {
                export.GalleryPics.Add(new GalleryExport(item, _urlManager));
            }

            return export;
        }
    }

    public class Export
    {
        public List<DeveloperExport> Developers { get; set; } = new List<DeveloperExport>();
        public List<GameExport> Games { get; set; } = new List<GameExport>();
        public List<TopicExport> Topics { get; set; } = new List<TopicExport>();
        public List<NewsExport> News { get; set; } = new List<NewsExport>();
        public List<ArticleCatExport> ArticlesCats { get; set; } = new List<ArticleCatExport>();
        public List<ArticleExport> Articles { get; set; } = new List<ArticleExport>();
        public List<FileCatExport> FilesCats { get; set; } = new List<FileCatExport>();
        public List<FileExport> Files { get; set; } = new List<FileExport>();
        public List<GalleryCatExport> GalleryCats { get; set; } = new List<GalleryCatExport>();
        public List<GalleryExport> GalleryPics { get; set; } = new List<GalleryExport>();
    }

    public class DeveloperExport
    {
        public DeveloperExport(Developer developer, BioUrlManager urlManager)
        {
            Id = developer.Id;
            Url = developer.Url;
            Name = developer.Name;
            Info = developer.Info;
            Desc = developer.Desc;
            Logo = urlManager.Base.ParentIconUrl(developer).ToString();
        }

        public int Id { get; set; }
        public string Url { get; set; }
        public string Name { get; set; }
        public string Info { get; set; }
        public string Desc { get; set; }
        public string Logo { get; set; }
    }

    public class GameExport
    {
        public GameExport(Game game, BioUrlManager urlManager)
        {
            Id = game.Id;
            DeveloperId = game.DeveloperId;
            Url = game.Url;
            Title = game.Title;
            Genre = game.Genre;
            ReleaseDate = game.ReleaseDate;
            Platforms = game.Platforms;
            Desc = game.Desc;
            Keywords = game.Keywords;
            Publisher = game.Publisher;
            Localizator = game.Localizator;
            Logo = urlManager.Base.GameLogoUrl(game).ToString();
            SmallLogo = urlManager.Base.ParentIconUrl(game).ToString();
            Date = DateTimeOffset.FromUnixTimeSeconds(game.Date);
            TweetTag = game.TweetTag;
        }

        public int Id { get; set; }
        public int DeveloperId { get; set; }
        public string Url { get; set; }
        public string Title { get; set; }
        public string Genre { get; set; }
        public string ReleaseDate { get; set; }
        public string Platforms { get; set; }
        public string Desc { get; set; }
        public string Keywords { get; set; }
        public string Publisher { get; set; }
        public string Localizator { get; set; }
        public string Logo { get; set; }
        public string SmallLogo { get; set; }
        public DateTimeOffset Date { get; set; }
        public string TweetTag { get; set; }
    }

    public class TopicExport
    {
        public TopicExport(Topic topic, BioUrlManager urlManager)
        {
            Id = topic.Id;
            Title = topic.Title;
            Url = topic.Url;
            Logo = urlManager.Base.ParentIconUrl(topic).ToString();
            Desc = topic.Desc;
        }

        public int Id { get; set; }
        public string Title { get; set; }
        public string Url { get; set; }
        public string Logo { get; set; }
        public string Desc { get; set; }
    }

    public class NewsExport
    {
        public NewsExport(News news)
        {
            Id = news.Id;
            GameId = news.GameId;
            DeveloperId = news.DeveloperId;
            TopicId = news.TopicId;
            Url = news.Url;
            Source = news.Source;
            Title = news.Title;
            ShortText = news.ShortText;
            AddText = news.AddText;
            AuthorId = news.AuthorId;
            ForumTopicId = news.ForumTopicId;
            ForumPostId = news.ForumPostId;
            Sticky = news.Sticky;
            Date = DateTimeOffset.FromUnixTimeSeconds(news.Date);
            LastChangeDate = DateTimeOffset.FromUnixTimeSeconds(news.LastChangeDate);
            Pub = news.Pub;
            Comments = news.Comments;
            TwitterId = news.TwitterId;
            FacebookId = news.FacebookId;
        }

        public int Id { get; set; }

        public int? GameId { get; set; }
        public int? DeveloperId { get; set; }
        public int? TopicId { get; set; }
        public string Url { get; set; }
        public string Source { get; set; }
        public string Title { get; set; }
        public string ShortText { get; set; }
        public string AddText { get; set; }
        public int AuthorId { get; set; }
        public int? ForumTopicId { get; set; }
        public int? ForumPostId { get; set; }
        public int Sticky { get; set; }
        public DateTimeOffset Date { get; set; }
        public DateTimeOffset LastChangeDate { get; set; }
        public int Pub { get; set; }
        public int Comments { get; set; }
        public long? TwitterId { get; set; }
        public string FacebookId { get; set; }
    }

    public class ArticleCatExport
    {
        public ArticleCatExport(ArticleCat cat)
        {
            Id = cat.Id;
            GameId = cat.GameId;
            DeveloperId = cat.DeveloperId;
            TopicId = cat.TopicId;
            CatId = cat.CatId;
            Title = cat.Title;
            Url = cat.Url;
            Desc = cat.Descr;
            Content = cat.Content;
        }

        public int Id { get; set; }
        public int? CatId { get; set; }
        public int? GameId { get; set; }
        public int? DeveloperId { get; set; }
        public int? TopicId { get; set; }
        public string Title { get; set; }
        public string Url { get; set; }
        public string Desc { get; set; }
        public string Content { get; set; }
    }

    public class ArticleExport
    {
        public ArticleExport(Article article)
        {
            Id = article.Id;

            GameId = article.GameId;
            DeveloperId = article.DeveloperId;
            TopicId = article.TopicId;
            Url = article.Url;
            Source = article.Source;
            CatId = article.CatId;
            Title = article.Title;
            Announce = article.Announce;
            Text = article.Text;
            AuthorId = article.AuthorId;
            Count = article.Count;
            Date = DateTimeOffset.FromUnixTimeSeconds(article.Date);
            Pub = article.Pub;
        }

        public int Id { get; set; }

        public int? GameId { get; set; }
        public int? DeveloperId { get; set; }
        public int? TopicId { get; set; }
        public string Url { get; set; }
        public string Source { get; set; }
        public int? CatId { get; set; }
        public string Title { get; set; }
        public string Announce { get; set; }
        public string Text { get; set; }
        public int AuthorId { get; set; }
        public int Count { get; set; }
        public DateTimeOffset Date { get; set; }
        public int Pub { get; set; }
    }

    public class FileCatExport
    {
        public FileCatExport(FileCat cat)
        {
            Id = cat.Id;
            GameId = cat.GameId;
            DeveloperId = cat.DeveloperId;
            TopicId = cat.TopicId;
            CatId = cat.CatId;
            Title = cat.Title;
            Url = cat.Url;
            Desc = cat.Descr;
        }

        public int Id { get; set; }
        public int? CatId { get; set; }
        public int? GameId { get; set; }
        public int? DeveloperId { get; set; }
        public int? TopicId { get; set; }
        public string Title { get; set; }
        public string Desc { get; set; }
        public string Url { get; set; }
    }

    public class FileExport
    {
        public FileExport(File file)
        {
            Id = file.Id;

            GameId = file.GameId;
            DeveloperId = file.DeveloperId;
            Url = file.Url;
            CatId = file.CatId;
            Title = file.Title;
            Announce = file.Announce;
            AuthorId = file.AuthorId;
            Count = file.Count;
            Desc = file.Desc;
            Size = file.Size;
            if (!string.IsNullOrEmpty(file.YtId))
            {
                YtId = file.YtId;
            }
            else
            {
                Link = file.Link.Replace("http://files.bioware.ru", "");    
            }

            Date = DateTimeOffset.FromUnixTimeSeconds(file.Date);
        }

        public int Id { get; set; }

        public int? GameId { get; set; }
        public int? DeveloperId { get; set; }
        public string Url { get; set; }
        public int CatId { get; set; }
        public string Title { get; set; }
        public string Desc { get; set; }
        public string Announce { get; set; }
        public string Link { get; set; }
        public int Size { get; set; }
        public string YtId { get; set; }
        public int AuthorId { get; set; }
        public int Count { get; set; }
        public DateTimeOffset Date { get; set; }
    }

    public class GalleryCatExport
    {
        public GalleryCatExport(GalleryCat cat)
        {
            Id = cat.Id;
            GameId = cat.GameId;
            DeveloperId = cat.DeveloperId;
            TopicId = cat.TopicId;
            CatId = cat.CatId;
            Title = cat.Title;
            Url = cat.Url;
            Desc = cat.Desc;
        }

        public int Id { get; set; }
        public int? CatId { get; set; }
        public int? GameId { get; set; }
        public int? DeveloperId { get; set; }
        public int? TopicId { get; set; }
        public string Title { get; set; }
        public string Desc { get; set; }
        public string Url { get; set; }
    }

    public class GalleryExport
    {
        public GalleryExport(GalleryPic pic, BioUrlManager urlManager)
        {
            Id = pic.Id;
            GameId = pic.Id;
            DeveloperId = pic.DeveloperId;
            CatId = pic.CatId;
            Desc = pic.Desc;
            Pub = pic.Pub;
            AuthorId = pic.AuthorId;
            Date = DateTimeOffset.FromUnixTimeSeconds(pic.Date);
            for (var i = 0; i < pic.Files.Count; i++)
            {
                var file = pic.Files[i];
                Files.Add(new GalleryPicExport
                {
                    FileName = file.Name,
                    Url = urlManager.Gallery.FullUrl(pic, i).ToString()
                });
            }
        }

        public int Id { get; set; }

        public int? GameId { get; set; }
        public int? DeveloperId { get; set; }
        public int CatId { get; set; }
        public string Desc { get; set; }
        public int Pub { get; set; }
        public int AuthorId { get; set; }
        public DateTimeOffset Date { get; set; }
        public List<GalleryPicExport> Files { get; set; } = new List<GalleryPicExport>();
    }

    public class GalleryPicExport
    {
        public string Url { get; set; }
        public string FileName { get; set; }
    }
}