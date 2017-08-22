using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using BioEngine.Common.Base;
using BioEngine.Common.Models;
using BioEngine.Search.Interfaces;
using BioEngine.Search.Models;
using JetBrains.Annotations;
using Microsoft.Extensions.Options;
using Nest;

namespace BioEngine.Search.ElasticSearch
{
    [UsedImplicitly]
    public abstract class ElasticSearchProvider<TModel, TSearchModel> : ISearchProvider<TModel>
        where TModel : class, IBaseModel where TSearchModel : class, TModel
    {
        private readonly IMapper _mapper;
        private string IndexName { get; } = typeof(TSearchModel).Name.ToLower();

        protected ElasticSearchProvider(IOptions<ElasticSearchProviderConfig> config, IMapper mapper)
        {
            _mapper = mapper;
            var settings = new ConnectionSettings(new Uri(config.Value.Url));

            _client = new ElasticClient(settings);
        }

        public async Task<IEnumerable<TModel>> Search(string term, int limit = 100)
        {
            var results = await _client.SearchAsync<TSearchModel>(x => GetSearchRequest(x, term, limit));

            return results.Documents;
        }

        public async Task<long> Count(string term)
        {
            var names = GetSearchText(term);
            var resultsCount = await _client.CountAsync<TSearchModel>(x =>
                x.Query(q => q.QueryString(qs => qs.AllFields().Query(names))).Index(IndexName));
            return resultsCount.Count;
        }


        private SearchDescriptor<TSearchModel> GetSearchRequest(SearchDescriptor<TSearchModel> descriptor, string term,
            int limit = 0)
        {
            var names = GetSearchText(term);

            return descriptor.Query(q => q.QueryString(x => x.AllFields().Query(names)))
                .Sort(s => s.Descending("_score").Descending("id")).Size(limit > 0 ? limit : 20).Index(IndexName);
        }

        private static string GetSearchText(string term)
        {
            var names = "";
            if (term != null)
            {
                names = term.Replace("+", " OR *");
            }
            names = names + "*";
            return names;
        }

        public async Task AddUpdateEntity(TModel entity)
        {
            //await _client.CreateIndexAsync(Descriptor);
            await _client.IndexAsync(_mapper.Map<TModel, TSearchModel>(entity), idx => idx.Index(IndexName));
        }

        public async Task AddUpdateEntities(IEnumerable<TModel> entities)
        {
            //await _client.CreateIndexAsync(Descriptor);
            await _client.IndexManyAsync(entities.Select(entity => _mapper.Map<TModel, TSearchModel>(entity)),
                IndexName);
        }

        public async Task DeleteEntity(TModel entity)
        {
            await _client.DeleteAsync<TModel>(_mapper.Map<TModel, TSearchModel>(entity), idx => idx.Index(IndexName));
        }

        public async Task DeleteIndex()
        {
            await _client.DeleteIndexAsync(Indices.All, x => x.Index(IndexName));
        }

        private readonly ElasticClient _client;
    }

    [UsedImplicitly]
    public class NewsSearchProvider : ElasticSearchProvider<News, NewsSearchModel>
    {
        public NewsSearchProvider(IOptions<ElasticSearchProviderConfig> config, IMapper mapper) : base(config, mapper)
        {
        }
    }

    [UsedImplicitly]
    public class GamesSearchProvider : ElasticSearchProvider<Game, GameSearchModel>
    {
        public GamesSearchProvider(IOptions<ElasticSearchProviderConfig> config, IMapper mapper) : base(config, mapper)
        {
        }
    }

    [UsedImplicitly]
    public class ArticlesSearchProvider : ElasticSearchProvider<Article, ArticleSearchModel>
    {
        public ArticlesSearchProvider(IOptions<ElasticSearchProviderConfig> config, IMapper mapper) : base(config,
            mapper)
        {
        }
    }

    [UsedImplicitly]
    public class ArticleCatsSearchProvider : ElasticSearchProvider<ArticleCat, ArticleCatSearchModel>
    {
        public ArticleCatsSearchProvider(IOptions<ElasticSearchProviderConfig> config, IMapper mapper) : base(config,
            mapper)
        {
        }
    }

    [UsedImplicitly]
    public class FilesSearchProvider : ElasticSearchProvider<File, FileSearchModel>
    {
        public FilesSearchProvider(IOptions<ElasticSearchProviderConfig> config, IMapper mapper) : base(config,
            mapper)
        {
        }
    }

    [UsedImplicitly]
    public class FileCatsSearchProvider : ElasticSearchProvider<FileCat, FileCatSearchModel>
    {
        public FileCatsSearchProvider(IOptions<ElasticSearchProviderConfig> config, IMapper mapper) : base(config,
            mapper)
        {
        }
    }

    [UsedImplicitly]
    public class GalleryCatsSearchProvider : ElasticSearchProvider<GalleryCat, GalleryCatSearchModel>
    {
        public GalleryCatsSearchProvider(IOptions<ElasticSearchProviderConfig> config, IMapper mapper) : base(config,
            mapper)
        {
        }
    }
}