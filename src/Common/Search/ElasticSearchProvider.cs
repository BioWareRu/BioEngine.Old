using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Microsoft.Extensions.Options;
using Nest;

namespace BioEngine.Common.Search
{
    [UsedImplicitly]
    public class ElasticSearchProvider<T> : ISearchProvider<T> where T : class, ISearchModel, new()
    {
        public ElasticSearchProvider(IOptions<ElasticSearchProviderConfig> config)
        {
            var settings = new ConnectionSettings(new Uri(config.Value.Url));

            _client = new ElasticClient(settings);
        }

        public async Task<IEnumerable<T>> Search(string term, int limit = 100)
        {
            var results = await _client.SearchAsync<T>(x => GetSearchRequest(x, term, limit));
            return results.Documents;
        }

        public async Task<long> Count(string term)
        {
            var names = GetSearchText(term);
            var resultsCount = await _client.CountAsync<T>(x =>
                x.Query(q => q.QueryString(qs => qs.AllFields().Query(names))).Index(GetIndexName()));
            return resultsCount.Count;
        }


        private SearchDescriptor<T> GetSearchRequest(SearchDescriptor<T> descriptor, string term, int limit = 0)
        {
            var names = GetSearchText(term);

            return descriptor.Query(q => q.QueryString(x => x.AllFields().Query(names)))
                .Sort(s => s.Descending("_score").Descending("id")).Size(limit > 0 ? limit : 20).Index(GetIndexName());
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

        private string GetIndexName()
        {
            return typeof(T).Name.ToLower();
        }

        public async Task AddUpdateEntity(T entity)
        {
            await _client.IndexAsync(entity, idx => idx.Index(GetIndexName()));
        }

        public async Task AddUpdateEntities(IEnumerable<T> entities)
        {
            await _client.IndexManyAsync(entities, GetIndexName());
        }

        public async Task DeleteEntity(T entitity)
        {
            await _client.DeleteAsync<T>(entitity, idx => idx.Index(GetIndexName()));
        }

        private ElasticClient _client;
    }
}