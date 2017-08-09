using BioEngine.Common.Base;
using BioEngine.Data.Core;
using Microsoft.AspNetCore.Mvc;

namespace BioEngine.API.Components
{
    public class QueryParams
    {
        [FromQuery(Name = "limit")]
        public int PageSize { get; set; } = 10;

        [FromQuery(Name = "offset")]
        public int? Page { get; set; } = 1;

        [FromQuery(Name = "order")]
        public string OrderBy { get; set; }
    }

    public static class ModelListQueryBaseExtensions
    {
        public static ModelsListQueryBase<TEntity> SetQueryParams<TEntity>(this ModelsListQueryBase<TEntity> query,
            QueryParams queryParams) where TEntity : IBaseModel
        {
            query.Page = queryParams.Page;
            query.PageSize = queryParams.PageSize;
            var orderByFunc = QueryableExtensions.GetOrderByFunc<TEntity>(queryParams.OrderBy);
            if (orderByFunc != null)
            {
                query.SetOrderBy(orderByFunc);
            }

            return query;
        }
    }
}