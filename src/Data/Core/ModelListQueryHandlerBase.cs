using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BioEngine.Common.Base;
using BioEngine.Common.DB;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace BioEngine.Data.Core
{
    internal abstract class
        ModelListQueryHandlerBase<TRequest, TResponse> : QueryHandlerBase<TRequest, (IEnumerable<TResponse>, int)>
        where TResponse : IBaseModel
        where TRequest : ModelsListQueryBase<TResponse>
    {
        protected async Task<(IEnumerable<TResponse> models, int totalCount)> GetData(IQueryable<TResponse> query,
            TRequest message)
        {
            int? totalCount = null;
            if (message.OrderByFunc != null)
            {
                query = message.OrderByFunc(query);
            }
            if (message.Page != null && message.Page > 0)
            {
                totalCount = await query.CountAsync();
                query = query.ApplyPaging(message.PageOffset, message.PageSize);
            }
            var models = await query.ToListAsync();
            return (models, totalCount ?? models.Count);
        }

        protected ModelListQueryHandlerBase(IMediator mediator, BWContext dbContext, ILogger logger) : base(mediator,
            dbContext, logger)
        {
        }
    }
}