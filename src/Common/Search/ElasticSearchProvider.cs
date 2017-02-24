using System;
using System.Collections.Generic;
using System.Linq;
using ElasticsearchCRUD;
using ElasticsearchCRUD.Model.SearchModel;
using ElasticsearchCRUD.Model.SearchModel.Queries;
using ElasticsearchCRUD.Model.SearchModel.Sorting;
using JetBrains.Annotations;

namespace BioEngine.Common.Search
{
    [UsedImplicitly]
    public class ElasticSearchProvider<T> : ISearchProvider<T>, IDisposable where T : ISearchModel
    {
        public ElasticSearchProvider()
        {
            _context = new ElasticsearchContext(ConnectionString, _elasticSearchMappingResolver);
        }

        private const string ConnectionString = "http://localhost:9200";

        private readonly IElasticsearchMappingResolver _elasticSearchMappingResolver =
            new ElasticsearchMappingResolver();

        private readonly ElasticsearchContext _context;

        public IEnumerable<T> Search(string term, int limit = 100)
        {
            var results = _context.Search<T>(BuildQueryStringSearch(term, limit));
            return results.PayloadResult.Hits.HitsResult.Select(t => t.Source);
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

        public void AddUpdateEntity(T skill)
        {
            _context.AddUpdateDocument(skill, skill.Id);
            _context.SaveChanges();
        }

        public void DeleteSkill(long deleteId)
        {
            _context.DeleteDocument<T>(deleteId);
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