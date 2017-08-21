using System;
using System.Collections.Generic;
using System.Linq;
using ElasticsearchCRUD;
using ElasticsearchCRUD.Model.SearchModel;
using ElasticsearchCRUD.Model.SearchModel.Queries;
using ElasticsearchCRUD.Model.SearchModel.Sorting;
using JetBrains.Annotations;
using Microsoft.Extensions.Options;

namespace BioEngine.Common.Search
{
    [UsedImplicitly]
    public class ElasticSearchProvider<T> : ISearchProvider<T>, IDisposable where T : ISearchModel
    {
        public ElasticSearchProvider(IOptions<ElasticSearchProviderConfig> config)
        {
            _context = new ElasticsearchContext(config.Value.Url, _elasticSearchMappingResolver);
        }

        private readonly IElasticsearchMappingResolver _elasticSearchMappingResolver =
            new ElasticsearchMappingResolver();

        private readonly ElasticsearchContext _context;

        public IEnumerable<T> Search(string term, int limit = 100)
        {
            var results = _context.Search<T>(BuildQueryStringSearch(term, limit));
            return results.PayloadResult.Hits != null
                ? results.PayloadResult.Hits?.HitsResult.Select(t => t.Source)
                : new List<T>();
        }

        public long Count(string term)
        {
            var resultsCount = _context.Count<T>(BuildQueryStringSearch(term));
            return resultsCount;
        }

        private ElasticsearchCRUD.Model.SearchModel.Search BuildQueryStringSearch(string term, int limit = 0)
        {
            var names = "";
            if (term != null)
            {
                names = term.Replace("+", " OR *");
            }

            var search = new ElasticsearchCRUD.Model.SearchModel.Search
            {
                Query = new Query(new QueryStringQuery(names + "*"))
            };

            if (limit > 0)
            {
                search.Size = limit;
            }
            search.Sort = new SortHolder(new List<ISort>()
            {
                new SortStandard("_score") {Order = OrderEnum.desc},
                new SortStandard("id") {Order = OrderEnum.desc}
            });

            return search;
        }

        public void AddUpdateEntity(T entity)
        {
            _context.AddUpdateDocument(entity, entity.GetId());
            _context.SaveChangesAndInitMappings();
        }

        public void AddUpdateEntities(IEnumerable<T> entities)
        {
            foreach (var entity in entities)
            {
                _context.AddUpdateDocument(entity, entity.GetId());
            }
            _context.SaveChangesAndInitMappings();
        }

        public void DeleteEntity(T entitity)
        {
            _context.DeleteDocument<T>(entitity.GetId());
            _context.SaveChanges();
        }

        private bool _isDisposed;

        public void Dispose()
        {
            if (_isDisposed)
            {
                _isDisposed = true;
                _context.Dispose();
            }
        }
    }
}