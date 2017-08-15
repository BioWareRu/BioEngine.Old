using BioEngine.Common.Base;
using BioEngine.Data.Core;
using Microsoft.AspNetCore.Mvc;

namespace BioEngine.API.Components
{
    public class QueryParams
    {
        [FromQuery(Name = "offset")]
        public int Offset { get; set; }

        [FromQuery(Name = "limit")]
        public int Limit { get; set; } = 20;

        [FromQuery(Name = "order")]
        public string OrderBy { get; set; }

        public int PageSize => Limit;

        public int? Page => Offset > 0 ? Offset / Limit + 1 : 1;
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