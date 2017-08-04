using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BioEngine.Common.Base;
using BioEngine.Common.Interfaces;
using BioEngine.Common.Models;
using BioEngine.Common.Search;
using BioEngine.Data.Articles.Queries;
using BioEngine.Data.Base.Queries;
using BioEngine.Data.Files.Queries;
using BioEngine.Data.Gallery.Queries;
using BioEngine.Data.News.Queries;
using BioEngine.Site.Base;
using BioEngine.Site.ViewModels.Search;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.DependencyInjection;
using BioEngine.Routing;
using MediatR;

namespace BioEngine.Site.Controllers
{
    public class SearchController : BaseController
    {
        private readonly IContentHelperInterface _contentHelper;

        public SearchController(IMediator mediator, IOptions<AppSettings> appSettingsOptions,
            IContentHelperInterface contentHelper)
            : base(mediator, appSettingsOptions, contentHelper)
        {
            _contentHelper = contentHelper;
        }

        public async Task<IActionResult> Index(string query, string block)
        {
            var viewModel = new SearchViewModel(ViewModelConfig, query);
            if (!string.IsNullOrEmpty(query))
            {
                var hasBlock = !string.IsNullOrEmpty(block);
                var limit = hasBlock ? 0 : 5;
                if (!hasBlock || block == "games")
                {
                    var games = SearchEntities<Game>(query, limit);
                    var gamesCount = CountEntities<Game>(query);
                    var searchBlock = CreateSearchBlock("Игры", Url.Search().BlockUrl("games", query), gamesCount,
                        games, x => x.Title,
                        x => Url.Base().PublicUrl(x),
                        x => _contentHelper.ReplacePlaceholders(x.NewsDesc));
                    viewModel.AddBlock(await searchBlock);
                }

                if (!hasBlock || block == "news")
                {
                    var news = SearchEntities<News>(query, limit);
                    var newsCount = CountEntities<News>(query);
                    var searchBlock = CreateSearchBlock("Новости", Url.Search().BlockUrl("news", query), newsCount,
                        news, x => x.Title,
                        x => Url.News().PublicUrl(x),
                        x => _contentHelper.ReplacePlaceholders(x.ShortText));
                    viewModel.AddBlock(await searchBlock);
                }

                if (!hasBlock || block == "articles")
                {
                    var articles = SearchEntities<Article>(query, limit);
                    var articlesCount = CountEntities<Article>(query);
                    var searchBlock = CreateSearchBlock("Статьи", Url.Search().BlockUrl("articles", query),
                        articlesCount,
                        articles, x => x.Title,
                        x => Url.Articles().PublicUrl(x),
                        x => _contentHelper.ReplacePlaceholders(x.Announce));
                    viewModel.AddBlock(await searchBlock);
                }

                if (!hasBlock || block == "articlesCats")
                {
                    var articlesCats = SearchEntities<ArticleCat>(query, limit);
                    var articlesCatsCount = CountEntities<ArticleCat>(query);
                    var searchBlock = CreateSearchBlock("Категории статей",
                        Url.Search().BlockUrl("articlesCats", query),
                        articlesCatsCount,
                        articlesCats, x => x.Title,
                        x => Url.Articles().CatPublicUrl(x),
                        x => _contentHelper.ReplacePlaceholders(x.Descr));
                    viewModel.AddBlock(await searchBlock);
                }

                if (!hasBlock || block == "files")
                {
                    var files = SearchEntities<File>(query, limit);
                    var filesCount = CountEntities<File>(query);
                    var searchBlock = CreateSearchBlock("Файлы", Url.Search().BlockUrl("files", query),
                        filesCount,
                        files, x => x.Title,
                        x => Url.Files().PublicUrl(x),
                        x => _contentHelper.ReplacePlaceholders(x.Announce));
                    viewModel.AddBlock(await searchBlock);
                }

                if (!hasBlock || block == "filesCat")
                {
                    var fileCats = SearchEntities<FileCat>(query, limit);
                    var fileCatsCount = CountEntities<FileCat>(query);
                    var searchBlock = CreateSearchBlock("Категории файлов",
                        Url.Search().BlockUrl("filesCat", query),
                        fileCatsCount,
                        fileCats, x => x.Title,
                        x => Url.Files().CatPublicUrl(x),
                        x => _contentHelper.ReplacePlaceholders(x.Descr));
                    viewModel.AddBlock(await searchBlock);
                }

                if (!hasBlock || block == "galleryCats")
                {
                    var galleryCats = SearchEntities<GalleryCat>(query, limit);
                    var galleryCatsCount = CountEntities<GalleryCat>(query);
                    var searchBlock = CreateSearchBlock("Категории картинок",
                        Url.Search().BlockUrl("galleryCats", query),
                        galleryCatsCount,
                        galleryCats, x => x.Title,
                        x => Url.Gallery().CatPublicUrl(x),
                        x => _contentHelper.ReplacePlaceholders(x.Desc));
                    viewModel.AddBlock(await searchBlock);
                }
            }
            return View(viewModel);
        }

        private async Task<SearchBlock> CreateSearchBlock<T>(string title, Uri url, long totalCount,
            IEnumerable<T> items,
            Func<T, string> getTitle, Func<T, Uri> getUrl, Func<T, Task<string>> getDesc)
        {
            var block = new SearchBlock(title, url, totalCount);
            foreach (var item in items)
            {
                block.AddItem(getTitle(item), getUrl(item), await getDesc(item));
            }
            return block;
        }

        private IEnumerable<T> SearchEntities<T>(string query, int limit = 0) where T : ISearchModel
        {
            var provider = HttpContext.RequestServices.GetService<ISearchProvider<T>>();
            return provider.Search(query, limit);
        }

        private long CountEntities<T>(string query) where T : ISearchModel
        {
            var provider = HttpContext.RequestServices.GetService<ISearchProvider<T>>();
            return provider.Count(query);
        }

        private void AddEntities<T>(IEnumerable<T> entities) where T : ISearchModel
        {
            var provider = HttpContext.RequestServices.GetService<ISearchProvider<T>>();
            provider.AddUpdateEntities(entities);
        }

        public async Task<string> Reindex()
        {
            AddEntities(await Mediator.Send(new GetGamesQuery()));
            AddEntities((await Mediator.Send(new GetNewsQuery())).news);
            AddEntities((await Mediator.Send(new GetArticlesQuery())).articles);
            AddEntities(await Mediator.Send(new GetArticlesCategoriesQuery()));
            AddEntities((await Mediator.Send(new GetFilesQuery())).files);
            AddEntities(await Mediator.Send(new GetFilesCategoriesQuery()));
            AddEntities(await Mediator.Send(new GetGalleryCategoriesQuery()));
            return "done";
        }
    }
}