using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BioEngine.Common.Base;
using BioEngine.Common.DB;
using BioEngine.Common.Models;
using BioEngine.Common.Search;
using BioEngine.Site.Base;
using BioEngine.Site.Components;
using BioEngine.Site.Components.Url;
using BioEngine.Site.ViewModels.Search;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace BioEngine.Site.Controllers
{
    public class SearchController : BaseController
    {
        public SearchController(BWContext context, ParentEntityProvider parentEntityProvider, UrlManager urlManager,
            IOptions<AppSettings> appSettingsOptions)
            : base(context, parentEntityProvider, urlManager, appSettingsOptions)
        {
        }

        [HttpGet("/search.html")]
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
                    var searchBlock = CreateSearchBlock("Игры", UrlManager.Search.BlockUrl("games", query), gamesCount,
                        games, x => x.Title,
                        x => Task.FromResult(UrlManager.Games.PublicUrl(x)),
                        x => x.NewsDesc);
                    viewModel.AddBlock(await searchBlock);
                }

                if (!hasBlock || block == "news")
                {
                    var news = SearchEntities<News>(query, limit);
                    var newsCount = CountEntities<News>(query);
                    var searchBlock = CreateSearchBlock("Новости", UrlManager.Search.BlockUrl("news", query), newsCount,
                        news, x => x.Title,
                        x => Task.FromResult(UrlManager.News.PublicUrl(x)),
                        x => x.ShortText);
                    viewModel.AddBlock(await searchBlock);
                }

                if (!hasBlock || block == "articles")
                {
                    var articles = SearchEntities<Article>(query, limit);
                    var articlesCount = CountEntities<Article>(query);
                    var searchBlock = CreateSearchBlock("Статьи", UrlManager.Search.BlockUrl("articles", query),
                        articlesCount,
                        articles, x => x.Title,
                        async x => await UrlManager.Articles.PublicUrl(x),
                        x => x.Announce);
                    viewModel.AddBlock(await searchBlock);
                }

                if (!hasBlock || block == "articlesCats")
                {
                    var articlesCats = SearchEntities<ArticleCat>(query, limit);
                    var articlesCatsCount = CountEntities<ArticleCat>(query);
                    var searchBlock = CreateSearchBlock("Категории статей",
                        UrlManager.Search.BlockUrl("articlesCats", query),
                        articlesCatsCount,
                        articlesCats, x => x.Title,
                        async x => await UrlManager.Articles.CatPublicUrl(x),
                        x => x.Descr);
                    viewModel.AddBlock(await searchBlock);
                }

                if (!hasBlock || block == "files")
                {
                    var files = SearchEntities<File>(query, limit);
                    var filesCount = CountEntities<File>(query);
                    var searchBlock = CreateSearchBlock("Файлы", UrlManager.Search.BlockUrl("files", query),
                        filesCount,
                        files, x => x.Title,
                        async x => await UrlManager.Files.PublicUrl(x),
                        x => x.Announce);
                    viewModel.AddBlock(await searchBlock);
                }

                if (!hasBlock || block == "filesCat")
                {
                    var fileCats = SearchEntities<FileCat>(query, limit);
                    var fileCatsCount = CountEntities<FileCat>(query);
                    var searchBlock = CreateSearchBlock("Категории файлов",
                        UrlManager.Search.BlockUrl("filesCat", query),
                        fileCatsCount,
                        fileCats, x => x.Title,
                        async x => await UrlManager.Files.CatPublicUrl(x),
                        x => x.Descr);
                    viewModel.AddBlock(await searchBlock);
                }

                if (!hasBlock || block == "galleryCats")
                {
                    var galleryCats = SearchEntities<GalleryCat>(query, limit);
                    var galleryCatsCount = CountEntities<GalleryCat>(query);
                    var searchBlock = CreateSearchBlock("Категории картинок",
                        UrlManager.Search.BlockUrl("galleryCats", query),
                        galleryCatsCount,
                        galleryCats, x => x.Title,
                        async x => await UrlManager.Gallery.CatPublicUrl(x),
                        x => x.Desc);
                    viewModel.AddBlock(await searchBlock);
                }
            }
            return View(viewModel);
        }

        private async Task<SearchBlock> CreateSearchBlock<T>(string title, string url, long totalCount,
            IEnumerable<T> items,
            Func<T, string> getTitle, Func<T, Task<string>> getUrl, Func<T, string> getDesc)
        {
            var block = new SearchBlock(title, url, totalCount);
            foreach (var item in items)
            {
                block.AddItem(getTitle(item), await getUrl(item), getDesc(item));
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

        public async Task<string> Reindex([FromServices] ISearchProvider<Game> searchProvider)
        {
            AddEntities(await Context.Games.ToListAsync());
            AddEntities(await Context.News.ToListAsync());
            AddEntities(await Context.Articles.ToListAsync());
            AddEntities(await Context.ArticleCats.ToListAsync());
            AddEntities(await Context.Files.ToListAsync());
            AddEntities(await Context.FileCats.ToListAsync());
            AddEntities(await Context.GalleryCats.ToListAsync());
            return "done";
        }
    }
}