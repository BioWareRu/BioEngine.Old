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
        private readonly BioUrlManager _urlManager;

        public ExportController(IMediator mediator, BioUrlManager urlManager)
        {
            _mediator = mediator;
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
                export.News.Add(new NewsExport(newse, _urlManager));
            }

            //articles cats
            var articlesCats = (await _mediator.Send(new GetArticlesCategoriesQuery())).models;
            foreach (var cat in articlesCats)
            {
                export.ArticlesCats.Add(new ArticleCatExport(cat, _urlManager));
            }

            //articles
            var articles = (await _mediator.Send(new GetArticlesQuery())).models;
            foreach (var item in articles)
            {
                export.Articles.Add(new ArticleExport(item, _urlManager));
            }

            //files cats
            var filesCats = (await _mediator.Send(new GetFilesCategoriesQuery())).models;
            foreach (var cat in filesCats)
            {
                export.FilesCats.Add(new FileCatExport(cat, _urlManager));
            }

            //files
            var files = (await _mediator.Send(new GetFilesQuery())).models;
            foreach (var item in files)
            {
                if (!string.IsNullOrEmpty(item.YtId) || item.Link.Contains("files.bioware.ru"))
                {
                    export.Files.Add(new FileExport(item, _urlManager));
                }
            }

            //gallery cats
            var galleryCats = (await _mediator.Send(new GetGalleryCategoriesQuery())).models;
            foreach (var cat in galleryCats)
            {
                export.GalleryCats.Add(new GalleryCatExport(cat, _urlManager));
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
        public readonly List<DeveloperExport> Developers = new List<DeveloperExport>();
        public readonly List<GameExport> Games = new List<GameExport>();
        public readonly List<TopicExport> Topics = new List<TopicExport>();
        public readonly List<NewsExport> News = new List<NewsExport>();
        public readonly List<ArticleCatExport> ArticlesCats = new List<ArticleCatExport>();
        public readonly List<ArticleExport> Articles = new List<ArticleExport>();
        public readonly List<FileCatExport> FilesCats = new List<FileCatExport>();
        public readonly List<FileExport> Files = new List<FileExport>();
        public readonly List<GalleryCatExport> GalleryCats = new List<GalleryCatExport>();
        public readonly List<GalleryExport> GalleryPics = new List<GalleryExport>();
    }

    public class DeveloperExport
    {
        public DeveloperExport(Developer developer, BioUrlManager urlManager)
        {
            Id = developer.Id;
            Url = developer.Url;
            FullUrl = urlManager.Base.ParentUrl(developer).ToString();
            Name = developer.Name;
            Info = developer.Info;
            Desc = developer.Desc;
            Logo = urlManager.Base.ParentIconUrl(developer).ToString();
        }

        public int Id;
        public string Url;
        public string FullUrl;
        public string Name;
        public string Info;
        public string Desc;
        public string Logo;
    }

    public class GameExport
    {
        public GameExport(Game game, BioUrlManager urlManager)
        {
            Id = game.Id;
            DeveloperId = game.DeveloperId;
            Url = game.Url;
            FullUrl = urlManager.Base.ParentUrl(game).ToString();
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

        public int Id;
        public int DeveloperId;
        public string Url;
        public string FullUrl;
        public string Title;
        public string Genre;
        public string ReleaseDate;
        public string Platforms;
        public string Desc;
        public string Keywords;
        public string Publisher;
        public string Localizator;
        public string Logo;
        public string SmallLogo;
        public DateTimeOffset Date;
        public string TweetTag;
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

        public int Id;
        public string Title;
        public string Url;
        public string Logo;
        public string Desc;
    }

    public class NewsExport
    {
        public NewsExport(News news, BioUrlManager urlManager)
        {
            Id = news.Id;
            GameId = news.GameId;
            DeveloperId = news.DeveloperId;
            TopicId = news.TopicId;
            Url = news.Url;
            FullUrl = urlManager.News.PublicUrl(news).ToString();
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

        public int Id;

        public int? GameId;
        public int? DeveloperId;
        public int? TopicId;
        public string Url;
        public string FullUrl;
        public string Source;
        public string Title;
        public string ShortText;
        public string AddText;
        public int AuthorId;
        public int? ForumTopicId;
        public int? ForumPostId;
        public int Sticky;
        public DateTimeOffset Date;
        public DateTimeOffset LastChangeDate;
        public int Pub;
        public int Comments;
        public long? TwitterId;
        public string FacebookId;
    }

    public class ArticleCatExport
    {
        public ArticleCatExport(ArticleCat cat, BioUrlManager urlManager)
        {
            Id = cat.Id;
            GameId = cat.GameId;
            DeveloperId = cat.DeveloperId;
            TopicId = cat.TopicId;
            CatId = cat.CatId;
            Title = cat.Title;
            Url = cat.Url;
            FullUrl = urlManager.Articles.CatPublicUrl(cat).ToString();
            Desc = cat.Descr;
            Content = cat.Content;
        }

        public int Id;
        public int? CatId;
        public int? GameId;
        public int? DeveloperId;
        public int? TopicId;
        public string Title;
        public string Url;
        public string FullUrl;
        public string Desc;
        public string Content;
    }

    public class ArticleExport
    {
        public ArticleExport(Article article, BioUrlManager urlManager)
        {
            Id = article.Id;

            GameId = article.GameId;
            DeveloperId = article.DeveloperId;
            TopicId = article.TopicId;
            Url = article.Url;
            FullUrl = urlManager.Articles.PublicUrl(article).ToString();
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

        public int Id;

        public int? GameId;
        public int? DeveloperId;
        public int? TopicId;
        public string Url;
        public string FullUrl;
        public string Source;
        public int? CatId;
        public string Title;
        public string Announce;
        public string Text;
        public int AuthorId;
        public int Count;
        public DateTimeOffset Date;
        public int Pub;
    }

    public class FileCatExport
    {
        public FileCatExport(FileCat cat, BioUrlManager urlManager)
        {
            Id = cat.Id;
            GameId = cat.GameId;
            DeveloperId = cat.DeveloperId;
            TopicId = cat.TopicId;
            CatId = cat.CatId;
            Title = cat.Title;
            Url = cat.Url;
            FullUrl = urlManager.Files.CatPublicUrl(cat).ToString();
            Desc = cat.Descr;
        }

        public int Id;
        public int? CatId;
        public int? GameId;
        public int? DeveloperId;
        public int? TopicId;
        public string Title;
        public string Desc;
        public string Url;
        public string FullUrl;
    }

    public class FileExport
    {
        public FileExport(File file, BioUrlManager urlManager)
        {
            Id = file.Id;

            GameId = file.GameId;
            DeveloperId = file.DeveloperId;
            Url = file.Url;
            FullUrl = urlManager.Files.PublicUrl(file).ToString();
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

        public int Id;

        public int? GameId;
        public int? DeveloperId;
        public string Url;
        public string FullUrl;
        public int CatId;
        public string Title;
        public string Desc;
        public string Announce;
        public string Link;
        public int Size;
        public string YtId;
        public int AuthorId;
        public int Count;
        public DateTimeOffset Date;
    }

    public class GalleryCatExport
    {
        public GalleryCatExport(GalleryCat cat, BioUrlManager urlManager)
        {
            Id = cat.Id;
            GameId = cat.GameId;
            DeveloperId = cat.DeveloperId;
            TopicId = cat.TopicId;
            CatId = cat.CatId;
            Title = cat.Title;
            Url = cat.Url;
            FullUrl = urlManager.Gallery.CatPublicUrl(cat).ToString();
            Desc = cat.Desc;
        }

        public int Id;
        public int? CatId;
        public int? GameId;
        public int? DeveloperId;
        public int? TopicId;
        public string Title;
        public string Desc;
        public string Url;
        public string FullUrl;
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

            FullUrl = urlManager.Gallery.PublicUrl(pic).ToString();
        }

        public int Id;

        public int? GameId;
        public int? DeveloperId;
        public int CatId;
        public string Desc;
        public int Pub;
        public int AuthorId;
        public DateTimeOffset Date;
        public readonly List<GalleryPicExport> Files = new List<GalleryPicExport>();
        public string FullUrl;
    }

    public class GalleryPicExport
    {
        public string Url;
        public string FileName;
    }
}
